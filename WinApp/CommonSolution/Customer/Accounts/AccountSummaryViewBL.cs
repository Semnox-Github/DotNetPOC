/********************************************************************************************
 * Project Name - Acconunt BL
 * Description  - Business Logic of the Account
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By         Remarks          
 *********************************************************************************************
 *2.130.11   08-Aug-2022    Yashodhara C H       Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Customer.Accounts
{
    public class AccountSummaryViewBL
    {
        private AccountSummaryViewDTO accountSummaryViewDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;


        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public AccountSummaryViewBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor having accountId
        /// </summary>
        public AccountSummaryViewBL(ExecutionContext executionContext, int accountId)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountId);
            AccountSummaryViewDataHandler accountSummaryViewDataHandler = new AccountSummaryViewDataHandler();
            accountSummaryViewDTO = accountSummaryViewDataHandler.GetAccountSummaryViewDTO(accountId);
            if (accountSummaryViewDTO == null)
            {
                string errorMessage = "Object not found";
                log.Error(errorMessage + " " + accountId);
                throw new ValidationException(errorMessage);
            }
            AccountSummaryViewBuilderBL accountSummaryViewBuilderBL = new AccountSummaryViewBuilderBL(executionContext, accountSummaryViewDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with accountSummaryViewDTO as parameter
        /// </summary>
        /// <param name="accountSummaryViewDTO"></param>
        private AccountSummaryViewBL(ExecutionContext executionContext, AccountSummaryViewDTO accountSummaryViewDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.accountSummaryViewDTO = accountSummaryViewDTO;
            log.LogMethodExit();
        }
    }
    public class AccountSummaryViewListBL
    {
        /// <summary>
        /// AccountSummaryView business logic 
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AccountSummaryViewListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public AccountSummaryViewListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AccountSummary list
        /// </summary>
        public List<AccountSummaryViewDTO> GetAccountSummaryViewDTOList(List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>> searchParameter, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameter);
            AccountSummaryViewDataHandler accountSummaryDataHandler = new AccountSummaryViewDataHandler(sqlTransaction);
            List<AccountSummaryViewDTO> accountSummaryViewDTOsList = accountSummaryDataHandler.GetAccountSummaryViewDTOList(searchParameter, sqlTransaction);
            if(accountSummaryViewDTOsList != null && accountSummaryViewDTOsList.Any())
            {
                foreach(AccountSummaryViewDTO accountSummaryViewDTO in accountSummaryViewDTOsList)
                {
                    AccountSummaryViewBuilderBL accountSummaryViewBuilderBL = new AccountSummaryViewBuilderBL(executionContext, accountSummaryViewDTO);
                }
            }
            log.LogMethodExit(accountSummaryViewDTOsList);
            return accountSummaryViewDTOsList;
        }

    }

    public class AccountSummaryViewBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AccountSummaryViewDTO accountSummaryViewDTO;

        public AccountSummaryViewBuilderBL (ExecutionContext executionContext, AccountSummaryViewDTO accountSummaryViewDTO)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.accountSummaryViewDTO = accountSummaryViewDTO;
            Build();
            log.LogMethodExit(executionContext);
        }

         private void Build()
        {
            accountSummaryViewDTO.TotalGamePlayCreditsBalance = (accountSummaryViewDTO.Credits.HasValue ? accountSummaryViewDTO.Credits.Value : 0) + (accountSummaryViewDTO.CreditPlusCardBalance.HasValue ? accountSummaryViewDTO.CreditPlusCardBalance.Value : 0) + (accountSummaryViewDTO.CreditPlusCredits.HasValue ? accountSummaryViewDTO.CreditPlusCredits.Value : 0);
            accountSummaryViewDTO.TotalBonusBalance = (accountSummaryViewDTO.Bonus.HasValue ? accountSummaryViewDTO.Bonus.Value : 0) + (accountSummaryViewDTO.CreditPlusBonus.HasValue ? accountSummaryViewDTO.CreditPlusBonus.Value : 0);
            accountSummaryViewDTO.TotalCreditsBalance = (accountSummaryViewDTO.Credits.HasValue ? accountSummaryViewDTO.Credits.Value : 0) + (accountSummaryViewDTO.CreditPlusCardBalance.HasValue ? accountSummaryViewDTO.CreditPlusCardBalance.Value : 0) + (accountSummaryViewDTO.CreditPlusItemPurchase.HasValue ? accountSummaryViewDTO.CreditPlusItemPurchase.Value : 0); 
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_TOTAL_CREDITS_AND_BONUS_BALANCE"))
            {
                accountSummaryViewDTO.TotalCourtesyBalance = accountSummaryViewDTO.TotalGamePlayCreditsBalance + accountSummaryViewDTO.TotalBonusBalance;
            }
            else
            {
                accountSummaryViewDTO.TotalCourtesyBalance = (accountSummaryViewDTO.Courtesy.HasValue ? accountSummaryViewDTO.Courtesy.Value : 0);
            }
            accountSummaryViewDTO.TotalTimeBalance = (accountSummaryViewDTO.CreditPlusTime.HasValue ? accountSummaryViewDTO.CreditPlusTime.Value : 0) + GetAccountTimeBalance();
            accountSummaryViewDTO.TotalTicketsBalance = (accountSummaryViewDTO.TicketCount.HasValue ? accountSummaryViewDTO.TicketCount.Value : 0) + (accountSummaryViewDTO.CreditPlusTickets.HasValue ? accountSummaryViewDTO.CreditPlusTickets.Value : 0);
            accountSummaryViewDTO.TotalGamesBalance = (accountSummaryViewDTO.TechGames.HasValue ? accountSummaryViewDTO.TechGames.Value : 0) + accountSummaryViewDTO.GamesBalance;
            accountSummaryViewDTO.TotalLoyaltyBalance = (accountSummaryViewDTO.LoyaltyPoints.HasValue ? accountSummaryViewDTO.LoyaltyPoints.Value : 0) + (accountSummaryViewDTO.CreditPlusLoyaltyPoints.HasValue? accountSummaryViewDTO.CreditPlusLoyaltyPoints.Value : 0);
            accountSummaryViewDTO.ExpiryDate = GetAccountExpiryDate();

        }

        private decimal GetAccountTimeBalance()
        {
            log.LogMethodEntry();
            decimal result = 0;
            if (accountSummaryViewDTO.Time.HasValue == false)
            {
                log.LogMethodExit(result, "Time is null");
                return result;
            }
            if (accountSummaryViewDTO.StartTime.HasValue == false)
            {
                result = accountSummaryViewDTO.Time.Value;
                log.LogMethodExit(result, "Start time is null");
                return result;
            }
            DateTime expiryDate = accountSummaryViewDTO.StartTime.Value.AddMinutes((double)accountSummaryViewDTO.Time.Value);
            result = (decimal)(((expiryDate) - ServerDateTime.Now).TotalMinutes);
            if (result < 0)
            {
                result = 0;
            }
            log.LogMethodExit(result);
            return result;
        }

        private DateTime? GetAccountExpiryDate()
        {
            log.LogMethodEntry();
            DateTime? result;
            if (accountSummaryViewDTO.ExpiryDate.HasValue)
            {
                result = accountSummaryViewDTO.ExpiryDate.Value;
                log.LogMethodExit(result, "Account Expiry Date");
                return result;
            }

            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_EXPIRY_RULE") == "LASTACTIVITY")
            {
                int cardValidity =
                    ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_VALIDITY", 12);
                result = accountSummaryViewDTO.LastUpdatedTime.AddMonths(cardValidity);
                log.LogMethodExit(result, "CARD_EXPIRY_RULE LASTACTIVITY");
                return result;
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_EXPIRY_RULE") == "NONE")
            {
                log.LogMethodExit(null, "CARD_EXPIRY_RULE NONE");
                return null;
            }
            log.LogMethodExit(null, "CARD_EXPIRY_RULE NOT SET NO EXPIRY DATE");
            return null;
        }
    }
}
