/********************************************************************************************
 * Project Name - PaymentModeDisplayGroupsDataHandler                                                                       
 * Description  - PaymentModeDisplayGroupsDataHandler
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.150.1      26-Jan-2023     Guru S A             Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PaymentModeDisplayGroupsDataHandler
    {

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PaymentModeDisplayGroups AS pdg ";
        private static readonly Dictionary<PaymentModeDisplayGroupsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PaymentModeDisplayGroupsDTO.SearchByParameters, string>
            {
                {PaymentModeDisplayGroupsDTO.SearchByParameters.PAYMENT_MODE_DISPLAY_GROUP_ID, "pdg.PaymentModeDisplayGroupId"},
                {PaymentModeDisplayGroupsDTO.SearchByParameters.PAYMENT_MODE_ID, "pdg.PaymentModeId"},
                {PaymentModeDisplayGroupsDTO.SearchByParameters.PRODUCT_DISPLAY_GROUP_ID, "pdg.ProductDisplayGroupId"},
                {PaymentModeDisplayGroupsDTO.SearchByParameters.IS_ACTIVE, "pdg.IsActive"},
                {PaymentModeDisplayGroupsDTO.SearchByParameters.MASTER_ENTITY_ID, "pdg.MasterEntityId"},
                {PaymentModeDisplayGroupsDTO.SearchByParameters.SITE_ID, "pdg.site_id"},
                {PaymentModeDisplayGroupsDTO.SearchByParameters.NOT_EXCLUDED_FOR_POS_MACHINE_ID, "ppe.POSMachineId"},
                {PaymentModeDisplayGroupsDTO.SearchByParameters.NOT_EXCLUDED_FOR_USER_ROLE_ID, "urdge.role_id"},
            };
        /// <summary>
        /// Default constructor of DataHandler class
        /// </summary>
        internal PaymentModeDisplayGroupsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        private List<SqlParameter> BuildSQLParameters(PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeDisplayGroupsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentModeDisplayGroupId", paymentModeDisplayGroupsDTO.PaymentModeDisplayGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentModeId", paymentModeDisplayGroupsDTO.PaymentModeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductDisplayGroupId", paymentModeDisplayGroupsDTO.ProductDisplayGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", paymentModeDisplayGroupsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));    
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", paymentModeDisplayGroupsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        internal PaymentModeDisplayGroupsDTO Insert(PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeDisplayGroupsDTO, loginId, siteId);
            string insertQuery = @"insert into PaymentModeDisplayGroups 
                                                     (                                                         
	                                                    PaymentModeId, 
                                                        ProductDisplayGroupId,  
                                                        IsActive ,
                                                        CreatedBy ,
                                                        CreationDate ,
                                                        LastUpdatedBy ,
                                                        LastUpdatedDate ,
                                                        Guid ,
                                                        site_id   ,
                                                        MasterEntityId ) 
                                                values (                                                        
                                                       @PaymentModeId,
                                                       @ProductDisplayGroupId, 
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          ) SELECT  * from PaymentModeDisplayGroups where PaymentModeDisplayGroupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(paymentModeDisplayGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPaymentModeDisplayGroupsDTO(paymentModeDisplayGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting EnabledAttibutesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(paymentModeDisplayGroupsDTO);
            return paymentModeDisplayGroupsDTO;
        }

        internal PaymentModeDisplayGroupsDTO Update(PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeDisplayGroupsDTO, loginId, siteId);
            string updateQuery = @"update PaymentModeDisplayGroups 
                                         set 
	                                        PaymentModeId = @PaymentModeId, 
                                            ProductDisplayGroupId = @ProductDisplayGroupId,    
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId 
                                         where PaymentModeDisplayGroupId =  @PaymentModeDisplayGroupId  
                                        SELECT  * from PaymentModeDisplayGroups where PaymentModeDisplayGroupId = @PaymentModeDisplayGroupId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(paymentModeDisplayGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPaymentModeDisplayGroupsDTO(paymentModeDisplayGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating EnabledAttibutesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(paymentModeDisplayGroupsDTO);
            return paymentModeDisplayGroupsDTO;
        }

        private void RefreshPaymentModeDisplayGroupsDTO(PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO, DataTable dt)
        {
            log.LogMethodEntry(paymentModeDisplayGroupsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                paymentModeDisplayGroupsDTO.PaymentModeDisplayGroupId = Convert.ToInt32(dt.Rows[0]["PaymentModeDisplayGroupId"]);
                paymentModeDisplayGroupsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                paymentModeDisplayGroupsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                paymentModeDisplayGroupsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                paymentModeDisplayGroupsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                paymentModeDisplayGroupsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                paymentModeDisplayGroupsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private PaymentModeDisplayGroupsDTO GetPaymentModeDisplayGroupsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO = new PaymentModeDisplayGroupsDTO(Convert.ToInt32(dataRow["PaymentModeDisplayGroupId"]),
                                                    dataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentModeId"]),
                                                    dataRow["ProductDisplayGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductDisplayGroupId"]), 
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                    );
            log.LogMethodExit(paymentModeDisplayGroupsDTO);
            return paymentModeDisplayGroupsDTO;
        }

        internal DateTime? GetPaymentModeDisplayGroupsLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastUpdatedDate) LastUpdatedDate from PaymentModeDisplayGroups WHERE (site_id = @siteId or @siteId = -1)";
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

        internal PaymentModeDisplayGroupsDTO GetPaymentModeDisplayGroupsDTO(int id)
        {
            log.LogMethodEntry(id);
            PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where pdg.PaymentModeDisplayGroupId = @PaymentModeDisplayGroupId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@PaymentModeDisplayGroupId", id);
            DataTable table = dataAccessHandler.executeSelectQuery(selectUserQuery, selectParameters, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                DataRow dataRow = table.Rows[0];
                paymentModeDisplayGroupsDTO = GetPaymentModeDisplayGroupsDTO(dataRow);
            }
            log.LogMethodExit(paymentModeDisplayGroupsDTO);
            return paymentModeDisplayGroupsDTO;
        }
        internal List<PaymentModeDisplayGroupsDTO> GetPaymentModeDisplayGroupsDTOList(List<int> paymentModeDisplayGroupsIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(paymentModeDisplayGroupsIdList, activeChildRecords);
            List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList = new List<PaymentModeDisplayGroupsDTO>();
            string query = SELECT_QUERY + @" , @PaymentModeDisplayGroupsIdList List
                            WHERE PaymentModeDisplayGroupId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@PaymentModeDisplayGroupsIdList", paymentModeDisplayGroupsIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                paymentModeDisplayGroupsDTOList = table.Rows.Cast<DataRow>().Select(x => GetPaymentModeDisplayGroupsDTO(x)).ToList();
            }
            log.LogMethodExit(paymentModeDisplayGroupsDTOList);
            return paymentModeDisplayGroupsDTOList;
        }

        internal List<PaymentModeDisplayGroupsDTO> GetPaymentModeDisplayGroupsDTOListByPaymentModeIdList(List<int> paymentModeIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(paymentModeIdList, activeChildRecords);
            List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList = new List<PaymentModeDisplayGroupsDTO>();
            string query = SELECT_QUERY + @" , @PaymentModeIdList List
                            WHERE PaymentModeId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@PaymentModeIdList", paymentModeIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                paymentModeDisplayGroupsDTOList = table.Rows.Cast<DataRow>().Select(x => GetPaymentModeDisplayGroupsDTO(x)).ToList();
            }
            log.LogMethodExit(paymentModeDisplayGroupsDTOList);
            return paymentModeDisplayGroupsDTOList;
        }
        internal List<PaymentModeDisplayGroupsDTO> GetPaymentModeDisplayGroupsDTOList(List<KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PaymentModeDisplayGroupsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PaymentModeDisplayGroupsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDisplayGroupsDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                            searchParameter.Key.Equals(PaymentModeDisplayGroupsDTO.SearchByParameters.PAYMENT_MODE_DISPLAY_GROUP_ID)||
                            searchParameter.Key.Equals(PaymentModeDisplayGroupsDTO.SearchByParameters.PAYMENT_MODE_ID)||
                            searchParameter.Key.Equals(PaymentModeDisplayGroupsDTO.SearchByParameters.PRODUCT_DISPLAY_GROUP_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDisplayGroupsDTO.SearchByParameters.NOT_EXCLUDED_FOR_POS_MACHINE_ID))
                        {
                            query.Append(joiner + @" NOT EXISTS IN (SELECT 1
							                                          FROM POSProductExclusions ppe
							                                         WHERE " + DBSearchParameters[searchParameter.Key]  + " = "+
                                                                             dataAccessHandler.GetParameterName(searchParameter.Key) 
                                                                   + " AND ppe.ProductDisplayGroupFormatId = pdg.ProductDisplayGroupId " +
                                                                     " AND ISNULL(ppe.IsActive,'Y') == 'Y' )) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDisplayGroupsDTO.SearchByParameters.NOT_EXCLUDED_FOR_USER_ROLE_ID))
                        {
                            query.Append(joiner + @" NOT EXISTS IN (SELECT 1
							                                          FROM UserRoleDisplayGroupExclusions urdge
							                                         WHERE " + DBSearchParameters[searchParameter.Key] + " = " +
                                                                             dataAccessHandler.GetParameterName(searchParameter.Key)
                                                                   + " AND urdge.ProductDisplayGroupId = pdg.ProductDisplayGroupId " +
                                                                     " AND ISNULL(urdge.IsActive,1) == 1 )) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                paymentModeDisplayGroupsDTOList = new List<PaymentModeDisplayGroupsDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO = GetPaymentModeDisplayGroupsDTO(dataRow);
                    paymentModeDisplayGroupsDTOList.Add(paymentModeDisplayGroupsDTO);
                }
            }
            log.LogMethodExit(paymentModeDisplayGroupsDTOList);
            return paymentModeDisplayGroupsDTOList;
        } 
    }
}
