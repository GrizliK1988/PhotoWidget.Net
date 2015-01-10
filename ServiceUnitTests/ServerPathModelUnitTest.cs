using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoWidget.Models.Service;

namespace ServiceUnitTests
{
    [TestClass]
    public class ServerPathModelUnitTest
    {
        private readonly char _dirSlash = Path.DirectorySeparatorChar;

        [TestMethod]
        public void TestWebPath()
        {
            var serverPath = new FileSystemPath("", _dirSlash + "test" + _dirSlash + "test2" + _dirSlash);
            Assert.AreEqual("test/test2", serverPath.Web());
        }

        [TestMethod]
        public void TestCd()
        {
            var correctDelimiter = new Func<string, string>(x => x.Split('/').Aggregate((i, j) => i + _dirSlash + j));

            var serverPath = new FileSystemPath(correctDelimiter("/test/root/path"), correctDelimiter("test/relative"));

            serverPath.Cd("path");

            Assert.AreEqual(correctDelimiter("/test/root/path/test/relative/path"), serverPath.Absolute());
            Assert.AreEqual("test/relative/path", serverPath.Web());

            serverPath.CdToParent();

            Assert.AreEqual(correctDelimiter("/test/root/path/test/relative"), serverPath.Absolute());
            Assert.AreEqual("test/relative", serverPath.Web());

            serverPath.Cd("dir");
            Assert.AreEqual(correctDelimiter("/test/root/path/test/relative/dir"), serverPath.Absolute());

            serverPath.GoToFile("test.txt");
            Assert.AreEqual(correctDelimiter("/test/root/path/test/relative/dir/test.txt"), serverPath.Absolute());
        }
    }
}
