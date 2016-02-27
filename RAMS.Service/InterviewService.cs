using RAMS.Data.Infrastructure;
using RAMS.Data.Repositories;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// InterviewService implements Interview specific services
    /// </summary>
    public class InterviewService : IInterviewService
    {
        IInterviewRepository InterviewRepository;

        ICandidateRepository CandidateRepository;

        IAgentRepository AgentRepository;

        IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="interviewRepository">Parameter for setting InterviewRepository</param>
        /// <param name="unitOfWork">Parameter for setting CandidateRepository</param>
        /// <param name="candidateRepository">Parameter for setting AgentRepository</param>
        /// <param name="agentRepository">Parameter for setting UnitOfWork</param>
        public InterviewService(IInterviewRepository interviewRepository, IUnitOfWork unitOfWork, ICandidateRepository candidateRepository, IAgentRepository agentRepository)
        {
            this.InterviewRepository = interviewRepository;

            this.CandidateRepository = candidateRepository;

            this.AgentRepository = agentRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the interviews
        /// </summary>
        /// <returns>All the interviews</returns>
        public IEnumerable<Interview> GetAllInterviews()
        {
            return this.InterviewRepository.GetAll();
        }

        /// <summary>
        /// Get interview with matching id
        /// </summary>
        /// <param name="id">Id of the interview to be compared with the context interviews' data</param>
        /// <returns>Interview with matching id</returns>
        public Interview GetOneInterviewById(int id)
        {
            return this.InterviewRepository.GetById(id);
        }

        /// <summary>
        /// Get multiple interviews that belong to specific candidate
        /// </summary>
        /// <param name="id">Id of the candidate who's interviews are being retrieved</param>
        /// <returns>Multiple interviews that belong to specific candidate</returns>
        public IEnumerable<Interview> GetManyInterviewsByCandidateId(int id)
        {
            var candidate = this.CandidateRepository.GetById(id);

            if (candidate != null)
            {
                return candidate.Interviews;
            }

            return null;
        }

        /// <summary>
        /// Get multiple interviews that belong to specific agent
        /// </summary>
        /// <param name="id">Id of the agent who's interviews are being retrieved</param>
        /// <returns>Multiple interviews that belong to specific agent</returns>
        public IEnumerable<Interview> GetManyInterviewsByAgentId(int id)
        {
            var agent = this.AgentRepository.GetById(id);

            if (agent != null)
            {
                return agent.Interviews;
            }

            return null;
        }

        /// <summary>
        /// Get multiple interviews that belong to specific agent by username
        /// </summary>
        /// <param name="username">Username of the agent who's interviews are being retrieved</param>
        /// <returns>Multiple interviews that belong to specific agent by username</returns>
        public IEnumerable<Interview> GetManyInterviewsByAgentUsername(string username)
        {
            var agent = this.AgentRepository.GetOneByUserName(username);

            if (agent != null)
            {
                return agent.Interviews;
            }

            return null;
        }

        /// <summary>
        /// Get multiple interviews with matching date
        /// </summary>
        /// <param name="date">Interview date to be compared with the context interviews' data</param>
        /// <returns>Multiple interviews with matching date</returns>
        public IEnumerable<Interview> GetManyInterviewsByDate(DateTime date)
        {
            return this.InterviewRepository.GetManyByDate(date);
        }
        #endregion

        /// <summary>
        /// Create new interview
        /// </summary>
        /// <param name="interview">Interview to be created</param>
        public void CreateInterview(Interview interview)
        {
            this.InterviewRepository.Add(interview);
        }

        /// <summary>
        /// Update existing interview
        /// </summary>
        /// <param name="interview">Interview to be updated</param>
        public void UpdateInterview(Interview interview)
        {
            this.InterviewRepository.Update(interview);
        }

        /// <summary>
        /// Delete existing interview
        /// </summary>
        /// <param name="interview">Interview to be deleted</param>
        public void DeleteInterview(Interview interview)
        {
            this.InterviewRepository.Delete(interview);
        }

        /// <summary>
        /// Save changes by using UnitOfWork's Commit method
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                this.UnitOfWork.Commit();
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateConcurrencyException ex)
            #pragma warning restore 0168 
            {
                throw;
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateException ex)
            #pragma warning restore 0168
            {
                throw;
            }
        }
    }
}
