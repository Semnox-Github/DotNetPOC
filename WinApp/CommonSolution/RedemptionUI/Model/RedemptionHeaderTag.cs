/********************************************************************************************
* Project Name - POS Redesign
* Description  - Redemption - model for redemption header tag
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionHeaderTag : ViewModelBase
    {
        #region Members
        private bool isActive;
        private int screenNumber;
        private double balanceTicket;
        private string cardNumber;
        private string cardNumberText;
        private string balanceTicketText;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        #region Properties
        public bool IsActive
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isActive);
                return isActive;
            }
            set
            {
                log.LogMethodEntry(isActive, value);
                SetProperty(ref isActive, value);
                log.LogMethodExit(isActive);
            }
        }

        public int ScreenNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(screenNumber);
                return screenNumber;
            }
            set
            {
                log.LogMethodEntry(screenNumber, value);
                SetProperty(ref screenNumber, value);
                log.LogMethodExit(screenNumber);
            }
        }

        public string CardNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
            set
            {
                log.LogMethodEntry(cardNumber, value);
                SetProperty(ref cardNumber, value);
                log.LogMethodExit(cardNumber);
            }
        }

        public double BalanceTicket
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(balanceTicket);
                return balanceTicket;
            }
            set
            {
                log.LogMethodEntry(balanceTicket, value);
                SetProperty(ref balanceTicket, value);
                BalanceTicketText = value.ToString();
                log.LogMethodExit(balanceTicket);
            }
        }

        public string BalanceTicketText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(balanceTicketText);
                return balanceTicketText;
            }
            set
            {
                log.LogMethodEntry(balanceTicketText, value);
                SetProperty(ref balanceTicketText, MessageViewContainerList.GetMessage(ExecutionContext, "Bal:") + balanceTicket.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")));
                log.LogMethodExit(balanceTicketText);
            }
        }

        public string CardNumberText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardNumberText);
                return cardNumberText;
            }
            set
            {
                log.LogMethodEntry(cardNumberText, value);
                SetProperty(ref cardNumberText, value);
                log.LogMethodExit(cardNumberText);
            }
        }

        public string ScreenNumberText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(screenNumber);
                return screenNumber.ToString();
            }
        }
        #endregion

        #region Constructor
        public RedemptionHeaderTag(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            isActive = false;
            screenNumber = 0;
            balanceTicket = 0;
            cardNumber = string.Empty;
            cardNumberText = string.Empty;
            balanceTicketText = string.Empty;
            log.LogMethodExit();
        }
        #endregion
    }
}
