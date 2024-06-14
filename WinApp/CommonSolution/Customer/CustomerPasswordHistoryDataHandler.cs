/********************************************************************************************
* Project Name - Customer
* Description  - Data Handler File for CustomerPasswordHistory
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.80        26-June-2020   Indrajeet Kumar         Created 
********************************************************************************************/
using System;
using System.Data;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomerPasswordHistory Data Handler - Handles insert, update and select of CustomerPasswordHistory objects
    /// </summary>
    public class CustomerPasswordHistoryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CustomerPasswordHistory AS cph ";

        /// <summary>
        /// Dictionary for searching Parameters for the CustomerPasswordHistory object.
        /// </summary>
        private static readonly Dictionary<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string> DBSearchParameters = new Dictionary<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string>
        {
            { CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.CUSTOMER_PASSWORD_HISTORY_ID,"cph.Id"},
            { CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.PROFILE_ID,"cph.ProfileId"},
            { CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.CHANGE_DATE,"cph.ChangeDate"},
            { CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.SITE_ID,"cph.site_id"},
            { CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.MASTER_ENTITY_ID,"cph.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for CustomerPasswordHistoryDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public CustomerPasswordHistoryDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerPasswordHistory Record.
        /// </summary>
        /// <param name="customerPasswordHistoryDTO">customerPasswordHistoryDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user</param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerPasswordHistoryDTO customerPasswordHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerPasswordHistoryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", customerPasswordHistoryDTO.CustomerPasswordHistoryId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileId", customerPasswordHistoryDTO.ProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Password", customerPasswordHistoryDTO.Password));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerPasswordHistoryDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", customerPasswordHistoryDTO.SynchStatus));
            
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to CustomerPasswordHistoryDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of CustomerPasswordHistoryDTO</returns>
        private CustomerPasswordHistoryDTO GetCustomerPasswordHistoryDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerPasswordHistoryDTO customerPasswordHistoryDTO = new CustomerPasswordHistoryDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["ProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProfileId"]),                                               
                                                dataRow["Password"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Password"]),
                                                dataRow["ChangeDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ChangeDate"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),                                               
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                );
            return customerPasswordHistoryDTO;
        }

        /// <summary>
        /// Gets the CustomerPasswordHistory data of passed CustomerPasswordHistory Id
        /// </summary>
        /// <param name="CustomerPasswordHistoryId"></param>
        /// <returns>Returns CustomerPasswordHistoryDTO</returns>
        public CustomerPasswordHistoryDTO GetCustomerPasswordHistoryDTO(int customerPasswordHistoryId)
        {
            log.LogMethodEntry(customerPasswordHistoryId);
            CustomerPasswordHistoryDTO result = null;
            string query = SELECT_QUERY + @" WHERE CustomerPasswordHistory.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", customerPasswordHistoryId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCustomerPasswordHistoryDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerPasswordHistoryDTO">CustomerPasswordHistoryDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable</param>
        private void RefreshCustomerPasswordHistoryDTO(CustomerPasswordHistoryDTO customerPasswordHistoryDTO, DataTable dt)
        {
            log.LogMethodEntry(customerPasswordHistoryDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerPasswordHistoryDTO.CustomerPasswordHistoryId = Convert.ToInt32(dt.Rows[0]["Id"]);
                customerPasswordHistoryDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                customerPasswordHistoryDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                customerPasswordHistoryDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                customerPasswordHistoryDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                customerPasswordHistoryDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                customerPasswordHistoryDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the record to the CustomerPasswordHistory Table. 
        /// </summary>
        /// <param name="customerPasswordHistoryDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated CustomerPasswordHistoryDTO</returns>
        public CustomerPasswordHistoryDTO Insert(CustomerPasswordHistoryDTO customerPasswordHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerPasswordHistoryDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CustomerPasswordHistory]
                            (
                            ProfileId,
                            Password,
                            ChangeDate,
                            site_id,
                            Guid,
                            SynchStatus,                            
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate
                            )
                            VALUES
                            (
                            @ProfileId,
                            @Password,
                            GETDATE(),
                            @site_id,
                            NEWID(),
                            @SynchStatus,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()                           
                            )
                            SELECT * FROM CustomerPasswordHistory WHERE ID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerPasswordHistoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerPasswordHistoryDTO(customerPasswordHistoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerPasswordHistoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerPasswordHistoryDTO);
            return customerPasswordHistoryDTO;
        }

        /// <summary>
        /// Update the record in the CustomerPasswordHistory Table. 
        /// </summary>
        /// <param name="customerPasswordHistoryDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated CustomerPasswordHistoryDTO</returns>
        public CustomerPasswordHistoryDTO Update(CustomerPasswordHistoryDTO customerPasswordHistoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerPasswordHistoryDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CustomerPasswordHistory]
                             SET
                             ProfileId = @ProfileId,
                             Password = @Password,
                             ChangeDate = GETDATE(),
                             site_id = @site_id,
                             SynchStatus = @SynchStatus,
                             MasterEntityId = @MasterEntityId,
                             CreatedBy = @CreatedBy,
                             LastUpdatedBy = @LastUpdatedBy    
                            WHERE Id = @Id 
                            SELECT * FROM CustomerPasswordHistory WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerPasswordHistoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerPasswordHistoryDTO(customerPasswordHistoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating CustomerPasswordHistoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerPasswordHistoryDTO);
            return customerPasswordHistoryDTO;
        }

        public List<CustomerPasswordHistoryDTO> GetCustomerPasswordHistoryDTOList(List<KeyValuePair<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CustomerPasswordHistoryDTO> customerPasswordHistoryDTOList = new List<CustomerPasswordHistoryDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.CUSTOMER_PASSWORD_HISTORY_ID ||
                            searchParameter.Key == CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.PROFILE_ID ||
                            searchParameter.Key == CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.CHANGE_DATE)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.SITE_ID)
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
                selectQuery = selectQuery + query + " order by changeDate desc";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerPasswordHistoryDTO customerPasswordHistoryDTO = GetCustomerPasswordHistoryDTO(dataRow);
                    customerPasswordHistoryDTOList.Add(customerPasswordHistoryDTO);
                }
            }
            log.LogMethodExit(customerPasswordHistoryDTOList);
            return customerPasswordHistoryDTOList;
        }
    }
}
