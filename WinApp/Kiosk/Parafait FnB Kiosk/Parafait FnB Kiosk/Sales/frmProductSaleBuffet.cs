/********************************************************************************************
* Project Name - Parafait_Kiosk -frmProductSaleBuffet.cs
* Description  - frmProductSaleBuffet 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;

namespace Parafait_FnB_Kiosk
{
    public partial class frmProductSaleBuffet : BaseFormMenuChoice
    {
        public frmProductSaleBuffet()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void frmProductSaleBuffet_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                foreach (ScreenModel.ElementParameter param in _callingElement.Parameters)
                {
                    if (param.Toplevel && param.DataSource.Rows.Count == 1)
                    {
                        if (!Helper.CheckProductAvailability(param.DataSource.Rows[0][0]))
                        {
                            this.Opacity = 0;

                            Common.ShowMessage(Common.utils.MessageUtils.getMessage(1108));
                            Close();
                            break;
                        }
                    }
                }

                base.RenderDefaultPanels();
                base.RenderPanelContent(_screenModel, flpBuffet, 3);

                int index = 1;

                if (_callingElement.VisibleParameters.Count == 2)
                {
                    panelParam3.Visible = false;
                }
                else if (_callingElement.VisibleParameters.Count == 1)
                {
                    panelParam2.Visible =
                    panelParam3.Visible = false;
                }
                cmbParam1.DropDownHeight = cmbParam1.Items.Count * cmbParam1.ItemHeight + 10;
                cmbParam2.DropDownHeight = cmbParam2.Items.Count * cmbParam2.ItemHeight + 10;

                foreach (ScreenModel.ElementParameter parameter in _callingElement.VisibleParameters)
                {
                    switch (index)
                    {
                        case 1:
                            {
                                lblParam1.Text = parameter.DisplayText1;

                                cmbParam1.SelectedIndexChanged += delegate
                                {
                                    base.ResetTimeOut();
                                    int qty = Convert.ToInt32(cmbParam1.SelectedItem.ToString());

                                    if (qty == 0)
                                    {
                                        parameter.UserSelectedEvent(DBNull.Value);
                                        parameter.UserQuantity = 0;
                                        lblParam1.Text = parameter.DisplayText1;
                                        cmbParam3.SelectedIndex = 0;
                                        lblParam3.Enabled = false;
                                        cmbParam3.Enabled = false;//2017-Jul-07
                                    }
                                    else if (parameter.DataSource.Rows.Count > 0)
                                    {
                                        parameter.UserSelectedEvent(parameter.DataSource.Rows[0][0]);
                                        parameter.UserQuantity = qty;
                                        if (parameter.DataSource.Columns.Count > 1)
                                            lblParam1.Text = parameter.DataSource.Rows[0][1].ToString() + ": " + qty.ToString();
                                        else
                                            lblParam1.Text = qty.ToString();

                                        cmbParam3.Items.Clear();

                                        qty = parameter.UserQuantity;
                                        for (int i = 0; i <= qty; i++)
                                            cmbParam3.Items.Add(i);

                                        lblParam3.Enabled = true;
                                        cmbParam3.Enabled = true;//2017-Jul-07
                                        cmbParam3.SelectedIndex = 0;
                                        cmbParam3.Height = Math.Max(qty * cmbParam3.ItemHeight, cmbParam3.ItemHeight);
                                    }
                                };

                                cmbParam1.Text = parameter.OrderedQuantity.ToString();
                                lblParam1.BringToFront();
                                break;
                            }
                        case 2:
                            {
                                lblParam2.Text = parameter.DisplayText1;
                                cmbParam2.SelectedIndexChanged += delegate
                                {
                                    base.ResetTimeOut();
                                    int qty = Convert.ToInt32(cmbParam2.SelectedItem.ToString());

                                    if (qty == 0)
                                    {
                                        parameter.UserSelectedEvent(DBNull.Value);
                                        parameter.UserQuantity = 0;
                                        lblParam2.Text = parameter.DisplayText1;
                                    }
                                    else if (parameter.DataSource.Rows.Count > 0)
                                    {
                                        parameter.UserSelectedEvent(parameter.DataSource.Rows[0][0]);
                                        parameter.UserQuantity = qty;
                                        if (parameter.DataSource.Columns.Count > 1)
                                            lblParam2.Text = parameter.DataSource.Rows[0][1].ToString() + ": " + qty.ToString();
                                        else
                                            lblParam2.Text = qty.ToString();
                                    }
                                };
                                cmbParam2.Text = parameter.OrderedQuantity.ToString();
                                lblParam2.BringToFront();
                                break;
                            }
                        case 3:
                            {
                                base.ResetTimeOut();
                                lblParam3.Text = parameter.DisplayText1;
                                cmbParam3.SelectedIndexChanged += delegate
                                {
                                    int qty = Convert.ToInt32(cmbParam3.SelectedItem.ToString());

                                    if (qty == 0)
                                    {
                                        parameter.UserSelectedEvent(DBNull.Value);
                                        parameter.UserQuantity = 0;
                                        lblParam3.Text = parameter.DisplayText1;
                                    }
                                    else if (parameter.DataSource.Rows.Count > 0)
                                    {
                                        parameter.UserSelectedEvent(parameter.DataSource.Rows[0][0]);
                                        parameter.UserQuantity = qty;
                                        if (parameter.DataSource.Columns.Count > 1)
                                            lblParam3.Text = parameter.DataSource.Rows[0][1].ToString() + ": " + qty.ToString();
                                        else
                                            lblParam3.Text = qty.ToString();
                                    }
                                };
                                cmbParam3.Text = parameter.OrderedQuantity.ToString();
                                lblParam3.BringToFront();
                                break;
                            }
                    }

                    index++;
                }

                lblParam1.Click += delegate
                {
                    cmbParam1.DroppedDown = true;
                };

                lblParam2.Click += delegate
                {
                    cmbParam2.DroppedDown = true;
                };

                lblParam3.Click += delegate
                {
                    cmbParam3.DroppedDown = true;
                };

                UpdateHeader();
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            bool selected = false;
            foreach (ScreenModel.ElementParameter parameter in _callingElement.Parameters)
            {
                parameter.OrderedValue = parameter.UserSelectedValue;
                parameter.OrderedQuantity = parameter.UserQuantity;

                if (parameter.OrderedValue != DBNull.Value && parameter.OrderedValue != null)
                    selected = true;
            }

            if (selected)
            {
                UserTransaction.OrderDetails.AddItem(_callingElement);
                UserTransaction.getOrderTotal();

                frmCheckout fco = new frmCheckout(true);
                fco.ShowDialog();
            }
            log.LogMethodExit();
        }
    }
}
