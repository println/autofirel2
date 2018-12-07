namespace Autofire.Core.UI.ViewModels.ViewModel
{
    public interface IRunnable
    {
        bool IsRunning { get; set; }
        bool CanRun { get; set; }
    }
}
