using System;
using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;

namespace Client.Scripts.Server
{
    public sealed class FakeWarServer : IWarServer, IDisposable
    {
        private readonly WarGameState _gameState;
        private readonly WarGameInitializer _gameInitializer;
        private readonly WarTurnProcessor _turnProcessor;
        private readonly FakeServerRequestSimulator _requestSimulator;

        private readonly SemaphoreSlim _requestLock = new(1, 1);

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
            await EnterAsync(cancellationToken);

            try
            {
                if (_isConnected)
                {
                    return;
                }

                await _requestSimulator.SimulateRequestAsync(cancellationToken);
                _isConnected = true;
            }
            finally
            {
                Exit();
            }
        }

        public async UniTask<StartGameResponse> StartGameAsync(CancellationToken cancellationToken)
        {
            await EnterAsync(cancellationToken);

            try
            {
                EnsureConnected();

                await _requestSimulator.SimulateRequestAsync(cancellationToken);
                return _gameInitializer.Initialize(_gameState);
            }
            finally
            {
                Exit();
            }
        }

        public async UniTask<DrawResponse> DrawAsync(CancellationToken cancellationToken)
        {
            await EnterAsync(cancellationToken);

            try
            {
                EnsureConnected();

                await _requestSimulator.SimulateRequestAsync(cancellationToken);

                EnsureGameStarted();
                EnsureGameNotFinished();

                return _turnProcessor.Process(_gameState);
            }
            finally
            {
                Exit();
            }
        }

        private async UniTask EnterAsync(CancellationToken cancellationToken)
        {
            await _requestLock.WaitAsync(cancellationToken);
        }

        private void Exit()
        {
            _requestLock.Release();
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

        public void Dispose()
        {
            _requestLock.Dispose();
        }
    }
}