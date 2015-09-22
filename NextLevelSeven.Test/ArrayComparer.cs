using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    static public class ArrayComparer
    {
        static public void AssertCompare(params Array[] arrays)
        {
            if (arrays.Length <= 0)
            {
                return;
            }

            Assert.AreEqual(1, arrays.GroupBy(a => a.GetType()).Count(), "Arrays are not the same type.");
            Assert.AreEqual(1, arrays.GroupBy(a => a.Length).Count(), "Arrays are not the same length.");

            var referenceArray = arrays.First();
            var length = referenceArray.Length;

            foreach (var array in arrays.Skip(1))
            {
                for (var i = 0; i < length; i++)
                {
                    var referenceValue = referenceArray.GetValue(i);
                    var observedValue = array.GetValue(i);
                    var referenceType = referenceValue.GetType();
                    var observedType = observedValue.GetType();

                    // verify types are identical
                    Assert.AreEqual(referenceType, observedType, string.Format(
                        "Value type mismatch at index {0} of {1}.\r\nReference:\r\n{2}\r\n\r\nObserved:\r\n{3}", i, length, referenceType, observedType));

                    // verify values are identical
                    Assert.IsTrue(referenceValue.Equals(observedValue), string.Format(
                        "Mismatch between arrays at index {0} of {1}.\r\nReference:\r\n{2}\r\n\r\nObserved:\r\n{3}", i, length, referenceValue, observedValue));
                }
            }
        }
    }
}
