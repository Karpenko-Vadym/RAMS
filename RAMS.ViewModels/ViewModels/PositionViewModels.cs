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
}
