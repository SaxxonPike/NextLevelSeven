using System;
using System.Diagnostics;

namespace NextLevelSeven.Test.Testing
{
    public static class Measure
    {
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
    }
}