/********************************************************************************************
 * Project Name - frm Locker Card Utils
 * Description  - User interface for frm Locker Card Utils
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Apr-2017   Raghuveera    Created 
 *2.70.2      12-Aug-2019   Deeksha       Added logger methods.
 *2.150.3     30-Jun-2023   Abhishek      Modified to get card details associated with Hecere locker.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// User interface for locker utility
    /// </summary>
    public partial class frmLockerCardUtils : Form
    {
        ParafaitLockCardHandler parafaitLockCardHandler;
        Utilities utilities;
        DeviceClass mifareReader;
        bool disposeInTheEnd = false;
        string approvedByLogin;
        private static readonly  Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<LockerZonesDTO> lockerZonesDTOList;
        /// <summary>
        /// Constructor with parameter
        /// </summary>        
        public frmLockerCardUtils(DeviceClass mifareReader,  Utilities inUtilities) : this(mifareReader, inUtilities, -1)
        {
            log.LogMethodEntry(mifareReader, inUtilities);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>   
        public frmLockerCardUtils(DeviceClass mifareReader,  Utilities inUtilities, int ApprovedBy)
        {
            log.LogMethodEntry(mifareReader, inUtilities, ApprovedBy);
            InitializeComponent();
            utilities = inUtilities;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);//added on 26-Jul-2017
            if (ApprovedBy != -1)
            {
                UsersList usersList = new UsersList(machineUserContext);
                List<UsersDTO> usersDTOList = new List<UsersDTO>();
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> userSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                userSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, ApprovedBy.ToString()));
                usersDTOList = usersList.GetAllUsers(userSearchParams);
                if (usersDTOList != null && usersDTOList.Count > 0)
                {
                    approvedByLogin = usersDTOList[0].LoginId;
                }
            }

            this.mifareReader = mifareReader;

            dtPickerFromDate.CustomFormat = "MMM dd, yyyy - HH:mm:ss";
            dtPickerToDate.CustomFormat = "MMM dd, yyyy - HH:mm:ss";
            dtPickerToDate.Value = inUtilities.getServerTime().AddDays(1).Date;
            lblLockerMake.Text = inUtilities.getParafaitDefaults("LOCKER_LOCK_MAKE");
            lockerModeFixedMode.Checked = inUtilities.getParafaitDefaults("LOCKER_SELECTION_MODE").Equals(ParafaitLockCardHandlerDTO.LockerSelectionMode.FIXED.ToString()) | lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString());
            lockerModeFreeMode.Checked = !lockerModeFixedMode.Checked;
            lockerModeFixedMode.Enabled = lockerModeFreeMode.Enabled = !lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString());
            btnCreateSettingCard.Visible = !lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString());
            if (lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))//Modification on 27-03-2017 PassTech integration
            {
                groupBox1.Enabled =
                btnParameterCard.Enabled =
                btnBlockCard.Enabled = false;
                chbEnableInAllZone.Visible = true;
            }//Modification on 27-03-2017 PassTech integration

            if (mifareReader == null)
            {
                registerMifareDevice();
                disposeInTheEnd = true;
            }

            if (mifareReader != null)
            {
                EventHandler mifareCardScanCompleteEvent = new EventHandler(MifareCardScanCompleteEventHandle);
                mifareReader.Register(mifareCardScanCompleteEvent);
                lnkResetMifareReader.Enabled = false;
            }

            log.LogMethodExit();
        }

        private void MifareCardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string cardNumber = checkScannedEvent.Message;

                log.LogVariableState("cardNumber", cardNumber);

                HandleCardRead(cardNumber);
            }

            log.LogMethodExit();
        }

        private bool registerMifareDevice()
        {
            log.LogMethodEntry();

            int deviceAddress = 0;
            bool response;
            string message = "";

            if (mifareReader != null)
            {
                mifareReader.Dispose();
            }

            response = true;
            try
            {
                mifareReader = new ACR122U();
            }
            catch (Exception e)
            {
                log.Error("Error occured while reading from the ACR Reader", e);

                try
                {
                    mifareReader = new MIBlack(deviceAddress);
                }
                catch (Exception e1)
                {
                    log.Error("Failed to create mifareReader", e1);
                    response = false;
                    log.LogVariableState("response", response);
                }
            }

            if (response)
            {
                log.LogVariableState("response", response);

                EventHandler mifareCardScanCompleteEvent = new EventHandler(MifareCardScanCompleteEventHandle);
                mifareReader.Register(mifareCardScanCompleteEvent);
            }

            txtMessage.Text = message;

            log.LogMethodExit(response);
            return response;
        }

        void HandleCardRead(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);

            try
            {
                Card card = new Card(mifareReader, cardNumber, utilities.ParafaitEnv.LoginID, utilities);
                LockerAllocationDTO lockerAllocationDTO;
                LockerPanel lockerPanel;
                Locker lockerBL;
                txtCardNumber.Text = cardNumber;
                if (lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.COCY.ToString()))
                    parafaitLockCardHandler = new CocyLockCardHandler(card.ReaderDevice, utilities.ExecutionContext, Convert.ToByte(utilities.ParafaitEnv.MifareCustomerKey));
                else if (lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))//Modification on 28-Mar-2017 for PassTech
                {
                    parafaitLockCardHandler = new PassTechLockCardHandler(card.ReaderDevice, utilities.ExecutionContext);
                }//Modification on 28-Mar-2017 for PassTech
                else if (lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString()))
                    parafaitLockCardHandler = new InnovateLockCardHandler(card.ReaderDevice, utilities.ExecutionContext, Convert.ToByte(utilities.ParafaitEnv.MifareCustomerKey), card.CardNumber);
                else if (lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()))
                    parafaitLockCardHandler = new HecereLockCardHandler(card.ReaderDevice, utilities.ExecutionContext, card.CardNumber);
                else
                {
                    MessageBox.Show("Manual(None) type lockers can not be issued.","Locker utilities");
                    log.LogMethodExit();
                    return;
                }
                parafaitLockCardHandler.ConfigCardApprovingUser = approvedByLogin;
                int lockerId = -1;
                try
                {
                    lockerAllocationDTO = parafaitLockCardHandler.GetLockerAllocationCardDetails(card.card_id);
                    txtCardInfo.Text = parafaitLockCardHandler.GetReadCardDetails(ref lockerId);

                    if (lockerAllocationDTO != null)
                    {
                        lockerBL = new Locker(lockerAllocationDTO.LockerId);
                        if (lockerBL.getLockerDTO != null)
                        {
                            if (lockerBL.getLockerDTO.LockerId > 0)
                            {
                                txtLockerId.Text = lockerBL.getLockerDTO.LockerId.ToString();
                                txtLockerName.Text = lockerBL.getLockerDTO.LockerName;
                                txtLockerNumberDisp.Text = lockerBL.getLockerDTO.Identifier.ToString();
                                lockerPanel = new LockerPanel(machineUserContext, lockerBL.getLockerDTO.PanelId);
                                txtPanelName.Text = lockerPanel.getLockerPanelDTO.PanelName;
                            }
                            else
                            {
                                txtPanelName.Text = txtLockerNumberDisp.Text = txtLockerId.Text = txtLockerName.Text = "";
                            }
                        }
                    }

                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    log.Fatal("HandleCardRead(cardNumber) method exception:" + ex.ToString());
                    txtCardInfo.Text = ex.Message;
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Fatal("HandleCardRead(cardNumber) method Exception:" + ex.ToString());
                txtCardInfo.Text = txtPanelName.Text = txtLockerNumberDisp.Text = txtLockerId.Text = txtLockerName.Text = "";
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmMifareUtils_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
            List<LockerDTO> lockerDTOList = new List<LockerDTO>();

            List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> lockerZoneSearchParams = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
            lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            lockerZonesDTOList = lockerZonesList.GetLockerZonesList(lockerZoneSearchParams);
            if (lockerZonesDTOList == null)
            {
                lockerZonesDTOList = new List<LockerZonesDTO>();
            }
            lockerZonesDTOList.Insert(0, new LockerZonesDTO());
            lockerZonesDTOList[0].ZoneName = "<Select>";
            cmbZone.DataSource = lockerZonesDTOList;
            cmbZone.ValueMember = "ZoneId";
            cmbZone.DisplayMember = "ZoneName";
            if (lockerZonesDTOList != null && lockerZonesDTOList.Count > 0)
            {
                if (lockerZonesDTOList[0].LockerPanelDTOList != null && lockerZonesDTOList[0].LockerPanelDTOList.Count > 0)
                {
                    foreach (LockerPanelDTO lockerPanelDTO in lockerZonesDTOList[0].LockerPanelDTOList)
                    {
                        if (lockerPanelDTO.LockerDTOList != null && lockerPanelDTO.LockerDTOList.Count > 0)
                        {
                            lockerDTOList.AddRange(lockerPanelDTO.LockerDTOList);
                        }
                    }
                    cmbLockers.DataSource = lockerDTOList;
                    cmbLockers.ValueMember = "Identifier";
                    cmbLockers.DisplayMember = "LockerName";
                    cmbLockers.Tag = lockerZonesDTOList[0];
                }
            }
            log.LogMethodExit();
        }

        private void btnSystemCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                if (parafaitLockCardHandler != null)
                {
                    parafaitLockCardHandler.CreateSystemCard(dtPickerFromDate.Value, dtPickerToDate.Value);
                    txtMessage.Text = "System card created successfully";
                }
                else
                {
                    txtMessage.Text = "Please tap the card.";
                }
            }
            catch (Exception ex)
            {
                log.Error("System card setup failed. ", ex);
                txtMessage.Text = "System card setup failed. " + ex.Message;
            }

            log.LogMethodExit();
        }
        DateTime currTime = DateTime.MinValue;
        private void btnCreateSettingCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                currTime = dtPickerFromDate.Value;
                if (parafaitLockCardHandler != null)
                {
                    parafaitLockCardHandler.CreateSetupCard(currTime, dtPickerToDate.Value, Convert.ToByte(dtPickerToDate.Value.Hour), Convert.ToByte(dtPickerToDate.Value.Minute));
                    txtMessage.Text = "Setup card created successfully";
                }
                else
                {
                    txtMessage.Text = "Please tap the card.";
                }
            }
            catch (Exception ex)
            {
                log.Error("Setup card setup failed. ", ex);
                txtMessage.Text = "Setup card setup failed. " + ex.Message;
            }

            log.LogMethodExit();
        }

        private void btnMasterCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                if (currTime == DateTime.MinValue)
                    currTime = dtPickerFromDate.Value;
                if (parafaitLockCardHandler != null)
                {
                    parafaitLockCardHandler.CreateMasterCard(currTime, dtPickerToDate.Value);
                    txtMessage.Text = "Master card created successfully";
                }
                else
                {
                    txtMessage.Text = "Please tap the card.";
                }
            }
            catch (Exception ex)
            {
                log.Error("Master card setup failed. ", ex);
                txtMessage.Text = "Master card setup failed. " + ex.Message;
            }

            log.LogMethodExit();
        }

        private void btnClockCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (parafaitLockCardHandler != null)
                {
                    parafaitLockCardHandler.CreateClockCard(dtPickerFromDate.Value, dtPickerToDate.Value);
                    txtMessage.Text = "Clock card created successfully";
                }
                else
                {
                    txtMessage.Text = "Please tap the card.";
                }
            }
            catch (Exception ex)
            {
                log.Error("Clock card setup failed. ", ex);
                txtMessage.Text = "Clock card setup failed. " + ex.Message;
            }

            log.LogMethodExit();
        }

        private void btnParameterCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                uint lockerNumber = 0;
                try
                {
                    lockerNumber = Convert.ToUInt32(txtLockerNo.Text);
                }
                catch (Exception ex)
                {
                    log.LogVariableState("lockerNumber", lockerNumber);
                    log.Error("Conversioin of txtLockerNo to integer failed", ex);
                }
                bool isFixed = true;
                if (lockerModeFixedMode.Checked == true)
                    isFixed = true;
                else if (lockerModeFreeMode.Checked == true)
                    isFixed = false;
                if (parafaitLockCardHandler != null)
                {
                    parafaitLockCardHandler.CreateParameterCard(dtPickerFromDate.Value, dtPickerToDate.Value, isFixed, false, lockerNumber);
                }
                else
                {
                    txtMessage.Text = "Please tap the card.";
                }
            int failureCount = 100;
                while (failureCount-- > 0)
                {
                    try
                    {
                        lockerNumber++;
                        cmbLockers.SelectedValue = lockerNumber;
                        if (cmbLockers.SelectedValue == null)
                            continue;
                        txtLockerNo.Text = lockerNumber.ToString();
                        break;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Conevrsion of lockerNumber to string failed", ex);
                    }
                }

                btnClear.PerformClick();
                txtMessage.Text = "Parameter card created successfully";
            }
            catch (Exception ex)
            {
                log.Error("Parameter card setup failed. ", ex);
                txtMessage.Text = "Parameter card setup failed. " + ex.Message;
            }

            log.LogMethodExit();
        }

        private void btnGuestCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                uint lockerNumber = 0;
                try
                {
                    lockerNumber = Convert.ToUInt32(txtLockerNo.Text);
                }
                catch (Exception ex)
                {
                    log.Error("Conversioin of txtLockerNo to integer failed", ex);
                }

                bool isFixed = true;
                if (lockerModeFixedMode.Checked == true)
                    isFixed = true;
                else if (lockerModeFreeMode.Checked == true)
                    isFixed = false;
                LockerZonesDTO lockerZonesDTO = cmbLockers.Tag as LockerZonesDTO;
                LockerDTO lockerDTO = (LockerDTO)cmbLockers.SelectedItem;
                if (parafaitLockCardHandler != null)
                {
                    parafaitLockCardHandler.CreateGuestCard(dtPickerFromDate.Value, dtPickerToDate.Value, isFixed, lockerNumber, (chbEnableInAllZone.Checked ? "ALL" : lockerZonesDTO.ZoneCode), lockerDTO.PanelId, lockerZonesDTO.LockerMake, lockerDTO.ExternalIdentifier);
                    txtMessage.Text = "Guest card created successfully";
                }
                else
                {
                    txtMessage.Text = "Please tap the card.";
                }
                
            }
            catch (Exception ex)
            {
                log.Error("Guest card setup failed. ", ex);
                txtMessage.Text = "Guest card setup failed. " + ex.Message;
            }
            log.LogMethodExit();
        }

        private void btnEraseCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                if (parafaitLockCardHandler != null)
                {
                    parafaitLockCardHandler.EraseCard();
                    txtMessage.Text = "Card erased successfully";
                }
                else
                {
                    txtMessage.Text = "Please tap the card.";
                }
            }
            catch (Exception ex)
            {
                log.Error("Erase card failed. ", ex);
                txtMessage.Text = "Erase card failed. " + ex.Message;
            }

            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            txtCardInfo.Text = txtCardNumber.Text = txtLockerId.Text = txtLockerName.Text = txtLockerNumberDisp.Text = txtMessage.Text = txtPanelName.Text = "";
            dtPickerFromDate.Value = DateTime.Now;
            dtPickerToDate.Value = DateTime.Now.AddDays(1).Date;
            log.LogMethodExit();
        }

        private void frmLockerCardUtils_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();

            if (mifareReader != null)
            {
                if (disposeInTheEnd)
                    mifareReader.Dispose();
                else
                    mifareReader.UnRegister();
            }

            log.LogMethodExit();
        }

        private void btnBlockCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LockerAllocationDTO lockerAllocationDTO = null;
            try
            {
                if (parafaitLockCardHandler != null)
                {                    
                    if (!string.IsNullOrEmpty(txtLockerNo.Text))
                    {
                        LockerDTO lockerDTO = (LockerDTO)cmbLockers.SelectedItem;
                        lockerAllocationDTO = parafaitLockCardHandler.GetLockerAllocationDetailsOnIdenitfier(Convert.ToInt32(txtLockerNo.Text), lockerDTO.PanelId);
                    }
                    parafaitLockCardHandler.CreateDisableCard(utilities.getServerTime());
                    txtMessage.Text = "Inhibit Card created successfully";
                }
                else
                {
                    txtMessage.Text = "Please tap the card.";
                }
            }
            catch (Exception ex)
            {
                log.Fatal("btnBlockCard_Click() event exception:" + ex.ToString());
                txtMessage.Text = "Inhibit card creation failed. " + ex.Message;
            }
            log.LogMethodExit();
        }

        private void lnkResetMifareReader_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            registerMifareDevice();
            log.LogMethodExit();
        }

        private void cmbLockers_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtLockerNo.Text = cmbLockers.SelectedValue.ToString();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            //groupBox1.Enabled =
            //       groupBoxLockerMode.Enabled = btnClose.Enabled = true;
            //gpMasterCardType.Visible = false;

            log.LogMethodExit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //log.LogMethodEntry(sender, e);

            //try
            //{
            //    Card card = new Card(utilities);
            //    if (parafaitLockCardHandler == null)
            //        parafaitLockCardHandler = new ParafaitLockCardHandler(card.ReaderDevice,utilities.ExecutionContext);
            //    if (rbMasterCard1.Checked) parafaitLockerLock.Create_Master_Card(0x31);
            //    else if (rbMasterCard2.Checked) parafaitLockerLock.Create_Master_Card(0x32);

            //    txtMessage.Text = ((rbMasterCard1.Checked) ? rbMasterCard1.Text : rbMasterCard2.Text) + " created successfully";
            //}
            //catch (Exception ex)
            //{
            //    txtMessage.Text = "Master card setup failed. " + ex.Message;
            //}

            //log.LogMethodExit(null);
        }

        private void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (!cmbZone.Text.Equals("Semnox.Parafait.Device.Lockers.LockerZonesDTO") && !cmbZone.Text.Equals("0"))
            {
                List<LockerZonesDTO> lockerZonesFilterDTOList = null;
                List<LockerDTO> lockerDTOList = new List<LockerDTO>();
                if (!cmbZone.SelectedValue.ToString().Equals("Semnox.Parafait.Device.Lockers.LockerZonesDTO"))
                {
                    if (lockerZonesDTOList != null && lockerZonesDTOList.Count > 0)
                    {
                        lockerZonesFilterDTOList = lockerZonesDTOList.Where(x => (bool)(x.ZoneId == Convert.ToInt32(cmbZone.SelectedValue))).ToList<LockerZonesDTO>();
                    }
                    if (lockerZonesFilterDTOList != null && lockerZonesFilterDTOList.Count > 0)
                    {
                        if (lockerZonesFilterDTOList[0].LockerPanelDTOList != null && lockerZonesFilterDTOList[0].LockerPanelDTOList.Count > 0)
                        {
                            foreach (LockerPanelDTO lockerPanelDTO in lockerZonesFilterDTOList[0].LockerPanelDTOList)
                            {
                                if (lockerPanelDTO.LockerDTOList != null && lockerPanelDTO.LockerDTOList.Count > 0)
                                {
                                    lockerDTOList.AddRange(lockerPanelDTO.LockerDTOList);
                                }
                            }
                            cmbLockers.DataSource = lockerDTOList;
                            cmbLockers.ValueMember = "Identifier";
                            cmbLockers.DisplayMember = "LockerName";
                            cmbLockers.Tag = lockerZonesFilterDTOList[0];
                            lblLockerMake.Text = string.IsNullOrEmpty(lockerZonesFilterDTOList[0].LockerMake)? ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext,"LOCKER_LOCK_MAKE") :lockerZonesFilterDTOList[0].LockerMake;
                            if (lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString())|| lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                            {
                                groupBox1.Enabled =
                                btnParameterCard.Enabled =
                                btnBlockCard.Enabled = false;
                                if (!lblLockerMake.Text.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                                {
                                    chbEnableInAllZone.Visible = true;
                                }
                            }
                            else
                            {
                                groupBox1.Enabled =
                                btnParameterCard.Enabled =
                                btnBlockCard.Enabled = true;
                                chbEnableInAllZone.Visible = false;
                            }
                        }
                        else
                        {
                            cmbLockers.DataSource = new List<LockerDTO>();
                            cmbLockers.ValueMember = "Identifier";
                            cmbLockers.DisplayMember = "LockerName";
                            lblLockerMake.Text = "";
                        }
                    }
                    else
                    {
                        cmbLockers.DataSource = new List<LockerDTO>();
                        cmbLockers.ValueMember = "Identifier";
                        cmbLockers.DisplayMember = "LockerName";
                        lblLockerMake.Text = "";
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
