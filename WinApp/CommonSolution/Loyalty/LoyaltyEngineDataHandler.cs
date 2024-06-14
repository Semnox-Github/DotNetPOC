using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Loyalty
{

    /// <summary>
    ///  Loyalty engine Data Handler - Handles insert, update runtime detail
    /// </summary>
    public class LoyaltyEngineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of LoyaltyEngineDataHandler class
        /// </summary>
        public LoyaltyEngineDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Set/update  loyalty engine Run time
        /// </summary>
        /// <param name="runTime">runTime</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param> 
        /// <returns>Returns true or false </returns>
        public bool SetLoyaltyEngineRunTime(DateTime runTime, string userId, int siteId)
        {
            log.LogMethodEntry(runTime, userId, siteId);
            bool setRunTime = false;
           

            string runTimequery;
            int runTimeId = -1;
            try
            {
                string query = @"SELECT 1 FROM EventLog 
                              where Source = 'LoyaltyBatchEngine' and type='D' and Description ='LoyaltyBatchEngineRunTime' 
                                and Category = 'LoyaltyBatchEngine'
                                and (site_id =  @SiteId OR @SiteId = -1) ";
                List<SqlParameter> selectParam = new List<SqlParameter>();
                selectParam.Add(new SqlParameter("@SiteId", siteId));
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, selectParam.ToArray(), sqlTransaction);
                if (dataTable.Rows.Count > 0)
                {
                    runTimequery = @"UPDATE EventLog  
                                        SET Data = @RunTime, 
                                            UserName =  @UserId, 
                                            Timestamp = GETDATE() 
                                    WHERE Source = 'LoyaltyBatchEngine' and type='D' 
                                     and Description ='LoyaltyBatchEngineRunTime' 
                                     and Category = 'LoyaltyBatchEngine' 
                                     and (site_id =  @SiteId OR @SiteId = -1) ";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    sqlParameters.Add(new SqlParameter("@SiteId", siteId));
                    sqlParameters.Add(new SqlParameter("@UserId", userId));
                    sqlParameters.Add(new SqlParameter("@RunTime", runTime));
                    runTimeId = dataAccessHandler.executeUpdateQuery(runTimequery, sqlParameters.ToArray(), sqlTransaction);
                    setRunTime = true;
                }
                else
                {
                    throw new Exception("Run Time data storage setup is missing"); 
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(setRunTime, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(setRunTime);
            return setRunTime;
        }

        /// <summary>
        /// Get  LoyaltyEngine Run time
        /// </summary> 
        public DateTime? GettLoyaltyEngineRunTime(string userId, int siteId)
        {
            log.LogMethodEntry(userId, siteId);
            DateTime? runTimeDate = null;
            try
            {
                 string query = @"SELECT top 1 data 
                                   FROM EventLog 
                                  where Source = 'LoyaltyBatchEngine' and type='D' 
                                    and Description ='LoyaltyBatchEngineRunTime' 
                                    and Category = 'LoyaltyBatchEngine' 
                                    " + (siteId > -1? " and site_Id = @SiteId ": "");
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId));
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["data"].ToString() == "")
                    { throw new Exception("Run Time data storage setup is missing"); }
                    else
                        runTimeDate = Convert.ToDateTime(dataTable.Rows[0]["data"]);

                }
                else
                {
                    throw new Exception("Run Time data storage setup is missing");
                }

            } 
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(runTimeDate, "throwing exception");
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(runTimeDate);
            return runTimeDate;
        }
    }
}
