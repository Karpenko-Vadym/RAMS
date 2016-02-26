using RAMS.Data.Infrastructure;
using RAMS.Data.Repositories;
using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// AgentService implements Agent specific services
    /// </summary>
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository AgentRepository;

        private readonly IPositionRepository PositionRepository;

        private readonly INotificationRepository NotificationRepository;

        private readonly IDepartmentRepository DepartmentRepository;

        private readonly IInterviewRepository InterviewRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="agentRepository">Parameter for setting AgentRepository</param>
        /// <param name="positionRepository">Parameter for setting PositionRepository</param>
        /// <param name="notificationRepository">Parameter for setting NotificationRepository</param>
        /// <param name="departmentRepository">Parameter for setting DepartmentRepository</param>
        /// <param name="interviewRepository">Parameter for setting InterviewRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public AgentService(IAgentRepository agentRepository, IPositionRepository positionRepository, INotificationRepository notificationRepository, IDepartmentRepository departmentRepository, IInterviewRepository interviewRepository, IUnitOfWork unitOfWork) 
        {
            this.AgentRepository = agentRepository;

            this.PositionRepository = positionRepository;

            this.NotificationRepository = notificationRepository;

            this.DepartmentRepository = departmentRepository;

            this.InterviewRepository = interviewRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the agents
        /// </summary>
        /// <returns>All the agents</returns>
        public IEnumerable<Agent> GetAllAgents()
        {
            return this.AgentRepository.GetAll();
        }

        /// <summary>
        /// Get an agent with matching id
        /// </summary>
        /// <param name="id">Id of the agent to be compared with the context agents' data</param>
        /// <returns>An agent with matching id</returns>
        public Agent GetOneAgentById(int id)
        {
            return this.AgentRepository.GetById(id);
        }

        /// <summary>
        /// Get an agent with matching user name
        /// </summary>
        /// <param name="userName">User name of the agent to be compared with the context agents' data</param>
        /// <returns>An agent with matching user name</returns>
        public Agent GetOneAgentByUserName(string userName)
        {
            return this.AgentRepository.GetOneByUserName(userName);
        }

        /// <summary>
        /// Get multiple agents with matching user type
        /// </summary>
        /// <param name="userType">User type of the agent to be compared with the context agents' data</param>
        /// <returns>Multiple agents with matching user type</returns>
        public IEnumerable<Agent> GetManyAgentsByUserType(UserType userType)
        {
            return this.AgentRepository.GetManyByUserType(userType);
        }

        /// <summary>
        /// Get multiple agents with matching phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number of the agent to be compared with the context agents' data</param>
        /// <returns>Multiple agents with matching phone number</returns>
        public IEnumerable<Agent> GetManyAgentsByPhoneNumber(string phoneNumber)
        {
            return this.AgentRepository.GetManyByPhoneNumber(phoneNumber);
        }

        /// <summary>
        /// Get multiple agents with matching user status
        /// </summary>
        /// <param name="userStatus">User status of the agent to be compared with the context agents' data</param>
        /// <returns>Multiple agents with matching user status</returns>
        public IEnumerable<Agent> GetManyAgentsByUserStatus(UserStatus userStatus)
        {
            return this.AgentRepository.GetManyByUserStatus(userStatus);
        }

        /// <summary>
        /// Get multiple agents with matching role
        /// </summary>
        /// <param name="role">Role of the agent to be compared with the context agents' data</param>
        /// <returns>Multiple agents with matching role</returns>
        public IEnumerable<Agent> GetManyAgentsByRole(Role role)
        {
            return this.AgentRepository.GetManyByRole(role);
        }

        /// <summary>
        /// Get an agent with specific position
        /// </summary>
        /// <param name="id">Id of the position for which agent is being retrieved</param>
        /// <returns>An agent with specific position</returns>
        public Agent GetOneAgentByPositionId(int id)
        {
            var position = this.PositionRepository.GetById(id);

            if (position != null)
            {
                return position.Agent;
            }

            return null;
        }

        /// <summary>
        /// Get an agent with specific notification
        /// </summary>
        /// <param name="id">Id of the notification for which agent is being retrieved</param>
        /// <returns>An agent with specific notification</returns>
        public Agent GetOneAgentByNotificationId(int id)
        {
            var notification = this.NotificationRepository.GetById(id);

            if (notification != null)
            {
                return notification.Agent;
            }

            return null;
        }

        /// <summary>
        /// Get multiple agents with specific department
        /// </summary>
        /// <param name="id">Id of the department for which agent is being retrieved</param>
        /// <returns>Multiple agents with specific department</returns>
        public IEnumerable<Agent> GetManyAgentsByDepartmentId(int id)
        {
            var department = this.DepartmentRepository.GetById(id);

            if (department != null)
            {
                return department.Agents;
            }

            return null;
        }

        /// <summary>
        /// Get an agent with specific interview
        /// </summary>
        /// <param name="id">Id of the interview for which agent is being retrieved</param>
        /// <returns>An agent with specific interview</returns>
        public Agent GetOneAgentByInterviewId(int id)
        {
            var interview = this.InterviewRepository.GetById(id);

            if (interview != null)
            {
                return interview.Interviewer;
            }

            return null;
        }

        /// <summary>
        /// Get multiple agents with matching status
        /// </summary>
        /// <param name="agentStatus">Status of the agent to be compared with the context agents' data</param>
        /// <returns>Multiple agents with matching status</returns>
        public IEnumerable<Agent> GetManyAgentsByAgentStatus(AgentStatus agentStatus)
        {
            return this.AgentRepository.GetManyByAgentStatus(agentStatus);
        }
        #endregion

        /// <summary>
        /// Create new agent
        /// </summary>
        /// <param name="agent">Agent to be created</param>
        public void CreateAgent(Agent agent)
        {
            this.AgentRepository.Add(agent);
        }

        /// <summary>
        /// Update existing agent
        /// </summary>
        /// <param name="agent">Agent to be updated</param>
        public void UpdateAgent(Agent agent)
        {
            this.AgentRepository.Update(agent);
        }

        /// <summary>
        /// Delete existing agent
        /// </summary>
        /// <param name="agent">Agent to be deleted</param>
        public void DeleteAgent(Agent agent)
        {
            this.AgentRepository.Delete(agent);
        }

        /// <summary>
        /// Save changes by using UnitOfWork's Commit method
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                this.UnitOfWork.Commit();
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateConcurrencyException ex)
            #pragma warning restore 0168 
            {
                throw;
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateException ex)
            #pragma warning restore 0168
            {
                throw;
            }
        }
    }
}
