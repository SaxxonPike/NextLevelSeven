using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ElementExtensionTests : CoreTestFixture
    {
        // Insert Element After

        static void ElementExtensions_CanInsertElementAfter(IElement element)
        {
            var message = element;
            var modifiedMessage = element.Clone();
            modifiedMessage.InsertAfter(2, message[2]);
            Assert.AreEqual(modifiedMessage[3].Value, message[2].Value, "Element insertion (after) failed: duplication didn't work.");
            Assert.AreEqual(modifiedMessage[4].Value, message[3].Value,
                "Element insertion (after) failed: shifted segments weren't in order.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanInsertElementAfter()
        {
            ElementExtensions_CanInsertElementAfter(Message.Build(ExampleMessages.Standard));
        }

        [TestMethod]
        public void ElementExtensions_Native_CanInsertElementAfter()
        {
            ElementExtensions_CanInsertElementAfter(Message.Create(ExampleMessages.Standard));
        }

        // Insert String After

        static void ElementExtensions_CanInsertStringAfter(IElement element)
        {
            const string testSegment = @"TST|0|";

            var message = element;
            var modifiedMessage = element.Clone();
            modifiedMessage[1].InsertAfter(testSegment);
            Assert.AreEqual(modifiedMessage[2].Value, testSegment,
                "Text insertion (after) failed: expected string didn't appear in modified message.");
            Assert.AreEqual(modifiedMessage[3].Value, message[2].Value,
                "Text insertion (after) failed: shifted segments weren't in order.");
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanInsertStringAfter()
        {
            ElementExtensions_CanInsertStringAfter(Message.Build(ExampleMessages.Standard));
        }

        [TestMethod]
        public void ElementExtensions_Native_CanInsertStringAfter()
        {
            ElementExtensions_CanInsertStringAfter(Message.Create(ExampleMessages.Standard));
        }

    }
}