using System.Collections.ObjectModel;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Core.UI.ViewModels.ViewModel
{
    public interface IMacroViewModel<T> : IRunnable, IDataModel<IMacro> where T : IActionViewModel
    {
        string Name { get; set; }
        string Hotkey { get; set; }
        bool IsRepeat { get; set; }
        ObservableCollection<T> Actions { get; set; }
    }
}
