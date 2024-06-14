/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - model for AccountRelationShip
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Parafait.AccountsUI;

namespace Semnox.Parafait.TransactionUI
{
    public class AccountRelationShipModel
    {
        #region members
        private int accountRelationshipId;
        private int? dailyLimitPercentage;
        private CardDetailsVM childCardDetailsVM;
        #endregion

        #region properties
        public int AccountRelationshipId
        {
            get
            {
                return accountRelationshipId;
            }
            set
            {
                accountRelationshipId = value;
            }
        }

        public int? DailyLimitPercentage
        {
            get
            {
                return dailyLimitPercentage;
            }
            set
            {
                dailyLimitPercentage = value;
            }
        }

        public CardDetailsVM ChildCardDetailsVM
        {
            get
            {
                return childCardDetailsVM;
            }
            set
            {
                childCardDetailsVM = value;
            }
        }
        #endregion
    }
}
