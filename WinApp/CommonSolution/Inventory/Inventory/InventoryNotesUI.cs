/********************************************************************************************
 * Project Name -InventoryNotes
 * Description  -UI of  InventoryNotes 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        6-Aug-2019    Deeksha          Modified:Added log() methods.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// frmInventoryNotes UI Class
    /// </summary>
    public partial class frmInventoryNotes : Form
    {
        /// <summary>
        /// Parafait Utilities
        /// </summary>
        public Utilities Utilities;

        int objectId;
        string objectName;

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public frmInventoryNotes()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        /// <summary>
        /// frmInventoryNotes constructor
        /// </summary>
        public frmInventoryNotes(int objId, string objName, Utilities _Utilites)
        {
            log.LogMethodEntry(objId, objName, _Utilites);

            InitializeComponent();
            Utilities = _Utilites;
            objectId = objId;
            objectName = objName;
            Utilities.setLanguage(this);
            Utilities.setupDataGridProperties(ref dgvInventoryNotes);
            if (Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(Utilities.ParafaitEnv.Username);
            LoadLookupValues();
            PopulateInventoryNoteGrid();

            //Begin: Modification for style GUI added on 12-Sep-2016 
            CommonUIDisplay.setupVisuals(this);
            CommonUIDisplay.Utilities = _Utilites;
            CommonUIDisplay.ParafaitEnv = _Utilites.ParafaitEnv;
            this.StartPosition = FormStartPosition.CenterScreen;
            //End: Modification for style GUI added on 12-Sep-2016 

            log.LogMethodExit();
        }

        /// <summary>
        /// LoadLookupValues method load combobox
        /// </summary>
        public void LoadLookupValues()
        {
            log.LogMethodEntry();

            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);

            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_NOTES"));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

            if (lookupValuesDTOList == null)
            {
                lookupValuesDTOList = new List<LookupValuesDTO>();
            }
            BindingSource bindingSource = new BindingSource();
            lookupValuesDTOList.Insert(0, new LookupValuesDTO());
            bindingSource.DataSource = lookupValuesDTOList;
            NoteTypeDataGridViewCombobox.DataSource = lookupValuesDTOList;
            NoteTypeDataGridViewCombobox.ValueMember = "LookupValueId";
            NoteTypeDataGridViewCombobox.ValueType = typeof(Int32);
            NoteTypeDataGridViewCombobox.DisplayMember = "LookupValue";
            cmbNoteType.DataSource = bindingSource;
            cmbNoteType.ValueMember = "LookupValueId";
            cmbNoteType.DisplayMember = "LookupValue";

            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (Convert.ToInt32(cmbNoteType.SelectedValue) <= 0)
                {
                    MessageBox.Show("Select note type");
                    return;
                }

                if (string.IsNullOrEmpty(txtRemarks.Text.Trim()))
                {
                    MessageBox.Show("Notes cannot be empty");
                    return;
                }

                if (objectId > -1)
                {
                    InventoryNotesDTO inventoryDTO = new InventoryNotesDTO();
                    inventoryDTO.NoteTypeId = Convert.ToInt32(cmbNoteType.SelectedValue);
                    inventoryDTO.ParafaitObjectName = objectName;
                    inventoryDTO.ParafaitObjectId = objectId;
                    inventoryDTO.Notes = txtRemarks.Text;
                    InventoryNotes inventoryNotes = new InventoryNotes(Utilities.ExecutionContext,inventoryDTO);
                    inventoryNotes.Save();
                }
                cmbNoteType.SelectedValue = -1;
                txtRemarks.ResetText();
                PopulateInventoryNoteGrid();

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
        }

        void PopulateInventoryNoteGrid()
        {
            log.LogMethodEntry();
            try
            {
                InventoryNotesList inventoryNotesList = new InventoryNotesList();

                List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> inventaoryNotesSearchParams = new List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>>();
                inventaoryNotesSearchParams.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_ID, objectId.ToString()));
                inventaoryNotesSearchParams.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_NAME, objectName.ToString()));
                List<InventoryNotesDTO> inventoryNotesListOnDisplay = inventoryNotesList.GetAllInventoryNotes(inventaoryNotesSearchParams);
                BindingSource inventoryNotesListBS = new BindingSource();

                if (inventoryNotesListOnDisplay != null)
                {
                    inventoryNotesListBS.DataSource = inventoryNotesListOnDisplay;
                }
                else
                {
                    inventoryNotesListBS.DataSource = new List<InventoryNotesDTO>();
                }

                dgvInventoryNotes.DataSource = inventoryNotesListBS;

                log.LogMethodExit();
            }

            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateInventoryNoteGrid();
            log.LogMethodExit();
        }

        private void dgvInventoryNotes_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show("Error in Inventory Notes grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + dgvInventoryNotes.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
    }
}
