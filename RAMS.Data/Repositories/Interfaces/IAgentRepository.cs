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
    /// IAgentRepository interface declares the Agent specific repository operations
    /// </summary>
    public interface IAgentRepository : IRepository<Agent> 
    {
        Agent GetOneByUserName(string userName);

        IEnumerable<Agent> GetManyByUserType(UserType userType);

        IEnumerable<Agent> GetManyByUserStatus(UserStatus userStatus);

        IEnumerable<Agent> GetManyByRole(Role role);

        IEnumerable<Agent> GetManyByFirstName(string firstName);

        IEnumerable<Agent> GetManyByLastName(string lastName);

        IEnumerable<Agent> GetManyByJobTitle(string jobTitle);

        IEnumerable<Agent> GetManyByCompanyName(string companyName);

        IEnumerable<Agent> GetManyByPhoneNumber(string phoneNumber);

        Agent GetOneByEmail(string email);

        IEnumerable<Agent> GetManyByEmail(string email);

        IEnumerable<Agent> GetManyByDepartmentId(int id);

        IEnumerable<Agent> GetManyByDepartment(Department department);

        Agent GetOneByPositionId(int id);

        Agent GetOneByPosition(Position position);

        Agent GetOneByNotificationId(int id);

        Agent GetOneByNotification(Notification notification);

        IEnumerable<Agent> GetManyByAgentStatus(AgentStatus status);
    }
}
