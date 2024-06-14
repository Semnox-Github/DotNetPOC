/********************************************************************************************
* Project Name - PurchaseTaxUI
* Description  - PurchaseTaxUI form
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*2.60       10-Apr-2019    Girish           Modified : Replaced purchase Tax 3 tier with Tax 3 tier 
*2.70.2       02-Aug-2019    Girish           Modified : Used Three Tier structure to Bind the Tax GridView
*                                                      Enabling Multiple record Update.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.Inventory
{
    public partial class PurchaseTaxUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext;
        private Utilities utilities;
        private List<TaxStructureDTO> taxStructureListOnDisplay = null;
        private List<TaxStructureDTO> taxStructureDTOList = null;
        private SortableBindingList<TaxDTO> taxSortableBindingList = null;
        private DataGridView dgvfocused = null;

        public PurchaseTaxUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            machineUserContext = _Utilities.ExecutionContext;
            utilities = _Utilities;
            utilities.setLanguage(this);
            LoadAll();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to set up the grid view
        /// </summary>
        private void DGVSetUp()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvTax);
            utilities.setupDataGridProperties(ref dgvTaxStructure);
            this.taxPercentageDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            this.taxPercentageDataGridViewTextBoxColumn.DefaultCellStyle.Format = "N4";
            this.percentageDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            this.percentageDataGridViewTextBoxColumn.DefaultCellStyle.Format = "N4";
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }

            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublishToSite.Visible = false;
            }
            else
            {
                lnkPublishToSite.Visible = false;
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Method to load all other methods.
        /// </summary>
        private void LoadAll()
        {
            log.LogMethodEntry();
            DGVSetUp();
            PopulatePurchaseTax();
            log.LogMethodExit();
        }


        /// <summary>
        /// Method to load the purchase tax to grid view
        /// </summary>
        private void PopulatePurchaseTax()
        {
            log.LogMethodEntry();
            try
            {
                TaxList purchaseTaxList = new TaxList(machineUserContext);
                List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> purchaseTaxSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                purchaseTaxSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<TaxDTO> purchaseTaxListOnDisplay = purchaseTaxList.GetAllTaxes(purchaseTaxSearchParams, true, false);
                taxSortableBindingList = new SortableBindingList<TaxDTO>();
                if (purchaseTaxListOnDisplay == null || purchaseTaxListOnDisplay.Any() == false)
                {
                    purchaseTaxListOnDisplay = new List<TaxDTO>();
                }
                taxSortableBindingList = new SortableBindingList<TaxDTO>(purchaseTaxListOnDisplay);
                purchaseTaxDTOBindingSource.DataSource = taxSortableBindingList;
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This method handles the current row changed for the binding source
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvTax_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                taxStructureDTOBindingSource.DataSource = null;
                if (purchaseTaxDTOBindingSource.Current != null 
                    && purchaseTaxDTOBindingSource.Current is TaxDTO)
                {
                    if ((purchaseTaxDTOBindingSource.Current as TaxDTO).TaxStructureDTOList != null 
                        && (purchaseTaxDTOBindingSource.Current as TaxDTO).TaxStructureDTOList.Count > 0)
                    {
                        taxStructureDTOList = (purchaseTaxDTOBindingSource.Current as TaxDTO).TaxStructureDTOList;
                    }
                    else
                    {
                        (purchaseTaxDTOBindingSource.Current as TaxDTO).TaxStructureDTOList = new List<TaxStructureDTO>();
                        taxStructureDTOList = (purchaseTaxDTOBindingSource.Current as TaxDTO).TaxStructureDTOList;
                    }
                }
                else
                {
                    taxStructureDTOBindingSource.DataSource = new List<TaxStructureDTO>();
                }
                if (purchaseTaxDTOBindingSource.Current != null)
                {
                    TaxDTO taxDTO = (purchaseTaxDTOBindingSource.Current as TaxDTO);
                    if (taxDTO != null)
                    {
                        PopulateParentStructure(taxDTO.TaxId);
                    }
                }
                taxStructureDTOBindingSource.DataSource = taxStructureDTOList;
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// This method load the parent tax structures to the combo box in the tax structure grid view 
        /// </summary>
        /// <param name="taxId">taxId</param>
        private void PopulateParentStructure(int taxId)
        {
            log.LogMethodEntry(taxId);
            try
            {
                TaxStructureList taxstructureList = new TaxStructureList(machineUserContext);
                List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>> SearchByTaxStructureParameters = new List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>>();
                SearchByTaxStructureParameters.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.TAX_ID, taxId.ToString()));
                taxStructureListOnDisplay = taxstructureList.GetTaxStructureList(SearchByTaxStructureParameters);
                if (taxStructureListOnDisplay == null)
                {
                    taxStructureListOnDisplay = new List<TaxStructureDTO>();
                }
                TaxStructureDTO taxStructureDTO = new TaxStructureDTO();
                taxStructureListOnDisplay.Insert(0, taxStructureDTO);
                taxStructureListOnDisplay[0].StructureName = "Select";
                parentStructureIdDataGridViewComboBoxColumn.DataSource = taxStructureListOnDisplay;
                parentStructureIdDataGridViewComboBoxColumn.DisplayMember = "StructureName";
                parentStructureIdDataGridViewComboBoxColumn.ValueMember = "TaxStructureId";
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit();
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// This method handles the adding new record to the tax binding data source
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void taxDTOBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            TaxDTO taxDTO = new TaxDTO();
            taxDTO.TaxStructureDTOList = new List<TaxStructureDTO>();
            e.NewObject = taxDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method handles the dgvTaxStructure enter event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaxStructure_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method checks for the DTO has modified 
        /// </summary>
        /// <returns>bool</returns>
        private bool HasChanges()
        {
            log.LogMethodEntry();
            bool result = false;
            SortableBindingList<TaxDTO> taxDTOList = new SortableBindingList<TaxDTO>();
            taxDTOList = (SortableBindingList<TaxDTO>)purchaseTaxDTOBindingSource.DataSource;

            if (taxDTOList != null && taxDTOList.Count > 0)
            {
                foreach (var TaxDTO in taxDTOList)
                {
                    if (TaxDTO.IsChangedRecursive)
                    {
                        result = true;
                        TaxDTO.IsChanged = result;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// This method handles the save for the tax structure 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!HasChanges())
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371));
                log.LogMethodExit("HasChanges() is false");
                dgvTax.CurrentCell = dgvTax.Rows[0].Cells[0];
                dgvTax.CurrentCell.Selected = true;
                return;
            }

            try
            {
                dgvTaxStructure.EndEdit();
                dgvTax.EndEdit();
                SortableBindingList<TaxDTO> taxDTOList = new SortableBindingList<TaxDTO>();
                taxDTOList = (SortableBindingList<TaxDTO>)purchaseTaxDTOBindingSource.DataSource;

                if (taxDTOList != null && taxDTOList.Count > 0)
                {
                    foreach (TaxDTO taxDTO in taxDTOList)
                    {
                        if (taxDTO.IsChanged)
                        {
                            if (!Validate(taxDTO))
                            {
                                log.LogMethodExit("!Validate(taxDTO)");
                                return;
                            }
                            else
                            {
                                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                                {
                                    try
                                    {
                                        parafaitDBTrx.BeginTransaction();
                                        Tax tax = new Tax(machineUserContext, taxDTO);
                                        tax.Save(parafaitDBTrx.SQLTrx);
                                        parafaitDBTrx.EndTransaction();
                                    }
                                    catch (Exception ex)
                                    {
                                        parafaitDBTrx.RollBack();
                                        MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + " " + ex.Message, utilities.MessageUtils.getMessage("Save Error"));
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    calculateTotalTaxPercentage();
                    MessageBox.Show(utilities.MessageUtils.getMessage(122));
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));

                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + " " + ex.Message, "Save Error");
                log.Error("Error in save. Exception:" + ex.ToString());
            }

        }

        /// <summary>
        /// This method  the validate the tax DTO 
        /// </summary>
        /// <param name="taxDTO"></param>
        /// <returns></returns>
        private bool Validate(TaxDTO taxDTO)
        {
            log.LogMethodEntry(taxDTO);
            if (string.IsNullOrEmpty(taxDTO.TaxName))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144, utilities.MessageUtils.getMessage("Tax Name")));
                log.LogMethodExit(false, utilities.MessageUtils.getMessage(1144, utilities.MessageUtils.getMessage("Tax Name")));
                return false;
            }
            if (string.IsNullOrEmpty(taxDTO.TaxPercentage.ToString()))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144, utilities.MessageUtils.getMessage("Tax Percentage")));
                log.LogMethodExit(false, utilities.MessageUtils.getMessage(1144, utilities.MessageUtils.getMessage("Tax Percentage")));
                return false;
            }
            return true;
        }


        /// <summary>
        /// This method calculates the tax percentages  for the taxes 
        /// </summary>
        void calculateTotalTaxPercentage()
        {
            log.LogMethodEntry();
            SqlCommand cmdTax = utilities.getCommand();
            cmdTax.CommandText = "select tax_id from tax";
            SqlDataAdapter daTax = new SqlDataAdapter(cmdTax);
            DataTable dtTax = new DataTable();
            daTax.Fill(dtTax);

            SqlCommand cmd = utilities.getCommand();
            for (int j = 0; j < dtTax.Rows.Count; j++)
            {
                cmd.CommandText = "select TaxStructureId from taxStructure where taxId = @taxId and ParentStructureId is null";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@taxId", dtTax.Rows[j]["tax_id"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                    continue;
                cmd.CommandText = @"WITH n(TaxStructureId, ParentStructureId, Percentage, effPercentage, level) AS 
                           (SELECT TaxStructureId, ParentStructureId, Percentage, convert(decimal(10, 4), Percentage) effPercentage, 1 as level
                            FROM TaxStructure
                            WHERE taxstructureId = @id
                                UNION ALL
                            SELECT nplus1.TaxStructureId, nplus1.ParentStructureId, nplus1.Percentage, Convert(decimal(10, 4), n.effPercentage*nplus1.Percentage), n.level + 1
                            FROM TaxStructure as nplus1, n
                            WHERE n.TaxStructureId = nplus1.ParentStructureId)    
                        SELECT sum(effPercentage / (power(100, level-1))) FROM n";
                decimal totalTaxPer = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", dt.Rows[i]["TaxStructureId"]);
                    totalTaxPer += Convert.ToDecimal(cmd.ExecuteScalar());
                }
                cmd.CommandText = "Update tax set tax_percentage = @taxPer where tax_percentage != @taxPer and tax_id = @tax_id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@tax_id", dtTax.Rows[j]["tax_id"]);
                cmd.Parameters.AddWithValue("@taxPer", totalTaxPer);
                cmd.ExecuteNonQuery();
            }
            PopulatePurchaseTax();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method refreshes the tax grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (HasChanges())
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 714), MessageContainerList.GetMessage(utilities.ExecutionContext, "Unsaved Data"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    log.LogMethodExit();
                    return;
                }
            }
            PopulatePurchaseTax();
            dgvTax.RefreshEdit();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method handles the enter event for the grid  dgvTax
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvTax_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        }


        /// <summary>
        /// This method handles the tax delete  , Inactivates the taxes
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvfocused != null)
            {
                if (this.dgvfocused.SelectedRows.Count <= 0 && this.dgvfocused.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvfocused.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvfocused.SelectedCells)
                    {
                        dgvfocused.Rows[cell.RowIndex].Selected = true;
                    }
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (DataGridViewRow dgvSelectedRow in this.dgvfocused.SelectedRows)
                         {
                        if (dgvSelectedRow.Cells[0].Value == null)
                            {
                                return;
                            }
                            if (Convert.ToInt32(dgvSelectedRow.Cells[0].Value.ToString()) <= 0)
                            {
                                dgvfocused.Rows.RemoveAt(dgvSelectedRow.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                BindingSource focussedGridDataListDTOBS = (BindingSource)dgvfocused.DataSource;
                                switch (dgvfocused.Name)
                                {
                                    case "dgvTax":
                                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm InActvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                        {
                                            confirmDelete = true;
                                            var taxDTOList = (SortableBindingList<TaxDTO>)focussedGridDataListDTOBS.DataSource;
                                            TaxDTO taxDTO = taxDTOList[dgvSelectedRow.Index];
                                            taxDTO.ActiveFlag = false;
                                            Tax taxBL = new Tax(machineUserContext, taxDTO);
                                            taxBL.Save(parafaitDBTrx.SQLTrx);
                                        }
                                        break;
                                    case "dgvTaxStructure":
                                        var taxStructureDTOList = (List<TaxStructureDTO>)focussedGridDataListDTOBS.DataSource;
                                        TaxStructureDTO taxStructureDTO = taxStructureDTOList[dgvSelectedRow.Index];
                                        taxStructureDTO.IsActive = false;
                                        TaxStructureBL taxStructureBL = new TaxStructureBL(machineUserContext, taxStructureDTO);
                                        taxStructureBL.Delete(parafaitDBTrx.SQLTrx);
                                        rowsDeleted = true;
                                        break;

                                }
                            }
                       }

                        parafaitDBTrx.EndTransaction();
                        calculateTotalTaxPercentage();
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + " " + ex.Message, utilities.MessageUtils.getMessage("Save Error"));
                        return;
                    }
                }
                if (rowsDeleted == true)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// This method handles the closing the form. Checks for the unsaved changes in the grid.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            if (HasChanges())
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 127), MessageContainerList.GetMessage(utilities.ExecutionContext, "Unsaved Data"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    log.LogMethodExit();
                    return;
                }
            }
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// this method  publishes the tax to the  site 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PublishUI publishUI;
            if (dgvTax.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvTax.SelectedCells)
                {
                    dgvTax.Rows[cell.RowIndex].Selected = true;
                }
            }
            if (dgvTax.SelectedRows.Count > 0)
            {
                if (dgvTax.SelectedRows[0].Cells["taxNameDataGridViewTextBoxColumn"].Value != null)
                {
                    publishUI = new PublishUI(utilities, Convert.ToInt32(dgvTax.SelectedRows[0].Cells["taxIdDataGridViewTextBoxColumn"].Value), "Tax", dgvTax.SelectedRows[0].Cells["taxNameDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// This method handles the data error in the dgv_Tax grid
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvTax_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.Exception != null)
                MessageBox.Show(utilities.MessageUtils.getMessage(585, Name, e.RowIndex + 1, dgvTax.Columns[e.ColumnIndex].DataPropertyName) + ": ", "Data Error");
            e.Cancel = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// This method handles the cell click event for the dgvTax grid
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvTax_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                return;
            }
            log.LogMethodExit();
        }


        private void dataGridViewTax_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            e.Row.Cells["activeflagDataGridViewTextBoxColumn"].Value = true;
            e.Row.Cells["taxPercentageDataGridViewTextBoxColumn"].Value = "0";
            log.LogMethodExit();
        }
        private void dataGridViewTaxStructure_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            e.Row.Cells["percentageDataGridViewTextBoxColumn"].Value = "0";
            log.LogMethodExit();
        }

        private void dgvTaxStructure_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.Exception != null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(585, Name, e.RowIndex + 1, dgvTaxStructure.Columns[e.ColumnIndex].DataPropertyName) + ": ", "Data Error");
            }
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
