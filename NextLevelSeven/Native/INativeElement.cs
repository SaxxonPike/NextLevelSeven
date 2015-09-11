using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;

namespace NextLevelSeven.Native
{
    /// <summary>
    ///     Common interface that represents any one of the HL7v2 message constructs that holds data.
    /// </summary>
    public interface INativeElement : IElement
    {
        /// <summary>
        ///     Get a descendant element at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new INativeElement this[int index] { get; }

        /// <summary>
        ///     Get the ancestor (higher level in the heirarchy) element.
        /// </summary>
        INativeElement AncestorElement { get; }

        /// <summary>
        ///     Get all descendant elements.
        /// </summary>
        IEnumerable<INativeElement> DescendantElements { get; }

        /// <summary>
        ///     If true, the element is considered to exist in the message. This is not dependent on the value being null.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        ///     If true, the element has meaningful descendants (not necessarily direct ones.)
        /// </summary>
        bool HasSignificantDescendants { get; }

        /// <summary>
        ///     Unique key of the element within the message.
        /// </summary>
        string Key { get; }

        /// <summary>
        ///     Get the root message for this element.
        /// </summary>
        INativeMessage Message { get; }

        /// <summary>
        ///     Event that is triggered whenever either this element's value
        /// </summary>
        event EventHandler ValueChanged;

        /// <summary>
        ///     Delete the element from its ancestor. This cannot be performed on the root element (message).
        /// </summary>
        void Delete();

        /// <summary>
        ///     Erase the element data. This does not delete the element, but does mark it as non-existant.
        /// </summary>
        void Erase();

        /// <summary>
        ///     Set the element data to null.
        /// </summary>
        void Nullify();
    }
}