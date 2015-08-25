using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    sealed internal class EmptyEnumerable<T> : IEnumerable<T> where T : class
    {
        public static readonly EmptyEnumerable<T> Instance = new EmptyEnumerable<T>();
        private static readonly EmptyEnumerator<T> Enumerator = new EmptyEnumerator<T>();

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Enumerator;
        }
    }
}
