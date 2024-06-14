/********************************************************************************************
 * Project Name - Device.Turnstile UI
 * Description  - Class for  of frmUI      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public partial class frmUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ParafaitAlohaIntegrator _alohaObject;
        private double _chargeAmount;
        private Timer timer = new Timer();
      
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="alohaObject"></param>
        /// <param name="chargeAmount"></param>
        public frmUI(ParafaitAlohaIntegrator alohaObject, double chargeAmount)
        {
            log.LogMethodEntry(alohaObject,chargeAmount);
            InitializeComponent();
            _alohaObject = alohaObject;
            _chargeAmount = chargeAmount;

            timer.Interval = 1500;
            timer.Tick += timer_Tick;
            log.LogMethodExit();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            timer.Stop();
            SecureString CCTrack = convertToSecureString(txtCCTrack.Text.Trim());
            txtCCTrack.Clear();
            txtCCTrack.Text = "";
            if (CCTrack.Length > 0)
                Process(CCTrack);
            CCTrack.Clear();
            log.LogMethodExit();
        }

        private void frmUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            this.ActiveControl = txtCCTrack;
            txtCCTrack.Focus();
            log.LogMethodExit();
        }

        private void txtCCTrack_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            timer.Stop();
            timer.Start();
            log.LogMethodExit();
        }

        string convertToUNSecureString(SecureString secstrPassword)
        {
            log.LogMethodEntry("secstrPassword");
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secstrPassword);
                string returnValue = Marshal.PtrToStringUni(unmanagedString);
                log.LogMethodExit("returnValue");
                return returnValue;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
            
        }

        SecureString convertToSecureString(string strPassword)
        {
            log.LogMethodEntry("strPassword");
            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            log.LogMethodExit("secureStr");
            return secureStr;
        }

        private string FormatCardNumber(string o)
        {
            log.LogMethodEntry("0");
            string returnValue = System.Text.RegularExpressions.Regex.Replace(o, "[^0-9]", string.Empty);
            log.LogMethodExit("returnValue");
            return returnValue;
        }

        void Process(SecureString CCTrack)
        {
            log.LogMethodEntry("CCTrack");
            SecureString track = new SecureString();
            string[] CardData = new string[0];
            SecureString cardId = new SecureString();
            string expDate = "";

            try
            {
                bool CaretPresent = false;
                bool EqualPresent = false;

                CaretPresent = convertToUNSecureString(CCTrack).Contains("^");
                EqualPresent = convertToUNSecureString(CCTrack).Contains("=");

                if (convertToUNSecureString(CCTrack).Contains("%"))
                    track = convertToSecureString(convertToUNSecureString(CCTrack).Substring(convertToUNSecureString(CCTrack).IndexOf("%")));
                else if (convertToUNSecureString(CCTrack).Contains(";"))
                    track = convertToSecureString(convertToUNSecureString(CCTrack).Substring(convertToUNSecureString(CCTrack).IndexOf(";")));
                else
                {
                    CCTrack.Clear();
                    return;
                }
                CCTrack.Clear();

                if (CaretPresent)
                {
                    CardData = convertToUNSecureString(track).Split('^');
                    //B1234123412341234^CardUser/John^030510100000019301000000877000000?

                    cardId = convertToSecureString(FormatCardNumber(CardData[0]));
                    expDate = CardData[2].Substring(0, 4);
                }
                else if (EqualPresent)
                {
                    CardData = convertToUNSecureString(track).Split('=');
                    //1234123412341234=0305101193010877?

                    cardId = convertToSecureString(FormatCardNumber(CardData[0]));
                    expDate = CardData[1].Substring(0, 4);
                }
                else
                {
                    track.Clear();
                    txtStatus.Text = "Swipe Error. Please retry...";
                    return;
                }
            }
            catch(Exception ex)
            {
                CCTrack.Clear();
                track.Clear();
                cardId.Clear();
                Array.Clear(CardData, 0, CardData.Length);
                txtStatus.Text = "Swipe Error. Please retry...";
                log.Error("Error :Swipe Error", ex);
                return;
            }

            Array.Clear(CardData, 0, CardData.Length);

            try
            {
                btnCancel.Enabled = false;
                txtStatus.Text = "PROCESSING...";

                int paymentId = _alohaObject.ApplyCreditCardPayment(_chargeAmount, convertToUNSecureString(cardId), expDate, convertToUNSecureString(track));

                track.Clear();
                string cardXX = new String('X', 12) + convertToUNSecureString(cardId).Substring(12);
                long status = ParafaitAlohaIntegrator.PAYMENT_WAITING;
                while (status == ParafaitAlohaIntegrator.PAYMENT_WAITING)
                {
                    System.Threading.Thread.Sleep(300);
                    status = _alohaObject.GetPaymentStatus(paymentId);
                }

                if (status == ParafaitAlohaIntegrator.PAYMENT_SUCCESS)
                {
                    print(cardXX);
                    txtStatus.Text = "APPROVED";
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    txtStatus.Text = "DECLINED";
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }

                Close();
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtStatus.Text = "SWIPE CARD";
                btnCancel.Enabled = true;
                log.Error("Error :Swipe Error", ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        void print(string cardId)
        {
            log.LogMethodEntry("cardId");
            string receipt = getReceiptText(cardId, false);

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += (sender, e) =>
            {
                Font f = new Font("Courier New", 9);
                e.Graphics.DrawString(receipt, f, Brushes.Black, 12, 20);
            };
            pd.Print();
            log.LogMethodExit();
        }
        
        string getReceiptText(string cardId, bool pMerchantReceipt = true)
        {
            log.LogMethodEntry("cardId", "pMerchantReceipt");
            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;

            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Far;

            string FieldValue = "";

            StringBuilder g = new StringBuilder();
            FieldValue = "Chuck E. Cheese";
            g.AppendLine(FieldValue);
            g.AppendLine();
            g.AppendLine();

            FieldValue = "Date: " + DateTime.Now.ToString("MM-dd-yyyy H:mm:ss");
            g.AppendLine(FieldValue);

            FieldValue = "Merchant ID: " + "";
            g.AppendLine(FieldValue);

            FieldValue = "Invoice No : " + _alohaObject.GetCheckId();
            g.AppendLine(FieldValue);

            FieldValue = "Type: " + "";
            g.AppendLine(FieldValue);

            FieldValue = "Card Number: " + cardId;
            g.AppendLine(FieldValue);

            FieldValue = "Amount: " + _chargeAmount;
            g.AppendLine(FieldValue);
            g.AppendLine();

            FieldValue = "Auth Code: " + "";
            g.AppendLine(FieldValue);

            FieldValue = "Ref: " + "";
            g.AppendLine(FieldValue);
            g.AppendLine();
            g.AppendLine();

            FieldValue = "I AGREE TO PAY THE ABOVE TOTAL AMOUNT ACCORDING TO CARD ISSUER AGREEMENT";
            g.AppendLine(FieldValue);
            g.AppendLine();

            if (pMerchantReceipt)
                FieldValue = "* Merchant Receipt *";
            else
                FieldValue = "* Customer Receipt *";

            g.AppendLine(FieldValue);
            g.AppendLine();
            log.LogMethodExit(g);
            return g.ToString();
        }
    }
}
