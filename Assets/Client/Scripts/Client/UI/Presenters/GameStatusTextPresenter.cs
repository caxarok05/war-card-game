using System;
using Zenject;

namespace Client.Scripts.Client
{
    public class GameStatusTextPresenter : IInitializable, IDisposable
    {
        private readonly GameStatusTextView _view;
        private readonly GameFlowLogic _gameFlowLogic;

        public GameStatusTextPresenter(GameStatusTextView view, GameFlowLogic gameFlowLogic)
        {
            _view = view;
            _gameFlowLogic = gameFlowLogic;
        }

        public void Initialize()
        {
            _gameFlowLogic.OnStatusTextChanged += TextChanged;
            _gameFlowLogic.OnStatusTextHidden += Hide;
        }
        public void Dispose()
        {
            _gameFlowLogic.OnStatusTextChanged -= TextChanged;
            _gameFlowLogic.OnStatusTextHidden -= Hide;
        }

        private void TextChanged(string text)
        {
            _view.Show(text);
        }

        private void Hide()
        {
            _view.Hide();
        }
    }
}