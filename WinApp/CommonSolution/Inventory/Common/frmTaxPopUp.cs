/********************************************************************************************
 * Project Name -TaxPopUp Form
 * Description  -Form
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 **********************************************************************************************
 *2.60        11-Apr-2019       Girish Kundar      Created
                                                   To display Tax Details as a Popup window
                                                   in Purchase Order and Receipt Forms.
*2.70.2         13-Aug-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class frmTaxPopUp : Form
    {
        /// <summary>
        /// Parafait Utilities
        /// </summary>
        public Utilities Utilities;
        // string objectName;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineDTO;
        public List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList;
        public string description;

        public frmTaxPopUp(string description, Utilities _Utilites, List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList)
        {
            log.LogMethodEntry(description, _Utilites, purchaseOrderTaxLineDTOList);
            InitializeComponent();
            initializeVariables();
            Utilities = _Utilites;
            this.description = description;
            this.purchaseOrderTaxLineDTOList = purchaseOrderTaxLineDTOList;
            Utilities.setLanguage(this);
            Utilities.setupDataGridProperties(ref dgv_purchaseTax);
            if (Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(Utilities.ParafaitEnv.Username);
            CommonUIDisplay.setupVisuals(this);
            CommonUIDisplay.Utilities = _Utilites;
            CommonUIDisplay.ParafaitEnv = _Utilites.ParafaitEnv;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.PurchaseOrderTaxGridShow();
            log.LogMethodExit();
        }
        public frmTaxPopUp(string description, Utilities _Utilites, List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineDTO)
        {
            log.LogMethodEntry(description, _Utilites, purchaseOrderTaxLineDTOList);
            InitializeComponent();
            initializeVariables();
            Utilities = _Utilites;
            this.description = description;
            this.purchaseOrderReceiveTaxLineDTO = purchaseOrderReceiveTaxLineDTO;
            Utilities.setLanguage(this);
            Utilities.setupDataGridProperties(ref dgv_purchaseTax);
            if (Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(Utilities.ParafaitEnv.Username);
            CommonUIDisplay.setupVisuals(this);
            CommonUIDisplay.Utilities = _Utilites;
            CommonUIDisplay.ParafaitEnv = _Utilites.ParafaitEnv;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.PurchaseOrderReceiveTaxGridShow();
            log.LogMethodExit();
        }
        public void PurchaseOrderReceiveTaxGridShow()
        {   // To show the Tax Grid from Order Receive screen.
            log.LogMethodEntry();
            this.Text = this.Text + "-" + "Tax Details";
            if (purchaseOrderReceiveTaxLineDTO != null && purchaseOrderReceiveTaxLineDTO.Count > 0)
            {
                foreach (PurchaseOrderReceiveTaxLineDTO dto in purchaseOrderReceiveTaxLineDTO)
                {
                    dgv_purchaseTax.Rows.Add(description, dto.PurchaseTaxName, dto.TaxStructureName + " " + dto.TaxPercentage + "%", dto.TaxAmount);

                }

            }
            log.LogMethodExit();
        }
        public void PurchaseOrderTaxGridShow()
        {     // To show the Tax Grid from Order Screen.
            log.LogMethodEntry();
            if (this.description.Equals("Total Tax"))
            {
                PurchaseOrderTotalTaxGridShow();
            }
            else
            {
                this.Text = this.Text + "-" + "Tax Details";
                if (purchaseOrderTaxLineDTOList != null && purchaseOrderTaxLineDTOList.Count > 0)
                {
                    foreach (PurchaseOrderTaxLineDTO dto in purchaseOrderTaxLineDTOList)
                    {
                        dgv_purchaseTax.Rows.Add(description, dto.PurchaseTaxName, dto.TaxStructureName + " " + dto.TaxPercentage + "%", dto.TaxAmount);

                    }

                }
            }
            log.LogMethodExit();
        }

        public void PurchaseOrderTotalTaxGridShow()
        {     // To show the Tax Grid from Order Screen.
            log.LogMethodEntry();
            DataGridView dataGrid = new DataGridView();
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setupDataGridProperties(ref dataGrid);
            dataGrid.Width = 450;
            dataGrid.AutoResizeColumns();
            dataGrid.BackgroundColor = Color.White;
            dataGrid.DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewAmountCellStyle();
            if (purchaseOrderTaxLineDTOList != null && purchaseOrderTaxLineDTOList.Count > 0)
            {
                this.Text = this.Text + "-" + "Consolidated Tax for Current PurchaseOrder";
                List<PurchaseOrderTaxLineDTO> newPurchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
                Dictionary<int, int> processedTax = new Dictionary<int, int>();
                dgv_purchaseTax.Columns.RemoveAt(0);
                var grouped = purchaseOrderTaxLineDTOList
                        .GroupBy(s => new
                        {
                            s.PurchaseTaxName,
                            s.TaxStructureName

                        })
                       .Select(group => new { TaxName = group.First().PurchaseTaxName, group.Last().TaxStructureName, TaxAmount = group.Sum((x => x.TaxAmount)) })
                       .Distinct();

                dataGrid.DataSource = grouped.ToList();
                this.Controls.Remove(dgv_purchaseTax);
                this.Controls.Add(dataGrid);

            }
            log.LogMethodExit();
        }


        private void initializeVariables()
        {
            log.LogMethodEntry();
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setupDataGridProperties(ref dgv_purchaseTax);
            this.dgv_purchaseTax.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_purchaseTax.Columns["TaxAmount"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewAmountCellStyle();
            log.LogMethodExit();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Dispose();
            log.LogMethodExit();
        }
    }
}
