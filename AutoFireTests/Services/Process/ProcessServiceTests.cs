using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoFireTests.Utils;
using System.Threading;
using System.Diagnostics;
using AutoFire.Services.Process;

namespace AutoFire.Services.Process.Tests
{
    [TestClass()]
    public class ProcessServiceTests
    {
        [TestMethod()]
        public void GetProcessesByNameTest()
        {
            using (Notepad notepad = new Notepad())
            using (TempFile tempFile = new TempFile("xpto_test.txt"))
            {
                notepad.OpenFile(tempFile.FilePath);
                Thread.Sleep(1000);
                var result = new ProcessService().GetProcessesByName("xpto_test.txt").Count;

                Assert.AreEqual(1, result);
            }
        }

        [TestMethod()]
        public void GetProcessByPidTest()
        {
            using (Notepad notepad = new Notepad())
            using (TempFile tempFile = new TempFile("xpto_test_2.txt"))
            {
                notepad.OpenFile(tempFile.FilePath);
                Thread.Sleep(1000);
                var pid = new ProcessService().GetProcessesByName("xpto_test_2.txt")[0];
                System.Diagnostics.Process result = new ProcessService().GetProcessByPid(pid);

                Assert.IsNotNull(result);
            }
        }
    }
}