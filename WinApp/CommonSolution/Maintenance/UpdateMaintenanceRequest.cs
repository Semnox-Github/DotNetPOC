/********************************************************************************************
 * Project Name - Update Maintenance Request UI
 * Description  - User interface for update maintenance request
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        03-Feb-2016   Raghuveera          Created
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Maintenance
{
    public partial class UpdateMaintenanceRequest : Form
    {
        int RequestId;
        Utilities utilities;
        AssetList assetList;
        List<GenericAssetDTO> genericAssetListOnDisplay;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserJobItemsDTO userJobItemsDTO;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public UpdateMaintenanceRequest(Utilities _Utilities, int pRequestId = -1)
        {
            log.LogMethodEntry(_Utilities, pRequestId);
            RequestId = pRequestId;
            InitializeComponent();
            utilities = _Utilities;

            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                lnkDeactivateMachine.Visible = false;
            }
            else
            {
                machineUserContext.SetSiteId(-1);
                lnkDeactivateMachine.Visible = true;
            }
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                btnPublishToSite.Visible = true;
            }
            else
            {
                btnPublishToSite.Visible = false;
            } 
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
        private void UpdateMaintenanceRequest_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e); 
            GetAssets();
            GetRequestTypes();
            GetUsers(-1);
            GetRequestStatus();
            GetRequestPriority();
            GetJobDetail();
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the details to the the form 
        /// </summary>
        private void GetJobDetail()
        {
            log.LogMethodEntry(RequestId);
            if (RequestId != -1)
            {
                List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> maintenanceJobSearchParams = new List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>>();
                maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID, RequestId.ToString()));
                maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                UserJobItemsListBL maintenanceJobList = new UserJobItemsListBL(machineUserContext);
                BindingSource maintenanceJobBS = new BindingSource();
                List<UserJobItemsDTO> maintenanceJobDTOList = maintenanceJobList.GetAllUserJobItemDTOList(maintenanceJobSearchParams, utilities.ParafaitEnv.User_Id);
                if (maintenanceJobDTOList != null && maintenanceJobDTOList.Count > 0)
                {
                    userJobItemsDTO = maintenanceJobDTOList[0];
                    cmbAssets.SelectedValue = userJobItemsDTO.AssetId;
                    txtRequestDate.Text = (string.IsNullOrEmpty(userJobItemsDTO.RequestDate)) ? DateTime.Now.ToString(dtpScheduleDate.CustomFormat) : DateTime.Parse(userJobItemsDTO.RequestDate).ToString(dtpScheduleDate.CustomFormat);//utilities.getDateFormat());
                    dtpScheduleDate.Value = DateTime.Parse(userJobItemsDTO.ChklstScheduleTime);
                    txtRequestTitle.Text = userJobItemsDTO.MaintJobName;
                    cmbRequestType.SelectedValue = userJobItemsDTO.RequestType;
                    txtRequestDetail.Text = userJobItemsDTO.RequestDetail;
                    cmbPriority.SelectedValue = userJobItemsDTO.Priority;
                    txtImageFile.Text = userJobItemsDTO.ImageName;
                    if (txtImageFile.Text.Length > 0)
                    {
                        btnImageOpen.Enabled = true;
                    }
                    txtContactPerson.Text = string.IsNullOrEmpty(userJobItemsDTO.RequestedBy) ? utilities.ParafaitEnv.LoginID : userJobItemsDTO.RequestedBy;
                    txtEmail.Text = userJobItemsDTO.ContactEmailId;
                    txtPhone.Text = userJobItemsDTO.ContactPhone;
                    cmbAssignedTo.SelectedValue = userJobItemsDTO.AssignedUserId;
                    txtResolution.Text = userJobItemsDTO.Resolution;
                    txtComments.Text = userJobItemsDTO.Comments;
                    txtFile.Text = userJobItemsDTO.DocFileName;
                    if (txtFile.Text.Length > 0)
                    {
                        btnOpen.Enabled = true;
                    }
                    cmbStatus.SelectedValue = userJobItemsDTO.Status;
                    txtRepairCost.Text = (userJobItemsDTO.RepairCost == -1) ? "" : userJobItemsDTO.RepairCost.ToString();
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage("No Records Found!"));
                    log.Debug("No Records found");
                    Close();
                }
            }
            else
            {
                userJobItemsDTO = new UserJobItemsDTO();
                txtRequestDate.Text = utilities.getServerTime().ToString(dtpScheduleDate.CustomFormat);//utilities.getDateFormat());
                txtContactPerson.Text = utilities.ParafaitEnv.LoginID;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads assets to the combobox
        /// </summary>
        private void GetAssets()
        {
            log.LogMethodEntry();
            assetList = new AssetList(machineUserContext);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, "Y"));
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            genericAssetListOnDisplay = assetList.GetAllAssets(assetSearchParams);
            if (genericAssetListOnDisplay != null)
            {
                BindingSource binding = new BindingSource();
                binding.DataSource = genericAssetListOnDisplay;
                genericAssetListOnDisplay.Insert(0, new GenericAssetDTO());

                cmbAssets.DataSource = binding;
                cmbAssets.ValueMember = "AssetId";
                cmbAssets.DisplayMember = "Name";
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the request type
        /// </summary>
        private void GetRequestTypes()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_REQUEST_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList != null)
                {
                    BindingSource bindingSource = new BindingSource();
                    lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                    //lookupValuesDTOList[0].Description = utilities.MessageUtils.getMessage("None");
                    bindingSource.DataSource = lookupValuesDTOList;
                    cmbRequestType.DataSource = bindingSource;
                    cmbRequestType.ValueMember = "LookupValueId";
                    cmbRequestType.DisplayMember = "LookupValue";
                }
                else
                {
                    MessageBox.Show("'MAINT_REQUEST_TYPE' is not found in Lookups");
                } 
            }
            catch (Exception e)
            {
                log.Error("Ends-GetRequestTypes() Method with an Exception:", e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the user details
        /// </summary>
        /// <param name="departmentID">department id</param>
        private void GetUsers(int departmentID)
        {
            log.LogMethodEntry();
            cmbAssignedTo.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbAssignedTo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            try
            { 
                UsersList usersList = new UsersList(machineUserContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<UsersDTO> usersDTOList = usersList.GetAllUsers(usersSearchParams);
                if (usersDTOList != null)
                {
                    BindingSource assignedToBS = new BindingSource();
                    usersDTOList.Insert(0, new UsersDTO());
                    assignedToBS.DataSource = usersDTOList;
                    cmbAssignedTo.DataSource = usersDTOList;
                    cmbAssignedTo.DisplayMember = "UserName";
                    cmbAssignedTo.ValueMember = "UserId";
                } 
            }
            catch (Exception e)
            {
                log.Error("Ends-GetUsers(departmentID) Method with an Exception:", e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads request status
        /// </summary>
        private void GetRequestStatus()
        {
            log.LogMethodEntry();
            try
            {
                // gets the request status-'open' or 'close' etc..-07-05-2015
                cmbStatus.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbStatus.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList != null)
                {
                    BindingSource bindingSource = new BindingSource();
                    lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                    //lookupValuesDTOList[0].Description = utilities.MessageUtils.getMessage("None");
                    bindingSource.DataSource = lookupValuesDTOList;
                    cmbStatus.DataSource = bindingSource;
                    cmbStatus.ValueMember = "LookupValueId";
                    cmbStatus.DisplayMember = "LookupValue";
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Open"));
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);
                    cmbStatus.SelectedValue = lookupOpenValuesDTOList[0].LookupValueId;
                }
                else
                {
                    MessageBox.Show("'MAINT_JOB_STATUS' is not found in Lookups");
                } 
            }
            catch (Exception e)
            {
                log.Error("Ends-getRequestStatus() Method with an Exception:", e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads Priority
        /// </summary>
        private void GetRequestPriority()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_PRIORITY"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList != null)
                {
                    BindingSource bindingSource = new BindingSource();
                    lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                    bindingSource.DataSource = lookupValuesDTOList;
                    cmbPriority.DataSource = bindingSource;
                    cmbPriority.ValueMember = "LookupValueId";
                    cmbPriority.DisplayMember = "LookupValue";
                }
                else
                {
                    MessageBox.Show("'MAINT_JOB_PRIORITY' is not found in Lookups");
                } 
            }
            catch (Exception e)
            {
                log.Error("Ends-getRequestPriority() Method with an Exception:", e);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// does enableing and disabling of controls
        /// </summary>
        private void DisableControls()
        {
            log.LogMethodEntry();
            SetControlState(!utilities.ParafaitEnv.Manager_Flag.Equals("Y"));
            log.LogMethodExit();
        }
        /// <summary>
        /// enables and disables the controls 
        /// </summary>
        /// <param name="val"> bolean value to enable or disable the control</param>
        private void SetControlState(bool val)
        {
            log.LogMethodEntry(val);
            dtpScheduleDate.Enabled = val;
            txtRequestDetail.ReadOnly = !val;
            txtRequestTitle.ReadOnly = !val;
            cmbRequestType.Enabled = val;
            cmbAssignedTo.Enabled = val;
            txtContactPerson.ReadOnly = !val;
            txtEmail.ReadOnly = !val;
            txtPhone.ReadOnly = !val;
            log.LogMethodExit();
        }
        /// <summary>
        /// gets the path for saving file
        /// </summary>
        /// <returns>returns the path</returns>
        private string GetSharedFolderPath()
        {
            log.LogMethodEntry();
            string folderPath = utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            log.LogMethodExit(folderPath);
            return folderPath;
        }

        /// <summary>
        /// Validates the input
        /// </summary>
        /// <returns></returns>
        private bool ValidateSave()
        {
            log.LogMethodEntry();
            bool valid = false;
            if ((int)cmbPriority.SelectedValue == -1 || cmbPriority.SelectedValue == null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(978), "Priority");
                log.Debug("Please select Priority");
                log.LogMethodExit(valid);
                return valid;
            }
            if (cmbAssets.SelectedValue == null || (int)cmbAssets.SelectedValue == -1)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(961));
                log.Debug("Please select the asset");
                log.LogMethodExit(valid);
                return valid;
            }
            if (cmbAssignedTo.SelectedValue == null || (int)cmbAssignedTo.SelectedValue == -1)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(979));
                log.Debug("Please select the assigned to");
                log.LogMethodExit(valid);
                return valid;
            }
            if (cmbStatus.SelectedValue == null || (int)cmbStatus.SelectedValue == -1)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(980));
                log.Debug("Please select the status");
                log.LogMethodExit(valid);
                return valid;
            }
            if (cmbRequestType.SelectedValue == null || (int)cmbRequestType.SelectedValue == -1)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(987));
                log.Debug("Invalid request type");
                log.LogMethodExit(valid);
                return valid;
            }
            if (txtRequestTitle.Text.Length == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(981), "Request Title");
                log.Debug("Request title is required");
                log.LogMethodExit(valid);
                return valid;
            }

            if (txtRequestDetail.Text.Length == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(982), "Request Details");
                log.Debug("Please enter request details");
                log.LogMethodExit(valid);
                return valid;
            }

            if (txtContactPerson.Text.Length == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(983), "Contact Person");
                log.Debug("Please enter contact person");
                log.LogMethodExit(valid);
                return valid;
            }
            if (!string.IsNullOrEmpty(txtRepairCost.Text))
            {
                double x;
                if (!double.TryParse(txtRepairCost.Text, out x))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(984), "Invalid repair cost.");
                    log.Debug("Invalid repair cost.");
                    log.LogMethodExit(valid);
                    return valid;
                }
            }

            if (cmbStatus.Text.ToUpper() == "CLOSED")
            {
                if (txtComments.Text.Length == 0 && txtResolution.Text.Length == 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(985), "Resolution");
                    log.Debug("Please enter comments/resolution");
                    log.LogMethodExit(valid);
                    return valid;
                }
            }
            valid = true;
            log.LogMethodExit(valid);
            return valid;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!ValidateSave())
            {
                log.LogMethodExit("ValidateSave() failed");
                return;
            }
            Save();
            log.LogMethodExit();
        }
        bool Save()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "close"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupCloseValuesDTO = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

            try
            {
                userJobItemsDTO.AssetId = Convert.ToInt32(cmbAssets.SelectedValue);
                userJobItemsDTO.AssetName = cmbAssets.Text;
                try
                {
                    userJobItemsDTO.RequestDate = string.IsNullOrEmpty(txtRequestDate.Text) ? DateTime.Now.ToString() : DateTime.Parse(txtRequestDate.Text).ToString();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(986));
                    log.LogMethodExit("Invalid RequestDate");
                    return false;
                }
                userJobItemsDTO.ChklstScheduleTime = dtpScheduleDate.Value.Date.ToString();
                userJobItemsDTO.MaintJobName = userJobItemsDTO.TaskName = txtRequestTitle.Text;
                try
                {
                    if (cmbRequestType.SelectedValue != null)
                        userJobItemsDTO.RequestType = Convert.ToInt32(cmbRequestType.SelectedValue);
                    else
                        userJobItemsDTO.RequestType = -1;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(987));
                    log.LogMethodExit("Invalid RequestType");
                    return false;
                }
                userJobItemsDTO.RequestDetail = txtRequestDetail.Text;
                try
                {
                    if (cmbPriority.SelectedValue != null)
                        userJobItemsDTO.Priority = Convert.ToInt32(cmbPriority.SelectedValue);
                    else
                        userJobItemsDTO.Priority = -1;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage("Invalid priority."));
                    log.LogMethodExit("Invalid priorit");
                    return false;
                }

                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Service Request"));
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);
                if (lookupOpenValuesDTOList != null)
                {
                    if (userJobItemsDTO.MaintJobType != lookupOpenValuesDTOList[0].LookupValueId)
                    {
                        userJobItemsDTO.MaintChklstdetId = -1;
                    }
                    userJobItemsDTO.MaintJobType = lookupOpenValuesDTOList[0].LookupValueId;
                }
                else
                {
                    MessageBox.Show("Lookup MAINT_JOB_TYPE is not found");
                    log.LogMethodExit("Lookup MAINT_JOB_TYPE is not found");
                    return false;
                }
                userJobItemsDTO.ImageName = Path.GetFileName(txtImageFile.Text);
                userJobItemsDTO.DocFileName = Path.GetFileName(txtFile.Text);
                userJobItemsDTO.RequestedBy = txtContactPerson.Text;
                userJobItemsDTO.ContactEmailId = txtEmail.Text;
                userJobItemsDTO.ContactPhone = txtPhone.Text;
                userJobItemsDTO.AssignedUserId = Convert.ToInt32(cmbAssignedTo.SelectedValue);
                userJobItemsDTO.AssignedTo = cmbAssignedTo.Text;
                userJobItemsDTO.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                userJobItemsDTO.Resolution = txtResolution.Text;
                userJobItemsDTO.Comments = txtComments.Text;
                userJobItemsDTO.RepairCost = (string.IsNullOrEmpty(txtRepairCost.Text)) ? -1 : Convert.ToDouble(txtRepairCost.Text);
                userJobItemsDTO.LastUpdatedDate = null;
                userJobItemsDTO.DurationToComplete = -1;
                userJobItemsDTO.Status = (int)cmbStatus.SelectedValue;
                if (userJobItemsDTO.Status == lookupCloseValuesDTO[0].LookupValueId)
                {
                    userJobItemsDTO.ChecklistCloseDate = utilities.getServerTime().ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    userJobItemsDTO.ChecklistCloseDate = "";
                }
                UserJobItemsBL maintenanceJob = new UserJobItemsBL(machineUserContext, userJobItemsDTO);
                maintenanceJob.Save();
                MessageBox.Show(utilities.MessageUtils.getMessage(452), utilities.MessageUtils.getMessage("Database Save"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "Save");
                return false;
            }
            string fileName;
            try
            {
                if (txtFile.Text.Length > 0)
                {
                    try
                    {
                        fileName = Path.GetFileName(txtFile.Text);
                    }
                    catch
                    {
                        fileName = txtFile.Text;
                    }

                    if (!File.Exists(Path.Combine(GetSharedFolderPath(), fileName)))
                    {
                        File.Copy(txtFile.Text, Path.Combine(GetSharedFolderPath(), fileName));
                        Application.DoEvents();
                    }
                }
            }
            catch (IOException ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(988) + ":" + Path.GetFileName(txtFile.Text) + Environment.NewLine + ex.Message);
            }

            try
            {
                if (txtImageFile.Text.Length > 0)
                {
                    try
                    {
                        fileName = Path.GetFileName(txtImageFile.Text);
                    }
                    catch
                    {
                        fileName = txtImageFile.Text;
                    }
                    if (!File.Exists(Path.Combine(GetSharedFolderPath(), fileName)))
                    {
                        File.Copy(txtImageFile.Text, Path.Combine(GetSharedFolderPath(), fileName));
                        Application.DoEvents();
                    }
                }
            }
            catch (IOException ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(988) + Path.GetFileName(txtImageFile.Text) + Environment.NewLine + ex.Message);
            }
            log.LogMethodExit();
            return true;
        }
        private void btnEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!ValidateSave())
            {
                log.LogMethodExit("ValidateSave() failed.");
                return;
            }

            //pbPicture.Image.Save(folderName + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            if (Save())
            {
                string  eMail = "", replyTo = "";                
                replyTo = txtEmail.Text;
                UsersList usersList = new UsersList(machineUserContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, cmbAssignedTo.SelectedValue.ToString()));

                List<UsersDTO> usersDTOList = usersList.GetAllUsers(usersSearchParams);
                if (usersDTOList != null && usersDTOList.Count > 0)
                {
                    eMail = usersDTOList[0].Email;
                }
                using (SendEmailUI semail = new SendEmailUI(eMail, /*The first parameter 'eMail' was added on 23-06-2015. Previously empty string was getting passed*/
                                                    "", replyTo /*If requestor and assignee are same then reply to field is made empty. otherwise txtemail.text value is assigned -23-06-2015*/,
                                                    txtRequestTitle.Text,
                                                    "Siteid: " + userJobItemsDTO.Siteid + "," + Environment.NewLine + Environment.NewLine +
                                                    "Request details: " + Environment.NewLine +
                                                    "Request Date: " + txtRequestDate.Text + Environment.NewLine +
                                                    "Machine: " + cmbAssets.Text + Environment.NewLine +
                                                    txtRequestDetail.Text + Environment.NewLine +
                                                    "Contact: " + txtContactPerson.Text + " / " + txtPhone.Text + " / " + txtEmail.Text,
                                                    "", "", false, utilities, true))
                {
                    semail.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            using (OpenFileDialog fDialog = new OpenFileDialog())
            {
                fDialog.Title = "Select file to upload";
                fDialog.Filter = "All Files|*.*";
                fDialog.Multiselect = false;
                if (fDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFile.Text = Path.GetFullPath(fDialog.FileName);
                }
            }
            log.LogMethodExit();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (RequestId == -1)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(989), "Entry not saved");
                    log.LogMethodExit("RequestId == -1");
                    return;
                }
                btnOpen.Enabled = false;
                Process Proc = new Process();
                Proc.StartInfo.FileName = Path.Combine(GetSharedFolderPath(), Path.GetFileName(txtFile.Text));
                Proc.Start();
                btnOpen.Enabled = true;
            }
            catch (Exception ex)
            {
                btnOpen.Enabled = true;
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }
        private void txtRepairCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void btnImageBrowse_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //to upload files -22-04-2015 
            using (OpenFileDialog fDialog = new OpenFileDialog())
            {
                fDialog.Title = "Select Image to upload";
                fDialog.Filter = "(*.bmp, *.jpg)|*.bmp;*.jpg";
                fDialog.Multiselect = false;
                if (fDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // txtFile.Text = fDialog.FileName.ToString();
                    txtImageFile.Text = Path.GetFullPath(fDialog.FileName);
                }
            }
            log.LogMethodExit();
        }

        private void btnImageOpen_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (RequestId == -1)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(989), "Entry not saved");
                    log.LogMethodExit("RequestId == -1");
                    return;
                }
                btnImageOpen.Enabled = false;
                Process Proc = new Process();
                Proc.StartInfo.FileName = Path.Combine(GetSharedFolderPath(), Path.GetFileName(txtImageFile.Text));
                Proc.Start();
                btnImageOpen.Enabled = true;
            }
            catch (Exception ex)
            {
                btnImageOpen.Enabled = true;
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (userJobItemsDTO != null)
                {
                    if (userJobItemsDTO.MaintChklstdetId >= 0)
                    {
                        Publish.PublishUI publishUI = new Publish.PublishUI(utilities, userJobItemsDTO.MaintChklstdetId, "Maintenance Job/Service", userJobItemsDTO.TaskName.ToString());
                        publishUI.ShowDialog();
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Maintenance Job Publish");
                log.Fatal("Ends-btnPublishToSite_Click() Event with exception. Exception: " + ex.ToString());
            }
            log.LogMethodExit();
        }

        private void lnkDeactivateMachine_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                bool IsExists = false;
                if (MessageBox.Show(utilities.MessageUtils.getMessage(567) + cmbAssets.Text + "?", utilities.MessageUtils.getMessage("Confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    GenericAssetDTO assetDTO = new GenericAssetDTO();
                    assetDTO = assetList.GetAsset((int)cmbAssets.SelectedValue);
                    if (assetDTO == null)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(620));
                        log.LogMethodExit("assetDTO == null");
                        return;
                    }
                    else
                    {
                        if (assetDTO.Machineid == -1)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(620));
                            log.LogMethodExit("assetDTO.Machineid == -1");
                            return;
                        }

                        Machine machine;
                        
                        MachineList machineList = new MachineList();
                        MachineDTO machineDTO = machineList.GetMachine(assetDTO.Machineid);

                        if (machineDTO != null)
                        {
                            List<MachineAttributeDTO> updatedAttributeList = new List<MachineAttributeDTO>();

                            foreach (MachineAttributeDTO machineAttributeDTO in machineDTO.GameMachineAttributes)
                            {
                                if (machineAttributeDTO.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)
                                {
                                    IsExists = true;
                                    machineAttributeDTO.AttributeValue = "1";
                                    machineAttributeDTO.IsChanged = true;
                                }
                            }
                            if (!IsExists)
                            {

                                MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE, "1", "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE,"",false, machineUserContext.GetSiteId(),machineUserContext.GetUserId(),DateTime.Now,-1,machineUserContext.GetUserId(),DateTime.Now);
                                machineDTO.GameMachineAttributes.Add(updatedAttribute);
                            }
                            machine = new Machine(machineDTO);
                            machine.PutOutOfService("Setting out of service from service request", "For reason please check service request id: " + userJobItemsDTO.MaintChklstdetId.ToString());
                            MessageBox.Show(utilities.MessageUtils.getMessage(622));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
