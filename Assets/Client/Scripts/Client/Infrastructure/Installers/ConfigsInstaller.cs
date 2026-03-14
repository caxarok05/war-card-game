using Client.Scripts.Client.Services;
using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public class ConfigsInstaller : MonoInstaller
    {
        [SerializeField] private ConfigProvider configsProvider;

        public override void InstallBindings()
        {
            Container.Bind<ConfigProvider>().FromInstance(configsProvider).AsSingle();
        }
    }
}