/********************************************************************************************
 * Project Name - Semnox.Parafait.DeliveryIntegration                                                                        
 * Description  - DeliveryIntegration Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
  *2.140.0     01-Jun-2021   Fiona Lishal     Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// DeliveryChannelDataHandler
    /// </summary>
    public class DeliveryChannelDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM DeliveryChannels AS dch ";
        private static readonly Dictionary<DeliveryChannelDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DeliveryChannelDTO.SearchByParameters, string>
            {
                {DeliveryChannelDTO.SearchByParameters.DELIVERY_CHANNEL_ID, "dch.DeliveryChannelId"},
                {DeliveryChannelDTO.SearchByParameters.CHANNEL_NAME, "dch.ChannelName"},
                {DeliveryChannelDTO.SearchByParameters.IS_ACTIVE, "dch.IsActive"},
                {DeliveryChannelDTO.SearchByParameters.MASTER_ENTITY_ID, "dch.MasterEntityId"},
                {DeliveryChannelDTO.SearchByParameters.SITE_ID, "dch.site_id"},
                {DeliveryChannelDTO.SearchByParameters.DELIVERY_INTEGRATION_ID, "dch.DeliveryIntegrationId"},
                {DeliveryChannelDTO.SearchByParameters.DELIVERY_INTEGRATION_NAME, "di.IntegrationName"}
            };
        /// <summary>
        /// Default constructor of OrderDetailDataHandler class
        /// </summary>
        public DeliveryChannelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DeliveryChannelDTO Record.
        /// </summary>
        /// <param name="deliveryChannelDTO">DeliveryChannelDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(DeliveryChannelDTO deliveryChannelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryChannelDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryChannelId", deliveryChannelDTO.DeliveryChannelId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChannelName", deliveryChannelDTO.ChannelName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChannelAPIUrl", deliveryChannelDTO.ChannelAPIUrl));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChannelAPIKey", deliveryChannelDTO.ChannelAPIKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", deliveryChannelDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AutoAcceptOrders", deliveryChannelDTO.AutoAcceptOrders));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ManualRiderAssignmentAllowed", deliveryChannelDTO.ManualRiderAssignmentAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DefaultRiderId", deliveryChannelDTO.DefaultRiderId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSourceReference", deliveryChannelDTO.ExternalSourceReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", deliveryChannelDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReConfirmOrder", deliveryChannelDTO.ReConfirmOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReConfirmPreparation", deliveryChannelDTO.ReConfirmPreparation));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryIntegrationId", deliveryChannelDTO.DeliveryIntegrationId, true));  
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryChannelDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DeliveryChannelDTO Insert(DeliveryChannelDTO deliveryChannelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryChannelDTO, loginId, siteId);
            string insertQuery = @"insert into DeliveryChannels 
                                                        (                                                         
                                                       ChannelName ,
                                                       ChannelAPIUrl,
                                                       ChannelAPIKey,
                                                       AutoAcceptOrders,
                                                       ManualRiderAssignmentAllowed,
                                                       DefaultRiderId,
                                                       ExternalSourceReference,
                                                       ReConfirmOrder ,
	                                                   ReConfirmPreparation  ,
                                                       IsActive ,
                                                       CreatedBy ,
                                                       CreationDate ,
                                                       LastUpdatedBy ,
                                                       LastUpdatedDate ,
                                                       Guid ,
                                                       site_id   ,
                                                       MasterEntityId,
                                                       DeliveryIntegrationId
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @ChannelName ,
                                                       @ChannelAPIUrl,
                                                       @ChannelAPIKey,
                                                       @AutoAcceptOrders,
                                                       @ManualRiderAssignmentAllowed,
                                                       @DefaultRiderId,
                                                       @ExternalSourceReference,
                                                       @ReConfirmOrder ,
	                                                   @ReConfirmPreparation, 
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId,
                                                       @DeliveryIntegrationId
                                          )SELECT  * from DeliveryChannels where DeliveryChannelId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(deliveryChannelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDeliveryChannelDTO(deliveryChannelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting DeliveryChannelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(deliveryChannelDTO);
            return deliveryChannelDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryChannelDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DeliveryChannelDTO Update(DeliveryChannelDTO deliveryChannelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(deliveryChannelDTO, loginId, siteId);
            string updateQuery = @"update DeliveryChannels 
                                         set 
                                            ChannelName = @ChannelName,
                                            ChannelAPIUrl = @ChannelAPIUrl,
                                            ChannelAPIKey = @ChannelAPIKey,
                                            AutoAcceptOrders = @AutoAcceptOrders,
                                            ManualRiderAssignmentAllowed= @ManualRiderAssignmentAllowed,
                                            DefaultRiderId= @DefaultRiderId,
                                            ExternalSourceReference= @ExternalSourceReference,
                                            ReConfirmOrder =@ReConfirmOrder,
	                                        ReConfirmPreparation  =@ReConfirmPreparation,
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId ,
                                            DeliveryIntegrationId = @DeliveryIntegrationId
                                      WHERE DeliveryChannelId =  @DeliveryChannelId;
                                        SELECT  * from DeliveryChannels where DeliveryChannelId = @DeliveryChannelId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(deliveryChannelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDeliveryChannelDTO(deliveryChannelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating DeliveryChannelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(deliveryChannelDTO);
            return deliveryChannelDTO;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="DeliveryChannelDTO"></param>
        ///// <returns></returns>
        //public DeliveryChannelDTO UpdateDeliveryChannelDTO(DeliveryChannelDTO DeliveryChannelDTO)
        //{
        //    log.LogMethodEntry(DeliveryChannelDTO);
        //    string updateQuery = @"update DeliveryChannels 
        //                                 set 
        //                                    ChannelName = @ChannelName,
        //                                    ChannelAPIUrl = @ChannelAPIUrl,
        //                                    ChannelAPIKey = @ChannelAPIKey,
        //                                    AutoAcceptOrders = @AutoAcceptOrders,
        //                                    ManualRiderAssignmentAllowed= @ManualRiderAssignmentAllowed,
        //                                    DefaultRiderId= @DefaultRiderId,
        //                                    IsActive = @IsActive
        //                                       where   DeliveryChannelId =  @DeliveryChannelId  
        //                                )SELECT  * from DeliveryChannels where DeliveryChannelId = @DeliveryChannelId ";
        //    try
        //    {
        //        DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(DeliveryChannelDTO, string.Empty, -1).ToArray(), sqlTransaction);
        //        RefreshDeliveryChannelDTO(DeliveryChannelDTO, dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occurred while Updating DeliveryChannelDTO", ex);
        //        log.LogMethodExit(null, "Throwing exception - " + ex.Message);
        //        throw;
        //    }
        //    log.LogMethodExit(DeliveryChannelDTO);
        //    return DeliveryChannelDTO;
        //}
        private void RefreshDeliveryChannelDTO(DeliveryChannelDTO deliveryChannelDTO, DataTable dt)
        {
            log.LogMethodEntry(deliveryChannelDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                deliveryChannelDTO.DeliveryChannelId = Convert.ToInt32(dt.Rows[0]["DeliveryChannelId"]);
                deliveryChannelDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                deliveryChannelDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                deliveryChannelDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                deliveryChannelDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                deliveryChannelDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                deliveryChannelDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private DeliveryChannelDTO GetDeliveryChannelDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow); 
            DeliveryChannelDTO deliveryChannelDTO = new DeliveryChannelDTO(Convert.ToInt32(dataRow["DeliveryChannelId"]),
                                                    dataRow["ChannelName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ChannelName"]),
                                                    dataRow["ChannelAPIUrl"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ChannelAPIUrl"]),
                                                    dataRow["ChannelAPIKey"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ChannelAPIKey"]),
                                                    dataRow["AutoAcceptOrders"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AutoAcceptOrders"]),
                                                    dataRow["ManualRiderAssignmentAllowed"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ManualRiderAssignmentAllowed"]),
                                                    dataRow["DefaultRiderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DefaultRiderId"]),
                                                    dataRow["ExternalSourceReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalSourceReference"]),
                                                    dataRow["ReconfirmOrder"] == DBNull.Value ? true: (Convert.ToBoolean(dataRow["ReconfirmOrder"])),
                                                    dataRow["ReConfirmPreparation"] == DBNull.Value ? true : (Convert.ToBoolean(dataRow["ReConfirmPreparation"])),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["DeliveryIntegrationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryIntegrationId"])
                                                    );
            log.LogMethodExit(deliveryChannelDTO);
            return deliveryChannelDTO;
        }

        /// <summary>
        /// Gets the DeliveryChannelDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DeliveryChannelDTO</returns>
        internal DeliveryChannelDTO GetDeliveryChannelDTO(int id)
        {
            log.LogMethodEntry(id);
            DeliveryChannelDTO deliveryChannelDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where dch.DeliveryChannelId = @DeliveryChannelId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@DeliveryChannelId", id);
            DataTable deliveryChannelIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (deliveryChannelIdTable.Rows.Count > 0)
            {
                DataRow gameMachineLevelRow = deliveryChannelIdTable.Rows[0];
                deliveryChannelDTO = GetDeliveryChannelDTO(gameMachineLevelRow);
            }
            log.LogMethodExit(deliveryChannelDTO);
            return deliveryChannelDTO;

        }

        /// <summary>
        /// GetDeliveryChannels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<DeliveryChannelDTO> GetDeliveryChannels(List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<DeliveryChannelDTO> deliveryChannelDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<DeliveryChannelDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DeliveryChannelDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DeliveryChannelDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(DeliveryChannelDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(DeliveryChannelDTO.SearchByParameters.DELIVERY_CHANNEL_ID) ||
                                 searchParameter.Key.Equals(DeliveryChannelDTO.SearchByParameters.DELIVERY_INTEGRATION_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DeliveryChannelDTO.SearchByParameters.CHANNEL_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DeliveryChannelDTO.SearchByParameters.DELIVERY_INTEGRATION_NAME)
                        {
                            query.Append(joiner + @" exists (select 1
                                                              from OnlineOrderDeliveryIntegration di
			                                                 where di.DeliveryIntegrationId = dch.DeliveryIntegrationId
			                                                  and " + DBSearchParameters[searchParameter.Key] + " = "
                                                               + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                deliveryChannelDTOList = new List<DeliveryChannelDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    DeliveryChannelDTO DeliveryChannelDTO = GetDeliveryChannelDTO(dataRow);
                    deliveryChannelDTOList.Add(DeliveryChannelDTO);
                }
            }
            log.LogMethodExit(deliveryChannelDTOList);
            return deliveryChannelDTOList;
        }

        internal List<DeliveryChannelDTO> GetDeliveryChannelDTOList(List<int> deliveryIntegrationIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(deliveryIntegrationIdList, activeRecords);
            List<DeliveryChannelDTO> deliveryChannelDTOList = new List<DeliveryChannelDTO>();
            string query = @"SELECT *
                            FROM DeliveryChannels dc, @DeliveryIntegrationIdList List
                            WHERE dc.DeliveryIntegrationId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@DeliveryIntegrationIdList", deliveryIntegrationIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                deliveryChannelDTOList = table.Rows.Cast<DataRow>().Select(x => GetDeliveryChannelDTO(x)).ToList();
            }
            log.LogMethodExit(deliveryChannelDTOList);
            return deliveryChannelDTOList;
        }

    }
}
