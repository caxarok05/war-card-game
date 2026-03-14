using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Client.Scripts.Shared
{
    public sealed class FakeServerRequestSimulator
    {
        private readonly FakeServerSettings _settings;
        private readonly Random _random;

        public FakeServerRequestSimulator(FakeServerSettings settings)
        {
            _settings = settings;
            _random = new Random();
        }

        public async UniTask SimulateRequestAsync(CancellationToken cancellationToken)
        {
            int latency = _random.Next(_settings.MinLatencyMs, _settings.MaxLatencyMs + 1);
            await UniTask.Delay(latency, cancellationToken: cancellationToken);

            float timeoutRoll = (float)_random.NextDouble();
            if (timeoutRoll < _settings.TimeoutChance)
            {
                throw new FakeServerTimeoutException();
            }

            float networkRoll = (float)_random.NextDouble();
            if (networkRoll < _settings.NetworkErrorChance)
            {
                throw new FakeServerNetworkException();
            }
        }
    }
}