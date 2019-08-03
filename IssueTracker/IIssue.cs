using System;

namespace IssueTracker
{
    public interface IIssue
    {
        uint Id { get; }
        IIssueDetails Details { get; }
        IUser Reporter { get; }
        IUser AssignedTo { get; set; }
        DateTime DueBy { get; }
        IssueStatus Status { get; set; }
        Severity Severity { get; }
    }
}