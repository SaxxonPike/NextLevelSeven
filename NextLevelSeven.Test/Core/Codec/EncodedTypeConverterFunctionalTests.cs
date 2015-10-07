using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Core.Codec
{
    [TestClass]
    public class EncodedTypeConverterFunctionalTests : CodecTestFixture
    {
        [TestMethod]
        public void Codec_CanGetDate()
        {
            var message = Message.Parse(Mock.Message());
            message[1][3].Value = Mock.DateTimeMillisecondsWithTimeZone();
            Assert.IsNotNull(message[1][3].Codec.AsDate);
        }

        [TestMethod]
        public void Codec_CanGetDates()
        {
            var message = Message.Parse(Mock.Message());
            message[1][3][1].Value = Mock.DateTimeMillisecondsWithTimeZone();
            message[1][3][2].Value = Mock.DateTimeMillisecondsWithTimeZone();
            Assert.AreEqual(2, message[1][3].Codec.AsDates.Count);
        }

        [TestMethod]
        public void Codec_CanGetDateTime()
        {
            var message = Message.Parse(Mock.Message());
            message[1][3].Value = Mock.DateTimeMillisecondsWithTimeZone();
            Assert.IsNotNull(message[1][3].Codec.AsDateTime);
        }

        [TestMethod]
        public void Codec_CanGetDateTimes()
        {
            var message = Message.Parse(Mock.Message());
            message[1][3][1].Value = Mock.DateTimeMillisecondsWithTimeZone();
            message[1][3][2].Value = Mock.DateTimeMillisecondsWithTimeZone();
            Assert.AreEqual(2, message[1][3].Codec.AsDateTimes.Count);
        }

        [TestMethod]
        public void Codec_CanGetDecimal()
        {
            var message = Message.Parse(Mock.Message());
            message[1][3].Value = Mock.Decimal();
            Assert.IsNotNull(message[1][3].Codec.AsDecimal);
        }
    }
}
