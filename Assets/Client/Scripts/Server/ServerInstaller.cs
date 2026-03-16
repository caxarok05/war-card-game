using Client.Scripts.Shared;
using UnityEngine;
using Zenject;

namespace Client.Scripts.Server
{
    public sealed class ServerInstaller : MonoInstaller
    {
        [SerializeField] private int _minLatencyMs = 500;
        [SerializeField] private int _maxLatencyMs = 1000;
        [SerializeField] [Range(0f, 1f)] private float _timeoutChance = 0.02f;
        [SerializeField] [Range(0f, 1f)] private float _networkErrorChance = 0.03f;

        public override void InstallBindings()
        {
            BindSettings();
            BindState();
            BindServices();
        }

        private void BindSettings()
        {
            var settings = new FakeServerSettings(
                _minLatencyMs,
                _maxLatencyMs,
                _timeoutChance,
                _networkErrorChance);

            Container.Bind<FakeServerSettings>().FromInstance(settings).AsSingle();
        }

        private void BindState()
        {
            Container.Bind<WarGameState>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<DeckBuilder>().AsSingle();
            Container.Bind<DeckShuffler>().AsSingle();
            Container.Bind<WarGameInitializer>().AsSingle();
            Container.Bind<DrawPileRefillService>().AsSingle();
            Container.Bind<GameOutcomeEvaluator>().AsSingle();
            Container.Bind<WarResolutionService>().AsSingle();
            Container.Bind<WarTurnProcessor>().AsSingle();
            Container.Bind<FakeServerRequestSimulator>().AsSingle();

            Container.Bind<IWarServer>().To<FakeWarServer>().AsSingle();
        }
    }
}