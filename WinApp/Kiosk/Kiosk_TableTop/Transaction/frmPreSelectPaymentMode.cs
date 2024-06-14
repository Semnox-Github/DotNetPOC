/********************************************************************************************
 * Project Name - Portrait Kiosk
 * Description  - user interface -frmReceiptDeliveryModeOptions
 * 
 **************
 **Version Log
 **************
 *Version        Date            Modified By    Remarks          
 *********************************************************************************************
 *2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.POS;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmPreSelectPaymentMode : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<PaymentModesContainerDTO> paymentModesWithDisplayGroups;
        private string IMAGEFOLDER;
        private PaymentModesContainerDTO selectedPaymentModesContainerDTO = new PaymentModesContainerDTO();
        internal PaymentModesContainerDTO SelectedPaymentModesContainerDTO { get { return selectedPaymentModesContainerDTO; } }

        public frmPreSelectPaymentMode(ExecutionContext executionContext, List<PaymentModesContainerDTO> pOSPaymentModestWithPaymentDisplayGroups)
        {
            log.LogMethodEntry(executionContext, pOSPaymentModestWithPaymentDisplayGroups);
            InitializeComponent();
            this.executionContext = executionContext;
            this.paymentModesWithDisplayGroups = pOSPaymentModestWithPaymentDisplayGroups;
            KioskStatic.setDefaultFont(this);
            IMAGEFOLDER = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY");
            DisplaybtnHome(false);
            DisplaybtnCart(false);
            DisplaybtnCancel(false);
            btnClose.Visible = true;
            lblGreeting.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            lblGreeting.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4984);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            //fLPPaymentModes.AutoScroll = false;
            DisplayPaymentModeOptions();
            //DisplayScrollButtons();
            SetCustomImages();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }
        protected override CreateParams CreateParams
        {
            //this method is used to avoid the screen flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }
        private void frmPreSelectPaymentMode_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
            AdjustflpPaymentModesSizeAndLocation();
            log.LogMethodExit();
        }
        private void DisplayPaymentModeOptions()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            fLPPaymentModes.SuspendLayout();

            ValidateCashPaymentConfiguration();
           foreach (PaymentModesContainerDTO paymentModeContainerDTO in paymentModesWithDisplayGroups)
            {
                Button btnPayment = new Button();
                InitPaymentButtons(btnPayment);
                String btnText = "";
                if (KioskStatic.pOSPaymentModeInclusionDTOList == null
                    || KioskStatic.pOSPaymentModeInclusionDTOList.Any() == false)
                {
                    KioskStatic.logToFile("Error: No payment mode set. KioskStatic.pOSPaymentModeInclusionDTOList returned empty");
                    log.LogMethodExit();
                    return;
                }
                string paymentModeFriendlyName = KioskStatic.pOSPaymentModeInclusionDTOList.Where(p => p.PaymentModeId == paymentModeContainerDTO.PaymentModeId).FirstOrDefault().FriendlyName;
                string paymentMode = KioskStatic.pOSPaymentModeInclusionDTOList.Where(p => p.PaymentModeId == paymentModeContainerDTO.PaymentModeId).FirstOrDefault().PaymentModeDTO.PaymentMode;

                if (String.IsNullOrEmpty(paymentModeContainerDTO.ImageFileName))
                {
                    //set backgroung image
                    if (paymentModeContainerDTO.IsCash == true && ThemeManager.CurrentThemeImages.CashButtonSquareImage != null)
                        btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.CashButtonSquareImage;
                    else if (paymentModeContainerDTO.IsDebitCard == true && ThemeManager.CurrentThemeImages.GameCardButtonSquareImage != null)
                        btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.GameCardButtonSquareImage;
                    else if (paymentModeContainerDTO.IsCreditCard == true && ThemeManager.CurrentThemeImages.CreditCardButtonSquareImage != null)
                        btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.CreditCardButtonSquareImage;

                    //set friendly name
                    btnText = String.IsNullOrEmpty(paymentModeFriendlyName) ? paymentMode : paymentModeFriendlyName;
                    btnPayment.Text = btnText;
                }
                else
                {
                    try
                    {
                        object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                new SqlParameter("@FileName", IMAGEFOLDER + "\\" + paymentModeContainerDTO.ImageFileName));

                        btnPayment.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                        btnPayment.Text = "";
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        KioskStatic.logToFile(ex.Message + ": " + IMAGEFOLDER + "\\" + paymentModeContainerDTO.ImageFileName);
                        if (paymentModeContainerDTO.IsCash && ThemeManager.CurrentThemeImages.CashButtonSquareImage != null)
                        {
                            btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.CashButtonSquareImage;
                        }
                        else if (paymentModeContainerDTO.IsDebitCard && ThemeManager.CurrentThemeImages.GameCardButtonSquareImage != null)
                        {
                            btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.GameCardButtonSquareImage;
                        }
                        else if (paymentModeContainerDTO.IsCreditCard && ThemeManager.CurrentThemeImages.CreditCardButtonSquareImage != null)
                        {
                            btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.CreditCardButtonSquareImage;
                        }
                        else
                        {
                            if(ThemeManager.CurrentThemeImages.DebitCardButtonSquareImage != null)
                            {
                                btnPayment.BackgroundImage = ThemeManager.CurrentThemeImages.DebitCardButtonSquareImage;
                            }
                        }
                    }

                }
                btnPayment.Name = "btn" + btnText;
                btnPayment.Tag = paymentModeContainerDTO;
                if (paymentModeContainerDTO.IsDebitCard == true 
                    && KioskStatic.Utilities.getParafaitDefaults("SHOW_DEBIT_CARD_BUTTON") == "N")
                {
                    btnPayment.Visible = false;
                }
                fLPPaymentModes.Controls.Add(btnPayment);
            }
            Application.DoEvents();
            fLPPaymentModes.ResumeLayout(); 
            log.LogMethodExit();
        }

        private void InitPaymentButtons(Button btnPayment)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            btnPayment.Margin = new System.Windows.Forms.Padding(10, 15, 10, 10);
            btnPayment.BackColor = System.Drawing.Color.Transparent;
            btnPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnPayment.FlatAppearance.BorderSize = 0;
            btnPayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnPayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            btnPayment.Font = new System.Drawing.Font(fontFamName, 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnPayment.ForeColor = System.Drawing.Color.White;
            btnPayment.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.PreSelectPaymentModeButtonTextAlignment);
            btnPayment.Location = new System.Drawing.Point(10, 10);
            btnPayment.Size = new System.Drawing.Size(250, 250); 
            btnPayment.TabIndex = 12;
            btnPayment.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            btnPayment.UseVisualStyleBackColor = false;
            btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            btnPayment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            btnPayment.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            log.LogMethodExit();
        }
        private void btnPayment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                Button btnPayment = sender as Button;
                selectedPaymentModesContainerDTO = (PaymentModesContainerDTO)btnPayment.Tag;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing btnPayment_Click", ex);
                KioskStatic.logToFile("Error on btnPayment Click: " + ex.Message);
                string errMsg = MessageContainerList.GetMessage(executionContext, "Sorry unexpected error");
                frmOKMsg.ShowUserMessage(errMsg);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        } 
        private void AdjustflpPaymentModesSizeAndLocation()
        {
            log.LogMethodEntry();
            int ctrlCount = fLPPaymentModes.Controls.Count; 
            int newLoc = 92; 
            if (ctrlCount == 2)
            {
                int adjustmentFactor = 66;
                int locAdjustmentFactor = 14;
                int btnWidth = 250;
                int orginalWidth = 860;
                int newWidth = orginalWidth - btnWidth - adjustmentFactor;
                fLPPaymentModes.Size = new System.Drawing.Size(newWidth, fLPPaymentModes.Size.Height);
                int newGap = this.Width - fLPPaymentModes.Width;
                newLoc = (newGap / 2) - locAdjustmentFactor;
                fLPPaymentModes.Location = new Point(newLoc, fLPPaymentModes.Location.Y);
            }
            bigVerticalScrollPaymentModes.UpdateButtonStatus();
            log.LogMethodExit();
        }
        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Cancel Button Pressed in frmPreSelectPaymentMode: Triggering Home Button Action ");
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        }
        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.PreSelectPaymentBackground;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnClose.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.OkMsgButtons);
                btnClose.BackgroundImageLayout = ImageLayout.Stretch;
                this.bigVerticalScrollPaymentModes.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            }
            catch (Exception ex)
            {
                string msg = "Error while Setting Customized background images for frmPreSelectPaymentMode: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();

            try
            {
                foreach (Control c in fLPPaymentModes.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PreSelectPaymentModeBtnTextForeColor;//Payment options buttons
                        ((Button)c).TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.PreSelectPaymentModeButtonTextAlignment);
                    }
                }

                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.PreSelectPaymentModeLblHeadingTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.PreSelectPaymentModeBackButtonTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.PreSelectPaymentModeCancelButtonTextForeColor;//Cancel button
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.PreSelectPaymentModeBtnHomeTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors in frmPreSelectPaymentMode", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmPreSelectPaymentMode: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }
        private void ValidateCashPaymentConfiguration()
        {
            log.LogMethodEntry();

            bool found = false;
            if (paymentModesWithDisplayGroups == null || paymentModesWithDisplayGroups.Any() == false)
            {
                log.LogMethodExit();
                return;
            }
            if (paymentModesWithDisplayGroups.Exists(p => p.IsCash == true))
            {
                if (KioskStatic.config.baport > 0)
                {
                    foreach (KioskStatic.configuration.acceptorInfo ai in KioskStatic.config.Notes)
                    {
                        if (ai != null)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        foreach (KioskStatic.configuration.acceptorInfo ai in KioskStatic.config.Coins)
                        {
                            if (ai != null)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        paymentModesWithDisplayGroups.RemoveAll(p => p.IsCash == true);
                        if (paymentModesWithDisplayGroups.Count == 1)
                        {
                            //Bank Note Validator Offline
                            frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(executionContext, 423));
                        }
                    }
                }
                else
                {
                    paymentModesWithDisplayGroups.RemoveAll(p => p.IsCash == true);
                }
                if (paymentModesWithDisplayGroups.Count == 1 && found == false)
                {
                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(executionContext, 423));
                }
            }
            log.LogMethodExit();
        }
        private void ScrollBtnClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
    }
}
