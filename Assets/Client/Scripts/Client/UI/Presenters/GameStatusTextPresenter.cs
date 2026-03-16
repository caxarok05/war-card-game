using System;
using Zenject;

namespace Client.Scripts.Client
{
    public class GameStatusTextPresenter : IInitializable, IDisposable
    {
        private readonly GameStatusTextView _view;
        private readonly GameFlowUiFacade _flowUiFacade;

        public GameStatusTextPresenter(GameStatusTextView view, GameFlowUiFacade flowUiFacade)
        {
            _view = view;
            _flowUiFacade = flowUiFacade;
        }

        public void Initialize()
        {
            _flowUiFacade.OnStatusTextChanged += TextChanged;
            _flowUiFacade.OnStatusTextHidden += Hide;
        }
        public void Dispose()
        {
            _flowUiFacade.OnStatusTextChanged -= TextChanged;
            _flowUiFacade.OnStatusTextHidden -= Hide;
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