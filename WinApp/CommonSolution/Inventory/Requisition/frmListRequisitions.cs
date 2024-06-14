/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - frm List Requisition
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       13-Aug-2019   Deeksha          Added logger methods.
 *2.110.0      21-Dec-2020   Abhishek         Modified for web API 
 *2.120.0      06-Jun-2021   Girish Kundar    Modified: Issue fix UOMId was not updated to master site during Issue process from Requisition 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmListRequisitions : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int selectedRequisitionId = -1;
        private RequisitionDTO requisitionDTO;
        private List<RequisitionLinesDTO> requisitionLineDTOList;
        private ExecutionContext executionContext;
        private string applicability;
        private int toSiteId;
        /// <summary>
        /// Selected requisition id
        /// </summary>        
        public int SelectedRequisitionId { get { return selectedRequisitionId; } }
        /// <summary>
        /// selected requisition 
        /// </summary>
        public RequisitionDTO SelectedRequisitionDTO { get { return requisitionDTO; } }
        /// <summary>
        /// selected requisition lines
        /// </summary>
        public List<RequisitionLinesDTO> SelectedRequisitionLineDTO { get { return requisitionLineDTOList; } }
        Utilities utilities;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Utilities"></param>
        /// <param name="applicability"></param>
        /// <param name="toSiteId"></param>
        public frmListRequisitions(Utilities _Utilities, string applicability, int toSiteId)
        {
            log.LogMethodEntry(_Utilities, applicability, toSiteId);
            InitializeComponent();
            this.utilities = _Utilities;
            this.applicability = applicability;
            this.toSiteId = toSiteId;
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmListRequisitions_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvRequisitions);
            populateGrid();
            log.LogMethodExit();
        }

        void populateGrid()
        {
            log.LogMethodEntry();
            DataTable dtRequisitions = new DataTable();
            RequisitionList requisitionList = new RequisitionList(executionContext);
            dtRequisitions = requisitionList.GetRequistionsToCreatePO(((utilities.ParafaitEnv.IsCorporate) ? utilities.ParafaitEnv.SiteId : -1), applicability, toSiteId);
            if (dtRequisitions.Rows.Count == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1099));
                this.Close();
            }
            else
                dgvRequisitions.DataSource = dtRequisitions;
            log.LogMethodExit();
        }

        private void btnSelectRequisitions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            selectValue();
            log.LogMethodExit();
        }

        private void dgvRequisitions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex >= 0 && dgvRequisitions.Columns[e.ColumnIndex].Name == "SelectRequisition")
                selectValue();
            log.LogMethodExit();
        }

        void selectValue()
        {
            log.LogMethodEntry();
            try
            {
                selectedRequisitionId = Convert.ToInt32(dgvRequisitions.SelectedRows[0].Cells["requisitionid"].Value);
                RequisitionBL requisitionBL = new RequisitionBL(executionContext, selectedRequisitionId);
                RequisitionLinesList requisitionLinesList = new RequisitionLinesList(executionContext);
                Product.ProductBL product;
                ProductDTO productDTO;
                ProductList productList;
                List<ProductDTO> productDTOList;
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchByProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                requisitionDTO = requisitionBL.GetRequisitionsDTO;
                if (requisitionDTO != null)
                {
                    RequisitionList requisitionList = new RequisitionList(executionContext);
                    List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> searchByRequisitionLinesParameters = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
                    searchByRequisitionLinesParameters.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, requisitionDTO.RequisitionId.ToString()));
                    requisitionLineDTOList = requisitionLinesList.GetAllRequisitionLines(searchByRequisitionLinesParameters);
                    if (requisitionDTO.SiteId != -1 && requisitionDTO.SiteId != utilities.ParafaitEnv.SiteId)
                    {
                        for (int i = 0; i < requisitionLineDTOList.Count; i++)
                        {
                            product = new Product.ProductBL(requisitionLineDTOList[i].ProductId);
                            productDTO = product.getProductDTO;
                            if (productDTO != null && productDTO.ProductId > -1)
                            {
                                productList = new ProductList();
                                searchByProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                                if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
                                {
                                    searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, productDTO.MasterEntityId.ToString()));
                                }
                                else
                                {
                                    searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, productDTO.MasterEntityId.ToString()));
                                    searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, requisitionDTO.ToSiteId.ToString()));
                                }

                                productDTOList = productList.GetAllProducts(searchByProductParameters);
                                if (productDTOList != null && productDTOList.Count > 0)
                                {
                                    requisitionLineDTOList[i].ProductId = productDTOList[0].ProductId;
                                }
                                else
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage("The product selected in the requisition is not published one."));
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage("The product does not exists."));
                                log.LogMethodExit();
                                return;
                            }
                            UOM uOM = new UOM(executionContext, requisitionLineDTOList[i].UOMId, false, false);
                            UOMDTO uOMDTO = uOM.getUOMDTO;
                            if (uOMDTO != null && uOMDTO.UOMId > -1)
                            {

                                List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchByUOMParameters = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
                                searchByUOMParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.IS_ACTIVE, "Y"));
                                if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
                                {
                                    searchByUOMParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.UOMID, uOMDTO.MasterEntityId.ToString()));
                                }
                                else
                                {
                                    searchByUOMParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.MASTER_ENTITY_ID, uOMDTO.MasterEntityId.ToString()));
                                    searchByUOMParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, requisitionDTO.ToSiteId.ToString()));
                                }
                                UOMList uOMList = new UOMList(executionContext);
                                List<UOMDTO> uomDTOList = uOMList.GetAllUOMDTOList(searchByUOMParameters, false, false);
                                if (uomDTOList != null && uomDTOList.Count > 0)
                                {
                                    requisitionLineDTOList[i].UOMId = uomDTOList[0].UOMId;
                                }
                                else
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage("The UOM selected in the requisition is not published one."));
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage("The UOM does not exists."));
                                log.LogMethodExit();
                                return;
                            }
                        }
                    }

                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Error while executing selectValue()" + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
