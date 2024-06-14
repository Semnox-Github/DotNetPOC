/********************************************************************************************
 * Project Name - Site
 * Description  - Data Handler Class for Purge Data
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
    /// <summary>
    /// PurgeDataHandler class to get all the details about Purge Data
    /// </summary>
    public class PurgeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of PurgeDataHandler class
        /// </summary>
        public PurgeDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for Purge Record.
        /// </summary>
        /// <param name="purgeDataDTO">purgeDataDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(PurgeDataDTO purgeDataDTO)
        {
            log.LogMethodEntry(purgeDataDTO);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@date", purgeDataDTO.Date));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Cardsdate", purgeDataDTO.CardsDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Gameplaydate", purgeDataDTO.Gameplaydate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Transactionsdate", purgeDataDTO.TransactionsDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Logsdate", purgeDataDTO.LogsDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@balance", purgeDataDTO.Balance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@manualPurge", purgeDataDTO.ManualPurge));
            log.LogMethodExit(parameters);
            return parameters;
        }

        public int PurgeData(PurgeDataDTO purgeDataDTO)
        {
            log.LogMethodEntry(purgeDataDTO);
            int purgeStatus = 0;
            try
            {
                string purgeQuery = @"exec PurgeOldData @ManualPurgeDate=@date , @CardsPurgedate=@Cardsdate," +
                                    "@GameplayPurgedate=@Gameplaydate,@TransactionsPurgedate=@Transactionsdate," +
                                    "@LogsPurgedate=@Logsdate,@credits=@balance,@ManualPurgeData = @manualPurge";

                purgeStatus = dataAccessHandler.executeUpdateQuery(purgeQuery, GetSQLParameters(purgeDataDTO).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(purgeStatus);
            return purgeStatus;
        }
    }
}