/********************************************************************************************
* Project Name - Parafait_Kiosk -frmProductSalePizza.cs
* Description  - frmProductSalePizza 
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
    public partial class frmProductSalePizza : BaseFormProductSale
    {
        public frmProductSalePizza()
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.lblToppingTitleRight.Text = Common.utils.MessageUtils.getMessage("Right Side Topping");
            lblToppingTitleLeft.Text = Common.utils.MessageUtils.getMessage("Topping");
            log.LogMethodExit();
        }

        bool FormLoaded = false;
        bool halfHalfAllowed = true;
        internal void frmProductSalePizza_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Height = panelBG.BackgroundImage.Height;

                // value meal
                if (lblScreenTitle.Text.Length > 30)
                {
                    lblScreenTitle.Top = btnClose.Top - 10;
                    lblScreenTitle.Height += lblScreenTitle.Height * 2;
                    flpParameters.Top += 30;
                }

                // when loaded for edit

                if (_callingElement.RightParameters.Count > 0
                    || _callingElement.LeftParameters.Count > 0)
                    CheckHalfHalfAllowed();

                if (_callingElement.RightParameters.Count > 0)
                {
                    rbHalfHalf.Checked = true;
                    flpRightSide.Visible = true;
                    rbHalfHalf.Image = Properties.Resources.Green_Btn;
                    rbWhole.Image = Properties.Resources.White_Btn;
                    lblToppingTitleLeft.Text = Common.utils.MessageUtils.getMessage("Left Side Topping");
                }
                else if (_callingElement.LeftParameters.Count > 0)
                {
                    rbHalfHalf.Checked = false;
                    flpRightSide.Visible = false;
                    rbHalfHalf.Image = Properties.Resources.White_Btn;
                    rbWhole.Image = Properties.Resources.Green_Btn;
                }

                RenderAllParameters();
                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);

                this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

                FormLoaded = true;
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
            if (_callingElement.Parameters.Count == 4)
            {
                frmUpsellProduct fup = null;

                //Common.utils.MessageUtils.getMessage(417, Convert.ToDouble(dt.Rows[0]["Price"]).ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL))

                if (_callingElement.Parameters[3].DataSource != null && _callingElement.Parameters[3].DataSource.Rows.Count > 1 && _callingElement.Parameters[3].DisplayImage != null)
                {
                    fup = new frmUpsellProduct("", "", "", _callingElement.Parameters[3].DisplayImage);
                    DialogResult dr = fup.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        _callingElement.Parameters[3].OrderedQuantity = 1;
                        _callingElement.Parameters[3].OrderedValue = _callingElement.Parameters[3].UserSelectedValue = _callingElement.Parameters[3].DataSource.Rows[1][0];
                    }
                    else
                    {
                        _callingElement.Parameters[3].OrderedQuantity = 0;
                        _callingElement.Parameters[3].OrderedValue = _callingElement.Parameters[3].UserSelectedValue = _callingElement.Parameters[3].DataSource.Rows[0][0];
                    }
                }
            }
            CallRenderParameter();
            RenderModifiers();

            if (_callingElement.Attribute.ActionScreenTitle2.Trim() != "")
                lblScreenTitle2.Text = _callingElement.Attribute.ActionScreenTitle2;
            else
                lblScreenTitle2.Text = Common.utils.MessageUtils.getMessage("Customize");

            if (_callingElement.Attribute.ActionScreenFooter1.Trim() != "")
            {
                lblScreenFooter1.Text = _callingElement.Attribute.ActionScreenFooter1;
            }

            if (_callingElement.Attribute.ActionScreenFooter2.Trim() != "")
            {
                lblScreenFooter2.Text = _callingElement.Attribute.ActionScreenFooter2;
            }
            log.LogMethodExit();
        }

        internal void CallRenderParameter()
        {
            if (_callingElement.VisibleParameters.Count == 0)
            {
                log.LogMethodExit();
                return;
            }

            ScreenModel.ElementParameter Parameter1;
            ScreenModel.ElementParameter Parameter2 = null;

            if (_callingElement.VisibleParameters.Count > 2) // not value meal
            {
                if (_callingElement.VisibleParameters[0].ParentParameter == null)
                {
                    Parameter1 = _callingElement.VisibleParameters[0];
                    Parameter2 = _callingElement.VisibleParameters[1];
                }
                else
                {
                    Parameter1 = _callingElement.VisibleParameters[1];
                    Parameter2 = _callingElement.VisibleParameters[0];
                }
            }
            else // value meal
            {
                Parameter1 = _callingElement.VisibleParameters[0];
                Parameter2 = null;

                flpParameter2.Visible = false;
                flpParameters.Height -= (flpParameter2.Height + flpParameter2.Margin.Top + flpParameter2.Margin.Bottom);
            }

            FlowLayoutPanel flpParameter;
            Label label1;
            Label label2;
            if (Parameter1.DisplayIndex == 1)
            {
                flpParameter = flpParameter1;
                label1 = lblDisplayText11;
                label2 = lblDisplayText12;
            }
            else
            {
                flpParameter = flpParameter2;
                label1 = lblDisplayText21;
                label2 = lblDisplayText22;
            }

            RenderButtonParameter(Parameter1, flpParameter, label1, label2, Parameter2);
            log.LogMethodExit();
        }

        bool disableModifierRender = false;
        internal void RenderButtonParameter(ScreenModel.ElementParameter Parameter, FlowLayoutPanel flpParameter, Label label1, Label label2, ScreenModel.ElementParameter Parameter2)
        {
            log.LogMethodEntry(Parameter, flpParameter, label1, label2, Parameter2);
            RadioButton rb1;
            flpParameter.Controls.Clear();
            flpParameter.Controls.Add(label1);
            RadioButton rbFirst = null;
            for (int i = 0; i < Parameter.DataSource.Rows.Count && i < 4; i++)
            {
                rb1 = new RadioButton();
                rb1.Appearance = this.rbtnSample1.Appearance = System.Windows.Forms.Appearance.Button;
                rb1.BackColor = this.rbtnSample1.BackColor = System.Drawing.Color.Transparent;
                rb1.BackgroundImageLayout = this.rbtnSample1.BackgroundImageLayout;
                rb1.FlatAppearance.BorderSize = this.rbtnSample1.FlatAppearance.BorderSize;
                rb1.FlatAppearance.CheckedBackColor = this.rbtnSample1.FlatAppearance.CheckedBackColor;
                rb1.FlatAppearance.MouseDownBackColor = this.rbtnSample1.FlatAppearance.MouseDownBackColor;
                rb1.FlatAppearance.MouseOverBackColor = this.rbtnSample1.FlatAppearance.MouseOverBackColor;
                rb1.FlatStyle = this.rbtnSample1.FlatStyle;
                rb1.Font = this.rbtnSample1.Font;
                rb1.ForeColor = this.rbtnSample1.ForeColor;
                rb1.Image = this.rbtnSample1.Image;
                rb1.Margin = this.rbtnSample1.Margin;
                rb1.Name = "rbtnSample" + i;
                rb1.Size = this.rbtnSample1.Size;
                if (Parameter.DataSource.Columns.Count >= 2)
                {
                    rb1.Text = Parameter.DataSource.Rows[i][1].ToString();
                }
                else
                {
                    rb1.Text = Parameter.DataSource.Rows[i][0].ToString();
                }
                rb1.Tag = Parameter.DataSource.Rows[i][0];
                rb1.TextAlign = this.rbtnSample2.TextAlign;
                rb1.UseVisualStyleBackColor = this.rbtnSample2.UseVisualStyleBackColor;
                rb1.Click += (sen, arg) =>
                {
                    RadioButton rbSelected;
                    rbSelected = (RadioButton)sen;
                    base.ResetTimeOut();
                    if (rbSelected.Checked)
                    {
                        foreach (Control c in flpParameter.Controls)
                        {
                            if (c.GetType().ToString().ToLower().Contains("radiobutton"))
                            {
                                ((RadioButton)c).Image = Properties.Resources.White_Btn;
                            }
                        }
                        rbSelected.Image = Properties.Resources.Green_Btn;
                        if (Parameter.Toplevel)
                            disableModifierRender = true;

                        Parameter.UserSelectedEvent(rbSelected.Tag);

                        if (Parameter.Toplevel)
                            disableModifierRender = false;

                        // main product parameter or half-half is determined at crust level
                        if ((Parameter.Toplevel || Parameter.DataSource.Columns.Count > 5) && FormLoaded && !disableModifierRender)
                        {
                            _callingElement.LeftParameters.Clear();
                            _callingElement.RightParameters.Clear();
                            FormLoaded = false; // to disable calculating trx amount for each modifier
                            RenderModifiers();
                            FormLoaded = true;
                        }

                        lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
                        if (Parameter2 != null)
                        {
                            if (Parameter2.DisplayIndex == 1)
                            {
                                RenderButtonParameter(Parameter2, flpParameter1, lblDisplayText11, lblDisplayText12, null);
                            }
                            else
                            {
                                RenderButtonParameter(Parameter2, flpParameter2, lblDisplayText21, lblDisplayText22, null);
                            }
                        }
                    }
                };

                if ((Parameter.DataSource.Rows.Count == 3 && i == 2) || (Parameter.DataSource.Rows.Count == 1 && i == 0))
                {
                    if (i == 2 || i == 0)
                    {
                        rb1.Margin = new Padding((flpParameter.Width - rb1.Width) / 2, rbtnSample1.Margin.Top, rbtnSample1.Margin.Right, rbtnSample1.Margin.Bottom);
                    }
                }

                flpParameter.Controls.Add(rb1);

                if (i == 0)
                {
                    rbFirst = rb1;
                    rbFirst.Image = Properties.Resources.Green_Btn;
                }
            }
            
            if (Parameter.OrderedValue != null)
            {
                foreach (Control c in flpParameter.Controls)
                {
                    if (Parameter.OrderedValue.Equals(c.Tag))
                    {
                        (c as RadioButton).PerformClick();
                        break;
                    }
                }
            }
            else if (rbFirst != null)
            {
                rbFirst.PerformClick();
            }

            label1.Text = Parameter.DisplayText1;
            label2.Text = Parameter.DisplayText2;
            if (string.IsNullOrEmpty(label2.Text))
            {
                if (flpParameter.Tag == null)
                {
                    flpParameter.Height -= label2.Height;
                    flpParameters.Height -= label2.Height;
                    flpParameter.Tag = 1; // flag as done so that height is not reduced again on re-render
                }
            }
            else
                flpParameter.Controls.Add(label2);
            log.LogMethodExit();
        }
        
        void CheckHalfHalfAllowed()
        {
            log.LogMethodEntry();
            ScreenModel.ElementParameter parameter = _callingElement.VisibleParameters[_callingElement.VisibleParameters.Count - 1]; // last parameter

            //check if half-half is disabled for the parent product (3rd column is 0)
            if (parameter.ParentParameter != null
                && parameter.ParentParameter.DataSource.Columns.Count > 2
                && parameter.ParentParameter.DataSource.Rows.Count > 0)
            {
                foreach (DataRow dr in parameter.ParentParameter.DataSource.Rows)
                {
                    if (dr[0].Equals(parameter.ParentParameter.UserSelectedValue)
                        && dr[2].ToString() == "0")
                    {
                        halfHalfAllowed = false;
                        break;
                    }
                }
            }

            //check if half-half is disabled for the selected modifier (like crust) (5rd column is 0)
            if (halfHalfAllowed)
            {
                if (parameter.ParentParameter != null)
                {
                    foreach (ScreenModel.ElementParameter param in _callingElement.VisibleParameters.FindAll(x => parameter.ParentParameter.Equals(x.ParentParameter)))
                    {
                        if (param.Equals(parameter))
                            continue;

                        if (param.DataSource.Columns.Count > 5)
                        {
                            foreach (DataRow dr in param.DataSource.Rows)
                            {
                                if (dr[0].Equals(param.UserSelectedValue)
                                    && dr[5].ToString() == "0")
                                {
                                    halfHalfAllowed = false;
                                    break;
                                }
                            }
                            if (!halfHalfAllowed)
                                break;
                        }
                    }
                }
            }

            if (halfHalfAllowed)
            {
                flpHalfHalf.Visible = true;
                if (rbHalfHalf.Checked)
                {
                    flpRightSide.Visible = true;
                    lblToppingTitleLeft.Text = Common.utils.MessageUtils.getMessage("Left Side Topping");
                }
                else
                {
                    flpRightSide.Visible = false;
                    rbWhole.Checked = true; 
                    rbWhole.Image = Properties.Resources.Green_Btn;
                    rbHalfHalf.Image = Properties.Resources.White_Btn;
                    lblToppingTitleLeft.Text = Common.utils.MessageUtils.getMessage("Topping");
                }
            }
            else
            {
                rbHalfHalf.Checked = false;
                rbHalfHalf_CheckedChanged(rbHalfHalf, null);
                flpHalfHalf.Visible = false;
                flpRightSide.Visible = false;
                lblToppingTitleLeft.Text = Common.utils.MessageUtils.getMessage("Topping");
            }
            log.LogMethodExit();
        }

        void RenderModifiers()
        {
            log.LogMethodEntry();
            halfHalfAllowed = true;
            if (_callingElement.LeftParameters.Count == 0 && _callingElement.VisibleParameters.Count > 0) // fresh load
            {
                CheckHalfHalfAllowed();

                ScreenModel.ElementParameter parameter = _callingElement.VisibleParameters[_callingElement.VisibleParameters.Count - 1]; // last parameter

                if (parameter.DataSource.Rows.Count > 0)
                {
                    foreach (DataRow dr in parameter.DataSource.Rows)
                    {
                        ScreenModel.ElementParameter clone = parameter.Clone();
                        parameter.OwningElement = _callingElement;
                        clone.Parameter = "select * from (" + parameter.Parameter
                                                + ") v where " + parameter.DataSource.Columns[0].ColumnName
                                                + " = " + dr[0].ToString();
                        clone.getDataSource();
                        clone.identifier = clone.ParameterId.ToString() + dr[0].ToString();

                        _callingElement.LeftParameters.Add(clone);
                    }

                    if (rbHalfHalf.Checked)
                    {
                        foreach (DataRow dr in parameter.DataSource.Rows)
                        {
                            ScreenModel.ElementParameter clone = parameter.Clone();
                            parameter.OwningElement = _callingElement;
                            clone.Parameter = "select * from (" + parameter.Parameter
                                                    + ") v where " + parameter.DataSource.Columns[0].ColumnName
                                                    + " = " + dr[0].ToString();
                            clone.getDataSource();
                            clone.identifier = clone.ParameterId.ToString() + dr[0].ToString();

                            _callingElement.RightParameters.Add(clone);
                        }
                    }
                }
            }

            flpParams1.Controls.Clear();
            foreach (ScreenModel.ElementParameter parameter in _callingElement.LeftParameters)
            {
                RenderParameter(parameter, flpParams1);
            }

            flpParamsRight.Controls.Clear();
            foreach (ScreenModel.ElementParameter parameter in _callingElement.RightParameters)
            {
                RenderParameter(parameter, flpParamsRight);
            }
            log.LogMethodExit();
        }

        internal void RenderParameter(ScreenModel.ElementParameter Parameter, FlowLayoutPanel flpPanel)
        {
            log.LogMethodEntry(Parameter, flpPanel);
            FlowLayoutPanel flpModifier = new FlowLayoutPanel();
            flpModifier.Size = flpModifierSample.Size;
            flpModifier.Margin = flpModifierSample.Margin;

            CheckBox chkParam = new CheckBox();
            chkParam.FlatStyle = FlatStyle.Flat;
            chkParam.FlatAppearance.BorderSize = 0;
            chkParam.FlatAppearance.CheckedBackColor =
                chkParam.FlatAppearance.MouseDownBackColor =
                chkParam.FlatAppearance.MouseOverBackColor = Color.Transparent;
            chkParam.Appearance = chkParamSample.Appearance;
            chkParam.Size = chkParamSample.Size;
            chkParam.Image = Properties.Resources.Check_No;
            chkParam.Margin = chkParamSample.Margin;
            chkParam.Text = "";
            chkParam.Checked = false;

            Label lbldisplay1 = new Label();
            lbldisplay1.AutoEllipsis = true;
            lbldisplay1.Size = lblDisplayText1Sample.Size;
            lbldisplay1.Font = lblDisplayText1Sample.Font;
            lbldisplay1.ForeColor = Color.Gray;
            lbldisplay1.TextAlign = lblDisplayText1Sample.TextAlign;
            lbldisplay1.Margin = lblDisplayText1Sample.Margin;
            if (Parameter.DataSource.Columns.Count > 1)
                lbldisplay1.Text = Parameter.DataSource.Rows[0][1].ToString();
            else
                lbldisplay1.Text = Parameter.DisplayText1;

            lbldisplay1.Click += delegate
            {
                base.ResetTimeOut();
                chkParam.Checked = !chkParam.Checked;
            };

            chkParam.CheckedChanged += delegate
            {
                if (chkParam.Checked)
                {
                    base.ResetTimeOut();
                    Parameter.UserSelectedEvent(Parameter.DataSource.Rows[0][0]);
                    chkParam.Image = Properties.Resources.Check_Yes;
                    lbldisplay1.ForeColor = Common.GreenColor;
                }
                else
                {
                    Parameter.UserSelectedEvent(DBNull.Value);
                    chkParam.Image = Properties.Resources.Check_No;
                    lbldisplay1.ForeColor = Color.Gray;
                }

                if (FormLoaded)
                    lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            };

            flpModifier.Controls.Add(chkParam);
            flpModifier.Controls.Add(lbldisplay1);

            flpPanel.Controls.Add(flpModifier);

            if (Parameter.OrderedValue == null)
            {
                if (Parameter.DefaultSelected)
                    chkParam.Checked = true;
                else
                    chkParam.Checked = false;
            }
            else if (Parameter.OrderedValue == DBNull.Value)
                chkParam.Checked = false;
            else
                chkParam.Checked = true;
            log.LogMethodExit();
        }

        private void rbHalfHalf_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            if (rbHalfHalf.Checked && FormLoaded)
            {
                rbHalfHalf.Image = Properties.Resources.Green_Btn;
                rbWhole.Image = Properties.Resources.White_Btn;

                _callingElement.LeftParameters.Clear();
                _callingElement.RightParameters.Clear();
                RenderModifiers();

                flpRightSide.Visible = true;
                lblToppingTitleLeft.Text = Common.utils.MessageUtils.getMessage("Left Side Topping");
            }
            log.LogMethodExit();
        }

        private void rbWhole_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            if (rbWhole.Checked && FormLoaded)
            {
                rbWhole.Image = Properties.Resources.Green_Btn;
                rbHalfHalf.Image = Properties.Resources.White_Btn;

                _callingElement.LeftParameters.Clear();
                _callingElement.RightParameters.Clear();
                RenderModifiers();

                flpRightSide.Visible = false;
                lblToppingTitleLeft.Text = Common.utils.MessageUtils.getMessage("Topping");
            }
            log.LogMethodExit();
        }
    }
}
