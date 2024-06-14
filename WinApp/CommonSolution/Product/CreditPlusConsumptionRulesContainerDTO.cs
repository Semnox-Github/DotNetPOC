/********************************************************************************************
 * Project Name - CreditPlusConsumptionRules Container DTO  
 * Description  - Data object of CreditPlusConsumptionRulesContainerDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *2.150.0     07-Mar-2022   Prajwal S                   Created
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class CreditPlusConsumptionRulesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int pKId;
        int productCreditPlusId;
        int pOSTypeId;
        DateTime? expiryDate;
        int gameId;
        int gameProfileId;
        int product_id;
        int? quantity;
        int? quantityLimit;
        int categoryId;
        int? discountAmount;
        decimal? discountPercentage;
        int? discountedPrice;
        int orderTypeId;
        string guid;

        public CreditPlusConsumptionRulesContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public CreditPlusConsumptionRulesContainerDTO(int pKId, int productCreditPlusId, int pOSTypeId, DateTime? expiryDate, string guid,
                                             int gameId, int gameProfileId, int product_id, int? quantity, int? quantityLimit, int categoryId,
                                             int? discountAmount, decimal? discountPercentage, int? discountedPrice, int orderTypeId)
        {
            log.LogMethodEntry(pKId, productCreditPlusId, pOSTypeId, expiryDate, guid, gameId, gameProfileId, product_id, quantity, quantityLimit,
                               categoryId, discountAmount, discountPercentage, discountedPrice, orderTypeId);

            this.pKId = pKId;
            this.productCreditPlusId = productCreditPlusId;
            this.pOSTypeId = pOSTypeId;
            this.expiryDate = expiryDate;
            this.guid = guid;
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.product_id = product_id;
            this.quantity = quantity;
            this.quantityLimit = quantityLimit;
            this.categoryId = categoryId;
            this.discountAmount = discountAmount;
            this.discountPercentage = discountPercentage;
            this.discountedPrice = discountedPrice;
            this.orderTypeId = orderTypeId;
            log.LogMethodExit();
        }

        public CreditPlusConsumptionRulesContainerDTO(CreditPlusConsumptionRulesContainerDTO creditPlusConsumptionRulesContainerDTO)
        : this(creditPlusConsumptionRulesContainerDTO.pKId, creditPlusConsumptionRulesContainerDTO.productCreditPlusId,
             creditPlusConsumptionRulesContainerDTO.pOSTypeId, creditPlusConsumptionRulesContainerDTO.expiryDate,
             creditPlusConsumptionRulesContainerDTO.guid, creditPlusConsumptionRulesContainerDTO.gameId,
             creditPlusConsumptionRulesContainerDTO.gameProfileId, creditPlusConsumptionRulesContainerDTO.product_id,
             creditPlusConsumptionRulesContainerDTO.quantity, creditPlusConsumptionRulesContainerDTO.quantityLimit,
             creditPlusConsumptionRulesContainerDTO.categoryId, creditPlusConsumptionRulesContainerDTO.discountAmount,
             creditPlusConsumptionRulesContainerDTO.discountPercentage, creditPlusConsumptionRulesContainerDTO.discountedPrice,
             creditPlusConsumptionRulesContainerDTO.orderTypeId)
        {
            log.LogMethodEntry(creditPlusConsumptionRulesContainerDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for PKId
        /// </summary>
        public int PKId { get { return pKId; } set { pKId = value; } }

        /// <summary>
        /// Get/Set for ProductCreditPlusId
        /// </summary>
        public int ProductCreditPlusId { get { return productCreditPlusId; } set { productCreditPlusId = value; } }

        /// <summary>
        /// Get/Set for POSTypeId
        /// </summary>
        public int POSTypeId { get { return pOSTypeId; } set { pOSTypeId = value; } }

        /// <summary>
        /// Get/Set for ExpiryDate
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }

        /// <summary>
        /// Get/Set for Guid
        /// </summary>
        public string Guid { get { return guid; } }

        /// <summary>
        /// Get/Set for GameId
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; } }

        /// <summary>
        /// Get/Set for GameProfileId
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; } }

        /// <summary>
        /// Get/Set for ProductId
        /// </summary>
        public int ProductId { get { return product_id; } set { product_id = value; } }

        /// <summary>
        /// Get/Set for Quantity
        /// </summary>
        public int? Quantity { get { return quantity; } set { quantity = value; } }

        /// <summary>
        /// Get/Set for QuantityLimit
        /// </summary>
        public int? QuantityLimit { get { return quantityLimit; } set { quantityLimit = value; } }

        /// <summary>
        /// Get/Set for CategoryId
        /// </summary>
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }

        /// <summary>
        /// Get/Set for DiscountAmount
        /// </summary>
        public int? DiscountAmount { get { return discountAmount; } set { discountAmount = value; } }

        /// <summary>
        /// Get/Set for DiscountPercentage
        /// </summary>
        public decimal? DiscountPercentage { get { return discountPercentage; } set { discountPercentage = value; } }

        /// <summary>
        /// Get/Set for DiscountedPrice
        /// </summary>
        public int? DiscountedPrice { get { return discountedPrice; } set { discountedPrice = value; } }

        /// <summary>
        /// Get/Set for OrderTypeId
        /// </summary>
        public int OrderTypeId { get { return orderTypeId; } set { orderTypeId = value; } }

    }
}
