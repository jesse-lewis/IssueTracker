using System;
using System.Collections.Generic;

namespace IssueTracker
{
    internal class Issue : IIssue, IEquatable<Issue>
    {
        public Issue(uint id, IBug bug, IUser reporter, IUser assignedTo, DateTime dueBy, IssueStatus status, Severity severity)
        {
            Id = id;
            Bug = bug;
            Reporter = reporter;
            AssignedTo = assignedTo;
            DueBy = dueBy;
            Status = status;
            Severity = severity;
        }

        public uint Id { get; }

        public IBug Bug { get; }

        public IUser Reporter { get; }

        public IUser AssignedTo { get; set; }

        public DateTime DueBy { get; }

        public IssueStatus Status { get; set; }

        public Severity Severity { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Issue);
        }

        public bool Equals(Issue other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public static bool operator ==(Issue left, Issue right)
        {
            return EqualityComparer<Issue>.Default.Equals(left, right);
        }

        public static bool operator !=(Issue left, Issue right)
        {
            return !(left == right);
        }
    }
}