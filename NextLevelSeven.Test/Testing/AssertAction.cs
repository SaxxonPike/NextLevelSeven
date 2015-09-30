using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test.Testing
{
    public static class AssertAction
    {
        public static void Throws<TException>(Action action, string message = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (ex is TException)
                {
                    return;
                }
                throw;
            }

            Assert.Fail(message ?? string.Format("Expected exception {0} was not thrown.", typeof (TException).Name));
        }
    }
}