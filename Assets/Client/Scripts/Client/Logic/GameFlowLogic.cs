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