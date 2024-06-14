/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Model for Transfer Balance
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     30-Mar-2021    Raja Uthanda           Created for POS UI Redesign 
 *2.130.2     13-Dec-2021    Deeksha                Modified as part of Transfer Balance to support playcredits,CounterItems,Time and Courtesy
 *******************************************************************************************/
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.TransactionUI
{
    public class TaskTransferChildModel : ViewModelBase
    {
        #region Members
        private string creditToBeTransfer;
        private string bonusToBeTransfer;
        private string ticketsToBeTransfer;
        private string playCreditsToBeTransfer;
        private string timeToBeTransfer;
        private string courtesytoBeTransfer;
        private string counterItemsToBeTransfer;

        private CardDetailsVM cardDetailsVM;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string CreditToBeTransfer
        {
            get
            {
                return creditToBeTransfer;
            }
            set
            {
                if (!object.Equals(creditToBeTransfer, value))
                {
                    creditToBeTransfer = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PlayCreditsToBeTransfer
        {
            get
            {
                return playCreditsToBeTransfer;
            }
            set
            {
                if (!object.Equals(playCreditsToBeTransfer, value))
                {
                    playCreditsToBeTransfer = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BonusToBeTransfer
        {
            get
            {
                return bonusToBeTransfer;
            }
            set
            {
                if (!object.Equals(bonusToBeTransfer, value))
                {
                    bonusToBeTransfer = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TicketsToBeTransfer
        {
            get
            {
                return ticketsToBeTransfer;
            }
            set
            {
                if (!object.Equals(ticketsToBeTransfer, value))
                {
                    ticketsToBeTransfer = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TimeToBeTransfer
        {
            get
            {
                return timeToBeTransfer;
            }
            set
            {
                if (!object.Equals(timeToBeTransfer, value))
                {
                    timeToBeTransfer = value;
                    OnPropertyChanged();
                }
            }
        }
        public string CourtesyToBeTransfer
        {
            get
            {
                return courtesytoBeTransfer;
            }
            set
            {
                if (!object.Equals(courtesytoBeTransfer, value))
                {
                    courtesytoBeTransfer = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CounterItemstoBeTransfer
        {
            get
            {
                return counterItemsToBeTransfer;
            }
            set
            {
                if (!object.Equals(counterItemsToBeTransfer, value))
                {
                    counterItemsToBeTransfer = value;
                    OnPropertyChanged();
                }
            }
        }

        public CardDetailsVM CardDetailsVM
        {
            get
            {
                return cardDetailsVM;
            }
            set
            {
                if (!object.Equals(cardDetailsVM, value))
                {
                    cardDetailsVM = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
