using System.Threading;
using Client.Scripts.Shared;
using Cysharp.Threading.Tasks;

namespace Client.Scripts.Client
{
    public sealed class TurnRequestLogic
    {
        private readonly IWarServer _warServer;

        public TurnRequestLogic(IWarServer warServer)
        {
            _warServer = warServer;
        }

        public UniTask<StartGameResponse> StartGameAsync(CancellationToken cancellationToken)
        {
            return _warServer.StartGameAsync(cancellationToken);
        }

        public UniTask<DrawResponse> DrawAsync(CancellationToken cancellationToken)
        {
            return _warServer.DrawAsync(cancellationToken);
        }
    }
}