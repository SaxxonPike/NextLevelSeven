using System;
using System.Collections.Generic;
using FluentAssertions;
using NextLevelSeven.Parsing.Dividers;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Parsing.Dividers
{
    [TestFixture]
    public class RootStringDividerUnitTestFixture : DividersBaseTestFixture
    {
        [Test]
        public void HasCorrectNumberOfDivisions()
        {
            var data = Any.DelimitedString(":", 4);
            var divider = new RootStringDivider(data, ':');
            divider.Count.Should().Be(4);
        }

        [Test]
        public void HasCorrectDivisions()
        {
            var data = Any.DelimitedString(":", 4);
            var divider = new RootStringDivider(data, ':');
            divider.Values.Should().Equal(data.Split(':'));
        }

        [Test]
        public void IsIndexable()
        {
            var data = Any.DelimitedString(":", 4);
            var divider = new RootStringDivider(data, ':');
            for (var i = 0; i < 4; i++)
            {
                divider[i].Should().Be(data.Split(':')[i]);
            }
        }

        [Test]
        public void IsIndexablePastEnd()
        {
            var data = Any.DelimitedString(":", 4);
            var divider = new RootStringDivider(data, ':');
            divider[5].Should().Be(string.Empty);
        }

        [Test]
        public void HasBaseValue()
        {
            var data = Any.DelimitedString(":", 4);
            var divider = new RootStringDivider(data, ':');
            divider.BaseValue.Length.Should().Be(data.Length);
        }

        [Test]
        public void CanBeNull()
        {
            var divider = new RootStringDivider(null, '\0');
            divider.IsNull.Should().BeTrue();
            divider.Value = Any.String();
            divider.IsNull.Should().BeFalse();
        }

        [Test]
        public void StoresDelimiter()
        {
            var delimiter = Any.Symbol()[0];
            var divider = new RootStringDivider(Any.String(), delimiter);
            divider.Delimiter.Should().Be(delimiter);
        }

        [Test]
        public void StoresValue()
        {
            var delimiter = Any.Symbol()[0];
            var dataBefore = Any.String();
            var dataAfter = Any.String();
            var divider = new RootStringDivider(dataBefore, delimiter);
            divider.Value.Should().Be(dataBefore);
            divider.Value = dataAfter;
            divider.Value.Should().Be(dataAfter);
        }

        [Test]
        public void StoresValues()
        {
            var delimiter = Any.Symbol();
            var dataBefore = new[] { Any.String(), Any.String() };
            var dataAfter = new[] { Any.String(), Any.String() };
            var divider = new RootStringDivider(string.Join(delimiter, dataBefore), delimiter[0]);
            divider.Values.Should().Equal(dataBefore);
            divider.Values = dataAfter;
            divider.Values.Should().Equal(dataAfter);
        }

        [Test]
        public void ChangesVersionWhenValueIsChanged()
        {
            var divider = new RootStringDivider(Any.String(), Any.Symbol()[0]);
            var oldVersion = divider.Version;
            divider.Value = Any.String();
            divider.Version.Should().NotBe(oldVersion);
        }

        [Test]
        public void GetsSubdivision()
        {
            var delimiter = Any.Symbol();
            var value0 = Any.String();
            var value1 = Any.String();
            var value = string.Join(delimiter, value0, value1);
            var divider = new RootStringDivider(value, delimiter[0]);
            var delimiterOffset = value.IndexOf(delimiter, StringComparison.Ordinal);

            var division0 = divider.GetSubDivision(0);
            division0.Valid.Should().BeTrue();
            division0.Offset.Should().Be(0);
            division0.Length.Should().Be(value0.Length);

            var division1 = divider.GetSubDivision(1);
            division1.Valid.Should().BeTrue();
            division1.Offset.Should().Be(delimiterOffset + 1);
            division1.Length.Should().Be(value1.Length);
        }

        [Test]
        public void PadsDivider()
        {
            var delimiter = Any.Symbol();
            var data = Any.String();
            var divider = new RootStringDivider(data, delimiter[0]);
            var divisions = new List<StringDivision>{ new StringDivision(0, data.Length) };
            divider.Pad(delimiter[0], 2, 0, data.Length, divisions);
            divider.Value.Should().Be(string.Join(delimiter, data, string.Empty, string.Empty));
        }

        [Test]
        public void PadsSubDivider()
        {
            var delimiter = Any.Symbol();
            var data = Any.String();
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.PadSubDivider(2);
            divider.Value.Should().Be(string.Join(delimiter, data, string.Empty, string.Empty));
        }

        [Test]
        public void Replaces()
        {
            var delimiter = Any.Symbol()[0];
            var data = Any.String();
            var insertedData = Any.String();
            var expectedData = string.Concat(data.Substring(0, 3), insertedData, data.Substring(6));
            var divider = new RootStringDivider(data, delimiter);
            divider.Replace(3, 3, insertedData.ToCharArray());
            divider.Value.Should().Be(expectedData);
        }

        [Test]
        public void DeletesAtBeginning()
        {
            var delimiter = Any.Symbol();
            var data0 = Any.String();
            var data1 = Any.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.Delete(0);
            divider.Value.Should().Be(data1);
        }

        [Test]
        public void DeletesAtMiddle()
        {
            var delimiter = Any.Symbol();
            var data0 = Any.String();
            var data1 = Any.String();
            var data2 = Any.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.Delete(1);
            divider.Value.Should().Be(string.Join(delimiter, data0, data2));
        }

        [Test]
        public void DeletesAtEnd()
        {
            var delimiter = Any.Symbol();
            var data0 = Any.String();
            var data1 = Any.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.Delete(1);
            divider.Value.Should().Be(data0);
        }

        [Test]
        public void MovesToBeginning()
        {
            var delimiter = Any.Symbol();
            var data0 = Any.String();
            var data1 = Any.String();
            var data2 = Any.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.Move(2, 0);
            divider.Value.Should().Be(string.Join(delimiter, data2, data0, data1));
        }

        [Test]
        public void MovesToEnd()
        {
            var delimiter = Any.Symbol();
            var data0 = Any.String();
            var data1 = Any.String();
            var data2 = Any.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.Move(1, 2);
            divider.Value.Should().Be(string.Join(delimiter, data0, data2, data1));
        }

        [Test]
        public void InsertsAtBeginning()
        {
            var delimiter = Any.Symbol();
            var data0 = Any.String();
            var data1 = Any.String();
            var data2 = Any.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.Insert(0, data2);
            divider.Value.Should().Be(string.Join(delimiter, data2, data0, data1));
        }

        [Test]
        public void InsertsAtMiddle()
        {
            var delimiter = Any.Symbol();
            var data0 = Any.String();
            var data1 = Any.String();
            var data2 = Any.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.Insert(1, data2);
            divider.Value.Should().Be(string.Join(delimiter, data0, data2, data1));
        }

        [Test]
        public void InsertsAtEnd()
        {
            var delimiter = Any.Symbol();
            var data0 = Any.String();
            var data1 = Any.String();
            var data2 = Any.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new RootStringDivider(data, delimiter[0]);
            divider.Insert(2, data2);
            divider.Value.Should().Be(string.Join(delimiter, data0, data1, data2));
        }

    }
}
