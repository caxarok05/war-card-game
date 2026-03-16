using System;
using Zenject;

namespace Client.Scripts.Client
{
    public class ServerExceptionPopUpPresenter : IInitializable, IDisposable
    {
        private readonly ServerExceptionPopUpView _view;
        private readonly GameFlowLogic _gameFlowLogic;

        public ServerExceptionPopUpPresenter(ServerExceptionPopUpView view, GameFlowLogic gameFlowLogic)
        {
            _view = view;
            _gameFlowLogic = gameFlowLogic;
        }

        public void Initialize()
        {
            _gameFlowLogic.OnExceptionHappened += HandleShowException;
            _view.OnCloseClicked += HandleCloseClicked;
        }

        public void Dispose()
        {
            _gameFlowLogic.OnExceptionHappened -= HandleShowException;
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