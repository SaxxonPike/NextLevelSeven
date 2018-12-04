using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class BuilderFunctionalTestFixture : BuildingBaseTestFixture
    {
        [Test]
        public void Builder_ImplementsEncodingAndReadOnlyEncodingIdentically()
        {
            var builder = Message.Build();
            builder.Encoding.Should().BeSameAs(((IElement) builder).Encoding);
        }

        [Test]
        public void Builder_CanBeErased()
        {
            var builder = Message.Build(Any.Message())[1][3];
            var value = Any.String();
            builder.RawValue = value;
            builder.Erase();
            builder.RawValue.Should().BeNull();
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void Builder_DefaultsToNonExistant()
        {
            var builder = Message.Build(Any.Message())[1][3];
            builder[2].Exists.Should().BeFalse();
        }

        [Test]
        public void Builder_ConvertsHl7NullToExistingNull()
        {
            var builder = Message.Build(Any.Message());
            builder[1][3].RawValue = "\"\"";
            builder[1][3].RawValue.Should().BeNull();
        }

        [Test]
        public void Builder_ShouldEqualItself()
        {
            var builder = (object)Message.Build(Any.Message());
            var builder2 = builder;
            builder.Should().BeEquivalentTo(builder2);
        }

        [Test]
        public void Builder_ShouldHaveHashCode()
        {
            Message.Build(Any.Message()).GetHashCode().Should().NotBe(0);
        }

        [Test]
        public void Builder_CanFormat()
        {
            var param = Any.String();
            const string message = "{0}|{1}";
            Message.BuildFormat(message, ExampleMessageRepository.Minimum, param)
                .RawValue.Should()
                .Be(string.Format(message, ExampleMessageRepository.Minimum, param));
        }
    }
}