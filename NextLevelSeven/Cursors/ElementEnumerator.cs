using System;
using System.Collections;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Cursors.Dividers;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    ///     Enumerator for element descendants.
    /// </summary>
    /// <typeparam name="T">Type of descendants.</typeparam>
    internal sealed class ElementEnumerator<T> : IEnumerator<T> where T : IElement
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

        private IStringDivider Divider { get; set; }

        private Func<int, T> Factory { get; set; }

        private int Index { get; set; }

        T IEnumerator<T>.Current
        {
            get { return Factory(Index); }
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return Factory(Index); }
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