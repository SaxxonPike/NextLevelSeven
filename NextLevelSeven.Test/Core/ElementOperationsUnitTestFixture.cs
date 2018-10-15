using System.Linq;
using FluentAssertions;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class ElementOperationsUnitTestFixture : CoreBaseTestFixture
    {
        [Test]
        [TestCase(0, ExpectedResult = false)]
        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = true)]
        [TestCase(3, ExpectedResult = false)]
        public bool IsEncodingCharacterField_ReturnsTrue_OnlyOnEncodingFields(int fieldIndex)
        {
            var element = Message.Build(Any.Message());
            return ElementOperations.IsEncodingCharacterField(element[1][fieldIndex]);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void IsEncodingCharacterField_ReturnsFalse_OnSegment(int index)
        {
            var element = Message.Build(Any.Message());
            ElementOperations.IsEncodingCharacterField(element[index]).Should().BeFalse();
        }

        [Test]
        public void IsEncodingCharacterField_ReturnsFalse_OnRepetition()
        {
            var element = Message.Build(Any.Message());
            ElementOperations.IsEncodingCharacterField(element[1][3][1]).Should().BeFalse();
        }

        [Test]
        public void IsEncodingCharacterField_ReturnsFalse_OnComponent()
        {
            var element = Message.Build(Any.Message());
            ElementOperations.IsEncodingCharacterField(element[1][3][1][1]).Should().BeFalse();
        }

        [Test]
        public void IsEncodingCharacterField_ReturnsFalse_OnSubcomponent()
        {
            var element = Message.Build(Any.Message());
            ElementOperations.IsEncodingCharacterField(element[1][3][1][1][1]).Should().BeFalse();
        }

        [Test]
        [TestCase(null, ExpectedResult = "*")]
        [TestCase(new[] { 1 }, ExpectedResult = "MSH1")]
        [TestCase(new[] { 1, 3 }, ExpectedResult = "MSH1.3")]
        [TestCase(new[] { 1, 3, 1 }, ExpectedResult = "MSH1.3.1")]
        [TestCase(new[] { 1, 3, 1, 1 }, ExpectedResult = "MSH1.3.1.1")]
        [TestCase(new[] { 1, 3, 1, 1, 1 }, ExpectedResult = "MSH1.3.1.1.1")]
        public string GetKey_ReturnsKey(int[] descendants)
        {
            IElement element = Message.Build(Any.Message());
            element = (descendants ?? new int[]{}).Aggregate(element, (current, index) => current[index]);
            return ElementOperations.GetKey(element);
        }
    }
}
