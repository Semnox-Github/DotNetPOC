/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmKioskActivituDetails.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80         4-Sep-2019       Deeksha        Added logger methods.
 *2.150.1      22-Feb-2023       Guru S A       Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;

// This form is created on 25-06-2015 to display the report of kiosk activity.
// this report is same as the option which is there in run report-> kiosk activity details report
namespace Parafait_Kiosk
{
    public partial class KioskActivityDetails : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        public List<int> listTrxId = new List<int>();
        int scrollIndex = 0;
        public KioskActivityDetails()
        {
            log.LogMethodEntry();
            Utilities.setLanguage();
            InitializeComponent();
            Utilities.setLanguage(this);

            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
                panelKioskActivity.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                if (KioskStatic.CurrentTheme.TextForeColor != Color.White)
                    dgvKioskActivity.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                else
                    dgvKioskActivity.ForeColor = Color.DarkOrchid;
                //Ends:Modification on 17-Dec-2015 for introducing new theme
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            clearFields();
            if (string.IsNullOrEmpty(KioskStatic.showActivityDuration) || KioskStatic.showActivityDuration.Equals("0"))//checking for the value if it is zero then setting 15 as default value.
            {
                KioskStatic.showActivityDuration = "15";// By setting 15 this report will display the report of past 15 min activity
            }
            lblGreeting.Text = KioskStatic.Utilities.MessageUtils.getMessage(lblGreeting.Text);

            //btnPrev.Top = this.Top + 10;//Modification on 17-Dec-2015 for introducing new theme
            log.LogMethodExit();
        }

        private void clearFields()
        {
            //Clearing the datagrid view
            log.LogMethodEntry();
            dgvKioskActivity.DataSource = null;
            log.LogMethodExit();
        }

        private void fetchTrxDetails()
        {
            log.LogMethodEntry();
            try
            {
                //Fetching the data and assigning to the datagridview.            
                SqlDataAdapter da;
                SqlCommand kioskActivityCmd = Utilities.getCommand();
                kioskActivityCmd.CommandText = "SELECT kioskname as 'Kiosk Name',TimeStamp as Date,Activity ,CardNumber as 'Card Number',case notecoinflag when 'N' then 'Note' when 'C' then 'Coin' else '' end 'Note/ Coin?',Value as Amount,Message,KioskTrxId,TrxId"
                    + " from dbo.KioskActivityLog where KioskName='" + Utilities.ParafaitEnv.POSMachine + "'  and TimeStamp >= @date order by TimeStamp desc";
                kioskActivityCmd.Parameters.AddWithValue("@date", DateTime.Now.Hour < 6 ? DateTime.Now.AddDays(-1).Date.AddHours(6) : DateTime.Now.Date.AddHours(6));
                DataTable kioskActivityTbl = new DataTable();
                da = new SqlDataAdapter(kioskActivityCmd);
                da.Fill(kioskActivityTbl);
                dgvKioskActivity.DataSource = kioskActivityTbl;
                Utilities.setLanguage(dgvKioskActivity);
                dgvKioskActivity.Columns["btnPrintTrx"].ReadOnly = false;
                dgvKioskActivity.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                dgvKioskActivity.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvKioskActivity.Columns["Note/ Coin?"].Width = 0;
                dgvKioskActivity.ColumnHeadersDefaultCellStyle.Font = new Font(dgvKioskActivity.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, dgvKioskActivity.ColumnHeadersDefaultCellStyle.Font.Style);
                dgvKioskActivity.Refresh();
                kioskActivityCmd.Connection.Close();
                if (dgvKioskActivity.SelectedCells.Count > 0)
                    dgvKioskActivity.SelectedCells[0].Selected = false;
                vScrollBarGp.Maximum = dgvKioskActivity.Rows.Count + 1;
                hScroll.Maximum = dgvKioskActivity.Columns.Count + 5;
                if (dgvKioskActivity.Rows.Count <= 6)
                {
                    btnRight.Visible = false;
                }
                btnLeft.Visible = false;


            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }
        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.back_btn;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            // btnPrev.BackgroundImage = Properties.Resources.back_btn_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void vScrollBarGp_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvKioskActivity.Rows.Count > 0)
                dgvKioskActivity.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }

        private void KioskActivityDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;

            this.Invalidate();
            this.Refresh();
            fetchTrxDetails();
            KioskTimerSwitch(false);
            log.LogMethodExit();
        }

        private void KioskActivityDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if ((Keys)e.KeyChar == Keys.Escape)
                this.Close();
            log.LogMethodExit();
        }

        private void hScroll_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvKioskActivity.Columns.Count > 0)
            {
                dgvKioskActivity.FirstDisplayedScrollingColumnIndex = e.NewValue;
            }
            log.LogMethodExit();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            scrollIndex -= 6;
            if (scrollIndex >= 6 || (scrollIndex == 0 && dgvKioskActivity.RowCount > 6))
                btnRight.Visible = true;
            dgvKioskActivity.FirstDisplayedScrollingRowIndex = scrollIndex;
            if (scrollIndex <= 0)
                btnLeft.Visible = false;
            log.LogMethodExit();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            scrollIndex += 6;
            btnLeft.Visible = true;
            dgvKioskActivity.FirstDisplayedScrollingRowIndex = scrollIndex;
            if (scrollIndex + 6 >= dgvKioskActivity.RowCount)
                btnRight.Visible = false;
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (listTrxId.Count == 0)
            {
                frmOKMsg fok = new frmOKMsg("No Transactions to Print");
                fok.ShowDialog();
                log.LogMethodExit();
                return;
            }
            KioskStatic.logToFile("Calling Print Method through admin options");
            string message = "";
            PrintMultipleTransactions printMultipleTransactions = new PrintMultipleTransactions(KioskStatic.Utilities);
            try
            {
                if (!printMultipleTransactions.Print(listTrxId, false, ref message))
                {
                    frmOKMsg fok = new frmOKMsg(message);
                    fok.ShowDialog();
                    KioskStatic.logToFile("Print message: " + message);
                    log.LogMethodExit();
                    return;

                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                frmOKMsg fok = new frmOKMsg(ex.Message);
                fok.ShowDialog();
                log.LogMethodExit();
                return;
            }
            KioskStatic.logToFile("Completed Print through Admin options");
            log.LogMethodExit();
        }

        private void dgvKioskActivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex < 0)
            {
                log.LogMethodExit(null);
                return;
            }
            if (dgvKioskActivity.Columns[e.ColumnIndex].Name == "btnPrintTrx")
            {
                if (dgvKioskActivity.CurrentRow == null || dgvKioskActivity.CurrentRow.Cells["TrxId"].Value == DBNull.Value || dgvKioskActivity.CurrentRow.Cells["Card Number"].Value == DBNull.Value)
                {
                    log.LogMethodExit();
                    return;
                }
                else if (listTrxId.Contains(Convert.ToInt32(dgvKioskActivity.CurrentRow.Cells["TrxId"].Value)))
                {
                    listTrxId.Remove(Convert.ToInt32(dgvKioskActivity.CurrentRow.Cells["TrxId"].Value));
                    dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
                    (dgvKioskActivity.CurrentRow.Cells["btnPrintTrx"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                }
                else
                {
                    listTrxId.Add(Convert.ToInt32(dgvKioskActivity.CurrentRow.Cells["TrxId"].Value));
                    dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Highlight;
                    dgvKioskActivity.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
                    (dgvKioskActivity.CurrentRow.Cells["btnPrintTrx"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                }
            }
            log.LogMethodExit();
        }
    }
}

