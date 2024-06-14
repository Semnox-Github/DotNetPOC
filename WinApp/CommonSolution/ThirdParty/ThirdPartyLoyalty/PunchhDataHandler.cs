/********************************************************************************************
* Project Name - Loyalty
* Description  - PunchhDataHandler - handler for the Punchh 
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    /// <summary>
    /// PunchhDataHandler
    /// </summary>
    public class PunchhDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// PunchhDataHandler
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public PunchhDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// GetTransactionDetails
        /// </summary>
        /// <returns></returns>
        public DataTable GetTransactionDetails( int siteId)
        {
            log.LogMethodEntry(siteId);
            DataTable dtLoyaltyTrxDetails = null;
            try
            {
                DateTime lastUpdateTime = GetLastUpdatedTime();
                DateTime endUpdateTime = ServerDateTime.Now;
                log.Debug("Last Synch Time: " + lastUpdateTime.ToString("dd-MMM-yyyy hh:mm:ss"));
                string trxQuery = @"SELECT TrxId, GUID, Convert(varchar, TrxDate, 112) TrxDate, 
                                                                                SiteCode, 
                                                                                LoyaltyCardNumber, 
                                                                                POSMachineId,
                                                                                (SELECT TOP 1 ProgramId 
			                                                                        FROM ConcurrentPrograms 
			                                                                        WHERE ProgramName = 'ThirdPartyLoyaltyProgram' 
			                                                                        AND Active=1
			                                                                        AND (site_id = @site_id or @site_id = -1)
			                                                                        ORDER BY LastUpdatedDate desc) LoyaltyProgramId
                                                                            FROM (
                                                                                select th.trxid, th.Guid, th.trxdate, siteInfo.SiteCode, 
                                                                                       th.External_System_Reference LoyaltyCardNumber , pos.POSMachineId
                                                                                    from trx_header th
																					    left outer join ( select opv.POSMachineId 
																						    from   parafait_defaults pd 
																							left outer join ParafaitOptionValues opv   on pd.default_value_id = opv.optionId 
																							and opv.activeFlag = 'Y' 
																							where pd.active_flag = 'Y' 
																							and opv.OptionId = (Select default_value_id from parafait_defaults 
																							                    where default_value_name ='LOYALTY_PROGRAM'
																												and default_value='PUNCHH'))POS
																							 on  pos.POSMachineId = th.POSMachineId,
	                                                                                    (select isnull(siteCode, site_id) siteCode
	                                                                                        from site 
                                                                                                  where (site_id = @site_id or @site_id = -1)
	                                                                                        ) siteInfo
                                                                                    where trxDate > @fromTime
                                                                                    and trxDate <= @toTime 	
                                                                                    and trxNetAmount > 0 and th.POSMachineId = pos.POSMachineId
                                                                                    and th.Status = 'CLOSED'
                                                                                    and th.External_System_Reference IS NOT NULL
                                                                                    and not exists 
		                                                                                (select 1 
			                                                                                from ConcurrentRequestDetails del
			                                                                                where ParafaitObjectGuid = th.Guid
			                                                                                and ParafaitObject = 'TrxId'
			                                                                                and ConcurrentProgramId = (SELECT TOP 1 ProgramId 
										                                                                                FROM ConcurrentPrograms 
										                                                                                WHERE ProgramName = 'ThirdPartyLoyaltyProgram' 
										                                                                                AND Active=1
										                                                                                AND (site_id = @site_id or @site_id = -1)
										                                                                                ORDER BY LastUpdatedDate desc)
			                                                                                and IsSuccessful = 1)
                                                                                UNION
                                                                                select th.trxid, th.Guid, th.trxdate, siteInfo.SiteCode, 
                                                                                       th.External_System_Reference LoyaltyCardNumber, 
                                                                                       pos.POSMachineId
                                                                                from trx_header th
                                                                                      left outer join ( select opv.POSMachineId 
																						    from   parafait_defaults pd 
																							left outer join ParafaitOptionValues opv   on pd.default_value_id = opv.optionId 
																							and opv.activeFlag = 'Y' 
																							where pd.active_flag = 'Y' 
																							and opv.OptionId = (Select default_value_id from parafait_defaults 
																							                    where default_value_name ='LOYALTY_PROGRAM'
																												and default_value='PUNCHH'))POS
																							 on  pos.POSMachineId = th.POSMachineId,
	                                                                                    (select siteCode 
	                                                                                        from site 
                                                                                            where (site_id = @site_id or @site_id = -1)
	                                                                                    ) siteInfo
                                                                                where
																				th.POSMachineId =  pos.POSMachineId  
																				 and th.Guid in 
                                                                                    (select ParafaitObjectGuid
                                                                                    from (select ParafaitObjectGuid, count(1) cnt
                                                                                        from ConcurrentRequestDetails ex1
                                                                                        where IsSuccessFul = 0
                                                                                        and ParafaitObject = 'TrxId'
                                                                                        and ConcurrentProgramId = (SELECT TOP 1 ProgramId 
									                                                                                FROM ConcurrentPrograms 
									                                                                                WHERE ProgramName = 'ThirdPartyLoyaltyProgram' 
									                                                                                AND Active=1
									                                                                                AND (site_id = @site_id or @site_id = -1)
									                                                                                ORDER BY LastUpdatedDate desc)
                                                                                        and not exists 
                                                                                            (select 1
                                                                                            from ConcurrentRequestDetails ex2
                                                                                            where ex2.ParafaitObjectGuid = ex1.ParafaitObjectGuid
                                                                                                and ex2.ParafaitObject = 'TrxId'
                                                                                                and ex1.ConcurrentProgramId = ex2.ConcurrentProgramId
                                                                                                and ex2.IsSuccessFul = 1)
                                                                                        group by ParafaitObjectGuid
                                                                                        having count(1) < 50) v)
	                                                                                ) A
                                                                        ORDER BY POSMachineId, TRXID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@site_id", siteId));
                parameters.Add(new SqlParameter("@fromTime", lastUpdateTime));
                parameters.Add(new SqlParameter("@toTime", endUpdateTime));
                dtLoyaltyTrxDetails = dataAccessHandler.executeSelectQuery(trxQuery, parameters.ToArray());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Debug("Failed to get transaction details at GetTransactionDetails() method");
            }
            log.LogMethodExit(dtLoyaltyTrxDetails);
            return dtLoyaltyTrxDetails;
        }

        internal DateTime GetLastUpdatedTime()
        {
            log.LogMethodEntry();
            DateTime lastUpdateTime = ServerDateTime.Now;
            try
            {
               string query  = @"SELECT top 1 ActualStartTime, RequestId
                                                             FROM ConcurrentRequests 
                                                            WHERE programId = (SELECT top 1 ProgramId 
                                                                                  FROM ConcurrentPrograms 
					                                                             WHERE ProgramName = 'ThirdPartyLoyaltyProgram' 
					                                                               AND Active=1)
                                                              AND Phase = 'Complete' 
                                                              AND status = 'Normal'
                                                           ORDER BY EndTime desc";
                DataTable logDT = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
                if (logDT != null && logDT.Rows.Count > 0 && logDT.Rows[0][0] != DBNull.Value)
                {
                    lastUpdateTime = Convert.ToDateTime((logDT.Rows[0][0]));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Debug("Failed to get transaction details at GetTransactionDetails() method");
            }
            log.LogMethodExit(lastUpdateTime);
            return lastUpdateTime;
        }

        internal int UpdateConcurrentRequestDetails(object trxId, object programId, object guid, bool success, 
                                                     object status, object data, object remarks,string username, int concurrentRequestId)
        {
            log.LogMethodEntry(trxId, programId, guid, success, status, data, remarks, username);

            log.LogVariableState("trxId", trxId);
            log.LogVariableState("programId", programId);
            log.LogVariableState("guid", guid);
            log.LogVariableState("success", success);
            log.LogVariableState("status", status);
            log.LogVariableState("data", data);
            log.LogVariableState("remarks", remarks);
            log.LogVariableState("username", username);

            int concurrentRequestDetailId = -1;
            try
            {
                string query = @"insert into ConcurrentRequestDetails
                                                    (TimeStamp, ConcurrentProgramId,
                                                    ParafaitObject, ParafaitObjectId,
                                                    ParafaitObjectGuid, IsSuccessFul,
                                                    Status, Data,
                                                    Remarks, CreationDate,
                                                    CreatedBy, LastUpdatedDate, LastUpdatedBy,ConcurrentRequestId)
                                               values 
                                                    (getdate(), @programId,
                                                    'TrxId', @trxId,
                                                    @guid, @success,
                                                    @status, substring(@data, 1, 500),
                                                    substring(@remarks, 1, 500),
                                                    getdate(),
                                                    @userId, getdate(), @userId,@concurrentRequestId)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(dataAccessHandler.GetSQLParameter("@trxId", trxId));
                parameters.Add(dataAccessHandler.GetSQLParameter("@programId", programId));
                parameters.Add(dataAccessHandler.GetSQLParameter("@guid", guid));
                parameters.Add(dataAccessHandler.GetSQLParameter("@success", success));
                parameters.Add(dataAccessHandler.GetSQLParameter("@status", status));
                parameters.Add(dataAccessHandler.GetSQLParameter("@data", data));
                parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", remarks));
                parameters.Add(dataAccessHandler.GetSQLParameter("@userId", username));
                parameters.Add(dataAccessHandler.GetSQLParameter("@concurrentRequestId", concurrentRequestId,true));
                concurrentRequestDetailId = dataAccessHandler.executeInsertQuery(query, parameters.ToArray(), sqlTransaction);
                log.Debug("ConcurrentRequestDetails Id : " + concurrentRequestDetailId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Debug("Failed to Update Concurrent Request Details");
            }
            log.LogMethodExit(concurrentRequestDetailId);
            return concurrentRequestDetailId;
        }

    }
}
