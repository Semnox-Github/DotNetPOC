/********************************************************************************************
 * Project Name -InventoryDocumentType
 * Description  -UI of  InventoryDocumentType 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        6-Aug-2019    Deeksha          Modified:Added log() methods.
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class InventoryDocumentTypesUI : Form
    {
        int siteId = -1;
        BindingSource inventoryDocumentTypeListBS;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        public InventoryDocumentTypesUI(int _SiteId)
        {
            log.LogMethodEntry(_SiteId);
            InitializeComponent();
            executionContext.SetSiteId(_SiteId);
            siteId = _SiteId;
            log.LogMethodExit();
        }

        private void InventoryDocumentTypesUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateGrid();
            log.LogMethodExit();
        }
        void PopulateGrid()
        {

            log.LogMethodEntry();
            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
            List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1" ));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, siteId.ToString()));
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeListOnDisplay = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
            inventoryDocumentTypeListBS = new BindingSource();
            if (inventoryDocumentTypeListOnDisplay != null)
            {
               
                SortableBindingList<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOSortList = new SortableBindingList<InventoryDocumentTypeDTO>(inventoryDocumentTypeListOnDisplay);
                inventoryDocumentTypeListBS.DataSource = inventoryDocumentTypeDTOSortList;
            }
            else
                inventoryDocumentTypeListBS.DataSource = new SortableBindingList<InventoryDocumentTypeDTO>();
            inventoryDocumentTypeListBS.AddingNew += dgvDocumentTypes_BindingSourceAddNew;
            dgvDocumentTypes.DataSource = inventoryDocumentTypeListBS;
            log.LogMethodExit();
        }
        private void dgvDocumentTypes_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvDocumentTypes.Rows.Count == inventoryDocumentTypeListBS.Count)
            {
                inventoryDocumentTypeListBS.RemoveAt(inventoryDocumentTypeListBS.Count - 1);
            }
            log.LogMethodExit();
        }
    }
}
