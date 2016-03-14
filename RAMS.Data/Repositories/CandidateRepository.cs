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
    /// Candidate repository implements Candidate specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class CandidateRepository : RepositoryBase<Candidate>, ICandidateRepository
    {
        public CandidateRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get first candidate with matching candidate id
        /// </summary>
        /// <param name="id">Id to match with candidates' data</param>
        /// <returns>First candidate with matching id</returns>
        public Candidate GetOneByCandidateId(int id)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").FirstOrDefault(c => c.CandidateId == id);
        }

        /// <summary>
        /// Get all candidates
        /// </summary>
        /// <returns>All candidates</returns>
        public IEnumerable<Candidate> GetAllCandidates()
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews");
        }

        /// <summary>
        /// Get first candidate with matching first name
        /// </summary>
        /// <param name="firstName">First name for comparing to context candidates' first names</param>
        /// <returns>First canditate with matching first name</returns>
        public Candidate GetOneByFirstName(string firstName)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").FirstOrDefault(c => c.FirstName == firstName);
        }

        /// <summary>
        /// Get first candidate with matching last name
        /// </summary>
        /// <param name="lastName">Last name for comparing to context candidates' last names</param>
        /// <returns>First canditate with matching last name</returns>
        public Candidate GetOneByLastName(string lastName)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").FirstOrDefault(c => c.LastName == lastName);
        }

        /// <summary>
        /// Get multiple candidates with matching email
        /// </summary>
        /// <param name="email">Email for comparing to context candidates' email addresses</param>
        /// <returns>Multiple canditates with matching email address</returns>
        public IEnumerable<Candidate> GetManyByEmail(string email)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").Where(c => c.Email == email).ToList();
        }

        /// <summary>
        /// Get multiple candidates with matching partial email (e. g. email.com, john.doe)
        /// </summary>
        /// <param name="email">Partial email for comparing to context candidates' email addresses</param>
        /// <returns>Multiple candidates with matching email address</returns>
        public IEnumerable<Candidate> GetManyByPartialEmail(string email)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").Where(c => c.Email.ToLower().Trim().Contains(email.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple candidates with matching country
        /// </summary>
        /// <param name="country">County for comparing to context candidates' countries</param>
        /// <returns>Multiple candidates with matching country</returns>
        public IEnumerable<Candidate> GetManyByCountry(string country)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").Where(c => c.Country.ToLower().Trim().Contains(country.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get first candidate with matching phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number for comparing to context candidates' phone numbers</param>
        /// <returns>First candidate with matching phone number</returns>
        public Candidate GetOneByPhoneNumber(string phoneNumber)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").FirstOrDefault(c => this.ToDigits(c.PhoneNumber.Trim()) == this.ToDigits(phoneNumber.Trim()));
        }

        /// <summary>
        /// Get multiple candidates with their score higher than acceptance score provided as a parameter
        /// </summary>
        /// <param name="score">Acceptance score</param>
        /// <returns>Multiple candidates with their score higher than acceptance score provided as a parameter</returns>
        public IEnumerable<Candidate> GetManyByScore(int score)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").Where(c => c.Score >= score).ToList();
        }

        /// <summary>
        /// Get multiple candidates with their status matching the status provided as a parameter
        /// </summary>
        /// <param name="status">Status for comparing to context candidates' statuses</param>
        /// <returns>Multiple candidates with their status matching the status provided as a parameter</returns>
        public IEnumerable<Candidate> GetManyByStatus(CandidateStatus status)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").Where(c => c.Status == status).ToList();
        }

        /// <summary>
        /// Get multiple candidates with their position matching the position provided as a parameter
        /// </summary>
        /// <param name="position">Position for comparing to context candidates' positions</param>
        /// <returns>Multiple candidates with their position matching the position provided as a parameter</returns>
        public IEnumerable<Candidate> GetManyByPosition(Position position)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").Where(c => c.PositionId == position.PositionId).ToList();
        }

        /// <summary>
        /// Get multiple candidates with their position matching the position id of which provided as a parameter
        /// </summary>
        /// <param name="id">Position id</param>
        /// <returns>Multiple candidates with their position matching the position id of which provided as a parameter</returns>
        public IEnumerable<Candidate> GetManyByPositionId(int id)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").Where(c => c.PositionId == id).ToList();
        }

        /// <summary>
        /// Get first candidate with his interview matching the interview provided as a parameter
        /// </summary>
        /// <param name="interview">Interview for comparing to context candidates' interviews</param>
        /// <returns>First candidate with his interview matching the interview provided as a parameter</returns>
        public Candidate GetOneByInterview(Interview interview)
        {
            return this.GetContext.Candidates.Include("Position").Include("Interviews").FirstOrDefault(c => c.CandidateId == interview.CandidateId);
        }

        /// <summary>
        /// Get first candidate with his interview matching the interview id of which provided as a parameter
        /// </summary>
        /// <param name="id">Interview id for comparing to context candidates' data</param>
        /// <returns>First candidate with his interview matching the interview id of which provided as a parameter</returns>
        public Candidate GetOneByInterviewId(int id)
        {
            var interview = this.GetContext.Interviews.FirstOrDefault(i => i.InterviewId == id);

            return this.GetContext.Candidates.Include("Position").Include("Interviews").FirstOrDefault(c => c.CandidateId == interview.CandidateId);
        }
        #endregion

        /// <summary>
        /// Override of default Add operation in order to set status to New
        /// </summary>
        /// <param name="resource">Resource to be added</param>
        public override void Add(Candidate resource)
        {
            resource.Status = CandidateStatus.New;

            base.Add(resource);
        }

        #region Helpers
        /// <summary>
        /// Strip all the characters from the string except digits
        /// </summary>
        /// <param name="data">String that may contain non-digit charactes</param>
        /// <returns>String with all the digits and no other characters</returns>
        private string ToDigits(string data)
        {
            return new string(data.Where(d => char.IsDigit(d)).ToArray());
        }
        #endregion
    }
}
