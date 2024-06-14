/********************************************************************************************
* Project Name - POS Redesign
* Description  - Header items model
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Raja Uthanda           to add button style
********************************************************************************************/

namespace Semnox.Parafait.CommonUI
{
    using Semnox.Core.Utilities;
    using Semnox.Parafait.ViewContainer;
    public enum GenericTransactionListItemType
    {
        Item,
        Ticket
    }

    public class GenericTransactionListItem : ViewModelBase
    {
        #region Members
        private bool isEnabled;
        private GenericTransactionListItemType redemptionRightSectionItemType;
        private string productName;
        private string ticketText;
        private string totalText;
        private string ticketNo;
        private string ticketDisplayText;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int ticket;
        private int count;
        private int key;
        private int? viewGroupingNumber;
        #endregion

        #region Properties
        public bool IsEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isEnabled);
                return isEnabled;
            }
            set
            {
                log.LogMethodEntry(isEnabled, value);
                SetProperty(ref isEnabled, value);
                log.LogMethodExit(isEnabled);
            }
        }

        public int Key
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(key);
                return key;
            }
            set
            {
                log.LogMethodEntry(key, value);
                SetProperty(ref key, value);
                log.LogMethodExit(key);
            }
        }
        public int? ViewGroupingNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(viewGroupingNumber);
                return viewGroupingNumber;
            }
            set
            {
                log.LogMethodEntry(viewGroupingNumber, value);
                viewGroupingNumber = value;
                log.LogMethodExit(viewGroupingNumber);
            }
        }

        public GenericTransactionListItemType RedemptionRightSectionItemType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionRightSectionItemType);
                return redemptionRightSectionItemType;
            }
            set
            {
                log.LogMethodEntry(redemptionRightSectionItemType, value);
                SetProperty(ref redemptionRightSectionItemType, value);
                SetTicketText();
                log.LogMethodExit(ticket);
            }
        }

        public string ProductName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(productName);
                return productName;
            }
            set
            {
                log.LogMethodEntry(productName, value);
                SetProperty(ref productName, value);
                log.LogMethodExit(productName);
            }
        }

        public int Ticket
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticket);
                return ticket;
            }
            set
            {
                log.LogMethodEntry(ticket, value);
                SetProperty(ref ticket, value);
                SetTicketText();
                log.LogMethodExit(ticket);
            }
        }

        public int Count
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(count);
                return count;
            }
            set
            {
                log.LogMethodEntry(count, value);
                SetProperty(ref count, value);
                SetTicketText();
                log.LogMethodExit(count);
            }
        }

        public string TicketText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticketText);
                return ticketText;
            }
            private set
            {
                log.LogMethodEntry(ticketText, value);
                SetProperty(ref ticketText, value);
                log.LogMethodExit(ticketText);
            }
        }

        public string TicketDisplayText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticketDisplayText);
                return ticketDisplayText;
            }
            set
            {
                log.LogMethodEntry(ticketDisplayText, value);
                SetProperty(ref ticketDisplayText, value);
                log.LogMethodExit(ticketDisplayText);
            }
        }

        public string TotalText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(totalText);
                return totalText;
            }
            private set
            {
                log.LogMethodEntry(totalText, value);
                SetProperty(ref totalText, value);
                log.LogMethodExit(totalText);
            }
        }

        public string TicketNo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticketNo);
                return ticketNo;
            }
            set
            {
                log.LogMethodEntry(ticketNo, value);
                SetProperty(ref ticketNo, value);
                log.LogMethodExit(ticketNo);
            }
        }
        #endregion

        #region Methods
        private void SetTicketText()
        {
            if (redemptionRightSectionItemType == GenericTransactionListItemType.Item)
            {
                TicketText = Count.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")) + " x "
                                    + Ticket.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")) + " " + ticketDisplayText;
                TotalText = (ticket * count).ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT"));
            }
            else
            {
                TicketText = "#" + ticketNo;
                ProductName = MessageViewContainerList.GetMessage(ExecutionContext, "Ticket");
                TotalText = ticket.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT"));
            }
        }
        #endregion

        #region Constructor
        public GenericTransactionListItem(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            productName = string.Empty;
            ticketText = string.Empty;
            ticketDisplayText = string.Empty;
            totalText = string.Empty;
            ticketNo = string.Empty;
            count = 0;
            ticket = 0;
            key = -1;
            isEnabled = true;
            redemptionRightSectionItemType = GenericTransactionListItemType.Item;
            log.LogMethodExit();
        }
        #endregion
    }
}
