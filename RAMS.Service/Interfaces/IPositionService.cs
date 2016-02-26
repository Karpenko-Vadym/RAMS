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
    /// IPositionService interface declares the Position specific service operations
    /// </summary>
    public interface IPositionService
    {
        IEnumerable<Position> GetAllPositions();

        Position GetOnePositionById(int id);

        IEnumerable<Position> GetManyPositionsByAgentId(int id);

        IEnumerable<Position> GetManyPositionsByClientId(int id);

        IEnumerable<Position> GetManyPositionsByClientName(string clientName);

        IEnumerable<Position> GetManyPositionsByAgentName(string agentName);

        IEnumerable<Position> GetManyPositionsByCategoryId(int id);

        IEnumerable<Position> GetManyPositionsByCategoryName(string name);

        IEnumerable<Position> GetManyPositionsByCreationDate(DateTime dateCreated);

        IEnumerable<Position> GetManyPositionsByExpiryDate(DateTime expiryDate);

        IEnumerable<Position> GetManyPositionsByTitle(string title);

        IEnumerable<Position> GetManyPositionsByDescription(string description);

        IEnumerable<Position> GetManyPositionsByCompanyDetails(string companyDetails);

        IEnumerable<Position> GetManyPositionsByLocation(string location);

        IEnumerable<Position> GetManyPositionsByQualifications(string qualifications);

        IEnumerable<Position> GetManyPositionsByKeyword(string keyword);

        IEnumerable<Position> GetManyPositionsByAssetSkills(string assetSkills);

        Position GetOnePositionByCandidateId(int id);

        IEnumerable<Position> GetManyPositionsByPeopleNeeded(int peopleNeeded);

        IEnumerable<Position> GetManyPositionsByAcceptanceScore(int acceptanceScore);

        IEnumerable<Position> GetManyPositionsByStatus(PositionStatus status);

        void CreatePosition(Position position);

        void UpdatePosition(Position position);

        void DeletePosition(Position position);

        void SaveChanges();
    }
}
