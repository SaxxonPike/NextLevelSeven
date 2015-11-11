using System;
using FluentAssertions;
using NextLevelSeven.Conversion;
using NUnit.Framework;

namespace NextLevelSeven.Test.Conversion
{
    [TestFixture]
    public class DateTimeConverterUnitTestFixture : ConversionBaseTestFixture
    {
        [Test]
        [ExpectedException(typeof(ConversionException))]
        public void DateTimeConverter_ThrowsWithInvalidYear()
        {
            DateTimeConverter.ConvertToDate("1").Should().NotHaveValue();
        }

        [Test]
        public void DateTimeConverter_DecodesTime()
        {
            DateTimeConverter.ConvertToTime("060810").Should()
                .Be(new TimeSpan(6, 8, 10));
        }

        [Test]
        public void DateTimeConverter_DecodesNullTime()
        {
            DateTimeConverter.ConvertToTime(null).Should().NotHaveValue();
        }

        [Test]
        public void DateTimeConverter_DecodesDate()
        {
            DateTimeConverter.ConvertToDate("20130528").Should()
                .Be(new DateTime(2013, 05, 28));
        }

        [Test]
        public void DateTimeConverter_DecodesNullDate()
        {
            DateTimeConverter.ConvertToDate(null).Should().NotHaveValue();
        }

        [Test]
        public void DateTimeConverter_DecodesDateTime()
        {
            DateTimeConverter.ConvertToDateTime("20130528073829").Should()
                .Be(new DateTime(2013, 05, 28, 07, 38, 29));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithNegativeTimeZoneOffset()
        {
            DateTimeConverter.ConvertToDateTime("20130528073829-0530").Should()
                .Be(new DateTimeOffset(2013, 05, 28, 07, 38, 29, new TimeSpan(-5, 30, 0)));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithPositiveTimeZoneOffset()
        {
            DateTimeConverter.ConvertToDateTime("20130528073829+0530").Should()
                .Be(new DateTimeOffset(2013, 05, 28, 07, 38, 29, new TimeSpan(5, 30, 0)));
        }

        [Test]
        public void DateTimeConverter_DecodesNullDateTime()
        {
            DateTimeConverter.ConvertToDateTime(null).Should().NotHaveValue();
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyYear()
        {
            DateTimeConverter.ConvertToDateTime("2015").Should()
                .Be(new DateTime(2015, 01, 01, 00, 00, 00));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyYearMonth()
        {
            DateTimeConverter.ConvertToDateTime("201507").Should()
                .Be(new DateTime(2015, 07, 01, 00, 00, 00));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDate()
        {
            DateTimeConverter.ConvertToDateTime("20150708").Should()
                .Be(new DateTime(2015, 07, 08, 00, 00, 00));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDateHour()
        {
            DateTimeConverter.ConvertToDateTime("2015070818").Should()
                .Be(new DateTime(2015, 07, 08, 18, 00, 00));
        }

        [Test]
        public void DateTimeConverter_DecodesDateTimeWithOnlyDateHourMinute()
        {
            DateTimeConverter.ConvertToDateTime("201507081852").Should()
                .Be(new DateTime(2015, 07, 08, 18, 52, 00));
        }

        [Test]
        public void DateTimeConverter_DecodesDateWithOnlyYear()
        {
            DateTimeConverter.ConvertToDate("2015").Should()
                .Be(new DateTime(2015, 01, 01));
        }

        [Test]
        public void DateTimeConverter_DecodesDateWithOnlyYearMonth()
        {
            DateTimeConverter.ConvertToDate("201507").Should()
                .Be(new DateTime(2015, 07, 01));
        }

        [Test]
        public void DateTimeConverter_DecodesTimeWithOnlyDateHour()
        {
            DateTimeConverter.ConvertToTime("13").Should()
                .Be(new TimeSpan(13, 00, 00));
        }

        [Test]
        public void DateTimeConverter_DecodesTimeWithOnlyDateHourMinute()
        {
            DateTimeConverter.ConvertToTime("1335").Should()
                .Be(new TimeSpan(13, 35, 00));
        }
    }
}