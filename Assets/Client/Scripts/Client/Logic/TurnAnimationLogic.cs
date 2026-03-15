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
        private readonly CustomPool<CardPresenter> _cardPool;
        private readonly GameBoardPresenter _gameBoardPresenter;
        private readonly TableLayoutLogic _tableLayoutLogic;
        private readonly ConfigProvider _configProvider;

        private readonly List<CardPresenter> _activeCards = new();

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
            _cardPool.ReleaseAll();
            _activeCards.Clear();

            await PlayRevealAsync(resolvedTurnData.Response, cancellationToken);

            if (resolvedTurnData.IsWar)
            {
                await PlayWarCameraPunchAsync(cancellationToken);
            }
            
            await UniTask.Delay(
                (int)(_configProvider.AnimationConfig.RevealPauseSeconds * 1000f),
                cancellationToken: cancellationToken);

            await PlayCollectAsync(resolvedTurnData.RoundOutcome, cancellationToken);

            ClearActiveCards();
        }

        private async UniTask PlayRevealAsync(
            DrawResponse response,
            CancellationToken cancellationToken)
        {
            int playerIndex = 0;
            int opponentIndex = 0;

            for (int i = 0; i < response.RevealedCards.Count; i++)
            {
                CardRevealData reveal = response.RevealedCards[i];

                int tableIndex;
                DeckPresenter deckPresenter;

                if (reveal.Owner == PlayerId.Player)
                {
                    tableIndex = playerIndex;
                    playerIndex++;
                    deckPresenter = _gameBoardPresenter.PlayerDeckPresenter;
                }
                else
                {
                    tableIndex = opponentIndex;
                    opponentIndex++;
                    deckPresenter = _gameBoardPresenter.OpponentDeckPresenter;
                }

                CardPresenter cardPresenter = _cardPool.Get();
                _activeCards.Add(cardPresenter);

                cardPresenter.gameObject.SetActive(true);
                cardPresenter.SetPositionAndRotation(
                    deckPresenter.GetSpawnPoint(),
                    deckPresenter.GetSpawnRotation());

                cardPresenter.SetSortingOrder(
                    _tableLayoutLogic.GetSortingOrder(reveal.Owner, tableIndex));

                if (reveal.IsFaceUp)
                {
                    var sprite = _configProvider.CardSpriteConfig.GetCardSprite(reveal.Card.Suit, reveal.Card.Rank);
                    cardPresenter.ShowFront(sprite);
                }
                else
                {
                    cardPresenter.ShowBack();
                }

                await cardPresenter.MoveToAsync(
                    _tableLayoutLogic.GetCardPosition(
                        reveal.Owner,
                        tableIndex,
                        _gameBoardPresenter.TablePresenter),
                    _tableLayoutLogic.GetCardRotation(
                        reveal.Owner,
                        tableIndex,
                        _gameBoardPresenter.TablePresenter),
                    _configProvider.AnimationConfig.RevealMoveDuration,
                    cancellationToken);

                await UniTask.Delay(
                    (int)(_configProvider.AnimationConfig.DelayBetweenRevealCards * 1000f),
                    cancellationToken: cancellationToken);
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

            List<UniTask> collectTasks = new List<UniTask>(_activeCards.Count);

            for (int i = 0; i < _activeCards.Count; i++)
            {
                CardPresenter cardPresenter = _activeCards[i];

                cardPresenter.ShowBack();
                cardPresenter.SetSortingOrder(1000 + i);

                float extraRotation = i % 2 == 0
                    ? _configProvider.AnimationConfig.CollectSpinAngle
                    : -_configProvider.AnimationConfig.CollectSpinAngle;

                collectTasks.Add(cardPresenter.FlyToDeckAsync(
                    targetDeckPresenter.GetSpawnPoint(),
                    targetDeckPresenter.GetSpawnRotation(),
                    _configProvider.AnimationConfig.CollectMoveDuration,
                    _configProvider.AnimationConfig.CollectArcHeight,
                    extraRotation,
                    cancellationToken));
            }

            await UniTask.WhenAll(collectTasks);

            await UniTask.Delay(
                (int)(_configProvider.AnimationConfig.CollectPauseSeconds * 1000f),
                cancellationToken: cancellationToken);
        }

        private async UniTask PlayWarCameraPunchAsync(CancellationToken cancellationToken)
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
        
        private DeckPresenter GetWinnerDeckPresenter(RoundOutcomeType roundOutcome)
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

        private void ClearActiveCards()
        {
            for (int i = 0; i < _activeCards.Count; i++)
            {
                _activeCards[i].ResetState();
            }

            _activeCards.Clear();
        }
    }
}