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
    /// Interview repository implements Interview specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class InterviewRepository : RepositoryBase<Interview>, IInterviewRepository
    {
        public InterviewRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get first interview with matching interview id
        /// </summary>
        /// <param name="id">Id to match with interviews' data</param>
        /// <returns>First interview with matching id</returns>
        public Interview GetOneByInterviewId(int id)
        {
            return this.GetContext.Interviews.Include("Candidate").Include("Interviewer").FirstOrDefault(i => i.InterviewId == id);
        }

        /// <summary>
        /// Get all interviews
        /// </summary>
        /// <returns>All interviews</returns>
        public IEnumerable<Interview> GetAllInterviews()
        {
            return this.GetContext.Interviews.Include("Candidate").Include("Interviewer");
        }

        /// <summary>
        /// Get multiple interviews with matching candidate id
        /// </summary>
        /// <param name="id">Candidate id to match with interviews' data</param>
        /// <returns>Multiple interviews with matching candidate id</returns>
        public IEnumerable<Interview> GetManyByCandidateId(int id)
        {
            return this.GetContext.Interviews.Include("Candidate").Include("Interviewer").Where(i => i.CandidateId == id).ToList();
        }

        /// <summary>
        /// Get multiple interviews with matching candidate
        /// </summary>
        /// <param name="candidate">Candidate to match with interviews' data</param>
        /// <returns>Multiple interviews with matching candidate</returns>
        public IEnumerable<Interview> GetManyByCandidate(Candidate candidate)
        {
            return this.GetContext.Interviews.Include("Candidate").Include("Interviewer").Where(i => i.CandidateId == candidate.CandidateId).ToList();
        }

        /// <summary>
        /// Get multiple interviews with matching agent id
        /// </summary>
        /// <param name="id">Agent id to match with interviews' data</param>
        /// <returns>Multiple interviews with matching agent id</returns>
        public IEnumerable<Interview> GetManyByAgentId(int id)
        {
            return this.GetContext.Interviews.Include("Candidate").Include("Interviewer").Where(i => i.InterviewerId == id).ToList();
        }

        /// <summary>
        /// Get multiple interviews with matching agent
        /// </summary>
        /// <param name="agent">Agent to match with interviews' data</param>
        /// <returns>Multiple interviews with matching agent</returns>
        public IEnumerable<Interview> GetManyByAgent(Agent agent)
        {
            return this.GetContext.Interviews.Include("Candidate").Include("Interviewer").Where(i => i.InterviewerId == agent.AgentId).ToList();
        }

        /// <summary>
        /// Get multiple interviews with matching date
        /// </summary>
        /// <param name="date">Date to match with interviews' data</param>
        /// <returns>Multiple interviews with matching date</returns>
        public IEnumerable<Interview> GetManyByDate(DateTime date)
        {
            return this.GetContext.Interviews.Include("Candidate").Include("Interviewer").Where(i => i.InterviewDate == date).ToList();
        }

        /// <summary>
        /// Get multiple interviews with matching interview status
        /// </summary>
        /// <param name="status">Interview status to match with interviews' data</param>
        /// <returns>Multiple interviews with matching interview status</returns>
        public IEnumerable<Interview> GetManyByStatus(InterviewStatus status)
        {
            return this.GetContext.Interviews.Include("Candidate").Include("Interviewer").Where(i => i.Status == status).ToList();
        }
        #endregion
    }
}
