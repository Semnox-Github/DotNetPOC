
/********************************************************************************************
 * Project Name - Concurrent Program Aarguments UI
 * Description  - User interface for concurrent Programs Arguments
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       25-Feb-2016    Amaresh          Created 
 *2.70.2       13-Aug-2019    Deeksha         Added logger methods.
 *2.90.0       13-Jun-2020    Faizan          Modified : Rest API phase -2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Program Argument UI
    /// </summary>

    public partial class ConcurrentProgramArgumentsUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;

        int argumentsProgramId;

        /// <summary>
        /// Parameterized constructor of ConcurrentProgramUI
        /// </summary>
        /// <param name="_Utilities">Utilities object as parameter</param>
        /// <param name="programId">Program Id to load</param>
        /// <param name="progamName">Program name to display</param>

        public ConcurrentProgramArgumentsUI(int programId, string progamName, Utilities _Utilities)
        {
            log.LogMethodEntry(programId, progamName, _Utilities);
            InitializeComponent();
            argumentsProgramId = programId;
            LoadDetails(argumentsProgramId);
            utilities = _Utilities;
            _Utilities.setupDataGridProperties(ref ConcurrentProgramArgumentsGridView);
            _Utilities.setLanguage(this);

            lblProgramName.Text = "Program Name:  " + progamName;

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
      
            //Binding data to Execution Method Dropdown
            DataTable dt = new DataTable();
            dt.Columns.Add("ArgumentType");
            dt.Columns.Add("Desc");
            dt.Rows.Add("S", "String");
            dt.Rows.Add("I", "Int");
            dt.Rows.Add("B", "Boolean");
            dt.Rows.Add("D", "DateTime");
            argumentTypeDataGridViewComboBoxColumn.DataSource = dt;
            argumentTypeDataGridViewComboBoxColumn.ValueMember = "ArgumentType";
            argumentTypeDataGridViewComboBoxColumn.DisplayMember = "Desc";

            log.LogMethodExit();
        }

        private void LoadDetails(int programId)
        {
            log.LogMethodEntry(programId);

          //  ConcurrentProgramArguments concurrentProgramArguments = new ConcurrentProgramArguments(utilities.ExecutionContext);

            ConcurrentProgramArgumentList concurentProgramArgumentsList = new ConcurrentProgramArgumentList();
            List<KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>>();
            searchParameters.Add(new KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>(ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.PROGRAM_ID, programId.ToString()));            
            if(ChkBoxActive.Checked)
            {
                searchParameters.Add(new KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>(ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.IS_ACTIVE, "1"));
            }
            List<ConcurrentProgramArgumentsDTO> concurentProgramArgumentsListOnDisplay = concurentProgramArgumentsList.GetConcurrentProgramArguments(searchParameters);

            BindingSource concurrentProgramsArgumentsListBS = new BindingSource();

            if (concurentProgramArgumentsListOnDisplay != null)
            {
                concurrentProgramsArgumentsListBS.DataSource = concurentProgramArgumentsListOnDisplay;
            }
            else
            {
                concurrentProgramsArgumentsListBS.DataSource = new List<ConcurrentProgramArgumentsDTO>();
            }

            ConcurrentProgramArgumentsGridView.DataSource = concurrentProgramsArgumentsListBS;

            log.LogMethodExit();
        }  

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            BindingSource concurrentProgramArgumentsBS = (BindingSource)ConcurrentProgramArgumentsGridView.DataSource;
            var concurrentProgramArgumentsListOnDisplay = (List<ConcurrentProgramArgumentsDTO>)concurrentProgramArgumentsBS.DataSource;

            if (concurrentProgramArgumentsBS != null && concurrentProgramArgumentsBS.Count > 0)
            {
                try
                {
                    foreach (ConcurrentProgramArgumentsDTO concurrentProgramArgumentsDTO in concurrentProgramArgumentsListOnDisplay)
                    {
                        if (concurrentProgramArgumentsDTO.ArgumentId == -1 && concurrentProgramArgumentsDTO.IsActive == false)
                        {
                            continue;
                        }
                        concurrentProgramArgumentsDTO.ProgramId = argumentsProgramId;
                        ConcurrentProgramArguments concurrentProgramArguments = new ConcurrentProgramArguments(utilities.ExecutionContext,concurrentProgramArgumentsDTO);
                        concurrentProgramArguments.Save();
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }

            LoadDetails(argumentsProgramId);
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void ChkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadDetails(argumentsProgramId);
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadDetails(argumentsProgramId);
            log.LogMethodExit();
        }
    }
}
