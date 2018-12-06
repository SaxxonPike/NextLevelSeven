using System;
using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NUnit.Framework;

namespace NextLevelSeven.Test.Performance
{
    [TestFixture]
    [Explicit("These are designed to be run with a profiler.")]
    public class LargeMessagePerformanceTestFixture : BaseTestFixture
    {
        [Test]
        public void Parse_Perf()
        {
            var fields = new[]
            {
                "MSH",
                "^~\\&"
            }.Concat(Enumerable.Range(0, 1000000).Select(i => $"{i}"))
                .ToArray();
            var messageText = string.Join("|", fields);
            var message = Message.Parse(messageText);
            message.Segments.First()[500000].Value.Should().Be(fields[500000 - 1]);
        }
        
        [Test]
        public void Replace_Perf()
        {
            var fields = new[]
                {
                    "MSH",
                    "^~\\&"
                }.Concat(Enumerable.Range(0, 1000000).Select(i => $"{i}"))
                .ToArray();
            var messageText = string.Join("|", fields);
            var message = Message.Parse(messageText);
            message.Segments.First()[500000].Value = "test";
        }
    }
}