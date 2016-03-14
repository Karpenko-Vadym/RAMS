using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.ViewModels
{
    /// <summary>
    /// InterviewListViewModels view model declares properties for scheduling partial views
    /// </summary>
    public class InterviewListViewModel
    {
        public int InterviewId { get; set; }

        public CandidateScheduleViewModel Candidate { get; set; }

        public DateTime InterviewDate { get; set; }

        public InterviewStatus Status { get; set; }
    }

    /// <summary>
    /// InterviewScheduleViewModel view model declares properties for scheduling partial views
    /// </summary>
    public class InterviewScheduleViewModel
    {
        public int CandidateId { get; set; }

        public string DisplayDate { get; set; }

        public bool Selected { get; set; }

        public string SelectedDateTime { get; set; }

        public List<InterviewListViewModel> Interviews { get; set; }

        /// <summary>
        /// Default InterviewScheduleViewModel constructor
        /// </summary>
        public InterviewScheduleViewModel()
        {
            this.Interviews = new List<InterviewListViewModel>();
        }

        /// <summary>
        /// InterviewScheduleViewModel constructor that sets all its properties
        /// </summary>
        /// <param name="displayDate">Setter for DisplayDate</param>
        public InterviewScheduleViewModel(string displayDate)
        {
            this.DisplayDate = displayDate;

            this.Interviews = new List<InterviewListViewModel>();
        }

        /// <summary>
        /// InterviewScheduleViewModel constructor that sets all its properties
        /// </summary>
        /// <param name="candidateId">Setter for CandidateId</param>
        /// <param name="displayDate">Setter for DisplayDate</param>
        /// /// <param name="selected">Setter for Selected</param>
        public InterviewScheduleViewModel(int candidateId, string displayDate, bool selected)
        {
            this.DisplayDate = displayDate;

            this.CandidateId = candidateId;

            this.Selected = selected;

            this.Interviews = new List<InterviewListViewModel>();
        }
    }
}
