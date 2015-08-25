using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    static public class Race
    {
        static public void ExecutionTime(Action nl7Action, Action otherAction)
        {
            Debug.WriteLine("Racing NL7...");

            long nl7Time = Measure.ExecutionTime(nl7Action, 100);
            long otherTime;

            try
            {
                Debug.WriteLine("Racing opponent...");
                otherTime = Measure.ExecutionTime(otherAction, 100);
            }
            catch (Exception)
            {
                Debug.WriteLine("Other action crashed.");
                otherTime = long.MaxValue;
            }

            Assert.IsTrue(nl7Time < otherTime);            
        }

        static public void ExecutionTime(Action<string> nl7Action, Action<string> otherAction, string data)
        {
            Debug.WriteLine("Racing NL7...");

            long nl7Time = Measure.ExecutionTime(nl7Action, data, 100);
            long otherTime;

            try
            {
                Debug.WriteLine("Racing opponent...");
                otherTime = Measure.ExecutionTime(otherAction, data, 100);
            }
            catch (Exception)
            {
                Debug.WriteLine("Other action crashed.");
                otherTime = long.MaxValue;
            }

            Assert.IsTrue(nl7Time < otherTime);
        }
    }
}
