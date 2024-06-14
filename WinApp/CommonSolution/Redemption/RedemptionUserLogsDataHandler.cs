/********************************************************************************************
 * Project Name - Redemption
 * Description  - Data handler of RedemptionUserLogs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        29-Jul-2019   Archana                 Created
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// RedemptionUserLogs Data Handler - Handles insert, update and select of  RedemptionUserLogs objects
    /// </summary>
    public class RedemptionUserLogsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RedemptionUserLogs AS rul";
        /// <summary>
        /// Dictionary for searching Parameters for the RedemptionUserLogs object.
        /// </summary>
        private static readonly Dictionary<RedemptionUserLogsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RedemptionUserLogsDTO.SearchByParameters, string>
        {
            { RedemptionUserLogsDTO.SearchByParameters.REDEMPTION_LOG_ID,"rul.RedemptionLogId"},
            { RedemptionUserLogsDTO.SearchByParameters.REDEMPTION_ID,"rul.RedemptionId"},
            { RedemptionUserLogsDTO.SearchByParameters.POS_MACHINE_ID,"rul.POSMachineId"},
            { RedemptionUserLogsDTO.SearchByParameters.TICKET_RECEIPT_ID,"rul.TicketReceiptId"},
            { RedemptionUserLogsDTO.SearchByParameters.CURRENCY_ID,"rul.CurrencyId"},
            { RedemptionUserLogsDTO.SearchByParameters.SITE_ID,"rul.site_id"},
            { RedemptionUserLogsDTO.SearchByParameters.MASTER_ENTITY_ID,"rul.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for RedemptionUserLogsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public RedemptionUserLogsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating RedemptionUserLogs Record.
        /// </summary>
        /// <param name="RedemptionUserLogsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(RedemptionUserLogsDTO redemptionUserLogsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionUserLogsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionLogId", redemptionUserLogsDTO.RedemptionLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionId", redemptionUserLogsDTO.RedemptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CurrencyId", redemptionUserLogsDTO.CurrencyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketReceiptId", redemptionUserLogsDTO.TicketReceiptId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoginId", redemptionUserLogsDTO.LoginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActivityDate", redemptionUserLogsDTO.ActivityDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", redemptionUserLogsDTO.PosMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Action", redemptionUserLogsDTO.Action));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Activity", redemptionUserLogsDTO.Activity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApproverId", redemptionUserLogsDTO.ApproverId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApprovalTime", redemptionUserLogsDTO.ApprovalTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", redemptionUserLogsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private RedemptionUserLogsDTO GetRedemptionUserLogsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(dataRow["RedemptionLogId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RedemptionLogId"]),
                                       dataRow["RedemptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RedemptionId"]),
                                       dataRow["CurrencyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyId"]),
                                       dataRow["TicketReceiptId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TicketReceiptId"].ToString()),
                                       dataRow["LoginId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LoginId"] as byte[]),
                                       dataRow["ActivityDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ActivityDate"] as byte[]),
                                       dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                       dataRow["Action"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Action"]),
                                       dataRow["Activity"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Activity"].ToString()),
                                       dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                       dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                       dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                       dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                       dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                       dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                       dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                       dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                       dataRow["ApproverId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ApproverId"]),
                                       dataRow["ApprovalTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ApprovalTime"])                                       
                                      );
            log.LogMethodExit(redemptionUserLogsDTO);
            return redemptionUserLogsDTO;
        }

        /// <summary>
        /// Gets the RedemptionUserLogs data of passed RedemptionLogId 
        /// </summary>
        /// <param name="redemptionLogId">integer type parameter</param>
        /// <returns>Returns RedemptionUserLogsDTO</returns>
        public RedemptionUserLogsDTO GetRedemptionUserLogsDTO(int redemptionLogId)
        {
            log.LogMethodEntry(redemptionLogId);
            RedemptionUserLogsDTO result = null;
            string query = SELECT_QUERY + @" WHERE rul.RedemptionLogId = @RedemptionLogId";
            SqlParameter parameter = new SqlParameter("@RedemptionLogId", redemptionLogId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRedemptionUserLogsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Inserts the record to RedemptionUserLogs Table
        /// </summary>
        /// <param name="redemptionUserLogsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public RedemptionUserLogsDTO Insert(RedemptionUserLogsDTO redemptionUserLogsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionUserLogsDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[RedemptionUserLogs]
                           (RedemptionId
                           ,CurrencyId
                           ,TicketReceiptId
                           ,LoginId
                           ,ActivityDate
                           ,POSMachineId
                           ,Action
                           ,Activity
                           ,CreationDate
                           ,CreatedBy
                           ,LastUpdateDate
                           ,LastUpdatedBy
                           ,site_id
                           ,Guid
                           ,MasterEntityId
                           ,ApproverId
                           ,ApprovalTime)
                     VALUES
                           (@RedemptionId
                           ,@CurrencyId
                           ,@TicketReceiptId
                           ,@LoginId
                           ,@ActivityDate
                           ,@POSMachineId
                           ,@Action
                           ,@Activity
                           ,GETDATE()
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,@site_id
                           ,NEWID()
                           ,@MasterEntityId
                           ,@ApproverId
                           ,@ApprovalTime)
                                SELECT * FROM RedemptionUserLogs WHERE RedemptionLogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionUserLogsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionUserLogsDTO(redemptionUserLogsDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting RedemptionUserLogsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionUserLogsDTO);
            return redemptionUserLogsDTO;
        }

        /// <summary>
        /// Update the record to RedemptionUserLogs Table
        /// </summary>
        /// <param name="redemptionUserLogsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public RedemptionUserLogsDTO Update(RedemptionUserLogsDTO redemptionUserLogsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionUserLogsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RedemptionUserLogs]
                            SET 
                            RedemptionId = @RedemptionId
                           ,CurrencyId = @CurrencyId
                           ,TicketReceiptId = @TicketReceiptId
                           ,LoginId = @LoginId
                           ,ActivityDate = @ActivityDate
                           ,POSMachineId = @POSMachineId
                           ,Action = @Action
                           ,Activity = @Activity
                           ,LastUpdateDate = GETDATE()
                           ,LastUpdatedBy = @LastUpdatedBy
                           -- ,site_id = @site_id
                           ,MasterEntityId = @MasterEntityId
                           ,ApproverId = @ApproverId
                           ,ApprovalTime = @ApprovalTime
                            Where RedemptionLogId = @RedemptionLogId
                            SELECT * FROM RedemptionUserLogs WHERE RedemptionLogId = @RedemptionLogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionUserLogsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionUserLogsDTO(redemptionUserLogsDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting RedemptionUserLogsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionUserLogsDTO);
            return redemptionUserLogsDTO;
        }

        /// <summary>
        /// Refresh RedemptionUserLogsDTO with DB values
        /// </summary>
        /// <param name="RedemptionUserLogsDTO">Redemption User logs DTO</param>
        /// <param name="dt">Data table</param>
        /// <param name="loginId">Login ID</param>
        /// <param name="siteId">site </param>
        private void RefreshRedemptionUserLogsDTO(RedemptionUserLogsDTO redemptionUserLogsDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionUserLogsDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                redemptionUserLogsDTO.RedemptionLogId = Convert.ToInt32(dt.Rows[0]["RedemptionLogId"]);
                redemptionUserLogsDTO.LastUpdateDate = Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                redemptionUserLogsDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                redemptionUserLogsDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                redemptionUserLogsDTO.LastUpdatedBy = Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                redemptionUserLogsDTO.CreatedBy = Convert.ToString(dt.Rows[0]["CreatedBy"]);
                redemptionUserLogsDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of RedemptionUserLogsDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<RedemptionUserLogsDTO> GetAllRedemptionUserLogsDTOList(List<KeyValuePair<RedemptionUserLogsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RedemptionUserLogsDTO> redemptionUserLogsDTOList = new List<RedemptionUserLogsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RedemptionUserLogsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RedemptionUserLogsDTO.SearchByParameters.REDEMPTION_LOG_ID
                            || searchParameter.Key == RedemptionUserLogsDTO.SearchByParameters.REDEMPTION_ID
                            || searchParameter.Key == RedemptionUserLogsDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == RedemptionUserLogsDTO.SearchByParameters.CURRENCY_ID
                            || searchParameter.Key == RedemptionUserLogsDTO.SearchByParameters.POS_MACHINE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionUserLogsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RedemptionUserLogsDTO redemptionUserLogsDTO = GetRedemptionUserLogsDTO(dataRow);
                    redemptionUserLogsDTOList.Add(redemptionUserLogsDTO);
                }
            }
            log.LogMethodExit(redemptionUserLogsDTOList);
            return redemptionUserLogsDTOList;
        }
    }
}
