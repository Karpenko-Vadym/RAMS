using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Models
{
    /// <summary>
    /// PositionArchive represent archived job posting
    /// </summary>
    public class PositionArchive : BaseEntity
    {
        public int PositionArchiveId { get; set; }

        public int PositionId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CompanyDetails { get; set; } // Company Description

        public string Location { get; set; }

        public string Qualifications { get; set; } // Skills & Qualifications

        public string AssetSkills { get; set; } // Skills that are an asset

        public string CategoryName { get; set; } // Position category

        public string ClientName { get; set; } // Who created this position

        public string AgentName { get; set; } // To whom this position is assigned

        public int PeopleNeeded { get; set; } // How many people needed for this position

        public int AcceptanceScore { get; set; } // Score cut off point, everything below will not qualify for the position

        public int TotalApplicants { get; set; }

        public int InterviewedApplicants { get; set; }

        public PositionStatus Status { get; set; }
    }
}