using System;

namespace Autofire.Support.Utils.ViewModel.Reactive
{
    public abstract class AbstractMessage
    {
        public string PropertyName { get; set; }
        public Type Type { get; set; }
    }
}