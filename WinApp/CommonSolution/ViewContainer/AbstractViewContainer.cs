/********************************************************************************************
* Project Name - Utilities
* Description  - Base class of view containers. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0     12-Nov-2019   Lakshminarayana         Created 
********************************************************************************************/
using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Base class of view containers. 
    /// </summary>
    public abstract class AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime lastRefreshTime;
        private readonly object locker = new object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AbstractViewContainer()
        {
            log.LogMethodEntry();
            lastRefreshTime = DateTime.Now;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method for the lastRefreshTime fields
        /// </summary>
        protected DateTime LastRefreshTime
        {
            get
            {
                lock(locker)
                {
                    return lastRefreshTime;
                }
            }

            set
            {
                lock(locker)
                {
                    lastRefreshTime = value;
                }
            }
        }

        /// <summary>
        /// Creates the system user execution context
        /// </summary>
        /// <returns></returns>
        protected static ExecutionContext GetSystemUserExecutionContext()
        {
            log.LogMethodEntry();
            ExecutionContext result = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
            log.LogMethodExit(result);
            return result;
        }
    }
}
