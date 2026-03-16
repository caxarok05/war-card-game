using System;
using Client.Scripts.Shared;

namespace Client.Scripts.Client
{
    public sealed class GameFlowUiFacade
    {
        public event Action<GameOutcomeType> OnGameOver;
        public event Action<string> OnExceptionHappened;
        public event Action OnInitializationStarted;
        public event Action OnInitializationFinished;
        public event Action<string> OnStatusTextChanged;
        public event Action OnStatusTextHidden;

        private readonly GameBoardPresenter _gameBoardPresenter;
        private readonly ConfigProvider _configProvider;

        public GameFlowUiFacade(
            GameBoardPresenter gameBoardPresenter,
            ConfigProvider configProvider)
        {
            _gameBoardPresenter = gameBoardPresenter;
            _configProvider = configProvider;
        }

        public void BeginInitialization()
        {
            _gameBoardPresenter.SetInputEnabled(false);
            OnInitializationStarted?.Invoke();
            OnStatusTextHidden?.Invoke();
        }

        public void FinishInitialization(StartGameResponse response)
        {
            _gameBoardPresenter.UpdateDeckCounts(
                response.PlayerDeckCount,
                response.OpponentDeckCount);

            _gameBoardPresenter.SetInputEnabled(true);
            OnInitializationFinished?.Invoke();
            OnStatusTextChanged?.Invoke(_configProvider.UIConfig.TapToDrawText);
        }

        public void FailInitialization()
        {
            _gameBoardPresenter.SetInputEnabled(true);
            OnInitializationFinished?.Invoke();
            OnExceptionHappened?.Invoke(_configProvider.UIConfig.InitalizeFailedText);
        }

        public void BeginTurnRequest()
        {
            _gameBoardPresenter.SetInputEnabled(false);
            OnStatusTextChanged?.Invoke(_configProvider.UIConfig.RequestingServerText);
        }

        public void ApplyAuthoritativeCounts(DrawResponse response)
        {
            _gameBoardPresenter.UpdateDeckCounts(
                response.PlayerTotalCardCount,
                response.OpponentTotalCardCount);
        }

        public void FinishNormalTurn()
        {
            _gameBoardPresenter.SetInputEnabled(true);
            OnStatusTextChanged?.Invoke(_configProvider.UIConfig.TapToDrawText);
        }

        public void FinishGame(GameOutcomeType outcome)
        {
            _gameBoardPresenter.SetInputEnabled(false);
            OnStatusTextHidden?.Invoke();
            OnGameOver?.Invoke(outcome);
        }

        public void FailTurn()
        {
            _gameBoardPresenter.SetInputEnabled(true);
            OnStatusTextHidden?.Invoke();
            OnExceptionHappened?.Invoke(_configProvider.UIConfig.RequestFailedText);
        }
    }
}