using System.Collections;
using System.Collections.Generic;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Enumerable interface for element descendants.
    /// </summary>
    internal sealed class NativeElementEnumerable : IEnumerable<INativeElement>
    {
        /// <summary>
        ///     Create an enumerable wrapper over an element.
        /// </summary>
        /// <param name="element">Element to wrap.</param>
        public NativeElementEnumerable(NativeElement element)
        {
            Element = element;
        }

        /// <summary>
        ///     Wrapped element.
        /// </summary>
        private NativeElement Element { get; set; }

        /// <summary>
        ///     Get the enumerator for the wrapper.
        /// </summary>
        /// <returns>Element enumerator.</returns>
        public IEnumerator<INativeElement> GetEnumerator()
        {
            return new NativeElementEnumerator<INativeElement>(Element.DescendantDivider, Element.GetDescendant);
        }

        /// <summary>
        ///     Get the enumerator for the wrapper.
        /// </summary>
        /// <returns>Element enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Get the contents of the wrapped element's raw value.
        /// </summary>
        /// <returns>Raw value.</returns>
        public override string ToString()
        {
            return Element.ToString();
        }
    }
}