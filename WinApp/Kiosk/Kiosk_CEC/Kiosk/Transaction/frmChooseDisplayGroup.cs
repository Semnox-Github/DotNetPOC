/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - frmChooseDisplayGroup.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        03-Sep-2019       Deeksha            Added logger methods.
 *2.80.1      30-Oct-2020       Deeksha            Modified to pull display group text message from the message container.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0     30-Jun-2021       Dakshak            Theme changes to support customized Font ForeColor
 *2.130.11    13-Oct-2022       Vignesh Bhat       Ability to display background images for display group 
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmChooseDisplayGroup : BaseFormKiosk
    {
        string Function;
        string selectedEntitlementType;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        private string imageFolder;
        private DataTable ProductTbl;
        public frmChooseDisplayGroup(string pFunction, string entitlementType)
        {
            log.LogMethodEntry(pFunction, entitlementType);
            KioskStatic.logToFile("frmChooseDisplayGroup()");
            selectedEntitlementType = entitlementType;
            imageFolder = KioskStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            KioskStatic.Utilities.setLanguage();
            InitializeComponent();

            Function = pFunction;

            this.ShowInTaskbar = false;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            KioskStatic.Utilities.setLanguage(this);

            try
            {
               this.BackgroundImage = KioskStatic.CurrentTheme.ProductBackgroundImage;
                KioskStatic.setDefaultFont(this);
            }
            catch { }

            txtMessage.Text = "";
            initializeDisplayGroups();
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void frmChooseDisplayGroup_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;

            if (Function == "I")
            {
                Audio.PlayAudio(Audio.SelectNewCardProduct);
            }
            else
            {
                Audio.PlayAudio(Audio.SelectTopUpProduct);
            }

            //lblGreeting1.ForeColor = lblGreeting1.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            lblGreeting1.Text = KioskStatic.Utilities.MessageUtils.getMessage("SELECT DISPLAY GROUP");
            this.FormClosing += frmChooseDisplayGroup_FormClosing;
            displaybtnCancel(true);
            log.LogMethodExit();
        }

        void frmChooseDisplayGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            Audio.Stop();
            KioskStatic.logToFile("exit frmChooseDisplayGroup()");
            log.LogMethodExit();
        }

        public void initializeDisplayGroups()
        {
            log.LogMethodEntry();
            flpCardProducts.Tag = null;
            flpCardProducts.Controls.Clear();
            ProductTbl = new DataTable();

            ProductTbl = KioskStatic.GetProductDisplayGroups(Function,selectedEntitlementType);
            if (ProductTbl != null && ProductTbl.Rows.Count>0)
            {
                for (int i = 0; i < ProductTbl.Rows.Count; i++)
                {
                    Button displayGroupButton = new Button();
                    displayGroupButton.Click += displayGroupButton_Click;
                    displayGroupButton.Name = "displayGroupButton" + i.ToString();
                    displayGroupButton.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext , ProductTbl.Rows[i]["display_group"].ToString());

                    displayGroupButton.Tag = ProductTbl.Rows[i]["display_group"];
                    if (KioskStatic.CurrentTheme.DisplayGroupButtonFont != null)
                    {
                        displayGroupButton.Font = new Font(KioskStatic.CurrentTheme.DisplayGroupButtonFont.Name, KioskStatic.CurrentTheme.DisplayGroupButtonFont.Size, KioskStatic.CurrentTheme.DisplayGroupButtonFont.Style);
                    }
                    else
                    {
                        displayGroupButton.Font = new Font(KioskStatic.CurrentTheme.DefaultFont.Name, btnSampleName.Font.Size, btnSampleName.Font.Style);
                    }
                    displayGroupButton.ForeColor = KioskStatic.CurrentTheme.ThemeForeColor;
                    displayGroupButton.Size = btnSampleName.Size; 
                    displayGroupButton.FlatStyle = btnSampleName.FlatStyle;
                    displayGroupButton.FlatAppearance.BorderColor = btnSampleName.FlatAppearance.BorderColor;
                    displayGroupButton.FlatAppearance.BorderSize = btnSampleName.FlatAppearance.BorderSize;
                    displayGroupButton.BackColor = btnSampleName.BackColor;
                    displayGroupButton.BackgroundImage = KioskStatic.CurrentTheme.DisplayGroupButton;
                    displayGroupButton.BackgroundImageLayout = btnSampleName.BackgroundImageLayout;
                    displayGroupButton.Margin = btnSampleName.Margin;

                    displayGroupButton.FlatAppearance.MouseOverBackColor = btnSampleName.FlatAppearance.MouseOverBackColor;
                    displayGroupButton.FlatAppearance.MouseDownBackColor = btnSampleName.FlatAppearance.MouseDownBackColor;
                    displayGroupButton.FlatAppearance.CheckedBackColor = btnSampleName.FlatAppearance.CheckedBackColor;

                    Panel prodPanel = new Panel();
                    prodPanel.Width = flpCardProducts.Width;
                    prodPanel.Height = btnSampleName.Height;
                    displayGroupButton.Location = new Point((prodPanel.Width - btnSampleName.Width) / 2, 0);
                    prodPanel.Controls.Add(displayGroupButton);
                    flpCardProducts.Controls.Add(prodPanel);

                    if (ProductTbl.Rows[i]["ImageFileName"].ToString().Trim() != string.Empty)
                    {
                        try
                        {
                            object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                    new SqlParameter("@FileName", imageFolder + "\\" + ProductTbl.Rows[i]["ImageFileName"].ToString()));

                            displayGroupButton.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                            displayGroupButton.Text = "";
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + ProductTbl.Rows[i]["ImageFileName"].ToString());
                            displayGroupButton.BackgroundImage = KioskStatic.CurrentTheme.DisplayGroupButton;
                            displayGroupButton.Text = ProductTbl.Rows[i]["display_group"].ToString();
                        }
                    }
                    else
                    {
                        displayGroupButton.BackgroundImage = KioskStatic.CurrentTheme.DisplayGroupButton;
                        displayGroupButton.Text = ProductTbl.Rows[i]["display_group"].ToString();
                    }

                }
                if (flpCardProducts.Controls.Count >6)
                {
                    vScrollBarProducts.Visible = true;
                    vScrollBarProducts.Maximum = (int)((flpCardProducts.Height - panelProducts.Height) * 1.3);
                    vScrollBarProducts.SmallChange = Math.Max(1, vScrollBarProducts.Maximum / 10);
                    vScrollBarProducts.LargeChange = Math.Max(1, vScrollBarProducts.Maximum / 4);
                }
                else
                {
                    vScrollBarProducts.Visible = false;
                }
                flpCardProducts.Refresh();
            }
            log.LogMethodExit();
        }
        private void displayGroupButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //string selectedEntitlement = string.Empty;
            Button displayGroupButton = new Button();
            displayGroupButton = (Button)sender;
            
            if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
            {
                if (KioskHelper.isTimeEnabledStore() == true)
                {
                    selectedEntitlementType = "Time";
                }
                else
                {
                    frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345));
                    if(frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        frmEntitle.Dispose();
                        log.LogMethodExit();
                        return;
                    }
                    selectedEntitlementType = frmEntitle.selectedEntitlement;
                    frmEntitle.Dispose();
                }
            }
            string backgroundImageFileName = GetBackgroundImageFileName(ProductTbl, displayGroupButton.Tag.ToString());
            //frmChooseProduct frm = new frmChooseProduct(Function, (displayGroupButton.Tag == null) ? "" : displayGroupButton.Tag.ToString());
            frmChooseProduct frm = new frmChooseProduct(Function, selectedEntitlementType, (displayGroupButton.Tag == null) ? "" : displayGroupButton.Tag.ToString(), backgroundImageFileName);
            if (frm == null || (frm != null && frm.selectedEntitlementType == null))
                frm.Dispose();
            else
            {
                if (frm.ShowDialog() == DialogResult.Cancel)
                {
                    Close();
                    Dispose();
                }
            }
            log.LogMethodExit();
        }      

        

        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Back pressed");
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

       // int ticks = 0;
        //private void inactivityTimer_Tick(object sender, EventArgs e)
        //{
        //    if (ticks > 20)
        //    {
        //        if (TimeOut.AbortTimeOut(this))
        //            ticks = 0;
        //        else
        //            Close();
        //    }
        //    else if (ticks > 30)
        //        Close();
        //    ticks++;
        //}

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.back_btn_pressed;
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.back_btn;
        }

        private void frmChooseDisplayGroup_Activated(object sender, EventArgs e)//Playpas1:starts
        {
            //ticks = 0;
            //inactivityTimer.Start();
            log.LogMethodEntry();
            StartKioskTimer();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void frmChooseDisplayGroup_Deactivate(object sender, EventArgs e)
        {
            //inactivityTimer.Stop();
            log.LogMethodEntry();
            StopKioskTimer();
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Cancel pressed");
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        private void vScrollBarProducts_Scroll(object sender, ScrollEventArgs e)
        {
            //ticks = 0;
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (e.NewValue > e.OldValue)
                    scrollDown(e.NewValue - e.OldValue);
                else if (e.NewValue < e.OldValue)
                    scrollUp(e.OldValue - e.NewValue);
            }
            catch { }
            log.LogMethodExit();
        }

        void scrollDown(int value = 10)
        {
            log.LogMethodEntry(value);
            if (flpCardProducts.Top + flpCardProducts.Height > panelProducts.Height)
            {
                flpCardProducts.Top = flpCardProducts.Top - value;
            }
            log.LogMethodExit();
        }

        void scrollUp(int value = 10)
        {
            log.LogMethodEntry(value);
            if (flpCardProducts.Top < 0)
                flpCardProducts.Top = Math.Min(0, flpCardProducts.Top + value);
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                foreach (Control c in flpCardProducts.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("panel"))
                    {
                        foreach (Control btn in c.Controls)
                        {
                            string btnType = btn.GetType().ToString().ToLower();
                            if (btnType.Contains("button"))
                            {
                                btn.ForeColor = KioskStatic.CurrentTheme.DisplayGroupBtnTextForeColor;//Products buttons 
                            }
                        }
                    }
                }
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.DisplayGroupGreetingsTextForeColor;//What would you like to Add? Label
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.DisplayGroupBackBtnTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.DisplayGroupCancelBtnTextForeColor;//Cancel button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.DisplayGroupFooterTextForeColor;//Footer text message
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private string GetBackgroundImageFileName(DataTable ProductTbl, string displayGroup)
        {
            log.LogMethodEntry();
            string backgroundImageFileName = String.Empty;
            for (int i = 0; i < ProductTbl.Rows.Count; i++)
            {
                if (ProductTbl.Rows[i]["display_group"].ToString().Trim() == displayGroup)
                {
                    backgroundImageFileName = ProductTbl.Rows[i]["BackgroundImageFileName"].ToString().Trim();
                    break;
                }
                else
                {
                    continue;
                }
            }
            log.LogMethodExit(backgroundImageFileName);
            return backgroundImageFileName;
        }
    }
}
