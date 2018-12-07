using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Support.Services.Delivery
{

    public class DeliveryService
    {
        private IProcessor _processor ;

        public ITrack Deliver(Process process, IMacro macro)
        {
            throw new NotImplementedException();
        }

        public ITrack ContinuousDelivery(IntPtr handle, IEnumerable<KeyInterval> kis)
        {
            throw new NotImplementedException();
        }
    }
}
