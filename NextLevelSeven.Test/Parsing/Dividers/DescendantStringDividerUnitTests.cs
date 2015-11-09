using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Parsing.Dividers;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Parsing.Dividers
{
    [TestFixture]
    public class DescendantStringDividerUnitTests : DividersUnitTestFixture
    {
        private StringDivider _ancestorDivider;
        private string _ancestorData;
        private string _ancestorDelimiter;

        [TestInitialize]
        public void InitializeAncestor()
        {
            _ancestorDelimiter = "\u0001";
            _ancestorData = MockFactory.DelimitedString(_ancestorDelimiter);
            _ancestorDivider = new RootStringDivider(_ancestorData, _ancestorDelimiter[0]);
        }

        [Test]
        public void HasCorrectNumberOfDivisions()
        {
            var data = MockFactory.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) {Value = data};
            Assert.AreEqual(4, divider.Count);
        }

        [Test]
        public void HasCorrectDivisions()
        {
            var data = MockFactory.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = data };
            AssertEnumerable.AreEqual(data.Split(':'), divider.Values);
        }

        [Test]
        public void IsIndexable()
        {
            var data = MockFactory.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = data };
            for (var i = 0; i < 4; i++)
            {
                AssertEnumerable.AreEqual(data.Split(':')[i], divider[i]);
            }
        }

        [Test]
        public void IsIndexablePastEnd()
        {
            var data = MockFactory.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = data };
            Assert.AreEqual(string.Empty, divider[5]);
        }

        [Test]
        public void HasBaseValue()
        {
            var data = MockFactory.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = data };
            Assert.AreSame(_ancestorDivider.BaseValue, divider.BaseValue);
        }

        [Test]
        public void CanBeNull()
        {
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = null };
            Assert.IsTrue(divider.IsNull);
            divider.Value = MockFactory.String();
            Assert.IsFalse(divider.IsNull);
        }

        [Test]
        public void StoresDelimiter()
        {
            var delimiter = MockFactory.Symbol()[0];
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter, 0) { Value = MockFactory.String() };
            Assert.AreEqual(delimiter, divider.Delimiter);
        }

        [Test]
        public void StoresValue()
        {
            var delimiter = MockFactory.Symbol()[0];
            var dataBefore = MockFactory.String();
            var dataAfter = MockFactory.String();
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter, 0) { Value = dataBefore };
            Assert.AreEqual(dataBefore, divider.Value);
            divider.Value = dataAfter;
            Assert.AreEqual(dataAfter, divider.Value);
        }

        [Test]
        public void StoresValues()
        {
            var delimiter = MockFactory.Symbol();
            var dataBefore = new[] { MockFactory.String(), MockFactory.String() };
            var dataAfter = new[] { MockFactory.String(), MockFactory.String() };
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = string.Join(delimiter, dataBefore) };
            AssertEnumerable.AreEqual(dataBefore, divider.Values);
            divider.Values = dataAfter;
            AssertEnumerable.AreEqual(dataAfter, divider.Values);
        }

        [Test]
        public void GetsSubdivision()
        {
            var delimiter = MockFactory.Symbol();
            var value0 = MockFactory.String();
            var value1 = MockFactory.String();
            var value = string.Join(delimiter, value0, value1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = value };
            var delimiterOffset = value.IndexOf(delimiter, StringComparison.Ordinal);

            var division0 = divider.GetSubDivision(0);
            Assert.IsTrue(division0.Valid);
            Assert.AreEqual(0, division0.Offset);
            Assert.AreEqual(value0.Length, division0.Length);

            var division1 = divider.GetSubDivision(1);
            Assert.IsTrue(division1.Valid);
            Assert.AreEqual(delimiterOffset + 1, division1.Offset);
            Assert.AreEqual(value1.Length, division1.Length);
        }

        [Test]
        public void PadsDivider()
        {
            var delimiter = MockFactory.Symbol();
            var data = MockFactory.String();
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            var divisions = new List<StringDivision> { new StringDivision(0, data.Length) };
            divider.Pad(delimiter[0], 2, 0, data.Length, divisions);
            Assert.AreEqual(string.Join(delimiter, data, string.Empty, string.Empty), divider.Value);
        }

        [Test]
        public void PadsAncestorSubDivider()
        {
            var delimiter = MockFactory.Symbol();
            var data = MockFactory.String();
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 9) { Value = data };
            divider.PadSubDivider(2);
            Assert.AreEqual(10, _ancestorDivider.Count);
        }

        [Test]
        public void Replaces()
        {
            var delimiter = MockFactory.Symbol()[0];
            var data = MockFactory.String();
            var insertedData = MockFactory.String();
            var expectedData = string.Concat(data.Substring(0, 3), insertedData, data.Substring(6));
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter, 0) { Value = data };
            divider.Replace(3, 3, insertedData.ToCharArray());
            Assert.AreEqual(expectedData, divider.Value);
        }

        [Test]
        public void DeletesAtBeginning()
        {
            var delimiter = MockFactory.Symbol();
            var data0 = MockFactory.String();
            var data1 = MockFactory.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Delete(0);
            Assert.AreEqual(data1, divider.Value);
        }

        [Test]
        public void DeletesAtMiddle()
        {
            var delimiter = MockFactory.Symbol();
            var data0 = MockFactory.String();
            var data1 = MockFactory.String();
            var data2 = MockFactory.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Delete(1);
            Assert.AreEqual(string.Join(delimiter, data0, data2), divider.Value);
        }

        [Test]
        public void DeletesAtEnd()
        {
            var delimiter = MockFactory.Symbol();
            var data0 = MockFactory.String();
            var data1 = MockFactory.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Delete(1);
            Assert.AreEqual(data0, divider.Value);
        }

        [Test]
        public void MovesToBeginning()
        {
            var delimiter = MockFactory.Symbol();
            var data0 = MockFactory.String();
            var data1 = MockFactory.String();
            var data2 = MockFactory.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Move(2, 0);
            Assert.AreEqual(string.Join(delimiter, data2, data0, data1), divider.Value);
        }

        [Test]
        public void MovesToEnd()
        {
            var delimiter = MockFactory.Symbol();
            var data0 = MockFactory.String();
            var data1 = MockFactory.String();
            var data2 = MockFactory.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Move(1, 2);
            Assert.AreEqual(string.Join(delimiter, data0, data2, data1), divider.Value);
        }

        [Test]
        public void InsertsAtBeginning()
        {
            var delimiter = MockFactory.Symbol();
            var data0 = MockFactory.String();
            var data1 = MockFactory.String();
            var data2 = MockFactory.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Insert(0, data2);
            Assert.AreEqual(string.Join(delimiter, data2, data0, data1), divider.Value);
        }

        [Test]
        public void InsertsAtMiddle()
        {
            var delimiter = MockFactory.Symbol();
            var data0 = MockFactory.String();
            var data1 = MockFactory.String();
            var data2 = MockFactory.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Insert(1, data2);
            Assert.AreEqual(string.Join(delimiter, data0, data2, data1), divider.Value);
        }

        [Test]
        public void InsertsAtEnd()
        {
            var delimiter = MockFactory.Symbol();
            var data0 = MockFactory.String();
            var data1 = MockFactory.String();
            var data2 = MockFactory.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Insert(2, data2);
            Assert.AreEqual(string.Join(delimiter, data0, data1, data2), divider.Value);
        }

    }
}
