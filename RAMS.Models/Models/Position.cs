using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Models
{
    /// <summary>
    /// Position represents job posting that will be interacted with by every actor
    /// </summary>
    public class Position : BaseEntity
    {
        public int PositionId { get; set; }

        public int CleintId { get; set; }

        public int? AgentId { get; set; }

        public int CategoryId { get; set; }

        public int DepartmentId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CompanyDetails { get; set; } // Company Description

        public string Location { get; set; }

        public string Qualifications { get; set; } // Skills & Qualifications

        public string AssetSkills { get; set; } // Skills that are an asset

        public virtual Department Department { get; set; } // Department to which position is assigned

        public virtual Category Category { get; set; } // Position category

        public virtual Client Client { get; set; } // Who created this position

        public virtual Agent Agent { get; set; } // To whom this position is assigned

        public virtual List<Candidate> Candidates { get; set; } // Applicants who applied for this position

        public int PeopleNeeded { get; set; } // How many people needed for this position

        public int AcceptanceScore { get; set; } // Score cut off point, everything below will not qualify for the position

        public PositionStatus Status { get; set; }

        public Position()
        {
            this.Candidates = new List<Candidate>();
        }
    }
}