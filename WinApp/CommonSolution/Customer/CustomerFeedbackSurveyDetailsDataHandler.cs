/********************************************************************************************
 * Project Name - Customer Feedback Survey Details Data Handler
 * Description  - Data handler of the Customer Feedback Survey Details class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2        19-Jul-2019    Girish Kundar       Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query         
 *2.80        24-Feb-2020   Mushahid Faizan      Modified GetSQLParameters().
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
    /// Customer Feedback Survey Details Data Handler - Handles insert, update and select of Customer Feedback SurveyDetails objects
    /// </summary>
    public class CustomerFeedbackSurveyDetailsDataHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustFeedbackSurveyDetails AS cfd";
        private static readonly Dictionary<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>
            {
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID, "cfd.CustFbSurveyDetailId"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, "cfd.CustFbSurveyId"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID_LIST, "cfd.CustFbSurveyId"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID, "cfd.CustFbQuestionId"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_RESPONSE_ID, "cfd.CustFbResponseId"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID, "cfd.CriteriaId"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE, "cfd.CriteriaValue"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.EXPECTED_CUST_FB_RESPONSE_VALUE_ID, "cfd.ExpectedCustFbResponseValueId"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.NEXT_QUESTION_ID, "cfd.NextQuestionId"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_RESPONSE_MANDATORY, "cfd.IsResponseMandatory"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "cfd.IsActive"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, "cfd.site_id"},
                {CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.MASTER_ENTITY_ID, "cfd.MasterEntityId"}
            };

        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyDetailsDataHandler class
        /// </summary>
        public CustomerFeedbackSurveyDetailsDataHandler(SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackSurveyDetails Record.
        /// </summary>
        /// <param name="customerFeedbackSurveyDetailsDTO">CustomerFeedbackSurveyDetailsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackSurveyDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyDetailId", customerFeedbackSurveyDetailsDTO.CustFbSurveyDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyId", customerFeedbackSurveyDetailsDTO.CustFbSurveyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbQuestionId", customerFeedbackSurveyDetailsDTO.CustFbQuestionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@nextQuestionId", customerFeedbackSurveyDetailsDTO.NextQuestionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@criteriaId", customerFeedbackSurveyDetailsDTO.CriteriaId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbResponseId", customerFeedbackSurveyDetailsDTO.CustFbResponseId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expectedCustFbResponseValueId", customerFeedbackSurveyDetailsDTO.ExpectedCustFbResponseValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@criteriaValue", string.IsNullOrEmpty(customerFeedbackSurveyDetailsDTO.CriteriaValue) ? DBNull.Value : (object)customerFeedbackSurveyDetailsDTO.CriteriaValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isResponseMandatory", customerFeedbackSurveyDetailsDTO.IsResponseMandatory));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isRecur", customerFeedbackSurveyDetailsDTO.IsRecur));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerFeedbackSurveyDetailsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackSurveyDetailsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Customer Feedback SurveyDetails record to the database
        /// </summary>
        /// <param name="custFeedbackSurveyDetails">CustomerFeedbackSurveyDetailsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        ///  <param name="sqlTrxn">sqlTransaction object</param>
        /// <returns>Returns CustomerFeedbackSurveyDetailsDTO</returns>
        public CustomerFeedbackSurveyDetailsDTO InsertCustomerFeedbackSurveyDetails(CustomerFeedbackSurveyDetailsDTO custFeedbackSurveyDetails, string loginId, int siteId, SqlTransaction sqlTrxn)
        {

            log.LogMethodEntry(custFeedbackSurveyDetails, loginId, siteId);
            string query = @"insert into CustFeedbackSurveyDetails 
                                                        ( 
                                                        CustFbSurveyId,
                                                        CustFbQuestionId,
                                                        NextQuestionId,
                                                        CriteriaId,
                                                        CriteriaValue,
                                                        CustFbResponseId,
                                                        ExpectedCustFbResponseValueId,
                                                        IsResponseMandatory,
                                                        IsRecur,
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
                                                         @custFbSurveyId,
                                                         @custFbQuestionId,
                                                         @nextQuestionId,
                                                         @criteriaId,
                                                         @criteriaValue,
                                                         @custFbResponseId,
                                                         @expectedCustFbResponseValueId,
                                                         @isResponseMandatory,
                                                         @isRecur,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                        ) SELECT * FROM CustFeedbackSurveyDetails WHERE CustFbSurveyDetailId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackSurveyDetails, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackSurveyDetails(custFeedbackSurveyDetails, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CustFeedbackSurveyDetails", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackSurveyDetails);
            return custFeedbackSurveyDetails;
        }

        /// <summary>
        /// Updates the Customer Feedback SurveyDetails record
        /// </summary>
        /// <param name="custFeedbackSurveyDetails">CustomerFeedbackSurveyDetailsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        ///  <param name="sqlTrxn">sqlTransaction object</param>
        /// <returns>Returns CustomerFeedbackSurveyDetailsDTO</returns>
        public CustomerFeedbackSurveyDetailsDTO UpdateCustomerFeedbackSurveyDetails(CustomerFeedbackSurveyDetailsDTO custFeedbackSurveyDetails, string loginId, int siteId, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(custFeedbackSurveyDetails, loginId, siteId);
            string query = @"update CustFeedbackSurveyDetails 
                                         set CustFbSurveyId=@custFbSurveyId,
                                             CustFbQuestionId=@custFbQuestionId,
                                             NextQuestionId=@nextQuestionId,
                                             CriteriaId=@criteriaId,
                                             CriteriaValue=@criteriaValue,
                                             CustFbResponseId=@custFbResponseId,
                                             ExpectedCustFbResponseValueId=@expectedCustFbResponseValueId,
                                             IsResponseMandatory=@isResponseMandatory, 
                                             IsRecur=@isRecur,                                           
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             MasterEntityId = @masterEntityId                                             
                                       where CustFbSurveyDetailId = @custFbSurveyDetailId 
                                 SELECT * FROM CustFeedbackSurveyDetails WHERE CustFbSurveyDetailId  = @custFbSurveyDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackSurveyDetails, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackSurveyDetails(custFeedbackSurveyDetails, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CustFeedbackSurveyDetails", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackSurveyDetails);
            return custFeedbackSurveyDetails;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerFeedbackSurveyDetailsDTO">CustomerFeedbackSurveyDetailsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustFeedbackSurveyDetails(CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackSurveyDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackSurveyDetailsDTO.CustFbSurveyDetailId = Convert.ToInt32(dt.Rows[0]["CustFbSurveyDetailId"]);
                customerFeedbackSurveyDetailsDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackSurveyDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackSurveyDetailsDTO.Guid = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
                customerFeedbackSurveyDetailsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackSurveyDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackSurveyDetailsDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerFeedbackSurveyDetailsDTO class type
        /// </summary>
        /// <param name="custFeedbackSurveyDetailsDataRow">CustomerFeedbackSurveyDetails DataRow</param>
        /// <returns>Returns CustomerFeedbackSurveyDetails</returns>
        private CustomerFeedbackSurveyDetailsDTO GetCustomerFeedbackSurveyDetailsDTO(DataRow custFeedbackSurveyDetailsDataRow)
        {
            log.LogMethodEntry(custFeedbackSurveyDetailsDataRow);
            CustomerFeedbackSurveyDetailsDTO custFeedbackSurveyDetailsDataObject = new CustomerFeedbackSurveyDetailsDTO(Convert.ToInt32(custFeedbackSurveyDetailsDataRow["CustFbSurveyDetailId"]),
                                            custFeedbackSurveyDetailsDataRow["CustFbSurveyId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDetailsDataRow["CustFbSurveyId"]),
                                            custFeedbackSurveyDetailsDataRow["CustFbQuestionId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDetailsDataRow["CustFbQuestionId"]),
                                            custFeedbackSurveyDetailsDataRow["NextQuestionId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDetailsDataRow["NextQuestionId"]),
                                            custFeedbackSurveyDetailsDataRow["CriteriaId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDetailsDataRow["CriteriaId"]),
                                            custFeedbackSurveyDetailsDataRow["CriteriaValue"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDetailsDataRow["CriteriaValue"]),
                                            custFeedbackSurveyDetailsDataRow["ExpectedCustFbResponseValueId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDetailsDataRow["ExpectedCustFbResponseValueId"]),
                                            custFeedbackSurveyDetailsDataRow["CustFbResponseId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDetailsDataRow["CustFbResponseId"]),
                                            custFeedbackSurveyDetailsDataRow["IsResponseMandatory"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDetailsDataRow["IsResponseMandatory"]),
                                            custFeedbackSurveyDetailsDataRow["IsRecur"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDetailsDataRow["IsRecur"]),
                                            custFeedbackSurveyDetailsDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDetailsDataRow["IsActive"]),
                                            custFeedbackSurveyDetailsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDetailsDataRow["CreatedBy"]),
                                            custFeedbackSurveyDetailsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDetailsDataRow["CreationDate"]),
                                            custFeedbackSurveyDetailsDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDetailsDataRow["LastUpdatedBy"]),
                                            custFeedbackSurveyDetailsDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDetailsDataRow["LastupdatedDate"]),
                                            custFeedbackSurveyDetailsDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDetailsDataRow["site_id"]),
                                            custFeedbackSurveyDetailsDataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDetailsDataRow["GUID"]),
                                            custFeedbackSurveyDetailsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDetailsDataRow["SynchStatus"]),
                                            custFeedbackSurveyDetailsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDetailsDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(custFeedbackSurveyDetailsDataObject);
            return custFeedbackSurveyDetailsDataObject;
        }

        /// <summary>
        /// Gets the Customer Feedback SurveyDetails data of passed Customer Fb QuestionId
        /// </summary>
        /// <param name="CustFbSurveyDetailId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackSurveyDetailsDTO</returns>
        public CustomerFeedbackSurveyDetailsDTO GetCustomerFeedbackSurveyDetails(int CustFbSurveyDetailId)
        {
            log.LogMethodEntry(CustFbSurveyDetailId);
            CustomerFeedbackSurveyDetailsDTO custFeedbackSurveyDetailsDataObject = null;
            string selectCustomerFeedbackSurveyDetailsQuery = SELECT_QUERY + "    WHERE cfd.CustFbSurveyDetailId = @CustFbSurveyDetailId";
            SqlParameter[] selectCustomerFeedbackSurveyDetailsParameters = new SqlParameter[1];
            selectCustomerFeedbackSurveyDetailsParameters[0] = new SqlParameter("@CustFbSurveyDetailId", CustFbSurveyDetailId);
            DataTable custFeedbackSurveyDetails = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyDetailsQuery, selectCustomerFeedbackSurveyDetailsParameters, sqlTransaction);
            if (custFeedbackSurveyDetails.Rows.Count > 0)
            {
                DataRow CustomerFeedbackSurveyDetailsRow = custFeedbackSurveyDetails.Rows[0];
                custFeedbackSurveyDetailsDataObject = GetCustomerFeedbackSurveyDetailsDTO(CustomerFeedbackSurveyDetailsRow);
            }
            log.LogMethodExit(custFeedbackSurveyDetailsDataObject);
            return custFeedbackSurveyDetailsDataObject;
        }

        /// <summary>
        /// Gets the CustomerFeedbackSurveyDetailsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackSurveyDetailsDTO matching the search criteria</returns>
        public List<CustomerFeedbackSurveyDetailsDTO> GetCustomerFeedbackSurveyDetailsList(List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCustomerFeedbackSurveyDetailsQuery = SELECT_QUERY;
            List<CustomerFeedbackSurveyDetailsDTO> custFeedbackSurveyDetailsList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_RESPONSE_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.NEXT_QUESTION_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_RESPONSE_MANDATORY
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.EXPECTED_CUST_FB_RESPONSE_VALUE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                    selectCustomerFeedbackSurveyDetailsQuery = selectCustomerFeedbackSurveyDetailsQuery + query;
            }
            DataTable custFeedbackSurveyDetailsData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyDetailsQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackSurveyDetailsData.Rows.Count > 0)
            {
                custFeedbackSurveyDetailsList = new List<CustomerFeedbackSurveyDetailsDTO>();
                foreach (DataRow custFeedbackSurveyDetailsDataRow in custFeedbackSurveyDetailsData.Rows)
                {
                    CustomerFeedbackSurveyDetailsDTO custFeedbackSurveyDetailsDataObject = GetCustomerFeedbackSurveyDetailsDTO(custFeedbackSurveyDetailsDataRow);
                    custFeedbackSurveyDetailsList.Add(custFeedbackSurveyDetailsDataObject);
                }
            }
            log.LogMethodExit(custFeedbackSurveyDetailsList);
            return custFeedbackSurveyDetailsList;
        }

        /// <summary>
        /// Gets the GetCustomerFeedbackSurveyDetailsOfInitialLoadList list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackSurveyDetailsDTO matching the search criteria</returns>
        public List<CustomerFeedbackSurveyDetailsDTO> GetCustomerFeedbackSurveyDetailsOfInitialLoadList(List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCustomerFeedbackSurveyDetailsQuery = SELECT_QUERY;
            List<CustomerFeedbackSurveyDetailsDTO> custFeedbackSurveyDetailsList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = "";
                foreach (KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_RESPONSE_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.NEXT_QUESTION_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_RESPONSE_MANDATORY
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.EXPECTED_CUST_FB_RESPONSE_VALUE_ID)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE)
                        {
                            continue;
                            //query.Append(joiner + DBSearchParameters[searchParameter.Key] + "='" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID)
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
                query = query.Append(joiner + " isnull(CriteriaValue,'1') = isnull((select min(CriteriaValue) from CustFeedbackSurveyDetails s where s.CustFbSurveyId = CustFbSurveyId),'1')");
                if (searchParameters.Count > 0)
                    selectCustomerFeedbackSurveyDetailsQuery = selectCustomerFeedbackSurveyDetailsQuery + query;
            }
            DataTable custFeedbackSurveyDetailsData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyDetailsQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackSurveyDetailsData.Rows.Count > 0)
            {
                custFeedbackSurveyDetailsList = new List<CustomerFeedbackSurveyDetailsDTO>();
                foreach (DataRow custFeedbackSurveyDetailsDataRow in custFeedbackSurveyDetailsData.Rows)
                {
                    CustomerFeedbackSurveyDetailsDTO custFeedbackSurveyDetailsDataObject = GetCustomerFeedbackSurveyDetailsDTO(custFeedbackSurveyDetailsDataRow);
                    custFeedbackSurveyDetailsList.Add(custFeedbackSurveyDetailsDataObject);
                }
            }
            log.LogMethodExit(custFeedbackSurveyDetailsList);
            return custFeedbackSurveyDetailsList;
        }
    }
}
