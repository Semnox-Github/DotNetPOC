/********************************************************************************************
 * Project Name - Customer Feedback SurveyMapping Data  Handler
 * Description  - Data handler of the Customer Feedback Survey Mapping class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2       19-Jul-2019    Girish Kundar       Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue 
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query   
 *2.70.3       21-02-2020     Girish Kundar     Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Customer Feedback Survey Dataset Data  Handler - Handles insert, update and select of Cust Feedback Survey objects
    /// </summary>
    public class CustomerFeedbackSurveyMappingDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustFeedbackSurveyMapping AS cfm";
        private static readonly Dictionary<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>
            {
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.CUST_FB_SURVEY_MAP_ID, "cfm.CustFbSurveyMapId"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.CUST_FB_SURVEY_DATA_SET_ID, "cfm.CustFbSurveyDataSetId"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.CUST_FB_SURVEY_DATA_SET_ID_LIST, "cfm.CustFbSurveyDataSetId"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_NAME, "cfm.ObjectName"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_ID, "cfm.ObjectId"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.LAST_CUST_FB_SURVEY_DETAIL_ID, "cfm.LastCustFbSurveyDetailId"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.LAST_VISIT_DATE, "cfm.LastVisitDate"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.VISIT_COUNT, "cfm.VisitCount"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.IS_ACTIVE, "cfm.IsActive"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.SITE_ID, "cfm.site_id"},
                {CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.MASTER_ENTITY_ID, "cfm.MasterEntityId"}
            };

        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyMappingDataHandler class
        /// </summary>
        public CustomerFeedbackSurveyMappingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackSurveyMapping Record.
        /// </summary>
        /// <param name="customerFeedbackSurveyMappingDTO">CustomerFeedbackSurveyMappingDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackSurveyMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustFbSurveyMapId", customerFeedbackSurveyMappingDTO.CustFbSurveyMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyDataSetId", customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastCustFbSurveyDetailId", customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@objectId", customerFeedbackSurveyMappingDTO.ObjectId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@visitCount", customerFeedbackSurveyMappingDTO.VisitCount, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastVisitDate", customerFeedbackSurveyMappingDTO.LastVisitDate == DateTime.MinValue ? DBNull.Value : (object)customerFeedbackSurveyMappingDTO.LastVisitDate));
            parameters.Add(new SqlParameter("@objectName", string.IsNullOrEmpty(customerFeedbackSurveyMappingDTO.ObjectName) ? string.Empty : (object)customerFeedbackSurveyMappingDTO.ObjectName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerFeedbackSurveyMappingDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackSurveyMappingDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Customer Feedback Survey Mapping record to the database
        /// </summary>
        /// <param name="customerFeedbackSurveyMapping">CustomerFeedbackSurveyMappingDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackSurveyMappingDTO</returns>
        public CustomerFeedbackSurveyMappingDTO InsertCustomerFeedbackSurveyMapping(CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMapping, string loginId, int siteId, SqlTransaction sqltrxn)
        {
            log.LogMethodEntry(customerFeedbackSurveyMapping, loginId, siteId);
            string query = @"insert into CustFeedbackSurveyMapping 
                                                        ( 
                                                        ObjectName,
                                                        ObjectId,
                                                        CustFbSurveyDataSetId,
                                                        LastVisitDate,
                                                        VisitCount,
                                                        LastCustFbSurveyDetailId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                         @objectName,
                                                         @objectId,
                                                         @custFbSurveyDataSetId,
                                                         @lastVisitDate,
                                                         @visitCount,
                                                         @lastCustFbSurveyDetailId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                        ) SELECT * FROM CustFeedbackSurveyMapping WHERE CustFbSurveyMapId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerFeedbackSurveyMapping, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerFeedbackSurveyMappingDTO(customerFeedbackSurveyMapping, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerFeedbackSurveyMapping", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerFeedbackSurveyMapping);
            return customerFeedbackSurveyMapping;
        }

        /// <summary>
        /// Updates the Customer Feedback Survey record
        /// </summary>
        /// <param name="customerFeedbackSurveyMapping">CustomerFeedbackSurveyMappingDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackSurveyMappingDTO</returns>
        public CustomerFeedbackSurveyMappingDTO UpdateCustomerFeedbackSurveyMapping(CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMapping, string loginId, int siteId, SqlTransaction sqltrxn)
        {
            log.LogMethodEntry(customerFeedbackSurveyMapping, loginId, siteId);
            string query = @"update CustFeedbackSurveyMapping 
                                         set ObjectName = @objectName,
                                             ObjectId = @objectId,
                                             CustFbSurveyDataSetId=@custFbSurveyDataSetId,
                                             LastVisitDate=@lastVisitDate,
                                             VisitCount=@visitCount,
                                             LastCustFbSurveyDetailId=@lastCustFbSurveyDetailId,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             MasterEntityId = @masterEntityId                                             
                                       where CustFbSurveyMapId = @CustFbSurveyMapId 
                          SELECT * FROM CustFeedbackSurveyMapping WHERE CustFbSurveyMapId  = @CustFbSurveyMapId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerFeedbackSurveyMapping, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerFeedbackSurveyMappingDTO(customerFeedbackSurveyMapping, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerFeedbackSurveyMapping", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerFeedbackSurveyMapping);
            return customerFeedbackSurveyMapping;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerFeedbackSurveyMappingDTO">CustomerFeedbackSurveyMappingDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerFeedbackSurveyMappingDTO(CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackSurveyMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackSurveyMappingDTO.CustFbSurveyMapId = Convert.ToInt32(dt.Rows[0]["CustFbSurveyMapId"]);
                customerFeedbackSurveyMappingDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackSurveyMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackSurveyMappingDTO.Guid = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
                customerFeedbackSurveyMappingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackSurveyMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackSurveyMappingDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerFeedbackSurveyMappingDTO class type
        /// </summary>
        /// <param name="customerFeedbackSurveyMappingDataRow">CustomerFeedbackSurveyMapping DataRow</param>
        /// <returns>Returns CustomerFeedbackSurveyMapping</returns>
        private CustomerFeedbackSurveyMappingDTO GetCustomerFeedbackSurveyMappingDTO(DataRow customerFeedbackSurveyMappingDataRow)
        {
            log.LogMethodEntry(customerFeedbackSurveyMappingDataRow);
            CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDataObject = new CustomerFeedbackSurveyMappingDTO(Convert.ToInt32(customerFeedbackSurveyMappingDataRow["CustFbSurveyMapId"]),
                                            customerFeedbackSurveyMappingDataRow["ObjectName"].ToString(),
                                            customerFeedbackSurveyMappingDataRow["ObjectId"] == DBNull.Value ? -1 : Convert.ToInt32(customerFeedbackSurveyMappingDataRow["ObjectId"]),
                                            customerFeedbackSurveyMappingDataRow["CustFbSurveyDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(customerFeedbackSurveyMappingDataRow["CustFbSurveyDataSetId"]),
                                            customerFeedbackSurveyMappingDataRow["LastVisitDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerFeedbackSurveyMappingDataRow["LastVisitDate"]),
                                            customerFeedbackSurveyMappingDataRow["VisitCount"] == DBNull.Value ? -1 : Convert.ToInt32(customerFeedbackSurveyMappingDataRow["VisitCount"]),
                                            customerFeedbackSurveyMappingDataRow["LastCustFbSurveyDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(customerFeedbackSurveyMappingDataRow["LastCustFbSurveyDetailId"]),
                                            customerFeedbackSurveyMappingDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(customerFeedbackSurveyMappingDataRow["IsActive"]),
                                            customerFeedbackSurveyMappingDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(customerFeedbackSurveyMappingDataRow["CreatedBy"]),
                                            customerFeedbackSurveyMappingDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerFeedbackSurveyMappingDataRow["CreationDate"]),
                                            customerFeedbackSurveyMappingDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(customerFeedbackSurveyMappingDataRow["LastUpdatedBy"]),
                                            customerFeedbackSurveyMappingDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerFeedbackSurveyMappingDataRow["LastupdatedDate"]),
                                            customerFeedbackSurveyMappingDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(customerFeedbackSurveyMappingDataRow["Site_id"]),
                                            customerFeedbackSurveyMappingDataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(customerFeedbackSurveyMappingDataRow["GUID"]),
                                            customerFeedbackSurveyMappingDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(customerFeedbackSurveyMappingDataRow["SynchStatus"]),
                                            customerFeedbackSurveyMappingDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(customerFeedbackSurveyMappingDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(customerFeedbackSurveyMappingDataObject);
            return customerFeedbackSurveyMappingDataObject;
        }

        /// <summary>
        /// Gets the Cust Feedback Survey data of passed Cust Fb QuestionId
        /// </summary>
        /// <param name="CustFbSurveyMapId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackSurveyMappingDTO</returns>
        public CustomerFeedbackSurveyMappingDTO GetCustomerFeedbackSurveyMapping(int CustFbSurveyMapId)
        {
            log.LogMethodEntry(CustFbSurveyMapId);
            CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDataObject = null;
            string selectCustomerFeedbackSurveyMappingQuery = SELECT_QUERY + "   WHERE cfm.CustFbSurveyMapId = @CustFbSurveyMapId";
            SqlParameter[] selectCustomerFeedbackSurveyMappingParameters = new SqlParameter[1];
            selectCustomerFeedbackSurveyMappingParameters[0] = new SqlParameter("@CustFbSurveyMapId", CustFbSurveyMapId);
            DataTable customerFeedbackSurveyMapping = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyMappingQuery, selectCustomerFeedbackSurveyMappingParameters, sqlTransaction);
            if (customerFeedbackSurveyMapping.Rows.Count > 0)
            {
                DataRow CustomerFeedbackSurveyMappingRow = customerFeedbackSurveyMapping.Rows[0];
                customerFeedbackSurveyMappingDataObject = GetCustomerFeedbackSurveyMappingDTO(CustomerFeedbackSurveyMappingRow);

            }
            log.LogMethodExit(customerFeedbackSurveyMappingDataObject);
            return customerFeedbackSurveyMappingDataObject;
        }

        /// <summary>
        /// Gets the CustomerFeedbackSurveyMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackSurveyMappingDTO matching the search criteria</returns>
        public List<CustomerFeedbackSurveyMappingDTO> GetCustomerFeedbackSurveyMappingList(List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCustomerFeedbackSurveyMappingQuery = SELECT_QUERY;
            List<CustomerFeedbackSurveyMappingDTO> customerFeedbackSurveyMappingList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;

                foreach (KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.CUST_FB_SURVEY_DATA_SET_ID
                            || searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.CUST_FB_SURVEY_MAP_ID
                            || searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.LAST_CUST_FB_SURVEY_DETAIL_ID
                            || searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_ID
                            || searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.VISIT_COUNT
                            || searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.IS_ACTIVE
                            || searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.CUST_FB_SURVEY_DATA_SET_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.LAST_VISIT_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    selectCustomerFeedbackSurveyMappingQuery = selectCustomerFeedbackSurveyMappingQuery + query;
            }
            DataTable customerFeedbackSurveyMappingData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyMappingQuery, parameters.ToArray(), sqlTransaction);
            if (customerFeedbackSurveyMappingData.Rows.Count > 0)
            {
                customerFeedbackSurveyMappingList = new List<CustomerFeedbackSurveyMappingDTO>();
                foreach (DataRow customerFeedbackSurveyMappingDataRow in customerFeedbackSurveyMappingData.Rows)
                {
                    CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDataObject = GetCustomerFeedbackSurveyMappingDTO(customerFeedbackSurveyMappingDataRow);
                    customerFeedbackSurveyMappingList.Add(customerFeedbackSurveyMappingDataObject);
                }
            }
            log.LogMethodExit(customerFeedbackSurveyMappingList);
            return customerFeedbackSurveyMappingList;
        }
    }
}
