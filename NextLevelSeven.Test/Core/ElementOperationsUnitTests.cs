using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ElementOperationsUnitTests : CoreTestFixture
    {
        [TestMethod]
        public void HasEncodingCharacters_ReturnsTrue_OnMsh1()
        {
            var element = Message.Build(Mock.Message());
            Assert.IsTrue(ElementOperations.HasEncodingCharacters(element[1][1]));
        }

        [TestMethod]
        public void HasEncodingCharacters_ReturnsTrue_OnMsh2()
        {
            var element = Message.Build(Mock.Message());
            Assert.IsTrue(ElementOperations.HasEncodingCharacters(element[1][2]));
        }

        [TestMethod]
        public void HasEncodingCharacters_ReturnsFalse_OnNonEncodingFields()
        {
            var element = Message.Build(Mock.Message());
            foreach (var field in element[1].Fields.Skip(3)) //skip type, msh1, msh2
            {
                Assert.IsFalse(ElementOperations.HasEncodingCharacters(field));
            }
        }

        [TestMethod]
        public void HasEncodingCharacters_ReturnsFalse_OnSegmentContainingFieldsWithEncodingCharacters()
        {
            var element = Message.Build(Mock.Message());
            Assert.IsFalse(ElementOperations.HasEncodingCharacters(element[1]));
        }

        [TestMethod]
        public void GetKey_ReturnsRootKey_WhenCalledWithRootElement()
        {
            var element = Message.Build(Mock.Message());
            Assert.AreEqual(ElementDefaults.RootElementKey, ElementOperations.GetKey(element));
        }

        [TestMethod]
        public void GetKey_ReturnsKey_WhenCalledOnSegment()
        {
            var element = Message.Build(Mock.Message());
            Assert.AreEqual("MSH1", ElementOperations.GetKey(element[1]));
        }

        [TestMethod]
        public void GetKey_ReturnsKey_WhenCalledOnField()
        {
            var element = Message.Build(Mock.Message());
            Assert.AreEqual("MSH1.3", ElementOperations.GetKey(element[1][3]));
        }

        [TestMethod]
        public void GetKey_ReturnsKey_WhenCalledOnRepetition()
        {
            var element = Message.Build(Mock.Message());
            Assert.AreEqual("MSH1.3.1", ElementOperations.GetKey(element[1][3][1]));
        }

        [TestMethod]
        public void GetKey_ReturnsKey_WhenCalledOnComponent()
        {
            var element = Message.Build(Mock.Message());
            Assert.AreEqual("MSH1.3.1.1", ElementOperations.GetKey(element[1][3][1][1]));
        }

        [TestMethod]
        public void GetKey_ReturnsKey_WhenCalledOnSubcomponent()
        {
            var element = Message.Build(Mock.Message());
            Assert.AreEqual("MSH1.3.1.1.1", ElementOperations.GetKey(element[1][3][1][1][1]));
        }

    }
}
