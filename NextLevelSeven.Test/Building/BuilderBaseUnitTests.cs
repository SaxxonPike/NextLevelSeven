using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class BuilderBaseUnitTests : BuildingTestFixture
    {
        [TestMethod]
        public void Builder_ImplementsEncodingAndReadOnlyEncodingIdentically()
        {
            var builder = Message.Build();
            Assert.AreSame(builder.Encoding, ((IElement) builder).Encoding);
        }

        [TestMethod]
        public void Builder_CanBeErased()
        {
            var builder = Message.Build(Mock.Message())[1][3];
            var value = Mock.String();
            builder.Value = value;
            builder.Erase();
            Assert.IsNull(builder.Value);
            Assert.IsFalse(builder.Exists);
        }

        [TestMethod]
        public void Builder_DefaultsToNonExistant()
        {
            var builder = Message.Build(Mock.Message())[1][3];
            Assert.IsFalse(builder[2].Exists);
        }

        [TestMethod]
        public void Builder_ConvertsHl7NullToExistingNull()
        {
            var builder = Message.Build(Mock.Message());
            builder[1][3].Value = "\"\"";
            Assert.IsNull(builder[1][3].Value);
        }

        [TestMethod]
        public void Builder_ShouldEqualItself()
        {
            var builder = (object)Message.Build(Mock.Message());
            var builder2 = builder;
            Assert.IsTrue(builder.Equals(builder2));
        }

        [TestMethod]
        public void Builder_ShouldHaveHashCode()
        {
            Assert.AreNotEqual(0, Message.Build(Mock.Message()).GetHashCode());
        }

        [TestMethod]
        public void Builder_CanFormat()
        {
            var param = Mock.String();
            const string message = "{0}|{1}";
            Assert.AreEqual(Message.BuildFormat(message, ExampleMessages.Minimum, param).Value,
                string.Format(message, ExampleMessages.Minimum, param));
        }
    }
}