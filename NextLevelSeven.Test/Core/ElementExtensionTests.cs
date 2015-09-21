using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Core
{
    [TestClass]
    public class ElementExtensionTests : CoreTestFixture
    {
        // Delete Element

        static void ElementExtensions_CanDelete(IElement element)
        {
            var modifiedElement = element.Clone();
            modifiedElement.Delete(2);
            Assert.AreEqual(modifiedElement[2].Value, element[3].Value);
            Assert.AreEqual(modifiedElement[3].Value, element[4].Value);
        }

        [TestMethod]
        public void ElementExtensions_Builder_CanDelete()
        {
            ElementExtensions_CanDelete(Message.Build(ExampleMessages.Standard));
        }

        [TestMethod]
        public void ElementExtensions_Native_CanDelete()
        {
            ElementExtensions_CanDelete(Message.Create(ExampleMessages.Standard));
        }

        // Insert Element After

        static void ElementExtensions_CanInsertElementAfter(IElement element)
        {
            var modifiedElement = element.Clone();
            modifiedElement.InsertAfter(2, element[2]);
            Assert.AreEqual(modifiedElement[3].Value, element[2].Value, "Element insertion (after) failed: duplication didn't work.");
            Assert.AreEqual(modifiedElement[4].Value, element[3].Value,
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

            var modifiedElement = element.Clone();
            modifiedElement[1].InsertAfter(testSegment);
            Assert.AreEqual(modifiedElement[2].Value, testSegment,
                "Text insertion (after) failed: expected string didn't appear in modified message.");
            Assert.AreEqual(modifiedElement[3].Value, element[2].Value,
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