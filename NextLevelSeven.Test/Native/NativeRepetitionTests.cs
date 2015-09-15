using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

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
            var count = repetition.ValueCount;
            var id = Randomized.String();
            repetition[count + 1].Value = id;
            Assert.AreEqual(count + 1, repetition.ValueCount,
                @"Number of elements after appending at the end of a repetition is incorrect.");
        }

        [TestMethod]
        public void Repetition_CanGetComponentsByIndexer()
        {
            var component = Message.Create(ExampleMessages.Standard)[8][13][2][2];
            Assert.AreEqual(@"ORN", component.Value);
        }

        [TestMethod]
        public void Repetition_CanDeleteComponent()
        {
            var message = Message.Create("MSH|^~\\&|\rTST|123^456~789^012");
            var component = message[2][1][2];
            component.Delete(1);
            Assert.AreEqual("MSH|^~\\&|\rTST|123^456~012", message.Value, @"Message was modified unexpectedly.");
        }

        [TestMethod]
        public void Repetition_CanWriteStringValue()
        {
            var repetition = Message.Create(ExampleMessages.Standard)[1][3][1];
            var value = Randomized.String();
            repetition.Value = value;
            Assert.AreEqual(value, repetition.Value, "Value mismatch after write.");
        }

        [TestMethod]
        public void Repetition_CanWriteNullValue()
        {
            var repetition = Message.Create(ExampleMessages.Standard)[1][3][1];
            var value = Randomized.String();
            repetition.Value = value;
            repetition.Value = null;
            Assert.IsNull(repetition.Value, "Value mismatch after write.");
        }
    }
}