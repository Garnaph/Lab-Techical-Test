using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabTechnicalTest.API.Logic;
using System.Collections.Generic;

namespace LabTechnicalTest.API.Test
{
    [TestClass]
    public class PostFileTests
    {
        [TestMethod]
        public void BlankFileTest()
        {
            //0
            string inputData = "";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(-1));
        }

        [TestMethod]
        public void OddNumberedCollectionSizeTest()
        {
            //15
            string inputData = "1,2,3,4,5,5,5,5,5,5,5,5,6,7,8";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(5));
        }

        [TestMethod]
        public void EvenNumberedCollectionSizeTest()
        {
            //16
            string inputData = "1,2,2,3,4,5,5,5,5,5,5,5,5,5,7,8";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(5));
        }

        [TestMethod]
        public void SmallCollectionSizeTest()
        {
            //3
            string inputData = "1,2,2";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(2));
        }

        [TestMethod]
        public void OneItemTest()
        {
            //1
            string inputData = "2";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(2));
        }

        [TestMethod]
        public void NoWinnerTest()
        {
            //10
            string inputData = "1,2,2,2,5,6,7,9,9,10";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(-1));
        }

        [TestMethod]
        public void ExampleFromSpecification1Test()
        {
            //10
            string inputData = "2, 2, 2, 2, 2, 3, 4, 4, 4, 6";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(-1));
        }

        [TestMethod]
        public void ExampleFromSpecification2Test()
        {
            //5
            string inputData = "1, 1, 1, 1, 50";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(1));
        }

        [TestMethod]
        public void NegativeNumberTest()
        {
            //5
            string inputData = "-13, 1, 1, 1, 50";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(-1));
        }

        [TestMethod]
        public void NumberTooLargeTest()
        {
            //5
            string inputData = "1, 1, 1, 1, 2147483648";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(-1));
        }

        [TestMethod]
        public void NotAValidIntTest()
        {
            //5
            string inputData = "1, 1, asdf, 1, 2";
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(-1));
        }

        [TestMethod]
        public void InputLengthTooLargeTest()
        {
            //100,001
            List<string> temp = new List<string>();
            for(int i=1 ; i<100002 ; i++)
            {
                temp.Add(i.ToString());
            }

            string inputData = string.Join(",", temp);
            var output = PostFile.ProcessInput(inputData);
            Assert.IsTrue(output.Result.Equals(-1));
        }
    }
}
