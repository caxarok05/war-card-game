using System;
using System.Collections.Generic;
using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class TurnAnimationLogic
    {
        private sealed class TableCardData
        {
            public CardPresenter Presenter;
            public PlayerId Owner;
            public int SequenceIndex;
            public int VisualIndex;
        }

        public event Action OnWarHappened;
        
        private readonly CustomPool<CardPresenter> _cardPool;
        private readonly GameBoardPresenter _gameBoardPresenter;
        private readonly TableLayoutLogic _tableLayoutLogic;
        private readonly ConfigProvider _configProvider;

        private readonly List<TableCardData> _activeCards = new();
        private readonly List<UniTask> _moveTasksBuffer = new(16);
        private readonly List<UniTask> _collectTasksBuffer = new(32);

        public TurnAnimationLogic(
            CustomPool<CardPresenter> cardPool,
            GameBoardPresenter gameBoardPresenter,
            TableLayoutLogic tableLayoutLogic,
            ConfigProvider configProvider)
        {
            _cardPool = cardPool;
            _gameBoardPresenter = gameBoardPresenter;
            _tableLayoutLogic = tableLayoutLogic;
            _configProvider = configProvider;
        }

        public async UniTask PlayAsync(
            ResolvedTurnData resolvedTurnData,
            CancellationToken cancellationToken)
        {
            SkipToFinalState();

            await PlayRevealAsync(resolvedTurnData.Response, cancellationToken);

            if (resolvedTurnData.IsWar)
            {
                OnWarHappened?.Invoke();
                await PlayWarCameraPunchAsync(cancellationToken);
            }

            await UniTask.Delay(
                (int)(_configProvider.AnimationConfig.RevealPauseSeconds * 1000f),
                cancellationToken: cancellationToken);

            await PlayCollectAsync(resolvedTurnData.RoundOutcome, cancellationToken);

            SkipToFinalState();
        }
        
        public void SkipToFinalState()
        {
            for (int i = 0; i < _activeCards.Count; i++)
            {
                _activeCards[i].Presenter.ResetState();
                _cardPool.Release(_activeCards[i].Presenter);
            }

            _activeCards.Clear();
            _moveTasksBuffer.Clear();
            _collectTasksBuffer.Clear();
        }

        private async UniTask PlayRevealAsync(
            DrawResponse response,
            CancellationToken cancellationToken)
        {
            int playerSequenceIndex = 0;
            int opponentSequenceIndex = 0;

            for (int i = 0; i < response.RevealedCards.Count; i++)
            {
                CardRevealData reveal = response.RevealedCards[i];

                int sequenceIndex;
                DeckPresenter deckPresenter;

                if (reveal.Owner == PlayerId.Player)
                {
                    sequenceIndex = playerSequenceIndex;
                    playerSequenceIndex++;
                    deckPresenter = _gameBoardPresenter.PlayerDeckPresenter;
                }
                else
                {
                    sequenceIndex = opponentSequenceIndex;
                    opponentSequenceIndex++;
                    deckPresenter = _gameBoardPresenter.OpponentDeckPresenter;
                }

                if (IsNextWarStepStart(sequenceIndex))
                {
                    await CollapseAllPreviousCardsToZeroAsync(reveal.Owner, cancellationToken);
                }

                int visualIndex = GetVisualIndex(sequenceIndex);

                CardPresenter cardPresenter = _cardPool.Get();

                cardPresenter.SetPositionAndRotation(
                    deckPresenter.GetSpawnPoint(),
                    deckPresenter.GetSpawnRotation());

                cardPresenter.SetSortingOrder(
                    _tableLayoutLogic.GetSortingOrder(reveal.Owner, sequenceIndex));

                if (reveal.IsFaceUp)
                {
                    Sprite sprite = _configProvider.CardSpriteConfig.GetCardSprite(
                        reveal.Card.Suit,
                        reveal.Card.Rank);

                    cardPresenter.ShowFront(sprite);
                }
                else
                {
                    cardPresenter.ShowBack();
                }

                TableCardData cardData = new TableCardData
                {
                    Presenter = cardPresenter,
                    Owner = reveal.Owner,
                    SequenceIndex = sequenceIndex,
                    VisualIndex = visualIndex
                };

                _activeCards.Add(cardData);

                await cardPresenter.MoveToAsync(
                    _tableLayoutLogic.GetCardPosition(
                        reveal.Owner,
                        visualIndex,
                        _gameBoardPresenter.TablePresenter),
                    _tableLayoutLogic.GetCardRotation(
                        reveal.Owner,
                        visualIndex,
                        _gameBoardPresenter.TablePresenter),
                    _configProvider.AnimationConfig.RevealMoveDuration,
                    cancellationToken);

                await UniTask.Delay(
                    (int)(_configProvider.AnimationConfig.DelayBetweenRevealCards * 1000f),
                    cancellationToken: cancellationToken);
            }
        }

        private bool IsNextWarStepStart(int sequenceIndex)
        {
            return sequenceIndex > 4 && (sequenceIndex - 5) % 4 == 0;
        }

        private int GetVisualIndex(int sequenceIndex)
        {
            if (sequenceIndex == 0)
            {
                return 0;
            }

            if (sequenceIndex >= 1 && sequenceIndex <= 4)
            {
                return sequenceIndex;
            }

            int offsetInsideWarStep = (sequenceIndex - 5) % 4;
            return 1 + offsetInsideWarStep;
        }

        private async UniTask CollapseAllPreviousCardsToZeroAsync(
            PlayerId owner,
            CancellationToken cancellationToken)
        {
            _moveTasksBuffer.Clear();

            for (int i = 0; i < _activeCards.Count; i++)
            {
                TableCardData cardData = _activeCards[i];

                if (cardData.Owner != owner)
                {
                    continue;
                }

                cardData.VisualIndex = 0;

                Vector3 targetPosition = _tableLayoutLogic.GetCardPosition(
                    cardData.Owner,
                    0,
                    _gameBoardPresenter.TablePresenter);

                Quaternion targetRotation = _tableLayoutLogic.GetCardRotation(
                    cardData.Owner,
                    0,
                    _gameBoardPresenter.TablePresenter);

                _moveTasksBuffer.Add(cardData.Presenter.MoveToAsync(
                    targetPosition,
                    targetRotation,
                    _configProvider.AnimationConfig.RevealMoveDuration * 0.5f,
                    cancellationToken));
            }

            if (_moveTasksBuffer.Count > 0)
            {
                await UniTask.WhenAll(_moveTasksBuffer);
            }
        }

        private async UniTask PlayCollectAsync(
            RoundOutcomeType roundOutcome,
            CancellationToken cancellationToken)
        {
            DeckPresenter targetDeckPresenter = GetWinnerDeckPresenter(roundOutcome);
            if (targetDeckPresenter == null)
            {
                return;
            }

            _collectTasksBuffer.Clear();

            for (int i = 0; i < _activeCards.Count; i++)
            {
                CardPresenter cardPresenter = _activeCards[i].Presenter;

                cardPresenter.ShowBack();
                cardPresenter.SetSortingOrder(1000 + i);

                float extraRotation = i % 2 == 0
                    ? _configProvider.AnimationConfig.CollectSpinAngle
                    : -_configProvider.AnimationConfig.CollectSpinAngle;

                _collectTasksBuffer.Add(cardPresenter.FlyToDeckAsync(
                    targetDeckPresenter.GetSpawnPoint(),
                    targetDeckPresenter.GetSpawnRotation(),
                    _configProvider.AnimationConfig.CollectMoveDuration,
                    _configProvider.AnimationConfig.CollectArcHeight,
                    extraRotation,
                    cancellationToken));
            }

            if (_collectTasksBuffer.Count > 0)
            {
                await UniTask.WhenAll(_collectTasksBuffer);
            }

            await UniTask.Delay(
                (int)(_configProvider.AnimationConfig.CollectPauseSeconds * 1000f),
                cancellationToken: cancellationToken);
        }

        private async UniTask PlayWarCameraPunchAsync(
            CancellationToken cancellationToken)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            Transform cameraTransform = mainCamera.transform;

            Tween punchTween = cameraTransform.DOPunchPosition(
                _configProvider.AnimationConfig.WarPunchStrength,
                _configProvider.AnimationConfig.WarPunchDuration,
                _configProvider.AnimationConfig.WarPunchVibrato,
                _configProvider.AnimationConfig.WarPunchElasticity);

            await punchTween.ToUniTask(cancellationToken: cancellationToken);
        }

        private DeckPresenter GetWinnerDeckPresenter(
            RoundOutcomeType roundOutcome)
        {
            switch (roundOutcome)
            {
                case RoundOutcomeType.PlayerWon:
                    return _gameBoardPresenter.PlayerDeckPresenter;
                case RoundOutcomeType.OpponentWon:
                    return _gameBoardPresenter.OpponentDeckPresenter;
                default:
                    return null;
            }
        }
    }
}