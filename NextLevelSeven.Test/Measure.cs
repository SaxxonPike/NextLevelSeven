using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NextLevelSeven.Test
{
    static public class Measure
    {
        static public int ExecutionIterations(Action action, long timeLimit)
        {
            var iterations = 0;
            var sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < timeLimit)
            {
                action();
                iterations++;
            }
            Debug.WriteLine("Iterations in {0}ms: {1}", timeLimit, iterations);
            return iterations;
        }

        static public long ExecutionTime(Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            Debug.WriteLine("Executed in {0}ms", sw.ElapsedMilliseconds);
            return sw.ElapsedMilliseconds;
        }

        static public long ExecutionTime(Action action, int iterations)
        {
            return ExecutionTime(() =>
            {
                while (iterations > 0)
                {
                    action();
                    iterations--;
                }
            });
        }

        static public long ExecutionTime(Action<string> action, string data)
        {
            var sw = new Stopwatch();
            sw.Start();
            action(data);
            sw.Stop();
            Debug.WriteLine("Executed in {0}ms", sw.ElapsedMilliseconds);
            return sw.ElapsedMilliseconds;            
        }

        static public long ExecutionTime(Action<string> action, string data, int iterations)
        {
            return ExecutionTime(() =>
            {
                while (iterations > 0)
                {
                    action(data);
                    iterations--;
                }
            });
        }

        static public long WaitTime(Func<bool> condition, long timeout, string message)
        {
            var sw = new Stopwatch();
            while (!condition())
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    sw.Stop();
                    Assert.Fail(message);
                }
                Thread.Sleep(1);
            }
            sw.Stop();
            Debug.WriteLine("Executed in {0}ms", sw.ElapsedMilliseconds);
            return sw.ElapsedMilliseconds;
        }
    }
}
