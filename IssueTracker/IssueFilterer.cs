using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssueTracker
{
    internal class IssueFilterer
    {
        public IEnumerable<IBug> FindBugs(
            IEnumerable<IIssue> issues,
            string description = null,
            DateTime? dueBy = null,
            Severity? severity = null,
            IssueStatus? issueStatus = null)
        {
            return FilterBugsByDescription(GetBugs(FilterIssuesOnDueBy(FilterIssuesOnSeverity(FilterIssuesOnIssueStatus(issues, issueStatus), severity), dueBy)), description);
        }

        private static IEnumerable<IBug> FilterBugsByDescription(IEnumerable<IBug> bugs, string description)
        {
            return description == null ? bugs : bugs.Where(b => b.Description.Contains(description));
        }

        private static IEnumerable<IBug> GetBugs(IEnumerable<IIssue> issues)
        {
            return issues.Select(i => i.Details).OfType<IBug>();
        }

        private static IEnumerable<IIssue> FilterIssuesOnDueBy(IEnumerable<IIssue> issues, DateTime? dueBy)
        {
            return dueBy == null ? issues : issues.Where(i => i.DueBy < dueBy);
        }

        private static IEnumerable<IIssue> FilterIssuesOnSeverity(IEnumerable<IIssue> issues, Severity? severity)
        {
            return severity == null ? issues : issues.Where(i => i.Severity == severity);
        }

        private static IEnumerable<IIssue> FilterIssuesOnIssueStatus(IEnumerable<IIssue> issues, IssueStatus? issueStatus)
        {
            return issueStatus == null ? issues : issues.Where(i => i.Status == issueStatus);
        }
    }
}
