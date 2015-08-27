using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NextLevelSeven.Core;
using NextLevelSeven.Transformation;

namespace NextLevelSeven.Test.Transformation
{
    [TestClass]
    public class ElementTransformTests
    {
        [TestMethod]
        public void ElementTransform_CanTransformValue()
        {
            var value = Randomized.String();
            var factory = new TestNameElementTransformFactory(value);
            var message = new Message();
            var transform = factory.CreateTransform(message);
            Assert.AreEqual(value, transform.Value, @"Transform didn't change the message value.");
            Assert.AreEqual(transform[1].Value, message[1].Value, @"Test transform should only affect Value property.");
        }
    }
}
