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
    public class ExternallyManagedUnitOfWork : UnitOfWork, IDisposable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private bool disposedValue;
        public ExternallyManagedUnitOfWork(SqlTransaction sqlTransaction = null) : base()
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        public override void Begin()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public override void Commit()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public override void AddToCache(Type type, int cacheKey, IEntityCacheData entityCacheData)
        {
            log.LogMethodEntry(type, cacheKey, entityCacheData);
            log.LogMethodExit();
        }

        public override void RollBack()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Disposed the unit of work
        /// </summary>
        public void Dispose()
        {
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
                }

                disposedValue = true;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get method of sql transaction
        /// </summary>
        public override SqlTransaction SQLTrx
        {
            get
            {
                return sqlTransaction;
            }
        }

        public override T GetCacheData<T>(Type type, int cacheKey)
        {
            log.LogMethodEntry(type, cacheKey);
            T result = default(T);
            log.LogMethodExit(result);
            return result;
        }

        public override void Track(ICacheableEntity cacheableEntity)
        {
            log.LogMethodEntry(cacheableEntity);
            log.LogMethodExit();
        }

        public override void RemoveFromCache(string entity, ICacheableEntity cacheableEntity)
        {
            log.LogMethodEntry(entity, cacheableEntity);
            log.LogMethodExit();
        }
    }

    
}
