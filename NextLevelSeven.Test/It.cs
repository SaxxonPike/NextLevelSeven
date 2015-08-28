using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test
{
    static public class It
    {
        static public void Throws<TException>(Action action, string message = null)
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

            if (message != null)
            {
                Assert.Fail(message);
            }
            else
            {
                Assert.Fail(String.Format("Expected exception {0} was not thrown.", typeof(TException).Name));
            }
        }
    }
}
