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
    /// ICandidateRepository interface declares the Candidate specific repository operations
    /// </summary>
    public interface ICandidateRepository : IRepository<Candidate> 
    {
        Candidate GetOneByFirstName(string firstName);

        Candidate GetOneByLastName(string lastName);

        IEnumerable<Candidate> GetManyByEmail(string email); // Get by full email

        IEnumerable<Candidate> GetManyByPartialEmail(string email); // Get by partial email

        IEnumerable<Candidate> GetManyByCountry(string country);

        Candidate GetOneByPhoneNumber(string phoneNumber);

        IEnumerable<Candidate> GetManyByScore(int score); // Get by acceptance score

        IEnumerable<Candidate> GetManyByStatus(CandidateStatus status);

        IEnumerable<Candidate> GetManyByPosition(Position position);

        IEnumerable<Candidate> GetManyByPositionId(int id);

        Candidate GetOneByInterview(Interview interview);

        Candidate GetOneByInterviewId(int id);
    }
}
