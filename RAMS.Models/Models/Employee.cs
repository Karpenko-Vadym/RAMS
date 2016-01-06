using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Models
{
    /// <summary>
    /// Employee represents company employee that will be interacting with the system. It can be Client, Agent, Manager, or System Admin
    /// </summary>
    public class Employee : BaseEntity
    {
        public string UserName { get; set; } // User name is the connection between Employee class and User class (Must be unique)

        public UserType UserType { get; set; } // User type defines to whom employee belongs to (Client, Agent/Manager, or SystemAdmin)

        public Role Role { get; set; } // Role determines what options are available for this employee 

        public UserStatus UserStatus { get; set; } // User can be Blocked, Active, Deleted

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public string Company { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string MediaType { get; set; } // File media type

        public byte[] FileContent { get; set; } // File content 
    }

    /// <summary>
    /// Client is an employee from the side of Client (Customer)
    /// </summary>
    public class Client : Employee
    {
        public int ClientId { get; set; }

        public virtual List<Position> Positions { get; set; }

        public virtual List<Notification> Notifications { get; set; }

        // Default constructor to initialize list(s) and other properties
        public Client()
        {
            this.Positions = new List<Position>();

            this.Notifications = new List<Notification>();
        }
    }

    /// <summary>
    /// Agent is an employee from the side of Recruiting Agency
    /// </summary>
    public class Agent : Employee
    {
        public int AgentId { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual List<Position> Positions { get; set; }

        public virtual List<Interview> Interviews { get; set; }

        public virtual List<Notification> Notifications { get; set; }

        public AgentStatus AgentStatus { get; set; }

        // Default constructor to initialize list(s) and other properties
        public Agent()
        {
            this.Positions = new List<Position>();

            this.Interviews = new List<Interview>();

            this.Notifications = new List<Notification>();
        }
    }

    /// <summary>
    /// Admin is an employee who is responsible for managing all application users
    /// </summary>
    public class Admin : Employee
    {
        public int AdminId { get; set; }

        public virtual List<Notification> Notifications { get; set; }

        public Admin()
        {
            this.Notifications = new List<Notification>();
        }
    }
}