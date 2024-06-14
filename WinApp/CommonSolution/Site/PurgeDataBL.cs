/********************************************************************************************
 * Project Name - Site
 * Description  - Business Logic for Purge Data
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        08-May-2019   Mushahid Faizan         Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Site
{
    public class PurgeDataBL
    {
        PurgeDataDTO purgeDataDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parametrized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public PurgeDataBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.purgeDataDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates PurgeDataBL object using the purgeDataDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="purgeDataDTO"></param>
        public PurgeDataBL(ExecutionContext executionContext, PurgeDataDTO purgeDataDTO)
        {
            log.LogMethodEntry(executionContext, purgeDataDTO);
            this.purgeDataDTO = purgeDataDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        public void Save(SqlTransaction sqltransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqltransaction);
                PurgeDataHandler purgeDataHandler = new PurgeDataHandler(sqltransaction);
                log.LogMethodExit();
                purgeDataHandler.PurgeData(purgeDataDTO);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }
    }
}
