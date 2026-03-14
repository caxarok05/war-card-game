namespace Client.Scripts.Shared
{
    public sealed class FakeServerSettings
    {
        public int MinLatencyMs { get; }
        public int MaxLatencyMs { get; }
        public float TimeoutChance { get; }
        public float NetworkErrorChance { get; }

        public FakeServerSettings(
            int minLatencyMs,
            int maxLatencyMs,
            float timeoutChance,
            float networkErrorChance)
        {
            MinLatencyMs = minLatencyMs;
            MaxLatencyMs = maxLatencyMs;
            TimeoutChance = timeoutChance;
            NetworkErrorChance = networkErrorChance;
        }
    }
}