using System;
using System.Linq;
using FluentAssertions;
using NextLevelSeven.Conversion;
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
        public void Codec_CanSetDate()
        {
            var message = Message.Parse(Any.Message());
            var date = DateTime.Now;
            message[1][3].Converter.AsDate = date;
            message[1][3].Value.Should().Be(DateTimeConverter.ConvertFromDate(date));
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
        public void Codec_CanSetDates()
        {
            var message = Message.Parse(Any.Message());
            var dates = new DateTime?[] { DateTime.Now, DateTime.Now.AddDays(-1) };
            message[1][3].Converter.AsDates.Items = dates;
            message[1][3].Values.Should().BeEquivalentTo(dates.Select(DateTimeConverter.ConvertFromDate));
        }

        [Test]
        public void Codec_CanGetDateTime()
        {
            var message = Message.Parse(Any.Message());
            message[1][3].Value = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].Converter.AsDateTime.Should().HaveValue();
        }

        [Test]
        public void Codec_CanSetDateTime()
        {
            var message = Message.Parse(Any.Message());
            var dateTime = DateTimeOffset.Now;
            message[1][3].Converter.AsDateTime = dateTime;
            message[1][3].Value.Should().Be(DateTimeConverter.ConvertFromDateTime(dateTime));
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
        public void Codec_CanSetDateTimes()
        {
            var message = Message.Parse(Any.Message());
            var dateTimes = new DateTimeOffset?[] { DateTimeOffset.Now, DateTimeOffset.Now.AddDays(-1) };
            message[1][3].Converter.AsDateTimes.Items = dateTimes;
            message[1][3].Values.Should().BeEquivalentTo(dateTimes.Select(DateTimeConverter.ConvertFromDateTime));
        }

        [Test]
        public void Codec_CanGetDecimal()
        {
            var message = Message.Parse(Any.Message());
            message[1][3].Value = Any.Decimal();
            message[1][3].Converter.AsDecimal.Should().HaveValue();
        }

        [Test]
        public void Codec_CanSetDecimal()
        {
            var input = NumberConverter.ConvertToDecimal(Any.Decimal());
            var message = Message.Parse(Any.Message());
            message[1][3].Converter.AsDecimal = input;
            message[1][3].Value.Should().Be(input.ToString());
        }
    }
}
