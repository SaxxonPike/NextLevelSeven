using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Common;
using NextLevelSeven.Core;
using NUnit.Framework;

namespace NextLevelSeven.Test
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public abstract class BaseTestFixture
    {
        private long _frequency;
        private Stopwatch _stopwatch;

        [SetUp]
        public void Fixture_Initialize()
        {
            _frequency = Stopwatch.Frequency;
            Debug.WriteLine("---> Timed test started.");
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        [TearDown]
        public void Fixture_Cleanup()
        {
            _stopwatch.Stop();
            var ticks = _stopwatch.ElapsedTicks;
            var milliseconds = _stopwatch.ElapsedMilliseconds;
            Debug.WriteLine("---> {0}ms ({1}\x00B5s)", milliseconds, ticks*1000000/_frequency);
        }

        protected static IElement CloneAndTest(Type type, IElement source)
        {
            var clone = (IElement)InvokeMethod(type, "Clone", source);
            clone.Should().NotBeNull();
            clone.Should().NotBeSameAs(source);
            clone.Value.Should().Be(source.Value);
            return clone;
        }

        protected static object InvokeGetter(Type type, string propertyName, object instance)
        {
            var property = type.GetPropertyByName(propertyName);
            return property.GetMethod.Invoke(instance, null);
        }

        protected static object InvokeMethod(Type type, string methodName, object instance, params object[] parameters)
        {
            var method = type.GetMethod(methodName);
            return method.Invoke(instance, parameters);
        }
    }
}