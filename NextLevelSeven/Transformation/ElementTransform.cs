using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Codecs;
using NextLevelSeven.Core;

namespace NextLevelSeven.Transformation
{
    /// <summary>
    /// Base class for performing transformations on elements.
    /// </summary>
    abstract public class ElementTransform : IElement
    {
        /// <summary>
        /// Create a transform.
        /// </summary>
        /// <param name="baseElement">Element to wrap.</param>
        protected ElementTransform(IElement baseElement)
        {
            BaseElement = baseElement;
        }

        /// <summary>
        /// Get the wrapped element.
        /// </summary>
        protected readonly IElement BaseElement;

        /// <summary>
        /// Get a descendant element at the specified index. Indices match the HL7 specification, and are not necessarily zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        virtual public IElement this[int index]
        {
            get { return BaseElement[index]; }
        }

        /// <summary>
        /// Get the ancestor (higher level in the heirarchy) element.
        /// </summary>
        virtual public IElement AncestorElement
        {
            get { return BaseElement.AncestorElement; }
        }

        /// <summary>
        /// Get conversion options for non-string datatypes, such as numeric and dates.
        /// </summary>
        virtual public ICodec As
        {
            get { return BaseElement.As; }
        }

        /// <summary>
        /// Create a detached clone of the element with no ancestors.
        /// </summary>
        /// <returns></returns>
        virtual public IElement CloneDetached()
        {
            return BaseElement.CloneDetached();
        }

        /// <summary>
        /// Create a clone of this element transform to be used with another element.
        /// </summary>
        /// <returns>Cloned transform.</returns>
        public abstract ElementTransform CloneTransform(IElement element);

        /// <summary>
        /// Delete the element from its ancestor. This cannot be performed on the root element (message).
        /// </summary>
        virtual public void Delete()
        {
            BaseElement.Delete();
        }

        /// <summary>
        /// Delimiter used to separate descendants.
        /// </summary>
        virtual public char Delimiter
        {
            get { return BaseElement.Delimiter; }
        }

        /// <summary>
        /// Get the number of descendant elements.
        /// </summary>
        virtual public int DescendantCount
        {
            get { return BaseElement.DescendantCount; }
        }

        /// <summary>
        /// Get all descendant elements.
        /// </summary>
        virtual public IEnumerable<IElement> DescendantElements
        {
            get { return BaseElement.DescendantElements; }
        }

        /// <summary>
        /// Erase the element data. This does not delete the element, but does mark it as non-existant.
        /// </summary>
        virtual public void Erase()
        {
            BaseElement.Erase();
        }

        /// <summary>
        /// If true, the element is considered to exist in the message. This is not dependent on the value being null.
        /// </summary>
        virtual public bool Exists
        {
            get { return BaseElement.Exists; }
        }

        /// <summary>
        /// If true, the element has meaningful descendants (not necessarily direct ones.)
        /// </summary>
        virtual public bool HasSignificantDescendants
        {
            get { return BaseElement.HasSignificantDescendants; }
        }

        /// <summary>
        /// Index of the element.
        /// </summary>
        virtual public int Index
        {
            get { return BaseElement.Index; }
        }

        /// <summary>
        /// Unique key of the element within the message.
        /// </summary>
        virtual public string Key
        {
            get { return BaseElement.Key; }
        }

        /// <summary>
        /// Get the root message for this element.
        /// </summary>
        virtual public IMessage Message
        {
            get { return BaseElement.Message; }
        }

        /// <summary>
        /// Set the element data to null.
        /// </summary>
        virtual public void Nullify()
        {
            BaseElement.Nullify();
        }

        /// <summary>
        /// Get or set the element data. When setting new data, descendents will automatically be repopulated.
        /// </summary>
        virtual public string Value
        {
            get { return BaseElement.Value; }
            set { BaseElement.Value = value; }
        }

        /// <summary>
        /// Get or set the element data. Delimiters are automatically inserted.
        /// </summary>
        virtual public string[] Values
        {
            get { return BaseElement.Values; }
            set { BaseElement.Values = value; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        virtual public IEnumerator<IElement> GetEnumerator()
        {
            return DescendantElements.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
