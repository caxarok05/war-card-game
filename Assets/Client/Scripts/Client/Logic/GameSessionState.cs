namespace Client.Scripts.Client
{
    public sealed class GameSessionState
    {
        public bool IsInitialized { get; private set; }
        public bool IsBusy { get; private set; }
        public bool IsGameOver { get; private set; }

        public void MarkInitialized()
        {
            IsInitialized = true;
        }

        public void SetBusy(bool isBusy)
        {
            IsBusy = isBusy;
        }

        public void MarkGameOver()
        {
            IsGameOver = true;
        }

        public void Reset()
        {
            IsInitialized = false;
            IsBusy = false;
            IsGameOver = false;
        }
    }
}