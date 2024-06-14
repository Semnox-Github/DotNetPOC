/********************************************************************************************
 * Project Name - Customer App User Log                                                                     
 * Description  - DTO for Customer App configuration
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
  *2.120.0    15-03-2021    Prajwal S            Modified : SiteId changes in Insert and Update.  
  *2.130.10   08-Sep-2022   Nitin Pai            Enhanced customer activity user log table
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Customer
{
    class CustomerActivityUserLogDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;

        private static readonly Dictionary<CustomerActivityUserLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerActivityUserLogDTO.SearchByParameters, string>
            {
                {CustomerActivityUserLogDTO.SearchByParameters.ID, "caul.ActivityUserLogId"},
                {CustomerActivityUserLogDTO.SearchByParameters.CUSTOMER_ID, "caul.CustomerId"},
                {CustomerActivityUserLogDTO.SearchByParameters.FROM_DATE, "caul.ActivityTime"},
                {CustomerActivityUserLogDTO.SearchByParameters.TO_DATE, "caul.ActivityTime"},
                {CustomerActivityUserLogDTO.SearchByParameters.SITE_ID, "caul.site_id"},
                {CustomerActivityUserLogDTO.SearchByParameters.MASTER_ENTITY_ID, "caul.MasterEntityId"},
                {CustomerActivityUserLogDTO.SearchByParameters.ISACTIVE, "caul.IsActive"}
            };

        private readonly DataAccessHandler dataAccessHandler;
        List<SqlParameter> parameters = new List<SqlParameter>();
        private static readonly string SELECT_QUERY = @"select * from CustomerActivityUserLog caul";

        /// <summary>
        /// Parameterised constructor for CustomerActivityUserLogDataHandler
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public CustomerActivityUserLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerActivityUserLog Record.
        /// </summary>
        /// <param name="customerActivityUserLogDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(CustomerActivityUserLogDTO customerActivityUserLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerActivityUserLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActivityUserLogId", customerActivityUserLogDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", customerActivityUserLogDTO.CustomerId == null ? DBNull.Value : (object)customerActivityUserLogDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeviceId", string.IsNullOrEmpty(customerActivityUserLogDTO.DeviceId) ? DBNull.Value : (object)customerActivityUserLogDTO.DeviceId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Action", string.IsNullOrEmpty(customerActivityUserLogDTO.Action) ? DBNull.Value : (object)customerActivityUserLogDTO.Action));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Activity", string.IsNullOrEmpty(customerActivityUserLogDTO.Activity) ? DBNull.Value : (object)customerActivityUserLogDTO.Activity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActivityTime", customerActivityUserLogDTO.ActivityTime == null ? DBNull.Value : (object)customerActivityUserLogDTO.ActivityTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerActivityUserLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerActivityUserLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Data", string.IsNullOrEmpty(customerActivityUserLogDTO.Data) ? DBNull.Value : (object)customerActivityUserLogDTO.Data));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Source", string.IsNullOrEmpty(customerActivityUserLogDTO.Source) ? DBNull.Value : (object)customerActivityUserLogDTO.Source));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Category", string.IsNullOrEmpty(customerActivityUserLogDTO.Category) ? DBNull.Value : (object)customerActivityUserLogDTO.Category));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Severity", string.IsNullOrEmpty(customerActivityUserLogDTO.Severity) ? DBNull.Value : (object)customerActivityUserLogDTO.Severity));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// returns CustomerActivityUserLogDTO from the given datarow
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private CustomerActivityUserLogDTO GetCustomerActivityUserLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(dataRow["ActivityUserLogId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ActivityUserLogId"]),
                                                dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                                dataRow["DeviceId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DeviceId"]),
                                                dataRow["Action"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Action"]),
                                                dataRow["Activity"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Activity"]),
                                                dataRow["ActivityTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ActivityTime"]),
                                                dataRow["Data"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Data"]),
                                                dataRow["Source"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Source"]),
                                                dataRow["Category"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Category"]),
                                                dataRow["Severity"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Severity"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                );
            return customerActivityUserLogDTO;
        }

        /// <summary>
        /// Gets the CustomerActivityUserLog data of passed Id 
        /// </summary>
        /// <param name="activityUserLogId"></param>
        /// <returns>Returns WorkShiftDTO</returns>
        public CustomerActivityUserLogDTO GetCustomerActivityUserLogDTO(int activityUserLogId)
        {
            log.LogMethodEntry(activityUserLogId);
            CustomerActivityUserLogDTO result = null;
            string query = SELECT_QUERY + @" WHERE caul.ActivityUserLogId = @ActivityUserLogId";
            SqlParameter parameter = new SqlParameter("@ActivityUserLogId", activityUserLogId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCustomerActivityUserLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerActivityUserLogDTO"></param>
        /// <param name="dt"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        private void RefreshCustomerActivityUserLogDTO(CustomerActivityUserLogDTO customerActivityUserLogDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(customerActivityUserLogDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                customerActivityUserLogDTO.Id = Convert.ToInt32(dt.Rows[0]["ActivityUserLogId"]);
                customerActivityUserLogDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                customerActivityUserLogDTO.CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                customerActivityUserLogDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                customerActivityUserLogDTO.LastUpdatedBy = loginId;
                customerActivityUserLogDTO.CreatedBy = loginId;
                customerActivityUserLogDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the CustomerActivityUserLog Table. 
        /// </summary>
        /// <param name="customerActivityUserLogDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated CustomerActivityUserLogDTO</returns>
        public CustomerActivityUserLogDTO Insert(CustomerActivityUserLogDTO customerActivityUserLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerActivityUserLogDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CustomerActivityUserLog]
                            (
                            CustomerId,
                            DeviceId,
                            Action,
                            Activity,
                            ActivityTime,
                            Data,
                            Source,
                            Category,
                            Severity,
                            Guid,
                            IsActive,
                            site_id,
                            MasterEntityId,
                            CreationDate,
                            CreatedBy,
                            LastUpdatedBy,
                            LastUpdateDate)
                            VALUES
                            (
                            @CustomerId,
                            @DeviceId,
                            @Action,
                            @Activity,
                            @ActivityTime,
                            @Data,
                            @Source,
                            @Category,
                            @Severity,
                            NEWID(),
                            @IsActive,
                            @site_id,
                            @MasterEntityId,
                            GETDATE(),
                            @CreatedBy,
                            @LastUpdatedBy,
                            GETDATE())
                            SELECT * FROM CustomerActivityUserLog WHERE ActivityUserLogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerActivityUserLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerActivityUserLogDTO(customerActivityUserLogDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting CustomerActivityUserLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerActivityUserLogDTO);
            return customerActivityUserLogDTO;
        }

        /// <summary>
        /// Update the record in the CustomerActivityUser Table. 
        /// </summary>
        /// <param name="customerActivityUserLogDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated CustomerActivityUserLogDTO</returns>
        public CustomerActivityUserLogDTO Update(CustomerActivityUserLogDTO customerActivityUserLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerActivityUserLogDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CustomerActivityUserLog]
                             SET
                             CustomerId = @CustomerId,
                             DeviceId = @DeviceId,
                             Action = @Action,
                             Activity = @Activity,
                             ActivityTime = @ActivityTime,
                             Data = @Data,
                             Source = @Source,
                             Category = @Category,
                             Severity = @Severity,
                             IsActive = @IsActive,
                             site_id = @site_id,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = @LastUpdateDate
                             WHERE ActivityUserLogId = @ActivityUserLogId
                            SELECT * FROM CustomerActivityUserLog WHERE ActivityUserLogId = @ActivityUserLogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerActivityUserLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerActivityUserLogDTO(customerActivityUserLogDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting CustomerActivityUserLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerActivityUserLogDTO);
            return customerActivityUserLogDTO;
        }

        /// <summary>
        /// Returns the List of CustomerActivityUserLogDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CustomerActivityUserLogDTO> GetCustomerActivityUserLogDTOList(List<KeyValuePair<CustomerActivityUserLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CustomerActivityUserLogDTO> customerActivityUserLogDTOList = new List<CustomerActivityUserLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomerActivityUserLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerActivityUserLogDTO.SearchByParameters.ID ||
                            searchParameter.Key == CustomerActivityUserLogDTO.SearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == CustomerActivityUserLogDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerActivityUserLogDTO.SearchByParameters.DEVICE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerActivityUserLogDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerActivityUserLogDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerActivityUserLogDTO.SearchByParameters.SITE_ID)
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
                    CustomerActivityUserLogDTO customerActivityUserLogDTO = GetCustomerActivityUserLogDTO(dataRow);
                    customerActivityUserLogDTOList.Add(customerActivityUserLogDTO);
                }
            }
            log.LogMethodExit(customerActivityUserLogDTOList);
            return customerActivityUserLogDTOList;
        }
    }
}
