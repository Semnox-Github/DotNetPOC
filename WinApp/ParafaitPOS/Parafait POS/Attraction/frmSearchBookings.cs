/* Project Name - frmSearchBookings
* Description  - frmSearchBookings form
*
**************
** Version Log
 **************
 * Version     Date          Modified By     Remarks
*********************************************************************************************
*2.80.0        20-Aug-2019    Girish Kundar  Modified :  Added logger methods and Removed unused namespace's
*2.110       24-Jan-2022      Girish Kundar  Modified : Do not consider the dates by default while searching attranction booking details
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Attraction
{
    public partial class frmSearchBookings : Form
    {
        private readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TagNumberParser tagNumberParser;
        private Utilities Utilities;
        private Card swipedCard;
        private CustomerDTO customerDTO;
        private FacilityMapDTO facilityMapDTO;
        private int trxId;
        private int attractionScheduleId;
        private string fromDate;
        private string toDate;
        private bool autoKeyboard = false;
        private AlphaNumericKeyPad keypad;
        private bool isDateTimeValueChanged;
        public Card SwipedCard { get { return swipedCard; } }
        public CustomerDTO CustomerDTO { get { return customerDTO; } }
        public FacilityMapDTO FacilityMapDTO { get { return facilityMapDTO; } }
        public int TrxId { get { return trxId; } }
        public string FromDate { get { return fromDate; } }
        public string ToDate { get { return toDate; } }
        public int AttractionScheduleId { get { return attractionScheduleId; } }
        public bool IsDateTimeValueChanged { get { return isDateTimeValueChanged; } }


        public frmSearchBookings(Utilities Utilities)
        {
            InitializeComponent();
            this.Utilities = Utilities;
            Utilities.setLanguage(this);
            autoKeyboard = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD");
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
        }

        private void frmSearchBookings_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));

            DateTime Now, From, To = Utilities.getServerTime();
            Now = From = To;

            int startHour = Convert.ToInt32(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"));
            if(Now.Hour < startHour)
            {
                To = To.Date.AddHours(startHour);
            }
            else
            {
                To = To.AddDays(1).Date.AddHours(startHour);
            }
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.CustomFormat = "ddd, dd MMM yyy";
            dtpFromDate.ShowUpDown = false;
            dtpFromDate.Value = From;

            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.CustomFormat = "ddd, dd MMM yyy";
            dtpToDate.ShowUpDown = false;
            dtpToDate.Value = To;

            cmbFromHour.SelectedItem = Now.Hour <= 12 ? (Now.Hour < 10 ? "0" : "") + Now.Hour.ToString() : (((Now.Hour - 12) < 10 ? "0" : "") + (Now.Hour-12).ToString()).ToString();
            int min = Now.Minute % 60;
            cmbFromMin.SelectedItem = min < 15 ? "00" : min < 30 ? "15" : min < 45 ? "30" : "45";
            cmbFromAM.SelectedItem = Now.Hour < 12 ? "AM" : "PM";
            cmbToHour.SelectedItem = startHour <= 12 ? (startHour < 10 ? "0" : "") + startHour.ToString() : (startHour - 12).ToString();
            cmbToMin.SelectedItem = "00";
            cmbToAM.SelectedItem = startHour < 12 ? "AM" : "PM";
            currentTextBox = txtCard;
            txtCard.Focus();

            List<FacilityMapDTO> facilityMapDTOList = GetAttractionFacilityMaps();
            cmbFacilityMap.DisplayMember = "FacilityMapName";
            cmbFacilityMap.ValueMember = "facilityMapId";
            cmbFacilityMap.DataSource = facilityMapDTOList;
            cmbFacilityMap.SelectedValue = -1;
            isDateTimeValueChanged = false;
            log.LogMethodExit();
        }

        private void frmSearchBookings_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Common.Devices.UnregisterCardReaders();
            log.LogMethodExit();
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry("Starts-CardScanCompleteEventHandle()");
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                string scannedTagNumber = checkScannedEvent.Message;
                DeviceClass encryptedTagDevice = sender as DeviceClass;
                if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
                {
                    string decryptedTagNumber = string.Empty;
                    try
                    {
                        decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number result: ", ex);
                        displayMessageLine(ex.Message);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    displayMessageLine(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }
                try
                {
                    CardSwiped(tagNumber.Value);
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message);
                    log.Error("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);
                }
            }
            log.LogMethodExit("Ends-CardScanCompleteEventHandle()");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (String.IsNullOrEmpty(txtTrxId.Text))
                trxId = -1;
            else
                int.TryParse(txtTrxId.Text, out trxId);

            if (cmbFromHour.SelectedItem.Equals("12"))
                cmbFromHour.SelectedItem = "00";
            if (cmbToHour.SelectedItem.Equals("12"))
                cmbToHour.SelectedItem = "00";
            if (isDateTimeValueChanged)
            {
                fromDate = dtpFromDate.Value.Date.ToString("yyyy-MM-dd") + " " + (cmbFromAM.SelectedItem.Equals("AM") ? cmbFromHour.SelectedItem : (Convert.ToInt32(cmbFromHour.SelectedItem) + 12).ToString()) + ":" + cmbFromMin.SelectedItem + ":00";
                log.LogVariableState("fromDate:", fromDate);

                toDate = dtpToDate.Value.Date.ToString("yyyy-MM-dd") + " " + (cmbToAM.SelectedItem.Equals("AM") ? cmbToHour.SelectedItem : (Convert.ToInt32(cmbToHour.SelectedItem) + 12).ToString()) + ":" + cmbToMin.SelectedItem + ":00";
                log.LogVariableState("toDate :", toDate);
            }
            attractionScheduleId = -1;
            if (cmbFacilityMapSchedules.SelectedIndex > -1)
                attractionScheduleId = Convert.ToInt32(cmbFacilityMapSchedules.SelectedValue);

            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();

        }

        private void btnOk_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnOk.BackgroundImage = Properties.Resources.normal1;
            log.LogMethodExit();
        }

        private void btnOk_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnOk.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnCancel.BackgroundImage = Properties.Resources.normal1;
            log.LogMethodExit();
        }

        private void btnCancel_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnCancel.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessageLine.Clear();
            try
            {
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities,
                                                                         txtFirstName.Text,
                                                                         "",
                                                                         "",
                                                                         "",
                                                                         "",
                                                                         "");
                customerLookupUI.MinimizeBox = false;
                customerLookupUI.MaximizeBox = false;
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    this.customerDTO = customerLookupUI.SelectedCustomerDTO;
                    txtFirstName.Text = customerDTO.FirstName;
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessageLine.Text = ex.GetAllValidationErrorMessages();
                POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages(), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Customer Lookup"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessageLine.Text = ex.Message;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnCustomerLookup_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnCustomerLookup.BackgroundImage = Properties.Resources.normal1;
            log.LogMethodExit();
        }

        private void btnCustomerLookup_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodExit();
            btnCustomerLookup.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void CardSwiped(string CardNumber)
        {
            log.LogMethodEntry("Starts-CardSwiped(" + CardNumber + ")");
            displayMessageLine("");

            Card inputCard = new Card(CardNumber, POSStatic.Utilities.ParafaitEnv.LoginID, Utilities);
            if (inputCard.technician_card.Equals('Y'))
            {
                displayMessageLine(POSStatic.MessageUtils.getMessage(197, CardNumber));
                log.Info(POSStatic.MessageUtils.getMessage(197, CardNumber));
                return;
            }
            else if (inputCard.CardStatus.Equals("NEW"))
            {
                //displayMessageLine("New Card");
                log.Info("CardSwiped(" + CardNumber + ") Swipped Card is a Issued Card");
            }

            swipedCard = inputCard;
            txtCard.Text = swipedCard.CardNumber;
            log.LogMethodExit();
        }

        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            log.Debug(message);
            txtMessageLine.Text = message;
            log.LogMethodExit();
        }

        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
            customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, txtFirstName.Text)
                                  .OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                if (customerDTOList.Count == 1)
                {
                    log.LogMethodExit("customerDTOList.Count == 1");
                    this.Cursor = Cursors.Default;
                    customerDTO = customerDTOList[0];
                    return;
                }
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities, txtFirstName.Text);
                customerLookupUI.MinimizeBox = false;
                customerLookupUI.MaximizeBox = false;
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    customerDTO = customerLookupUI.SelectedCustomerDTO;
                    txtFirstName.Text = customerDTO.FirstName;
                }
            }
            log.LogMethodExit();
        }

        private List<FacilityMapDTO> GetAttractionFacilityMaps()
        {
            log.LogMethodEntry();
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(Utilities.ExecutionContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, ProductTypeValues.ATTRACTION));
            List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParams);
            if (facilityMapDTOList == null || facilityMapDTOList.Count == 0)
            {
                facilityMapDTOList = new List<FacilityMapDTO>();
            }
            FacilityMapDTO facilityMapDTO = new FacilityMapDTO()
            {
                FacilityMapName = "-All -",
                FacilityMapId = -1
            };

            facilityMapDTOList = facilityMapDTOList.OrderBy(fac => fac.FacilityMapName).ToList();
            facilityMapDTOList.Insert(0, facilityMapDTO);

            log.LogMethodExit();
            return facilityMapDTOList;
        }

        private List<SchedulesDTO> schedulesDTOList = null;
        private void cmbFacilityMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            
            if (cmbFacilityMap != null && cmbFacilityMap.SelectedValue != null)
            {
                this.facilityMapDTO = (FacilityMapDTO)cmbFacilityMap.SelectedItem;
                if(this.facilityMapDTO.FacilityMapId > -1)
                {
                    cmbFacilityMapSchedules.DataSource = null;
                    SchedulesListBL schedulesListBL = new SchedulesListBL(this.Utilities.ExecutionContext);
                    List<KeyValuePair<SchedulesDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<SchedulesDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<SchedulesDTO.SearchByParameters, string>(SchedulesDTO.SearchByParameters.MASTER_SCHEDULE_ID, facilityMapDTO.MasterScheduleId.ToString()));
                    searchByParameters.Add(new KeyValuePair<SchedulesDTO.SearchByParameters, string>(SchedulesDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                    schedulesDTOList = schedulesListBL.GetScheduleDTOList(searchByParameters, true, facilityMapDTO.FacilityMapId);
                    if (schedulesDTOList != null && schedulesDTOList.Any())
                    {
                        schedulesDTOList = schedulesDTOList.OrderBy(x => x.ScheduleTime).ToList();
                        SetSchedulesComboBox();
                    }
                    else
                    {
                        cmbFacilityMapSchedules.Enabled = false;
                    }

                }
                else
                {
                    cmbFacilityMapSchedules.Enabled = false;
                }
            }
            log.LogMethodExit();
        }

        private void SetSchedulesComboBox()
        {
            cmbFacilityMapSchedules.SuspendLayout();
            cmbFacilityMapSchedules.DataSource = null;
            Dictionary<int, string> facilityMapSchedules = new Dictionary<int, string>();
            facilityMapSchedules.Add(-1, "- All - ");
            if (schedulesDTOList != null && cmbFacilityMap != null && cmbFacilityMap.SelectedValue != null && cmbFacilityMap.SelectedIndex > -1)
            {
                this.facilityMapDTO = (FacilityMapDTO)cmbFacilityMap.SelectedItem;
                Decimal fromTime = 0;
                fromTime = Convert.ToDecimal(cmbFromHour.SelectedItem) + (cmbFromAM.SelectedItem.Equals("AM") ? 0 : 12) + (Convert.ToInt32(cmbFromMin.SelectedItem) / 100);

                Decimal toTime = 0;
                toTime = Convert.ToDecimal(cmbToHour.SelectedItem) + (cmbToAM.SelectedItem.Equals("AM") ? 0 : 12) + (Convert.ToInt32(cmbToMin.SelectedItem) / 100);

                List<SchedulesDTO> tempList = new List<SchedulesDTO>();
                if (toTime < fromTime)
                {
                    tempList = schedulesDTOList.Where(x => x.ScheduleTime >= fromTime && x.ScheduleToTime <= 24).ToList();
                    tempList.AddRange(schedulesDTOList.Where(x => x.ScheduleTime >= 0 && x.ScheduleToTime <= toTime).ToList());
                }
                else
                {
                    tempList = schedulesDTOList.Where(x => x.ScheduleTime >= fromTime && x.ScheduleToTime <= toTime).ToList();
                }

                if (tempList != null && tempList.Any())
                {
                    
                    foreach (SchedulesDTO scheduleDTO in tempList)
                    {
                        facilityMapSchedules.Add(scheduleDTO.ScheduleId, scheduleDTO.ScheduleName + " " + scheduleDTO.ScheduleTime.ToString().Replace(".", ":"));
                    }

                    cmbFacilityMapSchedules.DataSource = new BindingSource(facilityMapSchedules, null);
                    cmbFacilityMapSchedules.DisplayMember = "Value";
                    cmbFacilityMapSchedules.ValueMember = "Key";
                    
                    cmbFacilityMapSchedules.SelectedValue = -1;
                    cmbFacilityMapSchedules.Enabled = true;
                }
            }
            else
            {
                cmbFacilityMapSchedules.Enabled = false;
            }
            cmbFacilityMapSchedules.ResumeLayout();
        }

        private void txtCard_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.swipedCard != null && swipedCard.CardNumber.Equals(txtCard.Text))
                return;

            CardSwiped(txtCard.Text);
            log.LogMethodExit();
        }

        TextBox currentTextBox;
        void txt_Enter(object sender, EventArgs e)
        {
            currentTextBox = sender as TextBox;
            if (this.autoKeyboard)
                btnKeyBoard_Click(sender, e);
        }

        private void btnKeyBoard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, currentTextBox);
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                keypad.Show();
            }
            else if (keypad.Visible)
                keypad.Hide();
            else
            {
                keypad.Show();
            }
            log.LogMethodExit();
        }

        private void cmbFromHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetSchedulesComboBox();
            isDateTimeValueChanged = true;
            log.LogMethodExit();
        }

        private void cmbFromMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetSchedulesComboBox();
            isDateTimeValueChanged = true;
            log.LogMethodExit();
        }

        private void cmbFromAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetSchedulesComboBox();
            isDateTimeValueChanged = true;
            log.LogMethodExit();
        }

        private void cmbToHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetSchedulesComboBox();
            isDateTimeValueChanged = true;
            log.LogMethodExit();
        }

        private void cmbToMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetSchedulesComboBox();
            isDateTimeValueChanged = true;
            log.LogMethodExit();
        }

        private void cmbToAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetSchedulesComboBox();
            isDateTimeValueChanged = true;
            log.LogMethodExit();
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            isDateTimeValueChanged = true;
            log.LogMethodExit();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            isDateTimeValueChanged = true;
            log.LogMethodExit();
        }
        
    }
}
