using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Enums
{
    /// <summary>
    /// Enumeration for position status
    /// </summary>
    public enum PositionStatus
    {
        New = 1,
        Assigned = 2,
        Approved = 3,
        Suspended = 4,
        Closed = 5
    }

    /// <summary>
    /// Enumeration for candidate status
    /// </summary>
    public enum CandidateStatus
    {
        New = 1, // Before system assign score
        Selected = 2,
        NotSelected = 3,
        Pending = 4, // Waiting for inteview
        Interviewed = 5,
        Recommended = 6
    }

    /// <summary>
    /// Enumeration for user status
    /// </summary>
    public enum UserStatus
    {
        Active = 1,
        Blocked = 2,
        Deleted = 3
    }

    /// <summary>
    /// Enumeration for interview status
    /// </summary>
    public enum InterviewStatus
    {
        Scheduled = 1,
        Complete = 2
    }

    /// <summary>
    /// Enumeration for notification status
    /// </summary>
    public enum NotificationStatus
    {
        Unread = 1,
        Read = 2
    }

    /// <summary>
    /// Enumeration for report type
    /// </summary>
    public enum ReportType
    {
        Position = 1,
        Staff = 2
    }

    /// <summary>
    /// Enumeration for agent status
    /// </summary>
    public enum AgentStatus
    {
        PartTime = 1,
        FullTime = 2
    }

    /// <summary>
    /// Enumeration for role
    /// </summary>
    public enum Role
    {
        Employee = 1,
        Manager = 2
    }

    /// <summary>
    /// Enumeration for user type
    /// </summary>
    public enum UserType
    {
        Agent = 1,
        Client = 2,
        Admin = 3
    }
}