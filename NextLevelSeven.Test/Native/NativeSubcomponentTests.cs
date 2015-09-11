using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class NativeSubcomponentTests : NativeTestFixture
    {
        [TestMethod]
        public void Subcomponent_CanBeCloned()
        {
            var subcomponent = Message.Create(ExampleMessages.Standard)[1][3][1][1][1];
            var clone = subcomponent.Clone();
            Assert.AreNotSame(subcomponent, clone, "Cloned subcomponent is the same referenced object.");
            Assert.AreEqual(subcomponent.Value, clone.Value, "Cloned subcomponent has different contents.");
        }

        [TestMethod]
        public void Subcomponent_CanAddDescendantsAtEnd()
        {
            var subcomponent = Message.Create(ExampleMessages.Standard)[2][3][4][1];
            var count = subcomponent.DescendantCount;
            var id = Randomized.String();
            subcomponent[count + 1].Value = id;
            Assert.AreEqual(count + 1, subcomponent.DescendantCount,
                @"Number of elements after appending at the end of a subcomponent is incorrect.");
        }
    }
}
