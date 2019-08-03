using IssueTracker;
using NUnit.Framework;
using System;

namespace Tests
{

    public class FakeBug : IBug
    {
        public string Description { get; set; }
    }
}