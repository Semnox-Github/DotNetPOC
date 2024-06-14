/********************************************************************************************
 * Project Name - Reservation
 * Description  - User control for Product trx line details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      26-Mar-2019   Guru S A                Created for Booking phase 2 enhancement changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_POS.Reservation
{
    public partial class usrCtrlProductTrxLineDetails : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ComboProductDTO comboProductDTO;
        private Transaction.TransactionLine productTrxLine;
        private ExecutionContext executionContext;
        private Utilities utilities;
        private int parentLineId;
        private int lineIndex;
        private ContextMenuStrip ctxBookingContextMenu;

        //private List<Transaction.TransactionLine> trxLines;
        public Transaction.TransactionLine ProductTrxLine { get { return productTrxLine; } }
        public ComboProductDTO ComboProductDTO { get { return comboProductDTO; } }
        public bool CbxSelectForEdit { get { return cbxSelectForEdit.Checked; } }

        internal delegate void RescheduleAttractionDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal RescheduleAttractionDelegate RescheduleAttraction;
        internal delegate void RescheduleAttractionGroupDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal RescheduleAttractionGroupDelegate RescheduleAttractionGroup;
        internal delegate void CancelProductLineDelegate(Transaction.TransactionLine trxLineItem);
        internal CancelProductLineDelegate CancelProductLine;
        internal delegate void ChangePriceDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal ChangePriceDelegate ChangePrice;
        internal delegate void ResetPriceDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal ResetPriceDelegate ResetPrice;

        public usrCtrlProductTrxLineDetails()
        {
            log.LogMethodEntry();
            this.utilities = POSStatic.Utilities;
            InitializeComponent();
            comboProductDTO = null;
            productTrxLine = null;
            parentLineId = -1;
            lineIndex = -1;
            this.executionContext = ExecutionContext.GetExecutionContext();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        public usrCtrlProductTrxLineDetails(ComboProductDTO comboProductDTO, Transaction.TransactionLine productTrxLine, int parentLineId, int lineIndex)
        {
            log.LogMethodEntry(comboProductDTO, productTrxLine, parentLineId, lineIndex);
            this.utilities = POSStatic.Utilities;
            InitializeComponent();
            this.comboProductDTO = comboProductDTO;
            this.productTrxLine = productTrxLine;
            this.parentLineId = parentLineId;
            this.lineIndex = lineIndex;
            //this.trxLines = trxLines;
            this.executionContext = ExecutionContext.GetExecutionContext();
            LoadTrxLineDetails();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadTrxLineDetails()
        {
            log.LogMethodEntry();
            if (comboProductDTO != null && productTrxLine != null)
            {
                btnParentLine.Text = (parentLineId != -1 ? parentLineId.ToString(utilities.ParafaitEnv.NUMBER_FORMAT) : string.Empty);
                btnParentLine.Tag = productTrxLine.ParentLine;
                btnProductName.Text = GetProductNamePadding() + productTrxLine.ProductName;
                string atbInfo = string.Empty;
                if (productTrxLine.LineAtb != null
                     && productTrxLine.LineAtb.AttractionBookingDTO != null)
                {
                    atbInfo = ": " + productTrxLine.LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                }
                if (string.IsNullOrEmpty(atbInfo) == false)
                {
                    btnProductName.Text = btnProductName.Text + atbInfo;
                }

                btnPrice.Text = (productTrxLine.Price).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                // txtProductType.Text = productTrxLine.ProductType;
                btnLineId.Text = (productTrxLine.DBLineId != 0 ? (productTrxLine.DBLineId - 1).ToString(utilities.ParafaitEnv.NUMBER_FORMAT)
                                                                 : (lineIndex != -1 ? lineIndex.ToString(utilities.ParafaitEnv.NUMBER_FORMAT) : string.Empty));
                btnQuantity.Text = (productTrxLine.quantity).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                //txtTransationProfile = productTrxLine.TrxProfileId.ToString()
                //txtDiscountId = productTrxLine.di;
                btnCardNumber.Text = productTrxLine.CardNumber;
                btnRemarks.Text = productTrxLine.Remarks;
                bool hasModifier = false;
                if (productTrxLine.DBLineId == 0 && productTrxLine.ProductTypeCode == ProductTypeValues.MANUAL)
                {
                    ProductModifiersList productModifiersListBL = new ProductModifiersList(executionContext);
                    List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.PRODUCT_ID, productTrxLine.ProductID.ToString()));
                    searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<ProductModifiersDTO> productModifiersDTOList = productModifiersListBL.GetAllProductModifiersList(searchParameters);
                    if (productModifiersDTOList != null && productModifiersDTOList.Count > 0)
                    {
                        hasModifier = true;
                    }
                }
                else
                {
                    hasModifier = productTrxLine.HasModifier;
                }
                if (hasModifier)
                {
                    cbxSelectForEdit.Enabled = true;
                    cbxSelectForEdit.Visible = true;
                }
                else
                {
                    cbxSelectForEdit.Enabled = false;
                    cbxSelectForEdit.Visible = false;
                }
                SetBookingContextMenu();
            }
            log.LogMethodExit();
        }

        private string GetProductNamePadding()
        {
            log.LogMethodEntry();
            string paddedString = string.Empty;
            int count = 0;
            count = GetParentCount(this.ProductTrxLine, count);
            while (count > 2)
            {
                paddedString += " ";
                count--;
            }
            log.LogMethodExit(paddedString);
            return paddedString;
        }

        private int GetParentCount(Transaction.TransactionLine trxLine, int count)
        {
            log.LogMethodEntry();
            if (trxLine.ParentLine != null)
            {
                count++;
                count = GetParentCount(trxLine.ParentLine, count);
            }
            log.LogMethodExit(count);
            return count;
        }


        private void SetBookingContextMenu()
        {
            log.LogMethodEntry();

            ctxBookingContextMenu = new System.Windows.Forms.ContextMenuStrip();
            System.Windows.Forms.ToolStripMenuItem rescheduleBooking = null;
            System.Windows.Forms.ToolStripMenuItem rescheduleBookingGroup = null;
            System.Windows.Forms.ToolStripMenuItem changePrice = null;
            System.Windows.Forms.ToolStripMenuItem resetPrice = null;
            if (productTrxLine != null && productTrxLine.DBLineId != 0 && productTrxLine.LineAtb != null && productTrxLine.LineAtb.AttractionBookingDTO != null)
            {
                rescheduleBooking = new System.Windows.Forms.ToolStripMenuItem();

                rescheduleBooking.Name = "Reschedule";
                rescheduleBooking.Size = new System.Drawing.Size(125, 22);
                rescheduleBooking.Text = MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Reschedule");

                rescheduleBookingGroup = new System.Windows.Forms.ToolStripMenuItem();

                rescheduleBookingGroup.Name = "RescheduleGroup";
                rescheduleBookingGroup.Size = new System.Drawing.Size(125, 22);
                rescheduleBookingGroup.Text = MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Reschedule Group");
            }
            if (productTrxLine != null && productTrxLine.AllowPriceOverride)
            {
                changePrice = new System.Windows.Forms.ToolStripMenuItem();

                changePrice.Name = "ChangePrice";
                changePrice.Size = new System.Drawing.Size(125, 22);
                changePrice.Text = MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Change Price");

                resetPrice = new System.Windows.Forms.ToolStripMenuItem();

                resetPrice.Name = "ResetPrice";
                resetPrice.Size = new System.Drawing.Size(125, 22);
                resetPrice.Text = MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Reset Price");

                if (productTrxLine.UserPrice)
                    resetPrice.Enabled = true;
                else
                    resetPrice.Enabled = false;
            }
            System.Windows.Forms.ToolStripMenuItem cancelLine = new System.Windows.Forms.ToolStripMenuItem();

            cancelLine.Name = "CancelLine";
            cancelLine.Size = new System.Drawing.Size(125, 22);
            cancelLine.Text = MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Cancel Line");

            if (rescheduleBooking != null)
            {
                ctxBookingContextMenu.Items.Add(rescheduleBooking);
            }
            if (rescheduleBookingGroup != null)
            {
                ctxBookingContextMenu.Items.Add(rescheduleBookingGroup);
            }
            if (cancelLine != null)
            {
                ctxBookingContextMenu.Items.Add(cancelLine);
            }
            if (changePrice != null)
            {
                ctxBookingContextMenu.Items.Add(changePrice);
            }
            if (resetPrice != null)
            {
                ctxBookingContextMenu.Items.Add(resetPrice);
            }
            ctxBookingContextMenu.Text = btnProductName.Text;
            ctxBookingContextMenu.Name = "ctxBookingContextMenu";
            ctxBookingContextMenu.Size = new System.Drawing.Size(126, 70);
            ctxBookingContextMenu.ItemClicked += ctxBookingContextMenu_ItemClicked;
            log.LogMethodExit();
        }


        void ctxBookingContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            if (this.productTrxLine != null)
            {
                if (e.ClickedItem.Name.Equals("Reschedule"))
                {
                    RescheduleAttraction(productTrxLine, lineIndex, this.comboProductDTO.ComboProductId);
                }
                else if (e.ClickedItem.Name.Equals("RescheduleGroup"))
                {
                    RescheduleAttractionGroup(productTrxLine, lineIndex, this.comboProductDTO.ComboProductId);
                }
                else if (e.ClickedItem.Name.Equals("ChangePrice"))
                {
                    ChangePrice(productTrxLine, lineIndex, this.comboProductDTO.ComboProductId);
                }
                else if (e.ClickedItem.Name.Equals("ResetPrice"))
                {
                    ResetPrice(productTrxLine, lineIndex, this.comboProductDTO.ComboProductId);
                }
                else
                {
                    if (IsPartOfCombo())
                    {
                        if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(this.utilities.ExecutionContext,
                                                                                    "This is part of combo setup. Cannot cancel individaual line. Do you want cancel the combo line and its child entries?"),
                                                                                    MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Cancel Line"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Transaction.TransactionLine comboParentLine = GetComboParentLine(productTrxLine);
                            CancelProductLine(comboParentLine);
                        }
                    }
                    else
                    {
                        CancelProductLine(productTrxLine);
                    }
                }
            }
            ctxBookingContextMenu.Hide();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }


        private void BtnProductName_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            ctxBookingContextMenu.Show(MousePosition.X, MousePosition.Y);
            log.LogMethodExit();
        }

        private bool IsPartOfCombo()
        {
            log.LogMethodEntry();
            bool isPartOfCombo = false;
            if (this.productTrxLine != null && this.productTrxLine.ComboproductId == -1 && this.productTrxLine.ParentLine != null)
            {
                isPartOfCombo = true;
            }
            log.LogMethodExit(isPartOfCombo);
            return isPartOfCombo;
        }

        private Transaction.TransactionLine GetComboParentLine(Transaction.TransactionLine productTrxLine)
        {
            log.LogMethodEntry();
            Transaction.TransactionLine comboParentLine = null;
            if (productTrxLine.ComboproductId > -1)
            {//this is parent line
                comboParentLine = productTrxLine;
            }
            else if (productTrxLine.ComboproductId == -1 && productTrxLine.ParentLine != null)
            {
                comboParentLine = GetComboParentLine(productTrxLine.ParentLine);
            }
            log.LogMethodExit(comboParentLine);
            return comboParentLine;
        }
    }
}
