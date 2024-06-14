/********************************************************************************************
 * Project Name - Common
 * Description  - UI Class for frmParafaitMessageBox
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019     Girish Kundar        Modified : Added Logger methods and Removed unused namespace's 
 *********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    public partial class frmParafaitMessageBox : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public frmParafaitMessageBox(string message, string Title, MessageBoxButtons msgboxButtons)
        {
            Logger.setRootLogLevel(log);
            log.LogMethodEntry(message, Title);
            InitializeComponent();
            lblTitle.Text = Title;
            try
            {
                btnYes.Text = POSStatic.Utilities.MessageUtils.getMessage("Yes");
                btnNo.Text = POSStatic.Utilities.MessageUtils.getMessage("No");
                btnCancel.Text = POSStatic.Utilities.MessageUtils.getMessage("Cancel");
            }
            catch (Exception)
            {
                btnYes.Text = "&Yes";
                btnNo.Text = "&No";
                btnCancel.Text = "&Cancel";
            }
            if (string.IsNullOrEmpty(message) == false && message.Length > 200)
            {
                lblMessage.MaximumSize = new Size(520, 0);
                panel1.HorizontalScroll.Maximum = 0;
                this.panel1.AutoScroll = true;
                panel1.HorizontalScroll.Enabled = false;
                panel1.HorizontalScroll.Visible = false;
                panel1.VerticalScroll.Enabled = true;
                panel1.VerticalScroll.Visible = true;
                lblMessage.AutoSize = true;
                lblMessage.Text = message;
                lblMessage.TextAlign = ContentAlignment.MiddleCenter;
                this.panel1.Height = 150;
                this.Height = 250;
                this.btnYes.Location = new Point(this.btnYes.Location.X, this.panel1.Height + 50);
                this.btnNo.Location = new Point(this.btnNo.Location.X, this.panel1.Height + 50);
                this.btnCancel.Location = new Point(this.btnCancel.Location.X, this.panel1.Height + 50);
            }
            else
            {
                lblMessage.Text = message;
                lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            }
            if (msgboxButtons == MessageBoxButtons.YesNoCancel)
            {
            }
            else if (msgboxButtons == MessageBoxButtons.OK)
            {
                btnNo.Visible = btnCancel.Visible = false;
                btnYes.Left = (this.Width - btnYes.Width) / 2;
                try
                {
                    btnYes.Text = POSStatic.Utilities.MessageUtils.getMessage("OK");
                }
                catch
                {
                    btnYes.Text = "OK";
                }
                this.CancelButton = btnYes;
            }
            else
            {
                btnNo.Left = btnCancel.Left;
                btnCancel.Visible = false;

                btnYes.Left += 50;
                btnNo.Left -= 50;
            }

            this.TopMost = this.TopLevel = true;
            log.LogMethodExit();
        }

        public frmParafaitMessageBox(string message, string Title, MessageBoxButtons msgboxButtons, string yesBtnText, string noBtnText, string cancelBtnText = "")
            : this(message, Title, msgboxButtons)
        {
            log.LogMethodEntry(message, Title, msgboxButtons, yesBtnText, noBtnText, cancelBtnText);
            if (msgboxButtons == MessageBoxButtons.YesNo || msgboxButtons == MessageBoxButtons.YesNoCancel)
            {
                if (string.IsNullOrWhiteSpace(yesBtnText) == false)
                {
                    btnYes.Text = yesBtnText;
                }
                if (string.IsNullOrWhiteSpace(noBtnText) == false)
                {
                    btnNo.Text = noBtnText;
                }
                if (string.IsNullOrWhiteSpace(cancelBtnText) == false)
                {
                    btnCancel.Text = noBtnText;
                } 
            }
            log.LogMethodExit();
        }
        private void btnYes_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private void btnYes_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }
    }
}
