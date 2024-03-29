using System;

namespace Waodng.CodeMaid.Integration.Events
{
    /// <summary>
    /// The base implementation of an event listener.
    /// </summary>
    internal abstract class BaseEventListener : ISwitchableFeature, IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        protected BaseEventListener(CodeMaidPackage package)
        {
            Package = package;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the hosting package.
        /// </summary>
        protected CodeMaidPackage Package { get; private set; }

        #endregion Properties

        #region ISwitchable Members

        protected bool IsListening { get; set; }

        public void Switch(bool on)
        {
            if (on && !IsListening)
            {
                IsListening = true;
                RegisterListeners();
            }
            else if (IsListening && !on)
            {
                IsListening = false;
                UnRegisterListeners();
            }
        }

        protected abstract void RegisterListeners();

        protected abstract void UnRegisterListeners();

        #endregion ISwitchable Members

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disposed.
        /// </summary>
        protected bool IsDisposed { get; set; }

        #endregion IDisposable Members
    }
}