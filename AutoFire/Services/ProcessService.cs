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


        public virtual Process GetProcess(uint pid)
        {
            return Process.GetProcesses()
                .First(p => p.Id == pid);
        }
    }
}
