using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class SubcomponentBuilderFunctionalTests : BuildingTestFixture
    {
        [TestMethod]
        public void SubcomponentBuilder_ExistsWithNonNullValue()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1][1];
            builder.Value = Mock.String();
            Assert.IsTrue(builder.Exists);
        }

        [TestMethod]
        public void SubcomponentBuilder_ExistsWithNullPresentValue()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1][1];
            builder.Value = HL7.Null;
            Assert.IsTrue(builder.Exists);
        }

        [TestMethod]
        public void SubcomponentBuilder_DoesNotExistWithNullValue()
        {
            var builder = Message.Build(Mock.Message())[1][3][1][1][1];
            builder.Value = null;
            Assert.IsFalse(builder.Exists);
        }

        [TestMethod]
        public void SubcomponentBuilder_GetsCodec()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            var codec = builder.Converter;
            var value = Mock.Number();
            codec.AsInt = value;
            Assert.AreEqual(value.ToString(CultureInfo.InvariantCulture), builder.Value);
        }

        [TestMethod]
        public void SubcomponentBuilder_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            Assert.AreEqual(0, builder.Descendants.Count());
        }

        [TestMethod]
        public void SubcomponentBuilder_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            Assert.AreEqual('\0', builder.Delimiter);
        }

        [TestMethod]
        public void SubcomponentBuilder_ThrowsOnIndex()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            AssertAction.Throws<ElementException>(() => Assert.Inconclusive(builder[1].Value));
        }

        [TestMethod]
        public void SubcomponentBuilder_ThrowsOnDelete()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            AssertAction.Throws<ElementException>(() => builder.Delete(1));
        }

        [TestMethod]
        public void SubcomponentBuilder_ThrowsOnMove()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            AssertAction.Throws<ElementException>(() => builder.Move(1, 2));
        }

        [TestMethod]
        public void SubcomponentBuilder_ThrowsOnInsertString()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            AssertAction.Throws<ElementException>(() => builder.Insert(1, Mock.String()));
        }

        [TestMethod]
        public void SubcomponentBuilder_ThrowsOnInsertElement()
        {
            var builder0 = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            var builder1 = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            AssertAction.Throws<ElementException>(() => builder0.Insert(1, builder1));
        }

        [TestMethod]
        public void SubcomponentBuilder_SetsValuesWithConcatenation()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Values = new[] {"a", "b", "c", "d"};
            Assert.AreEqual(builder.Value, "abcd");
        }

        [TestMethod]
        public void SubcomponentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void SubcomponentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1] as ISubcomponent;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void SubcomponentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1] as IElement;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void SubcomponentBuilder_CanGetMessage()
        {
            var message = Message.Build(Mock.Message());
            var builder = message[1][3][1][1][1];
            Assert.AreSame(message, builder.Message);
        }

        [TestMethod]
        public void SubcomponentBuilder_CloneHasNullMessage()
        {
            var message = Message.Build(Mock.Message());
            var builder = message[1][3][1][1][1].Clone();
            Assert.IsNull(builder.Message);
        }

        [TestMethod]
        public void SubcomponentBuilder_CanGetValue()
        {
            var val0 = Mock.String();
            var val1 = Mock.String();
            var builder =
                Message.Build(string.Format("MSH|^~\\&|{0}&{1}&{2}", val0, val1, Mock.String()))[1][3][1][1][2];
            Assert.AreEqual(builder.Value, val1);
        }

        [TestMethod]
        public void SubcomponentBuilder_CanGetValues()
        {
            var val1 = Mock.String();
            var val2 = Mock.String();
            var val3 = Mock.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Mock.String(), val1, val2, val3))[1][3][2][2][2];
            AssertEnumerable.AreEqual(builder.Values, new[] {val3});
        }

        [TestMethod]
        public void SubcomponentBuilder_CanBeCloned()
        {
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Mock.String(), Mock.String(), Mock.String(), Mock.String()))[1][3][2][2][2];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void SubcomponentBuilder_CanBeCloned_Generic()
        {
            IElement builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Mock.String(), Mock.String(), Mock.String(), Mock.String()))[1][3][2][2][2];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void SubcomponentBuilder_HasOneValue()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            Assert.AreEqual(1, builder.ValueCount);
        }
    }
}