using System;
using System.Collections.Generic;

namespace IssueTracker
{
    public interface IProject
    {
        uint Id { get; }
        string Name { get; }
        ISupervisor Supervisor { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IIssue> Issues { get; }

        void AddUser(IUser person);

        void RemoveUser(IUser person);

        void AssignSupervisor(ISupervisor supervisor);
    }
}