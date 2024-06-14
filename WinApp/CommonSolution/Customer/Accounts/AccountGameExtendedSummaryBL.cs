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
    public class AccountGameExtendedSummaryBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AccountGameExtendedSummaryDTO accountGameExtendedSummaryDTO;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountGameExtendedSummaryDTO"></param>
        public AccountGameExtendedSummaryBL(ExecutionContext executionContext, AccountGameExtendedSummaryDTO accountGameExtendedSummaryDTO)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            this.accountGameExtendedSummaryDTO = accountGameExtendedSummaryDTO;
            log.LogMethodExit();
        }

        public AccountGameExtendedSummaryDTO AccountGameExtendedSummaryDTO
        {
            get
            {
                return accountGameExtendedSummaryDTO;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountGameExtendedId field
        /// </summary>
        public int AccountGameExtendedId
        {
            get
            {
                return accountGameExtendedSummaryDTO.AccountGameExtendedId;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountCreditPlusId field
        /// </summary>
        public int AccountGameId
        {
            get
            {
                return accountGameExtendedSummaryDTO.AccountGameId;
            }
        }

        /// <summary>
        /// Get/Set Method of POSTypeId field
        /// </summary>
        public int GameId
        {
            get
            {
                return accountGameExtendedSummaryDTO.GameId;
            }
        }

        /// <summary>
        /// Get/Set Method of ExpiryDate field
        /// </summary>
        public string GameName
        {
            get
            {
                return accountGameExtendedSummaryDTO.GameName;
            }
        }

        /// <summary>
        /// Get/Set Method of ProductId field
        /// </summary>
        public int GameProfileId
        {
            get
            {
                return accountGameExtendedSummaryDTO.GameProfileId;
            }
        }

        /// <summary>
        /// Get/Set Method of GameProfileId field
        /// </summary>
        public string GameProfileName
        {
            get
            {
                return accountGameExtendedSummaryDTO.GameProfileName;
            }
        }

        /// <summary>
        /// Get/Set Method of GameId field
        /// </summary>
        public bool Exclude
        {
            get
            {
                return accountGameExtendedSummaryDTO.Exclude;
            }
        }

        /// <summary>
        /// Get/Set Method of DiscountPercentage field
        /// </summary>
        public int PlayLimitPerGame
        {
            get
            {
                return accountGameExtendedSummaryDTO.PlayLimitPerGame;
            }
        }
    }
}
