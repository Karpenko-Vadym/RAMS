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
    /// Position repository implements Position specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class PositionRepository : RepositoryBase<Position>, IPositionRepository
    {
        public PositionRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get one position with matching position id
        /// </summary>
        /// <param name="id">Position id for comparing to context positions' data</param>
        /// <returns>One position with matching position id</returns>
        public Position GetOneByPositionId(int id)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Include("Candidates.Interviews").FirstOrDefault(p => p.PositionId == id);
        }

        /// <summary>
        /// Get all positions
        /// </summary>
        /// <returns>All positions</returns>
        public IEnumerable<Position> GetAllPositions()
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").ToList();
        }

        /// <summary>
        /// Get multiple positions with matching client id
        /// </summary>
        /// <param name="id">Client id for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching client id</returns>
        public IEnumerable<Position> GetManyByClientId(int id)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.ClientId == id).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching client
        /// </summary>
        /// <param name="client">Client for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching client</returns>
        public IEnumerable<Position> GetManyByClient(Client client)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.ClientId == client.ClientId).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching agent id
        /// </summary>
        /// <param name="id">Agent id for comparing to context positions' data</param>
        /// <returns></returns>
        public IEnumerable<Position> GetManyByAgentId(int id)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.AgentId == id).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching agent
        /// </summary>
        /// <param name="agent">Agent for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching agent</returns>
        public IEnumerable<Position> GetManyByAgent(Agent agent)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.AgentId == agent.AgentId).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching category id
        /// </summary>
        /// <param name="id">Category id for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching category id</returns>
        public IEnumerable<Position> GetManyByCategoryId(int id)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.CategoryId == id).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching category
        /// </summary>
        /// <param name="category">Category for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching category</returns>
        public IEnumerable<Position> GetManyByCategory(Category category)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.CategoryId == category.CategoryId).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching creation date
        /// </summary>
        /// <param name="date">Date for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching creation date</returns>
        public IEnumerable<Position> GetManyByDateCreated(DateTime date)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.DateCreated == date).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching expiry date
        /// </summary>
        /// <param name="date">Date for comparing to context positions' data</param>
        /// <returns>Get multiple positions with matching expiry date</returns>
        public IEnumerable<Position> GetManyByExpiryDate(DateTime date)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.ExpiryDate == date).ToList();
        }

        /// <summary>
        /// Get multiple positions with title containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="title">Title phrase to match with Positions' title field</param>
        /// <returns>Multiple positions with title containing the phrase provided in the parameter list</returns>
        public IEnumerable<Position> GetManyByTitle(string title)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.Title.ToLower().Trim().Contains(title.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple positions with description containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="description">Description phrase to match with Positions' description field</param>
        /// <returns>Multiple positions with description containing the phrase provided in the parameter list</returns>
        public IEnumerable<Position> GetManyByDescription(string description)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.Description.ToLower().Trim().Contains(description.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple positions with company details field containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="companyDetails">Company details phrase to match with Positions' company details field</param>
        /// <returns>Multiple positions with company details field containing the phrase provided in the parameter list</returns>
        public IEnumerable<Position> GetManyByCompanyDetails(string companyDetails)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.CompanyDetails.ToLower().Trim().Contains(companyDetails.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple positions with location field containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="location">Location phrase to match with Positions' location field</param>
        /// <returns>Multiple positions with location field containing the phrase provided in the parameter list</returns>
        public IEnumerable<Position> GetManyByLocation(string location)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.Location.ToLower().Trim().Contains(location.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple positions with qualifications field containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="qualifications">Qualifications phrase to match with Positions' qualifications field</param>
        /// <returns>Multiple positions with qualifications field containing the phrase provided in the parameter list</returns>
        public IEnumerable<Position> GetManyByQualifications(string qualifications)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.Qualifications.ToLower().Trim().Contains(qualifications.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple positions with any field containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="keyword">Keyword to match with all Positions' fields</param>
        /// <returns>Multiple positions with any field containing the phrase provided in the parameter list</returns>
        public IEnumerable<Position> GetManyByKeyword(string keyword)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.Category.Name.ToLower().Trim().Contains(keyword.ToLower().Trim()) || p.CompanyDetails.ToLower().Trim().Contains(keyword.ToLower().Trim()) || p.Description.ToLower().Trim().Contains(keyword.ToLower().Trim()) || p.Title.ToLower().Trim().Contains(keyword.ToLower().Trim()) || p.Location.ToLower().Trim().Contains(keyword.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple positions with asset skills field containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="assetSkills">Asset skills phrase to match with Positions' asset skills field</param>
        /// <returns>Multiple positions with asset skills field containing the phrase provided in the parameter list</returns>
        public IEnumerable<Position> GetManyByAssetSkills(string assetSkills)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.AssetSkills.ToLower().Trim().Contains(assetSkills.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching number of people needed
        /// </summary>
        /// <param name="peopleNeeded">Number of people needed for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching number of people needed</returns>
        public IEnumerable<Position> GetManyByPeopleNeeded(int peopleNeeded)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.PeopleNeeded == peopleNeeded).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching acceptance score
        /// </summary>
        /// <param name="acceptanceScore">Acceptance score for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching acceptance score</returns>
        public IEnumerable<Position> GetManyByAccepatanceScore(int acceptanceScore)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.AcceptanceScore == acceptanceScore).ToList();
        }

        /// <summary>
        /// Get multiple positions with matching status
        /// </summary>
        /// <param name="status">Status for comparing to context positions' data</param>
        /// <returns>Multiple positions with matching status</returns>
        public IEnumerable<Position> GetManyByStatus(PositionStatus status)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").Where(p => p.Status == status).ToList();
        }

        /// <summary>
        /// Get first position with matching candidate id
        /// </summary>
        /// <param name="id">Candidate id for comparing to context positions' data</param>
        /// <returns>First position with matching candidate id</returns>
        public Position GetOneByCandidateId(int id)
        {
            var candidate = this.GetContext.Candidates.FirstOrDefault(c => c.CandidateId == id) ?? new Candidate();

            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").FirstOrDefault(p => p.PositionId == candidate.PositionId);
        }

        /// <summary>
        /// Get first position with matching candidate
        /// </summary>
        /// <param name="candidate">Candidate for comparing to context positions' data</param>
        /// <returns>First position with matching candidate</returns>
        public Position GetOneByCandidate(Candidate candidate)
        {
            return this.GetContext.Positions.Include("Category").Include("Client").Include("Agent").Include("Candidates").FirstOrDefault(p => p.PositionId == candidate.PositionId);
        }
        #endregion

        /// <summary>
        /// Override of default Add operation in order to set status to New
        /// </summary>
        /// <param name="resource">Resource to be added</param>
        public override void Add(Position resource)
        {
            resource.Status = PositionStatus.New;

            base.Add(resource);
        }
    }
}

/*  IMPORTANT - Same person may apply for different positions, however, this person will be different Candidate and have diferent Id in the database...
 *  That is because this person (Candidate) may prefer to use different credentials (Resume and other personal criteria) for different positions.
 */