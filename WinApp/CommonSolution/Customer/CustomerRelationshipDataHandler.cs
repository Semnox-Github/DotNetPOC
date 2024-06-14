/********************************************************************************************
 * Project Name - CustomerRelationship Data Handler
 * Description  - Data handler of the CustomerRelationship class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017    Lakshminarayana     Created 
 *2.70.2      19-Jul-2019    Girish Kundar       Modified :Structure of data Handler - insert /Update methods
 *                                                    Fix for SQL Injection Issue  
 *2.140.0     14-Sep-2021    Guru S A            Waiver mapping UI enhancements
 *2.130.7     23-Apr-2022    Nitin Pai           Get linked cards and child's cards for a customer in website
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  CustomerRelationship Data Handler - Handles insert, update and select of  CustomerRelationship objects
    /// </summary>
    public class CustomerRelationshipDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly string CUSTOMER_RELATIONSHIP_SELECT_QUERY = @"SELECT CustomerRelationship.*, 
                                                                              ISNULL(p1.FirstName,'') + ' ' + ISNULL(p1.LastName,'') AS CustomerName,
                                                                              ISNULL(p2.FirstName,'') + ' ' + ISNULL(p2.LastName,'') AS RelatedCustomerName  
                                                                              FROM CustomerRelationship, Customers c1, Customers c2, Profile p1, Profile p2
                                                                              WHERE c1.customer_id =  CustomerRelationship.CustomerId 
                                                                              AND c2.customer_id = CustomerRelationship.RelatedCustomerId
                                                                              AND p1.Id = c1.ProfileId
                                                                              AND p2.Id = c2.ProfileId";
        private static readonly Dictionary<CustomerRelationshipDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerRelationshipDTO.SearchByParameters, string>
            {
                {CustomerRelationshipDTO.SearchByParameters.ID, "CustomerRelationship.Id"},
                {CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, "CustomerRelationship.CustomerId"},
                {CustomerRelationshipDTO.SearchByParameters.RELATED_CUSTOMER_ID, "CustomerRelationship.RelatedCustomerId"},
                {CustomerRelationshipDTO.SearchByParameters.CUSTOMER_RELATIONSHIP_TYPE_ID, "CustomerRelationship.CustomerRelationshipTypeId"},
                {CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID_LIST, "CustomerRelationship.CustomerId"},
                {CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE,"CustomerRelationship.IsActive"},
                {CustomerRelationshipDTO.SearchByParameters.SITE_ID, "CustomerRelationship.site_id"},
                {CustomerRelationshipDTO.SearchByParameters.EFFECTIVE_DATE, "CustomerRelationship.EffectiveDate"},
                {CustomerRelationshipDTO.SearchByParameters.EXPIRY_DATE, "CustomerRelationship.ExpiryDate"},
                {CustomerRelationshipDTO.SearchByParameters.CUSTOMER_RELATIONSHIP_TYPE_ID_LIST, "CustomerRelationship.CustomerRelationshipTypeId"},
            };
         private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of CustomerRelationshipDataHandler class
        /// </summary>
        public CustomerRelationshipDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerRelationship Record.
        /// </summary>
        /// <param name="customerRelationshipDTO">CustomerRelationshipDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerRelationshipDTO customerRelationshipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerRelationshipDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", customerRelationshipDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", customerRelationshipDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RelatedCustomerId", customerRelationshipDTO.RelatedCustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerRelationshipTypeId", customerRelationshipDTO.CustomerRelationshipTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveDate", customerRelationshipDTO.EffectiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", customerRelationshipDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerRelationshipDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerRelationshipDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomerRelationship record to the database
        /// </summary>
        /// <param name="customerRelationshipDTO">CustomerRelationshipDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerRelationshipDTO</returns>
        public CustomerRelationshipDTO InsertCustomerRelationship(CustomerRelationshipDTO customerRelationshipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerRelationshipDTO, loginId, siteId);
            string query = @"INSERT INTO CustomerRelationship 
                                        ( 
                                            CustomerId,
                                            RelatedCustomerId,
                                            CustomerRelationshipTypeId,
                                            EffectiveDate,
                                            ExpiryDate,
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
                                            @CustomerId,
                                            @RelatedCustomerId,
                                            @CustomerRelationshipTypeId,
                                            @EffectiveDate,
                                            @ExpiryDate,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM CustomerRelationship WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerRelationshipDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerRelationshipDTO(customerRelationshipDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerRelationshipDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerRelationshipDTO);
            return customerRelationshipDTO;
        }

        /// <summary>
        /// Updates the CustomerRelationship record
        /// </summary>
        /// <param name="customerRelationshipDTO">CustomerRelationshipDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>ReturnsCustomerRelationshipDTO</returns>
        public CustomerRelationshipDTO UpdateCustomerRelationship(CustomerRelationshipDTO customerRelationshipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerRelationshipDTO, loginId, siteId);
            string query = @"UPDATE CustomerRelationship 
                             SET CustomerId=@CustomerId,
                                 RelatedCustomerId=@RelatedCustomerId,
                                 CustomerRelationshipTypeId=@CustomerRelationshipTypeId,
                                 EffectiveDate=@EffectiveDate,
                                 ExpiryDate=@ExpiryDate,
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId
                             WHERE Id = @Id 
                           SELECT * FROM CustomerRelationship WHERE Id  = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerRelationshipDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerRelationshipDTO(customerRelationshipDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerRelationshipDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerRelationshipDTO);
            return customerRelationshipDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerRelationshipDTO">CustomerRelationshipDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerRelationshipDTO(CustomerRelationshipDTO customerRelationshipDTO, DataTable dt)
        {
            log.LogMethodEntry(customerRelationshipDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerRelationshipDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                //Other fields are readonly
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerRelationshipDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomerRelationshipDTO</returns>
        private CustomerRelationshipDTO GetCustomerRelationshipDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerRelationshipDTO customerRelationshipDTO = new CustomerRelationshipDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["RelatedCustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RelatedCustomerId"]),
                                            dataRow["CustomerRelationshipTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerRelationshipTypeId"]),
                                            dataRow["CustomerName"] == DBNull.Value ? string.Empty : dataRow["CustomerName"].ToString(),
                                            dataRow["RelatedCustomerName"] == DBNull.Value ? string.Empty : dataRow["RelatedCustomerName"].ToString(),
                                            dataRow["EffectiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
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
            log.LogMethodExit(customerRelationshipDTO);
            return customerRelationshipDTO;
        }

        /// <summary>
        /// Gets the CustomerRelationship data of passed CustomerRelationship Id
        /// </summary>
        /// <param name="customerRelationshipId">integer type parameter</param>
        /// <returns>Returns CustomerRelationshipDTO</returns>
        public CustomerRelationshipDTO GetCustomerRelationshipDTO(int customerRelationshipId)
        {
            log.LogMethodEntry(customerRelationshipId);
            CustomerRelationshipDTO returnValue = null;
            string query = CUSTOMER_RELATIONSHIP_SELECT_QUERY
                            + " and CustomerRelationship.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", customerRelationshipId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomerRelationshipDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the CustomerRelationshipDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerRelationshipDTO matching the search criteria</returns>
        public List<CustomerRelationshipDTO> GetCustomerRelationshipDTOList(List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerRelationshipDTO> list = null;
            string selectQuery = CUSTOMER_RELATIONSHIP_SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = " and ";
                StringBuilder query = new StringBuilder("");
                foreach (KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.ID || 
                            searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.CUSTOMER_RELATIONSHIP_TYPE_ID ||
                            searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID 
                            || searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.RELATED_CUSTOMER_ID)
                        {
                            query.Append(joiner + " ( CustomerRelationship.CustomerId  =" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or CustomerRelationship.RelatedCustomerId = " + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            // query.Append(joiner + "(CustomerRelationship.CustomerId = " + searchParameter.Value + " OR CustomerRelationship.RelatedCustomerId = " + searchParameter.Value + " ) ");
                        }
                        //else if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.RELATED_CUSTOMER_ID)
                        //{
                        //    query.Append(joiner + "( CustomerRelationship.CustomerId  =" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or CustomerRelationship.RelatedCustomerId = " + dataAccessHandler.GetParameterName(searchParameter.Key) +")");
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        //    // query.Append(joiner + "(CustomerRelationship.CustomerId = " + searchParameter.Value + " OR CustomerRelationship.RelatedCustomerId = " + searchParameter.Value +" ) ");
                        //}
                        else if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID_LIST)
                        {
                            query.Append(joiner + " CustomerRelationship.CustomerId"  + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") or " + " CustomerRelationship.RelatedCustomerId" + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")" );
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            //query.Append(joiner + "(CustomerRelationship.CustomerId IN(" + searchParameter.Value + ") OR CustomerRelationship.RelatedCustomerId IN(" + searchParameter.Value + " )) ");
                        }
                        else if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.CUSTOMER_RELATIONSHIP_TYPE_ID_LIST)
                        {
                            query.Append(joiner + " CustomerRelationship.CustomerRelationshipTypeId" + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.EFFECTIVE_DATE)
                        {
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " is null or " +   DBSearchParameters[searchParameter.Key] + " <= '" + (searchParameter.Value) + "')");
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "  Is null  or " + DBSearchParameters[searchParameter.Key] + " <= " + dataAccessHandler.GetParameterName(searchParameter.Key) +")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerRelationshipDTO.SearchByParameters.EXPIRY_DATE)
                        {
                            // query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " is null or " +   DBSearchParameters[searchParameter.Key] + " >= '" + (searchParameter.Value) + "')");
                            query.Append(joiner +  "(" + DBSearchParameters[searchParameter.Key] + "  Is null or " + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key) +")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
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
                list = new List<CustomerRelationshipDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerRelationshipDTO customerRelationshipDTO = GetCustomerRelationshipDTO(dataRow);
                    list.Add(customerRelationshipDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        internal List<CustomerRelationshipDTO> GetCustomerRelationshipDTOList(List<int> customerIdList, bool activeRecords)
        {
            log.LogMethodEntry(customerIdList, activeRecords);
            List<CustomerRelationshipDTO> list = null;
            string selectQuery = "SELECT crfinal.* from ( " + CUSTOMER_RELATIONSHIP_SELECT_QUERY + " ) as crfinal " +
                                 @"inner join @CustomerIdList List 
                                    on (crfinal.CustomerId = List.Id OR crfinal.RelatedCustomerId = List.Id) ";
            if (activeRecords)
            {
                selectQuery = selectQuery + " where ISNULL(crfinal.IsActive,'1') = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(selectQuery, "@CustomerIdList", customerIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomerRelationshipDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;  
        }
    }
}
