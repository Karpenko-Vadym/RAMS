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
    /// ICandidateService interface declares the Candidate specific service operations
    /// </summary>
    public interface ICandidateService
    {
        IEnumerable<Candidate> GetAllCandidates();
        
        Candidate GetOneCandidateById(int id);

        IEnumerable<Candidate> GetManyCandidatesByPositionId(int id);

        Candidate GetOneCandidateByInterviewId(int id);

        IEnumerable<Candidate> GetManyCandidatesByAcceptanceScore(int score);

        IEnumerable<Candidate> GetManyCandidatesByStatus(CandidateStatus status);

        void CreateCandidate(Candidate candidate);

        void UpdateCandidate(Candidate candidate);

        void DeleteCandidate(Candidate candidate);

        void SaveChanges();
    }
}
