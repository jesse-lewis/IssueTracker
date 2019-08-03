using System;
using System.Collections.Generic;

namespace IssueTracker
{
    internal class User : IUser, IEquatable<User>
    {
        private readonly uint _id;

        public User(uint id, string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            _id = id;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User other)
        {
            return other != null && _id == other._id;
        }

        public override int GetHashCode()
        {
            return 1969571243 + _id.GetHashCode();
        }

        public static bool operator ==(User left, User right)
        {
            return EqualityComparer<User>.Default.Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !(left == right);
        }
    }
}