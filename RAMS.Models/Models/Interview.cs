using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Models
{
    
    /// <summary>
    /// Interview class represents interview between candidate and employee on certain date
    /// </summary>
    public class Interview : BaseEntity
    {
        public int InterviewId { get; set; }

        public int CandidateId { get; set; }

        public int InterviewerId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual Agent Interviewer { get; set; }

        public DateTime InterviewDate { get; set; }

        public InterviewStatus Status { get; set; }
    }
}