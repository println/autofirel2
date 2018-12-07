﻿namespace Autofire.Core.Features.Profile.Model
{
    public class Action:IAction
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public decimal Interval { get; set; }

        protected bool Equals(Action other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Key, other.Key) && Interval == other.Interval;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Action) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Interval.GetHashCode();
                return hashCode;
            }
        }
    }
}
