namespace Client.Scripts.Client
{
    public sealed class GameSessionLogic
    {
        private readonly GameSessionState _state;

        public GameSessionLogic(GameSessionState state)
        {
            _state = state;
        }

        public bool CanInitialize()
        {
            return !_state.IsInitialized;
        }

        public bool CanPlayTurn()
        {
            return _state.IsInitialized && !_state.IsBusy && !_state.IsGameOver;
        }

        public void BeginInitialization()
        {
            _state.Reset();
            _state.SetBusy(true);
        }

        public void CompleteInitialization()
        {
            _state.MarkInitialized();
            _state.SetBusy(false);
        }

        public void BeginTurn()
        {
            _state.SetBusy(true);
        }

        public void CompleteTurn()
        {
            _state.SetBusy(false);
        }

        public void CompleteGame()
        {
            _state.MarkGameOver();
            _state.SetBusy(false);
        }
    }
}