using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Test.Testing;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Core
{
    [TestFixture]
    public class ElementOperationsUnitTests : CoreTestFixture
    {
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void HasEncodingCharacters_ReturnsTrue_OnEncodingFields(int fieldIndex)
        {
            var element = Message.Build(Any.Message());
            ElementOperations.HasEncodingCharacters(element[1][fieldIndex]).Should().BeTrue();
        }

        [Test]
        public void HasEncodingCharacters_ReturnsFalse_OnNonEncodingFields()
        {
            var element = Message.Build(Any.Message());
            foreach (var field in element[1].Fields.Skip(3)) //skip type, msh1, msh2
            {
                ElementOperations.HasEncodingCharacters(field).Should().BeFalse();
            }
        }

        [Test]
        public void HasEncodingCharacters_ReturnsFalse_OnSegmentContainingFieldsWithEncodingCharacters()
        {
            var element = Message.Build(Any.Message());
            ElementOperations.HasEncodingCharacters(element[1]).Should().BeFalse();
        }

        [Test]
        [TestCase(null, Result = "*")]
        [TestCase(new[] { 1 }, Result = "MSH1")]
        [TestCase(new[] { 1, 3 }, Result = "MSH1.3")]
        [TestCase(new[] { 1, 3, 1 }, Result = "MSH1.3.1")]
        [TestCase(new[] { 1, 3, 1, 1 }, Result = "MSH1.3.1.1")]
        [TestCase(new[] { 1, 3, 1, 1, 1 }, Result = "MSH1.3.1.1.1")]
        public string GetKey_ReturnsKey(int[] descendants)
        {
            IElement element = Message.Build(Any.Message());
            element = (descendants ?? new int[]{}).Aggregate(element, (current, index) => current[index]);
            return ElementOperations.GetKey(element);
        }
    }
}
