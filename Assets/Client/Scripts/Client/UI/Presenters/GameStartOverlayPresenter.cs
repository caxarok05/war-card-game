using System;
using Zenject;

namespace Client.Scripts.Client
{
    public class GameStartOverlayPresenter : IInitializable, IDisposable
    {
        private readonly GameStartOverlayView _view;
        private readonly GameFlowLogic _gameFlowLogic;

        public GameStartOverlayPresenter(
            GameStartOverlayView view, 
            GameFlowLogic gameFlowLogic)
        {
            _view = view;
            _gameFlowLogic = gameFlowLogic;
        }

        public void Initialize()
        {
            _gameFlowLogic.OnInitializationStarted += Show;
            _gameFlowLogic.OnInitializationFinished += Hide;
        }

        public void Dispose()
        {
            _gameFlowLogic.OnInitializationStarted -= Show;
            _gameFlowLogic.OnInitializationFinished -= Hide;
        }
        
        private void Show()
        {
            _view.Show();
        }

        private void Hide()
        {
            _view.Hide();
        }
    }
}