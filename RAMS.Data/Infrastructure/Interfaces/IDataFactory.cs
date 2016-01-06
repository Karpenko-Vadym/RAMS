using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Infrastructure
{
    /// <summary>
    /// Factory interface responsible for initializing DataContext
    /// </summary>
    public interface IDataFactory : IDisposable
    {
        DataContext Init();
    }
}
