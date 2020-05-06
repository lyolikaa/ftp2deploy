using FtpToDeploymentFile;
using NUnit.Framework;
using System.Resources;

namespace NUnitTestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(@"C:\Publish/\/\\Web\Site2\badpath987.html", false)]
        [TestCase(@"C:\Publish\Common\base.dat", true)]
        public void CheckFilePathValidation(string path, bool result)
        {
            Assert.AreEqual(Validation.ValidatePath(path), result);
        }
        [Test]
        public void CheckValidateSource()
        {
            //Validation.ValidateSource
        }
        [Test]
        [TestCase("", "Common")]
        [TestCase("WebServer", "WebServer")]
        public void CheckValidateServerName(string input, string result)
        {
            Assert.AreEqual(Validation.ValidateServerName(input), result);
        }
        [Test]
        public void CheckGetTargetFiles()
        {
            //BL.GetTargetFiles
        }
    }
}