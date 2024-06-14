/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - View Order UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskViewOrderScreen : frmRedemptionKioskBaseForm
    {
        Utilities Utilities = Common.utils;
        public int totalTickets = 0;
        Card primaryCustomerCard;
        double redemptionDiscount;
        public frmRedemptionKioskViewOrderScreen()
        {
            log.LogMethodEntry();
            InitializeComponent();
            SetPrimaryCardDetails();
            log.LogMethodExit();
        }
        private void FrmViewOrder_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                base.RenderPanelContent(_screenModel, panelBottom, 2);
                ViewOrderProducts();
                Utilities.setLanguage(this);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message);
                log.LogMethodExit();
                Close();
            }

        }
        internal override bool ValidateAction(ScreenModel.UIPanelElement element)
        {
            log.LogMethodEntry(element);
            if (element.ActionScreenId > 0)
            {
                ResetTimeOut();
                ScreenModel screen = new ScreenModel(element.ActionScreenId);
                if (screen.CodeObjectName == "frmPrevScreen")
                {
                    this.Close();
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmStartOver")
                {
                    StartOver();
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmRedemptionKioskConfirmOrderScreen")
                {
                    return InvokeConfirmOrder();
                }
            }
            log.LogMethodExit(true);
            return true;
        }
        bool InvokeConfirmOrder()
        {
            log.LogMethodEntry();
            bool retValue = true;
            ResetTimeOut();
            if (!redemptionOrder.RedemptionHasGifts())
            {
                Common.ShowMessage(Utilities.MessageUtils.getMessage(1631)); 
                //"Please select Gift  to continue"
                retValue = false;
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        void ViewOrderProducts()
        {
            log.LogMethodEntry();
            UpdateHeader();
            flpOrderProducts.Controls.Clear();
            productViewUserControl viewUserControlItem;
            if (redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO != null 
                && redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Any())
            {
                foreach (RedemptionGiftsDTO gift in redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO)
                {
                    viewUserControlItem = new productViewUserControl(gift, redemptionOrder, redemptionDiscount)
                    {
                        setRefreshCallBack = ViewOrderProducts,
                        ResetTimeOutCallback = ResetTimeOut
                    };
                    flpOrderProducts.Controls.Add(viewUserControlItem);
                }
                flpOrderProducts.Refresh();
            }
            log.LogMethodExit();
        }

        public override void UpdateHeader()
        {
            log.LogMethodEntry();
            if (redemptionOrder != null && redemptionOrder.RedemptionDTO != null)
            {
                totalTickets = redemptionOrder.GetTotalTickets();
                lblTicketsLoaded.Text = totalTickets.ToString();
                lblRedeemedTickets.Text = redemptionOrder.GetRedeemedTickets().ToString();
                lblAvailableTickets.Text = redemptionOrder.GetAvailbleTickets().ToString();
                log.Debug("Header updated");
            }
            log.LogMethodExit();
            return;
        }

        void SetPrimaryCardDetails()
        {
            log.LogMethodEntry();

            if (primaryCustomerCard == null)
            {
                primaryCustomerCard = redemptionOrder.GetRedemptionPrimaryCard(null);
            }
            redemptionDiscount = 1;
            if (primaryCustomerCard != null)
            {
                redemptionDiscount = primaryCustomerCard.GetRedemptionDiscountForMembership();
                if (redemptionDiscount != 1)
                {
                    redemptionDiscount = 1 - redemptionDiscount;
                }
            }

            log.LogMethodExit();
        }
    }
}
