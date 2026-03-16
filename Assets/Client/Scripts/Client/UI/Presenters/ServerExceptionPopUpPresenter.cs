using System;
using Zenject;

namespace Client.Scripts.Client
{
    public class ServerExceptionPopUpPresenter : IInitializable, IDisposable
    {
        private readonly ServerExceptionPopUpView _view;
        private readonly GameFlowUiFacade _flowUiFacade;

        public ServerExceptionPopUpPresenter(ServerExceptionPopUpView view, GameFlowUiFacade flowUiFacade)
        {
            _view = view;
            _flowUiFacade = flowUiFacade;
        }

        public void Initialize()
        {
            _flowUiFacade.OnExceptionHappened += HandleShowException;
            _view.OnCloseClicked += HandleCloseClicked;
        }

        public void Dispose()
        {
            _flowUiFacade.OnExceptionHappened -= HandleShowException;
            _view.OnCloseClicked -= HandleCloseClicked;
        }

        private void HandleShowException(string message)
        {
            _view.Show(message);
        }

        private void HandleCloseClicked()
        {
            _view.Hide();
        }
    }
}