using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Native;

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

        [TestMethod]
        public void Field_CanGetRepetitionsByIndexer()
        {
            var repetition = Message.Create(ExampleMessages.Standard)[8][13][2];
            Assert.AreEqual(@"^ORN^CP", repetition.Value);
        }

        [TestMethod]
        public void Field_CanDeleteRepetition()
        {
            var message = Message.Create("MSH|^~\\&|\rTST|123~456|789~012");
            var field = message[2][1];
            field.Delete(1);
            Assert.AreEqual("MSH|^~\\&|\rTST|456|789~012", message.Value, @"Message was modified unexpectedly.");
        }

        [TestMethod]
        public void Field_WillPointToCorrectValue_WhenOtherFieldChanges()
        {
            var message = Message.Create(String.Format(@"MSH|^~\&|{0}|{1}", Randomized.String(), Randomized.String()));
            var msh3 = message[1][3];
            var msh4 = message[1][4];
            var newMsh3Value = Randomized.String();
            var newMsh4Value = Randomized.String();

            message.Value = String.Format(@"MSH|^~\&|{0}|{1}", newMsh3Value, newMsh4Value);
            Assert.AreEqual(newMsh3Value, msh3.Value, @"MSH-3 was not the expected value after changing MSH.");
            Assert.AreEqual(newMsh4Value, msh4.Value, @"MSH-4 was not the expected value after changing MSH.");
        }

        [TestMethod]
        public void Field_WithNoSignificantDescendants_ShouldNotClaimToHaveSignificantDescendants()
        {
            var message = Message.Create();
            Assert.IsFalse(message[1][3].HasSignificantDescendants,
                @"Element claims to have descendants when it should not.");
        }

        [TestMethod]
        public void Field_WillHaveValuesInterpretedAsNull()
        {
            var message = Message.Create();
            message[1][3].Value = "\"\"";
            Assert.AreEqual(message[1][3].Value, null, @"Value of two double quotes was not interpreted as null.");
            Assert.IsTrue(message[1][3].Exists, @"Explicitly set null value must appear to exist.");
        }
    }
}
