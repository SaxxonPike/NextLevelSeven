using System;
using System.Linq;
using FluentAssertions;
using Moq;
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
            message[1][3].RawValue = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].As.Date.Should().HaveValue();
        }

        [Test]
        public void Codec_CanSetDate()
        {
            var message = Message.Parse(Any.Message());
            var date = DateTime.Now;
            message[1][3].As.Date = date;
            message[1][3].RawValue.Should().Be(DateTimeConverter.ConvertFromDate(date));
        }

        [Test]
        public void Codec_CanGetDates()
        {
            var message = Message.Parse(Any.Message());
            message[1][3][1].RawValue = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3][2].RawValue = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].As.Dates.Count.Should().Be(2);
        }

        [Test]
        public void Codec_CanSetDates()
        {
            var message = Message.Parse(Any.Message());
            var dates = new DateTime?[] { DateTime.Now, DateTime.Now.AddDays(-1) };
            message[1][3].As.Dates.Items = dates;
            message[1][3].RawValues.Should().BeEquivalentTo(dates.Select(DateTimeConverter.ConvertFromDate));
        }

        [Test]
        public void Codec_CanGetDateTime()
        {
            var message = Message.Parse(Any.Message());
            message[1][3].RawValue = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].As.DateTime.Should().HaveValue();
        }

        [Test]
        public void Codec_CanSetDateTime()
        {
            var message = Message.Parse(Any.Message());
            var dateTime = DateTimeOffset.Now;
            message[1][3].As.DateTime = dateTime;
            message[1][3].RawValue.Should().Be(DateTimeConverter.ConvertFromDateTime(dateTime));
        }

        [Test]
        public void Codec_CanGetDateTimes()
        {
            var message = Message.Parse(Any.Message());
            message[1][3][1].RawValue = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3][2].RawValue = Any.DateTimeMillisecondsWithTimeZone();
            message[1][3].As.DateTimes.Count.Should().Be(2);
        }

        [Test]
        public void Codec_CanSetDateTimes()
        {
            var message = Message.Parse(Any.Message());
            var dateTimes = new DateTimeOffset?[] { DateTimeOffset.Now, DateTimeOffset.Now.AddDays(-1) };
            message[1][3].As.DateTimes.Items = dateTimes;
            message[1][3].RawValues.Should().BeEquivalentTo(dateTimes.Select(DateTimeConverter.ConvertFromDateTime));
        }

        [Test]
        public void Codec_CanGetDecimal()
        {
            var message = Message.Parse(Any.Message());
            message[1][3].RawValue = Any.Decimal();
            message[1][3].As.Decimal.Should().HaveValue();
        }

        [Test]
        public void Codec_CanSetDecimal()
        {
            var input = NumberConverter.ConvertToDecimal(Any.Decimal());
            var message = Message.Parse(Any.Message());
            message[1][3].As.Decimal = input;
            message[1][3].RawValue.Should().Be(input.ToString());
        }
        
        [Test]
        public void Codec_CanGetDecimals()
        {
            var message = Message.Parse(Any.Message());
            var values = Enumerable.Range(0, 3).Select(i => Any.Decimal()).ToList();
            message[1][3].RawValues = values;
            message[1][3].As.Decimals.Should().BeEquivalentTo(values.Select(decimal.Parse));
        }
        
        [Test]
        public void Codec_CanGetInteger()
        {
            var message = Message.Parse(Any.Message());
            message[1][3].RawValue = Any.Number().ToString();
            message[1][3].As.Integer.Should().HaveValue();
        }

        [Test]
        public void Codec_CanSetInteger()
        {
            var input = Any.Number();
            var message = Message.Parse(Any.Message());
            message[1][3].As.Integer = input;
            message[1][3].RawValue.Should().Be(input.ToString());
        }
        
        [Test]
        public void Codec_CanGetIntegers()
        {
            var message = Message.Parse(Any.Message());
            var values = Enumerable.Range(0, 3).Select(i => Any.Number()).ToList();
            message[1][3].RawValues = values.Select(v => v.ToString());
            message[1][3].As.Integers.Should().BeEquivalentTo(values);
        }
    }
}
