using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace AutoFire
{
    class WindowSeeker
    {
        public delegate void WindowEventHandler(string[] names, int[] ids);
        public event WindowEventHandler WindowEvent;

        public delegate void ActivityWinEventHandler(bool Act);
        public event ActivityWinEventHandler ActivityWinEvent;

        public delegate void WindowClosedEventHandler(bool critical, string info);
        public event WindowClosedEventHandler WindowClosedEvent;

        private Process[] ProcessesList;
        private Thread Finder;
        private string ProcessName;
        private static Process SelectedProcess = null;
        private bool canRun = true;

        public string Name
        {
            get { return ProcessName; }
            set { ProcessName = value; }
        }

        public void Start()
        {
            if (ProcessName != null)
            {
                Finder = new Thread(Listener);
                Finder.Name = "Finder";
                Finder.Start();
            }
        }

        public int Set(int index)
        {
            Process newProcess = ProcessesList[index];

            if (newProcess.StartInfo.UseShellExecute)
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                canRun = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if (canRun)
            {
                if (SelectedProcess == null || newProcess.MainWindowHandle != SelectedProcess.MainWindowHandle)
                {
                    if (SelectedProcess != null)
                        SelectedProcess.Exited -= new EventHandler(ExitCallback);

                    if (SelectedProcess != null && newProcess.Id != SelectedProcess.Id)
                        ShowWindow(newProcess.MainWindowHandle);

                    SelectedProcess = newProcess;
                    SelectedProcess.EnableRaisingEvents = true;
                    SelectedProcess.Exited += new EventHandler(ExitCallback);
                }


                return SelectedProcess.Id;
            }

            WindowClosedEvent(true, "Run as Administrator!");

            return -1;
        }

        public bool Destroy()
        {
            Finder.Abort();
            return !Finder.IsAlive;
        }

        //Method for separated thread
        private void Listener()
        {
            while (Finder.IsAlive && canRun)
            {
                FindProcess();
                Thread.Sleep(5000);
            }
        }

        //Checks new and old search results if contains same data
        private bool Compare(Process[] newProcesses, Process[] oldProcesses)
        {
            if (newProcesses != null && oldProcesses != null && newProcesses.Length == oldProcesses.Length)
            {
                bool check = true;

                for (int i = 0; i < newProcesses.Length; i++)
                    check = (newProcesses[i].MainWindowHandle != oldProcesses[i].MainWindowHandle) ? false : check;

                return check;
            }

            return false;
        }

        //Seeks processes
        private void FindProcess()
        {
            ActivityWinEvent(true);

            Process[] newProcesses;
            System.Collections.Generic.List<Process> tmpProcess = new System.Collections.Generic.List<Process>();
            
            
            foreach (Process process in Process.GetProcesses())
                if (process.MainWindowTitle.ToLower() == ProcessName.ToLower())
                    tmpProcess.Add(process);

            newProcesses = tmpProcess.ToArray();

            if (!Compare(newProcesses, ProcessesList))
            {
                ProcessesList = newProcesses;
                UpdateCallback();
            }

            ActivityWinEvent(false);
        }

        //callback to update processes list
        private void UpdateCallback()
        {
            int size = ProcessesList.Length;

            string[] names = new string[size];
            int[] ids = new int[size];

            for (int i = 0; i < size; i++)
            {
                Process p = ProcessesList[i];
                names[i] = p.MainWindowTitle;
                ids[i] = p.Id;
            }

            WindowEvent(names, ids);
        }

        //callback to close selected process
        private void ExitCallback(object sender, EventArgs e)
        {
            Destroy();
            Start();
            WindowClosedEvent(false, "The window was closed!");
        }

        public static Process GetProcess()//Static Method
        {
            return SelectedProcess;
        }

        #region SetForegroundWindow

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);//Restores window
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);//Sends window to foreground

        private void ShowWindow(IntPtr window)
        {
            const int SW_RESTORE = 9;
            ShowWindow(window, SW_RESTORE);
            SetForegroundWindow(window);
        }

        #endregion
    }
}
