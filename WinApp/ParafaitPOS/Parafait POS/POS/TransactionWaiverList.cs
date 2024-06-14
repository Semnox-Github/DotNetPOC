/********************************************************************************************
* Project Name - Semnox.Parafait.Waiver - TransactionWaiverList
* Description  - Transaction Waiver List 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.40.1      26-Oct-2018      Indhu K            Created for displaying the signed waiver 
*                                                along with the transactiondetails
* 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards
* 2.80       11-Nov-2019      Guru S A           Waiver phase 2 changes 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Waiver;

namespace Parafait_POS
{
    public partial class TransactionWaiverList : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        bool showAmountFieldsTransaction = false;
        int trxId = -1;
        string customerName = string.Empty;
        string phoneNumber1 = string.Empty;
        string phoneNumber2 = string.Empty;
        string cardNumber = string.Empty;
        int customAttributeId;
        string customAttributeValue = string.Empty;
        DateTime fromDate;
        DateTime toDate;
        TextBox currentTextBox = null;
        bool DataFound = true;
        private readonly TagNumberParser tagNumberParser;

        public TransactionWaiverList(Utilities Utilities)
        {
            InitializeComponent();
            this.Utilities = Utilities;
            showAmountFieldsTransaction = (ParafaitDefaultContainerList.GetParafaitDefault(this.Utilities.ExecutionContext, "SHOW_AMOUNT_FIELDS_MYTRANSACTIONS") == "Y");
            fromDate = dtpFrom.Value.Date;
            toDate = dtpTo.Value = dtpTo.Value.Date.AddDays(1).AddHours(Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
            this.Utilities.setupDataGridProperties(ref dgvTrxDetails);
            this.Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
        }

        private void SignedWaiversUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            if (Common.Devices.PrimaryBarcodeScanner != null)
            {
                Common.Devices.PrimaryBarcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
            }
            LoadCustomAttributes();
            RefreshLines();
            currentTextBox = txtTrxId;
            log.LogMethodExit(null);
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                cardNumber = tagNumber.Value;
                txtCardNumber.Text = cardNumber;
                txtCardNumber.Select(0, 0);
            }
            log.LogMethodExit(null);
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                cardNumber = POSStatic.Utilities.ProcessScannedBarCode(checkScannedEvent.Message, POSStatic.ParafaitEnv.LEFT_TRIM_BARCODE, POSStatic.ParafaitEnv.RIGHT_TRIM_BARCODE);
                txtCardNumber.Text = cardNumber;
            }
            log.LogMethodExit(null);
        }

        private void LoadCustomAttributes()
        {
            log.LogMethodEntry();

            List<CustomAttributesDTO> customAttributesDSList = new List<CustomAttributesDTO>();
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOM_ATTRIBUTES_IN_WAIVER"));
            lookupValuesDTOList = new LookupValuesList(Utilities.ExecutionContext).GetAllLookupValues(searchlookupParameters);
            if (lookupValuesDTOList != null)
            {
                foreach (LookupValuesDTO lookupValueDTO in lookupValuesDTOList)
                {
                    string[] parts = lookupValueDTO.LookupValue.Split('|');
                    int partsLength = 0;
                    parts = lookupValueDTO.LookupValue.Split('|');

                    while (partsLength < parts.Length)
                    {
                        CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(Utilities.ExecutionContext);
                        List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, Applicability.CUSTOMER.ToString()));
                        searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.NAME, parts[partsLength]));
                        List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters);
                        if (customAttributesDTOList != null)
                        {
                            customAttributesDSList.Add(customAttributesDTOList[0]);
                        }
                        partsLength++;
                    }
                }

            }

            if (customAttributesDSList != null && customAttributesDSList.Count > 0)
            {
                customAttributesDSList.Insert(0, new CustomAttributesDTO());
                customAttributesDSList[0].Name = "<SELECT>";
                customAttributesDSList[0].CustomAttributeId = -1;

                cmbCustomFields.DataSource = customAttributesDSList;
                cmbCustomFields.DisplayMember = "Name";
                cmbCustomFields.ValueMember = "CustomAttributeId";
            }
            log.LogMethodExit(null);
        }

        private void RefreshLines()
        {
            log.LogMethodEntry();

            WaiverSignatureListBL waiverSignedListBL = new WaiverSignatureListBL(this.Utilities.ExecutionContext);
            DataTable dataTable = waiverSignedListBL.GetTransactionWaiverList(trxId, fromDate, toDate,
                                                 phoneNumber1, phoneNumber2, cardNumber,
                                                customerName, Convert.ToInt32(cmbCustomFields.SelectedValue), customAttributeValue);

            if (dataTable.Rows.Count == 0)
                DataFound = false;
            else
                DataFound = true;

            dgvTrxDetails.DataSource = dataTable;

            Utilities.setupDataGridProperties(ref dgvTrxDetails);

            dgvTrxDetails.BackgroundColor = Color.White;
            dgvTrxDetails.Columns[1].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvTrxDetails.Columns[3].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvTrxDetails.Columns["WaiversSigned"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvTrxDetails.Columns["LineId"].Visible = false;

            Utilities.setLanguage(dgvTrxDetails);
            dgvTrxDetails.Refresh();
            this.dgvTrxDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dataTable.Rows.Count > 0)
                this.dgvTrxDetails.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dgvTrxDetails.Columns["WaiversSigned"].DefaultCellStyle.Font = new Font(dgvTrxDetails.DefaultCellStyle.Font,FontStyle.Underline);
            dgvTrxDetails.Columns["WaiversSigned"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            
            log.LogMethodExit(null);
        }

        private void DgvTrxDetails_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex != -1 &&
                dgvTrxDetails.Columns[e.ColumnIndex].DataPropertyName == "WaiversSigned")
            {
                dgvTrxDetails.Cursor = Cursors.Hand;
            }
            else
            {
                dgvTrxDetails.Cursor = Cursors.Default;
            }
            log.LogMethodExit(null);
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            trxId = txtTrxId.Text == string.Empty ? -1 : Convert.ToInt32(txtTrxId.Text);
            customerName = txtCustomerName.Text;
            phoneNumber1 = txtPhoneNumber.Text;
            phoneNumber2 = txtPhoneNumber.Text;
            fromDate = dtpFrom.Value.Date;
            toDate = dtpTo.Value;
            cardNumber = txtCardNumber.Text;
            customAttributeId = Convert.ToInt32(cmbCustomFields.SelectedValue);
            customAttributeValue = txtValue.Text;
            RefreshLines();
            if(!DataFound)
                MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, "No transaction with the entered details"));
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit(null);
        }

        AlphaNumericKeyPad keypad;
        private void BtnShowKeyPad_Click(object sender, EventArgs e)
        {
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, keypad == null ? currentTextBox : keypad.currentTextBox);
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Bottom - keypad.Height + 40);
                keypad.Show();
            }
            else if (keypad.Visible)
            {
                keypad.Hide();
            }
            else
            {
                keypad.Show();
            }
        }

        private void TxtTrxId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtTrxId;
            log.LogMethodExit(null);
        }

        private void TxtCustomerName_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtCustomerName;
            log.LogMethodExit(null);
        }

        private void TxtPhoneNumber_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtPhoneNumber;
            log.LogMethodExit(null);
        }

        private void TxtCardNumber_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtCardNumber;
            log.LogMethodExit(null);
        }

        private void TxtValue_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.currentTextBox = txtValue;
            log.LogMethodExit(null);
        }

        private void TransactionWaiverList_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.Devices.UnregisterCardReaders();
            if (Common.Devices.PrimaryBarcodeScanner != null)
                Common.Devices.PrimaryBarcodeScanner.UnRegister();
        }

        private void txtPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvTrxDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvTrxDetails.Columns[e.ColumnIndex].DataPropertyName == "WaiversSigned")
            {
                if (dgvTrxDetails.CurrentRow != null && dgvTrxDetails.CurrentRow.Cells["TrxId"].Value != null)
                {
                    if (dgvTrxDetails.CurrentRow != null)
                    {
                        int TrxId = Convert.ToInt32(dgvTrxDetails.CurrentRow.Cells["TrxId"].Value);
                        int lineId = Convert.ToInt32(dgvTrxDetails.CurrentRow.Cells["LineId"].Value);
                        int waiverCount = Convert.ToInt32(dgvTrxDetails.CurrentRow.Cells["WaiversSigned"].Value);
                        if (waiverCount > 0)
                        {
                            using (WaiverSignedUI waiverSignedUI = new WaiverSignedUI(TrxId, lineId, Utilities))
                            {
                                waiverSignedUI.ShowDialog();
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(null);
        }
    }
}
