/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - frm Product Activity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2         13-Aug-2019   Deeksha          Added logger methods.
 *2.70.2         18-Dec-2019   Jinto Thomas     Added parameter execution context for userrolrbl declaration with userid  
 *2.100.0        14-Aug-2020   Deeksha          Modified for Recipe Management enhancement.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class frmProductActivity : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        object gProductId, gLocationId;
        Utilities utilities;
        private List<EntityExclusionDetailDTO> entityExclusionDetailDTOList;
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public frmProductActivity(object ProductId, object LocationId, Utilities _Utilities)
        {
            log.LogMethodEntry(ProductId, LocationId, _Utilities);
            InitializeComponent();
            gProductId = ProductId;
            gLocationId = LocationId;
            utilities = _Utilities;
            MyPrintDocument = new PrintDocument();
            MyPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(MyPrintDocument_PrintPage);
            UserRoles userRoles = new UserRoles(_Utilities.ExecutionContext, Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.RoleId);
            entityExclusionDetailDTOList = userRoles.GetUIFieldsToHide("InventoryAdjustment");
            log.LogMethodExit();
        }

        private void frmProductActivity_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
                ProductActivityViewList productActivityViewList = new ProductActivityViewList(machineUserContext);
                List<ProductActivityViewDTO> productActivityViewDisplayList = productActivityViewList.GetProductActivity((Convert.ToInt32(gLocationId)), (Convert.ToInt32(gProductId)));
                if (productActivityViewDisplayList != null)
                {
                    dgvProductActivity.DataSource = productActivityViewDisplayList;

                    for (int i = 0; i < productActivityViewDisplayList.Count; i++)
                    {
                        if (productActivityViewDisplayList[i].TransferLocationId == -1)
                            productActivityViewDisplayList[i].TransferLocationId = productActivityViewDisplayList[i].LocationId;
                        if (productActivityViewDisplayList[i].UOMId == -1 && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                        {
                            productActivityViewDisplayList[i].UOMId = ProductContainer.productDTOList.Find(x => x.ProductId == productActivityViewDisplayList[i].ProductId).InventoryUOMId;
                        }
                        dgvProductActivity.Rows[i].Cells["txtUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == productActivityViewDisplayList[i].UOMId).UOM;
                    }

                    List<LocationDTO> locationIdDTOList;
                    LocationList locationList = new LocationList(machineUserContext);
                    List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> locationDTOSearchParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                    locationIdDTOList = locationList.GetAllLocations(locationDTOSearchParameters, null, true);
                    if (locationIdDTOList == null)
                    {
                        locationIdDTOList = new List<LocationDTO>();
                    }

                    locationIdDTOList.Insert(0, new LocationDTO());
                    locationIdDTOList[0].Name = "None";
                    locationIdDataGridViewTextBoxColumn.DataSource = locationIdDTOList;
                    locationIdDataGridViewTextBoxColumn.DisplayMember = "Name";
                    locationIdDataGridViewTextBoxColumn.ValueMember = "LocationId";

                    if (locationIdDTOList == null)
                    {
                        locationIdDTOList = new List<LocationDTO>();
                    }
                    locationIdDTOList.Insert(0, new LocationDTO());
                    locationIdDTOList[0].Name = "None";
                    transferLocationIdDataGridViewTextBoxColumn.DataSource = locationIdDTOList;
                    transferLocationIdDataGridViewTextBoxColumn.DisplayMember = "Name";
                    transferLocationIdDataGridViewTextBoxColumn.ValueMember = "LocationId";

                    utilities.setupDataGridProperties(ref dgvProductActivity);
                    dgvProductActivity.BackgroundColor = this.BackColor;
                    dgvProductActivity.Columns["Trx_Date"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
                    dgvProductActivity.Columns["Quantity"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
                    dgvProductActivity.Columns["Quantity"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;

                    lblHeading.Text = this.Text;
                    dgvProductActivity.BorderStyle = BorderStyle.FixedSingle;
                    utilities.setLanguage(this);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            utilities.ExportToExcel(dgvProductActivity, lblHeading.Text.Replace(':', '-'), "Product Activity for Product Id: " + gProductId.ToString());
            log.LogMethodExit();
        }

        PrintDocument MyPrintDocument;
        DataGridViewPrinter MyDataGridViewPrinter;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (SetupThePrinting())
            {
                try
                {
                    MyPrintDocument.Print();
                }
                catch (Exception ex)
                {
                    log.Error("Error while executing btnPrint_Click()" + ex.Message);
                    MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Print Error"));
                }
            }
            log.LogMethodExit();
        }

        private void MyPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            log.LogMethodEntry();
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
            log.LogMethodExit();
        }

        private void dgvProductActivity_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (entityExclusionDetailDTOList != null && entityExclusionDetailDTOList.Count > 0)
            {
                foreach (EntityExclusionDetailDTO entityExclusionDetailDTO in entityExclusionDetailDTOList)
                {
                    if (entityExclusionDetailDTO.FieldName == "AvlQuantity" &&  dgvProductActivity.Columns[e.ColumnIndex].DataPropertyName == "Quantity")
                    {
                        //dgvProducts.Rows[e.RowIndex].Tag = e.Value;
                        e.Value = new String('\u25CF', 6);
                    }
                }
            }
            log.LogMethodExit();
        }

        // The printing setup function
        private bool SetupThePrinting()
        {
            log.LogMethodEntry();
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.UseEXDialog = true;

            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
            {
                log.LogMethodExit(false);
                return false;
            }

            MyPrintDocument.DocumentName = lblHeading.Text.Replace(':', '-');
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;

            string reportTitle = lblHeading.Text + ' ' + DateTime.Now.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
            MyDataGridViewPrinter = new DataGridViewPrinter(dgvProductActivity,
                        MyPrintDocument, true, true, reportTitle, new Font("Tahoma", 12,
                        FontStyle.Bold, GraphicsUnit.Point), Color.Black, true);

            log.LogMethodExit(true);
            return true;
        }

    }
}
