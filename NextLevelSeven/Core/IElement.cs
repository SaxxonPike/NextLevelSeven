using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Codecs;

namespace NextLevelSeven.Core
{
    public interface IElement : IEnumerable<IElement>
    {
        /// <summary>
        /// Event that is triggered whenever either this element's value
        /// </summary>
        event EventHandler ValueChanged;

        /// <summary>
        /// Get a descendant element at the specified index. Indices match the HL7 specification, and are not necessarily zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        IElement this[int index] { get; }

        /// <summary>
        /// Get the ancestor (higher level in the heirarchy) element.
        /// </summary>
        IElement AncestorElement { get; }

        /// <summary>
        /// Get conversion options for non-string datatypes, such as numeric and dates.
        /// </summary>
        ICodec As { get; }

        /// <summary>
        /// Create a detached clone of the element with no ancestors.
        /// </summary>
        /// <returns></returns>
        IElement CloneDetached();

        /// <summary>
        /// Delete the element from its ancestor. This cannot be performed on the root element (message).
        /// </summary>
        void Delete();

        /// <summary>
        /// Delimiter used to separate descendants.
        /// </summary>
        char Delimiter { get; }

        /// <summary>
        /// Get the number of descendant elements.
        /// </summary>
        int DescendantCount { get; }

        /// <summary>
        /// Get all descendant elements.
        /// </summary>
        IEnumerable<IElement> DescendantElements { get; }

        /// <summary>
        /// Erase the element data. This does not delete the element, but does mark it as non-existant.
        /// </summary>
        void Erase();

        /// <summary>
        /// If true, the element is considered to exist in the message. This is not dependent on the value being null.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// If true, the element has meaningful descendants (not necessarily direct ones.)
        /// </summary>
        bool HasSignificantDescendants { get; }

        /// <summary>
        /// Index of the element.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Unique key of the element within the message.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Get the root message for this element.
        /// </summary>
        IMessage Message { get; }

        /// <summary>
        /// Set the element data to null.
        /// </summary>
        void Nullify();

        /// <summary>
        /// Get or set the element data. When setting new data, descendents will automatically be repopulated.
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Get or set the element data. Delimiters are automatically inserted.
        /// </summary>
        string[] Values { get; set; }
    }
}
