using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NextLevelSeven.Core.Encoding;
using NUnit.Framework;

namespace NextLevelSeven.Test.Core.Encoding
{
    [TestFixture]
    public class EncodingConfigurationUnitTestFixture : EncodingBaseTestFixture
    {
        [Test]
        public void IEncoding_SetsComponentDelimiter()
        {
            var encoding = new EncodingConfiguration();
            ((IEncoding) encoding).ComponentDelimiter = '$';
            encoding.ComponentDelimiter.Should().Be('$');
        }

        [Test]
        public void IEncoding_SetsEscapeCharacter()
        {
            var encoding = new EncodingConfiguration();
            ((IEncoding)encoding).EscapeCharacter = '$';
            encoding.EscapeCharacter.Should().Be('$');
        }

        [Test]
        public void IEncoding_SetsFieldDelimiter()
        {
            var encoding = new EncodingConfiguration();
            ((IEncoding)encoding).FieldDelimiter = '$';
            encoding.FieldDelimiter.Should().Be('$');
        }

        [Test]
        public void IEncoding_SetsRepetitionDelimiter()
        {
            var encoding = new EncodingConfiguration();
            ((IEncoding)encoding).RepetitionDelimiter = '$';
            encoding.RepetitionDelimiter.Should().Be('$');
        }

        [Test]
        public void IEncoding_SetsSubcomponentDelimiter()
        {
            var encoding = new EncodingConfiguration();
            ((IEncoding)encoding).SubcomponentDelimiter = '$';
            encoding.SubcomponentDelimiter.Should().Be('$');
        }
    }
}
