using System;
using System.Collections;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Native.Dividers;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Enumerator for element descendants.
    /// </summary>
    /// <typeparam name="T">Type of descendants.</typeparam>
    internal sealed class NativeElementEnumerator<T> : IEnumerator<T> where T : INativeElement
    {
        /// <summary>
        ///     Create an element enumerator.
        /// </summary>
        /// <param name="descendantDivider">Divider to get the data from.</param>
        /// <param name="descendantFactory">Function that creates descendant elements.</param>
        public NativeElementEnumerator(IStringDivider descendantDivider, ProxyFactory<int, T> descendantFactory)
        {
            Factory = descendantFactory;
            Divider = descendantDivider;
            Reset();
        }

        /// <summary>
        ///     Get the currently selected element.
        /// </summary>
        public INativeElement Current
        {
            get { return Factory(Index); }
        }

        /// <summary>
        ///     String divider used to split the values.
        /// </summary>
        private IStringDivider Divider { get; set; }

        /// <summary>
        ///     Function that creates descendant elements.
        /// </summary>
        private ProxyFactory<int, T> Factory { get; set; }

        /// <summary>
        ///     Currently selected index.
        /// </summary>
        private int Index { get; set; }

        /// <summary>
        ///     Get the currently selected item.
        /// </summary>
        T IEnumerator<T>.Current
        {
            get { return Factory(Index); }
        }

        /// <summary>
        ///     Dispose this object.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        ///     Get the currently selected item.
        /// </summary>
        object IEnumerator.Current
        {
            get { return Factory(Index); }
        }

        /// <summary>
        ///     Select the next item.
        /// </summary>
        /// <returns>True, if the action was possible.</returns>
        public bool MoveNext()
        {
            Index++;
            return Index <= Divider.Count;
        }

        /// <summary>
        ///     Reset the cursor to the first item.
        /// </summary>
        public void Reset()
        {
            Index = 0;
        }
    }
}