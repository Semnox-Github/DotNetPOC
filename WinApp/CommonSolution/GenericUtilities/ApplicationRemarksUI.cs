/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - ApplicationRemarks UI
 * 
 * 
 **************
 **Version Log
 **************
 *Version       Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 *2.70.3        17-Apr-2020            Archana        Added keyboard button      
 *2.80.0        17-Apr-2020            Guru S A       Redemption UI enhancement changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Application remarks user interface
    /// </summary>
    public partial class ApplicationRemarksUI : Form
    {
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities.Utilities utilities;
        string moduleName = "";
        string sourceName = "";
        string guid = "";
        /// <summary>
        /// delegate to save the record
        /// </summary>
        /// <param name="guid"> Guid of the object which calls the remark</param>
        /// <param name="applicationRemarksDTO">The remarks dto which has the entered remarks</param>
        public delegate void SaveRecord(string guid, ApplicationRemarksDTO applicationRemarksDTO);
        /// <summary>
        /// call back method 
        /// </summary>
        public SaveRecord setCallBack;
        private VirtualKeyboardController virtualKeyboard;
        public delegate void SetLastActivityTimeDelegate();
        public SetLastActivityTimeDelegate SetLastActivityTime;
        /// <summary>
        /// constructor for  ApplicationRemarksUI to view remarks
        /// </summary>
        public ApplicationRemarksUI(string moduleName, string sourceName, string guid, Utilities.Utilities _Utilities)
        {
            log.LogMethodEntry(moduleName, sourceName, guid, _Utilities);
            InitializeComponent();
            utilities = _Utilities;
            this.moduleName = moduleName;
            this.sourceName = sourceName;
            this.guid = guid;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            _Utilities.setupDataGridProperties(ref dgvViewRemarks);
            this.DialogResult = DialogResult.Cancel;
            PopulateRemarks(moduleName, sourceName, guid);
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            log.LogMethodExit();
        }

        private void PopulateRemarks(string moduleName, string sourceName, string guid)
        {
            log.LogMethodEntry(moduleName, sourceName, guid);
            BindingSource applicationRemarksListBS;
            ApplicationRemarksList applicationRemarksList = new ApplicationRemarksList(machineUserContext);
            List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> applicationRemarksSearchParams = new List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>>();
            applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ACTIVE_FLAG, "1"));
            applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if (!string.IsNullOrEmpty(moduleName))
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.MODULE_NAME, moduleName));
            applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME, sourceName));
            applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_GUID, guid));
            List<ApplicationRemarksDTO> applicationRemarksListOnDisplay = applicationRemarksList.GetAllApplicationRemarks(applicationRemarksSearchParams);
            applicationRemarksListBS = new BindingSource();
            if (applicationRemarksListOnDisplay != null)
            {
                SortableBindingList<ApplicationRemarksDTO> ApplicationRemarksDTOSortList = new SortableBindingList<ApplicationRemarksDTO>(applicationRemarksListOnDisplay);
                applicationRemarksListBS.DataSource = ApplicationRemarksDTOSortList;
            }
            else
                applicationRemarksListBS.DataSource = new SortableBindingList<ApplicationRemarksDTO>();
            dgvViewRemarks.DataSource = applicationRemarksListBS;
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            this.Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                ApplicationRemarksDTO applicationRemarksDTO = new ApplicationRemarksDTO();
                if (string.IsNullOrEmpty(txtRemarks.Text))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(201));
                    log.LogMethodExit();
                    return;
                }
                applicationRemarksDTO.Remarks = txtRemarks.Text;
                applicationRemarksDTO.ModuleName = moduleName;
                applicationRemarksDTO.SourceGuid = guid;
                applicationRemarksDTO.SourceName = sourceName;
                setCallBack(guid, applicationRemarksDTO);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured:" + ex.Message);
                log.Error("Ends-btnSave_Click() event. Exception:" + ex.ToString());
            }
            log.LogMethodExit();
        }

        private void FireSetLastActivityTime()
        {
            log.LogMethodEntry();
            if (SetLastActivityTime != null)
            {
                SetLastActivityTime();
            }
            log.LogMethodExit();
        }

        private void txtRemarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime(); 
            log.LogMethodExit();
        }
    }
}
