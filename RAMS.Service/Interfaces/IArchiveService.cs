using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// IArchiveService interface declares the Archive specific service operations
    /// </summary>
    public interface IArchiveService
    {
        void CreateArchivePosition(Archive archive);

        void SaveChanges();
    }
}
