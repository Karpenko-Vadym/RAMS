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

        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

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

            this.ExpiryDate = DateTime.UtcNow;
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
        [Required]
        [Display(Name = "Position Id")]
        public int PositionId { get; set; }

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

        public byte[] Timestamp { get; set; }

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

    /// <summary>
    /// PositionApprovalViewModel view model declares properties for _ApprovePosition partial view
    /// </summary>
    public class PositionApprovalViewModel
    {
        [Required]
        public int PositionId { get; set; }

        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Default PositionApprovalViewModel constructor
        /// </summary>
        public PositionApprovalViewModel()
        {
            this.PositionId = 0;

            this.Title = String.Empty;
        }

        /// <summary>
        /// PositionApprovalViewModel constructor that sets all its properties
        /// </summary>
        /// <param name="positionId">Setter for PositionId</param>
        /// <param name="title">Setter for Title</param>
        public PositionApprovalViewModel(int positionId, string title)
        {
            this.PositionId = positionId;

            this.Title = title;
        }
    }

    /// <summary>
    /// PositionClosureViewModel view model declares properties for _ClosePosition partial view
    /// </summary>
    public class PositionClosureViewModel
    {
        [Required]
        public int PositionId { get; set; }

        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Default PositionClosureViewModel constructor
        /// </summary>
        public PositionClosureViewModel()
        {
            this.PositionId = 0;

            this.Title = String.Empty;
        }

        /// <summary>
        /// PositionClosureViewModel constructor that sets all its properties
        /// </summary>
        /// <param name="positionId">Setter for PositionId</param>
        /// <param name="title">Setter for Title</param>
        public PositionClosureViewModel(int positionId, string title)
        {
            this.PositionId = positionId;

            this.Title = title;
        }
    }

    /// <summary>
    /// PositionAssignViewModel view model declares properties for _AssignPosition partial view
    /// </summary>
    public class PositionAssignViewModel
    {
        [Required]
        public int PositionId { get; set; }

        [Required]
        public int AgentId {get; set; }

        [Required]
        public int SelectedAgentId { get; set; }

        [Required]
        public string Title { get; set; }

        public List<AgentAssignPositionViewModel> Agents { set; get; }

        /// <summary>
        /// Default PositionAssignViewModel constructor
        /// </summary>
        public PositionAssignViewModel()
        {
            this.PositionId = 0;

            this.AgentId = 0;

            this.Title = String.Empty;

            this.Agents = new List<AgentAssignPositionViewModel>();
        }

        /// <summary>
        /// PositionAssignViewModel constructor that sets all its properties
        /// </summary>
        /// <param name="positionId">Setter for PositionId</param>
        /// <param name="agentId">Setter for AgentId</param>
        /// <param name="title">Setter for Title</param>
        public PositionAssignViewModel(int positionId, int agentId = 0, string title = "")
        {
            this.PositionId = positionId;

            this.AgentId = agentId;

            this.Title = title;

            this.Agents = new List<AgentAssignPositionViewModel>();
        }
    }

    /// <summary>
    /// PositionResultViewModel view model declares properties for _SuccessConfirmation, and _FailureConfirmation partial views in Position controller
    /// </summary>
    public class PositionResultViewModel
    {
        public int PositionId {get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Refresh Edit Form?")]
        public bool RefreshEditForm { get; set; }

        [Display(Name = "Refresh List?")]
        public bool RefreshList { get; set; }

        [Display(Name = "Clear Messages?")]
        public bool ClearMessages { get; set; }

        /// <summary>
        /// Default PositionResultViewModel constructor sets all the properties default values
        /// </summary>
        public PositionResultViewModel()
        {
            this.Message = String.Empty;

            this.RefreshEditForm = false;

            this.RefreshList = false;

            this.ClearMessages = false;
        }

        /// <summary>
        /// PositionResultViewModel constructor which sets the Message property
        /// </summary>
        /// <param name="message">Setter for message property</param>
        public PositionResultViewModel(string message)
        {
            this.Message = message;

            this.RefreshEditForm = false;

            this.RefreshList = false;

            this.ClearMessages = false;
        }

        /// <summary>
        /// PositionResultViewModel constructor which sets Message, RefreshEditForm, and RefreshList properties
        /// </summary>
        /// <param name="message">Setter for Message property</param>
        /// <param name="refreshEditForm">Setter for RefreshEditForm property</param>
        /// <param name="refreshList">Setter for RefreshList property</param>
        public PositionResultViewModel(string message, bool refreshEditForm, bool refreshList)
        {
            this.Message = message;

            this.RefreshEditForm = refreshEditForm;

            this.RefreshList = refreshList;
        }

        /// <summary>
        /// PositionResultViewModel constructor which sets Message, RefreshEditForm, and RefreshList properties
        /// </summary>
        /// <param name="message">Setter for Message property</param>
        /// <param name="refreshEditForm">Setter for RefreshEditForm property</param>
        /// <param name="refreshList">Setter for RefreshList property</param>
        public PositionResultViewModel(string message, bool refreshEditForm, bool refreshList, int positionId)
        {
            this.Message = message;

            this.RefreshEditForm = refreshEditForm;

            this.RefreshList = refreshList;

            this.PositionId = positionId; 
        }
    }

    /// <summary>
    /// PositionListForReportViewModel view model declares properties for _PositionList partial view in Report controller
    /// </summary>
    public class PositionListForReportViewModel : PositionListViewModel
    {
        [Display(Name = "Report Type")]
        public string ReportType { get; set; }
    }

    /// <summary>
    /// PositionReportDetailsViewModel view model declares properties for _PositionFinalReport, and _PositionStatusReport partial views in Report controller
    /// </summary>
    public class PositionReportDetailsViewModel : PositionEditViewModel
    {
        [Display(Name = "Id")]
        public string PositionIdForDisplay { get; set; }

        [Display(Name = "Close Date")]
        public DateTime? CloseDate { get; set; }

        [Display(Name = "Total Applicants")]
        public int TotalCandidates { get; set; }

        [Display(Name = "Applications Qualified")]
        public int TopCandidates { get; set; }

        [Display(Name = "Selected for Interview")]
        public int CandidatesSelected { get; set; }

        [Display(Name = "Average Score")]
        public double AverageScore { get; set; }
    }


    /// <summary>
    /// PositionReportDetailsForPrintViewModel view model declares properties for position report views
    /// </summary>
    public class PositionReportDetailsForPrintViewModel
    {
        [Display(Name = "Id")]
        public string PositionIdForDisplay { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Close Date")]
        public DateTime? CloseDate { get; set; }

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

        [Display(Name = "Acceptance Score")]
        public int AcceptanceScore { get; set; }

        [Display(Name = "Total Applicants")]
        public int TotalCandidates { get; set; }

        [Display(Name = "Applications Qualified")]
        public int TopCandidates { get; set; }

        [Display(Name = "Selected for Interview")]
        public int CandidatesSelected { get; set; }

        [Display(Name = "Average Score")]
        public double AverageScore { get; set; }

        public PositionStatus Status { get; set; }

        public List<CandidateReportDetailsViewModel> Candidates { get; set; }

        /// <summary>
        /// Default PositionReportDetailsForPrintViewModel constructor         
        /// </summary>
        public PositionReportDetailsForPrintViewModel()
        {
            this.Candidates = new List<CandidateReportDetailsViewModel>();
        }
    }
}
