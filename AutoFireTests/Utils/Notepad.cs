using System;
using System.Diagnostics;

namespace AutoFireTests.Utils
{
    public class Notepad : IDisposable
    {
        public Process Process { get; private set; }

        public bool IsRunning { get { return Process != null; } }

        public void OpenFile(string filepath)
        {
            if (IsRunning)
            {
                return;
            }

            StartProcess(filepath);
        }


        private void StartProcess(string filepath)
        {
            ProcessStartInfo notepad = new ProcessStartInfo("notepad.exe", filepath)
            {
                WindowStyle = ProcessWindowStyle.Minimized
            };

            this.Process = Process.Start(notepad);
        }

        public void Close()
        {
            if (IsRunning)
            {
                Process.CloseMainWindow();
                Process.Close();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
