namespace AutoFire.Services.Window
{
    public class WindowHandler
    {
        public System.Diagnostics.Process Process { get; private set; }

        public WindowHandler(System.Diagnostics.Process process)
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
