using AutoFire.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoFire.GUI
{
    class GUIManager
    {
        private Service Service { get; set; }

        public GUIManager() {
            Service.GetProfiles();
            Service.GetProfile(10);
            Service.GetWindows();
            Service.GetWindowName((uint)10);
        }
    }
}
