/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler -DeliveryOrderLineDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-Jun-2019   Girish Kundar           Created 
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
    /// This is the DeliveryOrderLineDataHandler data object Handles Insert,update and search for the DeliveryOrderLine  object
    /// </summary>
    public class DeliveryOrderLineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM DeliveryOrderLine As doLine";
        /// <summary>
        /// Dictionary for searching Parameters for the DeliveryOrderLine object.
        /// </summary>
        private static readonly Dictionary<DeliveryOrderLineDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DeliveryOrderLineDTO.SearchByParameters, string>
        {
            { DeliveryOrderLineDTO.SearchByParameters.DELIVERY_ORDER_LINE_ID , "doLine.DeliveryOrderLineId"},
            { DeliveryOrderLineDTO.SearchByParameters.DELIVERY_ORDER_ID,"doLine.DeliveryOrderId"},
            { DeliveryOrderLineDTO.SearchByParameters.PRODUCT_ID,"doLine.ProductId"},
            { DeliveryOrderLineDTO.SearchByParameters.PURCHASE_ORDER_LINE_ID,"doLine.PurchaseOrderLineId"},
            { DeliveryOrderLineDTO.SearchByParameters.SITE_ID,"doLine.site_id"},
            { DeliveryOrderLineDTO.SearchByParameters.DELIVERY_ORDER_ID_LIST,"doLine.DeliveryOrderId"},
            { DeliveryOrderLineDTO.SearchByParameters.IS_ACTIVE,"doLine.IsActive"}, // to be added
            { DeliveryOrderLineDTO.SearchByParameters.MASTER_ENTITY_ID,"doLine.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for DeliveryOrderLineDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public DeliveryOrderLineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DeliveryOrderLine Record.
        /// </summary>
        /// <param name="deliveryOrderLineDTO">deliveryOrderLineDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns>SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(DeliveryOrderLineDTO deliveryOrderLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryOrderLineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryOrderLineId", deliveryOrderLineDTO.DeliveryOrderLineId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryOrderId", deliveryOrderLineDTO.DeliveryOrderId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", deliveryOrderLineDTO.ProductId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderLineId", deliveryOrderLineDTO.PurchaseOrderLineId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryQuantity", deliveryOrderLineDTO.DeliveryQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceivedQuantity", deliveryOrderLineDTO.ReceivedQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", deliveryOrderLineDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", deliveryOrderLineDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to DeliveryOrderLineDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the DeliveryOrderLineDTO</returns>
        private DeliveryOrderLineDTO GetDeliveryOrderLineDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DeliveryOrderLineDTO deliveryOrderLineDTO = new DeliveryOrderLineDTO(dataRow["DeliveryOrderLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryOrderLineId"]),
                                            dataRow["DeliveryOrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryOrderId"]),
                                            dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                            dataRow["DeliveryQuantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["DeliveryQuantity"]),
                                            dataRow["ReceivedQuantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ReceivedQuantity"]),
                                            dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                            dataRow["PurchaseOrderLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PurchaseOrderLineId"]),
                                            true, // this column to be added
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                         );
                                           
            log.LogMethodExit(deliveryOrderLineDTO);
            return deliveryOrderLineDTO;
        }
        /// <summary>
        /// Gets the DeliveryOrderLineDTO data of passed deliveryOrderLineId 
        /// </summary>
        /// <param name="deliveryOrderLineId">deliveryOrderLineId of DeliveryOrderLineDTO</param>
        /// <returns>Returns the DeliveryOrderLineDTO</returns>
        public DeliveryOrderLineDTO GetDeliveryOrderLineDTO(int deliveryOrderLineId ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(deliveryOrderLineId);
            DeliveryOrderLineDTO result = null;
            string query = SELECT_QUERY + @" WHERE doLine.DeliveryOrderLineId = @DeliveryOrderLineId";
            SqlParameter parameter = new SqlParameter("@DeliveryOrderLineId", deliveryOrderLineId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDeliveryOrderLineDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///  Inserts the record to the deliveryOrderLine Table.
        /// </summary>
        /// <param name="deliveryOrderLineDTO">deliveryOrderLineDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns> Returns the DeliveryOrderLineDTO</returns>
        public DeliveryOrderLineDTO Insert(DeliveryOrderLineDTO deliveryOrderLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryOrderLineDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[DeliveryOrderLine]
                           (DeliveryOrderId,
                            ProductId,
                            DeliveryQuantity,
                            ReceivedQuantity,
                            Remarks,
                            PurchaseOrderLineId,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@DeliveryOrderId,
                            @ProductId,
                            @DeliveryQuantity,
                            @ReceivedQuantity,
                            @Remarks,
                            @PurchaseOrderLineId,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE() )
                           SELECT * FROM DeliveryOrderLine WHERE  DeliveryOrderLineId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(deliveryOrderLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDeliveryOrderLineDTO(deliveryOrderLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting DeliveryOrderLineDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(deliveryOrderLineDTO);
            return deliveryOrderLineDTO;
        }

        /// <summary>
        ///  Updates the record to the deliveryOrderLine Table.
        /// </summary>
        /// <param name="deliveryOrderLineDTO">deliveryOrderLineDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns> Returns the DeliveryOrderLineDTO</returns>
        public DeliveryOrderLineDTO Update(DeliveryOrderLineDTO deliveryOrderLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryOrderLineDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[DeliveryOrderLine]
                           SET
                            DeliveryOrderId      =  @DeliveryOrderId,
                            ProductId            =  @ProductId,
                            DeliveryQuantity     =  @DeliveryQuantity,
                            ReceivedQuantity     =  @ReceivedQuantity,
                            Remarks              =  @Remarks,
                            PurchaseOrderLineId  =  @PurchaseOrderLineId,
                            MasterEntityId       =  @MasterEntityId,
                            LastUpdatedBy        =  @LastUpdatedBy,
                            LastUpdateDate       =  GETDATE() 
                            WHERE DeliveryOrderLineId = @DeliveryOrderLineId
                           SELECT * FROM DeliveryOrderLine WHERE  DeliveryOrderLineId = @DeliveryOrderLineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(deliveryOrderLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDeliveryOrderLineDTO(deliveryOrderLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating DeliveryOrderLineDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(deliveryOrderLineDTO);
            return deliveryOrderLineDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="deliveryOrderLineDTO">DeliveryOrderLineDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshDeliveryOrderLineDTO(DeliveryOrderLineDTO deliveryOrderLineDTO, DataTable dt)
        {
            log.LogMethodEntry(deliveryOrderLineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                deliveryOrderLineDTO.DeliveryOrderLineId = Convert.ToInt32(dt.Rows[0]["DeliveryOrderLineId"]);
                deliveryOrderLineDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                deliveryOrderLineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                deliveryOrderLineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                deliveryOrderLineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                deliveryOrderLineDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                deliveryOrderLineDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of DeliveryOrderLineDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>returns the list of DeliveryOrderLineDTO</returns>
        public List<DeliveryOrderLineDTO> GetDeliveryOrderLineDTOList(List<KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string>> searchParameters , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<DeliveryOrderLineDTO> deliveryOrderLineDTOList = new List<DeliveryOrderLineDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<DeliveryOrderLineDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DeliveryOrderLineDTO.SearchByParameters.DELIVERY_ORDER_ID ||
                            searchParameter.Key == DeliveryOrderLineDTO.SearchByParameters.DELIVERY_ORDER_LINE_ID ||
                            searchParameter.Key == DeliveryOrderLineDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == DeliveryOrderLineDTO.SearchByParameters.PURCHASE_ORDER_LINE_ID ||
                            searchParameter.Key == DeliveryOrderLineDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DeliveryOrderLineDTO.SearchByParameters.DELIVERY_ORDER_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == DeliveryOrderLineDTO.SearchByParameters.SITE_ID)
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
                    DeliveryOrderLineDTO deliveryOrderLineDTO = GetDeliveryOrderLineDTO(dataRow);
                    deliveryOrderLineDTOList.Add(deliveryOrderLineDTO);
                }
            }
            log.LogMethodExit(deliveryOrderLineDTOList);
            return deliveryOrderLineDTOList;
        }
    }
}
