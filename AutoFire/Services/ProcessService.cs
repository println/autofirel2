using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AutoFire.Services
{
    public class ProcessService
    {
        public virtual IList<int> GetProcessesByName(string name)
        {
            return Process.GetProcesses()
                .Where(p => p.MainWindowTitle.ToUpper().Contains(name.ToUpper()))
                .Select(p=> p.Id)
                .ToList();
        }


        public virtual Process GetProcessByPid(int pid)
        {
            return Process.GetProcesses()
                .FirstOrDefault(p => p.Id == pid);
        }

        //http://stackoverflow.com/questions/967646/monitor-when-an-exe-is-launched
    }
}
