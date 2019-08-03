using IssueTracker;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests
{
    public class ProjectTests
    {
        private TestLogger _logger = new TestLogger();

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void OnCreation_IfProjectNameIsNullEmptyOrWhiteSpace_ThrowsArgumentException(string projectName)
        {
            Assert.Throws<ArgumentException>(() => CreateProject(projectName));
        }

        [Test]
        public void ReportBug_IfBugIsNull_LogsIssueAndDoesNotAddIssue()
        {
            var project = CreateProject();

            project.ReportBug(null, new FakeUser(), DateTime.Now, IssueStatus.Open, Severity.Minor);

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

        private static void ReportFakeBug(Project project)
        {
            project.ReportBug(new FakeBug(), new FakeUser(), DateTime.Now);
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