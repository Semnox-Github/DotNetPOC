/********************************************************************************************
 * Project Name - Reservation
 * Description  - Reservation Cancellation Charge form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      26-Mar-2019   Guru S A                Created for Booking phase 2 enhancement changes 
 *2.110.0      07-Dec-2020  Mushahid Faizan         Handled executionContext for Tax.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmReservationCancellationCharge : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private ReservationBL bookingBL;
        private ExecutionContext executionContext;
        public ReservationBL GetReservationBL { get { return bookingBL; } }
        public frmReservationCancellationCharge(ReservationBL reservationBL)
        {
            log.LogMethodEntry();
            this.utilities = POSStatic.Utilities;
            this.executionContext = ExecutionContext.GetExecutionContext();
            this.bookingBL = reservationBL;
            InitializeComponent();
            utilities.setLanguage();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmReservationCancellationCharge_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //this.DialogResult = DialogResult.Cancel;
            LoadCancellationDetails();
            utilities.setLanguage(this);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadCancellationDetails()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (bookingBL.BookingTransaction != null)
            {
                txtTransactionAmount.Text = bookingBL.BookingTransaction.Transaction_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                txtTotalPaidAmount.Text = bookingBL.BookingTransaction.TotalPaidAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                txtTotalPaidAmount.Tag = bookingBL.BookingTransaction.TotalPaidAmount;
                LoadPnlCancellationChargeDetails();
                RefreshCancellationCharge();
            }
            else
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2115));// "Sorry, Booking Transaction details are missing"
                this.Close();
            }
            log.LogMethodExit();
        }

        private void LoadPnlCancellationChargeDetails()
        {
            log.LogMethodEntry();
            List<ProductsDTO> cancellationProducts = bookingBL.GetCancellationChargeProducts(null);
            if (cancellationProducts != null && cancellationProducts.Count > 0)
            {
                int productCounter = 0;
                int lableLocationX = lblCancellationCharge1.Location.X;
                int lableLocationY = lblCancellationCharge1.Location.Y;
                int txtBoxLocationX = txtCancellationCharge1.Location.X;
                int txtBoxLocationY = txtCancellationCharge1.Location.Y;
                lblCancellationCharge1.Visible = false;
                txtCancellationCharge1.Visible = false;
                txtCancellationCharge1.ReadOnly = true;
                int panelHeight = pnlCancellationChargeDetails.Height;
                foreach (ProductsDTO cancellationProduct in cancellationProducts)
                {
                    Label lblCancellationCharge = new Label();
                    lblCancellationCharge.Name = "lblCancellationCharge0" + productCounter.ToString();
                    lblCancellationCharge.Width = lblCancellationCharge1.Width;
                    lblCancellationCharge.Height = lblCancellationCharge1.Height;
                    lblCancellationCharge.TextAlign = lblCancellationCharge1.TextAlign;
                    lblCancellationCharge.Text = MessageContainerList.GetMessage(executionContext, cancellationProduct.ProductName)+":";
                    lblCancellationCharge.Location = new Point(lableLocationX, lableLocationY + (productCounter * 40));
                    TextBox txtCancellationCharge = new TextBox();
                    txtCancellationCharge.Name = "txtCancellationCharge0" + productCounter.ToString();
                    decimal productPrice = cancellationProduct.Price;
                    if (cancellationProduct.TaxInclusivePrice == "N")
                    {
                        if (cancellationProduct.Tax_id > -1)
                        {
                            Tax taxBL = new Tax(executionContext,cancellationProduct.Tax_id);
                            TaxDTO taxDTO = taxBL.GetTaxDTO();
                            if (taxDTO != null)
                            { productPrice = cancellationProduct.Price + (decimal)(cancellationProduct.Price * (decimal)(taxDTO.TaxPercentage / 100));
                            }
                        }
                    }
                    txtCancellationCharge.Text = productPrice.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    txtCancellationCharge.Location = new Point(txtBoxLocationX, txtBoxLocationY + (productCounter * 40));
                    txtCancellationCharge.ReadOnly = true;
                    txtCancellationCharge.Validating += new System.ComponentModel.CancelEventHandler(this.txtCancellationCharge_Validating);
                    txtCancellationCharge.Tag = cancellationProduct;
                    pnlCancellationChargeDetails.Controls.Add(lblCancellationCharge);
                    pnlCancellationChargeDetails.Controls.Add(txtCancellationCharge);
                    productCounter++;
                }
                pnlCancellationChargeDetails.Refresh();
                panel3.Refresh();
                panel4.Refresh();
                flowLayoutPanel1.Height += 30;
                flowLayoutPanel1.Refresh();
            }
            log.LogMethodExit();
        }

        private double GetTotalCancellationCharge()
        {
            log.LogMethodEntry();
            double cancellationCharge = 0;
            foreach (Control panelControl in pnlCancellationChargeDetails.Controls)
            {
                if (panelControl.Name.StartsWith("txtCancellationCharge0"))
                {
                    TextBox txtCancellationCharge = (TextBox)panelControl;
                    double validatedValue = 0;
                    string amountValue = txtCancellationCharge.Text.Substring(txtCancellationCharge.Text.IndexOf(txtCancellationCharge.Text.First(Char.IsDigit)));
                    if (double.TryParse(amountValue, out validatedValue))
                    {
                        cancellationCharge += validatedValue;
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2116));// "Please enter valid amount"
                        txtCancellationCharge.Focus();
                        this.ActiveControl = txtCancellationCharge;
                        break;
                    }
                }
            }
            log.LogMethodExit(cancellationCharge);
            return cancellationCharge;
        }

        private void btnOverrideCancellationCharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                int managerId = utilities.ParafaitEnv.ManagerId;
                if (Authenticate.Manager(ref managerId))
                {
                    //Add audit info
                    EnabbleCancellationChargeEntries();
                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, "Manager approval required"));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void EnabbleCancellationChargeEntries()
        {
            log.LogMethodEntry();
            foreach (Control panelControl in pnlCancellationChargeDetails.Controls)
            {
                if (panelControl.Name.StartsWith("txtCancellationCharge0"))
                {
                    TextBox txtCancellationCharge = (TextBox)panelControl;
                    txtCancellationCharge.ReadOnly = false;
                    //int charge;
                    //int.TryParse(txtCancellationCharge.Text, out charge);
                    double validatedValue = 0;
                    string amountValue = txtCancellationCharge.Text.Substring(txtCancellationCharge.Text.IndexOf(txtCancellationCharge.Text.First(Char.IsDigit)));
                    double.TryParse(amountValue, out validatedValue);
                    txtCancellationCharge.Text = validatedValue.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                    txtCancellationCharge.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void txtCancellationCharge_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                TextBox selectedTxtBox = (TextBox)sender;
                ValidateCancellationCharageEntry(selectedTxtBox);
                RefreshCancellationCharge();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void RefreshCancellationCharge()
        {
            double cancellationCharge = GetTotalCancellationCharge();
            txtTotalCancellationCharge.Text = cancellationCharge.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            txtTotalCancellationCharge.Tag = cancellationCharge;
            txtBalanceAmount.Text = (bookingBL.BookingTransaction.TotalPaidAmount - cancellationCharge).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            txtBalanceAmount.Tag = (bookingBL.BookingTransaction.TotalPaidAmount - cancellationCharge);
        }

        private void ValidateCancellationCharageEntry(TextBox selectedTxtBox)
        {
            log.LogMethodEntry();
            double numberValidation = 0;
            if (double.TryParse(selectedTxtBox.Text, out numberValidation) == false)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2116));// "Please enter valid amount"
                selectedTxtBox.Text = ((ProductsDTO)selectedTxtBox.Tag).Price.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                selectedTxtBox.Focus();
            }
            else
            {
                selectedTxtBox.Text = Math.Round(numberValidation, 4).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
            }
            log.LogVariableState("numberValidation", numberValidation);
            log.LogMethodExit();

        }

        private void btnApplyCharges_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                double advancePaid = 0;
                double cancellationCharges = 0;
                double balanceAmount = 0;
                double.TryParse(txtTotalPaidAmount.Tag.ToString(), out advancePaid);
                double.TryParse(txtTotalCancellationCharge.Tag.ToString(), out cancellationCharges);
                double.TryParse(txtBalanceAmount.Tag.ToString(), out balanceAmount);
                if (balanceAmount < 0)
                {
                    //"Balance amount cannot be a negative value. Either receive payment from customer for the remaining dues or use override feature to adjust the cancellation charges"
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2117));
                }
                else if (advancePaid != (cancellationCharges + balanceAmount))
                {
                    // "Cancellation charge amount and Balance amount should match Advance paid"
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2118));
                }
                else
                {
                    List<KeyValuePair<int, double>> productList = new List<KeyValuePair<int, double>>();
                    foreach (Control panelControl in pnlCancellationChargeDetails.Controls)
                    {
                        if (panelControl.Name.StartsWith("txtCancellationCharge0"))
                        {
                            TextBox txtCancellationCharge = (TextBox)panelControl;
                            ProductsDTO cancellationProduct = (ProductsDTO)txtCancellationCharge.Tag;
                            // double cancellationAmount = 0;
                            double validatedValue = 0;
                            string amountValue = txtCancellationCharge.Text.Substring(txtCancellationCharge.Text.IndexOf(txtCancellationCharge.Text.First(Char.IsDigit)));
                            if (cancellationProduct != null && double.TryParse(amountValue, out validatedValue))
                            {
                                KeyValuePair<int, double> productInfo = new KeyValuePair<int, double>(cancellationProduct.ProductId, validatedValue);
                                productList.Add(productInfo);

                            }
                        }
                    }
                    int bookingId = -1;
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        bookingId = bookingBL.GetReservationDTO.BookingId;
                        bookingBL.AddCancellationProduct(productList, ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DEFAULT_PAY_MODE").Equals("1"));
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (ValidationException ex)
                    {
                        POSUtils.SetLastActivityDateTime();
                        log.Error(ex);
                        POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages());
                        bookingBL = new ReservationBL(executionContext, utilities, bookingId);
                    }
                    catch (Exception ex)
                    {
                        POSUtils.SetLastActivityDateTime();
                        log.Error(ex);
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
                        bookingBL = new ReservationBL(executionContext, utilities, bookingId);
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            //this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void btnOverrideCancellationCharge_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.R_black_pressed;
            log.LogMethodExit();
        }

        private void btnOverrideCancellationCharge_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.R_black_normal;
            log.LogMethodExit();
        }
    }
}
