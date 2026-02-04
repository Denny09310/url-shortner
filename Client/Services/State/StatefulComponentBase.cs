using Client.Services;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Shared
{
    public abstract class StatefulComponentBase : ComponentBase, IDisposable
    {
        private readonly Action _handler;
        private readonly HashSet<IState> _subscriptions = [];

        private bool _disposed;

        protected StatefulComponentBase()
        {
            _handler = () => InvokeAsync(async () =>
            {
                if (_disposed) return;

                try
                {
                    await OnStateChangedAsync();
                }
                finally
                {
                    StateHasChanged();
                }
            });
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var state in _subscriptions)
                {
                    state.StateChanged -= _handler;
                }

                _subscriptions.Clear();
            }

            _disposed = true;
        }

        protected virtual Task OnStateChangedAsync()
            => Task.CompletedTask;

        protected void Track(params IState[] states)
        {
            ThrowIfDisposed();

            foreach (var state in states)
            {
                if (!_subscriptions.Add(state))
                    continue;

                state.StateChanged += _handler;
            }
        }

        protected void Untrack(IState state)
        {
            ThrowIfDisposed();

            if (_subscriptions.Remove(state))
            {
                state.StateChanged -= _handler;
            }
        }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }
    }
}
