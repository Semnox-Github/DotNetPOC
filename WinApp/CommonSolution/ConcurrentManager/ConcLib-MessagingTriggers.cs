/********************************************************************************************
 * Project Name - ConcLib
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.40.0       9-Sep-2018       Guru S A           Modified to do message management changes for redemption status 
 *2.50.0      03-Dec-2018       Mathew Ninan       Deprecating StaticDataExchange class
 *2.60.0      03-May-2019       Laster             email and contact check changes for last activity based expiry message
 *2.100.0     15-Sep-2020       Nitin Pai          Push Notification: Generate Push notification messages via message management
 *2.110.0     13-Dec-2020       Guru S A           Subscription changes                                                                               
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Messaging;
using System.Drawing;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using EventLog = Semnox.Core.GenericUtilities.EventLog;
using System.Diagnostics;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.ConcurrentManager
{
    public partial class ConcLib
    {
        private string imageFolder;
        private string passPhrase;
        public string MessagingTriggers(int RequestId, string LogFileName)
        {
            log.LogMethodEntry(RequestId, LogFileName);

            //Uncomment this to debug the service
            //Debugger.Launch();
            try
            {
                _requestId = RequestId;
                _logFileName = LogFileName;

                MessageQueue messageQueue = MessageQueueUtils.GetMessageQueue(RequestId);

                System.Threading.ThreadStart thr = delegate
                {
                    while (true)
                    {
                        System.Messaging.Message msg = messageQueue.Receive();

                        msg.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                        QueueMessage = msg.Body.ToString();
                        if (QueueMessage.Equals("SHUTDOWN"))
                            break;
                    }
                };

                System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(thr));
                th.Start();

                imageFolder = _utilities.executeScalar("select dbo.get_parafait_defaults('IMAGE_DIRECTORY')").ToString() + "\\";
                int loopCount = 6;
                while (QueueMessage.Equals("SHUTDOWN") == false)
                {
                    System.Windows.Forms.Application.DoEvents();

                    if (loopCount++ < 6)
                    {
                        Thread.Sleep(5 * 1000);
                        continue;
                    }
                    loopCount = 0;

                    string encryptedPassPhrase = _utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
                    passPhrase = encryptedPassPhrase;

                    DateTime timeStamp = _utilities.getServerTime();
                    ProcessOnPurchaseMessaingTrigger(timeStamp);
                    ProcessRedemptionMessaingTrigger(timeStamp);
                    //DataTable dtTriggers = _utilities.executeDataTable(@"exec MessagingTriggersOnPurchase @ToTimeStamp = @toTimeStamp , @PassphraseEnteredByUser = @Passphrase", 
                    //                                                    new SqlParameter("@toTimeStamp", timeStamp), new SqlParameter("@Passphrase", passPhrase));

                    //log.LogVariableState("@toTimeStamp", timeStamp);

                    RegexUtilities regex = new RegexUtilities();
                    Messaging messaging = new Messaging(_utilities);


                    //The following section of code was written on 8-06-2015-to provide card validity expiry messages
                    if (_utilities.getParafaitDefaults("CARD_EXPIRY_RULE") == "LASTACTIVITY")
                    {
                        string gap = (string)_utilities.getParafaitDefaults("CARD_EXPIRY_ALERT_GAP");
                        string frequency = (string)_utilities.getParafaitDefaults("CARD_EXPIRY_MESSAGE_FREQUENCY");

                        if (gap == "") gap = "0";
                        if (frequency.Length > 0 && frequency != "0")
                        {
                            string query = "SELECT  c.card_id, c.card_number,c.credits, c.customer_id, cu.customer_name first_Name," +
                                                " cu.last_name, " +
                                                " cu.email as email, " +
                                                " cu.contact_phone1, " +
                                                " m.*,(SELECT " +
                                                          " DATEADD(dd, 0, DATEDIFF(dd, 0,dateadd(month,(SELECT convert(int,default_value) from parafait_defaults where " +
                                                          " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = m.site_Id)),c.last_update_time )))) Expirydate, " +
                                                          " DATEADD(dd, 0, DATEDIFF(dd, 0,DATEADD(day,(-1)*(@gap*(@freq-1)),(SELECT " +
                                                                                             " dateadd(month,(SELECT convert(int,default_value) " +
                                                                                             " from parafait_defaults where  " +
                                                                                             " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = m.site_Id)),c.last_update_time " +
                                                                                                 "    ) " +
                                                                                             " ) " +
                                                                  "))) as Mindate, " +
                                                          " DATEDIFF(day,DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())),(/*Expiry date part starts*/ dateadd(month,(SELECT convert(int,default_value) from parafait_defaults where  " +
                                                          " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = m.site_Id)),c.last_update_time )/*Expiry date part ends*/) " +
                                                          " ) as ValidityinDays" +
                                                          ",(DATEDIFF(day,(/*Mindate part starts*/DATEADD(dd, 0, DATEDIFF(dd, 0,DATEADD(day,(-1)*(@gap*(@freq-1)),(SELECT " +
                                                                                              " dateadd(month,(SELECT convert(int,default_value) from parafait_defaults where " +
                                                                                              " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = m.site_Id)),c.last_update_time )))/*Mindate part ends*/))),DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) " +
                                                                    " )/(case when @Gap=0 then 1 else @Gap end) +1)*(SELECT case when m.MessageType = 'B' or m.MessageType='E' then 1 else 0 end )  as Emailcount" +
                                                          ",(DATEDIFF(day,(/*Mindate part starts*/DATEADD(dd, 0, DATEDIFF(dd, 0,DATEADD(day,(-1)*(@gap*(@freq-1)),(SELECT " +
                                                                                              " dateadd(month,(SELECT convert(int,default_value) from parafait_defaults where " +
                                                                                              " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = m.site_Id)),c.last_update_time )))/*Mindate part ends*/))),DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) " +
                                                                    " )/(case when @Gap=0 then 1 else @Gap end) +1)*(SELECT case when m.MessageType = 'B' or m.MessageType='S' then 1 else 0 end )  as SMScount" +
                                                          ",(DATEDIFF(day,(/*Mindate part starts*/DATEADD(dd, 0, DATEDIFF(dd, 0,DATEADD(day,(-1)*(@gap*(@freq-1)),(SELECT " +
                                                                                              " dateadd(month,(SELECT convert(int,default_value) from parafait_defaults where " +
                                                                                              " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = m.site_Id)),c.last_update_time )))/*Mindate part ends*/))),DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) " +
                                                                    " )/(case when @Gap=0 then 1 else @Gap end) +1)*(SELECT case when m.MessageType = 'B' or m.MessageType='A' then 1 else 0 end )  as Notificationcount" +
                                                          ",(SELECT count(customerid) from MessagingRequests mr where mr.customerid=c.customer_id and mr.card_id=c.card_id and DATEADD(dd, 0, DATEDIFF(dd, 0,SendAttemptDate))>= " +
                                                             "(/*Mindate part starts*/DATEADD(dd, 0, DATEDIFF(dd, 0,DATEADD(day,(-1)*(@gap*(@freq-1)),(select " +
                                                                                                                    " dateadd(month,(select top 1 convert(int,default_value) from parafait_defaults where " +
                                                                                                                    " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = isnull(mr.site_Id, -1))),c.last_update_time ))) " +
                                                             "/*Mindate part ends*/))) and mr.MessageType='E'  " +
                                                          " )/(select case when count(1)=0 then 1 else count(1) end from MessagingTriggers where ActiveFlag=1 and TypeCode = 'V')as EmailSentCount " +
                                                          ",(SELECT count(customerid) from MessagingRequests mr where mr.customerid=c.customer_id and mr.card_id=c.card_id and DATEADD(dd, 0, DATEDIFF(dd, 0,SendAttemptDate))>= " +
                                                             "(/*Mindate part starts*/DATEADD(dd, 0, DATEDIFF(dd, 0,DATEADD(day,(-1)*(@gap*(@freq-1)),(select " +
                                                                                                                    " dateadd(month,(select top 1 convert(int,default_value) from parafait_defaults where " +
                                                                                                                    " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = isnull(mr.site_Id, -1))),c.last_update_time ))) " +
                                                             "/*Mindate part ends*/))) and mr.MessageType='S'  " +
                                                          " )/(select case when count(1)=0 then 1 else count(1) end from MessagingTriggers where ActiveFlag=1 and TypeCode = 'V')as SMSSentCount " +
                                                          ",(SELECT count(customerid) from MessagingRequests mr where mr.customerid = c.customer_id and mr.card_id = c.card_id and DATEADD(dd, 0, DATEDIFF(dd, 0, SendAttemptDate)) >= " +
                                                             "(/*Mindate part starts*/DATEADD(dd, 0, DATEDIFF(dd, 0,DATEADD(day,(-1)*(@gap*(@freq-1)),(select " +
                                                                                                                    " dateadd(month,(select top 1 convert(int,default_value) from parafait_defaults where " +
                                                                                                                    " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = isnull(mr.site_Id, -1))),c.last_update_time ))) " +
                                                             "/*Mindate part ends*/))) and mr.MessageType='A'  " +
                                                          " )/(select case when count(1)=0 then 1 else count(1) end from MessagingTriggers where ActiveFlag=1 and TypeCode = 'V')as NotificationSentCount " +
                                           /*Divide the total messages/emails sent with the number of entries in messagingtriggers, since for each entry one message/email will be 
                                           sent*/
                                           " from cards c  " +
                                           " join CustomerView(@PassphraseEnteredByUser) cu " +
                                           " on cu.customer_id = c.customer_id " +
                                           " LEFT OUTER JOIN (SELECT pnd.* , DENSE_RANK() OVER(PARTITION BY pnd.CustomerId ORDER BY pnd.LastUpdateDate DESC, pnd.Id DESC) rnk " +
                                           " FROM PushNotificationDevice pnd WHERE pnd.IsActive = 1) pd ON pd.CustomerId = cu.customer_id and pd.rnk = 1, " +
                                           " MessagingTriggers m " +
                                           " where " +
                                           " m.TypeCode = 'V' " +
                                           " and m.ActiveFlag = 1 and " +
                                           " ((m.MessageType = 'B' and cu.email is not null and cu.contact_phone1 is not null) " +
                                           " OR (m.MessageType = 'E' and cu.email is not null) " +
                                           " OR (m.MessageType = 'S' and cu.contact_phone1 is not null))" +
                                           " and c.valid_flag='Y' and " +
                                           /*select only the entries whose mindate values is less than or equal to today's date*/
                                           " DATEADD(dd, 0, DATEDIFF(dd, 0,DATEADD(day,(-1)*(@gap*(@freq-1)),(select " +
                                           " dateadd(month,(select convert(int,default_value) from parafait_defaults where " +
                                           " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = m.site_Id)),c.last_update_time ))))) " +
                                           " <=DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) and  " +
                                           /*Select only the entries whose expiry date is greater than or equal to today's date*/
                                           "( select " +
                                           " dateadd(month,(select convert(int,default_value) from parafait_defaults where " +
                                           " default_value_name='CARD_VALIDITY' and (isnull(site_id,-1) = -1 or site_id = m.site_Id)),c.last_update_time ))>=DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()) )" +
                                           " and not exists( select mr1.CustomerId from MessagingRequests mr1 where mr1.Reference =m.TypeCode and  mr1.CustomerId=c.customer_id  " +
                                           " and mr1.card_id=c.card_id " +
                                           " and ( DATEADD(dd, 0, DATEDIFF(dd, 0,mr1.SendAttemptDate))=DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) or SendAttemptDate is null ) )";

                            DataTable dtCardExpiry = _utilities.executeDataTable(query, new SqlParameter("@gap", gap), new SqlParameter("@freq", frequency), new SqlParameter("@PassphraseEnteredByUser", passPhrase));


                            log.LogVariableState("@gap", gap);
                            log.LogVariableState("@freq", frequency);

                            try
                            {
                                //// get the App Notification Messaging Client
                                //int messagingClientId = -1;
                                //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(_utilities.ExecutionContext);
                                //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "CARD_EXPIRY", "A");
                                //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                                //    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                                //// Get the app notification template
                                //String notificationTemplate = "";
                                //EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(_utilities.ExecutionContext);
                                //List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                                //searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, "APP_NOTIFICATION_TEMPLATE"));
                                //List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                                //if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                                //{
                                //    notificationTemplate = emailTemplateDTOList[0].EmailTemplate;
                                //}
                                List<MessagingTriggerDTO> messagingTriggerDTOList = new List<MessagingTriggerDTO>();
                                List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
                                List<AccountDTO> accountDTOList = new List<AccountDTO>();
                                foreach (DataRow dr in dtCardExpiry.Rows)
                                {
                                    string messageType = dr["MessageType"].ToString();
                                    int triggerId = GetTriggerId(dr["TriggerId"]);
                                    int customerId = GetCustomerId(dr["Customer_Id"]);
                                    int cardId = GetCardId(dr["Card_Id"]);
                                    MessagingTriggerDTO messagingTriggerDTO = GetMessagingTriggerDTO(messagingTriggerDTOList, triggerId);
                                    CustomerDTO customerDTO = GetCustomerDTO(customerDTOList, customerId);
                                    AccountDTO accountDTO = GetAccountDTO(accountDTOList, cardId);

                                    if (messageType == "B" || messageType == "E")
                                    {
                                        try
                                        {
                                            int emailCount = Convert.ToInt32(dr["Emailcount"]);
                                            int emailSentCount = Convert.ToInt32(dr["EmailSentCount"]);
                                            if (emailCount - emailSentCount > 0 && cardId > -1)
                                            {
                                                writeToLog("Messaging Trigger (Validity Expiry Email): " + dr["TriggerName"].ToString());
                                                if (Convert.ToBoolean(dr["MessageCustomer"]))
                                                {
                                                    string email = dr["Email"].ToString();
                                                    if (regex.IsValidEmail(email) == false || customerId == -1)
                                                    {
                                                        string status = "Email id invalid: " + email + "; Customer: " + dr["Customer_Id"].ToString();
                                                        writeToLog(status);
                                                    }
                                                    else
                                                    {
                                                        writeToLog("Sending email to " + email);
                                                        string body = string.Empty;// dr["EmailTemplate"].ToString();
                                                        //body = ReplaceKeywords(body, dr);
                                                        string AttachFile = null;
                                                        bool firstIteration = false;
                                                        for (int i = emailSentCount; i < emailCount; i++)
                                                        {
                                                            if (firstIteration == false)
                                                            {
                                                                //messaging.SendEMail(email, dr["EmailSubject"].ToString(), body, dr["customer_id"].ToString(), AttachFile, null, null, null, dr["TypeCode"].ToString(), dr["card_id"].ToString());
                                                                CardExpiryMessageTriggerEventBL cardExpiryMessageTriggerEventBL = new CardExpiryMessageTriggerEventBL(_utilities.ExecutionContext, messagingTriggerDTO,
                                                                    customerDTO, accountDTO, null);
                                                                cardExpiryMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL);
                                                                firstIteration = true;
                                                            }
                                                            else
                                                            {
                                                               messaging.SendEMail(email, dr["EmailSubject"].ToString(), body, dr["customer_id"].ToString(), AttachFile, "Success", DateTime.Now.ToString(), "3", "No Email Sent", dr["card_id"].ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            writeToLog("Send email error" + ex.Message);
                                        }
                                    }

                                    if (messageType == "B" || messageType == "S")
                                    {
                                        try
                                        {
                                            int smsCount = Convert.ToInt32(dr["SMScount"]);
                                            int smsSentCount = Convert.ToInt32(dr["SMSSentCount"]);
                                            if (smsCount - smsSentCount > 0)
                                            {
                                                writeToLog("Messaging Trigger (Validity Expiry SMS): " + dr["TriggerName"].ToString());
                                                if (Convert.ToBoolean(dr["MessageCustomer"]))
                                                {
                                                    string phone = dr["Contact_Phone1"].ToString();
                                                    if (string.IsNullOrEmpty(phone.Trim()) || phone.Length < 8)
                                                    {
                                                        string status = "Phone No invalid: " + phone + "; Customer: " + dr["Customer_Id"].ToString();
                                                        writeToLog(status);
                                                    }
                                                    else
                                                    {
                                                        writeToLog("Sending SMS to " + phone);
                                                        string body = string.Empty;//dr["SMSTemplate"].ToString();
                                                        //body = ReplaceKeywords(body, dr);
                                                        bool firstIteration = false;
                                                        for (int i = smsSentCount; i < smsCount; i++)
                                                        {
                                                            if (firstIteration == false)
                                                            {
                                                                //messaging.SendSMS(phone, body, dr["customer_id"].ToString(), null, null, null, dr["TypeCode"].ToString(), dr["card_id"].ToString());
                                                                CardExpiryMessageTriggerEventBL cardExpiryMessageTriggerEventBL = new CardExpiryMessageTriggerEventBL(_utilities.ExecutionContext, messagingTriggerDTO,
                                                                    customerDTO, accountDTO, null);
                                                                cardExpiryMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.SMS);
                                                                firstIteration = true;
                                                            }
                                                            else
                                                            {
                                                                messaging.SendSMS(phone, body, dr["customer_id"].ToString(), "Success", DateTime.Now.ToString(), "3", "No Message Sent", dr["card_id"].ToString()); ;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            writeToLog("Send email error" + ex.Message);
                                        }
                                    }

                                    if (messageType == "B" || messageType == "A")
                                    {
                                        try
                                        {
                                            int notificationCount = Convert.ToInt32(dr["notificationcount"]);
                                            int notificationSentCount = Convert.ToInt32(dr["notificationSentCount"]);
                                            if (notificationCount - notificationSentCount > 0)
                                            {
                                                writeToLog("Messaging Trigger (Validity Expiry Notification): " + dr["TriggerName"].ToString());
                                                //object customerId = dr["Customer_Id"];
                                                if (Convert.ToBoolean(dr["MessageCustomer"]) && customerId != -1)
                                                {
                                                    CardExpiryMessageTriggerEventBL cardExpiryMessageTriggerEventBL = new CardExpiryMessageTriggerEventBL(_utilities.ExecutionContext, messagingTriggerDTO,
                                                                    customerDTO, accountDTO, null);
                                                    cardExpiryMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION);

                                                    //PushNotificationDeviceListBL pushNotificationDeviceListBL = new PushNotificationDeviceListBL(_utilities.ExecutionContext);
                                                    //List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>> pndSearchParameters = new List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>>();
                                                    //pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                                                    //pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.IS_ACTIVE, "1"));
                                                    //List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList = pushNotificationDeviceListBL.GetPushNotificationDeviceDTOList(pndSearchParameters);

                                                    //if (pushNotificationDeviceDTOList == null || !pushNotificationDeviceDTOList.Any())
                                                    //{
                                                    //    string status = "Notification token is invalid: " + "; Customer: " + dr["Customer_Id"].ToString();
                                                    //    writeToLog(status);
                                                    //}
                                                    //else
                                                    //{
                                                    //    String reference = "C" + dr["card_number"];
                                                    //    //Add entry for 1 device only. The app notification method takes care of sending to all active device
                                                    //    PushNotificationDeviceDTO pndDTO = pushNotificationDeviceDTOList[0];
                                                    //    {
                                                    //        String token = pndDTO.PushNotificationToken;
                                                    //        writeToLog("Sending App notification to " + token);
                                                    //        string body = dr["SMSTemplate"].ToString();
                                                    //        body = ReplaceKeywords(body, dr);

                                                    //        if (!String.IsNullOrEmpty(notificationTemplate))
                                                    //        {
                                                    //            String tempString = notificationTemplate;
                                                    //            tempString = tempString.Replace("@notificationSubject", dr["EmailSubject"].ToString());
                                                    //            tempString = tempString.Replace("@notificationBody", body);
                                                    //            tempString = tempString.Replace("@CardNumber", dr["card_number"].ToString());
                                                    //            tempString = tempString.Replace("@notificationType", "CARD");
                                                    //            //tempString = tempString.Replace("@ToDevice", token);


                                                    //            MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, reference, "A", "", "", "", null, null, null, null,
                                                    //                dr["EmailSubject"].ToString(), tempString, Convert.ToInt32(dr["Customer_Id"].ToString()), Convert.ToInt32(dr["card_id"].ToString()), "", true, "", "", messagingClientId, false, token);
                                                    //            MessagingRequestBL messagingRequestBL = new MessagingRequestBL(_utilities.ExecutionContext, messagingRequestDTO);
                                                    //            messagingRequestBL.Save();
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            string status = "Notification template is not set APP_NOTIFICATION_TEMPLATE";
                                                    //            writeToLog(status);
                                                    //        }
                                                    //    }
                                                    //}
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            writeToLog("Send email error" + ex.Message);
                                        }
                                    }

                                    if (messageType == "B" || messageType == "W")
                                    {
                                        writeToLog("Messaging Trigger (Validity Expiry WhatsApp Message): " + dr["TriggerName"].ToString());
                                        
                                        if (Convert.ToBoolean(dr["MessageCustomer"]) && customerId != null && !String.IsNullOrEmpty(customerId.ToString()))
                                        {
                                            try
                                            {
                                                string phone = dr["Contact_Phone1"].ToString();
                                                if (string.IsNullOrEmpty(phone.Trim()) || phone.Length < 8)
                                                {
                                                    string status = "Phone No invalid: " + phone + "; Customer: " + dr["Customer_Id"].ToString();
                                                    writeToLog(status);
                                                }
                                                else
                                                {
                                                    CustomerBL customerBL = new CustomerBL(_executionContext, Convert.ToInt32(dr["Customer_Id"]));
                                                    if (customerBL.CustomerDTO != null && customerBL.CustomerDTO.ContactDTOList != null && customerBL.CustomerDTO.ProfileDTO != null)
                                                    {
                                                        if (customerBL.CustomerDTO.ProfileDTO.OptOutWhatsApp == false)
                                                        {
                                                            int index = customerBL.CustomerDTO.ContactDTOList.FindIndex(x => x.Attribute1 == dr["Contact_phone1"].ToString());
                                                            bool whatsAppEnabled = (index == -1 ? false : customerBL.CustomerDTO.ContactDTOList[index].WhatsAppEnabled);
                                                            log.LogVariableState("whatsAppEnabled", whatsAppEnabled);

                                                            if (whatsAppEnabled)
                                                            {
                                                                string body = string.Empty;
                                                                try
                                                                {
                                                                    CardExpiryMessageTriggerEventBL cardExpiryMessageTriggerEventBL = new CardExpiryMessageTriggerEventBL(_utilities.ExecutionContext, messagingTriggerDTO,
                                                                   customerDTO, accountDTO, null);
                                                                    cardExpiryMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE);

                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    writeToLog(ex.Message + ":" + body);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                string status = "Customer number is not enabled for whatsApp";
                                                                log.Debug(status);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string status = "User not opt for whatsApp Messaging";
                                                            log.Debug(status);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string status = "Customer details are missing, skipping whatsApp message";
                                                        log.Debug(status);
                                                    }
                                                }
                                                
                                            }
                                            catch (Exception ex)
                                            {
                                                writeToLog("Error occured while card expiry message " + ex.Message);
                                                log.Error("Error occured while card expiry message ", ex);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                writeToLog("Error occured while sending card validity expiry message" + ex.Message);
                                log.Error("Error occured while sending card validity expiry message", ex);
                                log.LogMethodExit(ex.Message);
                                _utilities.EventLog.logEvent("MessagingTrigger", 'E', "Error occured while sending card validity expiry message: " + ex.Message, "MessagingTriggerLog", "CardExpiry", 0);
                                return ex.Message;
                            }
                        }

                    }
                    //The changes on 8-06-2015-end
                }

                log.LogMethodExit("Success");
                return "Success";
            }
            catch (Exception ex)
            {
                log.Error("Error occured when reading from Message Queue", ex);
                log.LogMethodExit(ex.Message);
                _utilities.EventLog.logEvent("MessagingTrigger", 'E', "Error occured when reading from Message Queue: " + ex.Message, "MessagingTriggerLog", "MessagingTriggers", 0);
                return ex.Message;
            }
        }

        //private string ReplaceKeywords(string body, DataRow dr)
        //{
        //    log.LogMethodEntry(body, dr);

        //    if (dr.Table.Columns.Contains("first_name"))
        //        body = body.Replace("@CustomerName", dr["first_name"].ToString());
        //    else
        //        body = body.Replace("@CustomerName", "");

        //    if (dr.Table.Columns.Contains("email"))
        //        body = body.Replace("@EmailId", dr["email"].ToString());
        //    else
        //        body = body.Replace("@EmailId", "");

        //    if (dr.Table.Columns.Contains("card_number"))
        //        body = body.Replace("@CardNumber", dr["card_number"].ToString());
        //    else
        //        body = body.Replace("@CardNumber", "");

        //    if (dr.Table.Columns.Contains("TrxNetAmount") && dr["TrxNetAmount"] != DBNull.Value)
        //        body = body.Replace("@TrxAmount", Convert.ToDouble(dr["TrxNetAmount"]).ToString("N2"));
        //    else
        //        body = body.Replace("@TrxAmount", "");

        //    if (dr.Table.Columns.Contains("TrxDate") && dr["TrxDate"] != DBNull.Value)
        //        body = body.Replace("@TrxDate", Convert.ToDateTime(dr["TrxDate"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT));
        //    else
        //        body = body.Replace("@TrxDate", "");

        //    if (dr.Table.Columns.Contains("credits") && dr["credits"] != DBNull.Value)
        //        body = body.Replace("@CardCredits", Convert.ToDouble(dr["credits"]).ToString(_utilities.ParafaitEnv.AMOUNT_FORMAT));
        //    else
        //        body = body.Replace("@CardCredits", "");

        //    if (dr.Table.Columns.Contains("Expirydate") && dr["Expirydate"] != DBNull.Value)
        //        body = body.Replace("@CardExpiry", (string.IsNullOrEmpty(dr["Expirydate"].ToString()) ? "" : Convert.ToDateTime(dr["Expirydate"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT)));
        //    else
        //        body = body.Replace("@CardExpiry", "");

        //    if (dr.Table.Columns.Contains("ValidityinDays") && dr["ValidityinDays"] != DBNull.Value)
        //        body = body.Replace("@ValidityInDays", Convert.ToDouble(dr["ValidityinDays"]).ToString());
        //    else
        //        body = body.Replace("@ValidityInDays", "");

        //    if (dr.Table.Columns.Contains("RedemptionOrderNo"))
        //        body = body.Replace("@RedemptionOrderNo", dr["RedemptionOrderNo"].ToString());
        //    else
        //        body = body.Replace("@RedemptionOrderNo", "");

        //    if (dr.Table.Columns.Contains("redeemed_date") && dr["redeemed_date"] != DBNull.Value)
        //        body = body.Replace("@RedeemedDate", Convert.ToDateTime(dr["redeemed_date"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT));
        //    else
        //        body = body.Replace("@RedeemedDate", "");

        //    if (dr.Table.Columns.Contains("preparedDate") && dr["preparedDate"] != DBNull.Value)
        //        body = body.Replace("@RedemptionPreparedDate", Convert.ToDateTime(dr["preparedDate"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT));
        //    else
        //        body = body.Replace("@RedemptionPreparedDate", "");

        //    if (dr.Table.Columns.Contains("deliveredDate") && dr["deliveredDate"] != DBNull.Value)
        //        body = body.Replace("@RedemptionDeliveryDate", Convert.ToDateTime(dr["deliveredDate"]).ToString(_utilities.ParafaitEnv.DATE_FORMAT));
        //    else
        //        body = body.Replace("@RedemptionDeliveryDate", "");
        //    log.LogMethodExit(body);
        //    return body;
        //}

        //public static Bitmap getTrxReceipt(Bitmap bitmap, Semnox.Parafait.Printer.ReceiptClass ReceiptContent, Graphics eGraphics)
        //{
        //    log.LogMethodEntry(bitmap, ReceiptContent, eGraphics);

        //    int receiptLineIndex = 0;
        //    StringFormat stringFormat = new StringFormat();
        //    stringFormat.Alignment = StringAlignment.Center;
        //    stringFormat.FormatFlags = StringFormatFlags.NoClip;
        //    int receiptWidth = (int)eGraphics.VisibleClipBounds.Width;
        //    int xPos = 0;
        //    int lineHeight = 20;
        //    int heightOnPage = 24;
        //    int totalLines = ReceiptContent.TotalLines;
        //    float[] colWidth = new float[5];

        //    eGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        //    eGraphics.TextContrast = 0;

        //    while (receiptLineIndex < totalLines)
        //    {
        //        switch (ReceiptContent.ReceiptLines[receiptLineIndex].colCount)
        //        {
        //            case 1: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 1.0F; break;
        //            case 2: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.5F; break;
        //            case 3: colWidth[0] = receiptWidth * 0.5F; colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.25F; break;
        //            case 4:
        //                colWidth[0] = receiptWidth * .45F;
        //                colWidth[1] = receiptWidth * .15F;
        //                colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
        //            case 5:
        //                colWidth[0] = receiptWidth * .40F;
        //                colWidth[1] = receiptWidth * .15F;
        //                colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.15F; break;
        //            default: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
        //        }

        //        float cumWidth = 0;
        //        lineHeight = 20;
        //        for (int j = 0; j < 5; j++)
        //        {
        //            if (ReceiptContent.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || ReceiptContent.ReceiptLines[receiptLineIndex].Alignment[j] == null)
        //                continue;
        //            lineHeight = Math.Max(lineHeight, (int)Math.Ceiling(eGraphics.MeasureString(ReceiptContent.ReceiptLines[receiptLineIndex].Data[j], ReceiptContent.ReceiptLines[receiptLineIndex].LineFont, (int)colWidth[j]).Height));
        //        }

        //        if (ReceiptContent.ReceiptLines[receiptLineIndex].BarCode != null)
        //        {
        //            Bitmap img = new Bitmap(ReceiptContent.ReceiptLines[receiptLineIndex].BarCode);

        //            eGraphics.DrawImage(img, (receiptWidth - img.Width) / 2, heightOnPage);
        //            heightOnPage += img.Height;

        //            for (int j = 0; j < 5; j++)
        //            {
        //                if (ReceiptContent.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || ReceiptContent.ReceiptLines[receiptLineIndex].Alignment[j] == null)
        //                    continue;
        //                eGraphics.DrawString(ReceiptContent.ReceiptLines[receiptLineIndex].Data[j], ReceiptContent.ReceiptLines[receiptLineIndex].LineFont, Brushes.Black, (int)(xPos + cumWidth), heightOnPage);
        //                heightOnPage += 3;
        //            }
        //        }
        //        else
        //        {
        //            for (int j = 0; j < 5; j++)
        //            {
        //                if (ReceiptContent.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || ReceiptContent.ReceiptLines[receiptLineIndex].Alignment[j] == null)
        //                    continue;
        //                switch (ReceiptContent.ReceiptLines[receiptLineIndex].Alignment[j])
        //                {
        //                    case "L": stringFormat.Alignment = StringAlignment.Near; break;
        //                    case "R": stringFormat.Alignment = StringAlignment.Far; break;
        //                    case "C": stringFormat.Alignment = StringAlignment.Center; break;
        //                    default: stringFormat.Alignment = StringAlignment.Near; break;
        //                }
        //                try
        //                {
        //                    if (ReceiptContent.ReceiptLines[receiptLineIndex + 1].Data[j].StartsWith("--")) // heading
        //                        stringFormat.FormatFlags = StringFormatFlags.NoClip;
        //                    else if (ReceiptContent.ReceiptLines[receiptLineIndex].Data[j].StartsWith("--")) // -- below heading
        //                        stringFormat.FormatFlags = StringFormatFlags.NoWrap;
        //                    else
        //                        stringFormat.FormatFlags = StringFormatFlags.NoClip;
        //                }
        //                catch (Exception ex)
        //                {
        //                    log.Error("Error occured when setting up Format Flags", ex);
        //                }

        //                eGraphics.DrawString(ReceiptContent.ReceiptLines[receiptLineIndex].Data[j], ReceiptContent.ReceiptLines[receiptLineIndex].LineFont, Brushes.Black, new Rectangle((int)(xPos + cumWidth), heightOnPage, (int)(colWidth[j]), lineHeight), stringFormat);
        //                cumWidth += colWidth[j];
        //            }
        //        }
        //        receiptLineIndex++;
        //        heightOnPage += lineHeight;
        //    }

        //    Bitmap b = new Bitmap(bitmap.Width, heightOnPage);
        //    using (Graphics g = Graphics.FromImage(b))
        //    {
        //        g.Clear(Color.White);
        //        g.DrawImageUnscaled(bitmap, new Point(0, 0));
        //    }
        //    log.LogMethodExit(b); ;
        //    return b;
        //}

        private void ProcessOnPurchaseMessaingTrigger(DateTime timeStamp)
        {
            log.LogMethodEntry(timeStamp);
            object trxId = -1;
            int customerId = -1;
            try
            {
                DataTable dtTriggers = _utilities.executeDataTable(@"exec MessagingTriggersOnPurchase @ToTimeStamp = @toTimeStamp , @PassphraseEnteredByUser = @Passphrase",
                                                                            new SqlParameter("@toTimeStamp", timeStamp), new SqlParameter("@Passphrase", passPhrase));
                log.LogVariableState("@toTimeStamp", timeStamp);

                RegexUtilities regex = new RegexUtilities();
                Messaging messaging = new Messaging(_utilities);

                //int messagingClientId = -1;
                //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(_utilities.ExecutionContext);
                //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "PURCHASE", "A");
                //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                //    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                //String notificationTemplate = "";
                //EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(_utilities.ExecutionContext);
                //List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, "APP_NOTIFICATION_TEMPLATE"));
                //List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                //if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                //{
                //    notificationTemplate = emailTemplateDTOList[0].EmailTemplate;
                //}

                List<MessagingTriggerDTO> messagingTriggerDTOList = new List<MessagingTriggerDTO>();

                foreach (DataRow dr in dtTriggers.Rows)
                {
                    String trxReference = "T" + dr["TrxId"].ToString();
                    writeToLog("Messaging Trigger: " + dr["TriggerName"].ToString() + "; Trx id: " + dr["TrxId"].ToString());
                    trxId = dr["TrxId"];
                    int triggerId = GetTriggerId(dr["TriggerId"]);
                    MessagingTriggerDTO messagingTriggerDTO = GetMessagingTriggerDTO(messagingTriggerDTOList, triggerId);
                    customerId = GetCustomerId(dr["Customer_Id"]);

                    Semnox.Parafait.Transaction.Transaction Trx = null;//Delay the trx creation untill we are sure of processing the record

                    string messageType = dr["MessageType"].ToString();
                    if (messageType == "B" || messageType == "E")
                    {
                        try
                        {
                            if (Convert.ToBoolean(dr["MessageCustomer"]))
                            {
                                string email = dr["Email"].ToString();
                                if (regex.IsValidEmail(email) == false)
                                {
                                    string status = "Email id invalid: " + email + "; Customer: " + dr["Customer_Id"].ToString();
                                    writeToLog(status);
                                }
                                else
                                {
                                    Trx = GetTransaction(Convert.ToInt32(dr["TrxId"]), Trx);
                                    writeToLog("Sending email to " + email);
                                    PurchaseMessageTriggerEventBL purchaseMessageTriggerEventBL = new PurchaseMessageTriggerEventBL(_utilities.ExecutionContext, _utilities, messagingTriggerDTO, Trx);
                                    purchaseMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL);
                                    //string body = dr["EmailTemplate"].ToString();
                                    //body = ReplaceKeywords(body, dr);
                                    //string AttachFile = null;

                                    //if (dr["ReceiptTemplateId"].Equals(DBNull.Value) == false)
                                    //{
                                    //    TransactionUtils trxUtils = new TransactionUtils(_utilities);
                                    //    Semnox.Parafait.Transaction.Transaction Trx = trxUtils.CreateTransactionFromDB(Convert.ToInt32(dr["TrxId"]), _utilities, true);
                                    //    Trx.TransactionInfo.createTransactionInfo(Trx.Trx_id);

                                    //    PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1);
                                    //    //get default value for Receipt Template ID based on configuration RECEIPT_PRINT_TEMPLATE
                                    //    int printTemplateId = Convert.ToInt32(dr["ReceiptTemplateId"]);
                                    //    ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateHeaderBL(_utilities.ExecutionContext, printTemplateId, true).ReceiptPrintTemplateHeaderDTO;
                                    //    Semnox.Parafait.POS.POSPrinterDTO posPrinterDTO = new Semnox.Parafait.POS.POSPrinterDTO(-1, _utilities.ParafaitEnv.POSMachineId, -1, -1, -1, -1, printTemplateId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);


                                    //    Semnox.Parafait.Printer.ReceiptClass Content = POSPrint.PrintReceipt(Trx, posPrinterDTO, false);

                                    //    Bitmap receipt = new Bitmap(300, 2000);
                                    //    receipt = getTrxReceipt(receipt, Content, Graphics.FromImage(receipt));
                                    //    string contentID = Guid.NewGuid().ToString() + ".gif";
                                    //    AttachFile = imageFolder + contentID;
                                    //    receipt.Save(AttachFile, System.Drawing.Imaging.ImageFormat.Gif);

                                    //    body += "<img src=\"cid:" + contentID + "\">";
                                    //}
                                    ////messaging.SendEMail(email, dr["EmailSubject"].ToString(), body, AttachFile); //previous code
                                    //messaging.SendEMail(email, dr["EmailSubject"].ToString(), body, dr["customer_id"].ToString(), AttachFile); //New parameter-customer id is added on 04-06-2015
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            writeToLog("Error occured while transaction message " + ex.Message);
                            log.Error("Error occured while transaction message ", ex);
                        }
                    }

                    if (messageType == "B" || messageType == "S")
                    {
                        if (Convert.ToBoolean(dr["MessageCustomer"]))
                        {
                            try
                            {
                                string phone = dr["Contact_Phone1"].ToString();
                                if (string.IsNullOrEmpty(phone.Trim()) || phone.Length < 8)
                                {
                                    string status = "Phone No invalid: " + phone + "; Customer: " + dr["Customer_Id"].ToString();
                                    writeToLog(status);
                                }
                                else
                                {
                                    Trx = GetTransaction(Convert.ToInt32(dr["TrxId"]), Trx);
                                    writeToLog("Sending SMS to " + phone);
                                    PurchaseMessageTriggerEventBL purchaseMessageTriggerEventBL = new PurchaseMessageTriggerEventBL(_utilities.ExecutionContext, _utilities, messagingTriggerDTO, Trx);
                                    purchaseMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.SMS);
                                    //string body = dr["SMSTemplate"].ToString();
                                    //body = ReplaceKeywords(body, dr);

                                    ////messaging.SendSMS(phone, body); //previous code-b4-4-06-2015
                                    //messaging.SendSMS(phone, body, dr["customer_id"].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                writeToLog("Error occured while transaction message " + ex.Message);
                                log.Error("Error occured while transaction message ", ex);
                            }
                        }
                    }

                    if (messageType == "B" || messageType == "A")
                    {
                        if (Convert.ToBoolean(dr["MessageCustomer"]) && customerId > -1)
                        {
                            try
                            {
                                Trx = GetTransaction(Convert.ToInt32(dr["TrxId"]), Trx);
                                PurchaseMessageTriggerEventBL purchaseMessageTriggerEventBL = new PurchaseMessageTriggerEventBL(_utilities.ExecutionContext, _utilities, messagingTriggerDTO, Trx);
                                purchaseMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION);

                                //PushNotificationDeviceListBL pushNotificationDeviceListBL = new PushNotificationDeviceListBL(_utilities.ExecutionContext);
                                //List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>> pndSearchParameters = new List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>>();
                                //pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                                //pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.IS_ACTIVE, "1"));

                                //List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList = pushNotificationDeviceListBL.GetPushNotificationDeviceDTOList(pndSearchParameters);

                                //if (pushNotificationDeviceDTOList == null || !pushNotificationDeviceDTOList.Any())
                                //{
                                //    string status = "Notification token is invalid: " + "; Customer: " + dr["Customer_Id"].ToString();
                                //    writeToLog(status);
                                //}
                                //else
                                //{
                                //    //Add entry for 1 device only. The app notification method takes care of sending to all active device
                                //    PushNotificationDeviceDTO pndDTO = pushNotificationDeviceDTOList[0];
                                //    {
                                //        String token = pndDTO.PushNotificationToken;
                                //        writeToLog("Sending App notification to " + token);
                                //        string body = dr["SMSTemplate"].ToString();
                                //        body = ReplaceKeywords(body, dr);

                                //        if (!String.IsNullOrEmpty(notificationTemplate))
                                //        {
                                //            String tempString = notificationTemplate;
                                //            tempString = tempString.Replace("@notificationSubject", dr["EmailSubject"].ToString());
                                //            tempString = tempString.Replace("@notificationBody", body);
                                //            tempString = tempString.Replace("@transactionId", dr["TrxId"].ToString());
                                //            tempString = tempString.Replace("@notificationType", "TRX");
                                //            //tempString = tempString.Replace("@ToDevice", token);

                                //            try
                                //            {
                                //                MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, trxReference, "A", "", "", null, null, null, null, null,
                                //                    dr["EmailSubject"].ToString(), tempString, Convert.ToInt32(dr["Customer_Id"].ToString()), null, "", true, "", "", messagingClientId, false, token);
                                //                MessagingRequestBL messagingRequestBL = new MessagingRequestBL(_utilities.ExecutionContext, messagingRequestDTO);
                                //                messagingRequestBL.Save();
                                //                Thread.Sleep(100);
                                //            }
                                //            catch (Exception ex)
                                //            {
                                //                writeToLog(ex.Message + ":" + tempString);
                                //            }
                                //        }
                                //        else
                                //        {
                                //            string status = "Notification template is not set APP_NOTIFICATION_TEMPLATE";
                                //            writeToLog(status);
                                //        }
                                //    }
                                //}
                            }
                            catch (Exception ex)
                            {
                                writeToLog("Error occured while transaction message " + ex.Message);
                                log.Error("Error occured while transaction message ", ex);
                            }
                        }
                    }

                    if (messageType == "B" || messageType == "W")
                    {
                        if (Convert.ToBoolean(dr["MessageCustomer"]) && customerId > -1)
                        {
                            try
                            {
                                string phone = dr["Contact_Phone1"].ToString();
                                if (string.IsNullOrEmpty(phone.Trim()) || phone.Length < 8)
                                {
                                    string status = "Phone No invalid: " + phone + "; Customer: " + dr["Customer_Id"].ToString();
                                    writeToLog(status);
                                }
                                else
                                {
                                    CustomerBL customerBL = new CustomerBL(_executionContext, customerId);
                                    if (customerBL.CustomerDTO != null && customerBL.CustomerDTO.ContactDTOList != null && customerBL.CustomerDTO.ProfileDTO != null)
                                    {
                                        if (customerBL.CustomerDTO.ProfileDTO.OptOutWhatsApp == false)
                                        {
                                            int index = customerBL.CustomerDTO.ContactDTOList.FindIndex(x => x.Attribute1 == dr["Contact_phone1"].ToString());
                                            bool whatsAppEnabled = (index == -1 ? false : customerBL.CustomerDTO.ContactDTOList[index].WhatsAppEnabled);
                                            if (whatsAppEnabled)
                                            {
                                                Trx = GetTransaction(Convert.ToInt32(dr["TrxId"]), Trx);
                                                PurchaseMessageTriggerEventBL purchaseMessageTriggerEventBL = new PurchaseMessageTriggerEventBL(_utilities.ExecutionContext, _utilities, messagingTriggerDTO, Trx);
                                                purchaseMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE);

                                                //messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(_utilities.ExecutionContext);
                                                //messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "PURCHASE", "W");
                                                //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                                                //    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                                                //string body = dr["SMSTemplate"].ToString();
                                                //body = ReplaceKeywords(body, dr);

                                                //if (!String.IsNullOrEmpty(body))
                                                //{


                                                //    try
                                                //    {
                                                //        MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, trxReference, "W", "", dr["Contact_phone1"].ToString(), null, null, null, null, null,
                                                //            dr["EmailSubject"].ToString(), body, Convert.ToInt32(dr["Customer_Id"].ToString()), null, "", true, "", "", messagingClientId, false, "");
                                                //        MessagingRequestBL messagingRequestBL = new MessagingRequestBL(_utilities.ExecutionContext, messagingRequestDTO);
                                                //        messagingRequestBL.Save();
                                                //        Thread.Sleep(100);
                                                //    }
                                                //    catch (Exception ex)
                                                //    {
                                                //        writeToLog(ex.Message + ":" + body);
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    string status = "No Message defined";
                                                //    writeToLog(status);
                                                //}
                                            }
                                            else
                                            {
                                                string status = "Customer number is not enabled for whatsApp";
                                                log.Debug(status);
                                            }
                                        }
                                        else
                                        {
                                            string status = "User not opt for whatsApp Messaging";
                                            log.Debug(status);
                                        }
                                    }
                                    else
                                    {
                                        string status = "Customer details are missing, skipping whatsApp message";
                                        log.Debug(status);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                writeToLog("Error occured while transaction message " + ex.Message);
                                log.Error("Error occured while transaction message ", ex);
                            }
                        }
                    }

                    _utilities.executeNonQuery(@"update MessagingTriggers set Timestamp = @timestamp where TriggerId = @triggerId",
                                                new SqlParameter("@timestamp", dr["TrxDate"]),
                                                new SqlParameter("@triggerId", dr["TriggerId"]));

                    log.LogVariableState("@timestamp", dr["TrxDate"]);
                    log.LogVariableState("@triggerId", dr["TriggerId"]);

                    trxId = -1;
                    customerId = -1;
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                _utilities.EventLog.logEvent("MessagingTrigger", 'E', "Error while processing purchase message trigger for TrxId [" +
                                              trxId.ToString() + "], CustomerId [" + customerId.ToString() + "] : " + ex.Message, "MessagingTriggerLog", "OnPurchase", 0);
            }
        }


        private void ProcessRedemptionMessaingTrigger(DateTime timeStamp)
        {
            log.LogMethodEntry(timeStamp);
            ProcessRedemptionStatusMessaingTrigger(timeStamp, RedemptionDTO.RedemptionStatusEnum.OPEN.ToString());
            ProcessRedemptionStatusMessaingTrigger(timeStamp, RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString());
            ProcessRedemptionStatusMessaingTrigger(timeStamp, RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString());
            log.LogMethodExit();
        }


        private void ProcessRedemptionStatusMessaingTrigger(DateTime timeStamp, string redemptionStatus)
        {
            log.LogMethodEntry(timeStamp, redemptionStatus);
            object redemptionId = -1;
            int customerId = -1;
            try
            {
                DataTable dtTriggers = _utilities.executeDataTable(@"exec MessagingTriggersRedemptionStatus @ToTimeStamp = @toTimeStamp , @RedemptionStatus = @redemptionStatus, @PassphraseEnteredByUser = @Passphrase",
                                                                           new SqlParameter("@toTimeStamp", timeStamp), new SqlParameter("@RedemptionStatus", redemptionStatus), new SqlParameter("@Passphrase", passPhrase));

                log.LogVariableState("@toTimeStamp", timeStamp);

                RegexUtilities regex = new RegexUtilities();
                Messaging messaging = new Messaging(_utilities);

                //int messagingClientId = -1;
                //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(_utilities.ExecutionContext);
                //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", "REDEMPTION", "A");
                //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                //    messagingClientId = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                //String notificationTemplate = "";
                //EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(_utilities.ExecutionContext);
                //List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, "APP_NOTIFICATION_TEMPLATE"));
                //List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                //if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                //{
                //    notificationTemplate = emailTemplateDTOList[0].EmailTemplate;
                //}
                List<MessagingTriggerDTO> messagingTriggerDTOList = new List<MessagingTriggerDTO>();
                List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
                List<AccountDTO> accountDTOList = new List<AccountDTO>();
                foreach (DataRow dr in dtTriggers.Rows)
                {
                    writeToLog("Messaging Trigger: " + dr["TriggerName"].ToString() + "; Redemption id: " + dr["redemption_id"].ToString());
                    redemptionId = dr["redemption_id"];
                    customerId = GetCustomerId(dr["Customer_Id"]);
                    if (customerId > -1)
                    {
                        int triggerId = GetTriggerId(dr["TriggerId"]);
                        MessagingTriggerDTO messagingTriggerDTO = GetMessagingTriggerDTO(messagingTriggerDTOList, triggerId);
                        RedemptionDTO redemptionDTO = GetRedemptionDTO(redemptionId);
                        CustomerDTO customerDTO = GetCustomerDTO(customerDTOList, redemptionDTO.CustomerId);
                        AccountDTO accountDTO = GetAccountDTO(accountDTOList, redemptionDTO.CardId);

                        string messageType = dr["MessageType"].ToString();
                        if (messageType == "B" || messageType == "E")
                        {
                            if (Convert.ToBoolean(dr["MessageCustomer"]))
                            {
                                try
                                {
                                    string email = dr["Email"].ToString();
                                    if (regex.IsValidEmail(email) == false)
                                    {
                                        string status = "Email id invalid: " + email + "; Customer: " + dr["Customer_Id"].ToString();
                                        writeToLog(status);
                                    }
                                    else
                                    {
                                        writeToLog("Sending email to " + email);
                                        RedemptionMessageTriggerEventBL redemptionMessageTriggerEventBL = new RedemptionMessageTriggerEventBL(_utilities, _utilities.ExecutionContext, messagingTriggerDTO, redemptionDTO, customerDTO, accountDTO);
                                        redemptionMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL);
                                        //string body = dr["EmailTemplate"].ToString();
                                        //body = ReplaceKeywords(body, dr);
                                        //string AttachFile = null;

                                        //if (dr["ReceiptTemplateId"].Equals(DBNull.Value) == false)
                                        //{
                                        //    PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1);
                                        //    //get default value for Receipt Template ID based on configuration RECEIPT_PRINT_TEMPLATE
                                        //    int printTemplateId = Convert.ToInt32(dr["ReceiptTemplateId"]);
                                        //    ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateHeaderBL(_utilities.ExecutionContext, printTemplateId, true).ReceiptPrintTemplateHeaderDTO;
                                        //    Semnox.Parafait.POS.POSPrinterDTO posPrinterDTO = new Semnox.Parafait.POS.POSPrinterDTO(-1, _utilities.ParafaitEnv.POSMachineId, -1, -1, -1, -1, printTemplateId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);

                                        //    PrintRedemptionReceipt rdsReceipt = new PrintRedemptionReceipt(_utilities.ExecutionContext, _utilities);
                                        //    RedemptionBL redemptionBL = new RedemptionBL(Convert.ToInt32(dr["redemption_id"]), _utilities.ExecutionContext);
                                        //    Semnox.Parafait.Printer.ReceiptClass Content = rdsReceipt.GenerateRedemptionReceipt(redemptionBL, Convert.ToInt32(dr["ReceiptTemplateId"]), posPrinterDTO);

                                        //    Bitmap receipt = new Bitmap(300, 2000);
                                        //    receipt = getTrxReceipt(receipt, Content, Graphics.FromImage(receipt));
                                        //    string contentID = Guid.NewGuid().ToString() + ".gif";
                                        //    AttachFile = imageFolder + contentID;
                                        //    receipt.Save(AttachFile, System.Drawing.Imaging.ImageFormat.Gif);

                                        //    body += "<img src=\"cid:" + contentID + "\">";
                                        //}
                                        ////messaging.SendEMail(email, dr["EmailSubject"].ToString(), body, AttachFile); //previous code
                                        //messaging.SendEMail(email, dr["EmailSubject"].ToString(), body, dr["customer_id"].ToString(), AttachFile); //New parameter-customer id is added on 04-06-2015
                                    }
                                }
                                catch (Exception ex)
                                {
                                    writeToLog("Error occured while sending redemption message " + ex.Message);
                                    log.Error("Error occured while sending redemption message ", ex);
                                }
                            }
                        }

                        if (messageType == "B" || messageType == "S")
                        {
                            if (Convert.ToBoolean(dr["MessageCustomer"]))
                            {
                                try
                                {
                                    string phone = dr["Contact_Phone1"].ToString();
                                    if (string.IsNullOrEmpty(phone.Trim()) || phone.Length < 8)
                                    {
                                        string status = "Phone No invalid: " + phone + "; Customer: " + dr["Customer_Id"].ToString();
                                        writeToLog(status);
                                    }
                                    else
                                    {
                                        writeToLog("Sending SMS to " + phone);
                                        RedemptionMessageTriggerEventBL redemptionMessageTriggerEventBL = new RedemptionMessageTriggerEventBL(_utilities, _utilities.ExecutionContext, messagingTriggerDTO, redemptionDTO, customerDTO, accountDTO);
                                        redemptionMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.SMS);
                                        //string body = dr["SMSTemplate"].ToString();
                                        //body = ReplaceKeywords(body, dr);

                                        ////messaging.SendSMS(phone, body); //previous code-b4-4-06-2015
                                        //messaging.SendSMS(phone, body, dr["customer_id"].ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    writeToLog("Error occured while sending redemption message " + ex.Message);
                                    log.Error("Error occured while sending redemption message ", ex);
                                }
                            }
                        }

                        if (messageType == "B" || messageType == "A")
                        {
                            if (Convert.ToBoolean(dr["MessageCustomer"]) && customerId > -1)
                            {
                                try
                                {

                                    RedemptionMessageTriggerEventBL redemptionMessageTriggerEventBL = new RedemptionMessageTriggerEventBL(_utilities, _utilities.ExecutionContext, messagingTriggerDTO, redemptionDTO, customerDTO, accountDTO);
                                    redemptionMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.APP_NOTIFICATION);

                                    //PushNotificationDeviceListBL pushNotificationDeviceListBL = new PushNotificationDeviceListBL(_utilities.ExecutionContext);
                                    //List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>> pndSearchParameters = new List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>>();
                                    //pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                                    //pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.IS_ACTIVE, "1"));

                                    //List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList = pushNotificationDeviceListBL.GetPushNotificationDeviceDTOList(pndSearchParameters);

                                    //if (pushNotificationDeviceDTOList == null || !pushNotificationDeviceDTOList.Any())
                                    //{
                                    //    string status = "Notification token is invalid: " + "; Customer: " + dr["Customer_Id"].ToString();
                                    //    writeToLog(status);
                                    //}
                                    //else
                                    //{
                                    //    String reference = "R" + dr["redemption_id"].ToString();
                                    //    //Add entry for 1 device only. The app notification method takes care of sending to all active device
                                    //    PushNotificationDeviceDTO pndDTO = pushNotificationDeviceDTOList[0];
                                    //    {
                                    //        String token = pndDTO.PushNotificationToken;
                                    //        writeToLog("Sending App notification to " + token);
                                    //        string body = dr["SMSTemplate"].ToString();
                                    //        body = ReplaceKeywords(body, dr);

                                    //        if (!String.IsNullOrEmpty(notificationTemplate))
                                    //        {
                                    //            String tempString = notificationTemplate;
                                    //            tempString = tempString.Replace("@notificationSubject", dr["EmailSubject"].ToString());
                                    //            tempString = tempString.Replace("@notificationBody", body);
                                    //            tempString = tempString.Replace("@transactionId", dr["redemption_id"].ToString());
                                    //            tempString = tempString.Replace("@notificationType", "REDEMPTION");
                                    //            //tempString = tempString.Replace("@ToDevice", token);

                                    //            MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, reference, "A", "", "", "", null, null, null, null,
                                    //                dr["EmailSubject"].ToString(), tempString, Convert.ToInt32(dr["Customer_Id"].ToString()), null, "", true, "", "", messagingClientId, false, token);
                                    //            MessagingRequestBL messagingRequestBL = new MessagingRequestBL(_utilities.ExecutionContext, messagingRequestDTO);
                                    //            messagingRequestBL.Save();
                                    //        }
                                    //        else
                                    //        {
                                    //            string status = "Notification template is not set APP_NOTIFICATION_TEMPLATE";
                                    //            writeToLog(status);
                                    //        }
                                    //    }
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    writeToLog("Error occured while sending redemption message " + ex.Message);
                                    log.Error("Error occured while sending redemption message ", ex);
                                }
                            }
                        }
                        if (messageType == "B" || messageType == "W")
                        {
                            try
                            {
                                if(customerDTO != null && customerDTO.ContactDTOList != null && customerDTO.ProfileDTO != null)
                                {
                                    if(customerDTO.ProfileDTO.OptOutWhatsApp == false)
                                    {
                                        int index = customerDTO.ContactDTOList.FindIndex(x => x.Attribute1 == dr["Contact_phone1"].ToString());
                                        bool whatsAppEnabled = (index == -1 ? false : customerDTO.ContactDTOList[index].WhatsAppEnabled);
                                        log.LogVariableState("whatsAppEnabled", whatsAppEnabled);

                                        if (whatsAppEnabled)
                                        {
                                            RedemptionMessageTriggerEventBL redemptionMessageTriggerEventBL = new RedemptionMessageTriggerEventBL(_utilities, _utilities.ExecutionContext, messagingTriggerDTO, redemptionDTO, customerDTO, accountDTO);
                                            redemptionMessageTriggerEventBL.SendMessage(MessagingClientDTO.MessagingChanelType.WHATSAPP_MESSAGE);
                                        }
                                        else
                                        {
                                            string status = "Customer number is not enabled for whatsApp";
                                            log.Debug(status);
                                        }
                                    }
                                    else
                                    {
                                        string status = "User not opt for whatsApp Messaging";
                                        log.Debug(status);
                                    }
                                }
                                else
                                {
                                    string status = "Customer details are missing, skipping whatsApp message";
                                    log.Debug(status);
                                }

                            }
                            catch (Exception ex)
                            {
                                writeToLog("Error occured while sending redemption message " + ex.Message);
                                log.Error("Error occured while sending redemption message ", ex);
                            }
                        }
                    }
                    else
                    {
                        string status = "Redemption is not linked with Customer, CustomerId: " + dr["Customer_Id"].ToString();
                        writeToLog(status);
                    }
                    //log.LogVariableState("@timestamp", dr["TrxDate"]);
                    log.LogVariableState("@triggerId", dr["TriggerId"]);
                }
                log.LogVariableState("@dtTriggers.Rows.Count: ", dtTriggers.Rows.Count);
                if (dtTriggers.Rows.Count > 0)
                {
                    _utilities.executeNonQuery(@"update MessagingTriggers set Timestamp = @timestamp where TriggerId = @triggerId ",
                                                  new SqlParameter("@timestamp", timeStamp),
                                                  new SqlParameter("@triggerId", dtTriggers.Rows[0]["TriggerId"]));
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                _utilities.EventLog.logEvent("MessagingTrigger", 'E', "Error while processing redemption Status message trigger for RedemptionId [" +
                                              redemptionId.ToString() + "], CustomerId [" + customerId.ToString() + "] : " + ex.Message, "MessagingTriggerLog", "RedemptionPurchase", 0);
            }
        }

        private int GetTriggerId(object triggerIdColumnValue)
        {
            log.LogMethodEntry(triggerIdColumnValue);
            int triggerId = -1;
            if (triggerIdColumnValue != DBNull.Value)
            {
                triggerId = Convert.ToInt32(triggerIdColumnValue);
            }
            log.LogMethodExit(triggerId);
            return triggerId;
        }

        private static int GetCustomerId(object customerIdColumnValue)
        {
            log.LogMethodEntry(customerIdColumnValue);
            int customerId = -1;
            if (customerIdColumnValue != DBNull.Value)
            {
                customerId = Convert.ToInt32(customerIdColumnValue);
            }
            log.LogMethodExit(customerId);
            return customerId;
        }
        private static int GetCardId(object cardIdColumnValue)
        {
            log.LogMethodEntry(cardIdColumnValue);
            int cardId = -1;
            if (cardIdColumnValue != DBNull.Value)
            {
                cardId = Convert.ToInt32(cardIdColumnValue);
            }
            log.LogMethodExit(cardId);
            return cardId;
        }
        private MessagingTriggerDTO GetMessagingTriggerDTO(List<MessagingTriggerDTO> messagingTriggerDTOList, int triggerId)
        {
            log.LogMethodEntry(triggerId);
            MessagingTriggerDTO messagingTriggerDTO = messagingTriggerDTOList.Find(mDTO => mDTO.TriggerId == triggerId);
            if (messagingTriggerDTO == null)
            {
                MessagingTriggerBL messagingTriggerBL = new MessagingTriggerBL(_utilities.ExecutionContext, triggerId, true);
                messagingTriggerDTO = messagingTriggerBL.GetMessagingTriggerDTO;
                messagingTriggerDTOList.Add(messagingTriggerDTO);
            }
            log.LogMethodExit(messagingTriggerDTO);
            return messagingTriggerDTO;
        }
        private CustomerDTO GetCustomerDTO(List<CustomerDTO> customerDTOList, int customerId)
        {
            log.LogMethodEntry(customerId);
            CustomerDTO customerDTO = customerDTOList.Find(cDTO => cDTO.Id == customerId);
            if (customerDTO == null)
            {
                CustomerBL customerBL = new CustomerBL(_utilities.ExecutionContext, customerId, true);
                customerDTO = customerBL.CustomerDTO;
                customerDTOList.Add(customerDTO);
            }
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }
        private AccountDTO GetAccountDTO(List<AccountDTO> accountDTOList, int cardId)
        {
            log.LogMethodEntry(cardId);
            AccountDTO accountDTO = accountDTOList.Find(aDTO => aDTO.AccountId == cardId);
            if (accountDTO == null)
            {
                AccountBL accountBL = new AccountBL(_utilities.ExecutionContext, cardId, true);
                accountDTO = accountBL.AccountDTO;
                accountDTOList.Add(accountDTO);
            }
            log.LogMethodExit(accountDTO);
            return accountDTO;
        }

        private Transaction.Transaction GetTransaction(int trxId, Transaction.Transaction Trx)
        {
            log.LogMethodEntry(trxId);
            if (Trx == null)
            {
                TransactionUtils trxUtils = new TransactionUtils(_utilities);
                Trx = trxUtils.CreateTransactionFromDB(trxId, _utilities, true);
            }
            log.LogMethodExit();
            return Trx;
        }

        private RedemptionDTO GetRedemptionDTO(object redemptionId)
        {
            log.LogMethodEntry(redemptionId);
            RedemptionDTO redemptionDTO = null;
            if (redemptionId != DBNull.Value)
            {
                RedemptionBL redemptionBL = new RedemptionBL((int)redemptionId, _utilities.ExecutionContext);
                redemptionDTO = redemptionBL.RedemptionDTO;
            }
            log.LogMethodExit(redemptionDTO);
            return redemptionDTO;
        }
    }
}
