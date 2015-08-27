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
        public event EventHandler ValueChanged;

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
                var splits = Divisions;
                if (index >= splits.Count || index < 0)
                {
                    return null;
                }

                var split = splits[index];
                return BaseValue.Substring(split.Offset, split.Length);
            }
            set
            {
                if (index >= 0)
                {
                    List<StringDivision> divisions;
                    var paddedString = StringDivider.GetPaddedString(Value, index, Delimiter, out divisions);
                    if (index >= divisions.Count)
                    {
                        if (index > 0)
                        {
                            Value = String.Join(paddedString, value);
                        }
                        else
                        {
                            Value = value;
                        }
                    }
                    else
                    {
                        var d = divisions[index];
                        Value = StringDivider.GetSplicedString(paddedString, d.Offset, d.Length, value);
                    }
                }
            }
        }

        IStringDivider BaseDivider
        {
            get;
            set;
        }

        public string BaseValue
        {
            get { return BaseDivider.BaseValue; }
        }

        public int Count
        {
            get { return Divisions.Count; }
        }

        public void Delete(int index)
        {
            throw new NotImplementedException();
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
            _divisions = StringDivider.GetDivisions(BaseValue, Delimiter, BaseDivider.GetSubDivision(Index));
        }

        public string Value
        {
            get 
            {
                var d = BaseDivider.GetSubDivision(Index);
                if (d == null)
                {
                    return null;
                }
                return BaseValue.Substring(d.Offset, d.Length);
            }
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

        public StringDivision GetSubDivision(int index)
        {
            if (index < 0)
            {
                return null;
            }

            var d = Divisions;
            if (index >= d.Count)
            {
                return null;
            }

            return d[index];
        }
    }
}
