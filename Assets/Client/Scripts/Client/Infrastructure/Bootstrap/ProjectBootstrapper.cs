using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public sealed class ProjectBootstrapper : IInitializable, IDisposable
    {
        private readonly BootstrapLoadingPresenter _bootstrapPresenter;
        private CancellationTokenSource _cancellationTokenSource;

        public ProjectBootstrapper(BootstrapLoadingPresenter bootstrapPresenter)
        {
            _bootstrapPresenter = bootstrapPresenter;
        }

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            RunBootstrapAsync().Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTaskVoid RunBootstrapAsync()
        {
            try
            {
                await _bootstrapPresenter.RunAsync(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}