using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Autofire.Config;
using Autofire.Core.Features.Profile.Model;
using Autofire.Support.Repositories.Profile;

namespace Autofire.Core.Features.Profile
{
    public class ProfileManager :IProfileManager
    {
        private readonly IProfileRepository profileRepository;

        public event DataChangedEventHandler DataChangedEvent = delegate { };

        private int selectedIndex;

        public int SelectedIndex
        {
            get => this.selectedIndex;
            set
            {
                if (value != this.selectedIndex)
                {
                    this.LoadProfileByIndex(value);
                }
            }
        }

        public ObservableCollection<string> ProfileIdList { get; private set; }
        public IProfile ActiveProfile { get; private set; }

        public ProfileManager()
        {
            this.profileRepository = new ProfileRepository();
            this.LoadNextProfile();
        }

        public void CreateProfile(string name, string description)
        {
            var newProfile = AppConfig.ProfileBuilder.Build(name, description);
            this.CreateProfile(newProfile);
        }

        public void RenameActiveProfile(string name, string description)
        {
            var profile = this.ActiveProfile;
            profile.Name = name;
            profile.Description = description;
            this.UpdateProfile(profile);
        }

        public void SaveActiveProfile()
        {
            this.UpdateProfile(this.ActiveProfile);
        }

        public void DeleteActiveProfile()
        {
            this.profileRepository.Remove(this.ActiveProfile.Id);
            Trace.WriteLine($"Profile Deleted -> {this.ActiveProfile.Id}");
            this.LoadNextProfile();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        private void LoadNextProfile()
        {
            if (this.profileRepository.IsEmpty())
            {
                var newProfile = AppConfig.ProfileBuilder.Build();
                this.CreateProfile(newProfile);
            }
            else
            {
                this.LoadFirstProfile();
            }
        }

        private void LoadFirstProfile()
        {
            var profile = profileRepository.GetAll().First();
            Trace.WriteLine($"Profile Loaded -> {profile.Id}");
            this.SetActiveProfile(profile);
        }

        private void LoadProfileByIndex(int index)
        {
            var profile = profileRepository.GetById(this.ProfileIdList[index]);
            Trace.WriteLine($"Profile Loaded -> {profile.Id}");
            this.SetActiveProfile(profile);
        }

        private void CreateProfile(IProfile newProfile)
        {
            var createdProfile = profileRepository.Create(newProfile);
            Trace.WriteLine($"Profile Created -> {createdProfile.Id}");
            this.SetActiveProfile(createdProfile);
        }

        private void UpdateProfile(IProfile profile)
        {
            var updatedProfile = this.profileRepository.Update(profile.Id, profile);
            Trace.WriteLine($"Profile Updated -> {updatedProfile.Id}");

            if (updatedProfile != profile)
            {
                this.SetActiveProfile(updatedProfile);
            }
        }

        private void SetActiveProfile(IProfile profile)
        {
            this.ActiveProfile = profile;
            var types = this.UpdateProfileIdList(profile.Id);
            types.Add(DataChangedType.PROFILE);
            this.DataChangedEvent(types.ToArray());
        }

        private List<DataChangedType> UpdateProfileIdList(string profileId = null)
        {
            var changes = new List<DataChangedType>();

            if (this.ProfileIdList == null)
            {
                this.ProfileIdList = new ObservableCollection<string>(this.profileRepository.ListAll());
                this.selectedIndex = 0;
                changes.Add(DataChangedType.LIST);
                changes.Add(DataChangedType.INDEX);
                return changes;
            }


            var files = this.profileRepository.ListAll();
            if (!files.SequenceEqual(this.ProfileIdList))
            {
                this.ProfileIdList.Clear();
                this.profileRepository.ListAll().ToList().ForEach(p => this.ProfileIdList.Add(p));
                changes.Add(DataChangedType.LIST);
            }

            if (profileId == null)
            {
                return changes;
            }

            var index = this.ProfileIdList.IndexOf(profileId);
            if (this.selectedIndex != index)
            {
                this.selectedIndex = index;
                changes.Add(DataChangedType.INDEX);
            }

            return changes;
        }
    }
}