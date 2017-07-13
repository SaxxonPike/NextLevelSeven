using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Base class for message builders.</summary>
    internal abstract class Builder : IElementBuilder
    {
        /// <summary>Get the encoding used by this builder.</summary>
        public readonly IEncoding Encoding;

        /// <summary>Initialize the message builder base class.</summary>
        internal Builder()
        {
            Encoding = new BuilderEncodingConfiguration(this);
        }

        /// <summary>Initialize the message builder base class.</summary>
        /// <param name="config">Message's encoding configuration.</param>
        /// <param name="index">Index in the parent.</param>
        internal Builder(IEncoding config, int index)
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
        public virtual IEncodedTypeConverter Converter => new EncodedTypeConverter(this);

        /// <summary>Get the number of sub-values in this element.</summary>
        public abstract int ValueCount { get; }

        /// <summary>Get this element's section delimiter.</summary>
        public abstract char Delimiter { get; }

        /// <summary>Get the descendant builder at the specified index.</summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Descendant builder.</returns>
        public IElement this[int index] => GetGenericElement(index);

        /// <summary>Get the ancestor element. Null if it's a root element.</summary>
        IElement IElement.Ancestor => GetAncestor();

        /// <summary>Get descendant elements. For subcomponents, this will be empty.</summary>
        IEnumerable<IElement> IElement.Descendants => GetDescendants();

        /// <summary>Unique key of the element within the message.</summary>
        public string Key => ElementOperations.GetKey(this);

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
        IReadOnlyEncoding IElement.Encoding => Encoding;

        /// <summary>Get the encoding used by this builder.</summary>
        IEncoding IElementBuilder.Encoding => Encoding;

        /// <summary>Delete a descendant at the specified index.</summary>
        /// <param name="index">Index to delete at.</param>
        public void Delete(int index)
        {
            if (AssertIndexIsMovable(index))
            {
                DeleteDescendant(index);
            }
        }

        /// <summary>Insert a descendant element.</summary>
        /// <param name="element">Element to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public IElement Insert(int index, IElement element)
        {
            return AssertIndexIsMovable(index)
                ? InsertDescendant(index, element)
                : null;
        }

        /// <summary>Insert a descendant element string.</summary>
        /// <param name="value">Value to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public IElement Insert(int index, string value)
        {
            return AssertIndexIsMovable(index)
                ? InsertDescendant(index, value)
                : null;
        }

        /// <summary>Move descendant to another index.</summary>
        /// <param name="sourceIndex">Source index.</param>
        /// <param name="targetIndex">Target index.</param>
        public void Move(int sourceIndex, int targetIndex)
        {
            if (AssertIndexIsMovable(sourceIndex) && AssertIndexIsMovable(targetIndex))
            {
                MoveDescendant(sourceIndex, targetIndex);
            }
        }

        /// <summary>
        ///     Get the message for this builder.
        /// </summary>
        public IMessage Message
        {
            get
            {
                var ancestor = GetAncestor();
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

        /// <summary>
        ///     Returns true if the index can be moved or deleted.
        /// </summary>
        /// <param name="index">Index to verify.</param>
        /// <returns></returns>
        protected virtual bool AssertIndexIsMovable(int index)
        {
            return true;
        }

        /// <summary>Get the element at the specified index as an IElement.</summary>
        /// <param name="index">Index at which to get the element.</param>
        /// <returns>Generic element.</returns>
        protected abstract IElement GetGenericElement(int index);

        /// <summary>Get this builder's contents as a string.</summary>
        /// <returns>Builder's contents.</returns>
        public sealed override string ToString()
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

        /// <summary>Delete a descendant element at the specified index.</summary>
        /// <param name="index">Descendant index to delete.</param>
        private void DeleteDescendant(int index)
        {
            var cache = GetCache();
            var values = cache.Where(c => c.Key != index).ToList();
            foreach (var builder in values.Where(v => v.Key > index).Select(value => value.Value))
            {
                builder.Index--;
            }
            cache.Clear();
            foreach (var value in values)
            {
                cache[value.Value.Index] = value.Value;
            }
        }

        /// <summary>Move indices forward in preparation for insert.</summary>
        /// <param name="cache">Cache to modify.</param>
        /// <param name="index">Descendant index.</param>
        private static void ShiftForInsert(IIndexedCache<Builder> cache, int index)
        {
            var values = cache.ToList();
            foreach (var builder in values.Where(v => v.Key >= index).Select(value => value.Value))
            {
                builder.Index++;
            }
            cache.Clear();
            foreach (var value in values)
            {
                cache[value.Value.Index] = value.Value;
            }
        }

        /// <summary>Insert a descendant element at the specified index.</summary>
        /// <param name="index">Descendant index to delete.</param>
        /// <param name="value">Value to insert.</param>
        private IElementBuilder InsertDescendant(int index,
            string value)
        {
            var cache = GetCache();
            ShiftForInsert(cache, index);
            cache[index].Value = value;
            return cache[index];
        }

        /// <summary>Insert a descendant element string at the specified index.</summary>
        /// <param name="index">Descendant index to delete.</param>
        /// <param name="element">Element to insert.</param>
        private IElementBuilder InsertDescendant(int index,
            IElement element)
        {
            var cache = GetCache();
            ShiftForInsert(cache, index);
            element.CopyTo(cache[index]);
            return cache[index];
        }

        /// <summary>Move a descendant element to another index within the cache provided.</summary>
        /// <param name="source">Source index.</param>
        /// <param name="target">Target index.</param>
        private void MoveDescendant(int source, int target)
        {
            var cache = GetCache();
            var values = cache.ToList();
            var sourceValue = cache[source];

            foreach (var builder in values.Where(v => v.Key > source).Select(value => value.Value))
            {
                builder.Index--;
            }
            foreach (var builder in values.Where(v => v.Key > target).Select(value => value.Value))
            {
                builder.Index++;
            }

            if (sourceValue != null)
            {
                sourceValue.Index = target;
            }
            cache.Clear();
            foreach (var value in values)
            {
                cache[value.Value.Index] = value.Value;
            }
        }

        /// <summary>
        ///     Get an element's internal cache.
        /// </summary>
        /// <returns></returns>
        protected abstract IIndexedCache<Builder> GetCache();
    }
}