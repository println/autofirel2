using System;

namespace  Autofire.UI.Components.Delivery.Sender
{
    internal class PostPacket : ITrack
    {
        public PacketStatus Status { get; private set; }

        public event DeliveryeventHandler FinishedEvent = delegate { };

        private SyTask _syTask;

        public PostPacket(IntPtr handle, IMacro macro, KeySender sender )
        {
            _syTask = SyTask.Factory.NewCustomTask((wait) =>
            {
                var enumerator = macro.Actions.GetEnumerator();
                while (enumerator.MoveNext() && Status != PacketStatus.Abort)
                {
                    var action = enumerator.Current;

                    //sender.SendKey(handle, action.Key);
                    //wait(action.Interval);

                    if (!enumerator.MoveNext() && macro.ExecutionMode == ExecutionMode.Loop)
                    {
                        enumerator.Reset();
                    }
                }

                Status = PacketStatus.Finished;
                FinishedEvent(macro);
            });
        }

        public void Abort()
        {
            Status = PacketStatus.Abort;
        }

        internal void Start() {
            _syTask.Start();
            Status = PacketStatus.InTransit;
        }
    }
}
