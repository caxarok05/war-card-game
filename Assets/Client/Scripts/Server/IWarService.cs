using System.Threading;
using Cysharp.Threading.Tasks;

namespace Client.Scripts.Shared
{
    public interface IWarServer
    {
        UniTask ConnectAsync(CancellationToken cancellationToken);
        UniTask<StartGameResponse> StartGameAsync(CancellationToken cancellationToken);
        UniTask<DrawResponse> DrawAsync(CancellationToken cancellationToken);
    }
}