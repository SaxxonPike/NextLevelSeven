using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class NativeRepetitionTests : NativeTestFixture
    {
        [TestMethod]
        public void Repetition_CanBeCloned()
        {
            var repetition = Message.Create(ExampleMessages.Standard)[1][3][1];
            var clone = repetition.Clone();
            Assert.AreNotSame(repetition, clone, "Cloned repetition is the same referenced object.");
            Assert.AreEqual(repetition.Value, clone.Value, "Cloned repetition has different contents.");
        }

        [TestMethod]
        public void Repetition_CanAddDescendantsAtEnd()
        {
            var repetition = Message.Create(ExampleMessages.Standard)[2][3][4];
            var count = repetition.DescendantCount;
            var id = Randomized.String();
            repetition[count + 1].Value = id;
            Assert.AreEqual(count + 1, repetition.DescendantCount,
                @"Number of elements after appending at the end of a repetition is incorrect.");
        }

    }
}
