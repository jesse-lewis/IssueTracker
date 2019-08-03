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

        private static void ReportFakeBugs(Project project, int number)
        {
            for (int i = 0; i < number; i++)
            {
                ReportFakeBug(project);
            }
        }

        private static void ReportFakeBug(Project project)
        {
            project.ReportBug(CreateFakeBug("description"), new FakeUser(), DateTime.Now);
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