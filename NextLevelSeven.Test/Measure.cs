using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    public static class Measure
    {
        public static int ExecutionIterations(Action action, long timeLimit)
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

        public static long ExecutionTime(Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            Debug.WriteLine("Executed in {0}ms", sw.ElapsedMilliseconds);
            return sw.ElapsedMilliseconds;
        }

        public static long ExecutionTime(Action action, int iterations)
        {
            Debug.WriteLine("Measuring {0} iterations.", iterations);
            return ExecutionTime(() =>
            {
                while (iterations > 0)
                {
                    action();
                    iterations--;
                }
            });
        }

        public static long ExecutionTime(Action<string> action, string data)
        {
            var sw = new Stopwatch();
            sw.Start();
            action(data);
            sw.Stop();
            Debug.WriteLine("Executed in {0}ms", sw.ElapsedMilliseconds);
            return sw.ElapsedMilliseconds;
        }

        public static long ExecutionTime(Action<string> action, string data, int iterations)
        {
            Debug.WriteLine("Measuring {0} iterations.", iterations);
            return ExecutionTime(() =>
            {
                while (iterations > 0)
                {
                    action(data);
                    iterations--;
                }
            });
        }

        public static long WaitTime(Func<bool> condition, long timeout, string message)
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