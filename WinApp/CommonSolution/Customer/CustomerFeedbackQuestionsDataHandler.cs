/********************************************************************************************
 * Project Name - Cust Feedback Questions Data Handler
 * Description  - Data handler of the Cust Feedback Questions class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Girish Kundar       Modified : Structure of data Handler - insert /Update methods
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
    /// Customer Feedback Questions Data Handler - Handles insert, update and select of Cust Feedback Questions objects
    /// </summary>
    public class CustomerFeedbackQuestionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string passPhrase;
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private static readonly Dictionary<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string> DBSearchParameters = new Dictionary<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>
            {
                {CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID, "CFQ.CustFbQuestionId"},
                {CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID_LIST, "CFQ.CustFbQuestionId"},
                {CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_RESPONSE_ID, "CFQ.CustFbResponseId"},
                {CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.QUESTION, "CFQ.Question"},
                {CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.QUESTION_NO,"CFQ.QuestionNo"},
                {CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE, "CFQ.IsActive"},
                {CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.SITE_ID, "CFQ.site_id"}
            };


        /// <summary>
        /// Default constructor of CustomerFeedbackQuestionsDataHandler class
        /// </summary>
        public CustomerFeedbackQuestionsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerFeedbackQuestions Record.
        /// </summary>
        /// <param name="customerFeedbackQuestionsDTO">CustomerFeedbackQuestionsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerFeedbackQuestionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbQuestionId", customerFeedbackQuestionsDTO.CustFbQuestionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@custFbResponseId", customerFeedbackQuestionsDTO.CustFbResponseId, true));
            parameters.Add(new SqlParameter("@questionNo", string.IsNullOrEmpty(customerFeedbackQuestionsDTO.QuestionNo) ? string.Empty : (object)customerFeedbackQuestionsDTO.QuestionNo));
            parameters.Add(new SqlParameter("@question", string.IsNullOrEmpty(customerFeedbackQuestionsDTO.Question) ? string.Empty : (object)customerFeedbackQuestionsDTO.Question));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerFeedbackQuestionsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerFeedbackQuestionsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Customer Feedback Questions record to the database
        /// </summary>
        /// <param name="custFeedbackQuestionDTO">CustomerFeedbackQuestionsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerFeedbackQuestionsDTO</returns>
        public CustomerFeedbackQuestionsDTO InsertCustomerFeedbackQuestions(CustomerFeedbackQuestionsDTO custFeedbackQuestionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(custFeedbackQuestionDTO, loginId, siteId);
            string query = @"insert into CustFeedbackQuestions 
                                                        ( 
                                                        QuestionNo,
                                                        Question,
                                                        CustFbResponseId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        GUID,
                                                        site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                         @questionNo,
                                                         @question,
                                                         @custFbResponseId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId
                                                       
                                                        ) SELECT * FROM CustFeedbackQuestions WHERE CustFbQuestionId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackQuestionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackQuestionDTO(custFeedbackQuestionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting custFeedbackQuestionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackQuestionDTO);
            return custFeedbackQuestionDTO;
        }

        /// <summary>
        /// Updates the Cust Feedback Questions record
        /// </summary>
        /// <param name="custFeedbackQuestions">CustomerFeedbackQuestionsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns theCustomerFeedbackQuestionsDTO</returns>
        public CustomerFeedbackQuestionsDTO UpdateCustomerFeedbackQuestions(CustomerFeedbackQuestionsDTO custFeedbackQuestionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(custFeedbackQuestionDTO, loginId, siteId);
            string query = @"update CustFeedbackQuestions 
                                         set  QuestionNo=@questionNo,
                                              Question=@question,
                                              CustFbResponseId=@custFbResponseId,
                                              IsActive = @isActive, 
                                              LastUpdatedBy = @lastUpdatedBy, 
                                              LastupdatedDate = Getdate(),
                                             -- Site_id=@siteid ,
                                              MasterEntityId = @masterEntityId
                                        where CustFbQuestionId = @custFbQuestionId
                              SELECT * FROM CustFeedbackQuestions WHERE CustFbQuestionId  = @custFbQuestionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(custFeedbackQuestionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustFeedbackQuestionDTO(custFeedbackQuestionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating custFeedbackQuestionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(custFeedbackQuestionDTO);
            return custFeedbackQuestionDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerFeedbackQuestionsDTO">CustomerFeedbackQuestionsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustFeedbackQuestionDTO(CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO, DataTable dt)
        {
            log.LogMethodEntry(customerFeedbackQuestionsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerFeedbackQuestionsDTO.CustFbQuestionId = Convert.ToInt32(dt.Rows[0]["CustFbQuestionId"]);
                customerFeedbackQuestionsDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerFeedbackQuestionsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerFeedbackQuestionsDTO.Guid = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
                customerFeedbackQuestionsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerFeedbackQuestionsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerFeedbackQuestionsDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerFeedbackQuestionsDTO class type
        /// </summary>
        /// <param name="custFeedbackQuestionsDataRow">CustomerFeedbackQuestions DataRow</param>
        /// <returns>Returns CustomerFeedbackQuestions</returns>
        private CustomerFeedbackQuestionsDTO GetCustomerFeedbackQuestionsDTO(DataRow custFeedbackQuestionsDataRow)
        {
            log.LogMethodEntry(custFeedbackQuestionsDataRow);
            CustomerFeedbackQuestionsDTO custFeedbackQuestionsDataObject = new CustomerFeedbackQuestionsDTO(Convert.ToInt32(custFeedbackQuestionsDataRow["CustFbQuestionId"]),
                                            custFeedbackQuestionsDataRow["QuestionNo"].ToString(),
                                            custFeedbackQuestionsDataRow["Question"].ToString(),
                                            custFeedbackQuestionsDataRow["CustFbResponseId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackQuestionsDataRow["CustFbResponseId"]),
                                            custFeedbackQuestionsDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackQuestionsDataRow["IsActive"]),
                                            custFeedbackQuestionsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackQuestionsDataRow["CreatedBy"]),
                                            custFeedbackQuestionsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackQuestionsDataRow["CreationDate"]),
                                            custFeedbackQuestionsDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackQuestionsDataRow["LastUpdatedBy"]),
                                            custFeedbackQuestionsDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(custFeedbackQuestionsDataRow["LastupdatedDate"]),
                                            custFeedbackQuestionsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackQuestionsDataRow["site_id"]),
                                            custFeedbackQuestionsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(custFeedbackQuestionsDataRow["Guid"]),
                                            custFeedbackQuestionsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(custFeedbackQuestionsDataRow["SynchStatus"]),
                                            custFeedbackQuestionsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(custFeedbackQuestionsDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(custFeedbackQuestionsDataObject);
            return custFeedbackQuestionsDataObject;
        }

        /// <summary>
        /// Gets the Customer Feedback Questions data of passed Cust Fb QuestionId
        /// </summary>
        /// <param name="CustFbQuestionId">integer type parameter</param>
        /// <returns>Returns CustomerFeedbackQuestionsDTO</returns>
        public CustomerFeedbackQuestionsDTO GetCustomerFeedbackQuestions(int CustFbQuestionId, int LanguageId)
        {
            log.LogMethodEntry(CustFbQuestionId, LanguageId);
            CustomerFeedbackQuestionsDTO custFeedbackQuestionsDataObject = null;
            string selectCustomerFeedbackQuestionsQuery = @"Select CustFbQuestionId,QuestionNo,isnull( OTLS.Translation, Question) Question,CustFbResponseId,CFQ.IsActive,
                                                                    CFQ.CreatedBy,CFQ.CreationDate,CFQ.LastUpdatedBy,CFQ.LastupdatedDate,CFQ.Site_id,CFQ.GUID,CFQ.SynchStatus,CFQ.MasterEntityId 
                                                            FROM CustFeedbackQuestions CFQ Left Join  ObjectTranslations OTLS  on CFQ.GUID=OTLS.ElementGuid and OTLS.LanguageId=@LanguageId
                                             WHERE CFQ.CustFbQuestionId = @CustFbQuestionId";
            SqlParameter[] selectCustomerFeedbackQuestionsParameters = new SqlParameter[2];
            selectCustomerFeedbackQuestionsParameters[0] = new SqlParameter("@CustFbQuestionId", CustFbQuestionId);
            selectCustomerFeedbackQuestionsParameters[1] = new SqlParameter("@LanguageId", LanguageId);
            DataTable custFeedbackQuestions = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackQuestionsQuery, selectCustomerFeedbackQuestionsParameters, sqlTransaction);
            if (custFeedbackQuestions.Rows.Count > 0)
            {
                DataRow CustomerFeedbackQuestionsRow = custFeedbackQuestions.Rows[0];
                custFeedbackQuestionsDataObject = GetCustomerFeedbackQuestionsDTO(CustomerFeedbackQuestionsRow);
            }
            log.LogMethodExit(custFeedbackQuestionsDataObject);
            return custFeedbackQuestionsDataObject;
        }

        /// <summary>
        /// Gets the CustomerFeedbackQuestionsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerFeedbackQuestionsDTO matching the search criteria</returns>
        public List<CustomerFeedbackQuestionsDTO> GetCustomerFeedbackQuestionsList(List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchParameters, int LanguageId)
        {
            log.LogMethodEntry(searchParameters, LanguageId);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CustomerFeedbackQuestionsDTO> custFeedbackQuestionsList = null;
            string selectCustomerFeedbackQuestionsQuery = @"Select CustFbQuestionId,QuestionNo,isnull( OTLS.Translation, Question) Question,CustFbResponseId,CFQ.IsActive,
                                                                    CFQ.CreatedBy,CFQ.CreationDate,CFQ.LastUpdatedBy,CFQ.LastupdatedDate,CFQ.Site_id,CFQ.GUID,CFQ.SynchStatus,CFQ.MasterEntityId 
                                                            FROM CustFeedbackQuestions CFQ Left Join  ObjectTranslations OTLS  on CFQ.GUID=OTLS.ElementGuid and OTLS.LanguageId=@LanguageId ";
            parameters.Add(new SqlParameter("@LanguageId", LanguageId));
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID
                            || searchParameter.Key == CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_RESPONSE_ID
                            || searchParameter.Key == CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.QUESTION_NO
                            || searchParameter.Key == CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.QUESTION)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID_LIST)
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
                    selectCustomerFeedbackQuestionsQuery = selectCustomerFeedbackQuestionsQuery + query;
            }
            DataTable custFeedbackQuestionsData = dataAccessHandler.executeSelectQuery(selectCustomerFeedbackQuestionsQuery, parameters.ToArray(), sqlTransaction);
            if (custFeedbackQuestionsData.Rows.Count > 0)
            {
                custFeedbackQuestionsList = new List<CustomerFeedbackQuestionsDTO>();
                foreach (DataRow custFeedbackQuestionsDataRow in custFeedbackQuestionsData.Rows)
                {
                    CustomerFeedbackQuestionsDTO custFeedbackQuestionsDataObject = GetCustomerFeedbackQuestionsDTO(custFeedbackQuestionsDataRow);
                    custFeedbackQuestionsList.Add(custFeedbackQuestionsDataObject);
                }
            }
            log.LogMethodExit(custFeedbackQuestionsList);
            return custFeedbackQuestionsList;

        }
    }
}
