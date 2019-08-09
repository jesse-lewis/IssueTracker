using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueTracker
{
    public class Project : IProject
    {
        private readonly List<IIssue> _issues = new List<IIssue>();
        private readonly List<IUser> _users = new List<IUser>();
        private readonly ILogger _logger;

        public Project(uint id, string name, ISupervisor supervisor, ILogger logger, IEnumerable<IUser> users = null, IEnumerable<IIssue> issues = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Project Name cannot be blank");
            }

            Id = id;
            Name = name;
            Supervisor = supervisor ?? throw new ArgumentNullException(nameof(supervisor));
            _users.AddRange(users ?? new IUser[0]);
            _issues.AddRange(issues ?? new IIssue[0]);
            _logger = logger;
        }

        public uint Id { get; }

        public string Name { get; }

        public ISupervisor Supervisor { get; private set; }

        public IEnumerable<IUser> Users => _users;

        public IEnumerable<IIssue> Issues => _issues;

        public void ReportBug(IBug details, IUser reporter, DateTime dueBy, IssueStatus issueStatus = IssueStatus.Open, Severity severity = Severity.Undefined)
        {
            if (string.IsNullOrWhiteSpace(details?.Description))
            {
                _logger.Warning("Issue details missing from bug.");
                return;
            }
            AddIssue(new Issue(GetNextIssueId(), details, reporter, null, dueBy, issueStatus, severity));
        }

        private void AddIssue(IIssue issue)
        {
            _issues.Add(issue);
        }

        private uint GetNextIssueId()
        {
            return _issues.LastOrDefault()?.Id ?? 1;
        }

        public void AddUser(IUser user)
        {
            _users.Add(user);
        }

        public void AssignSupervisor(ISupervisor supervisor)
        {
            Supervisor = supervisor ?? throw new ArgumentNullException(nameof(supervisor));
        }

        public IEnumerable<IBug> FindBugs(string description = null,
                                          DateTime? dueBy = null,
                                          Severity? severity = null,
                                          IssueStatus? issueStatus = null)
        {
            return _issues
                .Where(i => issueStatus == null || i.Status == issueStatus)
                .Where(i => severity == null || i.Severity == severity)
                .Where(i => dueBy == null || i.DueBy < dueBy)
                .Select(i => i.Details).OfType<IBug>()
                .Where(b => description == null || b.Description.Contains(description));
        }

        public void RemoveUser(IUser user)
        {
            _users.Remove(user);
        }
    }
}