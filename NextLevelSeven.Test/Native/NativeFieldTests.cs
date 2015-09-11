using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class NativeFieldTests : NativeTestFixture
    {
        [TestMethod]
        public void Field_CanBeCloned()
        {
            var field = Message.Create(ExampleMessages.Standard)[1][3];
            var clone = field.Clone();
            Assert.AreNotSame(field, clone, "Cloned field is the same referenced object.");
            Assert.AreEqual(field.Value, clone.Value, "Cloned field has different contents.");
        }

        [TestMethod]
        public void Field_CanAddDescendantsAtEnd()
        {
            var field = Message.Create(ExampleMessages.Standard)[2][3];
            var count = field.DescendantCount;
            var id = Randomized.String();
            field[count + 1].Value = id;
            Assert.AreEqual(count + 1, field.DescendantCount,
                @"Number of elements after appending at the end of a field is incorrect.");
        }
    }
}
