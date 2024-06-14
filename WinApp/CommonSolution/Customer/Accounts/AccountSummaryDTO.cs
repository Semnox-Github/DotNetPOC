/********************************************************************************************
 * Project Name - Semnox.Parafait.Customer.Accounts -AccountCreditPlusSummaryDTO
 * Description  - AccountCreditPlusSummaryDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.5.0       13-Dec-2018   Lakshminarayana         Created 
 *2.70.2        05-Aug-2019   Girish Kundar           Modified : Removed Unused namespace.
 *2.80        26-Apr-2020   Lakshminarayana         Linux reader Account expiry date and total loyalty points display 
 *2.130.0     19-July-2021   Girish Kundar      Modified : CreditPlusVirtualPoints column added part of Arcade changes
 ********************************************************************************************/

using System;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountCreditPlus summary data object class.
    /// </summary>
    public class AccountSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountSummaryDTO()
        {
            log.LogMethodEntry();
            CreditPlusCardBalance = 0;
            CreditPlusGamePlayCredits = 0;
            CreditPlusItemPurchase = 0;
            CreditPlusBonus = 0;
            CreditPlusLoyaltyPoints = 0;
            CreditPlusTickets = 0;
            CreditPlusTime = 0;
            CreditPlusRefundableBalance = 0;
            //Virtual arcade changes
            CreditPlusVirtualPoints = 0;
            RedeemableCreditPlusLoyaltyPoints = 0;
            AccountGameBalance = 0;

            TotalGamePlayCreditsBalance = 0;
            TotalBonusBalance = 0;
            TotalCourtesyBalance = 0;
            TotalTimeBalance = 0;
            TotalGamesBalance = 0;
            TotalTicketsBalance = 0;
            TotalLoyaltyPointBalance = 0;
            //Virtual arcade changes
            TotalVirtualPointBalance = 0;
            AccountExpiryDate = null;
            log.LogMethodExit();
        }

        public decimal? CreditPlusCardBalance { get; set; }
        public decimal? CreditPlusGamePlayCredits { get; set; }
        public decimal? CreditPlusItemPurchase { get; set; }
        public decimal? CreditPlusBonus { get; set; }
        public decimal? CreditPlusLoyaltyPoints { get; set; }
        public decimal? CreditPlusTickets { get; set; }
        public decimal? CreditPlusVirtualPoints { get; set; }
        public decimal? CreditPlusTime { get; set; }
        public decimal? CreditPlusRefundableBalance { get; set; }
        public decimal? RedeemableCreditPlusLoyaltyPoints { get; set; }
        public int AccountGameBalance { get; set; }
        public decimal? TotalGamePlayCreditsBalance { get; set; }
        public decimal? TotalBonusBalance { get; set; }
        public decimal? TotalCourtesyBalance { get; set; }
        public decimal? TotalTimeBalance { get; set; }
        public decimal? TotalVirtualPointBalance { get; set; }
        public decimal? TotalGamesBalance { get; set; }
        public decimal? TotalTicketsBalance { get; set; }
        public decimal? TotalLoyaltyPointBalance { get; set;}
        public DateTime? AccountExpiryDate { get; set;}


        public string FormattedCreditPlusCardBalance { get; set; }
        public string FormattedCreditPlusVirtualPointBalance { get; set; }
        public string FormattedCreditPlusGamePlayCredits { get; set; }
        public string FormattedCreditPlusItemPurchase { get; set; }
        public string FormattedCreditPlusBonus { get; set; }
        public string FormattedCreditPlusLoyaltyPoints { get; set; }
        public string FormattedCreditPlusTickets { get; set; }
        public string FormattedCreditPlusTime { get; set; }
        public string FormattedCreditPlusRefundableBalance { get; set; }
        public string FormattedRedeemableCreditPlusLoyaltyPoints { get; set; }
        public string FormattedTotalGamePlayCreditsBalance { get; set; }
        public string FormattedTotalBonusBalance { get; set; }
        public string FormattedTotalCourtesyBalance { get; set; }
        public string FormattedTotalTimeBalance { get; set; }
        public string FormattedTotalGamesBalance { get; set; }
        public string FormattedTotalTicketsBalance { get; set; }
        public string FormattedTotalLoyaltyPointBalance { get; set;}
        public string FormattedAccountExpiryDate { get; set;}
    }
}
