/********************************************************************************************
 * Project Name - DeliveryIntegration 
 * Description  - Data handler for OnlineOrderDeliveryIntegration  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 2.140.3      11-Jul-2022   Guru S A       Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text; 

namespace Semnox.Parafait.DeliveryIntegration
{
    internal class OnlineOrderDeliveryIntegrationDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM OnlineOrderDeliveryIntegration as oodi ";
        /// <summary>
        /// Dictionary for searching Parameters for the OnlineOrderDeliveryIntegration object.
        /// </summary>
        private static readonly Dictionary<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>
        {
            { OnlineOrderDeliveryIntegrationDTO.SearchByParameters.DELIVERY_INTEGRATION_ID,"oodi.DeliveryIntegrationId"},
            { OnlineOrderDeliveryIntegrationDTO.SearchByParameters.INTEGRATION_NAME,"oodi.IntegrationName"},
            { OnlineOrderDeliveryIntegrationDTO.SearchByParameters.IS_ACTIVE,"oodi.IsActive"},
            { OnlineOrderDeliveryIntegrationDTO.SearchByParameters.SITE_ID,"oodi.site_id"}
        };

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public OnlineOrderDeliveryIntegrationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating the Record.
        /// </summary>
        /// <param name="onlineOrderDeliveryIntegrationDTO">OnlineOrderDeliveryIntegrationDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(OnlineOrderDeliveryIntegrationDTO onlineOrderDeliveryIntegrationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(onlineOrderDeliveryIntegrationDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryIntegrationId", onlineOrderDeliveryIntegrationDTO.DeliveryIntegrationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IntegrationName", onlineOrderDeliveryIntegrationDTO.IntegrationName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SemnoxWebhookAPIAuthorizationKey", onlineOrderDeliveryIntegrationDTO.SemnoxWebhookAPIAuthorizationKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IntegratorAPIAuthorizationKey", onlineOrderDeliveryIntegrationDTO.IntegratorAPIAuthorizationKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IntegratorAPIBaseURL", onlineOrderDeliveryIntegrationDTO.IntegratorAPIBaseURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StoreCreationCallBackURL", onlineOrderDeliveryIntegrationDTO.StoreCreationCallBackURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StoreActionsCallBackURL", onlineOrderDeliveryIntegrationDTO.StoreActionsCallBackURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CatalogueIngestionCallBackURL", onlineOrderDeliveryIntegrationDTO.CatalogueIngestionCallBackURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ItemActionsCallBackURL", onlineOrderDeliveryIntegrationDTO.ItemActionsCallBackURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ItemOptionActionsCallBackURL", onlineOrderDeliveryIntegrationDTO.ItemOptionActionsCallBackURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderRelayCallBackURL", onlineOrderDeliveryIntegrationDTO.OrderRelayCallBackURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderStatusChangeCallBackURL", onlineOrderDeliveryIntegrationDTO.OrderStatusChangeCallBackURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RiderStatusChangeCallBackURL", onlineOrderDeliveryIntegrationDTO.RiderStatusChangeCallBackURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FullfillmentModes", onlineOrderDeliveryIntegrationDTO.FullfillmentModes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PackagingChargeProductName", onlineOrderDeliveryIntegrationDTO.PackagingChargeProductName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PackagingChargePercentage", onlineOrderDeliveryIntegrationDTO.PackagingChargePercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentModeName", onlineOrderDeliveryIntegrationDTO.PaymentModeName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AggregatorDiscountName", onlineOrderDeliveryIntegrationDTO.AggregatorDiscountName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductTaxNameList", onlineOrderDeliveryIntegrationDTO.ProductTaxNameList));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumInventoryQtyForItems", onlineOrderDeliveryIntegrationDTO.MinimumInventoryQtyForItems));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FoodTypesSegmentName", onlineOrderDeliveryIntegrationDTO.FoodTypesSegmentName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GoodsTypeSegmentName", onlineOrderDeliveryIntegrationDTO.GoodsTypeSegmentName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TooManyRequestErrorCode", onlineOrderDeliveryIntegrationDTO.TooManyRequestErrorCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PackageChargeCode", onlineOrderDeliveryIntegrationDTO.PackageChargeCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PackageChargeApplicableOn", onlineOrderDeliveryIntegrationDTO.PackageChargeApplicableOn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", onlineOrderDeliveryIntegrationDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", onlineOrderDeliveryIntegrationDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to DTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of OnlineOrderDeliveryIntegrationDTO</returns>
        private OnlineOrderDeliveryIntegrationDTO GetOnlineOrderDeliveryIntegrationDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            OnlineOrderDeliveryIntegrationDTO onlineOrderDeliveryIntegrationDTO = new OnlineOrderDeliveryIntegrationDTO(
                                        dataRow["deliveryIntegrationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["deliveryIntegrationId"]),
                                        dataRow["integrationName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["integrationName"]),
                                        dataRow["semnoxWebhookAPIAuthorizationKey"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["semnoxWebhookAPIAuthorizationKey"]),
                                        dataRow["integratorAPIAuthorizationKey"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["integratorAPIAuthorizationKey"]),
                                        dataRow["integratorAPIBaseURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["integratorAPIBaseURL"]),
                                        dataRow["storeCreationCallBackURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["storeCreationCallBackURL"]),
                                        dataRow["storeActionsCallBackURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["storeActionsCallBackURL"]),
                                        dataRow["catalogueIngestionCallBackURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["catalogueIngestionCallBackURL"]),
                                        dataRow["itemActionsCallBackURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["itemActionsCallBackURL"]),
                                        dataRow["itemOptionActionsCallBackURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["itemOptionActionsCallBackURL"]),
                                        dataRow["orderRelayCallBackURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["orderRelayCallBackURL"]),
                                        dataRow["orderStatusChangeCallBackURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["orderStatusChangeCallBackURL"]),
                                        dataRow["riderStatusChangeCallBackURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["riderStatusChangeCallBackURL"]),
                                        dataRow["fullfillmentModes"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["fullfillmentModes"]),
                                        dataRow["PackagingChargeProductName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PackagingChargeProductName"]),
                                        dataRow["packagingChargePercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["packagingChargePercentage"]),
                                        dataRow["paymentModeName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["paymentModeName"]),
                                        dataRow["aggregatorDiscountName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["aggregatorDiscountName"]), 
                                        dataRow["productTaxNameList"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["productTaxNameList"]),
                                        dataRow["minimumInventoryQtyForItems"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["minimumInventoryQtyForItems"]),
                                        dataRow["foodTypesSegmentName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["foodTypesSegmentName"]),
                                        dataRow["goodsTypeSegmentName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["goodsTypeSegmentName"]),
                                        dataRow["tooManyRequestErrorCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["tooManyRequestErrorCode"]),
                                        dataRow["packageChargeCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["packageChargeCode"]),
                                        dataRow["packageChargeApplicableOn"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["packageChargeApplicableOn"]),
                                        dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                        dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                        dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                        dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                        dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                        dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                        dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                        dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                        dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));
            return onlineOrderDeliveryIntegrationDTO;
        }

        /// <summary>
        /// GetOnlineOrderDeliveryIntegration
        /// </summary>
        /// <param name="deliveryIntegrationId"></param>
        /// <returns></returns>
        public OnlineOrderDeliveryIntegrationDTO GetOnlineOrderDeliveryIntegration(int deliveryIntegrationId)
        {
            log.LogMethodEntry(deliveryIntegrationId);
            OnlineOrderDeliveryIntegrationDTO result = null;
            string query = SELECT_QUERY + @" WHERE oodi.DeliveryIntegrationId = @DeliveryIntegrationId";
            SqlParameter parameter = new SqlParameter("@DeliveryIntegrationId", deliveryIntegrationId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetOnlineOrderDeliveryIntegrationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="OnlineOrderDeliveryIntegrationDTO">OnlineOrderDeliveryIntegrationDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshOnlineOrderDeliveryIntegrationDTO(OnlineOrderDeliveryIntegrationDTO onlineOrderDeliveryIntegrationDTO, DataTable dt)
        {
            log.LogMethodEntry(onlineOrderDeliveryIntegrationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                onlineOrderDeliveryIntegrationDTO.DeliveryIntegrationId = Convert.ToInt32(dt.Rows[0]["DeliveryIntegrationId"]);
                onlineOrderDeliveryIntegrationDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                onlineOrderDeliveryIntegrationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                onlineOrderDeliveryIntegrationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                onlineOrderDeliveryIntegrationDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                onlineOrderDeliveryIntegrationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                onlineOrderDeliveryIntegrationDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the record to the database
        /// </summary>
        /// <param name="onlineOrderDeliveryIntegrationDTO">OnlineOrderDeliveryIntegrationDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns OnlineOrderDeliveryIntegrationDTO</returns>
        public OnlineOrderDeliveryIntegrationDTO Insert(OnlineOrderDeliveryIntegrationDTO onlineOrderDeliveryIntegrationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(onlineOrderDeliveryIntegrationDTO, loginId, siteId);
            string insertQuery = @"insert into OnlineOrderDeliveryIntegration 
                                  ( IntegrationName,
	                                SemnoxWebhookAPIAuthorizationKey,
	                                IntegratorAPIAuthorizationKey,
	                                IntegratorAPIBaseURL,
	                                StoreCreationCallBackURL,
	                                StoreActionsCallBackURL,
	                                CatalogueIngestionCallBackURL, 
	                                ItemActionsCallBackURL,
	                                ItemOptionActionsCallBackURL, 
	                                OrderRelayCallBackURL,
	                                OrderStatusChangeCallBackURL, 
	                                RiderStatusChangeCallBackURL,
	                                FullfillmentModes,
	                                PackagingChargeProductName,
	                                PackagingChargePercentage,
	                                PaymentModeName,
	                                AggregatorDiscountName,
	                                ProductTaxNameList,
	                                MinimumInventoryQtyForItems,
	                                FoodTypesSegmentName,
	                                GoodsTypeSegmentName,
	                                TooManyRequestErrorCode,
	                                PackageChargeCode ,
	                                PackageChargeApplicableOn,
	                                IsActive,
	                                CreatedBy,
	                                CreationDate,
	                                LastUpdatedBy,
	                                LastUpdatedDate,
	                                Guid,
	                                site_id,     
	                                MasterEntityId
                             )  values  (         
	                                @IntegrationName,
	                                @SemnoxWebhookAPIAuthorizationKey,
	                                @IntegratorAPIAuthorizationKey,
	                                @IntegratorAPIBaseURL,
	                                @StoreCreationCallBackURL,
	                                @StoreActionsCallBackURL,
	                                @CatalogueIngestionCallBackURL, 
	                                @ItemActionsCallBackURL,
	                                @ItemOptionActionsCallBackURL, 
	                                @OrderRelayCallBackURL,
	                                @OrderStatusChangeCallBackURL, 
	                                @RiderStatusChangeCallBackURL,
	                                @FullfillmentModes,
	                                @PackagingChargeProductName,
	                                @PackagingChargePercentage,
	                                @PaymentModeName,
	                                @AggregatorDiscountName,
	                                @ProductTaxNameList,
	                                @MinimumInventoryQtyForItems,
	                                @FoodTypesSegmentName,
	                                @GoodsTypeSegmentName,
	                                @TooManyRequestErrorCode,
	                                @PackageChargeCode ,
	                                @PackageChargeApplicableOn,
	                                @IsActive,
	                                @CreatedBy,
	                                GETDATE(),
	                                @LastUpdatedBy,
	                                GETDATE(),
	                                NEWID(),
	                                @Site_id,     
	                                @MasterEntityId
                            ) SELECT * FROM OnlineOrderDeliveryIntegration WHERE DeliveryIntegrationId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(onlineOrderDeliveryIntegrationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOnlineOrderDeliveryIntegrationDTO(onlineOrderDeliveryIntegrationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting onlineOrderDeliveryIntegrationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(onlineOrderDeliveryIntegrationDTO);
            return onlineOrderDeliveryIntegrationDTO;
        }
        /// <summary>
        /// Update the record in to the Table. 
        /// </summary>
        /// <param name="onlineOrderDeliveryIntegrationDTO">OnlineOrderDeliveryIntegrationDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated OnlineOrderDeliveryIntegrationDTO</returns>
        public OnlineOrderDeliveryIntegrationDTO Update(OnlineOrderDeliveryIntegrationDTO onlineOrderDeliveryIntegrationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(onlineOrderDeliveryIntegrationDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[OnlineOrderDeliveryIntegration]
                            SET IntegrationName = @IntegrationName,
                                SemnoxWebhookAPIAuthorizationKey = @SemnoxWebhookAPIAuthorizationKey,
                                IntegratorAPIAuthorizationKey = @IntegratorAPIAuthorizationKey,
                                IntegratorAPIBaseURL = @IntegratorAPIBaseURL,
                                StoreCreationCallBackURL = @StoreCreationCallBackURL,
                                StoreActionsCallBackURL = @StoreActionsCallBackURL,
                                CatalogueIngestionCallBackURL = @CatalogueIngestionCallBackURL, 
                                ItemActionsCallBackURL = @ItemActionsCallBackURL,
                                ItemOptionActionsCallBackURL = @ItemOptionActionsCallBackURL, 
                                OrderRelayCallBackURL = @OrderRelayCallBackURL,
                                OrderStatusChangeCallBackURL = @OrderStatusChangeCallBackURL, 
                                RiderStatusChangeCallBackURL = @RiderStatusChangeCallBackURL,
                                FullfillmentModes = @FullfillmentModes,
                                PackagingChargeProductName = @PackagingChargeProductName,
                                PackagingChargePercentage = @PackagingChargePercentage,
                                PaymentModeName = @PaymentModeName,
                                AggregatorDiscountName = @AggregatorDiscountName,
                                ProductTaxNameList = @ProductTaxNameList,
                                MinimumInventoryQtyForItems = @MinimumInventoryQtyForItems,
                                FoodTypesSegmentName = @FoodTypesSegmentName,
                                GoodsTypeSegmentName = @GoodsTypeSegmentName,
                                TooManyRequestErrorCode = @TooManyRequestErrorCode,
                                PackageChargeCode = @PackageChargeCode ,
                                PackageChargeApplicableOn = @PackageChargeApplicableOn,
                                IsActive = @IsActive, 
                                LastUpdatedBy = @LastUpdatedBy,
                                LastUpdatedDate = GETDATE(), 
                                MasterEntityId =  @MasterEntityId
                          WHERE DeliveryIntegrationId = @DeliveryIntegrationId;
                         SELECT * FROM OnlineOrderDeliveryIntegration WHERE DeliveryIntegrationId = @DeliveryIntegrationId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(onlineOrderDeliveryIntegrationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOnlineOrderDeliveryIntegrationDTO(onlineOrderDeliveryIntegrationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating onlineOrderDeliveryIntegrationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(onlineOrderDeliveryIntegrationDTO);
            return onlineOrderDeliveryIntegrationDTO;
        }

        /// <summary>
        /// Returns the List of OnlineOrderDeliveryIntegrationDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of OnlineOrderDeliveryIntegrationDTO </returns>
        public List<OnlineOrderDeliveryIntegrationDTO> GetOnlineOrderDeliveryIntegrationDTOList(List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>> searchParameters,
                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<OnlineOrderDeliveryIntegrationDTO> dTOList = new List<OnlineOrderDeliveryIntegrationDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == OnlineOrderDeliveryIntegrationDTO.SearchByParameters.DELIVERY_INTEGRATION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        } 
                        else if (searchParameter.Key == OnlineOrderDeliveryIntegrationDTO.SearchByParameters.INTEGRATION_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        } 
                        else if (searchParameter.Key == OnlineOrderDeliveryIntegrationDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OnlineOrderDeliveryIntegrationDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value.ToLower() == "true" || searchParameter.Value == "1" || searchParameter.Value == "Y"));
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
                    OnlineOrderDeliveryIntegrationDTO dtoObject = GetOnlineOrderDeliveryIntegrationDTO(dataRow);
                    dTOList.Add(dtoObject);
                }
            }
            log.LogMethodExit(dTOList);
            return dTOList;
        }

        /// <summary>
        /// Gets the OnlineOrderDeliveryIntegrationDTO List for guidList Value List
        /// </summary>
        /// <param name="deliveryIntegrationuidList">string list parameter</param>
        /// <returns>Returns List of OnlineOrderDeliveryIntegrationDTO</returns>
        public List<OnlineOrderDeliveryIntegrationDTO> GetOnlineOrderDeliveryIntegrationDTOList(List<string> deliveryIntegrationuidList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(deliveryIntegrationuidList);
            List<OnlineOrderDeliveryIntegrationDTO> dtoList = new List<OnlineOrderDeliveryIntegrationDTO>();
            string query = @"SELECT * FROM @DeliveryIntegrationuidList List, OnlineOrderDeliveryIntegration oodi
                                                       where oodi.Guid = List.Value";

            DataTable table = dataAccessHandler.BatchSelect(query, "@DeliveryIntegrationuidList", deliveryIntegrationuidList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                dtoList = table.Rows.Cast<DataRow>().Select(x => GetOnlineOrderDeliveryIntegrationDTO(x)).ToList();
            }
            log.LogMethodExit(dtoList);
            return dtoList;
        }
        internal bool GetIsRecordReferenced(int deliveryIntegrationId)
        {
            log.LogMethodEntry(deliveryIntegrationId);
            int referenceCount = 0;
            string query = @"SELECT COUNT(1) AS ReferenceCount
                            FROM DeliveryChannels
                            WHERE DeliveryIntegrationId = @DeliveryIntegrationId
                            AND IsActive = 1 ";
            SqlParameter parameter = new SqlParameter("@DeliveryIntegrationId", deliveryIntegrationId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                referenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            bool result = referenceCount > 0;
            log.LogMethodExit(result);
            return result;
        }

        internal DateTime? GetDeliveryIntegrationModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastupdatedDate) LastUpdatedDate from OnlineOrderDeliveryIntegration WHERE (site_id = @siteId or @siteId = -1) 
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from DeliveryChannels WHERE (site_id = @siteId or @siteId = -1)
                            ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
