using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.ViewModels
{
    /// <summary>
    /// PositionListViewModel view model declares properties for _PositionList partial view
    /// </summary>
    public class PositionListViewModel
    {
        public int PositionId { get; set; }

        [Display(Name = "Id")]
        public string PositionIdForDisplay { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Company Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        [Display(Name = "Status")]
        public PositionStatus Status { get; set; }
    }

    /// <summary>
    /// PositionAddViewModel view model declares properties for _NewPosition partial view
    /// </summary>
    public class PositionAddViewModel
    {
        public int? AgentId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Company Details")]
        public string CompanyDetails { get; set; } // Company Description

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Qualifications")]
        public string Qualifications { get; set; } // Skills & Qualifications

        [Required]
        [Display(Name = "Asset Skills")]
        public string AssetSkills { get; set; } // Skills that are assets

        [Required]
        [Display(Name = "People Needed")]
        public int PeopleNeeded { get; set; } // How many people needed for this position

        [Required]
        [Display(Name = "Acceptance Score")]
        public int AcceptanceScore { get; set; } // Score cut off point, everything below will not qualify for the position

        public PositionStatus Status { get; set; }

        public List<System.Web.Mvc.SelectListItem> Departments { get; set; } // Select list for dropdowns

        public List<System.Web.Mvc.SelectListItem> Categories { get; set; } // Select list for dropdowns
    }
}
