/********************************************************************************************
* Project Name - Parafait_Kiosk -frmProductSaleSubs.cs
* Description  - frmProductSaleSubs 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmProductSaleSubs : BaseFormProductSale
    {
        public frmProductSaleSubs()
        {
            log.LogMethodEntry();
            InitializeComponent();
            flpSubParams.Controls.Remove(panelParameterSample);
            panelParameterSample.Visible = false;
            flpSideParameter.Visible = false;
            log.LogMethodExit();
        }

        internal void frmProductSaleSubs_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Height = panelBG.BackgroundImage.Height - flpChoiceParameter.Height;
                RenderAllParameters();
                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            }
            catch(Exception ex)
            {
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }

        public void RenderAllParameters()
        {
            log.LogMethodEntry();
            List<ScreenModel.ElementParameter> cloneList = new List<ScreenModel.ElementParameter>();
            foreach (ScreenModel.ElementParameter parameter in _callingElement.VisibleParameters)
            {
                if (parameter.DisplayIndex == 1)
                    continue;

                if (parameter.Exploded == false && parameter.DataSource.Rows.Count > 1)
                {
                    foreach (DataRow dr in parameter.DataSource.Rows)
                    {
                        ScreenModel.ElementParameter clone = parameter.Clone();
                        clone.Parameter = "select * from (" + parameter.Parameter
                                            + ") v where " + parameter.DataSource.Columns[0].ColumnName
                                            + " = " + dr[0].ToString();
                        clone.getDataSource();
                        clone.identifier = clone.ParameterId.ToString() + dr[0].ToString();

                        cloneList.Add(clone);
                    }
                    parameter.Exploded = true;
                }
            }

            _callingElement.Parameters.AddRange(cloneList);

            foreach (ScreenModel.ElementParameter parameter in _callingElement.VisibleParameters)
            {
                if (parameter.DisplayIndex == 1)
                    RenderComboParameter(parameter);
                else if (parameter.Exploded == false)
                    RenderParameter(parameter);
            }

            if (_callingElement.Attribute.ActionScreenTitle2.Trim() != "")
                lblScreenTitle2.Text = _callingElement.Attribute.ActionScreenTitle2;
            else
                lblScreenTitle2.Text = Common.utils.MessageUtils.getMessage("Customize");

            if (_callingElement.Attribute.ActionScreenFooter1.Trim() != "")
            {
                lblScreenFooter1.Text = _callingElement.Attribute.ActionScreenFooter1;
                // remove and add it back to the end
                flpSubParams.Controls.Remove(lblScreenFooter1);
                flpSubParams.Controls.Add(lblScreenFooter1);
            }
            else
            {
                lblScreenFooter1.Visible = false;
            }

            if (flpSubParams.Controls.Count > 9)
            {
                flpSubParams.Height += panelParameterSample.Height * (flpSubParams.Controls.Count - 9);
                flpParameters.Height += panelParameterSample.Height * (flpSubParams.Controls.Count - 9);
            }
            else
            {
                int x = flpSubParams.Controls.Count * panelParameterSample.Height;
                flpSubParams.Controls[0].Margin = new Padding(flpSubParams.Controls[0].Margin.Left,
                                                                flpSubParams.Controls[0].Margin.Top + (flpSubParams.Height - x) / 2,
                                                                flpSubParams.Controls[0].Margin.Right,
                                                                flpSubParams.Controls[0].Margin.Bottom);
            }

            this.Height = flpParameters.Top + flpParameters.Height + panelAddItem.Height;

            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            log.LogMethodExit();
        }


        internal void RenderComboParameter(ScreenModel.ElementParameter Parameter)
        {
            log.LogMethodEntry(Parameter);
            flpSideParameter.Visible = true;

            lblSideParam.Click += delegate
            {
                base.ResetTimeOut();
                cmbSideParameter.DroppedDown = true;
            };

            lblDisplayText1.Text = Parameter.DisplayText1;
            lblDisplayText2.Text = Parameter.DisplayText2;

            cmbSideParameter.SelectedIndexChanged += delegate
            {
                base.ResetTimeOut();
                Parameter.UserSelectedEvent(cmbSideParameter.SelectedValue);
                lblSideParam.Text = cmbSideParameter.Text;

                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            };

            cmbSideParameter.ValueMember = Parameter.DataSource.Columns[0].ColumnName;
            if (Parameter.DataSource.Columns.Count > 1)
                cmbSideParameter.DisplayMember = Parameter.DataSource.Columns[1].ColumnName;
            else
                cmbSideParameter.DisplayMember = cmbSideParameter.ValueMember;

            cmbSideParameter.DataSource = Parameter.DataSource;
            cmbSideParameter.Height = Math.Max(Parameter.DataSource.Rows.Count * cmbSideParameter.ItemHeight, cmbSideParameter.ItemHeight);

            if (Parameter.OrderedValue != null)
                cmbSideParameter.SelectedValue = Parameter.OrderedValue;

            lblSideParam.BringToFront();
            log.LogMethodExit();
        }

        internal void RenderParameter(ScreenModel.ElementParameter Parameter)
        {
            log.LogMethodEntry(Parameter);
            Panel panelParameter = new Panel();
            panelParameter.Size = panelParameterSample.Size;
            panelParameter.BackgroundImage = panelParameterSample.BackgroundImage;
            panelParameter.BackgroundImageLayout = panelParameterSample.BackgroundImageLayout;
            panelParameter.Margin = panelParameterSample.Margin;

            FlowLayoutPanel flpDisplayTexts = new FlowLayoutPanel();
            flpDisplayTexts.Size = flpDisplayTextsSample.Size;
            flpDisplayTexts.Margin = flpSubParams.Margin;
            flpDisplayTexts.Location = flpDisplayTextsSample.Location;

            ComboBox cmbParam = new ComboBox();
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
            chkParam.CheckedChanged += delegate
            {
                base.ResetTimeOut();
                if (chkParam.Checked)
                    chkParam.Image = Properties.Resources.Check_Yes;
                else
                    chkParam.Image = Properties.Resources.Check_No;

                if (Parameter.DataSource.Rows.Count > 1)
                {
                    if (chkParam.Checked)
                        Parameter.UserSelectedEvent(cmbParam.SelectedValue);
                    else
                        Parameter.UserSelectedEvent(DBNull.Value);
                }
                else if (Parameter.DataSource.Rows.Count == 1)
                {
                    if (chkParam.Checked)
                        Parameter.UserSelectedEvent(Parameter.DataSource.Rows[0][0]);
                    else
                        Parameter.UserSelectedEvent(DBNull.Value);
                }
                else
                {
                    Parameter.UserSelectedEvent(DBNull.Value);
                }

                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            };

            Label lbldisplay1 = new Label();
            lbldisplay1.AutoSize = lblDisplayText1Sample.AutoSize;
            lbldisplay1.AutoEllipsis = lblDisplayText1Sample.AutoEllipsis;
            lbldisplay1.Size = lblDisplayText1Sample.Size;
            lbldisplay1.Font = lblDisplayText1Sample.Font;
            lbldisplay1.ForeColor = lblDisplayText1Sample.ForeColor;
            lbldisplay1.TextAlign = lblDisplayText1Sample.TextAlign;
            lbldisplay1.Margin = lblDisplayText1Sample.Margin;
            if (Parameter.DataSource.Columns.Count > 1)
                lbldisplay1.Text = Parameter.DataSource.Rows[0][1].ToString();
            else
                lbldisplay1.Text = Parameter.DisplayText1;

            lbldisplay1.Click += delegate
            {
                base.ResetTimeOut();

                if (Parameter.Disabled == false)
                    chkParam.Checked = !chkParam.Checked;
            };

            if (Parameter.DataSource.Rows.Count > 1)
            {
                cmbParam.Name = Parameter.ParameterName;
                cmbParam.Size = cmbParameter.Size;
                cmbParam.DropDownStyle = cmbParameter.DropDownStyle;
                cmbParam.Location = cmbParameter.Location;
                cmbParam.FlatStyle = cmbParameter.FlatStyle;
                cmbParam.Font = cmbParameter.Font;
                cmbParam.ForeColor = cmbParameter.ForeColor;
                panelParameter.Controls.Add(cmbParam);

                Label lblComboDisplay = new Label();
                lblComboDisplay.AutoSize = false;
                lblComboDisplay.Size = lblComboDispSample.Size;
                lblComboDisplay.Font = lblComboDispSample.Font;
                lblComboDisplay.ForeColor = lblComboDispSample.ForeColor;
                lblComboDisplay.TextAlign = lblComboDispSample.TextAlign;
                lblComboDisplay.Location = lblComboDispSample.Location;
                lblComboDisplay.Margin = lblComboDispSample.Margin;
                lblComboDisplay.Image = lblComboDispSample.Image;
                lblComboDisplay.ImageAlign = lblComboDispSample.ImageAlign;
                panelParameter.Controls.Add(lblComboDisplay);
                lblComboDisplay.BringToFront();

                lblComboDisplay.Click += delegate
                {
                    base.ResetTimeOut();
                    cmbParam.DroppedDown = true;
                };

                cmbParam.SelectedIndexChanged += delegate
                {
                    base.ResetTimeOut();
                    if (chkParam.Checked)
                        Parameter.UserSelectedEvent(cmbParam.SelectedValue);

                    lblComboDisplay.Text = cmbParam.Text;

                    lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
                };
            }

            flpDisplayTexts.Controls.Add(chkParam);
            flpDisplayTexts.Controls.Add(lbldisplay1);

            panelParameter.Controls.Add(flpDisplayTexts);
            flpSubParams.Controls.Add(panelParameter);

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

            if (Parameter.Disabled)
                chkParam.Enabled = false;

            if (Parameter.DataSource.Rows.Count > 1)
            {
                cmbParam.ValueMember = Parameter.DataSource.Columns[0].ColumnName;
                if (Parameter.DataSource.Columns.Count > 1)
                    cmbParam.DisplayMember = Parameter.DataSource.Columns[1].ColumnName;
                else
                    cmbParam.DisplayMember = cmbParam.ValueMember;

                cmbParam.DataSource = Parameter.DataSource;
                if (Parameter.OrderedValue != null && Parameter.OrderedValue != DBNull.Value)
                    cmbParam.SelectedValue = Parameter.OrderedValue;
            }
            log.LogMethodExit();
        }

        public override bool ValidateItemAddition()
        {
            log.LogMethodEntry();
            log.LogMethodExit(true);
            return true;
        }
    }
}
