/********************************************************************************************
 * Project Name - Product
 * Description  - CustomerProfilingDataHandler
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      24-Mar-2022     Girish Kundar              Created : Check in check out changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class CustomerProfilingDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CustomerProfilings AS cp ";
        private static readonly Dictionary<CustomerProfilingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerProfilingDTO.SearchByParameters, string>
            {
                {CustomerProfilingDTO.SearchByParameters.CUSTOMER_PROFILE_GROUP_ID, "cp.CustomerProfilingGroupId"},
                {CustomerProfilingDTO.SearchByParameters.CUSTOMER_PROFILE_ID, "cp.CustomerProfilingId"},
                {CustomerProfilingDTO.SearchByParameters.IS_ACTIVE, "cp.IsActive"},
                {CustomerProfilingDTO.SearchByParameters.MASTER_ENTITY_ID, "cp.MasterEntityId"},
                {CustomerProfilingDTO.SearchByParameters.SITE_ID, "cp.site_id"}
            };

        /// <summary>
        /// Default constructor of CustomerProfilingDataHandler class
        /// </summary>
        public CustomerProfilingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerProfilingDTO Record.
        /// </summary>
        /// <param name="customerProfilingDTO">CustomerProfilingDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(CustomerProfilingDTO customerProfilingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerProfilingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerProfilingId", customerProfilingDTO.CustomerProfilingId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerProfilingGroupId", customerProfilingDTO.CustomerProfilingGroupId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileType", customerProfilingDTO.ProfileType,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Operator", customerProfilingDTO.CompareOperator));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileValue", customerProfilingDTO.ProfileValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerProfilingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerProfilingDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Insert customerProfilingDTO
        /// </summary>
        /// <param name="customerProfilingDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CustomerProfilingDTO Insert(CustomerProfilingDTO customerProfilingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerProfilingDTO, loginId, siteId);
            string insertQuery = @"insert into CustomerProfilings 
                                                        (                                                         
                                                           CustomerProfilingGroupId ,
                                                           ProfileType,
                                                           Operator,
                                                           ProfileValue,
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
                                                       @CustomerProfilingGroupId,
                                                       @ProfileType,
                                                       @Operator,
                                                       @ProfileValue,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          ) SELECT  * from CustomerProfilings where CustomerProfilingId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(customerProfilingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerProfilingGroupsDTO(customerProfilingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CustomerProfilingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerProfilingDTO);
            return customerProfilingDTO;
        }

        /// <summary>
        /// Update customerProfilingDTO
        /// </summary>
        /// <param name="customerProfilingDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CustomerProfilingDTO Update(CustomerProfilingDTO customerProfilingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerProfilingDTO, loginId, siteId);
            string updateQuery = @"update CustomerProfilings 
                                         set 
                                            CustomerProfilingGroupId = @CustomerProfilingGroupId,
                                            ProfileType = @ProfileType,
                                            Operator = @Operator,
                                            ProfileValue = @ProfileValue,
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId 
                                               where   CustomerProfilingId =  @CustomerProfilingId  
                                        SELECT  * from CustomerProfilings where CustomerProfilingId = @CustomerProfilingId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(customerProfilingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerProfilingGroupsDTO(customerProfilingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CustomerProfilingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerProfilingDTO);
            return customerProfilingDTO;
        }

        private void RefreshCustomerProfilingGroupsDTO(CustomerProfilingDTO customerProfilingDTO, DataTable dt)
        {
            log.LogMethodEntry(customerProfilingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerProfilingDTO.CustomerProfilingId = Convert.ToInt32(dt.Rows[0]["CustomerProfilingId"]);
                customerProfilingDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                customerProfilingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerProfilingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerProfilingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerProfilingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerProfilingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private CustomerProfilingDTO GetCustomerProfilingDTO(DataRow dataRow)
        {
            try
            {
                log.LogMethodEntry(dataRow);
                CustomerProfilingDTO customerProfilingDTO = new CustomerProfilingDTO(Convert.ToInt32(dataRow["CustomerProfilingId"]),
                                                        dataRow["CustomerProfilingGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerProfilingGroupId"]),
                                                        dataRow["ProfileType"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProfileType"]),
                                                        dataRow["Operator"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Operator"]),
                                                        dataRow["ProfileValue"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ProfileValue"]),
                                                        dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                        dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                        dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                        dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                        dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                        dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                        dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                        dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                        dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                        );
                log.LogMethodExit(customerProfilingDTO);
                return customerProfilingDTO;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        internal List<CustomerProfilingDTO> GetCustomerProfilingDTOList(List<int> idList, bool activeRecords)
        {
            log.LogMethodEntry(idList, activeRecords);
            List<CustomerProfilingDTO> list = new List<CustomerProfilingDTO>();
            LookupValuesDataHandler valuesDataHandler = new LookupValuesDataHandler(sqlTransaction);
            string query = @"SELECT CustomerProfilings.*
                            FROM CustomerProfilings, @idList List
                            WHERE CustomerProfilingGroupId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = 1 ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@idList", idList, null, sqlTransaction);
            foreach (DataRow usersDataRow in table.Rows)
            {
                CustomerProfilingDTO customerProfilingDTO = GetCustomerProfilingDTO(usersDataRow);
                LookupValuesDTO lookupValues=  valuesDataHandler.GetLookupValues(customerProfilingDTO.ProfileType);
                if(lookupValues != null)
                {
                    customerProfilingDTO.ProfileTypeName = lookupValues.LookupValue;
                }
                list.Add(customerProfilingDTO);
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the CustomerProfilingDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns CustomerProfilingDTO</returns>
        internal CustomerProfilingDTO GetCustomerProfilingDTO(int id)
        {
            log.LogMethodEntry(id);
            CustomerProfilingDTO customerProfilingDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where cp.CustomerProfilingId = @CustomerProfilingId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@CustomerProfilingId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                customerProfilingDTO = GetCustomerProfilingDTO(dataRow);
            }
            log.LogMethodExit(customerProfilingDTO);
            return customerProfilingDTO;

        }

        /// <summary>
        /// GetCashdrawers
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CustomerProfilingDTO> GetCustomerProfilings(List<KeyValuePair<CustomerProfilingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CustomerProfilingDTO> customerProfilingDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomerProfilingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerProfilingDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerProfilingDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(CustomerProfilingDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(CustomerProfilingDTO.SearchByParameters.CUSTOMER_PROFILE_ID) ||
                                  searchParameter.Key.Equals(CustomerProfilingDTO.SearchByParameters.CUSTOMER_PROFILE_GROUP_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                customerProfilingDTOList = new List<CustomerProfilingDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    CustomerProfilingDTO CustomerProfilingDTO = GetCustomerProfilingDTO(dataRow);
                    customerProfilingDTOList.Add(CustomerProfilingDTO);
                }
            }
            log.LogMethodExit(customerProfilingDTOList);
            return customerProfilingDTOList;
        }
    }
}
