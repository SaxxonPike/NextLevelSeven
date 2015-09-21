using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>
    ///     A string divider that passes its value through rather than doing any splitting.
    /// </summary>
    internal sealed class ProxyStringDivider : IStringDivider
    {
        public ProxyStringDivider()
        {
            string proxyString = null;
            GetValue = () => proxyString;
            SetValue = v => proxyString = v;
        }

        public ProxyStringDivider(ProxyGetter<string> getValue, ProxySetter<string> setValue)
        {
            GetValue = getValue;
            SetValue = setValue;
        }

        private ProxyGetter<string> GetValue { get; set; }
        private ProxySetter<string> SetValue { get; set; }
        public event EventHandler ValueChanged;

        public string this[int index]
        {
            get { return Value; }
            set { Value = value; }
        }

        public char[] BaseValue
        {
            get { return ValueChars; }
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

        public int Index
        {
            get { return 0; }
            set { }
        }

        public bool IsNull
        {
            get { return Value == null; }
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public StringDivision GetSubDivision(int index)
        {
            return new StringDivision(0, Value.Length);
        }

        public char[] ValueChars
        {
            get { return GetValue().ToCharArray(); }
            set
            {
                if (ValueChanged != null)
                {
                    ValueChanged(this, EventArgs.Empty);
                }
                SetValue(new String(value));
            }
        }

        public void Delete(int index)
        {
        }

        public void Delete(StringDivision division)
        {
        }
    }
}