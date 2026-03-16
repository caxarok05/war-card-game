using System;
using Zenject;

namespace Client.Scripts.Client
{
    public class WarPopUpPresenter : IInitializable, IDisposable
    {
        private readonly WarPopUpView _view;
        private readonly TurnAnimationLogic _animationLogic;

        public WarPopUpPresenter(WarPopUpView view, TurnAnimationLogic animationLogic)
        {
            _view = view;
            _animationLogic = animationLogic;
        }

        public void Initialize()
        {
            _animationLogic.OnWarHappened += Show;
        }

        public void Dispose()
        {
            _animationLogic.OnWarHappened -= Show;
        }
        
        private void Show()
        {
            _view.Show();
        }
    }
}