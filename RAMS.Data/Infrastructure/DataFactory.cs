using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Infrastructure
{
    /// <summary>
    /// Implementation class for IDataFactory interface
    /// </summary>
    public class DataFactory : Disposable, IDataFactory
    {
        protected DataContext Context;

        /// <summary>
        /// Init method initializes DataContext
        /// </summary>
        /// <returns></returns>
        public DataContext Init()
        {
            return this.Context ?? (this.Context = new DataContext());
        }

        /// <summary>
        /// DataContext disposal
        /// </summary>
        protected override void DisposeCore()
        {
            if(this.Context != null)
            {
                this.Context.Dispose();
            }
        }
    }
}
