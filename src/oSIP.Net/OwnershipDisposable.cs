using System;

namespace oSIP.Net
{
    public abstract class OwnershipDisposable : IDisposable
    {
        private bool _isOwner;

        static OwnershipDisposable()
        {
            Parser.InitializeIfNecessary();
        }

        protected OwnershipDisposable(bool isOwner)
        {
            _isOwner = isOwner;
        }

        internal void ReleaseOwnership()
        {
            if (!_isOwner)
            {
                throw new InvalidOperationException(
                    $"The '{GetType().Name}' is already owned by someone else. Assign a new or cloned instance instead.");
            }
            _isOwner = false;
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (_isOwner)
            {
                OnDispose();
            }
            GC.SuppressFinalize(this);
        }

        ~OwnershipDisposable()
        {
            if (_isOwner)
            {
                OnDispose();
            }
        }

        protected abstract void OnDispose();
    }
}