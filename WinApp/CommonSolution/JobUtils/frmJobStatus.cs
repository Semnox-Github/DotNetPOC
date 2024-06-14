
/********************************************************************************************
 * Project Name - 
 * Description  - frmJobStatus
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.2       18-Sep-2019    Dakshakh         Modified : Added logger
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// frmJobStatus UI
    /// </summary>
    public partial class frmJobStatus : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;

        /// <summary>
        /// Default constructor
        /// </summary>
        public frmJobStatus(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();

            utilities = _Utilities;
            utilities.setLanguage(this);
            _Utilities.setupDataGridProperties(ref DgvJobSchedule);
            DgvJobSchedule.BorderStyle = BorderStyle.FixedSingle;

            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            if (_Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }

            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            machineUserContext.SetUserId(utilities.ParafaitEnv.Username);

            startTimeDataGridViewTextBoxColumn.DefaultCellStyle = endTimeDataGridViewTextBoxColumn.DefaultCellStyle = _Utilities.gridViewDateTimeCellStyle();

            cmbStatus.SelectedIndex = 0;
            cmbPhase.SelectedIndex = 0;
        }

        /// <summary>
        /// frmJobStatus_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmJobStatus_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateProgramStatusGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// UpdateColors
        /// </summary>
        void UpdateColors()
        {
            log.LogMethodEntry();
            foreach (DataGridViewRow dr in DgvJobSchedule.Rows)
            {
                if (dr.Cells["statusDataGridViewTextBoxColumn"].Value.ToString().Equals("Error"))
                    dr.Cells["statusDataGridViewTextBoxColumn"].Style.SelectionBackColor =
                        dr.Cells["statusDataGridViewTextBoxColumn"].Style.BackColor = Color.Red;

                if (dr.Cells["phaseDataGridViewTextBoxColumn"].Value.ToString().Equals("Running"))
                    dr.Cells["phaseDataGridViewTextBoxColumn"].Style.SelectionBackColor =
                        dr.Cells["phaseDataGridViewTextBoxColumn"].Style.BackColor = Color.Green;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// PopulateProgramStatusGrid
        /// </summary>
        private void PopulateProgramStatusGrid()
        {
            log.LogMethodEntry();

            ConcurrentProgramJobStatusList concurrentProgramJobStatusList = new ConcurrentProgramJobStatusList();
            try
            {
                List<KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>> concurrentProgramStatusSearchParams = new List<KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>>();
               
                if(cmbStatus.SelectedIndex > 0)
                {
                    concurrentProgramStatusSearchParams.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.STATUS, cmbStatus.Text.ToString()));
                }
                if(cmbPhase.SelectedIndex > 0)
                {
                    concurrentProgramStatusSearchParams.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PHASE, cmbPhase.Text.ToString()));
                }

                concurrentProgramStatusSearchParams.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.START_TIME, dtrStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                concurrentProgramStatusSearchParams.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.END_TIME, dtrEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                List<ConcurrentProgramJobStatusDTO> concurentProgramStatusListOnDisplay = concurrentProgramJobStatusList.GetAllConcurrentProgramStatusList(concurrentProgramStatusSearchParams);
                BindingSource concurrentProgramStatusListBS = new BindingSource();

                if (concurentProgramStatusListOnDisplay != null)
                {
                    concurrentProgramStatusListBS.DataSource = concurentProgramStatusListOnDisplay;
                }
                else
                {
                    concurrentProgramStatusListBS.DataSource = new List<ConcurrentProgramJobStatusDTO>();
                }

                DgvJobSchedule.DataSource = concurrentProgramStatusListBS;
                UpdateColors();
            }
            catch { }

            log.LogMethodExit();
        }

        /// <summary>
        /// BtnRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateProgramStatusGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// BtnSearch_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateProgramStatusGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// BtnClose_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// DgvJobSchedule_DataBindingComplete
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void DgvJobSchedule_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry();
            UpdateColors();
            log.LogMethodExit();
        }
    }
}
