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
    /// IAgentService interface declares the Agent specific service operations
    /// </summary>
    public interface IAgentService
    {
        IEnumerable<Agent> GetAllAgents();

        Agent GetOneAgentById(int id);

        Agent GetOneAgentByUserName(string userName);

        IEnumerable<Agent> GetManyAgentsByUserType(UserType userType);

        IEnumerable<Agent> GetManyAgentsByPhoneNumber(string phoneNumber);

        IEnumerable<Agent> GetManyAgentsByUserStatus(UserStatus userStatus);

        IEnumerable<Agent> GetManyAgentsByRole(Role role);

        Agent GetOneAgentByPositionId(int id);

        Agent GetOneAgentByNotificationId(int id);

        IEnumerable<Agent> GetManyAgentsByDepartmentId(int id);

        Agent GetOneAgentByInterviewId(int id);

        IEnumerable<Agent> GetManyAgentsByAgentStatus(AgentStatus agentStatus);

        void CreateAgent(Agent agent);

        void UpdateAgent(Agent agent);

        void DeleteAgent(Agent agent);

        void SaveChanges();
    }
}
