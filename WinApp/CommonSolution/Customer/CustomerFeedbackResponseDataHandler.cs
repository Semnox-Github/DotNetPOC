/********************************************************************************************
 * Project Name - Cust Feedback Response Data Handler
 * Description  - Data handler of the Cust Feedback Response class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Girish Kundar      Modified : Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
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
    /// Cust Feedback Response Data Handler - Handles insert, update and select of Cust Feedback Response objects
    /// </summary>
    public class CustomerFeedbackResponseDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string passPhrase;
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustFeedbackResponse AS cfr";
        private static readonly Dictionary<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>
            {

                {CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID, "cfr.CustFbResponseId"},
                {CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID_LIST, "cfr.CustFbResponseId"},
                {CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.RESPONSE_TYPE_ID, "cfr.ResponseTypeId"},
                {CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.RESPONSE_NAME, "cfr.ResponseName"},
                {CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, "cfr.IsActive"},
                {CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.MASTER_ENTITY_ID, "cfr.MasterEntityId"},
                {CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID, "cfr.Site_id"}
            };

        /// <summary>
        /// Default constructor of CustomerFeedbackResponseDataHandler class
        /// </summary>
        public CustomerFeedbackResponseDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackResponse Record.
        /// </summary>
        /// <param name="customerFeedbackResponseDTO">CustomerFeedbackResponseDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackResponseDTO customerFeedbackResponseDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackResponseDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbResponseId", customerFeedbackResponseDTO.CustFbResponseId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@responseTypeId", customerFeedbackResponseDTO.ResponseTypeId, true));
            parameters.Add(new SqlParameter("@responseName", string.IsNullOrEmpty(customerFeedbackResponseDTO.ResponseName) ? string.Empty : (object)customerFeedbackResponseDTO.ResponseName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerFeedbackResponseDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackResponseDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Cust Feedback Response record to the database
        /// </summary>
        /// <param name="custFeedbackResponse">CustomerFeedbackResponseDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackResponseDTO</returns>
        public CustomerFeedbackResponseDTO InsertCustomerFeedbackResponse(CustomerFeedbackResponseDTO custFeedbackResponse, string loginId, int siteId)
        {
            log.LogMethodEntry(custFeedbackResponse, loginId, siteId);
            string query = @"insert into CustFeedbackResponse 
                                                        ( 
                                                        ResponseName,
                                                        ResponseTypeId,
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
                                                         @responseName,
                                                         @responseTypeId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                        ) SELECT * FROM CustFeedbackResponse WHERE CustFbResponseId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackResponse, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackResponse(custFeedbackResponse, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting custFeedbackResponse", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackResponse);
            return custFeedbackResponse;

        }

        /// <summary>
        /// Updates the Cust Feedback Response record
        /// </summary>
        /// <param name="custFeedbackResponse">CustomerFeedbackResponseDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackResponseDTO</returns>
        public CustomerFeedbackResponseDTO UpdateCustomerFeedbackResponse(CustomerFeedbackResponseDTO custFeedbackResponse, string loginId, int siteId)
        {
            log.LogMethodEntry(custFeedbackResponse, loginId, siteId);
            string query = @"update CustFeedbackResponse 
                                         set ResponseName=@responseName,
                                             ResponseTypeId=@responseTypeId,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --Site_id=@siteid,
                                             MasterEntityId = @masterEntityId                                             
                                       where CustFbResponseId = @custFbResponseId 
                                       SELECT * FROM CustFeedbackResponse WHERE CustFbResponseId  =  @custFbResponseId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackResponse, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackResponse(custFeedbackResponse, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating custFeedbackResponse", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackResponse);
            return custFeedbackResponse;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerFeedbackResponseDTO">CustomerFeedbackResponseDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustFeedbackResponse(CustomerFeedbackResponseDTO customerFeedbackResponseDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackResponseDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackResponseDTO.CustFbResponseId = Convert.ToInt32(dt.Rows[0]["CustFbResponseId"]);
                customerFeedbackResponseDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackResponseDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackResponseDTO.Guid = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
                customerFeedbackResponseDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackResponseDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackResponseDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerFeedbackResponseDTO class type
        /// </summary>
        /// <param name="custFeedbackResponseDataRow">CustomerFeedbackResponse DataRow</param>
        /// <returns>Returns CustomerFeedbackResponse</returns>
        private CustomerFeedbackResponseDTO GetCustomerFeedbackResponseDTO(DataRow custFeedbackResponseDataRow)
        {
            log.LogMethodEntry(custFeedbackResponseDataRow);
            CustomerFeedbackResponseDTO custFeedbackResponseDataObject = new CustomerFeedbackResponseDTO(Convert.ToInt32(custFeedbackResponseDataRow["CustFbResponseId"]),
                                            custFeedbackResponseDataRow["ResponseName"].ToString(),
                                            custFeedbackResponseDataRow["ResponseTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackResponseDataRow["ResponseTypeId"]),
                                            custFeedbackResponseDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackResponseDataRow["IsActive"]),
                                            custFeedbackResponseDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackResponseDataRow["CreatedBy"]),
                                            custFeedbackResponseDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackResponseDataRow["CreationDate"]),
                                            custFeedbackResponseDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackResponseDataRow["LastUpdatedBy"]),
                                            custFeedbackResponseDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackResponseDataRow["LastupdatedDate"]),
                                            custFeedbackResponseDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackResponseDataRow["Site_id"]),
                                            custFeedbackResponseDataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackResponseDataRow["GUID"]),
                                            custFeedbackResponseDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackResponseDataRow["SynchStatus"]),
                                            custFeedbackResponseDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackResponseDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(custFeedbackResponseDataObject);
            return custFeedbackResponseDataObject;
        }

        /// <summary>
        /// Gets the Cust Feedback Response data of passed Cust Fb QuestionId
        /// </summary>
        /// <param name="CustFbResponseId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackResponseDTO</returns>
        public CustomerFeedbackResponseDTO GetCustomerFeedbackResponse(int CustFbResponseId)
        {
            log.LogMethodEntry(CustFbResponseId);
            CustomerFeedbackResponseDTO custFeedbackResponseDataObject = null;
            string selectCustomerFeedbackResponseQuery = SELECT_QUERY + "  WHERE cfr.CustFbResponseId = @CustFbResponseId";
            SqlParameter[] selectCustomerFeedbackResponseParameters = new SqlParameter[1];
            selectCustomerFeedbackResponseParameters[0] = new SqlParameter("@CustFbResponseId", CustFbResponseId);
            DataTable custFeedbackResponse = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackResponseQuery, selectCustomerFeedbackResponseParameters, sqlTransaction);
            if (custFeedbackResponse.Rows.Count > 0)
            {
                DataRow CustomerFeedbackResponseRow = custFeedbackResponse.Rows[0];
                custFeedbackResponseDataObject = GetCustomerFeedbackResponseDTO(CustomerFeedbackResponseRow);

            }
            log.LogMethodExit(custFeedbackResponseDataObject);
            return custFeedbackResponseDataObject;

        }

        /// <summary>
        /// Gets the CustomerFeedbackResponseDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackResponseDTO matching the search criteria</returns>
        public List<CustomerFeedbackResponseDTO> GetCustomerFeedbackResponseList(List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchParameters
                                                  , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CustomerFeedbackResponseDTO> custFeedbackResponseList = null;
            string selectCustomerFeedbackResponseQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : "  and ";
                        if (searchParameter.Key == CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID
                            || searchParameter.Key == CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.RESPONSE_TYPE_ID
                            || searchParameter.Key == CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.RESPONSE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE)
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
                    selectCustomerFeedbackResponseQuery = selectCustomerFeedbackResponseQuery + query;
            }
            DataTable custFeedbackResponseData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackResponseQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackResponseData.Rows.Count > 0)
            {
                custFeedbackResponseList = new List<CustomerFeedbackResponseDTO>();
                foreach (DataRow custFeedbackResponseDataRow in custFeedbackResponseData.Rows)
                {
                    CustomerFeedbackResponseDTO custFeedbackResponseDataObject = GetCustomerFeedbackResponseDTO(custFeedbackResponseDataRow);
                    custFeedbackResponseList.Add(custFeedbackResponseDataObject);
                }
            }
            log.LogMethodExit(custFeedbackResponseList);
            return custFeedbackResponseList;

        }
    }
}
