/********************************************************************************************
 * Project Name - Products
 * Description  - ProductsPriceConatinerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By      Remarks          
 *********************************************************************************************
 2.130.0      24-Aug-2021       Prajwal          Created
 ********************************************************************************************/
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ProductPrice
{
    public class ProductsPriceContainerDTO : ProductsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private decimal? finalPrice;
        private decimal? basePrice;
        private decimal? userRolePrice;
        private decimal? memberShipPrice;
        private decimal? transactionProfilePrice;
        private decimal? promotionPrice;
        private string priceType;
        private decimal? finalPriceWithTax;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsPriceContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ProductsPriceContainerDTO(ProductsContainerDTO productsContainerDTO, PriceContainerDetailDTO priceContainerDetailDTO, decimal? finalPriceWithTax)
            : base(productsContainerDTO)
        {
            log.LogMethodEntry(productsContainerDTO, finalPrice, basePrice, userRolePrice, memberShipPrice, transactionProfilePrice, promotionPrice, finalPriceWithTax);
            this.finalPrice = priceContainerDetailDTO.FinalPrice;
            this.basePrice = priceContainerDetailDTO.BasePrice;
            this.userRolePrice = priceContainerDetailDTO.UserRolePrice;
            this.memberShipPrice = priceContainerDetailDTO.MembershipPrice;
            this.transactionProfilePrice = priceContainerDetailDTO.TransactionProfilePrice;
            this.promotionPrice = priceContainerDetailDTO.PromotionPrice;
            this.priceType = priceContainerDetailDTO.PriceType;
            this.finalPriceWithTax = finalPriceWithTax;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the FinalPrice field
        /// </summary>
        public decimal? FinalPrice { get { return finalPrice; } set { finalPrice = value; } }


        /// <summary>
        /// Get/Set method of the BasePrice field
        /// </summary>
        public decimal? BasePrice { get { return basePrice; } set { basePrice = value; } }


        /// <summary>
        /// Get/Set method of the UserRolePrice field
        /// </summary>
        public decimal? UserRolePrice { get { return userRolePrice; } set { userRolePrice = value; } }


        /// <summary>
        /// Get/Set method of the MemberShipPrice field
        /// </summary>
        public decimal? MemberShipPrice { get { return memberShipPrice; } set { memberShipPrice = value; } }

        /// <summary>
        /// Get/Set method of the TransactionProfilePrice field
        /// </summary>
        public decimal? TransactionProfilePrice { get { return transactionProfilePrice; } set { transactionProfilePrice = value; } }

        /// <summary>
        /// Get/Set method of the PromotionPrice field
        /// </summary>
        public decimal? PromotionPrice { get { return promotionPrice; } set { promotionPrice = value; } }

        /// <summary>
        /// Get/Set method of the priceType field
        /// </summary>
        public string PriceType
        {
            get { return priceType; }
            set { priceType = value; }
        }

        /// <summary>
        /// Get/Set method of the FinalPriceWithTax field
        /// </summary>
        public decimal? FinalPriceWithTax
        {
            get { return finalPriceWithTax; }
            set { finalPriceWithTax = value; }
        }
    }
}
