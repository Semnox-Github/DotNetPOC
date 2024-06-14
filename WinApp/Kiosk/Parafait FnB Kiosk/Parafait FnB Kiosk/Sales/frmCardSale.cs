/********************************************************************************************
* Project Name - Parafait_Kiosk -frmCardSale.cs
* Description  - frmCardSale 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmCardSale : BaseForm
    {
        int _cardCount = 0;
        int MaxCards = 1;
        ScreenModel.ElementParameter productParameter;
        public frmCardSale()
        {
            log.LogMethodEntry();
            InitializeComponent();
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, 10);
            //this.Refresh();
            Common.utils.setLanguage(this);
            log.LogMethodExit();
        }

        bool getProductDetails()
        {
            log.LogMethodEntry();
            int product_id = Convert.ToInt32(productParameter.UserSelectedValue);

            DataTable dt = Helper.getProductDetails(product_id);
            string productType = dt.Rows[0]["product_type"].ToString();
            if (dt.Rows.Count > 0)
            {
                if (!Helper.CheckProductAvailability(product_id))
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1122, dt.Rows[0]["product_name"]));
                    log.LogMethodExit(false);
                    return false;
                }                

                MaxCards = Convert.ToInt32(dt.Rows[0]["CardCount"]);
                if (MaxCards < 1)
                    MaxCards = 0;

                if (productType.Equals("VARIABLECARD"))
                {
                    double varAmount = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm(Common.utils.MessageUtils.getMessage(480), '-', Common.utils);
                    if (varAmount <= 0)
                    {
                        log.LogMethodExit(false);
                        return false;
                    }

                    if (varAmount != Math.Round(varAmount, 0))
                    {
                        Common.ShowMessage(Common.utils.MessageUtils.getMessage(932));
                        log.LogMethodExit(false);
                        return false;
                    }

                    double maxVarAmount = Convert.ToDouble(Common.utils.getParafaitDefaults("MAX_VARIABLE_RECHARGE_AMOUNT"));
                    if (varAmount > maxVarAmount)
                    {
                        Common.ShowMessage(Common.utils.MessageUtils.getMessage(930, maxVarAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                        log.LogMethodExit(false);
                        return false;
                    }

                    productParameter.UserPrice = (decimal)varAmount;
                }
                else
                {
                    DataTable dtUpsell = Helper.getUpsellProducts(dt.Rows[0]["product_id"]);
                    if (dtUpsell.Rows.Count > 0)
                    {
                        string greeting1 = Common.utils.MessageUtils.getMessage(417, Convert.ToDouble(dt.Rows[0]["Price"]).ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                        string productName = dt.Rows[0]["product_name"].ToString();
                        string offerMessage = dtUpsell.Rows[0]["description"].ToString();
                        if (offerMessage.Trim() == "")
                            offerMessage = dtUpsell.Rows[0]["product_name"].ToString();
                        frmUpsellProduct fup = new frmUpsellProduct(greeting1, productName, offerMessage);
                        DialogResult dr = fup.ShowDialog();
                        if (dr == System.Windows.Forms.DialogResult.Cancel)
                        {
                            log.LogMethodExit(false);
                            return false;
                        }
                        else if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            productParameter.UserSelectedValue = dtUpsell.Rows[0]["product_id"];
                        }
                    }
                }

                // recharge or it is not a variable NEW card sale 
                if (productType.Equals("RECHARGE") || (productType.Equals("VARIABLECARD") && dt.Rows[0]["QuantityPrompt"].ToString().Equals("N")))
                {
                    frmTapCard ftc = new frmTapCard(productParameter);
                    if (ftc.ShowDialog() == DialogResult.OK)
                    {
                        productParameter.OrderedValue = productParameter.UserSelectedValue;
                        productParameter.OrderedQuantity = 1;
                        UserTransaction.OrderDetails.AddItem(_callingElement);
                        UserTransaction.getOrderTotal();

                        //frmCheckout fco = new frmCheckout(true);
                        // fco.ShowDialog();
                    }

                    log.LogMethodExit(false);
                    return false; // close the form, in load
                }
                else if (productType.Equals("VARIABLECARD") && dt.Rows[0]["QuantityPrompt"].ToString().Equals("Y")) // new variable card
                {
                    productParameter.OrderedValue = productParameter.UserSelectedValue;
                    productParameter.OrderedQuantity = 1;
                    productParameter.CardNumber = null;
                    UserTransaction.OrderDetails.AddItem(_callingElement);
                    UserTransaction.getOrderTotal();

                    //frmCheckout fco = new frmCheckout(true);
                    // fco.ShowDialog();

                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else
            {
                Common.ShowMessage("Invalid product");
                log.LogMethodExit(false);
                return false;
            }
        }

        private void frmCardSale_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (_callingElement.Parameters.Count == 0)
                {
                    Common.ShowMessage("Card product parameter not defined");
                    Close();
                    log.LogMethodExit();
                    return;
                }

                lblExistingCard.Visible = false;
                if (_callingElement.Parameters.Count > 1) // value meal / combo package
                {
                    // commented for now, not sure if value meal should support existing cards
                    //  lblExistingCard.Visible = true;
                }
                
                productParameter = _callingElement.Parameters[0];
                productParameter.ParameterType = ScreenModel.ParameterType.CardSale;

                if (productParameter.DataSource.Rows.Count == 0)
                {
                    Common.ShowMessage("Card product parameter value not present");
                    Close();
                    log.LogMethodExit();
                    return;
                }

                productParameter.UserSelectedValue = productParameter.DataSource.Rows[0][0];

                DataTable suggestiveTable = Helper.getSuggestiveSellProducts(Convert.ToInt32(productParameter.UserSelectedValue));
                if (suggestiveTable != null && suggestiveTable.Rows.Count > 0)
                {
                    pnlsuggestiveSale.Tag = suggestiveTable.Rows[0]["product_id"];
                    List<ScreenModel.ElementParameter> suggestiveList = _callingElement.Parameters.Where(x => (bool)(Convert.ToInt32(x.OrderedValue) == Convert.ToInt32(pnlsuggestiveSale.Tag))).ToList<ScreenModel.ElementParameter>();
                    if (suggestiveList != null && suggestiveList.Count > 0)
                    {
                        cmbWBCount.Text = lblComboWBCount.Text = suggestiveList[0].OrderedQuantity.ToString();
                    }
                    lblEachWristBand.Text = suggestiveTable.Rows[0]["OfferMessage"].ToString();
                    pnlsuggestiveSale.BackgroundImage = Helper.GetProductImage((suggestiveTable.Rows[0]["ImageFileName"] == DBNull.Value) ? "" : suggestiveTable.Rows[0]["ImageFileName"].ToString());
                    cmbWBCount.Tag = suggestiveTable.Rows[0]["Price"];
                    lblAmount.Text = (Convert.ToDouble(cmbWBCount.SelectedItem) * Convert.ToDouble(cmbWBCount.Tag)).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    if(pnlsuggestiveSale.BackgroundImage==null)
                    {
                        lblAmount.Visible = lblTotal.Visible = lblEachWristBand.Visible = pnlsuggestiveSale.Visible = panelQty.Visible = false;
                    }
                }
                else
                {
                    lblAmount.Visible = lblTotal.Visible = lblEachWristBand.Visible = pnlsuggestiveSale.Visible = panelQty.Visible = false;
                }

                // decide whether to show the form or not
                if (!getProductDetails())
                {
                    Close();
                    log.LogMethodExit();
                    return;
                }

                if (MaxCards == 0)
                    MaxCards = 6;
                

                if (MaxCards < 7)
                    panelOtherNumber.Visible = false;

                if (MaxCards < 6)
                {
                    lblDigit6.Visible = false;
                    if (MaxCards == 5)
                    {
                        lblDigit4.Margin = new Padding(lblDigit1.Margin.Left + lblDigit1.Margin.Left + lblDigit1.Width / 2,
                           lblDigit1.Margin.Top,
                           lblDigit1.Margin.Right,
                           lblDigit1.Margin.Bottom);
                    }
                }

                if (MaxCards < 5)
                {
                    lblDigit5.Visible = false;
                    if (MaxCards == 4)
                    {
                        lblDigit3.Margin = lblDigit1.Margin = new Padding(lblDigit1.Margin.Left + lblDigit1.Margin.Left + lblDigit1.Width / 2,
                          lblDigit1.Margin.Top,
                          lblDigit1.Margin.Right,
                          lblDigit1.Margin.Bottom);
                    }
                }

                if (MaxCards < 4)
                    lblDigit4.Visible = false;

                if (MaxCards < 3)
                {
                    lblDigit3.Visible = false;
                    if (MaxCards == 2)
                    {
                        lblDigit1.Margin = new Padding(lblDigit1.Margin.Left + lblDigit1.Margin.Left + lblDigit1.Width / 2,
                        lblDigit1.Margin.Top,
                        lblDigit1.Margin.Right,
                        lblDigit1.Margin.Bottom);
                    }
                }

                if (MaxCards < 2)
                {
                    lblDigit2.Visible = false;
                    lblEachCardMessage.Visible = false;
                    lblDigit1.Margin = new Padding(lblDigit1.Margin.Left * 2 + lblDigit1.Width,
                        lblDigit1.Margin.Top,
                        lblDigit1.Margin.Right,
                        lblDigit1.Margin.Bottom);
                }

                for (int i = 7; i <= MaxCards; i++)
                    cmbOtherNumber.Items.Add(i);

                _cardCount = productParameter.CardCount;
                if (MaxCards == 1)
                    lblDigit_Click(lblDigit1, null);
                if (_cardCount > 0)
                {
                    if (_cardCount < 7)
                    {
                        Label c = flpCardCount.Controls["lblDigit" + _cardCount.ToString()] as Label;
                        if (c != null)
                            c.Image = Properties.Resources.Keypad__Key_Green;
                    }
                    else
                        cmbOtherNumber.SelectedItem = _cardCount;
                }
                else if (_cardCount == -1)
                {
                    lblExistingCard.Image = Properties.Resources.Any_amount_Green;
                    lblOtherNumber.Font = lblDigit1.Font;
                }
                if (MaxCards == 1)
                    btnConfirm.PerformClick();
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }

            Control[] cntrlArray = flpCardCount.Controls.Find("lblTotal", false);
            if (cntrlArray != null && cntrlArray.Length > 0)
            {
                flpCardCount.Height = cntrlArray[0].Bottom + 20;
                this.Size = panelBG.Size = panelBG.BackgroundImage.Size;
                if (panelOtherNumber.Visible && pnlsuggestiveSale.Visible)
                {
                   this.Height = panelBG.Height = panelBG.BackgroundImage.Height + panelOtherNumber.Height;
                }
                if (panelOtherNumber.Visible && pnlsuggestiveSale.Visible)
                {
                    this.Height = panelBG.Height = panelBG.Height + lblExistingCard.Height;
                }

                if(panelBG.Height != panelBG.BackgroundImage.Height)
                {
                    panelBG.BackgroundImageLayout = ImageLayout.Stretch;
                }
                //this.Size = panelBG.Size = panelBG.BackgroundImage.Size; //flpCardCount.Bottom + panelConfirm.Height + 20;
            }

          log.LogMethodExit();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            if (_cardCount != 0)
            {
                productParameter.CardCount = _cardCount;

                if (_cardCount > 0)
                    productParameter.CardNumber = null;

                productParameter.OrderedValue = productParameter.UserSelectedValue;
                productParameter.OrderedQuantity = productParameter.UserQuantity;

                int suggesstiveProdId = (pnlsuggestiveSale.Tag != DBNull.Value && Convert.ToInt32(pnlsuggestiveSale.Tag) >= 0) ? Convert.ToInt32(pnlsuggestiveSale.Tag) : -1;

                List<ScreenModel.ElementParameter> suggestiveList = _callingElement.Parameters.Where(x => (bool)(x.OrderedValue != DBNull.Value && Convert.ToInt32(x.OrderedValue) == suggesstiveProdId)).ToList<ScreenModel.ElementParameter>();
                if (suggestiveList != null && suggestiveList.Count > 0)
                {
                    //updating quantity to the existing 
                    ScreenModel.ElementParameter param = _callingElement.Parameters.Where(x => (bool)(x.OrderedValue != DBNull.Value && Convert.ToInt32(x.OrderedValue) == suggesstiveProdId)).ToList<ScreenModel.ElementParameter>()[0];
                    param.OrderedQuantity = param.UserQuantity = Convert.ToInt32(cmbWBCount.SelectedItem);
                }
                else if (Convert.ToInt32(cmbWBCount.SelectedItem) > 0)
                {
                    ScreenModel.ElementParameter wbParam = new ScreenModel.ElementParameter();
                    wbParam.ActionScreenId = productParameter.ActionScreenId;
                    wbParam.OrderedQuantity = wbParam.UserQuantity = Convert.ToInt32(cmbWBCount.SelectedItem);
                    wbParam.OrderedValue = wbParam.UserSelectedValue = pnlsuggestiveSale.Tag; // suggest product id
                    _callingElement.Parameters.Add(wbParam);
                }

                UserTransaction.OrderDetails.AddItem(_callingElement);
                UserTransaction.getOrderTotal();

                //frmCheckout fco = new frmCheckout(true);
                //fco.ShowDialog();

                Close();
            }
            log.LogMethodExit();
        }

        private void lblDigit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            foreach (Control c in flpCardCount.Controls)
            {
                if (c.Name.ToLower().Contains("digit"))
                {
                    (c as Label).Image = Properties.Resources.Keypad__Key;
                }
            }
            lblExistingCard.Image = Properties.Resources.Any_amount;
            lblOtherNumber.Image = Properties.Resources.Any_amount;
            lblOtherNumber.Font = lblEachCardMessage.Font;
            lblOtherNumber.Text = Common.utils.MessageUtils.getMessage("Other number of Cards");

            Label lbl = sender as Label;
            lbl.Image = Properties.Resources.Keypad__Key_Green;

            _cardCount = Convert.ToInt32(lbl.Text);
            log.LogMethodExit();
        }

        private void lblExistingCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            foreach (Control c in flpCardCount.Controls)
            {
                if (c.Name.ToLower().Contains("digit"))
                {
                    (c as Label).Image = Properties.Resources.Keypad__Key;
                }
            }

            lblExistingCard.Image = Properties.Resources.Any_amount_Green;

            frmTapCard ftc = new frmTapCard(productParameter);
            if (ftc.ShowDialog() == DialogResult.OK)
            {
               _cardCount = -1;
            }
            else
                lblExistingCard.Image = Properties.Resources.Any_amount;
            log.LogMethodExit();
        }

        private void lblOtherNumber_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            cmbOtherNumber.DroppedDown = true;
            log.LogMethodExit();
        }

        private void cmbOtherNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            try
            {
                lblOtherNumber.Text = cmbOtherNumber.SelectedItem.ToString();
                lblOtherNumber.Image = Properties.Resources.Any_amount_Green;
                lblOtherNumber.Font = lblDigit1.Font;

                _cardCount = Convert.ToInt32(lblOtherNumber.Text);

                foreach (Control c in flpCardCount.Controls)
                {
                    if (c.Name.ToLower().Contains("digit"))
                    {
                        (c as Label).Image = Properties.Resources.Keypad__Key;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.logToFile(ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblComboWBCount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            cmbWBCount.DroppedDown = true;
            log.LogMethodExit();
        }

        private void cmbWBCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            try
            {
                lblComboWBCount.Text = cmbWBCount.SelectedItem.ToString();
                //lblComboWBCount.Image = Properties.Resources.Any_amount_Green;
                //lblComboWBCount.Font = lblDigit1.Font;
                lblAmount.Text = (Convert.ToDouble(cmbWBCount.SelectedItem) * Convert.ToDouble(cmbWBCount.Tag)).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.logToFile(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
