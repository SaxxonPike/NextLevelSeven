using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    internal abstract class ElementParser : IElementParser, IComparable, IComparable<IElement>, IComparable<string>,
        IEquatable<IElement>, IEquatable<string>, IDividable
    {
        /// <summary>
        ///     Encoding configuration override.
        /// </summary>
        protected EncodingConfigurationBase EncodingConfigurationOverride;

        /// <summary>
        ///     String divider used to split the element's raw value.
        /// </summary>
        private IStringDivider _descendantDivider;

        /// <summary>
        ///     Determines whether or not the descendant divider has been initialized.
        /// </summary>
        private bool _descendantDividerInitialized;

        /// <summary>
        ///     Create a root element with the default values.
        /// </summary>
        protected ElementParser()
        {
            Index = 0;
            ParentIndex = 0;
            _descendantDivider = GetDescendantDividerRoot(string.Empty);
            Ancestor = null;
        }

        /// <summary>
        ///     Create a root element with the specified initial value.
        /// </summary>
        /// <param name="value">Initial value.</param>
        protected ElementParser(string value)
        {
            Index = 0;
            ParentIndex = 0;
            _descendantDivider = GetDescendantDividerRoot(value);
            Ancestor = null;
        }

        /// <summary>
        ///     Create a root element with the specified initial value and encoding configuration.
        /// </summary>
        /// <param name="value">Initial value.</param>
        /// <param name="config">Encoding configuration.</param>
        protected ElementParser(string value, EncodingConfigurationBase config)
        {
            EncodingConfigurationOverride = config;
            _descendantDivider = GetDescendantDividerRoot(value);
            Ancestor = null;
        }

        /// <summary>
        ///     Create a descendant element with the specified indices.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="parentIndex">Zero-based index within the parent element's raw data.</param>
        /// <param name="externalIndex">Exposed index.</param>
        protected ElementParser(ElementParser ancestor, int parentIndex, int externalIndex)
        {
            Index = externalIndex;
            ParentIndex = parentIndex;
            Ancestor = ancestor;
        }

        /// <summary>
        ///     Get the encoding configuration.
        /// </summary>
        public virtual EncodingConfigurationBase EncodingConfiguration
        {
            get { return EncodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
            set { EncodingConfigurationOverride = value; }
        }

        /// <summary>
        ///     Zero-based index within the parent element's raw data.
        /// </summary>
        protected int ParentIndex { get; set; }

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
        ///     Ancestor element. Null if this element is a root element.
        /// </summary>
        public ElementParser Ancestor { get; private set; }

        /// <summary>
        ///     Get the string divider used to find descendant values.
        /// </summary>
        public IStringDivider DescendantDivider
        {
            get
            {
                if (_descendantDivider != null || _descendantDividerInitialized)
                {
                    return _descendantDivider;
                }

                _descendantDividerInitialized = true;
                _descendantDivider = (Ancestor == null)
                    ? GetDescendantDividerRoot(string.Empty)
                    : GetDescendantDivider(Ancestor, ParentIndex);
                return _descendantDivider;
            }
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
        ///     Ancestor element, as an INativeElement.
        /// </summary>
        public IElementParser AncestorElement
        {
            get { return Ancestor; }
        }

        /// <summary>
        ///     Get the codec used to convert values in this element.
        /// </summary>
        public IEncodedTypeConverter As
        {
            get { return new ParserCodec(this); }
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
                return new WrapperEnumerable<IElementParser>(i => this[i],
                    (i, v) => { },
                    () => ValueCount,
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
        ///     If true, meaningful subdivisions of the element's raw value exist.
        /// </summary>
        public virtual bool HasSignificantDescendants
        {
            get
            {
                if (!Exists || ValueCount == 0 || Delimiter == '\0')
                {
                    return false;
                }

                return (ValueCount > 1) || DescendantElements.Any(d => d.HasSignificantDescendants);
            }
        }

        /// <summary>
        ///     Get or set the exposed index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Get a key unique to this element in the tree.
        /// </summary>
        public virtual string Key
        {
            get
            {
                return (Ancestor != null)
                    ? String.Join(Ancestor.Key, ".", Index.ToString(CultureInfo.InvariantCulture))
                    : Index.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        ///     Get the message containing this element. Returns null if the element does not belong to a message.
        /// </summary>
        public virtual IMessageParser Message
        {
            get
            {
                if (Ancestor is MessageParser)
                {
                    return new MessageParser(Ancestor.ToString());
                }

                return (Ancestor != null)
                    ? Ancestor.Message
                    : null;
            }
        }

        /// <summary>
        ///     Get or set the raw value of this element.
        /// </summary>
        public virtual string Value
        {
            get
            {
                return (DescendantDivider == null)
                    ? null
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
            get { return AncestorElement; }
        }

        IEnumerable<IElement> IElement.Descendants
        {
            get { return DescendantElements; }
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
        ///     Get the descendant element at the specified index.
        /// </summary>
        /// <param name="index">Exposed index of the descendant element.</param>
        /// <returns>Descendant element at the specified index.</returns>
        public abstract IElementParser GetDescendant(int index);

        /// <summary>
        ///     Get a string divider for this descendant element.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Zero-based index within the parent's divider.</param>
        /// <returns>Descendant string divider.</returns>
        protected virtual IStringDivider GetDescendantDivider(ElementParser ancestor, int index)
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
        ///     Copy the contents of this element to a string.
        /// </summary>
        /// <returns>Copied string.</returns>
        public override sealed string ToString()
        {
            return Value;
        }
    }
}