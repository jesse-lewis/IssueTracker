using IssueTracker;

namespace Tests
{
    public class FakeBug : IBug
    {
        public string Description { get; set; }
    }
}