using AutoFire.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoFire.App
{
    public class Service
    {

        public ProcessService ProcessService { get; set; }

        public IList<int> GetProcesses() { return ProcessService.GetProcessesByName(""); }


        public void GetProcess(uint id) { throw new NotImplementedException(); }

        public IList<string> GetProfiles() { throw new NotImplementedException(); }
        public void GetProfile(uint id) { throw new NotImplementedException(); }

        internal void GetWindows() { throw new NotImplementedException(); }
        internal void GetWindowName(uint processId) { throw new NotImplementedException(); }        

        public void SetActiveProfile(string profileName) { throw new NotImplementedException(); }
    }
}
