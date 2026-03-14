using Zenject;

namespace Client.Scripts.Client
{
    public class InputInstaller : MonoInstaller
    {
        private AbstractInput _inputManager;
        public override void InstallBindings()
        {
            InitInputManager();
            Container.Bind<AbstractInput>().FromInstance(_inputManager).AsSingle();
            _inputManager.Init();
        }
        
        private void InitInputManager()
        {

#if UNITY_EDITOR
            _inputManager = new StandaloneInput();
#elif UNITY_ANDROID || UNITY_IOS
            _inputManager = new MobileInput();
#else
            _inputManager = new StandaloneInput();
#endif
        }
    }
}