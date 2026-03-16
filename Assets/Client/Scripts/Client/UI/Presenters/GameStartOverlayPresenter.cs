using System;
using Zenject;

namespace Client.Scripts.Client
{
    public class GameStartOverlayPresenter : IInitializable, IDisposable
    {
        private readonly GameStartOverlayView _view;
        private readonly GameFlowUiFacade _flowUiFacade;

        public GameStartOverlayPresenter(
            GameStartOverlayView view, 
            GameFlowUiFacade flowUiFacade)
        {
            _view = view;
            _flowUiFacade = flowUiFacade;
        }

        public void Initialize()
        {
            _flowUiFacade.OnInitializationStarted += Show;
            _flowUiFacade.OnInitializationFinished += Hide;
        }

        public void Dispose()
        {
            _flowUiFacade.OnInitializationStarted -= Show;
            _flowUiFacade.OnInitializationFinished -= Hide;
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