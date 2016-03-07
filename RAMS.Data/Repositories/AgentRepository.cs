using RAMS.Data.Infrastructure;
using RAMS.Enums;
using RAMS.Helpers;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Repositories
{
    /// <summary>
    /// Agent repository implements Agent specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class AgentRepository : RepositoryBase<Agent>, IAgentRepository
    {
        public AgentRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get first agent with matching agent id
        /// </summary>
        /// <param name="id">Id to match with agents' data</param>
        /// <returns>First agent with matching id</returns>
        public Agent GetOneByAgentId(int id)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Include("Interviews.Candidate").Include("Interviews.Interviewer").FirstOrDefault(a => a.AgentId == id);
        }

        /// <summary>
        /// Get all agents
        /// </summary>
        /// <returns>All agents</returns>
        public IEnumerable<Agent> GetAllAgents()
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications");
        }

        /// <summary>
        /// Get first agent with matching user name
        /// </summary>
        /// <param name="userName">User name of the agent</param>
        /// <returns>First agent with matching user name</returns>
        public Agent GetOneByUserName(string userName)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Positions.Candidates").Include("Positions.Category").Include("Interviews").Include("Notifications").Include("Interviews.Candidate").Include("Interviews.Interviewer").FirstOrDefault(a => a.UserName == userName);
        }

        /// <summary>
        /// Get multiple agents with matching user tpe
        /// </summary>
        /// <param name="userType">User type to match with agents' data</param>
        /// <returns>Multiple agents with matching user tpe</returns>
        public IEnumerable<Agent> GetManyByUserType(UserType userType)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.UserType == userType).ToList();
        }

        /// <summary>
        /// Get multiple agents with matching user status
        /// </summary>
        /// <param name="userStatus">User status to match with agents' data</param>
        /// <returns>Multiple agents with matching user status</returns>
        public IEnumerable<Agent> GetManyByUserStatus(UserStatus userStatus)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.UserStatus == userStatus).ToList();
        }

        /// <summary>
        /// Get multiple agents with matching role
        /// </summary>
        /// <param name="role">Role to match with agents' data</param>
        /// <returns>Multiple agents with matching role</returns>
        public IEnumerable<Agent> GetManyByRole(Role role)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.Role == role).ToList();
        }

        /// <summary>
        /// Get multiple agents with matching first name
        /// </summary>
        /// <param name="firstName">First name to match with agents' data</param>
        /// <returns>Multiple agents with matching first name</returns>
        public IEnumerable<Agent> GetManyByFirstName(string firstName)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.FirstName == firstName).ToList();
        }

        /// <summary>
        /// Get multiple agents with matching last name
        /// </summary>
        /// <param name="lastName">Last name to match with agents' data</param>
        /// <returns>Multiple agents with matching last name</returns>
        public IEnumerable<Agent> GetManyByLastName(string lastName)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.LastName == lastName).ToList();
        }

        /// <summary>
        /// Get multiple agents with matching job title
        /// </summary>
        /// <param name="jobTitle">Job title to match with agents' data</param>
        /// <returns>Multiple agents with matching job title</returns>
        public IEnumerable<Agent> GetManyByJobTitle(string jobTitle)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.JobTitle == jobTitle).ToList();
        }

        /// <summary>
        /// Get multiple agents with matching company name
        /// </summary>
        /// <param name="companyName">Company name to match with agents' data</param>
        /// <returns>Multiple agents with matching company name</returns>
        public IEnumerable<Agent> GetManyByCompanyName(string companyName)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.Company == companyName).ToList();
        }

        /// <summary>
        /// Get multiple agents with matching phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number to match with agents' data</param>
        /// <returns>Multiple agents with matching phone number</returns>
        public IEnumerable<Agent> GetManyByPhoneNumber(string phoneNumber)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.PhoneNumber == phoneNumber).ToList();
        }

        /// <summary>
        /// Get first agent with matching email
        /// </summary>
        /// <param name="email">Email to match with agents' data</param>
        /// <returns>First agent with matching email</returns>
        public Agent GetOneByEmail(string email)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").FirstOrDefault(a => a.Email == email);
        }

        /// <summary>
        /// Get multiple agents with matching partial email (e. g. email.com, john.doe)
        /// </summary>
        /// <param name="email">Partial email to match with agents' data</param>
        /// <returns>Multiple agents with matching partial email</returns>
        public IEnumerable<Agent> GetManyByEmail(string email)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.Email.ToLower().Trim().Contains(email.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple agents with matching department id
        /// </summary>
        /// <param name="id">Department id to match with agents' data</param>
        /// <returns>Multiple agents with matching department id</returns>
        public IEnumerable<Agent> GetManyByDepartmentId(int id)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.DepartmentId == id).ToList();
        }

        /// <summary>
        /// Get multiple agents with mathing department
        /// </summary>
        /// <param name="department">Department to match with agents' data</param>
        /// <returns>Multiple agents with mathing department</returns>
        public IEnumerable<Agent> GetManyByDepartment(Department department)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.DepartmentId == department.DepartmentId).ToList();
        }

        /// <summary>
        /// Get first agent with matching position id
        /// </summary>
        /// <param name="id">Position id to match with agents' data</param>
        /// <returns>First agent with matching position id</returns>
        public Agent GetOneByPositionId(int id)
        {
            var position = this.GetContext.Positions.FirstOrDefault(p => p.PositionId == id) ?? new Position();

            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").FirstOrDefault(a => a.AgentId == position.AgentId);
        }

        /// <summary>
        /// Get first agent with matching position
        /// </summary>
        /// <param name="position">Position to match with agents' data</param>
        /// <returns>First agent with matching position</returns>
        public Agent GetOneByPosition(Position position)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").FirstOrDefault(a => a.AgentId == position.AgentId);
        }

        /// <summary>
        /// Get first agent with matching notification id
        /// </summary>
        /// <param name="id">Notification id to match with agents' data</param>
        /// <returns>First agent with matching notification id</returns>
        public Agent GetOneByNotificationId(int id)
        {
            var notification = this.GetContext.Notifications.FirstOrDefault(n => n.NotificationId == id) ?? new Notification();

            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").FirstOrDefault(a => a.AgentId == notification.AgentId);
        }

        /// <summary>
        /// Get first agent with matching notification
        /// </summary>
        /// <param name="notification">Notification to match with agents' data</param>
        /// <returns>First agent with matching notification</returns>
        public Agent GetOneByNotification(Notification notification)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").FirstOrDefault(a => a.AgentId == notification.AgentId);
        }

        /// <summary>
        /// Get first agent with matching agent status
        /// </summary>
        /// <param name="status">Agent status to match with agents' data</param>
        /// <returns>First agent with matching agent status</returns>
        public IEnumerable<Agent> GetManyByAgentStatus(AgentStatus status)
        {
            return this.GetContext.Agents.Include("Department").Include("Positions").Include("Interviews").Include("Notifications").Where(a => a.AgentStatus == status).ToList();
        }
        #endregion
    }
}
