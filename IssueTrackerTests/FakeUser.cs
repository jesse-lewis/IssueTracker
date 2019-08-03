using IssueTracker;
using NUnit.Framework;
using System;

namespace Tests
{

    public class FakeUser : IUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}