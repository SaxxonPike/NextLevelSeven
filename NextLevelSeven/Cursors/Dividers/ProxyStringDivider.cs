using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Cursors.Dividers
{
    sealed internal class ProxyStringDivider : IStringDivider
    {
        public ProxyStringDivider()
        {
            string proxyString = null;
            GetValue = () => proxyString;
            SetValue = v => proxyString = v;
        }

        public ProxyStringDivider(Func<string> getValue, Action<string> setValue)
        {
            GetValue = getValue;
            SetValue = setValue;
        }

        public string this[int index]
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        public int Count
        {
            get { return 1; }
        }

        public char Delimiter
        {
            get { return '\0'; }
        }

        public IStringDivider Divide(int index, char delimiter)
        {
            return new ProxyStringDivider(GetValue, SetValue);
        }

        public IReadOnlyList<StringDivision> Divisions
        {
            get { return new List<StringDivision>(new[] {new StringDivision(0, Value.Length)}); }
        }

        Func<string> GetValue
        {
            get;
            set;
        }

        public int Index
        {
            get { return 0; }
            set { }
        }

        Action<string> SetValue
        {
            get;
            set;
        }

        public string Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        public int Version
        {
            get { return 0; }
        }

        public IEnumerator<string> GetEnumerator()
        {
            var value = new[] {Value};
            return value.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Delete(int index)
        {
            throw new InvalidOperationException(ErrorMessages.Get(ErrorCode.DescendantElementsCannotBeModified));
        }

        public StringDivision GetSubDivision(int index)
        {
            return new StringDivision(0, Value.Length);
        }

        public string BaseValue
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }
    }
}
