/********************************************************************************************
 * Project Name - Cust Feedback SurveyPOSMapping Data  Handler
 * Description  - Data handler of the Cust Feedback Survey POS Mapping class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2       19-Jul-2019    Girish Kundar   Modified :Structure of data Handler - insert /Update methods
 *                                                    Fix for SQL Injection Issue 
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query                                                    
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
    /// Customer Feedback SurveyPOSMapping Data  Handler - Handles insert, update and select of Cust Feedback Survey objects
    /// </summary>
    public class CustomerFeedbackSurveyPOSMappingDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>
            {
                {CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_POS_MAPPING_ID, "cspm.CustFbSurveyPOSMappingId"},
                {CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.POS_MACHINE_ID, "cspm.POSMachineId"},
                {CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID, "cspm.CustFbSurveyId"},
                {CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID_LIST, "cspm.CustFbSurveyId"},
                {CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.IS_ACTIVE, "cspm.IsActive"},
                {CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.MASTER_ENTITY_ID, "cspm.MasterEntityId"},
                {CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.SITE_ID, "cspm.site_id"}
            };
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustFeedbackSurveyPOSMapping AS cspm";
        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyPOSMappingDataHandler class
        /// </summary>
        public CustomerFeedbackSurveyPOSMappingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackSurveyPOSMapping Record.
        /// </summary>
        /// <param name="customerFeedbackSurveyPOSMappingDTO">CustomerFeedbackSurveyPOSMappingDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackSurveyPOSMappingDTO customerFeedbackSurveyPOSMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackSurveyPOSMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyPOSMappingId", customerFeedbackSurveyPOSMappingDTO.CustFbSurveyPOSMappingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pOSMachineId", customerFeedbackSurveyPOSMappingDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyId", customerFeedbackSurveyPOSMappingDTO.CustFbSurveyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerFeedbackSurveyPOSMappingDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", customerFeedbackSurveyPOSMappingDTO.SiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackSurveyPOSMappingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Customer Feedback Survey POS Mapping record to the database
        /// </summary>
        /// <param name="custFeedbackSurveyPOSMapping">CustomerFeedbackSurveyPOSMappingDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackSurveyPOSMappingDTO</returns>
        public CustomerFeedbackSurveyPOSMappingDTO InsertCustomerFeedbackSurveyPOSMapping(CustomerFeedbackSurveyPOSMappingDTO custFeedbackSurveyPOSMapping, string loginId, int siteId)
        {
            log.LogMethodEntry(custFeedbackSurveyPOSMapping, loginId, siteId);
            string query = @"insert into CustFeedbackSurveyPOSMapping 
                                                        ( 
                                                        POSMachineId,
                                                        CustFbSurveyId,                                                        
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        GUID,
                                                        Site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                         @pOSMachineId,
                                                         @custFbSurveyId,                                                         
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                        ) SELECT * FROM CustFeedbackSurveyPOSMapping WHERE CustFbSurveyPOSMappingId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackSurveyPOSMapping, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackSurveyPOSMappingDTO(custFeedbackSurveyPOSMapping, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting custFeedbackSurveyPOSMapping", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackSurveyPOSMapping);
            return custFeedbackSurveyPOSMapping;
        }

        /// <summary>
        /// Updates the Customer Feedback Survey record
        /// </summary>
        /// <param name="custFeedbackSurveyPOSMapping">CustomerFeedbackSurveyPOSMappingDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackSurveyPOSMappingDTO</returns>
        public CustomerFeedbackSurveyPOSMappingDTO UpdateCustomerFeedbackSurveyPOSMapping(CustomerFeedbackSurveyPOSMappingDTO custFeedbackSurveyPOSMapping, string loginId, int siteId)
        {
            log.LogMethodEntry(custFeedbackSurveyPOSMapping, loginId, siteId);
            string query = @"update CustFeedbackSurveyPOSMapping 
                                         set POSMachineId =@pOSMachineId,
                                             CustFbSurveyId=@custFbSurveyId,                                             
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --Site_id=@siteid,
                                             MasterEntityId = @masterEntityId    
                               where CustFbSurveyPOSMappingId = @custFbSurveyPOSMappingId
                               SELECT * FROM CustFeedbackSurveyPOSMapping WHERE CustFbSurveyPOSMappingId  = @custFbSurveyPOSMappingId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackSurveyPOSMapping, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackSurveyPOSMappingDTO(custFeedbackSurveyPOSMapping, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating custFeedbackSurveyPOSMapping", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackSurveyPOSMapping);
            return custFeedbackSurveyPOSMapping;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerMembershipProgressionDTO">CustomerFeedbackSurveyPOSMappingDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustFeedbackSurveyPOSMappingDTO(CustomerFeedbackSurveyPOSMappingDTO customerFeedbackSurveyPOSMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackSurveyPOSMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackSurveyPOSMappingDTO.CustFbSurveyPOSMappingId = Convert.ToInt32(dt.Rows[0]["CustFbSurveyPOSMappingId"]);
                customerFeedbackSurveyPOSMappingDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackSurveyPOSMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackSurveyPOSMappingDTO.Guid = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
                customerFeedbackSurveyPOSMappingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackSurveyPOSMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackSurveyPOSMappingDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerFeedbackSurveyPOSMappingDTO class type
        /// </summary>
        /// <param name="custFeedbackSurveyPOSMappingDataRow">CustomerFeedbackSurveyPOSMapping DataRow</param>
        /// <returns>Returns CustomerFeedbackSurveyPOSMapping</returns>
        private CustomerFeedbackSurveyPOSMappingDTO GetCustomerFeedbackSurveyPOSMappingDTO(DataRow custFeedbackSurveyPOSMappingDataRow)
        {
            log.LogMethodEntry(custFeedbackSurveyPOSMappingDataRow);
            CustomerFeedbackSurveyPOSMappingDTO custFeedbackSurveyPOSMappingDataObject = new CustomerFeedbackSurveyPOSMappingDTO(Convert.ToInt32(custFeedbackSurveyPOSMappingDataRow["CustFbSurveyPOSMappingId"]),
                                            custFeedbackSurveyPOSMappingDataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyPOSMappingDataRow["POSMachineId"]),
                                            custFeedbackSurveyPOSMappingDataRow["CustFbSurveyId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyPOSMappingDataRow["CustFbSurveyId"]),
                                            custFeedbackSurveyPOSMappingDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyPOSMappingDataRow["IsActive"]),
                                            custFeedbackSurveyPOSMappingDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyPOSMappingDataRow["CreatedBy"]),
                                            custFeedbackSurveyPOSMappingDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyPOSMappingDataRow["CreationDate"]),
                                            custFeedbackSurveyPOSMappingDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyPOSMappingDataRow["LastUpdatedBy"]),
                                            custFeedbackSurveyPOSMappingDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyPOSMappingDataRow["LastupdatedDate"]),
                                            custFeedbackSurveyPOSMappingDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyPOSMappingDataRow["site_id"]),
                                            custFeedbackSurveyPOSMappingDataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyPOSMappingDataRow["GUID"]),
                                            custFeedbackSurveyPOSMappingDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyPOSMappingDataRow["SynchStatus"]),
                                            custFeedbackSurveyPOSMappingDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyPOSMappingDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(custFeedbackSurveyPOSMappingDataObject);
            return custFeedbackSurveyPOSMappingDataObject;
        }

        /// <summary>
        /// Gets the Customer Feedback Survey data of passed Cust Fb QuestionId
        /// </summary>
        /// <param name="CustFbSurveyPOSMappingId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackSurveyPOSMappingDTO</returns>
        public CustomerFeedbackSurveyPOSMappingDTO GetCustomerFeedbackSurveyPOSMapping(int CustFbSurveyPOSMappingId)
        {
            log.LogMethodEntry(CustFbSurveyPOSMappingId);
            CustomerFeedbackSurveyPOSMappingDTO custFeedbackSurveyPOSMappingDataObject = null;
            string selectCustomerFeedbackSurveyPOSMappingQuery = SELECT_QUERY + "    WHERE cspm.CustFbSurveyPOSMappingId = @custFbSurveyPOSMappingId";
            SqlParameter[] selectCustomerFeedbackSurveyPOSMappingParameters = new SqlParameter[1];
            selectCustomerFeedbackSurveyPOSMappingParameters[0] = new SqlParameter("@CustFbSurveyPOSMappingId", CustFbSurveyPOSMappingId);
            DataTable custFeedbackSurveyPOSMapping = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyPOSMappingQuery, selectCustomerFeedbackSurveyPOSMappingParameters, sqlTransaction);
            if (custFeedbackSurveyPOSMapping.Rows.Count > 0)
            {
                DataRow CustomerFeedbackSurveyPOSMappingRow = custFeedbackSurveyPOSMapping.Rows[0];
                custFeedbackSurveyPOSMappingDataObject = GetCustomerFeedbackSurveyPOSMappingDTO(CustomerFeedbackSurveyPOSMappingRow);
            }
            log.LogMethodExit(custFeedbackSurveyPOSMappingDataObject);
            return custFeedbackSurveyPOSMappingDataObject;

        }

        /// <summary>
        /// Gets the CustomerFeedbackSurveyPOSMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackSurveyPOSMappingDTO matching the search criteria</returns>
        public List<CustomerFeedbackSurveyPOSMappingDTO> GetCustomerFeedbackSurveyPOSMappingList(List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCustomerFeedbackSurveyPOSMappingQuery = SELECT_QUERY;
            List<CustomerFeedbackSurveyPOSMappingDTO> custFeedbackSurveyPOSMappingList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_POS_MAPPING_ID
                            || searchParameter.Key == CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID
                            || searchParameter.Key == CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.POS_MACHINE_ID
                            || searchParameter.Key == CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.MASTER_ENTITY_ID
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                if (searchParameters.Count > 0)
                    selectCustomerFeedbackSurveyPOSMappingQuery = selectCustomerFeedbackSurveyPOSMappingQuery + query;
            }
            DataTable custFeedbackSurveyPOSMappingData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyPOSMappingQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackSurveyPOSMappingData.Rows.Count > 0)
            {
                custFeedbackSurveyPOSMappingList = new List<CustomerFeedbackSurveyPOSMappingDTO>();
                foreach (DataRow custFeedbackSurveyPOSMappingDataRow in custFeedbackSurveyPOSMappingData.Rows)
                {
                    CustomerFeedbackSurveyPOSMappingDTO custFeedbackSurveyPOSMappingDataObject = GetCustomerFeedbackSurveyPOSMappingDTO(custFeedbackSurveyPOSMappingDataRow);
                    custFeedbackSurveyPOSMappingList.Add(custFeedbackSurveyPOSMappingDataObject);
                }
            }
            log.LogMethodExit(custFeedbackSurveyPOSMappingList);
            return custFeedbackSurveyPOSMappingList;
        }
    }
}
