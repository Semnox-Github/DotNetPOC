/********************************************************************************************
*Project Name -  frmInventoryStockDetails                                                                         
*Description  - Form to show inventory item stock information
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*2.70        24-Jun-2019            Archana                     Created
*2.70.2        12-Nov-2019            Archana                     Modified to add LastReceivedDate field 
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class frmInventoryStockDetails : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId = -1;
        ExecutionContext executionContext;
        public frmInventoryStockDetails(ExecutionContext executioncontext, int productId)
        {
            log.LogMethodEntry(productId);
            InitializeComponent();
            this.executionContext = executioncontext;
            this.productId = productId;
            SetupGridProperty();
            log.LogMethodExit();
        }

        private void frmInventoryStockDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadStockInformation();
            PopulateLastReceivedDateField(productId);
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void  SetupGridProperty()
        {
            log.LogMethodEntry();
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setupDataGridProperties(ref dgvItemInfo);
            log.LogMethodExit();
        }
        void LoadStockInformation()
        {
            log.LogMethodEntry();
            ProductBL productBL = new ProductBL(executionContext, productId);
            if (productBL != null && productBL.getProductDTO != null)
            {
                txtProductCode.Text = productBL.getProductDTO.Code;
                txtProdDescription.Text = productBL.getProductDTO.Description;

                InventoryList inventoryList = new InventoryList();
                List<InventoryDTO> listInventoryDTO = inventoryList.GetProductStockDetails(productId);
                if (listInventoryDTO != null && listInventoryDTO.Count > 0)
                {
                    try
                    {
                        foreach (InventoryDTO invItem in listInventoryDTO)
                        {
                            DataGridViewRow dataRow = (DataGridViewRow)dgvItemInfo.Rows[0].Clone();
                            dataRow.Cells[DataGridViewLocationColumn.Index].Value = invItem.LocationName;
                            dataRow.Cells[DataGridViewQuantityColumn.Index].Value = invItem.Quantity.ToString();
                            dataRow.Cells[dataGridViewLotIdColumn.Index].Value = invItem.LotId != -1 ? invItem.LotId.ToString() : string.Empty;
                            dataRow.Cells[DataGridViewLotNumberColumn.Index].Value = invItem.LotNumber;
                            dataRow.Cells[DataGridViewOriginalQuantityColumn.Index].Value = invItem.InventoryLotDTO != null ? invItem.InventoryLotDTO.OriginalQuantity.ToString() : string.Empty;
                            dataRow.Cells[dataGridViewBalanceQtyColumn.Index].Value = invItem.InventoryLotDTO != null ? invItem.InventoryLotDTO.BalanceQuantity.ToString(Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.NUMBER_FORMAT) : string.Empty;
                            dataRow.Cells[dataGridViewExpiryDateColumn.Index].Value = (invItem.InventoryLotDTO != null && invItem.InventoryLotDTO.Expirydate != DateTime.MinValue) ? invItem.InventoryLotDTO.Expirydate.ToString(Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.DATE_FORMAT) : string.Empty;
                            if(invItem.UOMId == -1 && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                            {
                                invItem.UOMId = ProductContainer.productDTOList.Find(x => x.ProductId == invItem.ProductId).InventoryUOMId;
                            }
                            dataRow.Cells[txtUOM.Index].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == invItem.UOMId).UOM;
                            dgvItemInfo.Rows.Add(dataRow);
                        }
                        dgvItemInfo.Columns["DataGridViewQuantityColumn"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewNumericCellStyle();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            log.LogMethodExit();
        }

        void PopulateLastReceivedDateField(int productId)
        {
            log.LogMethodEntry();
            try
            {
                txtLastReceivedDate.Text = string.Empty;
                InventoryReceiptList inventoryReceiptsList = new InventoryReceiptList(executionContext);
                List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParams = new List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>>
                {
                new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.HAS_PRODUCT_ID, productId.ToString()),
                new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                List<InventoryReceiptDTO> inventoryReceiptDTOList = inventoryReceiptsList.GetAllInventoryReceipts(searchParams);
                if (inventoryReceiptDTOList != null && inventoryReceiptDTOList.Count > 0)
                {
                    txtLastReceivedDate.Text = inventoryReceiptDTOList[inventoryReceiptDTOList.Count - 1].ReceiveDate.ToString(Semnox.Parafait.Inventory.CommonFuncs.Utilities.getDateFormat());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }           
            log.LogMethodExit();
        }
    }
}
