using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Codecs;
using NextLevelSeven.Cursors.Dividers;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Cursors
{
    abstract internal class Element : IElement, IEquatable<string>
    {
        public event EventHandler ValueChanged;

        protected Element(string value)
        {
            Index = 0;
            ParentIndex = 0;
            _descendantDivider = GetDescendantDividerRoot(value);
            Ancestor = null;
        }

        protected Element(Element ancestor, int parentIndex, int externalIndex)
        {
            Index = externalIndex;
            ParentIndex = parentIndex;
            Ancestor = ancestor;
        }

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

        public bool Equals(string other)
        {
            return ToString() == other;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        static public implicit operator string(Element element)
        {
            return element.ToString();
        }

        public IElement this[int index]
        {
            get { return GetDescendant(index); }
        }

        protected Element Ancestor { get; private set; }

        public IElement AncestorElement {
            get { return Ancestor; }
        }

        public ICodec As
        {
            get { return new Codec(this); }
        }

        public abstract IElement CloneDetached();

        public void Delete()
        {
            // TODO: actually delete
        }

        abstract public char Delimiter { get; }

        virtual public int DescendantCount
        {
            get { return DescendantDivider.Count; }
        }

        private IStringDivider _descendantDivider;
        private bool _descendantDividerInitialized;
        public IStringDivider DescendantDivider
        {
            get
            {
                if (_descendantDivider == null && !_descendantDividerInitialized)
                {
                    _descendantDividerInitialized = true;
                    _descendantDivider = (Ancestor == null)
                        ? GetDescendantDividerRoot(string.Empty)
                        : GetDescendantDivider(Ancestor, ParentIndex);
                }
                return _descendantDivider;
            }
        }

        public IEnumerable<IElement> DescendantElements
        {
            get { return new ElementEnumerable(this); }
        }

        abstract public EncodingConfiguration EncodingConfiguration { get; }

        public void Erase()
        {
            // TODO: actually mark as nonexistant
            Nullify();
        }

        public bool Exists
        {
            get
            {
                if (Ancestor == null)
                {
                    return true;
                }
                return (Index <= Ancestor.DescendantCount);
            }
        }

        public abstract IElement GetDescendant(int index);

        virtual protected IStringDivider GetDescendantDivider(Element ancestor, int index)
        {
            return new StringSubDivider(ancestor.DescendantDivider, Delimiter, index);
        }

        IStringDivider GetDescendantDividerRoot(string value)
        {
            return new StringDivider(value, Delimiter);
        }

        virtual public bool HasSignificantDescendants
        {
            get
            {
                if (!Exists)
                {
                    return false;
                }

                if (DescendantCount == 0)
                {
                    return false;
                }

                if (Delimiter == '\0')
                {
                    return false;
                }

                return (DescendantCount > 1) || DescendantElements.Any(d => d.HasSignificantDescendants);
            }
        }

        public int Index
        {
            get;
            set;
        }

        virtual public string Key
        {
            get
            {
                if (Ancestor != null)
                {
                    return String.Join(Ancestor.Key, ".", Index.ToString(CultureInfo.InvariantCulture));
                }
                return Index.ToString();
            }
        }

        public IMessage Message
        {
            get
            {
                if (Ancestor is Message)
                {
                    return new Core.Message(Ancestor as Message);
                }

                if (Ancestor != null)
                {
                    return Ancestor.Message;
                }
                return null;
            }
        }

        public void Nullify()
        {
            Value = null;
        }

        protected int ParentIndex
        {
            get;
            set;
        }

        public override string ToString()
        {
            return DescendantDivider.Value;
        }

        virtual public string Value
        {
            get
            {
                if (DescendantDivider == null)
                {
                    return null;
                }

                var value = DescendantDivider.Value;
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }

                if (string.Equals("\"\"", value, StringComparison.Ordinal))
                {
                    return null;
                }

                return value;
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

        public string[] Values
        {
            get
            {
                if (DescendantCount > 1)
                {
                    return DescendantDivider.Value.Split(DescendantDivider.Delimiter);
                }
                return new[] { DescendantDivider.Value };
            }
            set
            {
                if (DescendantDivider.Delimiter != '\0')
                {
                    DescendantDivider.Value = string.Join(new string(DescendantDivider.Delimiter, 1), value);
                }
                else
                {
                    DescendantDivider.Value = string.Join(string.Empty, value);
                }
            }
        }

        public IEnumerator<IElement> GetEnumerator()
        {
            return new ElementEnumerator<IElement>(DescendantDivider, GetDescendant);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ElementEnumerator<IElement>(DescendantDivider, GetDescendant);
        }
    }
}
