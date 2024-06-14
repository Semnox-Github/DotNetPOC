/********************************************************************************************
 * Project Name - SendEmailUI
 * Description  - SendEmailUI class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value 
 *2.130.04    17-Feb-2022      Nitin Pai       Added FromEmail and Domain for SendGrid mailing service
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Net.Mail;
using System.Net;
using System.IO;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    public partial class SendEmailUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Added for file attachment on 28-jul-2016
        public class FileAttachment
        {
            public string attachedFilename = string.Empty;
            public string attachedDirectoryName = string.Empty;
            public double attachedFileSize = 0;
        }
        //end

        string toEmail, ccEmail, replyToEmail, subject, body, attachFileName, attachDir, attachImage, imageContentId;//Added two new varilable 06-Oct-2015
        bool backgroundMode;
        public bool EmailSentSuccessfully = false;
        Semnox.Core.Utilities.Utilities Utilities;

        //Added for file attachment on 28-jul-2016
        bool attachmentRequired = false;
        string fileName = string.Empty;
        string directoryNameWithDate = string.Empty;
        List<FileAttachment> LstFileAttachment = new List<FileAttachment>();
        //end

        //Begin: Booking Related changes.Added a new constructor to send images through email on 20-Sept-2015//
        public SendEmailUI(string ptoEmail, string pccEmail, string preplyToEmail, string psubject, string pbody, string pattachFileName, string pattachDir, string pattachImage, string pimageContentId, bool pbackgroundMode, Semnox.Core.Utilities.Utilities ParafaitUtilities, bool attachRequired)
            : this(ptoEmail, pccEmail, preplyToEmail, psubject, pbody, pattachFileName, pattachDir, pbackgroundMode, ParafaitUtilities)
        {
            log.LogMethodEntry(ptoEmail, pccEmail, preplyToEmail, psubject, pbody, pattachFileName, pattachDir, pattachImage, pimageContentId, pbackgroundMode, ParafaitUtilities, attachRequired);
            attachImage = pattachImage;
            imageContentId = pimageContentId;
            if (String.IsNullOrEmpty(pattachFileName) == false)
            {
                attachFileName = pattachFileName;
            }
            //Modified for enabling the attaching files 28-jul-2016
            btnAttach.Enabled = attachRequired;
            btnAttach.Visible = attachRequired;
            attachmentRequired = attachRequired;
            //end
            log.LogMethodExit(null);
        }
        //End: Booking Related changes on 20-Sept-2015

        //starts: Modified for enabling the attaching files 28-jul-2016
        public SendEmailUI(string ptoEmail, string pccEmail, string preplyToEmail, string psubject, string pbody, string pattachFileName, string pattachDir, bool pbackgroundMode, Semnox.Core.Utilities.Utilities ParafaitUtilities, bool attachRequired)
            : this(ptoEmail, pccEmail, preplyToEmail, psubject, pbody, pattachFileName, pattachDir, pbackgroundMode, ParafaitUtilities)
        {
            log.LogMethodEntry(ptoEmail,pccEmail, preplyToEmail, psubject, pbody, pattachFileName, pattachDir, pbackgroundMode, ParafaitUtilities, attachRequired);
            if (String.IsNullOrEmpty(pattachFileName) == false)
            {
                attachFileName = pattachFileName;
            }
            btnAttach.Enabled = attachRequired;
            btnAttach.Visible = attachRequired;
            attachmentRequired = attachRequired;
            log.LogMethodExit(null); 
        }
        //Ends: Modified for enabling the attaching files 28-jul-2016

        public SendEmailUI(string ptoEmail, string pccEmail, string preplyToEmail, string psubject, string pbody, string pattachFileName, string pattachDir, bool pbackgroundMode, Semnox.Core.Utilities.Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ptoEmail,pccEmail, preplyToEmail, psubject, pbody, pattachFileName, pattachDir, pbackgroundMode, ParafaitUtilities);
            InitializeComponent();

            Utilities = ParafaitUtilities;

            toEmail = ptoEmail;
            ccEmail = pccEmail;
            replyToEmail = preplyToEmail;
            subject = psubject;
            body = pbody;
            attachFileName = pattachFileName;
            attachDir = pattachDir;
            backgroundMode = pbackgroundMode;

            txtToEmails.Text = toEmail;
            txtCCEmails.Text = ccEmail;
            txtreplyToEmail.Text = replyToEmail;
            txtSubject.Text = subject;
            txtBody.Text = body;
            lnkAttachFile.Text = attachFileName;

            statusMessage("", 0);

            if (backgroundMode)
                btnSend_Click(null ,null);
            log.LogMethodExit(null);
        }

        void statusMessage(string message, int progress)
        {
            log.LogMethodEntry(message, progress);
            tsStatusLabel.Text = message;
            tsProgressBar.Value = progress;
            Application.DoEvents();
            log.LogMethodExit(null);
        }

        SmtpClient mailClient = null;
        private async void btnSend_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                statusMessage("Getting SMTP details...", 10);
                string smtpHost = Utilities.getParafaitDefaults("SMTP_HOST");
                int smtpPort = 587;
                try
                {
                    smtpPort = Convert.ToInt32(Utilities.getParafaitDefaults("SMTP_PORT")); // specify -1 to ignore, null to use 587
                }
                catch(Exception ex)
                {
                    log.Error("Error occured in smtp port", ex);
                    smtpPort = 587;
                }

                string smtpUsername = Utilities.getParafaitDefaults("SMTP_NETWORK_CREDENTIAL_USERNAME");
                string smtpPassword = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "SMTP_NETWORK_CREDENTIAL_PASSWORD");//Utilities.getParafaitDefaults("SMTP_NETWORK_CREDENTIAL_PASSWORD");
                string smtpDisplayName = Utilities.getParafaitDefaults("SMTP_FROM_DISPLAY_NAME");
                string smtpMailDomain = Utilities.getParafaitDefaults("SMTP_MAIL_ADDRESS_DOMAIN");
                string smtpMailFrom = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "SMTP_FROM_MAIL_ADDRESS");

                smtpHost = smtpHost == "" ? "smtp.gmail.com" : smtpHost;
                string replyTo = txtreplyToEmail.Text != "" ? txtreplyToEmail.Text : (smtpUsername == "" ? "noreply@noreply.noreply" : smtpUsername);
                smtpUsername = smtpUsername == "" ? "ParafaitReports@gmail.com" : smtpUsername;
                smtpPassword = smtpPassword == "" ? "semnox!1" : smtpPassword;
                smtpDisplayName = smtpDisplayName == "" ? Utilities.ParafaitEnv.SiteName : smtpDisplayName;

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

                if (Utilities.getParafaitDefaults("ENABLE_SMTP_SSL") == "Y")
                    mailClient.EnableSsl = true;
                else
                    mailClient.EnableSsl = false;

                if (txtToEmails.Text == "")
                {
                    if (!backgroundMode)
                        MessageBox.Show(Utilities.MessageUtils.getMessage(331), "To Email Error");
                    else
                    {
                        log.LogMethodExit(null, "Throwing Application excepton-" + Utilities.MessageUtils.getMessage(331));
                        throw new ApplicationException(Utilities.MessageUtils.getMessage(331));
                    }
                      
                    statusMessage(Utilities.MessageUtils.getMessage(491), 0);
                    log.LogMethodExit(null);
                    return;
                }

                string ToEmails = txtToEmails.Text;
                // code-02-06-2015--CC-ids were not getting emails.
                string ccEmails = txtCCEmails.Text;  //get the cc id in a variable,which can be passed to mail client              
                // code 02-06-2015 ends-
                string emailBody = txtBody.Text;
                if (emailBody.ToLower().Contains("<html") == false)
                {
                    emailBody = emailBody.Replace("\r\n", "\n");
                    emailBody = emailBody.Replace("\n\n", "<br /> <br /> ").Replace("\n", "<br />");
                }

                statusMessage("Creatng new message...", 20);
                MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.BodyEncoding = UnicodeEncoding.UTF8;

                //string mailFrom = smtpUsername.Contains("@") ? smtpUsername : smtpUsername + "@" + smtpHost;
                //string mailReplyTo = replyTo.Contains("@") ? replyTo : replyTo + "@" + smtpHost;
                string mailFrom = !String.IsNullOrEmpty(smtpMailFrom) ? smtpMailFrom : (smtpUsername.Contains("@") ? smtpUsername : smtpUsername + "@" + (String.IsNullOrEmpty(smtpMailDomain) ? smtpHost : smtpMailDomain));
                string mailReplyTo = replyTo.Contains("@") ? replyTo : replyTo + "@" + (String.IsNullOrEmpty(smtpMailDomain) ? smtpHost : smtpMailDomain);
                try
                {
                    msg.From = new MailAddress(mailFrom, smtpDisplayName);
                }
                catch (Exception ex)
                {
                     log.Error("Error occured while sending email", ex);
                    if (!backgroundMode)
                        MessageBox.Show(mailFrom + ": " + ex.Message, "From Email Error");
                    else
                    {
                        log.LogMethodExit(null, "Throwing application exception" + mailFrom + ": " + ex.Message);
                        throw new ApplicationException(mailFrom + ": " + ex.Message);
                    }
                       
                    statusMessage(Utilities.MessageUtils.getMessage(491), 0);
                    log.LogMethodExit(null);
                    return;
                }

                try
                {
                    msg.ReplyToList.Add(new MailAddress(mailReplyTo));
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while creating reply-to list", ex);
                    if (!backgroundMode)
                        MessageBox.Show(mailReplyTo + ": " + ex.Message, "Reply To Email Error");
                    else
                    {
                        log.LogMethodExit(null, "Throwing Application exception-" + mailReplyTo + ": " + ex.Message);
                        throw new ApplicationException(mailReplyTo + ": " + ex.Message);
                    }
                      
                    statusMessage(Utilities.MessageUtils.getMessage(491), 0);
                    log.LogMethodExit(null);
                    return;
                }
                msg.Subject = txtSubject.Text;
                msg.Body = emailBody;
                statusMessage("Adding emails...", 30);
                msg.To.Add(ToEmails);
                //code-02-06-2015--
                if (ccEmails.Length > 0)
                    msg.CC.Add(ccEmails); //Pass the cc ids also to mail client object
                // code-02-06-2015 ends--

                //starts : Modification for attaching files on 28-jul-2016
                if(attachmentRequired)
                {
                    try
                    {
                        if (LstFileAttachment != null && LstFileAttachment.Count > 0)
                        {
                            string fileDirectory;
                            foreach (FileAttachment d in LstFileAttachment)
                            {
                                fileDirectory = d.attachedDirectoryName.Substring(0, d.attachedDirectoryName.LastIndexOf("_"));
                                statusMessage("Attaching file...", 40);
                                msg.Attachments.Add(new Attachment(fileDirectory + "\\" + d.attachedFilename));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured in last file attachment", ex);
                        MessageBox.Show(ex.Message);
                        log.LogMethodExit(null);
                        return;
                    }
                }
                //ends : Modification for attaching files on 28-jul-2016

                if (attachFileName != null && attachFileName != "")
                {
                    try
                    {
                        statusMessage("Attaching file...", 40);
                        msg.Attachments.Add(new Attachment(attachDir + attachFileName));
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured in attaching the file", ex);
                        if (!backgroundMode)
                            MessageBox.Show(ex.Message, Utilities.MessageUtils.getMessage(332));
                        else
                        {
                            log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                            throw new ApplicationException(ex.Message);
                        }
                           
                        statusMessage(Utilities.MessageUtils.getMessage(491), 0);
                        log.LogMethodExit(null);
                        return;
                    }
                }  

                //Begin: Booking Related changes. Attaching the Image to the email body on 20-Sept-2015//
                if (attachImage != null && attachImage != "")
                {
                    try
                    {
                        statusMessage("Attaching file...", 40);
                        LinkedResource siteLogo = new LinkedResource(attachImage, "image/jpeg");
                        siteLogo.ContentId = imageContentId;
                        AlternateView imageView = AlternateView.CreateAlternateViewFromString(emailBody, null, "text/html");
                        imageView.LinkedResources.Add(siteLogo);
                        msg.AlternateViews.Add(imageView);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured in attaching the file", ex);
                        if (!backgroundMode)
                            MessageBox.Show(ex.Message, Utilities.MessageUtils.getMessage(332));
                        else
                        {
                            log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                            throw new ApplicationException(ex.Message);
                        }
                            
                        statusMessage(Utilities.MessageUtils.getMessage(491), 0);
                        log.LogMethodExit(null);
                        return;
                    }
                }
                //End: Booking Related changes.20-Sept-2015//

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    statusMessage("Sending mail...", 60);
                    //mailClient.Send(msg);
                    mailClient.SendCompleted += mailClient_SendCompleted;
                    btnSend.Enabled = false;
                    await mailClient.SendMailAsync(msg);
                    statusMessage(Utilities.MessageUtils.getMessage(333), 100);
                    EmailSentSuccessfully = true;
                    if (!backgroundMode)
                        MessageBox.Show(Utilities.MessageUtils.getMessage(333), "Success");

                    this.Close();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in sending mail to client", ex);
                    if (!backgroundMode)
                    {
                        statusMessage(Utilities.MessageUtils.getMessage(491), 0);
                        MessageBox.Show(ex.Message, "Send Error");
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                        throw new ApplicationException(ex.Message);
                    }
                       
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    msg.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in btn_send click", ex);
                statusMessage(Utilities.MessageUtils.getMessage(491), 0);
                if (!backgroundMode)
                    MessageBox.Show(ex.Message, "Exception");
                else
                {
                    log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                    throw new ApplicationException(ex.Message);
                }
                
            }
            finally
            {
                if (backgroundMode)
                    this.Close();
            }
            log.LogMethodExit(null);
        }

        void mailClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnSend.Enabled = true;
            log.LogMethodExit(null);
        }

        private void lnkAttachFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                System.Diagnostics.Process.Start(attachDir + attachFileName);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in attaching directory and filename", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (btnSend.Enabled == false && mailClient != null)
            {
                if (MessageBox.Show("Send Mail operation is in progress. Do you want to cancel?", "Cancel?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (mailClient != null)
                    {
                        mailClient.SendAsyncCancel();
                    }
                }
                else
                {
                    log.LogMethodExit(null);
                    return;
                }
                    
            }
            Close();
            log.LogMethodExit(null);
        }

        #region Enable File Attachments
        //Added on 28-jul-2016
        public void btnAttach_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            double sizeOfFile = 0;
            OpenFileDialog OpenDialog = new OpenFileDialog();
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(OpenDialog.FileName);

                if (extension == ".exe")
                {
                    MessageBox.Show("File not supported");
                    log.LogMethodExit(null);
                    return;
                }

                sizeOfFile = (double)new FileInfo(OpenDialog.FileName).Length / 1024; // convert to kilobytes
                if ((GetSizeOfAttachedFile() + sizeOfFile) > 10000)
                {
                    MessageBox.Show("File size cannot exceed 10MB");
                    log.LogMethodExit(null);
                    return;
                }

                try
                {
                    LinkLabel labelLink = new LinkLabel();
                    labelLink.Text = System.IO.Path.GetFileName(OpenDialog.FileName);
                    string fullPath = OpenDialog.FileName;
                    labelLink.Tag = fullPath.Substring(0, fullPath.LastIndexOf('\\')) + "_Date:" + DateTime.Now;
                    labelLink.AutoSize = true;
                    labelLink.Click += labelLink_Click;
                    flpAttachments.Controls.Add(labelLink);

                    FileAttachment attachFile = new FileAttachment();
                    attachFile.attachedFilename = labelLink.Text;
                    attachFile.attachedDirectoryName = labelLink.Tag.ToString();
                    attachFile.attachedFileSize = sizeOfFile;
                    LstFileAttachment.Add(attachFile);
                }
                catch(Exception ex)
                {
                    log.Error("Error occured in attaching file", ex);
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit(null);
        }

        //Get the total size of attached files
        double GetSizeOfAttachedFile()
        {
            log.LogMethodEntry();
            double size = 0;
            foreach (FileAttachment d in LstFileAttachment)
            {
              size = size + d.attachedFileSize;
            }
            log.LogMethodExit(size);
            return size;
        }
       
        public void labelLink_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                LinkLabel attachmentLink = (sender as LinkLabel);
                fileName = attachmentLink.Text;
                directoryNameWithDate = attachmentLink.Tag.ToString();
                AttachmentContextMenu.Show(attachmentLink, new Point(0, 0), ToolStripDropDownDirection.AboveRight);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in attachement of link", ex);
                MessageBox.Show(ex.Message);
            }
        }
        
        private void AttachmentContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ClickedItem == menuOpen)
                {
                    AttachmentContextMenu.Close();
                    string fileDirectory = directoryNameWithDate.Substring(0, directoryNameWithDate.LastIndexOf("_"));
                    System.Diagnostics.Process.Start(fileDirectory + "\\" + fileName);     
                }
                else if (e.ClickedItem == menuRemove)
                {
                    //Remove control from flowlayoutPanel
                    foreach (Control cntrl in flpAttachments.Controls)
                    {
                        if (cntrl.Text == fileName && cntrl.Tag.ToString() == directoryNameWithDate)
                        {
                            flpAttachments.Controls.Remove(cntrl);
                            break;
                        }
                    }

                    //Remove object from LstFileAttachment
                    foreach(FileAttachment d in LstFileAttachment)
                    {
                        if(d.attachedFilename == fileName && d.attachedDirectoryName == directoryNameWithDate)
                        {
                            LstFileAttachment.Remove(d);
                            break;
                        }
                    }
                    AttachmentContextMenu.Close();
                }
            }
            catch(Exception ex) 
            {
                log.Error("Error occured due to AttachmentContextMenu_ItemClicked", ex);
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
