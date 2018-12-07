using System.Collections.ObjectModel;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Core.UI.ViewModels.ViewModel
{
    public interface IProfileViewModel<T, A> : IDataModel<IProfile>
        where T : IMacroViewModel<A>
        where A : IActionViewModel
    {
        string Id { get; }
        string Name { get; set; }
        string Description { get; set; }

        bool IsGlobal { get; set; }
        ObservableCollection<T> Macros { get; set; }
    }
}
