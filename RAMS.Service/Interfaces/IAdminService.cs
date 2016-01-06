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
    /// IAdminService interface declares the Admin specific service operations
    /// </summary>
    public interface IAdminService
    {
        IEnumerable<Admin> GetAllAdmins();

        Admin GetOneAdminById(int id);

        Admin GetOneAdminByUserName(string userName);

        IEnumerable<Admin> GetManyAdminsByUserType(UserType userType);

        IEnumerable<Admin> GetManyAdminsByPhoneNumber(string phoneNumber);

        IEnumerable<Admin> GetManyAdminsByUserStatus(UserStatus userStatus);

        IEnumerable<Admin> GetManyAdminsByRole(Role role);

        Admin GetOneAdminByNotificationId(int id);

        void CreateAdmin(Admin admin);

        void UpdateAdmin(Admin admin);

        void DeleteAdmin(Admin admin);

        void SaveChanges();
    }
}
