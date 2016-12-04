using System.Collections.Generic;
using System.Linq;

namespace AutoFire.Services.Process
{
    public class ProcessService
    {
        public virtual IList<int> GetProcessesByName(string name)
        {
            return System.Diagnostics.Process.GetProcesses()
                .Where(p => p.MainWindowTitle.ToUpper().Contains(name.ToUpper()))
                .Select(p=> p.Id)
                .ToList();
        }


        public virtual System.Diagnostics.Process GetProcessByPid(int pid)
        {
            return System.Diagnostics.Process.GetProcesses()
                .FirstOrDefault(p => p.Id == pid);
        }

        //http://stackoverflow.com/questions/967646/monitor-when-an-exe-is-launched
    }
}
