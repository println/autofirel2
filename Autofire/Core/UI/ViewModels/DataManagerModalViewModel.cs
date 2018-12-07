using System.ComponentModel.DataAnnotations;
using Autofire.Support.Utils.ViewModel;

namespace Autofire.Core.UI.ViewModels
{
    public class DataManagerModalViewModel : AbstractModalViewModel<DataManagerModalViewModel>
    {
        public string Title { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [RegularExpression(@"^[\w\- ]+$", ErrorMessage = "Invalid character")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [RegularExpression(@"^[\w\- ]+$", ErrorMessage = "Invalid character")]
        public string Description { get; set; }

        public override DataManagerModalViewModel GetContent()
        {
            return this;
        }
    }
}
