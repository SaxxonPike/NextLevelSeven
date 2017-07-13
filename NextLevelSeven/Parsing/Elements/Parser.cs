using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Parsing.Dividers;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents a generic HL7 element, which may contain other elements.</summary>
    internal abstract class Parser : IElementParser
    {
        /// <summary>String divider used to split the element's raw value.</summary>
        private readonly Lazy<StringDivider> _descendantDivider;

        /// <summary>Base encoding configuration.</summary>
        private readonly Lazy<ReadOnlyEncodingConfiguration> _encodingConfiguration;

        /// <summary>Create a root element.</summary>
        protected Parser()
        {
            _descendantDivider = new Lazy<StringDivider>(GetDescendantDivider);
            _encodingConfiguration = new Lazy<ReadOnlyEncodingConfiguration>(GetEncodingConfiguration);
        }

        /// <summary>Create a root element with the specified encoding configuration.</summary>
        /// <param name="config"></param>
        protected Parser(ReadOnlyEncodingConfiguration config)
            : this()
        {
            _encodingConfiguration = new Lazy<ReadOnlyEncodingConfiguration>(() => config);
        }

        /// <summary>Ancestor element. Root elements return null.</summary>
        protected virtual Parser Ancestor => null;

        /// <summary>Get the string divider used to find descendant values.</summary>
        public StringDivider DescendantDivider => _descendantDivider.Value;

        /// <summary>Get the deliminter internally being used by the descendant divider.</summary>
        protected char DescendantDividerDelimiter => _descendantDivider.Value.Delimiter;

        /// <summary>Returns true if the descendant divider has been initialized.</summary>
        protected bool DescendantDividerInitialized => _descendantDivider.IsValueCreated;

        /// <summary>Get the encoding configuration.</summary>
        protected ReadOnlyEncodingConfiguration EncodingConfiguration => _encodingConfiguration.Value;

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Index of the desired element.</param>
        /// <returns>Element at the specified index.</returns>
        public IElementParser this[int index] => GetDescendant(index);

        /// <summary>Get the codec used to convert values in this element.</summary>
        public IEncodedTypeConverter Converter => new EncodedTypeConverter(this);

        /// <summary>Delimiter character to be used when locating sub-elements.</summary>
        public abstract char Delimiter { get; }

        /// <summary>Number of descendant values.</summary>
        public virtual int ValueCount => DescendantDivider.Count;

        /// <summary>Get descendant elements as an enumerable set.</summary>
        public virtual IEnumerable<IElementParser> Descendants
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return this[i];
                }
            }
        }

        /// <summary>If true, the element is considered to exist.</summary>
        public bool Exists => !string.IsNullOrEmpty(DescendantDivider?.Value);

        /// <summary>Get or set the exposed index.</summary>
        public int Index { get; set; }

        /// <summary>Get or set the raw value of this element.</summary>
        public virtual string Value
        {
            get => DescendantDivider == null 
                ? string.Empty 
                : DescendantDivider.Value;
            set => DescendantDivider.Value = value;
        }

        /// <summary>Get or set the descendant raw values of this element.</summary>
        public virtual IEnumerable<string> Values
        {
            get => DescendantDivider.Values;
            set => DescendantDivider.Values = value;
        }

        /// <summary>Create a deep clone of the element.</summary>
        /// <returns>Cloned element.</returns>
        public abstract IElement Clone();

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Index of the desired element.</param>
        /// <returns>Descendant element.</returns>
        IElement IElement.this[int index] => GetDescendant(index);

        /// <summary>Get the generic type ancestor element.</summary>
        IElement IElement.Ancestor => Ancestor;

        /// <summary>Get the generic type descendant collection.</summary>
        IEnumerable<IElement> IElement.Descendants => Descendants;

        /// <summary>Unique key of the element within the message.</summary>
        public string Key => ElementOperations.GetKey(this);

        /// <summary>Get the next available index.</summary>
        public virtual int NextIndex => ValueCount + 1;

        /// <summary>Erase an element from existence.</summary>
        public void Erase()
        {
            Value = null;
        }

        /// <summary>Get the encoding configuration being used for this parser.</summary>
        public IReadOnlyEncoding Encoding => EncodingConfiguration;

        /// <summary>Delete a descendant element.</summary>
        /// <param name="index">Index to insert at.</param>
        public virtual void Delete(int index)
        {
            if (index < 1)
            {
                throw new ElementException(ErrorCode.ElementIndexMustBeZeroOrGreater);
            }
            DescendantDivider.Delete(index - 1);
        }

        /// <summary>Insert a descendant element.</summary>
        /// <param name="element">Element to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public virtual IElement Insert(int index, IElement element)
        {
            if (index < 1)
            {
                throw new ElementException(ErrorCode.ElementIndexMustBeZeroOrGreater);
            }
            return Insert(index, element.Value);
        }

        /// <summary>Insert a descendant element.</summary>
        /// <param name="value">Value to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public virtual IElement Insert(int index, string value)
        {
            if (index < 1)
            {
                throw new ElementException(ErrorCode.ElementIndexMustBeZeroOrGreater);
            }
            DescendantDivider.Insert(index - 1, value);
            return GetDescendant(index);
        }

        /// <summary>Move a descendant.</summary>
        /// <param name="sourceIndex"></param>
        /// <param name="targetIndex"></param>
        public virtual void Move(int sourceIndex, int targetIndex)
        {
            if (sourceIndex < 1 || targetIndex < 1)
            {
                throw new ElementException(ErrorCode.ElementIndexMustBeZeroOrGreater);
            }
            DescendantDivider.Move(sourceIndex - 1, targetIndex - 1);
        }

        /// <summary>
        ///     Get the message for this builder.
        /// </summary>
        public IMessage Message
        {
            get
            {
                var ancestor = Ancestor;
                while (ancestor != null)
                {
                    if (ancestor is IMessage)
                    {
                        return ancestor as IMessage;
                    }
                    ancestor = ancestor.Ancestor;
                }
                return null;
            }
        }

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Exposed index of the descendant element.</param>
        /// <returns>Descendant element at the specified index.</returns>
        protected abstract IElementParser GetDescendant(int index);

        /// <summary>Copy the contents of this element to a string.</summary>
        /// <returns>Copied string.</returns>
        public sealed override string ToString()
        {
            return Value ?? string.Empty;
        }

        /// <summary>
        /// Get the encoding configuration or, if it doesn't exist and the class is a message element, create one.
        /// </summary>
        /// <returns>Encoding configuration.</returns>
        private ReadOnlyEncodingConfiguration GetEncodingConfiguration()
        {
            return this is IMessage
                ? new ParserEncodingConfiguration((ISegment)this[1])
                : Ancestor?.EncodingConfiguration;
        }

        /// <summary>Get a string divider for this descendant element.</summary>
        /// <returns>Descendant string divider.</returns>
        protected virtual StringDivider GetDescendantDivider()
        {
            return new RootStringDivider(string.Empty, Delimiter);
        }
    }
}