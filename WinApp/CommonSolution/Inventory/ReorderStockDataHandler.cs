/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler-ReorderStockDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70     04-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the ReorderStockDataHandler data object handles Insert ,Update and Search for the ReorderStock business object
    /// </summary>
    public class ReorderStockDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ReorderStock AS ros";
        /// <summary>
        /// Dictionary for searching Parameters for the ReorderStock object.
        /// </summary>
        private static readonly Dictionary<ReorderStockDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReorderStockDTO.SearchByParameters, string>
        {
            {ReorderStockDTO.SearchByParameters.REORDER_STOCK_ID , "ros.ReorderStockId"},
            {ReorderStockDTO.SearchByParameters.REMARKS,"ros.Remarks"},
            {ReorderStockDTO.SearchByParameters.SITE_ID,"ros.site_id"},
            {ReorderStockDTO.SearchByParameters.MASTER_ENTITY_ID,"ros.MasterEntityId"},
            {ReorderStockDTO.SearchByParameters.REORDER_STOCK_ID_LIST,"ros.ReorderStockId"}
        };

        /// <summary>
        /// Parameterized Constructor for ReorderStockDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public ReorderStockDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ReorderStock Record.
        /// </summary>
        /// <param name="reorderStockDTO">ReorderStockDTO object is passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ReorderStockDTO reorderStockDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reorderStockDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReorderStockId", reorderStockDTO.ReorderStockId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", reorderStockDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", reorderStockDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", reorderStockDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to ReorderStockDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the ReorderStockDTO</returns>
        private ReorderStockDTO GetReorderStockDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ReorderStockDTO reorderStockDTO = new ReorderStockDTO(dataRow["ReorderStockId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReorderStockId"]),
                                          dataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Timestamp"]),
                                          dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                          );
            log.LogMethodExit(reorderStockDTO);
            return reorderStockDTO;
        }

        /// <summary>
        /// Gets the ReorderStock data of passed ReorderStockId 
        /// </summary>
        /// <param name="reorderStockId">reorderStockId of ReorderStock</param>
        /// <returns>Returns ReorderStockDTO</returns>
        public ReorderStockDTO GetReorderStockDTO(int reorderStockId)
        {
            log.LogMethodEntry(reorderStockId);
            ReorderStockDTO result = null;
            string query = SELECT_QUERY + @" WHERE ros.ReorderStockId = @ReorderStockId";
            SqlParameter parameter = new SqlParameter("@ReorderStockId", reorderStockId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetReorderStockDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the ReorderStock Table.
        /// </summary>
        /// <param name="reorderStockDTO">ReorderStockDTO object is passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns> Returns the ReorderStockDTO</returns>
        public ReorderStockDTO Insert(ReorderStockDTO reorderStockDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reorderStockDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ReorderStock]
                           (Timestamp,
                            Remarks,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@Timestamp,
                            @Remarks,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE() )
                                    SELECT * FROM ReorderStock WHERE ReorderStockId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(reorderStockDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReorderStockDTO(reorderStockDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReorderStockDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reorderStockDTO);
            return reorderStockDTO;
        }
        /// <summary>
        ///  Updates the record to the ReorderStock Table.
        /// </summary>
        /// <param name="reorderStockDTO">ReorderStockDTO object is passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns> Returns the ReorderStockDTO</returns>
        public ReorderStockDTO Update(ReorderStockDTO reorderStockDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reorderStockDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ReorderStock]
                           SET
                            Timestamp      = @Timestamp,
                            Remarks        = @Remarks,
                            MasterEntityId = @MasterEntityId,
                            LastUpdatedBy  = @LastUpdatedBy,
                            LastUpdateDate = GETDATE() 
                           WHERE ReorderStockId =@ReorderStockId
                       SELECT * FROM ReorderStock WHERE ReorderStockId = @ReorderStockId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(reorderStockDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReorderStockDTO(reorderStockDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReorderStockDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reorderStockDTO);
            return reorderStockDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="reorderStockDTO">ReorderStockDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshReorderStockDTO(ReorderStockDTO reorderStockDTO, DataTable dt)
        {
            log.LogMethodEntry(reorderStockDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reorderStockDTO.ReorderStockId = Convert.ToInt32(dt.Rows[0]["ReorderStockId"]);
                reorderStockDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                reorderStockDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reorderStockDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                reorderStockDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                reorderStockDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reorderStockDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of ReorderStockDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>returns the List of ReorderStockDTOList</returns>
        public List<ReorderStockDTO> GetReorderStockDTOList(List<KeyValuePair<ReorderStockDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ReorderStockDTO> reorderStockDTOList = new List<ReorderStockDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReorderStockDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReorderStockDTO.SearchByParameters.REORDER_STOCK_ID
                            || searchParameter.Key == ReorderStockDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReorderStockDTO.SearchByParameters.REORDER_STOCK_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReorderStockDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        if (searchParameter.Key == ReorderStockDTO.SearchByParameters.REMARKS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                    ReorderStockDTO reorderStockDTO = GetReorderStockDTO(dataRow);
                    reorderStockDTOList.Add(reorderStockDTO);
                }
            }
            log.LogMethodExit(reorderStockDTOList);
            return reorderStockDTOList;
        }

    }
}
