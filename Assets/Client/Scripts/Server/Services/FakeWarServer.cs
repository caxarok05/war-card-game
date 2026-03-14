using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;

namespace Client.Scripts.Shared
{
    public sealed class FakeWarServer : IWarServer
    {
        private readonly WarGameState _gameState;
        private readonly WarGameInitializer _gameInitializer;
        private readonly WarTurnProcessor _turnProcessor;
        private readonly FakeServerRequestSimulator _requestSimulator;

        private bool _isConnected;

        public FakeWarServer(
            WarGameState gameState,
            WarGameInitializer gameInitializer,
            WarTurnProcessor turnProcessor,
            FakeServerRequestSimulator requestSimulator)
        {
            _gameState = gameState;
            _gameInitializer = gameInitializer;
            _turnProcessor = turnProcessor;
            _requestSimulator = requestSimulator;
        }

        public async UniTask ConnectAsync(CancellationToken cancellationToken)
        {
            if (_isConnected)
            {
                return;
            }

            await _requestSimulator.SimulateRequestAsync(cancellationToken);
            _isConnected = true;
        }

        public async UniTask<StartGameResponse> StartGameAsync(CancellationToken cancellationToken)
        {
            EnsureConnected();

            await _requestSimulator.SimulateRequestAsync(cancellationToken);
            return _gameInitializer.Initialize(_gameState);
        }

        public async UniTask<DrawResponse> DrawAsync(CancellationToken cancellationToken)
        {
            EnsureConnected();

            await _requestSimulator.SimulateRequestAsync(cancellationToken);

            EnsureGameStarted();
            EnsureGameNotFinished();

            return _turnProcessor.Process(_gameState);
        }

        private void EnsureConnected()
        {
            if (!_isConnected)
            {
                throw new ServerNotConnectedException();
            }
        }

        private void EnsureGameStarted()
        {
            if (!_gameState.IsStarted)
            {
                throw new GameNotStartedException();
            }
        }

        private void EnsureGameNotFinished()
        {
            if (_gameState.IsGameOver || _gameState.GameOutcome != GameOutcomeType.None)
            {
                throw new GameAlreadyFinishedException();
            }
        }
    }
}