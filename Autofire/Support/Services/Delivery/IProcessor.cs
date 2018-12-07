using System;
using System.Collections.Generic;

namespace Autofire.Support.Services.Delivery
{
    internal interface IProcessor
    {
        ITrack SimpleDelivery(IntPtr handle, IEnumerable<KeyInterval> kis);
        ITrack ContinuousDelivery(IntPtr handle, IEnumerable<KeyInterval> kis);
    }
}
