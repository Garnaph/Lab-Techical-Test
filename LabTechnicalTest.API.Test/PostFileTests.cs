using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabTechnicalTest.API.Logic;

namespace LabTechnicalTest.API.Test
{
    [TestClass]
    public class PostFileTests
    {
        [TestMethod]
        public void BlankFileTest()
        {
            string inputData = "";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(-1));
        }

        [TestMethod]
        public void NormalFileTest1()
        {
            string inputData = "1,2,3,4,5,5,5,5,5,5,5,5,6,7,8";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(5));
        }
    }
}
