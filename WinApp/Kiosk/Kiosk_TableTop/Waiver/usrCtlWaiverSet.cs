/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - usrCtlWaiverSet 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System; 
using System.Windows.Forms;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.Customer;
using Semnox.Core.Utilities;
using System.Drawing; 
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Languages;
using Semnox.Parafait.KioskCore;
using System.Linq;

namespace Parafait_Kiosk
{

    public partial class usrCtlWaiverSet : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private CustomerBL customerBL;
        private WaiverSetDTO waiverSetDTO;
        private Utilities utilities;  
        private bool evenRowNo;
        private LookupValuesList serverTimeObject;
        private DateTime serverTime;
        private CustomerDTO signatoryCustomerDTO;
        private int elementHeight = 130;
        internal delegate void SignWaiverSetDelegate(WaiverSetDTO waiverSetDTO);
        internal SignWaiverSetDelegate SignWaiverSet;
        public WaiverSetDTO WaiverSetDTO { get { return waiverSetDTO; } set { waiverSetDTO = value; } }


        public usrCtlWaiverSet(WaiverSetDTO waiverSetDTO, bool evenRowNo, CustomerDTO customerDTO)
        {
            log.LogMethodEntry(waiverSetDTO, evenRowNo, customerDTO);
            this.WaiverSetDTO = waiverSetDTO;
            this.utilities = KioskStatic.Utilities;
            this.signatoryCustomerDTO = customerDTO;
            utilities.setLanguage();
            //this.btnColor = new Color();
            //this.btnColor = Color.FromArgb(40, 35, 105);
            InitializeComponent();
            //this.btnSignWaiverSet.BackColor = this.btnColor;
            utilities.setLanguage(this);
            this.evenRowNo = evenRowNo;
            serverTimeObject = new LookupValuesList(utilities.ExecutionContext);
            serverTime = serverTimeObject.GetServerDateTime();
            LoadWaiverSetControlData();
            log.LogMethodExit();
        }

        private void LoadWaiverSetControlData()
        {
            log.LogMethodEntry();
            if (this.WaiverSetDTO != null)
            {
                Color backGroundColor = Color.White;
                if (this.evenRowNo == false)
                {
                    backGroundColor = Color.Azure;
                }
                this.BackColor = backGroundColor; 
                this.lblWaiverSetName.Height = (waiverSetDTO.WaiverSetDetailDTOList.Count * elementHeight);
                this.lblWaiverSetName.Text = WaiverSetDTO.Description;
                SetCustomizedFontColors();
                SetSignButtonBackground();
                //this.lblWaiverSetName.BorderStyle = BorderStyle.FixedSingle;
                //this.btnSignWaiverSet.Height = (waiverSetDTO.WaiverSetDetailDTOList.Count * 50 > this.lblWaiverSetName.Height ?
                //                                waiverSetDTO.WaiverSetDetailDTOList.Count * 50 : this.lblWaiverSetName.Height);
                if (waiverSetDTO.WaiverSetDetailDTOList.Count * elementHeight > this.lblWaiverSetName.Height)
                {
                    this.lblWaiverSetName.AutoSize = false;
                    this.lblWaiverSetName.Height = waiverSetDTO.WaiverSetDetailDTOList.Count * elementHeight;
                }
                //else
                //{
                //    this.btnSignWaiverSet.Height = this.lblWaiverSetName.Height - 2;
                //}
                // this.lblWaiverSetName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                BuildWaiverDetails();
                this.Height = this.tPnlWaiverSet.Height + 10;
                //SetCustomizedFontColors();
            }
            log.LogMethodExit();
        }

