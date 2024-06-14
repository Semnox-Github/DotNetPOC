/********************************************************************************************
 * Project Name - Messaging
 * Description  - Messaging class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modifed for fetching the encrypted key value 
 *2.70.2        04-Feb-2020      Nitin Pai    Guest App phase 2 changes - OTP provider can be different from SMS provider. Added new method to send OTP.
 *2.70.3    06-May-2020      Mathew Ninan   Added lookup to check for character limit and then 
 *                                            truncate message based on limit set. Default is 160
 *2.90.0        23-Jul-2020    Jinto Thomas   Modified:SendEMailSynchronous(), SendSMSSynchronous() for messaing enhancement                                           
 *2.100         12-Oct-2020     Nitin Pai     Fixed to use customer id
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;


namespace Semnox.Core.Utilities
{
    public class Messaging
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.Utilities _utilities;

        
        string smtpHost;
        int smtpPort = 587;
        string smtpUsername;
        string smtpPassword;
        string smtpDisplayName;
        bool EnableSsl;

        String SMSuserid;
        String SMSpasswd;
        string SMSurl;

        public Messaging(Semnox.Core.Utilities.Utilities inUtilities)
        {
            log.LogMethodEntry(inUtilities);
            _utilities = inUtilities;

            smtpHost = _utilities.getParafaitDefaults("CRM_SMTP_HOST");
            smtpPort = 587;
            smtpUsername = _utilities.getParafaitDefaults("CRM_SMTP_NETWORK_USERNAME");
            smtpPassword = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "CRM_SMTP_NETWORK_PASSWORD");// _utilities.getParafaitDefaults("CRM_SMTP_NETWORK_PASSWORD");
            smtpDisplayName = _utilities.getParafaitDefaults("CRM_SMTP_FROM_NAME");
            EnableSsl = (_utilities.getParafaitDefaults("CRM_ENABLE_SMTP_SSL") == "Y");
            
            SMSuserid = _utilities.getParafaitDefaults("CRM_SMS_USERNAME");
            SMSpasswd = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "CRM_SMS_PASSWORD");
            SMSurl = _utilities.getParafaitDefaults("CRM_SMS_PROVIDER_URL");
            log.LogMethodExit(null);
        }

        //public void SendEMail(string ToEmails, string Subject, string EmailBody, string AttachFile = null) //previous code
        public void SendEMail(string ToEmails, string Subject, string EmailBody, string custId = null, string AttachFile = null, string Status = null, string SendAttemptDate = null, string Attempts = null,
            string Reference = null, string Cardid = null, int? parafaitFunctionEventId = null)
        {
            log.LogMethodEntry(ToEmails, Subject, EmailBody, custId, AttachFile, Status, SendAttemptDate, Attempts, Reference, Cardid, parafaitFunctionEventId);
            //In the above line
            //-custid is added in parameter list-on-29-10-2015
            //Status,SendAttemptDate,Attempts,Reference-were added on 29-10-2015 to incorporate card validity expiry changes.
            if (String.IsNullOrEmpty(custId))
            {
                custId = "-1";
            }
            SendEMail(ToEmails, Subject, EmailBody, -1, Reference, Convert.ToInt32(custId), DateTime.MinValue, AttachFile, Status, SendAttemptDate, Attempts, Cardid, parafaitFunctionEventId);
            log.LogMethodExit(null);
        }

        public void SendEMail(string ToEmails, string Subject, string EmailBody, int BatchId, string Reference, int CustomerId, DateTime SendAt, string AttachFile = null, string Status = null, 
            string SendAttemptDate = null, string Attempts = null, string Cardid = null, int? parafaitFunctionEventId = null)
        {
            log.LogMethodEntry(ToEmails, Subject, EmailBody, BatchId, Reference, CustomerId, SendAt, AttachFile, Status, SendAttemptDate, Attempts, Cardid);
            //previous query commented on 29-10-2015
            _utilities.executeNonQuery(@"insert into MessagingRequests
                                        (BatchId,
                                        Reference,
                                        MessageType,
                                        ToEmails,
                                        SendDate,
                                        Subject,
                                        Body,
                                        CustomerId,
                                        AttachFile,
                                        Status,
                                        SendAttemptDate,
                                        Attempts,
                                        card_id, 
                                        parafaitFunctionEventId)
                                        values
                                       (@BatchId,
                                        @Reference,
                                        'E',
                                        @ToEmails,
                                        @SendDate,
                                        @Subject,
                                        @Body,
                                        @CustomerId,
                                        @AttachFile,
                                        @Status,
                                        @SendAttemptDate,
                                        @Attempts,
                                        @Cardid,
                                        @parafaitFunctionEventId)",
                                    new SqlParameter("@BatchId", BatchId > 0 ? (object)BatchId : DBNull.Value),
                                    new SqlParameter("@CustomerId", CustomerId > 0 ? (object)CustomerId : DBNull.Value),
                                    new SqlParameter("@Reference", string.IsNullOrEmpty(Reference) ? DBNull.Value : (object)Reference),
                                    new SqlParameter("@SendDate", SendAt.Equals(DateTime.MinValue) ? DBNull.Value : (object)SendAt),
                                    new SqlParameter("@ToEmails", ToEmails),
                                    new SqlParameter("@Subject", Subject),
                                    new SqlParameter("@Body", EmailBody),
                                    new SqlParameter("@AttachFile", string.IsNullOrEmpty(AttachFile) ? DBNull.Value : (object)AttachFile),
                                    new SqlParameter("@Status", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Status),
                                    new SqlParameter("@SendAttemptDate", string.IsNullOrEmpty(SendAttemptDate) ? DBNull.Value : (object)Convert.ToDateTime(SendAttemptDate)),
                                    new SqlParameter("@Attempts", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Convert.ToInt32(Attempts)),
                                    new SqlParameter("@Cardid", string.IsNullOrEmpty(Cardid) ? DBNull.Value : (object)Convert.ToInt32(Cardid)),
                                    new SqlParameter("@parafaitFunctionEventId", (parafaitFunctionEventId == null || parafaitFunctionEventId == -1)
                                                                              ? DBNull.Value : (object)Convert.ToInt32(parafaitFunctionEventId)));
            //new insertion statement-on 29-10-2015
            log.LogVariableState("@BatchId", BatchId > 0 ? (object)BatchId : DBNull.Value);
            log.LogVariableState("@CustomerId", CustomerId > 0 ? (object)CustomerId : DBNull.Value);
            log.LogVariableState("@Reference", string.IsNullOrEmpty(Reference) ? DBNull.Value : (object)Reference);
            log.LogVariableState("@SendDate", SendAt.Equals(DateTime.MinValue) ? DBNull.Value : (object)SendAt);
            log.LogVariableState("@ToEmails", ToEmails);
            log.LogVariableState("@Subject", Subject);
            log.LogVariableState("@Body", EmailBody);
            log.LogVariableState("@AttachFile", string.IsNullOrEmpty(AttachFile) ? DBNull.Value : (object)AttachFile);
            log.LogVariableState("@Status", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Status);
            log.LogVariableState("@SendAttemptDate", string.IsNullOrEmpty(SendAttemptDate) ? DBNull.Value : (object)Convert.ToDateTime(SendAttemptDate));
            log.LogVariableState("@Attempts", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Convert.ToInt32(Attempts));
            log.LogVariableState("@Cardid", string.IsNullOrEmpty(Cardid) ? DBNull.Value : (object)Convert.ToInt32(Cardid));
            log.LogVariableState("@parafaitFunctionEventId", (parafaitFunctionEventId == null || parafaitFunctionEventId == -1)
                                                                              ? DBNull.Value : (object)Convert.ToInt32(parafaitFunctionEventId));
            log.LogMethodExit(null);
        }

        //public void SendSMS(string ToPhoneNos, string Message) //code before-29-10-2015
        public void SendSMS(string ToPhoneNos, string Message, string custId=null, string Status = null, string SendAttemptDate = null, string Attempts = null, string Reference = null, string Cardid = null,
            int? parafaitFunctionEventId = null) //added custId on -4-06-2015
        {
            log.LogMethodEntry(ToPhoneNos, Message, custId, Status, SendAttemptDate, Attempts, Reference, Cardid, parafaitFunctionEventId);
            //In the above line
            //-custid is added in parameter list-on-29-10-2015
            //Status,SendAttemptDate,Attempts,Reference-were added on 29-10-2015 to incorporate card validity expiry changes.
            //SendSMS(ToPhoneNos, Message, -1, null, -1, DateTime.MinValue); //previous code
            if(String.IsNullOrEmpty(custId))
            {
                custId = "-1";
            }
            SendSMS(ToPhoneNos, Message, -1, Reference, Convert.ToInt32(custId), DateTime.MinValue, Status, SendAttemptDate, Attempts, Cardid, parafaitFunctionEventId);
            log.LogMethodExit(null);
        }

        //public void SendSMS(string ToPhoneNos, string Message, int BatchId, string Reference, int CustomerId, DateTime SendAt)
        public void SendSMS(string ToPhoneNos, string Message, int BatchId, string Reference, int CustomerId, DateTime SendAt, string Status = null, string SendAttemptDate = null, string Attempts = null, string Cardid = null, int? parafaitFunctionEventId = null)
        {
            log.LogMethodEntry(ToPhoneNos, Message, BatchId, Reference, CustomerId, SendAt, Status, SendAttemptDate, Attempts, Cardid);
            _utilities.executeNonQuery(@"insert into MessagingRequests
                                        (BatchId,
                                        Reference,
                                        MessageType,
                                        ToMobile,
                                        SendDate,
                                        Body,
                                        CustomerId,
                                        Status,
                                        SendAttemptDate,
                                        Attempts,
                                        card_id,
                                        parafaitFunctionEventId)
                                        values
                                       (@BatchId,
                                        @Reference,
                                        'S',
                                        @ToMobile,
                                        @SendDate,
                                        @Body,
                                        @CustomerId,
                                        @Status,
                                        @SendAttemptDate,
                                        @Attempts,
                                        @Cardid,
                                        @parafaitFunctionEventId)",
                                 new SqlParameter("@BatchId", BatchId > 0 ? (object)BatchId : DBNull.Value),
                                 new SqlParameter("@CustomerId", CustomerId > 0 ? (object)CustomerId : DBNull.Value),
                                 new SqlParameter("@Reference", string.IsNullOrEmpty(Reference) ? DBNull.Value : (object)Reference),
                                 new SqlParameter("@SendDate", SendAt.Equals(DateTime.MinValue) ? DBNull.Value : (object)SendAt),
                                 new SqlParameter("@ToMobile", ToPhoneNos),
                                 new SqlParameter("@Body", Message),
                                 new SqlParameter("@Status", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Status),
                                 new SqlParameter("@SendAttemptDate", string.IsNullOrEmpty(SendAttemptDate) ? DBNull.Value : (object)Convert.ToDateTime(SendAttemptDate)),
                                 new SqlParameter("@Attempts", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Convert.ToInt32(Attempts)),
                                 new SqlParameter("@Cardid", string.IsNullOrEmpty(Cardid) ? DBNull.Value : (object)Convert.ToInt32(Cardid)),
                                 new SqlParameter("@parafaitFunctionEventId", (parafaitFunctionEventId == null || parafaitFunctionEventId == -1) 
                                                                              ? DBNull.Value : (object)Convert.ToInt32(parafaitFunctionEventId)));
            //new insertion statement-on 29-10-2015
            log.LogVariableState("@BatchId", BatchId > 0 ? (object)BatchId : DBNull.Value);
            log.LogVariableState("@CustomerId", CustomerId > 0 ? (object)CustomerId : DBNull.Value);
            log.LogVariableState("@Reference", string.IsNullOrEmpty(Reference) ? DBNull.Value : (object)Reference);
            log.LogVariableState("@SendDate", SendAt.Equals(DateTime.MinValue) ? DBNull.Value : (object)SendAt);
            log.LogVariableState("@ToMobile", ToPhoneNos);
            log.LogVariableState("@Body", Message);
            log.LogVariableState("@Status", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Status);
            log.LogVariableState("@SendAttemptDate", string.IsNullOrEmpty(SendAttemptDate) ? DBNull.Value : (object)Convert.ToDateTime(SendAttemptDate));
            log.LogVariableState("@Attempts", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Convert.ToInt32(Attempts));
            log.LogVariableState("@Cardid", string.IsNullOrEmpty(Cardid) ? DBNull.Value : (object)Convert.ToInt32(Cardid));
            log.LogVariableState("@parafaitFunctionEventId", (parafaitFunctionEventId == null || parafaitFunctionEventId == -1)
                                                                              ? DBNull.Value : (object)Convert.ToInt32(parafaitFunctionEventId));
            log.LogMethodExit(null);
        }

		public void SendEMailSynchronous(string ToEmails, string ccEmail, string Subject, string EmailBody, string ReplyToEmail = null, string AttachFile = null, string bccEmail = null, string messagingClientDisplayName = null, int messagingClientSmtpPort = -1, 
                               string messagingClientHostUrl = null, string messagingClientUserName = null, string messagingClientPassword = null, bool messagingClientEnableSsl = false)
		{
			log.LogMethodEntry(ToEmails, ccEmail, Subject, EmailBody, ReplyToEmail, AttachFile);

			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
			SmtpClient mailClient;


            if(messagingClientSmtpPort != -1) // ignore port
            {
                smtpHost = messagingClientHostUrl;
            }
            if(!String.IsNullOrEmpty(messagingClientHostUrl))
            {
                smtpHost = messagingClientHostUrl;
                EnableSsl = messagingClientEnableSsl;
            }
            if (!String.IsNullOrEmpty(messagingClientUserName))
            {
                smtpUsername = messagingClientUserName;
            }
            if (!String.IsNullOrEmpty(messagingClientPassword))
            {
                smtpPassword = messagingClientPassword;
            }
            if (!String.IsNullOrEmpty(messagingClientDisplayName))
            {
                smtpDisplayName = messagingClientDisplayName;
            }
                      
			if (smtpPort == -1) // ignore port
			{
				mailClient = new SmtpClient(smtpHost);
			}
			else
			{
				mailClient = new SmtpClient(smtpHost, smtpPort);
			}

			NetworkCredential credential = new NetworkCredential(smtpUsername, smtpPassword);

			mailClient.Credentials = credential;
			mailClient.EnableSsl = EnableSsl;

			MailMessage msg = new MailMessage();
			msg.IsBodyHtml = true;

			string mailFrom = smtpUsername.Contains("@") ? smtpUsername : smtpUsername + "@" + smtpHost;
			try
			{
				msg.From = new MailAddress(mailFrom, smtpDisplayName);
			}
			catch (Exception ex)
			{
				log.LogVariableState("mailFrom", mailFrom);
				log.LogVariableState("smtpDisplayName", smtpDisplayName);
				log.Error("Error occured while sending email", ex);
				log.LogMethodExit(null, "Throwing Application Exception" + mailFrom + ": " + ex.Message);
				throw new ApplicationException(mailFrom + ": " + ex.Message);
			}

			if (!string.IsNullOrEmpty(ReplyToEmail))
			{
				string mailReplyTo = ReplyToEmail.Contains("@") ? ReplyToEmail : ReplyToEmail + "@" + smtpHost;
				try
				{
					msg.ReplyToList.Add(new MailAddress(mailReplyTo));
				}
				catch (Exception ex)
				{
					log.Error("Error occured while creating reply-to list", ex);
					log.LogMethodExit(null, "Throwing Application Exception" + mailReplyTo + ": " + ex.Message);
					throw new ApplicationException(mailReplyTo + ": " + ex.Message);
				}
			}

			if (EmailBody.ToLower().Contains("<html") == false)
			{
				EmailBody = EmailBody.Replace("\r\n", "\n");
				EmailBody = EmailBody.Replace("\n\n", "<br /> <br /> ").Replace("\n", "<br />");
			}

			msg.Subject = Subject;
			msg.Body = EmailBody;
			msg.BodyEncoding = UnicodeEncoding.UTF8;
			msg.To.Add(ToEmails);
            if (!string.IsNullOrEmpty(ccEmail))
            {
                msg.CC.Add(ccEmail);
            }
            if (!string.IsNullOrEmpty(bccEmail))
            {
                msg.Bcc.Add(bccEmail);
            }
                


            if (!string.IsNullOrEmpty(AttachFile))
			{
				try
				{
					Attachment oAttachment = new Attachment(AttachFile);
					string extn = System.IO.Path.GetExtension(AttachFile).ToLower();
					if (extn.Equals(".gif")
						|| extn.Equals(".jpg")
						|| extn.Equals(".bmp"))
					{
						oAttachment.ContentId = System.IO.Path.GetFileName(AttachFile);
					}
					msg.Attachments.Add(oAttachment);
				}
				catch (Exception ex)
				{
					log.Error("Error occured during message attachments", ex);
					log.LogMethodExit(null, "Throwing Application Exception" + AttachFile + ": " + ex.Message);
					throw new ApplicationException(AttachFile + ": " + ex.Message);
				}
			}

			try
			{
				mailClient.Send(msg);
			}
			catch (SmtpException smex)
			{
				log.Error("Smtp exception occured while sending message  to mailclient", smex);
				log.LogMethodExit(null, "Throwing Application Exception" + ToEmails + ": " + ((smex.InnerException==null)? smex.ToString():smex.InnerException.Message));
				throw new ApplicationException(ToEmails + ": " + ((smex.InnerException == null) ? smex.ToString() : smex.InnerException.Message));
			}
			catch (InvalidOperationException ioex)
			{
				log.Error("InvalidOperationException occured while sending message to mail client", ioex);
				log.LogMethodExit(null, "Throwing Application Exception" + ToEmails + ": " + ioex.Message);
				throw new ApplicationException(ToEmails + ": " + ioex.Message);
			}
			catch (Exception ex)
			{
				log.Error("Error occured while sending email synchronously", ex);
				log.LogMethodExit(null, "Throwing Application Exception" + ToEmails + ": " + ex.Message);
				throw new ApplicationException(ToEmails + ": " + ex.Message);
			}
			finally
			{
				msg.Dispose();
			}
			log.LogMethodExit(null);
		}

        public string sendOTPSMSSynchronous(string PhoneNos, string Message)
        {
            string otpUserId = _utilities.getParafaitDefaults("CRM_OTP_SMS_USERNAME");
            SMSuserid = !String.IsNullOrEmpty(otpUserId) ? otpUserId : SMSuserid;

            string otpUserPwd = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "CRM_OTP_SMS_PASSWORD");
            SMSpasswd = !String.IsNullOrEmpty(otpUserPwd) ? otpUserPwd : SMSpasswd;

            string otpURL = _utilities.getParafaitDefaults("CRM_OTP_SMS_PROVIDER_URL");
            SMSurl = !String.IsNullOrEmpty(otpURL) ? otpURL : SMSurl;

            String smsValue = sendSMSSynchronous(PhoneNos, Message);

            // reset as if non otp sms are sent on OTP user, then it blocks account
            SMSuserid = _utilities.getParafaitDefaults("CRM_SMS_USERNAME");
            SMSpasswd = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "CRM_SMS_PASSWORD");
            SMSurl = _utilities.getParafaitDefaults("CRM_SMS_PROVIDER_URL");

            return smsValue;

        }
        public string sendSMSSynchronous(string PhoneNos, string Message, string messagingClientUserName = null, string messagingClientPassword = null, string messagingClientURL = "") 
        {
            log.LogMethodEntry(PhoneNos, Message);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            string resultCode = "";
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SMS_GATEWAY_RESPONSE_CODE"));
                searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, (_utilities.ParafaitEnv.IsCorporate ? _utilities.ParafaitEnv.SiteId : -1).ToString()));
                lookupValuesDTOList = new LookupValuesList(_utilities.ExecutionContext).GetAllLookupValues(searchlookupParameters);
                LookupValuesDTO charLimitLookupDTO = lookupValuesDTOList.First(x => x.LookupValue == "CharLimit");
                int charLimit = 160;
                if (charLimitLookupDTO != null)
                {
                    int limitResult;
                    if (int.TryParse(charLimitLookupDTO.Description, out limitResult))
                        charLimit = limitResult;
                }
                if (Message.Length > charLimit)
                    Message = Message.Substring(0, charLimit);
                if (!String.IsNullOrEmpty(SMSuserid))
                {
                    SMSuserid = messagingClientUserName;
                }

                if (!String.IsNullOrEmpty(messagingClientPassword))
                {
                    SMSpasswd = messagingClientPassword;
                }

                if(!String.IsNullOrEmpty(messagingClientURL))
                {
                    SMSurl = messagingClientURL;
                }

                Message = System.Net.WebUtility.UrlEncode(Message);

                string smsapi = SMSurl.Replace("@UserId", SMSuserid).Replace
                                                ("@Password", SMSpasswd).Replace
                                                ("@Message", Message).Replace
                                                ("@PhoneNumber", PhoneNos);
                request = WebRequest.Create(smsapi);
                //in case u work behind proxy, uncomment the commented code and provide correct details
                /*WebProxy proxy = new WebProxy("http://proxy:80/",true);
                proxy.Credentials = new
                NetworkCredential("userId","password", "Domain");
                request.Proxy = proxy;*/
                // Send the 'HttpWebRequest' and wait for response.
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                resultCode = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                //29-10-2015:: Modified code as SMS provider was passing alpha numeric return value instead of Success

                //List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                //List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                //searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SMS_GATEWAY_RESPONSE_CODE"));
                //searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, (_utilities.ParafaitEnv.IsCorporate ? _utilities.ParafaitEnv.SiteId : -1).ToString()));
                //lookupValuesDTOList = new LookupValuesList(_utilities.ExecutionContext).GetAllLookupValues(searchlookupParameters);
                //if (Regex.IsMatch(result, @"^[a-zA-Z0-9]+$") || result.ToLower().StartsWith("success") || result.ToLower().Contains("successful"))
                if (lookupValuesDTOList != null)
                {
                    foreach (LookupValuesDTO lookup in lookupValuesDTOList)
                    {
                        if (resultCode.Contains(lookup.LookupValue))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    log.Error("SMS_GATEWAY_RESPONSE_CODE lookup values are not set to check the message send status");
                }

                if (Regex.IsMatch(resultCode, @"^[a-zA-Z0-9]+$") || result == "success")
                {                   
                    log.LogMethodExit(result);
                    return result;
                }

                else
                {
                    result = "Error";
                    log.LogMethodExit(null, "Throwing ApplicationException- " + result);
                    throw new ApplicationException(result);
                }
                //29-10-2015:: Modification end
            }
            catch (Exception ex)
            {
                log.Error("Error occured in sendSMSSynchronous", ex);
                log.LogMethodExit(null, "Throwing ApplicationException- " + PhoneNos + ": " + ex.Message);
                throw new ApplicationException(PhoneNos + ": " + ex.Message);
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }
    }
}
