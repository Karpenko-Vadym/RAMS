using RAMS.Data.Infrastructure;
using RAMS.Data.Repositories;
using RAMS.Enums;
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
    /// CandidateService implements Candidate specific services
    /// </summary>
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository CandidateRepository;

        private readonly IPositionRepository PositionRepository;

        private readonly IInterviewRepository InterviewRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="candidateRepository">Parameter for setting CandidateRepository</param>
        /// <param name="positionRepository">Parameter for setting PositionRepository</param>
        /// <param name="interviewRepository">Parameter for setting InterviewRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public CandidateService(ICandidateRepository candidateRepository, IPositionRepository positionRepository, IInterviewRepository interviewRepository, IUnitOfWork unitOfWork) 
        {
            this.CandidateRepository = candidateRepository;

            this.PositionRepository = positionRepository;

            this.InterviewRepository = interviewRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the candidates
        /// </summary>
        /// <returns>All the candidates</returns>
        public IEnumerable<Candidate> GetAllCandidates()
        {
            return this.CandidateRepository.GetAll();
        }

        /// <summary>
        /// Get a candidate with matching id
        /// </summary>
        /// <param name="id">Id of the candidate to be compared with the context candidates' data</param>
        /// <returns>A candidate with matching id</returns>
        public Candidate GetOneCandidateById(int id)
        {
            return this.CandidateRepository.GetById(id);
        }

        /// <summary>
        /// Get multiple candidates with specific position
        /// </summary>
        /// <param name="id">Id of the position for which candidates are being retrieved</param>
        /// <returns>Multiple candidates with specific position</returns>
        public IEnumerable<Candidate> GetManyCandidatesByPositionId(int id)
        {
            var position = this.PositionRepository.GetById(id);

            return position.Candidates.ToList();
        }

        /// <summary>
        /// Get a candidate with specific interview
        /// </summary>
        /// <param name="id">Id of the interview for which candidate is being retrieved</param>
        /// <returns>A candidate with specific interview</returns>
        public Candidate GetOneCandidateByInterviewId(int id)
        {
            var interview = this.InterviewRepository.GetById(id);

            return interview.Candidate;
        }

        /// <summary>
        /// Get multiple candidates with matching acceptance score
        /// </summary>
        /// <param name="score">Acceptance score to be compared with the context candidates' data</param>
        /// <returns>Multiple candidates with matching acceptance score</returns>
        public IEnumerable<Candidate> GetManyCandidatesByAcceptanceScore(int score)
        {
            return this.CandidateRepository.GetManyByScore(score);
        }

        /// <summary>
        /// Get multiple candidates with matching status
        /// </summary>
        /// <param name="status">Status to be compared with the context candidates' data</param>
        /// <returns>Multiple candidates with matching status</returns>
        public IEnumerable<Candidate> GetManyCandidatesByStatus(CandidateStatus status)
        {
            return this.CandidateRepository.GetManyByStatus(status);
        }
        #endregion

        /// <summary>
        /// Create new candidate
        /// </summary>
        /// <param name="candidate">Candidate to be created</param>
        public void CreateCandidate(Candidate candidate)
        {
            this.CandidateRepository.Add(candidate);
        }

        /// <summary>
        /// Update existing candidate
        /// </summary>
        /// <param name="candidate">Candidate to be updated</param>
        public void UpdateCandidate(Candidate candidate)
        {
            this.CandidateRepository.Update(candidate);
        }

        /// <summary>
        /// Delete existing candidate
        /// </summary>
        /// <param name="candidate">Candidate to be deleted</param>
        public void DeleteCandidate(Candidate candidate)
        {
            this.CandidateRepository.Delete(candidate);
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
