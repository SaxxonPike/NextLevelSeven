using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors.Dividers
{
    sealed internal class StringDivider : IStringDivider
    {
        public StringDivider(string s, char delimiter)
        {
            Initialize(s, delimiter);
        }

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    return null;
                }
                if (Value == null)
                {
                    return null;
                }
                var d = Divisions[index];
                return Value.Substring(d.Offset, d.Length);
            }
            set
            {
                if (index >= 0)
                {
                    List<StringDivision> divisions;
                    var paddedString = GetPaddedString(Value, index, Delimiter, out divisions);
                    if (index >= divisions.Count)
                    {
                        if (index > 0)
                        {
                            Initialize(paddedString + Delimiter + value, Delimiter);
                        }
                        else
                        {
                            Initialize(value, Delimiter);
                        }
                    }
                    else
                    {
                        var d = divisions[index];
                        Initialize(GetSplicedString(paddedString, d.Offset, d.Length, value), Delimiter);                        
                    }
                }
            }
        }

        public int Count { get { return Divisions.Count; } }
        public char Delimiter { get; private set; }

        private IReadOnlyList<StringDivision> _divisions;
        public IReadOnlyList<StringDivision> Divisions
        {
            get
            {
                if (_divisions == null)
                {
                    _divisions = GetDivisions(_value, Delimiter);
                }
                return _divisions;
            }
            private set
            {

            }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { Initialize(value, Delimiter); }
        }

        public IStringDivider Divide(int index, char delimiter)
        {
            return new StringSubDivider(this, delimiter, index);
        }

        static public List<StringDivision> GetDivisions(string s, char delimiter)
        {
            unchecked
            {
                var divisions = new List<StringDivision>();
                var length = 0;
                var offset = 0;

                if (s == null)
                {
                    s = string.Empty;
                }

                var inputLength = s.Length;

                if (delimiter == '\0')
                {
                    divisions.Add(new StringDivision(0, inputLength));
                    return divisions;
                }

                for (var index = 0; index < inputLength; index++)
                {
                    if (s[index] == delimiter)
                    {
                        divisions.Add(new StringDivision(offset, length));
                        length = 0;
                        offset = index + 1;
                    }
                    else
                    {
                        length++;
                    }
                }

                divisions.Add(new StringDivision(offset, length));
                return divisions;
            }
        }

        static public string GetPaddedString(string s, int index, char delimiter, out List<StringDivision> divisions)
        {
            unchecked
            {
                divisions = GetDivisions(s, delimiter);

                if (delimiter == '\0')
                {
                    return s;
                }

                if (s == null)
                {
                    s = string.Empty;
                }

                var divisionCount = divisions.Count;
                var stringLength = s.Length;
                var builder = new StringBuilder(s);

                while (divisionCount <= index)
                {
                    divisions.Add(new StringDivision(stringLength + 1, 0));
                    divisionCount++;
                    stringLength++;
                    builder.Append(delimiter);
                }

                return builder.ToString();
            }
        }

        static public string GetSplicedString(string s, int offset, int length, string replacement)
        {
            unchecked
            {
                var builder = new StringBuilder();
                if (offset > 0)
                {
                    builder.Append(s.Substring(0, offset));
                }
                builder.Append(replacement);
                if (s != null && (offset + length < s.Length))
                {
                    builder.Append(s.Substring(offset + length));
                }
                return builder.ToString();
            }
        }

        public int Index
        {
            get { return 0; }
            set { }
        }

        void Initialize(string s, char delimiter)
        {
            _divisions = null;
            _value = s;
            Delimiter = delimiter;
            Version++;                
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

        public int Version
        {
            get;
            private set;
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
