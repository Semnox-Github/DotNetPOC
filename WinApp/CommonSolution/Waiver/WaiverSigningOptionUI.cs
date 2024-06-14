/********************************************************************************************
 * Project Name - Waiver
 * Description  - WaiverSigningOptionUI class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Girish kundar    Modified :Added Logger methods and Removed Unused namespace's. 
 *2.80          27-Jun-2020   Deeksha          Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    /// WaiverSigningOptionUI  UI
    /// </summary>
    /// 
    public partial class WaiverSigningOptionUI : Form
    {

       private int WaiverSetId = -1;
       private Utilities utilities;
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
       private BindingSource waiverSetListBS = new BindingSource();
       private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Constructor of WaiverSigningOptionUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// 
        public WaiverSigningOptionUI(int waiverSetId, Utilities utilities)
        {
            log.LogMethodEntry(waiverSetId);
            WaiverSetId = waiverSetId;
       
            InitializeComponent();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            loadLookUps();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvWaiverSigningOption);
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();

        }

       

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            loadLookUps();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit(null);
        }

        public void loadLookUps()
        {
            log.LogMethodEntry();
            string lookupName = "WAIVER_SIGNING_OPTION";
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<LookupValuesDTO> LookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchlookupParameters);

            if (LookupValuesDTOList != null && LookupValuesDTOList.Count > 0)
            {
                SortableBindingList<LookupValuesDTO> sblookupValuesList = new SortableBindingList<LookupValuesDTO>(LookupValuesDTOList);
                dgvWaiverSigningOption.DataSource = sblookupValuesList;
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry("Inside the Save() method");
            if (dgvWaiverSigningOption.CurrentRow == null || dgvWaiverSigningOption.CurrentRow.IsNewRow || dgvWaiverSigningOption.CurrentRow.Cells[0].Value == DBNull.Value)
                return;

            else
            {
                foreach (DataGridViewRow row in dgvWaiverSigningOption.Rows)
                {
                    int lookupValueId = Convert.ToInt32(row.Cells["LookupValueId"].Value);

                    WaiverSetSigningOptionsBL waiverOption = null;
                    WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO = getWaiverSetSigningOptionsDTO(lookupValueId);
                    if (Convert.ToBoolean(row.Cells["chkWaiverSignedEnable"].Value))
                    {
                        waiverSetSigningOptionsDTO.WaiverSetId = WaiverSetId;
                        waiverSetSigningOptionsDTO.LookupValueId = lookupValueId;
                        waiverOption = new WaiverSetSigningOptionsBL(machineUserContext,waiverSetSigningOptionsDTO);
                        waiverOption.Save();
                    }
                    else
                    {
                        if (waiverSetSigningOptionsDTO.Id >= 0)
                        {
                            log.Debug("Deleting waiverSetSigningOptionsDTO");
                            waiverOption = new WaiverSetSigningOptionsBL(machineUserContext, waiverSetSigningOptionsDTO);
                            int status = waiverOption.Delete();
                        }
                    }
                   
                }
            }
            log.LogMethodExit();
        }

        private WaiverSetSigningOptionsDTO getWaiverSetSigningOptionsDTO(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            WaiverSetSigningOptionsListBL signingOptionList = new WaiverSetSigningOptionsListBL(machineUserContext);
            List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchChannelParameters = new List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>>();
            searchChannelParameters.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.LOOKUP_VALUE_ID, lookupValueId.ToString()));
            searchChannelParameters.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.WAIVER_SET_ID, WaiverSetId.ToString()));
            searchChannelParameters.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTO = signingOptionList.GetWaiverSetSigningOptionsList(searchChannelParameters);
            if (waiverSetSigningOptionsDTO.Count > 0) 
            {
                log.LogMethodExit(waiverSetSigningOptionsDTO[0]);
                return waiverSetSigningOptionsDTO[0];
            }
            else
            {
                log.LogMethodExit("Returning the empty waiverSetSigningOptionsDTO");
                return new WaiverSetSigningOptionsDTO();
            }
            
        }

        private void dgvWaiverSigningOption_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvWaiverSigningOption.CurrentRow == null || dgvWaiverSigningOption.CurrentRow.IsNewRow || dgvWaiverSigningOption.CurrentRow.Cells[0].Value == DBNull.Value)
                return;
            else
            {
                foreach (DataGridViewRow row in dgvWaiverSigningOption.Rows)
                {
                    int lookupValueId = Convert.ToInt32(row.Cells["LookupValueId"].Value);
                    WaiverSetSigningOptionsListBL waiverOption = new WaiverSetSigningOptionsListBL(machineUserContext);
                    WaiverSetSigningOptionsDTO waiverSetDetailSigningOptionsDTO = getWaiverSetSigningOptionsDTO(lookupValueId);
                    row.Cells["chkWaiverSignedEnable"].Value = (waiverSetDetailSigningOptionsDTO.WaiverSetId == -1 ? false : true);
                }
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvWaiverSigningOption.AllowUserToAddRows = true;
                dgvWaiverSigningOption.ReadOnly = false;
                btnSave.Enabled = true;
            }
            else
            {
                dgvWaiverSigningOption.AllowUserToAddRows = false;
                dgvWaiverSigningOption.ReadOnly = true;
                btnSave.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
