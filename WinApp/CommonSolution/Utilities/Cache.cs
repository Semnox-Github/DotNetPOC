/********************************************************************************************
 * Project Name - Utilities
 * Description  - LFU cache implementation. No of items to be held can be capped at max capacity. these values can be configured in configuration file
 * This allows changing the capacity based on the system load. 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Jan-2021       Lakshminarayana           Created : POS UI Redesign with REST API
 2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Cache to hold the resources. No of items to be held can be capped at max capacity. 
    /// these values can be configured in configuration file This allows changing the capacity based on the system load.
    /// </summary>
    /// <typeparam name="TKey">key</typeparam>
    /// <typeparam name="TValue">item to be cached</typeparam>
    public class Cache<TKey, TValue>
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ConcurrentDictionary<TKey, CacheItem<TKey, TValue>> dictionary;
        private readonly int maxCapacity;
        private string valueTypeName;
        private readonly object locker = new object();
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="parameterMaxCapacity"></param>
        public Cache(int parameterMaxCapacity = 50)
        {
            valueTypeName = typeof(TValue).Name;
            this.maxCapacity = GetMaxCapacity(parameterMaxCapacity);
            dictionary = new ConcurrentDictionary<TKey, CacheItem<TKey, TValue>>();
        }

        /// <summary>
        /// "ContainerListCapacity" app setting can be used to change the capacity of all the caches
        /// capacity of the individual caches can be controlled using adding valueTypeName+"ListCapacity"
        /// (i.e let say we are holding ParafaitDefaultContainerList inside the cache, to limit the cache size to 10, we can add
        /// a config with name "ParafaitDefaultContainerListCapacity" and value "10")
        /// </summary>
        /// <param name="parameterMaxCapacity"></param>
        /// <returns></returns>
        private int GetMaxCapacity(int parameterMaxCapacity)
        {
            int value;
            string config = ConfigurationManager.AppSettings[valueTypeName + "ListCapacity"];
            if (string.IsNullOrWhiteSpace(config))
            {
                config = ConfigurationManager.AppSettings["ContainerListCapacity"];
            }
            if (int.TryParse(config, out value) == false)
            {
                value = parameterMaxCapacity;
            }
            if (value < 0)
            {
                value = parameterMaxCapacity;
            }
            return value;
        }

        /// <summary>
        /// Gets a collection containing the keys in the dictionary
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                return dictionary.Keys;
            }

        }

        /// <summary>
        ///  Adds a key/value pair to the dictionary
        ///   by using the specified function, if the key does not already exist.
        ///   This method should be used to add new value to the cache and the creation of the 
        ///   value is costly. it makes sure that the resource is created only once 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            if (maxCapacity == 0)
            {
                return valueFactory(key);
            }
            CacheItem<TKey, TValue> value;
            if(dictionary.TryGetValue(key, out value))
            {
                return value.Value;
            }
            lock (locker)
            {
                log.LogVariableState("New key", key);
                value = dictionary.GetOrAdd(key, (k) => new CacheItem<TKey, TValue>(key, valueFactory));
                RemoveExcessItems();
                return value.Value;
            }
        }

        /// <summary>
        ///  Attempts to remove and return the value that has the specified key from the cache 
        /// </summary>
        /// <param name="key">The key of the element to remove and return.</param>
        /// <param name="value">When this method returns, contains the object removed from the cache or the default value of the TValue type if key does not exist</param>
        /// <returns>true if the object was removed successfully; otherwise, false.</returns>
        public bool TryRemove(TKey key, out TValue value)
        {
            lock (locker)
            {
                CacheItem<TKey, TValue> cacheItem;
                bool result = dictionary.TryRemove(key, out cacheItem);
                value = result ? cacheItem.Value : default(TValue);
                return result;
            } 
        }

        /// <summary>
        /// Attempts to get the value associated with the specified key from the cache.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the object from the cache
        ///                     that has the specified key, or the default value of the type if the operation failed.</param>
        /// <returns>true if the key was found in the cache otherwise, false.</returns>

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (maxCapacity == 0)
            {
                value = default(TValue);
                return false;
            }
            CacheItem<TKey, TValue> cacheItem;
            if(dictionary.TryGetValue(key, out cacheItem))
            {
                value = cacheItem.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// Sets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key of the value to set</param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            set
            {
                if (maxCapacity == 0)
                {
                    return;
                }
                lock (locker)
                {
                    log.LogVariableState("New key", key);
                    dictionary.AddOrUpdate(key, new CacheItem<TKey, TValue>(key, value), (k, v) => new CacheItem<TKey, TValue>(key, value));
                    RemoveExcessItems();
                }
            }
        }

        /// <summary>
        /// removes the excess items from the cache
        /// </summary>
        private void RemoveExcessItems()
        {
            if (dictionary.Count < maxCapacity)
            {
                return;
            }
            if (log.IsDebugEnabled)
            {
                log.LogVariableState("ValueTypeName", valueTypeName);
                log.LogVariableState(valueTypeName, dictionary.Skip(0).Select(x => "Key:" + x.Key + "Count:" + x.Value.AccessCount));

            }
            while (dictionary.Count > maxCapacity)
            {
                TKey keyToBeRemoved = dictionary.Values.Min().Key;
                log.LogVariableState("Key to be removed", keyToBeRemoved);
                CacheItem<TKey, TValue> valueToBeRemoved;
                if (dictionary.TryRemove(keyToBeRemoved, out valueToBeRemoved))
                {
                    if (log.IsDebugEnabled)
                    {
                        log.LogVariableState("usageCount", valueToBeRemoved.AccessCount);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Represents a item in the cache. this holds the access count which will be used to remove excess items from the cache.
    /// Cache evictions is based on LFU algorithm
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    class CacheItem<TKey, TValue> : IComparable<CacheItem<TKey, TValue>>
    {
        // 'GetOrAdd' call on the dictionary is not thread safe and we might end up creating the CacheItem more than
        // once. To prevent this Lazy<> is used. In the worst case multiple Lazy<> objects are created for multiple
        // threads but only one of the objects succeeds in creating a TValue.
        private Lazy<TValue> lazyValue;
        private TKey key;
        private long accessCount;

        /// <summary>
        /// Constructor with value factory. actual value is not created during the construction
        /// When the value is access the lazy object will create the value
        /// this will make sure we create the TValue only once in a multi threaded environment
        /// This constructor should be used when creating the TValue is costly
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="valueFactory">delegate to create the value</param>
        public CacheItem(TKey key, Func<TKey, TValue> valueFactory)
        {
            this.key = key;
            lazyValue = new Lazy<TValue>(() => valueFactory(key));
            accessCount = 0;
        }

        /// <summary>
        /// This constructor can be used when the value is already constructed 
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">item to be cached</param>
        public CacheItem(TKey key, TValue value)
        {
            this.key = key;
            lazyValue = new Lazy<TValue>(() => value);
            accessCount = 0;
        }

        /// <summary>
        /// Returns the key
        /// </summary>
        public TKey Key
        {
            get
            {
                return key;
            }
        }
        /// <summary>
        /// returns the value. also increments the access count
        /// </summary>
        public TValue Value
        {
            get
            {
                Interlocked.Increment(ref accessCount);
                return lazyValue.Value;
            }
        }

        /// <summary>
        /// returns the access count
        /// </summary>
        public long AccessCount
        {
            get
            {
                return accessCount;
            }
        }

        /// <summary>
        /// compares two cache items using access count
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CacheItem<TKey, TValue> other)
        {
            return accessCount.CompareTo(other.accessCount);
        }
    }
}
