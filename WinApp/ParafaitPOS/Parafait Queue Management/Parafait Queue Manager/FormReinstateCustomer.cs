/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - frmReinstateCustomer 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.80        12-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
//using CompletIT.Windows.Forms.Export.Excel;
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using USBListener;


namespace ParafaitQueueManagement
{
    public partial class frmReinstateCustomer : Form
    {
        Players p;
        static USBListener.CardListener rfidCardListener;
       // Utilities parafaitUtility = new Utilities();
        int leftTrimCard = 0;
        int rightTrimCard = 0;
        private readonly TagNumberParser tagNumberParser;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmReinstateCustomer()
        {
            log.LogMethodEntry();
            InitializeComponent();
            leftTrimCard =Convert.ToInt32(Common.Utilities.getParafaitDefaults("LEFT_TRIM_CARD_NUMBER"));
            rightTrimCard = Convert.ToInt32(Common.Utilities.getParafaitDefaults("RIGHT_TRIM_CARD_NUMBER"));
            tagNumberParser = new TagNumberParser(Common.Utilities.ExecutionContext);
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Search();
            log.LogMethodExit();
        }
        public void Search()
        {
            log.LogMethodEntry();
           DateTime fromDate = Convert.ToDateTime(dtFromdate.Value.ToString("dd-MMM-yyyy"));
           DateTime todate = Convert.ToDateTime(dtToDate.Value.ToString("dd-MMM-yyyy"));
            DateTime startSearch;
            DateTime endSearch;
            TimeSpan searchTime;
            todate = todate.AddDays(1);
            string cardNumber = txtCardNumber.Text.Trim();
            string custName = txtCustName.Text.Trim();
            bool isRemoved = chkRemoved.Checked;
            bool isExpired = chkExpired.Checked;
            startSearch = DateTime.Now;
            DataTable dt = p.GetRemovedExpiredPlayers(fromDate, todate, cardNumber, custName, isRemoved, isExpired);
            dgReinstateCust.DataSource = dt;
            endSearch = DateTime.Now;
            searchTime = endSearch - startSearch;
            //MessageBox.Show(searchTime.TotalSeconds.ToString());
            lblSrchResult.Text = "About " + dt.Rows.Count + " results" + " ( " + Math.Round(Convert.ToDecimal(searchTime.TotalMilliseconds) / 1000, 3) + " seconds ) ";
            if (!dgReinstateCust.Columns.Contains("Reinstate"))
            {
                DataGridViewCheckBoxColumn chkcolumn = new DataGridViewCheckBoxColumn();
                chkcolumn.Name = "Reinstate";
                chkcolumn.Width = 60;
                chkcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                chkcolumn.HeaderText = "Reinstate";
                dgReinstateCust.Columns.Add(chkcolumn);
            }
            foreach (DataGridViewRow dgvrow in dgReinstateCust.Rows)
            {
                if (dgvrow.Cells["QueueId"].Value != null && dgvrow.Cells["QueueId"].Value != DBNull.Value)
                {   
                    dgvrow.Cells["Reinstate"].Value = false;
                }
            }
            dgReinstateCust.Sort(dgReinstateCust.Columns["play request"], ListSortDirection.Ascending);
            log.LogMethodExit();
        }
        bool registerUSBDevice()
        {
            log.LogMethodEntry();
            string USBReaderVID = Common.Utilities.getParafaitDefaults("USB_READER_VID");
            string USBReaderPID = Common.Utilities.getParafaitDefaults("USB_READER_PID");
            string USBReaderOptionalString = Common.Utilities.getParafaitDefaults("USB_READER_OPT_STRING");

            EventHandler currEventHandler = new EventHandler(CardScanCompleteEventHandle);
            if (rfidCardListener != null)
                rfidCardListener.Dispose();

            if (IntPtr.Size == 8)
                rfidCardListener = new CardListener64();
            else
                rfidCardListener = new CardListener32();

            bool flag = rfidCardListener.InitializeUSBCardReader(this, currEventHandler, USBReaderVID, USBReaderPID, USBReaderOptionalString);
            if (rfidCardListener.isOpen)
            {
                //displayMessageLine(simpleCardListener.dInfo.deviceName, MESSAGE);
                //MessageBox.Show("Connected USB Card Reader");
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                //  MessageBox.Show("Unable to find USB card reader");
                log.LogMethodExit(true);
                return false;
            }
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is USBListener.CardReaderScannedEventArgs)
            {
                USBListener.CardReaderScannedEventArgs checkScannedEvent = e as USBListener.CardReaderScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    log.LogMethodExit();
                    return;
                }

                txtCardNumber.Text = tagNumber.Value;
                Search();
                log.LogMethodExit();

            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Search();
            log.LogMethodExit();
        }
        public void setupDataGridview(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            dgv.BackgroundColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = Color.LightSteelBlue;
            dgv.AlternatingRowsDefaultCellStyle = gridViewAlternateRowStyle();
            dgv.RowHeadersDefaultCellStyle = gridViewRowHeaderStyle();
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgv.ColumnHeadersDefaultCellStyle = gridViewColumnHeaderStyle();
            dgv.RowHeadersVisible =
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeColumns = true;
            log.LogMethodExit();
        }
        public static DataGridViewCellStyle gridViewAlternateRowStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.Azure;
            log.LogMethodExit(style);
            return style;
        }
        public static DataGridViewCellStyle gridViewRowHeaderStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.White;
            log.LogMethodExit(style);
            return style;
        }
        public static DataGridViewCellStyle gridViewColumnHeaderStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.Azure;
            log.LogMethodExit(style);
            return style;
        }

        private void frmReinstateCustomer_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            registerUSBDevice();
            //txtCardNumber.Select();
            setupDataGridview(ref dgReinstateCust);
            dtFromdate.Value = Common.Utilities.getServerTime().AddDays(-1);
            loadData();
            chkRemoved.Checked = true;
            chkExpired.Checked = true;
            p = new Players();
            Search();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (DataGridViewRow dgvrow in dgReinstateCust.Rows)
            {   
                if( dgvrow.Cells["Reinstate"].Value!=null)
                {
                    if (dgvrow.Cells["Reinstate"].Value.ToString() != "False")
                    {
                        p.ReInstate(Convert.ToInt32(dgvrow.Cells["QueueId"].Value));
                    }
                }
            }
            MessageBox.Show("Data has been saved successfully");
            dgReinstateCust.Refresh();
            Search();
            log.LogMethodExit();
        }
        public void loadData()
        {
            log.LogMethodEntry();
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            SqlCommand cmd = Common.Utilities.getCommand();
            cmd.CommandText = "select customer_name from CustomerView(@PassPhrase)";
            string encryptedPassPhrase = Common.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
            string passPhrase = encryptedPassPhrase;
            cmd.Parameters.Add(new SqlParameter("@PassPhrase", passPhrase));
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    collection.Add(dt.Rows[i][0].ToString());
                }
            }
            txtCustName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtCustName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCustName.AutoCompleteCustomSource = collection;
            log.LogMethodExit();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Common.Utilities.ExportToExcel(dgReinstateCust, "Reinstate Customer List " + dtToDate.Value.ToString("dd-MMM-yyyy"),"Reinstate Customers");
            log.LogMethodExit();
        }
    }
}
