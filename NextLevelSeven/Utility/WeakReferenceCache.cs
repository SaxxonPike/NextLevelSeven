using System;
using System.Collections.Generic;
using System.Linq;

namespace NextLevelSeven.Utility
{
    internal class WeakReferenceCache<TValue> : IndexedCache<TValue> where TValue : class
    {
        /// <summary>Create an indexed cache with weak references that uses the specified factory to generate items not already cached.</summary>
        /// <param name="factory"></param>
        public WeakReferenceCache(ProxyFactory<int, TValue> factory)
            : base(factory)
        {
        }

        /// <summary>Internal cache.</summary>
        private readonly Dictionary<int, WeakReference<TValue>> _cache = new Dictionary<int, WeakReference<TValue>>();

        /// <summary>Get or set an item in the cache.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Item in the cache.</returns>
        public override TValue this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, value);
        }

        private TValue GetValue(int index)
        {
            WeakReference<TValue> reference;
            TValue cachedValue;
            if (_cache.TryGetValue(index, out reference))
            {
                if (reference.TryGetTarget(out cachedValue))
                {
                    return cachedValue;
                }
                cachedValue = Factory(index);
                reference.SetTarget(cachedValue);
                return cachedValue;
            }
            cachedValue = Factory(index);
            _cache[index] = new WeakReference<TValue>(cachedValue);
            return cachedValue;            
        }

        private void SetValue(int index, TValue value)
        {
            WeakReference<TValue> reference;
            if (_cache.TryGetValue(index, out reference))
            {
                _cache[index].SetTarget(value);
            }
            else
            {
                _cache[index] = new WeakReference<TValue>(value);
            }
        }

        /// <summary>Returns true if the specified key exists in the cache.</summary>
        /// <param name="index">Index to search for.</param>
        /// <returns></returns>
        public override bool Contains(int index)
        {
            return _cache.ContainsKey(index);
        }

        /// <summary>Get the number of items in the cache.</summary>
        public override int Count => _cache.Count;

        /// <summary>Clear the cache.</summary>
        public override void Clear()
        {
            _cache.Clear();
        }

        /// <summary>Remove an item from the cache.</summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <returns>True, if removal was successful.</returns>
        public override bool Remove(int index)
        {
            return _cache.Remove(index);
        }

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        public override IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            return _cache.Select(kv => new KeyValuePair<int, TValue>(kv.Key, GetValue(kv.Key))).GetEnumerator();
        }
    }
}
