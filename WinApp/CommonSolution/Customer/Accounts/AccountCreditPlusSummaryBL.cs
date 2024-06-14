/********************************************************************************************
 * Project Name - Customer
 * Description  - AccountCreditPlusSummaryBl class
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
    ///  AccountCreditPlusSummaryList Class
    /// </summary>
    public class AccountCreditPlusSummaryBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AccountCreditPlusSummaryDTO accountCreditPlusSummaryDTO;
        private List<AccountCreditPlusConsumptionSummaryBL> accountCreditPlusConsumptionSummaryBLList = new List<AccountCreditPlusConsumptionSummaryBL>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountCreditPlusSummaryDTO"></param>
        public AccountCreditPlusSummaryBL(ExecutionContext executionContext, AccountCreditPlusSummaryDTO accountCreditPlusSummaryDTO)
        {
            log.LogMethodEntry(executionContext, accountCreditPlusSummaryDTO);
            this.accountCreditPlusSummaryDTO = accountCreditPlusSummaryDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Adds the child AccountCreditPlusConsumptionSummaryBL
        /// </summary>
        /// <param name="accountCreditPlusConsumptionSummaryBL"></param>
        public void AddChild(AccountCreditPlusConsumptionSummaryBL accountCreditPlusConsumptionSummaryBL)
        {
            log.LogMethodEntry(accountCreditPlusConsumptionSummaryBL);
            accountCreditPlusConsumptionSummaryBLList.Add(accountCreditPlusConsumptionSummaryBL);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the AccountCreditPlusSummaryDTO
        /// </summary>
        public AccountCreditPlusSummaryDTO GetAccountCreditPlusSummaryDTO
        {
            get
            {
                log.LogMethodEntry();
                AccountCreditPlusSummaryDTO result = new AccountCreditPlusSummaryDTO(accountCreditPlusSummaryDTO);
                List<AccountCreditPlusConsumptionSummaryDTO> accountCreditPlusConsumptionSummaryDTOList = new List<AccountCreditPlusConsumptionSummaryDTO>();
                foreach(var accountCreditPlusConsumptionSummaryBL in accountCreditPlusConsumptionSummaryBLList)
                {
                    accountCreditPlusConsumptionSummaryDTOList.Add(accountCreditPlusConsumptionSummaryBL.AccountCreditPlusConsumptionSummaryDTO);
                }
                result.AccountCreditPlusConsumptionSummaryDTOList = accountCreditPlusConsumptionSummaryDTOList;
                log.LogMethodExit(result);
                return result;
            }
        }


        /// <summary>
        /// Get/Set Method of AccountCreditPlusId field
        /// </summary>
        public int AccountCreditPlusId
        {
            get
            {
                return accountCreditPlusSummaryDTO.AccountCreditPlusId;
            }
        }

        /// <summary>
        /// Get/Set Method of CreditPlus field
        /// </summary>
        public decimal? CreditPlus
        {
            get
            {
                return accountCreditPlusSummaryDTO.CreditPlus;
            }
        }

        /// <summary>
        /// Get/Set Method of CreditPlusType field
        /// </summary>
        public CreditPlusType CreditPlusType
        {
            get
            {
                return accountCreditPlusSummaryDTO.CreditPlusType;
            }
        }

        /// <summary>
        /// Get/Set Method of CreditPlusName field
        /// </summary>
        public string CreditPlusTypeName
        {
            get
            {
                return accountCreditPlusSummaryDTO.CreditPlusTypeName;
            }
        }

        /// <summary>
        /// Get/Set Method of Refundable field
        /// </summary>
        public bool Refundable
        {
            get
            {
                return accountCreditPlusSummaryDTO.Refundable;
            }
        }

        /// <summary>
        /// Get/Set Method of Remarks field
        /// </summary>
        public string Remarks
        {
            get
            {
                return accountCreditPlusSummaryDTO.Remarks;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountId field
        /// </summary>
        public int AccountId
        {
            get
            {
                return accountCreditPlusSummaryDTO.AccountId;
            }
        }

        /// <summary>
        /// Get/Set Method of TransactionId field
        /// </summary>
        public int TransactionId
        {
            get
            {
                return accountCreditPlusSummaryDTO.TransactionId;
            }
        }

        /// <summary>
        /// Get/Set Method of TransactionLineId field
        /// </summary>
        public int TransactionLineId
        {
            get
            {
                return accountCreditPlusSummaryDTO.TransactionLineId;
            }
        }

        /// <summary>
        /// Get/Set Method of CreditPlusBalance field
        /// </summary>
        public decimal? CreditPlusBalance
        {
            get
            {
                return accountCreditPlusSummaryDTO.CreditPlusBalance;
            }
        }

        /// <summary>
        /// Get/Set Method of PeriodFrom field
        /// </summary>
        public DateTime? PeriodFrom
        {
            get
            {
                return accountCreditPlusSummaryDTO.PeriodFrom;
            }
        }

        /// <summary>
        /// Get/Set Method of PeriodTo field
        /// </summary>
        public DateTime? PeriodTo
        {
            get
            {
                return accountCreditPlusSummaryDTO.PeriodTo;
            }
        }

        /// <summary>
        /// Get/Set Method of TimeFrom field
        /// </summary>
        public decimal? TimeFrom
        {
            get
            {
                return accountCreditPlusSummaryDTO.TimeFrom;
            }
        }

        /// <summary>
        /// Get/Set Method of TimeTo field
        /// </summary>
        public decimal? TimeTo
        {
            get
            {
                return accountCreditPlusSummaryDTO.TimeTo;
            }
        }

        /// <summary>
        /// Get/Set Method of NumberOfDays field
        /// </summary>
        public int? NumberOfDays
        {
            get
            {
                return accountCreditPlusSummaryDTO.NumberOfDays;
            }
        }

        /// <summary>
        /// Get/Set Method of Monday field
        /// </summary>
        public bool Monday
        {
            get
            {
                return accountCreditPlusSummaryDTO.Monday;
            }
        }

        /// <summary>
        /// Get/Set Method of Tuesday field
        /// </summary>
        public bool Tuesday
        {
            get
            {
                return accountCreditPlusSummaryDTO.Tuesday;
            }
        }

        /// <summary>
        /// Get/Set Method of Wednesday field
        /// </summary>
        public bool Wednesday
        {
            get
            {
                return accountCreditPlusSummaryDTO.Wednesday;
            }
        }

        /// <summary>
        /// Get/Set Method of Thursday field
        /// </summary>
        public bool Thursday
        {
            get
            {
                return accountCreditPlusSummaryDTO.Thursday;
            }
        }

        /// <summary>
        /// Get/Set Method of Friday field
        /// </summary>
        public bool Friday
        {
            get
            {
                return accountCreditPlusSummaryDTO.Friday;
            }
        }

        /// <summary>
        /// Get/Set Method of Saturday field
        /// </summary>
        public bool Saturday
        {
            get
            {
                return accountCreditPlusSummaryDTO.Saturday;
            }
        }

        /// <summary>
        /// Get/Set Method of Sunday field
        /// </summary>
        public bool Sunday
        {
            get
            {
                return accountCreditPlusSummaryDTO.Sunday;
            }
        }

        /// <summary>
        /// Get/Set Method of MinimumSaleAmount field
        /// </summary>
        public decimal? MinimumSaleAmount
        {
            get
            {
                return accountCreditPlusSummaryDTO.MinimumSaleAmount;
            }
        }

        /// <summary>
        /// Get/Set Method of LoyaltyRuleId field
        /// </summary>
        public int LoyaltyRuleId
        {
            get
            {
                return accountCreditPlusSummaryDTO.LoyaltyRuleId;
            }
        }

        /// <summary>
        /// Get/Set Method of ExtendOnReload field
        /// </summary>
        public bool ExtendOnReload
        {
            get
            {
                return accountCreditPlusSummaryDTO.ExtendOnReload;
            }
        }

        /// <summary>
        /// Get/Set Method of PlayStartTime field
        /// </summary>
        public DateTime? PlayStartTime
        {
            get
            {
                return accountCreditPlusSummaryDTO.PlayStartTime;
            }
        }

        /// <summary>
        /// Get/Set Method of TicketAllowed field
        /// </summary>
        public bool TicketAllowed
        {
            get
            {
                return accountCreditPlusSummaryDTO.TicketAllowed;
            }
        }

        /// <summary>
        /// Get/Set Method of ForMembershipOnly field
        /// </summary>
        public bool ForMembershipOnly
        {
            get
            {
                return accountCreditPlusSummaryDTO.ForMembershipOnly;
            }
        }

        /// <summary>
        /// Get/Set Method of ExpireWithMembership field
        /// </summary>
        public bool ExpireWithMembership
        {
            get
            {
                return accountCreditPlusSummaryDTO.ExpireWithMembership;
            }
        }

        /// <summary>
        /// Get/Set Method of MembershipId field
        /// </summary>
        public int MembershipId
        {
            get
            {
                return accountCreditPlusSummaryDTO.MembershipId;
            }
        }

        /// <summary>
        /// Get/Set Method of MembershipName field
        /// </summary>
        public string MembershipName
        {
            get
            {
                return accountCreditPlusSummaryDTO.MembershipName;
            }
        }


        /// <summary>
        /// Get/Set Method of MembershipRewardsId field
        /// </summary>
        public int MembershipRewardsId
        {
            get
            {
                return accountCreditPlusSummaryDTO.MembershipRewardsId;
            }
        }

        /// <summary>
        /// Get/Set Method of MembershipRewardName field
        /// </summary>
        public string MembershipRewardName
        {
            get
            {
                return accountCreditPlusSummaryDTO.MembershipRewardName;
            }
        }

        /// <summary>
        /// Get/Set Method of PauseAllowed field
        /// </summary>
        public bool PauseAllowed
        {
            get
            {
                return accountCreditPlusSummaryDTO.PauseAllowed;
            }
        }

        /// <summary>
        /// Get/Set Method of ValidityStatus field
        /// </summary>
        public AccountDTO.AccountValidityStatus ValidityStatus
        {
            get
            {
                return accountCreditPlusSummaryDTO.ValidityStatus;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountCreditPlusConsumptionSummaryDTOList field
        /// </summary>
        public List<AccountCreditPlusConsumptionSummaryDTO> AccountCreditPlusConsumptionSummaryDTOList
        {
            get
            {
                return accountCreditPlusSummaryDTO.AccountCreditPlusConsumptionSummaryDTOList;
            }
        }

        /// <summary>
        /// Get/Set Method of SourceCreditPlusId field
        /// </summary>
        public int SourceCreditPlusId
        {
            get
            {
                return accountCreditPlusSummaryDTO.SourceCreditPlusId;
            }
        }

        /// <summary>
        /// Get/Set Method of SubscriptionBillingScheduleId field
        /// </summary>
        public int SubscriptionBillingScheduleId
        {
            get
            {
                return accountCreditPlusSummaryDTO.SubscriptionBillingScheduleId;
            }
        }
    }
}
