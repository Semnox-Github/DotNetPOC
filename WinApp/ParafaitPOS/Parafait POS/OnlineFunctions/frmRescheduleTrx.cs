/********************************************************************************************
* Project Name - Parafait_POS - frmRescheduleTrx
* Description  - frmRescheduleTrx 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
********************************************************************************************* 
*2.60.0      21-Mar-2019      Iqbal              Created 
*2.70.0      20-Apr-2019      Guru S A           Booking phase 2 changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.OnlineFunctions
{
    public partial class frmRescheduleTrx : Form
    {
        Transaction trx;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmRescheduleTrx(Transaction Trx)
        {
            log.LogMethodEntry(Trx);
            trx = Trx;
            InitializeComponent();
            POSStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmRescheduleTrx_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dtpChangeDate.CustomFormat = POSStatic.ParafaitEnv.DATE_FORMAT;
            lblCurrentDate.Text = trx.TransactionDate.ToString(POSStatic.ParafaitEnv.DATE_FORMAT);
            log.LogMethodExit();
        }

        private void btnReschedule_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                RescheduleTransaction trxChangeDate = new RescheduleTransaction(trx.Trx_id, dtpChangeDate.Value.Date);
                if (trxChangeDate.Perform())
                {
                    trx.TrxDate = trx.TransactionDate = dtpChangeDate.Value.Date;
                    lblCurrentDate.Text = dtpChangeDate.Value.Date.ToString(POSStatic.ParafaitEnv.DATE_FORMAT);
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2067));// Transaction rescheduled successfully";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Reschedule Trx"));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (trx != null && trx.Trx_id > 0)
                {

                    string contentID = "";
                    string attachFile = null;
                    Semnox.Parafait.Communication.SendEmailUI semail;
                    if (POSStatic.ParafaitEnv.CompanyLogo != null)
                    {
                        contentID = "ParafaitTransactionLogo" + Guid.NewGuid().ToString() + ".jpg";//Content Id is the identifier for the image
                        attachFile = POSStatic.GetCompanyLogoImageFile(contentID);
                        if (string.IsNullOrWhiteSpace(attachFile))
                        {
                            contentID = "";
                        }
                    }

                    string onlineTicketEmailTemplateName = ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "ONLINE_TICKETS_B2C_EMAIL_TEMPLATE");
                    int onlineTicketEmailTemplateId = -1;
                    string templateEmailSubject = string.Empty;
                    if (string.IsNullOrEmpty(onlineTicketEmailTemplateName) == false)
                    {
                        EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(POSStatic.Utilities.ExecutionContext);
                        List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, onlineTicketEmailTemplateName));
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, POSStatic.Utilities.ExecutionContext.GetSiteId().ToString()));
                        List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                        if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                        {
                            onlineTicketEmailTemplateId = emailTemplateDTOList[0].EmailTemplateId;
                            templateEmailSubject = emailTemplateDTOList[0].Description;
                        }
                    }

                    if (onlineTicketEmailTemplateId < 0)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Email template for Online Tickets B2C is not set"));
                        return;
                    }
                    TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(POSStatic.Utilities.ExecutionContext, POSStatic.Utilities, onlineTicketEmailTemplateId, trx, null);
                    string emailContent = transactionEmailTemplatePrint.GenerateEmailTemplateContent();
                    string emailId = (trx.customerDTO != null ? trx.customerDTO.Email : string.Empty);
                    if (string.IsNullOrWhiteSpace(emailId) == true)
                    {
                        emailId = trx.GetEmailIdFromCustomerIdentifier();
                    }
                    string emailSubject = (string.IsNullOrEmpty(templateEmailSubject) ? MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Rescheduled Transaction") : templateEmailSubject);
                    semail = new Semnox.Parafait.Communication.SendEmailUI(emailId,
                                                        "", "", emailSubject, emailContent, null, "", attachFile, contentID, false, POSStatic.Utilities, true);

                    semail.ShowDialog();
                    if (semail.EmailSentSuccessfully)
                    {
                        log.Info("Email Sent Successfully");
                        //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Email is sent"));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
    }
}
