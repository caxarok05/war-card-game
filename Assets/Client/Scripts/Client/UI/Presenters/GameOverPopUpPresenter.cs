using System;
using Client.Scripts.Shared;
using UnityEngine.SceneManagement;
using Zenject;

namespace Client.Scripts.Client
{
    public class GameOverPopUpPresenter : IInitializable, IDisposable
    {
        private readonly GameOverPopUpView _view;
        private readonly GameFlowLogic  _gameFlowLogic;
        private readonly ConfigProvider _configProvider;

        public GameOverPopUpPresenter(
            GameOverPopUpView view, 
            ConfigProvider configProvider, 
            GameFlowLogic gameFlowLogic)
        {
            _view = view;
            _configProvider = configProvider;
            _gameFlowLogic = gameFlowLogic;
        }

        public void Initialize()
        {
            _view.NextButtonClicked += HandleNextButtonClick;
            _gameFlowLogic.OnGameOver += Show;
        }

        public void Dispose()
        {
            _view.NextButtonClicked -= HandleNextButtonClick;
            _gameFlowLogic.OnGameOver -= Show;
        }

        public void Show(GameOutcomeType gameOutcomeType)
        {
            switch (gameOutcomeType)
            {
                case GameOutcomeType.PlayerWon:
                    _view.ShowPopUp(_configProvider.UIConfig.WinText);
                    break;
                case GameOutcomeType.OpponentWon:
                    _view.ShowPopUp(_configProvider.UIConfig.LoseText);
                    break;
                case GameOutcomeType.Draw:
                    _view.ShowPopUp(_configProvider.UIConfig.DrawText);
                    break;
                case GameOutcomeType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameOutcomeType), gameOutcomeType, null);
            }
        }

        private void HandleNextButtonClick()
        {
            SceneManager.LoadScene(_configProvider.BootstrapConfig.BootstrapSceneName);
        }
    }
}