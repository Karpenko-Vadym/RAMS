using RAMS.Data.Infrastructure;
using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Repositories
{
    /// <summary>
    /// IInterviewRepository interface declares the Interview specific repository operations
    /// </summary>
    public interface IInterviewRepository : IRepository<Interview> 
    {
        IEnumerable<Interview> GetManyByCandidateId(int id);

        IEnumerable<Interview> GetManyByCandidate(Candidate candidate);

        IEnumerable<Interview> GetManyByAgentId(int id);

        IEnumerable<Interview> GetManyByAgent(Agent agent);

        IEnumerable<Interview> GetManyByDate(DateTime date);

        IEnumerable<Interview> GetManyByStatus(InterviewStatus status);
    }
}
