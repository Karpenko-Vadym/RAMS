using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RAMS.ViewModels
{
    /// <summary>
    /// AgentProfileDetailsViewModel view model declares properties for _ProfileDetails partial view
    /// </summary>
    public class AgentProfileDetailsViewModel
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

        [Display(Name = "Status")]
        public AgentStatus AgentStatus { get; set; }

        [Display(Name = "Role")]
        public Role Role { get; set; }
    }

    /// <summary>
    /// AgentProfilePictureViewModel view model declares properties for _UploadProfilePicture partial view
    /// </summary>
    public class AgentProfilePictureViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        public AgentProfilePictureViewModel() { }

        public AgentProfilePictureViewModel(string userName)
        {
            this.UserName = userName;
        }
    }

    /// <summary>
    /// AgentAssignPositionViewModel view model declares properties for _AssignPosition partial view
    /// </summary>
    public class AgentAssignPositionViewModel
    {
        public int AgentId { get; set; }

        [Display(Name = "Id")]
        public string AgentIdForDisplay { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Positions")]
        public int Positions { get; set; }
    }

    /// <summary>
    /// AgentListViewModel view model declares properties for _AgentList partial view
    /// </summary>
    public class AgentListViewModel
    {
        public int AgentId { get; set; }

        [Display(Name = "Id")]
        public string AgentIdForDisplay { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Positions Assigned")]
        public int Positions { get; set; }

        [Display(Name = "Status")]
        public AgentStatus AgentStatus { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }
    }

    /// <summary>
    /// AgentDetailsViewModel view model declares properties for _AgentDetails partial view
    /// </summary>
    public class AgentDetailsViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Total Positions")]
        public int PositionsTotal { get; set; }

        [Display(Name = "Current Positions")]
        public int PositionsCurrent { get; set; }

        [Display(Name = "Interviews Conducted")]
        public int InterviewsCompleted { get; set; }

        [Display(Name = "Interviews Pending")]
        public int InterviewsPending { get; set; }

        [Display(Name = "Status")]
        public AgentStatus AgentStatus { get; set; }

        [Display(Name = "Role")]
        public Role Role { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
