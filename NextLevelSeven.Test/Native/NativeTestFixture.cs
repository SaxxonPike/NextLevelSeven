using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test.Native
{
    [TestClass]
    public class NativeTestFixture : PerformanceTestFixture
    {
        protected const int MediumIndex = 1000;
        protected const int HighIndex = 1000000;
    }
}