        private void SetSignButtonBackground()
        {
            log.LogMethodEntry();
            Image signedWaiverImage = Properties.Resources.WaiverButtonBorderSigned;
            if (this.signatoryCustomerDTO != null && this.WaiverSetDTO.WaiverSetDetailDTOList != null && this.WaiverSetDTO.WaiverSetDetailDTOList.Any())
            {
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, this.signatoryCustomerDTO);
                if (customerBL.HasSigned(this.WaiverSetDTO.WaiverSetDetailDTOList, serverTime))
                {
                    this.btnSignWaiverSet.BackgroundImage = signedWaiverImage;
                }
            }
            log.LogMethodExit();
        }

        private void BuildWaiverDetails()
        {
            log.LogMethodEntry();
            int labelHeight = 0;
            for (int i = 0; i < waiverSetDTO.WaiverSetDetailDTOList.Count; i++)
            {
                WaiversDTO waiversDTO = (waiverSetDTO.WaiverSetDetailDTOList[i] as WaiversDTO);
                Label lblWaiverName = new Label()
                {
                    Font = lblWaiverSetName.Font,
                    TextAlign = ContentAlignment.MiddleLeft,
                    MinimumSize = new Size(300, elementHeight),
                    Size = new Size(300, elementHeight),
                    MaximumSize = new Size(300, 0), 
                    AutoSize = true,
                    Anchor = AnchorStyles.Left| AnchorStyles.Top|AnchorStyles.Right
                }; 
                lblWaiverName.Text = waiversDTO.Name;
                tPnlWaivers.Controls.Add(lblWaiverName, 0, i);
                labelHeight = labelHeight + lblWaiverName.Height;
                Label lblValidityDays = new Label()
                {
                    Text = (waiversDTO.ValidForDays == null ? string.Empty : ((int)waiversDTO.ValidForDays).ToString(utilities.ParafaitEnv.NUMBER_FORMAT)),
                    Font = lblWaiverSetName.Font,
                    MinimumSize = new Size(100, elementHeight),
                    TextAlign = ContentAlignment.MiddleCenter
                }; 
                lblValidityDays.Size = new Size(100, lblWaiverName.Height);
                tPnlWaivers.Controls.Add(lblValidityDays, 1, i);
                Button btnViewWaiver = new Button()
                {
                    Text = "View",
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    MinimumSize = new Size(125, elementHeight),
                    Size = new Size(125, lblWaiverName.Height),
                    Font = new Font(lblWaiverSetName.Font.FontFamily, 14),
                    ForeColor = Color.White,
                    //BackColor = this.btnColor,
                    BackgroundImage = ThemeManager.CurrentThemeImages.WaiverButtonBorderImage,
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Tag = waiversDTO  ,
                    Image = Properties.Resources.ViewWaiver_Btn,
                    ImageAlign = ContentAlignment.MiddleRight ,
                    Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)))
                };
                btnViewWaiver.Click += new EventHandler(btnViewClick); 
                tPnlWaivers.Controls.Add(btnViewWaiver, 2, i); 
                RowStyle temp = tPnlWaivers.RowStyles[0];
                tPnlWaivers.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                tPnlWaivers.RowCount = i + 1; 
            }
            tPnlWaivers.Height = ((waiverSetDTO.WaiverSetDetailDTOList.Count * elementHeight) > labelHeight ?
                waiverSetDTO.WaiverSetDetailDTOList.Count * elementHeight : labelHeight )+ 2;
            this.btnSignWaiverSet.Height = tPnlWaivers.Height;

            this.tPnlWaiverSet.Height = tPnlWaivers.Height + 5;

            log.LogMethodExit();
        }


        private void btnViewClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("View waiver file button clicked");
            Button btnviewWaiver = (Button)sender;
            if (btnviewWaiver.Tag != null)
            {
                WaiversDTO waiversDTO = (WaiversDTO)btnviewWaiver.Tag;
                log.LogVariableState("waiversDTO.WaiverFileName", waiversDTO.WaiverFileName);
                if (waiversDTO != null && string.IsNullOrEmpty(waiversDTO.WaiverFileName) == false)
                {
                    using (frmViewWaiverUI frmViewWaiverUI = new frmViewWaiverUI(waiversDTO))
                    {
                        frmViewWaiverUI.ShowDialog();
                    }
                }
            }
            log.LogMethodExit();
        } 
        private void btnSignWaiverSet_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            { 
                btnSignWaiverSet.Enabled = false;
                KioskStatic.logToFile("Sign waiver set button clicked");
                if (WaiverSetDTO != null)
                {
                     SignWaiverSet(waiverSetDTO);  
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error signing waiverset: " + ex.Message);
            }
            finally
            {
                btnSignWaiverSet.Enabled = true;
            }
            log.LogMethodExit();
        }

        //internal void RefreshWaiverSetInfo(CustomerDTO signatoryCustomerDTO)
        //{
        //    log.LogMethodEntry(signatoryCustomerDTO);
        //    this.signatoryCustomerDTO = signatoryCustomerDTO;
        //    SetSignButtonBackground();
        //    this.Refresh();
        //    log.LogMethodExit();
        //}

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements in usrCtlWaiverSet");
            try
            {
                this.tPnlWaivers.ForeColor = KioskStatic.CurrentTheme.frmUsrCtltPnlWaiversTextForeColor;//Back button
                this.tPnlWaiverSet.ForeColor = KioskStatic.CurrentTheme.frmUsrCtltPnlWaiverSetTextForeColor;//Back button
                btnSignWaiverSet.BackgroundImage = ThemeManager.CurrentThemeImages.WaiverButtonBorderImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in usrCtlWaiverSet: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
