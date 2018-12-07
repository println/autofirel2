using System.IO;
using System.Linq;
using System.Reflection;
using Autofire.Config;
using Autofire.Core.Features;
using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Builder;
using NUnit.Framework;

namespace Autofire.Tests.Core.Features.Profile
{
    [TestFixture()]
    public class ProfileManagerTests
    {
        private string currentFolder;
        private ProfileManager profileManager;
        private DataChangedType[] changes;


        [SetUp]
        public void Setup()
        {
            this.currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            AppConfig.ProfileBuilder = new ProfileBuilder()
                .SetProfileDetails("testAB", "testAB")
                .AppendMacro("MacroName1", "Ctrl + 1", new[] {"F1", "F2"});

            this.profileManager = new ProfileManager();
            this.profileManager.DataChangedEvent += (c) => this.changes = c;
        }

        [TearDown]
        public void ClearFiles()
        {
            this.changes = null;
            Directory.GetFiles(currentFolder, "*profile.json", SearchOption.TopDirectoryOnly)
                .ToList()
                .ForEach(File.Delete);
        }

        [TestFixture]
        public class PublicDataTests : ProfileManagerTests
        {
            [Test()]
            public void InitialValue_ShouldPass()
            {
                Assert.AreEqual(1, this.profileManager.ProfileIdList.Count());
                Assert.AreEqual(0, this.profileManager.SelectedIndex);
                Assert.NotNull(this.profileManager.ActiveProfile);
                Assert.AreEqual("testAB-testAB", this.profileManager.ActiveProfile.Id);
            }

            [Test()]
            public void NewProfile_ShouldPass()
            {
                this.profileManager.CreateProfile("testNewProfile", "testNewProfile");
                Assert.AreEqual(2, this.profileManager.ProfileIdList.Count());
                Assert.AreEqual(1, this.profileManager.SelectedIndex);
                Assert.AreEqual("testNewProfile-testNewProfile", this.profileManager.ActiveProfile.Id);
                Assert.AreEqual(
                    new DataChangedType[] {DataChangedType.LIST, DataChangedType.INDEX, DataChangedType.PROFILE},
                    this.changes);
            }

            [Test()]
            public void DeleteProfile_Unique_ShouldPass()
            {
                this.profileManager.DeleteActiveProfile();
                Assert.AreEqual(1, this.profileManager.ProfileIdList.Count());
                Assert.AreEqual(0, this.profileManager.SelectedIndex);
                Assert.AreEqual("testAB-testAB", this.profileManager.ActiveProfile.Id);
                Assert.AreEqual(new DataChangedType[] {DataChangedType.PROFILE}, this.changes);
            }

            [Test()]
            public void DeleteProfile_Multi_ShouldPass()
            {
                this.profileManager.CreateProfile("testNewProfile", "testNewProfile");
                this.profileManager.DeleteActiveProfile();
                Assert.AreEqual(1, this.profileManager.ProfileIdList.Count());
                Assert.AreEqual(0, this.profileManager.SelectedIndex);
                Assert.AreEqual("testAB-testAB", this.profileManager.ActiveProfile.Id);
                Assert.AreEqual(
                    new DataChangedType[] {DataChangedType.LIST, DataChangedType.INDEX, DataChangedType.PROFILE},
                    this.changes);
            }

            [Test()]
            public void RenameProfile_ShouldPass()
            {
                this.profileManager.RenameActiveProfile("testRenameProfile", "testRenameProfile");
                Assert.AreEqual(1, this.profileManager.ProfileIdList.Count());
                Assert.AreEqual(0, this.profileManager.SelectedIndex);
                Assert.AreEqual("testRenameProfile-testRenameProfile", this.profileManager.ActiveProfile.Id);
                Assert.AreEqual(new DataChangedType[] {DataChangedType.LIST, DataChangedType.PROFILE}, this.changes);
            }
            
            [Test()]
            public void SaveProfile_ShouldPass()
            {
                
                Assert.AreEqual("F1", this.profileManager.ActiveProfile.Macros.ElementAt(0).Actions.ElementAt(0).Key);
                this.profileManager.ActiveProfile.Macros[0].Actions[0].Key = "F12";
                this.profileManager.SaveActiveProfile();
                Assert.AreEqual("F12", this.profileManager.ActiveProfile.Macros[0].Actions[0].Key);
                Assert.AreEqual(null, this.changes);
            }
        }

        [TestFixture]
        public class SpecialCasesTests : ProfileManagerTests
        {
            [Test()]
            public void CreateProfile_Duplicated_ShouldPass()
            {
                this.profileManager.CreateProfile("testAB", "testAB");
                Assert.AreEqual("testAB-testAB2", this.profileManager.ActiveProfile.Id);
                this.profileManager.CreateProfile("testAB", "testAB");
                Assert.AreEqual("testAB-testAB3", this.profileManager.ActiveProfile.Id);
            }
            
            [Test()]
            public void RenameProfile_Duplicated_ShouldPass()
            {
                this.profileManager.CreateProfile("testNewProfile", "testNewProfile");
                this.changes = null;
                this.profileManager.RenameActiveProfile("testAB", "testAB");
                Assert.AreEqual("testNewProfile-testNewProfile", this.profileManager.ActiveProfile.Id);
                Assert.AreEqual(null, this.changes);
            }

            [Test()]
            public void SwitchProfile_ShouldPass()
            {
                this.profileManager.CreateProfile("testNewProfile", "testNewProfile");
                this.changes = null;
                this.profileManager.SelectedIndex = 0;
                Assert.AreEqual("testAB-testAB", this.profileManager.ActiveProfile.Id);
                Assert.AreEqual(new DataChangedType[] {DataChangedType.INDEX, DataChangedType.PROFILE}, this.changes);

                this.profileManager.SelectedIndex = 1;
                Assert.AreEqual("testNewProfile-testNewProfile", this.profileManager.ActiveProfile.Id);
                Assert.AreEqual(new DataChangedType[] {DataChangedType.INDEX, DataChangedType.PROFILE}, this.changes);
            }
        }
    }
}