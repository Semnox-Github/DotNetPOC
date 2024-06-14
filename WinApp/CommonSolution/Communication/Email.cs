/********************************************************************************************
 * Project Name - Email
 * Description  - File encapsulating send email functionlity
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 2.130.04    17-Feb-2022   Nitin Pai       Added FromEmail and Domain for SendGrid mailing service
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Communication
{
    public class Email
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SmtpClient smtpClient;
        private readonly MailMessage msg;

        public Email(ExecutionContext executionContext,
                     string toEmailAddress,
                     string subject,
                     string body)
            :this(executionContext, 
                  toEmailAddress, 
                  string.Empty, 
                  string.Empty, 
                  subject, 
                  body)
        {
            log.LogMethodEntry(executionContext,
                               toEmailAddress,
                               subject,
                               body);
            log.LogMethodExit();
        }
        public Email(ExecutionContext executionContext, 
                     string toEmailAddress, 
                     string ccEmailAddress, 
                     string replyToEmailAddress, 
                     string subject, 
                     string body)
        {
            log.LogMethodEntry(executionContext, 
                               toEmailAddress, 
                               ccEmailAddress, 
                               replyToEmailAddress, 
                               subject, 
                               body);
            string smtpHost = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_HOST", "smtp.gmail.com");
            int smtpPort = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "SMTP_HOST", 587);
            string smtpUsername = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_NETWORK_CREDENTIAL_USERNAME", "ParafaitReports@gmail.com");
            string smtpPassword = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_NETWORK_CREDENTIAL_PASSWORD");
            string smtpDisplayName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext,"SMTP_FROM_DISPLAY_NAME");
            string smtpMailDomain = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_MAIL_ADDRESS_DOMAIN");
            string smtpMailFrom = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SMTP_FROM_MAIL_ADDRESS");
            string replyTo = replyToEmailAddress;
            if (string.IsNullOrWhiteSpace(replyTo))
            {
                replyTo = smtpUsername;
            }
            if (string.IsNullOrWhiteSpace(replyTo))
            {
                replyTo = "noreply@noreply.noreply";
            }
            if (string.IsNullOrWhiteSpace(smtpPassword))
            {
                replyTo = "semnox!1";
            }
            if (string.IsNullOrWhiteSpace(smtpDisplayName))
            {
                Site.Site site = new Site.Site(executionContext.GetSiteId());
                if(site.getSitedTO != null)
                {
                    smtpDisplayName = site.getSitedTO.SiteName;
                }
            }
            if(smtpPort == -1)
            {
                smtpClient = new SmtpClient(smtpHost);
            }
            else
            {
                smtpClient = new SmtpClient(smtpHost, smtpPort);
            }
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_SMTP_SSL") == "Y")
            {
                smtpClient.EnableSsl = true;
            }
            else
            {
                smtpClient.EnableSsl = false;
            }
            if(string.IsNullOrWhiteSpace(toEmailAddress))
            {
                log.LogMethodExit(null, "Throwing Application excepton-" + MessageContainerList.GetMessage(executionContext,331));
                throw new ApplicationException(MessageContainerList.GetMessage(executionContext, 331));
            }
            string ToEmails = toEmailAddress;
            string ccEmails = ccEmailAddress;
            string emailBody = body;
            if (emailBody.ToLower().Contains("<html") == false)
            {
                emailBody = emailBody.Replace("\r\n", "\n");
                emailBody = emailBody.Replace("\n\n", "<br /> <br /> ").Replace("\n", "<br />");
            }
            msg = new MailMessage();
            msg.IsBodyHtml = true;
            msg.BodyEncoding = UnicodeEncoding.UTF8;
            string mailFrom = !String.IsNullOrEmpty(smtpMailFrom) ? smtpMailFrom : (smtpUsername.Contains("@") ? smtpUsername : smtpUsername + "@" + (String.IsNullOrEmpty(smtpMailDomain) ? smtpHost : smtpMailDomain));
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
            msg.Subject = subject;
            msg.Body = emailBody;
            msg.To.Add(toEmailAddress);
        }

        public void Send()
        {
            log.LogMethodEntry();
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                smtpClient.Send(msg);
            }
            catch(Exception ex)
            {
                log.Error("Error occured in sending mail to client", ex);
                log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
