using Autofire.Support.Utils.ViewModel;

namespace Autofire.Core.UI.ViewModels
{
    public class DataManagerDeleteModalViewModel : AbstractModalViewModel<DataManagerDeleteModalViewModel>
    {
        public string Id { get; set; }

        public override DataManagerDeleteModalViewModel GetContent()
        {
            return this;
        }
    }
}
