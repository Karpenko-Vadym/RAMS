using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace RAMS.ViewModels
{
    /// <summary>
    /// ClientProfileDetailsViewModel view model declares properties for _ProfileDetails partial view
    /// </summary>
    public class ClientProfileDetailsViewModel
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Company Name")]
        public string Company { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    /// <summary>
    /// ClientProfilePictureViewModel view model declares properties for _UploadProfilePicture partial view
    /// </summary>
    public class ClientProfilePictureViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        public ClientProfilePictureViewModel() { }

        public ClientProfilePictureViewModel(string userName)
        {
            this.UserName = userName;
        }
    }

    /// <summary>
    /// ClientListViewModel view model declares properties for _ClientList partial view
    /// </summary>
    public class ClientListViewModel
    {
        public int ClientId { get; set; }

        [Display(Name = "Id")]
        public string ClientIdForDisplay { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Positions Assigned")]
        public int Positions { get; set; }

        [Display(Name = "Company Name")]
        public string Company { get; set; }
    }

    /// <summary>
    /// ClientDetailsViewModel view model declares properties for _ClientDetails partial view
    /// </summary>
    public class ClientDetailsViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Total Positions")]
        public int Positions { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Company Name")]
        public string Company { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
    }
}
