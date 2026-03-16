/*using System;
using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public sealed class GameFlowLogic : IInitializable, IDisposable
    {
        public event Action<GameOutcomeType> OnGameOver;
        public event Action<string> OnExceptionHappened;
        public event Action OnInitializationStarted;
        public event Action OnInitializationFinished;
        public event Action<string> OnStatusTextChanged;
        public event Action OnStatusTextHidden;

        private readonly GameSessionLogic _gameSessionLogic;
        private readonly TurnRequestLogic _turnRequestLogic;
        private readonly TurnResolutionLogic _turnResolutionLogic;
        private readonly TurnAnimationLogic _turnAnimationLogic;
        private readonly GameBoardPresenter _gameBoardPresenter;
        private readonly ConfigProvider _configProvider;

        private CancellationTokenSource _turnCancellation;

        public GameFlowLogic(
            GameSessionLogic gameSessionLogic,
            TurnRequestLogic turnRequestLogic,
            TurnResolutionLogic turnResolutionLogic,
            TurnAnimationLogic turnAnimationLogic,
            GameBoardPresenter gameBoardPresenter,
            ConfigProvider configProvider)
        {
            _gameSessionLogic = gameSessionLogic;
            _turnRequestLogic = turnRequestLogic;
            _turnResolutionLogic = turnResolutionLogic;
            _turnAnimationLogic = turnAnimationLogic;
            _gameBoardPresenter = gameBoardPresenter;
            _configProvider = configProvider;
        }

        public void Initialize()
        {
            _gameBoardPresenter.TapInputPresenter.InputPerformed += OnInputPerformed;
        }

        public async UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            if (!_gameSessionLogic.CanInitialize())
            {
                return;
            }

            _gameSessionLogic.BeginInitialization();
            _gameBoardPresenter.SetInputEnabled(false);

            OnInitializationStarted?.Invoke();
            OnStatusTextHidden?.Invoke();

            try
            {
                StartGameResponse response = await _turnRequestLogic.StartGameAsync(cancellationToken);

                _gameBoardPresenter.UpdateDeckCounts(
                    response.PlayerDeckCount,
                    response.OpponentDeckCount);

                _gameSessionLogic.CompleteInitialization();

                OnInitializationFinished?.Invoke();
                OnStatusTextChanged?.Invoke(_configProvider.UIConfig.TapToDrawText);
            }
            catch (OperationCanceledException)
            {
                _gameSessionLogic.CompleteTurn();

                OnInitializationFinished?.Invoke();
            }
            catch (Exception exception)
            {
                _gameSessionLogic.CompleteTurn();
                Debug.LogException(exception);

                OnInitializationFinished?.Invoke();
                OnExceptionHappened?.Invoke(_configProvider.UIConfig.InitalizeFailedText);
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

            OnStatusTextChanged?.Invoke(_configProvider.UIConfig.RequestingServerText);

            try
            {
                DrawResponse response = await _turnRequestLogic.DrawAsync(cancellationToken);
                ResolvedTurnData resolvedTurnData = _turnResolutionLogic.Resolve(response);

                await _turnAnimationLogic.PlayAsync(resolvedTurnData, cancellationToken);

                _gameBoardPresenter.UpdateDeckCounts(
                    response.PlayerTotalCardCount,
                    response.OpponentTotalCardCount);

                if (resolvedTurnData.IsGameOver)
                {
                    _gameSessionLogic.CompleteGame();

                    OnStatusTextHidden?.Invoke();
                    OnGameOver?.Invoke(resolvedTurnData.GameOutcome);
                    return;
                }

                _gameSessionLogic.CompleteTurn();

                OnStatusTextChanged?.Invoke(_configProvider.UIConfig.TapToDrawText);
            }
            catch (OperationCanceledException)
            {
                _gameSessionLogic.CompleteTurn();

                OnStatusTextChanged?.Invoke(_configProvider.UIConfig.TapToDrawText);
            }
            catch (Exception exception)
            {
                _gameSessionLogic.CompleteTurn();
                Debug.LogException(exception);

                OnStatusTextHidden?.Invoke();
                OnExceptionHappened?.Invoke(_configProvider.UIConfig.RequestFailedText);
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
            if (!_gameSessionLogic.CanPlayTurn())
                return;

            _turnCancellation?.Cancel();
            _turnCancellation?.Dispose();
            _turnCancellation = new CancellationTokenSource();

            PlayNextTurnAsync(_turnCancellation.Token).Forget();
        }

        public void Dispose()
        {
            _turnCancellation?.Cancel();
            _turnCancellation?.Dispose();

            if (!_gameBoardPresenter.IsDestroyed)
            {
                _gameBoardPresenter.TapInputPresenter.InputPerformed -= OnInputPerformed;
            }
        }
    }
}*/


using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Client.Scripts.Client
{
    public sealed class GameFlowLogic : IInitializable, IDisposable
    {
        private readonly GameBoardPresenter _gameBoardPresenter;
        private readonly GameSessionLogic _gameSessionLogic;
        private readonly GameTurnCoordinator _turnCoordinator;

        private CancellationTokenSource _lifetimeCts;

        public GameFlowLogic(
            GameBoardPresenter gameBoardPresenter,
            GameSessionLogic gameSessionLogic,
            GameTurnCoordinator turnCoordinator)
        {
            _gameBoardPresenter = gameBoardPresenter;
            _gameSessionLogic = gameSessionLogic;
            _turnCoordinator = turnCoordinator;
        }

        public void Initialize()
        {
            _lifetimeCts = new CancellationTokenSource();
            _gameBoardPresenter.TapInputPresenter.InputPerformed += OnInputPerformed;
        }

        public UniTask InitializeAsync()
        {
            return _turnCoordinator.InitializeAsync(_lifetimeCts.Token);
        }

        private void OnInputPerformed()
        {
            if (!_gameSessionLogic.CanPlayTurn())
            {
                return;
            }

            _turnCoordinator.PlayNextTurnAsync(_lifetimeCts.Token).Forget();
        }

        public void Dispose()
        {
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();

            if (!_gameBoardPresenter.IsDestroyed)
            {
                _gameBoardPresenter.TapInputPresenter.InputPerformed -= OnInputPerformed;
            }
        }
    }
}