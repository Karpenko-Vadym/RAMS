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
    /// IAdminRepository interface declares the Admin specific repository operations
    /// </summary>
    public interface IAdminRepository : IRepository<Admin>
    {
        Admin GetOneByAdminId(int id);

        IEnumerable<Admin> GetAllAdmins();

        Admin GetOneByUserName(string userName);

        IEnumerable<Admin> GetManyByUserType(UserType userType);

        IEnumerable<Admin> GetManyByUserStatus(UserStatus userStatus);

        IEnumerable<Admin> GetManyByRole(Role role);

        IEnumerable<Admin> GetManyByFirstName(string firstName);

        IEnumerable<Admin> GetManyByLastName(string lastName);

        IEnumerable<Admin> GetManyByJobTitle(string jobTitle);

        IEnumerable<Admin> GetManyByCompanyName(string companyName);

        IEnumerable<Admin> GetManyByPhoneNumber(string phoneNumber);

        Admin GetOneByEmail(string email);

        IEnumerable<Admin> GetManyByEmail(string email);

        Admin GetOneByNotificationId(int id);

        Admin GetOneByNotification(Notification notification);
    }
}
