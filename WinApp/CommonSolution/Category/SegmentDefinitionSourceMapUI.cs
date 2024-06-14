/********************************************************************************************
 * Project Name - Segment Definition Source Map UI
 * Description  - User interface for segment definition source map UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Raghuveera     Created 
* 2.80        28-Jun-2020   Deeksha        Modified to Make Product module read only in Windows Management Studio.
*2.110.0     07-Oct-2020   Mushahid Faizan    Modified as per inventory changes,
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Vendor;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.Category
{
    /// <summary>
    /// Segment Definition Source Map UI
    /// </summary>
    public partial class SegmentDefinitionSourceMapUI : Form
    {
        Utilities utilities;
        ParafaitEnv parafaitEnv;
        string applicability = "";
        BindingSource segmentDefinitionSourceMapListBS;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext segmentDefinitionContext = ExecutionContext.GetExecutionContext();
        BindingSource segmentDefinitionSourceValueBS;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities">utilities passed from environment from where this file is called</param>
        /// <param name="_ParafaitEnv">Parafait Environment</param>
        /// <param name="_Applicability">Applicability of the segment definition source map</param>
        public SegmentDefinitionSourceMapUI(Utilities _Utilities, ParafaitEnv _ParafaitEnv, string _Applicability)
        {
            log.LogMethodEntry(_Utilities, _ParafaitEnv, _Applicability);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            parafaitEnv = _ParafaitEnv;
            applicability = _Applicability;
            utilities.setupDataGridProperties(ref segmentDefinitionSourceMapDataGridView);
            utilities.setupDataGridProperties(ref SegmentDefinitionSourceValueDataGridView);
            if (parafaitEnv.IsCorporate)
            {
                segmentDefinitionContext.SetSiteId(parafaitEnv.SiteId);
            }
            else
            {
                segmentDefinitionContext.SetSiteId(-1);
            }
            segmentDefinitionContext.SetUserId(parafaitEnv.LoginID);
            PopulateSegmentDefinitionSourceMapGrid();
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublishToSite.Visible = true;
            }
            else
            {
                lnkPublishToSite.Visible = false;
            }
            if (_Applicability.Equals("POS PRODUCTS"))
            {
                managementStudioSwitch = new ManagementStudioSwitch(segmentDefinitionContext);
                UpdateUIElements();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads segment definition to the grid
        /// </summary>
        private void PopulateSegmentDefinitionSourceMapGrid()
        {
            log.LogMethodEntry();
            LoadDefinition();
            SegmentDefinitionSourceMapList segmentDefinitionSourceMapList = new SegmentDefinitionSourceMapList(segmentDefinitionContext);
            List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>> searchBySegmentDefinitionSourceMapDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>>();
            searchBySegmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_APPLICABILITY, applicability));
            searchBySegmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID, segmentDefinitionContext.GetSiteId().ToString()));

            List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapListOnDisplay = segmentDefinitionSourceMapList.GetAllSegmentDefinitionSourceMaps(searchBySegmentDefinitionSourceMapDTOSearchParams);//segmentDefinitionDTOSearchParams
            segmentDefinitionSourceMapListBS = new BindingSource();
            if (segmentDefinitionSourceMapListOnDisplay != null)
                segmentDefinitionSourceMapListBS.DataSource = new SortableBindingList<SegmentDefinitionSourceMapDTO>(segmentDefinitionSourceMapListOnDisplay);
            else
                segmentDefinitionSourceMapListBS.DataSource = new SortableBindingList<SegmentDefinitionSourceMapDTO>();
            segmentDefinitionSourceMapListBS.AddingNew += segmentDefinitionSourceMapDataGridView_BindingSourceAddNew;
            segmentDefinitionSourceMapDataGridView.DataSource = segmentDefinitionSourceMapListBS;
            for (int i = 0; i < segmentDefinitionSourceMapDataGridView.Rows.Count; i++)
            {
                if (segmentDefinitionSourceMapDataGridView.Rows[i].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value != null && (segmentDefinitionSourceMapDataGridView.Rows[i].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("DATE") || segmentDefinitionSourceMapDataGridView.Rows[i].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("TEXT")))
                {
                    segmentDefinitionSourceMapDataGridView.Rows[i].Cells["dataSourceEntityDataGridViewTextBoxColumn"].ReadOnly =
                    segmentDefinitionSourceMapDataGridView.Rows[i].Cells["dataSourceColumnDataGridViewTextBoxColumn"].ReadOnly = true;
                }
            }
            log.LogMethodExit();
        }
        private void PopulateSegmentDefinitionSourceValueDataGridView(int sourceId)
        {
            log.LogMethodEntry(sourceId);
            SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(segmentDefinitionContext);
            List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
            List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> segmentDefinitionSourceValueDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>>();
            segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID, sourceId.ToString()));
            segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID, segmentDefinitionContext.GetSiteId().ToString()));
            segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueList.GetAllSegmentDefinitionSourceValues(segmentDefinitionSourceValueDTOSearchParams);
            segmentDefinitionSourceValueBS = new BindingSource();
            if (segmentDefinitionSourceValueDTOList != null)
            {
                segmentDefinitionSourceValueBS.DataSource = new SortableBindingList<SegmentDefinitionSourceValueDTO>(segmentDefinitionSourceValueDTOList);
                lblValues.Visible = true;
                panelSourceValue.Visible = true;
            }
            else
            {
                lblValues.Visible = false;
                panelSourceValue.Visible = false;
                segmentDefinitionSourceValueBS.DataSource = new SortableBindingList<SegmentDefinitionSourceValueDTO>();
            }
            segmentDefinitionSourceValueBS.AddingNew += SegmentDefinitionSourceValueDataGridView_BindingSourceAddNew;
            SegmentDefinitionSourceValueDataGridView.DataSource = segmentDefinitionSourceValueBS;

            log.LogMethodExit();

        }
        private void segmentDefinitionSourceMapDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            if (segmentDefinitionSourceMapDataGridView.Rows.Count == segmentDefinitionSourceMapListBS.Count)
            {
                segmentDefinitionSourceMapListBS.RemoveAt(segmentDefinitionSourceMapListBS.Count - 1);
            }
            log.LogMethodExit();
        }
        private void SegmentDefinitionSourceValueDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            if (SegmentDefinitionSourceValueDataGridView.Rows.Count == segmentDefinitionSourceValueBS.Count)
            {
                segmentDefinitionSourceValueBS.RemoveAt(segmentDefinitionSourceValueBS.Count - 1);
            }
            log.LogMethodExit();
        }
        private void LoadDefinition()
        {
            log.LogMethodEntry();
            List<SegmentDefinitionDTO> segmentDefinitionListOnDisplay;
            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(segmentDefinitionContext);
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionDTOSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, applicability));
            segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, segmentDefinitionContext.GetSiteId().ToString()));
            segmentDefinitionListOnDisplay = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionDTOSearchParams);
            if (segmentDefinitionListOnDisplay == null)
            {
                segmentDefinitionListOnDisplay = new List<SegmentDefinitionDTO>();
            }
            segmentDefinitionListOnDisplay.Insert(0, new SegmentDefinitionDTO());
            BindingSource patchApplicationTypeBS = new BindingSource();
            segmentDefinitionListOnDisplay[0].SegmentName = utilities.MessageUtils.getMessage("<SELECT>");
            patchApplicationTypeBS.DataSource = segmentDefinitionListOnDisplay;
            segmentDefinitionIdDataGridViewTextBoxColumn.DataSource = patchApplicationTypeBS;
            segmentDefinitionIdDataGridViewTextBoxColumn.ValueMember = "SegmentDefinitionId";
            segmentDefinitionIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
            segmentDefinitionIdDataGridViewTextBoxColumn.DisplayMember = "SegmentName";
            log.LogMethodExit();
        }
        private void segmentDefinitionSourceMapSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int sourceId = -1;
            cmbDataColumn.Visible = false;
            BindingSource segmentDefinitionListBS = (BindingSource)segmentDefinitionSourceMapDataGridView.DataSource;
            var segmentDefinitionListOnDisplay = (SortableBindingList<SegmentDefinitionSourceMapDTO>)segmentDefinitionListBS.DataSource;
            if (segmentDefinitionListOnDisplay.Count > 0)
            {
                foreach (SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO in segmentDefinitionListOnDisplay)
                {
                    if (segmentDefinitionSourceMapDTO.IsChanged)
                    {
                        if (segmentDefinitionSourceMapDTO.SegmentDefinitionId == -1)
                        {
                            MessageBox.Show("Please select the definition.");
                            return;
                        }
                        if (string.IsNullOrEmpty(segmentDefinitionSourceMapDTO.DataSourceType))
                        {
                            MessageBox.Show("Please select the source type.");
                            return;
                        }
                        if (segmentDefinitionSourceMapDTO.DataSourceType.Equals("DYNAMIC LIST") && segmentDefinitionSourceMapDTO.IsActive.Equals("Y"))
                        {
                            if (string.IsNullOrEmpty(segmentDefinitionSourceMapDTO.DataSourceEntity))
                            {
                                MessageBox.Show("Please select the data source entity.");
                                return;
                            }

                            if (string.IsNullOrEmpty(segmentDefinitionSourceMapDTO.DataSourceColumn))
                            {
                                MessageBox.Show("Please select the data source column.");
                                return;
                            }
                        }
                    }
                    sourceId = segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId;
                    SegmentDefinitionSourceMap segmentDefinitionSourceMap = new SegmentDefinitionSourceMap(segmentDefinitionContext, segmentDefinitionSourceMapDTO);
                    segmentDefinitionSourceMap.Save();
                    if (SegmentDefinitionSourceValueDataGridView.Rows.Count > 1 || segmentDefinitionSourceMapDTO.DataSourceType.Equals("DYNAMIC LIST"))
                    {
                        if (SegmentDefinitionSourceValueDataGridView.Rows[0].Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn1"].Value != null && (int)SegmentDefinitionSourceValueDataGridView.Rows[0].Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn1"].Value == sourceId)
                        {
                            SaveSegmentDefinitionSourceValue(segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId);
                        }
                    }
                }
                PopulateSegmentDefinitionSourceMapGrid();
            }
            else
                MessageBox.Show("Nothing to save");
            log.LogMethodExit();
        }
        private bool SaveSegmentDefinitionSourceValue(int segmentDefinitionSourceId)
        {
            log.LogMethodEntry(segmentDefinitionSourceId);
            bool status = false;
            BindingSource segmentDefinitionSourceValueListBS = (BindingSource)SegmentDefinitionSourceValueDataGridView.DataSource;
            var segmentDefinitionSourceValueListOnDisplay = (SortableBindingList<SegmentDefinitionSourceValueDTO>)segmentDefinitionSourceValueListBS.DataSource;
            if (segmentDefinitionSourceValueListOnDisplay.Count > 0)
            {
                foreach (SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDTO in segmentDefinitionSourceValueListOnDisplay)
                {
                    if (segmentDefinitionSourceValueDTO.IsChanged)
                    {
                        if (segmentDefinitionSourceValueDTO.ListValue == null && segmentDefinitionSourceValueDTO.DBQuery == null)
                        {
                            continue;
                        }
                        segmentDefinitionSourceValueDTO.SegmentDefinitionSourceId = segmentDefinitionSourceId;
                    }
                    SegmentDefinitionSourceValue segmentDefinitionSourceValue = new SegmentDefinitionSourceValue(segmentDefinitionContext, segmentDefinitionSourceValueDTO);
                    segmentDefinitionSourceValue.Save();
                }
            }

            log.LogMethodExit(status);
            return status;

        }

        private void segmentDefinitionSourceMapDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (segmentDefinitionSourceMapDataGridView.Columns[e.ColumnIndex].Name.Equals("dataSourceTypeDataGridViewTextBoxColumn"))
                {
                    if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value == null || segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("DATE") || segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("TEXT") || segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("STATIC LIST"))
                    {
                        segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceEntityDataGridViewTextBoxColumn"].ReadOnly = true;
                        segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceEntityDataGridViewTextBoxColumn"].Value = null;
                        if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value != null && segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("STATIC LIST"))
                        {
                            PopulateSegmentDefinitionSourceValueDataGridView((int)segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn"].Value);
                            dBQueryDataGridViewTextBoxColumn.Visible = false;
                            listValueDataGridViewTextBoxColumn.Visible = true;
                            lblValues.Visible = true;
                            panelSourceValue.Visible = true;
                        }

                    }
                    else
                    {
                        segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceEntityDataGridViewTextBoxColumn"].ReadOnly = false;
                    }
                    cmbDataColumn.Visible = false;
                }
                else if (segmentDefinitionSourceMapDataGridView.Columns[e.ColumnIndex].Name.Equals("dataSourceColumnDataGridViewTextBoxColumn"))
                {
                    if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value != null && (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("DYNAMIC LIST") || segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("STATIC LIST")))
                    {
                        PopulateSegmentDefinitionSourceValueDataGridView((int)segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn"].Value);
                        if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("DYNAMIC LIST"))
                        {
                            listValueDataGridViewTextBoxColumn.Visible = false;
                            dBQueryDataGridViewTextBoxColumn.Visible = true;
                        }
                        else if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("STATIC LIST"))
                        {
                            listValueDataGridViewTextBoxColumn.Visible = true;
                            dBQueryDataGridViewTextBoxColumn.Visible = false;
                        }
                        dBQueryDataGridViewTextBoxColumn.ReadOnly = true;
                        lblValues.Visible = true;
                        panelSourceValue.Visible = true;
                    }
                    else
                    {
                        lblValues.Visible = false;
                        panelSourceValue.Visible = false;
                    }
                }
                else if (segmentDefinitionSourceMapDataGridView.Columns[e.ColumnIndex].Name.Equals("isActiveDataGridViewTextBoxColumn"))
                {
                    cmbDataColumn.Visible = false;
                }

            }
            log.LogMethodExit();
        }

        private void cmbDataColumn_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (segmentDefinitionSourceMapDataGridView.CurrentCell != null)
            {
                if (!string.IsNullOrEmpty(cmbDataColumn.Text) && !cmbDataColumn.Text.Equals("System.Data.DataRowView"))
                {
                    segmentDefinitionSourceMapDataGridView.Rows[segmentDefinitionSourceMapDataGridView.CurrentCell.RowIndex].Cells["dataSourceColumnDataGridViewTextBoxColumn"].Value = cmbDataColumn.Text;
                }
            }
            log.LogMethodExit();
        }

        private void segmentDefinitionSourceMapDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (segmentDefinitionSourceMapDataGridView.Columns[e.ColumnIndex].Name.Equals("dataSourceColumnDataGridViewTextBoxColumn"))
            {

                if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceEntityDataGridViewTextBoxColumn"].Value != null)
                {
                    if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceEntityDataGridViewTextBoxColumn"].Value.Equals("VENDOR"))
                    {
                        VendorList vendorList = new VendorList(utilities.ExecutionContext);
                        DataTable dTable = vendorList.GetVendorColumnsName();
                        cmbDataColumn.DataSource = dTable;
                        cmbDataColumn.DisplayMember = "Columns";
                        cmbDataColumn.ValueMember = "Columns";
                        if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceColumnDataGridViewTextBoxColumn"].Value != null)
                        {
                            cmbDataColumn.Text = segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceColumnDataGridViewTextBoxColumn"].Value.ToString();
                        }
                        int y = segmentDefinitionSourceMapDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Top;
                        int x = segmentDefinitionSourceMapDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Left;
                        cmbDataColumn.Location = new Point(x + 2, y + 3); // segmentDefinitionSourceMapDataGridView.GetCellDisplayRectangle(e.ColumnIndex + 1, e.RowIndex, false).Top;
                        cmbDataColumn.Width = dataSourceColumnDataGridViewTextBoxColumn.Width;
                        cmbDataColumn.BringToFront();
                        cmbDataColumn.Visible = true;
                    }
                    else if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceEntityDataGridViewTextBoxColumn"].Value.Equals("CATEGORY"))
                    {
                        CategoryList categoryList = new CategoryList(utilities.ExecutionContext);
                        DataTable dTable = categoryList.GetCategoryColumnsName();
                        cmbDataColumn.DataSource = dTable;
                        cmbDataColumn.DisplayMember = "Columns";
                        cmbDataColumn.ValueMember = "Columns";
                        if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceColumnDataGridViewTextBoxColumn"].Value != null)
                        {
                            cmbDataColumn.Text = segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceColumnDataGridViewTextBoxColumn"].Value.ToString();
                        }
                        int y = segmentDefinitionSourceMapDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Top;
                        int x = segmentDefinitionSourceMapDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Left;
                        cmbDataColumn.Location = new Point(x + 2, y + 3); // segmentDefinitionSourceMapDataGridView.GetCellDisplayRectangle(e.ColumnIndex + 1, e.RowIndex, false).Top;
                        cmbDataColumn.Width = dataSourceColumnDataGridViewTextBoxColumn.Width;
                        cmbDataColumn.BringToFront();
                        cmbDataColumn.Visible = true;
                    }
                    else
                    {
                        cmbDataColumn.DataSource = null;
                        cmbDataColumn.Visible = false;
                    }
                }
                else
                {
                    cmbDataColumn.DataSource = null;
                    cmbDataColumn.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        private void segmentDefinitionSourceMapRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateSegmentDefinitionSourceMapGrid();
            log.LogMethodExit();
        }

        private void segmentDefinitionSourceMapDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.segmentDefinitionSourceMapDataGridView.SelectedRows.Count <= 0 && this.segmentDefinitionSourceMapDataGridView.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-segmentDefinitionSourceMapDeleteBtn_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                int activeCount = 0;
                if (SegmentDefinitionSourceValueDataGridView.Rows.Count > 0)
                {
                    for (int i = 0; i < SegmentDefinitionSourceValueDataGridView.Rows.Count; i++)
                    {
                        if (SegmentDefinitionSourceValueDataGridView.Rows[i].Cells["isActiveDataGridViewTextBoxColumn1"].Value != null && SegmentDefinitionSourceValueDataGridView.Rows[i].Cells["isActiveDataGridViewTextBoxColumn1"].Value.ToString().Equals("Y"))
                        {
                            activeCount++;
                        }
                    }

                }
                if (this.SegmentDefinitionSourceValueDataGridView.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.SegmentDefinitionSourceValueDataGridView.SelectedCells)
                    {
                        SegmentDefinitionSourceValueDataGridView.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow segmentDefinitionRow in this.SegmentDefinitionSourceValueDataGridView.SelectedRows)
                {
                    if (segmentDefinitionRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(segmentDefinitionRow.Cells[0].Value.ToString()) <= 0)
                        {
                            SegmentDefinitionSourceValueDataGridView.Rows.RemoveAt(segmentDefinitionRow.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                BindingSource segmentDefinitionSourceValueDTOListBS = (BindingSource)SegmentDefinitionSourceValueDataGridView.DataSource;
                                var segmentDefinitionSourceValueDTOList = (SortableBindingList<SegmentDefinitionSourceValueDTO>)segmentDefinitionSourceValueDTOListBS.DataSource;
                                SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDTO = segmentDefinitionSourceValueDTOList[segmentDefinitionRow.Index];
                                segmentDefinitionSourceValueDTO.IsActive = false;
                                SegmentDefinitionSourceValue segmentDefinitionSourceValue = new SegmentDefinitionSourceValue(segmentDefinitionContext, segmentDefinitionSourceValueDTO);
                                segmentDefinitionSourceValue.Save();
                            }
                        }
                    }
                }

                if (activeCount <= 0)
                {
                    if (this.segmentDefinitionSourceMapDataGridView.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in this.segmentDefinitionSourceMapDataGridView.SelectedCells)
                        {
                            segmentDefinitionSourceMapDataGridView.Rows[cell.RowIndex].Selected = true;
                        }
                    }
                    foreach (DataGridViewRow segmentDefinitionRow in this.segmentDefinitionSourceMapDataGridView.SelectedRows)
                    {
                        if (segmentDefinitionRow.Cells[0].Value != null)
                        {
                            if (Convert.ToInt32(segmentDefinitionRow.Cells[0].Value.ToString()) <= 0)
                            {
                                segmentDefinitionSourceMapDataGridView.Rows.RemoveAt(segmentDefinitionRow.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    BindingSource segmentDefinitionSourceMapDTOListBS = (BindingSource)segmentDefinitionSourceMapDataGridView.DataSource;
                                    var segmentDefinitionSourceMapDTOList = (SortableBindingList<SegmentDefinitionSourceMapDTO>)segmentDefinitionSourceMapDTOListBS.DataSource;
                                    SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO = segmentDefinitionSourceMapDTOList[segmentDefinitionRow.Index];
                                    segmentDefinitionSourceMapDTO.IsActive = false;
                                    SegmentDefinitionSourceMap segmentDefinitionSourceMap = new SegmentDefinitionSourceMap(segmentDefinitionContext, segmentDefinitionSourceMapDTO);
                                    segmentDefinitionSourceMap.Save();
                                }
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateSegmentDefinitionSourceMapGrid();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-segmentDefinitionSourceMapDeleteBtn_Click() event with exception: " + ex.ToString());
                MessageBox.Show("Delete failed!!!.\n Error: " + ex.Message);
            }
        }

        private void segmentDefinitionSourceMapCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(); 
            this.Close();
            log.LogMethodExit();
        }

        private void SegmentDefinitionSourceValueDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex >= 0)
            {
                if (SegmentDefinitionSourceValueDataGridView.Columns[e.ColumnIndex].Name.Equals("dBQueryDataGridViewTextBoxColumn"))
                {
                    string tableName = segmentDefinitionSourceMapDataGridView.Rows[segmentDefinitionSourceMapDataGridView.SelectedCells[0].RowIndex].Cells["dataSourceEntityDataGridViewTextBoxColumn"].FormattedValue.ToString();
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        AdvanceSearchUI advanceSearchUI = new AdvanceSearchUI(utilities, tableName, tableName[0].ToString());
                        if (SegmentDefinitionSourceValueDataGridView.Rows[e.RowIndex].Cells["dBQueryDataGridViewTextBoxColumn"].Value != null)
                        {
                            advanceSearchUI.searchCriteria = SegmentDefinitionSourceValueDataGridView.Rows[e.RowIndex].Cells["dBQueryDataGridViewTextBoxColumn"].Value.ToString();
                        }
                        advanceSearchUI.ShowDialog();
                        if (!string.IsNullOrEmpty(advanceSearchUI.searchCriteria))
                        {
                            SegmentDefinitionSourceValueDataGridView.Rows[e.RowIndex].Cells["dBQueryDataGridViewTextBoxColumn"].Value = advanceSearchUI.searchCriteria;
                            SegmentDefinitionSourceValueDataGridView.NotifyCurrentCellDirty(true);
                            SegmentDefinitionSourceValueDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        private void segmentDefinitionSourceMapDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn"].Value != null)
            {
                PopulateSegmentDefinitionSourceValueDataGridView((int)segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn"].Value);
                if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value != null && segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("DYNAMIC LIST"))
                {
                    dBQueryDataGridViewTextBoxColumn.Visible = true;
                    listValueDataGridViewTextBoxColumn.Visible = false;
                    if (SegmentDefinitionSourceValueDataGridView.Rows.Count > 1)
                    {
                        SegmentDefinitionSourceValueDataGridView.AllowUserToAddRows = false;
                    }
                    panelSourceValue.Visible = true;
                }
                else if (segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value != null && segmentDefinitionSourceMapDataGridView.Rows[e.RowIndex].Cells["dataSourceTypeDataGridViewTextBoxColumn"].Value.Equals("STATIC LIST"))
                {
                    dBQueryDataGridViewTextBoxColumn.Visible = false;
                    listValueDataGridViewTextBoxColumn.Visible = true;
                    SegmentDefinitionSourceValueDataGridView.AllowUserToAddRows = true;
                    panelSourceValue.Visible = true;
                }
            }
            else
            {
                SegmentDefinitionSourceValueDataGridView.Rows.Clear();
                lblValues.Visible = false;
                panelSourceValue.Visible = false;
                SegmentDefinitionSourceValueDataGridView.AllowUserToAddRows = true;
            }
            log.LogMethodExit();
        }

        private void SegmentDefinitionSourceValueDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            if (segmentDefinitionSourceMapDataGridView.SelectedCells != null && segmentDefinitionSourceMapDataGridView.SelectedCells.Count > 0)
            {
                e.Row.Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn1"].Value = segmentDefinitionSourceMapDataGridView.Rows[segmentDefinitionSourceMapDataGridView.SelectedCells[0].RowIndex].Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn"].Value;
            }
            log.LogMethodExit();
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            PublishUI publishUI;

            if (segmentDefinitionSourceMapDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in segmentDefinitionSourceMapDataGridView.SelectedCells)
                {
                    segmentDefinitionSourceMapDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            if (segmentDefinitionSourceMapDataGridView.SelectedRows.Count > 0)
            {
                if (segmentDefinitionSourceMapDataGridView.SelectedRows[0].Cells["segmentDefinitionIdDataGridViewTextBoxColumn"].Value != null)
                {
                    publishUI = new PublishUI(utilities, Convert.ToInt32(segmentDefinitionSourceMapDataGridView.SelectedRows[0].Cells["segmentDefinitionSourceIdDataGridViewTextBoxColumn"].Value), "Segment_Definition_Source_Mapping", segmentDefinitionSourceMapDataGridView.SelectedRows[0].Cells["segmentDefinitionIdDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                segmentDefinitionSourceMapDataGridView.AllowUserToAddRows = true;
                segmentDefinitionSourceMapDataGridView.ReadOnly = false;
                SegmentDefinitionSourceValueDataGridView.AllowUserToAddRows = true;
                SegmentDefinitionSourceValueDataGridView.ReadOnly = false;
                segmentDefinitionSourceMapSaveBtn.Enabled = true;
                segmentDefinitionSourceMapDeleteBtn.Enabled = true;
                lnkPublishToSite.Enabled = true;
            }
            else
            {
                segmentDefinitionSourceMapDataGridView.AllowUserToAddRows = false;
                segmentDefinitionSourceMapDataGridView.ReadOnly = true;
                SegmentDefinitionSourceValueDataGridView.AllowUserToAddRows = false;
                SegmentDefinitionSourceValueDataGridView.ReadOnly = true;
                segmentDefinitionSourceMapSaveBtn.Enabled = false;
                segmentDefinitionSourceMapDeleteBtn.Enabled = false;
                lnkPublishToSite.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
