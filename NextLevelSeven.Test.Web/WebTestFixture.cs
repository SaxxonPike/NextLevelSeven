using System;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test.Web
{
    [TestClass]
    public class WebTestFixture : PerformanceTestFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            if (!IsTestRunnerAdmin())
            {
                Assert.Inconclusive("This test was not run. Web tests require administrator privileges.");
            }
        }

        private static bool IsTestRunnerAdmin()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                    .IsInRole(WindowsBuiltInRole.Administrator);
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}
