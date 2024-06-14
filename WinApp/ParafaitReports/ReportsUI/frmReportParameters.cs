/********************************************************************************************
* Project Name - Parafait Report
* Description  - UI of frmReportParameters 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        18-Sep-2019     Dakshakh raj        Modified : Added logs
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{

    /// <summary>
    /// frmReportParameters Class
    /// </summary>
    public partial class frmReportParameters : Form
    {
        int _reportId;
        private static readonly  Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        BindingSource reportParameterListBS;

        /// <summary>
        /// frmReportParameters Constructor
        /// </summary>
        /// <param name="ReportId">ReportId</param>
        public frmReportParameters(int ReportId)
        {
            log.LogMethodEntry(ReportId);
            try
            {
                InitializeComponent();
                if (Common.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(Common.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(Common.ParafaitEnv.LoginID);
                _reportId = ReportId;
                Common.setupVisuals(this);
                Common.Utilities.setLanguage(this);
                this.Width = Math.Max(1024, Screen.PrimaryScreen.Bounds.Width - 40);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-frmReportParameters(ReportId) constructor with exception: " + ex.ToString());
            }
        }

        /// <summary>
        /// frmReportParameters_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmReportParameters_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            // TODO: This line of code loads data into the 'parafaitDataSet.ReportParameters' table. You can move, or remove it, as needed.
            Common.Utilities.setupDataGridProperties(ref dgvReportParameters);
            PopulateReportParameters();
            dcLastUpdatedDate.DefaultCellStyle = Common.Utilities.gridViewDateTimeCellStyle();
            log.LogMethodExit();
        }

        /// <summary>
        /// PopulateReportParameters
        /// </summary>
        private void PopulateReportParameters()
        {
            log.LogMethodEntry();
            try
            {
                ReportsParameterList reportsParameterList = new ReportsParameterList();
                List<ReportParametersDTO> reportParametersDTOList = new List<ReportParametersDTO>();
                reportParametersDTOList = reportsParameterList.GetReportParameterListByReport(_reportId);

                reportParameterListBS = new BindingSource();

                if (reportParametersDTOList != null)
                    reportParameterListBS.DataSource = new SortableBindingList<ReportParametersDTO>(reportParametersDTOList);
                else
                {
                    reportParameterListBS.DataSource = new SortableBindingList<ReportScheduleDTO>();
                }

                reportParameterListBS.AddingNew += dgvReportParameters_BindingSourceAddNew;

                dgvReportParameters.DataSource = reportParameterListBS;
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Debug("Ends-PopulateReportParameters() method.");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// dgvReportParameters_BindingSourceAddNew
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvReportParameters_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvReportParameters.Rows.Count == reportParameterListBS.Count)
                {
                    reportParameterListBS.RemoveAt(reportParameterListBS.Count - 1);
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Debug("Ends-dgvReportParameters_BindingSourceAddNew() Event.");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// dgvReportParameters_DefaultValuesNeeded
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvReportParameters_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Row.Cells["operatorDataGridViewTextBoxColumn"].Value = "Default";
            e.Row.Cells["activeFlagDataGridViewCheckBoxColumn"].Value = true;
            e.Row.Cells["dcReportId"].Value = _reportId;
            e.Row.Cells["Mandatory"].Value = true;
            e.Row.Cells["dcLastUpdatedDate"].Value = DateTime.Now;
            e.Row.Cells["dcLastUpdatedUser"].Value = Common.ParafaitEnv.LoginID;
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSave_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                BindingSource reportParametersListBS = (BindingSource)dgvReportParameters.DataSource;
                var reportParametersListOnDisplay = (SortableBindingList<ReportParametersDTO>)reportParametersListBS.DataSource;
                if (reportParametersListOnDisplay.Count > 0)
                {
                    foreach (ReportParametersDTO reportParametersDTO in reportParametersListOnDisplay)
                    {
                        if (reportParametersDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(reportParametersDTO.ParameterName))
                            {
                                MessageBox.Show("Please enter the name.");
                                return;
                            }
                        }

                        ReportParameters reportParameters = new ReportParameters(reportParametersDTO);
                        reportParameters.Save();
                    }
                    PopulateReportParameters();
                }
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(371));
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnSave_Click() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// btnPreview_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnPreview_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                frmParameterView fpv = new frmParameterView(_reportId);
                fpv.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnDelete_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (MessageBox.Show("Are you sure you want to delete this parameter? This action is irrevocable.", "Confirm!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Common.Utilities.executeNonQuery(@"delete from reportparameterinlistvalues 
                                                    where ReportParameterValueId in 
                                                                (select ReportParameterValueId 
                                                                    from reportparametervalues 
                                                                    where ParameterId = @parameterId);
                                                   delete from reportparametervalues 
                                                    where ParameterId = @parameterId;
                                                    delete from ReportParameters 
                                                    where ParameterId = @parameterId;",
                                                        new System.Data.SqlClient.SqlParameter("@parameterId", dgvReportParameters.CurrentRow.Cells["ParameterId"].Value));
                    dgvReportParameters.Rows.Remove(dgvReportParameters.CurrentRow);
                    //this.reportParametersTableAdapter.Update(this.parafaitDataSet.ReportParameters);
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// btnCancel_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //if (parafaitDataSet.HasChanges())
            //{
            //    if (MessageBox.Show(Common.Utilities.MessageUtils.getMessage(712), "Refresh?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //        this.reportParametersTableAdapter.Fill(this.parafaitDataSet.ReportParameters);
            //}
        }

        /// <summary>
        /// dgvReportParameters_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvReportParameters_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                MessageBox.Show(Common.Utilities.MessageUtils.getMessage(585, Name, e.RowIndex + 1, dgvReportParameters.Columns[e.ColumnIndex].DataPropertyName) + ": " + e.Exception.Message, Common.Utilities.MessageUtils.getMessage("Data Error"));
            }
            catch { }
            e.Cancel = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmReportParameters_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (parafaitDataSet.HasChanges())
            //{
            //    if (MessageBox.Show(Common.Utilities.MessageUtils.getMessage(712), "Close?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            //        e.Cancel = true;
            //}
        }
    }
}
