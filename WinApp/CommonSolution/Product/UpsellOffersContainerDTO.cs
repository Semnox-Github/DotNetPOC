/********************************************************************************************
 * Project Name - Products
 * Description  - Holds data for the upsell offer container
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks                   
 ******************************************************************************************************************
*2.140.00    14-Sep-2021     Prajwal S     Created
 ******************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class UpsellOffersContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int offerId;
        private int productId;
        private int offerProductId;
        private int saleGroupId;
        private string offerMessage;
        private DateTime effectiveDate;
        private SalesOfferGroupContainerDTO salesOfferGroupContainerDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UpsellOffersContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public UpsellOffersContainerDTO(int offerId, int productId, int offerProductId, string offerMessage,
                                int saleGroupId, DateTime effectiveDate)
        : this()
        {
            log.LogMethodEntry(offerId, productId, offerProductId, offerMessage, saleGroupId);
            this.offerId = offerId;
            this.productId = productId;
            this.offerProductId = offerProductId;
            this.offerMessage = offerMessage;
            this.saleGroupId = saleGroupId;
            this.effectiveDate = effectiveDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public UpsellOffersContainerDTO(UpsellOffersContainerDTO upsellOffersContainerDTO)
        : this(upsellOffersContainerDTO.offerId, upsellOffersContainerDTO.productId, upsellOffersContainerDTO.offerProductId, upsellOffersContainerDTO.offerMessage,
                                upsellOffersContainerDTO.saleGroupId, upsellOffersContainerDTO.EffectiveDate)
        {
            log.LogMethodEntry(upsellOffersContainerDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the OfferId field
        /// </summary>
        public int OfferId { get { return offerId; } set { offerId = value; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; } }

        /// <summary>
        /// Get/Set method of the OfferProductId field
        /// </summary>
        public int OfferProductId { get { return offerProductId; } set { offerProductId = value; } }

        /// <summary>
        /// Get/Set method of the SaleGroupId field
        /// </summary>
        public int SaleGroupId { get { return saleGroupId; } set { saleGroupId = value; } }
        /// <summary>
        /// Get/Set method of the OfferMessage field
        /// </summary>
        public string OfferMessage { get { return offerMessage; } set { offerMessage = value; } }

        /// <summary>
        /// Get/Set method of the EffectiveDate field
        /// </summary>
        public DateTime EffectiveDate { get { return effectiveDate; } set { effectiveDate = value; } }

        /// <summary>
        /// Get/Set method of the SalesOfferGroupContainerDTO field
        /// </summary>
        public SalesOfferGroupContainerDTO SalesOfferGroupContainerDTO { get { return salesOfferGroupContainerDTO; } set { salesOfferGroupContainerDTO = value; } }
    }
}
