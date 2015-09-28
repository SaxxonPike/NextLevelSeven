using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Conversion;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Base class for message builders.</summary>
    internal abstract class Builder : IComparable, IComparable<IElement>, IComparable<string>, IEquatable<IElement>,
        IEquatable<string>, IBuilder
    {
        /// <summary>Initialize the message builder base class.</summary>
        internal Builder()
        {
            Encoding = new BuilderEncodingConfiguration(this);
        }

        /// <summary>Initialize the message builder base class.</summary>
        /// <param name="config">Message's encoding configuration.</param>
        /// <param name="index">Index in the parent.</param>
        internal Builder(BuilderEncodingConfiguration config, int index)
        {
            Encoding = config;
            Index = index;
        }

        /// <summary>Get or set the character used to separate component-level content.</summary>
        public virtual char ComponentDelimiter { get; set; }

        /// <summary>Get or set the character used to signify escape sequences.</summary>
        public virtual char EscapeCharacter { get; set; }

        /// <summary>Get or set the character used to separate fields.</summary>
        public virtual char FieldDelimiter { get; set; }

        /// <summary>Get or set the character used to separate field repetition content.</summary>
        public virtual char RepetitionDelimiter { get; set; }

        /// <summary>Get or set the character used to separate subcomponent-level content.</summary>
        public virtual char SubcomponentDelimiter { get; set; }

        /// <summary>Get the encoding used by this builder.</summary>
        public BuilderEncodingConfiguration Encoding { get; private set; }

        /// <summary>Get the index at which this builder is located in its descendant.</summary>
        public int Index { get; private set; }

        /// <summary>Deep clone this element.</summary>
        /// <returns>Cloned element.</returns>
        public abstract IElement Clone();

        /// <summary>Get or set this element's value.</summary>
        public abstract string Value { get; set; }

        /// <summary>Get or set this element's sub-values.</summary>
        public abstract IEnumerable<string> Values { get; set; }

        /// <summary>Get a converter which will interpret this element's value as other types.</summary>
        public abstract IEncodedTypeConverter Codec { get; }

        /// <summary>Get the number of sub-values in this element.</summary>
        public abstract int ValueCount { get; }

        /// <summary>Get this element's section delimiter.</summary>
        public abstract char Delimiter { get; }

        /// <summary>Get the descendant builder at the specified index.</summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Descendant builder.</returns>
        public IElement this[int index]
        {
            get { return GetGenericElement(index); }
        }

        /// <summary>Get or set the value as a formatted string.</summary>
        public string FormattedValue
        {
            get { return TextConverter.ConvertToString(Value); }
            set { Value = TextConverter.ConvertFromString(value); }
        }

        /// <summary>Get the ancestor element. Null if it's a root element.</summary>
        IElement IElement.Ancestor
        {
            get { return GetAncestor(); }
        }

        /// <summary>Get descendant elements. For subcomponents, this will be empty.</summary>
        IEnumerable<IElement> IElement.Descendants
        {
            get { return GetDescendants(); }
        }

        /// <summary>Unique key of the element within the message.</summary>
        public string Key
        {
            get { return ElementOperations.GetKey(this); }
        }

        /// <summary>Get the next available index.</summary>
        public virtual int NextIndex
        {
            get { return GetDescendants().Where(d => d.Exists).Max(d => d.Index) + 1; }
        }

        /// <summary>Erase this element's content and mark it non-existant.</summary>
        public void Erase()
        {
            Value = null;
        }

        /// <summary>True, if the element is considered to exist.</summary>
        public abstract bool Exists { get; }

        /// <summary>Get the encoding used by this builder.</summary>
        IReadOnlyEncoding IElement.Encoding
        {
            get { return Encoding; }
        }

        /// <summary>Get the encoding used by this builder.</summary>
        IEncoding IBuilder.Encoding
        {
            get { return Encoding; }
        }

        /// <summary>Compare this builder's value with another object's value. (IComparable support)</summary>
        /// <param name="obj">Other BuilderBase.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return obj == null ? 1 : CompareTo(obj.ToString());
        }

        /// <summary>Compare this builder's value with another element's value. (element IComparable support)</summary>
        /// <param name="other">Other element to compare to.</param>
        /// <returns></returns>
        public int CompareTo(IElement other)
        {
            return other == null ? 1 : CompareTo(other.Value);
        }

        /// <summary>Compare this builder's value with another string. (generic IComparable support)</summary>
        /// <param name="other">Other string to compare to.</param>
        /// <returns></returns>
        public int CompareTo(string other)
        {
            return string.Compare(Value, other, StringComparison.CurrentCulture);
        }

        /// <summary>Determines whether this builder's value is equivalent to another element's value. (element IEquatable support)</summary>
        /// <param name="other">Object to compare to.</param>
        /// <returns>True, if objects are considered to be equivalent.</returns>
        public bool Equals(IElement other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <summary>Determine if this builder's value is equal to another string. (IEquatable support)</summary>
        /// <param name="other">Other string.</param>
        /// <returns>True, if the two are equivalent.</returns>
        public bool Equals(string other)
        {
            return string.Equals(Value, other, StringComparison.Ordinal);
        }

        /// <summary>Get the element at the specified index as an IElement.</summary>
        /// <param name="index">Index at which to get the element.</param>
        /// <returns>Generic element.</returns>
        protected abstract IElement GetGenericElement(int index);

        /// <summary>Determines whether this object is equivalent to another object.</summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>True, if objects are considered to be equivalent.</returns>
        public override sealed bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (ReferenceEquals(null, obj))
            {
                return Value == null;
            }
            return Value == obj.ToString();
        }

        /// <summary>Get this builder's hash code.</summary>
        /// <returns>Hash code for the builder.</returns>
        public override sealed int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>Get this builder's contents as a string.</summary>
        /// <returns>Builder's contents.</returns>
        public override sealed string ToString()
        {
            return Value ?? string.Empty;
        }

        /// <summary>Get the ancestor element.</summary>
        /// <returns>Ancestor element.</returns>
        protected virtual IElement GetAncestor()
        {
            return null;
        }

        /// <summary>Get descendant elements.</summary>
        /// <returns>Descendant elements.</returns>
        protected virtual IEnumerable<IElement> GetDescendants()
        {
            var count = ValueCount;
            for (var i = 1; i <= count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        ///     Delete a descendant element.
        /// </summary>
        /// <param name="index">Index to delete at.</param>
        public abstract void DeleteDescendant(int index);

        /// <summary>
        ///     Delete a descendant element at the specified index.
        /// </summary>
        /// <typeparam name="TDescendant">Type of descendant element.</typeparam>
        /// <param name="cache">Cache to delete within.</param>
        /// <param name="index">Descendant index to delete.</param>
        static protected void DeleteDescendant<TDescendant>(IIndexedCache<int, TDescendant> cache, int index) where TDescendant : Builder
        {
            var values = cache.Where(c => c.Key != index).ToList();
            foreach (var value in values.Where(v => v.Key > index))
            {
                value.Value.Index--;
            }
            cache.Clear();
            foreach (var value in values)
            {
                cache[value.Value.Index] = value.Value;
            }
        }

        /// <summary>
        ///     Insert a copy of the specified element at the specified descendant index.
        /// </summary>
        /// <param name="index">Index to insert into.</param>
        /// <param name="element">Element to insert.</param>
        public abstract IElement InsertDescendant(IElement element, int index);

        /// <summary>
        ///     Insert a copy of the specified element at the specified descendant index.
        /// </summary>
        /// <param name="index">Index to insert into.</param>
        /// <param name="value">Value to insert.</param>
        public abstract IElement InsertDescendant(string value, int index);

        /// <summary>
        ///     Move indices forward in preparation for insert.
        /// </summary>
        /// <typeparam name="TDescendant">Type of descendant element.</typeparam>
        /// <param name="cache">Cache to modify.</param>
        /// <param name="index">Descendant index.</param>
        static private void ShiftForInsert<TDescendant>(IIndexedCache<int, TDescendant> cache, int index) where TDescendant : Builder
        {
            var values = cache.ToList();
            foreach (var value in values.Where(v => v.Key >= index))
            {
                value.Value.Index++;
            }
            cache.Clear();
            foreach (var value in values)
            {
                cache[value.Value.Index] = value.Value;
            }
        }

        /// <summary>
        ///     Insert a descendant element at the specified index.
        /// </summary>
        /// <typeparam name="TDescendant">Type of descendant element.</typeparam>
        /// <param name="cache">Cache to delete within.</param>
        /// <param name="index">Descendant index to delete.</param>
        /// <param name="value">Value to insert.</param>
        static protected IBuilder InsertDescendant<TDescendant>(IIndexedCache<int, TDescendant> cache, int index, string value) where TDescendant : Builder
        {
            ShiftForInsert(cache, index);
            cache[index].Value = value;
            return cache[index];
        }

        /// <summary>
        ///     Insert a descendant element string at the specified index.
        /// </summary>
        /// <typeparam name="TDescendant">Type of descendant element.</typeparam>
        /// <param name="cache">Cache to delete within.</param>
        /// <param name="index">Descendant index to delete.</param>
        /// <param name="element">Element to insert.</param>
        static protected IBuilder InsertDescendant<TDescendant>(IIndexedCache<int, TDescendant> cache, int index, IElement element) where TDescendant : Builder
        {
            ShiftForInsert(cache, index);
            element.CopyTo(cache[index]);
            return cache[index];
        }

        /// <summary>
        ///     Move a descendant element to another index.
        /// </summary>
        /// <param name="sourceIndex">Index to move from.</param>
        /// <param name="targetIndex">Index to move to.</param>
        public abstract void MoveDescendant(int sourceIndex, int targetIndex);

        /// <summary>
        ///     Move a descendant element to another index within the cache provided.
        /// </summary>
        /// <typeparam name="TDescendant">Type of descendant element.</typeparam>
        /// <param name="cache">Cache to move within.</param>
        /// <param name="source">Source index.</param>
        /// <param name="target">Target index.</param>
        static protected void MoveDescendant<TDescendant>(IIndexedCache<int, TDescendant> cache, int source, int target) where TDescendant : Builder
        {
            var values = cache.ToList();
            var sourceValue = cache[source];

            foreach (var value in values.Where(v => v.Key > source))
            {
                value.Value.Index--;
            }
            foreach (var value in values.Where(v => v.Key > target))
            {
                value.Value.Index++;
            }

            sourceValue.Index = target;
            cache.Clear();
            foreach (var value in values)
            {
                cache[value.Value.Index] = value.Value;
            }
        }
    }
}