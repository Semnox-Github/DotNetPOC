/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - Intermediate screen for frmSelectSlot UI for managing slowness while UI switch.
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.155.0.0    16-Jun-2023     Sathyavathi        Created for Attraction Sale in Kiosk
*2.152.0.0    12-Dec-2023     Suraj Pai          Modified for Attraction Sale in TableTop Kiosk
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;

namespace Parafait_Kiosk
{
    public partial class frmProcessingAttractions : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        private int itemCount;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public KioskAttractionDTO GetKioskAttractionDTO { get { return kioskAttractionDTO; } }

        public frmProcessingAttractions(KioskTransaction kioskTransaction, KioskAttractionDTO kioskAttractionDTO, int itemCount)
        {
            log.LogMethodEntry("kioskTransaction", kioskAttractionDTO, itemCount);
            this.kioskTransaction = kioskTransaction;
            this.kioskAttractionDTO = kioskAttractionDTO;
            this.itemCount = itemCount;
            InitializeComponent();
            StopKioskTimer();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            txtMessage.Text = "";
            KioskStatic.setDefaultFont(this);
            DisplaybtnCancel(false);
            DisplaybtnPrev(false);
            btnProceed.Visible = false;
            try
            {
                lblProcessingMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008); //Processing..Please wait...
                SetFont();
                SetCustomImages();
                SetCustomizedFontColors();
                SetKioskTimerTickValue(20);
                ResetKioskTimer();
                utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing frmProcessingAttractions(): " + ex);
            }

            log.LogMethodExit();
        }

        private void frmProcessingAttractions_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            try
            {
                Application.DoEvents();
                if (itemCount > -1)
                { 
                    int productId = -1;
                    int comboProductId = -1;
                    int attractionChildCount = 0;
                    bool enableDateEdit = (itemCount == 0 ? true : false);
                    if (kioskAttractionDTO.ChildAttractionBookingDTOList != null
                        && kioskAttractionDTO.ChildAttractionBookingDTOList.Any())
                    {
                        for (int i = 0; i < kioskAttractionDTO.ChildAttractionBookingDTOList.Count; i++)
                        {
                            KioskAttractionChildDTO childProduct = kioskAttractionDTO.ChildAttractionBookingDTOList[i];
                            if (childProduct.ChildProductType != ProductTypeValues.ATTRACTION)
                            {
                                continue;
                            }
                            if (attractionChildCount == itemCount)
                            {
                                productId = childProduct.ChildProductId;
                                comboProductId = childProduct.ComboProductId;
                                break;
                            }
                            attractionChildCount++;
                        }
                    }
                    else
                    {
                        productId = kioskAttractionDTO.ProductId;
                    }
                    using (frmSelectSlot frs = new frmSelectSlot(kioskTransaction, productId, comboProductId, kioskAttractionDTO, enableDateEdit, ServerDateTime.Now))
                    {
                        DialogResult dr = frs.ShowDialog();
                        kioskTransaction = frs.GetKioskTransaction;
                        kioskAttractionDTO = frs.KioskAttractionDTO;
                        this.DialogResult = dr;
                        Close();
                    }
                } 
                 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing btnProceed_Click() of frmProcessingAttractions: " + ex.Message);
            }
            finally
            {
                ResetKioskTimer();
                StartKioskTimer();
            }
            log.LogMethodExit();
        } 

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                Application.DoEvents();
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                lblProcessingMsg.Font = new System.Drawing.Font(lblProcessingMsg.Font.FontFamily, 50F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while Setting Customized fonts for frmProcessingAttractions";
                log.Error(msg, ex);
                KioskStatic.logToFile("ERROR: " + msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images for frmProcessingAttractions");
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.ProcessingAttractionsBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.btnPrev.BackgroundImage =
                    btnProceed.BackgroundImage =
                    btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while Setting Customized background images for frmProcessingAttractions";
                log.Error(msg, ex);
                KioskStatic.logToFile("ERROR: " + msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized font colors for the UI elements for frmProcessingAttractions");
            try
            {
                this.lblProcessingMsg.ForeColor = KioskStatic.CurrentTheme.ProcessingAttractionsLblProcessingMsgTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.ProcessingAttractionsFooterTxtMsgTextForeColor;//Footer text message
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.ProcessingAttractionsProceedButtonTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.ProcessingAttractionsCancelButtonTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.ProcessingAttractionsBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.ProcessingAttractionsHomeButtonTextForeColor;
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while setting customized font colors for the UI elements for frmProcessingAttractions: ";
                log.Error(msg, ex);
                KioskStatic.logToFile("ERROR: " + msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmProcessingAttractions_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmProcessingAttractions_FormClosed()", ex);
                KioskStatic.logToFile("Error occurred while executing frmProcessingAttractions_FormClosed(): " + ex.Message);
            }

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }
    }
}