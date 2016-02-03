using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Company Details")]
        public string CompanyDetails { get; set; } 

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Qualifications")]
        public string Qualifications { get; set; } 

        [Display(Name = "Asset Skills")]
        public string AssetSkills { get; set; } 

        [Required]
        [Display(Name = "People Needed")]
        public int PeopleNeeded { get; set; } 

        [Display(Name = "Acceptance Score")]
        public int AcceptanceScore { get; set; }

        public PositionStatus Status { get; set; }

        public List<System.Web.Mvc.SelectListItem> Categories { get; set; } // Select list for Category dropdown

        public PositionAddViewModel()
        {
            this.AcceptanceScore = 50;

            this.Status = PositionStatus.New;

            this.ExpiryDate = DateTime.Now;
        }
    }

    /// <summary>
    /// PositionConfirmationViewModel view model declares properties for _PositionConfirmation partial view
    /// </summary>
    public class PositionConfirmationViewModel
    {
        [Display(Name = "Id")]
        public int PositionId { get; set; }

        [Display(Name = "Requestor")]
        public string Client { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Company Details")]
        public string CompanyDetails { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Qualifications")]
        public string Qualifications { get; set; }

        [Display(Name = "Asset Skills")]
        public string AssetSkills { get; set; }

        [Display(Name = "People Needed")]
        public int PeopleNeeded { get; set; }

        [Display(Name = "Position Status")]
        public PositionStatus Status { get; set; }
    }

    /// <summary>
    /// PositionDetailsViewModel view model declares properties for _PositionDetails partial view
    /// </summary>
    public class PositionDetailsViewModel
    {
        [Display(Name = "Id")]
        public int PositionId { get; set; }

        public int AgentId { get; set; }

        [Display(Name = "Requestor")]
        public string Client { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Company Details")]
        public string CompanyDetails { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Qualifications")]
        public string Qualifications { get; set; }

        [Display(Name = "Asset Skills")]
        public string AssetSkills { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        [Display(Name = "People Needed")]
        public int PeopleNeeded { get; set; }

        [Display(Name = "Position Status")]
        public PositionStatus Status { get; set; }
    }

    /// <summary>
    /// PositionClosureConfirmationViewModel view model declares properties for _PositionClosureConfirmation partial view
    /// </summary>
    public class PositionClosureConfirmationViewModel
    {
        [Required]
        public int AgentId { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public string PositionTitle { get; set; }

        [Required]
        public string ClientUserName { get; set; }

        [Required]
        public string ClientFullName { get; set; }

        /// <summary>
        /// Default PositionClosureConfirmationViewModel constructor
        /// </summary>
        public PositionClosureConfirmationViewModel() { }

        /// <summary>
        /// PositionClosureConfirmationViewModel constructor that sets all its properties
        /// </summary>
        /// <param name="agentId">Setter for AgentId</param>
        /// <param name="positionId">Setter for PositionId</param>
        /// <param name="positionTitle">Setter for PositionTitle</param>
        /// <param name="clientUserName">Setter for ClientUserName</param>
        /// <param name="clientFullName">Setter for ClientFullName</param>
        public PositionClosureConfirmationViewModel(int agentId, int positionId, string positionTitle, string clientUserName, string clientFullName)
        {
            this.AgentId = agentId;

            this.PositionId = positionId;

            this.PositionTitle = positionTitle;

            this.ClientUserName = clientUserName;

            this.ClientFullName = clientFullName;
        }
    }

    /// <summary>
    /// PositionEditViewModel view model declares properties for _EditPosition partial view for agency
    /// </summary>
    public class PositionEditViewModel
    {
        public int? AgentId { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Company Details")]
        public string CompanyDetails { get; set; } 

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Qualifications")]
        public string Qualifications { get; set; } 

        [Display(Name = "Asset Skills")]
        public string AssetSkills { get; set; } 

        [Required]
        [Display(Name = "People Needed")]
        public int PeopleNeeded { get; set; }

        [Required]
        [Display(Name = "Acceptance Score")]
        public int AcceptanceScore { get; set; }

        [Required]
        public PositionStatus Status { get; set; }

        public List<System.Web.Mvc.SelectListItem> Categories { get; set; } // Select list for Category dropdown

        public List<CandidateListViewModel> Candidates { get; set; }

        /// <summary>
        /// Default PositionEditViewModel constructor         
        /// </summary>
        public PositionEditViewModel()
        {
            this.Candidates = new List<CandidateListViewModel>();
        }
    }
}
