using IssueTracker;
using NUnit.Framework;
using System;

namespace Tests
{

    public class FakeSupervisor : ISupervisor
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}