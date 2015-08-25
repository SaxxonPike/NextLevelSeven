using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Codecs;
using NextLevelSeven.Cursors.Dividers;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Cursors
{
    abstract internal class Element : IElement
    {
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

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
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

        abstract protected char Delimiter { get; }

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
                    _descendantDivider = GetDescendantDivider(Ancestor, ParentIndex);
                }
                return _descendantDivider;
            }
            private set
            {
                _descendantDivider = value;
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
                    return Ancestor.Key + "." + Index;
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
                var value = DescendantDivider.Value;
                if (string.Equals("\"\"", value, StringComparison.Ordinal))
                {
                    return null;
                }
                return value;
            }
            set
            {
                DescendantDivider.Value = value;
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
