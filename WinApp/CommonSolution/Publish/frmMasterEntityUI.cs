using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Publish
{
    /// <summary>
    /// UI class of frmMasterEntityUI
    /// </summary>
    public partial class frmMasterEntityUI : Form
    {
        bool intialBind = false;
        Utilities Utilities;
        MasterEntity masterEntityBL;
        string siteSearchCriteria = string.Empty;
        string masterSearchCriteria = string.Empty;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Dictionary<string, string> enteredEntityList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_utilities"></param>
        public frmMasterEntityUI(Utilities _utilities)
        {
            log.Debug("Starts-frmMasterEntityUI(_utilities) constructor");
            InitializeComponent();
            Utilities = _utilities;
            masterEntityBL = new MasterEntity(_utilities);
            log.Debug("ends-frmMasterEntityUI(_utilities) constructor");
        }

        private void frmMasterEntityUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmMasterEntityUI_Load(_utilities) event");

            tvOrganization.Nodes.Clear();

            DataTable dt = masterEntityBL.GetOrganizationDetails();
            foreach (DataRow dr in dt.Rows)
            {
                populateTree(dr["OrgId"], dr["OrgName"]);
            }

            LoadComboBoxValues();
            Utilities.setupDataGridProperties(ref dgvSiteData);
            Utilities.setupDataGridProperties(ref dgvMasterData);

            log.Debug("ends-frmMasterEntityUI_Load(_utilities) event");
        }

        #region Tree node population code
        void populateTree(object orgId, object Orgname)
        {
            log.Debug("Starts-populateTree() method");

            TreeNode node = new TreeNode(Orgname.ToString());
            node.Tag = orgId;
            tvOrganization.Nodes.Add(node);

            try
            {
                masterEntityBL.CheckOrganization(orgId);
            }
            catch
            {
                MessageBox.Show(Utilities.MessageUtils.getMessage(604));
                return;
            }
            getNodes(node);

            if (tvOrganization.Nodes.Count > 0)
            {
                tvOrganization.Nodes[0].ExpandAll();
                tvOrganization.Nodes[0].Text = tvOrganization.Nodes[0].Text; // reassign to set proper width for text
            }

            Utilities.setLanguage(tvOrganization);

            log.Debug("ends-populateTree() method");
        }

        TreeNode getNodes(TreeNode rootNode)
        {
            log.Debug("Starts-populateTree(rootNode) method");
            TreeNode[] tn = getChildren(Convert.ToInt32(rootNode.Tag));
            if (tn == null)
                return null;
            else
            {
                foreach (TreeNode tnode in tn)
                {
                    TreeNode node = getNodes(tnode);
                    if (node == null)
                        rootNode.Nodes.Add(tnode);
                    else
                        rootNode.Nodes.Add(node);
                }

                log.Debug("ends-populateTree(rootNode) method");
                return (rootNode);
            }
        }

        TreeNode[] getChildren(int parentOrgId)
        {
            log.Debug("Starts-getChildren(parentOrgId) method");
            DataTable dt = masterEntityBL.GetOrganizationChildren(parentOrgId);

            if (dt.Rows.Count == 0)
            {
                dt = masterEntityBL.GetChildSites(parentOrgId);

                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    TreeNode[] tnCollection = new TreeNode[dt.Rows.Count];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tnCollection[i] = new TreeNode(((dt.Rows[i]["SiteCode"] != DBNull.Value) ? dt.Rows[i]["SiteCode"].ToString() + " - " : "") + dt.Rows[i]["site_name"].ToString());
                        tnCollection[i].Tag = dt.Rows[i]["site_id"];
                        tnCollection[i].ForeColor = Color.Blue;
                    }
                    log.Debug("ends-getChildren(parentOrgId) method");
                    return (tnCollection);
                }
            }
            else
            {
                TreeNode[] tnCollection = new TreeNode[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tnCollection[i] = new TreeNode(dt.Rows[i]["OrgName"].ToString());
                    tnCollection[i].Tag = dt.Rows[i]["OrgId"];
                    tnCollection[i].ForeColor = Color.Gray;
                }

                log.Debug("ends-getChildren(parentOrgId) method");
                return (tnCollection);
            }
        }
