/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - TrasactionDeliveryDetails DataHandler
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     01-Jun-2021     Fiona             Created
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
    /// 
    /// </summary>
    public class TransactionDeliveryDetailsDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string passPhrase;
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT tdd.TrasactionDeliveryDetailId,
                                                     tdd.TransactionOrderDispensingId,
                                                     tdd.RiderId,
                                                     CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@passPhrase,tdd.RiderName)) as RiderName,
                                                     CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@passPhrase,tdd.RiderPhoneNumber)) as RiderPhoneNumber,
                                                     tdd.RiderDeliveryStatus,
                                                     tdd.Remarks,
                                                     tdd.ExternalSystemReference,
                                                     tdd.IsActive,
                                                     tdd.CreatedBy,
                                                     tdd.CreationDate,
                                                     tdd.LastUpdatedBy,
                                                     tdd.LastUpdatedDate,
                                                     tdd.Guid,
                                                     tdd.SynchStatus,
                                                     tdd.site_id,
                                                     tdd.MasterEntityId
                                                     FROM TransactionDeliveryDetails AS tdd ";
        private static readonly Dictionary<TransactionDeliveryDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionDeliveryDetailsDTO.SearchByParameters, string>
            {
                {TransactionDeliveryDetailsDTO.SearchByParameters.TRANSACTION_DELIVERY_DETAILS_ID, "tdd.TrasactionDeliveryDetailId"},
                {TransactionDeliveryDetailsDTO.SearchByParameters.IS_ACTIVE, "tdd.IsActive"},
                {TransactionDeliveryDetailsDTO.SearchByParameters.MASTER_ENTITY_ID, "tdd.MasterEntityId"},
                {TransactionDeliveryDetailsDTO.SearchByParameters.SITE_ID, "tdd.site_id"},
                {TransactionDeliveryDetailsDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, "tdd.ExternalSystemReference"},
                {TransactionDeliveryDetailsDTO.SearchByParameters.TRANSACTION_ORDER_DISPENSING_ID, "tdd.TransactionOrderDispensingId"}
            };
        /// <summary>
        /// Default constructor of TrasactionDeliveryDetailsDataHandler class
        /// </summary>
        public TransactionDeliveryDetailsDataHandler(string passPhrase, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.passPhrase = passPhrase;
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DeliveryChannelDTO Record.
        /// </summary>
        /// <param name="trasactionDeliveryDetailsDTO">DeliveryChannelDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(TransactionDeliveryDetailsDTO trasactionDeliveryDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trasactionDeliveryDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrasactionDeliveryDetailsId", trasactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderDispensingId", trasactionDeliveryDetailsDTO.TransctionOrderDispensingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RiderId", trasactionDeliveryDetailsDTO.RiderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RiderName", trasactionDeliveryDetailsDTO.ExternalRiderName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RidePhoneNumber", trasactionDeliveryDetailsDTO.RiderPhoneNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RiderDeliveryStatus", trasactionDeliveryDetailsDTO.RiderDeliveryStatus, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", trasactionDeliveryDetailsDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", trasactionDeliveryDetailsDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", trasactionDeliveryDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", trasactionDeliveryDetailsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passPhrase", passPhrase));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trasactionDeliveryDetailsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public TransactionDeliveryDetailsDTO Insert(TransactionDeliveryDetailsDTO trasactionDeliveryDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trasactionDeliveryDetailsDTO, loginId, siteId);
            string insertQuery = @"insert into TransactionDeliveryDetails 
                                                        (                                                         
                                                       TransactionOrderDispensingId ,
                                                       RiderId,
                                                       RiderName,
                                                       RiderPhoneNumber,
                                                       RiderDeliveryStatus,
                                                       Remarks,
                                                       ExternalSystemReference,
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
                                                       @OrderDispensingId ,
                                                       @RiderId,
                                                       ENCRYPTBYPASSPHRASE(@passPhrase, @RiderName),
                                                       ENCRYPTBYPASSPHRASE(@passPhrase, @RidePhoneNumber),
                                                       @RiderDeliveryStatus,
                                                       @Remarks,
                                                       @ExternalSystemReference,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from TransactionDeliveryDetails where TrasactionDeliveryDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(trasactionDeliveryDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrasactionDeliveryDetailsDTO(trasactionDeliveryDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TransactionDeliveryDetailsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trasactionDeliveryDetailsDTO);
            return trasactionDeliveryDetailsDTO;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="trasactionDeliveryDetailsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public TransactionDeliveryDetailsDTO Update(TransactionDeliveryDetailsDTO trasactionDeliveryDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trasactionDeliveryDetailsDTO, loginId, siteId);
            string updateQuery = @"update TransactionDeliveryDetails 
                                         set 
                                            TransactionOrderDispensingId=@OrderDispensingId,
                                            RiderId=@RiderId,
                                            RiderName=ENCRYPTBYPASSPHRASE(@passPhrase, @RiderName),
                                            RiderPhoneNumber=ENCRYPTBYPASSPHRASE(@passPhrase, @RidePhoneNumber),
                                            RiderDeliveryStatus=@RiderDeliveryStatus,
                                            Remarks=@Remarks,
                                            ExternalSystemReference=@ExternalSystemReference,
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate()
                                               where   TrasactionDeliveryDetailId =  @TrasactionDeliveryDetailsId  
                                        SELECT  * from TransactionDeliveryDetails where TrasactionDeliveryDetailId = @TrasactionDeliveryDetailsId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(trasactionDeliveryDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrasactionDeliveryDetailsDTO(trasactionDeliveryDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TrasactionDeliveryDetailsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trasactionDeliveryDetailsDTO);
            return trasactionDeliveryDetailsDTO;
        }

        private void RefreshTrasactionDeliveryDetailsDTO(TransactionDeliveryDetailsDTO trasactionDeliveryDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(trasactionDeliveryDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                trasactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId = Convert.ToInt32(dt.Rows[0]["TrasactionDeliveryDetailId"]);
                trasactionDeliveryDetailsDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                trasactionDeliveryDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                trasactionDeliveryDetailsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                trasactionDeliveryDetailsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                trasactionDeliveryDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                trasactionDeliveryDetailsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        private TransactionDeliveryDetailsDTO GetTrasactionDeliveryDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionDeliveryDetailsDTO trasactionDeliveryDetailsDTO = new TransactionDeliveryDetailsDTO(Convert.ToInt32(dataRow["TrasactionDeliveryDetailId"]),
                                                    dataRow["TransactionOrderDispensingId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionOrderDispensingId"]),
                                                    dataRow["RiderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RiderId"]),
                                                    dataRow["RiderName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RiderName"]),
                                                    dataRow["RiderPhoneNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RiderPhoneNumber"]),
                                                    dataRow["RiderDeliveryStatus"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RiderDeliveryStatus"]),
                                                    dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                    dataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalSystemReference"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(trasactionDeliveryDetailsDTO);
            return trasactionDeliveryDetailsDTO;
        }

        internal List<TransactionDeliveryDetailsDTO> GetTransactionDeliveryDetailsDTOList(List<int> transctionOrderDispensingIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(transctionOrderDispensingIdList, activeChildRecords);
            List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
            string query = SELECT_QUERY + @" , @transctionOrderDispensingIdList List
                            WHERE TransactionOrderDispensingId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@passPhrase", passPhrase);
            DataTable table = dataAccessHandler.BatchSelect(query, "@transctionOrderDispensingIdList", transctionOrderDispensingIdList, selectParameters, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                transactionDeliveryDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetTrasactionDeliveryDetailsDTO(x)).ToList();
            }
            log.LogMethodExit(transactionDeliveryDetailsDTOList);
            return transactionDeliveryDetailsDTOList;
        }

        internal TransactionDeliveryDetailsDTO GetTrasactionDeliveryDetailsDTO(int id)
        {
            log.LogMethodEntry(id);
            TransactionDeliveryDetailsDTO trasactionDeliveryDetailsDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where tdd.TrasactionDeliveryDetailId = @TrasactionDeliveryDetailsId";
            SqlParameter[] selectParameters = new SqlParameter[2];
            selectParameters[0] = new SqlParameter("@TrasactionDeliveryDetailsId", id);
            selectParameters[1] = new SqlParameter("@passPhrase", passPhrase);
            DataTable deliveryChannelIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectParameters, sqlTransaction);
            if (deliveryChannelIdTable.Rows.Count > 0)
            {
                DataRow dataRow = deliveryChannelIdTable.Rows[0];
                trasactionDeliveryDetailsDTO = GetTrasactionDeliveryDetailsDTO(dataRow);
            }
            log.LogMethodExit(trasactionDeliveryDetailsDTO);
            return trasactionDeliveryDetailsDTO;

        }
        /// <summary>
        /// GetTrasactionDeliveryDetailsDTO
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<TransactionDeliveryDetailsDTO> GetTrasactionDeliveryDetailsDTOList(List<KeyValuePair<TransactionDeliveryDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<TransactionDeliveryDetailsDTO> trasactionDeliveryDetailsDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionDeliveryDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionDeliveryDetailsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionDeliveryDetailsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(TransactionDeliveryDetailsDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(TransactionDeliveryDetailsDTO.SearchByParameters.TRANSACTION_DELIVERY_DETAILS_ID)||
                                  searchParameter.Key.Equals(TransactionDeliveryDetailsDTO.SearchByParameters.TRANSACTION_ORDER_DISPENSING_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionDeliveryDetailsDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
            parameters.Add(dataAccessHandler.GetSQLParameter("@passPhrase", passPhrase));
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                trasactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    TransactionDeliveryDetailsDTO trasactionDeliveryDetailsDTO = GetTrasactionDeliveryDetailsDTO(dataRow);
                    trasactionDeliveryDetailsDTOList.Add(trasactionDeliveryDetailsDTO);
                }
            }
            log.LogMethodExit(trasactionDeliveryDetailsDTOList);
            return trasactionDeliveryDetailsDTOList;
        }
    }
}
