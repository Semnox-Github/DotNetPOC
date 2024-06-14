/********************************************************************************************
 * Project Name - TrxUserLogs DH
 * Description  - Data handler logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        12-Sep-2018      Mathew Ninan        Created 
 *2.60.2      24-May-2019      Mathew Ninan        Added sqlTransaction
 *2.80        31-May-2020      Vikas Dwivedi       Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// data handler class for TrxUserLogsDataHandler 
    /// </summary>
    public class TrxUserLogsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM TrxUserLogs AS tul ";

        private static readonly Dictionary<TrxUserLogsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TrxUserLogsDTO.SearchByParameters, string>
            {
                {TrxUserLogsDTO.SearchByParameters.TRX_USER_LOG_ID, "tul.TrxUserLogId"},
                {TrxUserLogsDTO.SearchByParameters.TRX_ID, "tul.TrxId"},
                {TrxUserLogsDTO.SearchByParameters.LINE_ID, "tul.LineId"},
                {TrxUserLogsDTO.SearchByParameters.LOGIN_ID, "tul.LoginId"},
                {TrxUserLogsDTO.SearchByParameters.POS_MACHINE_ID,"tul.PosMachineId"},
                {TrxUserLogsDTO.SearchByParameters.SITE_ID, "tul.site_id"}
            };


        /// <summary>
        /// Default constructor of TrxUserLogsDataHandler class
        /// </summary>
        public TrxUserLogsDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of TrxUserLogsDataHandler class passing SQLTransaction
        /// </summary>
        public TrxUserLogsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// method to insert InsertTrxUserLogs details to database 
        /// </summary>
        /// <param name="TrxUserLogsDTO">InsertTrxUserLogs DTO object</param>
        /// <param name="loginId">ID of the currently loggedIn user</param>
        /// <param name="siteId">Current site id</param>
        /// <returns>Returns inserted record id</returns>
        public TrxUserLogsDTO InsertTrxUserLogs(TrxUserLogsDTO trxUserLogsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxUserLogsDTO, loginId, siteId);
            string insertQuery = @"INSERT INTO TrxUserLogs 
                                        ( 
                                            TrxId,
                                            LineId,
                                            LoginId,
                                            ActivityDate,
                                            PosMachineId,
                                            Action,
                                            Activity,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastupdateDate,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            ApproverId,
                                            ApprovalTime
                                        ) 
                                VALUES 
                                        (
                                            @TrxId,
                                            @LineId,
                                            @LoginId,
                                            @ActivityDate,
                                            @PosMachineId,
                                            @Action,
                                            @Activity,
                                            @CreatedBy,
                                            GetDate(),
                                            @LastUpdatedBy,
                                            GetDate(),
                                            NEWID(),
                                            @site_id,
                                            @MasterEntityId,
                                            @ApproverId,
                                            @ApprovalTime
                                        )SELECT * FROM TrxUserLogs WHERE TrxUserLogId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(trxUserLogsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxUserLogsDTO(trxUserLogsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TrxUserLogsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxUserLogsDTO);
            return trxUserLogsDTO;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AchievementProject Record.
        /// </summary>
        /// <param name="TrxUserLogsDTO">TrxUserLogsDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(TrxUserLogsDTO trxUserLogsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxUserLogsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxUserLogId", trxUserLogsDTO.TrxUserLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", trxUserLogsDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineId", trxUserLogsDTO.LineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoginId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActivityDate", trxUserLogsDTO.ActivityDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PosMachineId", trxUserLogsDTO.PosMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Action", trxUserLogsDTO.Action));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Activity", trxUserLogsDTO.Activity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", trxUserLogsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApproverId", trxUserLogsDTO.ApproverId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApprovalTime", trxUserLogsDTO.ApprovalTime));

            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="trxUserLogsDTO">TrxUserLogsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshTrxUserLogsDTO(TrxUserLogsDTO trxUserLogsDTO, DataTable dt)
        {
            log.LogMethodEntry(trxUserLogsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                trxUserLogsDTO.TrxUserLogId = Convert.ToInt32(dt.Rows[0]["TrxUserLogId"]);
                trxUserLogsDTO.LastupdateDate = dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                trxUserLogsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                trxUserLogsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                trxUserLogsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                trxUserLogsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                trxUserLogsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to TrxUserLogsDTO class type
        /// </summary>
        /// <param name="trxUserLogsDataRow">TrxUserLogs DataRow</param>
        /// <returns>Returns TrxUserLogs</returns>
        private TrxUserLogsDTO GetTrxUserLogsDTO(DataRow trxUserLogsDataRow)
        {
            log.LogMethodEntry(trxUserLogsDataRow);
            TrxUserLogsDTO trxUserLogsDataObject = new TrxUserLogsDTO(Convert.ToInt32(trxUserLogsDataRow["TrxUserLogId"]),
                                            trxUserLogsDataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(trxUserLogsDataRow["TrxId"]),
                                            trxUserLogsDataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(trxUserLogsDataRow["LineId"]),
                                            trxUserLogsDataRow["LoginId"].ToString(),
                                            trxUserLogsDataRow["ActivityDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(trxUserLogsDataRow["ActivityDate"]),
                                            trxUserLogsDataRow["PosMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(trxUserLogsDataRow["PosMachineId"]),
                                            trxUserLogsDataRow["Action"].ToString(),
                                            trxUserLogsDataRow["Activity"].ToString(),
                                            trxUserLogsDataRow["CreatedBy"].ToString(),
                                            trxUserLogsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(trxUserLogsDataRow["CreationDate"]),
                                            trxUserLogsDataRow["LastUpdatedBy"].ToString(),
                                            trxUserLogsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(trxUserLogsDataRow["LastUpdateDate"]),
                                            trxUserLogsDataRow["Guid"].ToString(),
                                            trxUserLogsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(trxUserLogsDataRow["site_id"]),
                                            trxUserLogsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(trxUserLogsDataRow["SynchStatus"]),
                                            trxUserLogsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(trxUserLogsDataRow["MasterEntityId"]),
                                            trxUserLogsDataRow["ApproverId"] == DBNull.Value ? "" : trxUserLogsDataRow["ApproverId"].ToString(),
                                            trxUserLogsDataRow["ApprovalTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(trxUserLogsDataRow["ApprovalTime"])
                                            );
            log.LogMethodExit(trxUserLogsDataObject);
            return trxUserLogsDataObject;
        }

        /// <summary>
        /// Gets the TrxUserLogsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TrxUserLogsDTO matching the search criteria</returns>
        public List<TrxUserLogsDTO> GetTrxUserLogsList(List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            string selectTrxUserLogsQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TrxUserLogsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == TrxUserLogsDTO.SearchByParameters.TRX_USER_LOG_ID
                            || searchParameter.Key == TrxUserLogsDTO.SearchByParameters.TRX_ID
                            || searchParameter.Key == TrxUserLogsDTO.SearchByParameters.LINE_ID
                            || searchParameter.Key == TrxUserLogsDTO.SearchByParameters.POS_MACHINE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == TrxUserLogsDTO.SearchByParameters.LOGIN_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "='" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == TrxUserLogsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetTrxUserLogsList(searchParameters) method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectTrxUserLogsQuery = selectTrxUserLogsQuery + query;
            }
            DataTable trxUserLogsData = dataAccessHandler.executeSelectQuery(selectTrxUserLogsQuery, null);
            if (trxUserLogsData.Rows.Count > 0)
            {
                List<TrxUserLogsDTO> trxUserLogsList = new List<TrxUserLogsDTO>();
                foreach (DataRow trxUserLogsDataRow in trxUserLogsData.Rows)
                {
                    TrxUserLogsDTO trxUserLogsDataObject = GetTrxUserLogsDTO(trxUserLogsDataRow);
                    trxUserLogsList.Add(trxUserLogsDataObject);
                }
                log.LogMethodExit(trxUserLogsList);
                return trxUserLogsList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the TrxUserLogsDTO list with updated product name
        /// </summary>
        public List<TrxUserLogsDTO> GetUpdatedTrxUserLogsDTO(List<TrxUserLogsDTO> trxUserLogsDTO)
        {
            log.LogMethodEntry(trxUserLogsDTO);
            for (int i = 0; i < trxUserLogsDTO.Count; i++)
            {
                if (trxUserLogsDTO[i].LineId > 0)
                {
                    string selectQuery = @"select products.product_name from trx_lines , products where LineId = @Line_Id
										and TrxId = @Trx_Id and trx_lines.product_id = products.product_id";
                    SqlParameter[] selectParameters = new SqlParameter[2];
                    selectParameters[0] = new SqlParameter("@Line_Id", trxUserLogsDTO[i].LineId);
                    selectParameters[1] = new SqlParameter("@Trx_Id", trxUserLogsDTO[i].TrxId);
                    DataTable dt = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);
                    if (dt.Rows.Count > 0)
                    {
                        trxUserLogsDTO[i].ProductName = dt.Rows[0]["product_name"].ToString();
                        trxUserLogsDTO[i].AcceptChanges();
                    }
                    else
                    {
                        trxUserLogsDTO[i].ProductName = "";
                        trxUserLogsDTO[i].AcceptChanges();
                    }

                }
            }
            log.LogMethodExit(trxUserLogsDTO);
            return trxUserLogsDTO;
        }
    }
}
