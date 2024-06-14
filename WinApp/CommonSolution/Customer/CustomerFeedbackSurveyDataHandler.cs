/********************************************************************************************
 * Project Name - Customer Feedback Survey Data Handler
 * Description  - Data handler of the Customer Feedback Survey class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2       19-Jul-2019    Girish Kundar       Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2        05-Dec-2019   Jinto Thomas        Removed siteid from update query                                                          
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
    /// Customer Feedback Survey Data Handler - Handles insert, update and select of Cust Feedback Survey objects
    /// </summary>
    public class CustomerFeedbackSurveyDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustFeedbackSurvey AS cfs";
        private static readonly Dictionary<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>
            {
                {CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.CUST_FB_SURVEY_ID, "cfs.CustFbSurveyId"},
                {CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.FROM_DATE, "cfs.FromDate"},
                {CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.TO_DATE, "cfs.ToDate"},
                {CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SURVEY_NAME, "cfs.SurveyName"},
                {CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE, "cfs.IsActive"},
                {CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SITE_ID, "cfs.site_id"},
                {CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.MASTER_ENTITY_ID, "cfs.MasterEntityId"},
                {CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.CUST_FB_SURVEY_ID_LIST, "cfs.CustFbSurveyId"},
            };

        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyDataHandler class
        /// </summary>
        public CustomerFeedbackSurveyDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackSurvey Record.
        /// </summary>
        /// <param name="customerFeedbackSurveyDTO">CustomerFeedbackSurveyDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackSurveyDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyId", customerFeedbackSurveyDTO.CustFbSurveyId, true));
            parameters.Add(new SqlParameter("@surveyName", string.IsNullOrEmpty(customerFeedbackSurveyDTO.SurveyName) ? string.Empty : (object)customerFeedbackSurveyDTO.SurveyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromDate", customerFeedbackSurveyDTO.FromDate == DateTime.MinValue ? DBNull.Value : (object)customerFeedbackSurveyDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toDate", customerFeedbackSurveyDTO.ToDate == DateTime.MinValue ? DBNull.Value : (object)customerFeedbackSurveyDTO.ToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isResponseMandatory", customerFeedbackSurveyDTO.IsResponseMandatory));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerFeedbackSurveyDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackSurveyDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Customer Feedback Survey record to the database
        /// </summary>
        /// <param name="customerFeedbackSurveyDTO">CustomerFeedbackSurveyDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackSurveyDTO</returns>
        public CustomerFeedbackSurveyDTO InsertCustomerFeedbackSurvey(CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO, string loginId, int siteId)
        {

            log.LogMethodEntry(customerFeedbackSurveyDTO, loginId, siteId);
            string query = @"insert into CustFeedbackSurvey 
                                                        ( 
                                                        SurveyName,
                                                        FromDate,
                                                        ToDate,                                                        
                                                        IsActive,
                                                        IsResponseMandatory,
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
                                                         @surveyName,
                                                         @fromDate,
                                                         @toDate,
                                                         @isResponseMandatory,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                        ) SELECT * FROM CustFeedbackSurvey WHERE CustFbSurveyId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerFeedbackSurveyDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerFeedbackSurveyDTO(customerFeedbackSurveyDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerFeedbackSurveyDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerFeedbackSurveyDTO);
            return customerFeedbackSurveyDTO;
        }

        /// <summary>
        /// Updates the Customer Feedback Survey record
        /// </summary>
        /// <param name="custFeedbackSurvey">CustomerFeedbackSurveyDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackSurveyDTO</returns>s
        public CustomerFeedbackSurveyDTO UpdateCustomerFeedbackSurvey(CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO, string loginId, int siteId)
        {

            log.LogMethodEntry(customerFeedbackSurveyDTO, loginId, siteId);
            string query = @"update CustFeedbackSurvey 
                                         set SurveyName=@surveyName,
                                             FromDate=@fromDate,
                                             ToDate=@toDate,                                                                                      
                                             IsResponseMandatory = @isResponseMandatory, 
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --Site_id=@siteid,
                                             MasterEntityId = @masterEntityId                                             
                                       where CustFbSurveyId = @custFbSurveyId 
                                       SELECT * FROM CustFeedbackSurvey WHERE CustFbSurveyId  = @custFbSurveyId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerFeedbackSurveyDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerFeedbackSurveyDTO(customerFeedbackSurveyDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerFeedbackSurveyDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerFeedbackSurveyDTO);
            return customerFeedbackSurveyDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerFeedbackSurveyDTO">CustomerFeedbackSurveyDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerFeedbackSurveyDTO(CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackSurveyDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackSurveyDTO.CustFbSurveyId = Convert.ToInt32(dt.Rows[0]["CustFbSurveyId"]);
                customerFeedbackSurveyDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackSurveyDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackSurveyDTO.Guid = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
                customerFeedbackSurveyDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackSurveyDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackSurveyDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerFeedbackSurveyDTO class type
        /// </summary>
        /// <param name="custFeedbackSurveyDataRow">CustomerFeedbackSurvey DataRow</param>
        /// <returns>Returns CustomerFeedbackSurvey</returns>
        private CustomerFeedbackSurveyDTO GetCustomerFeedbackSurveyDTO(DataRow custFeedbackSurveyDataRow)
        {
            log.LogMethodEntry(custFeedbackSurveyDataRow);
            CustomerFeedbackSurveyDTO custFeedbackSurveyDataObject = new CustomerFeedbackSurveyDTO(Convert.ToInt32(custFeedbackSurveyDataRow["CustFbSurveyId"]),
                                            custFeedbackSurveyDataRow["SurveyName"].ToString(),
                                            custFeedbackSurveyDataRow["FromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDataRow["FromDate"]),
                                            custFeedbackSurveyDataRow["ToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDataRow["ToDate"]),
                                            custFeedbackSurveyDataRow["IsResponseMandatory"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDataRow["IsResponseMandatory"]),
                                            custFeedbackSurveyDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDataRow["IsActive"]),
                                            custFeedbackSurveyDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDataRow["CreatedBy"]),
                                            custFeedbackSurveyDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDataRow["CreationDate"]),
                                            custFeedbackSurveyDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDataRow["LastUpdatedBy"]),
                                            custFeedbackSurveyDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDataRow["LastupdatedDate"]),
                                            custFeedbackSurveyDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDataRow["Site_id"]),
                                            custFeedbackSurveyDataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDataRow["GUID"]),
                                            custFeedbackSurveyDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDataRow["SynchStatus"]),
                                            custFeedbackSurveyDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(custFeedbackSurveyDataObject);
            return custFeedbackSurveyDataObject;
        }

        /// <summary>
        /// Gets the Customer Feedback Survey data of passed Customer Fb QuestionId
        /// </summary>
        /// <param name="CustFbSurveyId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackSurveyDTO</returns>
        public CustomerFeedbackSurveyDTO GetCustomerFeedbackSurvey(int CustFbSurveyId)
        {
            log.LogMethodEntry(CustFbSurveyId);
            CustomerFeedbackSurveyDTO custFeedbackSurveyDataObject = null;
            string selectCustomerFeedbackSurveyQuery = SELECT_QUERY + "   WHERE cfs.CustFbSurveyId = @CustFbSurveyId";
            SqlParameter[] selectCustomerFeedbackSurveyParameters = new SqlParameter[1];
            selectCustomerFeedbackSurveyParameters[0] = new SqlParameter("@CustFbSurveyId", CustFbSurveyId);
            DataTable custFeedbackSurvey = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyQuery, selectCustomerFeedbackSurveyParameters, sqlTransaction);
            if (custFeedbackSurvey.Rows.Count > 0)
            {
                DataRow CustomerFeedbackSurveyRow = custFeedbackSurvey.Rows[0];
                custFeedbackSurveyDataObject = GetCustomerFeedbackSurveyDTO(CustomerFeedbackSurveyRow);
            }
            log.LogMethodExit(custFeedbackSurveyDataObject);
            return custFeedbackSurveyDataObject;

        }

        /// <summary>
        /// Gets the CustomerFeedbackSurveyDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackSurveyDTO matching the search criteria</returns>
        public List<CustomerFeedbackSurveyDTO> GetCustomerFeedbackSurveyList(List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCustomerFeedbackSurveyQuery = SELECT_QUERY;
            List<CustomerFeedbackSurveyDTO> custFeedbackSurveyList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.CUST_FB_SURVEY_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SURVEY_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.FROM_DATE
                            || searchParameter.Key == CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.CUST_FB_SURVEY_ID_LIST)
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
                    selectCustomerFeedbackSurveyQuery = selectCustomerFeedbackSurveyQuery + query;
            }
            DataTable custFeedbackSurveyData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackSurveyData.Rows.Count > 0)
            {
                custFeedbackSurveyList = new List<CustomerFeedbackSurveyDTO>();
                foreach (DataRow custFeedbackSurveyDataRow in custFeedbackSurveyData.Rows)
                {
                    CustomerFeedbackSurveyDTO custFeedbackSurveyDataObject = GetCustomerFeedbackSurveyDTO(custFeedbackSurveyDataRow);
                    custFeedbackSurveyList.Add(custFeedbackSurveyDataObject);
                }
            }
            log.LogMethodExit(custFeedbackSurveyList);
            return custFeedbackSurveyList;
        }

        /// <summary>
        /// Gets the Posmachine 
        /// </summary>
        /// <param name="posName">posName</param>
        /// <returns>Returns posMachineId</returns>
        public int GetPosMachineId(string posName, int siteId)
        {
            log.LogMethodEntry(posName);
            int posMachineId = -1;
            string selectPOSQuery = @"select posMachineId from PosMachines WHERE POSName = @posName and (site_id = @siteId or @siteId = -1)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@posName", posName));
            sqlParameters.Add(new SqlParameter("@siteId", siteId));
            DataTable posMachineData = dataAccessHandler.executeSelectQuery(selectPOSQuery, sqlParameters.ToArray(), sqlTransaction);
            if (posMachineData.Rows.Count > 0)
            {
                posMachineId = Convert.ToInt32(posMachineData.Rows[0]["posMachineId"]);
            }
            log.LogMethodExit(posMachineId);
            return posMachineId;
        }
    }
}
