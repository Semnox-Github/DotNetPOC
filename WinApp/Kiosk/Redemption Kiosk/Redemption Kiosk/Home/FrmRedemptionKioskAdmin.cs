/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Redemption kiosk admin screen
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       12-Sep-2018      Archana            Created
 *2.70        01-Jul-2019      Lakshminarayana    Modified to add support for ULC cards 
 *2.90        12-Aug-2020      Guru SA            QA issue fix changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Transaction;

namespace Redemption_Kiosk
{
    public partial class FrmRedemptionKioskAdmin : frmRedemptionKioskBaseForm
    {
        ExecutionContext machineUserContext;
        DeviceClass adminScreenCardReaderDevice = null;
        DeviceClass admincreenBarcodeScannerDevice = null;
        private AlphaNumericKeyPad keypad;
        private TextBox currentActiveTextBox;
        private readonly TagNumberParser tagNumberParser;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="execustionContext"></param>
        public FrmRedemptionKioskAdmin(ExecutionContext execustionContext)
        {
            log.LogMethodEntry(execustionContext);
            machineUserContext = execustionContext;
            InitializeComponent();
            Common.utils.setLanguage(this);
            tagNumberParser = new TagNumberParser(Common.utils.ExecutionContext);
            log.LogMethodExit();
        }
        private void frmRedemptionKioskAdmin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RegisterDevices();
            LoadOrderStatusList();
            ClearSearchParameters();
            GetRedemptionDetails();
            log.LogMethodExit();
        }       

