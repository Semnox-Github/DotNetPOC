/********************************************************************************************
* Project Name - CustomerRelationshipType Data Handler
* Description  - Data handler of the CustomerRelationshipType class
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        06-Feb-2017   Lakshminarayana     Created 
*2.70.2       19-Jul-2019    Girish Kundar   Modified :Structure of data Handler - insert /Update methods
*                                                    Fix for SQL Injection Issue  
*2.130.0     31-Aug-2021   Mushahid Faizan   Modified : Pos UI redesign changes.                                                    
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  CustomerRelationshipType Data Handler - Handles insert, update and select of  CustomerRelationshipType objects
    /// </summary>
    public class CustomerRelationshipTypeDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<CustomerRelationshipTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerRelationshipTypeDTO.SearchByParameters, string>
            {
                {CustomerRelationshipTypeDTO.SearchByParameters.ID, "crt.Id"},
                {CustomerRelationshipTypeDTO.SearchByParameters.NAME, "crt.Name"},
                {CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE,"crt.IsActive"},
                {CustomerRelationshipTypeDTO.SearchByParameters.MASTER_ENTITY_ID,"crt.MasterEntityId"},
                {CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, "crt.site_id"}
            };
         private DataAccessHandler dataAccessHandler;
         private const string SELECT_QUERY = @"SELECT * from CustomerRelationshipType AS crt";
        /// <summary>
        /// Default constructor of CustomerRelationshipTypeDataHandler class
        /// </summary>
        public CustomerRelationshipTypeDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerRelationshipType Record.
        /// </summary>
        /// <param name="customerRelationshipTypeDTO">CustomerRelationshipTypeDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerRelationshipTypeDTO customerRelationshipTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerRelationshipTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", customerRelationshipTypeDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", customerRelationshipTypeDTO.Name, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", customerRelationshipTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerRelationshipTypeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerRelationshipTypeDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomerRelationshipType record to the database
        /// </summary>
        /// <param name="customerRelationshipTypeDTO">CustomerRelationshipTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerRelationshipTypeDTO</returns>
        public CustomerRelationshipTypeDTO InsertCustomerRelationshipType(CustomerRelationshipTypeDTO customerRelationshipTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerRelationshipTypeDTO, loginId, siteId);
            string query = @"INSERT INTO CustomerRelationshipType 
                                        ( 
                                            Name,
                                            Description,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId

                                        ) 
                                VALUES 
                                        (
                                            @Name,
                                            @Description,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM CustomerRelationshipType WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerRelationshipTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerRelationshipTypeDTO(customerRelationshipTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerRelationshipTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerRelationshipTypeDTO);
            return customerRelationshipTypeDTO;
        }

        /// <summary>
        /// Updates the CustomerRelationshipType record
        /// </summary>
        /// <param name="customerRelationshipTypeDTO">CustomerRelationshipTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerRelationshipTypeDTO</returns>
        public CustomerRelationshipTypeDTO UpdateCustomerRelationshipType(CustomerRelationshipTypeDTO customerRelationshipTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerRelationshipTypeDTO, loginId, siteId);
            string query = @"UPDATE CustomerRelationshipType 
                             SET Name=@Name,
                                 Description = @Description,
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate = GETDATE(),
                                 MasterEntityId=@MasterEntityId
                             WHERE Id = @Id 
                            SELECT * FROM CustomerRelationshipType WHERE Id  = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerRelationshipTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerRelationshipTypeDTO(customerRelationshipTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerRelationshipTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerRelationshipTypeDTO);
            return customerRelationshipTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerRelationshipTypeDTO">CustomerRelationshipTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerRelationshipTypeDTO(CustomerRelationshipTypeDTO customerRelationshipTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(customerRelationshipTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerRelationshipTypeDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                //Other fields are readonly
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomerRelationshipTypeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomerRelationshipTypeDTO</returns>
        private CustomerRelationshipTypeDTO GetCustomerRelationshipTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerRelationshipTypeDTO customerRelationshipTypeDTO = new CustomerRelationshipTypeDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                            dataRow["Description"] == DBNull.Value ? string.Empty : dataRow["Description"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(customerRelationshipTypeDTO);
            return customerRelationshipTypeDTO;
        }

        /// <summary>
        /// Gets the CustomerRelationshipType data of passed CustomerRelationshipType Id
        /// </summary>
        /// <param name="customerRelationshipTypeId">integer type parameter</param>
        /// <returns>Returns CustomerRelationshipTypeDTO</returns>
        public CustomerRelationshipTypeDTO GetCustomerRelationshipTypeDTO(int customerRelationshipTypeId)
        {
            log.LogMethodEntry(customerRelationshipTypeId);
            CustomerRelationshipTypeDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE crt.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", customerRelationshipTypeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomerRelationshipTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the CustomerRelationshipTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerRelationshipTypeDTO matching the search criteria</returns>
        public List<CustomerRelationshipTypeDTO> GetCustomerRelationshipTypeDTOList(List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerRelationshipTypeDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomerRelationshipTypeDTO.SearchByParameters.ID 
                            || searchParameter.Key == CustomerRelationshipTypeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CustomerRelationshipTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerRelationshipTypeDTO customerRelationshipTypeDTO = GetCustomerRelationshipTypeDTO(dataRow);
                    list.Add(customerRelationshipTypeDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        internal DateTime? GetCustomerRelationshipTypeLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from CustomerRelationshipType WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
