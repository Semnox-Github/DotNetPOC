/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - TrasactionDeliveryDetails DataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.140.0      01-Jun-2021   Fiona             Created 
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
    /// TransactionOrderDispensingDataHandler
    /// </summary>
    internal class TransactionOrderDispensingDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TransactionOrderDispensing AS tod ";
        private static readonly Dictionary<TransactionOrderDispensingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionOrderDispensingDTO.SearchByParameters, string>
            {
                {TransactionOrderDispensingDTO.SearchByParameters.TRANSACTION_ORDER_DISPENSING_ID, "tod.TransactionOrderDispensingId"},
                {TransactionOrderDispensingDTO.SearchByParameters.TRANSACTION_ID, "tod.TransactionId"},
                {TransactionOrderDispensingDTO.SearchByParameters.IS_ACTIVE, "tod.IsActive"},
                {TransactionOrderDispensingDTO.SearchByParameters.MASTER_ENTITY_ID, "tod.MasterEntityId"},
                {TransactionOrderDispensingDTO.SearchByParameters.SITE_ID, "tod.site_id"}
            };
        /// <summary>
        /// Default constructor of TransactionOrderDispensingDataHandler class
        /// </summary>
        public TransactionOrderDispensingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private List<SqlParameter> BuildSQLParameters(TransactionOrderDispensingDTO transactionOrderDispensingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionOrderDispensingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionOrderDispensingId", transactionOrderDispensingDTO.TransactionOrderDispensingId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", transactionOrderDispensingDTO.TransactionId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryChannelId", transactionOrderDispensingDTO.DeliveryChannelId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScheduledDispensingTime", transactionOrderDispensingDTO.ScheduledDispensingTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReconfirmationOrder", TransactionOrderDispensingDTO.ReConformationStatusToString(transactionOrderDispensingDTO.ReconfirmationOrder)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryAddressId", transactionOrderDispensingDTO.DeliveryAddressId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryContactId", transactionOrderDispensingDTO.DeliveryContactId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReConfirmPreparation", TransactionOrderDispensingDTO.ReConformationStatusToString(transactionOrderDispensingDTO.ReConfirmPreparation)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderDispensingTypeId", transactionOrderDispensingDTO.OrderDispensingTypeId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", transactionOrderDispensingDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryChannelCustomerReferenceNo", transactionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", transactionOrderDispensingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", transactionOrderDispensingDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionOrderDispensingDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public TransactionOrderDispensingDTO Insert(TransactionOrderDispensingDTO transactionOrderDispensingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionOrderDispensingDTO, loginId, siteId);
            string insertQuery = @"insert into TransactionOrderDispensing 
                                                     (                                                         
	                                                    TransactionId ,  
	                                                    OrderDispensingTypeId , 
                                                        DeliveryChannelId ,   
	                                                    ScheduledDispensingTime ,
	                                                    ReconfirmationOrder ,
	                                                    ReConfirmPreparation  ,
                                                        DeliveryAddressId,
                                                        DeliveryContactId,
                                                        ExternalSystemReference,
                                                        DeliveryChannelCustomerReferenceNo,
                                                        IsActive ,
                                                        CreatedBy ,
                                                        CreationDate ,
                                                        LastUpdatedBy ,
                                                        LastUpdatedDate ,
                                                        Guid ,
                                                        site_id   ,
                                                        MasterEntityId 
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @TransactionId,
                                                       @OrderDispensingTypeId,
                                                       @DeliveryChannelId,
                                                       @ScheduledDispensingTime,
                                                       @ReconfirmationOrder,
                                                       @ReConfirmPreparation,
                                                       @DeliveryAddressId,
                                                       @DeliveryContactId, 
                                                       @ExternalSystemReference,
                                                       @DeliveryChannelCustomerReferenceNo,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from TransactionOrderDispensing where TransactionOrderDispensingId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(transactionOrderDispensingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionOrderDispensingDTO(transactionOrderDispensingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TransactionOrderDispensingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionOrderDispensingDTO);
            return transactionOrderDispensingDTO;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="transactionOrderDispensingDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public TransactionOrderDispensingDTO Update(TransactionOrderDispensingDTO transactionOrderDispensingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionOrderDispensingDTO, loginId, siteId);
            string updateQuery = @"update TransactionOrderDispensing 
                                         set 
                                            TransactionId = @TransactionId,  
	                                        OrderDispensingTypeId = @OrderDispensingTypeId, 
                                            DeliveryChannelId = @DeliveryChannelId,   
	                                        ScheduledDispensingTime = @ScheduledDispensingTime,
	                                        ReconfirmationOrder = @ReconfirmationOrder,
	                                        ReConfirmPreparation = @ReConfirmPreparation ,
                                            DeliveryAddressId = @DeliveryAddressId ,
                                            DeliveryContactId = @DeliveryContactId,
                                            ExternalSystemReference = @ExternalSystemReference,
                                            DeliveryChannelCustomerReferenceNo = @DeliveryChannelCustomerReferenceNo,
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId 
                                      where TransactionOrderDispensingId =  @TransactionOrderDispensingId;
                                        SELECT  * from TransactionOrderDispensing where TransactionOrderDispensingId = @TransactionOrderDispensingId; ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(transactionOrderDispensingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionOrderDispensingDTO(transactionOrderDispensingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TransactionOrderDispensingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionOrderDispensingDTO);
            return transactionOrderDispensingDTO;
        }
        
        private void RefreshTransactionOrderDispensingDTO(TransactionOrderDispensingDTO transactionOrderDispensingDTO, DataTable dt)
        {
            log.LogMethodEntry(transactionOrderDispensingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                transactionOrderDispensingDTO.TransactionOrderDispensingId = Convert.ToInt32(dt.Rows[0]["TransactionOrderDispensingId"]);
                transactionOrderDispensingDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                transactionOrderDispensingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                transactionOrderDispensingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                transactionOrderDispensingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                transactionOrderDispensingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                transactionOrderDispensingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private TransactionOrderDispensingDTO GetTransactionOrderDispensingDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionOrderDispensingDTO transactionOrderDispensingDTO = new TransactionOrderDispensingDTO(Convert.ToInt32(dataRow["TransactionOrderDispensingId"]),
                                                    dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                                                    dataRow["OrderDispensingTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderDispensingTypeId"]),
                                                    dataRow["DeliveryChannelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryChannelId"]),
                                                    dataRow["ScheduledDispensingTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ScheduledDispensingTime"]),
                                                    dataRow["ReconfirmationOrder"] == DBNull.Value ? TransactionOrderDispensingDTO.ReConformationStatus.NO : TransactionOrderDispensingDTO.ReConformationStatusFromString(Convert.ToString(dataRow["ReconfirmationOrder"])),
                                                    dataRow["ReConfirmPreparation"] == DBNull.Value ? TransactionOrderDispensingDTO.ReConformationStatus.NO : TransactionOrderDispensingDTO.ReConformationStatusFromString(Convert.ToString(dataRow["ReConfirmPreparation"])),
                                                    dataRow["DeliveryAddressId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryAddressId"]),
                                                    dataRow["DeliveryContactId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryContactId"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalSystemReference"]),
                                                    dataRow["DeliveryChannelCustomerReferenceNo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DeliveryChannelCustomerReferenceNo"])
                                                    );
            log.LogMethodExit(transactionOrderDispensingDTO);
            return transactionOrderDispensingDTO;
        }
        internal TransactionOrderDispensingDTO GetTransactionOrderDispensingDTO(int id)
        {
            log.LogMethodEntry(id);
            TransactionOrderDispensingDTO transactionOrderDispensingDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where tod.TransactionOrderDispensingId = @TransactionOrderDispensingId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@TransactionOrderDispensingId", id);
            DataTable deliveryChannelIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (deliveryChannelIdTable.Rows.Count > 0)
            {
                DataRow dataRow = deliveryChannelIdTable.Rows[0];
                transactionOrderDispensingDTO = GetTransactionOrderDispensingDTO(dataRow);
            }
            log.LogMethodExit(transactionOrderDispensingDTO);
            return transactionOrderDispensingDTO;
        }
        public List<TransactionOrderDispensingDTO> GetTransactionOrderDispensingDTO(List<KeyValuePair<TransactionOrderDispensingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<TransactionOrderDispensingDTO> transactionOrderDispensingDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionOrderDispensingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionOrderDispensingDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionOrderDispensingDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(TransactionOrderDispensingDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(TransactionOrderDispensingDTO.SearchByParameters.TRANSACTION_ORDER_DISPENSING_ID)||
                                  searchParameter.Key.Equals(TransactionOrderDispensingDTO.SearchByParameters.TRANSACTION_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        //else if (searchParameter.Key == TrasactionDeliveryDetailsDTO.SearchByParameters.CHANNEL_NAME)
                        //{
                        //    query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        //}
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
                transactionOrderDispensingDTOList = new List<TransactionOrderDispensingDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    TransactionOrderDispensingDTO transactionOrderDispensingDTO = GetTransactionOrderDispensingDTO(dataRow);
                    transactionOrderDispensingDTOList.Add(transactionOrderDispensingDTO);
                }
            }
            log.LogMethodExit(transactionOrderDispensingDTOList);
            return transactionOrderDispensingDTOList;
        }
    }

}
