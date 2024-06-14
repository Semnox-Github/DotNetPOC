/********************************************************************************************
* Project Name - Parafait_Kiosk -frmKioskTransactionView.cs
* Description  - frmKioskTransactionView.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.120.0   16-Apr-2021      Guru S A            Wristband print process changes
 *2.130.0    30-Jun-2021      Dakshakh             Theme changes to support customized Font ForeColor
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms; 
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.POS; 
using Semnox.Parafait.Communication; 
using System.Linq;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmKioskTransactionView : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities = KioskStatic.Utilities;
        int scrollIndex = 0;
        private Label currentTextBox;
        private List<POSMachineDTO> posMachineDTOList;
        private int businessStartHour = 6;
        private DateTime currentFromDate;
        private DateTime currentToDate;
        internal class TransactionData
        {
            private DateTime trxDate;
            private int trxId;
            private string posName;
            internal TransactionData(DateTime trxDateValue, int id, string posName)
            {
                trxId = id;
                this.posName = posName;
                trxDate = trxDateValue;
            }
            internal int TrxId { get { return trxId; } set { trxId = value; } }
            internal string POSName { get { return posName; } set { posName = value; } }
            internal DateTime TrxDate { get { return trxDate; } set { trxDate = value; } }
        }
        public frmKioskTransactionView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            businessStartHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 6);
            utilities.setLanguage(this);
            float dgvHeaderFontSize = this.dgvKioskTransactions.ColumnHeadersDefaultCellStyle.Font.Size;
            KioskStatic.setDefaultFont(this);
            try
            {
                 this.BackgroundImage = KioskStatic.CurrentTheme.DefaultBackgroundImage;// ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
                                                                                        // panelKioskTransaction.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                                                                                        //btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                                                                                        //SetTextBoxFontColors();
                SetCustomizedFontColors();
                this.dgvKioskTransactions.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvKioskTransactions.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvHeaderFontSize);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            LoadPOSMachineList();
            ClearDGV();
            SetDefaultSearchParameters();
            if (string.IsNullOrEmpty(KioskStatic.showActivityDuration) || KioskStatic.showActivityDuration.Equals("0"))//checking for the value if it is zero then setting 15 as default value.
            {
                KioskStatic.showActivityDuration = "15";
            }
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            lblGreeting.Text = KioskStatic.Utilities.MessageUtils.getMessage(lblGreeting.Text);
            txtMessage.Text = lblGreeting.Text;
            log.LogMethodExit();
        } 
        private void LoadPOSMachineList()
        {
            log.LogMethodEntry();
            POSMachineList posMachineList = new POSMachineList(utilities.ExecutionContext);
            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParams = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            posMachineDTOList = posMachineList.GetPOSMachinesDTOList(searchParams);
            if (posMachineDTOList == null)
            {
                posMachineDTOList = new List<POSMachineDTO>();
            }
            POSMachineDTO posMachineDTO = new POSMachineDTO()
            {
                POSName = "ALL"
            };
            posMachineDTOList.Insert(0, posMachineDTO);
            cmbPosMachines.DataSource = posMachineDTOList;
            cmbPosMachines.DisplayMember = "POSName";
            cmbPosMachines.ValueMember = "POSMachineId";
            log.LogMethodExit();
        } 
        private void ClearDGV()
        {
            log.LogMethodEntry();
            dgvKioskTransactions.DataSource = null;
            log.LogMethodExit();
        }
        
        private void KioskTransactionView_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblSiteName.Text = KioskStatic.SiteHeading;
                
                LoadTransactionDetails();
                this.Invalidate();
                this.Refresh();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error loading KioskTransactionView: " + ex.Message);
            }
            finally
            {
                KioskTimerSwitch(false);
            }
            log.LogMethodExit();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                SetDefaultSearchParameters();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while clicking clear button: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetDefaultSearchParameters()
        {
            log.LogMethodEntry();
            SetFromDateComponents();
            SetToDateComponents();
            cmbPosMachines.SelectedValue = GetThisPOSMachineId();
            txtTrxId.Text = string.Empty;
            log.LogMethodExit();
        } 
        private void SetFromDateComponents()
        {
            log.LogMethodEntry();
            DateTime currentTime = utilities.getServerTime(); //Convert.ToDateTime("20-Apr-2021 01:00 AM");// 
            int fromHrs = businessStartHour;
            string fromTimeTT = "AM";
            if (currentTime.Hour < businessStartHour)
            {
                currentFromDate = currentTime.Date.AddDays(-1);
            }
            else
            {
                currentFromDate = currentTime.Date;
                DateTime fromDate = currentTime.AddHours(-2);
                fromHrs = fromDate.Hour;
                fromTimeTT = (fromDate.Hour != 12 ? fromDate.ToString("tt", System.Globalization.CultureInfo.GetCultureInfo("en")) : "AM");
            }
            int fromMins = 0;
            txtFromTimeHrs.Text = GetHoursInTTFormat(fromHrs);
            txtFromTimeMins.Text = String.Format("{0,2:00}", fromMins);
            cmbFromTimeTT.SelectedItem = fromTimeTT;
            log.LogMethodExit();
        } 
        private void SetToDateComponents()
        {
            log.LogMethodEntry();
            DateTime currentTime = utilities.getServerTime();//Convert.ToDateTime("20-Apr-2021 01:00 AM");
            int toHrs = businessStartHour;
            string toTimeTT = "AM";
            int toMins = 0;
            currentToDate = currentTime.Date;
            if ((currentTime.Hour < businessStartHour) == false)
            {
                currentToDate = currentTime.Date;
                DateTime toDate = currentTime.AddHours(1);
                toHrs = toDate.Hour;
                toTimeTT = toDate.ToString("tt", System.Globalization.CultureInfo.GetCultureInfo("en"));
                toMins = 30;
            }
            txtToTimeHrs.Text = GetHoursInTTFormat(toHrs);
            txtToTimeMins.Text = String.Format("{0,2:00}", toMins);
            cmbToTimeTT.SelectedItem = toTimeTT;
            log.LogMethodExit();
        } 
        private int GetThisPOSMachineId()
        {
            log.LogMethodEntry();
            int posMachineId = -1;
            try
            {
                if (posMachineDTOList != null)
                {
                    if (posMachineDTOList.Count == 1)
                    {
                        posMachineId = posMachineDTOList[0].POSMachineId;
                    }
                    else
                    {
                        for (int i = 0; i < posMachineDTOList.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(posMachineDTOList[i].ComputerName) == false
                                && posMachineDTOList[i].ComputerName == Environment.MachineName)
                            {
                                posMachineId = posMachineDTOList[i].POSMachineId;
                                break;
                            }
                            else if (string.IsNullOrWhiteSpace(posMachineDTOList[i].POSName) == false
                                && posMachineDTOList[i].POSName == Environment.MachineName)
                            {
                                posMachineId = posMachineDTOList[i].POSMachineId;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(posMachineId);
            return posMachineId;
        }
        private string GetHoursInTTFormat(int fromHrs)
        {
            log.LogMethodEntry(fromHrs);
            int finalValue = -1;
            if (fromHrs < 13)
            {
                finalValue = fromHrs;
            }
            else
            {
                finalValue = 24 - (24 - fromHrs) - 12;
            }
            string finalStringValue = String.Format("{0,2:00}", finalValue);
            log.LogMethodExit(finalStringValue);
            return finalStringValue;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                LoadTransactionDetails();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
                using (frmOKMsg fok = new frmOKMsg(ex.Message))
                {
                    fok.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
        private void LoadTransactionDetails()
        {
            log.LogMethodEntry();
            DateTime fromDateTime = GetFromDateTime();
            DateTime toDateTime = GetToDateTime();
            if (fromDateTime > toDateTime)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 971));
                //To Date should be greater than from date.
            }
            if ((toDateTime - fromDateTime).Days > 2)
            {  
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 3001, 2));
                //Date range for search should not be more than &1 days
            }
            int trxId = GetTrxId();
            int posMachineId = GetPOSMachineId();
            FetchTrxDetails(fromDateTime, toDateTime, trxId, posMachineId);
            log.LogMethodExit();
        }
        
        private DateTime GetFromDateTime()
        {
            log.LogMethodEntry();
            DateTime fromTime = DateTime.MinValue;
            DateTime dateComponent = currentFromDate.Date;
            string ttComponent = GetTTComponent(cmbFromTimeTT.SelectedItem, "From Time AM/PM");
            int hrsComponent = GetHrComponent(txtFromTimeHrs.Text, ttComponent, "From Time Hours");
            int minsComponent = GetMinsComponent(txtFromTimeMins.Text, "From Time Minutes");
            fromTime = BuildDateTimeValue(dateComponent, hrsComponent, minsComponent, ttComponent);
            log.LogMethodExit(fromTime);
            return fromTime;
        }
        private DateTime GetToDateTime()
        {
            log.LogMethodEntry();
            DateTime toTime = DateTime.MinValue;
            DateTime dateComponent = currentToDate.Date;
            string ttComponent = GetTTComponent(cmbToTimeTT.SelectedItem, "To Time AM/PM");
            int hrsComponent = GetHrComponent(txtToTimeHrs.Text, ttComponent, "To Time Hours");
            int minsComponent = GetMinsComponent(txtToTimeMins.Text, "To Time Minutes");
            toTime = BuildDateTimeValue(dateComponent, hrsComponent, minsComponent, ttComponent);
            log.LogMethodExit(toTime);
            return toTime;
        }
        private int GetTrxId()
        {
            log.LogMethodEntry();
            int trxId = -1;
            if (string.IsNullOrWhiteSpace(txtTrxId.Text) == false && int.TryParse(txtTrxId.Text, out trxId) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                 MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction Id")));
                //Please enter valid value for &1 
            }
            log.LogMethodExit(trxId);
            return trxId;
        }
        private int GetPOSMachineId()
        {
            log.LogMethodEntry();
            int posMachineId = -1;
            if (cmbPosMachines.SelectedValue == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                 MessageContainerList.GetMessage(utilities.ExecutionContext, "POS Machine")));
                //Please enter valid value for &1 
            }
            else
            {
                posMachineId = Convert.ToInt32(cmbPosMachines.SelectedValue);
            }
            log.LogMethodExit(posMachineId);
            return posMachineId;
        }

        private int GetHrComponent(string text, string ttFormat, string labelText)
        {
            log.LogMethodEntry();
            int hrComponent = 0;
            if (int.TryParse(text, out hrComponent) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                 MessageContainerList.GetMessage(utilities.ExecutionContext, labelText)));
                //Please enter valid value for &1
            }
            if (ttFormat == "PM")
            {
                hrComponent = 12 + hrComponent;
            }
            log.LogMethodExit(hrComponent);
            return hrComponent;
        }
        private int GetMinsComponent(string text, string labelText)
        {
            log.LogMethodEntry();
            int minsComponent = 0;
            if (int.TryParse(text, out minsComponent) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                 MessageContainerList.GetMessage(utilities.ExecutionContext, labelText)));
                //Please enter valid value for &1
            }
            if (minsComponent > 60 || minsComponent < 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                 MessageContainerList.GetMessage(utilities.ExecutionContext, labelText)));
                //Please enter valid value for &1
            }
            log.LogMethodExit(minsComponent);
            return minsComponent;
        }
        private string GetTTComponent(object selectedValue, string labelText)
        {
            log.LogMethodEntry();
            string ttComponent = string.Empty;
            if (selectedValue != null && string.IsNullOrWhiteSpace(selectedValue.ToString()) == false)
            {
                ttComponent = selectedValue.ToString();
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                 MessageContainerList.GetMessage(utilities.ExecutionContext, labelText)));
                //Please enter valid value for &1
            }
            log.LogMethodExit(ttComponent);
            return ttComponent;
        }
        private static DateTime BuildDateTimeValue(DateTime dateComponent, int hrsComponent, int minsComponent, string ttComponent)
        {
            log.LogMethodEntry();
            DateTime dateTimeValue;
            dateTimeValue = dateComponent.AddHours(hrsComponent).AddMinutes(minsComponent); 
            log.LogMethodExit(dateTimeValue);
            return dateTimeValue;
        }
        private void FetchTrxDetails(DateTime fromDateTime, DateTime toDateTime, int trxId, int posMachineId)
        {
            log.LogMethodEntry(fromDateTime, toDateTime, trxId, posMachineId);
            try
            {
                List<TransactionData> transactionDataList = new List<TransactionData>();
                try
                {
                    TransactionListBL transactionListBL = new TransactionListBL(utilities.ExecutionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParm = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();

                    if (trxId > 0)
                    {
                        searchParm.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, trxId.ToString()));
                    }
                    else
                    {
                        searchParm.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, fromDateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                        searchParm.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, toDateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                        if (posMachineId > -1)
                        {
                            searchParm.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));
                        }
                    }
                    List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchParm, utilities, null, 0, 1000);
                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        transactionDTOList = transactionDTOList.OrderByDescending(th => th.TransactionDate).ToList();
                        for (int i = 0; i < transactionDTOList.Count; i++)
                        {
                            TransactionData transactionData = new TransactionData(transactionDTOList[i].TransactionDate,
                                                                                  transactionDTOList[i].TransactionId,
                                                                                  transactionDTOList[i].PosMachine);
                            transactionDataList.Add(transactionData);
                        }
                        txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 3014);
                        // "Select a record to view details"
                    }
                    else
                    { 
                        txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 3015);
                        // "Search returned zero records"
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                } 
                dgvKioskTransactions.ReadOnly = false;
                dgvKioskTransactions.Rows.Clear();
                for (int i = 0; i < transactionDataList.Count; i++)
                {
                    int rowId = dgvKioskTransactions.Rows.Add();
                    DataGridViewRow row = dgvKioskTransactions.Rows[rowId];
                    row.Cells["TransactionDate"].Value = transactionDataList[i].TrxDate;
                    row.Cells["TransactionId"].Value = transactionDataList[i].TrxId;
                    row.Cells["POSName"].Value = transactionDataList[i].POSName;
                }
                dgvKioskTransactions.EndEdit(); 
                dgvKioskTransactions.Columns["TransactionId"].DefaultCellStyle = utilities.gridViewNumericCellStyle(); 
                dgvKioskTransactions.Columns["TransactionDate"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle(); 
                if (dgvKioskTransactions.SelectedCells.Count > 0)
                    dgvKioskTransactions.SelectedCells[0].Selected = false;
                vScrollBarGp.Maximum = dgvKioskTransactions.Rows.Count + 1;
                hScroll.Maximum = dgvKioskTransactions.Columns.Count + 5;
                if (dgvKioskTransactions.Rows.Count <= 10)
                {
                    buttonNext.Enabled = false;
                }
                else
                {
                    buttonNext.Enabled = true;
                }
                buttonPrev.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                dgvKioskTransactions.ReadOnly = true;
            }
            log.LogMethodExit();
        }
        void ShowKeyPad()
        {
            log.LogMethodEntry();
            if (currentTextBox != null)
            {
                ResetKioskTimer();
                string keyPadValue = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm("", currentTextBox.Text, utilities).ToString();
                if (keyPadValue != "-1" && string.IsNullOrWhiteSpace(keyPadValue) == false)
                {
                    if (currentTextBox.Name == "txtTrxId") //for trxId print as is
                    {
                        currentTextBox.Text = keyPadValue;
                    }
                    else
                    {
                        //for hr/time components pad zero to make it 2 digit
                        currentTextBox.Text = String.Format("{0,2:00}", Convert.ToInt32(keyPadValue));
                    }
                }
            }
            log.LogMethodExit();
        }
        private void txtFromTimeHrs_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                currentTextBox = this.txtFromTimeHrs;
                ShowKeyPad();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void txtFromTimeHrs_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (string.IsNullOrWhiteSpace(txtFromTimeHrs.Text) == false)
                {
                    int hrsComponent = GetHrComponent(txtFromTimeHrs.Text, "AM", "From Time Hours");
                    if (hrsComponent > 12 || hrsComponent < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                MessageContainerList.GetMessage(utilities.ExecutionContext, "From Time Hours")));
                        //Please enter valid value for &1
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void txtToTimeHrs_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                currentTextBox = this.txtToTimeHrs;
                ShowKeyPad();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
            }
        }
        private void txtToTimeHrs_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (string.IsNullOrWhiteSpace(txtToTimeHrs.Text) == false)
                {
                    int hrsComponent = GetHrComponent(txtToTimeHrs.Text, "AM", "To Time Hours");
                    if (hrsComponent > 12 || hrsComponent < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                MessageContainerList.GetMessage(utilities.ExecutionContext, "To Time Hours")));
                        //Please enter valid value for &1
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void txtFromTimeMins_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                currentTextBox = this.txtFromTimeMins;
                ShowKeyPad();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void txtFromTimeMins_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (string.IsNullOrWhiteSpace(txtFromTimeMins.Text) == false)
                {
                    int minsComponent = GetMinsComponent(txtFromTimeMins.Text, "From Time Minutes");
                    if (minsComponent > 60 || minsComponent < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                MessageContainerList.GetMessage(utilities.ExecutionContext, "From Time Minutes")));
                        //Please enter valid value for &1
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
                txtFromTimeMins.Text = "00";
            }
            log.LogMethodExit();
        }
        private void txtToTimeMins_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                currentTextBox = this.txtToTimeMins;
                ShowKeyPad();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void txtToTimeMins_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (string.IsNullOrWhiteSpace(txtToTimeMins.Text) == false)
                {
                    int minsComponent = GetMinsComponent(txtToTimeMins.Text, "To Time Minutes");
                    if (minsComponent > 60 || minsComponent < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144,
                                                MessageContainerList.GetMessage(utilities.ExecutionContext, "To Time Minutes")));
                        //Please enter valid value for &1
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
                txtToTimeMins.Text = "00";
            }
            log.LogMethodExit();

        }
        private void txtTrxId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                currentTextBox = this.txtTrxId;
                ShowKeyPad();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if (currentTextBox == null)
                {
                    currentTextBox = this.txtTrxId;
                }
                ShowKeyPad();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }
        private void KioskActivityDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if ((Keys)e.KeyChar == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void vScrollBarGp_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if (dgvKioskTransactions.Rows.Count > 0)
                    dgvKioskTransactions.FirstDisplayedScrollingRowIndex = e.NewValue;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void hScroll_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if (dgvKioskTransactions.Columns.Count > 0)
                {
                    dgvKioskTransactions.FirstDisplayedScrollingColumnIndex = e.NewValue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void buttonNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                btnSearch.Focus();
                scrollIndex += 10;
                buttonPrev.Enabled = true;
                dgvKioskTransactions.FirstDisplayedScrollingRowIndex = scrollIndex;
                if (scrollIndex + 10 >= dgvKioskTransactions.RowCount)
                    buttonNext.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void buttonPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                btnSearch.Focus();
                scrollIndex -= 10;
                if (scrollIndex >= 10 || (scrollIndex == 0 && dgvKioskTransactions.RowCount > 10))
                    buttonNext.Enabled = true;
                dgvKioskTransactions.FirstDisplayedScrollingRowIndex = scrollIndex;
                if (scrollIndex <= 0)
                    buttonPrev.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void dgvKioskTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer(); 
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 3014);
                //"Select a record to view details"
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    //if (dgvKioskTransactions.Columns[e.ColumnIndex].Name == "btnPrintTrx")
                    {
                        int trxId = Convert.ToInt32(dgvKioskTransactions.Rows[e.RowIndex].Cells["TransactionId"].Value);
                        using (frmPrintTransactionLines fptl = new frmPrintTransactionLines(utilities.ExecutionContext, trxId))
                        {
                            fptl.ShowDialog();
                        }
                        dgvKioskTransactions.ClearSelection();
                        btnSearch.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while clicking on dgvKioskTransactions cell: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewLblGreetingTextForeColor;//How many points or minutes per card label
                this.lblFromDate.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewLblFromDateTextForeColor;//Back button
                this.txtFromTimeHrs.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewTxtFromTimeHrsTextForeColor;//Back button
                this.txtFromTimeMins.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewTxtFromTimeMinsTextForeColor;//Back button
                this.cmbFromTimeTT.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewCmbFromTimeTTTextForeColor;//Back button
                this.label1.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewLabel1TextForeColor;//Cancel button
                this.txtToTimeHrs.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewTxtToTimeHrsTextForeColor;//Variable button
                this.txtToTimeMins.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewTxtToTimeMinsTextForeColor;//Footer text message
                this.cmbToTimeTT.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewCmbToTimeTTTextForeColor;//Footer text message
                this.lblPosMachines.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewLblPosMachinesTextForeColor;//Footer text message
                this.cmbPosMachines.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewCmbPosMachinesTextForeColor;//Footer text message
                this.lblTrxId.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewLblTrxIdTextForeColor;//Footer text message
                this.txtTrxId.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewTxtTrxIdTextForeColor;//Footer text message
                this.btnSearch.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewBtnSearchTextForeColor;//Footer text message
                this.btnClear.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewBtnClearTextForeColor;//Footer text message
                this.lblTransaction.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewLblTransactionTextForeColor;//Footer text message
                this.dgvKioskTransactions.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewDgvKioskTransactionsTextForeColor;//Footer text message
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewTextBtnPrevForeColor;//Footer text message
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransViewTextTxtMessageForeColor;//Footer text message
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.FrmKioskTransactionViewSiteTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
