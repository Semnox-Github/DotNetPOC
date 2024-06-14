/********************************************************************************************
 * Project Name - Customer Feedback Survey Data Set Data Handler
 * Description  - Data handler of the customer feedback survey data set data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Dec-2016   Raghuveera          Created 
 *2.70.2       19-Jul-2019    Girish Kundar     Modified :Structure of data Handler - insert /Update methods
 *                                                     Fix for SQL Injection Issue  
 *2.70.2        05-Dec-2019   Jinto Thomas      Removed siteid from update query   
 *2.70.3       21-02-2020     Girish Kundar     Modified : 3 tier Changes for REST API  
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
    /// Customer Feedback Survey Data Set Data Handler - Handles insert, update and select of customer feedback survey data set data objects
    /// </summary>
    public class CustomerFeedbackSurveyDataSetDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustFeedbackSurveyDataset AS cfsd";
        private static readonly Dictionary<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>
               {
                    {CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.CUST_FB_SURVEY_DATA_SET_ID, "cfsd.CustFbSurveyDataSetId"},
                    {CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.CUST_FB_SURVEY_DATA_SET_ID_LIST, "cfsd.CustFbSurveyDataSetId"},
                    {CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.SITE_ID, "cfsd.site_id"}  ,
                    {CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.MASTER_ENTITY_ID, "cfsd.MasterEntityid"}
               };

        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyDataSetDataHandler class
        /// </summary>
        public CustomerFeedbackSurveyDataSetDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackSurveyDataSet Record.
        /// </summary>
        /// <param name="customerFeedbackSurveyDataSetDTO">CustomerFeedbackSurveyDataSetDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbSurveyDataSetId", customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackSurveyDataSetDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Customer Feedback Survey Data Set record to the database
        /// </summary>
        /// <param name="customerFeedbackSurveyDataSetDTO">CustomerFeedbackSurveyDataSetDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        ///  <param name="sqlTrxn">sqlTransaction object</param>
        /// <returns>Returns inserted record id</returns>
        public CustomerFeedbackSurveyDataSetDTO InsertCustomerFeedbackSurveyDataSet(CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDTO, string loginId, int siteId, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetDTO, loginId, siteId);
            string query = @"insert into CustFeedbackSurveyDataset 
                                                        (
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @createdBy,
                                                        getDate(), 
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @masterEntityId
                                                        ) SELECT * FROM CustFeedbackSurveyDataset WHERE CustFbSurveyDataSetId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerFeedbackSurveyDataSetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerFeedbackSurveyDataSetDTO(customerFeedbackSurveyDataSetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerFeedbackSurveyDataSetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerFeedbackSurveyDataSetDTO);
            return customerFeedbackSurveyDataSetDTO;
        }
        /// <summary>
        /// Updates the Customer Feedback Survey Data Set record
        /// </summary>
        /// <param name="customerFeedbackSurveyDataSetDTO">CustomerFeedbackSurveyDataSetDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        ///  <param name="sqlTrxn">sqlTransaction object</param>
        /// <returns>Returns the count of updated rows</returns>
        public CustomerFeedbackSurveyDataSetDTO UpdateCustomerFeedbackSurveyDataSet(CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDTO, string loginId, int siteId, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetDTO, loginId, siteId);
            string query = @"update CustFeedbackSurveyDataset 
                                         set LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             MasterEntityId = @masterEntityId    
                                             Where CustFbSurveyDataSetId = @custFbSurveyDataSetId
                                   SELECT * FROM CustFeedbackSurveyDataset WHERE CustFbSurveyDataSetId  =  @custFbSurveyDataSetId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerFeedbackSurveyDataSetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerFeedbackSurveyDataSetDTO(customerFeedbackSurveyDataSetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerFeedbackSurveyDataSetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerFeedbackSurveyDataSetDTO);
            return customerFeedbackSurveyDataSetDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerFeedbackSurveyDataSetDTO">CustomerFeedbackSurveyDataSetDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerFeedbackSurveyDataSetDTO(CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId = Convert.ToInt32(dt.Rows[0]["CustFbSurveyDataSetId"]);
                customerFeedbackSurveyDataSetDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackSurveyDataSetDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackSurveyDataSetDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerFeedbackSurveyDataSetDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackSurveyDataSetDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackSurveyDataSetDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomerFeedbackSurveyDataSetDTO class type
        /// </summary>
        /// <param name="customerFeedbackSurveyDataSetDataRow">CustomerFeedbackSurveyDataSetDTO DataRow</param>
        /// <returns>Returns CustomerFeedbackSurveyDataSetDTO</returns>
        private CustomerFeedbackSurveyDataSetDTO GetCustomerFeedbackSurveyDataSetDTO(DataRow customerFeedbackSurveyDataSetDataRow)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetDataRow);
            CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDataObject = new CustomerFeedbackSurveyDataSetDTO(Convert.ToInt32(customerFeedbackSurveyDataSetDataRow["CustFbSurveyDataSetId"]),
                                             customerFeedbackSurveyDataSetDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(customerFeedbackSurveyDataSetDataRow["CreatedBy"]),
                                             customerFeedbackSurveyDataSetDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerFeedbackSurveyDataSetDataRow["CreationDate"]),
                                             customerFeedbackSurveyDataSetDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(customerFeedbackSurveyDataSetDataRow["LastUpdatedBy"]),
                                             customerFeedbackSurveyDataSetDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerFeedbackSurveyDataSetDataRow["LastupdatedDate"]),
                                             customerFeedbackSurveyDataSetDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(customerFeedbackSurveyDataSetDataRow["Guid"]),
                                             customerFeedbackSurveyDataSetDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(customerFeedbackSurveyDataSetDataRow["site_id"]),
                                             customerFeedbackSurveyDataSetDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(customerFeedbackSurveyDataSetDataRow["SynchStatus"]),
                                             customerFeedbackSurveyDataSetDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(customerFeedbackSurveyDataSetDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(customerFeedbackSurveyDataSetDataObject);
            return customerFeedbackSurveyDataSetDataObject;
        }

        /// <summary>
        /// Gets the Cust Feedback Survey Data Set data of passed Cust Feedback Survey Data Set id
        /// </summary>
        /// <param name="CustFbSurveyDataSetId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackSurveyDataSetDTO</returns>
        public CustomerFeedbackSurveyDataSetDTO GetCustomerFeedbackSurveyDataSet(int CustFbSurveyDataSetId)
        {
            log.LogMethodEntry(CustFbSurveyDataSetId);
            CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDataObject = null;
            string selectCustomerFeedbackSurveyDataSetQuery = SELECT_QUERY + "  where cfsd.CustFbSurveyDataSetId = @custFbSurveyDataSetId";
            SqlParameter[] selectCustomerFeedbackSurveyDataSetParameters = new SqlParameter[1];
            selectCustomerFeedbackSurveyDataSetParameters[0] = new SqlParameter("@custFbSurveyDataSetId", CustFbSurveyDataSetId);
            DataTable customerFeedbackSurveyDataSet = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyDataSetQuery, selectCustomerFeedbackSurveyDataSetParameters, sqlTransaction);
            if (customerFeedbackSurveyDataSet.Rows.Count > 0)
            {
                DataRow customerFeedbackSurveyDataSetRow = customerFeedbackSurveyDataSet.Rows[0];
                customerFeedbackSurveyDataSetDataObject = GetCustomerFeedbackSurveyDataSetDTO(customerFeedbackSurveyDataSetRow);

            }
            log.LogMethodExit(customerFeedbackSurveyDataSetDataObject);
            return customerFeedbackSurveyDataSetDataObject;
        }
        /// <summary>
        /// Retrieving Customer Feedback Survey Data Set by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retrieving the Customer Feedback Survey Data Set</param>
        /// <returns> List of CustomerFeedbackSurveyDataSetDTO </returns>
        public List<CustomerFeedbackSurveyDataSetDTO> GetCustomerFeedbackSurveyDataSetList(string sqlQuery)
        {
            log.LogMethodEntry(sqlQuery);
            string Query = sqlQuery.ToUpper();
            if (Query.Contains("DROP") || Query.Contains("UPDATE") || Query.Contains("DELETE"))
            {
                log.LogMethodExit();
                return null;
            }
            DataTable customerFeedbackSurveyDataSetData = dataAccessHandler.executeSelectQuery(sqlQuery, null);
            if (customerFeedbackSurveyDataSetData.Rows.Count > 0)
            {
                List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetList = new List<CustomerFeedbackSurveyDataSetDTO>();
                foreach (DataRow customerFeedbackSurveyDataSetDataRow in customerFeedbackSurveyDataSetData.Rows)
                {
                    CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDataObject = GetCustomerFeedbackSurveyDataSetDTO(customerFeedbackSurveyDataSetDataRow);
                    customerFeedbackSurveyDataSetList.Add(customerFeedbackSurveyDataSetDataObject);
                }
                log.LogMethodExit(customerFeedbackSurveyDataSetList);
                return customerFeedbackSurveyDataSetList; ;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the CustomerFeedbackSurveyDataSetDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackSurveyDataSetDTO matching the search criteria</returns>
        public List<CustomerFeedbackSurveyDataSetDTO> GetCustomerFeedbackSurveyDataSetList(List<KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCustomerFeedbackSurveyDataSetQuery = SELECT_QUERY;
            List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.CUST_FB_SURVEY_DATA_SET_ID)
                        || searchParameter.Key.Equals(CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.CUST_FB_SURVEY_DATA_SET_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                if (searchParameters.Count > 0)
                    selectCustomerFeedbackSurveyDataSetQuery = selectCustomerFeedbackSurveyDataSetQuery + query;
            }

            DataTable customerFeedbackSurveyDataSetData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackSurveyDataSetQuery, parameters.ToArray(), sqlTransaction);
            if (customerFeedbackSurveyDataSetData.Rows.Count > 0)
            {
                customerFeedbackSurveyDataSetList = new List<CustomerFeedbackSurveyDataSetDTO>();
                foreach (DataRow customerFeedbackSurveyDataSetDataRow in customerFeedbackSurveyDataSetData.Rows)
                {
                    CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDataObject = GetCustomerFeedbackSurveyDataSetDTO(customerFeedbackSurveyDataSetDataRow);
                    customerFeedbackSurveyDataSetList.Add(customerFeedbackSurveyDataSetDataObject);
                }
            }
            log.LogMethodExit(customerFeedbackSurveyDataSetList);
            return customerFeedbackSurveyDataSetList;

        }
    }
}
