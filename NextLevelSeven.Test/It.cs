using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    public static class It
    {
        public static void Throws<TException>(Action action, string message = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof (TException))
                {
                    return;
                }
                throw;
            }

            Assert.Fail(message ?? String.Format("Expected exception {0} was not thrown.", typeof (TException).Name));
        }
    }
}