#endregion

        void LoadComboBoxValues()
        {
            log.Debug("starts-LoadComboBoxValues() method");
            try
            {
                intialBind = true;
                LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> SearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                SearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "HQ_PUBLISH_TABLES"));
                //SearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, Convert.ToString(Utilities.ExecutionContext.GetSiteId())));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(SearchParameters);

                if (lookupValuesDTOList == null || lookupValuesDTOList.Count == 0)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }

                foreach (LookupValuesDTO d in lookupValuesDTOList)
                {
                    string[] var =  d.Description.Split('|');

                    d.Description = var[0];
                    if(var.Length > 1)
                    {
                        enteredEntityList.Add(d.LookupValue, var[1]); //ActiveFlag column name
                    }
                    else
                    {
                        enteredEntityList.Add(d.LookupValue, "");
                    }
                }

                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupId = -1;
                lookupValuesDTOList[0].LookupValue = "None";

                cmbTables.DataSource = lookupValuesDTOList;
                cmbTables.DisplayMember = "LookupValue";
                cmbTables.ValueMember = "LookupValueId";
                cmbTables.SelectedValue = -1;
            }
            catch (Exception ex)
            {
                log.Debug("ends-LoadComboBoxValues() method");
                MessageBox.Show(ex.Message);
            }

            intialBind = false;
            log.Debug("ends-LoadComboBoxValues() method");
        }

        bool IsEntityValid()
        {
            bool IsValid = true;

            //Check the active flag column name
            string value;
            if (enteredEntityList.TryGetValue(cmbTables.Text, out value) && !string.IsNullOrEmpty(value))
            {
               IsValid = masterEntityBL.IsActiveFlagValid(cmbTables.Text, value);
            }

            if (!IsValid)
                return IsValid;

            //Check first column is primary key or not
            string columnNames = ((LookupValuesDTO)(cmbTables.SelectedItem)).Description;

            if(!string.IsNullOrEmpty(columnNames))
            {
                string[] var = columnNames.Split(',');
                IsValid = masterEntityBL.IsPrimaryKey(cmbTables.Text, var[0].Trim());
            }

            return IsValid;
        }

        private void cmbTables_SelectedValueChanged(object sender, EventArgs e)
        {
            log.Debug("starts-cmbTables_SelectedValueChanged() event");
           
            if (!intialBind)
            {
                try
                {
                    if (Convert.ToInt32(cmbTables.SelectedValue) > 0)
                    {
                        masterSearchCriteria = "";
                        siteSearchCriteria = "";

                        if (PopulateDgvMasterData()) //Check master data populated successfully
                        {
                            PopulateDgvSiteData(); //populate site records
                        }
                        else
                        {
                            dgvMasterData.DataSource = null;
                            dgvSiteData.DataSource = null;
                        }
                    }
                    else
                    {
                        dgvMasterData.DataSource = null;
                        dgvSiteData.DataSource = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            log.Debug("ends-cmbTables_SelectedValueChanged() event");
        }
        
        bool PopulateDgvMasterData()
        {
            log.Debug("starts-PopulateDgvMasterData() method");
            dgvMasterData.DataSource = null;
            int siteId = GetSelectedSiteId();

            if (siteId > -1)
            {
                if (IsEntityValid())
                {
                    string columnNames = ((LookupValuesDTO)(cmbTables.SelectedItem)).Description;

                    if (!string.IsNullOrEmpty(columnNames))
                    {
                        DataTable matserDT = masterEntityBL.GetMasterDetails(cmbTables.Text.ToString(), columnNames, siteId, masterSearchCriteria);
                        dgvMasterData.DataSource = matserDT;
                        Utilities.setupDataGridProperties(ref dgvMasterData);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show(Utilities.MessageUtils.getMessage(1188), Utilities.MessageUtils.getMessage("Validation"));
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(Utilities.MessageUtils.getMessage(1187), Utilities.MessageUtils.getMessage("Validation"));
                    return false;
                }
            }
            else
            {
                log.Debug("ends-PopulateDgvMasterData() method");
                return false;
            }
        }

        void PopulateDgvSiteData()
        {
            log.Debug("Starts-PopulateDgvSiteData() method");
            intialBind = true;
            dgvSiteData.DataSource = null;
            intialBind = false;
            int siteId = GetSelectedSiteId();

            if (siteId > -1)
            {
                if (IsEntityValid())
                {
                    string columnNames = ((LookupValuesDTO)(cmbTables.SelectedItem)).Description;

                    if (!string.IsNullOrEmpty(columnNames))
                    {
                        DataTable siteDataDT = masterEntityBL.GetSiteDetails(cmbTables.Text.ToString(), columnNames, siteId, siteSearchCriteria);
                        dgvSiteData.DataSource = siteDataDT;
                        Utilities.setupDataGridProperties(ref dgvSiteData);
                    }
                    else
                    {
                        MessageBox.Show(Utilities.MessageUtils.getMessage(1188), Utilities.MessageUtils.getMessage("Validation"));
                    }
                }
                else
                {
                    MessageBox.Show(Utilities.MessageUtils.getMessage(1187), Utilities.MessageUtils.getMessage("Validation"));
                }
            }

            siteSearchCriteria = "";
            #region Make first column is editable
            if (dgvSiteData.Rows.Count > 0)
            {
                for (int i = 0; i < dgvSiteData.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        dgvSiteData.Columns[i].ReadOnly = false;
                    }
                    else
                    {
                        dgvSiteData.Columns[i].ReadOnly = true;
                    }
                }
            }
            #endregion
            log.Debug("ends-PopulateDgvSiteData() method");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("starts-btnClose_Click() event");
            this.Close();
            log.Debug("ends-btnClose_Click() event");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("starts-btnSave_Click() event");
            try
            {
                List<EntityDetails> lstEntityList = new List<EntityDetails>();
                EntityDetails entityDetails;
                int selectedSiteId = GetSelectedSiteId();
              
                if (selectedSiteId > -1 && dgvSiteData.Rows.Count > 0)
                {
                    foreach (DataGridViewRow rw in dgvSiteData.Rows)
                    {
                        if(!string.IsNullOrEmpty(rw.Cells[0].Value.ToString()))
                        {
                            entityDetails = new EntityDetails();
                            entityDetails.sitePkId = Convert.ToInt32(rw.Cells[1].Value);
                            entityDetails.publishEntityPKId = Convert.ToInt32(rw.Cells[0].Value);
                            entityDetails.entityName = cmbTables.Text;
                            entityDetails.selectedSiteId = selectedSiteId;

                            //Get the valid flag column name
                            string value = "";
                            enteredEntityList.TryGetValue(cmbTables.Text, out value);
                            entityDetails.activeFlagColumnName = value;

                            lstEntityList.Add(entityDetails);
                        }
                    }

                    bool published = false;
                    if (lstEntityList.Count > 0)
                    {
                       if (DialogResult.Yes == MessageBox.Show(Utilities.MessageUtils.getMessage(1191),  Utilities.MessageUtils.getMessage("Confirmation"), MessageBoxButtons.YesNo))
                       {
                           published = masterEntityBL.PublishAllEntities(lstEntityList);

                           if (published)
                           {
                               MessageBox.Show(cmbTables.Text + " " + Utilities.MessageUtils.getMessage(691));
                               cmbTables_SelectedValueChanged(null, null);
                           }
                       }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.Debug("ends-btnSave_Click() event");
        }

        private void DeselectAllNodes(TreeNode currentNode)
        {
            log.Debug("starts-DeselectAllNodes(currentNode) method");
            var allNodes = tvOrganization.Nodes.Cast<TreeNode>().SelectMany(GetNodeBranch);

            foreach (var treeNode in allNodes)
            {
                if(treeNode != currentNode)
                treeNode.Checked = false;
            }
            log.Debug("ends-DeselectAllNodes(currentNode) method");
        }

        private IEnumerable<TreeNode> GetNodeBranch(TreeNode node)
        {
            log.Debug("ends-GetNodeBranch(node) method");
            yield return node;

            foreach (TreeNode child in node.Nodes)
                foreach (var childChild in GetNodeBranch(child))
                    yield return childChild;

            log.Debug("ends-GetNodeBranch(node) method");
        }

        private void tvOrganization_AfterCheck(object sender, TreeViewEventArgs e)
        {
            log.Debug("starts-tvOrganization_AfterCheck() event");
            try
            {
                if (e.Node.Checked && e.Node.ForeColor == Color.Gray)
                {
                    e.Node.Checked = false;
                    MessageBox.Show(Utilities.MessageUtils.getMessage(1186), Utilities.MessageUtils.getMessage("Validation"));
                    return;
                }

                if (e.Node.Checked && e.Node.ForeColor == Color.Blue)
                {
                    DeselectAllNodes(e.Node);
                    cmbTables_SelectedValueChanged(null, null);
                }
            }
            catch { }
            log.Debug("ends-tvOrganization_AfterCheck() event");
        }

        int GetSelectedSiteId()
        {
            log.Debug("Starts-GetSelectedSiteId() event");
            int siteId = -1;
            var allNodes = tvOrganization.Nodes.Cast<TreeNode>().SelectMany(GetNodeBranch);

            foreach (var treeNode in allNodes)
            {
                if(treeNode.Checked && treeNode.ForeColor == Color.Blue)
                {
                    siteId = Convert.ToInt32(treeNode.Tag);
                    break;
                }
            }

            log.Debug("ends-GetSelectedSiteId() event");
            return siteId;
        }

        //Advance Search for master records
        private void btnMasterSearchParameter_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnMasterSearchParameter_Click() event");
            try
            {
                masterSearchCriteria = "";
                AdvanceSearchUI advanceSearch = new AdvanceSearchUI(Utilities, cmbTables.Text, "P1");
                advanceSearch.ShowDialog();

                if (!string.IsNullOrEmpty(advanceSearch.searchCriteria))
                {
                    masterSearchCriteria = advanceSearch.searchCriteria;
                    if (Convert.ToInt32(cmbTables.SelectedValue) > 0)
                    {
                        PopulateDgvMasterData();
                    }
                    else
                    {
                        cmbTables_SelectedValueChanged(null, null);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.Debug("Ends-btnMasterSearchParameter_Click() event");
        }

        //Advance Search for site records
        private void btnSiteSearchParameter_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSiteSearchParameter_Click() event");
            try
            {
                siteSearchCriteria = "";
                AdvanceSearchUI advanceSearch = new AdvanceSearchUI(Utilities, cmbTables.Text, "");
                advanceSearch.ShowDialog();

                if (!string.IsNullOrEmpty(advanceSearch.searchCriteria))
                {
                    siteSearchCriteria = advanceSearch.searchCriteria;
                    if (Convert.ToInt32(cmbTables.SelectedValue) > 0)
                    {
                        PopulateDgvSiteData();
                    }
                    else
                    {
                        cmbTables_SelectedValueChanged(null, null);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.Debug("Ends-btnSiteSearchParameter_Click() event");
        }

        private void dgvSiteData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.Debug("Starts-dgvSiteData_CellValidating() event");
            if (dgvSiteData.Rows.Count > 0 && !intialBind)
            {
                if(e.ColumnIndex == 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(e.FormattedValue.ToString()))
                        {
                            #region Check the number is valid or not
                            try
                            {
                                Convert.ToInt32(e.FormattedValue);
                                e.Cancel = false;
                            }
                            catch
                            {
                                MessageBox.Show(Utilities.MessageUtils.getMessage(646), Utilities.MessageUtils.getMessage("Validation"));
                                e.Cancel = true;
                                return;
                            }
                            #endregion

                            #region Check the number is exists in master data
                            if (!IsValidPrimaryNumber(e.FormattedValue))
                            {
                                MessageBox.Show(Utilities.MessageUtils.getMessage(1189), Utilities.MessageUtils.getMessage("Validation"));
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            #endregion

                            #region Check the number is duplicate
                            if (IsNumberDuplicate(e.FormattedValue, e.RowIndex))
                            {
                                MessageBox.Show(Utilities.MessageUtils.getMessage(1190), Utilities.MessageUtils.getMessage("Validation"));
                                e.Cancel = true;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            #endregion
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            log.Debug("Ends-dgvSiteData_CellValidating() event");
        }

        bool IsValidPrimaryNumber(object number)
        {
            log.Debug("Starts-IsValidPrimaryNumber(object number) method");
            foreach (DataGridViewRow rw in dgvMasterData.Rows)
            {
                if (!string.IsNullOrEmpty(rw.Cells[0].Value.ToString()))
                {
                    if (rw.Cells[0].Value.ToString().Equals(number.ToString()))
                    {
                        return true;
                    }
                }
            }
            log.Debug("Ends-IsValidPrimaryNumber(object number) method");
            return false;
        }

        bool IsNumberDuplicate(object number, int rowIndex)
        {
            log.Debug("Starts-IsNumberDuplicate(object number, int rowIndex) method");
            foreach (DataGridViewRow rw in dgvSiteData.Rows)
            {
                if (!string.IsNullOrEmpty(rw.Cells[0].Value.ToString()) && rw.Index != rowIndex)
                {
                    if (rw.Cells[0].Value.ToString().Equals(number.ToString()))
                    {
                        return true;
                    }
                }
            }
            log.Debug("Ends-IsNumberDuplicate(object number, int rowIndex) method");
            return false;
        }
    }
}
