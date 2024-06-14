/********************************************************************************************
 * Project Name - Segment Definition UI
 * Description  - User interface for segment definition UI
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
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.Category
{
    /// <summary>
    /// Segment Definition UI
    /// </summary>
    public partial class SegmentDefinitionUI : Form
    {
        Utilities utilities;
        ParafaitEnv parafaitEnv;
        string apllicability = "";
        BindingSource segmentDefinitionListBS;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext segmentDefinitionContext = ExecutionContext.GetExecutionContext();
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities">utilities passed from environment from where this file is called</param>
        /// <param name="_ParafaitEnv">Parafait Environment</param>
        /// <param name="_Apllicability">Applicability of the segment definition</param>
        public SegmentDefinitionUI(Utilities _Utilities, ParafaitEnv _ParafaitEnv, string _Apllicability)
        {
            log.LogMethodExit();
            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            parafaitEnv = _ParafaitEnv;
            apllicability = _Apllicability;
            utilities.setupDataGridProperties(ref segmentDefinitionDataGridView);
            if (parafaitEnv.IsCorporate)
            {
                segmentDefinitionContext.SetSiteId(parafaitEnv.SiteId);
            }
            else
            {
                segmentDefinitionContext.SetSiteId(-1);
            }
            segmentDefinitionContext.SetUserId(parafaitEnv.LoginID);
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublishToSite.Visible = true;
            }
            else
            {
                lnkPublishToSite.Visible = false;
            }
            PopulateSegmentDefinitionGrid();
            if (_Apllicability.Equals("POS PRODUCTS"))
            {
                managementStudioSwitch = new ManagementStudioSwitch(segmentDefinitionContext);
                UpdateUIElements();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads segment definition to the grid
        /// </summary>
        private void PopulateSegmentDefinitionGrid()
        {
            log.LogMethodEntry();
            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(segmentDefinitionContext);
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionDTOSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, apllicability));
            segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, segmentDefinitionContext.GetSiteId().ToString()));
            List<SegmentDefinitionDTO> segmentDefinitionListOnDisplay = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionDTOSearchParams);
            segmentDefinitionListBS = new BindingSource();
            if (segmentDefinitionListOnDisplay != null)
                segmentDefinitionListBS.DataSource = new SortableBindingList<SegmentDefinitionDTO>(segmentDefinitionListOnDisplay);
            else
                segmentDefinitionListBS.DataSource = new SortableBindingList<SegmentDefinitionDTO>();
            segmentDefinitionListBS.AddingNew += segmentDefinitionDataGridView_BindingSourceAddNew;
            segmentDefinitionDataGridView.DataSource = segmentDefinitionListBS;
            log.LogMethodExit();
        }
        private void segmentDefinitionDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            if (segmentDefinitionDataGridView.Rows.Count == segmentDefinitionListBS.Count)
            {
                segmentDefinitionListBS.RemoveAt(segmentDefinitionListBS.Count - 1);
            }
            log.LogMethodExit();
        }   
        private void segmentDefinitionSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BindingSource segmentDefinitionListBS = (BindingSource)segmentDefinitionDataGridView.DataSource;
            var segmentDefinitionListOnDisplay = (SortableBindingList<SegmentDefinitionDTO>)segmentDefinitionListBS.DataSource;
            if (segmentDefinitionListOnDisplay.Count > 0)
            {
                foreach (SegmentDefinitionDTO segmentDefinitionDTO in segmentDefinitionListOnDisplay)
                {
                    List<SegmentDefinitionDTO> tempList = new List<SegmentDefinitionDTO>(segmentDefinitionListOnDisplay);
                    var isNull = tempList.Any(item => item.SegmentName == null);
                    if (isNull)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(2607, utilities.MessageUtils.getMessage("Segment")), utilities.MessageUtils.getMessage("Validation Error"));
                        log.LogMethodExit();
                        return;
                    }
                    List<string> nameList = tempList.Select(x => x.SegmentName.Trim().ToLower()).ToList();
                    if (nameList.Count != nameList.Distinct().Count())
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(2608, utilities.MessageUtils.getMessage("Segment")), utilities.MessageUtils.getMessage("Validation Error"));
                        log.LogMethodExit();
                        return;
                    }
                    if (segmentDefinitionDTO.IsChanged)
                    {
                        if (string.IsNullOrEmpty(segmentDefinitionDTO.SegmentName))
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1132));
                            return;
                        }
                        if (string.IsNullOrEmpty(segmentDefinitionDTO.SequenceOrder))
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1255));
                            return;
                        }
                    }
                    SegmentDefinition segmentDefinition = new SegmentDefinition(segmentDefinitionContext, segmentDefinitionDTO);
                    segmentDefinition.Save();
                }
                PopulateSegmentDefinitionGrid();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void segmentDefinitionRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateSegmentDefinitionGrid();
            log.LogMethodExit();
        }

        private void segmentDefinitionDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.segmentDefinitionDataGridView.SelectedRows.Count <= 0 && this.segmentDefinitionDataGridView.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-segmentDefinitionDeleteBtn_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.segmentDefinitionDataGridView.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.segmentDefinitionDataGridView.SelectedCells)
                    {
                        segmentDefinitionDataGridView.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow segmentDefinitionRow in this.segmentDefinitionDataGridView.SelectedRows)
                {
                    if (segmentDefinitionRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(segmentDefinitionRow.Cells[0].Value) <= 0)
                        {
                            segmentDefinitionDataGridView.Rows.RemoveAt(segmentDefinitionRow.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                BindingSource segmentDefinitionDTOListDTOBS = (BindingSource)segmentDefinitionDataGridView.DataSource;
                                var segmentDefinitionDTOList = (SortableBindingList<SegmentDefinitionDTO>)segmentDefinitionDTOListDTOBS.DataSource;
                                SegmentDefinitionDTO segmentDefinitionDTO = segmentDefinitionDTOList[segmentDefinitionRow.Index];
                                segmentDefinitionDTO.IsActive = false;
                                SegmentDefinition segmentDefinition = new SegmentDefinition(segmentDefinitionContext, segmentDefinitionDTO);
                                segmentDefinition.Save();
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateSegmentDefinitionGrid();
            }
            catch(Exception ex)
            {
                log.Error("Ends-segmentDefinitionSourceMapDeleteBtn_Click() event with exception: " + ex.ToString());
                MessageBox.Show("Delete failed!!!.\n Error: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void segmentDefinitionCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            PublishUI publishUI;
            if (segmentDefinitionDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in segmentDefinitionDataGridView.SelectedCells)
                {
                    segmentDefinitionDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }            
            if (segmentDefinitionDataGridView.SelectedRows.Count > 0)
            {
                if (segmentDefinitionDataGridView.SelectedRows[0].Cells["segmentNameDataGridViewTextBoxColumn"].Value != null)
                {
                    publishUI = new PublishUI(utilities, Convert.ToInt32(segmentDefinitionDataGridView.SelectedRows[0].Cells["segmentDefinitionIdDataGridViewTextBoxColumn"].Value), "Segment_Definition", segmentDefinitionDataGridView.SelectedRows[0].Cells["segmentNameDataGridViewTextBoxColumn"].Value.ToString());
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
                segmentDefinitionDataGridView.AllowUserToAddRows = true;
                segmentDefinitionDataGridView.ReadOnly = false;
                segmentDefinitionSaveBtn.Enabled = true;
                segmentDefinitionDeleteBtn.Enabled = true;
                lnkPublishToSite.Enabled = true;
            }
            else
            {
                segmentDefinitionDataGridView.AllowUserToAddRows = false;
                segmentDefinitionDataGridView.ReadOnly = true;
                segmentDefinitionSaveBtn.Enabled = false;
                segmentDefinitionDeleteBtn.Enabled = false;
                lnkPublishToSite.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
