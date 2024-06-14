/********************************************************************************************
 * Project Name - Customer
 * Description  - Data Object class for AccountCreditPlusSummary
 * 
 **************
 **Version Log
 **************
 *Version      Date           Modified By             Remarks          
 *********************************************************************************************
 **2.150.02   27-Mar-2023     Yashodhara C H          Created
 ********************************************************************************************/

using System;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountCreditPlus Consumption summary data object class.
    /// </summary>
    public class AccountCreditPlusConsumptionSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountCreditPlusConsumptionSummaryDTO()
        {
            log.LogMethodEntry();
            AccountCreditPlusConsumptionId = -1;
            AccountCreditPlusId = -1;
            POSTypeId = -1;
            ProductId = -1;
            GameId = -1;
            GameProfileId = -1;
            CategoryId = -1;
            OrderTypeId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public AccountCreditPlusConsumptionSummaryDTO(int accountCreditPlusConsumptionId, int accountCreditPlusId, int pOSTypeId, DateTime? expiryDate,
                 int productId, string productName, int gameId, string gameName, int gameProfileId, string gameProfileName, decimal? discountPercentage, decimal? discountedPrice,
                 int? consumptionBalance, int? quantityLimit, int categoryId, string categoryName, decimal? discountAmount, int orderTypeId, string orderTypeName, int? consumptionQty)
    : this()
        {
            log.LogMethodEntry(accountCreditPlusConsumptionId, accountCreditPlusId, pOSTypeId, expiryDate, productId, productName,
                               gameId, gameName, gameProfileId, gameProfileName, discountPercentage, discountedPrice,
                               consumptionBalance, quantityLimit, categoryId, categoryName, discountAmount, orderTypeId, orderTypeName, consumptionQty);
            AccountCreditPlusConsumptionId = accountCreditPlusConsumptionId;
            AccountCreditPlusId = accountCreditPlusId;
            POSTypeId = pOSTypeId;
            ExpiryDate = expiryDate;
            ProductId = productId;
            ProductName = productName;
            GameId = gameId;
            GameName = gameName;
            GameProfileId = gameProfileId;
            GameProfileName = gameProfileName;
            DiscountPercentage = discountPercentage;
            DiscountedPrice = discountedPrice;
            ConsumptionBalance = consumptionBalance;
            QuantityLimit = quantityLimit;
            CategoryId = categoryId;
            CategoryName = categoryName;
            DiscountAmount = discountAmount;
            OrderTypeId = orderTypeId;
            OrderTypeName = orderTypeName;
            ConsumptionQty = consumptionQty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public AccountCreditPlusConsumptionSummaryDTO(AccountCreditPlusConsumptionSummaryDTO accountCreditPlusConsumptionSummaryDTO)
          : this(accountCreditPlusConsumptionSummaryDTO.AccountCreditPlusConsumptionId,
                accountCreditPlusConsumptionSummaryDTO.AccountCreditPlusId,
                 accountCreditPlusConsumptionSummaryDTO.POSTypeId,
                 accountCreditPlusConsumptionSummaryDTO.ExpiryDate,
                 accountCreditPlusConsumptionSummaryDTO.ProductId,
                 accountCreditPlusConsumptionSummaryDTO.ProductName,
                 accountCreditPlusConsumptionSummaryDTO.GameId,
                 accountCreditPlusConsumptionSummaryDTO.GameName,
                 accountCreditPlusConsumptionSummaryDTO.GameProfileId,
                 accountCreditPlusConsumptionSummaryDTO.GameProfileName,
                 accountCreditPlusConsumptionSummaryDTO.DiscountPercentage,
                 accountCreditPlusConsumptionSummaryDTO.DiscountedPrice,
                 accountCreditPlusConsumptionSummaryDTO.ConsumptionBalance,
                 accountCreditPlusConsumptionSummaryDTO.QuantityLimit,
                 accountCreditPlusConsumptionSummaryDTO.CategoryId,
                 accountCreditPlusConsumptionSummaryDTO.CategoryName,
                 accountCreditPlusConsumptionSummaryDTO.DiscountAmount,
                 accountCreditPlusConsumptionSummaryDTO.OrderTypeId,
                 accountCreditPlusConsumptionSummaryDTO.OrderTypeName,
                 accountCreditPlusConsumptionSummaryDTO.ConsumptionQty)
        {
            log.LogMethodEntry();
            
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set Method of AccountCreditPlusConsumptionId field
        /// </summary>
        public int AccountCreditPlusConsumptionId { get; set; }

        /// <summary>
        /// Get/Set Method of AccountCreditPlusId field
        /// </summary>
        public int AccountCreditPlusId { get; set; }

        /// <summary>
        /// Get/Set Method of POSTypeId field
        /// </summary>
        public int POSTypeId { get; set; }

        /// <summary>
        /// Get/Set Method of ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Get/Set Method of ProductId field
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Get/Set Method of ProductName field
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Get/Set Method of GameProfileId field
        /// </summary>
        public int GameProfileId { get; set; }

        /// <summary>
        /// Get/Set Method of GameProfileName field
        /// </summary>
        public string GameProfileName { get; set; }

        /// <summary>
        /// Get/Set Method of GameId field
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Get/Set Method of GameName field
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// Get/Set Method of DiscountPercentage field
        /// </summary>
        public decimal? DiscountPercentage { get; set; }

        /// <summary>
        /// Get/Set Method of DiscountedPrice field
        /// </summary>
        public decimal? DiscountedPrice { get; set; }

        /// <summary>
        /// Get/Set Method of ConsumptionQty field
        /// </summary>
        public int? ConsumptionQty { get; set; }

        /// <summary>
        /// Get/Set Method of ConsumptionBalance field
        /// </summary>
        public int? ConsumptionBalance { get; set; }

        /// <summary>
        /// Get/Set Method of QuantityLimit field
        /// </summary>
        public int? QuantityLimit { get; set; }

        /// <summary>
        /// Get/Set Method of CategoryId field
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Get/Set Method of CategoryName field
        /// </summary>
        public string CategoryName { get; set; }


        /// <summary>
        /// Get/Set Method of DiscountAmount field
        /// </summary>
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// Get/Set Method of OrderTypeId field
        /// </summary>
        public int OrderTypeId { get; set; }

        /// <summary>
        /// Get/Set Method of OrderTypeName field
        /// </summary>
        public string OrderTypeName{ get; set; }
    }
}
