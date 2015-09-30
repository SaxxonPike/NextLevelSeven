using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Building;
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
        public void Builder_CanBeCompared()
        {
            var content = Mock.Message();
            var builder0 = Message.Build(content);
            var builder1 = Message.Build(content);
            builder0.Segment(1).Field(3).Value = "0";
            builder1.Segment(1).Field(3).Value = "1";
            var list = new List<IBuilder> {builder1, builder0};
            list.Sort();
            Assert.AreSame(list[0], builder0);
            Assert.AreSame(list[1], builder1);
            list.Reverse();
            Assert.AreSame(list[0], builder1);
            Assert.AreSame(list[1], builder0);
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
            IElement element = Message.Parse(message);
            var builder = (IEquatable<IElement>) Message.Build(message);
            Assert.IsTrue(builder.Equals(element));
        }
    }
}