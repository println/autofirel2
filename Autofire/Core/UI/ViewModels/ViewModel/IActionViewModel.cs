using Autofire.Core.Features.Profile.Model;

namespace Autofire.Core.UI.ViewModels.ViewModel
{
    public interface IActionViewModel : IRunnable, IDataModel<IAction>
    {
        string Name { get; set; }
        string Key { get; set; }
        decimal Interval { get; set; }
    }
}
