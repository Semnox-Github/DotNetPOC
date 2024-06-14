/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler - DeliveryOrderHeaderDataHandler
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
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the DeliveryOrderHeaderDataHandler object Handles Insert,Update and Search for DeliveryOrderHeader objects
    /// </summary>
   public class DeliveryOrderHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM DeliveryOrderHeader as doHeader ";
        /// <summary>
        /// Dictionary for searching Parameters for the DeliveryOrderHeader object.
        /// </summary>
        private static readonly Dictionary<DeliveryOrderHeaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DeliveryOrderHeaderDTO.SearchByParameters, string>
        {
            { DeliveryOrderHeaderDTO.SearchByParameters.DELIVERY_ORDER_ID , "doHeader.DeliveryOrderId"},
            { DeliveryOrderHeaderDTO.SearchByParameters.DELIVERY_ORDER_NUMBER,"doHeader.DeliveryOrderNumber"},
            { DeliveryOrderHeaderDTO.SearchByParameters.DELIVERY_ORDER_DATE,"doHeader.DeliveryOrderDate"},
            { DeliveryOrderHeaderDTO.SearchByParameters.PURCHASE_ORDER_ID,"doHeader.PurchaseOrderId"},
            { DeliveryOrderHeaderDTO.SearchByParameters.SITE_ID,"doHeader.site_id"},
            { DeliveryOrderHeaderDTO.SearchByParameters.MASTER_ENTITY_ID,"doHeader.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for DeliveryOrderHeaderDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public DeliveryOrderHeaderDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DeliveryOrderHeader Record.
        /// </summary>
        /// <param name="deliveryOrderHeaderDTO">DeliveryOrderHeaderDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(DeliveryOrderHeaderDTO deliveryOrderHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryOrderHeaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryOrderId", deliveryOrderHeaderDTO.DeliveryOrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrderId", deliveryOrderHeaderDTO.PurchaseOrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliverTo", deliveryOrderHeaderDTO.DeliverTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryOrderDate", deliveryOrderHeaderDTO.DeliveryOrderDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryOrderNumber", deliveryOrderHeaderDTO.DeliveryOrderNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GRN", deliveryOrderHeaderDTO.GRN));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Issuer", deliveryOrderHeaderDTO.Issuer));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceivedBy", deliveryOrderHeaderDTO.ReceivedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceivedDate", deliveryOrderHeaderDTO.ReceivedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", deliveryOrderHeaderDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", deliveryOrderHeaderDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", deliveryOrderHeaderDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", deliveryOrderHeaderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to DeliveryOrderHeaderDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the DeliveryOrderHeaderDTO</returns>
        private DeliveryOrderHeaderDTO GetDeliveryOrderHeaderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DeliveryOrderHeaderDTO deliveryOrderHeaderDTO = new DeliveryOrderHeaderDTO(dataRow["DeliveryOrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryOrderId"]),
                                          dataRow["DeliveryOrderDate"]  == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["DeliveryOrderDate"]),
                                          dataRow["DeliveryOrderNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DeliveryOrderNumber"]),
                                          dataRow["Type"] == DBNull.Value ? ' ' : Convert.ToChar(dataRow["Type"].ToString().Trim()), // to be checked 
                                          dataRow["PurchaseOrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PurchaseOrderId"]),
                                          dataRow["Issuer"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Issuer"]),
                                          dataRow["DeliverTo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DeliverTo"]),
                                          dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["ReceivedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ReceivedDate"]),
                                          dataRow["ReceivedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ReceivedBy"]),
                                          dataRow["GRN"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GRN"]),
                                          dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"])
                                          );
            log.LogMethodExit(deliveryOrderHeaderDTO);
            return deliveryOrderHeaderDTO;
        }

        /// <summary>
        /// Gets the DeliveryOrderHeaderDTO data of passed deliveryOrderId 
        /// </summary>
        /// <param name="deliveryOrderId">DeliveryOrderId of DeliveryOrderHeaderDTO</param>
        /// <returns>Returns DeliveryOrderHeaderDTO</returns>
        public DeliveryOrderHeaderDTO GetDeliveryOrderHeaderDTO(int deliveryOrderId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(deliveryOrderId);
            DeliveryOrderHeaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE doHeader.DeliveryOrderId = @DeliveryOrderId";
            SqlParameter parameter = new SqlParameter("@DeliveryOrderId", deliveryOrderId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDeliveryOrderHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the DeliveryOrderHeader Table.
        /// </summary>
        /// <param name="deliveryOrderHeaderDTO">DeliveryOrderHeaderDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns> returns the DeliveryOrderHeaderDTO</returns>
        public DeliveryOrderHeaderDTO Insert(DeliveryOrderHeaderDTO deliveryOrderHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryOrderHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[DeliveryOrderHeader]
                           (DeliveryOrderDate,
                            DeliveryOrderNumber,
                            Type,
                            PurchaseOrderId,
                            site_id,
                            Issuer,
                            DeliverTo,
                            Status,
                            ReceivedDate,
                            ReceivedBy,
                            GRN,
                            Remarks,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdatedDate,
                            Guid,
                            MasterEntityId)
                     VALUES
                           (@DeliveryOrderDate,
                            @DeliveryOrderNumber,
                            @Type,
                            @PurchaseOrderId,
                            @site_id,
                            @Issuer,
                            @DeliverTo,
                            @Status,
                            GETDATE(),
                            @ReceivedBy,
                            @GRN,
                            @Remarks,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            NEWID(),
                            @MasterEntityId)
                                    SELECT * FROM DeliveryOrderHeader WHERE DeliveryOrderId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(deliveryOrderHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDeliveryOrderHeaderDTO(deliveryOrderHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting DeliveryOrderHeader ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(deliveryOrderHeaderDTO);
            return deliveryOrderHeaderDTO;
        }

        /// <summary>
        ///  Updates the record to the DeliveryOrderHeader Table.
        /// </summary>
        /// <param name="deliveryOrderHeaderDTO">DeliveryOrderHeaderDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user</param>
        /// <returns> returns the DeliveryOrderHeaderDTO</returns>
        public DeliveryOrderHeaderDTO Update(DeliveryOrderHeaderDTO deliveryOrderHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryOrderHeaderDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[DeliveryOrderHeader]
                           SET 
                            DeliveryOrderDate      =  @DeliveryOrderDate,
                            DeliveryOrderNumber    =  @DeliveryOrderNumber,
                            Type                   =  @Type,
                            PurchaseOrderId        =  @PurchaseOrderId,
                            Issuer                 =  @Issuer,
                            DeliverTo              =  @DeliverTo,
                            Status                 =  @Status,
                            ReceivedDate           =  GETDATE(),
                            ReceivedBy             =  @ReceivedBy,
                            GRN                    =  @GRN,
                            Remarks                =  @Remarks,
                            LastUpdatedBy          =  @LastUpdatedBy,
                            LastUpdatedDate        =  GETDATE(),
                            MasterEntityId         =  @MasterEntityId
                           WHERE DeliveryOrderId = @DeliveryOrderId 
                           SELECT * FROM DeliveryOrderHeader WHERE DeliveryOrderId = @DeliveryOrderId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(deliveryOrderHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDeliveryOrderHeaderDTO(deliveryOrderHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating DeliveryOrderHeader ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(deliveryOrderHeaderDTO);
            return deliveryOrderHeaderDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="deliveryOrderHeaderDTO">DeliveryOrderHeaderDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshDeliveryOrderHeaderDTO(DeliveryOrderHeaderDTO deliveryOrderHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(deliveryOrderHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                deliveryOrderHeaderDTO.DeliveryOrderId = Convert.ToInt32(dt.Rows[0]["DeliveryOrderId"]);
                deliveryOrderHeaderDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                deliveryOrderHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                deliveryOrderHeaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                deliveryOrderHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                deliveryOrderHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                deliveryOrderHeaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
                                          
        }

        /// <summary>
        /// Returns the List of DeliveryOrderHeaderDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the list of DeliveryOrderHeaderDTO</returns>
        public List<DeliveryOrderHeaderDTO> GetDeliveryOrderHeaderDTOList(List<KeyValuePair<DeliveryOrderHeaderDTO.SearchByParameters, string>> searchParameters ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<DeliveryOrderHeaderDTO> deliveryOrderHeaderDTOList = new List<DeliveryOrderHeaderDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<DeliveryOrderHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DeliveryOrderHeaderDTO.SearchByParameters.DELIVERY_ORDER_ID
                            || searchParameter.Key == DeliveryOrderHeaderDTO.SearchByParameters.PURCHASE_ORDER_ID
                            || searchParameter.Key == DeliveryOrderHeaderDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DeliveryOrderHeaderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        if (searchParameter.Key == DeliveryOrderHeaderDTO.SearchByParameters.DELIVERY_ORDER_NUMBER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        if (searchParameter.Key == DeliveryOrderHeaderDTO.SearchByParameters.DELIVERY_ORDER_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE())>=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                    DeliveryOrderHeaderDTO deliveryOrderHeaderDTO = GetDeliveryOrderHeaderDTO(dataRow);
                    deliveryOrderHeaderDTOList.Add(deliveryOrderHeaderDTO);
                }
            }
            log.LogMethodExit(deliveryOrderHeaderDTOList);
            return deliveryOrderHeaderDTOList;
        }
    }
}
