using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Mapping
{
    public class MapConfiguration<TKey> : IMapConfiguration<TKey>
    {
        private readonly Dictionary<TKey, MapLocation> _map = new Dictionary<TKey, MapLocation>();

        public void Add(TKey key, MapLocation value)
        {
            _map.Add(key, value);
        }

        public void Add(TKey key, string value)
        {
            
        }

        public bool ContainsKey(TKey key)
        {
            return _map.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _map.Keys; }
        }

        public bool Remove(TKey key)
        {
            return _map.Remove(key);
        }

        public bool TryGetValue(TKey key, out MapLocation value)
        {
            return _map.TryGetValue(key, out value);
        }

        public ICollection<MapLocation> Values
        {
            get { return _map.Values; }
        }

        public MapLocation this[TKey key]
        {
            get { return _map[key]; }
            set { _map[key] = value; }
        }

        public void Add(KeyValuePair<TKey, MapLocation> item)
        {
            _map.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _map.Clear();
        }

        public bool Contains(KeyValuePair<TKey, MapLocation> item)
        {
            return _map.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, MapLocation>[] array, int arrayIndex)
        {
            foreach (var item in _map)
            {
                array[arrayIndex++] = item;
            }
        }

        public int Count
        {
            get { return _map.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, MapLocation> item)
        {
            return _map.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, MapLocation>> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator();
        }
    }
}
