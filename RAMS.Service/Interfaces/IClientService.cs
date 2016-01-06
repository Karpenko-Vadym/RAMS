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
    /// IClientService interface declares the Client specific service operations
    /// </summary>
    public interface IClientService
    {
        IEnumerable<Client> GetAllClients();

        Client GetOneClientById(int id);

        Client GetOneClientByUserName(string userName);

        IEnumerable<Client> GetManyClientsByUserType(UserType userType);

        IEnumerable<Client> GetManyClientsByPhoneNumber(string phoneNumber);

        IEnumerable<Client> GetManyClientsByUserStatus(UserStatus userStatus);

        IEnumerable<Client> GetManyCleintsByRole(Role role);

        Client GetOneClientByPositionId(int id);

        Client GetOneClientByNotificationId(int id);

        void CreateClient(Client client);

        void UpdateClient(Client client);

        void DeleteClient(Client client);

        void SaveChanges();
    }
}
