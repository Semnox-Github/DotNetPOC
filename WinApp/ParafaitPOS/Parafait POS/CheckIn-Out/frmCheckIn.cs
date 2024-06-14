/***************************************************************************************************************************
 * Project Name - frmCheckIn
 * Description  - Check In Form
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 ****************************************************************************************************************************
*2.70.0       28-Jun-2019     Mathew Ninan        Use CheckInDTO and CheckInDetailDTO
*2.70        1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards
*2.70.3       14-Feb-2020     Lakshminarayana      Modified: Creating unregistered customer during check-in process
*2.100.0      21-Sep-2020     Mathew Ninan        Added encryption and decryption logic to check in photo.
*2.140.0      09-Sep-2021      Girish Kundar  Modified: Check In/Check out changes
*2.130.4     22-Feb-2022      Mathew Ninan    Modified DateTime to ServerDateTime 
*2.150.3     26-May-2023      Mathew Ninan        Ticket# 126687 - Performance improvement for checkin form load
*****************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AForge.Video.DirectShow;
using Semnox.Parafait.Device;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.UI;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Product;
using System.Globalization;
using Semnox.Parafait.Languages;
using System.Threading.Tasks;

namespace Parafait_POS
{
    public partial class CheckIn : Form
    {
        string PhotoDirectory;
        Card inCard;
        int inUnits = 0;
        int isCheckOut = 0;
        int CheckInFacilityId;
        // public clsCheckIn checkIn;//to be removed
        // public clsCheckOut checkOut;//to be removed
        public CheckInDTO checkInDTO;
        public CheckInDetailDTO checkInDetailDTO;
        public CheckInDetailDTO checkOutDetailDTO;
        //Create webcam object
        VideoCaptureDevice videoSource;
        private string checkInOut;

        Utilities Utilities = POSStatic.Utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        //TaskProcs TaskProcs = POSStatic.TaskProcs;
        ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;
        List<CustomerDTO> customerDTOList;
        Transaction CurrentTrx;
        private VirtualKeyboardController virtualKeyboardController;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TagNumberParser tagNumberParser;
        private decimal productQuantity;
        private bool allowAddRows;
        public List<CustomCheckInDTO> customCheckInDTOList;
        public List<ComboCheckInDetailDTO> customCheckInDetailDTOList;
        private Dictionary<int, int> comboCheckinQuantityList;
        private ProductsContainerDTO.PauseUnPauseType checkInAttribute;

        public CheckIn(int pCheckInFacilityId, Card card, int Units, string checkInOut, CheckInDTO pCheckInDTO,
                      Utilities pUtilities, Transaction inTrx, ref decimal productQuantity, bool allowUserToAddRows = false,
                      Dictionary<int, int> quantityList = null) // check in
        {
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            log.LogMethodEntry(pCheckInFacilityId, card, Units, checkInOut, pCheckInDTO, pUtilities, inTrx, productQuantity, allowUserToAddRows);
            this.checkInOut = checkInOut;
            this.comboCheckinQuantityList = quantityList;
            Utilities = pUtilities;
            Utilities.setLanguage();
            InitializeComponent();
            lblMessage.Text = "";
            this.productQuantity = productQuantity;
            tagNumberParser = new TagNumberParser(pUtilities.ExecutionContext);
            allowAddRows = allowUserToAddRows;
            virtualKeyboardController = new VirtualKeyboardController();
            virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            customCheckInDTOList = new List<CustomCheckInDTO>();
            CurrentTrx = inTrx;
            PhotoDirectory = Utilities.ParafaitEnv.CheckInPhotoDirectory;
            if (pCheckInDTO != null) // used for editing from POS
            {
                pCheckInFacilityId = pCheckInDTO.CheckInFacilityId;
                card = new Card(pCheckInDTO.CardId, Utilities.ParafaitEnv.LoginID, Utilities);
                Units = pCheckInDTO.CheckInDetailDTOList.Count <= 1 ? 0 : pCheckInDTO.CheckInDetailDTOList.Count;
                checkInOut = "CHECK-IN";
                checkInDTO = pCheckInDTO;
                cmbCheckInStatus.Visible = false;
                if (!string.IsNullOrEmpty(checkInDTO.PhotoFileName))
                {
                    try
                    {
                        SqlCommand cmdImage = Utilities.getCommand();
                        cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";

                        cmdImage.Parameters.AddWithValue("@FileName", PhotoDirectory + "\\" + checkInDTO.PhotoFileName);
                        object o = cmdImage.ExecuteScalar();
                        byte[] imageInBytes = o as byte[];
                        try
                        {
                            imageInBytes = Encryption.Decrypt(imageInBytes);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            imageInBytes = o as byte[];
                        }
                        pictureBoxCamera.Image = Utilities.ConvertToImage(imageInBytes);
                        pictureBoxCamera.Tag = checkInDTO.PhotoFileName;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                        log.Fatal("Ends-CheckIn(" + pCheckInFacilityId + "," + card + "," + Units + "," + checkInOut + "," + pCheckInDTO + "," + pUtilities + "," + inTrx + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    }
                }

                if (checkInDTO.TableId != -1)
                {
                    txtTableName.Tag = checkInDTO.TableId;
                    txtTableName.Text = new FacilityTables(Utilities.ExecutionContext, checkInDTO.TableId).FacilityTableDTO.TableName;
                }
            }

            inCard = card;
            inUnits = Units;
            CheckInFacilityId = pCheckInFacilityId;
            if (Units < 0)
                inUnits = -1;
            LoadStatuses(ProductsContainerDTO.PauseUnPauseType.NONE);
            EnableStatusSearchOption();
            this.cmbCheckInStatus.SelectedIndexChanged += new System.EventHandler(CmbCheckInStatus_SelectedIndexChanged);
            FacilityDTO facilityDTO = new FacilityBL(Utilities.ExecutionContext, CheckInFacilityId).FacilityDTO;
            if (checkInOut == "CHECK-IN")
            {
                log.Info("CheckIn(" + pCheckInFacilityId + "," + card + "," + Units + "," + checkInOut + "," + pCheckInDTO + "," + pUtilities + "," + inTrx + ") - CHECK-IN");//Added for logger function on 08-Mar-2016
                dgvCheckedInList.Enabled = false;
                setDGVUnits();
                if (CurrentTrx.TrxLines.Exists(x => x.LineCheckInDTO != null)) // same transation without save
                {
                    checkInDTO = CurrentTrx.TrxLines.Where(x => x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO;
                    if (checkInDTO.CheckInId > -1)
                    {
                        checkInDTO = new CheckInBL(Utilities.ExecutionContext, checkInDTO.CheckInId, true, true).CheckInDTO;
                        if (checkInDTO.CheckInDetailDTOList != null)
                        {
                            foreach (CheckInDetailDTO checkInDetailDTO in checkInDTO.CheckInDetailDTOList)
                            {
                                var dto = CurrentTrx.TrxLines.Where(x => x.LineCheckInDetailDTO != null && x.card != null &&
                                         x.LineCheckInDetailDTO.CheckInDetailId == checkInDetailDTO.CheckInDetailId).FirstOrDefault();
                                if (dto != null)
                                {
                                    checkInDetailDTO.AccountNumber = dto.CardNumber;
                                    checkInDetailDTO.CardId = dto.card.card_id;
                                    dto.LineCheckInDetailDTO = checkInDetailDTO;
                                }
                                var line = CurrentTrx.TrxLines.Where(x => x.LineCheckInDetailDTO != null &&
                                                  x.LineCheckInDetailDTO.CheckInDetailId == checkInDetailDTO.CheckInDetailId).FirstOrDefault();
                                CustomCheckInDTO customCheckInDTO = new CustomCheckInDTO();
                                customCheckInDTO.checkInDetailDTO = line.LineCheckInDetailDTO;
                                customCheckInDTO.transactionLine = line;
                                customCheckInDTOList.Add(customCheckInDTO);
                            }
                            foreach (Transaction.TransactionLine line in CurrentTrx.TrxLines)
                            {
                                if (line.LineCheckInDetailDTO != null && line.LineCheckInDetailDTO.CheckInDetailId == -1)
                                {
                                    CustomCheckInDTO customCheckInDTO = new CustomCheckInDTO();
                                    customCheckInDTO.checkInDetailDTO = line.LineCheckInDetailDTO;
                                    customCheckInDTO.transactionLine = line;
                                    customCheckInDTOList.Add(customCheckInDTO);
                                    allowAddRows = true;
                                }

                            }
                        }
                    }
                    else // New Lines 
                    {
                        foreach (Transaction.TransactionLine line in CurrentTrx.TrxLines)
                        {
                            if (line.LineCheckInDetailDTO != null && line.LineCheckInDetailDTO.CheckInDetailId == -1)
                            {
                                CustomCheckInDTO customCheckInDTO = new CustomCheckInDTO();
                                customCheckInDTO.checkInDetailDTO = line.LineCheckInDetailDTO;
                                customCheckInDTO.transactionLine = line;
                                customCheckInDTOList.Add(customCheckInDTO);
                            }
                        }
                    }
                    dgvCheckInDetails.AllowUserToAddRows = allowUserToAddRows;
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Check In");
                }

                if (checkInDTO == null)
                {
                    checkInDTO = new CheckInDTO(); //New object for CheckInDTO
                }
                else
                {
                    if (customCheckInDTOList != null && customCheckInDTOList.Any())
                    {
                        int j = 0;
                        while (j < customCheckInDTOList.Count)
                        {
                            if (inUnits == 0 || inUnits == -1 || allowAddRows)
                                dgvCheckInDetails.Rows.Add();
                            if (customCheckInDTOList[j].checkInDetailDTO.Age > 0)
                                dgvCheckInDetails["Age", j].Value = customCheckInDTOList[j].checkInDetailDTO.Age;
                            dgvCheckInDetails["Allergies", j].Value = customCheckInDTOList[j].checkInDetailDTO.Allergies;
                            if (customCheckInDTOList[j].checkInDetailDTO.DateOfBirth != DateTime.MinValue)
                                dgvCheckInDetails["DateOfBirth", j].Value = customCheckInDTOList[j].checkInDetailDTO.DateOfBirth;
                            if (customCheckInDTOList[j].checkInDetailDTO.CardId > 0)
                                dgvCheckInDetails["detailCard", j].Value = new Card(customCheckInDTOList[j].checkInDetailDTO.CardId, Utilities.ParafaitEnv.LoginID, Utilities).CardNumber;
                            else
                            {
                                dgvCheckInDetails["detailCard", j].Value = customCheckInDTOList[j].checkInDetailDTO.AccountNumber;
                            }
                            dgvCheckInDetails["detailName", j].Value = customCheckInDTOList[j].checkInDetailDTO.Name;
                            dgvCheckInDetails["Remarks", j].Value = customCheckInDTOList[j].checkInDetailDTO.Remarks;
                            dgvCheckInDetails["SpecialNeeds", j].Value = customCheckInDTOList[j].checkInDetailDTO.SpecialNeeds;
                            dgvCheckInDetails["VehicleColor", j].Value = customCheckInDTOList[j].checkInDetailDTO.VehicleColor;
                            dgvCheckInDetails["VehicleModel", j].Value = customCheckInDTOList[j].checkInDetailDTO.VehicleModel;
                            dgvCheckInDetails["VehicleNumber", j].Value = customCheckInDTOList[j].checkInDetailDTO.VehicleNumber;
                            dgvCheckInDetails["CheckIndetailId", j].Value = customCheckInDTOList[j].checkInDetailDTO.CheckInDetailId;
                            if (customCheckInDTOList[j].checkInDetailDTO.CheckInDetailId > -1)
                            {
                                dgvCheckInDetails.Rows[j].ReadOnly = true;
                                dgvCheckInDetails.Rows[j].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                            dgvCheckInDetails["Status", j].Value = customCheckInDTOList[j].checkInDetailDTO.Status;
                            dgvCheckInDetails["Line", j].Value = customCheckInDTOList[j].transactionLine;
                            j++;
                        }
                    }
                    //else
                    //{
                    //    int j = 0;
                    //    while (j < checkInDTO.CheckInDetailDTOList.Count)
                    //    {
                    //        if (inUnits == 0 || inUnits == -1)
                    //            dgvCheckInDetails.Rows.Add();
                    //        if (checkInDTO.CheckInDetailDTOList[j].Age > 0)
                    //            dgvCheckInDetails["Age", j].Value = checkInDTO.CheckInDetailDTOList[j].Age;
                    //        dgvCheckInDetails["Allergies", j].Value = checkInDTO.CheckInDetailDTOList[j].Allergies;
                    //        if (checkInDTO.CheckInDetailDTOList[j].DateOfBirth != DateTime.MinValue)
                    //            dgvCheckInDetails["DateOfBirth", j].Value = checkInDTO.CheckInDetailDTOList[j].DateOfBirth;
                    //        if (checkInDTO.CheckInDetailDTOList[j].CardId > 0)
                    //            dgvCheckInDetails["detailCard", j].Value = new Card(checkInDTO.CheckInDetailDTOList[j].CardId, Utilities.ParafaitEnv.LoginID, Utilities).CardNumber;
                    //        else
                    //        {
                    //            dgvCheckInDetails["detailCard", j].Value = checkInDTO.CheckInDetailDTOList[j].AccountNumber;
                    //        }
                    //        dgvCheckInDetails["detailName", j].Value = checkInDTO.CheckInDetailDTOList[j].Name;
                    //        dgvCheckInDetails["Remarks", j].Value = checkInDTO.CheckInDetailDTOList[j].Remarks;
                    //        dgvCheckInDetails["SpecialNeeds", j].Value = checkInDTO.CheckInDetailDTOList[j].SpecialNeeds;
                    //        dgvCheckInDetails["VehicleColor", j].Value = checkInDTO.CheckInDetailDTOList[j].VehicleColor;
                    //        dgvCheckInDetails["VehicleModel", j].Value = checkInDTO.CheckInDetailDTOList[j].VehicleModel;
                    //        dgvCheckInDetails["VehicleNumber", j].Value = checkInDTO.CheckInDetailDTOList[j].VehicleNumber;
                    //        dgvCheckInDetails["CheckIndetailId", j].Value = checkInDTO.CheckInDetailDTOList[j].CheckInDetailId;
                    //        if (checkInDTO.CheckInDetailDTOList[j].CheckInDetailId > -1)
                    //        {
                    //            dgvCheckInDetails.Rows[j].ReadOnly = true;
                    //            dgvCheckInDetails.Rows[j].DefaultCellStyle.BackColor = Color.Gray;
                    //        }
                    //        dgvCheckInDetails["Status", j].Value = checkInDTO.CheckInDetailDTOList[j].Status;
                    //        dgvCheckInDetails["Line", j].Value = null;
                    //        j++;
                    //    }
                    //}
                }

                if (card != null)
                {
                    checkInDTO.CustomerDTO = card.customerDTO;
                    if (card.customerDTO != null)
                        checkInDTO.CustomerId = card.customerDTO.Id;
                    txtCardNumber.Text = card.CardNumber;
                    if (checkInDTO.CustomerDTO != null)
                    {
                        populateCustomerDetails();
                    }
                    else
                    {
                        checkInDTO.CustomerDTO = new CustomerDTO();
                        if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMERTYPE") != "N")
                        {
                            checkInDTO.CustomerDTO.CustomerType = CustomerType.UNREGISTERED;
                        }
                    }
                }
                else
                {
                    if (inTrx != null && inTrx.customerDTO != null)
                    {
                        checkInDTO.CustomerDTO = inTrx.customerDTO;
                    }
                    if (checkInDTO.CustomerDTO == null)
                    {
                        checkInDTO.CustomerDTO = new CustomerDTO();
                        if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMERTYPE") != "N")
                        {
                            checkInDTO.CustomerDTO.CustomerType = CustomerType.UNREGISTERED;
                        }
                    }
                    else
                        populateCustomerDetails();
                }

                //checkIn.AvailableUnits = inUnits;
                //txtCapacity.Text = checkIn.getCapacity(CheckInFacilityId, null).ToString();
                txtCapacity.Text = facilityDTO.Capacity == null ? "0" : facilityDTO.Capacity.ToString();
                if (txtCapacity.Text == "0")
                    txtCapacity.Text = "--";
                this.Text = "Check-In " + facilityDTO.FacilityName;

                // for single person check in use default customer name
                if (card != null && card.CardStatus == "ISSUED" && txtFirstName.Text != "" && Units == 1 && pCheckInDTO == null && Utilities.ParafaitEnv.PHOTO_MANDATORY_FOR_CHECKIN == "N")
                {
                    dgvCheckInDetails["DetailName", 0].Value = txtFirstName.Text;
                }

            }
            else // check out
            {
                log.Info("CheckIn(" + pCheckInFacilityId + "," + card + "," + Units + "," + checkInOut + "," + pCheckInDTO + "," + pUtilities + "," + inTrx + ") - CHECK-OUT");//Added for logger function on 08-Mar-2016
                pbCapture.Enabled = pbFingerPrint.Enabled = btnCheckIn.Enabled = false;
                btnCustomerDetails.Enabled = true;
                cmbCheckInStatus.Visible = true;
                checkInDTO = new CheckInDTO();//new object for CheckInDTO
                checkOutDetailDTO = new CheckInDetailDTO();
                isCheckOut = 1;
                if (card != null && card.CardStatus != "NEW")
                {
                    checkInDTO.CustomerDTO = card.customerDTO;
                    if (card.customerDTO != null)
                        checkInDTO.CustomerId = card.customerDTO.Id;
                    txtCardNumber.Text = card.CardNumber;
                    txtCardNumber.ReadOnly = true;
                    if (checkInDTO.CustomerDTO != null)
                    {
                        populateCustomerDetails();
                    }
                }
                else
                    txtCardNumber.ReadOnly = false;

                //txtCapacity.Text = checkIn.getCapacity(CheckInFacilityId, null).ToString();
                txtCapacity.Text = facilityDTO.Capacity == null ? "0" : facilityDTO.Capacity.ToString();
                if (txtCapacity.Text == "0")
                    txtCapacity.Text = "--";
                //this.Text = "Check-Out " + checkIn.FacilityName;
                this.Text = "Check-Out " + facilityDTO.FacilityName;
            }

            lblFacility.Text = facilityDTO.FacilityName;

            SetupCustomDetailControls();
            log.LogMethodExit();
        }

        private void CmbCheckInStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ComboBox comboBox = (ComboBox)sender;
                CheckInStatusFilter status;
                string selectedValue = ((KeyValuePair<string, string>)comboBox.SelectedItem).Key;
                Enum.TryParse<CheckInStatusFilter>(selectedValue, out status);
                log.Debug("selectedValue :" + selectedValue);
                log.Debug("status :" + status);
                if (isCheckOut == 1)
                {
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Check Out");
                    btnSearch.PerformClick();
                    if (string.IsNullOrWhiteSpace(txtCardNumber.Text) && string.IsNullOrWhiteSpace(txtFirstName.Text))
                    {
                        searchFilter = "";
                    }

                }
                else if (status.ToString() == CheckInStatusFilter.ORDERED.ToString())
                {
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Check In");
                    groupBox2.Text = MessageUtils.getMessage("Pending CheckIn List");
                }
                else if (status.ToString() == CheckInStatusFilter.CHECKEDIN.ToString() || status.ToString() == CheckInStatusFilter.PAUSED.ToString())
                {
                    groupBox2.Text = MessageUtils.getMessage("Paused/UnPaused List");
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Pause/UnPause");
                    //btnSearch.PerformClick();
                }
                else if (status.ToString() == CheckInStatusFilter.PENDING.ToString())
                {
                    groupBox2.Text = MessageUtils.getMessage("Pending List");
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Pending");
                }
                else if (status.ToString() == CheckInStatusFilter.ALL.ToString())
                {
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("CheckIn/Pause");
                }
                //btnSearch.PerformClick();
                if (string.IsNullOrWhiteSpace(txtCardNumber.Text) && string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    searchFilter = "";
                }
                refreshCheckIns(status.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                refreshCheckIns(CheckInStatusFilter.CHECKEDIN.ToString());
            }
        }

        private void LoadStatuses(ProductsContainerDTO.PauseUnPauseType pauseType)
        {
            log.LogMethodEntry(pauseType);
            cmbCheckInStatus.Visible = false;
            List<KeyValuePair<string, string>> statusList = new List<KeyValuePair<string, string>>();
            Array statusArray = Enum.GetValues(typeof(CheckInStatusFilter));
            foreach (CheckInStatusFilter status in statusArray)
            {
                string listValue = status.ToString();
                if (status == CheckInStatusFilter.CHECKEDIN)
                {
                    listValue = MessageContainerList.GetMessage(Utilities.ExecutionContext, "CHECKED IN");
                }
                if (status == CheckInStatusFilter.CHECKEDOUT)
                {
                    listValue = MessageContainerList.GetMessage(Utilities.ExecutionContext, "CHECKED OUT");
                }
                statusList.Add(new KeyValuePair<string, string>(status.ToString(), listValue));
            }

            // default show only Paused records and dont allow to select and search other status
            if (pauseType == ProductsContainerDTO.PauseUnPauseType.UNPAUSE)
            {
                statusList = statusList.Where(x => x.Key == CheckInStatusFilter.PAUSED.ToString()).ToList();
                cmbCheckInStatus.Enabled = false;
            }
            // default show only Checked in records and dont allow to select and search other status
            if (pauseType == ProductsContainerDTO.PauseUnPauseType.PAUSE)
            {
                statusList = statusList.Where(x => x.Key == CheckInStatusFilter.CHECKEDIN.ToString()).ToList();
                cmbCheckInStatus.Enabled = false;
            }

            cmbCheckInStatus.DataSource = statusList;
            cmbCheckInStatus.DisplayMember = "Value";
            cmbCheckInStatus.ValueMember = "Key";
            log.LogMethodExit();
        }

        private void EnableStatusSearchOption()
        {
            log.LogMethodEntry();
            cmbCheckInStatus.Visible = false;
            if (CurrentTrx.TrxLines.Exists(x => x.LineCheckInDTO != null)
                     && CurrentTrx.TrxLines.Exists(x => x.LineCheckInDetailDTO != null
                     && (x.LineCheckInDetailDTO.Status == CheckInStatus.ORDERED || x.LineCheckInDetailDTO.Status == CheckInStatus.PENDING)))
            {
                cmbCheckInStatus.Visible = true;
            }
            log.LogMethodExit();
        }

        private void SetupCustomDetailControls()
        {
            log.LogMethodEntry();
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "NOTES") == "N")
                {
                    flpNotes.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "CITY") == "N")
                {
                    flpCity.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "STATE") == "N")
                {
                    flpState.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "COUNTRY") == "N")
                {
                    flpCountry.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "PIN") == "N")
                {
                    flpPin.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "ADDRESS1") == "N")
                {
                    flpAddress1.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "ADDRESS2") == "N")
                {
                    flpAddress2.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "CONTACT_PHONE") == "N")
                {
                    flpMobile.Visible = false;
                    flpPhone.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "MIDDLE_NAME") == "N")
                {
                    txtMiddleName.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "CUSTOMER_NAME") == "N")
                {
                    txtFirstName.Visible = false;
                    lblName.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "LAST_NAME") == "N")
                {
                    txtLastName.Visible = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "EMAIL") == "N")
                {
                    flpEmail.Visible = false;
                }
                vsbCustomerDetails.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        public CheckIn(Utilities pUtilities, ProductsContainerDTO.PauseUnPauseType checkInAttribute = ProductsContainerDTO.PauseUnPauseType.NONE)
        {
            log.LogMethodEntry(checkInAttribute);
            Utilities = pUtilities;
            Utilities.setLanguage();
            CheckInFacilityId = -1;
            this.checkInAttribute = checkInAttribute;
            tagNumberParser = new TagNumberParser(pUtilities.ExecutionContext);
            InitializeComponent();
            lblMessage.Text = "";
            if (checkInDTO == null)
                checkInDTO = new CheckInDTO();
            if (checkOutDetailDTO == null)
            {
                pbCapture.Enabled = false;
                dgvCheckInDetails.Enabled = false;
                pbFingerPrint.Enabled = false;
                btnCheckIn.Enabled = false;
                txtCardNumber.Enabled = true;
            }

            if (string.IsNullOrEmpty(checkInOut) && checkOutDetailDTO == null)
            {
                groupBox2.Text = MessageUtils.getMessage("Paused/UnPaused List");
                dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Pause/UnPause");
            }
            SetupCustomDetailControls();
            LoadStatuses(checkInAttribute);
            cmbCheckInStatus.Visible = true;
            this.cmbCheckInStatus.SelectedIndexChanged += new System.EventHandler(CmbCheckInStatus_SelectedIndexChanged);
            log.LogMethodExit();
        }

        private void SelectRelatedCustomers(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchCustomerRelationshipParams = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.EFFECTIVE_DATE, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.EXPIRY_DATE, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            List<CustomerRelationshipDTO> customerRelationshipDTOList = null;
            CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(Utilities.ExecutionContext);
            customerRelationshipDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchCustomerRelationshipParams);
            if (customerRelationshipDTOList != null)
            {
                SelectRelatedCustomerUI selectRelatedCustomerUI = new SelectRelatedCustomerUI(Utilities, customerDTO, MessageBox.Show);
                DialogResult dr = selectRelatedCustomerUI.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (allowAddRows == false && comboCheckinQuantityList == null)
                    {
                        log.Debug("Cannot add the check in details. Kiosk transactions");
                        return;
                    }
                    if (selectRelatedCustomerUI.CustomerDTOList != null && selectRelatedCustomerUI.CustomerDTOList.Count > 0)
                    {
                        List<CustomerDTO> tempCustomerDTOList = new List<CustomerDTO>();
                        if (customerDTOList == null)
                            tempCustomerDTOList = new List<CustomerDTO>(selectRelatedCustomerUI.CustomerDTOList);
                        else
                        {
                            foreach (CustomerDTO tempCustomerDTO in selectRelatedCustomerUI.CustomerDTOList)
                            {
                                if (!customerDTOList.Exists(x => x.Id == tempCustomerDTO.Id))
                                    tempCustomerDTOList.Add(tempCustomerDTO);
                            }
                        }

                        foreach (CustomerDTO c in tempCustomerDTOList)
                        {
                            DataGridViewRow row = (DataGridViewRow)dgvCheckInDetails.Rows[0].Clone();

                            row.Cells[customerIdDataGridViewTextBoxColumn.Index].Value = c.Id;
                            row.Cells[DetailName.Index].Value = c.FirstName;
                            row.Cells[DateOfBirth.Index].Value = c.DateOfBirth;
                            if (c.DateOfBirth != null &&
                                ((ServerDateTime.Now.Date.Year - Convert.ToDateTime(c.DateOfBirth).Year) < 100) &&
                                ((Convert.ToDateTime(c.DateOfBirth).Year) != 1900))
                                row.Cells[Age.Index].Value = ServerDateTime.Now.Date.Year - Convert.ToDateTime(c.DateOfBirth).Year;
                            row.DefaultCellStyle.BackColor = Color.White;//MistyRose
                            dgvCheckInDetails.Rows.Add(row);
                        }

                        if (customerDTOList == null)
                            customerDTOList = new List<CustomerDTO>();
                        customerDTOList.AddRange(tempCustomerDTOList);
                    }
                }
            }
            log.LogMethodExit(null);
        }

        private string GetStateName(int stateId)
        {
            log.LogMethodEntry(stateId);
            string stateName = "";
            if (stateId != -1)
            {
                StateBL stateBL = new StateBL(stateId);
                if (stateBL.GetStateDTO != null)
                {
                    stateName = stateBL.GetStateDTO.Description;
                }
            }
            log.LogMethodExit(stateName);
            return stateName;
        }

        private string GetCountryName(int countryId)
        {
            log.LogMethodEntry(countryId);
            string countryName = "";
            if (countryId != -1)
            {
                CountryBL countryBL = new CountryBL(countryId);
                if (countryBL.GetCountryDTO != null)
                {
                    countryName = countryBL.GetCountryDTO.CountryName;
                }
            }
            log.LogMethodExit(countryName);
            return countryName;
        }

        void populateCustomerDetails()
        {
            log.Debug("Starts-CheckIn()");//Added for logger function on 08-Mar-2016
            AddressDTO addressDTO = null;
            if (checkInDTO.CustomerDTO.AddressDTOList != null)
            {
                addressDTO = checkInDTO.CustomerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
            }
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtCountry.Text = string.Empty;
            txtPIN.Text = string.Empty;
            txtState.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtMobile.Text = string.Empty;
            if (addressDTO != null)
            {
                txtAddress1.Text = addressDTO.Line1;
                txtAddress2.Text = addressDTO.Line2;
                txtCity.Text = addressDTO.City;
                txtCountry.Text = GetCountryName(addressDTO.CountryId);
                txtPIN.Text = addressDTO.PostalCode;
                txtState.Text = GetStateName(addressDTO.StateId);
            }
            txtPhone.Text = checkInDTO.CustomerDTO.PhoneNumber;
            txtMobile.Text = checkInDTO.CustomerDTO.SecondaryPhoneNumber;
            txtFirstName.Text = checkInDTO.CustomerDTO.FirstName;
            txtEmail.Text = checkInDTO.CustomerDTO.Email;
            txtNotes.Text = checkInDTO.CustomerDTO.Notes;
            if (string.IsNullOrEmpty(checkInDTO.PhotoFileName) && !string.IsNullOrEmpty(checkInDTO.CustomerDTO.PhotoURL))
            {
                try
                {
                    pictureBoxCamera.Image = (new CustomerBL(Utilities.ExecutionContext, checkInDTO.CustomerDTO)).GetCustomerImage();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                    log.Fatal("Ends-populateCustomerDetails() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }

            if (checkInDTO.CustomerDTO.Id != -1)
            {
                grpCustomer.Enabled = false;
            }
            log.LogMethodExit();
        }

        void initWebCam()
        {
            log.Debug("Starts-initWebCam()");//Added for logger function on 08-Mar-2016
            try
            {
                //List all available video sources. (That can be webcams as well as tv cards, etc)
                FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                //Check if atleast one video source is available
                if (videosources != null)
                {
                    foreach (FilterInfo source in videosources)
                    {
                        //For example use first video device. You may check if this is your webcam.
                        videoSource = new VideoCaptureDevice(source.MonikerString);
                        if (videoSource.VideoCapabilities.Length > 0)
                        {
                            break;
                        }
                    }

                    try
                    {
                        //Check if the video device provides a list of supported resolutions
                        VideoCapabilities video = null;
                        if (videoSource.VideoCapabilities.Length > 0)
                        {
                            int maxPixel = 800;
                            List<VideoCapabilities> capabilities = videoSource.VideoCapabilities.ToList().Where(v => v.FrameSize.Width <= maxPixel).ToList();
                            if (capabilities != null && capabilities.Any())
                            {
                                int max = capabilities.Max(v => v.FrameSize.Width);
                                video = capabilities.FirstOrDefault(s => s.FrameSize.Width == max);
                                if (video != null)
                                {
                                    videoSource.DesiredFrameSize = video.FrameSize;
                                }
                            }
                            if (video == null)
                            {
                                string highestSolution = "0;0";
                                //Search for the highest resolution
                                for (int i = 0; i < videoSource.VideoCapabilities.Length; i++)
                                {
                                    if (videoSource.VideoCapabilities[i].FrameSize.Width > Convert.ToInt32(highestSolution.Split(';')[0]))
                                        highestSolution = videoSource.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
                                }
                                //Set the highest resolution as active
                                videoSource.DesiredFrameSize = videoSource.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])].FrameSize;
                            }
                            pictureBoxCamera.Height = (int)((decimal)pictureBoxCamera.Width / ((decimal)videoSource.DesiredFrameSize.Width / (decimal)videoSource.DesiredFrameSize.Height));
                        }
                    }
                    catch
                    {
                        log.Fatal("Ends-initWebCam() due to exception in Setting the highest resolution  ");//Added for logger function on 08-Mar-2016
                    }

                    if (videoSource != null)
                    {
                        videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);
                    }
                }
                else
                    pbCapture.Enabled = false;
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(6), "Web Camera");
                POSUtils.ParafaitMessageBox(ex.Message);
                pbCapture.Enabled = false;
                log.Fatal("Ends-initWebCam()  interface due to Unable to open camera due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.Debug("Ends-initWebCam()");//Added for logger function on 08-Mar-2016
        }

        void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            log.Debug("Starts-videoSource_NewFrame()");//Added for logger function on 08-Mar-2016
            //Cast the frame as Bitmap object and don't forget to use ".Clone()" otherwise
            //you'll probably get access violation exceptions
            pictureBoxCamera.Image = (Bitmap)eventArgs.Frame.Clone();
            log.Debug("Ends-videoSource_NewFrame()");//Added for logger function on 08-Mar-2016
        }

        /// <summary>
        /// Validate Table for count and max check-ins
        /// </summary>
        /// <returns>true or false</returns>
        public bool validateTable()
        {
            log.LogMethodEntry();
            if ((isCheckOut == 0 || inCard == null) && checkInDTO.TableId == -1)
            {
                CheckIn_Out.frmTables ftb = new CheckIn_Out.frmTables(CheckInFacilityId, -1);
                DialogResult dr = ftb.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    log.Info("validateTable() - frmTables dialog OK is clicked");//Added for logger function on 08-Mar-2016
                    int tableCheckedInCount = 0;
                    foreach (Transaction.TransactionLine tl in CurrentTrx.TrxLines)
                    {
                        if (tl.LineCheckInDTO != null && tl.LineCheckInDTO.TableId == ftb.Table.TableId)
                        {
                            tableCheckedInCount += tl.LineCheckInDTO.CheckInDetailDTOList
                                                                     .Where(x => x.CheckOutTime == null
                                                                     || x.CheckOutTime > Utilities.getServerTime()).ToList().Count;
                        }
                    }
                    CheckInBL checkInCountBL = new CheckInBL(Utilities.ExecutionContext);
                    int totalCheckedInForTable = checkInCountBL.GetTotalCheckedInForTable(ftb.Table.TableId);
                    if (isCheckOut == 0
                        && ftb.Table.MaxCheckIns > 0
                            && totalCheckedInForTable + tableCheckedInCount >= ftb.Table.MaxCheckIns)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(517, ftb.Table.MaxCheckIns.ToString()));
                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        this.Close();
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                    {
                        txtTableName.Text = ftb.Table.TableName;
                        txtTableName.Tag = ftb.Table.TableId;
                        log.Info("Ends-validateTable()");//Added for logger function on 08-Mar-2016
                        return true;
                    }
                }
                else if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    log.Info("Ends-validateTable() - frmTables dialog Cancel is clicked");//Added for logger function on 08-Mar-2016
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                    return false;
                }
            }

            log.LogMethodExit(true);
            return true;
        }

        public bool ValidateProductQuantity(decimal productQuantity)
        {
            log.LogMethodEntry(productQuantity);
            bool result = true;
            int newCheckInDetailCount = 0;
            // Get count of new records

            if (customCheckInDTOList != null && customCheckInDTOList.Any())
            {
                newCheckInDetailCount = customCheckInDTOList.Where(x => x.checkInDetailDTO != null && x.checkInDetailDTO.CheckInDetailId == -1
                                                                   && x.transactionLine == null).ToList().Count();
            }
            if (productQuantity > 1 && newCheckInDetailCount < productQuantity)
            {
                log.Debug("Entered quantity is less than the selected quantity");
                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "You have selected " + productQuantity + " quantities. Do you want to Proceed?");
                if (POSUtils.ParafaitMessageBox(message, "Check In Count", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (productQuantity > 1 && newCheckInDetailCount > productQuantity)
            {
                log.Debug("Entered quantity is more than the selected quantity");
                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "You have selected " + productQuantity + " quantities. Please remove  " + (newCheckInDetailCount - productQuantity) + " check in detail entries.");
                POSUtils.ParafaitMessageBox(message, "Check In Count");
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }
        private void frmCheckIn_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();//Added for logger function on 08-Mar-2016
            initWebCam();
            btnSearch.PerformClick();
            Utilities.setupDataGridProperties(ref dgvCheckedInList);
            Utilities.setupDataGridProperties(ref dgvCheckInDetails);
            dgvCheckInDetails.BackgroundColor = dgvCheckedInList.BackgroundColor = this.BackColor;
            dgvCheckInDetails.RowHeadersVisible = false;
            dgvCheckInDetails.Columns["DateOfBirth"].DefaultCellStyle = Utilities.gridViewDateCellStyle();
            dgvCheckInDetails.MultiSelect = false;
            dgvCheckInDetails.BorderStyle = BorderStyle.FixedSingle;
            this.ActiveControl = btnSearch;
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            if (ParafaitEnv.AUTO_CHECK_IN_POS && ParafaitEnv.AUTO_CHECK_IN_PRODUCT > -1)
            {
                doCheckIn();
                log.Info("Ends-frmCheckIn_Load() as AUTO_CHECK_IN_POS");//Added for logger function on 08-Mar-2016
                return;
            }
            Utilities.setLanguage(this);
            log.LogMethodExit();//Added for logger function on 08-Mar-2016
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.Debug("Starts-CardScanCompleteEventHandle()");//Added for logger function on 08-Mar-2016
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
                        POSUtils.ParafaitMessageBox(ex.Message);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        POSUtils.ParafaitMessageBox(ex.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        POSUtils.ParafaitMessageBox(ex.Message);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    POSUtils.ParafaitMessageBox(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                try
                {
                    CardSwiped(CardNumber);
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message);
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }
            log.Debug("Ends-CardScanCompleteEventHandle()");//Added for logger function on 08-Mar-2016
        }

        private void CardSwiped(string CardNumber)
        {
            log.Debug("Starts-CardSwiped(" + CardNumber + ")");//Added for logger function on 08-Mar-2016
            Card card = new Card(CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            if (card.technician_card.Equals('Y'))
            {
                //Technician Card (&1) not allowed for Transaction
                string warningMsg = MessageUtils.getMessage(197, card.CardNumber);
                POSUtils.ParafaitMessageBox(warningMsg);
                log.Warn(warningMsg);
                return;
            }
            if (dgvCheckInDetails.CurrentRow != null)
            {
                dgvCheckInDetails.CurrentRow.Cells["detailCard"].Value = CardNumber;
                if (card.customerDTO != null)
                    dgvCheckInDetails.CurrentRow.Cells["detailName"].Value = card.customerDTO.FirstName;
            }
            else if (isCheckOut == 1 && txtCardNumber.ReadOnly == false)
            {
                txtCardNumber.Text = CardNumber;
                btnSearch.PerformClick();
            }
            else if (checkOutDetailDTO == null)
            {
                if (card.customerDTO != null && checkInDTO != null)
                {
                    checkInDTO.CustomerDTO = card.customerDTO;
                    checkInDTO.CustomerId = card.customerDTO.Id;
                }
                else
                {
                    if (checkInDTO != null)
                        checkInDTO.CustomerDTO = null;
                }
                txtCardNumber.Text = CardNumber;
                btnSearch.PerformClick();
            }
            //Added to move the currentcell to the next row automatically after a card scan event
            if ((dgvCheckInDetails != null && dgvCheckInDetails.CurrentRow != null)
                && (dgvCheckInDetails.CurrentRow.Index + 1 < dgvCheckInDetails.RowCount))
            {
                dgvCheckInDetails.Rows[dgvCheckInDetails.CurrentRow.Index + 1].Cells["detailCard"].Selected = true;
            }
            log.Debug("Ends-CardSwiped(" + CardNumber + ")");//Added for logger function on 08-Mar-2016
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCheckIn_Click()");//Added for logger function on 08-Mar-2016
            doCheckIn();
            log.Debug("Ends-btnCheckIn_Click()");//Added for logger function on 08-Mar-2016
        }

        public void doCheckIn()
        {
            log.Debug("Starts-doCheckIn()");//Added for logger function on 08-Mar-2016

            if (Utilities.ParafaitEnv.PHOTO_MANDATORY_FOR_CHECKIN == "Y")
            {
                if (pbCapture.Tag == null || pbCapture.Tag.ToString() == "file")
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(7), "Photo Error");
                    log.Warn("doCheckIn() - no photo was captured before checking in");//Added for logger function on 08-Mar-2016
                    return;
                }
            }
            if (checkInDTO.CustomerDTO != null)
            {
                if (string.IsNullOrWhiteSpace(checkInDTO.CustomerDTO.FirstName))
                {
                    if (inCard != null)
                        checkInDTO.CustomerDTO.FirstName = inCard.CardNumber;
                    else
                        checkInDTO.CustomerDTO.FirstName = MessageUtils.getMessage(471);
                    this.ValidateChildren();
                }
                CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, checkInDTO.CustomerDTO);
                List<ValidationError> validationErrorList = customerBL.Validate();
                if (validationErrorList.Count > 0)
                {
                    lblMessage.Text = validationErrorList[0].Message;
                    return;
                }
            }
            else
            {
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(2157), "Customer Error");
                log.Warn("doCheckIn() - Customer is mandatory for check-in");//Added for logger function on 08-Mar-2016
                return;
            }

            if (dgvCheckInDetails["detailName", 0].Value == null || dgvCheckInDetails["detailName", 0].Value.ToString().Trim() == "")
            {
                dgvCheckInDetails["detailName", 0].Value = txtFirstName.Text;
            }

            if (txtTableName.Tag != null)
            {
                checkInDTO.TableId = Convert.ToInt32(txtTableName.Tag);
                //checkIn.TableNumber = txtTableName.Text;
            }

            if (inCard != null)
            {
                checkInDTO.AccountNumber = inCard.CardNumber;
                checkInDTO.CardId = inCard.card_id;
                //checkIn.card.customerDTO = checkIn.customerDTO;
            }

            if (pbCapture.Tag != null && pbCapture.Tag.ToString() != "file")
                checkInDTO.PhotoFileName = pictureBoxCamera.Tag.ToString();

            //checkIn.Remarks = txtNotes.Text;

            checkInDTO.CheckInFacilityId = CheckInFacilityId;

            checkInDTO.CheckInDetailDTOList.Clear();
            customCheckInDTOList.Clear();
            for (int i = 0; i < dgvCheckInDetails.Rows.Count; i++)
            {
                if (dgvCheckInDetails["detailName", i].Value == null || dgvCheckInDetails["detailName", i].Value.ToString().Trim() == "")
                    continue;
                string status = dgvCheckInDetails["Status", i].Value == null ? CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN) : Convert.ToString(dgvCheckInDetails["Status", i].Value.ToString());
                if (Utilities.ParafaitEnv.CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS == "Y")
                {
                    if (dgvCheckInDetails["detailCard", i].Value == null)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(9), "Detail RFID");
                        log.Warn("doCheckIn() - Enter Card / Wrist Band for Check-In details");//Added for logger function on 08-Mar-2016
                        return;
                    }
                }
                CustomCheckInDTO customCheckInDTO = new CustomCheckInDTO();
                CheckInDetailDTO chkDet = new CheckInDetailDTO();
                checkInDTO.CheckInDetailDTOList.Add(chkDet);
                chkDet.Name = dgvCheckInDetails["detailName", i].Value.ToString();
                chkDet.CheckInDetailId = dgvCheckInDetails["CheckInDetailId", i].Value == null ? -1 : Convert.ToInt32(dgvCheckInDetails["CheckInDetailId", i].Value.ToString());
                if (dgvCheckInDetails["DateOfBirth", i].Value != null)
                {
                    try
                    {
                        chkDet.DateOfBirth = Convert.ToDateTime(dgvCheckInDetails["DateOfBirth", i].Value);
                    }
                    catch
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(10), "Format Error");
                        log.Fatal("Ends-doCheckIn() due to exception Invalid Date Format for Date Of Birth field");//Added for logger function on 08-Mar-2016
                        return;
                    }
                }

                try
                {
                    chkDet.Age = Convert.ToDecimal(dgvCheckInDetails["Age", i].Value);
                }
                catch
                {
                    log.Fatal("Ends-doCheckIn() due to exception Invalid Age Format for Age field");//Added for logger function on 08-Mar-2016
                }
                //End: Modified for Adding logger feature on 08-Mar-2016
                if (dgvCheckInDetails["SpecialNeeds", i].Value != null)
                    chkDet.SpecialNeeds = dgvCheckInDetails["SpecialNeeds", i].Value.ToString();

                if (dgvCheckInDetails["Allergies", i].Value != null)
                    chkDet.Allergies = dgvCheckInDetails["Allergies", i].Value.ToString();

                if (dgvCheckInDetails["Remarks", i].Value != null)
                    chkDet.Remarks = dgvCheckInDetails["Remarks", i].Value.ToString();

                if (dgvCheckInDetails["VehicleNumber", i].Value != null)
                    chkDet.VehicleNumber = dgvCheckInDetails["VehicleNumber", i].Value.ToString();

                if (dgvCheckInDetails["VehicleModel", i].Value != null)
                    chkDet.VehicleModel = dgvCheckInDetails["VehicleModel", i].Value.ToString();

                if (dgvCheckInDetails["VehicleColor", i].Value != null)
                    chkDet.VehicleColor = dgvCheckInDetails["VehicleColor", i].Value.ToString();

                if (dgvCheckInDetails["detailCard", i].Value != null)
                {
                    chkDet.AccountNumber = dgvCheckInDetails["detailCard", i].Value.ToString();
                    chkDet.CardId = new Card(dgvCheckInDetails["detailCard", i].Value.ToString(), Utilities.ParafaitEnv.LoginID, Utilities).card_id;

                }
                if (dgvCheckInDetails["detailCard", i].Value == null &&
                    Utilities.ParafaitEnv.CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS == "N")
                {
                    chkDet.Status = CheckInStatus.ORDERED;
                }
                customCheckInDTO.checkInDetailDTO = chkDet;
                if (dgvCheckInDetails["Line", i].Value != null)
                {
                    customCheckInDTO.transactionLine = (Transaction.TransactionLine)dgvCheckInDetails["Line", i].Value;
                }
                else
                {
                    customCheckInDTO.transactionLine = null;
                }
                customCheckInDTOList.Add(customCheckInDTO);

                // If card is null and status is pending Ischanged = false
                int capacity = 0;
                try
                {
                    capacity = Convert.ToInt32(txtCapacity.Text);
                }
                catch
                {
                    log.Fatal("Ends-doCheckIn() due to exception Invalid capacity Format for capacity field");//Added for logger function on 08-Mar-2016
                }
                if (capacity > 0)
                {
                    if (capacity < Convert.ToInt32(txtTotalCheckedIn.Text) + checkInDTO.CheckInDetailDTOList.Count)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(11), "Check-In Save");
                        log.Info("Ends-doCheckIn() as Total Checked-Ins are more than Capacity in Facility");//Added for logger function on 08-Mar-2016
                        return;
                    }
                }
            }

            // This check to be done only for combo check ins
            if (comboCheckinQuantityList != null && comboCheckinQuantityList.Any())
            {
                // Method to validate the number of vistors and assign the ProductId for check in details based on the profiling
                customCheckInDetailDTOList = ValidateCheckinDetails(customCheckInDTOList);
                if (customCheckInDetailDTOList == null ||
                    (customCheckInDetailDTOList != null && customCheckInDetailDTOList.Any() == false)
                    || (customCheckInDetailDTOList != null && customCheckInDetailDTOList.Exists(x => x.CheckInDetailDTOList != null && x.CheckInDetailDTOList.Any() == false)))
                {
                    log.Debug("quantity does not match");
                    log.Info("Please complete Check-In details for all " + productQuantity + " units");
                    return;
                }
            }
            int checkedInCount = checkInDTO.CheckInDetailDTOList.Count;

            if (allowAddRows)
            {
                inUnits = customCheckInDTOList.Count;
            }
            if (inUnits != 0 && inUnits != -1)
            {
                if (inUnits != checkedInCount)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(12, inUnits), "Check-In Save");
                    log.Info("Ends-doCheckIn() as Please complete Check-In details for all " + inUnits + " units");//Added for logger function on 08-Mar-2016
                    return;
                }
            }
            if (ValidateProductQuantity(productQuantity) == false)
            {
                log.Debug("quantity not matches");
                log.Info("doCheckIn() as Please complete Check-In details for all " + productQuantity + " units");//Added for logger function on 08-Mar-2016
                return;
            }
            if (string.IsNullOrEmpty(checkInDTO.PhotoFileName) == false && pictureBoxCamera.Image != null)
            {
                try
                {
                    byte[] checkInphoto = Utilities.ConvertToByteArray(pictureBoxCamera.Image);
                    string encryptedString = Encryption.Encrypt(checkInphoto);
                    checkInphoto = Convert.FromBase64String(encryptedString);
                    SqlCommand cmd = Utilities.getCommand();
                    cmd.CommandText = "exec SaveBinaryDataToFile @Image, @FileName";
                    cmd.Parameters.AddWithValue("@Image", checkInphoto);
                    cmd.Parameters.AddWithValue("@FileName", PhotoDirectory + "\\" + checkInDTO.PhotoFileName);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message);
                    log.Fatal("Ends-doCheckIn() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }

            this.DialogResult = DialogResult.OK;
            //populateCheckInDTO();
            this.Close();
            log.LogMethodExit();
        }

        private List<ComboCheckInDetailDTO> ValidateCheckinDetails(List<CustomCheckInDTO> customCheckInDTOList)
        {
            log.LogMethodEntry(customCheckInDTOList);
            try
            {
                List<ComboCheckInDetailDTO> result = new List<ComboCheckInDetailDTO>();
                if (customCheckInDTOList != null && customCheckInDTOList.Any() && comboCheckinQuantityList != null && comboCheckinQuantityList.Any())
                {
                    List<int> checkinIdList = comboCheckinQuantityList.Keys.ToList();
                    if (checkinIdList != null && checkinIdList.Any())
                    {
                        foreach (int checkinComboId in checkinIdList)
                        {
                            ComboCheckInDetailDTO customCheckInDetailDTO = new ComboCheckInDetailDTO();
                            customCheckInDetailDTO.CheckInProductId = checkinComboId;

                            int selectedQuantity = comboCheckinQuantityList[checkinComboId];
                            if (selectedQuantity == 0)
                                continue;

                            ProductsContainerDTO comboCheckinProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext.GetSiteId(), checkinComboId);
                            if (comboCheckinProductsContainerDTO != null && comboCheckinProductsContainerDTO.CustomerProfilingGroupContainerDTO != null
                                && (comboCheckinProductsContainerDTO.CustomerProfilingGroupContainerDTO.CustomerProfilingContainerDTOList != null
                                    && comboCheckinProductsContainerDTO.CustomerProfilingGroupContainerDTO.CustomerProfilingContainerDTOList.Any()))
                            {
                                Dictionary<int, int> productIdCheckInDetail = new Dictionary<int, int>();
                                CustomerProfilingGroupBL customerProfilingGroupBL = new CustomerProfilingGroupBL(Utilities.ExecutionContext, comboCheckinProductsContainerDTO.CustomerProfilingGroupId, true, true);
                                foreach (CustomCheckInDTO customCheckInDTO in customCheckInDTOList)
                                {
                                    // DOB validation can be null then add it to adults
                                    // if customer profile exists and DOB is not there then ask to enter
                                    // If picking records from relationship form then update the customer DOB
                                    // if year is 1900 then populate the customer screen to update the details
                                    // Age field will be populated on the grid
                                    decimal checkInDetailCustomerAge = 0;
                                    bool canAddCheckInDetails = false;
                                    if (customCheckInDTO.checkInDetailDTO.DateOfBirth.HasValue == false)
                                    {
                                        log.Debug("Profiling exists but date of birth is not entered. Please enter DOB");
                                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Please enter date of birth")), "Validation");
                                        btnCustomerDetails.PerformClick();
                                        return null;
                                    }
                                    if (customCheckInDTO.checkInDetailDTO.DateOfBirth.HasValue &&
                                        Convert.ToDateTime(customCheckInDTO.checkInDetailDTO.DateOfBirth).Year == 1900)
                                    {
                                        log.Debug("Profiling exists but Year of birth is not entered. Please enter DOB");
                                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Please enter year in date of birth")), "Validation");
                                        btnCustomerDetails.PerformClick();
                                        return null;
                                    }
                                    DateTime birthDate;
                                    if (DateTime.TryParse(customCheckInDTO.checkInDetailDTO.DateOfBirth.ToString(), out birthDate) == false)
                                    {
                                        log.Debug("Unable to convert the birthdate in to valid date");
                                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Unable to cast the birthdate")), "Validation");
                                        return null;
                                    }
                                    if (customCheckInDTO.checkInDetailDTO.Age > 0)
                                    {
                                        checkInDetailCustomerAge = customCheckInDTO.checkInDetailDTO.Age;
                                        log.Debug("CheckInDetailDTO.Age : " + customCheckInDTO.checkInDetailDTO.Age);
                                    }
                                    else
                                    {
                                        DateTime dateOfBirth = Convert.ToDateTime(customCheckInDTO.checkInDetailDTO.DateOfBirth);
                                        if (dateOfBirth != DateTime.MinValue &&
                                             ((DateTime.Today.Year - Convert.ToDateTime(dateOfBirth).Year) < 100) &&
                                             ((Convert.ToDateTime(dateOfBirth).Year) != 1900))
                                            checkInDetailCustomerAge = DateTime.Today.Year - Convert.ToDateTime(dateOfBirth).Year;
                                        log.Debug("checkInDetailCustomerAge: " + checkInDetailCustomerAge);
                                    }
                                    if (checkInDetailCustomerAge > 0)
                                    {
                                        if (checkInDetailCustomerAge >= comboCheckinProductsContainerDTO.AgeLowerLimit &&
                                               checkInDetailCustomerAge <= comboCheckinProductsContainerDTO.AgeUpperLimit)
                                        {
                                            canAddCheckInDetails = true;
                                        }
                                    }
                                    if (canAddCheckInDetails)
                                    {
                                        customCheckInDetailDTO.CheckInDetailDTOList.Add(customCheckInDTO.checkInDetailDTO);
                                    }
                                }

                                if (customCheckInDetailDTO.CheckInDetailDTOList.Count != selectedQuantity)
                                {
                                    //Get new message
                                    log.Debug("Entered quantity is less than the selected quantity");
                                    string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Guest date of birth entered does not match the profiling set up for the product - " + comboCheckinProductsContainerDTO.ProductName + ". Please check");
                                    POSUtils.ParafaitMessageBox(message, "Check In Count");
                                    return null;
                                }
                                result.Add(customCheckInDetailDTO);
                                log.LogVariableState("customCheckInDetailDTO", customCheckInDetailDTO);
                            }
                            else
                            {
                                if (selectedQuantity <= customCheckInDTOList.Count)
                                {
                                    for (int i = 0; i < selectedQuantity; i++)
                                    {
                                        var updatedCustomCheckInDTO = customCheckInDTOList[0];
                                        customCheckInDTOList.RemoveAt(0);
                                        customCheckInDetailDTO.CheckInProductId = checkinComboId;
                                        customCheckInDetailDTO.CheckInDetailDTOList.Add(updatedCustomCheckInDTO.checkInDetailDTO);
                                    }
                                    result.Add(customCheckInDetailDTO);
                                }
                                else
                                {
                                    // selected  quantity is more than the entered quantity
                                    log.Debug("Entered quantity is more than the selected quantity");
                                    string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "You have selected " + selectedQuantity + " quantities for " + comboCheckinProductsContainerDTO.ProductName + ". Please add  " + (selectedQuantity - customCheckInDTOList.Count) + " check in detail entries.");
                                    POSUtils.ParafaitMessageBox(message, "Check In Count");
                                    return null;
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        //Modified for check-in/out Pause changes
        private void refreshCheckIns(string statusQuery = "ALL")
        {
            log.Debug("Starts-refreshCheckIns()");//Added for logger function on 08-Mar-2016

            SqlCommand cmd = Utilities.getCommand();
            string where = "and 1 = 1 ";

            if (statusQuery != CheckInStatusFilter.ALL.ToString())
            {
                where = where + " and d.Status ='" + statusQuery + "'  ";
            }

            string customerQueryText = "CustomerView(@PassPhrase) c ";
            if (!string.IsNullOrWhiteSpace(searchFilter)
                && (searchFilter.Contains("address")
                    || searchFilter.Contains("phone")
                    || searchFilter.Contains("city")
                    || searchFilter.Contains("notes")))
            {
                customerQueryText = "CustomerView(@PassPhrase) c ";
            }
            else
            {
                customerQueryText = "Customers c ";
            }

            cmd.CommandText = "select top 100 c.customer_name + ' ' + isnull(last_name, '') Customer, cusCard.card_number card, d.name Detail, cd.card_number detail_card, " +
                            "d.CheckInTime check_in_time, CheckOutTime check_out_time,   d.Status CheckInStatus, cif.FacilityName facility_name,  " +
                            "datediff(\"mi\", d.CheckInTime, isnull(CheckoutTime, getdate())) Duration, " +
                            "isnull(Total_Pause_Time,0) Total_Pause_Time , " +
                            "isnull(AllowedTimeInMinutes, 0) time_allowed, " +
                            "case when datediff(\"mi\", d.CheckInTime, isnull(CheckoutTime, getdate())) > isnull(AllowedTimeInMinutes, 0) then null else isnull(AllowedTimeInMinutes, 0) - datediff(\"mi\", d.CheckInTime, isnull(CheckoutTime, getdate())) end balance_time, " +
                            "case when datediff(\"mi\", d.CheckInTime, isnull(CheckoutTime, getdate())) < isnull(AllowedTimeInMinutes, 0) then null else " +
                            "(datediff(\"mi\", d.CheckInTime, isnull(CheckoutTime, getdate())) - isnull(AllowedTimeInMinutes, 0)) - isnull(Total_Pause_Time,0) end overdue, " +
                            "TableName Table#, " +
                            "VehicleNumber Vehicle_Number, VehicleModel Model, VehicleColor Color, " +
                            "age, address1, address2, city, contact_phone1, contact_phone2, email, " +
                            "d.last_updated_user user_name, d.Remarks, " +
                            "h.PhotoFileName, c.customer_id, d.CheckinDetailId, h.CheckInId, h.CheckInFacilityId " +
                            "from CheckIns h left outer join cards cusCard on cusCard.card_id = h.cardId " +
                                 "left outer join CheckInFacility cif on h.CheckInFacilityId=cif.FacilityId " +
                                 "left outer join FacilityTables f on f.TableId = h.tableId, " +
                                 "checkInDetails d left outer join cards cd on cd.card_id = d.cardId " +
                                 "left outer join (select CheckInDetailId, sum(isnull(TotalPauseTime, 0)) Total_Pause_Time from CheckInPauseLog group by CheckInDetailId)" +
                                 " cp  on cp.CheckInDetailId = d.CheckInDetailId," +
                                 customerQueryText +
                            "where h.checkInId = d.CheckInId " +
                            "and (h.tableId = @tableId or @tableId is null) " +
                            "and (1 = @checkInPauseLog or h.CheckInFacilityId = @CheckInFacilityId )" +
                            "and c.customer_id = h.customer_id " +
                            where +
                            searchFilter +
                            "group by d.CheckinDetailId, c.customer_name ,last_name,cusCard.card_number," +
                            "d.name,cd.card_number,d.CheckInTime,CheckOutTime,d.Status , AllowedTimeInMinutes, " +
                            "TableName, VehicleNumber, VehicleModel, VehicleColor, age, address1, " +
                            "address2, city, contact_phone1, contact_phone2, email, d.last_updated_user , d.Remarks, " +
                            "h.PhotoFileName, c.customer_id , h.CheckInId, Total_Pause_Time, h.CheckInFacilityId, cif.FacilityName  " +
                            "order by d.CheckInTime desc";

            if (checkInDTO == null || checkInDTO.CustomerDTO == null)
                cmd.Parameters.AddWithValue("@customer_id", -1);
            else
                cmd.Parameters.AddWithValue("@customer_id", checkInDTO.CustomerDTO.Id);

            if (checkOutDetailDTO == null)
                cmd.Parameters.AddWithValue("@checkInPauseLog", 1);
            else
                cmd.Parameters.AddWithValue("@checkInPauseLog", 0);

            if (txtTableName.Tag != null)
                cmd.Parameters.AddWithValue("@tableId", txtTableName.Tag);
            else
                cmd.Parameters.AddWithValue("@tableId", DBNull.Value);

            cmd.Parameters.AddWithValue("@isCheckOutFlag", isCheckOut);
            cmd.Parameters.AddWithValue("@CheckInFacilityId", CheckInFacilityId);
            cmd.Parameters.AddWithValue("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE"));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvCheckedInList.DataSource = dt;
            dgvCheckedInList.Columns[dgvCheckedInList.Columns.Count - 1].Visible = false;
            dgvCheckedInList.Columns[dgvCheckedInList.Columns.Count - 2].Visible = false;
            dgvCheckedInList.Columns[dgvCheckedInList.Columns.Count - 3].Visible = false;
            dgvCheckedInList.Columns[dgvCheckedInList.Columns.Count - 4].Visible = false;
            dgvCheckedInList.Columns["CheckInFacilityId"].Visible = false;

            dgvCheckedInList.Columns["check_in_time"].DefaultCellStyle =
            dgvCheckedInList.Columns["check_out_time"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();

            dgvCheckedInList.Columns["duration"].DefaultCellStyle
            = dgvCheckedInList.Columns["time_allowed"].DefaultCellStyle
            = dgvCheckedInList.Columns["balance_time"].DefaultCellStyle
            = dgvCheckedInList.Columns["overdue"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();

            CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext);
            txtTotalCheckedIn.Text = checkInBL.GetTotalCheckedInForFacility(CheckInFacilityId, null).ToString();

            dgvCheckedInList.Columns["Total_Pause_Time"].DefaultCellStyle.Font = new Font(dgvCheckedInList.DefaultCellStyle.Font, FontStyle.Underline);
            dgvCheckedInList.Columns["Total_Pause_Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            RefreshGrid();

            log.LogMethodExit();
        }


        private void btnCapture_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCapture_Click()");//Added for logger function on 08-Mar-2016
            if (videoSource == null)
            {
                log.Info("Starts-btnCapture_Click() as videoSource == null");//Added for logger function on 08-Mar-2016
                return;
            }

            if (pbCapture.Tag != null)
            {
                pbCapture.Image = Properties.Resources.camera_icon_normal;
                pbCapture.Tag = null;
                videoSource.Start();
                return;
            }

            try
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                playSimpleSound();
                string photoFile = Guid.NewGuid().ToString();
                pictureBoxCamera.Tag = photoFile;
                if (pictureBoxCamera.Image != null)
                    pbCapture.Image = pictureBoxCamera.Image;
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(7, ex.Message), "Save Image");
                log.Fatal("Ends-btnCapture_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }

            pbCapture.Tag = "camera";
            log.Debug("Ends-btnCapture_Click()");//Added for logger function on 08-Mar-2016
        }

        private void playSimpleSound()
        {
            log.Debug("Starts-playSimpleSound()");//Added for logger function on 08-Mar-2016
            try
            {
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(Properties.Resources.camera_click);
                simpleSound.Play();
            }
            catch
            {
                log.Fatal("Ends-playSimpleSound() due to exception in simpleSound");//Added for logger function on 08-Mar-2016
            }
            log.Debug("Ends-playSimpleSound()");//Added for logger function on 08-Mar-2016
        }

        void clearTexts()
        {
            log.Debug("Starts-clearTexts()");//Added for logger function on 08-Mar-2016
            if (!grpCustomer.Enabled)
            {
                log.Info("Ends-clearTexts() as !grpCustomer.Enabled");//Added for logger function on 08-Mar-2016
                return;
            }

            txtAddress1.Text =
                txtAddress2.Text =
                txtCity.Text =
                txtCountry.Text =
                txtEmail.Text =
                txtMobile.Text =
                txtFirstName.Text =
                txtMiddleName.Text =
                txtLastName.Text =
                txtPhone.Text =
                txtPIN.Text =
                txtNotes.Text =
                txtState.Text =
                txtCardNumber.Text = "";

            setDGVUnits();

            pbCapture.Image = Properties.Resources.camera_icon_normal;
            pbCapture.Tag = "file";

            log.Debug("Ends-clearTexts()");//Added for logger function on 08-Mar-2016
        }

        void setDGVUnits()
        {
            log.Debug("Starts-setDGVUnits()");//Added for logger function on 08-Mar-2016
            dgvCheckInDetails.Rows.Clear();

            if (inUnits == 0 || inUnits == -1)
            {
                dgvCheckInDetails.AllowUserToAddRows = true;
            }
            else
            {
                dgvCheckInDetails.Rows.Add(inUnits);
                dgvCheckInDetails.AllowUserToAddRows = false;
            }

            pbCapture.Image = Properties.Resources.camera_icon_normal;
            pbCapture.Tag = "file";
            log.Debug("Ends-setDGVUnits()");//Added for logger function on 08-Mar-2016
        }

        private void dgvCheckedInList_SelectionChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dgvCheckedInList_SelectionChanged()");//Added for logger function on 08-Mar-2016
            if (isCheckOut == 0)
            {
                log.Info("Ends-dgvCheckedInList_SelectionChanged() as isCheckOut == 0");//Added for logger function on 08-Mar-2016
                return;
            }

            if (dgvCheckedInList.SelectedRows.Count > 0)
            {
                lblMessage.Text = "";

                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                }

                if (dgvCheckedInList.SelectedRows[0].Cells["PhotoFileName"].Value.ToString() != "")
                {
                    try
                    {
                        SqlCommand cmdImage = Utilities.getCommand();
                        cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";

                        string fileName = PhotoDirectory + dgvCheckedInList.SelectedRows[0].Cells["PhotoFileName"].Value.ToString();
                        cmdImage.Parameters.AddWithValue("@FileName", fileName);
                        object o = cmdImage.ExecuteScalar();
                        if (o != null)
                        {
                            byte[] imageInBytes = o as byte[];
                            try
                            {
                                imageInBytes = Encryption.Decrypt(imageInBytes);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                imageInBytes = o as byte[];
                            }
                            pictureBoxCamera.Image = Utilities.ConvertToImage(imageInBytes);
                            pbCapture.Tag = "file";
                        }
                    }
                    catch (Exception ex)
                    {
                        pictureBoxCamera.Image = null;
                        lblMessage.Text = ex.Message;
                        log.Fatal("Ends-dgvCheckedInList_SelectionChanged() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    }
                }
                else
                    pictureBoxCamera.Image = null;
            }
            log.Debug("Ends-dgvCheckedInList_SelectionChanged()");//Added for logger function on 08-Mar-2016
        }

        private void btnOpenSearch_Click(object sender, EventArgs e)
        {
            //log.Debug("Starts-btnOpenSearch_Click()");//Added for logger function on 08-Mar-2016
            //btnOpenSearch.BackColor = btnOpenSearch.FlatAppearance.MouseOverBackColor = Color.MediumBlue;
            //btnAllSearch.BackColor = btnAllSearch.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            //refreshCheckIns(CheckInStatusFilter.ALL.ToString());
            log.Debug("Ends-btnOpenSearch_Click()");//Added for logger function on 08-Mar-2016
        }

        private void btnAllSearch_Click(object sender, EventArgs e)
        {
            //log.Debug("Starts-btnAllSearch_Click()");//Added for logger function on 08-Mar-2016 
            //btnOpenSearch.BackColor = btnOpenSearch.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            //btnAllSearch.BackColor = btnAllSearch.FlatAppearance.MouseOverBackColor = Color.MediumBlue;
            //refreshCheckIns(CheckInStatusFilter.ALL.ToString());
            log.Debug("Ends-btnAllSearch_Click()");//Added for logger function on 08-Mar-2016
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClear_Click()");//Added for logger function on 08-Mar-2016
            clearTexts();
            searchFilter = "";
            log.Debug("Ends-btnClear_Click()");//Added for logger function on 08-Mar-2016
        }

        string searchFilter = "";
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSearch_Click()");//Added for logger function on 08-Mar-2016
            string cardNumberSearch = "";
            if (dgvCheckInDetails.Rows[0].Cells["detailCard"].Value != null)
                cardNumberSearch = "and cd.card_number = '" + dgvCheckInDetails.Rows[0].Cells["detailCard"].Value.ToString() + "' ";

            if (txtCardNumber.Text.Trim() != "") // card number available
            {
                if (txtFirstName.Text == "") // card holder not yet a customerDTO. either new card or detail card swiped in POS
                {
                    cardNumberSearch += "and (exists (select 1 from " +
                                                    "CheckInDetails cid, cards ca " +
                                                    "where ca.card_number = '" + txtCardNumber.Text + "' " +
                                                    "and cid.CheckInId = h.CheckInId " +
                                                    "and ca.card_id = cid.cardId " +
                                                    "and ca.valid_flag = 'Y')" +
                                            "or exists (select 1 " +
                                                    "from cards ca " +
                                                    "where ca.card_number = '" + txtCardNumber.Text + "' " +
                                                    "and ca.card_id = h.cardId " +
                                                    "and ca.valid_flag = 'Y'))";
                }
                else
                {
                    cardNumberSearch += "and (c.customer_id = @customer_id or " +
                                                    "(@isCheckOutFlag = 1 and " +
                                                        "(@customer_id = -1) or " +
                                                                "exists (select 1 from " +
                                                                        "CheckInDetails cid, cards ca " +
                                                                        "where ca.card_number = '" + txtCardNumber.Text + "' " +
                                                                        "and cid.CheckInId = h.CheckInId " +
                                                                        "and ca.card_id = cid.cardId " +
                                                                        "and ca.valid_flag = 'Y')))";
                }
            }
            else
                cardNumberSearch += "and c.customer_name like '%" + txtFirstName.Text + "%' " +
                                    "and isnull(c.last_name, '') like '%" + txtLastName.Text + "%' " +
                                    "and (c.customer_id = @customer_id or (@isCheckOutFlag = 1 and @customer_id = -1)) ";

            searchFilter = searchFilter + cardNumberSearch;
            if (dgvCheckInDetails.Rows[0].Cells["detailName"].Value != null)
                searchFilter = searchFilter + "and d.name like '%" + (dgvCheckInDetails.Rows[0].Cells["detailName"].Value.ToString()) + "%' ";

            if (string.IsNullOrWhiteSpace(txtCardNumber.Text.Trim()))
            {
                if (!string.IsNullOrWhiteSpace(txtAddress1.Text))
                    searchFilter = searchFilter + "and isnull(address1, '') like '%" + txtAddress1.Text + "%' ";
                if (!string.IsNullOrWhiteSpace(txtAddress2.Text))
                    searchFilter = searchFilter + "and isnull(address2, '') like '%" + txtAddress2.Text + "%' ";
                if (!string.IsNullOrWhiteSpace(txtCity.Text))
                    searchFilter = searchFilter + "and isnull(city, '') like '%" + txtCity.Text + "%' ";
                if (!string.IsNullOrWhiteSpace(txtPhone.Text))
                    searchFilter = searchFilter + "and isnull(contact_phone1, '') like '%" + txtPhone.Text + "%' ";
                if (!string.IsNullOrWhiteSpace(txtMobile.Text))
                    searchFilter = searchFilter + "and isnull(contact_phone2, '') like '%" + txtMobile.Text + "%' ";
                if (!string.IsNullOrWhiteSpace(txtNotes.Text))
                    searchFilter = searchFilter + "and isnull(c.notes, '') like '%" + txtNotes.Text + "%' ";
            }
            //searchFilter = "and d.name like '%" + (dgvCheckInDetails.Rows[0].Cells["detailName"].Value == null ? "" : dgvCheckInDetails.Rows[0].Cells["detailName"].Value.ToString()) + "%' " +
            //                cardNumberSearch +
            //                "and isnull(address1, '') like '%" + txtAddress1.Text + "%' " +
            //                "and isnull(address2, '') like '%" + txtAddress2.Text + "%' " +
            //                "and isnull(city, '') like '%" + txtCity.Text + "%' " +
            //                "and isnull(contact_phone1, '') like '%" + txtPhone.Text + "%' " +
            //                "and isnull(contact_phone2, '') like '%" + txtMobile.Text + "%' " +
            //                "and isnull(c.notes, '') like '%" + txtNotes.Text + "%' ";

            // Get selected status
            CheckInStatusFilter status;
            string selectedValue = ((KeyValuePair<string, string>)cmbCheckInStatus.SelectedItem).Key;
            Enum.TryParse<CheckInStatusFilter>(selectedValue, out status);
            string detailName = dgvCheckInDetails.Rows[0].Cells["detailName"].Value == null ? "" : dgvCheckInDetails.Rows[0].Cells["detailName"].Value.ToString();
            if (string.IsNullOrWhiteSpace(txtCardNumber.Text) &&
                string.IsNullOrWhiteSpace(txtFirstName.Text) &&
                string.IsNullOrWhiteSpace(detailName))
            {
                searchFilter = "";
            }
            refreshCheckIns(status.ToString());

            log.Debug("Ends-btnSearch_Click()");//Added for logger function on 08-Mar-2016
        }

        private void dgvCheckedInList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvCheckedInList_CellClick()");//Added for logger function on 08-Mar-2016
            if (e.RowIndex < 0)
            {
                log.Info("Ends-dgvCheckedInList_CellClick() as e.RowIndex < 0");//Added for logger function on 08-Mar-2016
                return;
            }

            if (e.ColumnIndex == 0)
            {
                checkInDTO = new CheckInBL(Utilities.ExecutionContext, Convert.ToInt32(dgvCheckedInList["CheckInId", e.RowIndex].Value), true, true).CheckInDTO;
                if (checkInDTO.CheckInDetailDTOList.Exists(x => x.Status == CheckInStatus.ORDERED))
                {
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Check In");
                }
                else if (checkInDTO.CheckInDetailDTOList.Exists(x => x.Status == CheckInStatus.PENDING))
                {
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Pending");
                }
                CheckOut(e.RowIndex);
            }

            //Modified for check-in/out Pause changes
            if (dgvCheckedInList.Columns[e.ColumnIndex].DataPropertyName == "Total_Pause_Time")
            {
                CheckInPauseLogUI checkInPauseLogUI = new CheckInPauseLogUI(Convert.ToInt32(dgvCheckedInList.CurrentRow.Cells["CheckInDetailId"].Value), Utilities, dgvCheckedInList.CurrentRow.Cells["Detail"].Value.ToString());
                checkInPauseLogUI.ShowDialog();
            }

            log.Debug("Ends-dgvCheckedInList_CellClick()");//Added for logger function on 08-Mar-2016
        }

        Button btn;

        //Modified for check-in/out Pause changes
        bool CheckOut(int RowIndex)
        {
            log.Debug("Starts-CheckOut(" + RowIndex + ")");
            CheckInStatusFilter status;
            Enum.TryParse<CheckInStatusFilter>(cmbCheckInStatus.SelectedValue.ToString(), out status);
            if (dgvCheckedInList["CheckInStatus", RowIndex].Value != null &&
                         Convert.ToString(dgvCheckedInList["CheckInStatus", RowIndex].Value) == CheckInStatusConverter.ToString(CheckInStatus.PENDING))
            {
                log.Error("Cannot perform check in process from Pending state");
                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4084);
                POSUtils.ParafaitMessageBox(message, "Check In Options");
                return false;
            }

            if (dgvCheckedInList["Check_Out_Time", RowIndex].Value == DBNull.Value
                || Convert.ToDateTime(dgvCheckedInList["Check_Out_Time", RowIndex].Value) > Utilities.getServerTime())
            {
                if (isCheckOut == 1)
                {
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Check Out");
                    if (Convert.ToString(dgvCheckedInList["CheckInStatus", RowIndex].Value) == CheckInStatusConverter.ToString(CheckInStatus.ORDERED))
                    {
                        log.Error("Not checked in to Check out. Please check in First");
                        return false;
                    }
                }
                else if (dgvCheckedInList["CheckInStatus", RowIndex].Value != null)
                {
                    if (Convert.ToString(dgvCheckedInList["CheckInStatus", RowIndex].Value) == CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN))
                    {
                        dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Pause/UnPause");
                    }
                    else if (Convert.ToString(dgvCheckedInList["CheckInStatus", RowIndex].Value) == CheckInStatusConverter.ToString(CheckInStatus.PENDING))
                    {
                        log.Error("Cannot perform check in process from Pending state");
                        string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4084);
                        POSUtils.ParafaitMessageBox(message, "Check In Options");
                        return false;
                    }
                    else if (Convert.ToString(dgvCheckedInList["CheckInStatus", RowIndex].Value) == CheckInStatusConverter.ToString(CheckInStatus.PAUSED))
                    {
                        log.Debug("PAUSED");
                        dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Pause/UnPause");
                    }
                    else if (Convert.ToString(dgvCheckedInList["CheckInStatus", RowIndex].Value) == CheckInStatusConverter.ToString(CheckInStatus.ORDERED))
                    {
                        log.Debug("PAUSED");
                        dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Check In");
                    }
                    else if (Convert.ToString(dgvCheckedInList["CheckInStatus", RowIndex].Value) == CheckInStatusConverter.ToString(CheckInStatus.CHECKEDOUT))
                    {
                        log.Error("Cannot perform check in process from Checked out state");
                        string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Cannot perform check in process from Checked out state");
                        POSUtils.ParafaitMessageBox(message, "Check In Options");
                        return false;
                    }
                }
                string checkInOptions = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CHECK_IN_OPTIONS_IN_POS");
                checkInDTO = new CheckInBL(Utilities.ExecutionContext, Convert.ToInt32(dgvCheckedInList["CheckInId", RowIndex].Value), true, true).CheckInDTO;
                int CheckedInCount = checkInDTO.CheckInDetailDTOList.Where(x => x.CheckOutTime == null && x.Status == CheckInStatus.CHECKEDIN
                                                                                     || (x.CheckOutTime != null && x.CheckOutTime > Utilities.getServerTime())).ToList().Count();
                bool paused = false;
                CheckInPauseLogListBL checkInPauseLogListBL = new CheckInPauseLogListBL(Utilities.ExecutionContext);
                List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, Convert.ToInt32(dgvCheckedInList["CheckinDetailId", RowIndex].Value).ToString()));
                searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
                List<CheckInPauseLogDTO> checkInPauseLogDTOList = checkInPauseLogListBL.GetCheckInPauseLogDTOList(searchParams);
                if (checkInPauseLogDTOList != null && checkInPauseLogDTOList.Count > 0)
                {
                    paused = true;
                }
                int pendingCheckInCount = 0;
                if (isCheckOut == 0 && checkInDTO.CheckInDetailDTOList.Exists(x => x.Status == CheckInStatus.ORDERED))
                {
                    pendingCheckInCount = checkInDTO.CheckInDetailDTOList.Where(x => x.Status == CheckInStatus.ORDERED).ToList().Count();
                }
                else if (dgvCheckedInList.Columns[0].HeaderText == MessageUtils.getMessage("Check In"))
                {
                    //return;
                }
                else if (dgvCheckedInList.Columns[0].HeaderText != MessageUtils.getMessage("Check Out"))
                {
                    dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Pause/UnPause");
                }
                int PausedCheckInCount = FetchCheckInPauseLogDetails(RowIndex, paused);

                if (dgvCheckedInList["Check_Out_Time", RowIndex].Value != DBNull.Value
                    && checkOutDetailDTO == null)
                {
                    return false;
                }
                if (dgvCheckedInList.Columns[0].HeaderText == MessageUtils.getMessage("Check In"))
                {
                    if (dgvCheckedInList["check_in_time", RowIndex].Value != null &&
                         dgvCheckedInList["CheckInStatus", RowIndex].Value != null &&
                         Convert.ToString(dgvCheckedInList["CheckInStatus", RowIndex].Value) == CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN))
                    {
                        string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Aleady Checked In");
                        dgvCheckedInList.Columns[0].HeaderText = MessageUtils.getMessage("Pause/UnPause");
                        CheckedInCount = 1;
                    }
                }
                if (dgvCheckedInList.Columns[0].HeaderText == MessageUtils.getMessage("Pause/UnPause")) //Pause mode
                {
                    checkInDTO = null;
                }
                if (pendingCheckInCount > 0 && dgvCheckedInList.Columns[0].HeaderText == MessageUtils.getMessage("Check In"))
                {
                    checkInDTO = new CheckInBL(Utilities.ExecutionContext, Convert.ToInt32(dgvCheckedInList["CheckInId", RowIndex].Value), true, true).CheckInDTO;
                }

                CheckOut chk = new CheckOut(dgvCheckedInList["detail", RowIndex].Value.ToString(), CheckedInCount, PausedCheckInCount, paused, checkInDTO == null ? null : checkInDTO, pendingCheckInCount);
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(Utilities.ExecutionContext);
                chk.setCallBack += this.CheckOutButtonSelected;
                DialogResult dr = chk.ShowDialog();
                chk.TopMost = true;
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    if (btn != null)
                    {
                        CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, Convert.ToInt32(dgvCheckedInList["CheckInId", RowIndex].Value), true, true);
                        List<CheckInDetailDTO> toBeUpdatedList = new List<CheckInDetailDTO>();
                        if (btn.Name == "btnSingle")// single
                        {
                            checkOutDetailDTO = checkInDTO.CheckInDetailDTOList.Find(x => x.CheckInDetailId == Convert.ToInt32(dgvCheckedInList["CheckInDetailId", RowIndex].Value));
                            if (checkOutDetailDTO == null)
                                return false;
                            if (POSStatic.HIDE_CHECK_IN_DETAILS)
                                checkOutDetailDTO.Detail = "";
                            else
                                checkOutDetailDTO.Detail = dgvCheckedInList["detail", RowIndex].Value.ToString();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            log.Info("Ends-CheckOut(" + RowIndex + ") as single");//Added for logger function on 08-Mar-2016
                            return true;
                        }
                        else if (btn.Name == "btnGroup") // group
                        {
                            checkOutDetailDTO = null;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            log.Info("Ends-CheckOut(" + RowIndex + ") as Group");//Added for logger function on 08-Mar-2016
                            return true;
                        }
                        else if (btn.Name == "btnCheckInSingle")// single
                        {
                            if (checkInOptions == "NO")
                            {
                                log.Error("POS check in option is set as 'NO' . Cannot check in from POS ");
                                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4083); //POS check in option is set as NO . Cannot check in from POS
                                POSUtils.ParafaitMessageBox(message, "Check In Options");
                                return false;
                            }
                            checkInDetailDTO = checkInDTO.CheckInDetailDTOList.Find(x => x.CheckInDetailId == Convert.ToInt32(dgvCheckedInList["CheckInDetailId", RowIndex].Value));
                            if (checkInDetailDTO == null)
                            {
                                return false;
                            }
                            checkInDetailDTO.Status = CheckInStatus.CHECKEDIN;
                            toBeUpdatedList.Add(checkInDetailDTO);
                            try
                            {
                                if (toBeUpdatedList.Any())
                                {
                                    using (NoSynchronizationContextScope.Enter())
                                    {
                                        Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, toBeUpdatedList);
                                        t.Wait();
                                    }
                                    List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> tagIssuedSearchParameters = new List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>>();
                                    tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                                    tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.CARDID, checkInDetailDTO.CardId.ToString()));
                                    NotificationTagIssuedListBL notificationTagIssuedListBL = new NotificationTagIssuedListBL(Utilities.ExecutionContext);
                                    List<NotificationTagIssuedDTO> notificationTagIssuedListDTO = notificationTagIssuedListBL.GetAllNotificationTagIssuedDTOList(tagIssuedSearchParameters);
                                    log.LogVariableState("notificationTagIssuedListDTO", notificationTagIssuedListDTO);
                                    NotificationTagIssuedDTO notificationTagIssuedDTO = null;
                                    if (notificationTagIssuedListDTO != null && notificationTagIssuedListDTO.Any()
                                                        && notificationTagIssuedListDTO.Exists(tag => tag.StartDate == DateTime.MinValue))
                                    {
                                        notificationTagIssuedListDTO = notificationTagIssuedListDTO.OrderBy(x => x.IssueDate).ToList(); // get latest record 
                                        notificationTagIssuedDTO = notificationTagIssuedListDTO.Where(tag => tag.StartDate == DateTime.MinValue
                                                                                           && (tag.ExpiryDate == DateTime.MinValue || tag.ExpiryDate > Utilities.getServerTime())).FirstOrDefault();
                                        log.LogVariableState("notificationTagIssuedDTO", notificationTagIssuedDTO);
                                        NotificationTagIssuedBL notificationTagIssuedBL = new NotificationTagIssuedBL(Utilities.ExecutionContext, notificationTagIssuedDTO);
                                        notificationTagIssuedBL.UpdateStartTime();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                return false;
                            }
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            log.Info("Ends-CheckIn(" + RowIndex + ") as single");//Added for logger function on 08-Mar-2016
                            return true;
                        }
                        else if (btn.Name == "btnCheckInGroup") // group
                        {
                            if (checkInOptions == "NO")
                            {
                                log.Error("POS check in option is set as 'NO' . Cannot check in from POS ");
                                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4083); //POS check in option is set as NO . Cannot check in from POS
                                POSUtils.ParafaitMessageBox(message, "Check In Options");
                                return false;
                            }
                            toBeUpdatedList = checkInDTO.CheckInDetailDTOList.Where(x => x.Status == CheckInStatus.ORDERED).ToList();
                            if (toBeUpdatedList == null || toBeUpdatedList.Any() == false)
                            {
                                return false;
                            }
                            this.DialogResult = DialogResult.OK;
                            this.Close();

                            foreach (CheckInDetailDTO checkInDetailDTO in toBeUpdatedList)
                            {
                                checkInDetailDTO.Status = CheckInStatus.CHECKEDIN;
                            }
                            try
                            {
                                if (toBeUpdatedList.Any())
                                {
                                    using (NoSynchronizationContextScope.Enter())
                                    {
                                        Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, toBeUpdatedList);
                                        t.Wait();
                                    }
                                    foreach (CheckInDetailDTO checkInDetailDTO in toBeUpdatedList)
                                    {
                                        List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> tagIssuedSearchParameters = new List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>>();
                                        tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                                        tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.CARDID, checkInDetailDTO.CardId.ToString()));
                                        NotificationTagIssuedListBL notificationTagIssuedListBL = new NotificationTagIssuedListBL(Utilities.ExecutionContext);
                                        List<NotificationTagIssuedDTO> notificationTagIssuedListDTO = notificationTagIssuedListBL.GetAllNotificationTagIssuedDTOList(tagIssuedSearchParameters);
                                        log.LogVariableState("notificationTagIssuedListDTO", notificationTagIssuedListDTO);
                                        NotificationTagIssuedDTO notificationTagIssuedDTO = null;
                                        if (notificationTagIssuedListDTO != null && notificationTagIssuedListDTO.Any()
                                                            && notificationTagIssuedListDTO.Exists(tag => tag.StartDate == DateTime.MinValue))
                                        {
                                            notificationTagIssuedListDTO = notificationTagIssuedListDTO.OrderBy(x => x.IssueDate).ToList(); // get latest record 
                                            notificationTagIssuedDTO = notificationTagIssuedListDTO.Where(tag => tag.StartDate == DateTime.MinValue
                                                                                               && (tag.ExpiryDate == DateTime.MinValue || tag.ExpiryDate > Utilities.getServerTime())).FirstOrDefault();
                                            log.LogVariableState("notificationTagIssuedDTO", notificationTagIssuedDTO);
                                            NotificationTagIssuedBL notificationTagIssuedBL = new NotificationTagIssuedBL(Utilities.ExecutionContext, notificationTagIssuedDTO);
                                            notificationTagIssuedBL.UpdateStartTime();
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                return false;
                            }
                            log.Info("Ends-CheckOut(" + RowIndex + ") as Group");//Added for logger function on 08-Mar-2016
                            return true;
                        }
                        else if (btn.Name == "btnPauseSingle")
                        {
                            CheckInDetailBL checkInDetailBL = new CheckInDetailBL(Utilities.ExecutionContext, Convert.ToInt32(dgvCheckedInList["CheckinDetailId", RowIndex].Value));
                            checkInDTO = checkInBL.CheckInDTO;

                            if (!paused)
                            {
                                if (checkInAttribute == ProductsContainerDTO.PauseUnPauseType.NONE)
                                {
                                    CheckInPauseLogDTO checkInPauseLogDTO = new CheckInPauseLogDTO();
                                    checkInPauseLogDTO.CheckInDetailId = Convert.ToInt32(dgvCheckedInList["CheckinDetailId", RowIndex].Value);
                                    checkInPauseLogDTO.PauseStartTime = Utilities.getServerTime();
                                    checkInPauseLogDTO.PausedBy = Utilities.ParafaitEnv.Username;
                                    checkInPauseLogDTO.POSMachine = Utilities.ParafaitEnv.POSMachine;

                                    CheckInPauseLogBL checkInPauseLogBL = new CheckInPauseLogBL(Utilities.ExecutionContext, checkInPauseLogDTO);
                                    checkInPauseLogBL.Save();

                                    // Change status to Paused
                                    checkInDetailBL.CheckInDetailDTO.Status = CheckInStatus.PAUSED;
                                    toBeUpdatedList.Add(checkInDetailBL.CheckInDetailDTO);
                                }
                                checkInDTO.CheckInDetailDTOList.Clear();
                                checkInDTO.CheckInDetailDTOList.Add(checkInDetailBL.CheckInDetailDTO);

                                if (dgvCheckedInList.Rows[RowIndex].Tag == null ||
                                    dgvCheckedInList.Rows[RowIndex].Tag.ToString() != Color.Orange.Name)
                                    dgvCheckedInList.Rows[RowIndex].Tag = dgvCheckedInList.Rows[RowIndex].DefaultCellStyle.BackColor.Name;

                                dgvCheckedInList.Rows[RowIndex].DefaultCellStyle.BackColor = Color.Orange;
                                this.dgvCheckedInList.ClearSelection();
                                if (this.dgvCheckedInList.RowCount <= 0)
                                    btnCustomerDetails.Enabled = false;
                            }
                            else
                            {
                                if (checkInAttribute == ProductsContainerDTO.PauseUnPauseType.NONE)
                                {
                                    CheckInPauseLogListBL checkInPauseLogList = new CheckInPauseLogListBL(Utilities.ExecutionContext);
                                    List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchCheckInPauseLogParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                                    searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, Convert.ToInt32(dgvCheckedInList["CheckinDetailId", RowIndex].Value).ToString()));
                                    searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
                                    List<CheckInPauseLogDTO> listCheckInPauseLogDTO = checkInPauseLogList.GetCheckInPauseLogDTOList(searchCheckInPauseLogParams);
                                    if (listCheckInPauseLogDTO != null && listCheckInPauseLogDTO.Count > 0)
                                    {
                                        DateTime serverDateTime = Utilities.getServerTime();
                                        listCheckInPauseLogDTO[0].PauseEndTime = serverDateTime;
                                        listCheckInPauseLogDTO[0].TotalPauseTime = Convert.ToInt32((serverDateTime - listCheckInPauseLogDTO[0].PauseStartTime).TotalMinutes);
                                        listCheckInPauseLogDTO[0].UnPausedBy = Utilities.ParafaitEnv.Username;
                                        CheckInPauseLogBL checkInPauseLogBL = new CheckInPauseLogBL(Utilities.ExecutionContext, listCheckInPauseLogDTO[0]);
                                        checkInPauseLogBL.Save();

                                        //Unpause then back to checked In 
                                        checkInDetailBL.CheckInDetailDTO.Status = CheckInStatus.CHECKEDIN;
                                        toBeUpdatedList.Add(checkInDetailBL.CheckInDetailDTO);
                                    }
                                }
                                checkInDTO.CheckInDetailDTOList.Clear();
                                checkInDTO.CheckInDetailDTOList.Add(checkInDetailBL.CheckInDetailDTO);
                            }
                            try
                            {
                                if (toBeUpdatedList.Any() && checkInAttribute == ProductsContainerDTO.PauseUnPauseType.NONE)
                                {
                                    using (NoSynchronizationContextScope.Enter())
                                    {
                                        Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, toBeUpdatedList);
                                        t.Wait();
                                        string selectedValue = ((KeyValuePair<string, string>)cmbCheckInStatus.SelectedItem).Key;
                                        Enum.TryParse<CheckInStatusFilter>(selectedValue, out status);
                                        log.Debug("selectedValue :" + selectedValue);
                                        log.Debug("status :" + status);
                                        refreshCheckIns(status.ToString());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                return false;
                            }
                        }
                        else if (btn.Name == "btnPauseGroup")
                        {
                            checkInDTO = checkInBL.CheckInDTO;
                            checkInDTO.CheckInDetailDTOList.Clear();
                            if (!paused)
                            {
                                DataTable dataTable = Utilities.executeDataTable(@"select * from CheckInDetails where CheckInId = @CheckInId",
                                                                                    new SqlParameter[] { new SqlParameter("@CheckInId", Convert.ToInt32(dgvCheckedInList["CheckInId", RowIndex].Value)) });
                                if (dataTable.Rows.Count > 0)
                                {
                                    foreach (DataRow checkInPauseLogDataRow in dataTable.Rows)
                                    {
                                        CheckInDetailBL checkInDetailBL = new CheckInDetailBL(Utilities.ExecutionContext, Convert.ToInt32(checkInPauseLogDataRow["CheckinDetailId"]));

                                        if (checkInAttribute == ProductsContainerDTO.PauseUnPauseType.NONE)
                                        {
                                            CheckInPauseLogListBL checkInPauseLogList = new CheckInPauseLogListBL(Utilities.ExecutionContext);
                                            List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchCheckInPauseLogParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                                            searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, Convert.ToInt32(checkInPauseLogDataRow["CheckInDetailId"]).ToString()));
                                            List<CheckInPauseLogDTO> listCheckInPauseLogDTO = checkInPauseLogList.GetCheckInPauseLogDTOList(searchCheckInPauseLogParams);
                                            if (listCheckInPauseLogDTO != null && listCheckInPauseLogDTO.Count > 0)
                                            {
                                                CheckInPauseLogDTO tempCheckInPauseLogDTO = listCheckInPauseLogDTO.OrderByDescending(x => x.PauseStartTime).First();

                                                if (tempCheckInPauseLogDTO.PauseEndTime != null)
                                                {
                                                    CheckInPauseLogDTO checkInPauseLogDTO = new CheckInPauseLogDTO();
                                                    checkInPauseLogDTO.CheckInDetailId = Convert.ToInt32(checkInPauseLogDataRow["CheckInDetailId"]);
                                                    checkInPauseLogDTO.PauseStartTime = Utilities.getServerTime();
                                                    checkInPauseLogDTO.POSMachine = Utilities.ParafaitEnv.POSMachine;
                                                    checkInPauseLogDTO.PausedBy = Utilities.ParafaitEnv.Username;

                                                    CheckInPauseLogBL checkInPauseLogBL = new CheckInPauseLogBL(Utilities.ExecutionContext, checkInPauseLogDTO);
                                                    checkInPauseLogBL.Save();
                                                    checkInDetailBL.CheckInDetailDTO.Status = CheckInStatus.PAUSED;
                                                    toBeUpdatedList.Add(checkInDetailBL.CheckInDetailDTO);
                                                }
                                            }
                                            else
                                            {
                                                CheckInPauseLogDTO checkInPauseLogDTO = new CheckInPauseLogDTO();
                                                checkInPauseLogDTO.CheckInDetailId = Convert.ToInt32(checkInPauseLogDataRow["CheckInDetailId"]);
                                                checkInPauseLogDTO.PauseStartTime = Utilities.getServerTime();
                                                checkInPauseLogDTO.POSMachine = Utilities.ParafaitEnv.POSMachine;
                                                checkInPauseLogDTO.PausedBy = Utilities.ParafaitEnv.Username;

                                                CheckInPauseLogBL checkInPauseLogBL = new CheckInPauseLogBL(Utilities.ExecutionContext, checkInPauseLogDTO);
                                                checkInPauseLogBL.Save();

                                                checkInDetailBL.CheckInDetailDTO.Status = CheckInStatus.PAUSED;
                                                toBeUpdatedList.Add(checkInDetailBL.CheckInDetailDTO);
                                            }
                                        }
                                        checkInDTO.CheckInDetailDTOList.Add(checkInDetailBL.CheckInDetailDTO);
                                    }
                                    // changes status to Paused 
                                    try
                                    {
                                        if (toBeUpdatedList.Any() && checkInAttribute == ProductsContainerDTO.PauseUnPauseType.NONE)
                                        {
                                            using (NoSynchronizationContextScope.Enter())
                                            {
                                                Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, toBeUpdatedList);
                                                t.Wait();
                                                string selectedValue = ((KeyValuePair<string, string>)cmbCheckInStatus.SelectedItem).Key;
                                                Enum.TryParse<CheckInStatusFilter>(selectedValue, out status);
                                                log.Debug("selectedValue :" + selectedValue);
                                                log.Debug("status :" + status);
                                                refreshCheckIns(status.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        return false;
                                    }

                                    if (dgvCheckedInList.Rows.Count > 0)
                                    {
                                        foreach (DataGridViewRow row in dgvCheckedInList.Rows)
                                        {
                                            if (row.Tag == null || row.Tag.ToString() != Color.Orange.Name)
                                            {
                                                row.Tag = dgvCheckedInList.Rows[RowIndex].DefaultCellStyle.BackColor.Name;
                                            }
                                            try
                                            {
                                                if (row.Cells[Status.Index].Value != null &&
                                                        row.Cells[Status.Index].Value.ToString() == CheckInStatusFilter.PAUSED.ToString())
                                                {
                                                    row.DefaultCellStyle.BackColor = Color.Orange;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error(ex);
                                            }
                                        }
                                    }
                                    this.dgvCheckedInList.ClearSelection();
                                    if (this.dgvCheckedInList.RowCount <= 0)
                                        btnCustomerDetails.Enabled = false;

                                }
                                return true;
                            }
                            else
                            {
                                DataTable dataTable = Utilities.executeDataTable(@"select * from CheckInDetails where CheckInId = @CheckInId",
                                                                                    new SqlParameter[] { new SqlParameter("@CheckInId", Convert.ToInt32(dgvCheckedInList["CheckInId", RowIndex].Value)) });
                                if (dataTable.Rows.Count > 0)
                                {
                                    foreach (DataRow checkInPauseLogDataRow in dataTable.Rows)
                                    {
                                        CheckInDetailBL checkInDetailBL = new CheckInDetailBL(Utilities.ExecutionContext, Convert.ToInt32(checkInPauseLogDataRow["CheckinDetailId"]));
                                        if (checkInAttribute == ProductsContainerDTO.PauseUnPauseType.NONE)
                                        {
                                            CheckInPauseLogListBL checkInPauseLogList = new CheckInPauseLogListBL(Utilities.ExecutionContext);
                                            List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchCheckInPauseLogParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                                            searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, Convert.ToInt32(checkInPauseLogDataRow["CheckInDetailId"]).ToString()));
                                            List<CheckInPauseLogDTO> listCheckInPauseLogDTO = checkInPauseLogList.GetCheckInPauseLogDTOList(searchCheckInPauseLogParams);
                                            if (listCheckInPauseLogDTO != null && listCheckInPauseLogDTO.Count > 0)
                                            {
                                                CheckInPauseLogDTO tempCheckInPauseLogDTO = listCheckInPauseLogDTO.OrderByDescending(x => x.PauseStartTime).First();

                                                if (tempCheckInPauseLogDTO.PauseEndTime == null)
                                                {
                                                    checkInPauseLogList = new CheckInPauseLogListBL(Utilities.ExecutionContext);
                                                    searchCheckInPauseLogParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                                                    searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, Convert.ToInt32(checkInPauseLogDataRow["CheckInDetailId"]).ToString()));
                                                    searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
                                                    listCheckInPauseLogDTO = checkInPauseLogList.GetCheckInPauseLogDTOList(searchCheckInPauseLogParams);
                                                    if (listCheckInPauseLogDTO != null && listCheckInPauseLogDTO.Count > 0)
                                                    {
                                                        DateTime serverDateTime = Utilities.getServerTime();
                                                        listCheckInPauseLogDTO[0].PauseEndTime = serverDateTime;
                                                        listCheckInPauseLogDTO[0].TotalPauseTime = Convert.ToInt32((serverDateTime - listCheckInPauseLogDTO[0].PauseStartTime).TotalMinutes);
                                                        listCheckInPauseLogDTO[0].UnPausedBy = Utilities.ParafaitEnv.Username;
                                                        CheckInPauseLogBL checkInPauseLogBL = new CheckInPauseLogBL(Utilities.ExecutionContext, listCheckInPauseLogDTO[0]);
                                                        checkInPauseLogBL.Save();
                                                    }
                                                }
                                            }
                                            checkInDetailBL.CheckInDetailDTO.Status = CheckInStatus.CHECKEDIN;
                                            toBeUpdatedList.Add(checkInDetailBL.CheckInDetailDTO);
                                        }
                                        checkInDTO.CheckInDetailDTOList.Add(checkInDetailBL.CheckInDetailDTO);
                                    }
                                    //Change status from Unpaused to Checked IN
                                    try
                                    {
                                        if (toBeUpdatedList.Any() && checkInAttribute == ProductsContainerDTO.PauseUnPauseType.NONE)
                                        {
                                            using (NoSynchronizationContextScope.Enter())
                                            {
                                                Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, toBeUpdatedList);
                                                t.Wait();
                                                string selectedValue = ((KeyValuePair<string, string>)cmbCheckInStatus.SelectedItem).Key;
                                                Enum.TryParse<CheckInStatusFilter>(selectedValue, out status);
                                                log.Debug("selectedValue :" + selectedValue);
                                                log.Debug("status :" + status);
                                                refreshCheckIns(status.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                    }
                    return false;
                }
                else
                {
                    log.Info("Ends-CheckOut(" + RowIndex + ") as not Single/Group");//Added for logger function on 08-Mar-2016
                    return false;
                }
            }
            else
            {
                log.Debug("Ends-CheckOut(" + RowIndex + ")");//Added for logger function on 08-Mar-2016
                log.Error("Cannot perform check in process from Checked out state");
                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Already Checked out");
                POSUtils.ParafaitMessageBox(message, "Check Out");
                return false;
            }
        }

        private void RefreshGrid()
        {
            if (dgvCheckedInList.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvCheckedInList.Rows)
                {
                    object pauseEndTime = Utilities.executeScalar(@"select top 1 isnull(PauseEndTime,null) from CheckInPauseLog 
                                                    where CheckInDetailId = @CheckInDetailId 
                                                    order by PauseStartTime desc",
                                                        new SqlParameter("@CheckInDetailId", Convert.ToInt32(dgvCheckedInList["CheckinDetailId", row.Index].Value)));
                    if (pauseEndTime == DBNull.Value
                        && dgvCheckedInList["CheckInStatus", row.Index].Value != null
                        && dgvCheckedInList["CheckInStatus", row.Index].Value.ToString() == CheckInStatusFilter.PAUSED.ToString())
                    {
                        dgvCheckedInList.Rows[row.Index].DefaultCellStyle.BackColor = Color.Orange;
                    }
                }
            }

            dgvCheckedInList.ClearSelection();
        }

        private int FetchCheckInPauseLogDetails(int RowIndex, bool paused)
        {
            int count = -1;
            StringBuilder stringBuilder = new StringBuilder();

            int i = 0;
            foreach (DataRow dr in Utilities.executeDataTable(@"select * from CheckInDetails where CheckInId = @CheckInId",
                                                                   new SqlParameter[] { new SqlParameter("@CheckInId", Convert.ToInt32(dgvCheckedInList["CheckInId", RowIndex].Value)) }).Rows)
            {
                if (i > 0)
                    stringBuilder.Append(",");
                stringBuilder.Append("'");
                stringBuilder.Append(dr["CheckInDetailId"].ToString());
                stringBuilder.Append("'");
                i++;
            }

            CheckInPauseLogListBL checkInPauseLogListBL = new CheckInPauseLogListBL(Utilities.ExecutionContext);
            List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
            searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID_LIST, stringBuilder.ToString()));
            searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
            List<CheckInPauseLogDTO> checkInPauseLogDTOList = checkInPauseLogListBL.GetCheckInPauseLogDTOList(searchParams);
            if (checkInPauseLogDTOList != null && checkInPauseLogDTOList.Count > 0)
            {
                count = checkInPauseLogDTOList.Count;
            }

            if (count > 0 && count != i && !paused)
                count = i - count;
            return count;
        }

        void CheckOutButtonSelected(object sender)
        {
            btn = sender as Button;
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            //log.Debug("Starts-txtName_Leave()");//Added for logger function on 08-Mar-2016
            this.ActiveControl = dgvCheckInDetails;
            //log.Debug("Ends-txtName_Leave()");//Added for logger function on 08-Mar-2016
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click()");//Added for logger function on 08-Mar-2016
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.Debug("Ends-btnClose_Click()");//Added for logger function on 08-Mar-2016
        }

        private void CheckIn_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.Debug("Starts-CheckIn_FormClosed()");//Added for logger function on 08-Mar-2016
            Common.Devices.UnregisterCardReaders();

            //Stop and free the webcam object if application is closing
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource = null;
            }

            log.Debug("Ends-CheckIn_FormClosed()");//Added for logger function on 08-Mar-2016
        }

        private void dgvCheckInDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvCheckInDetails_CellContentClick()");//Added for logger function on 08-Mar-2016
            try
            {
                if (e.ColumnIndex == 0)
                {
                    int detailId = dgvCheckInDetails.Rows[e.RowIndex].Cells[CheckIndetailId.Index].Value == null ? -1 : Convert.ToInt32(dgvCheckInDetails.Rows[e.RowIndex].Cells[CheckIndetailId.Index].Value);
                    if (detailId > -1)
                    {
                        log.Debug("Cannot delete saved Check In detail records ");
                        return;
                    }
                    else
                    {
                        if (customerDTOList != null && customerDTOList.Any())
                        {
                            var itemToRemove = customerDTOList.Single(r => r.Id == (dgvCheckInDetails.Rows[e.RowIndex].Cells[customerIdDataGridViewTextBoxColumn.Index].Value == null ? -1 : Convert.ToInt32(dgvCheckInDetails.Rows[e.RowIndex].Cells[customerIdDataGridViewTextBoxColumn.Index].Value)));
                            customerDTOList.Remove(itemToRemove);
                        }
                        if (inUnits > 0)
                        {
                            Transaction.TransactionLine trxLine = (Transaction.TransactionLine)dgvCheckInDetails.Rows[e.RowIndex].Cells[Line.Index].Value;
                            if (trxLine != null)
                            {
                                trxLine.CancelledLine = true;
                            }
                            for (int i = 0; i < dgvCheckInDetails.Columns.Count; i++)
                            {
                                dgvCheckInDetails.Rows[e.RowIndex].Cells[i].Value = null;
                            }

                        }
                        else
                        {

                            Transaction.TransactionLine trxLine = (Transaction.TransactionLine)dgvCheckInDetails.Rows[e.RowIndex].Cells[Line.Index].Value;
                            if (trxLine != null)
                            {
                                trxLine.CancelledLine = true;
                            }
                            dgvCheckInDetails.Rows.RemoveAt(e.RowIndex);
                            //--inUnits;
                        }
                    }
                }
            }
            catch
            {
                log.Info("Ends-dgvCheckInDetails_CellContentClick() due to exception");//Added for logger function on 08-Mar-2016               
            }

            log.Debug("Ends-dgvCheckInDetails_CellContentClick()");//Added for logger function on 08-Mar-2016
        }

        private void dgvCheckInDetails_Enter(object sender, EventArgs e)
        {
            //dgvCheckInDetails.CurrentCell = dgvCheckInDetails[1, 0];
        }

        private void txtFirstName_Validated(object sender, EventArgs e)
        {
            log.Debug("Starts-txtFirstName_Validated()");//Added for logger function on 08-Mar-2016
            if (btnCheckIn.Enabled)
            {
                if (dgvCheckInDetails["DetailName", 0].Value == null || dgvCheckInDetails["DetailName", 0].Value.ToString().Trim() == "")
                {
                    dgvCheckInDetails["DetailName", 0].Value = txtFirstName.Text.Trim() + (txtLastName.Text.Trim() == "" ? "" : " " + txtLastName.Text.Trim());
                }
            }
            log.Debug("Ends-txtFirstName_Validated()");//Added for logger function on 08-Mar-2016
        }

        private void dgvCheckedInList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvCheckedInList_CellDoubleClick()");//Added for logger function on 08-Mar-2016
            if (e.RowIndex < 0)
            {
                log.Info("Ends-dgvCheckedInList_CellDoubleClick() as e.RowIndex < 0 ");//Added for logger function on 08-Mar-2016
                return;
            }

            CheckOut(e.RowIndex);
            log.Debug("Ends-dgvCheckedInList_CellDoubleClick()");//Added for logger function on 08-Mar-2016
        }

        public int getCheckedInCount()
        {
            btnSearch_Click(null, null);
            return dgvCheckedInList.Rows.Count;
        }

        public bool doCheckOut()
        {
            return CheckOut(0);
        }


        private void btnCustomerDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerDTO customerDTO;
            if (checkInDTO == null)
            {
                customerDTO = new CustomerDTO();
                if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMERTYPE") != "N")
                {
                    customerDTO.CustomerType = CustomerType.UNREGISTERED;
                }
            }
            else
            {
                customerDTO = checkInDTO.CustomerDTO;
            }
            using (CustomerDetailForm customerDetailForm = new CustomerDetailForm(Utilities, customerDTO, POSUtils.ParafaitMessageBox, ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD")))
            {
                customerDetailForm.MinimizeBox = customerDetailForm.MaximizeBox = false;
                DialogResult dr = customerDetailForm.ShowDialog();
                if (dr == DialogResult.OK && (!string.IsNullOrEmpty(checkInOut) && checkInOut != "CHECK-OUT"))
                {
                    if (inCard != null &&
                        customerDetailForm.CustomerDTO != null &&
                        customerDetailForm.CustomerDTO.CustomerType == CustomerType.REGISTERED)
                    {
                        inCard.customerDTO = customerDetailForm.CustomerDTO;
                    }

                    bool showCustomerRelationUI = false;
                    if (checkInDTO.CustomerDTO != null && checkInDTO.CustomerDTO.Id > 0)
                        showCustomerRelationUI = true;
                    checkInDTO.CustomerDTO = customerDetailForm.CustomerDTO;
                    populateCustomerDetails();
                    if (showCustomerRelationUI)
                        SelectRelatedCustomers(checkInDTO.CustomerDTO);
                }
                if (dr == DialogResult.OK && (string.IsNullOrEmpty(checkInOut) || checkInOut == "CHECK-OUT"))//Modified for check-in/out Pause changes
                {
                    if (checkInDTO != null)
                    {
                        checkInDTO.CustomerDTO = customerDetailForm.CustomerDTO;
                    }
                    refreshCheckIns();

                    if (dgvCheckedInList.RowCount > 0 && dgvCheckedInList["CheckInFacilityId", 0].Value != DBNull.Value)
                    {
                        FacilityDTO facilityDTO = new FacilityBL(Utilities.ExecutionContext, Convert.ToInt32(dgvCheckedInList["CheckInFacilityId", 0].Value)).FacilityDTO;
                        txtCapacity.Text = facilityDTO.Capacity == null ? "0" : facilityDTO.Capacity.ToString();
                        lblFacility.Text = facilityDTO.FacilityName;
                        CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext);
                        txtTotalCheckedIn.Text = checkInBL.GetTotalCheckedInForFacility(Convert.ToInt32(dgvCheckedInList["CheckInFacilityId", 0].Value), null).ToString();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CheckIn_Shown(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            vsbCheckInDetails.UpdateButtonStatus();
            hsbCheckInDetails.UpdateButtonStatus();
            hsbCheckedInList.UpdateButtonStatus();
            vsbCheckedInList.UpdateButtonStatus();
            if (checkInOut == "CHECK-IN")
            {
                if (checkInDTO.CustomerDTO != null && checkInDTO.CustomerDTO.Id < 0)
                {
                    btnCustomerDetails.PerformClick();
                }
            }

            if (checkInDTO.CustomerDTO != null && checkInOut == "CHECK-IN")
            {
                SelectRelatedCustomers(checkInDTO.CustomerDTO);
            }
            log.LogMethodExit();
        }

        private void dgvCheckInDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            vsbCheckInDetails.UpdateButtonStatus();
        }

        private void dgvCheckInDetails_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            vsbCheckInDetails.UpdateButtonStatus();
        }
    }
}
