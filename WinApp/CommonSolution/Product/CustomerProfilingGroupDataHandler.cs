/********************************************************************************************
 * Project Name - Product
 * Description  - CustomerProfilingGroupDataHandler
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
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class CustomerProfilingGroupDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CustomerProfilingGroups AS cpg ";
        private static readonly Dictionary<CustomerProfilingGroupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerProfilingGroupDTO.SearchByParameters, string>
            {
                {CustomerProfilingGroupDTO.SearchByParameters.CUSTOMER_PROFILE_GROUP_ID, "cpg.CustomerProfilingGroupId"},
                {CustomerProfilingGroupDTO.SearchByParameters.GROUP_NAME, "cpg.GroupName"},
                {CustomerProfilingGroupDTO.SearchByParameters.IS_ACTIVE, "cpg.IsActive"},
                {CustomerProfilingGroupDTO.SearchByParameters.MASTER_ENTITY_ID, "cpg.MasterEntityId"},
                {CustomerProfilingGroupDTO.SearchByParameters.SITE_ID, "cpg.site_id"}
            };

        /// <summary>
        /// Default constructor of CustomerProfilingGroupDataHandler class
        /// </summary>
        public CustomerProfilingGroupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerProfilingGroupDTO Record.
        /// </summary>
        /// <param name="customerProfilingGroupDTO">CustomerProfilingGroupDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(CustomerProfilingGroupDTO customerProfilingGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerProfilingGroupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerProfilingGroupId", customerProfilingGroupDTO.CustomerProfilingGroupId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GroupName", customerProfilingGroupDTO.GroupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerProfilingGroupDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerProfilingGroupDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Insert the CustomerProfilingGroupDTO
        /// </summary>
        /// <param name="customerProfilingGroupDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CustomerProfilingGroupDTO Insert(CustomerProfilingGroupDTO customerProfilingGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerProfilingGroupDTO, loginId, siteId);
            string insertQuery = @"insert into CustomerProfilingGroups 
                                                        (                                                         
                                                           GroupName,
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
                                                       @GroupName,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          ) SELECT  * from CustomerProfilingGroups where CustomerProfilingGroupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(customerProfilingGroupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerProfilingGroupsDTO(customerProfilingGroupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CustomerProfilingGroupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerProfilingGroupDTO);
            return customerProfilingGroupDTO;
        }

        /// <summary>
        /// Update CustomerProfilingGroupDTO
        /// </summary>
        /// <param name="customerProfilingGroupDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CustomerProfilingGroupDTO Update(CustomerProfilingGroupDTO customerProfilingGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerProfilingGroupDTO, loginId, siteId);
            string updateQuery = @"update CustomerProfilingGroups 
                                         set 
                                            GroupName = @GroupName,
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId 
                                               where   CustomerProfilingGroupId =  @CustomerProfilingGroupId  
                                        SELECT  * from CustomerProfilingGroups where CustomerProfilingGroupId = @CustomerProfilingGroupId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(customerProfilingGroupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerProfilingGroupsDTO(customerProfilingGroupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CustomerProfilingGroupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerProfilingGroupDTO);
            return customerProfilingGroupDTO;
        }
        
        private void RefreshCustomerProfilingGroupsDTO(CustomerProfilingGroupDTO customerProfilingGroupDTO, DataTable dt)
        {
            log.LogMethodEntry(customerProfilingGroupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerProfilingGroupDTO.CustomerProfilingGroupId = Convert.ToInt32(dt.Rows[0]["CustomerProfilingGroupId"]);
                customerProfilingGroupDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                customerProfilingGroupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerProfilingGroupDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerProfilingGroupDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerProfilingGroupDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerProfilingGroupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private CustomerProfilingGroupDTO GetCustomerProfilingGroupDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerProfilingGroupDTO customerProfilingGroupDTO = new CustomerProfilingGroupDTO(Convert.ToInt32(dataRow["CustomerProfilingGroupId"]),
                                                    dataRow["GroupName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GroupName"]),
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
            log.LogMethodExit(customerProfilingGroupDTO);
            return customerProfilingGroupDTO;
        }

        /// <summary>
        /// Gets the CustomerProfilingGroupDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns CustomerProfilingGroupDTO</returns>
        internal CustomerProfilingGroupDTO GetCustomerProfilingGroupDTO(int id)
        {
            log.LogMethodEntry(id);
            CustomerProfilingGroupDTO customerProfilingGroupDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where cpg.CustomerProfilingGroupId = @CustomerProfilingGroupId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@CustomerProfilingGroupId", id);
            DataTable table = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                DataRow dataRow = table.Rows[0];
                customerProfilingGroupDTO = GetCustomerProfilingGroupDTO(dataRow);
            }
            log.LogMethodExit(customerProfilingGroupDTO);
            return customerProfilingGroupDTO;

        }

        /// <summary>
        /// GetCashdrawers
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CustomerProfilingGroupDTO> GetCustomerProfilingGroups(List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerProfilingGroupDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerProfilingGroupDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(CustomerProfilingGroupDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(CustomerProfilingGroupDTO.SearchByParameters.CUSTOMER_PROFILE_GROUP_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerProfilingGroupDTO.SearchByParameters.GROUP_NAME)
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
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                customerProfilingGroupDTOList = new List<CustomerProfilingGroupDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    CustomerProfilingGroupDTO CustomerProfilingGroupDTO = GetCustomerProfilingGroupDTO(dataRow);
                    customerProfilingGroupDTOList.Add(CustomerProfilingGroupDTO);
                }
            }
            log.LogMethodExit(customerProfilingGroupDTOList);
            return customerProfilingGroupDTOList;
        }

        internal List<CustomerProfilingGroupDTO> GetCustomerProfilingGroupForProducts(List<int> productIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productIdList);
            List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList = new List<CustomerProfilingGroupDTO>();
            string query = @"SELECT *
                            FROM CustomerProfilingGroups, @productIdList List
                            WHERE Productd = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 1 or IsActive is null) ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@productIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                customerProfilingGroupDTOList = table.Rows.Cast<DataRow>().Select(x => GetCustomerProfilingGroupDTO(x)).ToList();
            }
            log.LogMethodExit(customerProfilingGroupDTOList);
            return customerProfilingGroupDTOList;
        }
    }
}
