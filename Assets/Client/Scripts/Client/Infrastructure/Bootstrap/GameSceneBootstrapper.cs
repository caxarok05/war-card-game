using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public sealed class GameSceneBootstrapper : IInitializable, IDisposable
    {
        private GameFlowLogic _gameFlowLogic;
        private CancellationTokenSource _cancellationTokenSource;

        public GameSceneBootstrapper(GameFlowLogic gameFlowLogic)
        {
            _gameFlowLogic = gameFlowLogic;
        }
        
        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            RunAsync().Forget();
        }

        private async UniTaskVoid RunAsync()
        {
            try
            {
                await _gameFlowLogic.InitializeAsync(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                // TODO: show initialization error UI.
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}