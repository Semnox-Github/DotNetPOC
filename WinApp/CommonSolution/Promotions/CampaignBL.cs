/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  Campaign
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        24-Jun-2019     Girish Kundar         Created 
 *2.100.0     15-Sep-2020     Nitin Pai             Push Notification: Send Customer Campaign Messages
 *2.110.0     08-Dec-2020     Guru S A              Subscription changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for Campaign class.
    /// </summary>
    public class CampaignBL
    {
        private CampaignDTO campaignDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        /// <summary>
        /// Parameterized constructor of CampaignBL class
        /// </summary>
        /// <param name="executionContext"></param>
        private CampaignBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CampaignBL object using the campaignDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="campaignDTO">CampaignDTO object is passed as parameter</param>
        public CampaignBL(ExecutionContext executionContext, CampaignDTO campaignDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignDTO);
            this.campaignDTO = campaignDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Campaign id as the parameter
        /// Would fetch the Campaign object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of Campaign Object </param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false.</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        /// <param name="sqlTransaction">sqlTransaction</param>

        public CampaignBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CampaignDataHandler campaignDataHandler = new CampaignDataHandler(sqlTransaction);
            campaignDTO = campaignDataHandler.GetCampaignDTO(id);
            if (campaignDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Campaign", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (campaignDTO != null && loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for Campaign object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            CampaignCustomerListBL campaignCustomerListBL = new CampaignCustomerListBL(executionContext);
            List<KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>(CampaignCustomerDTO.SearchByParameters.CAMPAIGN_ID, campaignDTO.CampaignId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>(CampaignCustomerDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            campaignDTO.CampaignCustomerDTOList = campaignCustomerListBL.GetCampaignCustomerDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AppUIPanel
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (campaignDTO.IsChangedRecursive == false &&
                campaignDTO.CampaignId >-1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CampaignDataHandler campaignDataHandler = new CampaignDataHandler(sqlTransaction);
            if (campaignDTO.IsActive == true)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (campaignDTO.CampaignId < 0)
                {
                    log.LogVariableState("CampaignDTO", campaignDTO);
                    campaignDTO = campaignDataHandler.Insert(campaignDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignDTO.AcceptChanges();
                }
                else if (campaignDTO.IsChanged)
                {
                    log.LogVariableState("CampaignDTO", campaignDTO);
                    campaignDTO = campaignDataHandler.Update(campaignDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignDTO.AcceptChanges();
                }
                SaveCampaignCustomers(sqlTransaction);
            }
            else  
            {
                if (campaignDTO.CampaignCustomerDTOList != null && campaignDTO.CampaignCustomerDTOList.Any(x => x.IsActive == true))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                SaveCampaignCustomers(sqlTransaction);
                if (campaignDTO.CampaignId >= 0)
                {
                    campaignDataHandler.Delete(campaignDTO);
                }
                campaignDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records :CampaignCustomer 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveCampaignCustomers(SqlTransaction sqlTransaction)
        {
            if (campaignDTO.CampaignCustomerDTOList != null &&
                campaignDTO.CampaignCustomerDTOList.Any())
            {
                List<CampaignCustomerDTO> updatedCampaignCustomerDTOList = new List<CampaignCustomerDTO>();
                foreach (var campaignCustomerDTO in campaignDTO.CampaignCustomerDTOList)
                {
                    if (campaignCustomerDTO.CampaignId != campaignDTO.CampaignId)
                    {
                        campaignCustomerDTO.CampaignId = campaignDTO.CampaignId;
                    }
                    if (campaignCustomerDTO.IsChanged)
                    {
                        updatedCampaignCustomerDTOList.Add(campaignCustomerDTO);
                    }
                }
                if (updatedCampaignCustomerDTOList.Any())
                {
                    CampaignCustomerListBL campaignCustomerListBL = new CampaignCustomerListBL(executionContext, updatedCampaignCustomerDTOList);
                    campaignCustomerListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the CampaignDTO  ,CampaignCustomerDTO - child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (campaignDTO.CampaignCustomerDTOList != null)
            {
                foreach (var campaignCustomerDTO in campaignDTO.CampaignCustomerDTOList)
                {
                    if (campaignCustomerDTO.IsChanged)
                    {
                        CampaignCustomerBL campaignCustomerBL = new CampaignCustomerBL(executionContext, campaignCustomerDTO);
                        validationErrorList.AddRange(campaignCustomerBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignDTO CampaignDTO
        {
            get
            {
                return campaignDTO;
            }
        }

        public void SendMessages(SqlTransaction sqlTransaction = null)
        {
            try
            {
                //int appMessagingClientId = -1;
                //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "CAMPAIGN_MESSAGE", "A");
                //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                //    appMessagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                //int emailMessagingClientId = -1;
                //messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "CAMPAIGN_MESSAGE", "E");
                //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                //    emailMessagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                //int smsMessagingClientId = -1;
                //messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "CAMPAIGN_MESSAGE", "S");
                //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                //    smsMessagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                //String notificationTemplate = "";
                //EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext);
                //List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, "APP_NOTIFICATION_TEMPLATE"));
                //List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                //if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                //{
                //    notificationTemplate = emailTemplateDTOList[0].EmailTemplate;
                //}

                CampaignCustomerListBL campaignCustomerListBL = new CampaignCustomerListBL(executionContext);
                campaignDTO.CampaignCustomerDTOList = campaignCustomerListBL.GetCustomersInCampaign(campaignDTO.CampaignId);
                List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
                if (campaignDTO.CampaignCustomerDTOList != null && campaignDTO.CampaignCustomerDTOList.Any())
                {
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    List<int> customerIdList = campaignDTO.CampaignCustomerDTOList.Select(cc => cc.CustomerId).Distinct().ToList();
                    customerDTOList = customerListBL.GetCustomerDTOList(customerIdList, true, true, false, sqlTransaction);
                    if (customerDTOList == null)
                    {
                        customerDTOList = new List<CustomerDTO>();
                    }
                }  
                
                foreach (CampaignCustomerDTO campaignCustomerDTO in campaignDTO.CampaignCustomerDTOList)
                {
                    //string body = campaignDTO.MessageTemplate;
                    //body = ReplaceKeywords(body, campaignCustomerDTO);
                    CustomerDTO customerDTO = customerDTOList.Find(cust => cust.Id == campaignCustomerDTO.CustomerId);
                    if (campaignDTO.CommunicationMode == "Email" || campaignDTO.CommunicationMode == "Both")
                    {
                        if (!String.IsNullOrEmpty(campaignCustomerDTO.Email))
                        {
                            try
                            {
                                CampaignMessageEventsBL campaignMessageEventsBL = new CampaignMessageEventsBL(executionContext, campaignDTO, campaignCustomerDTO, customerDTO, sqlTransaction);
                                campaignMessageEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL);
                                //MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "C" + campaignDTO.CampaignId, "E", campaignCustomerDTO.Email, "", "", null, null, null, null,
                                //      campaignDTO.MessageSubject, body, campaignCustomerDTO.CustomerId, null, "", true, "", "", emailMessagingClientId, false, "");
                                //MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                                //messagingRequestBL.Save();

                                campaignCustomerDTO.EmailSentDate = DateTime.Now;
                                campaignCustomerDTO.EmailStatus = "Success";
                            }
                            catch (Exception ex)
                            {
                                EventLog eventLog = new EventLog(new Utilities());
                                eventLog.logEvent("Campaign", 'D', "Promotional Email Campaign", "Unable to save message request for Promotional Email Campaign to customer with id " + campaignCustomerDTO.CustomerId, "Campaign", 0, "", campaignCustomerDTO.CustomerId.ToString(), null);
                                log.Error("Error occured while sending e-mail", ex);
                            }
                        }
                    }

                    if (campaignDTO.CommunicationMode == "SMS" || campaignDTO.CommunicationMode == "Both")
                    {
                        if (!String.IsNullOrEmpty(campaignCustomerDTO.ContactPhone1))
                        {
                            try
                            {
                                CampaignMessageEventsBL campaignMessageEventsBL = new CampaignMessageEventsBL(executionContext, campaignDTO, campaignCustomerDTO, customerDTO, sqlTransaction);
                                campaignMessageEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.SMS);
                                //MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "C" + campaignDTO.CampaignId, "S", "", campaignCustomerDTO.ContactPhone1, "", null, null, null, null,
                                //      "", body, campaignCustomerDTO.CustomerId, null, "", true, "", "", smsMessagingClientId, false, "");
                                //MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                                //messagingRequestBL.Save();

                                campaignCustomerDTO.SMSSentDate = DateTime.Now;
                                campaignCustomerDTO.SMSStatus = "Success";
                            }
                            catch (Exception ex)
                            {
                                EventLog eventLog = new EventLog(new Utilities());
                                eventLog.logEvent("Campaign", 'D', "Promotional SMS Campaign", "Unable to save message request for Promotional SMS Campaign to customer with id " + campaignCustomerDTO.CustomerId, "Campaign", 0, "", campaignCustomerDTO.CustomerId.ToString(), null);
                                log.Error("Error occured while sending e-mail", ex);
                            }
                        }
                    }
                    //WMS sends as App for AppNotification look Up- 
                    if (campaignDTO.CommunicationMode == "App" || campaignDTO.CommunicationMode == "Both")
                    {
                        if (!String.IsNullOrEmpty(campaignCustomerDTO.NotificationToken))// && !String.IsNullOrEmpty(notificationTemplate))
                        {
                            //String tempString = notificationTemplate;
                            //tempString = tempString.Replace("@notificationSubject", campaignDTO.MessageSubject);
                            //tempString = tempString.Replace("@notificationBody", body);
                            //tempString = tempString.Replace("@cardNumber", campaignCustomerDTO.CardNumber);
                            //tempString = tempString.Replace("@notificationType", "PROMOTION");
                            //tempString = tempString.Replace("@PromotionId", campaignDTO.CampaignId.ToString());
                            //tempString = tempString.Replace("@ToDevice", campaignCustomerDTO.NotificationToken);
                            try
                            {
                                CampaignMessageEventsBL campaignMessageEventsBL = new CampaignMessageEventsBL(executionContext, campaignDTO, campaignCustomerDTO, customerDTO, sqlTransaction);
                                campaignMessageEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION);
                                //MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, "C" + campaignDTO.CampaignId, "A", "", "", "", null, null, null, null,
                                //      campaignDTO.MessageSubject, tempString, campaignCustomerDTO.CustomerId, null, "", true, "", "", appMessagingClientId, false, campaignCustomerDTO.NotificationToken, false);
                                //MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                                //messagingRequestBL.Save();

                                campaignCustomerDTO.NotificationSentDate = DateTime.Now;
                                campaignCustomerDTO.NotificationStatus = "Success";
                            }
                            catch (Exception ex)
                            {
                                EventLog eventLog = new EventLog(new Utilities());
                                eventLog.logEvent("Campaign", 'D', "App Notification Campaign", "Unable to save message request for App Notification to customer with id " + campaignCustomerDTO.CustomerId, "Campaign", 0, "", campaignCustomerDTO.CustomerId.ToString(), null);
                                log.Error("Error occured while sending e-mail", ex);
                            }
                        }
                    }

                    try
                    {
                        CampaignCustomerBL campaignCustomerBL = new CampaignCustomerBL(executionContext, campaignCustomerDTO);
                        campaignCustomerBL.Save(sqlTransaction);
                    }
                    catch (Exception ex)
                    {
                        EventLog eventLog = new EventLog(new Utilities());
                        eventLog.logEvent("Campaign", 'D', "Promotional SMS Campaign", "Unable to save campaign customer table for Promotional Campaign for customer with id " + campaignCustomerDTO.CustomerId, "Campaign", 0, "", campaignCustomerDTO.CustomerId.ToString(), null);
                        log.Error("Error occured while sending e-mail", ex);
                    }
                }
            }
            catch (ApplicationException ex)
            {
                log.Error(ex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
            }
        }

        //private string ReplaceKeywords(string body, CampaignCustomerDTO campaignCustomerDTO)
        //{
        //    log.LogMethodEntry(body, campaignCustomerDTO);

        //    if (!String.IsNullOrEmpty(campaignCustomerDTO.Name))
        //        body = body.Replace("@CustomerName", campaignCustomerDTO.Name);
        //    else
        //        body = body.Replace("@CustomerName", "");

        //    if (!String.IsNullOrEmpty(campaignCustomerDTO.Email))
        //        body = body.Replace("@EmailId", campaignCustomerDTO.Email);
        //    else
        //        body = body.Replace("@EmailId", "");

        //    if (!String.IsNullOrEmpty(campaignCustomerDTO.CardNumber))
        //        body = body.Replace("@CardNumber", campaignCustomerDTO.CardNumber);
        //    else
        //        body = body.Replace("@CardNumber", "");

        //    //if (dr.Table.Columns.Contains("TrxNetAmount") && dr["TrxNetAmount"] != DBNull.Value)
        //    //    body = body.Replace("@TrxAmount", Convert.ToDouble(dr["TrxNetAmount"]).ToString("N2"));
        //    //else
        //    //    body = body.Replace("@TrxAmount", "");

        //    //if (dr.Table.Columns.Contains("TrxDate") && dr["TrxDate"] != DBNull.Value)
        //    //    body = body.Replace("@TrxDate", Convert.ToDateTime(dr["TrxDate"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT));
        //    //else
        //    //    body = body.Replace("@TrxDate", "");

        //    if (campaignCustomerDTO.Credit != null)
        //        body = body.Replace("@CardCredits", Convert.ToDouble(campaignCustomerDTO.Credit.ToString()).ToString(ParafaitDefaultContainerList.GetParafaitDefault(this.executionContext,"AMOUNT_FORMAT")));
        //    else
        //        body = body.Replace("@CardCredits", "");

        //    //if (dr.Table.Columns.Contains("Expirydate") && dr["Expirydate"] != DBNull.Value)
        //    //    body = body.Replace("@CardExpiry", (string.IsNullOrEmpty(dr["Expirydate"].ToString()) ? "" : Convert.ToDateTime(dr["Expirydate"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT)));
        //    //else
        //    //    body = body.Replace("@CardExpiry", "");

        //    //if (dr.Table.Columns.Contains("ValidityinDays") && dr["ValidityinDays"] != DBNull.Value)
        //    //    body = body.Replace("@ValidityInDays", Convert.ToDouble(dr["ValidityinDays"]).ToString());
        //    //else
        //    //    body = body.Replace("@ValidityInDays", "");

        //    //if (dr.Table.Columns.Contains("RedemptionOrderNo"))
        //    //    body = body.Replace("@RedemptionOrderNo", dr["RedemptionOrderNo"].ToString());
        //    //else
        //    //    body = body.Replace("@RedemptionOrderNo", "");

        //    //if (dr.Table.Columns.Contains("redeemed_date") && dr["redeemed_date"] != DBNull.Value)
        //    //    body = body.Replace("@RedeemedDate", Convert.ToDateTime(dr["redeemed_date"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT));
        //    //else
        //    //    body = body.Replace("@RedeemedDate", "");

        //    //if (dr.Table.Columns.Contains("preparedDate") && dr["preparedDate"] != DBNull.Value)
        //    //    body = body.Replace("@RedemptionPreparedDate", Convert.ToDateTime(dr["preparedDate"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT));
        //    //else
        //    //    body = body.Replace("@RedemptionPreparedDate", "");

        //    //if (dr.Table.Columns.Contains("deliveredDate") && dr["deliveredDate"] != DBNull.Value)
        //    //    body = body.Replace("@RedemptionDeliveryDate", Convert.ToDateTime(dr["deliveredDate"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT));
        //    //else
        //    //    body = body.Replace("@RedemptionDeliveryDate", "");
        //    log.LogMethodExit(body);
        //    return body;
        //}

        /// <summary>
        /// Manages the list of Campaign
        /// </summary>
        public class CampaignListBL
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private readonly ExecutionContext executionContext;
            private List<CampaignDTO> campaignDTOList = new List<CampaignDTO>(); // To be initialized
            /// <summary>
            /// Parameterized constructor of CampaignListBL
            /// </summary>
            /// <param name="executionContext">executionContext object</param>
            public CampaignListBL(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                this.executionContext = executionContext;
                log.LogMethodExit();
            }

            /// <summary>
            /// Parameterized constructor
            /// </summary>
            /// <param name="executionContext">executionContext</param>
            /// <param name="campaignDTOList">CampaignDTO List is passed as parameter </param>
            public CampaignListBL(ExecutionContext executionContext,
                                                   List<CampaignDTO> campaignDTOList)
                : this(executionContext)
            {
                log.LogMethodEntry(executionContext, campaignDTOList);
                this.campaignDTOList = campaignDTOList;
                log.LogMethodExit();
            }

            /// <summary>
            ///  Returns the Get the Campaign DTO list
            /// </summary>
            /// <param name="searchParameters">search Parameters</param>
            /// <param name="loadChildRecords">loadChildRecords true or false</param>
            /// <param name="activeChildRecords">activeChildRecords true or false </param>
            /// <param name="sqlTransaction">sqlTransaction</param>
            /// <returns>The List of CampaignDTO </returns>
            public List<CampaignDTO> GetCampaignDTOList(List<KeyValuePair<CampaignDTO.SearchByParameters, string>> searchParameters,
                                                             bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            {
                //child records needs to  build
                log.LogMethodEntry(searchParameters, sqlTransaction);
                CampaignDataHandler campaignDataHandler = new CampaignDataHandler(sqlTransaction);
                List<CampaignDTO> campaignDTOList = campaignDataHandler.GetCampaignDTOList(searchParameters);
                if (campaignDTOList != null && campaignDTOList.Any() && loadChildRecords)
                {
                    Build(campaignDTOList, activeChildRecords, sqlTransaction);
                }
                log.LogMethodExit(campaignDTOList);
                return campaignDTOList;
            }

            /// <summary>
            /// Builds the List of Campaign objects based on the list of Campaign id.
            /// </summary>
            /// <param name="campaignDTOList">CampaignDTO List is passed as parameter</param>
            /// <param name="activeChildRecords">activeChildRecords either true or false</param>
            /// <param name="sqlTransaction">SqlTransaction object </param>
            private void Build(List<CampaignDTO> campaignDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(campaignDTOList, activeChildRecords, sqlTransaction);
                Dictionary<int, CampaignDTO> campaignIdCustomerIdDictionary = new Dictionary<int, CampaignDTO>();
                string  campaignIdIdSet = string.Empty;
                StringBuilder sb = new StringBuilder("");
                for (int i = 0; i < campaignDTOList.Count; i++)
                {
                    if (campaignDTOList[i].CampaignId == -1 ||
                        campaignIdCustomerIdDictionary.ContainsKey(campaignDTOList[i].CampaignId))
                    {
                        continue;
                    }
                    if (i != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(campaignDTOList[i].CampaignId);
                    campaignIdCustomerIdDictionary.Add(campaignDTOList[i].CampaignId, campaignDTOList[i]);
                }
                campaignIdIdSet = sb.ToString();
                CampaignCustomerListBL campaignCustomerListBL = new CampaignCustomerListBL(executionContext);
                List<KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>(CampaignCustomerDTO.SearchByParameters.CAMPAIGN_ID_LIST, campaignIdIdSet.ToString()));
                List<CampaignCustomerDTO> campaignCustomerDTOList = campaignCustomerListBL.GetCampaignCustomerDTOList(searchParameters, sqlTransaction);
                if (campaignCustomerDTOList.Any())
                {
                    log.LogVariableState("CampaignCustomerDTOList", campaignCustomerDTOList);
                    foreach (var campaignCustomerDTO in campaignCustomerDTOList)
                    {
                        if (campaignIdCustomerIdDictionary.ContainsKey(campaignCustomerDTO.CampaignId))
                        {
                            if (campaignIdCustomerIdDictionary[campaignCustomerDTO.CampaignId].CampaignCustomerDTOList == null)
                            {
                                campaignIdCustomerIdDictionary[campaignCustomerDTO.CampaignId].CampaignCustomerDTOList = new List<CampaignCustomerDTO>();
                            }
                            campaignIdCustomerIdDictionary[campaignCustomerDTO.CampaignId].CampaignCustomerDTOList.Add(campaignCustomerDTO);
                        }
                    }
                }
                log.LogMethodExit();
            }


            /// <summary>
            /// Saves the  list of  CampaignIdDTO.
            /// </summary>
            /// <param name="sqlTransaction">sqlTransaction object</param>
            public void Save(SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(sqlTransaction);
                if (campaignDTOList == null ||
                    campaignDTOList.Any() == false)
                {
                    log.LogMethodExit(null, "List is empty");
                    return;
                }
                for (int i = 0; i < campaignDTOList.Count; i++)
                {
                    var campaignDTO = campaignDTOList[i];
                    if (campaignDTO.IsChangedRecursive == false)
                    {
                        continue;
                    }
                    try
                    {
                        CampaignBL campaignBL = new CampaignBL(executionContext, campaignDTO);
                        campaignBL.Save(sqlTransaction);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while saving CampaignDTO.", ex);
                        log.LogVariableState("Record Index ", i);
                        log.LogVariableState("CampaignDTO", campaignDTO);
                        throw;
                    }
                }
                log.LogMethodExit();
            }

        }

    }
}
