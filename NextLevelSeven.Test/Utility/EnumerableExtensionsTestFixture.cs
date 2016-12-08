using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NextLevelSeven.Test.Testing;
using NextLevelSeven.Utility;
using NUnit.Framework;

namespace NextLevelSeven.Test.Utility
{
    [TestFixture]
    public class EnumerableExtensionsTestFixture : UtilityBaseTestFixture
    {
        [Test]
        public void Yield_WrapsReferences()
        {
            // Arrange.
            var item = new object();

            // Act.
            var observed = item.Yield();

            // Assert.
            observed.Should().BeEquivalentTo(item);
        }

        [Test]
        public void Yield_WrapsValues()
        {
            // Arrange.
            var item = Any.Number();

            // Act.
            var observed = item.Yield();

            // Assert.
            observed.Should().BeEquivalentTo(item);
        }
    }
}
