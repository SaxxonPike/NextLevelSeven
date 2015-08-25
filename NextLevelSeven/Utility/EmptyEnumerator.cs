using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    sealed internal class EmptyEnumerator<T> : IEnumerator<T> where T : class
    {
        public T Current
        {
            get { return null; }
        }

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current
        {
            get { return null; }
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }
    }
}
