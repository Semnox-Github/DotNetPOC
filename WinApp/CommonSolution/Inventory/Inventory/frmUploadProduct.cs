/********************************************************************************************
 * Project Name - eZee Inventory
 * Description  - Upload Inventory products
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************
 *2.50.0      28-Nov-2018      Guru S A       site fix
 *2.50.0      15-Feb-2018      Archana        Redemption gift search and Inventory UI changes
 *2.60.0      09-Apr-2019      Archana        Include/Exclude pos products from redemption
 *2.60        11-Apr-2019      Girish Kundar  Modified : Replaced Purchase Tax 3 tier with Tax 3 tier
 *2.70.2      21-Dec-2019      Girish Kundar  Modified : Inventory Constructor added category name field
 *2.80.0      29-Apr-2020      Jinto Thomas   inventory upload product feature to include sales tax field for manual products
 *2.90.0      03-Jun-2020      Deeksha        Modified : Inventory weighted average costing changes.
 *2.90.0      03-Aug-2020      Deeksha        Modified Upload download feature to accept Y or N as excel input.
 *2.90.0      14-Aug-2020      Deeksha        Modified Upload download feature to accept Y or N as excel input.
 *2.100.0     13-Sep-2020      Deeksha        Modified for Recipe Management Enhancement
 *********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class frmUploadProduct : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private object ProductId;
        private int MasterSiteId = -1;
        private int manualProductTypeId = -1;
        private List<TaxDTO> salesTaxListDTO;
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        UOMContainer uomcontainer;

        public frmUploadProduct(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            if (Semnox.Parafait.Inventory.CommonFuncs.Utilities == null)
            {
                Semnox.Parafait.Inventory.CommonFuncs.Utilities = this.utilities;
                Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv = this.utilities.ParafaitEnv;
            }
            utilities.setLanguage(this);
            btnCreateProduct.Visible = false;
            log.LogMethodExit();
        }

        public frmUploadProduct(object pProductId, object ProductCode, int pMasterSiteId)
        {
            log.LogMethodEntry(pProductId, ProductCode, pMasterSiteId);
            InitializeComponent();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            if (Semnox.Parafait.Inventory.CommonFuncs.Utilities == null)
            {
                Semnox.Parafait.Inventory.CommonFuncs.Utilities = this.utilities;
                Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv = this.utilities.ParafaitEnv;
            }
            ProductId = pProductId;
            MasterSiteId = pMasterSiteId;
            btnFormat.Visible = btnUpload.Visible = lnkError.Visible = false;
            btnCreateProduct.Visible = true;
            this.Text = "Create product [" + ProductCode.ToString() + "] in HQ (Master) Site";
            log.LogMethodExit();
        }

        private void UploadProducts_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            uomcontainer = CommonFuncs.GetUOMContainer();
            PopulateVendor();
            PopulateLocation();
            PopulateUOM();
            PopulatePurchaseTax();
            PopulateCategory();
            SetManualProductTypeId();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                if (MasterSiteId != -1)
                {
                    int saveSiteId = utilities.ParafaitEnv.SiteId;
                    utilities.ParafaitEnv.SiteId = MasterSiteId;
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(UOMBindingSource);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(VendorBindingSource);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(CategoryBindingSource);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(locationDTOBindingSource2);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(loactionDTObindingSource1);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(TaxBindingSource);
                    Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.SiteId = saveSiteId;
                }
                else
                {
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(UOMBindingSource);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(VendorBindingSource);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(CategoryBindingSource);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(locationDTOBindingSource2);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(loactionDTObindingSource1);
                    Semnox.Parafait.Inventory.CommonFuncs.setSiteIdFilter(TaxBindingSource);
                }
            }

            lblMessage.Text = lnkError.Text = "";

            log.LogMethodExit();
        }

        void SetManualProductTypeId()
        {
            log.LogMethodEntry();
            ProductTypeListBL productTypeListBL = new ProductTypeListBL(machineUserContext);
            List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, "MANUAL"),
                new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
        };
            List<ProductTypeDTO> listProductTypeDTOs = productTypeListBL.GetProductTypeDTOList(searchParameters);
            if (listProductTypeDTOs != null && listProductTypeDTOs.Any())
            {
                manualProductTypeId = listProductTypeDTOs[0].ProductTypeId;
            }
            log.LogMethodExit();
        }

        void PopulateVendor()
        {
            log.LogMethodEntry();
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorListSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            vendorListSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, machineUserContext.GetSiteId().ToString()));
            vendorListSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.IS_ACTIVE, "Y"));
            VendorList vendorList = new VendorList(machineUserContext);
            List<VendorDTO> vendorListOnDisplay = vendorList.GetAllVendors(vendorListSearchParams);
            if (vendorListOnDisplay == null)
            {
                vendorListOnDisplay = new List<VendorDTO>();
            }
            cmbVendor.DataSource = vendorListOnDisplay;
            cmbVendor.DisplayMember = "Name";
            cmbVendor.ValueMember = "VendorId";

            log.LogMethodExit();
        }

        void PopulateLocation()
        {
            log.LogMethodEntry();
            List<LocationDTO> outBoundLocationDTOList;
            List<LocationDTO> inBoundLocationDTOList;
            LocationList locationList = new LocationList(machineUserContext);
            BindingSource inboundLocationBS = new BindingSource();
            inBoundLocationDTOList = locationList.GetAllLocations("Store");
            if (inBoundLocationDTOList == null)
            {
                inBoundLocationDTOList = new List<LocationDTO>();
            }
            if (inBoundLocationDTOList.Count > 0)
            {
                inboundLocationBS.DataSource = inBoundLocationDTOList.OrderBy(inLocation => inLocation.Name);
            }
            else
            {
                inboundLocationBS.DataSource = inBoundLocationDTOList;
            }

            cmbInboundLocation.DataSource = inboundLocationBS;
            cmbInboundLocation.ValueMember = "LocationId";
            cmbInboundLocation.DisplayMember = "Name";


            BindingSource outboundLocationBS = new BindingSource();
            string outboundLocationString = "Store";
            outboundLocationString = outboundLocationString + "," + "Department";
            outBoundLocationDTOList = locationList.GetAllLocations(outboundLocationString);
            if (outBoundLocationDTOList == null)
            {
                outBoundLocationDTOList = new List<LocationDTO>();
            }
            if (outBoundLocationDTOList.Count > 0)
            {
                outboundLocationBS.DataSource = outBoundLocationDTOList.OrderBy(outLocation => outLocation.Name);
            }
            else
            {
                outboundLocationBS.DataSource = outBoundLocationDTOList;
            }

            cmbOutboundLocation.DataSource = outboundLocationBS;
            cmbOutboundLocation.ValueMember = "LocationId";
            cmbOutboundLocation.DisplayMember = "Name";

            log.LogMethodExit();
        }

        void PopulatePurchaseTax()
        {
            log.LogMethodEntry();
            List<TaxDTO> taxDTOList;
            TaxList taxList = new TaxList(machineUserContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxDTOSearchParams;
            taxDTOSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
            BindingSource taxBS = new BindingSource();
            taxDTOList = taxList.GetAllTaxes(taxDTOSearchParams);

            if (taxDTOList == null)
            {
                taxDTOList = new List<TaxDTO>();
            }
            if (taxDTOList.Count > 0)
            {
                taxBS.DataSource = taxDTOList.OrderBy(tax => tax.TaxName);
            }
            else
            {
                taxBS.DataSource = taxDTOList;
            }
            cmbTax.DataSource = taxBS;
            cmbTax.ValueMember = "TaxId";
            cmbTax.DisplayMember = "TaxName";

            log.LogMethodExit();
        }

        void PopulateCategory()
        {
            log.LogMethodEntry();
            CategoryList categoryList = new CategoryList(machineUserContext);
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categoryListSearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            categoryListSearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            categoryListSearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "Y"));
            List<CategoryDTO> categoryListOnDisplay = categoryList.GetAllCategory(categoryListSearchParams);

            if (categoryListOnDisplay == null)
            {
                categoryListOnDisplay = new List<CategoryDTO>();
            }
            BindingSource categoryBS = new BindingSource();
            if (categoryListOnDisplay.Count > 0)
            {
                categoryBS.DataSource = categoryListOnDisplay.OrderBy(category => category.Name);
            }
            else
            {
                categoryBS.DataSource = categoryListOnDisplay;
            }
            cmbCategory.DataSource = categoryBS;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryId";

            log.LogMethodExit();
        }

        void PopulateUOM()
        {
            log.LogMethodEntry();
            List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomListSearchParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
            uomListSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, machineUserContext.GetSiteId().ToString()));
            uomListSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.IS_ACTIVE, "1"));
            UOMDataHandler uomDataHandler = new UOMDataHandler();
            List<UOMDTO> uomListOnDisplay = uomDataHandler.GetUOMList(uomListSearchParams);

            if (uomListOnDisplay == null)
            {
                uomListOnDisplay = new List<UOMDTO>();
            }
            BindingSource uomBS = new BindingSource();
            if (uomListOnDisplay.Count > 0)
            {
                uomBS.DataSource = uomListOnDisplay.OrderBy(uom => uom.UOM);
            }
            else
            {
                uomBS.DataSource = uomListOnDisplay;
            }

            cmbUOM.DataSource = uomBS;
            cmbUOM.DisplayMember = "UOM";
            cmbUOM.ValueMember = "UOMId";
            log.LogMethodExit();
        }
        //Start update 6-May-2016
        //string[] ExcelColumns = { "Code", "Description", "Category", "Price In Tickets", 
        //                           "Cost", "Reorder Point", "Reorder Qty", "Sale Price",
        //                           "Vendor Name", "Bar Code", "Remarks", "Redeemable",
        //                           "Sellable", "Inventory Qty", "UOM" };

        //Added function to dynamically create columns in excel that is downloaded
        string[] GetExcelColumns()
        {
            log.LogMethodEntry();
            //Updated column list to exclude "Inventory Qty" 23-Feb-2017
            string[] ExcelColumns = { "Code","Product Name", "Description", "Category", "Price In Tickets",
                                   "Cost", "Reorder Point", "Reorder Qty", "Sale Price",
                                   "Vendor Name", "Bar Code",
                                   "LotControlled(Y/N)", "MarketListItem(Y/N)", "ExpiryType", "IssuingApproach", "ExpiryDays",
                                   "Remarks", "Redeemable(Y/N)",
                                   "Sellable(Y/N)", "UOM","Item Markup %", "Auto Update PIT?(Y/N)", "Display In POS?(Y/N)", "Display Group",
                "HSN SAC Code", "Sales Tax", "CostIncludesTax(Y/N)","Item Type" , "Yield Percentage", "IncludeInPlan(Y/N)" , "Recipe Description" ,
                "Inventory UOM", "Preparation Time" ,
                "Opening Qty", "Receive Price", "Expiry Date"};
            int ExcelColumnsOriginalLength = ExcelColumns.Length;

            log.LogVariableState("ExcelColumns", ExcelColumns);
            DataTable dtSegments = GetSegments();
            log.LogVariableState("dtSegments", dtSegments);
            if (dtSegments.Rows.Count > 0)
            {
                string[] Segments = new string[dtSegments.Rows.Count];
                for (int i = 0; i < dtSegments.Rows.Count; i++)
                {
                    Segments[i] = Convert.ToString(dtSegments.Rows[i]["name"]);
                }
                Array.Resize<string>(ref ExcelColumns, ExcelColumnsOriginalLength + Segments.Length);
                Array.Copy(Segments, 0, ExcelColumns, ExcelColumnsOriginalLength, Segments.Length);
            }

            log.LogMethodExit(ExcelColumns);
            return ExcelColumns;
        }

        DataTable GetSegments()
        {
            log.LogMethodEntry();
            SqlParameter sqlParam;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                sqlParam = new SqlParameter("@site_id", machineUserContext.GetSiteId());
            }
            else
            {
                sqlParam = new SqlParameter("@site_id", -1);
            }
            log.LogVariableState("sqlParam", sqlParam);
            DataTable dt = utilities.executeDataTable(@"select segmentname name, d.segmentdefinitionid, datasourcetype, d.isMandatory
                                from segment_definition d, segment_definition_source_mapping m
                                where d.isactive = 'Y'
	                                and applicableentity = 'PRODUCT'
	                                and d.segmentdefinitionid = m.segmentdefinitionid
                                    and m.isactive = 'Y'  and (d.site_id = @site_id or @site_id = -1)", sqlParam);
            log.LogMethodExit(dt);
            return dt;
        }

        void SaveExcelFormat()
        {
            log.LogMethodEntry();
            string[] ExcelColumns = GetExcelColumns(); //Added 6-May-2016
            Excel.Application objApp;
            objApp = new Excel.Application();

            if (objApp == null)
            {
                log.LogMethodExit("objApp == null");
                return;
            }

            Excel._Workbook objBook;

            Excel.Workbooks objBooks;
            Excel.Sheets objSheets;
            Excel._Worksheet objSheet;
            Excel.Range range;

            try
            {
                // Instantiate Excel and start a new workbook.
                objApp = new Excel.Application();
                objBooks = objApp.Workbooks;
                objBook = objBooks.Add(Missing.Value);
                objBook.Title = "Upload Products";
                objSheets = objBook.Worksheets;
                objSheet = (Excel._Worksheet)objSheets.get_Item(1);
                objSheet.Name = "Products";

                //Get the range where the starting cell has the address
                //m_sStartingCell and its dimensions are m_iNumRows x m_iNumCols.
                range = objSheet.get_Range("A1", Missing.Value);

                range = range.get_Resize(1, ExcelColumns.Length);

                string[,] raSet = new string[1, ExcelColumns.Length];
                for (long iCol = 0; iCol < ExcelColumns.Length; iCol++)
                {
                    //Put the row and column address in the cell.
                    raSet[0, iCol] = ExcelColumns[iCol];
                }

                //Set the range value to the array.
                range.set_Value(Missing.Value, raSet);
                range.Font.Bold = true;
                range.Columns.AutoFit();

                //Return control of Excel to the user.
                objApp.Visible = true;
                objApp.UserControl = true;
            }
            catch (Exception theException)
            {
                log.Error(theException);
                String errorMessage;
                errorMessage = utilities.MessageUtils.getMessage("Error") + " : ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, utilities.MessageUtils.getMessage("Error"));
            }
            log.LogMethodExit();
        }

        DataTable ReadExcel()
        {
            log.LogMethodEntry();
            string[] ExcelColumns = GetExcelColumns();//Added 6-May-2016
            Excel.Application objApp;
            objApp = new Excel.Application();
            if (objApp == null)
            {
                log.LogMethodExit("objApp == null");
                return null;
            }

            Excel.Sheets objSheets;
            Excel._Worksheet objSheet;
            Excel.Range range;
            try
            {
                try
                {
                    //Get a reference to the first sheet of the workbook.

                    objApp.Visible = false;
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.FileName = "*.xls";
                    fileDialog.Title = "Upload Products";
                    fileDialog.Filter = "Excel Files (*.xls)|*.xls|Excel Files (*.xlsx)|*.xlsx";
                    if (fileDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        log.LogMethodExit("fileDialog.ShowDialog() == DialogResult.Cancel");
                        return null;
                    }

                    objSheets = ((Excel.Workbook)objApp.Workbooks.Open(fileDialog.FileName, false, true, 1, Missing.Value, Missing.Value, true, Missing.Value,
                                                        Missing.Value, false, false, Missing.Value, false, false, false)).Sheets;

                    objSheet = (Excel._Worksheet)objSheets.get_Item(1);
                }

                catch (Exception theException)
                {
                    String errorMessage;
                    errorMessage = utilities.MessageUtils.getMessage(881);

                    MessageBox.Show(errorMessage + " " + theException.Message, utilities.MessageUtils.getMessage("Missing Workbook?"));
                    log.LogMethodExit("Exception theException");
                    //You can't automate Excel if you can't find the data you created, so 
                    //leave the subroutine.
                    return null;
                }

                //Get a range of data.
                char endCol = Convert.ToChar((int)'A' + ExcelColumns.Length - 1);
                //range = objSheet.get_Range("A1", endCol.ToString() + "5000");
                range = objSheet.UsedRange; //Added statement to fix excel upload issue 8-Feb-2017

                //Retrieve the data from the range.
                Object[,] saRet;
                saRet = (System.Object[,])range.get_Value(Missing.Value);

                //Determine the dimensions of the array.
                long iRows;
                long iCols;
                iRows = saRet.GetUpperBound(0);
                iCols = saRet.GetUpperBound(1);

                object[] valueArray = new object[iCols];
                object value;

                DataTable dt = new DataTable();
                foreach (string col in ExcelColumns)
                {
                    dt.Columns.Add(col);
                }
                dt.Columns.Add("Status");

                for (long rowCounter = 2; rowCounter <= iRows; rowCounter++)
                {
                    if (saRet[rowCounter, 1] == null || string.IsNullOrEmpty(saRet[rowCounter, 1].ToString()))
                        continue;
                    for (long colCounter = 1; colCounter <= iCols; colCounter++)
                    {
                        value = saRet[rowCounter, colCounter];
                        valueArray[colCounter - 1] = (value == null ? DBNull.Value : value);
                    }
                    dt.Rows.Add(valueArray);
                    //Write in a new line.
                }
                log.LogMethodExit(dt);
                return (dt);
            }
            catch (Exception theException)
            {
                log.Error(theException);
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, utilities.MessageUtils.getMessage("Error"));
                log.LogMethodExit("Exception theException 2");
                return null;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DataTable dt = ReadExcel();
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(882));
                        log.LogMethodExit("dt.Rows.Count == 0");
                        return;
                    }
                    UploadData(dt);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Error"));
            }
            log.LogMethodExit();
        }

        void UploadData(DataTable dt)
        {
            log.LogMethodEntry(dt);
            DataTable dtSegments = GetSegments();

            List<ProductDisplayGroupFormatDTO> listProductDisplayGroupFormatDTO = new List<ProductDisplayGroupFormatDTO>();
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(machineUserContext);
            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, (machineUserContext.GetSiteId().ToString() == null ? "-1" : machineUserContext.GetSiteId().ToString())));
            listProductDisplayGroupFormatDTO = productDisplayGroupList.GetAllProductDisplayGroup(searchParams);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Display Group"] != DBNull.Value)
                {
                    if (listProductDisplayGroupFormatDTO != null && listProductDisplayGroupFormatDTO.Any())
                    {
                        if (listProductDisplayGroupFormatDTO.Any(x => x.DisplayGroup == (dt.Rows[i]["Display Group"]).ToString()))
                        {
                            continue;
                        }
                        else
                        {
                            throw new Exception(dt.Rows[i]["Display Group"].ToString() + " " + utilities.MessageUtils.getMessage(2078));
                        }
                    }
                }
                else
                {
                    dt.Rows[i]["Display Group"] = "Parafait Inventory Products";
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Category"] != DBNull.Value)
                {
                    List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categorySearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                    categorySearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.NAME, dt.Rows[i]["Category"].ToString()));
                    categorySearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    CategoryDataHandler categoryDataHandler = new CategoryDataHandler();// new CategoryDataHandler();
                    List<CategoryDTO> categoryListDTO = new List<CategoryDTO>();
                    CategoryDTO categoryDTO = new CategoryDTO();
                    categoryListDTO = categoryDataHandler.GetCategoryList(categorySearchParams);
                    if (categoryListDTO == null || categoryListDTO.Any() == false)
                    {
                        categoryDTO.Name = dt.Rows[i]["Category"].ToString();
                        categoryDTO.IsActive = true;
                        categoryDTO.LastUpdatedUserId = utilities.ParafaitEnv.Username;
                        if (machineUserContext.GetSiteId().ToString() == null)
                            categoryDTO.Site_Id = -1;
                        else
                            categoryDTO.Site_Id = Convert.ToInt32(machineUserContext.GetSiteId().ToString());
                        categoryDTO.ParentCategoryId = -1;
                        categoryDataHandler.InsertCategory(categoryDTO, utilities.ParafaitEnv.LoginID, (machineUserContext.GetSiteId().ToString() == null ? -1 : Convert.ToInt32(machineUserContext.GetSiteId().ToString())), null);
                    }
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Vendor Name"] != DBNull.Value)
                {
                    List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                    vendorSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.NAME, dt.Rows[i]["Vendor Name"].ToString()));
                    vendorSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, machineUserContext.GetSiteId().ToString()));
                    VendorDataHandler vendorDataHandler = new VendorDataHandler();
                    List<VendorDTO> vendorListDTO = new List<VendorDTO>();
                    VendorDTO vendorDTO = new VendorDTO();
                    vendorListDTO = vendorDataHandler.GetVendorList(vendorSearchParams);

                    if (vendorListDTO == null || vendorListDTO.Any() == false)
                    {
                        vendorDTO.Name = dt.Rows[i]["Vendor Name"].ToString();
                        vendorDTO.Remarks = "";
                        vendorDTO.DefaultPaymentTermsId = -1;
                        vendorDTO.Address1 = "";
                        vendorDTO.Address2 = "";
                        vendorDTO.City = "";
                        vendorDTO.State = "";
                        vendorDTO.Country = "";
                        vendorDTO.PostalCode = "";
                        vendorDTO.AddressRemarks = "";
                        vendorDTO.ContactName = "";
                        vendorDTO.Phone = "";
                        vendorDTO.Fax = "";
                        vendorDTO.Email = "";
                        vendorDTO.IsActive = true;
                        vendorDTO.Website = "";
                        vendorDTO.VendorMarkupPercent = 0.0;
                        vendorDTO.PurchaseTaxId = -1;
                        vendorDataHandler.InsertVendor(vendorDTO, utilities.ParafaitEnv.LoginID, (machineUserContext.GetSiteId().ToString() == null ? -1 : Convert.ToInt32(machineUserContext.GetSiteId().ToString())));
                    }
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["uom"] != DBNull.Value)
                {
                    List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomSearchParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
                    uomSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.UOM, dt.Rows[i]["uom"].ToString()));
                    uomSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, machineUserContext.GetSiteId().ToString()));
                    UOMDataHandler uOMDataHandler = new UOMDataHandler();
                    List<UOMDTO> uomListDTO = new List<UOMDTO>();
                    UOMDTO uomDTO = new UOMDTO();
                    uomListDTO = uOMDataHandler.GetUOMList(uomSearchParams);

                    if (uomListDTO == null || uomListDTO.Any() == false)
                    {
                        uomDTO.UOM = dt.Rows[i]["uom"].ToString();
                        uomDTO.Remarks = dt.Rows[i]["uom"].ToString();
                        uOMDataHandler.InsertUOM(uomDTO, machineUserContext.GetUserId().ToString(), (machineUserContext.GetSiteId().ToString() == null ? -1 : Convert.ToInt32(machineUserContext.GetSiteId().ToString())));
                    }
                }
            }

            int successRowCount = 0;
            int productID = -1;
            lblMessage.Text = lnkError.Text = "";
            ProductList productBL = new ProductList();

            double openingQuantity = 0;
            double receivePrice = 0;
            int lotId = -1;
            DateTime expiryDate = DateTime.MinValue;
            InventoryTransactionList inventoryTransactionList = new InventoryTransactionList(machineUserContext);
            int inventoryTransactionTypeId = inventoryTransactionList.GetInventoryTransactionTypeId("OpeningQuantity");

            TaxList salesTaxList = new TaxList(machineUserContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchBySTaxParameters;
            searchBySTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            searchBySTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
            if (utilities.ParafaitEnv.IsCorporate)
                searchBySTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            salesTaxListDTO = salesTaxList.GetAllTaxes(searchBySTaxParameters);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool bBreak = false;
                openingQuantity = 0;
                receivePrice = 0;
                expiryDate = DateTime.MinValue;

                ProductDTO productDTO = new ProductDTO();
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                productSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH, dt.Rows[i]["Code"].ToString().Trim()));
                if (utilities.ParafaitEnv.IsCorporate)
                    productSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                ProductDataHandler productDataHandler = new ProductDataHandler();
                List<ProductDTO> productListDTO = new List<ProductDTO>();
                productListDTO = productDataHandler.GetProductList(productSearchParams);

                //Check if product already exists
                if (productListDTO == null || productListDTO.Any() == false)
                {
                    productID = -1;
                }
                else//Inventory product exist
                {
                    productDTO = productListDTO[0];
                    productID = productDTO.ProductId;
                }

                //If product does not exist, add product
                if (productID == -1)
                {
                    using (SqlConnection TrxCnn = utilities.createConnection())
                    {
                        using (SqlTransaction SQLTrx = TrxCnn.BeginTransaction())
                        {
                            try
                            {
                                productDTO = new ProductDTO();
                                productDTO.CategoryId = GetCategoryId((dt.Rows[i]["Category"] == DBNull.Value || dt.Rows[i]["Category"] == null) ? cmbCategory.Text : dt.Rows[i]["Category"].ToString());
                                productDTO.DefaultVendorId = GetVendorId((dt.Rows[i]["Vendor Name"] == DBNull.Value || dt.Rows[i]["Vendor Name"] == null) ? cmbVendor.Text : dt.Rows[i]["Vendor Name"].ToString());
                                string uomName = dt.Rows[i]["UOM"].ToString();
                                if (string.IsNullOrEmpty(uomName))
                                {
                                    uomName = cmbUOM.Text;
                                }
                                int productUOMId = GetUOMId(uomName);
                                InventoryList inventoryList = new InventoryList(machineUserContext);
                                decimal stock = inventoryList.GetProductStockQuantity(productDTO.ProductId);
                                if (stock <= 0)
                                {
                                    productDTO.UomId = productUOMId;
                                    List<UOMDTO> uomDTOList = uomcontainer.relatedUOMDTOList.FirstOrDefault(x => x.Key == productUOMId).Value;
                                    string inventoryUOMName = dt.Rows[i]["Inventory UOM"].ToString();
                                    if (!string.IsNullOrEmpty(inventoryUOMName))
                                    {
                                        UOMDTO uomDTO = (uomDTOList != null && uomDTOList.Any()) ? uomDTOList.Find(x => x.UOM == inventoryUOMName) : null;
                                        if (uomDTO != null)
                                        {
                                            productDTO.InventoryUOMId = uomDTO.UOMId;
                                        }
                                        else
                                        {
                                            productDTO.InventoryUOMId = productUOMId;
                                        }

                                    }
                                    else
                                    {
                                        productDTO.InventoryUOMId = productUOMId;
                                    }
                                }
                                productDTO.Code = dt.Rows[i]["Code"].ToString().Trim();
                                productDTO.ProductName = dt.Rows[i]["Product Name"].ToString().Trim();
                                productDTO.Description = dt.Rows[i]["Description"].ToString().Trim();
                                productDTO.RecipeDescription = dt.Rows[i]["Recipe Description"].ToString().Trim();
                                if (dt.Rows[i]["Price In Tickets"] != DBNull.Value || dt.Rows[i]["Price In Tickets"] != null)
                                {
                                    if (dt.Rows[i]["Price In Tickets"].ToString() == "")
                                        productDTO.PriceInTickets = 0;
                                    else
                                        productDTO.PriceInTickets = Convert.ToDouble(dt.Rows[i]["Price In Tickets"].ToString().Trim());
                                }
                                else
                                    productDTO.PriceInTickets = 0;

                                if (dt.Rows[i]["Cost"] != DBNull.Value || dt.Rows[i]["Cost"] != null)
                                {
                                    if (dt.Rows[i]["Cost"].ToString() == "")
                                        productDTO.Cost = 0;
                                    else
                                        productDTO.Cost = Convert.ToDouble(dt.Rows[i]["Cost"].ToString().Trim());
                                }
                                else
                                    productDTO.Cost = 0;
                                if (dt.Rows[i]["Reorder Point"] != DBNull.Value || dt.Rows[i]["Reorder Point"] != null)
                                {
                                    if (dt.Rows[i]["Reorder Point"].ToString() == "")
                                        productDTO.ReorderPoint = 0;
                                    else
                                        productDTO.ReorderPoint = Convert.ToDouble(dt.Rows[i]["Reorder Point"].ToString().Trim());
                                }
                                else
                                    productDTO.ReorderPoint = 0;
                                if (dt.Rows[i]["Reorder Qty"] != DBNull.Value || dt.Rows[i]["Reorder Qty"] != null)
                                {
                                    if (dt.Rows[i]["Reorder Qty"].ToString() == "")
                                        productDTO.ReorderQuantity = 0;
                                    else
                                        productDTO.ReorderQuantity = Convert.ToDouble(dt.Rows[i]["Reorder Qty"].ToString().Trim());
                                }
                                else
                                    productDTO.ReorderQuantity = 0;

                                productDTO.PurchaseTaxId = cmbTax.SelectedIndex < 0 ? -1 : (cmbTax.SelectedValue == null || cmbTax.SelectedValue == DBNull.Value) ? -1 : Convert.ToInt32(cmbTax.SelectedValue);

                                if (dt.Rows[i]["Sale Price"] != DBNull.Value || dt.Rows[i]["Sale Price"] != null)
                                {
                                    if (dt.Rows[i]["Sale Price"].ToString() == "")
                                    {
                                        productDTO.SalePrice = 0;
                                    }
                                    else
                                    {
                                        productDTO.SalePrice = Convert.ToDouble(dt.Rows[i]["Sale Price"].ToString().Trim());
                                    }
                                }
                                else
                                {
                                    productDTO.SalePrice = 0;
                                }

                                if (dt.Rows[i]["Remarks"] != DBNull.Value || dt.Rows[i]["Remarks"] != null)
                                {
                                    if (dt.Rows[i]["Remarks"].ToString() == "")
                                        productDTO.Remarks = "";
                                    else
                                        productDTO.Remarks = dt.Rows[i]["Remarks"].ToString().Trim();
                                }
                                else
                                    productDTO.Remarks = "";

                                string isRedeemable = dt.Rows[i]["Redeemable(Y/N)"] == DBNull.Value ? "Y" : dt.Rows[i]["Redeemable(Y/N)"].ToString().Trim().Substring(0, 1).ToUpper();
                                if (isRedeemable.StartsWith("Y") || isRedeemable.StartsWith("1"))
                                {
                                    productDTO.IsRedeemable = "Y";
                                }
                                else if (isRedeemable.StartsWith("N") || isRedeemable.StartsWith("0"))
                                {
                                    productDTO.IsRedeemable = "N";
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "Redeemable(Y/N)"));
                                }

                                string isSellable = dt.Rows[i]["Sellable(Y/N)"] == DBNull.Value ? "Y" : dt.Rows[i]["Sellable(Y/N)"].ToString().Trim().Substring(0, 1).ToUpper();
                                if (isSellable.StartsWith("Y") || isSellable.StartsWith("1"))
                                {
                                    productDTO.IsSellable = "Y";
                                }
                                else if (isSellable.StartsWith("N") || isSellable.StartsWith("0"))
                                {
                                    productDTO.IsSellable = "N";
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "Sellable(Y/N)"));
                                }

                                string lotControlled = (dt.Rows[i]["LotControlled(Y/N)"] == DBNull.Value || dt.Rows[i]["LotControlled(Y/N)"] == null) ? "N" : dt.Rows[i]["LotControlled(Y/N)"].ToString().ToUpper();
                                if (lotControlled.StartsWith("Y") || lotControlled.StartsWith("1"))
                                {
                                    productDTO.LotControlled = true;
                                }
                                else if (lotControlled.StartsWith("N") || lotControlled.StartsWith("0"))
                                {
                                    productDTO.LotControlled = false;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "LotControlled(Y/N)"));
                                }

                                string marketListItem = (dt.Rows[i]["MarketListItem(Y/N)"] == DBNull.Value || dt.Rows[i]["MarketListItem(Y/N)"] == null) ? "N" : dt.Rows[i]["MarketListItem(Y/N)"].ToString().ToUpper();
                                if (marketListItem.StartsWith("Y") || marketListItem.StartsWith("1"))
                                {
                                    productDTO.MarketListItem = true;
                                }
                                else if (marketListItem.StartsWith("N") || marketListItem.StartsWith("0"))
                                {
                                    productDTO.LotControlled = false;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "MarketListItem(Y / N)"));
                                }
                                productDTO.ExpiryType = (dt.Rows[i]["ExpiryType"] == DBNull.Value || dt.Rows[i]["ExpiryType"] == null) ? "N" : dt.Rows[i]["ExpiryType"].ToString();
                                productDTO.IssuingApproach = (dt.Rows[i]["IssuingApproach"] == DBNull.Value || dt.Rows[i]["IssuingApproach"] == null) ? "None" : dt.Rows[i]["IssuingApproach"].ToString();
                                productDTO.ExpiryDays = (dt.Rows[i]["ExpiryDays"] == DBNull.Value || dt.Rows[i]["ExpiryDays"] == null) ? 0 : Convert.ToInt32(dt.Rows[i]["ExpiryDays"]);
                                string costIncludesTax = dt.Rows[i]["CostIncludesTax(Y/N)"] == DBNull.Value ? "Y" : dt.Rows[i]["CostIncludesTax(Y/N)"].ToString().ToUpper();
                                if (costIncludesTax.StartsWith("Y") || costIncludesTax.StartsWith("1"))
                                {
                                    productDTO.CostIncludesTax = true;
                                }
                                else if (costIncludesTax.StartsWith("N") || costIncludesTax.StartsWith("0"))
                                {
                                    productDTO.CostIncludesTax = false;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "CostIncludesTax(Y/N)"));
                                }
                                //Start update 21-Feb-2017
                                //Added condition 
                                if (productDTO.LotControlled && productDTO.IssuingApproach == "None")
                                {
                                    if (productDTO.ExpiryType == "E" || productDTO.ExpiryType == "D")
                                        productDTO.IssuingApproach = "FEFO";
                                    else
                                        productDTO.IssuingApproach = "FIFO";
                                }
                                else
                                    productDTO.IssuingApproach = (dt.Rows[i]["IssuingApproach"] == DBNull.Value || dt.Rows[i]["IssuingApproach"] == null) ? "None" : dt.Rows[i]["IssuingApproach"].ToString();
                                //End update 21-Feb-2017
                                productDTO.DefaultLocationId = Convert.ToInt32(cmbInboundLocation.SelectedValue);
                                productDTO.OutboundLocationId = Convert.ToInt32(cmbOutboundLocation.SelectedValue);
                                productDTO.ItemMarkupPercent = (dt.Rows[i]["Item Markup %"] == DBNull.Value || dt.Rows[i]["Item Markup %"] == null || dt.Rows[i]["Item Markup %"].ToString() == "NaN") ? double.NaN : Convert.ToDouble(dt.Rows[i]["Item Markup %"]);
                                string autoUpdateMarkupValue = (dt.Rows[i]["Auto Update PIT?(Y/N)"] == DBNull.Value || dt.Rows[i]["Auto Update PIT?(Y/N)"] == null) ? "N" : dt.Rows[i]["Auto Update PIT?(Y/N)"].ToString().ToUpper();
                                if (autoUpdateMarkupValue.StartsWith("Y") || autoUpdateMarkupValue.StartsWith("1"))
                                {
                                    productDTO.AutoUpdateMarkup = true;
                                }
                                else if (autoUpdateMarkupValue.StartsWith("N") || autoUpdateMarkupValue.StartsWith("0"))
                                {
                                    productDTO.AutoUpdateMarkup = false;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "Auto Update PIT?(Y/N)"));
                                }


                                string itemType = dt.Rows[i]["Item Type"].ToString();
                                if (string.IsNullOrEmpty(itemType))
                                {
                                    itemType = "STANDARD_ITEM";
                                }
                                if (itemType.StartsWith("Semi"))
                                {
                                    itemType = "SEMI_FINISHED_ITEM";
                                }
                                else if (itemType.StartsWith("Finished"))
                                {
                                    itemType = "FINISHED_ITEM";
                                }
                                else
                                {
                                    itemType = "STANDARD_ITEM";
                                }
                                List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                                lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("PRODUCT_ITEM_TYPE", machineUserContext.GetSiteId());
                                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                                {
                                    LookupValuesDTO lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == itemType).FirstOrDefault();
                                    if (lookupValuesDTO != null)
                                    {
                                        productDTO.ItemType = Convert.ToInt32(lookupValuesDTO.LookupValueId);
                                    }
                                }
                                if (productDTO.IsRedeemable == "N" && productDTO.AutoUpdateMarkup)
                                    productDTO.AutoUpdateMarkup = false;
                                if (productDTO.AutoUpdateMarkup && productDTO.IsRedeemable == "Y")
                                {
                                    try
                                    {  //recalculate PIT.
                                        double newPITValue = productBL.calculatePITByMarkUp(productDTO.Cost, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);
                                        if (newPITValue > 0)
                                            productDTO.PriceInTickets = newPITValue;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        dt.Rows[i]["Status"] += ex.Message;
                                        continue;
                                    }
                                }
                                productDTO.IsPurchaseable = (cbxIsPurchasable.Checked == true ? "Y" : "N");

                                if (dt.Rows[i]["Yield Percentage"] == DBNull.Value || dt.Rows[i]["Yield Percentage"] == null)
                                {
                                    productDTO.YieldPercentage = null;
                                }
                                else 
                                {
                                    try
                                    {
                                        productDTO.YieldPercentage = Convert.ToDecimal(dt.Rows[i]["Yield Percentage"].ToString().Trim());
                                        if (productDTO.YieldPercentage < 0)
                                        {
                                            throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 2857));
                                            //Please enter valid yield percentage value.
                                        }
                                        else
                                        {
                                            log.LogVariableState("Yield Percentage", productDTO.YieldPercentage);
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        log.Error(ex);
                                        dt.Rows[i]["Status"] += ex.Message;
                                        continue;
                                    }
                                }

                                if (dt.Rows[i]["Preparation Time"] == DBNull.Value || dt.Rows[i]["Preparation Time"] == null)
                                {
                                    productDTO.PreparationTime = null;
                                }
                                else
                                {
                                   productDTO.PreparationTime = Convert.ToInt32(dt.Rows[i]["Preparation Time"].ToString().Trim());
                                }

                                string includeInPlan = dt.Rows[i]["IncludeInPlan(Y/N)"] == DBNull.Value ? "N" : dt.Rows[i]["IncludeInPlan(Y/N)"].ToString().ToUpper();
                                if (includeInPlan.StartsWith("Y") || includeInPlan.StartsWith("1"))
                                {
                                    productDTO.IncludeInPlan = true;
                                }
                                else if (includeInPlan.StartsWith("N") || includeInPlan.StartsWith("0"))
                                {
                                    productDTO.IncludeInPlan = false;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "IncludeInPlan(Y/N)"));
                                }

                                try
                                {
                                    if (!String.IsNullOrEmpty(dt.Rows[i]["Opening Qty"].ToString()))
                                        openingQuantity = (dt.Rows[i]["Opening Qty"] == DBNull.Value || dt.Rows[i]["Opening Qty"] == null || dt.Rows[i]["Opening Qty"].ToString() == "NaN") ? double.NaN : Convert.ToDouble(dt.Rows[i]["Opening Qty"]);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    dt.Rows[i]["Status"] += utilities.MessageUtils.getMessage(1365);
                                    continue;
                                }

                                if (openingQuantity != 0 && openingQuantity != double.NaN)
                                {
                                    try
                                    {
                                        if (dt.Rows[i]["Receive Price"].ToString().Length == 0)
                                        {
                                            dt.Rows[i]["Status"] += utilities.MessageUtils.getMessage(1738);
                                            continue;
                                        }
                                        else
                                            receivePrice = Convert.ToDouble(dt.Rows[i]["Receive Price"]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        dt.Rows[i]["Status"] += utilities.MessageUtils.getMessage(1364);
                                        continue;
                                    }
                                    try
                                    {
                                        if (productDTO.LotControlled)
                                        {
                                            if (dt.Rows[i]["Expiry Date"].ToString().Length == 0)
                                            {
                                                dt.Rows[i]["Status"] += utilities.MessageUtils.getMessage(1363);
                                                continue;
                                            }
                                            else
                                                expiryDate = DateTime.Parse(dt.Rows[i]["Expiry Date"].ToString());
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        dt.Rows[i]["Status"] += utilities.MessageUtils.getMessage(597);
                                        continue;
                                    }
                                }
                                if (dt.Rows[i]["Sales Tax"] != DBNull.Value || dt.Rows[i]["Sales Tax"] != null)
                                {
                                    if (!string.IsNullOrEmpty(dt.Rows[i]["Sales Tax"].ToString().Trim()))
                                    {
                                        try
                                        {
                                            GetSalesTaxID(dt.Rows[i]["Sales Tax"].ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            dt.Rows[i]["Status"] += ex.Message;
                                            continue;
                                        }

                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                //dt.Rows[i]["Status"] = ex.Message;
                                dt.Rows[i]["Status"] += ex.Message;
                                continue;
                            }//End update 06-May-2016
                            string[] delimiter = new string[] { "|" }; //20-May-2016 changed delimiter to '|' instead of '||'
                            string[] barCode = dt.Rows[i]["Bar Code"].ToString().Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                            try
                            {
                                //manualProductId = RefreshManualProducts(-1, dt.Rows[i], SQLTrx);
                                productID = RefreshManualProducts(productDTO, dt.Rows[i], SQLTrx);
                                //productDTO.ManualProductId = manualProductId;
                                //productID = productDataHandler.InsertProduct(productDTO, utilities.ParafaitEnv.LoginID.ToString(), (machineUserContext.GetSiteId().ToString() == null ? -1 : Convert.ToInt32(machineUserContext.GetSiteId().ToString())), (dt.Rows[i]["Category"] == DBNull.Value || dt.Rows[i]["Category"] == null) ? cmbCategory.Text : dt.Rows[i]["Category"].ToString(), (dt.Rows[i]["Vendor Name"] == DBNull.Value || dt.Rows[i]["Vendor Name"] == null) ? cmbVendor.Text : dt.Rows[i]["Vendor Name"].ToString(), (dt.Rows[i]["UOM"] == DBNull.Value || dt.Rows[i]["UOM"] == null) ? cmbUOM.Text : dt.Rows[i]["UOM"].ToString(), SQLTrx);

                                if (productID != -1)
                                {
                                    using (SqlCommand cmdBarcode = new SqlCommand())
                                    {
                                        cmdBarcode.Connection = TrxCnn;
                                        cmdBarcode.Transaction = SQLTrx;

                                        if (dt.Rows[i]["Bar Code"] != DBNull.Value || dt.Rows[i]["Bar Code"] != null)
                                        {
                                            //Updated query to update value of LastUpdatedDate column 19-Apr-2016 
                                            cmdBarcode.CommandText = "insert into productbarcode(barcode, productid, isactive, lastupdated_userid, site_id, LastUpdatedDate) values(@barcode, @productid, @isactive, @lastupdated_userid, @site_id, getdate())";

                                            for (int j = 0; j < barCode.Length; j++)
                                            {
                                                if (string.IsNullOrEmpty(barCode[j]))
                                                    continue;

                                                DataTable dtBarcode = new DataTable();

                                                using (SqlCommand cmdBarcodeExists = new SqlCommand())
                                                {
                                                    string condition = "";

                                                    cmdBarcodeExists.Connection = TrxCnn;
                                                    cmdBarcodeExists.Transaction = SQLTrx;
                                                    cmdBarcodeExists.Parameters.Clear();
                                                    if (utilities.ParafaitEnv.IsCorporate)
                                                    {
                                                        condition = "  and (b.site_id = @siteId or @siteId = -1)";
                                                        cmdBarcodeExists.Parameters.AddWithValue("@siteId", machineUserContext.GetSiteId().ToString());
                                                    }
                                                    cmdBarcodeExists.CommandText = "select * from product p, productbarcode b where p.productid = b.productid and p.isactive = 'Y' and b.isactive = 'Y' and b.barcode = @barcode " + condition;
                                                    cmdBarcodeExists.Parameters.AddWithValue("@barcode", barCode[j]);
                                                    SqlDataAdapter da = new SqlDataAdapter(cmdBarcodeExists);
                                                    da.Fill(dtBarcode);

                                                    if (dtBarcode.Rows.Count > 0)
                                                    {
                                                        dt.Rows[i]["Status"] = "Barcode exists";
                                                        SQLTrx.Rollback();
                                                        bBreak = true;
                                                        //break; 
                                                    }
                                                    if (bBreak) // check if inner loop set break
                                                        break;

                                                    cmdBarcode.Transaction = SQLTrx;
                                                    cmdBarcode.Parameters.Clear();
                                                    cmdBarcode.Parameters.AddWithValue("@barcode", barCode[j]);
                                                    cmdBarcode.Parameters.AddWithValue("@productid", productID);
                                                    cmdBarcode.Parameters.AddWithValue("@isactive", 'Y');
                                                    cmdBarcode.Parameters.AddWithValue("@lastupdated_userid", utilities.ParafaitEnv.LoginID);
                                                    //Added site_id condition for insert
                                                    if (machineUserContext.GetSiteId() == -1)
                                                    {
                                                        cmdBarcode.Parameters.AddWithValue("@site_id", DBNull.Value);
                                                    }
                                                    else
                                                    {
                                                        cmdBarcode.Parameters.AddWithValue("@site_id", machineUserContext.GetSiteId().ToString());
                                                    }
                                                    cmdBarcode.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }

                                    if (openingQuantity != 0 && openingQuantity != Double.NaN)
                                    {
                                        try
                                        {

                                            lotId = -1;
                                            if (productDTO.LotControlled)
                                            {
                                                InventoryLotDTO inventoryLotRecDTO = new InventoryLotDTO(-1, "", openingQuantity, openingQuantity, receivePrice,
                                                                                                    -1, expiryDate, true, machineUserContext.GetSiteId(), "", false, -1, "", ServerDateTime.Now, machineUserContext.GetUserId(), ServerDateTime.Now, machineUserContext.GetUserId(), productDTO.InventoryUOMId);
                                                InventoryLotBL inventoryLotBL = new InventoryLotBL(inventoryLotRecDTO, machineUserContext);
                                                inventoryLotBL.Save(SQLTrx);
                                                lotId = inventoryLotRecDTO.LotId;
                                            }
                                            InventoryDTO inventoryDTO = new InventoryDTO(productID, productDTO.DefaultLocationId, openingQuantity, ServerDateTime.Now, machineUserContext.GetUserId(), 0,
                                                                        machineUserContext.GetSiteId(), "", false, "", -1, lotId, receivePrice, -1, "", "", "", "", null, "", "", "", "", 0, 0, "", "", ServerDateTime.Now, DateTime.Now, "", "", "", "", productDTO.InventoryUOMId);
                                            Inventory inventoryBl = new Inventory(inventoryDTO, machineUserContext);
                                            inventoryBl.Save(SQLTrx);
                                            double taxPercentage = GetTaxPercentage(productDTO.PurchaseTaxId);



                                            InventoryTransactionDTO inventoryTransactionDTO = new InventoryTransactionDTO(-1, -1, ServerDateTime.Now, machineUserContext.GetUserId(), utilities.ParafaitEnv.POSMachine,
                                                                                                        productID, productDTO.DefaultLocationId, openingQuantity, -1, taxPercentage, productDTO.TaxInclusiveCost, -1, utilities.ParafaitEnv.POSMachineId,
                                                                                                        machineUserContext.GetSiteId(), "", false, -1, inventoryTransactionTypeId, lotId, "", "", machineUserContext.GetUserId(), ServerDateTime.Now, machineUserContext.GetUserId(), ServerDateTime.Now, productDTO.InventoryUOMId);
                                            InventoryTransactionBL inventoryTransactionBL = new InventoryTransactionBL(inventoryTransactionDTO, machineUserContext);
                                            inventoryTransactionBL.Save(SQLTrx);

                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            dt.Rows[i]["Status"] += ex.Message;
                                            continue;
                                        }
                                    }

                                    if (bBreak)
                                        continue;
                                }

                                if (bBreak)
                                    continue;

                                SQLTrx.Commit();
                                //Start update 06-May-2016
                                //Added code to see that segment values are updated
                                string message = "";
                                if (dtSegments.Rows.Count > 0)
                                {
                                    //13-Jun-2016 Updated code to pass the right productid to SaveSegmentData function
                                    if (!SaveSegmentData(dt.Rows[i], dtSegments, productID, ref message))
                                    {
                                        dt.Rows[i]["Status"] = message;
                                        continue;
                                    }
                                }//End update 06-May-2016
                                dt.Rows[i]["Status"] = "Success";
                                successRowCount++;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                dt.Rows[i]["Status"] = ex.Message;
                                SQLTrx.Rollback();
                            }
                        }
                    }
                }
                else //Update product if it already exists
                {
                    using (SqlConnection TrxCnn = utilities.createConnection())
                    {
                        using (SqlTransaction SQLTrx = TrxCnn.BeginTransaction())
                        {
                            try
                            {
                                productDTO.Description = dt.Rows[i]["Description"].ToString().Trim();
                                productDTO.RecipeDescription = dt.Rows[i]["Recipe Description"].ToString().Trim();
                                if (productDTO.ProductName == "")
                                {
                                    productDTO.ProductName = productDTO.Code.ToString();
                                }
                                else
                                {
                                    productDTO.ProductName = dt.Rows[i]["Product Name"].ToString().Trim();
                                }

                                if (dt.Rows[i]["Price In Tickets"] != DBNull.Value || dt.Rows[i]["Price In Tickets"] != null)
                                {
                                    if (dt.Rows[i]["Price In Tickets"].ToString() == "")
                                        productDTO.PriceInTickets = 0;
                                    else
                                        productDTO.PriceInTickets = Convert.ToDouble(dt.Rows[i]["Price In Tickets"].ToString().Trim());
                                }
                                else
                                    productDTO.PriceInTickets = 0;
                                productDTO.InnerPackQty = 1;

                                if (dt.Rows[i]["Cost"] != DBNull.Value || dt.Rows[i]["Cost"] != null)
                                {
                                    if (dt.Rows[i]["Cost"].ToString() == "")
                                        productDTO.Cost = 0;
                                    else
                                        productDTO.Cost = Convert.ToDouble(dt.Rows[i]["Cost"].ToString().Trim());
                                }
                                else
                                    productDTO.Cost = 0;
                                if (dt.Rows[i]["Reorder Point"] != DBNull.Value || dt.Rows[i]["Reorder Point"] != null)
                                {
                                    if (dt.Rows[i]["Reorder Point"].ToString() == "")
                                        productDTO.ReorderPoint = 0;
                                    else
                                        productDTO.ReorderPoint = Convert.ToDouble(dt.Rows[i]["Reorder Point"].ToString().Trim());
                                }
                                else
                                    productDTO.ReorderPoint = 0;
                                if (dt.Rows[i]["Reorder Qty"] != DBNull.Value || dt.Rows[i]["Reorder Qty"] != null)
                                {
                                    if (dt.Rows[i]["Reorder Qty"].ToString() == "")
                                        productDTO.ReorderQuantity = 0;
                                    else
                                        productDTO.ReorderQuantity = Convert.ToDouble(dt.Rows[i]["Reorder Qty"].ToString().Trim());
                                }
                                else
                                    productDTO.ReorderQuantity = 0;

                                if (dt.Rows[i]["Sale Price"] != DBNull.Value || dt.Rows[i]["Sale Price"] != null)
                                {
                                    if (dt.Rows[i]["Sale Price"].ToString() == "")
                                        productDTO.SalePrice = 0;
                                    else
                                        productDTO.SalePrice = Convert.ToDouble(dt.Rows[i]["Sale Price"].ToString().Trim());
                                }
                                else
                                    productDTO.SalePrice = 0;

                                if (dt.Rows[i]["Remarks"] != DBNull.Value || dt.Rows[i]["Remarks"] != null)
                                {
                                    if (dt.Rows[i]["Remarks"].ToString() == "")
                                        productDTO.Remarks = "";
                                    else
                                        productDTO.Remarks = dt.Rows[i]["Remarks"].ToString().Trim();
                                }
                                else
                                    productDTO.Remarks = "";

                                //Start update 21-Feb-2017
                                //Added condition 
                                if (productDTO.LotControlled && productDTO.IssuingApproach == "None")
                                {
                                    if (productDTO.ExpiryType == "E" || productDTO.ExpiryType == "D")
                                        productDTO.IssuingApproach = "FEFO";
                                    else
                                        productDTO.IssuingApproach = "FIFO";
                                }
                                else
                                    productDTO.IssuingApproach = (dt.Rows[i]["IssuingApproach"] == DBNull.Value || dt.Rows[i]["IssuingApproach"] == null) ? "None" : dt.Rows[i]["IssuingApproach"].ToString();
                                //End update 21-Feb-2017
                                string isRedeemable = dt.Rows[i]["Redeemable(Y/N)"] == DBNull.Value ? "Y" : dt.Rows[i]["Redeemable(Y/N)"].ToString().Trim().Substring(0, 1).ToUpper();
                                if (isRedeemable.StartsWith("Y") || isRedeemable.StartsWith("1"))
                                {
                                    productDTO.IsRedeemable = "Y";
                                }
                                else if (isRedeemable.StartsWith("N") || isRedeemable.StartsWith("0"))
                                {
                                    productDTO.IsRedeemable = "N";
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "Redeemable(Y/N)"));
                                }

                                string isSellable = dt.Rows[i]["Sellable(Y/N)"] == DBNull.Value ? "Y" : dt.Rows[i]["Sellable(Y/N)"].ToString().Trim().Substring(0, 1).ToUpper();
                                if (isSellable.StartsWith("Y") || isSellable.StartsWith("1"))
                                {
                                    productDTO.IsSellable = "Y";
                                }
                                else if (isSellable.StartsWith("N") || isSellable.StartsWith("0"))
                                {
                                    productDTO.IsSellable = "N";
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "Sellable(Y/N)"));
                                }
                                string itemType = dt.Rows[i]["Item Type"].ToString();
                                if (string.IsNullOrEmpty(itemType))
                                {
                                    itemType = "STANDARD_ITEM";
                                }
                                if (itemType.StartsWith("Semi"))
                                {
                                    itemType = "SEMI_FINISHED_ITEM";
                                }
                                else if (itemType.StartsWith("Finished"))
                                {
                                    itemType = "FINISHED_ITEM";
                                }
                                else
                                {
                                    itemType = "STANDARD_ITEM";
                                }
                                List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                                lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("PRODUCT_ITEM_TYPE", machineUserContext.GetSiteId());
                                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                                {
                                    LookupValuesDTO lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == itemType).FirstOrDefault();
                                    if (lookupValuesDTO != null)
                                    {
                                        productDTO.ItemType = Convert.ToInt32(lookupValuesDTO.LookupValueId);
                                    }
                                }

                                string costIncludesTax = dt.Rows[i]["CostIncludesTax(Y/N)"] == DBNull.Value ? "Y" : dt.Rows[i]["CostIncludesTax(Y/N)"].ToString().ToUpper();
                                if (costIncludesTax.StartsWith("Y") || costIncludesTax.StartsWith("1"))
                                {
                                    productDTO.CostIncludesTax = true;
                                }
                                else if (costIncludesTax.StartsWith("N") || costIncludesTax.StartsWith("0"))
                                {
                                    productDTO.CostIncludesTax = false;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "CostIncludesTax(Y/N)"));
                                }
                                productDTO.ProductId = productID;
                                productDTO.ItemMarkupPercent = (dt.Rows[i]["Item Markup %"] == DBNull.Value || dt.Rows[i]["Item Markup %"] == null || dt.Rows[i]["Item Markup %"].ToString() == "NaN") ? double.NaN : Convert.ToDouble(dt.Rows[i]["Item Markup %"]);
                                string autoUpdateMarkupValue = dt.Rows[i]["Auto Update PIT?(Y/N)"] == DBNull.Value ? "N" : dt.Rows[i]["Auto Update PIT?(Y/N)"].ToString().ToUpper();
                                if (autoUpdateMarkupValue.StartsWith("Y") || autoUpdateMarkupValue.StartsWith("1"))
                                {
                                    productDTO.AutoUpdateMarkup = true;
                                }
                                else if (autoUpdateMarkupValue.StartsWith("N") || autoUpdateMarkupValue.StartsWith("0"))
                                {
                                    productDTO.AutoUpdateMarkup = false;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, "Auto Update PIT?(Y/N)"));
                                }
                                string includeInPlan = dt.Rows[i]["IncludeInPlan(Y/N)"] == DBNull.Value ? "N" : dt.Rows[i]["IncludeInPlan(Y/N)"].ToString().ToUpper();
                                if (includeInPlan.StartsWith("Y") || includeInPlan.StartsWith("1"))
                                {
                                    productDTO.IncludeInPlan = true;
                                }
                                else if (includeInPlan.StartsWith("N") || includeInPlan.StartsWith("0"))
                                {
                                    productDTO.IncludeInPlan = false;
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1144, "IncludeInPlan(Y/N)"));
                                }
                                if (dt.Rows[i]["Yield Percentage"] == DBNull.Value || dt.Rows[i]["Yield Percentage"] == null)
                                {
                                    productDTO.YieldPercentage = null;
                                }
                                else
                                {
                                    try
                                    {
                                        productDTO.YieldPercentage = Convert.ToDecimal(dt.Rows[i]["Yield Percentage"].ToString().Trim());
                                        if (productDTO.YieldPercentage < 0 || productDTO.YieldPercentage > 100)
                                        {
                                            throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 2857));
                                            //Please enter valid yield percentage value.
                                        }
                                        else
                                        {
                                            log.LogVariableState("Yield Percentage", productDTO.YieldPercentage);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        dt.Rows[i]["Status"] += ex.Message;
                                        continue;
                                    }
                                }

                                if (dt.Rows[i]["Preparation Time"] == DBNull.Value || dt.Rows[i]["Preparation Time"] == null)
                                {
                                    productDTO.PreparationTime = null;
                                }
                                else
                                {
                                    productDTO.PreparationTime = Convert.ToInt32(dt.Rows[i]["Preparation Time"].ToString().Trim());
                                }
                                if (productDTO.IsRedeemable == "N" && productDTO.AutoUpdateMarkup)
                                    productDTO.AutoUpdateMarkup = false;
                                if (productDTO.AutoUpdateMarkup && productDTO.IsRedeemable == "Y")
                                {
                                    try
                                    {  //recalculate PIT.
                                        double newPITValue = productBL.calculatePITByMarkUp(productDTO.Cost, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);
                                        if (newPITValue > 0)
                                            productDTO.PriceInTickets = newPITValue;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        dt.Rows[i]["Status"] += ex.Message;
                                        continue;
                                    }
                                }
                                productDTO.CategoryId = GetCategoryId((dt.Rows[i]["Category"] == DBNull.Value || dt.Rows[i]["Category"] == null) ? cmbCategory.Text : dt.Rows[i]["Category"].ToString());
                                productDTO.DefaultVendorId = GetVendorId((dt.Rows[i]["Vendor Name"] == DBNull.Value || dt.Rows[i]["Vendor Name"] == null) ? cmbVendor.Text : dt.Rows[i]["Vendor Name"].ToString());
                                string uomName = dt.Rows[i]["UOM"].ToString();
                                if (string.IsNullOrEmpty(uomName))
                                {
                                    uomName = cmbUOM.Text;
                                }
                                int productUOMId = GetUOMId(uomName);
                                InventoryList inventoryList = new InventoryList(machineUserContext);
                                decimal stock = inventoryList.GetProductStockQuantity(productDTO.ProductId);
                                if (stock <= 0)
                                {
                                    productDTO.UomId = productUOMId;
                                    List<UOMDTO> uomDTOList = uomcontainer.relatedUOMDTOList.FirstOrDefault(x => x.Key == productUOMId).Value;
                                    string inventoryUOMName = dt.Rows[i]["Inventory UOM"].ToString();
                                    if (!string.IsNullOrEmpty(inventoryUOMName))
                                    {
                                        UOMDTO uomDTO = (uomDTOList != null && uomDTOList.Any()) ? uomDTOList.Find(x => x.UOM == inventoryUOMName) : null;
                                        if (uomDTO != null)
                                        {
                                            productDTO.InventoryUOMId = uomDTO.UOMId;
                                        }
                                        else
                                        {
                                            productDTO.InventoryUOMId = productUOMId;
                                        }
                                    }
                                    else
                                    {
                                        productDTO.InventoryUOMId = productUOMId;
                                    }
                                }
                                if (dt.Rows[i]["Sales Tax"] != DBNull.Value || dt.Rows[i]["Sales Tax"] != null)
                                {
                                    if (!string.IsNullOrEmpty(dt.Rows[i]["Sales Tax"].ToString().Trim()))
                                    {
                                        try
                                        {
                                            GetSalesTaxID(dt.Rows[i]["Sales Tax"].ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            dt.Rows[i]["Status"] += ex.Message;
                                            continue;
                                        }

                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                //dt.Rows[i]["Status"] = ex.Message;
                                dt.Rows[i]["Status"] += ex.Message;
                                continue;
                            }//End update 06-May-2016


                            using (SqlCommand update_cmd = new SqlCommand())
                            {
                                update_cmd.Connection = TrxCnn;
                                update_cmd.Transaction = SQLTrx;

                                try
                                {
                                    RefreshManualProducts(productDTO, dt.Rows[i], SQLTrx);
                                    //productDTO.ManualProductId = manualProductId;
                                    //productDataHandler.UpdateProduct(productDTO, utilities.ParafaitEnv.LoginID.ToString(), (machineUserContext.GetSiteId().ToString() == null ? -1 : Convert.ToInt32(machineUserContext.GetSiteId().ToString())), SQLTrx);
                                    //ProductDataHandler dataHandler = new ProductDataHandler(SQLTrx);
                                    //dataHandler.Save(productDTO, utilities.ParafaitEnv.LoginID.ToString(), (machineUserContext.GetSiteId().ToString() == null ? -1 : Convert.ToInt32(machineUserContext.GetSiteId().ToString())));
                                    //Start update 23-Feb-2017
                                    if (productDTO.LotControlled && productDTO.IssuingApproach == "FIFO")
                                    {
                                        InventoryLotBL inventoryLot = new InventoryLotBL(machineUserContext);
                                        inventoryLot.UpdateNonLotableToLotable(productDTO.ProductId, SQLTrx);
                                    }
                                    //End update 23-Feb-2017

                                    if (dt.Rows[i]["Bar Code"] != DBNull.Value || dt.Rows[i]["Bar Code"] != null)
                                    {
                                        string[] delimiter = new string[] { "|" }; //20-May-2016 changed delimiter to '|' instead of '||'
                                        string[] barCode = dt.Rows[i]["Bar Code"].ToString().Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                                        DataTable dtBarcode = new DataTable();
                                        SqlDataAdapter da;

                                        using (SqlCommand barcodeExists_cmd = new SqlCommand())
                                        {
                                            barcodeExists_cmd.Connection = TrxCnn;
                                            barcodeExists_cmd.Transaction = SQLTrx;

                                            barcodeExists_cmd.CommandText = "select * from productbarcode where productid = @productid";
                                            barcodeExists_cmd.Parameters.Clear();
                                            barcodeExists_cmd.Parameters.AddWithValue("@productid", productID);
                                            da = new SqlDataAdapter(barcodeExists_cmd);
                                            da.Fill(dtBarcode);

                                            foreach (DataRow drBarcode in dtBarcode.Rows)
                                            {
                                                if (!barCode.Contains(drBarcode["barcode"].ToString()))
                                                {
                                                    update_cmd.Transaction = SQLTrx;
                                                    //Updated query to update value of LastUpdatedDate column 19-Apr-2016 
                                                    update_cmd.CommandText = "update productbarcode set isactive = 'N', LastUpdatedDate = getdate() where ID = @ID";
                                                    update_cmd.Parameters.Clear();
                                                    update_cmd.Parameters.AddWithValue("@ID", drBarcode["ID"]);
                                                    update_cmd.ExecuteNonQuery();
                                                }
                                            }

                                            for (int j = 0; j < barCode.Length; j++)
                                            {
                                                if (string.IsNullOrEmpty(barCode[j]))
                                                    continue;

                                                dtBarcode = new DataTable();

                                                using (SqlCommand cmdBarcodeExists = new SqlCommand())
                                                {
                                                    string condition = "";
                                                    cmdBarcodeExists.Connection = TrxCnn;
                                                    cmdBarcodeExists.Transaction = SQLTrx;
                                                    cmdBarcodeExists.Parameters.Clear();
                                                    cmdBarcodeExists.CommandText = "select * from product p, productbarcode b where p.productid = b.productid and p.isactive = 'Y' and b.isactive = 'Y' and b.barcode = @barcode and b.productid <> @productid " + condition;
                                                    if (utilities.ParafaitEnv.IsCorporate)
                                                    {
                                                        condition = "  and (b.site_id = @siteId or @siteId = -1)";
                                                        cmdBarcodeExists.Parameters.AddWithValue("@siteId", machineUserContext.GetSiteId().ToString());
                                                    }
                                                    cmdBarcodeExists.Parameters.AddWithValue("@barcode", barCode[j]);
                                                    cmdBarcodeExists.Parameters.AddWithValue("@productid", productID);
                                                    //cmdBarcodeExists.Parameters.AddWithValue("@productid", productID);
                                                    da = new SqlDataAdapter(cmdBarcodeExists);
                                                    da.Fill(dtBarcode);

                                                    if (dtBarcode.Rows.Count > 0)
                                                    {
                                                        dt.Rows[i]["Status"] = "Barcode exists";
                                                        SQLTrx.Rollback();
                                                        //break;
                                                        bBreak = true;
                                                        //break;
                                                    }
                                                    if (bBreak) // check if inner loop set break
                                                        break;

                                                    using (SqlCommand cmdBarcode = new SqlCommand())
                                                    {
                                                        cmdBarcode.Connection = TrxCnn;
                                                        cmdBarcode.Transaction = SQLTrx;
                                                        //Updated query to update value of LastUpdatedDate column 19-Apr-2016 
                                                        cmdBarcode.CommandText = @"if not exists(select * from product p, productbarcode b where p.productid = b.productid and p.isactive = 'Y' and b.isactive = 'Y' and b.barcode = @barcode and b.productid = @productid)
                                                           insert into productbarcode(barcode, productid, isactive, lastupdated_userid, site_id, LastUpdatedDate) 
                                                           values(@barcode, @productid, @isactive, @lastupdated_userid, @site_id, getdate())";
                                                        cmdBarcode.Parameters.Clear();
                                                        cmdBarcode.Parameters.AddWithValue("@barcode", barCode[j]);
                                                        cmdBarcode.Parameters.AddWithValue("@productid", productID);
                                                        cmdBarcode.Parameters.AddWithValue("@isactive", 'Y');
                                                        cmdBarcode.Parameters.AddWithValue("@lastupdated_userid", utilities.ParafaitEnv.LoginID);
                                                        //Added site_id condition for insert
                                                        if (machineUserContext.GetSiteId() == -1)
                                                        {
                                                            cmdBarcode.Parameters.AddWithValue("@site_id", DBNull.Value);
                                                        }
                                                        else
                                                        {
                                                            cmdBarcode.Parameters.AddWithValue("@site_id", machineUserContext.GetSiteId().ToString());
                                                        }
                                                        cmdBarcode.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (bBreak) // check if inner loop set break
                                        continue;

                                    SQLTrx.Commit();
                                    //Start update 06-May-2016
                                    //Added code to see that segment values are updated
                                    if (dtSegments.Rows.Count > 0)
                                    {
                                        string message = "";
                                        if (!SaveSegmentData(dt.Rows[i], dtSegments, productID, ref message))
                                        {
                                            dt.Rows[i]["Status"] = message;
                                            continue;
                                        }
                                    }//End update 06-May-2016
                                    dt.Rows[i]["Status"] = "Success";
                                    successRowCount++;
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    SQLTrx.Rollback();
                                    dt.Rows[i]["Status"] = ex.Message;
                                }
                            }
                        }
                    }
                }
            }
            lblMessage.Text = "Successfully loaded " + successRowCount.ToString() + " of " + dt.Rows.Count.ToString() + " records";
            if (dt.Rows.Count - successRowCount > 0)
            {
                lnkError.Text = (dt.Rows.Count - successRowCount).ToString() + " Errors";
                lnkError.Enabled = true;

                int count = dt.Rows.Count;

                for (int i = 0; i < count; i++)
                {
                    if (dt.Rows[i]["Status"].ToString() == "Success")
                    {
                        dt.Rows.RemoveAt(i);
                        count = dt.Rows.Count;
                        i = 0;
                    }
                }
                lnkError.Tag = dt;
            }
            else
            {
                lnkError.Enabled = false;
                lnkError.Tag = null;
            }
            log.LogMethodExit();
        }

        int RefreshManualProducts(ProductDTO inventoryProductDTO, DataRow dt, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            int manualProductId = -1;
            ProductsDTO manualProductDTO = null;
            Products products = null;
            if (inventoryProductDTO.ProductId != -1)
            {
                products = new Products(inventoryProductDTO.ManualProductId);
                if (products.GetProductsDTO != null)
                {
                    manualProductDTO = products.GetProductsDTO;
                }
            }
            if (manualProductDTO == null)
            {
                manualProductDTO = new ProductsDTO();
            }
            manualProductDTO.InventoryItemDTO = inventoryProductDTO;
            manualProductDTO.DisplayGroup = (dt["Display Group"].ToString() == null ? "Parafait Inventory Products" : dt["Display Group"].ToString().Trim());
            manualProductDTO.CategoryId = inventoryProductDTO.CategoryId;
            manualProductDTO.DisplayInPOS = dt["Display In POS?(Y/N)"] == DBNull.Value ? "Y" : dt["Display In POS?(Y/N)"].ToString().Trim().Substring(0, 1).ToUpper();
            manualProductDTO.InventoryProductCode = dt["Code"].ToString().Trim();
            manualProductDTO.ProductName = dt["Product Name"].ToString().Trim();
            manualProductDTO.Description = dt["Description"].ToString().Trim();
            manualProductDTO.DisplayGroup = dt["Display Group"].ToString().Trim();
            manualProductDTO.MapedDisplayGroup = dt["Display Group"].ToString().Trim();
            manualProductDTO.HsnSacCode = dt["HSN SAC Code"].ToString().Trim();
            manualProductDTO.ActiveFlag = true;
            if (dt["Sale Price"] != DBNull.Value || dt["Sale Price"] != null)
            {
                if (dt["Sale Price"].ToString() == "")
                {
                    manualProductDTO.Price = 0;
                }
                else
                {
                    manualProductDTO.Price = Convert.ToDecimal(dt["Sale Price"].ToString().Trim());
                }
            }
            else
            {
                manualProductDTO.Price = 0;
            }
            if (dt["Sales Tax"] != DBNull.Value || dt["Sales Tax"] != null)
            {
                if (string.IsNullOrEmpty(dt["Sales Tax"].ToString().Trim()))
                {
                    manualProductDTO.Tax_id = -1;
                }
                else
                {
                    manualProductDTO.Tax_id = GetSalesTaxID(dt["Sales Tax"].ToString()); 
                }
            }

            try
            {
                manualProductDTO.ProductTypeId = manualProductTypeId;
                Products manualProducts = new Products(machineUserContext, manualProductDTO);
                manualProducts.Save(sqlTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

            log.LogMethodExit(manualProductId);
            return inventoryProductDTO.ProductId;
        }

        private double GetTaxPercentage(int taxId)
        {
            log.LogMethodEntry(taxId);
            double taxPercentage = 0;
            TaxList purchaseTaxList = new TaxList(machineUserContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchByPTaxParameters;
            searchByPTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            searchByPTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_ID, taxId.ToString()));
            searchByPTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
            List<TaxDTO> purchaseTaxListDTO = purchaseTaxList.GetAllTaxes(searchByPTaxParameters);

            if (purchaseTaxListDTO != null && purchaseTaxListDTO.Any())
            {
                if (purchaseTaxListDTO.Count > 1)
                {
                    throw new Exception(utilities.MessageUtils.getMessage(1303, taxId.ToString()));
                }
                else
                {
                    taxPercentage = purchaseTaxListDTO[0].TaxPercentage;
                }
            }
            log.LogMethodExit(taxPercentage);
            return taxPercentage;
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Added try catch block
            try
            {
                SaveExcelFormat();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Error"));
            }
            log.LogMethodExit();
        }

        private void lnkError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DataTable dt = lnkError.Tag as DataTable;
            using (Form f = new Form())
            {
                DataGridView dgv = new DataGridView();
                dgv.Name = "DgvProducts";
                dgv.DataSource = dt;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = dgv.AllowUserToDeleteRows = false;
                utilities.setupDataGridProperties(ref dgv);
                f.Controls.Add(dgv);
                dgv.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                f.Size = new Size(800, 500);
                f.ShowInTaskbar = false;
                f.Text = "Upload Status";
                dgv.Size = new Size(f.Width - 25, f.Height - 100);
                dgv.Location = new Point(5, 5);
                dgv.BackgroundColor = f.BackColor;
                f.StartPosition = FormStartPosition.CenterParent;
                f.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                Button bClose = new Button();
                bClose.Location = new Point(f.Width / 2 - 100, f.Height - 80);
                bClose.Text = "Close";
                bClose.Anchor = AnchorStyles.Bottom;
                f.Controls.Add(bClose);
                f.CancelButton = bClose;

                Button bExcel = new Button();
                bExcel.Location = new Point(f.Width / 2, f.Height - 80);
                bExcel.Text = "Save as Excel";
                bExcel.Width = 120;
                bExcel.Anchor = AnchorStyles.Bottom;
                bExcel.Click += new EventHandler(bExcel_Click);
                f.Controls.Add(bExcel);

                f.Load += new EventHandler(f_Load);

                f.ShowDialog();
            }
            log.LogMethodExit();
        }

        void bExcel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Cursor = Cursors.WaitCursor;
            String file_path = "";

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "Upload Products";
            saveFileDialog1.Filter = "XLS Files (*.xls)|*.xls";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    file_path = saveFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(883) + ex.Message);
                }

                MessageBox.Show(utilities.MessageUtils.getMessage(884), utilities.MessageUtils.getMessage("Save to Disk"));

                using (Form f = (sender as Button).Parent as Form)
                {
                    // exporter.Export(f.Controls["DgvProducts"] as DataGridView, exportSettings);
                    SaveErrorExcel(f.Controls["DgvProducts"] as DataGridView, file_path);
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }


        void f_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //using (Form f = sender as Form)
            {
                Form f = sender as Form;
                DataGridView dgv = f.Controls["DgvProducts"] as DataGridView;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv["Status", i].Value.ToString() != "Success")
                        dgv["Code", i].Style.BackColor = Color.OrangeRed;
                }
            }
            log.LogMethodExit();
        }

        void SaveErrorExcel(DataGridView dgvToBeExported, string fileName)
        {
            log.LogMethodEntry(fileName);
            this.Cursor = Cursors.WaitCursor;

            dgvToBeExported.RowHeadersVisible = false;
            dgvToBeExported.SelectAll();
            DataObject dataObj = dgvToBeExported.GetClipboardContent();
            if (dataObj != null)
            {
                Clipboard.SetDataObject(dataObj);
            }

            Excel.Application objApp = new Excel.Application();
            if (objApp == null)
            {
                log.LogMethodExit("objApp == null");
                return;
            }

            Excel._Workbook objBook;

            Excel.Workbooks objBooks;
            Excel.Sheets objSheets;
            Excel._Worksheet objSheet;
            Excel.Range range;

            try
            {
                // Instantiate Excel and start a new workbook.
                //objApp = new Excel.Application();
                objBooks = objApp.Workbooks;
                objBook = objBooks.Add(Missing.Value);
                objBook.Title = "Upload Errors";
                objSheets = objBook.Worksheets;
                objSheet = (Excel._Worksheet)objSheets.get_Item(1);
                objSheet.Name = "Products";

                //Get the range where the starting cell has the address
                //m_sStartingCell and its dimensions are m_iNumRows x m_iNumCols.
                range = objSheet.get_Range("A1", Missing.Value);

                range = range.get_Resize(1, dgvToBeExported.Columns.Count);

                string[,] raSet = new string[1, dgvToBeExported.Columns.Count];
                for (int iCol = 0; iCol < dgvToBeExported.Columns.Count; iCol++)
                {
                    //Put the row and column address in the cell.
                    raSet[0, iCol] = dgvToBeExported.Columns[iCol].Name;
                }
                //Set the range value to the array.
                range.set_Value(Missing.Value, raSet);
                range.Font.Bold = true;
                range.Columns.AutoFit();
                Excel.Range CR = (Excel.Range)objSheet.Cells[2, 1];
                CR.Select();
                objSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                var firstCell = objSheet.get_Range("A1", "A1");
                firstCell.Select();
                //Return control of Excel to the user.
                objApp.DisplayAlerts = false;
                objApp.ActiveWorkbook.SaveAs(fileName);
                objApp.Visible = true;
                objApp.UserControl = true;

                Clipboard.Clear();
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = utilities.MessageUtils.getMessage("Error") + " : ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, utilities.MessageUtils.getMessage("Error"));
                log.Error(theException);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnCreateProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            using (SqlConnection cnn = utilities.createConnection())
            {
                SqlTransaction SQLTrx = cnn.BeginTransaction();

                try
                {
                    object o = utilities.executeScalar(@"insert into product
                                                   ([Code]
                                                   ,[Description]
                                                   ,[Remarks]
                                                   ,[CategoryId]
                                                   ,[DefaultLocationId]
                                                   ,[ReorderPoint]
                                                   ,[ReorderQuantity]
                                                   ,[UomId]
                                                   ,[MasterPackQty]
                                                   ,[InnerPackQty]
                                                   ,[Picture]
                                                   ,[DefaultVendorId]
                                                   ,[Cost]
                                                   ,[LastPurchasePrice]
                                                   ,[IsRedeemable]
                                                   ,[IsSellable]
                                                   ,[IsPurchaseable]
                                                   ,[LastModUserId]
                                                   ,[LastModDttm]
                                                   ,[IsActive]
                                                   ,[PriceInTickets]
                                                   ,[OutboundLocationId]
                                                   ,[SalePrice]
                                                   ,[TaxInclusiveCost]
                                                   ,[ImageFileName]
                                                   ,[LowerLimitCost]
                                                   ,[UpperLimitCost]
                                                   ,[CostVariancePercentage] 
                                                   ,[ItemMarkupPercent]
                                                   ,[AutoUpdateMarkup]
                                                   ,[ProductName]  
                                                    ,[PurchaseTaxId]
                                                   ,[site_id]
                                                   ,[CostIncludesTax])
                                            select [Code]
                                                   ,[Description]
                                                   ,[Remarks]
                                                   ,@CategoryId
                                                   ,@DefaultLocationId
                                                   ,[ReorderPoint]
                                                   ,[ReorderQuantity]
                                                   ,@UomId
                                                   ,[MasterPackQty]
                                                   ,[InnerPackQty]
                                                   ,[Picture]
                                                   ,@DefaultVendorId
                                                   ,[Cost]
                                                   ,[LastPurchasePrice]
                                                   ,[IsRedeemable]
                                                   ,[IsSellable]
                                                   ,[IsPurchaseable]
                                                   ,[LastModUserId]
                                                   ,[LastModDttm]
                                                   ,[IsActive]
                                                   ,[PriceInTickets]
                                                   ,@OutboundLocationId
                                                   ,[SalePrice]
                                                   ,[TaxInclusiveCost]
                                                   ,[ImageFileName]
                                                   ,[LowerLimitCost]
                                                   ,[UpperLimitCost]
                                                   ,[CostVariancePercentage]
                                                   ,[ItemMarkupPercent]
                                                   ,[AutoUpdateMarkup]
                                                   ,[ProductName]
                                                    ,@PurchaseTaxId
                                                   ,@site_id
                                                   ,[CostIncludesTax]
                                        from product where productId = @productId; select @@identity", SQLTrx,
                                               new SqlParameter[]{ new SqlParameter("@CategoryId", cmbCategory.SelectedValue),
                                                            new SqlParameter("@DefaultLocationId", cmbInboundLocation.SelectedValue),
                                                            new SqlParameter("@UomId", cmbUOM.SelectedValue),
                                                            new SqlParameter("@DefaultVendorId", cmbVendor.SelectedValue),
                                                            new SqlParameter("@OutboundLocationId", cmbOutboundLocation.SelectedValue),
                                                            new SqlParameter("@PurchaseTaxId", cmbTax.SelectedValue),
                                                            new SqlParameter("@site_id", MasterSiteId),
                                                            new SqlParameter("@productId", ProductId) });
                    log.LogVariableState("o", o);
                    if (o != null)
                    {
                        //Updated query to update value of LastUpdatedDate column 19-Apr-2016 
                        utilities.executeNonQuery(@"insert into productbarcode (barcode, productid, isactive, Lastupdated_userid, site_id, LastUpdatedDate) " +
                                                                    " select barcode, @productId, 'Y', @user, @site_id, getdate() from productbarcode where productid = @productid1 " +
                                                                "insert into inventory (productId, locationId, quantity, TimeStamp, Lastupdated_userid, site_id) " +
                                                                    "values (@productId, @locationId, @quantity, getdate(), @user, @site_id)", SQLTrx,
                                                                    new SqlParameter[]
                                                                    {new SqlParameter("@productId", o),
                                                                new SqlParameter("@locationId", cmbOutboundLocation.SelectedValue),
                                                                new SqlParameter("@quantity", SqlDbType.Decimal, 0, ParameterDirection.Input, false, 0, 0, "quantity", DataRowVersion.Current, 0),
                                                                new SqlParameter("@user", utilities.ParafaitEnv.Username),
                                                                new SqlParameter("@productid1", ProductId),
                                                                new SqlParameter("@site_id", MasterSiteId)});
                    }

                    SQLTrx.Commit();
                    MessageBox.Show(utilities.MessageUtils.getMessage(885));
                    Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    SQLTrx.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Added try catch block
            try
            {
                Application.DoEvents();

                ApplicationContext ap = new ApplicationContext();
                System.Threading.ThreadStart thr = delegate
                {
                    using (Form f = new Form())
                    {
                        System.Windows.Forms.Button btnWait;
                        btnWait = new System.Windows.Forms.Button();
                        // Redemption.Redemption.Properties.Resources
                        // btnWait
                        // 
                        btnWait.BackColor = System.Drawing.Color.Transparent;
                        btnWait.BackgroundImage = Semnox.Parafait.Inventory.Properties.Resources.pressed1;
                        btnWait.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                        btnWait.Dock = System.Windows.Forms.DockStyle.Fill;
                        btnWait.FlatAppearance.BorderSize = 0;
                        btnWait.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                        btnWait.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                        btnWait.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btnWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        btnWait.ForeColor = System.Drawing.Color.White;
                        btnWait.Image = Semnox.Parafait.Inventory.Properties.Resources.PreLoader;
                        btnWait.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
                        btnWait.Location = new System.Drawing.Point(0, 0);
                        btnWait.Name = "btnWait";
                        btnWait.Size = new System.Drawing.Size(346, 89);
                        btnWait.TabIndex = 2;
                        btnWait.Text = utilities.MessageUtils.getMessage(683); //Please wait while Excel opens your file
                        btnWait.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                        btnWait.UseVisualStyleBackColor = false;
                        // 
                        // Form
                        // 
                        f.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                        f.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        f.BackColor = System.Drawing.Color.White;
                        f.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                        f.ClientSize = new System.Drawing.Size(346, 89);
                        f.Controls.Add(btnWait);
                        f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                        f.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                        f.TransparencyKey = System.Drawing.Color.White;

                        f.ShowInTaskbar = false;
                        f.TopLevel = f.TopMost = true;
                        try
                        {
                            ap.MainForm = f;
                            Application.Run(ap);
                        }
                        catch (Exception ex) { log.Error(ex); }
                    }
                };
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(thr));
                thread.Start();

                try
                {
                    // SaveExcelWithData();
                    SaveExcelWithDataNew();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    log.Error(ex);
                }
                finally
                {
                    ap.ExitThread();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Error"));
            }
        }

        //Used by download function
        DataTable GetProductData()
        {
            log.LogMethodEntry();
            DataTable dt = new DataTable();
            using (SqlCommand cmd = utilities.getCommand())
            {
                string siteId = machineUserContext.GetSiteId().ToString();
                string condition = " and (p.site_id = " + siteId + " or " + siteId + " = -1) ";
                cmd.CommandText = @"DECLARE @cols AS NVARCHAR(MAX),
	                                @pivot as nvarchar(max) = '',
                                    @query  AS NVARCHAR(MAX)

                                select @cols = STUFF((SELECT ',' + QUOTENAME(SegmentName) 
                                                      from Segment_Definition where (site_id = " + siteId + " or " + siteId + " = -1)" +
                                                      @"FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,'')
                                if(@cols is not null)
	                                set @pivot = 'pivot 
				                                 (
					                                max(valuechar)
					                                for SegmentName in  (' + @cols + N')
				                                 ) p'
                                set @query = N'select Code,ProductName ''Product Name'', Description, Category, PriceInTickets ''Price In Tickets'', 
	                                                Cost, ReorderPoint ''Reorder Point'', ReorderQuantity ''Reorder Qty'', SalePrice ''Sale Price'',
                                                    Name ''Vendor Name'', case when isnull(BarCode, '''') <> '''' then substring(BarCode, 2, len(BarCode)) else BarCode end BarCode,
                                                    LotControlled, MarketListItem, ExpiryType, IssuingApproach, ExpiryDays,
                                                    Remarks, IsRedeemable Redeemable,
	                                                IsSellable Sellable, UOM , ItemMarkupPercent ''Item Markup %'', AutoUpdateMarkup ''Auto Update PIT?'', DisplayInPOS ''Display In POS?'',
                                                    display_group ''Display Group'', HsnSacCode ''HSN SAC Code'',salesTax ''Sales Tax '',CostIncludesTax ,
                                                     ItemType,
													 YieldPercentage, IncludeInPlan, RecipeDescription, InventoryUOM ,PreparationTime ,null,null,null'+ isnull(',' + @cols, '') + N' 
                                                from(
		                                                select Code, p.ProductName, p.Description, c.Name Category, PriceInTickets, 
											                Cost, ReorderPoint, ReorderQuantity, SalePrice, DisplayInPOS, 
                                                            (select top 1 displaygroup 
																from ProductDisplayGroupFormat pdgf, ProductsDisplayGroup pdg 
																where pdgf.Id  = Displaygroupid 
																and pdg.ProductId = ps.product_id
                                                            )display_group,
                                                            HsnSacCode,
											                v.Name,
											                (SELECT ''|''+ BarCode 
														        FROM ProductBarcode 
														        WHERE productid = p.ProductId and isactive = ''Y''
														        FOR XML PATH('''') 
															) BarCode, 
                                                        case isnull(LotControlled, 0) when 0 then ''N'' else ''Y'' end LotControlled,
                                                        case isnull(MarketListItem, 0) when 0 then ''N'' else ''Y'' end MarketListItem,
                                                        isnull(ExpiryType, ''N'') ExpiryType,
                                                        ExpiryDays,
                                                        isnull(IssuingApproach, ''None'') IssuingApproach, 
                                                        p.Remarks, IsRedeemable, t.tax_name as salesTax,
											                IsSellable, UOM, ItemMarkupPercent, case isnull(AutoUpdateMarkup,0) when 0 then ''N'' else ''Y'' end AutoUpdateMarkup ,
                                                            case isnull(CostIncludesTax,1) when 1 then ''Y'' else ''N'' end CostIncludesTax ,
                                                         (select description from lookupValues where lookupvalueid in(
                                                         isnull(p.ItemType , (select lookupValueId from LookUpValues where LookupValue = ''STANDARD_ITEM'')))) ItemType,
														 YieldPercentage, case isnull(IncludeInPlan,0) when 0 then ''N'' else ''Y'' end IncludeInPlan, RecipeDescription,
														case isnull(p.InventoryUOMId , p.uomId)  when p.InventoryUOMId then (select uom from UOM where uomid = p.InventoryUOMid) else
														(select uom from UOM where uomId = p.UOMid) end
														InventoryUOM , PreparationTime' + isnull(',' + @cols, '') + N' 
										            from product p left outer join products ps on p.ManualProductId = ps.product_id 
										                    left outer join (SELECT segmentcategoryid ' + isnull(',' + @cols, '') + N'
															                 from 
															                 (
															                    select segmentcategoryid, d.SegmentName, valuechar
															                    from Segment_Definition d join segmentdataview s on d.SegmentDefinitionId = s.SegmentDefinitionId 
															                    where d.ApplicableEntity = ''PRODUCT''
														                     ) x
														                    ' + @pivot + N'
													                        )s on p.segmentcategoryid = s.segmentcategoryid
											left outer join vendor v on p.defaultvendorid = v.VendorId left outer join tax t on t.tax_id = ps.tax_id, category c, uom u" +
                                            " where p.categoryid = c.categoryid " + condition +
                                                "and p.uomid = u.uomid)v '" +
                                            "exec sp_executesql @query ";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                log.LogMethodExit(dt);
            }
            return dt;
        }

        //Save excel with product data when Download button is clicked
        void SaveExcelWithData()
        {
            log.LogMethodEntry();
            string[] ExcelColumns = GetExcelColumns();//Added 6-May-2016
            DataTable dt = new DataTable();
            dt = GetProductData();
            if (dt.Rows.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("No products to be downloaded"), utilities.MessageUtils.getMessage("Product download"));
                log.LogMethodExit("dt.Rows.Count <= 0");
                return;
            }

            using (Form frm = new Form())
            {
                DataGridView dgv = new DataGridView();
                frm.Controls.Add(dgv);
                dgv.Visible = true;
                dgv.DataSource = dt;

                Excel.Application objApp;
                objApp = new Excel.Application();
                if (objApp == null)
                {
                    log.LogMethodExit("objApp == null");
                    return;
                }

                Excel._Workbook objBook;

                Excel.Workbooks objBooks;
                Excel.Sheets objSheets;
                Excel._Worksheet objSheet;
                Excel.Range range;

                try
                {
                    // Instantiate Excel and start a new workbook.
                    objApp = new Excel.Application();
                    objBooks = objApp.Workbooks;
                    objBook = objBooks.Add(Missing.Value);
                    objBook.Title = "Upload Products";
                    objSheets = objBook.Worksheets;
                    objSheet = (Excel._Worksheet)objSheets.get_Item(1);
                    objSheet.Name = "Products";

                    //Get the range where the starting cell has the address
                    //m_sStartingCell and its dimensions are m_iNumRows x m_iNumCols.
                    range = objSheet.get_Range("A1", Missing.Value);

                    range = range.get_Resize(1, ExcelColumns.Length);

                    string[,] raSet = new string[1, ExcelColumns.Length];
                    for (long iCol = 0; iCol < ExcelColumns.Length; iCol++)
                    {
                        //Put the row and column address in the cell.
                        raSet[0, iCol] = ExcelColumns[iCol];
                    }

                    int worksheetRow = 2;
                    //   string colStart, colEnd;
                    int WORKSHEETSTARTCOL = 1;//, colNum = 1;
                    worksheetRow = 2;
                    //Copy data rows
                    var fileLoad = new object[dgv.Rows.Count + 1, ExcelColumns.Length];
                    for (int rowCount = 0; rowCount < dgv.Rows.Count; rowCount++)
                    {
                        //colStart = Char.ConvertFromUtf32('A' + WORKSHEETSTARTCOL - 1) + worksheetRow.ToString();
                        //colEnd = Char.ConvertFromUtf32('A' + colNum - 1) + worksheetRow.ToString();
                        //Excel.Range xlRowRange = objSheet.get_Range(colStart, colEnd);

                        int worksheetcol = WORKSHEETSTARTCOL;
                        for (int colCount = 0; colCount < dgv.Columns.Count; colCount++)
                        {
                            Excel.Range xlRange = (Excel.Range)objSheet.Cells[worksheetRow, worksheetcol];
                            try
                            {
                                if (colCount == 0 && dgv.Rows[rowCount].Cells[colCount].Value != null && dgv.Rows[rowCount].Cells[colCount].Value != DBNull.Value)
                                {
                                    //xlRange.Value2 = "\t" + dgv.Rows[rowCount].Cells[colCount].Value.ToString();
                                    fileLoad[rowCount, colCount] = "\t" + dgv.Rows[rowCount].Cells[colCount].Value.ToString();
                                }
                                else
                                {
                                    //xlRange.Value2 = dgv.Rows[rowCount].Cells[colCount].FormattedValue;
                                    fileLoad[rowCount, colCount] = dgv.Rows[rowCount].Cells[colCount].FormattedValue;
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                continue;
                            }
                            worksheetcol += 1;
                        }
                        worksheetRow += 1;
                    }

                    var startCell = (Excel.Range)objSheet.Cells[2, 1];
                    var endCell = (Excel.Range)objSheet.Cells[dgv.Rows.Count + 1, ExcelColumns.Length];
                    var writeRange = objSheet.Range[startCell, endCell];
                    writeRange.Value2 = fileLoad;

                    //Set the range value to the array.
                    range.set_Value(Missing.Value, raSet);
                    range.Font.Bold = true;
                    range.Columns.AutoFit();

                    //Return control of Excel to the user.
                    objApp.Visible = true;
                    objApp.UserControl = true;
                }
                catch (Exception theException)
                {
                    String errorMessage;
                    errorMessage = utilities.MessageUtils.getMessage("Error") + " : ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);

                    MessageBox.Show(errorMessage, utilities.MessageUtils.getMessage("Error"));
                    log.Error(theException);
                }
            }
            log.LogMethodExit();
        }

        void SaveExcelWithDataNew()
        {
            log.LogMethodEntry();
            string[] ExcelColumns = GetExcelColumns();//Added 6-May-2016
            DataTable dt = new DataTable();
            dt = GetProductData();
            if (dt.Rows.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("No products to be downloaded"), utilities.MessageUtils.getMessage("Product download"));
                log.LogMethodExit("dt.Rows.Count <= 0");
                return;
            }

            using (Form frm = new Form())
            {
                DataGridView dgv = new DataGridView();
                frm.Controls.Add(dgv);
                dgv.Visible = true;
                dgv.DataSource = dt;

                dgv.RowHeadersVisible = false;
                dgv.SelectAll();
                DataObject dataObj = dgv.GetClipboardContent();
                if (dataObj != null)
                {
                    Clipboard.SetDataObject(dataObj);
                }

                Excel.Application objApp;
                objApp = new Excel.Application();
                if (objApp == null)
                {
                    log.LogMethodExit("objApp == null");
                    return;
                }

                Excel._Workbook objBook;

                Excel.Workbooks objBooks;
                Excel.Sheets objSheets;
                Excel._Worksheet objSheet;
                Excel.Range range;

                try
                {
                    // Instantiate Excel and start a new workbook.
                    objApp = new Excel.Application();
                    objBooks = objApp.Workbooks;
                    objBook = objBooks.Add(Missing.Value);
                    objBook.Title = "Upload Products";
                    objSheets = objBook.Worksheets;
                    objSheet = (Excel._Worksheet)objSheets.get_Item(1);
                    objSheet.Name = "Products";

                    //Get the range where the starting cell has the address
                    //m_sStartingCell and its dimensions are m_iNumRows x m_iNumCols.
                    range = objSheet.get_Range("A1", Missing.Value);

                    range = range.get_Resize(1, ExcelColumns.Length);

                    string[,] raSet = new string[1, ExcelColumns.Length];
                    for (long iCol = 0; iCol < ExcelColumns.Length; iCol++)
                    {
                        //Put the row and column address in the cell.
                        raSet[0, iCol] = ExcelColumns[iCol];
                    }



                    //Set the range value to the array.
                    range.set_Value(Missing.Value, raSet);
                    range.Font.Bold = true;
                    range.Columns.AutoFit();
                    Excel.Range CR = (Excel.Range)objSheet.Cells[2, 1];
                    CR.Select();
                    objSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);


                    //Return control of Excel to the user.
                    objApp.Visible = true;
                    objApp.UserControl = true;
                    Clipboard.Clear();
                }
                catch (Exception theException)
                {
                    String errorMessage;
                    errorMessage = utilities.MessageUtils.getMessage("Error") + " : ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);

                    MessageBox.Show(errorMessage, utilities.MessageUtils.getMessage("Error"));
                    log.Error(theException);
                }
            }
            log.LogMethodExit();
        }

        //Start update 06-May-2016
        //Added method to update segment values
        bool SaveSegmentData(DataRow drUploadRow, DataTable dtSegments, int ProductId, ref string message)
        {
            log.LogMethodEntry(drUploadRow, dtSegments, ProductId, message);
            int SegmentCategoryID = -1;//Added 06-May-2016
            #region
            try
            {
                using (SqlCommand cmd = utilities.getCommand())
                {
                    cmd.CommandText = "select SegmentCategoryID from Product where Productid = " + ProductId.ToString();
                    object o = cmd.ExecuteScalar();

                    if (o != DBNull.Value)
                    {
                        SegmentCategoryID = Convert.ToInt32(o);
                    }

                    using (SqlCommand cmdDMLSql = utilities.getCommand())
                    {
                        SqlTransaction SQLTrx = cmdDMLSql.Connection.BeginTransaction();
                        cmdDMLSql.Transaction = SQLTrx;
                        SqlCommand cmdSql = utilities.getCommand();
                        cmdSql.Transaction = SQLTrx;
                        if (SegmentCategoryID == -1)
                        {
                            cmdSql.CommandText = @"insert into Segment_Categorization 
                                                            (
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastupdatedDate,
                                                            Guid,
                                                            site_id) 
                                                        values 
                                                                (                                                        
                                                                @createdBy,
                                                                getDate(),
                                                                @lastUpdatedBy,
                                                                getDate(),
                                                                NewId(),
                                                                @site_id)SELECT CAST(scope_identity() AS int)";
                            cmdSql.Parameters.AddWithValue("@createdBy", utilities.ParafaitEnv.Username);
                            cmdSql.Parameters.AddWithValue("@lastUpdatedBy", utilities.ParafaitEnv.Username);
                            if (utilities.ParafaitEnv.IsCorporate)
                                cmdSql.Parameters.AddWithValue("@site_id", utilities.ParafaitEnv.SiteId);
                            else
                                cmdSql.Parameters.AddWithValue("@site_id", DBNull.Value);

                            SegmentCategoryID = Convert.ToInt32(cmdSql.ExecuteScalar());
                            cmdSql.CommandText = "update product set segmentcategoryid = " + SegmentCategoryID.ToString() + " where productid = " + ProductId.ToString();
                            cmdSql.ExecuteNonQuery();
                        }

                        try
                        {
                            foreach (DataRow dr in dtSegments.Rows)
                            {
                                string dataType = Convert.ToString(dr["datasourcetype"]);
                                string SegmentName = dr["name"].ToString();
                                int SegmentDefinitionID = Convert.ToInt32(dr["segmentdefinitionid"]);

                                if (dr["segmentdefinitionid"] != null)
                                {
                                    if (dataType.Equals("TEXT") || dataType.Equals("DATE"))
                                    {
                                        string val = Convert.ToString(drUploadRow[SegmentName]);
                                        cmdDMLSql.Parameters.Clear();
                                        if (val == "")
                                        {
                                            //Start Update 20-May-2016
                                            //Added condition to see if segment value is mandatory
                                            if (Convert.ToString(dr["IsMandatory"]) == "Y")
                                            {
                                                message = SegmentName + utilities.MessageUtils.getMessage(" is a mandatory field.");
                                                SQLTrx.Rollback();
                                                log.LogVariableState("message", message);
                                                log.LogMethodExit(false);
                                                return false;
                                            }
                                            else
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", DBNull.Value);
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", DBNull.Value);
                                            }
                                            //End Update 20-May-2016
                                        }
                                        else
                                        {
                                            if (dataType == "TEXT")
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", val);
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", DBNull.Value);
                                            }
                                            else if (dataType == "DATE")
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", DBNull.Value);
                                                try
                                                {
                                                    cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", Convert.ToDateTime(val));
                                                }
                                                catch
                                                {
                                                    message = utilities.MessageUtils.getMessage(15, " :") + val;
                                                    SQLTrx.Rollback();
                                                    log.LogVariableState("message", message);
                                                    log.LogMethodExit(false);
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", val);
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", DBNull.Value);
                                            }
                                        }
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentCategoryId", SegmentCategoryID);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentDefinitionId", SegmentDefinitionID);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentStaticValueId", DBNull.Value);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentDynamicValueId", DBNull.Value);
                                        cmdDMLSql.Parameters.AddWithValue("@LastUpdatedBy", utilities.ParafaitEnv.Username);

                                        cmdDMLSql.CommandText = "update segment_categorization_values set SegmentStaticValueId = @SegmentStaticValueId, SegmentValueText = @SegmentValueText, SegmentValueDate = @SegmentValueDate, " +
                                                        " LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = getdate() " +
                                                        "where SegmentCategoryId = @SegmentCategoryId and SegmentDefinitionID = @SegmentDefinitionId";

                                        if (cmdDMLSql.ExecuteNonQuery() == 0)
                                        {
                                            cmdDMLSql.CommandText = "insert into segment_categorization_values (SegmentCategoryId, SegmentDefinitionID, SegmentStaticValueId, SegmentValueText, SegmentValueDate, site_id, CreatedBy, CreationDate, LastUpdatedBy, LastUpdatedDate, IsActive) " +
                                                        "values (@SegmentCategoryId, @SegmentDefinitionId, @SegmentStaticValueId, @SegmentValueText, @SegmentValueDate, @site_id, @CreatedBy, getdate(), @LastUpdatedBy, getdate(), 'Y')";
                                            cmdDMLSql.Parameters.AddWithValue("@CreatedBy", utilities.ParafaitEnv.Username);
                                            //cmdDMLSql.Parameters.AddWithValue("@LastUpdatedBy", utilities.ParafaitEnv.Username);
                                            if (utilities.ParafaitEnv.IsCorporate)
                                                cmdDMLSql.Parameters.AddWithValue("@site_id", utilities.ParafaitEnv.SiteId);
                                            else
                                                cmdDMLSql.Parameters.AddWithValue("@site_id", DBNull.Value);

                                            cmdDMLSql.ExecuteNonQuery();
                                        }
                                    }
                                    else if (dataType.Contains("LIST"))
                                    {
                                        if (dataType.Equals("STATIC LIST"))
                                        {
                                            cmdDMLSql.Parameters.Clear();
                                            cmdDMLSql.Parameters.AddWithValue("@SegmentDynamicValueId", DBNull.Value);
                                            if (string.IsNullOrEmpty(drUploadRow[SegmentName].ToString()))
                                            {
                                                //Start Update 20-May-2016
                                                //Added condition to see if segment value is mandatory
                                                if (Convert.ToString(dr["IsMandatory"]) == "Y")
                                                {
                                                    message = SegmentName + utilities.MessageUtils.getMessage(" is a mandatory field.");
                                                    SQLTrx.Rollback();
                                                    log.LogVariableState("message", message);
                                                    log.LogMethodExit(false);
                                                    return false;
                                                }
                                                else
                                                {
                                                    cmdDMLSql.Parameters.AddWithValue("@SegmentStaticValueId", DBNull.Value);
                                                }
                                                //End Update 20-May-2016
                                            }
                                            else
                                            {
                                                SqlCommand cmdValues = utilities.getCommand();
                                                cmdValues.Transaction = SQLTrx;
                                                cmdValues.CommandText = @"select segmentdefinitionsourceValueid
                                                                from segment_definition_source_mapping m, segment_definition_source_values s
                                                                where m.SegmentDefinitionSourceId = s.SegmentDefinitionSourceId
	                                                                and segmentdefinitionid = @segmentdefinitionid
	                                                                and listvalue = @ListValue";
                                                cmdValues.Parameters.AddWithValue("@SegmentDefinitionId", SegmentDefinitionID);
                                                cmdValues.Parameters.AddWithValue("@ListValue", drUploadRow[SegmentName]);
                                                o = cmdValues.ExecuteScalar();
                                                if (o != null && o != DBNull.Value)
                                                {
                                                    cmdDMLSql.Parameters.AddWithValue("@SegmentStaticValueId", Convert.ToInt32(o));
                                                }
                                                else
                                                {
                                                    message = utilities.MessageUtils.getMessage("Invalid value for Segment ") + SegmentName;
                                                    SQLTrx.Rollback();
                                                    log.LogVariableState("message", message);
                                                    log.LogMethodExit(false);
                                                    return false;
                                                }
                                            }
                                        }
                                        else if (dataType.Equals("DYNAMIC LIST"))
                                        {
                                            cmdDMLSql.Parameters.Clear();
                                            cmdDMLSql.Parameters.AddWithValue("@SegmentStaticValueId", DBNull.Value);
                                            if (string.IsNullOrEmpty(drUploadRow[SegmentName].ToString()))
                                            {
                                                cmdDMLSql.Parameters.AddWithValue("@SegmentDynamicValueId", DBNull.Value);
                                            }
                                            else
                                            {
                                                SqlCommand cmdValues = utilities.getCommand();
                                                cmdValues.Transaction = SQLTrx;
                                                cmdValues.CommandText = @"select DBQuery, m.DataSourceEntity, m.DataSourceColumn
                                                                from segment_definition_source_mapping m, segment_definition_source_values s
                                                                where m.SegmentDefinitionSourceId = s.SegmentDefinitionSourceId
	                                                                and segmentdefinitionid = @SegmentDefinitionId
	                                                                and m.isactive = 'Y'
	                                                                and s.isactive = 'Y'";
                                                cmdValues.Parameters.Clear();
                                                cmdValues.Parameters.AddWithValue("@SegmentDefinitionId", SegmentDefinitionID);
                                                DataTable dt = new DataTable();
                                                SqlDataAdapter da = new SqlDataAdapter(cmdValues);
                                                da.Fill(dt);
                                                if (dt.Rows.Count > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(dt.Rows[0]["DBQuery"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["DataSourceEntity"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["DataSourceColumn"].ToString()))
                                                    {
                                                        string query = "select " + dt.Rows[0]["DataSourceColumn"].ToString() +
                                                                       " from " + dt.Rows[0]["DataSourceEntity"].ToString() + " " + dt.Rows[0]["DataSourceEntity"].ToString().Substring(0, 1) +
                                                                       " where " + dt.Rows[0]["DBQuery"].ToString() + " and " + dt.Rows[0]["DataSourceColumn"].ToString() + " = '" + drUploadRow[SegmentName] + "'";
                                                        cmdValues.CommandText = query;
                                                        o = cmdValues.ExecuteScalar();
                                                        if (o != null && o != DBNull.Value)
                                                            cmdDMLSql.Parameters.AddWithValue("@SegmentDynamicValueId", Convert.ToString(o));
                                                        else
                                                        {
                                                            message = utilities.MessageUtils.getMessage("Invalid value for Segment ") + SegmentName;
                                                            SQLTrx.Rollback();
                                                            log.LogVariableState("message", message);
                                                            log.LogMethodExit(false);
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        message = utilities.MessageUtils.getMessage("Invalid value for Segment ") + SegmentName;
                                                        SQLTrx.Rollback();
                                                        log.LogVariableState("message", message);
                                                        log.LogMethodExit(false);
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    message = utilities.MessageUtils.getMessage("Invalid value for Segment ") + SegmentName;
                                                    log.LogVariableState("message", message);
                                                    SQLTrx.Rollback();
                                                    log.LogMethodExit(false);
                                                    return false;
                                                }
                                            }
                                        }
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentCategoryId", SegmentCategoryID);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentDefinitionId", SegmentDefinitionID);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentValueText", DBNull.Value);
                                        cmdDMLSql.Parameters.AddWithValue("@SegmentValueDate", DBNull.Value);

                                        cmdDMLSql.CommandText = "update segment_categorization_values set SegmentStaticValueId = @SegmentStaticValueId, SegmentDynamicValueId = @SegmentDynamicValueId, SegmentValueText = @SegmentValueText, SegmentValueDate = @SegmentValueDate " +
                                                        "where SegmentCategoryId = @SegmentCategoryId and SegmentDefinitionId = @SegmentDefinitionId";

                                        if (cmdDMLSql.ExecuteNonQuery() == 0)
                                        {
                                            cmdDMLSql.CommandText = "insert into segment_categorization_values (SegmentDefinitionId, SegmentCategoryId, SegmentStaticValueId, SegmentDynamicValueId, SegmentValueText, SegmentValueDate, site_id, CreatedBy, CreationDate, LastUpdatedBy, LastUpdatedDate, IsActive) " +
                                                        "values (@SegmentDefinitionId, @SegmentCategoryId, @SegmentStaticValueId, @SegmentDynamicValueId, @SegmentValueText, @SegmentValueDate, @site_id, @CreatedBy, getdate(), @LastUpdatedBy, getdate(), 'Y')";
                                            cmdDMLSql.Parameters.AddWithValue("@CreatedBy", utilities.ParafaitEnv.Username);
                                            cmdDMLSql.Parameters.AddWithValue("@LastUpdatedBy", utilities.ParafaitEnv.Username);
                                            if (utilities.ParafaitEnv.IsCorporate)
                                                cmdDMLSql.Parameters.AddWithValue("@site_id", utilities.ParafaitEnv.SiteId);
                                            else
                                                cmdDMLSql.Parameters.AddWithValue("@site_id", DBNull.Value);
                                            cmdDMLSql.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            SQLTrx.Commit();
                            log.LogMethodExit(true);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            message = ex.Message;
                            SQLTrx.Rollback();
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = "Error: " + ex.Message;
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit();
            #endregion
        }//End update 06-May-2016

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to get category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns>categoryId</returns>
        int GetCategoryId(string categoryName)
        {
            log.LogMethodEntry(categoryName);
            int categoryId = -1;
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categorySearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            categorySearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.NAME, categoryName.ToString()));
            categorySearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler();
            List<CategoryDTO> categoryListDTO = new List<CategoryDTO>();
            CategoryDTO categoryDTO = new CategoryDTO();
            categoryListDTO = categoryDataHandler.GetCategoryList(categorySearchParams);
            if (categoryListDTO != null && categoryListDTO.Any())
            {
                categoryId = categoryListDTO[0].CategoryId;
            }
            log.LogMethodExit(categoryId);
            return categoryId;
        }

        /// <summary>
        /// Method to get vendor id
        /// </summary>
        /// <param name="vendorName"></param>
        /// <returns>vendorId</returns>
        int GetVendorId(string vendorName)
        {
            log.LogMethodEntry(vendorName);
            int vendorId = -1;
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            vendorSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.NAME, vendorName.ToString()));
            vendorSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, machineUserContext.GetSiteId().ToString()));
            VendorDataHandler vendorDataHandler = new VendorDataHandler();
            List<VendorDTO> vendorListDTO = new List<VendorDTO>();
            VendorDTO vendorDTO = new VendorDTO();
            vendorListDTO = vendorDataHandler.GetVendorList(vendorSearchParams);
            if (vendorListDTO != null && vendorListDTO.Any())
            {
                vendorId = vendorListDTO[0].VendorId;
            }
            log.LogMethodExit(vendorId);
            return vendorId;
        }

        /// <summary>
        /// Method to get UOM id
        /// </summary>
        /// <param name="uomName"></param>
        /// <returns>uomId</returns>
        int GetUOMId(string uomName)
        {
            log.LogMethodEntry(uomName);
            int uomId = -1;
            UOMContainer uomcontainer = new UOMContainer(machineUserContext);
            List<UOMDTO> uomListDTO = UOMContainer.uomDTOList;
            if (uomListDTO != null & uomListDTO.Any())
            {
                uomId = uomListDTO.Find(x => x.UOM == uomName).UOMId;
            }
            log.LogMethodExit(uomId);
            return uomId;
        }
        /// <summary>
        /// Method to get Sales tax id
        /// </summary>
        /// <param name="salesTax"></param>
        /// <returns>taxId</returns>
        private int GetSalesTaxID(string salesTax)
        {
            log.LogMethodEntry(salesTax);
            int salesTaxId = -1;
            if (salesTaxListDTO == null || !salesTaxListDTO.Any(x => x.TaxName == (salesTax)))
            {
                throw new Exception(utilities.MessageUtils.getMessage(2661, salesTax));
            }
            List<TaxDTO> matchTaxDTOs = salesTaxListDTO.FindAll(x => x.TaxName == (salesTax));
            if (matchTaxDTOs.Count > 1)
            {
                throw new Exception(utilities.MessageUtils.getMessage(2662, salesTax));
            }
            salesTaxId = matchTaxDTOs[0].TaxId;
            log.LogMethodExit(salesTaxId);
            return salesTaxId;
        }
    }
}
