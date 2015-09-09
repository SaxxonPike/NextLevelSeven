using System;
using System.Collections.Generic;
using NextLevelSeven.Codecs;
using NextLevelSeven.Core;

namespace NextLevelSeven.Transformation
{
    /// <summary>
    ///     Base class for performing transformations on elements.
    /// </summary>
    public abstract class ElementTransform : INativeElement
    {
        /// <summary>
        ///     Get the wrapped element.
        /// </summary>
        protected readonly INativeElement BaseElement;

        /// <summary>
        ///     Create a transform.
        /// </summary>
        /// <param name="baseElement">Element to wrap.</param>
        protected ElementTransform(INativeElement baseElement)
        {
            BaseElement = baseElement;
        }

        public event EventHandler ValueChanged;

        /// <summary>
        ///     Get a descendant element at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        public virtual INativeElement this[int index]
        {
            get { return BaseElement[index]; }
        }

        /// <summary>
        ///     Get the ancestor (higher level in the heirarchy) element.
        /// </summary>
        public virtual INativeElement AncestorElement
        {
            get { return BaseElement.AncestorElement; }
        }

        /// <summary>
        ///     Get conversion options for non-string datatypes, such as numeric and dates.
        /// </summary>
        public virtual ICodec As
        {
            get { return BaseElement.As; }
        }

        /// <summary>
        ///     Create a detached clone of the element with no ancestors.
        /// </summary>
        /// <returns></returns>
        public virtual INativeElement CloneDetached()
        {
            return BaseElement.CloneDetached();
        }

        /// <summary>
        ///     Delete the element from its ancestor. This cannot be performed on the root element (message).
        /// </summary>
        public virtual void Delete()
        {
            BaseElement.Delete();
        }

        /// <summary>
        ///     Delimiter used to separate descendants.
        /// </summary>
        public virtual char Delimiter
        {
            get { return BaseElement.Delimiter; }
        }

        /// <summary>
        ///     Get the number of descendant elements.
        /// </summary>
        public virtual int DescendantCount
        {
            get { return BaseElement.DescendantCount; }
        }

        /// <summary>
        ///     Get all descendant elements.
        /// </summary>
        public virtual IEnumerable<INativeElement> DescendantElements
        {
            get { return BaseElement.DescendantElements; }
        }

        /// <summary>
        ///     Erase the element data. This does not delete the element, but does mark it as non-existant.
        /// </summary>
        public virtual void Erase()
        {
            BaseElement.Erase();
        }

        /// <summary>
        ///     If true, the element is considered to exist in the message. This is not dependent on the value being null.
        /// </summary>
        public virtual bool Exists
        {
            get { return BaseElement.Exists; }
        }

        /// <summary>
        ///     If true, the element has meaningful descendants (not necessarily direct ones.)
        /// </summary>
        public virtual bool HasSignificantDescendants
        {
            get { return BaseElement.HasSignificantDescendants; }
        }

        /// <summary>
        ///     Index of the element.
        /// </summary>
        public virtual int Index
        {
            get { return BaseElement.Index; }
        }

        /// <summary>
        ///     Unique key of the element within the message.
        /// </summary>
        public virtual string Key
        {
            get { return BaseElement.Key; }
        }

        /// <summary>
        ///     Get the root message for this element.
        /// </summary>
        public virtual INativeMessage Message
        {
            get { return BaseElement.Message; }
        }

        /// <summary>
        ///     Set the element data to null.
        /// </summary>
        public virtual void Nullify()
        {
            BaseElement.Nullify();
        }

        /// <summary>
        ///     Get or set the element data. When setting new data, descendents will automatically be repopulated.
        /// </summary>
        public virtual string Value
        {
            get { return BaseElement.Value; }
            set
            {
                BaseElement.Value = value;

                if (ValueChanged != null)
                {
                    ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     Get or set the element data. Delimiters are automatically inserted.
        /// </summary>
        public virtual string[] Values
        {
            get { return BaseElement.Values; }
            set { BaseElement.Values = value; }
        }

        /// <summary>
        ///     Create a clone of this element transform to be used with another element.
        /// </summary>
        /// <returns>Cloned transform.</returns>
        public abstract ElementTransform CloneTransform(INativeElement element);
    }
}