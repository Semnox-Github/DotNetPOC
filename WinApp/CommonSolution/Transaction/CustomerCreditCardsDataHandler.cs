/********************************************************************************************
 * Project Name - CustomerCreditCards Data Handler
 * Description  - Data handler of the CustomerCreditCards 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Guru S A           Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A           For Subscription phase one changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text; 

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// CustomerCreditCardsDataHandler
    /// </summary>
    public class CustomerCreditCardsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string passPhrase;
        private static readonly Dictionary<CustomerCreditCardsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerCreditCardsDTO.SearchByParameters, string>
            {
                {CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_CREDITCARDS_ID, "ccc.CustomerCreditCardsId"},
                {CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_ID, "ccc.CustomerId"},
                {CustomerCreditCardsDTO.SearchByParameters.CARD_PROFILE_ID, "ccc.CardProfileId"},
                {CustomerCreditCardsDTO.SearchByParameters.TOKEN_ID, "CCC.TokenId"}, 
                {CustomerCreditCardsDTO.SearchByParameters.IS_ACTIVE, "ccc.IsActive"},
                {CustomerCreditCardsDTO.SearchByParameters.MASTER_ENTITY_ID,"ccc.MasterEntityId"}, 
                {CustomerCreditCardsDTO.SearchByParameters.SITE_ID, "ccc.site_id"} ,
                {CustomerCreditCardsDTO.SearchByParameters.EXPIREDCARD_LINKED_WITH_UNBILLED_SUBSCRIPTIONS, "ccc.CustomerCreditCardsId"},
                {CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_BEFORE_NEXT_UNBILLED_CYCLE, "ccc.CustomerCreditCardsId"},
                {CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_IN_X_DAYS, ""},
                {CustomerCreditCardsDTO.SearchByParameters.LINKED_WITH_ACTIVE_SUBSCRIPTIONS, "ccc.CustomerCreditCardsId"},
                {CustomerCreditCardsDTO.SearchByParameters.PAYMENT_MODE_ID, "ccc.PaymentModeId"} 
            };
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT CustomerCreditCardsId
                                                      ,CustomerId
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CardProfileId)) AS CardProfileId  
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,TokenId)) AS TokenId
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CustomerNameOnCard)) AS CustomerNameOnCard   
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CreditCardNumber)) AS CreditCardNumber  
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CardExpiry)) AS CardExpiry 	 
                                                      ,IsActive
                                                      ,CreatedBy
                                                      ,CreationDate
                                                      ,LastUpdatedBy
                                                      ,LastUpdatedDate
                                                      ,Guid
                                                      ,SynchStatus
                                                      ,site_id
                                                      ,MasterEntityId
                                                      ,PaymentModeId
                                                      ,LastCreditCardExpiryReminderSentOn
                                                      ,CreditCardExpiryReminderCount
                                                 FROM dbo.CustomerCreditCards  AS ccc ";
        /// <summary>
        /// Default constructor of CustomerCreditCardsDataHandler class
        /// </summary>
        public CustomerCreditCardsDataHandler(string passPhrase, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry("passPhrase", sqlTransaction);
            this.passPhrase = passPhrase;
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
         
        private List<SqlParameter> BuildSQLParameters(CustomerCreditCardsDTO customerCreditCardsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(customerCreditCardsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCreditCardsId", customerCreditCardsDTO.CustomerCreditCardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", customerCreditCardsDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardProfileId", customerCreditCardsDTO.CardProfileId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentModeId", customerCreditCardsDTO.PaymentModeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TokenId", customerCreditCardsDTO.TokenId));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@CustomerNameOnCard", customerCreditCardsDTO.CustomerNameOnCard));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@CreditCardNumber", ((string.IsNullOrEmpty(customerCreditCardsDTO.CreditCardNumber) ? customerCreditCardsDTO.CreditCardNumber
                                                                             : (new String('X', 12) + ((customerCreditCardsDTO.CreditCardNumber.Length > 4) ? customerCreditCardsDTO.CreditCardNumber.Substring(customerCreditCardsDTO.CreditCardNumber.Length - 4)
                                                                                                                         : customerCreditCardsDTO.CreditCardNumber))))));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@CardExpiry", customerCreditCardsDTO.CardExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastCreditCardExpiryReminderSentOn", customerCreditCardsDTO.LastCreditCardExpiryReminderSentOn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditCardExpiryReminderCount", customerCreditCardsDTO.CreditCardExpiryReminderCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerCreditCardsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerCreditCardsDTO.MasterEntityId, true)); 
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomerCreditCards record to the database
        /// </summary>
        /// <param name="customerCreditCardsDTO">CustomerCreditCardsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted CustomerCreditCards record</returns>
        public CustomerCreditCardsDTO InsertCustomerCreditCards(CustomerCreditCardsDTO customerCreditCardsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(customerCreditCardsDTO, userId, siteId);
            string query = @"INSERT INTO CustomerCreditCards 
                                        ( 
                                            CustomerId,
                                            CardProfileId,
                                            PaymentModeId,
	                                        TokenId,
	                                        CustomerNameOnCard,
	                                        CreditCardNumber,
	                                        CardExpiry,
                                            LastCreditCardExpiryReminderSentOn,
                                            CreditCardExpiryReminderCount,
	                                        IsActive,
	                                        CreatedBy,
	                                        CreationDate,
	                                        LastUpdatedBy,
	                                        LastUpdatedDate,
	                                        Guid, 
	                                        site_id,
	                                        MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                           @CustomerId,
	                                       ENCRYPTBYPASSPHRASE(@PassPhrase, @CardProfileId), 
                                           @PaymentModeId,
	                                       ENCRYPTBYPASSPHRASE(@PassPhrase, @TokenId), 
	                                       ENCRYPTBYPASSPHRASE(@PassPhrase, @CustomerNameOnCard), 
	                                       ENCRYPTBYPASSPHRASE(@PassPhrase, @CreditCardNumber), 
	                                       ENCRYPTBYPASSPHRASE(@PassPhrase, @CardExpiry),    
                                           @LastCreditCardExpiryReminderSentOn,
                                           @CreditCardExpiryReminderCount,
	                                       @IsActive,
	                                       @CreatedBy,
	                                       GETDATE(),
	                                       @LastUpdatedBy,
	                                       GETDATE(),
	                                       NEWID(),
	                                       @site_id,
	                                       @MasterEntityId
                                        )
                                        SELECT  CustomerCreditCardsId
                                                      ,CustomerId
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CardProfileId)) AS CardProfileId  
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,TokenId)) AS TokenId
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CustomerNameOnCard)) AS CustomerNameOnCard   
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CreditCardNumber)) AS CreditCardNumber  
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CardExpiry)) AS CardExpiry 	 
                                                      ,IsActive
                                                      ,CreatedBy
                                                      ,CreationDate
                                                      ,LastUpdatedBy
                                                      ,LastUpdatedDate
                                                      ,Guid
                                                      ,SynchStatus
                                                      ,site_id
                                                      ,MasterEntityId
                                                      ,PaymentModeId
                                                      ,LastCreditCardExpiryReminderSentOn
                                                      ,CreditCardExpiryReminderCount FROM CustomerCreditCards WHERE CustomerCreditCardsId = scope_identity()";


            List<SqlParameter> parameters = BuildSQLParameters(customerCreditCardsDTO, userId, siteId);
            try
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshCustomerCreditCardsDTO(customerCreditCardsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting the Customer Credit Cards ", ex);
                log.LogVariableState("CustomerCreditCardsDTO", customerCreditCardsDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(customerCreditCardsDTO);
            return customerCreditCardsDTO;
        }

        /// <summary>
        /// Updates the CustomerCreditCards record
        /// </summary>
        /// <param name="customerCreditCardsDTO">CustomerCreditCardsDTO type parameter</param>
        /// <param name="userId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public CustomerCreditCardsDTO UpdateCustomerCreditCards(CustomerCreditCardsDTO customerCreditCardsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(customerCreditCardsDTO, userId, siteId);
            string query = @"UPDATE CustomerCreditCards 
                             SET CustomerId = @CustomerId,
                                 CardProfileId = ENCRYPTBYPASSPHRASE(@PassPhrase, @CardProfileId), 
                                 PaymentModeId = @PaymentModeId,
	                             TokenId = ENCRYPTBYPASSPHRASE(@PassPhrase, @TokenId), 
	                             CustomerNameOnCard =ENCRYPTBYPASSPHRASE(@PassPhrase, @CustomerNameOnCard), 
	                             CreditCardNumber =ENCRYPTBYPASSPHRASE(@PassPhrase, @CreditCardNumber), 
	                             CardExpiry =ENCRYPTBYPASSPHRASE(@PassPhrase, @CardExpiry), 
                                 LastCreditCardExpiryReminderSentOn = @LastCreditCardExpiryReminderSentOn,
                                 CreditCardExpiryReminderCount = @CreditCardExpiryReminderCount,
	                             IsActive = @IsActive, 
	                             LastUpdatedBy = @LastUpdatedBy,
	                             LastUpdatedDate = GETDATE(), 
	                             --site_id = @site_id,
	                             MasterEntityId = @MasterEntityId
                             WHERE CustomerCreditCardsId = @CustomerCreditCardsId
                             SELECT  CustomerCreditCardsId
                                                      ,CustomerId
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CardProfileId)) AS CardProfileId  
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,TokenId)) AS TokenId
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CustomerNameOnCard)) AS CustomerNameOnCard   
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CreditCardNumber)) AS CreditCardNumber  
                                                      ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,CardExpiry)) AS CardExpiry 	 
                                                      ,IsActive
                                                      ,CreatedBy
                                                      ,CreationDate
                                                      ,LastUpdatedBy
                                                      ,LastUpdatedDate
                                                      ,Guid
                                                      ,SynchStatus
                                                      ,site_id
                                                      ,MasterEntityId
                                                      ,PaymentModeId
                                                      ,LastCreditCardExpiryReminderSentOn
                                                      ,CreditCardExpiryReminderCount FROM CustomerCreditCards WHERE CustomerCreditCardsId = @CustomerCreditCardsId";
            List<SqlParameter> parameters = BuildSQLParameters(customerCreditCardsDTO, userId, siteId);
            try
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshCustomerCreditCardsDTO(customerCreditCardsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating the Customer Credit Cards ", ex);
                log.LogVariableState("CustomerCreditCardsDTO", customerCreditCardsDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(customerCreditCardsDTO);
            return customerCreditCardsDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerCreditCardsDTO">CustomerCreditCardsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerCreditCardsDTO(CustomerCreditCardsDTO customerCreditCardsDTO, DataTable dt)
        {
            log.LogMethodEntry(customerCreditCardsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerCreditCardsDTO.CustomerCreditCardsId = Convert.ToInt32(dt.Rows[0]["CustomerCreditCardsId"]);
                customerCreditCardsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerCreditCardsDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                customerCreditCardsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                customerCreditCardsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                customerCreditCardsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                customerCreditCardsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]); 
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomerCreditCardsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomerCreditCardsDTO</returns>
        private CustomerCreditCardsDTO GetCustomerCreditCardsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerCreditCardsDTO customerCreditCardsDTO = new CustomerCreditCardsDTO(Convert.ToInt32(dataRow["CustomerCreditCardsId"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["CardProfileId"] == DBNull.Value ? "" : Convert.ToString(dataRow["CardProfileId"]),
                                            dataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentModeId"]),
                                            dataRow["TokenId"] == DBNull.Value ? "" : Convert.ToString(dataRow["TokenId"]),
                                            dataRow["CustomerNameOnCard"] == DBNull.Value ? "" : Convert.ToString(dataRow["CustomerNameOnCard"]),
                                            dataRow["CreditCardNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardNumber"]),
                                            dataRow["CardExpiry"] == DBNull.Value ? "" : Convert.ToString(dataRow["CardExpiry"]),
                                            dataRow["LastCreditCardExpiryReminderSentOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastCreditCardExpiryReminderSentOn"]),
                                            dataRow["CreditCardExpiryReminderCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["CreditCardExpiryReminderCount"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(customerCreditCardsDTO);
            return customerCreditCardsDTO;
        }

        /// <summary>
        /// Gets the CustomerCreditCards data of passed CustomerCreditCards Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns CustomerCreditCardsDTO</returns>
        public CustomerCreditCardsDTO GetCustomerCreditCardsDTO(int id)
        {
            log.LogMethodEntry(id);
            CustomerCreditCardsDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE ccc.CustomerCreditCardsId = @CustomerCreditCardsId";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCreditCardsId", id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomerCreditCardsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// GetCustomerCreditCardsDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CustomerCreditCardsDTO> GetCustomerCreditCardsDTOList(List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerCreditCardsDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_CREDITCARDS_ID ||
                            searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.MASTER_ENTITY_ID )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.CARD_PROFILE_ID ||
                                 searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.TOKEN_ID)
                        {
                            query.Append(joiner + " DECRYPTBYPASSPHRASE(@PassPhrase, " + DBSearchParameters[searchParameter.Key] + ")" + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if ( 
                                 searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_IN_X_DAYS 
                                 )
                        {
                            query.Append(joiner + " 1 = 1"  ); //BL to handle these parameters
                        }
                        else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.EXPIREDCARD_LINKED_WITH_UNBILLED_SUBSCRIPTIONS ||
                            searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_BEFORE_NEXT_UNBILLED_CYCLE )
                        {
                            //Just ensuring that card is linked with active subscriptions with pending billing
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                              FROM subscriptionHeader sh, 
                                                                   subscriptionBillingSchedule sbs 
                                                              where sh.subscriptionHeaderId= sbs.subscriptionHeaderId 
                                                                AND sh.customerCreditCardsId = " + DBSearchParameters[searchParameter.Key] + " " +
                                                              @"AND sh.status = 'ACTIVE' 
                                                                AND sbs.Status = 'ACTIVE' 
                                                                AND CASE WHEN sbs.TransactionId IS NULL THEN 1 ELSE 0 END = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

                        }
                        else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.LINKED_WITH_ACTIVE_SUBSCRIPTIONS)
                        {
                            query.Append(joiner + @" CASE WHEN EXISTS( SELECT top 1 1
                                                               FROM subscriptionBillingSchedule sbs,
                                                                    SubscriptionHeader sh
                                                              WHERE sh.subscriptionHeaderId = sbs.subscriptionHeaderId 
                                                                AND sh.CustomerCreditCardsID = ccc.CustomerCreditCardsID 
                                                                AND ISNULL(sbs.IsActive,0) = 1
                                                                AND ISNULL(sh.IsActive,0) = 1
                                                                AND sh.Status = 'ACTIVE' ) THEN 1 ELSE 0 END  = " + dataAccessHandler.GetParameterName(searchParameter.Key)  + "  ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                selectQuery = selectQuery + query;
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CustomerCreditCardsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerCreditCardsDTO customerCreditCardsDTO = GetCustomerCreditCardsDTO(dataRow);
                    list.Add(customerCreditCardsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the CustomerCreditCardsDTO List for header Id List
        /// </summary>
        /// <param name="customerCreditCardsIdList"></param>
        /// <returns></returns>
        public List<CustomerCreditCardsDTO> GetCustomerCreditCardsDTOList(List<int> customerCreditCardsIdList)
        {
            log.LogMethodEntry(customerCreditCardsIdList);
            List<CustomerCreditCardsDTO> list = new List<CustomerCreditCardsDTO>();
            string query = @"SELECT ccc.CustomerCreditCardsId
                                    ,ccc.CustomerId
                                    ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,ccc.CardProfileId)) AS CardProfileId  
                                    ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,ccc.TokenId)) AS TokenId   
                                    ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,ccc.CustomerNameOnCard)) AS CustomerNameOnCard  
                                    ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,ccc.CreditCardNumber)) AS CreditCardNumber 
                                    ,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,ccc.CardExpiry)) AS CardExpiry
                                    ,ccc.IsActive
                                    ,ccc.CreatedBy
                                    ,ccc.CreationDate
                                    ,ccc.LastUpdatedBy
                                    ,ccc.LastUpdatedDate
                                    ,ccc.Guid
                                    ,ccc.SynchStatus
                                    ,ccc.site_id
                                    ,ccc.MasterEntityId
                                    ,ccc.PaymentModeId
                                    ,ccc.LastCreditCardExpiryReminderSentOn
                                    ,ccc.CreditCardExpiryReminderCount
                              FROM CustomerCreditCards AS ccc
                                  inner join @CustomerCreditCardsIdList List on ccc.CustomerCreditCardsId = List.Id ";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable table = dataAccessHandler.BatchSelect(query, "@CustomerCreditCardsIdList", customerCreditCardsIdList, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomerCreditCardsDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
