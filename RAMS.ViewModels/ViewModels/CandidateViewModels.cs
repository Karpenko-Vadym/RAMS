using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace RAMS.ViewModels
{
    /// <summary>
    /// CandidateListViewModel view model declares properties for _PositionList partial view
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
    }


}
