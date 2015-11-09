using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Core.Codec
{
    [TestFixture]
    public class EncodedTypeConverterFunctionalTests : CodecTestFixture
    {
        [Test]
        public void Codec_CanGetDate()
        {
            var message = Message.Parse(MockFactory.Message());
            message[1][3].Value = MockFactory.DateTimeMillisecondsWithTimeZone();
            Assert.IsNotNull(message[1][3].Converter.AsDate);
        }

        [Test]
        public void Codec_CanGetDates()
        {
            var message = Message.Parse(MockFactory.Message());
            message[1][3][1].Value = MockFactory.DateTimeMillisecondsWithTimeZone();
            message[1][3][2].Value = MockFactory.DateTimeMillisecondsWithTimeZone();
            Assert.AreEqual(2, message[1][3].Converter.AsDates.Count);
        }

        [Test]
        public void Codec_CanGetDateTime()
        {
            var message = Message.Parse(MockFactory.Message());
            message[1][3].Value = MockFactory.DateTimeMillisecondsWithTimeZone();
            Assert.IsNotNull(message[1][3].Converter.AsDateTime);
        }

        [Test]
        public void Codec_CanGetDateTimes()
        {
            var message = Message.Parse(MockFactory.Message());
            message[1][3][1].Value = MockFactory.DateTimeMillisecondsWithTimeZone();
            message[1][3][2].Value = MockFactory.DateTimeMillisecondsWithTimeZone();
            Assert.AreEqual(2, message[1][3].Converter.AsDateTimes.Count);
        }

        [Test]
        public void Codec_CanGetDecimal()
        {
            var message = Message.Parse(MockFactory.Message());
            message[1][3].Value = MockFactory.Decimal();
            Assert.IsNotNull(message[1][3].Converter.AsDecimal);
        }
    }
}
