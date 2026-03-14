using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public class CardObjectPoolInstaller : MonoInstaller
    {
        [SerializeField] private int _prewarmedObjects = 26;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private GameObject _parent;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PoolFactory<CardPresenter>>()
                .AsSingle()
                .WithArguments(_parent, _prefab.GetComponent<CardPresenter>());

            Container.Bind<CustomPool<CardPresenter>>()
                .AsSingle()
                .WithArguments(_prewarmedObjects);
        }
    }
}