using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameBoardPresenter _gameBoardPresenter;
        
        [SerializeField] private GameOverPopUpView _gameOverPopUpView;
        [SerializeField] private GameStartOverlayView _gameStartOverlayView;
        [SerializeField] private GameStatusTextView _gameStatusTextView;
        [SerializeField] private ServerExceptionPopUpView _serverExceptionPopUpView;
        [SerializeField] private WarPopUpView _warPopUpView;
        
        public override void InstallBindings()
        {
            BindScenePresenters();
            BindLogic();
            BindUI();
            Container.BindInterfacesAndSelfTo<GameSceneBootstrapper>().AsSingle().NonLazy();
        }
        
        private void BindScenePresenters()
        {
            Container.Bind<GameBoardPresenter>().FromInstance(_gameBoardPresenter).AsSingle();
            Container.Bind<TablePresenter>().FromInstance(_gameBoardPresenter.TablePresenter).AsSingle();
            Container.Bind<DeckPresenter>().WithId("PlayerDeck").FromInstance(_gameBoardPresenter.PlayerDeckPresenter).AsCached();
            Container.Bind<DeckPresenter>().WithId("OpponentDeck").FromInstance(_gameBoardPresenter.OpponentDeckPresenter).AsCached();
            Container.Bind<TapInputPresenter>().FromInstance(_gameBoardPresenter.TapInputPresenter).AsSingle();
            
            Container.QueueForInject(_gameBoardPresenter.TapInputPresenter);
        }
        
        private void BindLogic()
        {
            Container.Bind<GameSessionState>().AsSingle();
            Container.Bind<GameSessionLogic>().AsSingle();
            Container.Bind<TurnRequestLogic>().AsSingle();
            Container.Bind<TurnResolutionLogic>().AsSingle();
            Container.Bind<TableLayoutLogic>().AsSingle();
            Container.Bind<TurnAnimationLogic>().AsSingle();
            Container.Bind<GameFlowUiFacade>().AsSingle();
            Container.Bind<GameTurnCoordinator>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameFlowLogic>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<GameOverPopUpView>().FromInstance(_gameOverPopUpView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverPopUpPresenter>().AsSingle();
            
            Container.Bind<GameStartOverlayView>().FromInstance(_gameStartOverlayView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameStartOverlayPresenter>().AsSingle();
            
            Container.Bind<GameStatusTextView>().FromInstance(_gameStatusTextView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameStatusTextPresenter>().AsSingle();
            
            Container.Bind<ServerExceptionPopUpView>().FromInstance(_serverExceptionPopUpView).AsSingle();
            Container.BindInterfacesAndSelfTo<ServerExceptionPopUpPresenter>().AsSingle();
            
            Container.Bind<WarPopUpView>().FromInstance(_warPopUpView).AsSingle();
            Container.BindInterfacesAndSelfTo<WarPopUpPresenter>().AsSingle();
        }
    }
}