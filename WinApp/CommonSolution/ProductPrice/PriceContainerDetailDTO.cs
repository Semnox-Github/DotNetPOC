/********************************************************************************************
 * Project Name - Product
 * Description  - price container detail data transfer object
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      17-Aug-2021      Lakshminarayana           Created : price container enhancement
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Price Container Detail Data transfer object
    /// </summary>
    public class PriceContainerDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime startDateTime;
        private DateTime endDateTime;
        private decimal? basePrice;
        private decimal? membershipPrice;
        private int membershipPriceListId;
        private decimal? userRolePrice;
        private int userRolePriceListId;
        private decimal? transactionProfilePrice;
        private int transactionProfilePriceListId;
        private decimal? promotionPrice;
        private string priceType;
        private decimal? finalPrice;
        private int promotionId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PriceContainerDetailDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with fields
        /// </summary>
        public PriceContainerDetailDTO(DateTime startDateTime, 
                                       DateTime endDateTime, 
                                       decimal? basePrice)
        :this(startDateTime:startDateTime, 
              endDateTime: endDateTime, 
              basePrice: basePrice,
              priceType: string.Empty,
              membershipPrice: null,
              membershipPriceListId: -1,
              userRolePrice:null, 
              userRolePriceListId: -1,
              transactionProfilePrice: null,
              transactionProfilePriceListId: -1,
              promotionPrice: null,
              finalPrice: null,
              promotionId: -1)
        {
            log.LogMethodEntry(startDateTime, endDateTime, basePrice);
            
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the fields
        /// </summary>
        public PriceContainerDetailDTO(DateTime startDateTime, 
                                       DateTime endDateTime, 
                                       decimal? basePrice, 
                                       string priceType,
                                       decimal? membershipPrice,
                                       int membershipPriceListId,
                                       decimal? userRolePrice,
                                       int userRolePriceListId,
                                       decimal? transactionProfilePrice,
                                       int transactionProfilePriceListId,
                                       decimal? promotionPrice,
                                       decimal? finalPrice,
                                       int promotionId)
        {
            log.LogMethodEntry(startDateTime, endDateTime, basePrice, priceType,
                               membershipPrice, membershipPriceListId, userRolePrice, userRolePriceListId,
                               transactionProfilePrice, transactionProfilePriceListId,
                               promotionPrice, finalPrice, promotionId);
            this.startDateTime = startDateTime;
            this.endDateTime = endDateTime;
            this.basePrice = basePrice;
            this.priceType = priceType;
            this.membershipPrice = membershipPrice;
            this.membershipPriceListId = membershipPriceListId;
            this.userRolePrice = userRolePrice;
            this.userRolePriceListId = userRolePriceListId;
            this.transactionProfilePrice = transactionProfilePrice;
            this.transactionProfilePriceListId = transactionProfilePriceListId;
            this.promotionPrice = promotionPrice;
            this.finalPrice = finalPrice;
            this.promotionId = promotionId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="priceContainerDetailDTO"></param>
        public PriceContainerDetailDTO(PriceContainerDetailDTO priceContainerDetailDTO)
            :this(priceContainerDetailDTO.startDateTime, priceContainerDetailDTO.endDateTime, priceContainerDetailDTO.basePrice,
                  priceContainerDetailDTO.priceType, priceContainerDetailDTO.membershipPrice, priceContainerDetailDTO.membershipPriceListId, priceContainerDetailDTO.userRolePrice, priceContainerDetailDTO.userRolePriceListId, 
                  priceContainerDetailDTO.transactionProfilePrice, priceContainerDetailDTO.transactionProfilePriceListId,
                  priceContainerDetailDTO.promotionPrice, priceContainerDetailDTO.finalPrice, priceContainerDetailDTO.promotionId)
        {
            log.LogMethodEntry(priceContainerDetailDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the startDateTime field
        /// </summary>
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set { startDateTime = value; }
        }

        /// <summary>
        /// Get/Set method of the endDateTime field
        /// </summary>
        public DateTime EndDateTime
        {
            get { return endDateTime; }
            set { endDateTime = value; }
        }

        /// <summary>
        /// Get/Set method of the basePrice field
        /// </summary>
        public decimal? BasePrice
        {
            get { return basePrice; }
            set { basePrice = value; }
        }

        /// <summary>
        /// Get/Set method of the priceType field
        /// </summary>
        public string PriceType
        {
            get { return priceType; }
            set { priceType = value; }
        }

        /// <summary>
        /// Get/Set method of the membershipPrice field
        /// </summary>
        public decimal? MembershipPrice
        {
            get { return membershipPrice; }
            set { membershipPrice = value; }
        }

        /// <summary>
        /// Get/Set method of the userRolePrice field
        /// </summary>
        public decimal? UserRolePrice
        {
            get { return userRolePrice; }
            set { userRolePrice = value; }
        }

        /// <summary>
        /// Get/Set method of the transactionProfilePrice field
        /// </summary>
        public decimal? TransactionProfilePrice
        {
            get { return transactionProfilePrice; }
            set { transactionProfilePrice = value; }
        }

        /// <summary>
        /// Get/Set method of the promotionPrice field
        /// </summary>
        public decimal? PromotionPrice
        {
            get { return promotionPrice; }
            set { promotionPrice = value; }
        }

        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        public decimal? FinalPrice
        {
            get { return finalPrice; }
            set { finalPrice = value; }
        }

        /// <summary>
        /// Get/Set method of the userRolePriceListId field
        /// </summary>
        public int UserRolePriceListId
        {
            get { return userRolePriceListId; }
            set { userRolePriceListId = value; }
        }

        /// <summary>
        /// Get/Set method of the membershipPriceListId field
        /// </summary>
        public int MembershipPriceListId
        {
            get { return membershipPriceListId; }
            set { membershipPriceListId = value; }
        }

        /// <summary>
        /// Get/Set method of the membershipPriceListId field
        /// </summary>
        public int TransactionProfilePriceListId
        {
            get { return transactionProfilePriceListId; }
            set { transactionProfilePriceListId = value; }
        }

        /// <summary>
        /// Get/Set method of the promotionId field
        /// </summary>
        public int PromotionId
        {
            get { return promotionId; }
            set { promotionId = value; }
        }

        

    }
}
