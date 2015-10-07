using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

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
        protected readonly Dictionary<int, WeakReference<TValue>> Cache = new Dictionary<int, WeakReference<TValue>>();

        /// <summary>Get or set an item in the cache.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Item in the cache.</returns>
        override public TValue this[int index]
        {
            get { return GetValue(index); }
            set { SetValue(index, value); }
        }

        TValue GetValue(int index)
        {
            WeakReference<TValue> reference;
            TValue cachedValue;
            if (Cache.TryGetValue(index, out reference))
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
            Cache[index] = new WeakReference<TValue>(cachedValue);
            return cachedValue;            
        }

        void SetValue(int index, TValue value)
        {
            WeakReference<TValue> reference;
            if (Cache.TryGetValue(index, out reference))
            {
                Cache[index].SetTarget(value);
            }
            else
            {
                Cache[index] = new WeakReference<TValue>(value);
            }
        }

        /// <summary>Returns true if the specified key exists in the cache.</summary>
        /// <param name="index">Index to search for.</param>
        /// <returns></returns>
        override public bool Contains(int index)
        {
            return Cache.ContainsKey(index);
        }

        /// <summary>Get the number of items in the cache.</summary>
        override public int Count
        {
            get { return Cache.Count; }
        }

        /// <summary>Clear the cache.</summary>
        override public void Clear()
        {
            Cache.Clear();
        }

        /// <summary>Remove an item from the cache.</summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <returns>True, if removal was successful.</returns>
        override public bool Remove(int index)
        {
            return Cache.Remove(index);
        }

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        override public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            return Cache.Select(kv => new KeyValuePair<int, TValue>(kv.Key, GetValue(kv.Key))).GetEnumerator();
        }
    }
}
