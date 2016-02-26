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
    /// AdminService implements Admin specific services
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository AdminRepository;

        private readonly INotificationRepository NotificationRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="adminRepository">Parameter for setting AdminRepository</param>
        /// <param name="notificationRepository">Parameter for setting NotificationRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public AdminService(IAdminRepository adminRepository, INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            this.AdminRepository = adminRepository;

            this.NotificationRepository = notificationRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the admins
        /// </summary>
        /// <returns>All the admins</returns>
        public IEnumerable<Admin> GetAllAdmins()
        {
            return this.AdminRepository.GetAll();
        }

        /// <summary>
        /// Get an admin with matching id
        /// </summary>
        /// <param name="id">Id of the admin to be compared with the context admins' data</param>
        /// <returns>An admin with matching id</returns>
        public Admin GetOneAdminById(int id)
        {
            return this.AdminRepository.GetById(id);
        }

        /// <summary>
        /// Get an admin with matching user name
        /// </summary>
        /// <param name="userName">User name of the admin to be compared with the context admins' data</param>
        /// <returns>An admin with matching user name</returns>
        public Admin GetOneAdminByUserName(string userName)
        {
            return this.AdminRepository.GetOneByUserName(userName);
        }

        /// <summary>
        /// Get multiple admins with matching user type
        /// </summary>
        /// <param name="userType">User type of the admin to be compared with the context admins' data</param>
        /// <returns>Multiple admins with matching user type</returns>
        public IEnumerable<Admin> GetManyAdminsByUserType(UserType userType)
        {
            return this.AdminRepository.GetManyByUserType(userType);
        }

        /// <summary>
        /// Get multiple admins with matching phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number of the admin to be compared with the context admins' data</param>
        /// <returns>Multiple admins with matching phone number</returns>
        public IEnumerable<Admin> GetManyAdminsByPhoneNumber(string phoneNumber)
        {
            return this.AdminRepository.GetManyByPhoneNumber(phoneNumber);
        }

        /// <summary>
        /// Get multiple admins with matching user status
        /// </summary>
        /// <param name="userStatus">User status of the admin to be compared with the context admins' data</param>
        /// <returns>Multiple admins with matching user status</returns>
        public IEnumerable<Admin> GetManyAdminsByUserStatus(UserStatus userStatus)
        {
            return this.AdminRepository.GetManyByUserStatus(userStatus);
        }

        /// <summary>
        /// Get multiple admins with matching role
        /// </summary>
        /// <param name="role">Admin's role to be compared with the context admins' data</param>
        /// <returns>Multiple admins with matching role</returns>
        public IEnumerable<Admin> GetManyAdminsByRole(Role role)
        {
            return this.AdminRepository.GetManyByRole(role);
        }

        /// <summary>
        /// Get an admin with specific notification
        /// </summary>
        /// <param name="id">Id of the notification for which admin is being retrieved</param>
        /// <returns>An admin with specific notification</returns>
        public Admin GetOneAdminByNotificationId(int id)
        {
            var notification = this.NotificationRepository.GetById(id);

            if (notification != null)
            {
                return notification.Admin;
            }

            return null;
        }
        #endregion

        /// <summary>
        /// Create new admin
        /// </summary>
        /// <param name="admin">Admin to be created</param>
        public void CreateAdmin(Admin admin)
        {
            this.AdminRepository.Add(admin);
        }

        /// <summary>
        /// Update existing admin
        /// </summary>
        /// <param name="admin">Admin to be updated</param>
        public void UpdateAdmin(Admin admin)
        {
            this.AdminRepository.Update(admin);
        }

        /// <summary>
        /// Delete existing admin
        /// </summary>
        /// <param name="admin">Admin to be deleted</param>
        public void DeleteAdmin(Admin admin)
        {
            this.AdminRepository.Delete(admin);
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
