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
    /// IClientRepository interface declares the Client specific repository operations
    /// </summary>
    public interface IClientRepository : IRepository<Client> 
    {
        Client GetOneByClientId(int id);

        IEnumerable<Client> GetAllClients();

        Client GetOneByUserName(string userName);

        IEnumerable<Client> GetManyByUserType(UserType userType);

        IEnumerable<Client> GetManyByUserStatus(UserStatus userStatus);

        IEnumerable<Client> GetManyByRole(Role role);

        IEnumerable<Client> GetManyByFirstName(string firstName);

        IEnumerable<Client> GetManyByLastName(string lastName);

        IEnumerable<Client> GetManyByJobTitle(string jobTitle);

        IEnumerable<Client> GetManyByCompanyName(string companyName);

        IEnumerable<Client> GetManyByPhoneNumber(string phoneNumber);

        Client GetOneByEmail(string email);

        IEnumerable<Client> GetManyByEmail(string email);

        Client GetOneByPositionId(int id);

        Client GetOneByPosition(Position position);

        Client GetOneByNotificationId(int id);

        Client GetOneByNotification(Notification notification);
    }
}
