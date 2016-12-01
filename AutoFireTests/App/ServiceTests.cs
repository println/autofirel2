using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AutoFire.Services;
using Rhino.Mocks;

namespace AutoFire.App.Tests
{



    [TestClass()]
    public class ServiceTests
    {
        Service _service;


        [TestInitialize()]
        public void Startup()
        {
            MockRepository mocks = new MockRepository();
            ProcessService service = (ProcessService)mocks.StrictMock<ProcessService>();
            Expect.Call(service.GetProcessesByName("")).Return(new List<int>() {1, 2});
            mocks.ReplayAll();
            
            this._service = new Service() { ProcessService = service};
        }

        [TestMethod()]
        public void GetProcessesTest()
        {
            Assert.AreEqual(2, _service.GetProcesses().Count);  
        }
    }
}