using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Model;
using Autofire.Support.Repositories.Profile;
using NUnit.Framework;

namespace Autofire.Tests.Support.Repositories.Profile
{
    [TestFixture()]
    public class ProfileRepositoryTests
    {
        private ProfileRepository Repository { get; set; }
        protected string _filename = "test-01.profile.json";
        protected string FilePath { get; set; }
        protected string CurrentFolder { get; set; }

        public ProfileRepositoryTests()
        {
            this.Repository = new ProfileRepository();
        }

        [SetUp]
        public void CreateFile()
        {
            CurrentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FilePath = Path.Combine(CurrentFolder, _filename);
            File.WriteAllText(FilePath, JsonContent());
        }

        [TearDown]
        public void ClearFiles()
        {
            Directory.GetFiles(CurrentFolder, "test*.*", SearchOption.TopDirectoryOnly)
               .ToList()
               .ForEach(File.Delete);
        }

        [Test()]
        public void ListTest()
        {
            var list = Repository.ListAll();
            CollectionAssert.IsNotEmpty(list);
            Assert.AreEqual("test-01", list.First());
        }

        [Test()]
        public void GetAllTest()
        {
            var list = Repository.GetAll();
            CollectionAssert.IsNotEmpty(list);
            Assert.AreEqual("test-01", list.First().Id);
            Assert.AreEqual(CoverageMode.Focus, list.First().Coverage);
        }

        [Test()]
        public void GetOneTest()
        {
            var profile = Repository.GetById("test-01");
            Assert.AreEqual("test-01", profile.Id);
            Assert.AreEqual(CoverageMode.Focus, profile.Coverage);
        }

        [Test()]
        public void CreateTest()
        {
            ClearFiles();
            Assert.DoesNotThrow(() => Repository.Create(MakeProfile()));
            Assert.NotNull(GetContent());
        }

        [Test()]
        public void UpdateTest()
        {
            var profile = MakeProfile();
            profile.Name = "test2";
            profile.Coverage = CoverageMode.Any;

            Assert.DoesNotThrow(() => Repository.Update("test-01", profile));
            Assert.Throws<FileNotFoundException>(() => Repository.GetById("test-01"));

            profile = Repository.GetById("test2-01");
            Assert.AreEqual("test2-01", profile.Id);
            Assert.AreEqual(CoverageMode.Any, profile.Coverage);
        }

        [Test()]
        public void RemoveTest()
        {
            Assert.DoesNotThrow(() => Repository.Remove("test-01"));
            Assert.IsFalse(HasFile());
        }

        [Test()]
        public void LegacyTest()
        {
            ClearFiles();
            string xmlFile = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\TestFiles\prototype.config");
            string newFile = Path.Combine(CurrentFolder, @"test.config");
            File.Copy(xmlFile, newFile);
            Assert.DoesNotThrow(() => Repository.LegacyToJson());

            var profile = Repository.GetById("test-legacy");
            Assert.AreEqual("test-legacy", profile.Id);
            Assert.AreEqual(CoverageMode.Focus, profile.Coverage);
            CollectionAssert.IsNotEmpty(profile.Macros);
            Assert.AreEqual(0.14m, profile.Macros.First().Actions.First().Interval);

            profile.Macros.ToList()
                .ForEach(m => Assert.AreEqual(4, m.Actions.Count()));

        }

        private bool HasFile()
        {
            return File.Exists(FilePath);
        }

        private string GetContent()
        {
            string content = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(content))
            {
                throw new Exception("No has content");
            }
            return content;
        }

        private static IProfile MakeProfile()
        {
            return new Autofire.Core.Features.Profile.Model.Profile() { Name = "test", Description = "01", Coverage = CoverageMode.Focus };
        }

        private string JsonContent()
        {
            return "{\"Id\": \"test-01\",\"Name\": \"test\",\"Description\": \"01\",\"Coverage\": \"Focus\", \"Macros\":[]}";
        }
    }
}