/********************************************************************************************
 * Project Name - AccountCreditPlusConsumptionSummaryBL
 * Description  - BL for Manage Account CreditPlus Consumption Summary Details
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
  **2.150.02   27-Mar-2023     Yashodhara C H          Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account Credit plus Consumption summary class
    /// </summary>
    public class AccountCreditPlusConsumptionSummaryBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private AccountCreditPlusConsumptionSummaryDTO accountCreditPlusConsumptionSummaryDTO;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountCreditPlusConsumptionSummaryDTO"></param>
        public AccountCreditPlusConsumptionSummaryBL(ExecutionContext executionContext, AccountCreditPlusConsumptionSummaryDTO accountCreditPlusConsumptionSummaryDTO)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            this.accountCreditPlusConsumptionSummaryDTO = accountCreditPlusConsumptionSummaryDTO;
            log.LogMethodExit();
        }

        public AccountCreditPlusConsumptionSummaryDTO AccountCreditPlusConsumptionSummaryDTO
        {
            get
            {
               return  accountCreditPlusConsumptionSummaryDTO;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountCreditPlusConsumptionId field
        /// </summary>
        public int AccountCreditPlusConsumptionId
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.AccountCreditPlusConsumptionId;
            }
        }

        /// <summary>
        /// Get/Set Method of AccountCreditPlusId field
        /// </summary>
        public int AccountCreditPlusId
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.AccountCreditPlusId;
            }
        }

        /// <summary>
        /// Get/Set Method of POSTypeId field
        /// </summary>
        public int POSTypeId
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.POSTypeId;
            }
        }

        /// <summary>
        /// Get/Set Method of ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.ExpiryDate;
            }
        }

        /// <summary>
        /// Get/Set Method of ProductId field
        /// </summary>
        public int ProductId
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.ProductId;
            }
        }

        /// <summary>
        /// Get/Set Method of ProductName field
        /// </summary>
        public string ProductName
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.ProductName;
            }
        }

        /// <summary>
        /// Get/Set Method of GameProfileId field
        /// </summary>
        public int GameProfileId
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.GameProfileId;
            }
        }

        /// <summary>
        /// Get/Set Method of GameProfileName field
        /// </summary>
        public string GameProfileName
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.GameProfileName;
            }
        }

        /// <summary>
        /// Get/Set Method of GameId field
        /// </summary>
        public int GameId
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.GameId;
            }
        }

        /// <summary>
        /// Get/Set Method of GameName field
        /// </summary>
        public string GameName
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.GameName;
            }

        }
        /// <summary>
        /// Get/Set Method of DiscountPercentage field
        /// </summary>
        public decimal? DiscountPercentage
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.DiscountPercentage;
            }
        }

        /// <summary>
        /// Get/Set Method of DiscountedPrice field
        /// </summary>
        public decimal? DiscountedPrice
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.DiscountedPrice;
            }
        }

        /// <summary>
        /// Get/Set Method of ConsumptionQty field
        /// </summary>
        public int? ConsumptionQty
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.ConsumptionQty;
            }
        }

        /// <summary>
        /// Get/Set Method of ConsumptionBalance field
        /// </summary>
        public int? ConsumptionBalance
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.ConsumptionBalance;
            }
        }

        /// <summary>
        /// Get/Set Method of QuantityLimit field
        /// </summary>
        public int? QuantityLimit
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.QuantityLimit;
            }
        }

        /// <summary>
        /// Get/Set Method of CategoryId field
        /// </summary>
        public int CategoryId
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.CategoryId;
            }
        }

        /// <summary>
        /// Get/Set Method of CategoryId field
        /// </summary>
        public string CategoryName
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.CategoryName;
            }
        }


        /// <summary>
        /// Get/Set Method of DiscountAmount field
        /// </summary>
        public decimal? DiscountAmount
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.DiscountAmount;
            }
        }

        /// <summary>
        /// Get/Set Method of OrderTypeId field
        /// </summary>
        public int OrderTypeId
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.OrderTypeId;
            }
        }

        /// <summary>
        /// Get/Set Method of OrderTypeName field
        /// </summary>
        public string OrderTypeName
        {
            get
            {
                return accountCreditPlusConsumptionSummaryDTO.OrderTypeName;
            }
        }
    }
}
