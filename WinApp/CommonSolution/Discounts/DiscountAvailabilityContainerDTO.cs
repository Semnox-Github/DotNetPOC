/********************************************************************************************
 * Project Name - Discounts
 * Description  - Data structure of DiscountAvailability
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      05-May-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the Discount Availability data object class
    /// </summary>
    public class DiscountAvailabilityContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int discountId;
        private List<DiscountAvailabilityDetailContainerDTO> discountAvailabilityDetailDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountAvailabilityContainerDTO()
        {
            log.LogMethodEntry();
            discountId = -1;
            discountAvailabilityDetailDTOList = new List<DiscountAvailabilityDetailContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountAvailabilityContainerDTO(int discountId, List<DiscountAvailabilityDetailContainerDTO> discountAvailabilityDetailDTOList)
        {
            log.LogMethodEntry(discountId, discountAvailabilityDetailDTOList);
            this.discountId = discountId;
            this.discountAvailabilityDetailDTOList = discountAvailabilityDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the discountId field
        /// </summary>
        public int DiscountId
        {
            get { return discountId; }
            set { discountId = value; }
        }

        /// <summary>
        /// Get/Set method of the discountAvailabilityDetailDTOList field
        /// </summary>
        public List<DiscountAvailabilityDetailContainerDTO> DiscountAvailabilityDetailDTOList
        {
            get { return discountAvailabilityDetailDTOList; }
            set { discountAvailabilityDetailDTOList = value; }
        }
    }
}
