using System.Globalization;
using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class SubcomponentBuilderFunctionalTests : BuildingTestFixture
    {
        [Test]
        public void SubcomponentBuilder_ExistsWithNonNullValue()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1][1];
            builder.Value = MockFactory.String();
            builder.Exists.Should().BeTrue();
        }

        [Test]
        public void SubcomponentBuilder_ExistsWithNullPresentValue()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1][1];
            builder.Value = HL7.Null;
            builder.Exists.Should().BeTrue();
        }

        [Test]
        public void SubcomponentBuilder_DoesNotExistWithNullValue()
        {
            var builder = Message.Build(MockFactory.Message())[1][3][1][1][1];
            builder.Value = null;
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void SubcomponentBuilder_GetsCodec()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            var codec = builder.Converter;
            var value = MockFactory.Number();
            codec.AsInt = value;
            builder.Value.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void SubcomponentBuilder_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Descendants.Should().BeEmpty();
        }

        [Test]
        public void SubcomponentBuilder_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Delimiter.Should().Be('\0');
        }

        [Test]
        [ExpectedException(typeof(BuilderException))]
        public void SubcomponentBuilder_ThrowsOnIndex()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder[1].Value.Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(BuilderException))]
        public void SubcomponentBuilder_ThrowsOnDelete()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Delete(1);
        }

        [Test]
        [ExpectedException(typeof(BuilderException))]
        public void SubcomponentBuilder_ThrowsOnMove()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Move(1, 2);
        }

        [Test]
        [ExpectedException(typeof(BuilderException))]
        public void SubcomponentBuilder_ThrowsOnInsertString()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Insert(1, MockFactory.String()).Should().BeNull();
        }

        [Test]
        [ExpectedException(typeof(BuilderException))]
        public void SubcomponentBuilder_ThrowsOnInsertElement()
        {
            var builder0 = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            var builder1 = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder0.Insert(1, builder1).Should().BeNull();
        }

        [Test]
        public void SubcomponentBuilder_SetsValuesWithConcatenation()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Values = new[] {"a", "b", "c", "d"};
            builder.Value.Should().Be("abcd");
        }

        [Test]
        public void SubcomponentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SubcomponentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1] as ISubcomponent;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SubcomponentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1] as IElement;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SubcomponentBuilder_CanGetMessage()
        {
            var message = Message.Build(MockFactory.Message());
            var builder = message[1][3][1][1][1];
            message.Should().BeSameAs(builder.Message);
        }

        [Test]
        public void SubcomponentBuilder_CloneHasNullMessage()
        {
            var message = Message.Build(MockFactory.Message());
            var builder = message[1][3][1][1][1].Clone();
            builder.Message.Should().BeNull();
        }

        [Test]
        public void SubcomponentBuilder_CanGetValue()
        {
            var val0 = MockFactory.String();
            var val1 = MockFactory.String();
            var builder =
                Message.Build(string.Format("MSH|^~\\&|{0}&{1}&{2}", val0, val1, MockFactory.String()))[1][3][1][1][2];
            builder.Value.Should().Be(val1);
        }

        [Test]
        public void SubcomponentBuilder_CanGetValues()
        {
            var val1 = MockFactory.String();
            var val2 = MockFactory.String();
            var val3 = MockFactory.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                MockFactory.String(), val1, val2, val3))[1][3][2][2][2];
            builder.Values.Should().Equal(val3);
        }

        [Test]
        public void SubcomponentBuilder_CanBeCloned()
        {
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                MockFactory.String(), MockFactory.String(), MockFactory.String(), MockFactory.String()))[1][3][2][2][2];
            var clone = builder.Clone();
            builder.Should().NotBeSameAs(clone);
            builder.ToString().Should().Be(clone.ToString());
        }

        [Test]
        public void SubcomponentBuilder_CanBeCloned_Generic()
        {
            IElement builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                MockFactory.String(), MockFactory.String(), MockFactory.String(), MockFactory.String()))[1][3][2][2][2];
            var clone = builder.Clone();
            builder.Should().NotBeSameAs(clone);
            builder.ToString().Should().Be(clone.ToString());
        }

        [Test]
        public void SubcomponentBuilder_HasOneValue()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.ValueCount.Should().Be(1);
        }
    }
}