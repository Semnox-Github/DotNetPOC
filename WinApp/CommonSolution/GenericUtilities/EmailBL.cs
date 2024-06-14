/********************************************************************************************
 * Project Name - Maintenance
 * Description  - This class holds all details of smtp server and logic to send email with attachments
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        17-May-2019   Muhammed Mehraj   Created 
 *2.70        17-Sept-2019  Mushahid Faizan   added email logic for Campaign entity.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Semnox.Core.GenericUtilities
{
    public class EmailBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private EmailDTO email;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public EmailBL(ExecutionContext executionContext, EmailDTO email)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.email = email;
            log.LogMethodExit();
        }

        SmtpClient mailClient = null;
        /// <summary>
        /// Sends Email to the client
        /// Holds information like smtpserver,from,to,cc,subject and body.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string SendMail(string entityName = null)
        {
            try
            {
                log.LogMethodEntry(entityName);
                string smtpHost = string.Empty;
                int smtpPort = 587;
                string smtpUsername = string.Empty;
                string smtpPassword = string.Empty;
                string smtpDisplayName = string.Empty;
                string smtpMailDomain = string.Empty;

                if (string.IsNullOrEmpty(entityName))
                {
                    switch (entityName.ToUpper().ToString())
                    {
                        case "REQUISITION":
                            smtpUsername = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_NETWORK_CREDENTIAL_USERNAME");
                            smtpPassword = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_NETWORK_CREDENTIAL_PASSWORD");//Utilities.getParafaitDefaults("SMTP_NETWORK_CREDENTIAL_PASSWORD");
                            smtpDisplayName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_FROM_DISPLAY_NAME");
                            smtpHost = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_HOST");
                            smtpMailDomain = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_MAIL_ADDRESS_DOMAIN");

                            try
                            {
                                smtpPort = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_PORT")); // specify -1 to ignore, null to use 587
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occured in smtp port", ex);
                                smtpPort = 587;
                            }

                            break;
                        case "CAMPAIGN":
                            smtpUsername = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_NETWORK_USERNAME");
                            smtpPassword = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_NETWORK_PASSWORD");//Common.Utilities.getParafaitDefaults("CRM_SMTP_NETWORK_PASSWORD");
                            smtpDisplayName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_FROM_NAME");
                            smtpHost = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_HOST");

                            try
                            {
                                smtpPort = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_PORT")); // specify -1 to ignore, null to use 587
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occured in smtp port", ex);
                                smtpPort = 587;
                            }
                            break;
                    }
                }

                SystemOptionsBL systemOptionsBl = new SystemOptionsBL(executionContext,"Parafait Keys", "DefultSMTPHost");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    string encryptedOptionValue = systemOptionsBl.GetSystemOptionsDTO.OptionValue;
                    smtpHost = Encryption.Decrypt(encryptedOptionValue);
                }
                systemOptionsBl = new SystemOptionsBL(executionContext, "Parafait Keys", "DefultSMTPUserName");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    string encryptedOptionValue = systemOptionsBl.GetSystemOptionsDTO.OptionValue;
                    smtpUsername = Encryption.Decrypt(encryptedOptionValue);
                }
                systemOptionsBl = new SystemOptionsBL(executionContext, "Parafait Keys", "DefultSMTPPassword");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    string encryptedOptionValue = systemOptionsBl.GetSystemOptionsDTO.OptionValue;
                    smtpPassword = Encryption.Decrypt(encryptedOptionValue);
                }
                smtpHost = String.IsNullOrEmpty(smtpHost) ? smtpHost : smtpHost;                
                string replyTo = !String.IsNullOrEmpty(email.ReplyTo) ? email.ReplyTo : (String.IsNullOrEmpty(smtpUsername) ? "noreply@noreply.noreply" : smtpUsername);
                smtpUsername = String.IsNullOrEmpty(smtpUsername) ? smtpUsername : smtpUsername;
                smtpPassword = String.IsNullOrEmpty(smtpPassword) ? smtpPassword : smtpPassword;
                SiteDTO siteDTO = new SiteDTO();
                string siteName = string.Empty;

                SiteList siteList = new SiteList(executionContext);
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParameters);
                if(siteDTOList != null && siteDTOList.Any())
                {
                    siteName = siteDTOList[0].SiteName;
                }
                smtpDisplayName = String.IsNullOrEmpty(smtpDisplayName) ? siteName : smtpDisplayName;

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

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_SMTP_SSL") == "Y")
                    mailClient.EnableSsl = true;
                else
                    mailClient.EnableSsl = false;

                string ToEmails = email.To;

                string ccEmails = email.Cc;  //get the cc id in a variable,which can be passed to mail client              

                if (email.Body == null)
                {
                    email.Body = string.Empty;
                }
                string emailBody = email.Body;
                if (emailBody.ToLower().Contains("<html") == false)
                {
                    emailBody = emailBody.Replace("\r\n", "\n");
                    emailBody = emailBody.Replace("\n\n", "<br /> <br /> ").Replace("\n", "<br />");
                }

                MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.BodyEncoding = UnicodeEncoding.UTF8;

                string mailFrom = smtpUsername.Contains("@") ? smtpUsername : smtpUsername + "@" + (String.IsNullOrEmpty(smtpMailDomain) ? smtpHost : smtpMailDomain);
                string mailReplyTo = replyTo.Contains("@") ? replyTo : replyTo + "@" + (String.IsNullOrEmpty(smtpMailDomain) ? smtpHost : smtpMailDomain);

                try
                {
                    msg.From = new MailAddress(mailFrom, smtpDisplayName);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while sending email", ex);
                    log.LogMethodExit(null, "Throwing application exception" + mailFrom + ": " + ex.Message);
                    throw new ApplicationException(mailFrom + ": " + ex.Message);
                }
                try
                {
                    msg.ReplyToList.Add(new MailAddress(mailReplyTo));
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while creating reply-to list", ex);
                    log.LogMethodExit(null, "Throwing Application exception-" + mailReplyTo + ": " + ex.Message);
                    throw new ApplicationException(mailReplyTo + ": " + ex.Message);
                }
                msg.Subject = email.Subject;
                msg.Body = emailBody;
                msg.To.Add(ToEmails);

                if (!string.IsNullOrEmpty(ccEmails))
                {
                    if (ccEmails.Length > 0)
                        msg.CC.Add(ccEmails); //Pass the cc ids also to mail client object
                }

                if (email.Attachments != null && email.Attachments.Count > 0)
                {
                    try
                    {
                        foreach (FileAttachment fileAttachment in email.Attachments)
                        {
                            byte[] bytes = System.Convert.FromBase64String(fileAttachment.FileContentBase64);
                            MemoryStream memAttachment = new MemoryStream(bytes);
                            Attachment attachment = new Attachment(memAttachment, fileAttachment.Info.Name);
                            msg.Attachments.Add(attachment);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured in attaching the file", ex);
                        log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                        throw new ApplicationException(ex.Message);
                    }
                }
                try
                {
                    mailClient.Send(msg);
                    log.LogMethodExit();
                    return MessageContainerList.GetMessage(executionContext, 333);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in sending mail to client", ex);
                    log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                    throw new ApplicationException(ToEmails + ": " + ex.Message);
                }
                finally
                {
                    msg.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in SendEmails method", ex);
                log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
