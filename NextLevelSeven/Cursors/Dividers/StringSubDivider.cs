using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors.Dividers
{
    sealed internal class StringSubDivider : IStringDivider
    {
        public StringSubDivider(IStringDivider baseDivider, char delimiter, int parentIndex)
        {
            BaseDivider = baseDivider;
            Index = parentIndex;
            Delimiter = delimiter;
        }

        public StringSubDivider(IStringDivider baseDivider, int baseDividerOffset, int parentIndex)
        {
            BaseDivider = baseDivider;
            Index = parentIndex;
            Delimiter = baseDivider.Value[baseDividerOffset];
        }

        public string this[int index]
        {
            get
            {
                var currentValue = Value;
                if (currentValue == null)
                {
                    return null;
                }

                var splits = Divisions;
                if (index >= splits.Count || index < 0)
                {
                    return null;
                }

                var split = splits[index];
                return currentValue.Substring(split.Offset, split.Length);
            }
            set
            {
                if (index >= 0)
                {
                    List<StringDivision> divisions;
                    var paddedString = StringDivider.GetPaddedString(Value, index, Delimiter, out divisions);
                    var d = divisions[index];
                    Value = StringDivider.GetSplicedString(paddedString, d.Offset, d.Length, value);
                }
            }
        }

        IStringDivider BaseDivider
        {
            get;
            set;
        }

        public int Count
        {
            get { return Divisions.Count; }
        }

        public char Delimiter
        {
            get;
            private set;
        }

        public IStringDivider Divide(int index, char delimiter)
        {
            return new StringSubDivider(this, delimiter, index);
        }

        private IReadOnlyList<StringDivision> _divisions;
        public IReadOnlyList<StringDivision> Divisions
        {
            get
            {
                if (_divisions == null || Version != BaseDivider.Version)
                {
                    Update();
                }
                return _divisions;
            }
        }

        public int Index
        {
            get;
            set;
        }

        void Update()
        {
            Version = BaseDivider.Version;
            _divisions = StringDivider.GetDivisions(Value, Delimiter);            
        }

        public string Value
        {
            get { return BaseDivider[Index]; }
            set { BaseDivider[Index] = value; }
        }

        public int Version
        {
            get;
            private set;
        }

        public IEnumerator<string> GetEnumerator()
        {
            var value = Value;
            return Divisions.Select(d => value.Substring(d.Offset, d.Length)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Value;
        }

        public void Delete(int index)
        {
            throw new NotImplementedException();
        }

        public void InsertAfter(int index, string value = null)
        {
            throw new NotImplementedException();
        }

        public void InsertBefore(int index, string value = null)
        {
            throw new NotImplementedException();
        }
    }
}
