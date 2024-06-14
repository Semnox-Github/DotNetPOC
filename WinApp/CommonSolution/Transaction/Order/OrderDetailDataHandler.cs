/********************************************************************************************
 * Project Name - Transactions                                                                       
 * Description  - OrderDetailBL - Business logic class for the order details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// OrderDetailDataHandler 
    /// </summary>
    public class OrderDetailDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM OrderDetails AS odl ";
        private static readonly Dictionary<OrderDetailDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<OrderDetailDTO.SearchByParameters, string>
            {
                {OrderDetailDTO.SearchByParameters.ORDER_DETAIL_ID, "odl.OrderDetailId"},
                {OrderDetailDTO.SearchByParameters.ORDER_ID_LIST, "odl.OrderDetailId"},
                {OrderDetailDTO.SearchByParameters.ORDER_ID, "odl.OrderId"},
                {OrderDetailDTO.SearchByParameters.DELIVERY_CHANNEL_ID, "odl.DeliveryChannelId"},
                {OrderDetailDTO.SearchByParameters.RIDER_NAME, "odl.RiderName"},
                {OrderDetailDTO.SearchByParameters.IS_ACTIVE, "odl.IsActive"},
                {OrderDetailDTO.SearchByParameters.MASTER_ENTITY_ID, "odl.MasterEntityId"},
                {OrderDetailDTO.SearchByParameters.SITE_ID, "odl.site_id"},
                {OrderDetailDTO.SearchByParameters.LAST_UPDATED_DATE, "odl.LastUpdatedDate"}
            };
        /// <summary>
        /// Default constructor of OrderDetailDataHandler class
        /// </summary>
        public OrderDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating OrderDetailDTO Record.
        /// </summary>
        /// <param name="orderDetailDTO">OrderDetailDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(OrderDetailDTO orderDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderDetailId", orderDetailDTO.OrderDetailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderId", orderDetailDTO.OrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryChannelId", orderDetailDTO.DeliveryChannelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RiderName", orderDetailDTO.RiderName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RiderDeliveryStatus", orderDetailDTO.RiderDeliveryStatus,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RiderPhoneNumber", orderDetailDTO.RiderPhoneNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", orderDetailDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", orderDetailDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", orderDetailDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryType", orderDetailDTO.DeliveryType));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public OrderDetailDTO Insert(OrderDetailDTO OrderDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(OrderDetailDTO, loginId, siteId);
            string insertQuery = @"insert into OrderDetails 
                                                        (                                                         
                                                       OrderId ,
                                                       DeliveryChannelId,
                                                       RiderName,
                                                       RiderPhoneNumber ,
                                                       RiderDeliveryStatus,
                                                       Remarks,
                                                       IsActive ,
                                                       CreatedBy ,
                                                       CreationDate ,
                                                       LastUpdatedBy ,
                                                       LastUpdatedDate ,
                                                       Guid ,
                                                       site_id   ,
                                                       MasterEntityId, 
                                                       DeliveryType 
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @OrderId ,
                                                       @DeliveryChannelId,
                                                       @RiderName,
                                                       @RiderPhoneNumber ,
                                                       @RiderDeliveryStatus,
                                                       @Remarks,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId, 
                                                       @DeliveryType 
                                          )SELECT  * from OrderDetails where OrderDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(OrderDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderDetailDTO(OrderDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting OrderDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(OrderDetailDTO);
            return OrderDetailDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public OrderDetailDTO Update(OrderDetailDTO orderDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderDetailDTO, loginId, siteId);
            string updateQuery = @"update OrderDetails 
                                         set 
                                            OrderId  =  @OrderId,
                                            DeliveryChannelId= @DeliveryChannelId,
                                            RiderName=  @RiderName,
                                            IsActive=  @IsActive,
                                            RiderPhoneNumber= @RiderPhoneNumber ,
                                            RiderDeliveryStatus= @RiderDeliveryStatus,
                                            Remarks= @Remarks,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            DeliveryType = @DeliveryType,
                                            MasterEntityId =  @MasterEntityId 
                                               where   OrderDetailId =  @OrderDetailId  
                                    SELECT  * from OrderDetails where  OrderDetailId = @OrderDetailId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(orderDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderDetailDTO(orderDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating OrderDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(orderDetailDTO);
            return orderDetailDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderDetailDTO"></param>
        /// <returns></returns>
        public OrderDetailDTO UpdateOrderDetailDTO(OrderDetailDTO OrderDetailDTO)
        {
            log.LogMethodEntry(OrderDetailDTO);
            string updateQuery = @"update OrderDetails 
                                         set 
                                            OrderId  =  @OrderId,
                                            DeliveryChannelId= @DeliveryChannelId,
                                            RiderName=  @RiderName,
                                            IsActive=  @IsActive,
                                            RiderPhoneNumber= @RiderPhoneNumber ,
                                            RiderDeliveryStatus= @RiderDeliveryStatus,
                                            DeliveryType= @DeliveryType,
                                            Remarks= @Remarks
                                    where   OrderDetailId =  @OrderDetailId  
                                    SELECT  * from OrderDetails where  OrderDetailId = @OrderDetailId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(OrderDetailDTO, string.Empty, -1).ToArray(), sqlTransaction);
                RefreshOrderDetailDTO(OrderDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating OrderDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(OrderDetailDTO);
            return OrderDetailDTO;
        }
        private void RefreshOrderDetailDTO(OrderDetailDTO orderDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(orderDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                orderDetailDTO.OrderDetailId = Convert.ToInt32(dt.Rows[0]["OrderDetailId"]);
                orderDetailDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                orderDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                orderDetailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                orderDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                orderDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                orderDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private OrderDetailDTO GetOrderDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            OrderDetailDTO orderDetailDTO = new OrderDetailDTO(Convert.ToInt32(dataRow["OrderDetailId"]),
                                                    dataRow["OrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderId"]),
                                                    dataRow["DeliveryChannelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryChannelId"]),
                                                    dataRow["RiderName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RiderName"]),
                                                    dataRow["RiderPhoneNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RiderPhoneNumber"]),
                                                    dataRow["RiderDeliveryStatus"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RiderDeliveryStatus"]),
                                                    dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["DeliveryType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DeliveryType"])
                                                    );
            log.LogMethodExit(orderDetailDTO);
            return orderDetailDTO;
        }

        /// <summary>
        /// Gets the OrderDetailDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns OrderDetailDTO</returns>
        internal OrderDetailDTO GetOrderDetailDTO(int id)
        {
            log.LogMethodEntry(id);
            OrderDetailDTO orderDetailDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where odl.OrderDetailId = @OrderDetailId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@OrderDetailId", id);
            DataTable orderDetailTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (orderDetailTable.Rows.Count > 0)
            {
                DataRow gameMachineLevelRow = orderDetailTable.Rows[0];
                orderDetailDTO = GetOrderDetailDTO(gameMachineLevelRow);
            }
            log.LogMethodExit(orderDetailDTO);
            return orderDetailDTO;

        }

        internal List<OrderDetailDTO> GetOrderDetailDTOList(List<int> orderHeaderIdList, bool activeRecords)
        {
            log.LogMethodEntry(orderHeaderIdList);
            List<OrderDetailDTO> orderDetailDTOList = new List<OrderDetailDTO>();
            string query = @"SELECT *
                            FROM OrderDetails, @orderHeaderIdList List
                            WHERE OrderId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@orderHeaderIdList", orderHeaderIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                orderDetailDTOList = table.Rows.Cast<DataRow>().Select(x => GetOrderDetailDTO(x)).ToList();
            }
            log.LogMethodExit(orderDetailDTOList);
            return orderDetailDTOList;
        }
        public List<OrderDetailDTO> GetOrderDetails(List<KeyValuePair<OrderDetailDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<OrderDetailDTO> orderDetailDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<OrderDetailDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == OrderDetailDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderDetailDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(OrderDetailDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(OrderDetailDTO.SearchByParameters.ORDER_ID) ||
                                  searchParameter.Key.Equals(OrderDetailDTO.SearchByParameters.DELIVERY_CHANNEL_ID) ||
                                  searchParameter.Key.Equals(OrderDetailDTO.SearchByParameters.ORDER_DETAIL_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderDetailDTO.SearchByParameters.RIDER_NAME 
                                || searchParameter.Key == OrderDetailDTO.SearchByParameters.DELIVERY_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(OrderDetailDTO.SearchByParameters.LAST_UPDATED_DATE))
                        {
                            query.Append(joiner + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        counter++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                orderDetailDTOList = new List<OrderDetailDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    OrderDetailDTO OrderDetailDTO = GetOrderDetailDTO(dataRow);
                    orderDetailDTOList.Add(OrderDetailDTO);
                }
            }
            log.LogMethodExit(orderDetailDTOList);
            return orderDetailDTOList;
        }
    }
}
