/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - pop up message box with message
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By            Remarks          
 *********************************************************************************************
 *2.120.0     19-May-2021            Lakshminarayana        Created
 ********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// pop up message box with message
    /// </summary>
    public partial class WaitDialog : Form
    {
        public WaitDialog(string message, bool showProgressbar = false)
        {
            InitializeComponent();
            label.Text = message;
            lblProgressMessage.Visible = showProgressbar;
            pbProgress.Visible = showProgressbar;
            if (showProgressbar == false)
            {
                label.Location = new System.Drawing.Point(label.Location.X, (this.Height - label.Height) / 2);
            }
        }

        public void ReportProgress(ProgressReport progressReport)
        {
            lblProgressMessage.Text = progressReport.ProgressMessage;
            pbProgress.Value = progressReport.Percentage;
        }
    }

    public class ProgressReport
    {
        public ProgressReport(int percentage, string progressMessage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentException("Invalid percentage parameter", "percentage");
            }
            Percentage = percentage;
            ProgressMessage = string.IsNullOrWhiteSpace(progressMessage) ? string.Empty : progressMessage.Trim().Substring(0, Math.Min(200, progressMessage.Length));
        }

        public int Percentage { get; private set; }

        public string ProgressMessage { get; private set; }
    }
}
