using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Parsing.Dividers;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Parsing.Dividers
{
    [TestClass]
    public class DescendantStringDividerUnitTests : DividersUnitTestFixture
    {
        private StringDivider _ancestorDivider;
        private string _ancestorData;
        private string _ancestorDelimiter;

        [TestInitialize]
        public void InitializeAncestor()
        {
            _ancestorDelimiter = "\u0001";
            _ancestorData = Mock.DelimitedString(_ancestorDelimiter);
            _ancestorDivider = new RootStringDivider(_ancestorData, _ancestorDelimiter[0]);
        }

        [TestMethod]
        public void HasCorrectNumberOfDivisions()
        {
            var data = Mock.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) {Value = data};
            Assert.AreEqual(4, divider.Count);
        }

        [TestMethod]
        public void HasCorrectDivisions()
        {
            var data = Mock.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = data };
            AssertEnumerable.AreEqual(data.Split(':'), divider.Values);
        }

        [TestMethod]
        public void IsIndexable()
        {
            var data = Mock.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = data };
            for (var i = 0; i < 4; i++)
            {
                AssertEnumerable.AreEqual(data.Split(':')[i], divider[i]);
            }
        }

        [TestMethod]
        public void IsIndexablePastEnd()
        {
            var data = Mock.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = data };
            Assert.AreEqual(string.Empty, divider[5]);
        }

        [TestMethod]
        public void HasBaseValue()
        {
            var data = Mock.DelimitedString(":", 4);
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = data };
            Assert.AreSame(_ancestorDivider.BaseValue, divider.BaseValue);
        }

        [TestMethod]
        public void CanBeNull()
        {
            var divider = new DescendantStringDivider(_ancestorDivider, ':', 0) { Value = null };
            Assert.IsTrue(divider.IsNull);
            divider.Value = Mock.String();
            Assert.IsFalse(divider.IsNull);
        }

        [TestMethod]
        public void StoresDelimiter()
        {
            var delimiter = Mock.Symbol()[0];
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter, 0) { Value = Mock.String() };
            Assert.AreEqual(delimiter, divider.Delimiter);
        }

        [TestMethod]
        public void StoresValue()
        {
            var delimiter = Mock.Symbol()[0];
            var dataBefore = Mock.String();
            var dataAfter = Mock.String();
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter, 0) { Value = dataBefore };
            Assert.AreEqual(dataBefore, divider.Value);
            divider.Value = dataAfter;
            Assert.AreEqual(dataAfter, divider.Value);
        }

        [TestMethod]
        public void StoresValues()
        {
            var delimiter = Mock.Symbol();
            var dataBefore = new[] { Mock.String(), Mock.String() };
            var dataAfter = new[] { Mock.String(), Mock.String() };
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = string.Join(delimiter, dataBefore) };
            AssertEnumerable.AreEqual(dataBefore, divider.Values);
            divider.Values = dataAfter;
            AssertEnumerable.AreEqual(dataAfter, divider.Values);
        }

        [TestMethod]
        public void GetsSubdivision()
        {
            var delimiter = Mock.Symbol();
            var value0 = Mock.String();
            var value1 = Mock.String();
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

        [TestMethod]
        public void PadsDivider()
        {
            var delimiter = Mock.Symbol();
            var data = Mock.String();
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            var divisions = new List<StringDivision> { new StringDivision(0, data.Length) };
            divider.Pad(delimiter[0], 2, 0, data.Length, divisions);
            Assert.AreEqual(string.Join(delimiter, data, string.Empty, string.Empty), divider.Value);
        }

        [TestMethod]
        public void PadsAncestorSubDivider()
        {
            var delimiter = Mock.Symbol();
            var data = Mock.String();
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 9) { Value = data };
            divider.PadSubDivider(2);
            Assert.AreEqual(10, _ancestorDivider.Count);
        }

        [TestMethod]
        public void Replaces()
        {
            var delimiter = Mock.Symbol()[0];
            var data = Mock.String();
            var insertedData = Mock.String();
            var expectedData = string.Concat(data.Substring(0, 3), insertedData, data.Substring(6));
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter, 0) { Value = data };
            divider.Replace(3, 3, insertedData.ToCharArray());
            Assert.AreEqual(expectedData, divider.Value);
        }

        [TestMethod]
        public void DeletesAtBeginning()
        {
            var delimiter = Mock.Symbol();
            var data0 = Mock.String();
            var data1 = Mock.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Delete(0);
            Assert.AreEqual(data1, divider.Value);
        }

        [TestMethod]
        public void DeletesAtMiddle()
        {
            var delimiter = Mock.Symbol();
            var data0 = Mock.String();
            var data1 = Mock.String();
            var data2 = Mock.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Delete(1);
            Assert.AreEqual(string.Join(delimiter, data0, data2), divider.Value);
        }

        [TestMethod]
        public void DeletesAtEnd()
        {
            var delimiter = Mock.Symbol();
            var data0 = Mock.String();
            var data1 = Mock.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Delete(1);
            Assert.AreEqual(data0, divider.Value);
        }

        [TestMethod]
        public void MovesToBeginning()
        {
            var delimiter = Mock.Symbol();
            var data0 = Mock.String();
            var data1 = Mock.String();
            var data2 = Mock.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Move(2, 0);
            Assert.AreEqual(string.Join(delimiter, data2, data0, data1), divider.Value);
        }

        [TestMethod]
        public void MovesToEnd()
        {
            var delimiter = Mock.Symbol();
            var data0 = Mock.String();
            var data1 = Mock.String();
            var data2 = Mock.String();
            var data = string.Join(delimiter, data0, data1, data2);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Move(1, 2);
            Assert.AreEqual(string.Join(delimiter, data0, data2, data1), divider.Value);
        }

        [TestMethod]
        public void InsertsAtBeginning()
        {
            var delimiter = Mock.Symbol();
            var data0 = Mock.String();
            var data1 = Mock.String();
            var data2 = Mock.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Insert(0, data2);
            Assert.AreEqual(string.Join(delimiter, data2, data0, data1), divider.Value);
        }

        [TestMethod]
        public void InsertsAtMiddle()
        {
            var delimiter = Mock.Symbol();
            var data0 = Mock.String();
            var data1 = Mock.String();
            var data2 = Mock.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Insert(1, data2);
            Assert.AreEqual(string.Join(delimiter, data0, data2, data1), divider.Value);
        }

        [TestMethod]
        public void InsertsAtEnd()
        {
            var delimiter = Mock.Symbol();
            var data0 = Mock.String();
            var data1 = Mock.String();
            var data2 = Mock.String();
            var data = string.Join(delimiter, data0, data1);
            var divider = new DescendantStringDivider(_ancestorDivider, delimiter[0], 0) { Value = data };
            divider.Insert(2, data2);
            Assert.AreEqual(string.Join(delimiter, data0, data1, data2), divider.Value);
        }

    }
}
