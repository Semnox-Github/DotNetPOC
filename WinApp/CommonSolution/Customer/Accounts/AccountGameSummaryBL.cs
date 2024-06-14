/********************************************************************************************
 * Project Name - Customer
 * Description  - AccountGameSummaryBl class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks          
 *********************************************************************************************
 **2.150.02   27-Mar-2023     Yashodhara C H        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountGameSummaryList Class
    /// </summary>
    public class AccountGameSummaryBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AccountGameSummaryDTO accountGameSummaryDTO;
        private List<AccountGameExtendedSummaryBL> accountGameExtendedSummaryBLList = new List<AccountGameExtendedSummaryBL>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountGameSummaryDTO"></param>
        public AccountGameSummaryBL(ExecutionContext executionContext, AccountGameSummaryDTO accountGameSummaryDTO)
        {
            log.LogMethodEntry(executionContext, accountGameSummaryDTO);
            this.accountGameSummaryDTO = accountGameSummaryDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Adds the child AccountGameExtendedSummaryBL
        /// </summary>
        /// <param name="accountGameExtendedSummaryBL"></param>
        public void AddChild(AccountGameExtendedSummaryBL accountGameExtendedSummaryBL)
        {
            log.LogMethodEntry(accountGameExtendedSummaryBL);
            accountGameExtendedSummaryBLList.Add(accountGameExtendedSummaryBL);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the accountGameSummaryDTO
        /// </summary>
        public AccountGameSummaryDTO GetAccountGameSummaryDTO
        {
            get
            {
                log.LogMethodEntry();
                AccountGameSummaryDTO result = new AccountGameSummaryDTO(accountGameSummaryDTO);
                List<AccountGameExtendedSummaryDTO> accountGameExtendedSummaryDTOList = new List<AccountGameExtendedSummaryDTO>();
                foreach(var accountGameExtendedSummaryBL in accountGameExtendedSummaryBLList)
                {
                    accountGameExtendedSummaryDTOList.Add(accountGameExtendedSummaryBL.AccountGameExtendedSummaryDTO);
                }
                result.AccountGameExtendedSummaryDTOList = accountGameExtendedSummaryDTOList;
                log.LogMethodExit(result);
                return result;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountGameId field
        /// </summary>
        public int AccountGameId
        {
            get
            {
                return accountGameSummaryDTO.AccountGameId;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountId field
        /// </summary>
        public int AccountId
        {
            get
            {
                return accountGameSummaryDTO.AccountId;
            }
        }

        /// <summary>
        /// Get/Set Method of GameId field
        /// </summary>
        public int GameId
        {
            get
            {
                return accountGameSummaryDTO.GameId;
            }
        }

        /// <summary>
        /// Get/Set Method of GameName field
        /// </summary>
        public string GameName
        {
            get
            {
                return accountGameSummaryDTO.GameName;
            }
        }

        /// <summary>
        /// Get/Set Method of Quantity field
        /// </summary>
        public decimal Quantity
        {
            get
            {
                return accountGameSummaryDTO.Quantity;
            }
        }

        /// <summary>
        /// Get/Set Method of ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate
        {
            get
            {
                return accountGameSummaryDTO.ExpiryDate;
            }
        }

        /// <summary>
        /// Get/Set Method of GameProfileId field
        /// </summary>
        public int GameProfileId
        {
            get
            {
                return accountGameSummaryDTO.GameProfileId;
            }
        }


        /// <summary>
        /// Get/Set Method of GameProfileName field
        /// </summary>
        public string GameProfileName
        {
            get
            {
                return accountGameSummaryDTO.GameProfileName;
            }
        }

        /// <summary>
        /// Get/Set Method of Frequency field
        /// </summary>
        public string Frequency
        {
            get
            {
                return accountGameSummaryDTO.Frequency;
            }
        }

        /// <summary>
        /// Get/Set Method of LastPlayedTime field
        /// </summary>
        public DateTime? LastPlayedTime
        {
            get
            {
                return accountGameSummaryDTO.LastPlayedTime;
            }
        }

        /// <summary>
        /// Get/Set Method of BalanceGame field
        /// </summary>
        public int BalanceGame
        {
            get
            {
                return accountGameSummaryDTO.BalanceGames;
            }
        }

        /// <summary>
        /// Get/Set Method of TransactionId field
        /// </summary>
        public int TransactionId
        {
            get
            {
                return accountGameSummaryDTO.TransactionId;
            }
        }

        /// <summary>
        /// Get/Set Method of TransactionLineId field
        /// </summary>
        public int TransactionLineId
        {
            get
            {
                return accountGameSummaryDTO.TransactionLineId;
            }
        }

        /// <summary>
        /// Get/Set Method of EntitlementType field
        /// </summary>
        public string EntitlementType
        {
            get
            {
                return accountGameSummaryDTO.EntitlementType;
            }
        }

        /// <summary>
        /// Get/Set Method of OptionalAttribute field
        /// </summary>
        public string OptionalAttribute
        {
            get
            {
                return accountGameSummaryDTO.OptionalAttribute;
            }
        }

        /// <summary>
        /// Get/Set Method of CustomDataSetId field
        /// </summary>
        public int CustomDataSetId
        {
            get
            {
                return accountGameSummaryDTO.CustomDataSetId;
            }
        }

        /// <summary>
        /// Get/Set Method of TicketAllowed field
        /// </summary>
        public bool TicketAllowed
        {
            get
            {
                return accountGameSummaryDTO.TicketAllowed;
            }
        }

        /// <summary>
        /// Get/Set Method of FromDate field
        /// </summary>
        public DateTime? FromDate
        {
            get
            {
                return accountGameSummaryDTO.FromDate;
            }
        }

        /// <summary>
        /// Get/Set Method of Monday field
        /// </summary>
        public bool Monday
        {
            get
            {
                return accountGameSummaryDTO.Monday;
            }
        }

        /// <summary>
        /// Get/Set Method of Tuesday field
        /// </summary>
        public bool Tuesday
        {
            get
            {
                return accountGameSummaryDTO.Tuesday;
            }
        }

        /// <summary>
        /// Get/Set Method of Wednesday field
        /// </summary>
        public bool Wednesday
        {
            get
            {
                return accountGameSummaryDTO.Wednesday;
            }
        }

        /// <summary>
        /// Get/Set Method of Thursday field
        /// </summary>
        public bool Thursday
        {
            get
            {
                return accountGameSummaryDTO.Thursday;
            }
        }

        /// <summary>
        /// Get/Set Method of Friday field
        /// </summary>
        public bool Friday
        {
            get
            {
                return accountGameSummaryDTO.Friday;
            }
        }

        /// <summary>
        /// Get/Set Method of Saturday field
        /// </summary>
        public bool Saturday
        {
            get
            {
                return accountGameSummaryDTO.Saturday;
            }
        }

        /// <summary>
        /// Get/Set Method of Sunday field
        /// </summary>
        public bool Sunday
        {
            get
            {
                return accountGameSummaryDTO.Sunday;
            }
        }

        /// <summary>
        /// Get/Set Method of ExpireWithMembership field
        /// </summary>
        public bool ExpireWithMembership
        {
            get
            {
                return accountGameSummaryDTO.ExpireWithMembership;
            }
        }

        /// <summary>
        /// Get/Set Method of MembershipId field
        /// </summary>
        public int MembershipId
        {
            get
            {
                return accountGameSummaryDTO.MembershipId;
            }
        }

        /// <summary>
        /// Get/Set Method of MembershipName field
        /// </summary>
        public string MembershipName
        {
            get
            {
                return accountGameSummaryDTO.MembershipName;
            }
        }

        /// <summary>
        /// Get/Set Method of MembershipRewardsId field
        /// </summary>
        public int MembershipRewardsId
        {
            get
            {
                return accountGameSummaryDTO.MembershipRewardsId;
            }
        }

        /// <summary>
        /// Get/Set Method of MembershipRewardsName field
        /// </summary>
        public string  MembershipRewardsName
        {
            get
            {
                return accountGameSummaryDTO.MembershipRewardsName;
            }
        }

        /// <summary>
        /// Get/Set Method of ValidityStatus field
        /// </summary>
        public AccountDTO.AccountValidityStatus ValidityStatus
        {
            get
            {
                return accountGameSummaryDTO.ValidityStatus;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountGameExtendedSummaryDTOList field
        /// </summary>
        public List<AccountGameExtendedSummaryDTO> AccountGameExtendedSummaryDTOList
        {
            get
            {
                return accountGameSummaryDTO.AccountGameExtendedSummaryDTOList;
            }
        }

        /// <summary>
        /// Get/Set Method of SubscriptionBillingScheduleId field
        /// </summary>
        public int SubscriptionBillingScheduleId
        {
            get
            {
                return accountGameSummaryDTO.SubscriptionBillingScheduleId;
            }
        }
    }
}
