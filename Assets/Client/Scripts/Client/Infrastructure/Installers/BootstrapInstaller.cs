using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public sealed class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private BootstrapLoadingView _loadingView;

        public override void InstallBindings()
        {
            Container.Bind<BootstrapLoadingView>().FromInstance(_loadingView).AsSingle();

            Container.Bind<BootstrapLoadingPresenter>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ProjectBootstrapper>().AsSingle().NonLazy();
        }
    }
}