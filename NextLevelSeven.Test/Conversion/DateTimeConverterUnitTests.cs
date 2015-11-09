using System;
using NextLevelSeven.Conversion;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Conversion
{
    [TestFixture]
    public class DateTimeConverterUnitTests : ConversionTestFixture
    {
        [Test]
        public void DateTimeConverter_ThrowsWithInvalidYear()
        {
            AssertAction.Throws<ConversionException>(() => DateTimeConverter.ConvertToDate("1"));
        }

        [Test]
        public void DateTimeConverter_DecodesTime()
        {
            Assert.AreEqual(new TimeSpan(6, 8, 10), DateTimeConverter.ConvertToTime("060810"));
        }

        [Test]
        public void DateTimeConverter_DecodesNullTime()
        {
            Assert.AreEqual(null, DateTimeConverter.ConvertToTime(null));
        }

        [Test]
        public void DateTimeConverter_DecodesDate()
        {
            Assert.AreEqual(new DateTime(2013, 05, 28), DateTimeConverter.ConvertToDate("20130528"));
        }

        [Test]
        public void DateTimeConverter_DecodesNullDate()
        {
            Assert.AreEqual(null, DateTimeConverter.ConvertToDate(null));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTime()
        {
            Assert.AreEqual(new DateTime(2013, 05, 28, 07, 38, 29),
                DateTimeConverter.ConvertToDateTime("20130528073829"));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithNegativeTimeZoneOffset()
        {
            Assert.AreEqual(new DateTimeOffset(2013, 05, 28, 07, 38, 29, new TimeSpan(-5, 30, 0)),
                DateTimeConverter.ConvertToDateTime("20130528073829-0530"));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithPositiveTimeZoneOffset()
        {
            Assert.AreEqual(new DateTimeOffset(2013, 05, 28, 07, 38, 29, new TimeSpan(5, 30, 0)),
                DateTimeConverter.ConvertToDateTime("20130528073829+0530"));
        }

        [Test]
        public void DateTimeConverter_DecodesNullDateTime()
        {
            Assert.AreEqual(null, DateTimeConverter.ConvertToDateTime(null));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyYear()
        {
            Assert.AreEqual(new DateTime(2015, 01, 01, 00, 00, 00), DateTimeConverter.ConvertToDateTime("2015"));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyYearMonth()
        {
            Assert.AreEqual(new DateTime(2015, 07, 01, 00, 00, 00), DateTimeConverter.ConvertToDateTime("201507"));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDate()
        {
            Assert.AreEqual(new DateTime(2015, 07, 08, 00, 00, 00), DateTimeConverter.ConvertToDateTime("20150708"));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDateHour()
        {
            Assert.AreEqual(new DateTime(2015, 07, 08, 18, 00, 00), DateTimeConverter.ConvertToDateTime("2015070818"));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDateHourMinute()
        {
            Assert.AreEqual(new DateTime(2015, 07, 08, 18, 52, 00), DateTimeConverter.ConvertToDateTime("201507081852"));
        }

        [Test]
        public void DateTimeConverter_DecodesDateWithOnlyYear()
        {
            Assert.AreEqual(new DateTime(2015, 01, 01), DateTimeConverter.ConvertToDate("2015"));
        }

        [Test]
        public void DateTimeConverter_DecodesDateWithOnlyYearMonth()
        {
            Assert.AreEqual(new DateTime(2015, 07, 01), DateTimeConverter.ConvertToDate("201507"));
        }

        [Test]
        public void DateTimeConverter_DecodesTimeWithOnlyDateHour()
        {
            Assert.AreEqual(new TimeSpan(13, 00, 00), DateTimeConverter.ConvertToTime("13"));
        }

        [Test]
        public void DateTimeConverter_DecodesTimeWithOnlyDateHourMinute()
        {
            Assert.AreEqual(new TimeSpan(13, 35, 00), DateTimeConverter.ConvertToTime("1335"));
        }
    }
}