using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test.Testing
{
    [TestClass]
    public class TestUnitTests
    {
        [TestMethod]
        public void Measure_ExecutionTime_InvokesAction()
        {
            var invoked = false;
            Action action = () => { invoked = true; };
            Debug.Write(Measure.ExecutionTime(action));
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void Measure_ExecutionTime_InvokesActionWithIterations()
        {
            var invokedTimes = 0;
            Action action = () => { invokedTimes++; };
            Debug.Write(Measure.ExecutionTime(action, 5));
            Assert.AreEqual(5, invokedTimes);
        }

        [TestMethod]
        public void Measure_ExecutionTime_InvokesActionWithIterationsAndData()
        {
            var data = Mock.String();
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
