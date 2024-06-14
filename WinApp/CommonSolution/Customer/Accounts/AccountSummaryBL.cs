/********************************************************************************************
 * Project Name - AccountSummaryBL
 * Description  - BL for Manage Account Summary Details
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.70.2      06-Nov-2019       Jinto Thomas       Modified GetCreditPlusTime() method
 *2.70.2      11-Nov-2019       Nitin Pai          Guest App related changes
 *2.80.0      19-Mar-2020       Mathew NInan       Use new field ValidityStatus to track
 *                                                 status of entitlements
 *2.80.0      20-Apr-2020       Lakshminarayana    Linux reader Account expiry date and total loyalty points display
 *2.130.0     19-July-2021      Girish Kundar      Modified : CreditPlusVirtualPoints column added part of Arcade changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Globalization;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account Credit plus summary class
    /// </summary>
    public class AccountSummaryBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AccountSummaryDTO accountSummaryDTO;
        private DateTime serverDate;
        private readonly ExecutionContext executionContext;
        private readonly bool includeFuture;


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountDTO"></param>
        /// <param name="includeFuture"></param>
        public AccountSummaryBL(ExecutionContext executionContext, AccountDTO accountDTO, bool includeFuture = false)
        {
            log.LogMethodEntry(accountDTO);
            this.executionContext = executionContext;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            this.includeFuture = includeFuture;
            serverDate = lookupValuesList.GetServerDateTime();
            accountSummaryDTO = new AccountSummaryDTO();
            CalculateAccountSummary(accountDTO);
            log.LogMethodExit();
        }

        private void CalculateAccountSummary(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            CalculateCreditPlusSummary(accountDTO);
            CalculateGamesSummary(accountDTO);
            UpdateAccountSummary(accountDTO);
            log.LogMethodExit();
        }

        private void UpdateAccountSummary(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            accountSummaryDTO.TotalGamePlayCreditsBalance = (accountDTO.Credits.HasValue? accountDTO.Credits.Value : 0) + accountSummaryDTO.CreditPlusGamePlayCredits.Value + accountSummaryDTO.CreditPlusCardBalance.Value;
            accountSummaryDTO.TotalBonusBalance = (accountDTO.Bonus.HasValue ? accountDTO.Bonus.Value : 0) + accountSummaryDTO.CreditPlusBonus.Value;
            if(ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_TOTAL_CREDITS_AND_BONUS_BALANCE"))
            {
                accountSummaryDTO.TotalCourtesyBalance = accountSummaryDTO.TotalGamePlayCreditsBalance + accountSummaryDTO.TotalBonusBalance;
            }
            else
            {
                accountSummaryDTO.TotalCourtesyBalance = (accountDTO.Courtesy.HasValue ? accountDTO.Courtesy.Value : 0);
            }
            accountSummaryDTO.TotalTimeBalance = accountSummaryDTO.CreditPlusTime.Value + GetAccountTimeBalance(accountDTO);
            accountSummaryDTO.TotalTicketsBalance = (accountDTO.TicketCount.HasValue ? accountDTO.TicketCount.Value : 0) + accountSummaryDTO.CreditPlusTickets.Value;
            
            //Virtual arcade
            accountSummaryDTO.TotalVirtualPointBalance =  accountSummaryDTO.CreditPlusVirtualPoints.Value;
            // ends here

            accountSummaryDTO.TotalGamesBalance = (accountDTO.TechGames.HasValue ? accountDTO.TechGames.Value : 0) + accountSummaryDTO.AccountGameBalance;
            accountSummaryDTO.TotalLoyaltyPointBalance =
                (accountDTO.LoyaltyPoints.HasValue ? accountDTO.LoyaltyPoints.Value : 0) +
                accountSummaryDTO.CreditPlusLoyaltyPoints;
            accountSummaryDTO.AccountExpiryDate = GetAccountExpiryDate(accountDTO); 

            string readerPriceDisplayFormat =
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "READER_PRICE_DISPLAY_FORMAT",
                    "##0.00");
            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT",
                "##0.00");
            //accountSummaryDTO.FormattedCreditPlusCardBalance = (accountSummaryDTO.CreditPlusCardBalance).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedCreditPlusGamePlayCredits = (accountSummaryDTO.CreditPlusGamePlayCredits).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedCreditPlusItemPurchase = (accountSummaryDTO.CreditPlusItemPurchase).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedCreditPlusBonus = (accountSummaryDTO.CreditPlusBonus).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedCreditPlusLoyaltyPoints = (accountSummaryDTO.CreditPlusLoyaltyPoints).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedCreditPlusTickets = (accountSummaryDTO.CreditPlusTickets).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedCreditPlusTime = (accountSummaryDTO.CreditPlusTime).Value.ToString().PadLeft(3, ' ').Substring(0, 3) + MessageContainerList.GetMessage(executionContext, " Mins"); 
            //accountSummaryDTO.FormattedCreditPlusRefundableBalance = (accountSummaryDTO.CreditPlusRefundableBalance).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedRedeemableCreditPlusLoyaltyPoints = (accountSummaryDTO.RedeemableCreditPlusLoyaltyPoints).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);

            //accountSummaryDTO.FormattedTotalGamePlayCreditsBalance = (accountSummaryDTO.TotalGamePlayCreditsBalance).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedTotalBonusBalance = (accountSummaryDTO.TotalBonusBalance).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedTotalCourtesyBalance = (accountSummaryDTO.TotalCourtesyBalance).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedTotalTimeBalance = (accountSummaryDTO.TotalTimeBalance).Value.ToString(CultureInfo.InvariantCulture).PadLeft(3, ' ').Substring(0, 3) + MessageContainerList.GetMessage(executionContext, " Mins");
            //accountSummaryDTO.FormattedTotalGamesBalance = ((int)(accountSummaryDTO.TotalGamesBalance).Value).ToString().PadLeft(4, ' ').Substring(0, 4);
            //accountSummaryDTO.FormattedTotalTicketsBalance = (accountSummaryDTO.TotalTicketsBalance).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedTotalLoyaltyPointBalance = (accountSummaryDTO.TotalLoyaltyPointBalance).Value.ToString(readerPriceDisplayFormat).PadLeft(6, ' ').Substring(0, 6);
            //accountSummaryDTO.FormattedAccountExpiryDate = accountSummaryDTO.AccountExpiryDate.HasValue == false ? new string(' ', 10) : (accountSummaryDTO.AccountExpiryDate).Value.ToString(dateFormat).PadLeft(6, ' ').Substring(0, 6);
            
            //Virtual arcade
            accountSummaryDTO.FormattedCreditPlusVirtualPointBalance = (accountSummaryDTO.CreditPlusVirtualPoints).Value.ToString(readerPriceDisplayFormat); 
            // ends here

            accountSummaryDTO.FormattedCreditPlusCardBalance = (accountSummaryDTO.CreditPlusCardBalance).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedCreditPlusGamePlayCredits = (accountSummaryDTO.CreditPlusGamePlayCredits).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedCreditPlusItemPurchase = (accountSummaryDTO.CreditPlusItemPurchase).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedCreditPlusBonus = (accountSummaryDTO.CreditPlusBonus).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedCreditPlusLoyaltyPoints = (accountSummaryDTO.CreditPlusLoyaltyPoints).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedCreditPlusTickets = (accountSummaryDTO.CreditPlusTickets).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedCreditPlusTime = (accountSummaryDTO.CreditPlusTime).Value.ToString() + MessageContainerList.GetMessage(executionContext, " Mins");
            accountSummaryDTO.FormattedCreditPlusRefundableBalance = (accountSummaryDTO.CreditPlusRefundableBalance).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedRedeemableCreditPlusLoyaltyPoints = (accountSummaryDTO.RedeemableCreditPlusLoyaltyPoints).Value.ToString(readerPriceDisplayFormat);

            accountSummaryDTO.FormattedTotalGamePlayCreditsBalance = (accountSummaryDTO.TotalGamePlayCreditsBalance).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedTotalBonusBalance = (accountSummaryDTO.TotalBonusBalance).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedTotalCourtesyBalance = (accountSummaryDTO.TotalCourtesyBalance).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedTotalTimeBalance = (accountSummaryDTO.TotalTimeBalance).Value.ToString(CultureInfo.InvariantCulture) + MessageContainerList.GetMessage(executionContext, " Mins");
            accountSummaryDTO.FormattedTotalGamesBalance = ((int)(accountSummaryDTO.TotalGamesBalance).Value).ToString();
            accountSummaryDTO.FormattedTotalTicketsBalance = (accountSummaryDTO.TotalTicketsBalance).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedTotalLoyaltyPointBalance = (accountSummaryDTO.TotalLoyaltyPointBalance).Value.ToString(readerPriceDisplayFormat);
            accountSummaryDTO.FormattedAccountExpiryDate = accountSummaryDTO.AccountExpiryDate.HasValue == false ? new string(' ', 10) : (accountSummaryDTO.AccountExpiryDate).Value.ToString(dateFormat);

            log.LogMethodExit();
        }

        private DateTime? GetAccountExpiryDate(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            DateTime? result;
            if (accountDTO.ExpiryDate.HasValue)
            {
                result = accountDTO.ExpiryDate.Value;
                log.LogMethodExit(result, "Account Expiry Date");
                return result;
            }

            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_EXPIRY_RULE") == "LASTACTIVITY")
            {
                int cardValidity =
                    ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_VALIDITY", 12);
                result = accountDTO.LastUpdateDate.AddMonths(cardValidity);
                log.LogMethodExit(result, "CARD_EXPIRY_RULE LASTACTIVITY");
                return result;
            }
            else if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_EXPIRY_RULE") == "NONE")
            {
                log.LogMethodExit(null, "CARD_EXPIRY_RULE NONE");
                return null;
            }
            log.LogMethodExit(null, "CARD_EXPIRY_RULE NOT SET NO EXPIRY DATE");
            return null;
        }

        private decimal GetAccountTimeBalance(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            decimal result = 0;
            AccountBL accountBL = new AccountBL(executionContext, accountDTO);
            result = accountBL.GetAccountTimeBalance();
            //if(accountDTO.Time.HasValue == false)
            //{
            //    log.LogMethodExit(result, "Time is null");
            //    return result;
            //}
            //if (accountDTO.StartTime.HasValue == false)
            //{
            //    result = accountDTO.Time.Value;
            //    log.LogMethodExit(result, "Start time is null");
            //    return result;
            //}
            //DateTime expiryDate = accountDTO.StartTime.Value.AddMinutes((double)accountDTO.Time.Value);
            //result = (decimal)(((expiryDate) - serverDate).TotalMinutes);
            //if (result < 0)
            //{
            //    result = 0;
            //}
            log.LogMethodExit(result);
            return result;
        }

        private void CalculateGamesSummary(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            if (accountDTO.AccountGameDTOList == null ||
                accountDTO.AccountGameDTOList.Count == 0)
            {
                log.LogMethodExit(null, "Empty AccountGameDTOList");
                return;
            }
            foreach (var accountGameDTO in accountDTO.AccountGameDTOList)
            {
                //Added condition to call updateCreditPlusSummary only if ValidityStatus == "Y"
                if (accountGameDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Valid)
                    UpdateGameSummary(accountGameDTO);
            }
            log.LogMethodExit();
        }

        private void CalculateCreditPlusSummary(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            if (accountDTO.AccountCreditPlusDTOList == null ||
                accountDTO.AccountCreditPlusDTOList.Count == 0)
            {
                log.LogMethodExit(null, "Empty AccountCreditPlusDTOList");
                return;
            }
            foreach (var accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
            {
                //Added condition to call updateCreditPlusSummary only if ValidityStatus == "Y"
                if (accountCreditPlusDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Valid)
                    UpdateCreditPlusSummary(accountCreditPlusDTO);
            }
            log.LogMethodExit();
        }

        private void UpdateGameSummary(AccountGameDTO accountGameDTO)
        {
            log.LogMethodEntry(accountGameDTO);
            if(accountGameDTO.ExpiryDate.HasValue && accountGameDTO.ExpiryDate.Value < serverDate)
            {
                log.LogMethodExit(null, "Expired account game");
                return;
            }
            accountSummaryDTO.AccountGameBalance += GetBalanceGames(accountGameDTO);
            log.LogMethodExit();
        }

        private int GetBalanceGames(AccountGameDTO accountGameDTO)
        {
            log.LogMethodEntry(accountGameDTO);
            int result = 0;
            if(accountGameDTO.LastPlayedTime.HasValue == false)
            {
                result = Convert.ToInt32(accountGameDTO.BalanceGames);
                log.LogMethodExit(result);
                return result;
            }
            switch (accountGameDTO.Frequency)
            {
                case "N":
                    {
                        result = accountGameDTO.BalanceGames;
                        break;
                    }
                case "D":
                    {
                        if(GetUniqueYearDay(accountGameDTO.LastPlayedTime.Value) == GetUniqueYearDay(serverDate))
                        {
                            result = accountGameDTO.BalanceGames;   
                        }
                        else
                        {
                            result = Convert.ToInt32(accountGameDTO.Quantity);
                        }
                        break;
                    }
                case "W":
                    {
                        if (GetUniqueYearWeek(accountGameDTO.LastPlayedTime.Value) == GetUniqueYearWeek(serverDate))
                        {
                            result = accountGameDTO.BalanceGames;
                        }
                        else
                        {
                            result = Convert.ToInt32(accountGameDTO.Quantity);
                        }
                        break;
                    }
                case "M":
                    {
                        if (GetUniqueYearMonth(accountGameDTO.LastPlayedTime.Value) == GetUniqueYearMonth(serverDate))
                        {
                            result = accountGameDTO.BalanceGames;
                        }
                        else
                        {
                            result = Convert.ToInt32(accountGameDTO.Quantity);
                        }
                        break;
                    }
                case "Y":
                    {
                        if (accountGameDTO.LastPlayedTime.Value.Year == serverDate.Year)
                        {
                            result = accountGameDTO.BalanceGames;
                        }
                        else
                        {
                            result = Convert.ToInt32(accountGameDTO.Quantity);
                        }
                        break;
                    }
            }
            log.LogMethodExit(result);
            return result;
        }

        private int GetUniqueYearDay(DateTime value)
        {
            return value.Year * 1000 + value.DayOfYear;
        }

        private int GetUniqueYearWeek(DateTime value)
        {
            return value.Year * 1000 + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private int GetUniqueYearMonth(DateTime value)
        {
            return value.Year * 1000 + value.Month;
        }

        private void UpdateCreditPlusSummary(AccountCreditPlusDTO accountCreditPlusDTO)
        {
            log.LogMethodEntry(accountCreditPlusDTO);
            if (accountCreditPlusDTO.PeriodFrom.HasValue &&
                accountCreditPlusDTO.PeriodFrom.Value > serverDate && !includeFuture)
            {
                log.LogMethodExit(null, "Credit plus entry is not yet active");
                return;
            }
            if (accountCreditPlusDTO.PeriodTo.HasValue &&
                accountCreditPlusDTO.PeriodTo.Value < serverDate)
            {
                log.LogMethodExit(null, "Credit plus entry is expired");
                return;
            }
            if (accountCreditPlusDTO.CreditPlusBalance.HasValue == false)
            {
                log.LogMethodExit(null, "Empty Credit plus balance");
                return;
            }
            switch (accountCreditPlusDTO.CreditPlusType)
            {
                case CreditPlusType.CARD_BALANCE:
                    {
                        accountSummaryDTO.CreditPlusCardBalance = accountSummaryDTO.CreditPlusCardBalance + accountCreditPlusDTO.CreditPlusBalance;
                        if(accountCreditPlusDTO.Refundable)
                        {
                            accountSummaryDTO.CreditPlusRefundableBalance = accountSummaryDTO.CreditPlusRefundableBalance + accountCreditPlusDTO.CreditPlusBalance;
                        }
                        break;
                    }
                case CreditPlusType.LOYALTY_POINT:
                    {
                        accountSummaryDTO.CreditPlusLoyaltyPoints = accountSummaryDTO.CreditPlusLoyaltyPoints + accountCreditPlusDTO.CreditPlusBalance;
                        if(accountCreditPlusDTO.ForMembershipOnly == false)
                        {
                            accountSummaryDTO.RedeemableCreditPlusLoyaltyPoints = accountSummaryDTO.RedeemableCreditPlusLoyaltyPoints + accountCreditPlusDTO.CreditPlusBalance;
                        }
                        break;
                    }
                    
                case CreditPlusType.TICKET:
                    {
                        accountSummaryDTO.CreditPlusTickets = accountSummaryDTO.CreditPlusTickets + accountCreditPlusDTO.CreditPlusBalance;
                        break;
                    }
                case CreditPlusType.GAME_PLAY_CREDIT:
                    {
                        accountSummaryDTO.CreditPlusGamePlayCredits = accountSummaryDTO.CreditPlusGamePlayCredits + accountCreditPlusDTO.CreditPlusBalance;
                        if (accountCreditPlusDTO.Refundable)
                        {
                            accountSummaryDTO.CreditPlusRefundableBalance = accountSummaryDTO.CreditPlusRefundableBalance + accountCreditPlusDTO.CreditPlusBalance;
                        }
                        break;
                    }
                case CreditPlusType.COUNTER_ITEM:
                    {
                        accountSummaryDTO.CreditPlusItemPurchase = accountSummaryDTO.CreditPlusItemPurchase + accountCreditPlusDTO.CreditPlusBalance;
                        if (accountCreditPlusDTO.Refundable)
                        {
                            accountSummaryDTO.CreditPlusRefundableBalance = accountSummaryDTO.CreditPlusRefundableBalance + accountCreditPlusDTO.CreditPlusBalance;
                        }
                        break;
                    }
                case CreditPlusType.GAME_PLAY_BONUS:
                    {
                        accountSummaryDTO.CreditPlusBonus = accountSummaryDTO.CreditPlusBonus + accountCreditPlusDTO.CreditPlusBalance;
                        break;
                    }
                case CreditPlusType.TIME:
                    {
                        accountSummaryDTO.CreditPlusTime = accountSummaryDTO.CreditPlusTime + GetCreditPlusTime(accountCreditPlusDTO);
                        break;
                    }
                case CreditPlusType.VIRTUAL_POINT:
                    {
                        accountSummaryDTO.CreditPlusVirtualPoints = accountSummaryDTO.CreditPlusVirtualPoints + accountCreditPlusDTO.CreditPlusBalance;
                        break;
                    }
            }
            log.LogMethodExit();
        }

        private decimal GetCreditPlusTime(AccountCreditPlusDTO accountCreditPlusDTO)
        {
            log.LogMethodEntry(accountCreditPlusDTO);
            decimal result = 0;
            AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, accountCreditPlusDTO);
            result = accountCreditPlusBL.GetCreditPlusTime(serverDate);
            log.LogMethodExit(result);
            return result;
        }

        internal AccountSummaryDTO AccountSummaryDTO
        {
            get
            {
                return accountSummaryDTO;
            }
        }

    }
}
