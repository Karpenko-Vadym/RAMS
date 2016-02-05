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
    /// PositionService implements Position specific services
    /// </summary>
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository PositionRepository;

        private readonly ICategoryRepository CategoryRepository;

        private readonly IDepartmentRepository DepartmentRepository;

        private readonly IClientRepository ClientRepository;

        private readonly IAgentRepository AgentRepository;

        private readonly ICandidateRepository CandidateRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="positionRepository">Parameter for setting PositionRepository</param>
        /// <param name="categoryRepository">Parameter for setting CategoryRepository</param>
        /// <param name="departmentRepository">Parameter for setting DepartmentRepository</param>
        /// <param name="clientRepository">Parameter for setting ClientRepository</param>
        /// <param name="agentRepository">Parameter for setting AgentRepository</param>
        /// <param name="candidateRepository">Parameter for setting CandidateRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public PositionService(IPositionRepository positionRepository, ICategoryRepository categoryRepository, IDepartmentRepository departmentRepository, IClientRepository clientRepository, IAgentRepository agentRepository, ICandidateRepository candidateRepository, IUnitOfWork unitOfWork)
        {
            this.PositionRepository = positionRepository;

            this.CategoryRepository = categoryRepository;

            this.DepartmentRepository = departmentRepository;

            this.ClientRepository = clientRepository;

            this.AgentRepository = agentRepository;

            this.CandidateRepository = candidateRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the positions
        /// </summary>
        /// <returns>All the positions</returns>
        public IEnumerable<Position> GetAllPositions()
        {
            return this.PositionRepository.GetAll();
        }

        /// <summary>
        /// Get position with matching id
        /// </summary>
        /// <param name="id">Id of the position to be compared with the context positions' data</param>
        /// <returns>Position with matching id</returns>
        public Position GetOnePositionById(int id)
        {
            return this.PositionRepository.GetById(id);
        }

        /// <summary>
        /// Get multiple positions that belong to specific agent
        /// </summary>
        /// <param name="id">Id of the agent who's positions are being retrieved</param>
        /// <returns>Multiple positions that belong to specific agent</returns>
        public IEnumerable<Position> GetManyPositionsByAgentId(int id)
        {
            var agent = this.AgentRepository.GetById(id);

            return agent.Positions;
        }

        /// <summary>
        /// Get multiple positions that belong to specific client
        /// </summary>
        /// <param name="id">Id of the client who's positions are being retrieved</param>
        /// <returns>Multiple positions that belong to specific client</returns>
        public IEnumerable<Position> GetManyPositionsByClientId(int id)
        {
            var client = this.ClientRepository.GetById(id);

            return client.Positions;
        }

        /// <summary>
        /// Get multiple positions that belong to specific client
        /// </summary>
        /// <param name="clientName">User name of the client who's positions are being retrieved</param>
        /// <returns>Multiple positions that belong to specific client</returns>
        public IEnumerable<Position> GetManyPositionsByClientName(string clientName)
        {
            var client = this.ClientRepository.GetOneByUserName(clientName);

            return client.Positions;
        }

        /// <summary>
        /// Get multiple positions that belong to specific agent
        /// </summary>
        /// <param name="agentName">User name of the agent who's positions are being retrieved</param>
        /// <returns>Multiple positions that belong to specific agent</returns>
        public IEnumerable<Position> GetManyPositionsByAgentName(string agentName)
        {
            var agent = this.AgentRepository.GetOneByUserName(agentName);

            return agent.Positions;
        }

        /// <summary>
        /// Get multiple positions with specific category
        /// </summary>
        /// <param name="id">Id of the category for which positions are being retrieved</param>
        /// <returns>Multiple positions with specific category</returns>
        public IEnumerable<Position> GetManyPositionsByCategoryId(int id)
        {
            var category = this.CategoryRepository.GetById(id);

            return category.Positions;
        }


        /// <summary>
        /// Get multiple positions with matching creation date 
        /// </summary>
        /// <param name="dateCreated">Creation date to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching creation date</returns>
        public IEnumerable<Position> GetManyPositionsByCreationDate(DateTime dateCreated)
        {
            return this.PositionRepository.GetManyByDateCreated(dateCreated);
        }

        /// <summary>
        /// Get multiple positions with matching expiry date
        /// </summary>
        /// <param name="expiryDate">Expiry date to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching expiry date</returns>
        public IEnumerable<Position> GetManyPositionsByExpiryDate(DateTime expiryDate)
        {
            return this.PositionRepository.GetManyByExpiryDate(expiryDate);
        }

        /// <summary>
        /// Get multiple positions with matching title
        /// </summary>
        /// <param name="title">Title to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching title</returns>
        public IEnumerable<Position> GetManyPositionsByTitle(string title)
        {
            return this.PositionRepository.GetManyByTitle(title);
        }

        /// <summary>
        /// Get multiple positions with matching description (A word or a phrase)
        /// </summary>
        /// <param name="description">Descripton to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching description (A word or a phrase)</returns>
        public IEnumerable<Position> GetManyPositionsByDescription(string description)
        {
            return this.PositionRepository.GetManyByDescription(description);
        }

        /// <summary>
        /// Get multiple positions with matching company details (Full or partial)
        /// </summary>
        /// <param name="companyDetails">Company details to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching company details (Full or partial)</returns>
        public IEnumerable<Position> GetManyPositionsByCompanyDetails(string companyDetails)
        {
            return this.PositionRepository.GetManyByCompanyDetails(companyDetails);
        }

        /// <summary>
        /// Get multiple positions with matching location (Full or partial)
        /// </summary>
        /// <param name="location">Location to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching location (Full or partial)</returns>
        public IEnumerable<Position> GetManyPositionsByLocation(string location)
        {
            return this.PositionRepository.GetManyByLocation(location);
        }

        /// <summary>
        /// Get multiple positions with matching qualifications (Full or partial)
        /// </summary>
        /// <param name="qualifications">Qualifications to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching qualifications (Full or partial)</returns>
        public IEnumerable<Position> GetManyPositionsByQualifications(string qualifications)
        {
            return this.PositionRepository.GetManyByQualifications(qualifications);
        }

        /// <summary>
        /// Get multiple positions with matching asset skills (Full or partial)
        /// </summary>
        /// <param name="assetSkills">Asset skills to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching asset skills (Full or partial)</returns>
        public IEnumerable<Position> GetManyPositionsByAssetSkills(string assetSkills)
        {
            return this.PositionRepository.GetManyByAssetSkills(assetSkills);
        }

        /// <summary>
        /// Get single position with specific candidate
        /// </summary>
        /// <param name="id">Id of the candidate who's position is being retrieved</param>
        /// <returns>Single position with specific candidate</returns>
        public Position GetOnePositionByCandidateId(int id)
        {
            var candidate = this.CandidateRepository.GetById(id);

            return candidate.Position;
        }

        /// <summary>
        /// Get multiple positions with matching number of people needed
        /// </summary>
        /// <param name="peopleNeeded">Number of people needed to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching number of people needed</returns>
        public IEnumerable<Position> GetManyPositionsByPeopleNeeded(int peopleNeeded)
        {
            return this.PositionRepository.GetManyByPeopleNeeded(peopleNeeded);
        }

        /// <summary>
        /// Get multiple positions with matching acceptance score
        /// </summary>
        /// <param name="acceptanceScore">Acceptance score to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching acceptance score</returns>
        public IEnumerable<Position> GetManyPositionsByAcceptanceScore(int acceptanceScore)
        {
            return this.PositionRepository.GetManyByAccepatanceScore(acceptanceScore);
        }

        /// <summary>
        /// Get multiple positions with matching status
        /// </summary>
        /// <param name="status">Status to be compared with the context positions' data</param>
        /// <returns>Multiple positions with matching status</returns>
        public IEnumerable<Position> GetManyPositionsByStatus(PositionStatus status)
        {
            return this.PositionRepository.GetManyByStatus(status);
        }
        #endregion

        /// <summary>
        /// Create new positions
        /// </summary>
        /// <param name="position">Position to be created</param>
        public void CreatePosition(Position position)
        {
            this.PositionRepository.Add(position);
        }

        /// <summary>
        /// Update existing position
        /// </summary>
        /// <param name="position">Position to be updates</param>
        public void UpdatePosition(Position position)
        {
            this.PositionRepository.Update(position);
        }

        /// <summary>
        /// Delete existing position
        /// </summary>
        /// <param name="position">Position to be deleted</param>
        public void DeletePosition(Position position)
        {
            this.PositionRepository.Delete(position);
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
