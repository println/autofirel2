using System;
using Autofire.Core.Features;
using Autofire.Core.Features.Game;
using Autofire.Core.Features.Profile;
using Caliburn.Micro;

namespace Autofire.Core
{
    public class Core : IDisposable
    {
        public IProfileManager Data { get; }
        public IGameManager Game { get; }
        public IControlPanelService Control { get; }

        public Core(IEventAggregator eventAggregator)
        {
            Data = new ProfileManager();
            Game = new GameManager();
        }

        public void Start()
        {
           
        }

        public void Stop() { throw new NotImplementedException(); }

        public void Dispose()
        {
            Stop();
        }
    }
}
