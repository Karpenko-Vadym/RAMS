using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// IInterviewService interface declares the Interview specific service operations
    /// </summary>
    public interface IInterviewService
    {
        IEnumerable<Interview> GetAllInterviews();

        Interview GetOneInterviewById(int id);

        IEnumerable<Interview> GetManyInterviewsByCandidateId(int id);

        IEnumerable<Interview> GetManyInterviewsByAgentId(int id);

        IEnumerable<Interview> GetManyInterviewsByDate(DateTime date);

        void CreateInterview(Interview interview);

        void UpdateInterview(Interview interview);

        void DeleteInterview(Interview interview);

        void SaveChanges();
    }
}
