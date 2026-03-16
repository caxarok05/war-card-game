using System;
using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class GameTurnCoordinator
    {
        private readonly GameSessionLogic _gameSessionLogic;
        private readonly TurnRequestLogic _turnRequestLogic;
        private readonly TurnResolutionLogic _turnResolutionLogic;
        private readonly TurnAnimationLogic _turnAnimationLogic;
        private readonly GameFlowUiFacade _ui;

        public GameTurnCoordinator(
            GameSessionLogic gameSessionLogic,
            TurnRequestLogic turnRequestLogic,
            TurnResolutionLogic turnResolutionLogic,
            TurnAnimationLogic turnAnimationLogic,
            GameFlowUiFacade ui)
        {
            _gameSessionLogic = gameSessionLogic;
            _turnRequestLogic = turnRequestLogic;
            _turnResolutionLogic = turnResolutionLogic;
            _turnAnimationLogic = turnAnimationLogic;
            _ui = ui;
        }

        public async UniTask InitializeAsync(CancellationToken lifetimeToken)
        {
            if (!_gameSessionLogic.CanInitialize())
            {
                return;
            }

            _gameSessionLogic.BeginInitialization();
            _ui.BeginInitialization();

            try
            {
                StartGameResponse response =
                    await _turnRequestLogic.StartGameAsync(lifetimeToken);

                _gameSessionLogic.CompleteInitialization();
                _ui.FinishInitialization(response);
            }
            catch (OperationCanceledException)
            {
                _gameSessionLogic.CompleteTurn();
            }
            catch (Exception e)
            {
                _gameSessionLogic.CompleteTurn();
                Debug.LogException(e);
                _ui.FailInitialization();
            }
        }

        public async UniTask PlayNextTurnAsync(CancellationToken lifetimeToken)
        {
            if (!_gameSessionLogic.CanPlayTurn())
            {
                return;
            }

            _gameSessionLogic.BeginTurn();
            _ui.BeginTurnRequest();

            try
            {
                DrawResponse response = await _turnRequestLogic.DrawAsync(lifetimeToken);
                ResolvedTurnData resolved = _turnResolutionLogic.Resolve(response);

                try
                {
                    await _turnAnimationLogic.PlayAsync(resolved, lifetimeToken);
                }
                catch (OperationCanceledException)
                {
                    _turnAnimationLogic.SkipToFinalState();
                }

                _ui.ApplyAuthoritativeCounts(response);
                
                if (resolved.IsGameOver)
                {
                    _gameSessionLogic.CompleteGame();
                    _ui.FinishGame(resolved.GameOutcome);
                    return;
                }

                _gameSessionLogic.CompleteTurn();
                _ui.FinishNormalTurn();
            }
            catch (OperationCanceledException)
            {
                _gameSessionLogic.CompleteTurn();
                _turnAnimationLogic.SkipToFinalState();
                _ui.FinishNormalTurn();
            }
            catch (Exception e)
            {
                _gameSessionLogic.CompleteTurn();
                _turnAnimationLogic.SkipToFinalState();
                Debug.LogException(e);
                _ui.FailTurn();
            }
        }
    }
}