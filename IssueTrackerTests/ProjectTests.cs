using IssueTracker;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;

namespace Tests
{
    public class ProjectTests
    {
        private readonly TestLogger _logger = new TestLogger();
        private static readonly object[] _failingBugs = { null, CreateFakeBug(null), CreateFakeBug(""), CreateFakeBug(" ") };

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void OnCreation_IfProjectNameIsNullEmptyOrWhiteSpace_ThrowsArgumentException(string projectName)
        {
            Assert.Throws<ArgumentException>(() => CreateProject(projectName));
        }

        [Test]
        [TestCase(null)]
        [TestCaseSource(nameof(_failingBugs))]
        public void ReportBug_IfBugOrBugDescriptionIsNull_LogsIssueAndDoesNotAddIssue(IBug bug)
        {
            var project = CreateProject();

            project.ReportBug(bug, new FakeUser(), DateTime.Now, IssueStatus.Open, Severity.Minor);

            Assert.That(!string.IsNullOrWhiteSpace(_logger.LastLog));
            Assert.That(!project.Issues.Any());
        }

        [Test]
        public void ReportBug_IfNoIssueStatusIsSpecified_SetsToOpen()
        {
            var project = CreateProject();

            ReportFakeBug(project);

            Assert.That(GetLastIssue(project).Status, Is.EqualTo(IssueStatus.Open));
        }

        [Test]
        public void ReportBug_IfNoSeverityIsSpecified_SetsToUndefined()
        {
            var project = CreateProject();

            ReportFakeBug(project);

            Assert.That(GetLastIssue(project).Severity, Is.EqualTo(Severity.Undefined));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void FindBugs_WithNoParameters_ReturnsAllBugs(int bugs)
        {
            var project = CreateProject();

            ReportFakeBugs(project, bugs);

            Assert.That(project.FindBugs().Count(), Is.EqualTo(bugs));
        }

        [Test]
        [TestCase("description", 1)]
        [TestCase("description", 2)]
        public void FindBugs_WithDescriptionParameter_ReturnsAllBugsContainingPartOfTheDescription(string description, int bugs)
        {
            var project = CreateProject();

            ReportFakeBugs(project, bugs);

            Assert.That(project.FindBugs(description).Count(), Is.EqualTo(bugs));
        }

        [Test]
        [TestCase("1/1/2019","1/2/2019",1)]
        [TestCase("1/2/2019","1/1/2019",0)]
        public void FindBugs_WithDueByParameter_ReturnsAllBugsDueBeforeThatDate(string bugDueBy, string findDueBy, int expectedCount)
        {
            var project = CreateProject();

            var bugDueByDate = DateTime.Parse(bugDueBy);
            var findDueByDate = DateTime.Parse(findDueBy);

            ReportFakeBug(project, dueBy: bugDueByDate);

            Assert.That(project.FindBugs(dueBy: findDueByDate).Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        [TestCase(Severity.Undefined, Severity.Undefined, 1)]
        [TestCase(Severity.Minor, Severity.Undefined, 0)]
        [TestCase(Severity.Undefined, Severity.Minor, 0)]
        public void FindBugs_WithSeverityParameter_ReturnsAllBugsWithTheSpecifiedSeverity(Severity bugSeverity, Severity searchSeverity, int expectedCount)
        {
            var project = CreateProject();

            ReportFakeBug(project, severity: bugSeverity);

            Assert.That(project.FindBugs(severity: searchSeverity).Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        [TestCase(IssueStatus.Open,IssueStatus.Open,1)]
        [TestCase(IssueStatus.Closed,IssueStatus.Closed,1)]
        [TestCase(IssueStatus.Open,IssueStatus.Closed,0)]
        [TestCase(IssueStatus.Closed,IssueStatus.Open,0)]
        public void FindBugs_WithIssueStatusParameter_ReturnsAllBugsWithTheSpecifiedIssueStatus(IssueStatus bugStatus, IssueStatus searchStatus, int expectedCount)
        {
            var project = CreateProject();

            ReportFakeBug(project, issueStatus: bugStatus);

            Assert.That(project.FindBugs(issueStatus: searchStatus).Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        [TestCase(IssueStatus.Closed, IssueStatus.Closed, "description", "description", 1)]
        [TestCase(IssueStatus.Closed, IssueStatus.Open, "description", "description", 0)]
        [TestCase(IssueStatus.Closed, IssueStatus.Closed, "description", "test", 0)]
        [TestCase(IssueStatus.Closed, IssueStatus.Closed, "test", "description", 0)]
        public void FindBugs_WithIssueStatusAndDescriptionParameters_ReturnsAllBugsWithTheSpecifiedParamaters(IssueStatus bugStatus, IssueStatus searchStatus, string bugDescription, string searchDescription, int expectedCount)
        {
            var project = CreateProject();

            ReportFakeBug(project, issueStatus: bugStatus, description: bugDescription);

            Assert.That(project.FindBugs(description: searchDescription, issueStatus: searchStatus).Count(), Is.EqualTo(expectedCount));
        }

        private static void ReportFakeBugs(Project project, int number)
        {
            for (int i = 0; i < number; i++)
            {
                ReportFakeBug(project);
            }
        }

        private static void ReportFakeBug(Project project)
        {
            ReportFakeBug(project, dueBy: DateTime.Now);
        }

        private static void ReportFakeBug(Project project, string description = "description", DateTime? dueBy = null, IssueStatus issueStatus = IssueStatus.Open, Severity severity = Severity.Undefined)
        {
            project.ReportBug(CreateFakeBug(description), new FakeUser(), dueBy ?? DateTime.Now, issueStatus, severity);
        }


        private static FakeBug CreateFakeBug(string description)
        {
            return new FakeBug { Description = description };
        }

        private static IIssue GetLastIssue(Project project)
        {
            return project.Issues.Last();
        }

        private Project CreateProject()
        {
            return CreateProject("Test");
        }

        private Project CreateProject(string projectName, ISupervisor supervisor = null)
        {
            return new Project(1, projectName, supervisor ?? new FakeSupervisor(), _logger);
        }
    }
}