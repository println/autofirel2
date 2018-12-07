using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Support.Services.Delivery
{
    public enum PacketStatus { Waiting, InTransit, Abort, Finished }

    public delegate void DeliveryeventHandler(IMacro macro);

    public interface ITrack
    {
        event DeliveryeventHandler FinishedEvent;

        PacketStatus Status { get; }

        void Abort();
    }
}
