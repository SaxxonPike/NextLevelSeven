using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Parsing;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class MessageExtensionFunctionalTests : ParsingTestFixture
    {
        [Test]
        public void MessageExtensions_Builder_CanFilterPid_FromMessage()
        {
            var message = Message.Build(ExampleMessages.MultiplePid);
            message.Segments.ExceptType("PID").Should().NotBeEmpty()
                .And.OnlyContain(s => s.Type != "PID");
        }

        [Test]
        public void MessageExtensions_Parser_CanFilterPid_FromMessage()
        {
            var message = Message.Parse(ExampleMessages.MultiplePid);
            message.Segments.ExceptType("PID").Should().NotBeEmpty()
                .And.OnlyContain(s => s.Type != "PID");
        }

        [Test]
        public void MessageExtensions_Builder_CanFilterPidAndObx_FromMessage()
        {
            var message = Message.Build(ExampleMessages.MultiplePid);
            message.Segments.ExceptTypes("PID", "OBX").Should().NotBeEmpty()
                .And.OnlyContain(s => s.Type != "PID" && s.Type != "OBX");
        }

        [Test]
        public void MessageExtensions_Parser_CanFilterPidAndObx_FromMessage()
        {
            var message = Message.Parse(ExampleMessages.MultiplePid);
            message.Segments.ExceptTypes("PID", "OBX").Should().NotBeEmpty()
                .And.OnlyContain(s => s.Type != "PID" && s.Type != "OBX");
        }

        [Test]
        public void MessageExtensions_Builder_CanGetObrSplitsWithoutExtras()
        {
            var message = Message.Build(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR");
            splits.Count().Should().Be(message.Segments.OfType("OBR").Count());
        }

        [Test]
        public void MessageExtensions_Parser_CanGetObrSplitsWithoutExtras()
        {
            var message = Message.Parse(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR");
            splits.Count().Should().Be(message.Segments.OfType("OBR").Count());
        }

        [Test]
        public void MessageExtensions_Builder_CanGetObrSplitsWithExtras()
        {
            var message = Message.Build(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR", true);
            Assert.AreEqual(message.Segments.OfType("OBR").Count() + 1, splits.Count(),
                "OBR split count (with extras) doesn't match number of OBR segments + 1.");
        }

        [Test]
        public void MessageExtensions_Parser_CanGetObrSplitsWithExtras()
        {
            var message = Message.Parse(ExampleMessages.MultipleObr);
            var splits = message.SplitSegments("OBR", true);
            Assert.AreEqual(message.Segments.OfType("OBR").Count() + 1, splits.Count(),
                "OBR split count (with extras) doesn't match number of OBR segments + 1.");
        }
    }
}