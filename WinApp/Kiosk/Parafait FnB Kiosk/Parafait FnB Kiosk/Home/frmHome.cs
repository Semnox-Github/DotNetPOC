/********************************************************************************************
* Project Name - Parafait_Kiosk -frmHome.cs
* Description  - frmHome 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmHome : BaseForm
    {
        bool showPartySize;
        public frmHome()
        {
            log.LogMethodEntry();
            InitializeComponent();
            DisplayPartySize();
            log.LogMethodExit();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                base.RenderPanelContent(_screenModel, panelHeader, 1);
                base.RenderPanelContent(_screenModel, flpOptions, 2);

                this.KeyPreview = true;
                this.KeyPress += frmHome_KeyPress;

               

            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }

        void frmHome_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();

            if ((int)e.KeyChar == 3)
                Cursor.Show();
            else if ((int)e.KeyChar == 8)
                Cursor.Hide();
            else if ((int)e.KeyChar == 18)
                Application.Restart();
            else if ((int)e.KeyChar == 19)
            {
                Parafait_Kiosk.SetUp s = new Parafait_Kiosk.SetUp();
                s.ShowDialog();
            }
            else if ((int)e.KeyChar == 5) // ctrl e
            {
                Common.logToFile("Ctrl-E pressed");
                Application.Exit();
            }
            else
                e.Handled = true;
            log.LogMethodExit();
        }

        internal override bool ValidateAction(ScreenModel.UIPanelElement element)
        {
            log.LogMethodEntry(element);
            if (element.ActionScreenId > 0)
            {
                ScreenModel screen = new ScreenModel(element.ActionScreenId);
                if (screen.CodeObjectName == "frmChangeLanguage")
                {
                    changeLanguage();
                    log.LogMethodExit(false);
                    return false;
                }
                else if (screen.CodeObjectName == "frmShowContent")
                {
                    log.LogMethodExit(true);
                    return true;
                }

                if ((!UserTransaction.OrderDetails.GuestCountSelected) && showPartySize)
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1110));
                    log.LogMethodExit(false);
                    return false;
                }
            }

            log.LogMethodExit(true);
            return true;
        }
        
        private void buttonNumber_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            Button b = sender as Button;

            if (b.Equals(btnOther))
            {
                cmbOtherNumber.DroppedDown = true;
            }
            else
            {
                foreach (Control c in flpDigits.Controls)
                {
                    (c as Button).Image = Properties.Resources.Number_Btn;
                    (c as Button).ForeColor = Common.PrimaryForeColor;
                }

                b.Image = Properties.Resources.Number_Btn_Pressed;
                b.ForeColor = Color.White;
                btnOther.Text = Common.utils.MessageUtils.getMessage("Other");
                btnOther.Image = Properties.Resources.Other_Btn;
                btnOther.ForeColor = Common.PrimaryForeColor;

                UserTransaction.OrderDetails.NumberOfGuests = Convert.ToInt16(b.Text);
            }
            log.LogMethodExit();
        }

        private void cmbOtherNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            btnOther.Text = cmbOtherNumber.SelectedItem.ToString();
            btnOther.Image = Properties.Resources.Other_Btn_Pressed;
            btnOther.ForeColor = Color.White;

            UserTransaction.OrderDetails.NumberOfGuests = Convert.ToInt16(btnOther.Text);

            foreach (Control c in flpDigits.Controls)
            {
                (c as Button).Image = Properties.Resources.Number_Btn;
                (c as Button).ForeColor = Common.PrimaryForeColor;
            }
            log.LogMethodExit();
        }

        private void frmHome_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (Control c in flpDigits.Controls)
            {
                (c as Button).Image = Properties.Resources.Number_Btn;
                (c as Button).ForeColor = Common.PrimaryForeColor;
            }

            btnOther.Text = Common.utils.MessageUtils.getMessage("Other");
            btnOther.Image = Properties.Resources.Other_Btn;
            btnOther.ForeColor = Common.PrimaryForeColor;

            UserTransaction.OrderDetails.NumberOfGuests = 0;
            log.LogMethodExit();
        }

        int semnoxClickCount = 0;
        private void panelHeader_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (semnoxClickCount++ >= 2)
            {
                semnoxClickCount = 0;
                Common.logToFile("Semnox logo multiple click");
                frmAdmin adminForm = new frmAdmin();
                DialogResult dr = adminForm.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    Common.logToFile("Exit from Admin screen");
                    Application.Exit();
                }
                else if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    Common.logToFile("Cancel from Admin screen");
                }
            }
            log.LogMethodExit();
        }

        private void changeLanguage()
        {
            log.LogMethodEntry();
            DataTable dt = Common.utils.executeDataTable(@"select LanguageCode, LanguageName, LanguageId, LanguageName sort 
                                                        from languages 
                                                        where Active = 1
                                                        union all 
                                                        select 'en-US', 'English', -1, '  '
                                                        where not exists (select 1 from Languages where LanguageName = 'English') 
                                                        order by sort");
            if (dt.Rows.Count > 1)
            {

                int languageId;
                string languageName;
                if (dt.Rows[0]["LanguageName"].ToString().Equals(btnLanguage.Text))
                {
                    languageId = (int)dt.Rows[0]["LanguageId"];
                    languageName = dt.Rows[0]["LanguageName"].ToString();
                }
                else
                {
                    languageId = (int)dt.Rows[1]["LanguageId"];
                    languageName = dt.Rows[1]["LanguageName"].ToString();
                }

                Common.logToFile("Language change to " + languageName);

                Common.utils.setLanguage(languageId);
                this.lblNumberOfPeople.Text = "How many people are in your party?";
                string savLanguage = btnLanguage.Text;

                Common.utils.setLanguage(this);

                _screenModel.Refresh();

                ScreenModel.UIPanel modelPanel = _screenModel.getPanelByIndex(2);
                int index = 1;
                foreach (Control c in flpOptions.Controls)
                {
                    if (!(c is Button))
                        continue;

                    ScreenModel.UIPanelElement element = modelPanel.getElementByIndex(index);
                    if (element != null)
                    {
                        if (element.Attribute.DisplayImage == null)
                        {
                            c.Text = element.Attribute.DisplayText;
                        }
                        else
                        {
                            Button b = c as Button;
                            b.Name = element.ElementName;
                            b.Image = element.Attribute.DisplayImage;
                            b.Size = b.Image.Size;
                            b.Height -= 2; // to remove the border line
                            b.Text = element.Attribute.DisplayText;
                        }
                    }
                    index++;
                }

                if (dt.Rows[0]["LanguageName"].ToString().Equals(savLanguage))
                    btnLanguage.Text = dt.Rows[1]["LanguageName"].ToString();
                else
                    btnLanguage.Text = dt.Rows[0]["LanguageName"].ToString();
            }
            log.LogMethodExit();
        }

        private void DisplayPartySize()
        {
            log.LogMethodEntry();
            showPartySize = Helper.GetShowPartySizeFlag();

            if (!showPartySize)
            {
                this.flowLayoutPanel1.Visible = false;
                this.panel1.Visible = false;
                this.flpDigits.Visible = false;
                //this.flowLayoutPanel1.Location = new System.Drawing.Point(196, 333);
                //this.flowLayoutPanel1.Size = new System.Drawing.Size(730, 564);
                this.flpOptions.Location = new System.Drawing.Point(0, 230);
                this.flpOptions.Size = new System.Drawing.Size(1080, 1012 + 564+70+44);

                this.Controls.Remove(this.lblNumberOfPeople);
                this.Controls.Remove(this.flowLayoutPanel1);
            }
            log.LogMethodExit();
        }
    }
}
