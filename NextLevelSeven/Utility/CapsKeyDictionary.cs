using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    /// <summary>
    /// A dictionary that forces keys to be uppercase.
    /// </summary>
    /// <typeparam name="TItem">Type of values in the dictionary.</typeparam>
    internal class CapsKeyDictionary<TItem> : IDictionary<string, TItem>
    {
        private readonly Dictionary<string, TItem> _dictionary = new Dictionary<string, TItem>();

        public void Add(string key, TItem value)
        {
            _dictionary.Add(key.ToUpperInvariant(), value);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key.ToUpperInvariant());
        }

        public bool TryGetValue(string key, out TItem value)
        {
            return _dictionary.TryGetValue(key.ToUpperInvariant(), out value);
        }

        public ICollection<TItem> Values
        {
            get { return _dictionary.Values; }
        }

        public TItem this[string key]
        {
            get { return _dictionary[key.ToUpperInvariant()]; }
            set { _dictionary[key.ToUpperInvariant()] = value; }
        }

        public void Add(KeyValuePair<string, TItem> item)
        {
            _dictionary.Add(item.Key.ToUpperInvariant(), item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, TItem> item)
        {
            return _dictionary.Contains(new KeyValuePair<string, TItem>(item.Key.ToUpperInvariant(), item.Value));
        }

        public void CopyTo(KeyValuePair<string, TItem>[] array, int arrayIndex)
        {
            foreach (var kvp in _dictionary)
            {
                array[arrayIndex] = kvp;
                arrayIndex++;
            }
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, TItem> item)
        {
            return _dictionary.Remove(item.Key.ToUpperInvariant());
        }

        public IEnumerator<KeyValuePair<string, TItem>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }
    }
}
