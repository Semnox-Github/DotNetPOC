/********************************************************************************************
* Project Name - Parafait_Kiosk -frmProductSaleBasic.cs
* Description  - frmProductSaleBasic 
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
    public partial class frmProductSaleBasic : BaseFormProductSale
    {
        public frmProductSaleBasic()
        {
            log.LogMethodEntry();
            InitializeComponent();
            flpComboParameterSample.Visible = false;
            flpParameters.Height = 0;
            log.LogMethodExit();
        }

        private void frmProductSaleBasic_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                RenderAllParameters();
                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }

        public void RenderAllParameters()
        {
            log.LogMethodEntry();
            foreach (ScreenModel.ElementParameter parameter in _callingElement.VisibleParameters)
            {
                if (parameter.UIType == ScreenModel.ParameterUIType.Default)
                    RenderComboParameter(parameter);
                else if (parameter.UIType == ScreenModel.ParameterUIType.ChoiceButton)
                    RenderChoiceButtonParameter(parameter, flpParameters);
            }

            foreach (ScreenModel.ElementParameter parameter in _callingElement.VisibleParameters)
            {
                if (parameter.Toplevel && parameter.UIType == ScreenModel.ParameterUIType.Default)
                {
                    if (parameter.OrderedValue != null)
                        parameter.UIControl.SelectedValue = parameter.OrderedValue;
                }
            }

            foreach (ScreenModel.ElementParameter parameter in _callingElement.VisibleParameters)
            {
                if (parameter.Toplevel == false)
                {
                    if (parameter.UIType == ScreenModel.ParameterUIType.Default)
                    {
                        if (parameter.OrderedValue != null)
                            parameter.UIControl.SelectedValue = parameter.OrderedValue;
                    }
                }
            }

            if (_callingElement.VisibleParameters.Count > 2)
                this.Height += (_callingElement.Parameters.Count - 2) * flpComboParameterSample.Height;

            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            btnClose.BringToFront();

            if (lblScreenTitle.Visible == false)
                flpParameters.Top = lblScreenTitle.Top;

            if (_callingElement.VisibleParameters.Count > 3)
                panelBG.BackgroundImage = Properties.Resources.ProductSalePopUp3;
            else if (_callingElement.VisibleParameters.Count > 2)
                panelBG.BackgroundImage = Properties.Resources.ProductSalePopUp2;
            else
                panelBG.BackgroundImage = Properties.Resources.ProductSalePopUp1;
            log.LogMethodExit();
        }

        internal void RenderComboParameter(ScreenModel.ElementParameter Parameter)
        {
            log.LogMethodEntry(Parameter);
            FlowLayoutPanel flpParam = new FlowLayoutPanel();
            flpParam.Size = flpComboParameterSample.Size;
            flpParam.Margin = flpComboParameterSample.Margin;

            Label lbldisplay1 = new Label();
            lbldisplay1.AutoSize = false;
            lbldisplay1.Size = lblDisplayText1.Size;
            lbldisplay1.Font = lblDisplayText1.Font;
            lbldisplay1.ForeColor = lblDisplayText1.ForeColor;
            lbldisplay1.TextAlign = lblDisplayText1.TextAlign;
            lbldisplay1.Text = Parameter.DisplayText1;

            Panel panelParamSelection = new Panel();
            panelParamSelection.Size = panelParameterSelection.Size;
            panelParamSelection.BackgroundImage = panelParameterSelection.BackgroundImage;
            panelParamSelection.BackgroundImageLayout = panelParameterSelection.BackgroundImageLayout;
            panelParamSelection.Margin = panelParameterSelection.Margin;

            ComboBox cmbParam = new ComboBox();
            cmbParam.Name = Parameter.ParameterName;
            cmbParam.Size = cmbParameter.Size;
            cmbParam.DropDownStyle = cmbParameter.DropDownStyle;
            cmbParam.Location = cmbParameter.Location;
            cmbParam.FlatStyle = cmbParameter.FlatStyle;
            cmbParam.Font = cmbParameter.Font;
            cmbParam.ForeColor = cmbParameter.ForeColor;
            panelParamSelection.Controls.Add(cmbParam);
            Parameter.UIControl = cmbParam;

            Label lblComboDisplay = new Label();
            lblComboDisplay.AutoSize = false;
            lblComboDisplay.Size = lblComboDispSample.Size;
            lblComboDisplay.Font = lblComboDispSample.Font;
            lblComboDisplay.ForeColor = lblComboDispSample.ForeColor;
            lblComboDisplay.TextAlign = lblComboDispSample.TextAlign;
            lblComboDisplay.Location = lblComboDispSample.Location;
            lblComboDisplay.Image = lblComboDispSample.Image;
            panelParamSelection.Controls.Add(lblComboDisplay);
            lblComboDisplay.BringToFront();

            lblComboDisplay.Click += delegate
            {
                cmbParam.DroppedDown = true;
            };

            flpParam.Controls.Add(lbldisplay1);
            flpParam.Controls.Add(panelParamSelection);

            if (!string.IsNullOrEmpty(Parameter.DisplayText2.Trim()))
            {
                Label lbldisplay2 = new Label();
                lbldisplay2.AutoSize = false;
                lbldisplay2.Size = lblDisplayText2.Size;
                lbldisplay2.Font = lblDisplayText2.Font;
                lbldisplay2.ForeColor = lblDisplayText2.ForeColor;
                lbldisplay2.TextAlign = lblDisplayText2.TextAlign;
                lbldisplay2.Text = Parameter.DisplayText2;

                flpParam.Controls.Add(lbldisplay2);
            }

            cmbParam.SelectedIndexChanged += delegate
            {
                Parameter.UserSelectedEvent(cmbParam.SelectedValue);
                lblComboDisplay.Text = cmbParam.Text;

                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            };

            flpParameters.Height += flpParam.Height + flpParam.Margin.Top + flpParam.Margin.Bottom;
            flpParameters.Controls.Add(flpParam);

            cmbParam.ValueMember = Parameter.DataSource.Columns[0].ColumnName;
            if (Parameter.DataSource.Columns.Count > 1)
                cmbParam.DisplayMember = Parameter.DataSource.Columns[1].ColumnName;
            else
                cmbParam.DisplayMember = cmbParam.ValueMember;

            cmbParam.DataSource = Parameter.DataSource;
            cmbParam.Height = Math.Max(Parameter.DataSource.Rows.Count * cmbParam.ItemHeight, cmbParam.ItemHeight);
            log.LogMethodExit();
        }
    }
}
