using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RAMS.ViewModels
{
    /// <summary>
    /// CandidateListViewModel view model declares properties for _EditPosition partial view
    /// </summary>
    public class CandidateListViewModel
    {
        public int CandidateId { get; set; }

        [Display(Name = "Id")]
        public string CandidateIdDisplay { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Display(Name = "Score")]
        public int Score { get; set; }

        [Display(Name = "Status")]
        public CandidateStatus Status { get; set; }

        public bool Selected { get; set; }
    }

    /// <summary>
    /// CandidateEditViewModel view model declares properties for _EditCandidate partial view
    /// </summary>
    public class CandidateEditViewModel
    {
        [Required]
        public int CandidateId { get; set; }

        [Display(Name = "Id")]
        public string CandidateIdDisplay { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Feedback")]
        [StringLength(1000, ErrorMessage = "The {0} value cannot exceed 1000 characters.")]
        public string Feedback { get; set; }

        public string FileName { get; set; }

        public string MediaType { get; set; }

        public byte[] FileContent { get; set; }

        [Display(Name = "Score")]
        public int Score { get; set; }

        [Display(Name = "Status")]
        public CandidateStatus Status { get; set; }

        public string PositionStatus { get; set; }

        [Display(Name = "Is Interviewed?")]
        public bool IsInterviewed { get; set; }
    }

    /// <summary>
    /// CandidateConfirmationViewModel view model declares properties for _CandidateEditConfirmation partial view
    /// </summary>
    public class CandidateEditConfirmationViewModel
    {
        [Display(Name = "Id")]
        public string CandidateIdDisplay { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Feedback")]
        public string Feedback { get; set; }

        [Display(Name = "Score")]
        public int Score { get; set; }

        public int PositionId { get; set; }

        [Display(Name = "Status")]
        public CandidateStatus Status { get; set; }
    }

    /// <summary>
    /// CandidateReportDetailsViewModel view model declares properties for various types of reports
    /// </summary>
    public class CandidateReportDetailsViewModel
    {
        public int CandidateId { get; set; }

        [Display(Name = "Id")]
        public string CandidateIdDisplay { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Feedback")]
        public string Feedback { get; set; }

        public string FileName { get; set; }

        public string MediaType { get; set; }

        public byte[] FileContent { get; set; }

        [Display(Name = "Score")]
        public int Score { get; set; }

        [Display(Name = "Status")]
        public CandidateStatus Status { get; set; }

        public bool Selected { get; set; }
    }

    /// <summary>
    /// CandidateScheduleViewModel view model declares properties for scheduling partial views
    /// </summary>
    public class CandidateScheduleViewModel
    {
        public int CandidateId { get; set; }

        public string FullName { get; set; }
    }

    /// <summary>
    /// CandidateResumeDownloadViewModel view model declares properties for downloading resume
    /// </summary>
    public class CandidateResumeDownloadViewModel
    {
        public string FileName { get; set; }

        public string MediaType { get; set; }

        public byte[] FileContent { get; set; }
    }
}
