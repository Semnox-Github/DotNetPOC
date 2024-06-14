/********************************************************************************************
* Project Name - frmFPEnrollDeactive
* Description  - This form will enroll the Thumb Impression and Deactivation of thumb impression.
* 
**************
**Version Log
**************
*Version     Date             Modified By              Remarks          
*********************************************************************************************
*2.80.0      13-Feb-2020      Indrajeet Kumar          Created 
********************************************************************************************/
using System;
using System.Linq;
using System.Drawing;
using Semnox.Parafait.User;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Device.Biometric;

namespace Parafait_POS.Login
{
    public partial class frmFPEnrollDeactive : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FingerPrintReader fingerPrintReader = null;
        List<UserRolesDTO> UserRoleDtoList = null;
        List<UsersDTO> UsersDtoList = null;
        private byte[] fingerTemplate = null; // Hold the Template
        List<LookupValuesDTO> LookupValuesDTOList; //Populate Finger Number to name
        Utilities utilities = POSStatic.Utilities;

        public frmFPEnrollDeactive()
        {
            log.LogMethodEntry();
            InitializeComponent();
            EventHandler fingerprintScanCompleteEvent = new EventHandler(FingerPrintScanCompleteEventHandle);
            ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(utilities.ExecutionContext);
            List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParafaitDefaultsParam = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            searchParafaitDefaultsParam.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "BIOMETRIC_DEVICE_TYPE"));
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParafaitDefaultsParam);
            if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count > 0)
            {
                ParafaitDefaultsDTO parafaitDefaultsDTO = parafaitDefaultsDTOList[0];
                BiometricFactory.GetInstance().Initialize();
                fingerPrintReader = BiometricFactory.GetInstance().GetBiometricDeviceType(parafaitDefaultsDTO.DefaultValue);
                fingerPrintReader.Initialize(-1, string.Empty, DisplayText, this.pbThumbImpresion.Handle, QualityProgressBar);
                fingerPrintReader.Register(new EventHandler(fingerprintScanCompleteEvent));
            }
            else
            {
                POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2820), this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// DisplayText Method used to display the FingerPrint Status Message Bar while scanning the fingerPrint.
        /// </summary>
        /// <param name="status"></param>
        public void DisplayText(int status)
        {
            log.LogMethodEntry(status);
            try
            {
                if (txtMessage.InvokeRequired)
                {
                    MessagePrint messagePrint = new MessagePrint(DisplayText);
                    Invoke(messagePrint, new object[] { status });
                }
                else
                {
                    string fpStatusMessage = string.Empty;
                    switch (status)
                    {
                        case 0:
                            fpStatusMessage = "No finger";
                            break;
                        case 1:
                            fpStatusMessage = "Move finger up";
                            break;
                        case 2:
                            fpStatusMessage = "Move finger down";
                            break;
                        case 3:
                            fpStatusMessage = "Move finger left";
                            break;
                        case 4:
                            fpStatusMessage = "Move finger right";
                            break;
                        case 5:
                            fpStatusMessage = "Press finger harder";
                            break;
                        case 6:
                            fpStatusMessage = "Latent";
                            break;
                        case 7:
                            fpStatusMessage = "Remove your finger";
                            break;
                        case 8:
                            fpStatusMessage = "Finger accepted";
                            break;
                        case 9:
                            fpStatusMessage = "Finger was detected";
                            break;
                        case 10:
                            fpStatusMessage = "Finger is misplaced";
                            break;
                        case 11:
                            fpStatusMessage = "Don't remove your finger";
                            break;
                        default:
                            fpStatusMessage = "No finger";
                            break;
                    }
                    txtMessage.Text = fpStatusMessage;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
                throw ex;
            }
        }

        /// <summary>
        /// QualityProgress Bar Method used to display the FingerPrint Quality Progess Bar while scanning the fingerPrint.
        /// </summary>
        /// <param name="quality"></param>
        public void QualityProgressBar(byte quality)
        {
            log.LogMethodEntry(quality);
            try
            {
                if (prbStatus.InvokeRequired)
                {
                    QualityProgress qualityProgress = new QualityProgress(QualityProgressBar);
                    Invoke(qualityProgress, new object[] { quality });
                }
                else
                {
                    this.prbStatus.Value = quality < this.prbStatus.Maximum ? quality : this.prbStatus.Maximum;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
                throw ex;
            }
            log.LogMethodExit();
        }


        private void FingerPrintScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                log.Debug("Start - FingerPrintScanCompleteEventHandle");
                if (e is MorphoScannedEventArgs)
                {
                    log.Debug("Start - Evenet e" + e);
                    MorphoScannedEventArgs checkScannedEvent = e as MorphoScannedEventArgs;
                    fingerTemplate = checkScannedEvent.fingerPrintTemplate;
                    log.Debug("checkScannedEvent Value" + checkScannedEvent);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method is used to populate User Role. 
        /// </summary>
        private void PopulateUserRoleCombo()
        {
            log.LogMethodEntry();
            try
            {
                UserRolesList userRoleList = new UserRolesList();
                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> SearchParameter = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                SearchParameter.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                UserRoleDtoList = userRoleList.GetAllUserRoles(SearchParameter);
                if (UserRoleDtoList == null)
                {
                    UserRoleDtoList = new List<UserRolesDTO>();
                }
                UserRoleDtoList.Insert(0, new UserRolesDTO());
                UserRoleDtoList[0].RoleId = 0;
                UserRoleDtoList[0].Role = "-- Select --";
                foreach (UserRolesDTO userRolesDTO in UserRoleDtoList)
                {
                    cmbUserRole.DisplayMember = "Role";
                    cmbUserRole.ValueMember = "RoleId";
                }
                cmbUserRole.DataSource = UserRoleDtoList;
                cmbUserRole.DisplayMember = "Role";
                cmbUserRole.ValueMember = "RoleId";
                cmbUserRole.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message);
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method is used to populate User into a combo box.
        /// By Default - UserRole will be --All-- which will dsplay all the User.
        /// If UserRole is Select then based on that user will shown in combobox.
        /// </summary>
        private void PopulateUserCombo()
        {
            log.LogMethodEntry();
            try
            {
                UsersList usersList = new UsersList(utilities.ExecutionContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> UserSearchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                UserSearchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                if (Convert.ToInt32(cmbUserRole.SelectedValue) > 0)
                {
                    UserSearchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_ID, cmbUserRole.SelectedValue.ToString()));
                }
                UsersDtoList = usersList.GetAllUsers(UserSearchParameter, true);
                if (UsersDtoList == null)
                {
                    UsersDtoList = new List<UsersDTO>();
                }
                UsersDtoList.Insert(0, new UsersDTO());
                UsersDtoList[0].UserId = 0;
                UsersDtoList[0].LoginId = "-- Select --";

                //foreach (UsersDTO usersDTO in UsersDtoList)
                //{
                //    cmbUserName.DisplayMember = "loginid";
                //    cmbUserName.ValueMember = "userid";
                //}

                cmbUserName.DataSource = UsersDtoList;
                cmbUserName.DisplayMember = "loginid";
                cmbUserName.ValueMember = "userid";
                cmbUserName.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message);
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method is used to populate FingerPosition into a combo box.
        /// </summary>
        private void PopulateFingerPosition()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> LookUpSearchParameter = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                LookUpSearchParameter.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                LookUpSearchParameter.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FINGER_POSITION"));
                LookupValuesDTOList = lookupValuesList.GetAllLookupValues(LookUpSearchParameter);
                if (LookupValuesDTOList == null)
                {
                    LookupValuesDTOList = new List<LookupValuesDTO>();
                }
                else if (LookupValuesDTOList != null && LookupValuesDTOList.Any())
                {
                    LookupValuesDTOList = LookupValuesDTOList.OrderBy(x => x.Description).ToList();
                    cmbFingerPosition.DataSource = LookupValuesDTOList;
                    cmbFingerPosition.ValueMember = "LookupValue";
                    cmbFingerPosition.DisplayMember = "Description";
                    cmbFingerPosition.SelectedValue = cmbFingerPosition.SelectedValue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message);
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method will scan the finger print and get the Template.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScan_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (!string.IsNullOrEmpty(cmbUserRole.Text) && !string.IsNullOrEmpty(cmbUserName.Text) && !string.IsNullOrEmpty(cmbFingerPosition.Text))
                {
                    // Disable the Combo Box and disable Scan Button on a Click Event.
                    // Enable Combo Box & Scan Button on click of Refresh button.
                    cmbUserRole.Enabled = false;
                    cmbUserName.Enabled = false;
                    cmbFingerPosition.Enabled = false;
                    btnScan.Enabled = false;
                    btnScan.BackColor = System.Drawing.Color.Gray;

                    fingerTemplate = fingerPrintReader.Scan();
                    if (fingerTemplate != null && fingerTemplate.Length > 0)
                    {
                        POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2823), "Message");
                    }
                    log.Debug("Finger Template Value : " + fingerTemplate);
                }
                else
                {
                    log.LogVariableState("Message", "Found Empty Value in Dropdown");
                    POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2824), "Alert");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
                if (fingerPrintReader != null)
                    fingerPrintReader.Dispose();
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method will Register the fingerPrint after Scan of the FingerPrint.
        /// </summary>     
        private void btnRegister_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if ((!string.IsNullOrEmpty(cmbUserRole.Text) && !string.IsNullOrEmpty(cmbUserName.Text) && !string.IsNullOrEmpty(cmbFingerPosition.Text)) && (fingerTemplate != null))
            {
                if (Convert.ToInt32(cmbUserName.SelectedValue) > 0)
                {
                    UsersDTO usersDTO = UsersDtoList.FirstOrDefault(user => user.UserId == Convert.ToInt32(cmbUserName.SelectedValue));

                    if (usersDTO != null)
                    {
                        List<UserIdentificationTagsDTO> userIdentificationTagsDTOList
                            = usersDTO.UserIdentificationTagsDTOList.Where(x => string.IsNullOrEmpty(x.CardNumber)
                                                                       && x.FingerNumber > 0
                                                                       && x.FPTemplate.Length > 0
                                                                       && x.FingerNumber == Convert.ToInt32(cmbFingerPosition.SelectedValue)).ToList();
                        usersDTO.UserIdentificationTagsDTOList.ForEach(x => x.IsChanged = false);

                        if (userIdentificationTagsDTOList != null && userIdentificationTagsDTOList.Any())
                        {
                            UserIdentificationTagsDTO existingUserIdentificationTagsDTO = usersDTO.UserIdentificationTagsDTOList.FirstOrDefault(user => user.FingerNumber == Convert.ToInt32(cmbFingerPosition.SelectedValue));
                            if (existingUserIdentificationTagsDTO != null)
                            {
                                existingUserIdentificationTagsDTO.ActiveFlag = true;
                                existingUserIdentificationTagsDTO.IsChanged = true;
                                existingUserIdentificationTagsDTO.FPTemplate = fingerTemplate;

                                /// Dialogue Box will pop to confirm should user want to update a existing record.
                                if (POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2825), "Confirm Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    using(ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                                    {
                                        parafaitDBTransaction.BeginTransaction();
                                        Users users = new Users(utilities.ExecutionContext, usersDTO);
                                        users.Save();
                                        parafaitDBTransaction.EndTransaction();
                                    }
                                    POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2826), "Confirm Update", MessageBoxButtons.OK);
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                        else
                        {
                            UserIdentificationTagsDTO userIdentificationTagsDTO = new UserIdentificationTagsDTO(-1, Convert.ToInt32(cmbUserName.SelectedValue), null, null, Convert.ToInt32(cmbFingerPosition.SelectedValue), true, DateTime.MinValue, DateTime.MinValue, false, -1, fingerTemplate, null);
                            userIdentificationTagsDTO.IsChanged = true;
                            usersDTO.UserIdentificationTagsDTOList.Add(userIdentificationTagsDTO);
                            log.Debug("UserIdentificationTagsDTO Value :" + userIdentificationTagsDTO);
                            using(ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                            {
                                parafaitDBTransaction.BeginTransaction();
                                Users users = new Users(utilities.ExecutionContext, usersDTO);
                                users.Save();
                                parafaitDBTransaction.EndTransaction();
                            }
                            POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2829, usersDTO.UserName.ToUpper()), "Message");
                        }
                    }
                    RefreshDataGridView();
                }
            }
            else
            {
                log.LogVariableState("Message", "Found Empty Value in Dropdown");
                POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2824), "Message");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// On Load of Event Form - 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFPEnrollDeactive_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateFingerPosition();
            PopulateUserRoleCombo();
            PopulateUserCombo();
            log.LogMethodExit();
        }

        /// <summary>
        /// Event will trigger when UserRole Index will Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbUserRole.SelectedIndex >= 0)
            {
                PopulateUserCombo();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Event will trigger when LoginId will Change and loads the into datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DGVUserFingerDetails();
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method will display Value the Data Grid View - 
        /// </summary>
        private void DGVUserFingerDetails()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<string, bool>> userFingerPrintDetailList = new List<KeyValuePair<string, bool>>();
                if (!string.IsNullOrEmpty(cmbUserName.SelectedValue.ToString()) && cmbUserName.SelectedIndex > 0)
                {
                    UsersDTO usersDTO = UsersDtoList.FirstOrDefault(user => user.UserId == Convert.ToInt32(cmbUserName.SelectedValue));
                    if (usersDTO != null && usersDTO.UserIdentificationTagsDTOList.Any() && usersDTO.UserIdentificationTagsDTOList != null)
                    {
                        if (LookupValuesDTOList == null)
                        {
                            PopulateFingerPosition();
                        }
                        //List<UserIdentificationTagsDTO> userIdentificationTagsDTOTemp = new List<UserIdentificationTagsDTO>();
                        foreach (UserIdentificationTagsDTO userIdentificationTagsDTO in usersDTO.UserIdentificationTagsDTOList)
                        {
                            if (userIdentificationTagsDTO.FPTemplate != null && userIdentificationTagsDTO.FingerNumber > 0)
                            {
                                string fingerName = LookupValuesDTOList.First(x => x.LookupValue == userIdentificationTagsDTO.FingerNumber.ToString()).Description;
                                KeyValuePair<String, bool> userFPDetail = new KeyValuePair<string, bool>(fingerName, userIdentificationTagsDTO.ActiveFlag);
                                userFingerPrintDetailList.Add(userFPDetail);
                                //userIdentificationTagsDTOTemp.Add(userIdentificationTagsDTO);
                            }
                        }

                        gblblUserName.Text = usersDTO.UserName;
                        gblblLoginId.Text = usersDTO.LoginId;
                        dgvUserFingerDetails.AutoGenerateColumns = false;
                        dgvUserFingerDetails.BackgroundColor = Color.White;
                        dgvUserFingerDetails.RowHeadersVisible = false;
                        dgvUserFingerDetails.DataSource = userFingerPrintDetailList;
                    }
                    else
                    {
                        gblblUserName.Text = null;
                        gblblLoginId.Text = null;
                        dgvUserFingerDetails.DataSource = null;
                        dgvUserFingerDetails.Rows.Clear();
                    }
                }
                else
                {
                    gblblUserName.Text = null;
                    gblblLoginId.Text = null;
                    dgvUserFingerDetails.DataSource = null;
                    dgvUserFingerDetails.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Refresh the DataGridView
        /// </summary>
        private void RefreshDataGridView()
        {
            log.LogMethodEntry();
            DGVUserFingerDetails();
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method will Deactivate the UserIdentificationTags record based on UserId
        /// </summary>
        private void btnDeactivation_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (!string.IsNullOrEmpty(cmbUserName.SelectedValue.ToString()))
                {
                    UsersDTO usersDTO = UsersDtoList.FirstOrDefault(user => user.UserId == Convert.ToInt32(cmbUserName.SelectedValue));
                    if (usersDTO != null && usersDTO.UserIdentificationTagsDTOList.Any() && dgvUserFingerDetails.Rows.Count > 0)
                    {
                        if (POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(958), "Confirm Deactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            List<UserIdentificationTagsDTO> UserIdentificationTagsDTOList = usersDTO.UserIdentificationTagsDTOList.Where(x => string.IsNullOrEmpty(x.CardNumber)).ToList();
                            if (UserIdentificationTagsDTOList != null && UserIdentificationTagsDTOList.Any())
                            {
                                UserIdentificationTagsDTOList.ForEach(x => { x.ActiveFlag = false; });
                                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                                {
                                    parafaitDBTransaction.BeginTransaction();
                                    Users user = new Users(utilities.ExecutionContext, usersDTO);
                                    user.Save(parafaitDBTransaction.SQLTrx);
                                    parafaitDBTransaction.EndTransaction();
                                }
                            }
                            POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2828, usersDTO.UserName.ToUpper()), "Message");
                        }
                        RefreshDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Below Event is used to Destroy the Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (fingerPrintReader != null)
            {
                fingerPrintReader.Dispose();
            }
            this.Dispose();
            this.Close();
            log.LogMethodExit();
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Refresh();
            RefreshDataGridView();
            cmbUserRole.Enabled = true;
            cmbUserName.Enabled = true;
            cmbFingerPosition.Enabled = true;
            btnScan.Enabled = true;
            btnScan.BackColor = System.Drawing.Color.Navy;
            log.LogMethodExit();
        }
    }
}

