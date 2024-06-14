/********************************************************************************************
 * Project Name - Parafait POS
 * Description  - POS application
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.70        4-Jul -2019      Girish Kundar  Modified :Changed the Currency BL resulting changes in the constructors of Currency class.
*                                                      Get CurrencyDTO List by passing Currency Id is moved form CurrenctList to Currency class.
*2.130.11    13-Oct-2022      Vignesh Bhat   Tender Amount UI enhancement to support AMOUNT_FORMAT configuration                                                
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.Currency;
using Semnox.Core.Utilities;
namespace Parafait_POS
{
    public partial class frmTender : Form
    {
        //Added on 3-oct-2016 for Multicurrency implementation
        int usedCurrencyId = -1;
        public int CurrencyID = -1;
        //end 

        public double TenderedAmount = 0;
        public double MultiCurrencyAmount = 0; //Added on 18-Oct-2016 for fixing multicurrency issue
        NumberPad numPad;
        double _Amount;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        private readonly ExecutionContext executionContext;
        private string amountFormat;
        private string currencySymbol;
        private const int maximumBtnPaymentModeWidth = 169;
        public frmTender(double Amount)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmTender(" + Amount + ")");//Modified for Adding logger feature on 08-Mar-2016
            
            InitializeComponent();
            MinimizeBox = MaximizeBox = false;
            _Amount = Amount;
            amountFormat = POSStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT;
            lblChange.Text = (0).ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            currencySymbol = POSStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL;
            string[] denoms = POSStatic.Utilities.getParafaitDefaults("PAYMENT_DENOMINATIONS").Split('|');
            int maximumDenominationLength = GetMaximumDenominationLength(denoms);

            foreach (string denomination in denoms)
            {
                if (string.IsNullOrEmpty(denomination.Trim()))
                    continue;
                string formattedDenomination = NumberPad.ConvertToFormattedAmount(denomination, amountFormat);
                Button btnPaymentMode = new Button();
                btnPaymentMode.FlatStyle = FlatStyle.Flat;
                btnPaymentMode.FlatAppearance.BorderSize = 0;
                btnPaymentMode.FlatAppearance.CheckedBackColor = Color.Transparent;
                btnPaymentMode.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btnPaymentMode.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btnPaymentMode.BackgroundImageLayout = ImageLayout.Stretch;
                btnPaymentMode.BackColor = Color.Transparent;
                btnPaymentMode.Tag = 0;
                btnPaymentMode.Name = "B" + denomination.Trim();
                btnPaymentMode.Text = currencySymbol + " " + formattedDenomination;
                btnPaymentMode.Click += btnPaymentMode_Click;
                btnPaymentMode.Size = maximumDenominationLength > 5 ? new Size(maximumBtnPaymentModeWidth, btnSample.Height) : new Size(btnSample.Width, btnSample.Height);
                btnPaymentMode.BackgroundImage = btnSample.BackgroundImage;
                btnPaymentMode.Font = btnSample.Font;
                btnPaymentMode.ForeColor = btnSample.ForeColor;

                btnPaymentMode.MouseDown += btnPaymentMode_MouseDown;
                btnPaymentMode.MouseUp += btnPaymentMode_MouseUp;

                flpTenders.Controls.Add(btnPaymentMode);
            }

            numPad = new NumberPad(POSStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT, POSStatic.Utilities.ParafaitEnv.RoundingPrecision);
            numPad.handleaction(Amount.ToString());
            numPad.NewEntry = true;

            Panel NumberPadVarPanel = numPad.NumPadPanel();
            NumberPadVarPanel.Location = new System.Drawing.Point(2, 2);
            this.Controls.Add(NumberPadVarPanel);
            numPad.setReceiveAction = EventnumPadOKReceived;
            numPad.setKeyAction = EventnumPadKeyPressReceived;

            this.KeyPreview = true;

            this.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
            this.FormClosing += new FormClosingEventHandler(FormNumPad_FormClosing);

            //Added on 29-Sep-2016 for Multicurrency Implementaion
            PoupulateCurrencyGrid();
            flpCurrency.Enabled = true;

            log.Debug("Ends-frmTender(" + Amount + ")");//Modified for Adding logger feature on 08-Mar-2016
        }

        void PoupulateCurrencyGrid()
        {
            log.LogMethodEntry();
            try
            {
                CurrencyList currencyList = new CurrencyList(executionContext);
                List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>> searchParams = new List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>>();
                searchParams.Add(new KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>(CurrencyDTO.SearchByCurrencyParameters.IS_ACTIVE, "1"));
                List<CurrencyDTO> currencyListOnDisplay = currencyList.GetAllCurrency(searchParams);
                if (currencyListOnDisplay != null)
                {
                    gbCurrency.Location = new Point(279, 3);
                    gbCurrency.Height = 175;
                    gbDenomination.Location = new Point(279, 179);
                    gbDenomination.Height = 239;

                    for (int i = 0; i < currencyListOnDisplay.Count; i++)
                    {
                        if (string.IsNullOrEmpty(currencyListOnDisplay[i].CurrencyCode))
                            continue;

                        Button btnCurrency = new Button();
                        btnCurrency.FlatStyle = FlatStyle.Flat;
                        btnCurrency.FlatAppearance.BorderSize = 0;
                        btnCurrency.FlatAppearance.CheckedBackColor = Color.Transparent;
                        btnCurrency.FlatAppearance.MouseDownBackColor = Color.Transparent;
                        btnCurrency.FlatAppearance.MouseOverBackColor = Color.Transparent;
                        btnCurrency.BackgroundImageLayout = ImageLayout.Zoom;
                        btnCurrency.BackColor = Color.Transparent;
                        btnCurrency.Tag = currencyListOnDisplay[i].CurrencyId;
                        btnCurrency.Name = "Btn" + currencyListOnDisplay[i].CurrencyCode.Trim();
                        btnCurrency.Text = currencyListOnDisplay[i].CurrencySymbol + " " + currencyListOnDisplay[i].CurrencyCode.Trim();
                        btnCurrency.Click += btnCurrency_Click;
                        btnCurrency.Size = btnSample.Size;
                        btnCurrency.BackgroundImage = btnCurencySample.BackgroundImage;
                        btnCurrency.Font = btnCurencySample.Font;
                        btnCurrency.ForeColor = btnCurencySample.ForeColor;

                        btnCurrency.MouseDown += btnCurrency_MouseDown;
                        btnCurrency.MouseUp += btnCurrency_MouseUp;

                        flpCurrency.Controls.Add(btnCurrency);
                    }
                }
                else
                {
                    gbCurrency.Visible = false;
                    gbDenomination.Location = new Point(279, 12);
                    gbDenomination.Height = 400;
                }
            }
            catch(Exception ex)
            {
                log.Error("Error in PoupulateCurrencyGrid() - " + ex.Message);
            }
            log.LogMethodExit();
        }

        void btnCurrency_MouseUp(object sender, MouseEventArgs e)
        {
            //(sender as Button).BackgroundImage = Properties.Resources.ManualProduct;
        }

        void btnCurrency_MouseDown(object sender, MouseEventArgs e)
        {
            // (sender as Button).BackgroundImage = Properties.Resources.ManualProduct;
        }

        void btnPaymentMode_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.DiplayGroupButton;
        }

        void btnPaymentMode_MouseDown(object sender, MouseEventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.ProductPressed;
        }

        void FormNumPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("Starts-FormNumPad_FormClosing()");//Modified for Adding logger feature on 08-Mar-2016
            if (this.DialogResult == DialogResult.Cancel)
                TenderedAmount = -1;

            log.Debug("Ends-FormNumPad_FormClosing()");//Modified for Adding logger feature on 08-Mar-2016
        }

        void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
                numPad.GetKey(e.KeyChar);
        }

        private void EventnumPadOKReceived()
        {
            log.Debug("Starts-EventnumPadOKReceived()");//Modified for Adding logger feature on 08-Mar-2016
            TenderedAmount = numPad.ReturnNumber;
            MultiCurrencyAmount = numPad.ReturnNumber; //Added on 18-Oct-2016 for fixing multicurrency issue

            //Added on 29-Sep-2016 for Multicurrency Implementaion
            if(usedCurrencyId != -1)
            {
                CurrencyID = usedCurrencyId;
            }//end

            this.DialogResult = DialogResult.OK;
            this.Close();
            log.Debug("Ends-EventnumPadOKReceived()");//Modified for Adding logger feature on 08-Mar-2016
        }

        void EventnumPadKeyPressReceived()
        {
            log.LogMethodEntry();//Modified for Adding logger feature on 08-Mar-2016
            TenderedAmount = numPad.ReturnNumber;
       
            //added by suneetha on - 29-Sep-2016 for multi currency implementation
            flpCurrency.Enabled = false;
            if (flpTenders.Tag != null)
            {
                Currency currency = new Currency(executionContext, Convert.ToInt32(flpTenders.Tag)); //// Added on 2- jul-2019 : Modified the BL based on new structure. 
                CurrencyDTO currencyDisplay = currency.CurrencyDTO;
                if (currencyDisplay != null)
                {
                    double saleRate = Convert.ToDouble(currencyDisplay.SellRate);
                    double returnAMt = 0;
                    if (saleRate != 0)
                    {
                        double currencyConversionAmt = _Amount * saleRate;
                        returnAMt = TenderedAmount - currencyConversionAmt;
                    }
                    lblChange.Text = (returnAMt / saleRate).ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                }
            }
            else
            {
                lblChange.Text = (TenderedAmount - _Amount).ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            }

            if (TenderedAmount == 0)
            {
                foreach (Control payMode in flpTenders.Controls)
                {
                    payMode.Text = currencySymbol + " " + NumberPad.ConvertToFormattedAmount(payMode.Name.Substring(1), amountFormat);
                    payMode.Tag = 0;
                }
            }
            log.LogMethodExit();//Modified for Adding logger feature on 08-Mar-2016
        }

        //added on - 29-Sep-2016 for multi currency implementation
        void btnCurrency_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Button currencyTpeBtn = (sender as Button);

            foreach (Control d in flpCurrency.Controls)
            {
                d.BackgroundImage = Properties.Resources.GameTime;
            }

            double trxNetAmt = _Amount;
            if (currencyTpeBtn.Tag != null)
            {
                int btnCurrencyTypeId = Convert.ToInt32(currencyTpeBtn.Tag);
                Currency currency = new Currency(executionContext, btnCurrencyTypeId); // Added on 2- jul-2019 : Modified the Currency BL based on new structure. 
                CurrencyDTO currencyDisplay = currency.CurrencyDTO;
                currencyTpeBtn.BackgroundImage = Properties.Resources.ManualProduct;
                currencySymbol = currencyDisplay.CurrencySymbol;
                if (currencyDisplay != null)
                {
                    usedCurrencyId = currencyDisplay.CurrencyId;
                    flpTenders.Tag = currencyDisplay.CurrencyId;
                    double saleRate = Convert.ToDouble(currencyDisplay.SellRate);
                    double currencyConversionAmt = 0;

                    if (saleRate != 0)
                    {
                        currencyConversionAmt = trxNetAmt * saleRate;
                        string strAmount = currencyConversionAmt.ToString();  //.ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT);

                        numPad.NewEntry = true;
                        foreach (char c in strAmount)
                            numPad.GetKey(c);
                        numPad.NewEntry = true;

                        if(flpTenders.Controls != null)
                        {
                            foreach(Control contr in flpTenders.Controls)
                            {
                                if(contr.Tag != null)
                                {
                                    string[] textCount = contr.Text.Split(' ');
                                    if (textCount.Length > 1)
                                    {
                                        string denomination = textCount[1];
                                        contr.Text = currencyDisplay.CurrencySymbol + " " + denomination.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    gbDenomination.Tag = null;
                }
            }
            log.Debug("Ends-btnCurrency_Click()");//Modified for Adding logger feature on 08-Mar-2016
        } //end

        void btnPaymentMode_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPaymentMode_Click()");//Modified for Adding logger feature on 08-Mar-2016
            Button payMode = (sender as Button);
            
            string formattedPayModeName = NumberPad.ConvertToFormattedAmount(payMode.Name.Substring(1), amountFormat);
            double amount = Convert.ToDouble(payMode.Name.Substring(1));
            payMode.Tag = Convert.ToInt32(payMode.Tag) + 1;
            payMode.Text = currencySymbol + " " + formattedPayModeName + " x " + payMode.Tag.ToString();

            TenderedAmount += amount;
            string strAmount = TenderedAmount.ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT);
            numPad.NewEntry = true;
            foreach (char c in strAmount)
                numPad.GetKey(c);
            numPad.NewEntry = true;

            //added on - 29-Sep-2016 for multicurrency implementation
            flpCurrency.Enabled = false;
            if (flpTenders.Tag != null)
            {
                string[] btnText = payMode.Text.Split(' ');

                int btnCurrencyTypeId = Convert.ToInt32(flpTenders.Tag);
                Currency currency = new Currency(executionContext, btnCurrencyTypeId); // Added on 2- jul-2019 : Modified the Currency BL based on new structure. 
                CurrencyDTO currencyDisplay = currency.CurrencyDTO;
                if(currencyDisplay != null)
                {
                    payMode.Text = currencyDisplay.CurrencySymbol + " " + formattedPayModeName + " x " + payMode.Tag.ToString();
                    double saleRate = Convert.ToDouble(currencyDisplay.SellRate);
                    double returnAMt = 0 ;
                    if (saleRate != 0)
                    {
                        double currencyConversionAmt = _Amount * saleRate;
                        returnAMt = TenderedAmount - currencyConversionAmt;
                    }
                    lblChange.Text = (returnAMt / saleRate).ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                }
            }
            else//end modification
            {
                payMode.Text = currencySymbol + " " + formattedPayModeName + " x " + payMode.Tag.ToString();
                lblChange.Text = (TenderedAmount - _Amount).ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            }

            log.Debug("Ends-btnPaymentMode_Click()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.customer_button_pressed;
            usedCurrencyId = -1; // Added on 3-oct-2016 for Multicurrency Implementaion
        }

        private void btnCancel_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.customer_button_normal;
        } 
        private int GetMaximumDenominationLength(string[] denoms)
        {
            log.LogMethodEntry(denoms);
            int maximumDenominationLength = 0;
            foreach (string denomination in denoms)
            {
                if (denomination.Length > maximumDenominationLength)
                {
                    maximumDenominationLength = denomination.Length;
                }
            }
            log.LogMethodExit(maximumDenominationLength);
            return maximumDenominationLength;
        }
    }
}
