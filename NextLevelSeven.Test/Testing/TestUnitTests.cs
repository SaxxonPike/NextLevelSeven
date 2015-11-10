using System;
using System.Diagnostics;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NextLevelSeven.Test.Testing
{
    [TestFixture]
    public class TestUnitTests
    {
        [Test]
        public void Measure_ExecutionTime_InvokesAction()
        {
            var invoked = false;
            Action action = () => { invoked = true; };
            Debug.Write(Measure.ExecutionTime(action));
            Assert.IsTrue(invoked);
        }

        [Test]
        public void Measure_ExecutionTime_InvokesActionWithIterations()
        {
            var invokedTimes = 0;
            Action action = () => { invokedTimes++; };
            Debug.Write(Measure.ExecutionTime(action, 5));
            Assert.AreEqual(5, invokedTimes);
        }

        [Test]
        public void Measure_ExecutionTime_InvokesActionWithIterationsAndData()
        {
            var data = Any.String();
            var invokedTimes = 0;
            Action<string> action = d =>
            {
                Assert.AreEqual(d, data);
                invokedTimes++;
            };
            Debug.Write(Measure.ExecutionTime(action, data, 5));
            Assert.AreEqual(5, invokedTimes);
        }
    }
}
