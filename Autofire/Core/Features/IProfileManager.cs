using System.Collections.ObjectModel;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Core.Features
{
    
    public delegate void DataChangedEventHandler(params DataChangedType[] types);
    public interface IProfileManager
    {
        event DataChangedEventHandler DataChangedEvent;
        
        int SelectedIndex { get; set; }
        
        ObservableCollection<string> ProfileIdList { get; }
        
        IProfile ActiveProfile { get; }
        
        void CreateProfile(string name, string description);
        
        void RenameActiveProfile(string name, string description);

        void SaveActiveProfile();
        
        void DeleteActiveProfile();
        
        void Refresh();
    }

    public enum DataChangedType
    {
        INDEX, LIST, PROFILE
    }
}