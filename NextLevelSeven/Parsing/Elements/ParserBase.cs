using System;
using System.Collections.Generic;
using NextLevelSeven.Conversion;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Parsing.Dividers;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>
    ///     Represents a generic HL7 message element, which may contain other elements.
    /// </summary>
    internal abstract class ParserBase : IElementParser, IComparable, IComparable<IElement>, IComparable<string>,
        IEquatable<IElement>, IEquatable<string>, IEncodedElement
    {
        /// <summary>
        ///     Base encoding configuration.
        /// </summary>
        private EncodingConfigurationBase _encodingConfiguration;

        /// <summary>
        ///     Create a root element.
        /// </summary>
        protected ParserBase()
        {
        }

        /// <summary>
        ///     Create a root element with the specified encoding configuration.
        /// </summary>
        /// <param name="config"></param>
        protected ParserBase(EncodingConfigurationBase config)
        {
            _encodingConfiguration = config;
        }

        /// <summary>
        ///     Create a descendant element with the specified ancestor.
        /// </summary>
        protected ParserBase(ParserBase ancestor)
        {
            Ancestor = ancestor;
        }

        /// <summary>
        ///     Create a descendant element with the specified ancestor and encoding configuration.
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="config"></param>
        protected ParserBase(ParserBase ancestor, EncodingConfigurationBase config)
        {
            Ancestor = ancestor;
            _encodingConfiguration = config;
        }

        /// <summary>
        ///     String divider used to split the element's raw value.
        /// </summary>
        protected IStringDivider DescendantStringDivider { get; private set; }

        /// <summary>
        ///     Zero-based index within the parent element's raw data.
        /// </summary>
        protected int ParentIndex { get; set; }

        /// <summary>
        ///     Ancestor element. Null if this element is a root element.
        /// </summary>
        protected ParserBase Ancestor { get; private set; }

        /// <summary>
        ///     Get the string divider used to find descendant values.
        /// </summary>
        public IStringDivider DescendantDivider
        {
            get
            {
                if (DescendantStringDivider != null)
                {
                    return DescendantStringDivider;
                }

                DescendantStringDivider = (Ancestor == null)
                    ? GetDescendantDividerRoot(string.Empty)
                    : GetDescendantDivider(Ancestor, ParentIndex);
                return DescendantStringDivider;
            }
        }

        /// <summary>
        ///     Compare this builder's value with another object's value. (IComparable support)
        /// </summary>
        /// <param name="obj">Other BuilderBase.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return obj == null
                ? 1
                : CompareTo(obj.ToString());
        }

        /// <summary>
        ///     Compare this builder's value with another element's value. (element IComparable support)
        /// </summary>
        /// <param name="other">Other element to compare to.</param>
        /// <returns></returns>
        public int CompareTo(IElement other)
        {
            return other == null
                ? 1
                : CompareTo(other.Value);
        }

        /// <summary>
        ///     Compare this builder's value with another string. (generic IComparable support)
        /// </summary>
        /// <param name="other">Other string to compare to.</param>
        /// <returns></returns>
        public int CompareTo(string other)
        {
            return string.Compare(Value, other, StringComparison.CurrentCulture);
        }

        /// <summary>
        ///     Event that is raised when this element's raw value has changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        ///     Get the descendant element at the specified index.
        /// </summary>
        /// <param name="index">Index of the desired element.</param>
        /// <returns>Element at the specified index.</returns>
        public IElementParser this[int index]
        {
            get { return GetDescendant(index); }
        }

        /// <summary>
        ///     Get the codec used to convert values in this element.
        /// </summary>
        public IEncodedTypeConverter As
        {
            get { return new EncodedTypeConverter(this); }
        }

        /// <summary>
        ///     Delimiter character to be used when locating sub-elements.
        /// </summary>
        public abstract char Delimiter { get; }

        /// <summary>
        ///     Number of descendant values.
        /// </summary>
        public virtual int ValueCount
        {
            get { return DescendantDivider.Count; }
        }

        /// <summary>
        ///     Get descendant elements as an enumerable set.
        /// </summary>
        public virtual IEnumerable<IElementParser> DescendantElements
        {
            get
            {
                return new ProxyEnumerable<IElementParser>(GetDescendant,
                    null,
                    GetValueCount,
                    1);
            }
        }

        /// <summary>
        ///     If true, the element is considered to exist.
        /// </summary>
        public bool Exists
        {
            get
            {
                if (Ancestor == null)
                {
                    return true;
                }
                return (Index <= Ancestor.ValueCount);
            }
        }

        /// <summary>
        ///     Get or set the exposed index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Get or set the raw value of this element.
        /// </summary>
        public virtual string Value
        {
            get
            {
                return (DescendantDivider == null)
                    ? string.Empty
                    : DescendantDivider.Value;
            }
            set
            {
                DescendantDivider.Value = value;
                if (ValueChanged != null)
                {
                    ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     Get or set the descendant raw values of this element.
        /// </summary>
        public virtual IEnumerable<string> Values
        {
            get
            {
                return (ValueCount > 1)
                    ? DescendantDivider.Value.Split(DescendantDivider.Delimiter)
                    : new[] {DescendantDivider.Value};
            }
            set
            {
                DescendantDivider.Value = (DescendantDivider.Delimiter != '\0')
                    ? string.Join(new string(DescendantDivider.Delimiter, 1), value)
                    : string.Join(string.Empty, value);
            }
        }

        /// <summary>
        ///     Create a deep clone of the element.
        /// </summary>
        /// <returns>Cloned element.</returns>
        public abstract IElement Clone();

        /// <summary>
        ///     Get the descendant element at the specified index.
        /// </summary>
        /// <param name="index">Index of the desired element.</param>
        /// <returns>Descendant element.</returns>
        IElement IElement.this[int index]
        {
            get { return GetDescendant(index); }
        }

        /// <summary>
        ///     Get or set the value as a formatted string.
        /// </summary>
        public string FormattedValue
        {
            get { return TextConverter.ConvertToString(Value); }
            set { Value = TextConverter.ConvertFromString(value); }
        }

        IElement IElement.Ancestor
        {
            get { return Ancestor; }
        }

        IEnumerable<IElement> IElement.Descendants
        {
            get { return DescendantElements; }
        }

        /// <summary>
        ///     Unique key of the element within the message.
        /// </summary>
        public string Key
        {
            get { return ElementOperations.GetKey(this); }
        }

        /// <summary>
        ///     Get the encoding configuration.
        /// </summary>
        public EncodingConfigurationBase EncodingConfiguration
        {
            get
            {
                if (_encodingConfiguration != null)
                {
                    return _encodingConfiguration;
                }
                if (Ancestor != null)
                {
                    return Ancestor.EncodingConfiguration;
                }
                if (!(this is MessageParser))
                {
                    return new EncodingConfiguration();
                }
                _encodingConfiguration = new MessageParserEncodingConfiguration(this);
                return _encodingConfiguration;
            }
        }

        /// <summary>
        ///     Determines whether this builder's value is equivalent to another element's value. (element IEquatable support)
        /// </summary>
        /// <param name="other">Object to compare to.</param>
        /// <returns>True, if objects are considered to be equivalent.</returns>
        public bool Equals(IElement other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Determine equality with a string.
        /// </summary>
        /// <param name="other">String to compare to.</param>
        /// <returns>True, if the element's raw value and the specified string are equivalent.</returns>
        public bool Equals(string other)
        {
            return ToString() == other;
        }

        /// <summary>
        ///     Determine value equality with another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True, if this element's value and the object are equivalent.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj.ToString() == ToString();
        }

        /// <summary>
        ///     Get the hash code for this element.
        /// </summary>
        /// <returns>Hash code of the value's string.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        ///     Get the number of values in the element.
        /// </summary>
        /// <returns>Number of values in the element.</returns>
        protected int GetValueCount()
        {
            return ValueCount;
        }

        /// <summary>
        ///     Get the descendant element at the specified index.
        /// </summary>
        /// <param name="index">Exposed index of the descendant element.</param>
        /// <returns>Descendant element at the specified index.</returns>
        public abstract IElementParser GetDescendant(int index);

        /// <summary>
        ///     Copy the contents of this element to a string.
        /// </summary>
        /// <returns>Copied string.</returns>
        public override sealed string ToString()
        {
            return Value;
        }

        /// <summary>
        ///     Get a string divider for this descendant element.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Zero-based index within the parent's divider.</param>
        /// <returns>Descendant string divider.</returns>
        protected virtual IStringDivider GetDescendantDivider(ParserBase ancestor, int index)
        {
            return new StringSubDivider(ancestor.DescendantDivider, Delimiter, index);
        }

        /// <summary>
        ///     Get a string divider for this root element.
        /// </summary>
        /// <param name="value">Initial value.</param>
        /// <returns>String divider.</returns>
        private IStringDivider GetDescendantDividerRoot(string value)
        {
            return new StringDivider(value, Delimiter);
        }

        /// <summary>
        ///     Get the next available index.
        /// </summary>
        public virtual int NextIndex
        {
            get { return ValueCount + 1; }
        }
    }
}