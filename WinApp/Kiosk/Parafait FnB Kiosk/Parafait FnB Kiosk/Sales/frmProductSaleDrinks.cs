/********************************************************************************************
* Project Name - Parafait_Kiosk -frmProductSaleDrinks.cs
* Description  - frmProductSaleDrinks 
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
using System.Collections.Generic;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmProductSaleDrinks : BaseFormProductSale
    {
        int AlcoholQtyLimit = 2;
        int defaultQuantity = 0;
        public frmProductSaleDrinks()
        {
            log.LogMethodEntry();
            InitializeComponent();
            flpDrinkParams.Controls.Remove(panelParameterSample);
            panelParameterSample.Visible = false;
            log.LogMethodExit();
        }

        internal void frmProductSaleDrinks_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Height = panelBG.BackgroundImage.Height - flpChoiceParameter.Height;
                RenderAllParameters();
                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
                Int32.TryParse(Common.utils.getParafaitDefaults("ALCOHOL_SALE_QUANTITY_LIMIT"), out AlcoholQtyLimit);
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
            List<ScreenModel.ElementParameter> cloneList = new List<ScreenModel.ElementParameter>();
            foreach (ScreenModel.ElementParameter parameter in _callingElement.VisibleParameters)
            {
                if (parameter.UIType == ScreenModel.ParameterUIType.ChoiceButton)
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

            int count = _callingElement.VisibleParameters.FindAll(x => x.Exploded == false && x.UIType == ScreenModel.ParameterUIType.Default).Count;
            if (count == 1)
                defaultQuantity = 1;

            foreach (ScreenModel.ElementParameter parameter in _callingElement.VisibleParameters)
            {
                if (parameter.Exploded == false && parameter.UIType == ScreenModel.ParameterUIType.Default)
                    RenderParameter(parameter);
                else if (parameter.UIType == ScreenModel.ParameterUIType.ChoiceButton)
                    RenderChoiceButtonParameter(parameter, flpParameters);
            }

            lblScreenTitle2.Text = _callingElement.Attribute.ActionScreenTitle2;

            if (_callingElement.Attribute.ActionScreenFooter1.Trim() != "")
                lblScreenFooter1.Text = _callingElement.Attribute.ActionScreenFooter1;
            else
            {
                lblScreenFooter1.Text = "";
            }

            if (_callingElement.Attribute.ActionScreenFooter2.Trim() != "")
                lblScreenFooter2.Text = _callingElement.Attribute.ActionScreenFooter2;
            else
            {
                lblScreenFooter2.Visible = false;
            }

            if (flpDrinkParams.Controls.Count > 6)
            {
                flpDrinkParams.Height += panelParameterSample.Height * (flpDrinkParams.Controls.Count - 6);
                flpParameters.Height += panelParameterSample.Height * (flpDrinkParams.Controls.Count - 6);
            }
            else if (flpDrinkParams.Controls.Count > 0 && flpDrinkParams.Controls.Count < 6)
            {
                int x = (6 - flpDrinkParams.Controls.Count) * panelParameterSample.Height;
                flpDrinkParams.Controls[0].Margin = new Padding(flpDrinkParams.Controls[0].Margin.Left,
                                                                flpDrinkParams.Controls[0].Margin.Top + x / 2,
                                                                flpDrinkParams.Controls[0].Margin.Right,
                                                                flpDrinkParams.Controls[0].Margin.Bottom);
            }

            this.Height = flpParameters.Top + flpParameters.Height + panelAddItem.Height;

            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            btnClose.BringToFront();
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

            ComboBox cmbParam = new ComboBox();
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

            FlowLayoutPanel flpDisplayTexts = new FlowLayoutPanel();
            flpDisplayTexts.Size = flpDisplayTextsSample.Size;
            flpDisplayTexts.Margin = flpDrinkParams.Margin;
            flpDisplayTexts.Location = flpDisplayTextsSample.Location;

            Label lbldisplay1 = new Label();
            lbldisplay1.AutoSize = true;
            lbldisplay1.Size = lblDisplayText1Sample.Size;
            lbldisplay1.Font = lblDisplayText1Sample.Font;
            lbldisplay1.ForeColor = lblDisplayText1Sample.ForeColor;
            lbldisplay1.TextAlign = lblDisplayText1Sample.TextAlign;
            if (Parameter.DataSource.Columns.Count > 1)
                lbldisplay1.Text = Parameter.DataSource.Rows[0][1].ToString();
            else
                lbldisplay1.Text = Parameter.DataSource.Rows[0][0].ToString();
            lbldisplay1.Click += delegate
            {
                base.ResetTimeOut();
                try
                {
                    if (Parameter.QuantityLimit)
                    {
                        if (getLimitedQuantityOrdered() < AlcoholQtyLimit)
                            cmbParam.SelectedIndex++;
                    }
                    else
                        cmbParam.SelectedIndex++;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing RenderParameter()" + ex.Message);
                }
            };

            flpDisplayTexts.Controls.Add(lbldisplay1);

            if (!string.IsNullOrEmpty(Parameter.DisplayText1.Trim()))
            {
                Label lbldisplay2 = new Label();
                lbldisplay2.AutoSize = true;
                lbldisplay2.Size = lblDisplayText2Sample.Size;
                lbldisplay2.Font = lblDisplayText2Sample.Font;
                lbldisplay2.ForeColor = lblDisplayText2Sample.ForeColor;
                lbldisplay2.TextAlign = lblDisplayText2Sample.TextAlign;
                lbldisplay2.Text = Parameter.DisplayText1;
                lbldisplay2.Margin = lblDisplayText2Sample.Margin;

                flpDisplayTexts.Controls.Add(lbldisplay2);
            }

            panelParameter.Controls.Add(flpDisplayTexts);
            flpDrinkParams.Controls.Add(panelParameter);

            lblComboDisplay.Click += delegate
            {
                base.ResetTimeOut();
                cmbParam.DroppedDown = true;
            };

            cmbParam.SelectedIndexChanged += delegate
            {
                base.ResetTimeOut();

                if (Parameter.QuantityLimit == true)
                {
                    int newQty = Convert.ToInt32(cmbParam.Text) - Parameter.UserQuantity;
                    int ordered = getLimitedQuantityOrdered();
                    if (ordered + newQty > AlcoholQtyLimit)
                    {
                        cmbParam.Text = Parameter.UserQuantity.ToString();
                        log.LogMethodExit();
                        return;
                    }
                }

                Parameter.UserQuantity = Convert.ToInt32(cmbParam.Text);

                if (Parameter.UserQuantity > 0)
                    Parameter.UserSelectedEvent(cmbParam.Tag);
                else
                    Parameter.UserSelectedEvent(DBNull.Value);

                lblComboDisplay.Text = cmbParam.Text;

                lblTotalAmount.Text = UserTransaction.getElementUserTotal(_callingElement);
            };
            if (Parameter.DisplayImage != null)//Goody Bag Changes
            {
                pnlImage.BackgroundImage = Parameter.DisplayImage;
            }
            else
            {
                if (pnlImage.Visible)
                {
                    pnlImage.Visible = false;
                    flpParameters.Height = flpParameters.Height - pnlImage.Height;
                }
            }

            if (Parameter.DataSource.Rows.Count > 0)
            {
                cmbParam.Tag = Parameter.DataSource.Rows[0][0];
                int maxQty = 10;
                if (this is frmProductSaleAlcohol)
                {
                    Parameter.QuantityLimit = true;
                    maxQty = AlcoholQtyLimit;
                }

                for (int i = 0; i <= maxQty; i++)
                {
                    cmbParam.Items.Add(i);
                }
            }

            if (Parameter.OrderedQuantity == 0)
                cmbParam.Text = defaultQuantity.ToString();
            else
                cmbParam.Text = Parameter.OrderedQuantity.ToString();
        log.LogMethodExit();
        }

        int getLimitedQuantityOrdered()
        {
            log.LogMethodEntry();
            int ordered = 0;
            if (UserTransaction.OrderDetails.ElementList.Contains(_callingElement) == false)
            {
                ordered = UserTransaction.OrderDetails.getOrderedLimitedQty();
                int totQty = 0;
                ScreenModel.UIPanelElement element = _callingElement;
                List<ScreenModel.ElementParameter> qtLimitParamList =
                    element.Parameters.FindAll(
                                            x => x.UserSelectedValue != null
                                            && x.UserSelectedValue != DBNull.Value
                                            && x.UserQuantity > 0
                                            && x.QuantityLimit == true);
                foreach (ScreenModel.ElementParameter parameter in qtLimitParamList)
                {
                    totQty += parameter.UserQuantity;
                }

                ordered += totQty;
            }
            else
            {
                foreach (ScreenModel.UIPanelElement element in UserTransaction.OrderDetails.ElementList)
                {
                    if (element.Equals(_callingElement))
                        continue;

                    List<ScreenModel.ElementParameter> qtLimitParamList =
                        element.Parameters.FindAll(
                                                x => x.UserSelectedValue != null
                                                && x.UserSelectedValue != DBNull.Value
                                                && x.OrderedQuantity > 0
                                                && x.QuantityLimit == true);
                    foreach (ScreenModel.ElementParameter parameter in qtLimitParamList)
                    {
                        ordered += parameter.OrderedQuantity;
                    }
                }

                int totQty = 0;
                ScreenModel.UIPanelElement editingElement = _callingElement;
                List<ScreenModel.ElementParameter> qtLimitEditingParamList =
                    editingElement.Parameters.FindAll(
                                            x => x.UserSelectedValue != null
                                            && x.UserSelectedValue != DBNull.Value
                                            && x.UserQuantity > 0
                                            && x.QuantityLimit == true);
                foreach (ScreenModel.ElementParameter parameter in qtLimitEditingParamList)
                {
                    totQty += parameter.UserQuantity;
                }

                ordered += totQty;
            }

            log.LogMethodExit(ordered);
            return ordered;
        }

    }
}
