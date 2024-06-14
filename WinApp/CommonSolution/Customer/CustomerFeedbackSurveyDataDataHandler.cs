/********************************************************************************************
 * Project Name - Cust Feedback Survey Dataset Data  Handler
 * Description  - Data handler of the Cust Feedback Survey class
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
    public class CustomerFeedbackSurveyDataDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustFeedbackSurveyData AS csd";
        private static readonly Dictionary<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>
            {
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATA_ID, "csd.CustFbSurveyDataId"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID, "csd.CustFbSurveyDataSetId"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID_LIST, "csd.CustFbSurveyDataSetId"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DETAIL_ID, "csd.CustFbSurveyDetailId"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_VALUE_ID, "csd.CustFbResponseValueId"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_DATE, "csd.CustFbResponseDate"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_TEXT, "csd.CustFbResponseText"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.IS_ACTIVE, "csd.IsActive"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.MASTER_ENTITY_ID, "csd.MasterEntityId"},
                {CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.SITE_ID, "csd.Site_id"}
            };

        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyDataDataHandler class
        /// </summary>
        public CustomerFeedbackSurveyDataDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackQuestions Record.
        /// </summary>
        /// <param name="customerFeedbackSurveyDataDTO">CustomerFeedbackSurveyDataDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyDataId", customerFeedbackSurveyDataDTO.CustFbSurveyDataId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyDataSetId", customerFeedbackSurveyDataDTO.CustFbSurveyDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyDetailId", customerFeedbackSurveyDataDTO.CustFbSurveyDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustFbResponseValueId", customerFeedbackSurveyDataDTO.CustFbResponseValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbResponseText", string.IsNullOrEmpty(customerFeedbackSurveyDataDTO.CustFbResponseText) ? DBNull.Value : (object)customerFeedbackSurveyDataDTO.CustFbResponseText));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbResponseDate", customerFeedbackSurveyDataDTO.CustFbResponseDate == DateTime.MinValue ? DBNull.Value : (object)customerFeedbackSurveyDataDTO.CustFbResponseDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerFeedbackSurveyDataDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackSurveyDataDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Customer Feedback Survey record to the database
        /// </summary>
        /// <param name="custFeedbackSurveyData">CustomerFeedbackSurveyDataDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        ///  <param name="sqlTrxn">sqlTransaction object</param>
        /// <returns>Returns CustomerFeedbackSurveyDataDTO</returns>
        public CustomerFeedbackSurveyDataDTO InsertCustomerFeedbackSurveyData(CustomerFeedbackSurveyDataDTO custFeedbackSurveyData, string loginId, int siteId, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(custFeedbackSurveyData, loginId, siteId);
            string query = @"insert into CustFeedbackSurveyData 
                                                        ( 
                                                        CustFbSurveyDataSetId,
                                                        CustFbSurveyDetailId,
                                                        CustFbResponseValueId,
                                                        CustFbResponseText,
                                                        CustFbResponseDate,
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
                                                         @custFbSurveyDataSetId,
                                                         @custFbSurveyDetailId,
                                                         @custFbResponseValueId,
                                                         @custFbResponseText,
                                                         @custFbResponseDate,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                        ) SELECT * FROM CustFeedbackSurveyData WHERE CustFbSurveyDataId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackSurveyData, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackSurveyData(custFeedbackSurveyData, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting custFeedbackSurveyData", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackSurveyData);
            return custFeedbackSurveyData;
        }

        /// <summary>
        /// Updates the Customer Feedback Survey record
        /// </summary>
        /// <param name="custFeedbackSurveyData">CustomerFeedbackSurveyDataDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        ///  <param name="sqlTrxn">sqlTransaction object</param>
        /// <returns>Returns the CustomerFeedbackSurveyDataDTO</returns>
        public CustomerFeedbackSurveyDataDTO UpdateCustomerFeedbackSurveyData(CustomerFeedbackSurveyDataDTO custFeedbackSurveyData, string loginId, int siteId, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(custFeedbackSurveyData, loginId, siteId);
            string query = @"update CustFeedbackSurveyData 
                                         set CustFbSurveyDataSetId = @custFbSurveyDataSetId,
                                             CustFbSurveyDetailId=@custFbSurveyDetailId,
                                             CustFbResponseValueId = @custFbResponseValueId,
                                             CustFbResponseText = @custFbResponseText,
                                             CustFbResponseDate = @custFbResponseDate,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id = @siteid,
                                             MasterEntityId = @masterEntityId                                            
                                       where CustFbSurveyDataId=@custFbSurveyDataId
                                      SELECT * FROM CustFeedbackSurveyData WHERE CustFbSurveyDataId  = @custFbSurveyDataId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackSurveyData, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackSurveyData(custFeedbackSurveyData, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating custFeedbackSurveyData", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackSurveyData);
            return custFeedbackSurveyData;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerFeedbackSurveyDataDTO">CustomerFeedbackSurveyDataDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustFeedbackSurveyData(CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackSurveyDataDTO.CustFbSurveyDataId = Convert.ToInt32(dt.Rows[0]["CustFbSurveyDataId"]);
                customerFeedbackSurveyDataDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackSurveyDataDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackSurveyDataDTO.Guid = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
                customerFeedbackSurveyDataDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackSurveyDataDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackSurveyDataDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerFeedbackSurveyDataDTO class type
        /// </summary>
        /// <param name="custFeedbackSurveyDataDataRow">CustomerFeedbackSurveyData DataRow</param>
        /// <returns>Returns CustomerFeedbackSurveyData</returns>
        private CustomerFeedbackSurveyDataDTO GetCustomerFeedbackSurveyDataDTO(DataRow custFeedbackSurveyDataDataRow)
        {
            log.LogMethodEntry(custFeedbackSurveyDataDataRow);
            CustomerFeedbackSurveyDataDTO custFeedbackSurveyDataDataObject = new CustomerFeedbackSurveyDataDTO(Convert.ToInt32(custFeedbackSurveyDataDataRow["CustFbSurveyDataId"]),
                                            custFeedbackSurveyDataDataRow["CustFbSurveyDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDataDataRow["CustFbSurveyDataSetId"]),
                                            custFeedbackSurveyDataDataRow["CustFbSurveyDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDataDataRow["CustFbSurveyDetailId"]),
                                            custFeedbackSurveyDataDataRow["CustFbResponseValueId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDataDataRow["CustFbResponseValueId"]),
                                            custFeedbackSurveyDataDataRow["CustFbResponseText"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDataDataRow["CustFbResponseText"]),
                                            custFeedbackSurveyDataDataRow["CustFbResponseDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDataDataRow["CustFbResponseDate"]),
                                            custFeedbackSurveyDataDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDataDataRow["IsActive"]),
                                            custFeedbackSurveyDataDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDataDataRow["CreatedBy"]),
                                            custFeedbackSurveyDataDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDataDataRow["CreationDate"]),
                                            custFeedbackSurveyDataDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDataDataRow["LastUpdatedBy"]),
                                            custFeedbackSurveyDataDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackSurveyDataDataRow["LastupdatedDate"]),
                                            custFeedbackSurveyDataDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDataDataRow["Site_id"]),
                                            custFeedbackSurveyDataDataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackSurveyDataDataRow["GUID"]),
                                            custFeedbackSurveyDataDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackSurveyDataDataRow["SynchStatus"]),
                                            custFeedbackSurveyDataDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackSurveyDataDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(custFeedbackSurveyDataDataObject);
            return custFeedbackSurveyDataDataObject;
        }

        /// <summary>
        /// Gets the Customer Feedback Survey data of passed Customer Fb QuestionId
        /// </summary>
        /// <param name="CustFbSurveyDataSetId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackSurveyDataDTO</returns>
        public CustomerFeedbackSurveyDataDTO GetCustomerFeedbackSurveyData(int CustFbSurveyDataId)
        {
            log.LogMethodEntry(CustFbSurveyDataId);
            CustomerFeedbackSurveyDataDTO custFeedbackSurveyDataDataObject = null;
            string selectCustomerFeedbackSurveyDataQuery = SELECT_QUERY + "   WHERE csd.CustFbSurveyDataId = @CustFbSurveyDataId";
            SqlParameter[] selectCustomerFeedbackSurveyDataParameters = new SqlParameter[1];
            selectCustomerFeedbackSurveyDataParameters[0] = new SqlParameter("@CustFbSurveyDataId", CustFbSurveyDataId);
            DataTable custFeedbackSurveyData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyDataQuery, selectCustomerFeedbackSurveyDataParameters, sqlTransaction);
            if (custFeedbackSurveyData.Rows.Count > 0)
            {
                DataRow CustomerFeedbackSurveyDataRow = custFeedbackSurveyData.Rows[0];
                custFeedbackSurveyDataDataObject = GetCustomerFeedbackSurveyDataDTO(CustomerFeedbackSurveyDataRow);
            }
            log.LogMethodExit(custFeedbackSurveyDataDataObject);
            return custFeedbackSurveyDataDataObject;

        }

        /// <summary>
        /// Gets the CustomerFeedbackSurveyDataDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackSurveyDataDTO matching the search criteria</returns>
        public List<CustomerFeedbackSurveyDataDTO> GetCustomerFeedbackSurveyDataList(List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCustomerFeedbackSurveyDataQuery = SELECT_QUERY;
            List<CustomerFeedbackSurveyDataDTO> custFeedbackSurveyDataList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_VALUE_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DETAIL_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATA_ID
                            || searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_TEXT)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.IS_ACTIVE)
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
                    selectCustomerFeedbackSurveyDataQuery = selectCustomerFeedbackSurveyDataQuery + query;
            }
            DataTable custFeedbackSurveyDataData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyDataQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackSurveyDataData.Rows.Count > 0)
            {
                custFeedbackSurveyDataList = new List<CustomerFeedbackSurveyDataDTO>();
                foreach (DataRow custFeedbackSurveyDataDataRow in custFeedbackSurveyDataData.Rows)
                {
                    CustomerFeedbackSurveyDataDTO custFeedbackSurveyDataDataObject = GetCustomerFeedbackSurveyDataDTO(custFeedbackSurveyDataDataRow);
                    custFeedbackSurveyDataList.Add(custFeedbackSurveyDataDataObject);
                }
            }
            log.LogMethodExit(custFeedbackSurveyDataList);
            return custFeedbackSurveyDataList;
        }
    }
}
