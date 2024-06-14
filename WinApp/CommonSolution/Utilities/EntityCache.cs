/********************************************************************************************
 * Project Name - Utilities
 * Description  - LFU cache implementation using last accessed time. No of items to be held can be capped at max capacity. these values can be configured in configuration file
 * This allows changing the capacity based on the system load. 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Jan-2021       Lakshminarayana           Created : POS UI Redesign with REST API
 2.130.0     12-Jul-2021       Lakshminarayana           Modified : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Timers;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Cache to hold the resources. No of items to be held can be capped at max capacity. 
    /// these values can be configured in configuration file This allows changing the capacity based on the system load.
    /// </summary>
    /// <typeparam name="TKey">key</typeparam>
    /// <typeparam name="TValue">item to be cached</typeparam>
    public class EntityCache<TKey, TValue>
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ConcurrentDictionary<TKey, EntityCacheItem<TKey, TValue>> dictionary;
        private readonly int maxCapacity;
        private string valueTypeName;
        private readonly object locker = new object();
        private readonly Timer refreshTimer;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="parameterMaxCapacity"></param>
        public EntityCache(int parameterMaxCapacity = 1000, double parameterEvictionFrequency = 900000)
        {
            log.LogMethodEntry(parameterMaxCapacity, parameterEvictionFrequency);
            valueTypeName = typeof(TValue).Name;
            this.maxCapacity = GetMaxCapacity(parameterMaxCapacity);
            dictionary = new ConcurrentDictionary<TKey, EntityCacheItem<TKey, TValue>>();
            double evictionFrequency = GetEntityCacheEvictionFrequency(parameterEvictionFrequency);
            refreshTimer = new Timer(evictionFrequency);
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private double GetEntityCacheEvictionFrequency(double parameterEvictionFrequency)
        {
            log.LogMethodEntry(parameterEvictionFrequency);
            double result = EntityCacheEvictionFrequency.GetValue();
            if (result < 60 * 1000)
            {
                result = parameterEvictionFrequency;
            }
            if(result < 60 * 1000)
            {
                result = 60 * 1000;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the entity cache capacity from the configuration, If no configuration exists, parameter capacity is returned. If the parameter capacity is less than zero capacity is set to 1000
        /// </summary>
        /// <param name="parameterMaxCapacity"></param>
        /// <returns></returns>
        private int GetMaxCapacity(int parameterMaxCapacity)
        {
            int value;
            string config = ConfigurationManager.AppSettings["EntityCacheCapacity"];
            if (int.TryParse(config, out value) == false)
            {
                value = parameterMaxCapacity;
            }
            if (value < 0)
            {
                value = parameterMaxCapacity;
            }
            if(value < 0)
            {
                value = 1000;
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
        ///  Attempts to remove and return the value that has the specified key from the cache 
        /// </summary>
        /// <param name="key">The key of the element to remove and return.</param>
        /// <param name="value">When this method returns, contains the object removed from the cache or the default value of the TValue type if key does not exist</param>
        /// <returns>true if the object was removed successfully; otherwise, false.</returns>
        public bool TryRemove(TKey key, out TValue value)
        {
            lock (locker)
            {
                EntityCacheItem<TKey, TValue> cacheItem;
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
            EntityCacheItem<TKey, TValue> cacheItem;
            if (dictionary.TryGetValue(key, out cacheItem))
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
                    dictionary.AddOrUpdate(key, new EntityCacheItem<TKey, TValue>(key, value), (k, v) => new EntityCacheItem<TKey, TValue>(key, value));
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
                log.LogVariableState(valueTypeName, dictionary.Skip(0).Select(x => "Key:" + x.Key + "AccessTime:" + x.Value.AccessTime));

            }
            while (dictionary.Count > maxCapacity)
            {
                TKey keyToBeRemoved = dictionary.Values.Min().Key;
                log.LogVariableState("Key to be removed", keyToBeRemoved);
                EntityCacheItem<TKey, TValue> valueToBeRemoved;
                if (dictionary.TryRemove(keyToBeRemoved, out valueToBeRemoved))
                {
                    if (log.IsDebugEnabled)
                    {
                        log.LogVariableState("AccessTime", valueToBeRemoved.AccessTime);
                    }
                }
            }
        }
        private void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            RemoveEntiresOlderThanMinutes(EntityCacheEvictionFrequency.GetValueInMinutes());
        }

        private void RemoveEntiresOlderThanMinutes(double threshouldInMinutes)
        {
            lock (locker)
            {
                DateTime thresholdTime = DateTime.Now.AddMinutes(-Math.Abs(threshouldInMinutes));
                List<EntityCacheItem<TKey, TValue>> valuesToBeRemoved = new List<EntityCacheItem<TKey, TValue>>();
                foreach (var value in dictionary.Values)
                {
                    if(value.AccessTime < thresholdTime)
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.LogVariableState("Key to be removed", value.Key);
                            log.LogVariableState("AccessTime", value.AccessTime);
                        }
                        valuesToBeRemoved.Add(value);
                    }
                }
                foreach (var value in valuesToBeRemoved)
                {
                    EntityCacheItem<TKey, TValue> valueToBeRemoved;
                    if (dictionary.TryRemove(value.Key, out valueToBeRemoved))
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("Successfully to removed " + valueTypeName + " " + value.Key + " AccessTime " + value.AccessTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        }
                    }
                    else
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("Failed to removed " + valueTypeName + " " + value.Key + " AccessTime " + value.AccessTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        }
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
    class EntityCacheItem<TKey, TValue> : IComparable<EntityCacheItem<TKey, TValue>>
    {
        // 'GetOrAdd' call on the dictionary is not thread safe and we might end up creating the CacheItem more than
        // once. To prevent this Lazy<> is used. In the worst case multiple Lazy<> objects are created for multiple
        // threads but only one of the objects succeeds in creating a TValue.
        private TValue value;
        private TKey key;
        private DateTime lastAccessTime;

        /// <summary>
        /// This constructor can be used when the value is already constructed 
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">item to be cached</param>
        public EntityCacheItem(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
            lastAccessTime = DateTime.Now;
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
                return value;
            }
        }

        /// <summary>
        /// returns the access time
        /// </summary>
        public DateTime AccessTime
        {
            get
            {
                return lastAccessTime;
            }
        }

        /// <summary>
        /// compares two cache items using access count
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(EntityCacheItem<TKey, TValue> other)
        {
            return lastAccessTime.CompareTo(other.lastAccessTime);
        }
    }
}
