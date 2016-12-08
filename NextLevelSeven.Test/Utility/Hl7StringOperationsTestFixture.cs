using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NextLevelSeven.Test.Testing;
using NextLevelSeven.Utility;
using NUnit.Framework;

namespace NextLevelSeven.Test.Utility
{
    [TestFixture]
    public class Hl7StringOperationsTestFixture : UtilityBaseTestFixture
    {
        [Test]
        [TestCase("abcd")]
        [TestCase("abcd|efgh")]
        public void NormalizeLineEndings_ReplacesEnvironmentNewlines(string input)
        {
            // Arrange.
            var platformInput = input.Replace("|", Environment.NewLine);
            var delimiter = Any.Symbol();

            // Act.
            var result = Hl7StringOperations.NormalizeLineEndings(platformInput, delimiter[0]);

            // Assert.
            result.Should().Be(platformInput.Replace(Environment.NewLine, delimiter));
        }

        [Test]
        [TestCase("abcd")]
        [TestCase("abcd\nefgh")]
        public void NormalizeLineEndings_ReplacesLineBreaks(string input)
        {
            // Arrange.
            var delimiter = Any.Symbol();

            // Act.
            var result = Hl7StringOperations.NormalizeLineEndings(input, delimiter[0]);

            // Assert.
            result.Should().Be(input.Replace("\n", delimiter));
        }
    }
}
