using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameBoardPresenter _gameBoardPresenter;
        
        public override void InstallBindings()
        {
            BindScenePresenters();
            BindLogic();
            Container.BindInterfacesAndSelfTo<GameSceneBootstrapper>().AsSingle().NonLazy();
        }
        
        private void BindScenePresenters()
        {
            Container.Bind<GameBoardPresenter>().FromInstance(_gameBoardPresenter).AsSingle();
            Container.Bind<TablePresenter>().FromInstance(_gameBoardPresenter.TablePresenter).AsSingle();
            Container.Bind<DeckPresenter>().WithId("PlayerDeck").FromInstance(_gameBoardPresenter.PlayerDeckPresenter).AsSingle();
            Container.Bind<DeckPresenter>().WithId("OpponentDeck").FromInstance(_gameBoardPresenter.OpponentDeckPresenter).AsSingle();
        }
        
        private void BindLogic()
        {
            Container.Bind<GameSessionState>().AsSingle();
            Container.Bind<GameSessionLogic>().AsSingle();
            Container.Bind<TurnRequestLogic>().AsSingle();
            Container.Bind<TurnResolutionLogic>().AsSingle();
            Container.Bind<TableLayoutLogic>().AsSingle();
            Container.Bind<TurnAnimationLogic>().AsSingle();
            Container.Bind<GameFlowLogic>().AsSingle();
        }
    }
}