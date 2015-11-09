using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Building
{
    [TestFixture]
    public sealed class BuilderBaseFunctionalTests : BuildingTestFixture
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
            var builder = Message.Build(MockFactory.Message())[1][3];
            var value = MockFactory.String();
            builder.Value = value;
            builder.Erase();
            builder.Value.Should().BeNull();
            builder.Exists.Should().BeFalse();
        }

        [Test]
        public void Builder_DefaultsToNonExistant()
        {
            var builder = Message.Build(MockFactory.Message())[1][3];
            builder[2].Exists.Should().BeFalse();
        }

        [Test]
        public void Builder_ConvertsHl7NullToExistingNull()
        {
            var builder = Message.Build(MockFactory.Message());
            builder[1][3].Value = "\"\"";
            builder[1][3].Value.Should().BeNull();
        }

        [Test]
        public void Builder_ShouldEqualItself()
        {
            var builder = (object)Message.Build(MockFactory.Message());
            var builder2 = builder;
            builder.ShouldBeEquivalentTo(builder2);
        }

        [Test]
        public void Builder_ShouldHaveHashCode()
        {
            Message.Build(MockFactory.Message()).GetHashCode().Should().NotBe(0);
        }

        [Test]
        public void Builder_CanFormat()
        {
            var param = MockFactory.String();
            const string message = "{0}|{1}";
            Message.BuildFormat(message, ExampleMessages.Minimum, param)
                .Value.Should()
                .Be(string.Format(message, ExampleMessages.Minimum, param));
        }
    }
}