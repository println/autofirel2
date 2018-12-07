using System;

namespace Autofire.Support.Daemons
{
    public interface IDaemon: IDisposable
    {
        void Start();
        
        void Stop();
        
    }
}