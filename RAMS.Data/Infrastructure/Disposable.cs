using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Infrastructure
{
    /// <summary>
    /// Disposable class allows to clean up Control before it is released from memory
    /// </summary>
    public class Disposable : IDisposable
    {
        private bool IsDisposed;

        public void Dispose()
        {
            this.Dispose(true);

                                       // This method sets a bit in the object header of obj, which the runtime checks when calling finalizers. 
                                       // A finalizer, which is represented by the Object. Finalize method, 
            GC.SuppressFinalize(this); // is used to release unmanaged resources before an object is garbage-collected. 
                                       // If obj does not have a finalizer, the call to the SuppressFinalize method has no effect. 
                                       // Reference: https://msdn.microsoft.com/en-us/library/system.gc.suppressfinalize%28v=vs.110%29.aspx
        }

        private void Dispose(bool dispose)
        {
            if(this.IsDisposed && dispose)
            {
                DisposeCore();
            }

            this.IsDisposed = true;
        }

        ~Disposable()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Needs to be overriden in order to dispose custom objects
        /// </summary>
        protected virtual void DisposeCore() { }
    }
}
