using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.logging;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Product
{
    public partial class frmlistBarCodes : Form
    {
        int pProductID;
        string pCode;
        string pDescription;
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmlistBarCodes(int ProductID, string code, string Description, Utilities _Utilities)
        {
            InitializeComponent();
            pProductID = ProductID;
            pCode = code;
            utilities = _Utilities;
            pDescription = Description;
            utilities.setupDataGridProperties(ref barcode_dgv);
            utilities.setLanguage(this);
            barcode_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void PopulateAddBarcodeGrid()
        {
            log.Debug("Starts-PopulateProductBarcodeGrid() method.");
            ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(utilities.ExecutionContext);
            List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> SearchParameter = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
            SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.PRODUCT_ID, pProductID.ToString()));
            List<ProductBarcodeDTO> productBarcodeListOnDisplay = productBarcodeListBL.GetProductBarcodeDTOList(SearchParameter);

            if (productBarcodeListOnDisplay != null)
            {
                SortableBindingList<ProductBarcodeDTO> productBarcodeDTOSortList = new SortableBindingList<ProductBarcodeDTO>(productBarcodeListOnDisplay);
                productBarcodeBindingSource.DataSource = productBarcodeDTOSortList;
            }
            else
                productBarcodeBindingSource.DataSource = new SortableBindingList<ProductBarcodeListBL>();


            barcode_dgv.DataSource = productBarcodeBindingSource;
            lblHeading.Text = "Barcode for " + pCode + " - " + pDescription;
            log.Debug("Ends-PopulateProductBarcodeGrid() method.");
        }

        private void frmlistBarCodes_Load(object sender, EventArgs e)
        {
            lblHeading.Text = "Barcode for " + pCode + " - " + pDescription;
            PopulateAddBarcodeGrid();
        }
    }
}
