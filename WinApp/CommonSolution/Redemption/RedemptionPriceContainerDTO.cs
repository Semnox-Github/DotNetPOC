/********************************************************************************************
 * Project Name - Redemption
 * Description  - Data structure of RedemptionPriceViewContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Data structure of RedemptionPriceViewContainer
    /// </summary>
    public class RedemptionPriceContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int productId;
        private int membershipId;
        private decimal priceInTickets;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RedemptionPriceContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public RedemptionPriceContainerDTO(int productId, int membershipId, decimal price)
            : this()
        {
            log.LogMethodEntry(productId, membershipId, price);
            this.productId = productId;
            this.membershipId = membershipId;
            this.priceInTickets = price;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the productId field
        /// </summary>
        public int ProductId
        {
            get
            {
                return productId;
            }

            set
            {
                productId = value;
            }
        }



        /// <summary>
        /// Get/Set method of the membershipId field
        /// </summary>
        public int MembershipId
        {
            get
            {
                return membershipId;
            }

            set
            {
                membershipId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the priceInTickets field
        /// </summary>
        public decimal PriceInTickets
        {
            get
            {
                return priceInTickets;
            }

            set
            {
                priceInTickets = value;
            }
        }

    }
}
