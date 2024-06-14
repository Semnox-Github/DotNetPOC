using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Membership
{ 
    /// <summary>
    ///  Membership engine Data Handler - Handles insert, update runtime detail
     /// </summary>
    public class MembershipEngineDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private SqlTransaction sqlTransaction;
        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of MembershipEngineDataHandler class
        /// </summary>
        public MembershipEngineDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction); 
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Set/update  membership engine Run time
        /// </summary>
        /// <param name="runTime">runTime</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param> 
        /// <returns>Returns true or false </returns>
        public bool SetMembershipEngineRunTime(DateTime runTime, string userId, int siteId)
        {
            log.LogMethodEntry(runTime, userId, siteId);
            bool setRunTime = false;
            string query = @"SELECT 1 FROM EventLog 
                              where Source = 'MembershipEngine' and type='D' and Description ='MembershipEngineRunTime' 
                                and Category = 'MembershipEngine'
                                and (site_id =  @SiteId OR @SiteId = -1) ";
            List<SqlParameter> selectParam = new List<SqlParameter>();
            selectParam.Add(new SqlParameter("@SiteId", siteId));

            string runTimequery;
            int runTimeId = -1;
            try
            {
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, selectParam.ToArray(), sqlTransaction);
                if (dataTable.Rows.Count > 0)
                {
                    runTimequery = @"UPDATE EventLog  
                                        SET Data = @RunTime, 
                                            UserName =  @UserId, 
                                            Timestamp = GETDATE() 
                                    WHERE Source = 'MembershipEngine' and type='D' 
                                     and Description ='MembershipEngineRunTime' 
                                     and Category = 'MembershipEngine' 
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
        /// Get  membership engine Run time
        /// </summary> 
        public DateTime? GettMembershipEngineRunTime(string userId, int siteId)
        {
            log.LogMethodEntry(userId, siteId);
            DateTime? runTimeDate = null;
            try
            {
                int runTimeId = -1;
                string runTimequery;
                string query = @"SELECT data 
                                   FROM EventLog 
                                  where Source = 'MembershipEngine' and type='D' 
                                    and Description ='MembershipEngineRunTime' 
                                    and Category = 'MembershipEngine' 
                                    and ( site_id = @SiteId or @SiteId = -1) ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId));
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["data"].ToString() == "")
                        runTimeDate = null;
                    else
                        runTimeDate = Convert.ToDateTime(dataTable.Rows[0]["data"]);
                }
                else
                {
                    runTimequery = @"INSERT INTO [dbo].[EventLog] (
                                                                    [Source]
                                                                   ,[Timestamp]
                                                                   ,[Type]
                                                                   ,[Username]
                                                                   ,[Computer]
                                                                   ,[Data]
                                                                   ,[Description]
                                                                   ,[Category]
                                                                   ,[Severity] 
                                                                   ,[site_id]
                                                                   , createdBy, creationDate, LastUpdatedBy, LastUpdateDate )
                                                             VALUES
                                                                   ('MembershipEngine'
                                                                   ,GETDATE()
                                                                   ,'D'
                                                                   ,@UserId
                                                                   ,null
                                                                   ,null
                                                                   ,'MembershipEngineRunTime'
                                                                   ,'MembershipEngine'
                                                                   ,0 
                                                                   ,@SiteId,  'semnox', getdate(),'semnox', getdate()  ) 
                                                                   SELECT CAST(scope_identity() AS int) ";
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    if (siteId == -1)
                        sqlParameters.Add(new SqlParameter("@SiteId", DBNull.Value));
                    else
                        sqlParameters.Add(new SqlParameter("@SiteId", siteId));
                    sqlParameters.Add(new SqlParameter("@UserId", userId));
                    runTimeId = dataAccessHandler.executeInsertQuery(runTimequery, sqlParameters.ToArray(), sqlTransaction);
                    runTimeDate = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(runTimeDate, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(runTimeDate);
            return runTimeDate;
        }

    }
}
