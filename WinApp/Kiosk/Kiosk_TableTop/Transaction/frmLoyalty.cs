/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmLoyalty.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.ThirdParty.ThirdPartyLoyalty;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmLoyalty : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        public LoyaltyMemberDetails loyaltyMembers = new LoyaltyMemberDetails();
        public string selectedLoyaltyCardNo = "";
        private LoyaltyPrograms loyaltyPrograms;
        public frmLoyalty()
        {
            log.LogMethodEntry();
            InitializeComponent();
            InitializeLoyaltyProgram();
            KioskStatic.setDefaultFont(this);
            SetBackGroundImage();
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void InitializeLoyaltyProgram()
        {
            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_LOYALTY_INTERFACE").ToString() == "Y")
            {
                string LOYALTY_PROGRAM = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOYALTY_PROGRAM");
                //LOYALTY_PROGRAM = "ALOHA";
                ThirdPartyLoyaltyFactory.GetInstance().Initialize(Utilities);
                loyaltyPrograms = ThirdPartyLoyaltyFactory.GetInstance().GetLoyaltyProgram(LOYALTY_PROGRAM);
                if (loyaltyPrograms != null)
                {
                    loyaltyPrograms.LoadLoyaltyConfigs();
                }
                else
                {
                    log.Error("unable to intialize the Loyalty program instance. ");
                    KioskStatic.logToFile("Unable to intialize the Loyalty program instance.Check settings");
                    if (keypad != null)
                    {
                        keypad.Hide();
                    }
                    using (frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage("Loyalty program is not set up.Please check")))
                    {
                        fok.ShowDialog();
                        this.DialogResult = DialogResult.Cancel;
                    }
                    log.LogMethodExit();
                    return;
                }
            }
        }

        private void frmLoyalty_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("frmLoyalty_Load()");
            try
            {
                pbLoyalty.BackgroundImage = ThemeManager.CurrentThemeImages.LoyaltyFormLogoImage;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmLoyalty_Load()" + ex.Message);
                //pbLoyalty.BackgroundImage = Properties.Resources.CEC_MoreCheese_Logo;
                pbLoyalty.BackgroundImage = ThemeManager.CurrentThemeImages.LoyaltyFormLogoImage;
            }
            panelPhoneNoInfo.Visible = false;
            panelFirstNameInfo.Visible = false;
            //Are you a More Cheese™ Rewards member?
            lblLoyaltyText.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 5071);
            lblPhoneNo.Text = KioskStatic.Utilities.MessageUtils.getMessage("Enter Phone Number:");
            txtPhoneNo.Text = txtFirstName.Text = txtMessage.Text = "";
            listboxNames.Visible = false;
            txtPhoneNo.ReadOnly = false;
            btnOk.Enabled = false;
            btnProceedWithoutLoyalty.Visible = false;
            btnCancel.Enabled = false;
            log.LogMethodExit();
        }

        private void SetBackGroundImage()
        {
            log.LogMethodEntry();
            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.LoyaltyFormBackgroundImage;
                btnLoyaltyYes.BackgroundImage = btnLoyaltyNo.BackgroundImage = ThemeManager.CurrentThemeImages.YesNoButtonImage;
                btnProceedWithoutLoyalty.BackgroundImage = btnLoyaltyNo.BackgroundImage = ThemeManager.CurrentThemeImages.YesNoButtonImage;
                btnOk.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnGo.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                //txtFirstName.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                //txtMessage.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                //txtPhoneNo.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                //listboxNames.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;

        void ShowKeyPad()
        {
            log.LogMethodEntry();
            keypad = new AlphaNumericKeyPad(this, txtPhoneNo, KioskStatic.CurrentTheme.KeypadSizePercentage);
            keypad.Location = new Point(panelPhoneNoInfo.Location.X + 512, (panelPhoneNoInfo.Bottom + (flpInfo.Height / 2) + panelPhoneNoInfo.Height)-178);
            keypad.Show();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnCancel_Click");
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void btnLoyaltyYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnLoyaltyYes_Click()");
            panelPhoneNoInfo.Visible = true;
            txtMessage.Text = MessageUtils.getMessage(1461);//Message:- "Please Enter The Phone Number"
            btnLoyaltyYes.Enabled = false;
            btnLoyaltyNo.Enabled = false;
            btnCancel.Enabled = true;
            ShowKeyPad();
            log.LogMethodExit();
        }
        int exitCount = 3;
        private async void btnGo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Start:btnGo_Click()");
            listboxNames.Controls.Clear();
            panelFirstNameInfo.Visible = false;
            if (exitCount <= 0)
            {
                KioskStatic.logToFile("frmLoyalty() is Closing Because No Reward Member Found in 3 Attempt");
                keypad.Hide();
                frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(1459));
                //Message : "Reward member not found. Please see front counter.");
                fok.ShowDialog();
                this.Close();
            }
            Regex phNum = new Regex("^[\\d]{10}$");
            Match getmatch = phNum.Match(txtPhoneNo.Text);
            if (getmatch.Success)
            {
                txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(1457);//"Getting Loyalty Information...");
                btnCancel.Enabled = false;
                // ResetKioskTimer();
                try
                {
                    loyaltyMembers = await loyaltyPrograms.GetCustomers(txtPhoneNo.Text);
                }
                catch (ValidationException vlex)
                {
                    log.Debug(vlex.Message);
                    KioskStatic.logToFile("GetCustomers():We’re sorry, rewards are not available at this time. Please see front counter.");
                    keypad.Hide(); 
                    frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(3021));
                    // Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
                    fok.ShowDialog();
                    btnProceedWithoutLoyalty.Visible = true;
                    btnOk.Visible = false;
                    btnCancel.Visible = false;

                    log.LogMethodExit();
                    return;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("GetCustomers():We’re sorry, rewards are not available at this time. Please see front counter.");
                    keypad.Hide();
                    frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(1458));
                    // Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
                    fok.ShowDialog();
                    this.Close();
                    btnProceedWithoutLoyalty.Visible = true;
                    btnOk.Visible = false;
                    btnCancel.Visible = false;
                    log.LogMethodExit();
                    return;
                }
                if (loyaltyMembers == null || loyaltyMembers.LoyaltyFirstName.Count == 0)
                {
                    exitCount--;
                    KioskStatic.logToFile("Reward member not found");
                    txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(1456);// message :Reward member not found");
                    btnCancel.Visible = false;
                    btnOk.Visible = false;
                    btnProceedWithoutLoyalty.Visible = true;
                }
                else 
                {
                    keypad.Hide();
                    txtPhoneNo.ReadOnly = true;
                    //loyaltyMembers.LoyaltyFirstName = new List<string>() { "Ted" };
                    //loyaltyMembers.AlohaLoyaltyCardNumber = new List<string>() { "ABCD1234", "EFGH4321" };
                    if (loyaltyMembers.LoyaltyFirstName.Count > 1)
                        txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(1460);//Message :- Please Select  Fisrt Name
                    else
                        txtMessage.Text = "";
                    Dictionary <string, string> lstLoyaltyMember = new Dictionary<string, string>();
                    foreach (string firstName in loyaltyMembers.LoyaltyFirstName)
                    {
                        foreach (string cardNo in loyaltyMembers.AlohaLoyaltyCardNumber)
                        {
                            lstLoyaltyMember.Add(cardNo, firstName);
                        }
                    }
                    listboxNames.DataSource = new BindingSource(lstLoyaltyMember, null);
                    listboxNames.DisplayMember = "Value";
                    listboxNames.ValueMember = "Key";
                    try
                    {
                        panelFirstNameInfo.SuspendLayout();
                        panelFirstNameInfo.Visible = true;
                        listboxNames.Visible = true;
                        listboxNames.Height = lblFirstName.Height * lstLoyaltyMember.Count;
                        btnCancel.Enabled = true;
                        btnOk.Enabled = true;
                        btnCancel.Visible = true;
                        btnOk.Visible = true;
                        btnProceedWithoutLoyalty.Visible = false;
                    }
                    finally
                    {
                        panelFirstNameInfo.ResumeLayout(true);
                    }
                }
            }
            else
            {
                txtMessage.Text = MessageUtils.getMessage(1462);//Message :- "Please Enter The valid Phone Number"
            }
            log.LogMethodExit();
        }
        //private async Task CallWebAPIGetMemberDetails()
        //{
        //    log.LogMethodEntry();
        //    KioskStatic.logToFile("CallWebAPIGetMemberDetails():Method start");
        //    string searchUrl = "?searchby=phone&phone=";
        //    searchUrl = searchUrl + txtPhoneNo.Text;
        //    LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
        //    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
        //    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ALOHA_LOYALTY_CONFIG"));
        //    String serviceUri = "";
        //    string username = "";
        //    string password = "";
        //    string applicationId = "";
        //    List<LookupValuesDTO> alohaConfigValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
        //    if (alohaConfigValueList != null)
        //    {
        //        foreach (LookupValuesDTO value in alohaConfigValueList)
        //        {
        //            if (value.LookupValue == "ALOHA_LOYALTY_INTERFACE_URL")
        //                serviceUri = value.Description.ToString();
        //            else if(value.LookupValue == "ALOHA_LOYALTY_INTERFACE_USERNAME")
        //                username = value.Description.ToString();
        //            else if (value.LookupValue == "ALOHA_LOYALTY_INTERFACE_PASSWORD")
        //                password = value.Description.ToString();
        //            else if(value.LookupValue == "ALOHA_LOYALTY_APPLICATION_ID")
        //                applicationId = value.Description.ToString();
        //        }
        //    }
        //    searchUrl = serviceUri + searchUrl;
        //    string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.Timeout = TimeSpan.FromSeconds(10);
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Add("Application-Id", applicationId);
        //        using (HttpResponseMessage response = await client.GetAsync(searchUrl))
        //        {
        //            var responseContent = await response.Content.ReadAsStringAsync();
        //            if (response.IsSuccessStatusCode)
        //            {
        //                JavaScriptSerializer serializer = new JavaScriptSerializer();
        //                KioskStatic.AlohaLoyaltyMember memberDetails = serializer.Deserialize<KioskStatic.AlohaLoyaltyMember>(responseContent);
        //                if (memberDetails != null && memberDetails.values != null)
        //                {
        //                    foreach (String[] val in memberDetails.values)
        //                    {
        //                        loyaltyMembers = new LoyaltyMemberDetails(val[0], val[2], val[3], val[4]);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                KioskStatic.logToFile("CallWebAPIGetMemberDetails():We’re sorry, rewards are not available at this time. Please see front counter.");
        //                keypad.Hide();
        //                frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(1458));// Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
        //                fok.ShowDialog();
        //                this.Close();
        //                log.LogMethodExit();
        //                return;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        KioskStatic.logToFile("CallWebAPIGetMemberDetails():We’re sorry, rewards are not available at this time. Please see front counter.");
        //        keypad.Hide();
        //        frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(1458));// Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
        //        fok.ShowDialog();
        //        this.Close();
        //        log.LogMethodExit();
        //        return;
        //    }
        //}
       
        private void listboxNames_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtFirstName.Text = listboxNames.SelectedItem.ToString();
            log.LogMethodExit();
        }

        private void txtPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar == '+'))
                e.Handled = true;
            log.LogMethodExit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnOk_Click");
            selectedLoyaltyCardNo = listboxNames.SelectedValue.ToString();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }
        private void btnProceedWithoutLoyalty_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnProceedWithouLoyalty_Click");
            txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage("Proceeding without loyalty");
            frmYesNo fok = new frmYesNo(KioskStatic.Utilities.MessageUtils.getMessage(3022));
            DialogResult dialogResult = fok.ShowDialog();
            if (dialogResult == DialogResult.Yes)
            {
                log.Debug("Proceed without loyalty");
                selectedLoyaltyCardNo = string.Empty;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                this.Close();
                DialogResult = System.Windows.Forms.DialogResult.No;
            }
            log.LogMethodExit();
        }

        private void frmLoyalty_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmLoyalty_FormClosing()");
            if (keypad != null)
            {
                keypad.Hide();
                keypad.Dispose();
            }
            listboxNames.Controls.Clear();
            txtPhoneNo.Clear();
            log.LogMethodExit();
        }

        private void btnLoyaltyNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnLoyaltyNo_Click()");
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }
        private void frmLoyalty_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void txtPhoneNo_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            if (txtPhoneNo.ReadOnly == false)
            {
                if (keypad != null)
                {
                    keypad.Hide();
                    keypad.Dispose();
                }
                ShowKeyPad();
            }
            //keypad.Show();
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblLoyaltyText.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyLblLoyaltyTextTextForeColor;//How many points or minutes per card label
                this.btnLoyaltyYes.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyBtnLoyaltyYesTextForeColor;//Back button
                this.btnLoyaltyNo.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyBtnLoyaltyNoTextForeColor;//Cancel button
                this.lblPhoneNo.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyLblPhoneNoTextForeColor;//Variable button
                this.txtPhoneNo.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyTxtPhoneNoTextForeColor;//Footer text message
                this.btnGo.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyBtnGoTextForeColor;//Footer text message
                this.listboxNames.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyListboxNamesTextForeColor;//Footer text message
                this.txtFirstName.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyTxtFirstNameTextForeColor;//Footer text message
                this.btnOk.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyBtnOkTextForeColor;//Footer text message
                this.btnProceedWithoutLoyalty.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyBtnProceedWithoutLoyaltyTextForeColor;//Footer text message
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyBtnCancelTextForeColor;//Footer text message
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmLoyaltyTxtMessageTextForeColor;//Footer text message 
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmLoyalty: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}











