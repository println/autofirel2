using System;

namespace Autofire.Core.Features.Control
{
    public class ControlPanel : IControlPanelService
    {
        public bool IsRunning { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CanRun { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
