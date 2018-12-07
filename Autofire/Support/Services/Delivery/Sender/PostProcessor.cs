using System;
using System.Collections.Generic;

namespace  Autofire.UI.Components.Delivery.Sender
{
    internal class PostProcessor : IProcessor
    {
        private KeySender sender = new KeySender();

        public ITrack ContinuousDelivery(IntPtr handle, IEnumerable<KeyInterval> kis)
        {
            throw new NotImplementedException();
        }

        public ITrack SimpleDelivery(IntPtr handle, IEnumerable<KeyInterval> kis)
        {
            throw new NotImplementedException();
        }
    }
}
