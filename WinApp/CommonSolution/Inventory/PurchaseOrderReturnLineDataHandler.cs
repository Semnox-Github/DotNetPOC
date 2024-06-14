/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70       03-Jun-2019   Girish Kundar           Created 
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
    /// This is the PurchaseOrderReturnLineDataHandler handles Insert ,update and search for the PurchaseOrderReturn_Line business object
    /// </summary>
    public class PurchaseOrderReturnLineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PurchaseOrderReturn_Line AS poRLine";
        /// <summary>
        /// Dictionary for searching Parameters for the PurchaseOrderReturn_Line object.
        /// </summary>
        private static readonly Dictionary<PurchaseOrderReturnLineDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PurchaseOrderReturnLineDTO.SearchByParameters, string>
        {
            { PurchaseOrderReturnLineDTO.SearchByParameters.PURCHASE_ORD_RET_LINE_ID , "poRLine.PurchaseOrderReturnLineId"},
            { PurchaseOrderReturnLineDTO.SearchByParameters.PURCHASE_ORDER_ID,"poRLine.PurchaseOrderId"},
            { PurchaseOrderReturnLineDTO.SearchByParameters.PRODUCT_ID,"poRLine.ProductId"},
            { PurchaseOrderReturnLineDTO.SearchByParameters.SITE_ID,"poRLine.site_id"},
            { PurchaseOrderReturnLineDTO.SearchByParameters.MASTER_ENTITY_ID,"poRLine.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for PurchaseOrderReturnLineDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public PurchaseOrderReturnLineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PurchaseOrderReturnLine Record.
        /// </summary>
        /// <param name="purchaseOrderReturnLineDTO">purchaseOrderReturnLineDTO object passed as parameter</param>
        /// <param name="loginId">login id of the user</param>
        /// <param name="siteId">site Id of  the user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(PurchaseOrderReturnLineDTO purchaseOrderReturnLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderReturnLineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderReturnLineId", purchaseOrderReturnLineDTO.PurchaseOrderReturnLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderId", purchaseOrderReturnLineDTO.PurchaseOrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", purchaseOrderReturnLineDTO.ProductId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Quantity", purchaseOrderReturnLineDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", purchaseOrderReturnLineDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubTotal", purchaseOrderReturnLineDTO.SubTotal));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", purchaseOrderReturnLineDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnitPrice", purchaseOrderReturnLineDTO.UnitPrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorItemCode", purchaseOrderReturnLineDTO.VendorItemCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", purchaseOrderReturnLineDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Gets the PurchaseOrderReturnLineDTO data of passed purchaseOrderReturnLineId 
        /// </summary>
        /// <param name="purchaseOrderReturnLineId">purchaseOrderReturnLineId -integer type parameter</param>
        /// <returns>Returns PurchaseOrderReturnLineDTO</returns>
        public PurchaseOrderReturnLineDTO GetPurchaseOrderReturnLineDTO(int purchaseOrderReturnLineId ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(purchaseOrderReturnLineId);
            PurchaseOrderReturnLineDTO result = null;
            string query = SELECT_QUERY + @" WHERE poRLine.PurchaseOrderReturnLineId = @PurchaseOrderReturnLineId";
            SqlParameter parameter = new SqlParameter("@PurchaseOrderReturnLineId", purchaseOrderReturnLineId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPurchaseOrderReturnLineDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Converts the Data row object to PurchaseOrderReturnLineDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>PurchaseOrderReturnLineDTO</returns>
        private PurchaseOrderReturnLineDTO GetPurchaseOrderReturnLineDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PurchaseOrderReturnLineDTO purchaseOrderReturnLineDTO = new PurchaseOrderReturnLineDTO(dataRow["PurchaseOrderReturnLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PurchaseOrderReturnLineId"]),
                                          dataRow["PurchaseOrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PurchaseOrderId"]),
                                          dataRow["ProductId"] == DBNull.Value ? -1 :Convert.ToInt32(dataRow["ProductId"]),
                                          dataRow["VendorItemCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["VendorItemCode"]),
                                          dataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Quantity"]),
                                          dataRow["UnitPrice"] == DBNull.Value ? 0 :  Convert.ToDecimal(dataRow["UnitPrice"]),
                                          dataRow["SubTotal"] == DBNull.Value ?  0 : Convert.ToDecimal(dataRow["SubTotal"]),
                                          dataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Timestamp"]),
                                          dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                          );
            log.LogMethodExit(purchaseOrderReturnLineDTO);
            return purchaseOrderReturnLineDTO;
        }

        /// <summary>
        ///  Inserts the record to the PurchaseOrderReturnLine Table.
        /// </summary>
        /// <param name="purchaseOrderReturnLineDTO">purchaseOrderReturnLineDTO object passed as parameter</param>
        /// <param name="loginId">login id of the user</param>
        /// <param name="siteId">site Id of  the user</param>
        /// <returns> Returns the PurchaseOrderReturnLineDTO</returns>
        public PurchaseOrderReturnLineDTO Insert(PurchaseOrderReturnLineDTO purchaseOrderReturnLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderReturnLineDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[PurchaseOrderReturn_Line]
                           (PurchaseOrderId
                           ,ProductId
                           ,VendorItemCode
                           ,Quantity
                           ,UnitPrice
                           ,SubTotal
                           ,Timestamp
                           ,Remarks
                           ,site_id
                           ,Guid
                           ,MasterEntityId
                           ,CreatedBy
                           ,CreationDate
                           ,LastUpdatedBy
                           ,LastUpdateDate)
                     VALUES
                           (@PurchaseOrderId
                           ,@ProductId
                           ,@VendorItemCode
                           ,@Quantity
                           ,@UnitPrice
                           ,@SubTotal
                           ,GETDATE()
                           ,@Remarks
                           ,@site_id
                           ,NEWID()
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE())
                                    SELECT * FROM PurchaseOrderReturn_Line WHERE PurchaseOrderReturnLineId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseOrderReturnLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderReturnLineDTO(purchaseOrderReturnLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PurchaseOrderReturnLineDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(purchaseOrderReturnLineDTO);
            return purchaseOrderReturnLineDTO;
        }

        /// <summary>
        ///  Updates the record to the PurchaseOrderReturnLine Table.
        /// </summary>
        /// <param name="purchaseOrderReturnLineDTO">purchaseOrderReturnLineDTO object passed as parameter</param>
        /// <param name="loginId">login id of the user</param>
        /// <param name="siteId">site Id of  the user</param>
        /// <returns> Returns the PurchaseOrderReturnLineDTO</returns>
        public PurchaseOrderReturnLineDTO Update(PurchaseOrderReturnLineDTO purchaseOrderReturnLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderReturnLineDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[PurchaseOrderReturn_Line]
                           SET
                            PurchaseOrderId = @PurchaseOrderId
                           ,ProductId       = @ProductId
                           ,VendorItemCode  = @VendorItemCode
                           ,Quantity        = @Quantity
                           ,UnitPrice       = @UnitPrice
                           ,SubTotal        = @SubTotal
                           ,Timestamp       = GETDATE()
                           ,Remarks         = @Remarks
                           ,MasterEntityId  = @MasterEntityId
                           ,LastUpdatedBy   = @LastUpdatedBy
                           ,LastUpdateDate  = GETDATE()
                          WHERE PurchaseOrderReturnLineId = @PurchaseOrderReturnLineId
                           SELECT * FROM PurchaseOrderReturn_Line WHERE PurchaseOrderReturnLineId = @PurchaseOrderReturnLineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseOrderReturnLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderReturnLineDTO(purchaseOrderReturnLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating PurchaseOrderReturnLineDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(purchaseOrderReturnLineDTO);
            return purchaseOrderReturnLineDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="purchaseOrderReturnLineDTO">PurchaseOrderReturnLineDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshPurchaseOrderReturnLineDTO(PurchaseOrderReturnLineDTO purchaseOrderReturnLineDTO, DataTable dt)
        {
            log.LogMethodEntry(purchaseOrderReturnLineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                purchaseOrderReturnLineDTO.PurchaseOrderReturnLineId = Convert.ToInt32(dt.Rows[0]["PurchaseOrderReturnLineId"]);
                purchaseOrderReturnLineDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                purchaseOrderReturnLineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                purchaseOrderReturnLineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                purchaseOrderReturnLineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                purchaseOrderReturnLineDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                purchaseOrderReturnLineDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of PurchaseOrderReturnLineDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PurchaseOrderReturnLineDTO</returns>
        public List<PurchaseOrderReturnLineDTO> GetPurchaseOrderReturnLineDTOList(List<KeyValuePair<PurchaseOrderReturnLineDTO.SearchByParameters, string>> searchParameters , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PurchaseOrderReturnLineDTO> purchaseOrderReturnLineDTOList = new List<PurchaseOrderReturnLineDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PurchaseOrderReturnLineDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PurchaseOrderReturnLineDTO.SearchByParameters.PURCHASE_ORD_RET_LINE_ID
                            || searchParameter.Key == PurchaseOrderReturnLineDTO.SearchByParameters.PURCHASE_ORDER_ID
                            || searchParameter.Key == PurchaseOrderReturnLineDTO.SearchByParameters.PRODUCT_ID
                            || searchParameter.Key == PurchaseOrderReturnLineDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PurchaseOrderReturnLineDTO.SearchByParameters.SITE_ID)
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
                    PurchaseOrderReturnLineDTO purchaseOrderReturnLineDTO = GetPurchaseOrderReturnLineDTO(dataRow);
                    purchaseOrderReturnLineDTOList.Add(purchaseOrderReturnLineDTO);
                }
            }
            log.LogMethodExit(purchaseOrderReturnLineDTOList);
            return purchaseOrderReturnLineDTOList;
        }
    }
}
