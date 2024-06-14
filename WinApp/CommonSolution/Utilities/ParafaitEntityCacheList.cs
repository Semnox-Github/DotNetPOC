/********************************************************************************************
 * Project Name - Utilities
 * Description  - Entity cache list for hold entities in memory for faster access for business processes.
 * a separate cache is created for each of the entity.
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         08-Dec-2020       Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Concurrent;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Represents a list of entity caches in the parafait system. 
    /// Business entities can cache the data for faster access.
    /// </summary>
    public class ParafaitEntityCacheList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<string, EntityCache<int, IEntityCacheData>> cacheDictionary = new ConcurrentDictionary<string, EntityCache<int, IEntityCacheData>>();

        public static T GetEntityCacheData<T>(string entity, int key) where T : class, IEntityCacheData
        {
            log.LogMethodEntry(entity, key);
            EntityCache<int, IEntityCacheData> entityCache = cacheDictionary.GetOrAdd(entity, (k)=> { return new EntityCache<int, IEntityCacheData>();});
            IEntityCacheData result;
            if (entityCache.TryGetValue(key, out result) == false)
            {
                result = default(T);
                log.LogMethodExit(result, "Unable to find the cache data");
                return result as T;
            }
            IEntityCacheData clone = result.Clone();
            log.LogMethodExit(clone);
            return clone as T;
        }

        public static void AddEntityCacheData(string entity, int key, IEntityCacheData value)
        {
            log.LogMethodEntry(entity, key, value);
            EntityCache<int, IEntityCacheData> entityCache = cacheDictionary.GetOrAdd(entity, (k) => { return new EntityCache<int, IEntityCacheData>(); });
            entityCache[key] = value;
            log.LogMethodExit();
        }

        public static void RemoveEntityCacheData(string entity, int key)
        {
            log.LogMethodEntry(entity, key);
            EntityCache<int, IEntityCacheData> entityCache = cacheDictionary.GetOrAdd(entity, (k) => { return new EntityCache<int, IEntityCacheData>(); });
            IEntityCacheData entityCacheData;
            if(entityCache.TryRemove(key, out entityCacheData))
            {
                log.Debug("Removed data from the cache for entity " + entity + " key " + key);
            }
            else
            {
                log.Debug("Unable to remove data from the cache for entity " + entity + " key " + key);
            }
            log.LogMethodExit();
        }
    }
}
