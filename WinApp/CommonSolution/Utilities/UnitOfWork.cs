/********************************************************************************************
 * Project Name - Utilities
 * Description  - Represent a object responsible for handling SQL transaction and caching entities.
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     27-Oct-2021      Lakshminarayana     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Represent a object responsible for handling SQL transaction and caching entities.
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string connectionString;
        private SqlConnection sqlConnection;//remove
        private SqlTransaction sqlTransaction;
        private Dictionary<string, Dictionary<int, ICacheableEntity>> sessionEntityCache = new Dictionary<string, Dictionary<int, ICacheableEntity>>();
        private Dictionary<string, HashSet<int>> removeableEntities = new Dictionary<string, HashSet<int>>();
        private bool disposedValue;
        private bool isSqlTransactionCreated = false;
        public UnitOfWork() : this(GetConnectionStringFromConfiguration())
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private static string GetConnectionStringFromConfiguration()
        {
            log.LogMethodEntry();
            string result = "";
            try
            {
                result = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            }
            catch { }
            if (string.IsNullOrWhiteSpace(result))
            {
                result = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
            }
                
            log.LogMethodExit(result);
            return result;
        }

        public UnitOfWork(string connectionStringParameter)
        {
            log.LogMethodEntry(connectionStringParameter);
            if (string.IsNullOrWhiteSpace(connectionStringParameter))
            {
                string errorMessage = "Connection string is not specified";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new ParafaitApplicationException(errorMessage);
            }
            connectionString = StaticUtils.getParafaitConnectionString(connectionStringParameter);
            log.LogMethodExit();
        }

        public virtual void Begin()
        {
            log.LogMethodEntry();
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                isSqlTransactionCreated = true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the connection", ex);
                throw new ParafaitApplicationException("Error occurred while creating the connection", ex);
            }
            
            log.LogMethodExit();
        }

        public virtual void Commit()
        {
            log.LogMethodEntry();
            try
            {
                SQLTrx.Commit();
                Dispose();
            }
            catch (Exception ex)
            {
                string errorMessage = "Error occurred while committing the sql transaction";
                log.Error(errorMessage, ex);
                log.LogMethodExit("Throwing Exception- " + errorMessage);
                throw new ParafaitApplicationException(errorMessage, ex);
            }
            CacheEntities();
            log.LogMethodExit();
        }

        private void CacheEntities()
        {
            log.LogMethodEntry();
            foreach (var entity in sessionEntityCache.Keys)
            {
                foreach (var cacheKey in sessionEntityCache[entity].Keys)
                {
                    if (removeableEntities.ContainsKey(entity) && removeableEntities[entity].Contains(cacheKey))
                    {
                        ParafaitEntityCacheList.RemoveEntityCacheData(entity, cacheKey);
                        continue;
                    }
                    sessionEntityCache[entity][cacheKey].CacheData();
                }
            }
            log.LogMethodExit();
        }

        public virtual void AddToCache(Type type, int cacheKey, IEntityCacheData entityCacheData)
        {
            log.LogMethodEntry(type, cacheKey, entityCacheData);
            ParafaitEntityCacheList.AddEntityCacheData(type.AssemblyQualifiedName, cacheKey, entityCacheData);
            log.LogMethodExit();
        }

        public virtual void RollBack()
        {
            log.LogMethodEntry();
            try
            {
                SQLTrx.Rollback();
                Dispose();
            }
            catch (Exception ex)
            {
                string errorMessage = "Error occurred while rolling back the sql transaction";
                log.Error(errorMessage, ex);
                log.LogMethodExit("Throwing Exception- " + errorMessage);
                throw new ParafaitApplicationException(errorMessage, ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Disposed the unit of work
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (isSqlTransactionCreated == false)
                {
                    CacheEntities();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

            
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            log.LogMethodEntry(disposing);
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (sqlConnection != null)
                    {
                        try
                        {
                            sqlConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while closing the connection", ex);
                        }
                    }
                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Dispose();
                    }
                }

                disposedValue = true;
            }
            log.LogMethodExit();
        }

        public virtual SqlTransaction SQLTrx
        {
            get
            {
                return sqlTransaction;
            }
        }

        public virtual T GetCacheData<T>(Type type, int cacheKey) where T : class, IEntityCacheData
        {
            log.LogMethodEntry(type, cacheKey);
            string entity = type.AssemblyQualifiedName;
            if (IsCachedEntityExists(entity, cacheKey))
            {
                //This will execute if in the same session we are trying to create multiple instance of the same entity
                //This may lead to unknown behavior. we need to check whether the entity already created in the session, If so we can 
                // retrieve the reference by calling GetCachedEntity method.
                string errorMessage = "The entity is already created in the current session.";
                log.LogMethodExit("Throwing Exception -" + errorMessage);
                throw new ParafaitApplicationException(errorMessage);
            }
            T result = ParafaitEntityCacheList.GetEntityCacheData<T>(entity, cacheKey);
            log.LogMethodExit(result);
            return result;
        }

        public T GetCachedEntity<T>(Type type, int cacheKey) where T : class, ICacheableEntity
        {
            log.LogMethodEntry(type, cacheKey);
            string entity = type.AssemblyQualifiedName;
            T result;
            if (IsCachedEntityExists(entity, cacheKey) == false)
            {
                result = default(T);
                log.LogMethodExit(result);
                return result;
            }
            result = sessionEntityCache[entity][cacheKey] as T;
            log.LogMethodExit(result);
            return result;
        }

        private bool IsCachedEntityExists(string entity, int cacheKey)
        {
            log.LogMethodEntry(entity, cacheKey);
            if (sessionEntityCache.ContainsKey(entity) == false)
            {
                sessionEntityCache.Add(entity, new Dictionary<int, ICacheableEntity>());
            }
            bool result = sessionEntityCache[entity].ContainsKey(cacheKey);
            log.LogMethodExit(result);
            return result;
        }

        public virtual void Track(ICacheableEntity cacheableEntity)
        {
            log.LogMethodEntry(cacheableEntity);
            string entity = cacheableEntity.GetType().AssemblyQualifiedName;
            if (sessionEntityCache.ContainsKey(entity) == false)
            {
                sessionEntityCache.Add(entity, new Dictionary<int, ICacheableEntity>());
            }
            if(sessionEntityCache[entity].ContainsKey(cacheableEntity.CacheKey))
            {
                string errorMessage = "Duplicate entity created within a session";
                log.LogMethodExit("Throwing Exception -" + errorMessage);
                throw new ParafaitApplicationException(errorMessage);
            }
            sessionEntityCache[entity].Add(cacheableEntity.CacheKey, cacheableEntity);
            log.LogMethodExit();
        }

        public virtual void RemoveFromCache(string entity, ICacheableEntity cacheableEntity)
        {
            log.LogMethodEntry(entity, cacheableEntity);
            if (removeableEntities.ContainsKey(entity) == false)
            {
                removeableEntities.Add(entity, new HashSet<int>());
            }
            removeableEntities[entity].Add(cacheableEntity.CacheKey);
            log.LogMethodExit();
        }
    }

    
}
