using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace RAMS.ViewModels
{
    /// <summary>
    /// AdminProfileDetailsViewModel view model declares properties for _ProfileDetails partial view
    /// </summary>
    public class AdminProfileDetailsViewModel
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
    /// AdminProfilePictureViewModel view model declares properties for _UploadProfilePicture partial view
    /// </summary>
    public class AdminProfilePictureViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        public AdminProfilePictureViewModel() { }

        public AdminProfilePictureViewModel(string userName)
        {
            this.UserName = userName;
        }
    }
}
