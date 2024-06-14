/********************************************************************************************
* Project Name - Logger
* Description  - frmMonitor
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.70.2        09-Sep-2019    Jinto Thomas        Added logger for methods
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.GenericUI;
using Semnox.Parafait.GenericCommon;
using Semnox.Parafait.DataSet;


namespace Semnox.Parafait.logger
{
    public partial class frmMonitor : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmMonitor()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void frmMonitor_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            // TODO: This line of code loads data into the 'monitorDataSet.MonitorPriority' table. You can move, or remove it, as needed.
            this.monitorPriorityTableAdapter.Fill(this.monitorDataSet.MonitorPriority);
            this.monitorAssetTypeTableAdapter.Fill(this.monitorDataSet.MonitorAssetType);
            this.monitorAppModuleTableAdapter.Fill(this.monitorDataSet.MonitorAppModule);
            this.monitorApplicationTableAdapter.Fill(this.monitorDataSet.MonitorApplication);
            this.monitorTypeTableAdapter.Fill(this.monitorDataSet.MonitorType);
            this.monitorAssetTableAdapter.Fill(this.monitorDataSet.MonitorAsset);
            this.monitorTableAdapter.Fill(this.monitorDataSet.Monitor);

            Common.Utilities.setupDataGridProperties(ref dgvMonitorAsset);
            Common.setupGrid(ref dgvMonitorAsset);
            Common.Utilities.setupDataGridProperties(ref dgvMonitor);
            Common.setupGrid(ref dgvMonitor);
            Common.Utilities.setupDataGridProperties(ref dgvMonitorPriority);
            Common.setupGrid(ref dgvMonitorPriority);

            Common.setSiteIdFilter(monitorPriorityBindingSource);
            Common.setSiteIdFilter(monitorAssetTypeBindingSource);
            Common.setSiteIdFilter(monitorApplicationBindingSource);
            Common.setSiteIdFilter(monitorAppModuleBindingSource);
            Common.setSiteIdFilter(monitorAssetBindingSource);
            Common.setSiteIdFilter(monitorTypeBindingSource);

            if (Common.ParafaitEnv.IsCorporate && Common.ParafaitEnv.IsMasterSite)
                lnkPublishPriority.Visible = lnkPublish.Visible = true;
            else
                lnkPublishPriority.Visible = lnkPublish.Visible = false;

            lastUpdatedDateDataGridViewTextBoxColumn.DefaultCellStyle =
                lastUpdatedDateDataGridViewTextBoxColumn1.DefaultCellStyle = Common.Utilities.gridViewDateTimeCellStyle();

            intervalDataGridViewTextBoxColumn.DefaultCellStyle = Common.Utilities.gridViewNumericCellStyle();

            Timer delayedStart = new Timer();
            delayedStart.Tick += delayedStart_Tick;
            delayedStart.Interval = 5;
            delayedStart.Start();
            log.LogMethodExit();
        }

        void delayedStart_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            (sender as Timer).Stop();
            populateTree();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodExit();
            foreach (DataRow dr in monitorDataSet.MonitorAsset.Rows)
            {
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                {
                    dr["lastUpdatedDate"] = DateTime.Now;
                    dr["lastUpdatedBy"] = Common.ParafaitEnv.Username;
                }
            }
            try
            {
                this.monitorAssetTableAdapter.Update(this.monitorDataSet.MonitorAsset);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (Common.ContinueWithoutSaving(monitorDataSet))
            {
                this.monitorAssetTableAdapter.Fill(this.monitorDataSet.MonitorAsset);
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvMonitorAsset.Rows.Remove(dgvMonitorAsset.CurrentRow);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (Common.ContinueWithoutSaving(monitorDataSet))
                Close();
            log.LogMethodExit();
        }

        private void dgvMonitorAsset_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            Common.DGVDataError("Monitor Asset", dgvMonitorAsset, e);
            log.LogMethodExit();
        }

        private void dgvMonitor_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            Common.DGVDataError("Monitor", dgvMonitor, e);
            log.LogMethodExit();
        }

        private void btnSaveMonitor_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (DataRow dr in monitorDataSet.Monitor.Rows)
            {
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                {
                    dr["lastUpdatedDate"] = DateTime.Now;
                    dr["lastUpdatedBy"] = Common.ParafaitEnv.Username;
                }
            }
            try
            {
                this.monitorTableAdapter.Update(this.monitorDataSet.Monitor);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnRefreshMonitor_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (Common.ContinueWithoutSaving(monitorDataSet))
            {
                this.monitorAppModuleTableAdapter.Fill(this.monitorDataSet.MonitorAppModule);
                this.monitorApplicationTableAdapter.Fill(this.monitorDataSet.MonitorApplication);
                this.monitorTypeTableAdapter.Fill(this.monitorDataSet.MonitorType);
                this.monitorAssetTableAdapter.Fill(this.monitorDataSet.MonitorAsset);
                this.monitorTableAdapter.Fill(this.monitorDataSet.Monitor);
            }
            log.LogMethodExit();
        }

        private void btnDeleteMonitor_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvMonitor.Rows.Remove(dgvMonitor.CurrentRow);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnCloseMonitor_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (Common.ContinueWithoutSaving(monitorDataSet))
                Close();
            log.LogMethodExit();
        }

        private void dgvMonitor_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            e.Row.Cells["activeDataGridViewCheckBoxColumn"].Value = true;
            e.Row.Cells["intervalDataGridViewTextBoxColumn"].Value = 5;
            log.LogMethodExit();
        }

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                (new Reports.Monitor.frmViewMonitorLog(dgvMonitor.CurrentRow.Cells["monitorIdDataGridViewTextBoxColumn"].Value)).ShowDialog();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void tcMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (tcMonitor.SelectedTab.Equals(tpDashboard))
            {
                populateTree();
            }
            log.LogMethodExit();
        }

        void populateTree()
        {
            log.LogMethodEntry();
            tvSites.Nodes.Clear();
            tvSites.CheckBoxes = false;

            TreeNode node = new TreeNode("Company");
            node.Tag = -1;
            tvSites.Nodes.Add(node);

            DataTable dt = Common.Utilities.executeDataTable(@"select s.site_id, s.site_name, 
                                                                    (select top 1 1 from monitorErrorView mv where s.site_id = mv.site_id) error
                                                                from site s 
                                                                order by error desc, site_name");
            if (dt.Rows.Count == 0)
            {
                log.LogMethodExit();
                return;
            }
            else
            {

                foreach(DataRow dr in dt.Rows)
                {
                    TreeNode tnode = new TreeNode();
                    tnode = new TreeNode(dr["site_name"].ToString());
                    tnode.Tag = dr["site_id"];
                    if (dr["error"].ToString().Equals("1"))
                        tnode.BackColor = Color.Red;
                    else
                        tnode.BackColor = Color.Green;
                    tnode.ForeColor = Color.White;
                    node.Nodes.Add(tnode);
                }

                tvSites.Nodes[0].ExpandAll();

                try
                {
                    tvSites.SelectedNode = tvSites.Nodes[0].Nodes[0];
                }
                catch { }

                this.ActiveControl = tvSites;
            }
            log.LogMethodExit();
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            object savSelectedNode = tvSites.SelectedNode.Tag;
            populateTree();
            if (savSelectedNode != null && Convert.ToInt32(savSelectedNode) != -1)
            {
                foreach (TreeNode tn in tvSites.Nodes[0].Nodes)
                {
                    if (tn.Tag.Equals(savSelectedNode))
                    {
                        tvSites.SelectedNode = tn;
                        break;
                    }
                }
            }

            this.ActiveControl = tvSites;
            log.LogMethodExit();
        }

        private void tvSites_AfterSelect(object sender, TreeViewEventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = e.Node.Text;
            refreshLogDGV(e);
            log.LogMethodExit();
        }

        void refreshLogDGV(TreeViewEventArgs e)
        {
            log.LogMethodEntry();
            DataTable dt = Common.Utilities.executeDataTable(@"select MonitorName, AssetName, Status, Timestamp, AssetHostName, ApplicationName, ModuleName, LogText, LogKey, LogValue, MonitorId from MonitorStatusView where site_id = @site_id",
                                new System.Data.SqlClient.SqlParameter("@site_id", e.Node.Tag));
            dgvMonitorLog.DataSource = dt;

            Common.Utilities.setupDataGridProperties(ref dgvMonitorLog);
            dgvMonitorLog.Columns["Timestamp"].DefaultCellStyle = Common.Utilities.gridViewDateTimeCellStyle();
            foreach(DataGridViewRow dr in dgvMonitorLog.Rows)
            {
                if (dr.Cells["Timestamp"].Value != DBNull.Value)
                {
                    if ((DateTime.Now - Convert.ToDateTime(dr.Cells["Timestamp"].Value)).TotalMinutes > 10)
                        dr.Cells["Timestamp"].Style.BackColor = Color.Red;
                    else if (dr.Cells["Status"].Value.ToString() == "ERROR")
                        dr.Cells["Status"].Style.BackColor = Color.Red;
                    else if (dr.Cells["Status"].Value.ToString() == "WARNING")
                        dr.Cells["Status"].Style.BackColor = Color.LightGoldenrodYellow;
                }
                else
                    dr.Cells["Timestamp"].Style.BackColor = Color.Red;
            }
            log.LogMethodExit();
        }

        private void lnkOpenLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvMonitorLog.CurrentRow != null)
                    (new frmViewMonitorLog(dgvMonitorLog.CurrentRow.Cells["monitorId"].Value)).ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvMonitorPriority_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            Common.DGVDataError("Monitor Priority", dgvMonitorAsset, e);
            log.LogMethodExit();
        }

        private void btnSaveMasterData_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.monitorPriorityTableAdapter.Update(this.monitorDataSet.MonitorPriority);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnRefreshMasterData_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (Common.ContinueWithoutSaving(monitorDataSet))
                this.monitorPriorityTableAdapter.Fill(this.monitorDataSet.MonitorPriority);
            log.LogMethodExit();
        }

        private void btnDeleteMaserData_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvMonitorPriority.Rows.Remove(dgvMonitorPriority.CurrentRow);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnCloseMasterData_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (Common.ContinueWithoutSaving(monitorDataSet))
                Close();
            log.LogMethodExit();
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                object monitorId = dgvMonitor.CurrentRow.Cells["monitorIdDataGridViewTextBoxColumn"].Value;
                if (monitorId != null)
                {
                    HQPublish hqPublish = new HQPublish(Convert.ToInt32(monitorId), "Monitor", dgvMonitor.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString());
                    hqPublish.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void lnkPublishPriority_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                object priorityId = dgvMonitorPriority.CurrentRow.Cells["priorityIdDataGridViewTextBoxColumn"].Value;
                if (priorityId != null)
                {
                    HQPublish hqPublish = new HQPublish(Convert.ToInt32(priorityId), "MonitorPriority", dgvMonitorPriority.CurrentRow.Cells["nameDataGridViewTextBoxColumn2"].Value.ToString());
                    hqPublish.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }        
    }
}
