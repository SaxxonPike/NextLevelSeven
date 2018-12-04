using System.Globalization;
using FluentAssertions;
using NextLevelSeven.Building;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NextLevelSeven.Test.Utility;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class SubcomponentBuilderFunctionalTestFixture : DescendantElementBuilderBaseTestFixture<ISubcomponentBuilder, ISubcomponent>
    {
        protected override ISubcomponentBuilder BuildBuilder()
        {
            return Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
        }

        [Test]
        public void SubcomponentBuilder_ExistsWithNonNullValue()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1][1];
            builder.RawValue = Any.String();
            builder.Exists.Should().BeTrue();
        }

        [Test]
        public void SubcomponentBuilder_ExistsWithNullPresentValue()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1][1];
            builder.RawValue = HL7.Null;
            builder.Exists.Should().BeTrue();
        }

        [Test]
        public void SubcomponentBuilder_DoesNotExistWithNullValue()
        {
            var builder = Message.Build(Any.Message())[1][3][1][1][1];
            builder.RawValue = null;
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void SubcomponentBuilder_GetsCodec()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            var codec = builder.As;
            var value = Any.Number();
            codec.Integer = value;
            builder.RawValue.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void SubcomponentBuilder_HasNoDescendants()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.Descendants.Should().BeEmpty();
        }

        [Test]
        public void SubcomponentBuilder_HasNoDelimiter()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.Delimiter.Should().Be('\0');
        }

        [Test]
        public void SubcomponentBuilder_ThrowsOnIndex()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.Invoking(b => b[1].RawValue.Ignore()).Should().Throw<ElementException>();
        }

        [Test]
        public void SubcomponentBuilder_ThrowsOnDelete()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.Invoking(b => b.Delete(1)).Should().Throw<ElementException>();
        }

        [Test]
        public void SubcomponentBuilder_ThrowsOnMove()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.Invoking(b => b.Move(1, 2)).Should().Throw<ElementException>();
        }

        [Test]
        public void SubcomponentBuilder_ThrowsOnInsertString()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.Invoking(b => b.Insert(1, Any.String())).Should().Throw<ElementException>();
        }

        [Test]
        public void SubcomponentBuilder_ThrowsOnInsertElement()
        {
            var builder0 = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            var builder1 = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder0.Invoking(b => b.Insert(1, builder1)).Should().Throw<ElementException>();
        }

        [Test]
        public void SubcomponentBuilder_SetsValuesWithConcatenation()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.RawValues = new[] {"a", "b", "c", "d"};
            builder.RawValue.Should().Be("abcd");
        }

        [Test]
        public void SubcomponentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SubcomponentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1] as ISubcomponent;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SubcomponentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1] as IElement;
            builder.Should().BeSameAs(builder.Ancestor[1]);
        }

        [Test]
        public void SubcomponentBuilder_CanGetMessage()
        {
            var message = Message.Build(Any.Message());
            var builder = message[1][3][1][1][1];
            message.Should().BeSameAs(builder.Message);
        }

        [Test]
        public void SubcomponentBuilder_CloneHasNullMessage()
        {
            var message = Message.Build(Any.Message());
            var builder = message[1][3][1][1][1].Clone();
            builder.Message.Should().BeNull();
        }

        [Test]
        public void SubcomponentBuilder_CanGetValue()
        {
            var val0 = Any.String();
            var val1 = Any.String();
            var builder =
                Message.Build($"MSH|^~\\&|{val0}&{val1}&{Any.String()}")[1][3][1][1][2];
            builder.RawValue.Should().Be(val1);
        }

        [Test]
        public void SubcomponentBuilder_CanGetValues()
        {
            var val1 = Any.String();
            var val2 = Any.String();
            var val3 = Any.String();
            var builder = Message.Build($"MSH|^~\\&|{Any.String()}~{val1}^{val2}&{val3}")[1][3][2][2][2];
            builder.RawValues.Should().Equal(val3);
        }

        [Test]
        public void SubcomponentBuilder_HasOneValue()
        {
            var builder = Message.Build(ExampleMessageRepository.Standard)[1][3][1][1][1];
            builder.ValueCount.Should().Be(1);
        }
    }
}