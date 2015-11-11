using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Core.Codec
{
    [TestFixture]
    public class EncodedTypeConverterFunctionalTestFixture : CodecBaseTestFixture
    {
        [Test]
        public void Codec_CanGetDate()
        {
            var message = Message.Parse(Any.Message());
            message[1][3].Value = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].Converter.AsDate.Should().HaveValue();
        }

        [Test]
        public void Codec_CanGetDates()
        {
            var message = Message.Parse(Any.Message());
            message[1][3][1].Value = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3][2].Value = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].Converter.AsDates.Count.Should().Be(2);
        }

        [Test]
        public void Codec_CanGetDateTime()
        {
            var message = Message.Parse(Any.Message());
            message[1][3].Value = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].Converter.AsDateTime.Should().HaveValue();
        }

        [Test]
        public void Codec_CanGetDateTimes()
        {
            var message = Message.Parse(Any.Message());
            message[1][3][1].Value = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3][2].Value = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].Converter.AsDateTimes.Count.Should().Be(2);
        }

        [Test]
        public void Codec_CanGetDecimal()
        {
            var message = Message.Parse(Any.Message());
            message[1][3].Value = Any.Decimal();
            message[1][3].Converter.AsDecimal.Should().HaveValue();
        }
    }
}
