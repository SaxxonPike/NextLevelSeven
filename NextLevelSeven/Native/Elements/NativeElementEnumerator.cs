using System;
using System.Collections;
using System.Collections.Generic;
using NextLevelSeven.Native.Dividers;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Enumerator for element descendants.
    /// </summary>
    /// <typeparam name="T">Type of descendants.</typeparam>
    internal sealed class NativeElementEnumerator<T> : IEnumerator<T> where T : INativeElement
    {
        public NativeElementEnumerator(IStringDivider descendantDivider, Func<int, T> descendantFactory)
        {
            Factory = descendantFactory;
            Divider = descendantDivider;
            Reset();
        }

        public INativeElement Current
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