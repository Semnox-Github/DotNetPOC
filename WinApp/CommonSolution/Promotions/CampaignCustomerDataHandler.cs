/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler -CampaignCustomerDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      07-Jun-2019   Girish Kundar           Created 
 *2.70      04-Sep-2019   Mushahid Faizan         Added GetPhoneCount(), GetEmailCount(), 
 *                                                UpdateCampaignCustomerRecordEmail() & UpdateCampaignCustomerRecordSMS methods.
 *2.80      18-Nov-2019   Rakesh                  Added GetCustomersInCampaign and GetCustomersInCampaign method                                                
 *          24-Dec-2019   Jagan Mohana            Modified the search query GetCustomersInCampaign()
 *2.90      28-Jul-2020   Mushahid Faizan         Modified search query in GetCampaignCustomerDTOList()
 *2.100.0   15-Sep-2020   Nitin Pai               Push Notification changes to hold notification sent date and time
 *2.130.0   12-Jul-2020   Girish Kundar           Modified : Issue fix - Push notification token in GetNotificationCount Method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// CampaignCustomerDataHandler Data Handler - Handles insert, update and select of  CampaignCustomers objects
    /// </summary>
    public class CampaignCustomerDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CampaignCustomers AS campgnCustms";
        /// <summary>
        /// Dictionary for searching Parameters for the CampaignCustomerDTO object.
        /// </summary>
        private static readonly Dictionary<CampaignCustomerDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CampaignCustomerDTO.SearchByParameters, string>
        {
            { CampaignCustomerDTO.SearchByParameters.CAMPAIGN_ID,"campgnCustms.CampaignId"},
            { CampaignCustomerDTO.SearchByParameters.CAMPAIGN_ID_LIST,"campgnCustms.CampaignId"},
            { CampaignCustomerDTO.SearchByParameters.CARD_ID,"campgnCustms.CardId"},
            { CampaignCustomerDTO.SearchByParameters.CUSTOMER_ID,"campgnCustms.CustomerId"},
            { CampaignCustomerDTO.SearchByParameters.ID,"campgnCustms.Id"},
            { CampaignCustomerDTO.SearchByParameters.EMAIL_STATUS,"campgnCustms.EmailStatus"},
            { CampaignCustomerDTO.SearchByParameters.SMS_STATUS,"campgnCustms.SMSStatus"},
            { CampaignCustomerDTO.SearchByParameters.NOTIFICATION_STATUS,"campgnCustms.NotificationStatus"},
            { CampaignCustomerDTO.SearchByParameters.SITE_ID,"campgnCustms.site_id"},
            { CampaignCustomerDTO.SearchByParameters.MASTER_ENTITY_ID,"campgnCustms.MasterEntityId"},
            { CampaignCustomerDTO.SearchByParameters.IS_ACTIVE,"campgnCustms.IsActive"}
        };


        /// <summary>
        /// Parameterized Constructor for CampaignCustomerDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public CampaignCustomerDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            dataAccessHandler.CommandTimeOut = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CampaignCustomers Record.
        /// </summary>
        /// <param name="campaignCustomerDTO">CampaignCustomerDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CampaignCustomerDTO campaignCustomerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", campaignCustomerDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignId", campaignCustomerDTO.CampaignId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", campaignCustomerDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", campaignCustomerDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EmailSentDate", campaignCustomerDTO.EmailSentDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EmailStatus", campaignCustomerDTO.EmailStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SMSSentDate", campaignCustomerDTO.SMSSentDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SMSStatus", campaignCustomerDTO.SMSStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NotificationSentDate", campaignCustomerDTO.NotificationSentDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NotificationStatus", campaignCustomerDTO.NotificationStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", campaignCustomerDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", campaignCustomerDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to CampaignCustomerDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object of DataRow</param>
        /// <returns>returns the object of CampaignCustomerDTO</returns>
        private CampaignCustomerDTO GetCampaignCustomerDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CampaignCustomerDTO campaignCustomerDTO = new CampaignCustomerDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                         dataRow["CampaignId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignId"]),
                                          dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                          dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                          dataRow["EmailSentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EmailSentDate"]),
                                          dataRow["SMSSentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SMSSentDate"]),
                                          dataRow["NotificationSentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["NotificationSentDate"]),
                                          dataRow["EmailStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EmailStatus"]),
                                          dataRow["SMSStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SMSStatus"]),
                                          dataRow["NotificationStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["NotificationStatus"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                        );
            log.LogMethodExit(campaignCustomerDTO);
            return campaignCustomerDTO;
        }

        /// <summary>
        /// Gets the CampaignCustomerDTO data of passed Id 
        /// </summary>
        /// <param name="id">id of CampaignCustomer is passed as parameter</param>
        /// <returns>Returns the object of CampaignCustomerDTO</returns>
        public CampaignCustomerDTO GetCampaignCustomerDTO(int id)
        {
            log.LogMethodEntry(id);
            CampaignCustomerDTO result = null;
            string query = SELECT_QUERY + @" WHERE campgnCustms.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCampaignCustomerDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the CampaignCustomer record
        /// </summary>
        /// <param name="campaignCustomerDTO">CampaignCustomerDTO is passed as parameter</param>
        internal void Delete(CampaignCustomerDTO campaignCustomerDTO)
        {
            log.LogMethodEntry(campaignCustomerDTO);
            string query = @"DELETE  
                             FROM CampaignCustomers
                             WHERE CampaignCustomers.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", campaignCustomerDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            campaignCustomerDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the CampaignCustomers Table.
        /// </summary>
        /// <param name="campaignCustomerDTO">CampaignCustomerDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns> CampaignCustomerDTO</returns>
        public CampaignCustomerDTO Insert(CampaignCustomerDTO campaignCustomerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CampaignCustomers]
                               (CampaignId,
                                CustomerId,
                                CardId,
                                EmailSentDate,
                                SMSSentDate,
                                NotificationSentDate,
                                EmailStatus,
                                SMSStatus,
                                NotificationStatus,
                                site_id,
                                Guid,
                                MasterEntityId,
                                CreatedBy,
                                CreationDate,
                                LastUpdatedBy,
                                LastUpdateDate,
                                IsActive)
                         VALUES
                               (@CampaignId,
                                @CustomerId,
                                @CardId,
                                @EmailSentDate,
                                @SMSSentDate,
                                @NotificationSentDate,
                                @EmailStatus,
                                @SMSStatus,
                                @NotificationStatus,
                                @site_id,
                                NEWID(),
                                @MasterEntityId,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE() ,
                                @IsActive)
                                    SELECT * FROM CampaignCustomers WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignCustomerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignCustomerDTO(campaignCustomerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CampaignCustomerDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignCustomerDTO);
            return campaignCustomerDTO;
        }

        /// <summary>
        ///  Updates the record to the CampaignCustomers Table.
        /// </summary>
        /// <param name="campaignCustomerDTO">CampaignCustomerDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns> CampaignCustomerDTO</returns>
        public CampaignCustomerDTO Update(CampaignCustomerDTO campaignCustomerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(campaignCustomerDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CampaignCustomers]
                               SET
                                CampaignId     = @CampaignId,
                                CustomerId     = @CustomerId,
                                CardId         = @CardId,
                                EmailSentDate  = @EmailSentDate,
                                SMSSentDate    = @SMSSentDate,
                                NotificationSentDate    = @NotificationSentDate,
                                EmailStatus    = @EmailStatus,
                                SMSStatus      = @SMSStatus,
                                NotificationStatus      = @NotificationStatus,
                               -- site_id        = @site_id,
                                MasterEntityId = @MasterEntityId,
                                LastUpdatedBy  = @LastUpdatedBy,
                                LastUpdateDate = GETDATE() ,
                                IsActive = @IsActive
                               WHERE Id = @Id
                                    SELECT * FROM CampaignCustomers WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(campaignCustomerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCampaignCustomerDTO(campaignCustomerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CampaignCustomerDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(campaignCustomerDTO);
            return campaignCustomerDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="campaignCustomerDTO">CampaignCustomerDTO object as parameter</param>
        /// <param name="dt">dt is an object of type DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshCampaignCustomerDTO(CampaignCustomerDTO campaignCustomerDTO, DataTable dt)
        {
            log.LogMethodEntry(campaignCustomerDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                campaignCustomerDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                campaignCustomerDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                campaignCustomerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                campaignCustomerDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                campaignCustomerDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                campaignCustomerDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                campaignCustomerDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of CampaignCustomerDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of CampaignCustomerDTO</returns>
        public List<CampaignCustomerDTO> GetCampaignCustomerDTOList(List<KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CampaignCustomerDTO> campaignCustomerDTOList = new List<CampaignCustomerDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CampaignCustomerDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CampaignCustomerDTO.SearchByParameters.CAMPAIGN_ID
                            || searchParameter.Key == CampaignCustomerDTO.SearchByParameters.ID
                            || searchParameter.Key == CampaignCustomerDTO.SearchByParameters.CARD_ID
                            || searchParameter.Key == CampaignCustomerDTO.SearchByParameters.CUSTOMER_ID
                            || searchParameter.Key == CampaignCustomerDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCustomerDTO.SearchByParameters.CAMPAIGN_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CampaignCustomerDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CampaignCustomerDTO.SearchByParameters.EMAIL_STATUS
                            || searchParameter.Key == CampaignCustomerDTO.SearchByParameters.SMS_STATUS
                            || searchParameter.Key == CampaignCustomerDTO.SearchByParameters.NOTIFICATION_STATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CampaignCustomerDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CampaignCustomerDTO campaignCustomerDTO = GetCampaignCustomerDTO(dataRow);
                    campaignCustomerDTOList.Add(campaignCustomerDTO);
                }
            }
            log.LogMethodExit(campaignCustomerDTOList);
            return campaignCustomerDTOList;
        }
        
        /// <summary>
        /// Returns the Customers phoneCount matching the searchParameters
        /// </summary>
        /// <returns>no of Customers phoneCount matching the searchParameters</returns>
        public DataTable GetPhoneCount(int campaignId, string passPhrase)
        {
            log.LogMethodEntry();
            string selectQuery = @"select c.customer_Name, c.contact_phone1, 
                                                            cc.id, ca.card_number, ca.credits
                                                            from campaignCustomers cc, CustomerView(@PassphraseEnteredByUser) c, cards ca 
                                                            where cc.customerId = c.customer_id
                                                            and isnull(ltrim(c.contact_phone1), '') != ''
                                                            and SMSSentDate is null
                                                            and ca.card_id = cc.cardId
                                                            and cc.campaignId = @id";


            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", campaignId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassphraseEnteredByUser", passPhrase));

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), null);
            log.LogMethodExit(dataTable);
            return dataTable;
        }
        /// <summary>
        /// Returns the Customers Email Count matching the searchParameters
        /// </summary>
        /// <returns>no of Customers Email Count matching the searchParameters</returns>
        public DataTable GetEmailCount(int campaignId, string passPhrase)
        {
            log.LogMethodEntry();
            string selectQuery = @"select c.customer_Name, c.Email, 
                                                            cc.id, ca.card_number, ca.credits
                                                            from campaignCustomers cc, CustomerView(@PassphraseEnteredByUser) c, cards ca
                                                            where cc.customerId = c.customer_id
                                                            and isnull(ltrim(c.Email), '') != ''
                                                            and EmailSentDate is null
                                                            and ca.card_id = cc.cardId
                                                            and cc.campaignId = @id";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", campaignId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassphraseEnteredByUser", passPhrase));

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), null);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the Customers Notification Count matching the searchParameters
        /// </summary>
        /// <returns>no of Customers Notification Count matching the searchParameters</returns>
        public DataTable GetNotificationCount(int campaignId, string passPhrase)
        {
            log.LogMethodEntry();
            string selectQuery = @"select c.customer_Name, pd.PushNotificationToken, 
                                                            cc.id, ca.card_number, ca.credits
                                                            from campaignCustomers cc
                                                            LEFT OUTER JOIN (SELECT pnd.* , DENSE_RANK() OVER(PARTITION BY pnd.CustomerId ORDER BY pnd.LastUpdateDate DESC, pnd.Id DESC) rnk 
                                                            FROM PushNotificationDevice pnd WHERE pnd.IsActive = 1) pd ON pd.CustomerId = cc.customerId and pd.rnk = 1,
                                                            CustomerView(@PassphraseEnteredByUser) c, cards ca
                                                            where cc.customerId = c.customer_id
                                                            and isnull(ltrim(pd.PushNotificationToken), '') != ''
                                                            and cc.NotificationSentDate is null
                                                            and ca.card_id = cc.cardId
                                                            and cc.campaignId = @id";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", campaignId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassphraseEnteredByUser", passPhrase));

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), null);
            log.LogMethodExit(dataTable);
            return dataTable;
        }


        public List<CampaignCustomerDTO> GetCampaignCustomerDTOList(string passPhrase, string membership = null, string totalAddPoint = null,
                                      string totalSpend = null, string totalAddPointOperator = null, string totalSpendOperator = null, string birthDayInDays = null,
                                      CampaignCustomerSearchCriteria advCustomerCriteria = null, AccountSearchCriteria advCardCriteria = null)
        {
            log.LogMethodEntry(passPhrase, membership, totalAddPoint, totalSpend, totalAddPointOperator, totalSpendOperator, birthDayInDays, advCustomerCriteria, advCardCriteria);
            StringBuilder query = new StringBuilder("");
            if (!string.IsNullOrEmpty(membership))
            {
                query.Append(" and customers.MembershipId = " + membership);
            }
            if (!string.IsNullOrEmpty(totalSpend) && !string.IsNullOrEmpty(totalSpendOperator))
            {
                query.Append(" and card.credits_played " + totalSpendOperator + totalSpend);
            }
            if (!string.IsNullOrEmpty(totalAddPoint) && !string.IsNullOrEmpty(totalAddPointOperator))
            {
                query.Append(" and (select isnull(sum(amount), 0) from trx_lines where card_id = card.card_id) " + totalAddPointOperator + totalAddPoint);
            }
            if (!string.IsNullOrEmpty(birthDayInDays))
            {
                query.Append(@" and DATEDIFF(dd, 
                                        DATEADD(yy, DATEDIFF(yy, 0, birth_date), 0), 
                                                dateadd(YY, case when (datepart(mm, birth_date) * 100 + DATEPART(dd, birth_date) < datepart(mm, GETDATE()) * 100 + DATEPART(dd, GETDATE()))
                                                            then 1 else 0 end, birth_date))
                                        between
                                            DATEDIFF(dd, DATEADD(yy, DATEDIFF(yy, 0, getdate()), 0), getdate()) and
                                            DATEDIFF(dd, DATEADD(yy, DATEDIFF(yy, 0, getdate()), 0), getdate()) + 1 + " + birthDayInDays);
            }

            string selectQuery = @"SELECT Distinct customers.customer_id , card.card_id,customers.customer_name + isnull(' ' + last_name, '') name,card.card_number, 
                            case when ContactType.Name ='EMAIL' then CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1)) end email , 
                            case when ContactType.Name ='PHONE' then CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1)) end contact_phone1 , 
                            CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth)) birth_date  
                                                                 FROM  Customers customers
                                                                 INNER JOIN Profile ON Profile.Id = customers.ProfileId
                                                                 LEFT OUTER JOIN ProfileType ON Profile.ProfileTypeId = ProfileType.Id
                                                                 LEFT OUTER JOIN Contact ON Contact.ProfileId = Profile.Id
                                                                 LEFT OUTER JOIN ContactType ON ContactType.Id = Contact.ContactTypeId
                                                                 LEFT OUTER JOIN Address ON Address.ProfileId = Profile.Id
                                                                 LEFT OUTER JOIN AddressType ON AddressType.Id = Address.AddressTypeId
																 LEFT OUTER JOIN State ON State.StateId = Address.StateId
                                                                 LEFT OUTER JOIN Country ON Country.CountryId = Address.CountryId
                                                                 OUTER APPLY (SELECT TOP 1 
																 CASE WHEN cards.valid_flag = 'Y' THEN cards.card_number ELSE cards.card_number + '[Inactive]' END card_number,
                                                                 cards.card_id card_id,cards.credits_played credits_played                                                                    
                                                                 FROM cards WHERE customers.customer_id = cards.customer_id
                                                                 ORDER BY valid_flag DESC, last_update_time desc) card
																 where  Isnull(Profile.OptInPromotions ,'Y') = 'Y'
                            and (1 = 1 " + query + ")";
            if (advCustomerCriteria != null)
            {
                selectQuery += advCustomerCriteria.GetAndQuery();
            }
            if (advCardCriteria != null)
            {
                selectQuery += advCardCriteria.GetAndQuery();
            }
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<SqlParameter> parameterList = new List<SqlParameter>(advCustomerCriteria.GetSqlParameters());

            parameterList.AddRange(new List<SqlParameter>(advCardCriteria.GetSqlParameters()));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameterList.ToArray(), sqlTransaction);
            List<CampaignCustomerDTO> campaignCustomerList = new List<CampaignCustomerDTO>();
            int rowCount = dataTable.Rows.Count;
            if (rowCount > 0)
            {
                Dictionary<int, CampaignCustomerDTO> keyValuePairs = new Dictionary<int, CampaignCustomerDTO>(rowCount);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int customerId = dataRow["customer_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["customer_id"]);
                    string email = dataRow["email"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["email"]);
                    string phoneNumber = dataRow["contact_phone1"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["contact_phone1"]);
                    // TBD: ADD PushNotificationToken;
                    if (keyValuePairs.ContainsKey(customerId))
                    {
                        var customerDTO = keyValuePairs[customerId];
                        if (string.IsNullOrWhiteSpace(email) == false
                             && string.IsNullOrWhiteSpace(customerDTO.Email))
                        {
                            customerDTO.Email = email;
                        }
                        if (string.IsNullOrWhiteSpace(phoneNumber) == false
                             && string.IsNullOrWhiteSpace(customerDTO.ContactPhone1))
                        {
                            customerDTO.ContactPhone1 = phoneNumber;
                        }
                    }
                    else
                    {
                        CampaignCustomerDTO campaignCustomerDTO = new CampaignCustomerDTO(-1,
                                         -1,
                                         customerId,
                                         dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                         dataRow["name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["name"]),
                                         dataRow["Card_Number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Card_Number"]),
                                         email,
                                         phoneNumber,
                                         dataRow["birth_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["birth_date"]),
                                         (DateTime?)null,
                                         (DateTime?)null,
                                         (DateTime?)null,
                                         string.Empty,
                                         string.Empty,
                                         string.Empty,
                                         (decimal?)null,
                                         string.Empty);
                        keyValuePairs.Add(campaignCustomerDTO.CustomerId, campaignCustomerDTO);
                        campaignCustomerList.Add(campaignCustomerDTO);
                    }
                }
            }
            log.LogMethodExit(campaignCustomerList);
            return campaignCustomerList;
        }
        public List<CampaignCustomerDTO> GetCustomersInCampaign(int campaignId, string passPhrase)
        {
            log.LogMethodEntry(campaignId, passPhrase);
            string selectQuery = @"select cc.id, ca.card_id, customer_name + isnull(' ' + last_name, '') name,
                                                            ca.card_number, ca.credits, cc.CampaignId,
                                                            c.email,
                                                            c.contact_phone1, 
                                                            EmailSentDate Email_Sent_Date, EmailStatus Email_Status, 
                                                            SMSSentDate SMS_Sent_Date, SMSStatus SMS_Status,
                                                            NotificationSentDate, NotificationStatus, pd.PushNotificationToken, c.customer_id
                                                        from campaignCustomers cc
                                                        LEFT OUTER JOIN (SELECT pnd.* , DENSE_RANK() OVER(PARTITION BY pnd.CustomerId ORDER BY pnd.LastUpdateDate DESC, pnd.Id DESC) rnk 
                                                        FROM PushNotificationDevice pnd WHERE pnd.IsActive = 1) pd ON pd.CustomerId = cc.customerId and pd.rnk = 1,
                                                        CustomerView(@PassphraseEnteredByUser) c, cards ca
                                                       where cc.customerId = c.customer_id
                                                       and ca.card_id = cc.cardId
                                                        and cc.campaignId = @campaignId";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, new[] { new SqlParameter("@campaignId", campaignId), new SqlParameter("@PassphraseEnteredByUser", passPhrase) }, null);
            List<CampaignCustomerDTO> campaignCustomerList = new List<CampaignCustomerDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CampaignCustomerDTO campaignCustomerDTO = new CampaignCustomerDTO(dataRow["id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["id"]),
                                         dataRow["CampaignId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignId"]),
                                         dataRow["customer_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["customer_id"]),
                                         dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                         dataRow["name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["name"]),
                                         dataRow["card_number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["card_number"]),
                                         dataRow["email"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["email"]),
                                         dataRow["contact_phone1"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["contact_phone1"]),
                                         (DateTime?)null,
                                         dataRow["Email_Sent_Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Email_Sent_Date"]),
                                         dataRow["SMS_Sent_Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SMS_Sent_Date"]),
                                         dataRow["NotificationSentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["NotificationSentDate"]),
                                         dataRow["Email_Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Email_Status"]),
                                         dataRow["SMS_Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SMS_Status"]),
                                         dataRow["NotificationStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["NotificationStatus"]),
                                         dataRow["credits"] == DBNull.Value ? -1 : Convert.ToDecimal(dataRow["credits"]),
                                         dataRow["PushNotificationToken"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PushNotificationToken"]));

                    campaignCustomerList.Add(campaignCustomerDTO);
                }
            }
            log.LogMethodExit(campaignCustomerList);
            return campaignCustomerList;
        }
    }
}
