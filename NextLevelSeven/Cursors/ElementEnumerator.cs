using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Cursors.Dividers;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    /// Enumerator for element descendants.
    /// </summary>
    /// <typeparam name="T">Type of descendants.</typeparam>
    internal class ElementEnumerator<T> : IEnumerator<T> where T : IElement
    {
        public ElementEnumerator(IStringDivider descendantDivider, Func<int, T> descendantFactory)
        {
            Factory = descendantFactory;
            Divider = descendantDivider;
            Reset();
        }

        public IElement Current
        {
            get { return Factory(Index); }
        }

        T IEnumerator<T>.Current
        {
            get { return Factory(Index); }
        }

        public void Dispose()
        {
        }

        IStringDivider Divider
        {
            get;
            set;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Factory(Index); }
        }

        Func<int, T> Factory
        {
            get;
            set;
        }

        int Index
        {
            get;
            set;
        }

        public bool MoveNext()
        {
            Index++;
            if (Index > Divider.Count)
            {
                return false;
            }
            return true;
        }

        public void Reset()
        {
            Index = 0;
        }
    }
}
