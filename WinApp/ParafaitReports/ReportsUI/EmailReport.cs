/********************************************************************************************
* Project Name - Parafait Report
* Description  - UI of EmailReport
 **************
 **Version Log
 **************
 *Version     Date              Modified By           Remarks          
 *********************************************************************************************
 * 2.80       23-Aug-2019      Jinto Thomas        Added logger into methods
 * 2.80        18-Sep-2019       Dakshakh raj           Modified : added logs 
 ********************************************************************************************/

using System;
using Semnox.Parafait.Reports;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//using ReportsLibrary;

using Telerik.Reporting;

namespace Semnox.Parafait.Report.Reports
{
    /// <summary>
    /// EmailReport Class
    /// </summary>
    public partial class EmailReport : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SendOnDemandEmailDelegate delegate
        /// </summary>
        public delegate void SendOnDemandEmailDelegate(string Format, string EmailList, Label lblEmailSendingStatus);

        /// <summary>
        /// SendOnDemandEmailDelegate
        /// </summary>
        public SendOnDemandEmailDelegate setEmailParamsCallback;

        /// <summary>
        /// format
        /// </summary>
        public string format;

        /// <summary>
        /// emailList
        /// </summary>
        public string emailList;

        /// <summary>
        /// backgroundMode
        /// </summary>
        public bool backgroundMode;

        /// <summary>
        /// rptSrc
        /// </summary>
        public ReportSource rptSrc;

        /// <summary>
        /// reportKey
        /// </summary>
        string reportKey =null, reportName=null;

        /// <summary>
        /// fromDate
        /// </summary>
        DateTime fromDate = new DateTime(), toDate = new DateTime();
        ArrayList sites=null;

         
        List<clsReportParameters.SelectedParameterValue> arguments=null;
        //BackgroundWorker bgWorker;


        /// <summary>
        /// EmailReport()
        /// </summary>
        public EmailReport()
        {
            log.LogMethodEntry();
            InitializeComponent();
            backgroundMode = false;
            Common.Utilities.setLanguage(this);
            lblSendEmailStatus.Text = "";
            log.LogMethodExit();
        }

        //public EmailReport(string ReportKey, string ReportName, DateTime FromDate, DateTime ToDate, ArrayList Sites, List<clsReportParameters.SelectedParameterValue> Args)
        //{
        //    InitializeComponent();
        //    backgroundMode = true;
        //    reportKey = ReportKey;
        //    reportName = ReportName;
        //    fromDate = FromDate;
        //    toDate = ToDate;
        //    sites = Sites;
        //    arguments = Args;
        //    lblSendEmailStatus.Text = "";
        //    bgWorker = new BackgroundWorker();
        //    bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
        //    bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        //}
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnEmailReport_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnEmailReport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (txtEmailList.Text == "")
            {
                MessageBox.Show(Common.MessageUtils.getMessage(572));
                return;
            }

            string[] toEmail = txtEmailList.Text.Split(',');
            foreach (string email in toEmail)
            {
                //Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                Regex regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
                Match match = regex.Match(email.ToLower());
                if (!match.Success)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(572));
                    return;
                }
            }
            lblSendEmailStatus.Text = Common.MessageUtils.getMessage("Sending Email");
            format = cmbOutputFormat.SelectedItem.ToString();
            emailList = txtEmailList.Text;
            setEmailParamsCallback(format, emailList, lblSendEmailStatus);
            log.LogMethodExit();
        }

        /// <summary>
        /// EmailReport_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void EmailReport_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cmbOutputFormat.SelectedIndex = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// bgWorker_DoWork
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            log.LogMethodEntry();
            string message = "";
            rptSrc = Common.GetReportSource(reportKey, reportName, fromDate, toDate, sites, ref message, arguments,backgroundMode);
            log.LogMethodExit();
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }
}
