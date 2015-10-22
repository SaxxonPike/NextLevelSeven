using System.Collections.Generic;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Core
{
    /// <summary>Represents an abstract element in an HL7 message.</summary>
    public interface IElement
    {
        /// <summary>Get a sub-element at the specified index.</summary>
        /// <param name="index"></param>
        /// <returns>Sub-element at the specified index.</returns>
        IElement this[int index] { get; }

        /// <summary>Get the ancestor element. Null if the element is a root element.</summary>
        IElement Ancestor { get; }

        /// <summary>Get a codec which can be used to interpret the stored value as other types.</summary>
        IEncodedTypeConverter Converter { get; }

        /// <summary>Get the delimiter character of the element. This will be zero if there are no sub-elements.</summary>
        char Delimiter { get; }

        /// <summary>Get the descendant elements within this element.</summary>
        IEnumerable<IElement> Descendants { get; }

        /// <summary>Get the encoding characters this element uses.</summary>
        IReadOnlyEncoding Encoding { get; }

        /// <summary>Returns true if the element is considered existant.</summary>
        bool Exists { get; }

        /// <summary>Get the index of the element.</summary>
        int Index { get; }

        /// <summary>Unique key of the element within the message.</summary>
        string Key { get; }

        /// <summary>Next available index for adding elements.</summary>
        int NextIndex { get; }

        /// <summary>Get or set the complete value of the element.</summary>
        string Value { get; set; }

        /// <summary>Get the number of subvalues in the element.</summary>
        int ValueCount { get; }

        /// <summary>Get or set the subvalues of the element.</summary>
        IEnumerable<string> Values { get; set; }

        /// <summary>
        ///     Get the parent message for the element.
        /// </summary>
        IMessage Message { get; }

        /// <summary>Delete a descendant element.</summary>
        void Delete(int index);

        /// <summary>Insert a descendant element as a value at the specified index.</summary>
        /// <param name="value">Value to insert.</param>
        /// <param name="index">Index where to insert.</param>
        /// <returns></returns>
        IElement Insert(int index, string value);

        /// <summary>Insert a descendant element at the specified index.</summary>
        /// <param name="element">Element to insert.</param>
        /// <param name="index">Index where to insert.</param>
        /// <returns></returns>
        IElement Insert(int index, IElement element);

        /// <summary>Move a descendant element to another index.</summary>
        /// <param name="sourceIndex"></param>
        /// <param name="targetIndex"></param>
        void Move(int sourceIndex, int targetIndex);

        /// <summary>Erase this element's content and mark it non-existant.</summary>
        void Erase();

        /// <summary>Get a copy of the element.</summary>
        IElement Clone();
    }
}