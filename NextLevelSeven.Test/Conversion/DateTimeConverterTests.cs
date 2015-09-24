using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Conversion;

namespace NextLevelSeven.Test.Conversion
{
    [TestClass]
    public class DateTimeConverterTests : ConversionTestFixture
    {
        [TestMethod]
        public void DateTimeConverter_DecodesTime()
        {
            Assert.AreEqual(new TimeSpan(6, 8, 10), DateTimeConverter.ConvertToTime("060810"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesNullTime()
        {
            Assert.AreEqual(null, DateTimeConverter.ConvertToTime(null));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDate()
        {
            Assert.AreEqual(new DateTime(2013, 05, 28), DateTimeConverter.ConvertToDate("20130528"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesNullDate()
        {
            Assert.AreEqual(null, DateTimeConverter.ConvertToDate(null));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateTime()
        {
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29),
                DateTimeConverter.ConvertToDateTime("20130528073829"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateTimeWithNegativeTimeZoneOffset()
        {
            Assert.AreEqual(new DateTimeOffset(2013, 05, 28, 07, 38, 29, new TimeSpan(-5, 30, 0)),
                DateTimeConverter.ConvertToDateTime("20130528073829-0530"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateTimeWithPositiveTimeZoneOffset()
        {
            Assert.AreEqual(new DateTimeOffset(2013, 05, 28, 07, 38, 29, new TimeSpan(5, 30, 0)),
                DateTimeConverter.ConvertToDateTime("20130528073829+0530"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesNullDateTime()
        {
            Assert.AreEqual(null, DateTimeConverter.ConvertToDateTime(null));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateTimeWithOnlyYear()
        {
            Assert.AreEqual(new DateTime(2015, 01, 01, 00, 00, 00), DateTimeConverter.ConvertToDateTime("2015"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateTimeWithOnlyYearMonth()
        {
            Assert.AreEqual(new DateTime(2015, 07, 01, 00, 00, 00), DateTimeConverter.ConvertToDateTime("201507"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDate()
        {
            Assert.AreEqual(new DateTime(2015, 07, 08, 00, 00, 00), DateTimeConverter.ConvertToDateTime("20150708"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDateHour()
        {
            Assert.AreEqual(new DateTime(2015, 07, 08, 18, 00, 00), DateTimeConverter.ConvertToDateTime("2015070818"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDateHourMinute()
        {
            Assert.AreEqual(new DateTime(2015, 07, 08, 18, 52, 00), DateTimeConverter.ConvertToDateTime("201507081852"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateWithOnlyYear()
        {
            Assert.AreEqual(new DateTime(2015, 01, 01), DateTimeConverter.ConvertToDate("2015"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesDateWithOnlyYearMonth()
        {
            Assert.AreEqual(new DateTime(2015, 07, 01), DateTimeConverter.ConvertToDate("201507"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesTimeWithOnlyDateHour()
        {
            Assert.AreEqual(new TimeSpan(13, 00, 00), DateTimeConverter.ConvertToTime("13"));
        }

        [TestMethod]
        public void DateTimeConverter_DecodesTimeWithOnlyDateHourMinute()
        {
            Assert.AreEqual(new TimeSpan(13, 35, 00), DateTimeConverter.ConvertToTime("1335"));
        }
    }
}