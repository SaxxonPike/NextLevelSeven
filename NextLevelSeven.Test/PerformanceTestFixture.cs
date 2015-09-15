using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    [TestClass]
    public class PerformanceTestFixture
    {
        private long _frequency;
        private Stopwatch _stopwatch;

        [TestInitialize]
        public void Fixture_Initialize()
        {
            _frequency = Stopwatch.Frequency;
            Debug.WriteLine("---> Timed test started.");
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        [TestCleanup]
        public void Fixture_Cleanup()
        {
            _stopwatch.Stop();
            var ticks = _stopwatch.ElapsedTicks;
            var milliseconds = _stopwatch.ElapsedMilliseconds;
            Debug.WriteLine("---> {0}ms ({1}\x00B5s)", milliseconds, (ticks*1000000)/_frequency);
        }
    }
}