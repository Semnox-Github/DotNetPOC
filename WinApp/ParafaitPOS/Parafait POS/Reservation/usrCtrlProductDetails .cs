/********************************************************************************************
 * Project Name - Reservation
 * Description  - User control for Product details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      26-Mar-2019   Guru S A                Created for Booking phase 2 enhancement changes 
 *2.70.3      06-May-2020   Guru S A                handling qty and price refresh
 *2.80.0      09-Jun-2020   Jinto Thomas            Enable Active flag for Comboproduct data
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
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;

namespace Parafait_POS.Reservation
{
    public partial class usrCtrlProductDetails : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<Transaction.TransactionLine> trxLineList;
        private ComboProductDTO comboProductDTO;
        private ExecutionContext executionContext;
        private Utilities utilities;
        private int guestQty;
        private string bookingStatus;
        private bool userAction = true;
        internal delegate void CancelProductLineDelegate(Transaction.TransactionLine trxLineItem);
        internal CancelProductLineDelegate CancelProductLine;
        internal delegate List<Transaction.TransactionLine> RefreshTransactionLinesDelegate(int productId, int comboProductId);
        internal RefreshTransactionLinesDelegate RefreshTransactionLines;
        internal delegate List<Transaction.TransactionLine> AddProductToTransactionDelegate(int productId, double price, int quantity, string productType, int comboProductId);// PurchasedProducts purchasedProduct, List<AttractionBooking> atbList, List<ReservationDTO.SelectedCategoryProducts> categoryProducts);
        internal AddProductToTransactionDelegate AddProductToTransaction;
        internal delegate void GetTrxProfileDelegate(List<Transaction.TransactionLine> trxLineList = null);
        internal GetTrxProfileDelegate GetTrxProfile;
        internal delegate void SetDiscountsDelegate(int productId, int comboProductId);
        internal SetDiscountsDelegate SetDiscounts;
        internal delegate int GetTrxLineIndexDelegate(Transaction.TransactionLine trxLine);
        internal GetTrxLineIndexDelegate GetTrxLineIndex;
        internal delegate void EditModifiersDelegate(List<Transaction.TransactionLine> selectedTrxLineWithModifiers);
        internal EditModifiersDelegate EditModifiers;
        internal delegate void RescheduleAttractionDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal RescheduleAttractionDelegate RescheduleAttraction;
        internal delegate void RescheduleAttractionGroupDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal RescheduleAttractionGroupDelegate RescheduleAttractionGroup;
        internal delegate void ChangePriceDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal ChangePriceDelegate ChangePrice;
        internal delegate void ResetPriceDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal ResetPriceDelegate ResetPrice;
        public bool CbxSelectedProductChecked { get { return cbxSelectedProduct.Checked; } set { cbxSelectedProduct.Checked = value; } }

        public void HidePriceFieldAndAdjustUI()
        {
            log.LogMethodEntry();
            this.txtPrice.Visible = false;
            this.txtInclusiveQty.Visible = true;
            this.txtInclusiveQty.Location = new Point(this.txtPrice.Location.X, this.txtPrice.Location.Y);
            // int widthToAdjust = this.txtPrice.Width;
            //this.pcbDecreaseQty.Location = new Point(this.pcbDecreaseQty.Location.X - widthToAdjust, this.pcbDecreaseQty.Location.Y);
            //this.txtQty.Location = new Point(this.txtQty.Location.X - widthToAdjust, this.txtQty.Location.Y);
            //this.pcbIncreaseQty.Location = new Point(this.pcbIncreaseQty.Location.X - widthToAdjust, this.pcbIncreaseQty.Location.Y);
            //this.btnLineMenu.Location = new Point(this.btnLineMenu.Location.X - widthToAdjust, this.btnLineMenu.Location.Y);
            //this.pcbLineDetails.Location = new Point(this.pcbLineDetails.Location.X - widthToAdjust, this.pcbLineDetails.Location.Y);
            //this.pcbApplyDiscount.Location = new Point(this.pcbApplyDiscount.Location.X - widthToAdjust, this.pcbApplyDiscount.Location.Y);
            //this.pcbApplyTrxProfile.Location = new Point(this.pcbApplyTrxProfile.Location.X - widthToAdjust, this.pcbApplyTrxProfile.Location.Y);
            log.LogMethodExit();
        }

        public usrCtrlProductDetails()
        {
            log.LogMethodEntry();
            this.utilities = POSStatic.Utilities;
            InitializeComponent();
            this.comboProductDTO = null;
            this.trxLineList = null;
            this.guestQty = 0;
            LoadLineDetails();
            log.LogMethodExit();
        }
        public usrCtrlProductDetails(ComboProductDTO comboProductDTO, List<Transaction.TransactionLine> productTrxLines, int guestQty, string bookingStatus)
        {
            log.LogMethodEntry(executionContext, comboProductDTO, productTrxLines, guestQty);
            this.utilities = POSStatic.Utilities;
            SetUtilsAndContext();
            InitializeComponent();
            this.comboProductDTO = comboProductDTO;
            this.trxLineList = productTrxLines;
            this.guestQty = guestQty;
            this.bookingStatus = bookingStatus;
            LoadLineDetails();
            log.LogMethodExit();
        }
        private void SetUtilsAndContext()
        {
            log.LogMethodEntry();
            this.utilities = POSStatic.Utilities;
            this.executionContext = ExecutionContext.GetExecutionContext();
            if (utilities.ParafaitEnv.IsCorporate)//Starts:Modification on 02-jan-2017 fo customer feedback
            {
                this.executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                this.executionContext.SetSiteId(-1);
            }
            this.executionContext.SetMachineId(utilities.ParafaitEnv.POSMachineId);
            log.LogMethodExit();
        }

        private void SetContextMenuText()
        {
            log.LogMethodEntry();
            foreach (ToolStripItem tm in ctxLineMenu.Items)
            {
                tm.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, tm.Text);
            }
            log.LogMethodExit();
        }
        private void LoadLineDetails()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            if (comboProductDTO != null)
            {
                try
                {
                    userAction = false;
                    this.txtProductName.Text = comboProductDTO.ChildProductName;
                    int qty = 0;
                    Transaction.TransactionLine trxLine = null;
                    if (trxLineList != null && trxLineList.Count > 0)
                    {
                        cbxSelectedProduct.Checked = true;
                        trxLine = trxLineList.Find(tl => tl.ProductID == comboProductDTO.ChildProductId && tl.LineValid == true && tl.CancelledLine == false && tl.ComboproductId == comboProductDTO.ComboProductId);
                        qty = trxLineList.Count(tl => tl.ProductID == comboProductDTO.ChildProductId && tl.LineValid == true && tl.CancelledLine == false && tl.ComboproductId == comboProductDTO.ComboProductId);
                    }
                    decimal productPrice = 0;
                    int inclusiveQty = 0;
                    if (comboProductDTO.AdditionalProduct)
                    {
                        productPrice = GetProductPrice();
                    }
                    else
                    {
                        inclusiveQty = (comboProductDTO.Quantity == null ? 0 : (int)comboProductDTO.Quantity);
                    }
                    this.txtInclusiveQty.Text = (inclusiveQty > 0 ? inclusiveQty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT) : string.Empty);
                    this.txtPrice.Text = (trxLine != null ? trxLine.Price : (productPrice > 0 ? (double)productPrice : 0)).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                    //this.txtPrice.Text = (trxLine != null ? trxLine.LineAmount : (productPrice > 0 ? (double)productPrice : 0)).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                    txtQty.Tag = (qty > 0 ? qty : ((comboProductDTO.AdditionalProduct == false && comboProductDTO.Quantity != null && comboProductDTO.Quantity > 0) ? (int)comboProductDTO.Quantity : guestQty));
                    this.txtQty.Text = (qty > 0 ? qty : ((comboProductDTO.AdditionalProduct == false && comboProductDTO.Quantity != null && comboProductDTO.Quantity > 0) ? (int)comboProductDTO.Quantity : guestQty)).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);// utilities.ParafaitEnv.NUMBER_FORMAT); 
                }
                finally { userAction = true; }

            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void cbxSelectedProduct_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                if (userAction && comboProductDTO != null)
                {
                    if (cbxSelectedProduct.Checked)
                    {
                        try
                        {
                            int qty = GetQtyValue(this.txtQty.Text);// (string.IsNullOrEmpty(this.txtQty.Text) == false ? Convert.ToInt32(this.txtQty.Text) : 0);
                            if (qty > 0)
                            {
                                if (comboProductDTO != null && comboProductDTO.IsActive == false)
                                {
                                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Sorry cannot add or increase quantity. Product is no longer part of the package"));
                                }
                                else
                                {
                                    CreateProductLines(qty);
                                }
                            }
                            else
                            {
                                userAction = false;
                                cbxSelectedProduct.Checked = false;
                                userAction = true;
                            }
                        }
                        catch
                        {
                            if (this.trxLineList == null || this.trxLineList.Count == 0)
                            {
                                userAction = false;
                                cbxSelectedProduct.Checked = false;
                                txtQty.Text = (0).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                                txtQty.Tag = txtQty.Text;
                                userAction = true;
                            }
                        }
                    }
                    else
                    {
                        if (this.trxLineList != null)
                        {
                            //"This action will remove the selected product item from the booking. Do you want to proceed?"
                            if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2090), MessageContainerList.GetMessage(executionContext, "Remove"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                List<Transaction.TransactionLine> parentTrxLineList = trxLineList.Where(tl => tl.ProductID == comboProductDTO.ChildProductId && tl.LineValid == true
                                                                                                              && tl.CancelledLine == false && tl.ComboproductId == comboProductDTO.ComboProductId).ToList();
                                foreach (Transaction.TransactionLine trxLineItem in parentTrxLineList)
                                {
                                    CancelProductLine(trxLineItem);
                                }
                                userAction = false;
                                txtQty.Text = (0).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                                txtQty.Tag = txtQty.Text;
                                userAction = true;
                            }
                            else
                            {
                                userAction = false;
                                cbxSelectedProduct.Checked = true;
                                userAction = true;
                            }
                        }
                        else
                        {
                            userAction = false;
                            txtQty.Text = (0).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                            txtQty.Tag = txtQty.Text;
                            userAction = true;
                        }
                    }
                    this.trxLineList = RefreshTransactionLines(comboProductDTO.ChildProductId, comboProductDTO.ComboProductId);
                    SetPriceValue();
                }
                this.Cursor = Cursors.Default;
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.GetAllValidationErrorMessages()));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            POSUtils.SetLastActivityDateTime();
            SetFormFocus();
            log.LogMethodExit();
        }

        private void CreateProductLines(int qty)
        {
            log.LogMethodEntry(qty);
            this.Cursor = Cursors.WaitCursor;
            if (comboProductDTO != null)
            {
                POSUtils.SetLastActivityDateTime();
                double price = (string.IsNullOrEmpty(this.txtPrice.Text) == false ? Convert.ToDouble(this.txtPrice.Text) : 0.00);
                AddProductToTransaction(comboProductDTO.ChildProductId, price, qty, comboProductDTO.ChildProductType, comboProductDTO.ComboProductId);//, null, null, null);
            }
            SetFormFocus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void pcbIncreaseQty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            int qty = (string.IsNullOrEmpty(this.txtQty.Text) == false ? Convert.ToInt32(this.txtQty.Text) : 0);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (qty < 9999)
                {
                    txtQty.Text = (qty + 1).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                    UpdateProductQty();
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtQty.Text = qty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.GetAllValidationErrorMessages()));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtQty.Text = qty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            CallRefreshLines();
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            SetFormFocus();
            log.LogMethodExit();
        }

        private void pcbDecreaseQty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            int qty = GetQtyValue(this.txtQty.Text);// (string.IsNullOrEmpty(this.txtQty.Text) == false ? Convert.ToInt32(this.txtQty.Text) : 0);
            try
            {
                if (qty > 0)
                {
                    if (comboProductDTO != null && comboProductDTO.IsActive == false)
                    {
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Sorry cannot add or increase quantity. Product is no longer part of the package"));
                    }
                    else
                    {
                        txtQty.Text = (qty - 1).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                        UpdateProductQty();
                    }
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtQty.Text = qty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.GetAllValidationErrorMessages()));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtQty.Text = qty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            POSUtils.SetLastActivityDateTime();
            CallRefreshLines();
            this.Cursor = Cursors.Default;
            SetFormFocus();
            log.LogMethodExit();
        }

        private void txtQty_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (userAction)
                {
                    UpdateProductQty();
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.GetAllValidationErrorMessages()));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            CallRefreshLines();
            pcbIncreaseQty.Focus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }



        private void UpdateProductQty()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            if (txtQty.Text.ToString() != txtQty.Tag.ToString() && this.comboProductDTO != null)
            {

                int qtyDiff = GetQtyValue(this.txtQty.Text) - GetQtyValue(txtQty.Tag.ToString());//Convert.ToInt32((string.IsNullOrEmpty(txtQty.Text) == true ? "0" : txtQty.Text)) - Convert.ToInt32((string.IsNullOrEmpty(txtQty.Tag.ToString()) == true ? "0" : txtQty.Tag));
                if (qtyDiff > 0)
                {
                    if (comboProductDTO != null && comboProductDTO.IsActive == false)
                    {
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Sorry cannot add or increase quantity. Product is no longer part of the package"));
                    }
                    else
                    {
                        CreateProductLines(qtyDiff);
                    }
                }
                else if (qtyDiff < 0)
                {
                    if (trxLineList != null && trxLineList.Count > 0)
                    {
                        List<Transaction.TransactionLine> parentTrxLineList = trxLineList.Where(tl => tl.ProductID == comboProductDTO.ChildProductId && tl.LineValid == true && tl.CancelledLine == false && tl.ComboproductId == comboProductDTO.ComboProductId).ToList();
                        if (parentTrxLineList != null && parentTrxLineList.Count > 0)
                        {
                            for (int i = parentTrxLineList.Count - 1; i >= (parentTrxLineList.Count - Math.Abs(qtyDiff)); i--)
                            {
                                CancelProductLine(parentTrxLineList[i]);
                            }
                        }
                    }
                }
                txtQty.Tag = txtQty.Text;
            }
            SetFormFocus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void RefreshTrxLines()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            if (comboProductDTO != null)
            {
                POSUtils.SetLastActivityDateTime();
                this.trxLineList = RefreshTransactionLines(comboProductDTO.ChildProductId, this.comboProductDTO.ComboProductId);
                POSUtils.SetLastActivityDateTime();
                if (this.trxLineList != null && this.trxLineList.Count > 0)
                {
                    if (cbxSelectedProduct.Checked == false)
                    {
                        userAction = false;
                        cbxSelectedProduct.Checked = true;
                        SetQtyValue();
                        SetPriceValue();
                        userAction = true;
                    }
                    else
                    {
                        userAction = false;
                        SetQtyValue();
                        SetPriceValue();
                        userAction = true;
                    }
                }
                else if (cbxSelectedProduct.Checked && (this.trxLineList == null || this.trxLineList.Count == 0))
                {
                    userAction = false;
                    cbxSelectedProduct.Checked = false;
                    SetPriceValue();
                    userAction = true;
                }
            }
            SetFormFocus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void ShowLineDetails()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (comboProductDTO != null && this.trxLineList != null && this.trxLineList.Count > 0)
                {
                    using (frmProductTrxLineDetails frmProductTrxLineDetail = new frmProductTrxLineDetails(comboProductDTO, trxLineList, bookingStatus))
                    {
                        frmProductTrxLineDetail.GetTrxLineIndex += new frmProductTrxLineDetails.GetTrxLineIndexDelegate(GetTrxLineIndex);
                        frmProductTrxLineDetail.EditModifiers += new frmProductTrxLineDetails.EditModifiersDelegate(EditModifiers);
                        frmProductTrxLineDetail.RefreshTransactionLines += new frmProductTrxLineDetails.RefreshTransactionLinesDelegate(RefreshTransactionLines);
                        frmProductTrxLineDetail.RescheduleAttraction += new frmProductTrxLineDetails.RescheduleAttractionDelegate(RescheduleAttractionUsrCtl);
                        frmProductTrxLineDetail.RescheduleAttractionGroup += new frmProductTrxLineDetails.RescheduleAttractionGroupDelegate(RescheduleAttractionGroupUsrCtl);
                        frmProductTrxLineDetail.CancelProductLine += new frmProductTrxLineDetails.CancelProductLineDelegate(CancelProductLineUsrCtl);
                        frmProductTrxLineDetail.ChangePrice += new frmProductTrxLineDetails.ChangePriceDelegate(ChangePriceUsrCtl);
                        frmProductTrxLineDetail.ResetPrice += new frmProductTrxLineDetails.ResetPriceDelegate(ResetPriceUsrCtl);
                        frmProductTrxLineDetail.ShowDialog();
                    }
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.GetAllValidationErrorMessages()));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            CallRefreshLines();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void ApplyDiscount()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (this.comboProductDTO != null && this.trxLineList != null && this.trxLineList.Count > 0)
                {
                    SetDiscounts(this.comboProductDTO.ChildProductId, this.comboProductDTO.ComboProductId);
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.GetAllValidationErrorMessages()));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            CallRefreshLines();
            SetFormFocus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void ApplyTrxProfile()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (this.comboProductDTO != null && this.trxLineList != null && this.trxLineList.Count > 0)
                {
                    List<Transaction.TransactionLine> parentLines = this.trxLineList.Where(tl => tl.LineValid == true &&
                                                                                                 tl.ProductID == comboProductDTO.ChildProductId &&
                                                                                                 tl.ComboproductId == comboProductDTO.ComboProductId).ToList();
                    if (parentLines != null && parentLines.Count > 0)
                    {
                        GetTrxProfile(parentLines);
                    }
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.GetAllValidationErrorMessages()));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            CallRefreshLines();
            SetFormFocus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        public void UpdateUIElements(ReservationDTO.ReservationStatus reservationStatus)
        {
            log.LogMethodEntry(reservationStatus);
            SetDefaultUIElementStatus();
            switch (reservationStatus)
            {
                case ReservationDTO.ReservationStatus.NEW:
                case ReservationDTO.ReservationStatus.WIP:
                case ReservationDTO.ReservationStatus.BLOCKED:
                    SetUIElementInEditMode();
                    break;
                default:
                    break;
            }

            log.LogMethodExit();
        }

        private void SetDefaultUIElementStatus()
        {
            log.LogMethodEntry();
            cbxSelectedProduct.Enabled = txtProductName.Enabled = txtPrice.Enabled = pcbIncreaseQty.Enabled = pcbDecreaseQty.Enabled = false;
            txtQty.Enabled = false;
            btnLineMenu.Enabled = true;
            if (comboProductDTO != null && comboProductDTO.IsActive == false)
            {
                this.BackColor = Color.Red;
            }
            //pcbLineDetails.Enabled = pcbApplyDiscount.Enabled = pcbApplyTrxProfile.Enabled = true;
            SetContextMenuText();
            log.LogMethodExit();
        }

        private void SetUIElementInEditMode()
        {
            log.LogMethodEntry();
            cbxSelectedProduct.Enabled = pcbIncreaseQty.Enabled = pcbDecreaseQty.Enabled = true;
            txtQty.Enabled = true;
            if (comboProductDTO != null && comboProductDTO.IsActive == false)
            {
                this.BackColor = Color.Red;
                txtQty.Enabled = false;
            }
            log.LogMethodExit();
        }

        private void txtQty_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int qty;
            int.TryParse(txtQty.Text, out qty);
            try
            {
                POSUtils.SetLastActivityDateTime();
                int guestNo = (int)NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, 1834), txtQty.Text, utilities);
                if (guestNo >= 0 && guestNo < 10000)
                {
                    txtQty.Text = guestNo.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                    UpdateProductQty();
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtQty.Text = qty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, ex.GetAllValidationErrorMessages()));

            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtQty.Text = qty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            CallRefreshLines();
            pcbIncreaseQty.Focus();
            log.LogMethodExit();
        }
        private void CallRefreshLines()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                RefreshTrxLines();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            SetFormFocus();
            this.Cursor = Cursors.Default;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void btnRemarks_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.ctxLineMenu.Show(MousePosition.X, MousePosition.Y);
            SetFormFocus();
            log.LogMethodExit();
        }

        private void ctxLineMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (e.ClickedItem == remarksToolStripMenuItem)
                {
                    EnterRemarks();
                }
                else if (e.ClickedItem == detailsToolStripMenuItem)
                {
                    ShowLineDetails();
                }
                else if (e.ClickedItem == applyDiscountToolStripMenuItem)
                {
                    ApplyDiscount();
                }
                else if (e.ClickedItem == applyTransactionProfileToolStripMenuItem)
                {
                    ApplyTrxProfile();
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Error(ex);
            }
        }

        private void EnterRemarks()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            POSUtils.SetLastActivityDateTime();
            try
            {
                if (this.bookingStatus == ReservationDTO.ReservationStatus.NEW.ToString()
                    || this.bookingStatus == ReservationDTO.ReservationStatus.BLOCKED.ToString()
                    || this.bookingStatus == ReservationDTO.ReservationStatus.WIP.ToString())
                {
                    if (this.comboProductDTO != null && this.trxLineList != null && this.trxLineList.Count > 0)
                    {
                        string remarks = string.Empty;
                        List<Transaction.TransactionLine> parentLines = this.trxLineList.Where(tl => tl.LineValid == true &&
                                                                                                     tl.ProductID == comboProductDTO.ChildProductId &&
                                                                                                     tl.ComboproductId == comboProductDTO.ComboProductId).ToList();
                        if (parentLines != null && parentLines.Count > 0)
                        {
                            remarks = parentLines[0].Remarks;
                        }
                        string updatedRemarks = DisplayRemarks(remarks);
                        if (remarks != updatedRemarks)
                        {
                            if (parentLines != null && parentLines.Count > 0)
                            {
                                for (int i = 0; i < parentLines.Count; i++)
                                {
                                    parentLines[i].Remarks = updatedRemarks;
                                }
                                CallRefreshLines();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private string DisplayRemarks(string remarks)
        {
            log.LogMethodEntry(remarks);
            this.Cursor = Cursors.WaitCursor;
            POSUtils.SetLastActivityDateTime();
            using (GenericDataEntry trxLineRemarks = new GenericDataEntry(1))
            {
                trxLineRemarks.Text = MessageContainerList.GetMessage(executionContext, "Product Remarks");
                trxLineRemarks.DataEntryObjects[0].mandatory = false;
                trxLineRemarks.DataEntryObjects[0].label = MessageContainerList.GetMessage(executionContext, "Remarks");
                trxLineRemarks.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
                if (string.IsNullOrEmpty(remarks) == false)
                {
                    trxLineRemarks.DataEntryObjects[0].data = remarks;
                }
                if (trxLineRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    remarks = trxLineRemarks.DataEntryObjects[0].data;
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit(remarks);
            return remarks;
        }

        private void pcbDecreaseQty_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            PictureBox selectedPCBX = (PictureBox)sender;
            selectedPCBX.Image = Properties.Resources.R_Decrease_Qty_Hover;
            log.LogMethodExit();
        }

        private void pcbDecreaseQty_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            PictureBox selectedPCBX = (PictureBox)sender;
            selectedPCBX.Image = Properties.Resources.R_Decrease_Qty_Normal;
            log.LogMethodExit();
        }

        private void pcbIncreaseQty_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            PictureBox selectedPCBX = (PictureBox)sender;
            selectedPCBX.Image = Properties.Resources.R_Increase_Qty_Hover;
            log.LogMethodExit();
        }

        private void pcbIncreaseQty_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            PictureBox selectedPCBX = (PictureBox)sender;
            selectedPCBX.Image = Properties.Resources.R_Increase_Qty_Normal;
            log.LogMethodExit();
        }

        private void btnLineMenu_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedBtn = (Button)sender;
            selectedBtn.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private void btnLineMenu_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedBtn = (Button)sender;
            selectedBtn.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void SetFormFocus()
        {
            log.LogMethodEntry();
            try
            {
                this.ParentForm.BringToFront();
                //this.Focus();
            }
            catch { }
            log.LogMethodExit();
        }

        private void SetPriceValue()
        {
            log.LogMethodEntry();
            if (this.txtPrice.Visible)
            {
                decimal productPrice = 0;
                if (comboProductDTO.AdditionalProduct)
                {
                    productPrice = GetProductPrice();
                }
                Transaction.TransactionLine trxLine = null;
                if (trxLineList != null && trxLineList.Count > 0)
                {
                    trxLine = trxLineList.Find(tl => tl.ProductID == comboProductDTO.ChildProductId && tl.LineValid == true && tl.CancelledLine == false && tl.ComboproductId == comboProductDTO.ComboProductId);
                }
                this.txtPrice.Text = (trxLine != null ? trxLine.Price : (productPrice > 0 ? (double)productPrice : 0)).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);


            }
            log.LogMethodExit();
        }

        private void SetQtyValue()
        {
            log.LogMethodEntry();
            if (txtQty.Tag == null || string.IsNullOrEmpty(txtQty.Tag.ToString()) || GetQtyValue(txtQty.Tag.ToString()) != this.trxLineList.Count)// Convert.ToInt32(txtQty.Tag) != this.trxLineList.Count)
            {
                List<Transaction.TransactionLine> pkgLines = this.trxLineList.Where(tl => tl.ProductID == comboProductDTO.ChildProductId && tl.ComboproductId == this.comboProductDTO.ComboProductId && tl.LineValid).ToList();
                if (pkgLines != null && pkgLines.Count > 0)
                {
                    txtQty.Text = pkgLines.Count.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                    txtQty.Tag = txtQty.Text;
                }
                else
                {
                    txtQty.Text = 0.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                    txtQty.Tag = txtQty.Text;
                }
            }
            log.LogMethodExit();
        }
        private decimal GetProductPrice()
        {
            log.LogMethodEntry();
            decimal productPrice = 0;
            Products product = new Products(comboProductDTO.ChildProductId);
            if (product.GetProductsDTO != null)
            {
                productPrice = product.GetProductsDTO.Price;
                //if (product.GetProductsDTO.TaxInclusivePrice == "N" && product.GetProductsDTO.Tax_id > -1)
                //{
                //    Tax taxBL = new Tax(product.GetProductsDTO.Tax_id);
                //    TaxDTO taxDTO = taxBL.GetTaxDTO();
                //    if (taxDTO != null)
                //    {
                //        productPrice = product.GetProductsDTO.Price + (decimal)(product.GetProductsDTO.Price * (decimal)(taxDTO.TaxPercentage / 100));
                //    }
                //}
            }
            log.LogMethodExit(productPrice);
            return productPrice;
        }
        private void RescheduleAttractionGroupUsrCtl(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                RescheduleAttractionGroup(productTrxLine, lineIndex, comboProductId);
            }
            finally
            {
                CallRefreshLines();
            }
            log.LogMethodExit();
        }

        private void RescheduleAttractionUsrCtl(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                RescheduleAttraction(productTrxLine, lineIndex, comboProductId);
            }
            finally
            {
                CallRefreshLines();
            }
            log.LogMethodExit();
        }
        private void CancelProductLineUsrCtl(Transaction.TransactionLine productTrxLine)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                CancelProductLine(productTrxLine);
                CallRefreshLines();
            }
            finally
            {
                CallRefreshLines();
            }
            log.LogMethodExit();
        }

        private int GetQtyValue(string inputValue)
        {
            log.LogMethodEntry(inputValue);
            int quantityValue = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(inputValue) == false)
                {
                    quantityValue = GenericUtils.ConvertStringToInt(executionContext, inputValue);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2360));//Please enter valid quantity
            }
            log.LogMethodExit(quantityValue);
            return quantityValue;
        }
        private void ChangePriceUsrCtl(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                ChangePrice(productTrxLine, lineIndex, comboProductId);
            }
            finally
            {
                CallRefreshLines();
            }
            log.LogMethodExit();
        }
        private void ResetPriceUsrCtl(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                ResetPrice(productTrxLine, lineIndex, comboProductId);
            }
            finally
            {
                CallRefreshLines();
            }
            log.LogMethodExit();
        }
    }
}
