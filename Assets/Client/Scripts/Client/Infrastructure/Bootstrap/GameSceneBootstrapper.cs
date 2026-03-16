using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public sealed class GameSceneBootstrapper : IInitializable
    {
        private GameFlowLogic _gameFlowLogic;

        public GameSceneBootstrapper(GameFlowLogic gameFlowLogic)
        {
            _gameFlowLogic = gameFlowLogic;
        }
        
        public void Initialize()
        {
            RunAsync().Forget();
        }

        private async UniTaskVoid RunAsync()
        {
            try
            {
                await _gameFlowLogic.InitializeAsync();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}