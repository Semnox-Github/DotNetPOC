/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.      03-Jun-2019   Girish Kundar           Created 
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
    /// This is the ReorderStockLineDataHandler data object handles Insert,update and select for the ReorderStock_Line business object
    /// </summary>
    public class ReorderStockLineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ReorderStock_Line AS rsl";
        /// <summary>
        /// Dictionary for searching Parameters for the ReorderStock_Line object.
        /// </summary>
        private static readonly Dictionary<ReorderStockLineDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReorderStockLineDTO.SearchByParameters, string>
        {
            { ReorderStockLineDTO.SearchByParameters.REORDER_STOCK_LINE_ID ,"rsl.ReorderStockLineId"},
            { ReorderStockLineDTO.SearchByParameters.REORDER_STOCK_ID,"rsl.ReorderStockId"},
            { ReorderStockLineDTO.SearchByParameters.PRODUCT_ID,"rsl.ProductId"},
            { ReorderStockLineDTO.SearchByParameters.SITE_ID,"rsl.site_id"},
            { ReorderStockLineDTO.SearchByParameters.MASTER_ENTITY_ID,"rsl.MasterEntityId"},
             { ReorderStockLineDTO.SearchByParameters.REORDER_STOCK_ID_LIST,"rsl.ReorderStockId"}
        };

        /// <summary>
        /// Parameterized Constructor for ReorderStockLineDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public ReorderStockLineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ReorderStockLine Record.
        /// </summary>
        /// <param name="reorderStockLineDTO">ReorderStockLineDTO object passed as parameter</param>
        /// <param name="loginId">user id of the user </param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ReorderStockLineDTO reorderStockLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reorderStockLineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReorderStockLineId", reorderStockLineDTO.ReorderStockLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReorderStockId", reorderStockLineDTO.ReorderStockId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", reorderStockLineDTO.ProductId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QuantityOnHand", reorderStockLineDTO.QuantityOnHand));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReorderPoint", reorderStockLineDTO.ReorderPoint));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReorderQuantity", reorderStockLineDTO.ReorderQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorId", reorderStockLineDTO.VendorId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", reorderStockLineDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to ReorderStockLineDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the ReorderStockLineDTO</returns>
        private ReorderStockLineDTO GetReorderStockLineDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ReorderStockLineDTO reorderStockLineDTO = new ReorderStockLineDTO(dataRow["ReorderStockLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReorderStockLineId"]),
                                          dataRow["ReorderStockId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReorderStockId"]),
                                          dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                          dataRow["QuantityOnHand"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["QuantityOnHand"]),
                                          dataRow["ReorderPoint"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ReorderPoint"]),
                                          dataRow["ReorderQuantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ReorderQuantity"]),
                                          dataRow["VendorId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["VendorId"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                          );
            log.LogMethodExit(reorderStockLineDTO);
            return reorderStockLineDTO;
        }

        /// <summary>
        /// Gets the  data of passed ReorderStockLineId 
        /// </summary>
        /// <param name="reorderStockLineId">reorderStockLineId of ReorderStockLineDTO</param>
        /// <returns>Returns the ReorderStockLineDTO</returns>
        public ReorderStockLineDTO GetReorderStockLineDTO(int reorderStockLineId)
        {
            log.LogMethodEntry(reorderStockLineId);
            ReorderStockLineDTO result = null;
            string query = SELECT_QUERY + @" WHERE rsl.ReorderStockLineId = @ReorderStockLineId";
            SqlParameter parameter = new SqlParameter("@ReorderStockLineId", reorderStockLineId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetReorderStockLineDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the ReorderStock_Line Table.
        /// </summary>
        /// <param name="reorderStockLineDTO">ReorderStockLineDTO object passed as parameter</param>
        /// <param name="loginId">user id of the user </param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns> Returns the ReorderStockLineDTO</returns>
        public ReorderStockLineDTO Insert(ReorderStockLineDTO reorderStockLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reorderStockLineDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ReorderStock_Line]
                               (ReorderStockId,
                                ProductId,
                                QuantityOnHand,
                                ReorderPoint,
                                ReorderQuantity,
                                VendorId,
                                site_id,
                                Guid,
                                MasterEntityId,
                                CreatedBy,
                                CreationDate,
                                LastUpdatedBy,
                                LastUpdateDate)
                         VALUES
                               (@ReorderStockId,
                                @ProductId,
                                @QuantityOnHand,
                                @ReorderPoint,
                                @ReorderQuantity,
                                @VendorId,
                                @site_id,
                                NEWID(),
                                @MasterEntityId,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE() )
                                    SELECT * FROM ReorderStock_Line WHERE ReorderStockLineId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(reorderStockLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReorderStockLineDTO(reorderStockLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReorderStockLineDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reorderStockLineDTO);
            return reorderStockLineDTO;
        }

        /// <summary>
        ///  Updates the record to the ReorderStock_Line Table.
        /// </summary>
        /// <param name="reorderStockLineDTO">ReorderStockLineDTO object passed as parameter</param>
        /// <param name="loginId">user id of the user </param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns> Returns the ReorderStockLineDTO</returns>
        public ReorderStockLineDTO Update(ReorderStockLineDTO reorderStockLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reorderStockLineDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ReorderStock_Line]
                               SET 
                                ReorderStockId = @ReorderStockId,
                                ProductId      = @ProductId,
                                QuantityOnHand = @QuantityOnHand,
                                ReorderPoint   = @ReorderPoint,
                                ReorderQuantity= @ReorderQuantity,
                                VendorId       = @VendorId,
                                MasterEntityId = @MasterEntityId,
                                LastUpdatedBy  = @LastUpdatedBy,
                                LastUpdateDate = GETDATE() 
                                WHERE  ReorderStockLineId = @ReorderStockLineId
                                    SELECT * FROM ReorderStock_Line WHERE ReorderStockLineId = @ReorderStockLineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(reorderStockLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReorderStockLineDTO(reorderStockLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReorderStockLineDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reorderStockLineDTO);
            return reorderStockLineDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="reorderStockLineDTO">ReorderStockLineDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshReorderStockLineDTO(ReorderStockLineDTO reorderStockLineDTO, DataTable dt)
        {
            log.LogMethodEntry(reorderStockLineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reorderStockLineDTO.ReorderStockLineId = Convert.ToInt32(dt.Rows[0]["ReorderStockLineId"]);
                reorderStockLineDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                reorderStockLineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reorderStockLineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                reorderStockLineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                reorderStockLineDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reorderStockLineDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of ReorderStockLineDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ReorderStockLineDTO</returns>
        public List<ReorderStockLineDTO> GetReorderStockLineDTOList(List<KeyValuePair<ReorderStockLineDTO.SearchByParameters, string>> searchParameters,
                                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ReorderStockLineDTO> reorderStockLineDTOList = new List<ReorderStockLineDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReorderStockLineDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReorderStockLineDTO.SearchByParameters.REORDER_STOCK_LINE_ID
                            || searchParameter.Key == ReorderStockLineDTO.SearchByParameters.REORDER_STOCK_ID
                            || searchParameter.Key == ReorderStockLineDTO.SearchByParameters.PRODUCT_ID
                            || searchParameter.Key == ReorderStockLineDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReorderStockLineDTO.SearchByParameters.REORDER_STOCK_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReorderStockLineDTO.SearchByParameters.SITE_ID)
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
                    ReorderStockLineDTO reorderStockLineDTO = GetReorderStockLineDTO(dataRow);
                    reorderStockLineDTOList.Add(reorderStockLineDTO);
                }
            }
            log.LogMethodExit(reorderStockLineDTOList);
            return reorderStockLineDTOList;
        }
    }
}
