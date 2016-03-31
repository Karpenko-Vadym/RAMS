using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Models
{
    /// <summary>
    /// CandidateArchive represents an archive for an individual who applied for one or more positions (Job seeker)
    /// </summary>
    public class CandidateArchive : BaseEntity
    {
        public int CandidateArchiveId { get; set; }

        public int PositionId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string Feedback { get; set; }

        public string FileName { get; set; } // Name of the resume (Might not be needed)

        public string MediaType { get; set; } // File media type

        public byte[] FileContent { get; set; } // File content

        public int Score { get; set; }

        public CandidateStatus Status { get; set; }
    }
}