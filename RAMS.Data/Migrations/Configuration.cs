namespace RAMS.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RAMS.Data.Infrastructure;
    using RAMS.Data.Repositories;
    using RAMS.Enums;
    using RAMS.Models;
    using RAMS.Web.Identity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// Configuration class allows us to configure migrations and seed the database
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<RAMS.Data.DataContext>
    {
        public static IDataFactory dataFactory = new DataFactory();

        public static IUnitOfWork unitOfWork = new UnitOfWork(dataFactory);

        public static IClientRepository clientRepository = new ClientRepository(dataFactory);

        public static IPositionRepository positionRepository = new PositionRepository(dataFactory);

        public static ICategoryRepository categoryRepository = new CategoryRepository(dataFactory);

        public static IDepartmentRepository departmentRepository = new DepartmentRepository(dataFactory);

        public static IAdminRepository adminRepository = new AdminRepository(dataFactory);

        public static IAgentRepository agentRepository = new AgentRepository(dataFactory);

        public static ICandidateRepository candidateRepository = new CandidateRepository(dataFactory);

        public static IInterviewRepository interviewRepository = new InterviewRepository(dataFactory);

        public static INotificationRepository notificationRepository = new NotificationRepository(dataFactory);

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Seed method attempts to intsert the initial data into the database everytime when database is being updated after migration
        /// </summary>
        /// <param name="context">Database context on which seed method is being called (Not used in this scenario because repositories are used to insert the data)</param>
        protected override void Seed(RAMS.Data.DataContext context)
        {
            // Add users
            using (var applicationDbContext = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(applicationDbContext);
                
                var userManager = new UserManager<ApplicationUser>(userStore);

                if (userManager.FindByName("john.doe") == null)
                {
                    var users = GetUsers();

                    foreach (var user in users)
                    {
                        userManager.Create(user, "123RAMSApp!");

                        // Add user claims for each user
                        if (user.UserName == "superuser")
                        {
                            userManager.AddClaim(user.Id, new Claim("FullName", "superuser"));
                            userManager.AddClaim(user.Id, new Claim("Role", "Manager"));
                            userManager.AddClaim(user.Id, new Claim("UserType", "Admin"));
                            userManager.AddClaim(user.Id, new Claim("UserStatus", "Active"));
                        }
                        else if (user.UserName == "john.doe")
                        {
                            userManager.AddClaim(user.Id, new Claim("FullName", "John Doe"));
                            userManager.AddClaim(user.Id, new Claim("Role", "Manager"));
                            userManager.AddClaim(user.Id, new Claim("UserType", "Agent"));
                            userManager.AddClaim(user.Id, new Claim("UserStatus", "Active"));
                        }
                        else if (user.UserName == "james.smith")
                        {
                            userManager.AddClaim(user.Id, new Claim("FullName", "James Smith"));
                            userManager.AddClaim(user.Id, new Claim("Role", "Employee"));
                            userManager.AddClaim(user.Id, new Claim("UserType", "Agent"));
                            userManager.AddClaim(user.Id, new Claim("UserStatus", "Active"));
                        }
                        else if (user.UserName == "mary.watson")
                        {
                            userManager.AddClaim(user.Id, new Claim("FullName", "Mary Watson"));
                            userManager.AddClaim(user.Id, new Claim("Role", "Employee"));
                            userManager.AddClaim(user.Id, new Claim("UserType", "Agent"));
                            userManager.AddClaim(user.Id, new Claim("UserStatus", "Active"));
                        }
                        else if (user.UserName == "tommy.jordan")
                        {
                            userManager.AddClaim(user.Id, new Claim("FullName", "Tommy Jordan"));
                            userManager.AddClaim(user.Id, new Claim("Role", "Employee"));
                            userManager.AddClaim(user.Id, new Claim("UserType", "Admin"));
                            userManager.AddClaim(user.Id, new Claim("UserStatus", "Active"));
                        }
                        else if (user.UserName == "kathy.doe")
                        {
                            userManager.AddClaim(user.Id, new Claim("FullName", "Kathy Doe"));
                            userManager.AddClaim(user.Id, new Claim("Role", "Employee"));
                            userManager.AddClaim(user.Id, new Claim("UserType", "Admin"));
                            userManager.AddClaim(user.Id, new Claim("UserStatus", "Active"));
                        }
                        else if (user.UserName == "jimmy.thomson")
                        {
                            userManager.AddClaim(user.Id, new Claim("FullName", "Jimmy Thomson"));
                            userManager.AddClaim(user.Id, new Claim("Role", "Employee"));
                            userManager.AddClaim(user.Id, new Claim("UserType", "Client"));
                            userManager.AddClaim(user.Id, new Claim("UserStatus", "Active"));
                        }
                        else if (user.UserName == "nancy.clinton")
                        {
                            userManager.AddClaim(user.Id, new Claim("FullName", "Nancy Clinton"));
                            userManager.AddClaim(user.Id, new Claim("Role", "Employee"));
                            userManager.AddClaim(user.Id, new Claim("UserType", "Client"));
                            userManager.AddClaim(user.Id, new Claim("UserStatus", "Active"));
                        }
                    }
                }
            }

            // Add departments
            if (!departmentRepository.GetAll().Any())
            {
                GetDepartments().ForEach(d => departmentRepository.Add(d));

                unitOfWork.Commit();
            }

            // Add agents
            if (!agentRepository.GetAll().Any())
            {
                GetAgents().ForEach(a => agentRepository.Add(a));

                unitOfWork.Commit();
            }

            // Add clients
            if (!clientRepository.GetAll().Any())
            {
                GetClients().ForEach(c => clientRepository.Add(c));

                unitOfWork.Commit();
            }

            // Add admins
            if (!adminRepository.GetAll().Any())
            {
                GetAdmins().ForEach(a => adminRepository.Add(a));

                unitOfWork.Commit();
            }

            // Add categories
            if (!categoryRepository.GetAll().Any())
            {
                GetCategories().ForEach(c => categoryRepository.Add(c));

                unitOfWork.Commit();
            }

            // Add positions
            if (!positionRepository.GetAll().Any())
            {
                GetPositions().ForEach(p => positionRepository.Add(p));

                unitOfWork.Commit();
            }

            // Add candidates
            if (!candidateRepository.GetAll().Any())
            {
                GetCandidates().ForEach(c => candidateRepository.Add(c));

                unitOfWork.Commit();
            }

            // Add interiews
            if (!interviewRepository.GetAll().Any())
            {
                GetInterviews().ForEach(i => interviewRepository.Add(i));

                unitOfWork.Commit();
            }

            // Add notifications
            if (!notificationRepository.GetAll().Any())
            {
                GetNotifications().ForEach(n => notificationRepository.Add(n));

                unitOfWork.Commit();
            }

            
        }
        #region Getters
        /// <summary>
        /// Gettter that returns a list of Departments
        /// </summary>
        /// <returns>List of Departments</returns>
        private static List<Department> GetDepartments()
        {
            return new List<Department>
            {
                new Department { Name = "Human Resources" },
                new Department { Name = "Marketing" },
                new Department { Name = "Research & Development" }
            };
        }

        /// <summary>
        /// Gettter that returns a list of Agents
        /// </summary>
        /// <returns>List of Agents</returns>
        private static List<Agent> GetAgents()
        {
            return new List<Agent>
            {
                new Agent 
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Company = "Recruiting Agency Inc.",
                    AgentStatus = AgentStatus.FullTime,
                    Email = "john.doe@email.com",
                    DepartmentId = departmentRepository.GetAll().FirstOrDefault().DepartmentId,
                    JobTitle = "Relations Manager",
                    PhoneNumber = "416 123 4567",
                    Role = Role.Manager,
                    UserName = "john.doe",
                    UserType = UserType.Agent,
                    UserStatus = UserStatus.Active,
                },
                new Agent 
                {
                    FirstName = "James",
                    LastName = "Smith",
                    Company = "Recruiting Agency Inc.",
                    AgentStatus = AgentStatus.FullTime,
                    Email = "james.smith@email.com",
                    DepartmentId = departmentRepository.GetAll().FirstOrDefault().DepartmentId,
                    JobTitle = "Training Co-ordinator",
                    PhoneNumber = "416 123 4567",
                    Role = Role.Employee,
                    UserName = "james.smith",
                    UserType = UserType.Agent,
                    UserStatus = UserStatus.Active
                },
                new Agent 
                {
                    FirstName = "Mary",
                    LastName = "Watson",
                    Company = "Recruiting Agency Inc.",
                    AgentStatus = AgentStatus.FullTime,
                    Email = "mary.watson@email.com",
                    DepartmentId = departmentRepository.GetAll().FirstOrDefault().DepartmentId,
                    JobTitle = "Recruiting",
                    PhoneNumber = "416 123 4567",
                    Role = Role.Employee,
                    UserName = "mary.watson",
                    UserType = UserType.Agent,
                    UserStatus = UserStatus.Active
                }
            };
        }

        /// <summary>
        /// Gettter that returns a list of Clients
        /// </summary>
        /// <returns>List of Clients</returns>
        private static List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    FirstName = "Jimmy",
                    LastName = "Thomson",
                    Company = "Company That Hires People Inc.",
                    Email = "jimmy.thomson@email.com",
                    JobTitle = "General manager",
                    PhoneNumber = "416 123 4567",
                    Role = Role.Employee,
                    UserName = "jimmy.thomson",
                    UserType = UserType.Client,
                    UserStatus = UserStatus.Active
                },
                new Client
                {
                    FirstName = "Nancy",
                    LastName = "Clinton",
                    Company = "Another Company That Hires People Inc.",
                    Email = "nancy.clinton@email.com",
                    JobTitle = "Project manager",
                    PhoneNumber = "416 123 4567",
                    Role = Role.Employee,
                    UserName = "nancy.clinton",
                    UserType = UserType.Client,
                    UserStatus = UserStatus.Active
                }
            };
        }

        /// <summary>
        /// Gettter that returns a list of Admins
        /// </summary>
        /// <returns>List of Clients</returns>
        private static List<Admin> GetAdmins()
        {
            return new List<Admin>
            {
                new Admin
                {
                    FirstName = "Super",
                    LastName = "User",
                    Company = "Not Applicable",
                    Email = "superuser@email.com",
                    JobTitle = "Not Applicable",
                    PhoneNumber = "Not Applicable",
                    Role = Role.Manager,
                    UserName = "superuser",
                    UserType = UserType.Admin,
                    UserStatus = UserStatus.Active,
                },
                new Admin
                {
                    FirstName = "Tommy",
                    LastName = "Jordan",
                    Company = "Company That Hires People Inc.",
                    Email = "tommy.jordan@email.com",
                    JobTitle = "Database Administrator",
                    PhoneNumber = "416 123 4567",
                    Role = Role.Employee,
                    UserName = "tommy.jordan",
                    UserType = UserType.Admin,
                    UserStatus = UserStatus.Active,
                },
                new Admin
                {
                    FirstName = "Kathy",
                    LastName = "Doe",
                    Company = "Another Company That Hires People Inc.",
                    Email = "kathy.doe@email.com",
                    JobTitle = "System Admin",
                    PhoneNumber = "416 123 4567",
                    Role = Role.Employee,
                    UserName = "kathy.doe",
                    UserType = UserType.Admin,
                    UserStatus = UserStatus.Active
                }
            };
        }

        /// <summary>
        /// Gettter that returns a list of Categories
        /// </summary>
        /// <returns>List of Categories</returns>
        public static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Name = "Banking & Finance"
                },
                new Category
                {
                    Name = "Customer Service"
                },
                new Category
                {
                    Name = "Engineering"
                },
                new Category
                {
                    Name = "Government"
                },
                new Category
                {
                    Name = "Health Care"
                },
                new Category
                {
                    Name = "Information Technology"
                },
                new Category
                {
                    Name = "Sales & Marketing"
                },
                new Category
                {
                    Name = "Science & Biotech"
                },
                new Category
                {
                    Name = "Transportation"
                }
            };
        }

        /// <summary>
        /// Gettter that returns a list of Positions
        /// </summary>
        /// <returns>List of Positions</returns>
        private static List<Position> GetPositions()
        {
            return new List<Position>
            {
                new Position
                {
                    AcceptanceScore = 30,
                    AssetSkills = "French; Russian; Chinese",
                    CategoryId = categoryRepository.GetOneByName("Health Care").CategoryId,
                    ClientId = clientRepository.GetOneByUserName("jimmy.thomson").ClientId,
                    AgentId = agentRepository.GetOneByUserName("james.smith").AgentId,
                    CompanyDetails = "<p>Eclipse is a well known clinical agency that provides treatments and assesments to elderly people.</p>",
                    DateCreated = DateTime.UtcNow,
                    Description = "<p><strong>Responsibilities include:</strong></p><ul><li>Conduct individual assesments;</li><li>Maintain clinical records according to standards and policies;</li><li>Provide family therapy;</li><li>Create treatment plans;</li></ul><p><strong>Skills &amp; Qualifications:</strong></p><ul><li>Masters degreee in Clinical Medicine (MCM);</li><li>Strong communication skills (Verbal and written);</li><li>Excellent organizational skills;</li></ul><p><strong>Required experience:</strong></p><ul><li>2 years in clinical therapy;</li></ul>",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Location = "Toronto",
                    PeopleNeeded = 8,
                    Qualifications = "MCM; Clinical Medicine; Communication; Therapy",
                    Status = PositionStatus.New,
                    Title = "Clinical Therapist"
                },
                new Position
                {
                    AcceptanceScore = 30,
                    AssetSkills = "Immunology; Medicine; French",
                    CategoryId = categoryRepository.GetOneByName("Science & Biotech").CategoryId,
                    ClientId = clientRepository.GetOneByUserName("jimmy.thomson").ClientId,
                    AgentId = agentRepository.GetOneByUserName("james.smith").AgentId,
                    CompanyDetails = "<p>We are a fast-growing pahrma/biotech startup based in the heart of Montreal&#39;s downtown core.</p>",
                    DateCreated = DateTime.UtcNow,
                    Description = "<p>We are seeking a Research Assistant to join the research department in a biotech startup company. Relevant industry experience is highly desirable. Ideal candidate should possess deep knowledge in cellular biology.</p><p><strong>Position Requirements:</strong></p><ul><li>3+ years experience in biotech industry.</li><li>M.Sc. or Ph.D. in a technical field.</li><li>Strong written and verbal communication skills in English.</li><li>Knowledge in pharma.</li><li>Ability to communicate&nbsp;in French is a big asset.</li></ul>",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Location = "Montreal",
                    PeopleNeeded = 5,
                    Qualifications = "Biotech; Pharma; Drugs; MSc",
                    Status = PositionStatus.New,
                    Title = "Research Assistant"
                },
                new Position
                {
                    AcceptanceScore = 20,
                    AssetSkills = "Immunology; Medicine; French",
                    CategoryId = categoryRepository.GetOneByName("Information Technology").CategoryId,
                    ClientId = clientRepository.GetOneByUserName("jimmy.thomson").ClientId,
                    AgentId = agentRepository.GetOneByUserName("james.smith").AgentId,
                    CompanyDetails = "<p>Eclipse is one of the leaders in the industry and providing services for many customers and partners on daily basis.</p>",
                    DateCreated = DateTime.UtcNow,
                    Description = "<p><strong>Responsibilities</strong></p><ul><li>Design, implement, test, and maintain web based applications using various technologies;</li><li>Actively participate in team meetings and other project related events;</li><li>Research new technologies and improve the application accordingly;</li><li>Ensure that an application is upgraded and updated at all times;</li></ul><p><strong>Skills &amp; Qualifications:</strong></p><ul><li>3+ years of analytical and programming experience in C#, SQL, JQuery/JavaScript;</li><li>Knowlegde of following technologies: ASP.NET MVC, ASP.NET Web API, LINQ, Bootstrap;</li></ul><p><strong>Softskills:</strong></p><ul><li>Team player;</li><li>Dedicated and detail oriented;</li><li>Possess analytical mindset;</li><li>Customer oriented;</li></ul>",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Location = "Montreal",
                    PeopleNeeded = 5,
                    Qualifications = "Biotech; Pharma; Drugs; MSc",
                    Status = PositionStatus.New,
                    Title = "ASP.NET Developer"
                },
                new Position
                {
                    AcceptanceScore = 50,
                    AssetSkills = "Word; Excel; HPQC",
                    CategoryId = categoryRepository.GetOneByName("Information Technology").CategoryId,
                    ClientId = clientRepository.GetOneByUserName("jimmy.thomson").ClientId,
                    AgentId = agentRepository.GetOneByUserName("james.smith").AgentId,
                    CompanyDetails = "Our company has been on the market for many years and proven to have high quality standards.",
                    DateCreated = DateTime.UtcNow,
                    Description = "The Technical Support Specialist’s primary mission is to provide customers with in-service support.",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Location = "Guelph",
                    PeopleNeeded = 2,
                    Qualifications = "C#; ASP.NET; MVC; Web API; JavaScript; JQuery; Bootstrap",
                    Status = PositionStatus.New,
                    Title = "Technical Support Specialist"
                },
                new Position
                {
                    AcceptanceScore = 60,
                    AssetSkills = "Word; Excel; Visio",
                    CategoryId = categoryRepository.GetOneByName("Customer Service").CategoryId,
                    ClientId = clientRepository.GetOneByUserName("nancy.clinton").ClientId,
                    AgentId = agentRepository.GetOneByUserName("john.doe").AgentId,
                    CompanyDetails = "ABC Development, provides end-to-end application solutions that address the big data challenges of mission-critical business functions.",
                    DateCreated = DateTime.UtcNow,
                    Description = "Write and/or edit technical documents, including business proposals, reports, user manuals, and other project deliverables",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Location = "Mississauga",
                    PeopleNeeded = 3,
                    Qualifications = "Epic; McKesson; EHR",
                    Status = PositionStatus.New,
                    Title = "Technical Writer"
                },
                new Position
                {
                    AcceptanceScore = 10,
                    AssetSkills = "Class A CDL; Class A Commercial Driver's License",
                    CategoryId = categoryRepository.GetOneByName("Transportation").CategoryId,
                    ClientId = clientRepository.GetOneByUserName("jimmy.thomson").ClientId,
                    AgentId = agentRepository.GetOneByUserName("mary.watson").AgentId,
                    CompanyDetails = "Our company has been on the market for many years and proven to have high quality standards.",
                    DateCreated = DateTime.UtcNow,
                    Description = "The Technical Support Specialist’s primary mission is to provide customers with in-service support.",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Location = "Montreal",
                    PeopleNeeded = 5,
                    Qualifications = "Class B CDL; Class B Commercial Driver's License",
                    Status = PositionStatus.New,
                    Title = "Commercial Truck Driver"
                }
            };
        }

        /// <summary>
        /// Gettter that returns a list of Candidates
        /// </summary>
        /// <returns>List of Candidates</returns>
        private static List<Candidate> GetCandidates()
        {
            return new List<Candidate>
            {
                new Candidate
                {
                    Country = "Canada",
                    Email = "danny1985@email.com",
                    Feedback = "",
                    FirstName = "Danny",
                    LastName = "Goldman",
                    PhoneNumber = "(416) 738-7762",
                    PositionId = positionRepository.GetManyByStatus(PositionStatus.New).FirstOrDefault().PositionId,
                    PostalCode = "M2R 7T3",
                    Status = CandidateStatus.New,
                    Score = 78
                }
            };
        }

        /// <summary>
        /// Gettter that returns a list of Interviews
        /// </summary>
        /// <returns>List of Interviews</returns>
        private static List<Interview> GetInterviews()
        {
            return new List<Interview>
            {
                new Interview
                {
                    InterviewDate = DateTime.UtcNow.AddMonths(3),
                    CandidateId = candidateRepository.GetAll().FirstOrDefault().CandidateId,
                    InterviewerId = agentRepository.GetAll().FirstOrDefault().AgentId,
                    Status = InterviewStatus.Scheduled
                }
            };
        }

        /// <summary>
        /// Gettter that returns a list of Notifications
        /// </summary>
        /// <returns>List of Notifications</returns>
        private static List<Notification> GetNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    AdminId = adminRepository.GetOneByUserName("superuser").AdminId,
                    DateCreated = DateTime.UtcNow,
                    Details = "Your account has been created on " + DateTime.UtcNow.ToString(),
                    Status = NotificationStatus.Unread,
                    Title = "Welcome to RAMS!"
                },
                new Notification
                {
                    AgentId = agentRepository.GetOneByUserName("john.doe").AgentId,
                    DateCreated = DateTime.UtcNow,
                    Details = "Your account has been created on " + DateTime.UtcNow.ToString(),
                    Status = NotificationStatus.Unread,
                    Title = "Welcome to RAMS!"
                },
                new Notification
                {
                    AgentId = agentRepository.GetOneByUserName("james.smith").AgentId,
                    DateCreated = DateTime.UtcNow,
                    Details = "Your account has been created on " + DateTime.UtcNow.ToString(),
                    Status = NotificationStatus.Unread,
                    Title = "Welcome to RAMS!"
                },
                new Notification
                {
                    AgentId = agentRepository.GetOneByUserName("mary.watson").AgentId,
                    DateCreated = DateTime.UtcNow,
                    Details = "Your account has been created on " + DateTime.UtcNow.ToString(),
                    Status = NotificationStatus.Unread,
                    Title = "Welcome to RAMS!"
                },
                new Notification
                {
                    ClientId = clientRepository.GetOneByUserName("jimmy.thomson").ClientId,
                    DateCreated = DateTime.UtcNow,
                    Details = "Your account has been created on " + DateTime.UtcNow.ToString(),
                    Status = NotificationStatus.Unread,
                    Title = "Welcome to RAMS!"
                },
                new Notification
                {
                    ClientId = clientRepository.GetOneByUserName("nancy.clinton").ClientId,
                    DateCreated = DateTime.UtcNow,
                    Details = "Your account has been created on " + DateTime.UtcNow.ToString(),
                    Status = NotificationStatus.Unread,
                    Title = "Welcome to RAMS!"
                },
                new Notification
                {
                    AdminId = adminRepository.GetOneByUserName("tommy.jordan").AdminId,
                    DateCreated = DateTime.UtcNow,
                    Details = "Your account has been created on " + DateTime.UtcNow.ToString(),
                    Status = NotificationStatus.Unread,
                    Title = "Welcome to RAMS!"
                },
                new Notification
                {
                    AdminId = adminRepository.GetOneByUserName("kathy.doe").AdminId,
                    DateCreated = DateTime.UtcNow,
                    Details = "Your account has been created on " + DateTime.UtcNow.ToString(),
                    Status = NotificationStatus.Unread,
                    Title = "Welcome to RAMS!"
                }
            };
        }

        /// <summary>
        /// Gettter that returns a list of ApplicationUsers
        /// </summary>
        /// <returns>List of ApplicationUsers</returns>
        private static List<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Email = "superuser@email.com",
                    PhoneNumber = "Not Applicable",
                    UserName = "superuser",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser 
                {
                    Email = "john.doe@email.com",  
                    PhoneNumber = "416 123 4567",
                    UserName = "john.doe",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser 
                {

                    Email = "james.smith@email.com",
                    PhoneNumber = "416 123 4567",
                    UserName = "james.smith",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser 
                {
                    Email = "mary.watson@email.com",
                    PhoneNumber = "416 123 4567",
                    UserName = "mary.watson",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    Email = "tommy.jordan@email.com",
                    PhoneNumber = "416 123 4567",
                    UserName = "tommy.jordan",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    Email = "kathy.doe@email.com",
                    PhoneNumber = "416 123 4567",
                    UserName = "kathy.doe",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    Email = "jimmy.thomson@email.com",
                    PhoneNumber = "416 123 4567",
                    UserName = "jimmy.thomson",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser
                {
                    Email = "nancy.clinton@email.com",
                    PhoneNumber = "416 123 4567",
                    UserName = "nancy.clinton",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                }
            };
        }
        #endregion
    }
}
