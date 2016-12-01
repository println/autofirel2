using System.Diagnostics;

namespace AutoFire.Services
{
    public class WindowHandler
    {
        public Process Process { get; private set; }

        public WindowHandler(Process process)
        {
            this.Process = process;
        }

        public void Focus()
        {

        }

        public void Blur()
        {

        }

        public void Close()
        {

        }

        // on close event register
    }
}
