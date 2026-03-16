using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.Client
{
    public class BootstrapLoadingPresenter
    {
        private readonly BootstrapLoadingView _view;
        private readonly ConfigProvider _configProvider;
        private readonly IWarServer _server;

        public BootstrapLoadingPresenter(
            BootstrapLoadingView view,
            ConfigProvider configsProvider,
            IWarServer server)
        {
            _view = view;
            _configProvider = configsProvider;
            _server = server;
        }

        public async UniTask RunAsync(CancellationToken ct)
        {
            await ProcessStage(BootstrapStage.ConnectingToServer, ConnectToServer, ct);
            await ProcessStage(BootstrapStage.LoadingGameScene, LoadGameScene, ct);
        }

        private async UniTask ProcessStage(
            BootstrapStage stage,
            System.Func<CancellationToken, UniTask> action,
            CancellationToken ct)
        {
            BootstrapStageData stageData = _configProvider.BootstrapConfig.GetStageData(stage);

            _view.SetStatus(stageData.Message);
            _view.SetProgress(stageData.Progress);

            await action(ct);
        }

        private async UniTask ConnectToServer(CancellationToken ct)
        {
            await _server.ConnectAsync(ct);
        }

        private async UniTask LoadGameScene(CancellationToken ct)
        {
            AsyncOperation operation =
                SceneManager.LoadSceneAsync(_configProvider.BootstrapConfig.GameSceneName); //in normal project that should be done through service 

            while (!operation.isDone)
            {
                await UniTask.Yield(ct);
            }
        }
    }
}