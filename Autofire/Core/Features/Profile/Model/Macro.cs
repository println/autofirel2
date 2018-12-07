using System.Collections.Generic;

namespace Autofire.Core.Features.Profile.Model
{
    public class Macro : IMacro
    {
        public string Name { get; set; }
        public string Hotkey { get; set; }
        public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.OneShot;
        public IList<IAction> Actions { get; set; }

        protected bool Equals(Macro other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Hotkey, other.Hotkey) &&
                   ExecutionMode == other.ExecutionMode && Equals(Actions, other.Actions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Macro) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Hotkey != null ? Hotkey.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) ExecutionMode;
                hashCode = (hashCode * 397) ^ (Actions != null ? Actions.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}