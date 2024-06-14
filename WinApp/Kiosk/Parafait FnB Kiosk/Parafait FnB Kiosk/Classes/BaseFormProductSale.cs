/********************************************************************************************
* Project Name - Parafait_Kiosk -BaseFormProductSale.cs
* Description  - BaseFormProductSale 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class BaseFormProductSale : BaseForm
    {
        public BaseFormProductSale()
        {
            log.LogMethodEntry();
            InitializeComponent();
            flpChoiceParameter.Visible = false;
            this.Load += BaseFormProductSale_Load;
            log.LogMethodExit();
        }

        void BaseFormProductSale_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (ScreenModel.ElementParameter param in _callingElement.Parameters)
            {
                if (param.Toplevel && param.DataSource.Rows.Count == 1)
                {
                    if (!Helper.CheckProductAvailability(param.DataSource.Rows[0][0]))
                    {
                        this.Opacity = 0;

                        Common.ShowMessage(Common.utils.MessageUtils.getMessage(1122, param.DataSource.Columns.Count > 1 ? param.DataSource.Rows[0][1].ToString() : param.DataSource.Rows[0][0].ToString()));
                        Close();
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        internal override void SetScreenModel(ScreenModel ScreenModel, ScreenModel.UIPanelElement CallingElement)
        {
            log.LogMethodEntry(ScreenModel, CallingElement);
            base.SetScreenModel(ScreenModel, CallingElement);

            if (!string.IsNullOrEmpty(CallingElement.Attribute.ActionScreenTitle1.Trim()))
                lblScreenTitle.Text = CallingElement.Attribute.ActionScreenTitle1;
            else
            {
                lblScreenTitle.Text = "";
                lblScreenTitle.Visible = false;
            }
            log.LogMethodExit();
        }

        internal void RenderChoiceButtonParameter(ScreenModel.ElementParameter Parameter, FlowLayoutPanel flpParameters)
        {
            log.LogMethodEntry(Parameter, flpParameters);
            FlowLayoutPanel flpParam = new FlowLayoutPanel();
            flpParam.Size = flpChoiceParameter.Size;
            flpParam.Margin = flpChoiceParameter.Margin;

            Label lbldisplay1 = new Label();
            lbldisplay1.AutoSize = false;
            lbldisplay1.Size = lblChoiceDisplayText1.Size;
            lbldisplay1.Font = lblChoiceDisplayText1.Font;
            lbldisplay1.ForeColor = lblChoiceDisplayText1.ForeColor;
            lbldisplay1.TextAlign = lblChoiceDisplayText1.TextAlign;
            lbldisplay1.Text = Parameter.DisplayText1;

            Panel panelParamSelection = new Panel();
            panelParamSelection.Size = panelChoiceParameter.Size;
            panelParamSelection.BackgroundImage = panelChoiceParameter.BackgroundImage;
            panelParamSelection.BackgroundImageLayout = panelChoiceParameter.BackgroundImageLayout;
            panelParamSelection.Margin = panelChoiceParameter.Margin;

            RadioButton btnChoiceFalse = new RadioButton();
            RadioButton btnChoiceTrue = new RadioButton();
            btnChoiceFalse.Appearance = btnChoiceTrue.Appearance = Appearance.Button;
            btnChoiceFalse.FlatStyle = btnChoiceTrue.FlatStyle = FlatStyle.Flat;
            btnChoiceTrue.FlatAppearance.BorderSize = btnChoiceFalse.FlatAppearance.BorderSize = 0;
            btnChoiceFalse.FlatAppearance.CheckedBackColor = btnChoiceTrue.FlatAppearance.CheckedBackColor =
            btnChoiceFalse.FlatAppearance.MouseDownBackColor = btnChoiceTrue.FlatAppearance.MouseDownBackColor =
                btnChoiceFalse.FlatAppearance.MouseOverBackColor = btnChoiceTrue.FlatAppearance.MouseOverBackColor = btnChoiceTrueSample.FlatAppearance.MouseDownBackColor;

            btnChoiceTrue.Size = btnChoiceFalse.Size = btnChoiceFalseSample.Size;
            btnChoiceFalse.Image =
            btnChoiceTrue.Image = btnChoiceFalseSample.Image;
            btnChoiceFalse.Font = btnChoiceTrue.Font = btnChoiceTrueSample.Font;

            btnChoiceTrue.TextAlign = btnChoiceFalse.TextAlign = btnChoiceFalseSample.TextAlign;

            btnChoiceFalse.Location = btnChoiceFalseSample.Location;
            btnChoiceTrue.Location = btnChoiceTrueSample.Location;

            if (Parameter.DataSource.Columns.Count > 1)
            {
                if (Parameter.DataSource.Rows[0][0] == DBNull.Value)
                {
                    btnChoiceFalse.Text = Parameter.DataSource.Rows[0][1].ToString();
                    btnChoiceTrue.Text = Parameter.DataSource.Rows[1][1].ToString();
                }
                else
                {
                    btnChoiceFalse.Text = Parameter.DataSource.Rows[1][1].ToString();
                    btnChoiceTrue.Text = Parameter.DataSource.Rows[0][1].ToString();
                }
            }
            else
            {
                btnChoiceFalse.Text = btnChoiceFalseSample.Text;
                btnChoiceTrue.Text = btnChoiceTrueSample.Text;
            }

            btnChoiceFalse.CheckedChanged += delegate
            {
                base.ResetTimeOut();
                if (btnChoiceTrue.Checked)
                {
                    btnChoiceTrue.Image = Properties.Resources.Green_Btn;
                    btnChoiceFalse.Image = Properties.Resources.White_Btn;
                }
                else
                {
                    btnChoiceFalse.Image = Properties.Resources.Green_Btn;
                    btnChoiceTrue.Image = Properties.Resources.White_Btn;
                }

                Parameter.UserSelectedEvent(btnChoiceTrue.Checked ? (Parameter.DataSource.Rows[0][0] == DBNull.Value ? Parameter.DataSource.Rows[1][0] : Parameter.DataSource.Rows[0][0]) : DBNull.Value);
                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            };

            btnChoiceTrue.CheckedChanged += delegate
            {
                base.ResetTimeOut();
                if (btnChoiceTrue.Checked)
                {
                    btnChoiceTrue.Image = Properties.Resources.Green_Btn;
                    btnChoiceFalse.Image = Properties.Resources.White_Btn;
                }
                else
                {
                    btnChoiceFalse.Image = Properties.Resources.Green_Btn;
                    btnChoiceTrue.Image = Properties.Resources.White_Btn;
                }

                Parameter.UserSelectedEvent(btnChoiceTrue.Checked ? (Parameter.DataSource.Rows[0][0] == DBNull.Value ? Parameter.DataSource.Rows[1][0] : Parameter.DataSource.Rows[0][0]) : DBNull.Value);
                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            };

            if (Parameter.OrderedValue == DBNull.Value)
                btnChoiceFalse.Checked = true;
            else if (Parameter.UserSelectedValue != null)
                btnChoiceTrue.Checked = true;

            panelParamSelection.Controls.Add(btnChoiceFalse);
            panelParamSelection.Controls.Add(btnChoiceTrue);

            flpParam.Controls.Add(lbldisplay1);
            flpParam.Controls.Add(panelParamSelection);

            flpParameters.Height += flpParam.Height + flpParam.Margin.Top * 2 + flpParam.Margin.Bottom * 2;
            flpParameters.Controls.Add(flpParam);
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        public virtual bool ValidateItemAddition()
        {
            log.LogMethodEntry();
            log.LogMethodExit(true);
            return true;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            AddItem();
            log.LogMethodExit();
        }

        public virtual void AddItem()
        {
            log.LogMethodEntry();
            if (ValidateItemAddition())
            {
                foreach (ScreenModel.ElementParameter parameter in _callingElement.AllParameters)
                {
                    parameter.OrderedValue = parameter.UserSelectedValue;
                    parameter.OrderedQuantity = parameter.UserQuantity;
                }

                UserTransaction.OrderDetails.AddItem(_callingElement);

                UserTransaction.getOrderTotal();

                Close();
            }
            log.LogMethodExit();
        }
    }
}
