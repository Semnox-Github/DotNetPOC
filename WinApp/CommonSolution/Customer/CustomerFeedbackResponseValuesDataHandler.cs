/********************************************************************************************
 * Project Name - Cust Feedback Response Values Data Handler
 * Description  - Data handler of the Cust Feedback Response Values class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
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
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{

    /// <summary>
    /// Cust Feedback Response Values Data Handler - Handles insert, update and select of Cust Feedback ResponseValues objects
    /// </summary>
    public class CustomerFeedbackResponseValuesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustFeedbackResponseValues AS crv";
        private static readonly Dictionary<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>
            {
                {CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_VALUE_ID, "crv.CustFbResponseValueId"},
                {CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID, "crv.CustFbResponseId"},
                {CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID_LIST, "crv.CustFbResponseId"},
                {CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.RESPONSE_VALUE, "crv.ResponseValue"},
                {CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE, "crv.IsActive"},
                {CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.MASTER_ENTITY_ID, "crv.MasterEntityId"},
                {CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.SITE_ID, "crv.site_id"}
            };

        /// <summary>
        /// Default constructor of CustomerFeedbackResponseValuesDataHandler class
        /// </summary>
        public CustomerFeedbackResponseValuesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackResponseValues Record.
        /// </summary>
        /// <param name="customerFeedbackResponseValuesDTO">CustomerFeedbackResponseValuesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackResponseValuesDTO customerFeedbackResponseValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackResponseValuesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbResponseValueId", customerFeedbackResponseValuesDTO.CustFbResponseValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbResponseId", customerFeedbackResponseValuesDTO.CustFbResponseId, true));
            parameters.Add(new SqlParameter("@responseValue", string.IsNullOrEmpty(customerFeedbackResponseValuesDTO.ResponseValue) ? string.Empty : (object)customerFeedbackResponseValuesDTO.ResponseValue));
            SqlParameter parameter = new SqlParameter("@image", SqlDbType.VarBinary);
            if (customerFeedbackResponseValuesDTO.Image == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = customerFeedbackResponseValuesDTO.Image;
            }
            parameters.Add(parameter);
            parameters.Add(dataAccessHandler.GetSQLParameter("@score", customerFeedbackResponseValuesDTO.Score));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sortOrder", customerFeedbackResponseValuesDTO.SortOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerFeedbackResponseValuesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackResponseValuesDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Customer Feedback ResponseValues record to the database
        /// </summary>
        /// <param name="custFeedbackResponseValues">CustomerFeedbackResponseValuesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackResponseValuesDTO</returns>
        public CustomerFeedbackResponseValuesDTO InsertCustomerFeedbackResponseValues(CustomerFeedbackResponseValuesDTO custFeedbackResponseValues, string loginId, int siteId)
        {
            log.LogMethodEntry(custFeedbackResponseValues, loginId, siteId);
            string query = @"insert into CustFeedbackResponseValues 
                                                        ( 
                                                        CustFbResponseId,
                                                        ResponseValue,
                                                        Image,
                                                        Score,
                                                        SortOrder,
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
                                                         @custFbResponseId,
                                                         @responseValue,
                                                         @image,
                                                         @score,
                                                         @sortOrder,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                        ) SELECT * FROM CustFeedbackResponseValues WHERE CustFbResponseValueId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackResponseValues, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackResponseValues(custFeedbackResponseValues, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting custFeedbackResponseValues", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackResponseValues);
            return custFeedbackResponseValues;
        }

        /// <summary>
        /// Updates the Cust Feedback ResponseValues record
        /// </summary>
        /// <param name="custFeedbackResponseValues">CustomerFeedbackResponseValuesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackResponseValuesDTO</returns>
        public CustomerFeedbackResponseValuesDTO UpdateCustomerFeedbackResponseValues(CustomerFeedbackResponseValuesDTO custFeedbackResponseValues, string loginId, int siteId)
        {
            log.LogMethodEntry(custFeedbackResponseValues, loginId, siteId);
            string query = @"update CustFeedbackResponseValues 
                                         set  CustFbResponseId = @custFbResponseId,
                                              ResponseValue = @responseValue,
                                              Image = @image,
                                              Score = @score,
                                              SortOrder = @sortOrder,
                                              IsActive = @isActive, 
                                              LastUpdatedBy = @lastUpdatedBy, 
                                              LastupdatedDate = Getdate()
                                              --site_id = @siteid
                                       where CustFbResponseValueId = @custFbResponseValueId
                           SELECT * FROM CustFeedbackResponseValues WHERE CustFbResponseValueId  = @custFbResponseValueId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackResponseValues, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackResponseValues(custFeedbackResponseValues, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating custFeedbackResponseValues", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackResponseValues);
            return custFeedbackResponseValues;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerFeedbackResponseValuesDTO">CustomerFeedbackResponseValuesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustFeedbackResponseValues(CustomerFeedbackResponseValuesDTO customerFeedbackResponseValuesDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackResponseValuesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackResponseValuesDTO.CustFbResponseValueId = Convert.ToInt32(dt.Rows[0]["CustFbResponseValueId"]);
                customerFeedbackResponseValuesDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackResponseValuesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackResponseValuesDTO.Guid = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
                customerFeedbackResponseValuesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackResponseValuesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackResponseValuesDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerFeedbackResponseValuesDTO class type
        /// </summary>
        /// <param name="custFeedbackResponseValuesDataRow">CustomerFeedbackResponseValues DataRow</param>
        /// <returns>Returns CustomerFeedbackResponseValues</returns>
        private CustomerFeedbackResponseValuesDTO GetCustomerFeedbackResponseValuesDTO(DataRow custFeedbackResponseValuesDataRow)
        {
            log.LogMethodEntry(custFeedbackResponseValuesDataRow);
            CustomerFeedbackResponseValuesDTO custFeedbackResponseValuesDataObject = new CustomerFeedbackResponseValuesDTO(Convert.ToInt32(custFeedbackResponseValuesDataRow["CustFbResponseValueId"]),
                                            custFeedbackResponseValuesDataRow["ResponseValue"].ToString(),
                                            custFeedbackResponseValuesDataRow["CustFbResponseId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackResponseValuesDataRow["CustFbResponseId"]),
                                            custFeedbackResponseValuesDataRow["Image"] == DBNull.Value ? null : (byte[])custFeedbackResponseValuesDataRow["Image"],
                                            custFeedbackResponseValuesDataRow["Score"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(custFeedbackResponseValuesDataRow["Score"]),
                                            custFeedbackResponseValuesDataRow["SortOrder"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackResponseValuesDataRow["SortOrder"]),
                                            custFeedbackResponseValuesDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackResponseValuesDataRow["IsActive"]),
                                            custFeedbackResponseValuesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackResponseValuesDataRow["CreatedBy"]),
                                            custFeedbackResponseValuesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackResponseValuesDataRow["CreationDate"]),
                                            custFeedbackResponseValuesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackResponseValuesDataRow["LastUpdatedBy"]),
                                            custFeedbackResponseValuesDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackResponseValuesDataRow["LastupdatedDate"]),
                                            custFeedbackResponseValuesDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackResponseValuesDataRow["Site_id"]),
                                            custFeedbackResponseValuesDataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackResponseValuesDataRow["GUID"]),
                                            custFeedbackResponseValuesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackResponseValuesDataRow["SynchStatus"]),
                                            custFeedbackResponseValuesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackResponseValuesDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(custFeedbackResponseValuesDataObject);
            return custFeedbackResponseValuesDataObject;
        }

        /// <summary>
        /// Gets the Cust Feedback ResponseValues data of passed Customer Fb QuestionId
        /// </summary>
        /// <param name="CustFbResponseValueId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackResponseValuesDTO</returns>
        public CustomerFeedbackResponseValuesDTO GetCustomerFeedbackResponseValues(int CustFbResponseValueId, int LanguageId)
        {
            log.LogMethodEntry(CustFbResponseValueId, LanguageId);
            CustomerFeedbackResponseValuesDTO custFeedbackResponseValuesDataObject = null;
            string selectCustomerFeedbackResponseValuesQuery = @"SELECT CustFbResponseValueId,CustFbResponseId,isnull( OTLS.Translation,ResponseValue) ResponseValue,Image,CRV.IsActive,CRV.CreatedBy,CRV.CreationDate,CRV.LastUpdatedBy,CRV.LastupdatedDate,CRV.Site_id,CRV.GUID,CRV.SynchStatus,CRV.MasterEntityId
                                                                FROM CustFeedbackResponseValues CRV Left Join  ObjectTranslations OTLS  on CRV.GUID=OTLS.ElementGuid and OTLS.LanguageId=@LanguageId 
                                             WHERE CustFbResponseValueId = @CustFbResponseValueId";
            SqlParameter[] selectCustomerFeedbackResponseValuesParameters = new SqlParameter[2];
            selectCustomerFeedbackResponseValuesParameters[0] = new SqlParameter("@CustFbResponseValueId", CustFbResponseValueId);
            selectCustomerFeedbackResponseValuesParameters[1] = new SqlParameter("@LanguageId", LanguageId);
            DataTable custFeedbackResponseValues = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackResponseValuesQuery, selectCustomerFeedbackResponseValuesParameters, sqlTransaction);
            if (custFeedbackResponseValues.Rows.Count > 0)
            {
                DataRow CustomerFeedbackResponseValuesRow = custFeedbackResponseValues.Rows[0];
                custFeedbackResponseValuesDataObject = GetCustomerFeedbackResponseValuesDTO(CustomerFeedbackResponseValuesRow);

            }
            log.LogMethodExit(custFeedbackResponseValuesDataObject);
            return custFeedbackResponseValuesDataObject;

        }

        /// <summary>
        /// Gets the CustomerFeedbackResponseValuesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackResponseValuesDTO matching the search criteria</returns>
        public List<CustomerFeedbackResponseValuesDTO> GetCustomerFeedbackResponseValuesList(List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCustomerFeedbackResponseValuesQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CustomerFeedbackResponseValuesDTO> custFeedbackResponseValuesList = null;
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID
                            || searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_VALUE_ID
                            || searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.RESPONSE_VALUE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.SITE_ID)
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
                    selectCustomerFeedbackResponseValuesQuery = selectCustomerFeedbackResponseValuesQuery + query;
            }
            DataTable custFeedbackResponseValuesData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackResponseValuesQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackResponseValuesData.Rows.Count > 0)
            {
                custFeedbackResponseValuesList = new List<CustomerFeedbackResponseValuesDTO>();
                foreach (DataRow custFeedbackResponseValuesDataRow in custFeedbackResponseValuesData.Rows)
                {
                    CustomerFeedbackResponseValuesDTO custFeedbackResponseValuesDataObject = GetCustomerFeedbackResponseValuesDTO(custFeedbackResponseValuesDataRow);
                    custFeedbackResponseValuesList.Add(custFeedbackResponseValuesDataObject);
                }
            }
            log.LogMethodExit(custFeedbackResponseValuesList);
            return custFeedbackResponseValuesList;
        }

        /// <summary>
        /// Gets the CustomerFeedbackResponseValuesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackResponseValuesDTO matching the search criteria</returns>
        public List<CustomerFeedbackResponseValuesDTO> GetCustomerFeedbackResponseValuesList(List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchParameters, int LanguageId)
        {
            log.LogMethodEntry(searchParameters, LanguageId);
            int count = 0;
            string selectCustomerFeedbackResponseValuesQuery = @"SELECT CustFbResponseValueId,CustFbResponseId,isnull( OTLS.Translation,ResponseValue) ResponseValue,Image,CRV.IsActive,CRV.CreatedBy,CRV.CreationDate,CRV.LastUpdatedBy,CRV.LastupdatedDate,CRV.Site_id,CRV.GUID,CRV.SynchStatus,CRV.MasterEntityId
                                                                FROM CustFeedbackResponseValues CRV Left Join  ObjectTranslations OTLS  on CRV.GUID=OTLS.ElementGuid and OTLS.LanguageId=@LanguageId ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CustomerFeedbackResponseValuesDTO> custFeedbackResponseValuesList = null;
            parameters.Add(new SqlParameter("@LanguageId", LanguageId));
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID
                            || searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_VALUE_ID
                            || searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE
                            || searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.RESPONSE_VALUE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.SITE_ID)
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
                    selectCustomerFeedbackResponseValuesQuery = selectCustomerFeedbackResponseValuesQuery + query;
            }
            DataTable custFeedbackResponseValuesData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackResponseValuesQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackResponseValuesData.Rows.Count > 0)
            {
                custFeedbackResponseValuesList = new List<CustomerFeedbackResponseValuesDTO>();
                foreach (DataRow custFeedbackResponseValuesDataRow in custFeedbackResponseValuesData.Rows)
                {
                    CustomerFeedbackResponseValuesDTO custFeedbackResponseValuesDataObject = GetCustomerFeedbackResponseValuesDTO(custFeedbackResponseValuesDataRow);
                    custFeedbackResponseValuesList.Add(custFeedbackResponseValuesDataObject);
                }
            }
            log.LogMethodExit(custFeedbackResponseValuesList);
            return custFeedbackResponseValuesList;
        }
    }
}
