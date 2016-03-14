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
    /// IPositionRepository interface declares the Position specific repository operations
    /// </summary>
    public interface IPositionRepository : IRepository<Position>
    {
        Position GetOneByPositionId(int id);

        IEnumerable<Position> GetAllPositions();

        IEnumerable<Position> GetManyByClientId(int id);

        IEnumerable<Position> GetManyByClient(Client client);

        IEnumerable<Position> GetManyByAgentId(int id);

        IEnumerable<Position> GetManyByAgent(Agent agent);

        IEnumerable<Position> GetManyByCategoryId(int id);

        IEnumerable<Position> GetManyByCategory(Category category);

        IEnumerable<Position> GetManyByDateCreated(DateTime date);

        IEnumerable<Position> GetManyByExpiryDate(DateTime date);

        IEnumerable<Position> GetManyByTitle(string title);

        IEnumerable<Position> GetManyByDescription(string description);

        IEnumerable<Position> GetManyByCompanyDetails(string companyDetails);

        IEnumerable<Position> GetManyByLocation(string location);

        IEnumerable<Position> GetManyByQualifications(string qualifications);

        IEnumerable<Position> GetManyByKeyword(string keyword);

        IEnumerable<Position> GetManyByAssetSkills(string assetSkills);

        IEnumerable<Position> GetManyByPeopleNeeded(int poepleNeeded);

        IEnumerable<Position> GetManyByAccepatanceScore(int acceptanceScore);

        IEnumerable<Position> GetManyByStatus(PositionStatus status);

        Position GetOneByCandidateId(int id);

        Position GetOneByCandidate(Candidate candidate);
    }
}
