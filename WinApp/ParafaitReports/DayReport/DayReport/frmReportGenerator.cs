using System;
using System.Windows.Forms;

namespace DayReport
{
    public partial class frmReportGenerator : Form
    {
        public string Messages="";
        int secondsRemaining = 180;
        public frmReportGenerator()
        {
            InitializeComponent();
           // this.Width = panelSalesReport.Width + 10;
            exitTimer.Start();
        }       

        private void btnOK_Click(object sender, EventArgs e)
        {
            secondsRemaining = 180;
            lblMessage.Text = "Processing...";
            lblMessage.Refresh();
            DayReport dayRpt = new DayReport();
            if (string.IsNullOrEmpty(dtPicker.Text))
            {                
                lblMessage.Text="Please Enter Date.";
                lblMessage.Refresh();
                dtPicker.Focus();
            }
            else
            {
                Messages = Messages + DateTime.Now.ToString() + ":Sales report generating for the date:" + dtPicker.Text.ToString() + ".*";
                if (!dayRpt.processData(dtPicker.Text.ToString()))
                {
                    Messages = Messages + DateTime.Now.ToString() + ":Error in generating the sales report for the date:" + dtPicker.Text.ToString() + ".*";
                    lblMessage.Text="Error in generating the sales report.";
                    lblMessage.Refresh();
                }
                else
                {
                    Messages = Messages + DateTime.Now.ToString() + ":Sales report generation completed for the date:" + dtPicker.Text.ToString() + ".*";
                    lblMessage.Text="Sales report generation completed.";
                    lblMessage.Refresh();
                }
            }
        }

        private void exitTimer_Tick(object sender, EventArgs e)
        {
            if (secondsRemaining == 0)
            {
                exitTimer.Stop();
                Messages = Messages + DateTime.Now.ToString() + ":Closing manual report generator automatically.*";
                this.Close();
            }
            secondsRemaining--;
        }       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "closing...";
            lblMessage.Refresh();
            exitTimer.Stop();
            this.Close();
        }       
    }
}
