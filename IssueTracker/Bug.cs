namespace IssueTracker
{
    public class Bug : IBug
    {
        public Bug(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}