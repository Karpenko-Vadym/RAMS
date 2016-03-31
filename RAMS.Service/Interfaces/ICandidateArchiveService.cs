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
    /// ICandidateArchiveService interface declares the CandidateArchive specific service operations
    /// </summary>
    public interface ICandidateArchiveService
    {

        void CreateCandidateArchive(CandidateArchive candidateArchive);

        void SaveChanges();
    }
}
