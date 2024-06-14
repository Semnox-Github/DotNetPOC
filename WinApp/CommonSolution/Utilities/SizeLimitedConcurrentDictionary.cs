/********************************************************************************************
 * Project Name - Utilities
 * Description  - Dictionary to hold the container. No of container to be held can be capped at max capacity. these values can be configured in configuration file
 * This allows changing the capacity based on the system load. 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Jan-2021       Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Dictionary to hold the container. No of container to be held can be capped at max capacity. 
    /// these values can be configured in configuration file This allows changing the capacity based on the system load.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SizeLimitedConcurrentDictionary<TKey, TValue>
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ConcurrentDictionary<TKey, TValue> dictionary;
        private readonly ConcurrentDictionary<TKey, decimal> accessCountDictionary;
        private int maxCapacity;
        private readonly object locker = new object();
        private string valueTypeName;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="parameterMaxCapacity"></param>
        public SizeLimitedConcurrentDictionary(int parameterMaxCapacity = 50)
        {
            log.LogMethodEntry(parameterMaxCapacity);
            valueTypeName = typeof(TValue).Name;
            this.maxCapacity = GetMaxCapacity(parameterMaxCapacity);
            dictionary = new ConcurrentDictionary<TKey, TValue>();
            accessCountDictionary = new ConcurrentDictionary<TKey, decimal>();
            log.LogMethodExit();
        }

        private int GetMaxCapacity(int parameterMaxCapacity)
        {
            log.LogMethodEntry(parameterMaxCapacity);
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
            log.LogMethodExit(value);
            return value;
        }

        /// <summary>
        /// Gets a collection containing the keys in the dictionary
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                lock (locker)
                {
                    return dictionary.Keys;
                }
            }

        }

        /// <summary>
        /// Sets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key of the value to set</param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                lock (locker)
                {
                    if (dictionary.ContainsKey(key) == false)
                    {
                        return default(TValue);
                    }
                    accessCountDictionary[key] = accessCountDictionary[key] + 1;
                    return dictionary[key];
                }
            }
            set
            {
                lock (locker)
                {
                    if(maxCapacity == 0)
                    {
                        log.LogMethodExit(0, "maxCapacity == 0");
                        return;
                    }
                    if (log.IsDebugEnabled)
                    {
                        log.LogVariableState("ValueTypeName", valueTypeName);
                        log.LogVariableState("Before List", accessCountDictionary.Select(x => "Key:" + x.Key + "Count:" + x.Value));
                        log.LogVariableState("New key", key);
                    }


                    if (dictionary.ContainsKey(key) == false)
                    {
                        log.Debug("Key doesn't exists in the dictionary");
                        if (dictionary.Count >= maxCapacity)
                        {
                            log.Debug("Max capacity exceeded in dictionary.");
                            while (dictionary.Count >= maxCapacity)
                            {
                                TKey keyToBeRemoved = accessCountDictionary.OrderBy(x => x.Value).First().Key;
                                log.LogVariableState("Key to be removed", keyToBeRemoved);
                                TValue valueToBeRemoved;
                                decimal usageCount;
                                if (dictionary.TryRemove(keyToBeRemoved, out valueToBeRemoved) == false ||
                                    accessCountDictionary.TryRemove(keyToBeRemoved, out usageCount) == false)
                                {
                                    string error = "Unable to remove a object of type " + valueTypeName + " from dictionary with key " + key.ToString();
                                    log.Error(error);
                                    throw new Exception(error);
                                }
                                else
                                {
                                    log.LogVariableState("value", valueToBeRemoved);
                                    log.LogVariableState("usageCount", usageCount);
                                }
                            }
                        }
                        accessCountDictionary[key] = 0;
                    }
                    else
                    {
                        accessCountDictionary[key] = accessCountDictionary[key] + 1; 
                    }
                    dictionary[key] = value;
                    
                    if (log.IsDebugEnabled)
                    {
                        log.LogVariableState("After List", accessCountDictionary.Select(x => "Key:" + x.Key + "Count:" + x.Value));
                    }
                    
                }
            }
        }
        
    }
}
