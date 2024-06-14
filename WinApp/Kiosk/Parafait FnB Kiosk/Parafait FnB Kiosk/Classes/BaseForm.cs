/********************************************************************************************
* Project Name - Parafait_Kiosk -BaseForm.cs
* Description  - BaseForm 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class BaseForm : Form
    {
        internal ScreenModel _screenModel;
        internal ScreenModel.UIPanelElement _callingElement;
        private System.Windows.Forms.Timer inactivityTimer;
        internal int SecondsRemaining;

        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void ResetTimeOut()
        {
            log.LogMethodEntry();
            SecondsRemaining = Common.TimeOutDuration;
            log.LogMethodExit();
        }

        public void InactivityTimerSwitch(bool swicthOnOff)
        {
            log.LogMethodEntry(swicthOnOff);
            inactivityTimer.Enabled = swicthOnOff;
            log.LogMethodExit();
        }

        public BaseForm()
        {
            log.LogMethodEntry();
            InitializeComponent();

            ResetTimeOut();

            this.components = new System.ComponentModel.Container();
            this.inactivityTimer = new System.Windows.Forms.Timer(this.components);
            this.inactivityTimer.Interval = 1000;
            this.inactivityTimer.Tick += new System.EventHandler(this.inactivityTimer_Tick);

            if (!(this is frmSplash))
                this.inactivityTimer.Enabled = true;

            this.FormClosed += delegate
            {
                inactivityTimer.Stop();
            };
            log.LogMethodExit();
        }

        internal virtual void inactivityTimer_Tick(object sender, EventArgs e)
        {            
            log.LogMethodEntry();
            if (this == ActiveForm)
            {
                if (SecondsRemaining <= 10)
                {
                    if (this is frmHome)
                    {
                        Close();
                    }
                    else if (!(this is frmCheckout && UserTransaction.OrderDetails.transactionPaymentsDTO.GatewayPaymentProcessed == true))
                    {
                        if (TimeOut.AbortTimeOut(this))
                            ResetTimeOut();
                        else
                        {
                            if (this is frmMenuChoiceBasic)
                                Common.GoHome();
                            else
                                Close();
                        }
                    }
                }

                SecondsRemaining--;
            }
            else
                ResetTimeOut();
            log.LogMethodExit();
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

        internal virtual void SetScreenModel(ScreenModel ScreenModel, ScreenModel.UIPanelElement CallingElement)
        {
            log.LogMethodEntry(ScreenModel, CallingElement);
            _screenModel = ScreenModel;
            _callingElement = CallingElement;
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

        public virtual void RenderPanelContent(ScreenModel _screenModel, Control Panel, int PanelIndex)
        {
            log.LogMethodEntry(_screenModel, Panel, PanelIndex);
            ScreenModel.UIPanel modelPanel = _screenModel.getPanelByIndex(PanelIndex);
            if (modelPanel != null)
            {
                if (modelPanel.PanelWidth >= 0)
                {
                    Panel.Width = modelPanel.PanelWidth;
                    Panel.Left = (Screen.PrimaryScreen.WorkingArea.Width - Panel.Width) / 2;
                    Panel.Left = (1080 - Panel.Width) / 2;
                }

                int index = 1;
                foreach (var pb in Panel.Controls.OfType<PictureBox>())
                {
                    ScreenModel.UIPanelElement element = modelPanel.getElementByIndex(index);
                    Panel.BackgroundImage = element.Attribute.DisplayImage;
                    Panel.BackgroundImageLayout = ImageLayout.None;
                    Panel.Height = element.Attribute.DisplayImage.Height;
                    (pb as PictureBox).Visible = false;
                    index++;

                    if (element.ActionScreenId != -1)
                    {
                        Panel.Click += delegate
                        {
                            if (ValidateAction(element))
                            {
                                Common.OpenScreen(element.Clone());
                            }

                            UpdateHeader();
                        };
                    }
                }

                foreach (Control c in Panel.Controls)
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
                Panel.Visible = false;
            log.LogMethodExit();
        }

        private void BaseForm_Activated(object sender, EventArgs e)
        {
            log.LogMethodExit();
            UpdateHeader();
            log.LogMethodExit();
        }
    }
}
