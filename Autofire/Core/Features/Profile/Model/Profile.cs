﻿using System.Collections.Generic;

namespace Autofire.Core.Features.Profile.Model
{
    public class Profile: IProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CoverageMode Coverage { get; set; } = CoverageMode.Focus;
        public IList<IMacro> Macros { get; set; }

        protected bool Equals(Profile other)
        {
            return string.Equals(Id, other.Id) && string.Equals(Name, other.Name) && string.Equals(Description, other.Description) && Coverage == other.Coverage && Equals(Macros, other.Macros);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Profile) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Coverage;
                hashCode = (hashCode * 397) ^ (Macros != null ? Macros.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
