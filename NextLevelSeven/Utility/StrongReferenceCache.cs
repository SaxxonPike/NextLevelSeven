using System.Collections.Generic;

namespace NextLevelSeven.Utility
{
    internal class StrongReferenceCache<TValue> : IndexedCache<TValue> where TValue : class
    {
        /// <summary>Create an indexed cache with strong references that uses the specified factory to generate items not already cached.</summary>
        /// <param name="factory"></param>
        public StrongReferenceCache(ProxyFactory<int, TValue> factory)
            : base(factory)
        {
        }

        /// <summary>Internal cache.</summary>
        protected readonly Dictionary<int, TValue> Cache = new Dictionary<int, TValue>();

        /// <summary>Get or set an item in the cache.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Item in the cache.</returns>
        public override TValue this[int index]
        {
            get
            {
                TValue value;
                if (Cache.TryGetValue(index, out value))
                {
                    return value;
                }
                value = Factory(index);
                Cache[index] = value;
                return value;
            }
            set => Cache[index] = value;
        }

        /// <summary>Returns true if the specified key exists in the cache.</summary>
        /// <param name="index">Index to search for.</param>
        /// <returns></returns>
        public override bool Contains(int index)
        {
            return Cache.ContainsKey(index);
        }

        /// <summary>Get the number of items in the cache.</summary>
        public override int Count => Cache.Count;

        /// <summary>Clear the cache.</summary>
        public override void Clear()
        {
            Cache.Clear();
        }

        /// <summary>Remove an item from the cache.</summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <returns>True, if removal was successful.</returns>
        public override bool Remove(int index)
        {
            return Cache.Remove(index);
        }

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        public override IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            return Cache.GetEnumerator();
        }
    }
}
