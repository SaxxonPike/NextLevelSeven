using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class NativeComponentTests : NativeTestFixture
    {
        [TestMethod]
        public void Component_CanBeCloned()
        {
            var component = Message.Create(ExampleMessages.Standard)[1][3][1][1];
            var clone = component.Clone();
            Assert.AreNotSame(component, clone, "Cloned component is the same referenced object.");
            Assert.AreEqual(component.Value, clone.Value, "Cloned component has different contents.");
        }

        [TestMethod]
        public void Component_CanAddDescendantsAtEnd()
        {
            var component = Message.Create(ExampleMessages.Standard)[2][3][4][1];
            var count = component.DescendantCount;
            var id = Randomized.String();
            component[count + 1].Value = id;
            Assert.AreEqual(count + 1, component.DescendantCount,
                @"Number of elements after appending at the end of a component is incorrect.");
        }
    }
}
