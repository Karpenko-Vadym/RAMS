using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Infrastructure
{
    /// <summary>
    /// IUnitOfWork interface is responsible for sending Commit command to the database
    /// </summary>
    public interface IUnitOfWork
    {
        void Commit();
    }
}
