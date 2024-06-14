/********************************************************************************************
* Project Name - Parafait_Kiosk -BaseFormMenuChoices.cs
* Description  - BaseFormMenuChoices 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class BaseFormMenuChoice : BaseForm
    {
        public BaseFormMenuChoice()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        public void RenderDefaultPanels()
        {
            log.LogMethodEntry();
            base.RenderPanelContent(_screenModel, panelHeader, 1);
            base.RenderPanelContent(_screenModel, flpMenu, 2);

            lblMenuHeading.Text = Common.utils.MessageUtils.getMessage(1116);
            btnStartOver.Text = Common.utils.MessageUtils.getMessage("Start Over");
            btnCheckout.Text = Common.utils.MessageUtils.getMessage("Checkout");
            double btnCounts = 0;
            int btnHeightfactor = 4;
            foreach (Control c in flpMenu.Controls)
            {
                if (c.Visible)
                {
                    btnCounts++;
                }
            }
            if (btnCounts == 0)
                btnCounts = 1;
            btnHeightfactor = btnHeightfactor - (int)GenericUtils.RoundOffFunction(btnCounts/3, 1, 0, "CEILING"); 
            panelMenu.Height = 470 - (92 * btnHeightfactor);
            flpMenu.Height = 370 - (92 * btnHeightfactor); 

            log.LogMethodExit();
        }

        private void BaseFormMenuChoice_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                for (int i = 2; i <= 12; i++)
                {
                    Button b = new Button();
                    b.Name = "btnMenu" + i.ToString();
                    b.Text = "";
                    b.FlatStyle = btnMenu1.FlatStyle;
                    b.FlatAppearance.BorderColor = btnMenu1.FlatAppearance.BorderColor;
                    b.FlatAppearance.BorderSize = btnMenu1.FlatAppearance.BorderSize;
                    b.FlatAppearance.CheckedBackColor = btnMenu1.FlatAppearance.CheckedBackColor;
                    b.FlatAppearance.MouseDownBackColor = btnMenu1.FlatAppearance.MouseDownBackColor;
                    b.FlatAppearance.MouseOverBackColor = btnMenu1.FlatAppearance.MouseOverBackColor;
                    b.Size = btnMenu1.Size;
                    b.Image = btnMenu1.Image;

                    b.Margin = btnMenu1.Margin;

                    flpMenu.Controls.Add(b);
                }

                panelMenu.Height = 470;
                flpMenu.Height = 370;
                lblMenuHeading.Height = 93;
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }

        private void btnStartOver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (UserTransaction.OrderDetails.ElementList.Count > 0
                && UserTransaction.OrderDetails.ElementList.Find(x => x.Parameters.Find(y => y.OrderedQuantity > 0) != null) != null)
            {
                if (Common.ShowDialog(Common.utils.MessageUtils.getMessage(1120)) == System.Windows.Forms.DialogResult.Yes)
                    Common.GoHome();
            }
            else
                Common.GoHome();
            log.LogMethodExit();
        }

        public override void UpdateHeader()
        {
            log.LogMethodEntry();
            btnViewOrder.Text = Common.utils.MessageUtils.getMessage("View Order") + ": " + UserTransaction.OrderDetails.TotalAmount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            btnPlayPoints.Text = Common.utils.MessageUtils.getMessage("Play Points") + ": " + UserTransaction.OrderDetails.PlayPoints.ToString(Common.utils.ParafaitEnv.NUMBER_FORMAT);
            log.LogMethodExit();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (UserTransaction.OrderDetails.ElementList.Count > 0)
            {
                bool allCardSale = UserTransaction.OrderDetails.AllCardSale();

                if ((this is frmCheckout) == false) // not already in checkout screen
                {
                    if (allCardSale)
                    {
                        frmCheckout fco = new frmCheckout();
                        fco.ShowDialog();
                    }
                    else
                    {
                        if (Helper.ShowTent())
                        {
                            frmTentSelection fts = new frmTentSelection();
                            fts.ShowDialog();
                        }
                        else
                        {
                            frmCheckout fco = new frmCheckout();
                            fco.ShowDialog();
                        }
                    }
                }
                else if ((this as frmCheckout)._viewOnly) // in view order screen
                {
                    if (allCardSale)
                    {
                        frmCheckout fco = new frmCheckout();
                        fco.ShowDialog();
                    }
                    else
                    {
                        if (Helper.ShowTent())
                        {
                            frmTentSelection fts = new frmTentSelection();
                            if (fts.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                                Close();
                        }
                        else
                        {
                            frmCheckout fco = new frmCheckout();
                            fco.ShowDialog();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }



        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (UserTransaction.OrderDetails.ElementList.Count > 0)
            {
                if ((this is frmCheckout) == false)
                {
                    frmCheckout fco = new frmCheckout(true);
                    fco.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
    }
}
