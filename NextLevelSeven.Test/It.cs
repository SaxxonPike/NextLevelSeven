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
        static public void Throws<TException>(Action action)
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
        }
    }
}
