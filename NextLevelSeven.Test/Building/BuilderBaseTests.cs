using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public class BuilderBaseTests : BuildingTestFixture
    {
        [TestMethod]
        public void Builder_CanFormat()
        {
            var param = Randomized.String();
            const string message = "{0}|{1}";
            Assert.AreEqual(Message.BuildFormat(message, ExampleMessages.Minimum, param).Value,
                string.Format(message, ExampleMessages.Minimum, param));
        }

        [TestMethod]
        public void Builders_AreContentEquivalent()
        {
            var message = ExampleMessages.Minimum;
            var builder = (object) Message.Build(message);
            Assert.IsTrue(builder.Equals(message));
        }

        [TestMethod]
        public void Builders_AreNullContentEquivalent()
        {
            var message = ExampleMessages.Minimum;
            Assert.IsTrue(Message.Build(message)[1][3].Equals(null));
        }

        [TestMethod]
        public void Builders_AreStringEquivalent()
        {
            var message = ExampleMessages.Minimum;
            var builder = (IEquatable<string>) Message.Build(message);
            Assert.IsTrue(builder.Equals(message));
        }

        [TestMethod]
        public void Builders_AreElementEquivalent()
        {
            var message = ExampleMessages.Minimum;
            IElement element = Message.Create(message);
            var builder = (IEquatable<IElement>) Message.Build(message);
            Assert.IsTrue(builder.Equals(element));
        }
    }
}
