/********************************************************************************************
 * Project Name - MessagingClientBL 
 * Description  -MessagingClientBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     24-Dec-2021      Girish          Barcode image issue in email template
 *2.120.6     20-Feb-2022   Nitin Pai          SendGrid Email Change - From Email and Domain Name
 ********************************************************************************************/

using GenCode128;
using QRCoder;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Semnox.Parafait.MessagingClients
{
    class Email : MessagingClientBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string smtpHost;
        int smtpPort;
        string smtpUsername;
        string smtpPassword;
        string smtpDisplayName;
        string smtpMailFrom;
        string smtpMailDomain;
        bool EnableSsl;

        private MessagingClientDTO messagingClientDTO;
        private ExecutionContext executionContext;
        public Email(ExecutionContext executionContext, MessagingClientDTO messagingClientDTO) : base(executionContext, messagingClientDTO)
        {
            log.LogMethodEntry(executionContext, messagingClientDTO);
            this.executionContext = executionContext;
            this.messagingClientDTO = messagingClientDTO;
            Initialize();
            log.LogMethodExit(null);
        }


        /// <summary>
        /// Initialize method to initialize Gateway Configuration
        /// </summary>
        /// <param name="inUtilities"></param>
        /// <returns></returns>
        public void Initialize()
        {
            log.LogMethodEntry();
            // Utilities = inUtilities;

            //smtpHost = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_HOST");
            //smtpPort = 587;
            //smtpUsername = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_NETWORK_USERNAME");
            //smtpPassword = ParafaitDefaultContainerList.GetDecryptedParafaitDefault(executionContext, "CRM_SMTP_NETWORK_PASSWORD"); //Utilities.getParafaitDefaults("CRM_SMTP_NETWORK_PASSWORD");
            //smtpDisplayName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_FROM_NAME");
            //EnableSsl = (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_ENABLE_SMTP_SSL") == "Y");

            smtpUsername = messagingClientDTO.UserName;
            smtpPassword = messagingClientDTO.Password;//Utilities.getParafaitDefaults("CRM_SMS_PASSWORD");
            smtpHost = messagingClientDTO.HostUrl;
            smtpPort = messagingClientDTO.SmtpPort;
            smtpMailFrom = messagingClientDTO.FromEmail;
            smtpMailDomain = messagingClientDTO.Domain;
            smtpDisplayName = messagingClientDTO.Sender;
            EnableSsl = messagingClientDTO.EnableSsl;
            log.LogMethodExit();
            // return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagingRequestDTO"></param>
        public override MessagingRequestDTO Send(MessagingRequestDTO messagingRequestDTO)
        {
            log.LogMethodEntry(messagingClientDTO);
            base.Send(messagingRequestDTO);
            AlternateView alternateView = null;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            SmtpClient mailClient;
            string ReplyToEmail = null;

            string result = string.Empty;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

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

                string mailFrom = !String.IsNullOrEmpty(smtpMailFrom) ? smtpMailFrom : (smtpUsername.Contains("@") ? smtpUsername : smtpUsername + "@" + (String.IsNullOrEmpty(smtpMailDomain) ? smtpHost : smtpMailDomain));
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

                if (messagingRequestDTO.Body.ToLower().Contains("<html") == false)
                {
                    messagingRequestDTO.Body = messagingRequestDTO.Body.Replace("\r\n", "\n");
                    messagingRequestDTO.Body = messagingRequestDTO.Body.Replace("\n\n", "<br /> <br /> ").Replace("\n", "<br />");
                }
                if (messagingRequestDTO.Body.Contains("<TransactionOTP>") == true)
                {
                    log.Debug("messagingRequestDTO.Body.Contains TransactionOTP");
                    string body = messagingRequestDTO.Body;
                    String beginTagSearchString = "<TransactionOTP>";
                    int startIndex = body.IndexOf(beginTagSearchString);
                    String endTagSearchString = "</" + beginTagSearchString.Substring(1);
                    log.Debug("searchString :" + endTagSearchString);
                    int endIndex = body.IndexOf(endTagSearchString);
                    string TransactionOTP = body.Substring(startIndex + beginTagSearchString.Length, endIndex - (startIndex + beginTagSearchString.Length));
                    log.Debug("TransactionOTP : " + TransactionOTP);
                    if (!String.IsNullOrWhiteSpace(TransactionOTP))
                    {
                        log.Debug("TransactionOTP Not empty : " + TransactionOTP);
                        Image barcodeImage;
                        barcodeImage = Code128Rendering.MakeBarcodeImage(TransactionOTP, 2, true);
                        byte[] trxBarCode = null;
                        if (barcodeImage != null)
                        {
                            log.Debug("barcodeImage != null");
                            using (var stream = new MemoryStream())
                            {
                                barcodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                trxBarCode = stream.ToArray();
                            }
                        }
                        if (trxBarCode != null)
                        {
                            log.Debug("trxBarCode != null");
                            var imgLogo1 = new LinkedResource(new MemoryStream(trxBarCode))
                            {
                                ContentId = Guid.NewGuid().ToString(),
                                ContentType = new ContentType("image/png")
                            };
                            var imageTag = string.Format("<p><img src=cid:{0} /><p>", imgLogo1.ContentId);
                            string emailBody = messagingRequestDTO.Body;
                            messagingRequestDTO.Body = emailBody.Replace("<TransactionOTP>" + TransactionOTP + "</TransactionOTP>", imageTag);
                            log.Debug("messagingRequestDTO.Body : " + messagingRequestDTO.Body);
                            alternateView = AlternateView.CreateAlternateViewFromString(messagingRequestDTO.Body, null, "text/html");
                            alternateView.LinkedResources.Add(imgLogo1);
                            log.Debug(" alternateView.LinkedResources added ");
                        }
                    }
                }
                if (messagingRequestDTO.Body.Contains("<TransactionOTPQR>") == true)
                {
                    log.Debug("messagingRequestDTO.Body.Contains TransactionOTPQR");
                    string body = messagingRequestDTO.Body;
                    String beginTagSearchString = "<TransactionOTPQR>";
                    int startIndex = body.IndexOf(beginTagSearchString);
                    String endTagSearchString = "</" + beginTagSearchString.Substring(1);
                    log.Debug("searchString :" + endTagSearchString);
                    int endIndex = body.IndexOf(endTagSearchString);
                    string TransactionOTP = body.Substring(startIndex + beginTagSearchString.Length, endIndex - (startIndex + beginTagSearchString.Length));
                    log.Debug("TransactionOTP : " + TransactionOTP);
                    if (!String.IsNullOrWhiteSpace(TransactionOTP))
                    {
                        log.Debug("TransactionOTP Not empty : " + TransactionOTP);

                        byte[] trxQRCode = null;
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(TransactionOTP, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);

                        if (qrCode != null)
                        {
                            log.Debug("Initialized QR code generator");
                            Image image = qrCode.GetGraphic(5);
                            if (image != null)
                            {
                                log.Debug("Got QR code image");
                                using (var stream = new MemoryStream())
                                {
                                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    trxQRCode = stream.ToArray();
                                }
                            }
                        }

                        if (trxQRCode != null)
                        {
                            log.Debug("trxBarCode != null");
                            var imgLogo1 = new LinkedResource(new MemoryStream(trxQRCode))
                            {
                                ContentId = Guid.NewGuid().ToString(),
                                ContentType = new ContentType("image/png")
                            };
                            var imageTag = string.Format("<p><img src=cid:{0} /><p>", imgLogo1.ContentId);
                            string emailBody = messagingRequestDTO.Body;
                            messagingRequestDTO.Body = emailBody.Replace("<TransactionOTPQR>" + TransactionOTP + "</TransactionOTPQR>", imageTag);
                            log.Debug("messagingRequestDTO.Body : " + messagingRequestDTO.Body);
                            alternateView = AlternateView.CreateAlternateViewFromString(messagingRequestDTO.Body, null, "text/html");
                            alternateView.LinkedResources.Add(imgLogo1);
                            log.Debug(" alternateView.LinkedResources added ");
                        }
                    }
                }
                msg.Subject = messagingRequestDTO.Subject;
                if (alternateView != null)
                {
                    log.Debug("AlternateViews ");
                    msg.AlternateViews.Add(alternateView);
                    log.Debug(" alternateView.LinkedResources added to body ");
                }
                else
                {
                    log.Debug("Normal View ");
                    msg.Body = messagingRequestDTO.Body;
                }
                msg.Subject = messagingRequestDTO.Subject;
                msg.BodyEncoding = UnicodeEncoding.UTF8;
                msg.To.Add(messagingRequestDTO.ToEmail);
                if (!string.IsNullOrEmpty(messagingRequestDTO.Cc))
                {
                    msg.CC.Add(messagingRequestDTO.Cc);
                }
                if (!string.IsNullOrEmpty(messagingRequestDTO.Bcc))
                {
                    msg.Bcc.Add(messagingRequestDTO.Bcc);
                }
                if (!string.IsNullOrEmpty(messagingRequestDTO.AttachFile))
                {
                    try
                    {
                        Attachment oAttachment = new Attachment(messagingRequestDTO.AttachFile);
                        string extn = System.IO.Path.GetExtension(messagingRequestDTO.AttachFile).ToLower();
                        if (extn.Equals(".gif")
                            || extn.Equals(".jpg")
                            || extn.Equals(".bmp")
                            || extn.Equals(".png"))
                        {
                            oAttachment.ContentId = System.IO.Path.GetFileName(messagingRequestDTO.AttachFile);
                        }
                        msg.Attachments.Add(oAttachment);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured during message attachments", ex);
                        log.LogMethodExit(null, "Throwing Application Exception" + messagingRequestDTO.AttachFile + ": " + ex.Message);
                        throw new ApplicationException(messagingRequestDTO.AttachFile + ": " + ex.Message);
                    }
                }

                try
                {
                    mailClient.Send(msg);
                    messagingRequestDTO.SendDate = ServerDateTime.Now;
                    base.UpdateResults(messagingRequestDTO, "Success", "Success");
                }
                catch (SmtpException smex)
                {
                    log.Error("Smtp exception occured while sending message  to mailclient", smex);
                    log.LogMethodExit(null, "Throwing Application Exception" + messagingRequestDTO.ToEmail + ": " + ((smex.InnerException == null) ? smex.ToString() : smex.InnerException.Message));
                    base.UpdateResults(messagingRequestDTO, "Error", smex.Message);
                    //throw new ApplicationException(messagingRequestDTO.ToEmail + ": " + ((smex.InnerException == null) ? smex.ToString() : smex.InnerException.Message));
                }
                catch (InvalidOperationException ioex)
                {
                    log.Error("InvalidOperationException occured while sending message to mail client", ioex);
                    log.LogMethodExit(null, "Throwing Application Exception" + messagingRequestDTO.ToEmail + ": " + ioex.Message);
                    base.UpdateResults(messagingRequestDTO, "Error", ioex.Message);
                    //throw new ApplicationException(messagingRequestDTO.ToEmail + ": " + ioex.Message);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while sending email synchronously", ex);
                    log.LogMethodExit(null, "Throwing Application Exception" + messagingRequestDTO.ToEmail + ": " + ex.Message);
                    base.UpdateResults(messagingRequestDTO, "Error", ex.Message);
                    //throw new ApplicationException(messagingRequestDTO.ToEmail + ": " + ex.Message);
                }
                finally
                {
                    msg.Dispose();
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occured in sendSMSSynchronous", ex);
                log.LogMethodExit(null, "Throwing ApplicationException- " + messagingRequestDTO.ToMobile + ": " + ex.Message);
                throw new ApplicationException(messagingRequestDTO.ToMobile + ": " + ex.Message);
            }

            log.LogMethodExit(result);
            return messagingRequestDTO;
        }

        /// <summary>
        /// Populate Template method to populate the Template parameters 
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="paramsList"></param>
        ///// <returns></returns>
        //public override string PopulateTemplate(string Template, List<KeyValuePair<string, string>> paramsList)
        //{
        //    log.LogMethodEntry(Template, paramsList);

        //    foreach(var parameter in paramsList)
        //    {
        //        Template = Template.Replace(parameter.Key.ToLower(), parameter.Value);
        //    }

        //    log.LogMethodExit(Template);
        //    return Template;
        //}


        /// <summary>
        /// SendRequest Method to send the SMS Request
        /// </summary>
        /// <param name="PhoneNos"></param>
        /// <param name="Template"></param>
        /// <returns></returns>
        //public override string SendRequest(string PhoneNos, string Template)
        //{
        //    log.LogMethodEntry(PhoneNos, Template);
        //    string result = string.Empty;
        //    try
        //    {
        //        string responseCode = string.Empty;
        //        if (Template.Length > 160)
        //            Template = Template.Substring(0, 160);

        //        Template = System.Net.WebUtility.UrlEncode(Template);

        //        string smsapi = SMSurl.Replace("@UserId", SMSuserid).Replace
        //                                        ("@Password", SMSpasswd).Replace
        //                                        ("@Message", Template).Replace
        //                                        ("@PhoneNumber", PhoneNos);
        //        result = sendHttpRequest(smsapi);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured in sendSMSSynchronous", ex);
        //        log.LogMethodExit(null, "Throwing ApplicationException- " + PhoneNos + ": " + ex.Message);
        //        throw new ApplicationException(PhoneNos + ": " + ex.Message);
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}


        /// <summary>
        /// sendHttpRequest Method to send Http Request to API
        /// </summary>
        /// <param name="smsapi"></param>
        /// <returns></returns>
        //    protected string sendHttpRequest(string smsapi)
        //    {
        //        log.LogMethodEntry(smsapi);
        //        string responseCode = string.Empty;
        //        WebRequest request = null;
        //        HttpWebResponse response = null;
        //        try
        //        {
        //            request = WebRequest.Create(smsapi);
        //            response = (HttpWebResponse)request.GetResponse();
        //            Stream stream = response.GetResponseStream();
        //            Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
        //            StreamReader reader = new System.IO.StreamReader(stream, ec);
        //            responseCode = reader.ReadToEnd();
        //            reader.Close();
        //            stream.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error("Error occurred in sendHttpRequest", ex);
        //            log.LogMethodExit(null, "Throwing ApplicationException- " + responseCode);
        //        }
        //        finally
        //        {
        //            if (response != null)
        //                response.Close();
        //        }
        //        log.LogMethodExit(responseCode);
        //        return responseCode;
        //    }

        //}
    }
}
