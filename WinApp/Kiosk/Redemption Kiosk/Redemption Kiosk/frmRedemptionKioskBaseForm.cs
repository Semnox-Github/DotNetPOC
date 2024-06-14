/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Here we define timer and redemption order object.
 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
 
using System;
using System.Linq;
using System.Windows.Forms;
using Semnox.Parafait.Redemption;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskBaseForm : Form
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ScreenModel _screenModel;
        internal ScreenModel.UIPanelElement _callingElement;
        private System.Windows.Forms.Timer inactivityTimer;
        internal int SecondsRemaining;
        internal static RedemptionBL redemptionOrder; 
        public virtual void ResetTimeOut()
        {
            log.LogMethodEntry();
            SecondsRemaining = Common.TimeOutDuration;
            log.LogMethodExit(SecondsRemaining); 
        }

        public void InactivityTimerSwitch(bool swicthOnOff)
        {
            log.LogMethodEntry(swicthOnOff);
            inactivityTimer.Enabled = swicthOnOff;
            log.LogMethodExit();
        }

        public frmRedemptionKioskBaseForm()
        {
            log.LogMethodEntry();
            InitializeComponent();

            ResetTimeOut();

            this.components = new System.ComponentModel.Container();
            this.inactivityTimer = new System.Windows.Forms.Timer(this.components);
            this.inactivityTimer.Interval = 1000;
            this.inactivityTimer.Tick += new System.EventHandler(this.InactivityTimer_Tick);

            if (!(this is frmRedemptionKioskSplashScreen))
            {
                this.inactivityTimer.Enabled = true;
            }

            this.FormClosed += delegate
            {
                inactivityTimer.Stop();
            };
            log.LogMethodExit();
        }

        public void SetKioskTimerSecondsValue(int secRemaining)
        {
            log.LogMethodEntry(secRemaining);
            SecondsRemaining = secRemaining;
            log.LogMethodExit();
        }

        public int GetKioskTimerSecondsValue()
        {
            log.LogMethodEntry();
            log.LogMethodExit(SecondsRemaining);
            return SecondsRemaining;
        }

        internal virtual void InactivityTimer_Tick(object sender, EventArgs e)
        {
            if (this == ActiveForm)
            {
                if (SecondsRemaining <= 10)
                {
                    if (this is frmRedemptionKioskHomeScreen)
                    {
                        if (SecondsRemaining <= 0)
                        {
                            log.Debug("SecondsRemaining " + SecondsRemaining);
                            ClearOrderProcess();
                            Close();
                        }
                    }
                    else if (TimeOut.AbortTimeOut(this))
                    {
                        ResetTimeOut();
                    }
                    else
                    {
                        ClearOrderProcess();
                        Common.GoHome();
                    }
                }
                SecondsRemaining--;
            }
            else
            {
                ResetTimeOut();
            }
        }

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the table layout flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        internal virtual void SetScreenModel(ScreenModel screenModel, ScreenModel.UIPanelElement callingElement)
        {
            log.LogMethodEntry(screenModel, callingElement);
            _screenModel = screenModel;
            _callingElement = callingElement;
            log.LogMethodExit();
        }

        internal virtual bool ValidateAction(ScreenModel.UIPanelElement element)
        {
            log.LogMethodEntry(element);
            log.LogMethodExit(true);
            return true;
        }

        public virtual void UpdateHeader()
        {
        }

        public virtual void RenderPanelContent(ScreenModel _screenModel, Control panel, int panelIndex)
        {
            log.LogMethodEntry(_screenModel, panel, panelIndex);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ScreenModel.UIPanel modelPanel = _screenModel.GetPanelByIndex(panelIndex);
                if (modelPanel != null)
                {
                    if (modelPanel.PanelWidth >= 0)
                    {
                        panel.Width = modelPanel.PanelWidth;
                        panel.Left = (Screen.PrimaryScreen.WorkingArea.Width - panel.Width) / 2;
                        panel.Left = (1080 - panel.Width) / 2;
                    }

                    int index = 1;
                    foreach (var pb in panel.Controls.OfType<PictureBox>())
                    {
                        ScreenModel.UIPanelElement element = modelPanel.getElementByIndex(index);
                        panel.BackgroundImage = element.Attribute.DisplayImage;
                        panel.BackgroundImageLayout = ImageLayout.None;
                        panel.Height = element.Attribute.DisplayImage.Height;
                        (pb as PictureBox).Visible = false;
                        index++;

                        if (element.ActionScreenId != -1)
                        {
                            panel.Click += delegate
                            {
                                if (ValidateAction(element))
                                {
                                    Common.OpenScreen(element.Clone());
                                }

                                UpdateHeader();
                            };
                        }
                    }

                    foreach (Control c in panel.Controls)
                    {
                        if (c is Panel || c is FlowLayoutPanel)
                        {
                            foreach (var pb in c.Controls.OfType<PictureBox>())
                            {
                                ScreenModel.UIPanelElement panelElement = modelPanel.getElementByIndex(index);
                                c.BackgroundImage = panelElement.Attribute.DisplayImage;
                                c.BackgroundImageLayout = ImageLayout.None;
                                c.Height = panelElement.Attribute.DisplayImage.Height - 2;
                                c.Width = panelElement.Attribute.DisplayImage.Width;
                                (pb as PictureBox).Visible = false;
                                index++;
                                break;
                            }
                        }
                        else if (c is Button)
                        {
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
                                    //b.Tag = element.Parameters[0].UserSelectedValue;
                                }

                                if (element.ActionScreenId != -1)
                                {
                                    c.Click += delegate
                                    {
                                        if (this._callingElement == null || this._callingElement.UIPanelElementId != element.UIPanelElementId)
                                        {
                                            if (ValidateAction(element))
                                            {
                                                Common.OpenScreen(element.Clone());
                                            }
                                            UpdateHeader();
                                        }
                                    };
                                }
                            }
                            else
                            {
                                c.Visible = false;
                            }
                            index++;
                        }
                    }
                }
                else
                {
                    panel.Visible = false;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit(null);
        }

        private void BaseForm_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateHeader();
            log.LogMethodExit();
        }
        public virtual void StartOver()
        {
            log.LogMethodEntry();
            try
            {
                ResetTimeOut();
                //"Are You sure want to exit? "
                if (Common.ShowDialog(Common.utils.MessageUtils.getMessage(1611)) == System.Windows.Forms.DialogResult.Yes)
                {
                    ClearOrderProcess();
                    Common.GoHome();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public void ClearOrderProcess()
        {
            log.LogMethodEntry();
            if (redemptionOrder != null)
            {
                redemptionOrder = null;
            }
            log.LogMethodExit();
        } 
    }
}