        private void ClearSearchParameters()
        {
            log.LogMethodEntry();
            ResetTimeOut();
            dtRedemptionFromDate.Value = ServerDateTime.Now.Date;
            dtRedemptionToDate.Value = ServerDateTime.Now.Date.AddDays(1);
            txtProdCode.Text = txtRedemptionOrderNo.Text = txtCardNumber.Text = string.Empty;
            cbxRedemptionStatus.SelectedIndex = 0;
            log.LogMethodExit();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetTimeOut();
                GetRedemptionDetails();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message);
            }
            log.LogMethodExit();
        }
        private void btnKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //InactivityTimerSwitch(false);
                ResetTimeOut();
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, currentActiveTextBox);
                    keypad.Location = new Point((this.Width - keypad.Width) / 2, pnlBottom.Bottom + 50);
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
            catch (Exception ex)
            {
                log.Error(ex);
            }
            //InactivityTimerSwitch(true);
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (keypad != null)
                {
                    keypad.Hide();
                }
                this.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex); 
            }
            log.LogMethodExit();
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (keypad != null)
                {
                    keypad.Hide();
                }
                if (Common.ShowDialog(MessageContainerList.GetMessage(machineUserContext, 1662)) == System.Windows.Forms.DialogResult.Yes)
                {
                    log.LogMethodExit();
                    System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 00");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex); 
            }

        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvRedemptionOrders.CurrentRow != null)
                {
                    int redemptionReceiptTemplate = -1;
                    try
                    {
                        redemptionReceiptTemplate = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "REDEMPTION_RECEIPT_TEMPLATE"));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        redemptionReceiptTemplate = -1;
                    }
                    int redemptionId = Convert.ToInt32(dgvRedemptionOrders.CurrentRow.Cells["redemptionIdDataGridViewTextBoxColumn"].Value);
                    RedemptionBL redemptionbl = new RedemptionBL(redemptionId, machineUserContext, null, true);
                    PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(Common.utils.ExecutionContext, Common.utils);
                    printRedemptionReceipt.PrintRedemption(redemptionbl, redemptionReceiptTemplate);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message);
            }
            log.LogMethodExit();
        }
        private void dgvRedemptionOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvRedemptionOrders.CurrentRow != null && dgvRedemptionOrders.Columns[e.ColumnIndex].Name == "viewDetailsDataGridViewButtonColumn")
            {
                try
                {
                    UnRegisterDevices();
                    if (keypad != null)
                    {
                        keypad.Hide();
                    }
                    using (FrmRedemptionKioskRedemptionDetails frmRedemptionDetails = new FrmRedemptionKioskRedemptionDetails(machineUserContext, Convert.ToInt32(dgvRedemptionOrders.CurrentRow.Cells["redemptionIdDataGridViewTextBoxColumn"].Value)))
                    {
                        frmRedemptionDetails.ShowDialog();
                        frmRedemptionDetails.BringToFront();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Common.ShowMessage(ex.Message);
                }
                finally
                {
                    RegisterDevices();
                }
            }
            log.LogMethodExit();
        }
        private void GetRedemptionDetails()
        {
            log.LogMethodEntry();
            SortableBindingList<RedemptionDTO> redemptionOrdersSortableList;
            RedemptionListBL redemptionBL = new RedemptionListBL();
            List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();

            if (String.IsNullOrEmpty(txtRedemptionOrderNo.Text) && String.IsNullOrEmpty(txtCardNumber.Text))
            {
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE, dtRedemptionFromDate.Value.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE, dtRedemptionToDate.Value.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                if (cbxRedemptionStatus != null && cbxRedemptionStatus.SelectedIndex > 0)
                {
                    searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_STATUS, cbxRedemptionStatus.SelectedValue.ToString()));
                }
            }

            if (!String.IsNullOrEmpty(txtRedemptionOrderNo.Text))
            {
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO, txtRedemptionOrderNo.Text));
            }

            if (!String.IsNullOrEmpty(txtCardNumber.Text))
            {
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.PRIMARY_CARD, txtCardNumber.Text));
            }

            if (!String.IsNullOrEmpty(txtProdCode.Text))
            {
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.GIFT_CODE_DESC_BARCODE, txtProdCode.Text));
            }
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Convert.ToString(machineUserContext.GetSiteId())));
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.POS_MACHINE_ID, Convert.ToString(machineUserContext.GetMachineId())));

            List<RedemptionDTO> redemptionDTOList = redemptionBL.GetRedemptionDTOList(searchParameters);
            //if (redemptionDTOList != null && redemptionDTOList.Count > 0)

            //List<RedemptionDTO> newredemptionDTOList = new List<RedemptionDTO>();
            //if (redemptionDTOList.Count > 10)
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        newredemptionDTOList.Add(redemptionDTOList[i]);
            //    }
            //}
            //else
            //{
            //    newredemptionDTOList = redemptionDTOList;
            //}
            if (redemptionDTOList == null)
            {
                redemptionDTOList = new List<RedemptionDTO>();
            }
            redemptionOrdersSortableList = new SortableBindingList<RedemptionDTO>(redemptionDTOList);
            //redemptionDTOBindingSource.Clear();
            try
            {
                redemptionDTOBindingSource.DataSource = redemptionOrdersSortableList;
                SetDgvRedemptionOrdersHeaderFont();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            dgvRedemptionOrders.Refresh();
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //InactivityTimerSwitch(false);
                ResetTimeOut();
                //if (keypad != null)
                //    keypad.currentTextBox = sender as TextBox;
                currentActiveTextBox = sender as TextBox;
                if (keypad != null)
                {
                    keypad.currentTextBox = currentActiveTextBox;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void RegisterDevices()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "REDEMPTION_KIOSK_DEVICE"));

            List<LookupValuesDTO> redemptionDeviceValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (redemptionDeviceValueList != null && redemptionDeviceValueList[0].LookupValue == "DeviceToEnable")
            {
                if (redemptionDeviceValueList[0].Description == "CARD")
                {
                    adminScreenCardReaderDevice = RedemptionKioskHelper.RegisterCardReader(this, CardScanCompleteEventHandle);

                }
                else if (redemptionDeviceValueList[0].Description == "BARCODE")
                {
                    admincreenBarcodeScannerDevice = RedemptionKioskHelper.RegisterBarCodeScanner(this, BarCodeScanCompleteEventHandle);

                }
                else if (redemptionDeviceValueList[0].Description == "BOTH")
                {
                    adminScreenCardReaderDevice = RedemptionKioskHelper.RegisterCardReader(this, CardScanCompleteEventHandle);
                    admincreenBarcodeScannerDevice = RedemptionKioskHelper.RegisterBarCodeScanner(this, BarCodeScanCompleteEventHandle);
                }
                else
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1620, "REDEMPTION_KIOSK_DEVICE" + Common.utils.MessageUtils.getMessage(441))); //"Lookup value is not defined for REDEMPTION_KIOSK_DEVICE" + Common.utils.MessageUtils.getMessage(441)));
                }
            }
            log.LogMethodExit();
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
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    Common.ShowMessage(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                CardNumber = RedemptionKioskHelper.ReverseTopupCardNumber(CardNumber);
                try
                {
                    ResetTimeOut();
                    HandleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string scannedBarcode = Common.utils.ProcessScannedBarCode(checkScannedEvent.Message, Common.utils.ParafaitEnv.LEFT_TRIM_BARCODE, Common.utils.ParafaitEnv.RIGHT_TRIM_BARCODE);
                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        ResetTimeOut();
                        HandleBarcodeRead(scannedBarcode);
                    });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void HandleCardRead(string cardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(cardNumber, readerDevice);
            Card card = null;
            try
            {
                try
                {
                    card = RedemptionKioskHelper.HandleCardRead(cardNumber, readerDevice);
                    cbxRedemptionStatus.SelectedIndex = 0;
                    txtCardNumber.Text = cardNumber;
                    GetRedemptionDetails();

                }
                catch (ValidationException ex)
                {
                    log.Info(ex.Message);
                    Common.ShowMessage(MessageContainerList.GetMessage(machineUserContext, 1663));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void HandleBarcodeRead(string scannedBarcode)
        {
            log.LogMethodEntry(scannedBarcode);
            if (scannedBarcode != "")
            {
                txtCardNumber.Text = txtRedemptionOrderNo.Text = string.Empty;
                cbxRedemptionStatus.SelectedIndex = 0;
                txtProdCode.Text = scannedBarcode;
                GetRedemptionDetails();
            }
            log.LogMethodExit();
        }

        private void FrmRedemptionKioskAdmin_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            } 
            log.LogMethodExit();
        }

        private void FrmRedemptionKioskAdmin_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            // UnRegisterDevices();
            log.LogMethodExit();
        }

        private void dgvRedemptionOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (dgvRedemptionOrders.Columns[e.ColumnIndex].DataPropertyName == "RedemptionId")
            //{
            //    dgvRedemptionOrders.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Regular);
            //}
            log.LogMethodExit();
        }

        private void FrmRedemptionKioskAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                UnRegisterDevices();
                if (keypad != null)
                {
                    keypad.Close();
                    keypad.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void UnRegisterDevices()
        {
            log.LogMethodEntry();
            if (adminScreenCardReaderDevice != null)
            {
                adminScreenCardReaderDevice.UnRegister();
                adminScreenCardReaderDevice.Dispose();
            }

            if (admincreenBarcodeScannerDevice != null)
            {
                admincreenBarcodeScannerDevice.UnRegister();
                admincreenBarcodeScannerDevice.Dispose();
            }
            log.LogMethodExit();
        }

        private void SetDgvRedemptionOrdersHeaderFont()
        {
            log.LogMethodEntry();
            dgvRedemptionOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Bold);
            dgvRedemptionOrders.DefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Regular);
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (keypad != null)
                    keypad.Hide();
                if (Common.ShowDialog(Common.utils.MessageUtils.getMessage(1716)) == System.Windows.Forms.DialogResult.Yes)
                {
                    ClearOrderProcess();
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                ClearSearchParameters();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
	private void dtRedemptionFromDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (keypad != null)
                {
                    keypad.Hide();
                }
                System.Windows.Forms.SendKeys.Send("%{DOWN}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void FrmRedemptionKioskAdmin_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void textBox_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dtRedemptionFromDate_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dtRedemptionFromDate_KeyPress(object sender, KeyPressEventArgs e)
        {

            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dtRedemptionFromDate_MouseDown(object sender, MouseEventArgs e)
        {

            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dtRedemptionFromDate_ValueChanged(object sender, EventArgs e)
        {

            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void LoadOrderStatusList()
        {
            log.LogMethodEntry();
            List<string> orderStatusList = new List<string>();
            orderStatusList.Add("ALL");
            orderStatusList.Add(RedemptionDTO.RedemptionStatusEnum.OPEN.ToString());
            orderStatusList.Add(RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString());
            orderStatusList.Add(RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString());
            cbxRedemptionStatus.DataSource = orderStatusList;
            cbxRedemptionStatus.SelectedIndex = 0;
            log.LogMethodExit();
        }
    }
}
