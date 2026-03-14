using System;
using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class GameFlowLogic : IDisposable
    {
        private readonly GameSessionLogic _gameSessionLogic;
        private readonly TurnRequestLogic _turnRequestLogic;
        private readonly TurnResolutionLogic _turnResolutionLogic;
        private readonly TurnAnimationLogic _turnAnimationLogic;
        private readonly GameBoardPresenter _gameBoardPresenter;

        public GameFlowLogic(
            GameSessionLogic gameSessionLogic,
            TurnRequestLogic turnRequestLogic,
            TurnResolutionLogic turnResolutionLogic,
            TurnAnimationLogic turnAnimationLogic,
            GameBoardPresenter gameBoardPresenter)
        {
            _gameSessionLogic = gameSessionLogic;
            _turnRequestLogic = turnRequestLogic;
            _turnResolutionLogic = turnResolutionLogic;
            _turnAnimationLogic = turnAnimationLogic;
            _gameBoardPresenter = gameBoardPresenter;

            _gameBoardPresenter.InputPerformed += OnInputPerformed;
        }

        public async UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            if (!_gameSessionLogic.CanInitialize())
            {
                return;
            }

            _gameSessionLogic.BeginInitialization();
            _gameBoardPresenter.ResetBoard();
            _gameBoardPresenter.SetInputEnabled(false);

            try
            {
                // TODO: Show game scene initialization/loading state in UI.

                StartGameResponse response = await _turnRequestLogic.StartGameAsync(cancellationToken);

                _gameBoardPresenter.UpdateDeckCounts(
                    response.PlayerDeckCount,
                    response.OpponentDeckCount);

                _gameSessionLogic.CompleteInitialization();

                // TODO: Show "Tap to draw" state in UI.
            }
            catch (Exception exception)
            {
                _gameSessionLogic.CompleteTurn();
                Debug.LogException(exception);

                // TODO: Show initialization error state in UI.
            }
            finally
            {
                if (!_gameBoardPresenter.IsDestroyed)
                {
                    _gameBoardPresenter.SetInputEnabled(true);
                }
            }
        }

        public async UniTask PlayNextTurnAsync(CancellationToken cancellationToken)
        {
            if (!_gameSessionLogic.CanPlayTurn())
            {
                return;
            }

            _gameSessionLogic.BeginTurn();
            _gameBoardPresenter.SetInputEnabled(false);

            try
            {
                // TODO: Show request/loading state in UI.

                DrawResponse response = await _turnRequestLogic.DrawAsync(cancellationToken);
                ResolvedTurnData resolvedTurnData = _turnResolutionLogic.Resolve(response);

                await _turnAnimationLogic.PlayAsync(response, cancellationToken);

                _gameBoardPresenter.UpdateDeckCounts(
                    response.PlayerTotalCardCount,
                    response.OpponentTotalCardCount);

                if (resolvedTurnData.IsGameOver)
                {
                    _gameSessionLogic.CompleteGame();

                    // TODO: Show game over UI with resolvedTurnData.GameOutcome.
                    return;
                }

                _gameSessionLogic.CompleteTurn();

                // TODO: Show round result / war state in UI if needed.
            }
            catch (Exception exception)
            {
                _gameSessionLogic.CompleteTurn();
                Debug.LogException(exception);

                // TODO: Show request error UI.
            }
            finally
            {
                if (!_gameBoardPresenter.IsDestroyed)
                {
                    _gameBoardPresenter.SetInputEnabled(true);
                }
            }
        }

        private void OnInputPerformed()
        {
            PlayNextTurnAsync(CancellationToken.None).Forget();
        }

        public void Dispose()
        {
            if (!_gameBoardPresenter.IsDestroyed)
            {
                _gameBoardPresenter.InputPerformed -= OnInputPerformed;
            }
        }
    }
}