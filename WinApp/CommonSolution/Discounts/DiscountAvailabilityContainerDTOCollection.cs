/********************************************************************************************
 * Project Name - Discounts 
 * Description  - Data object of DiscountAvailabilityDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.150.0      05-May-2021   Lakshminarayana       Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the collection of Discount Availability data object class. 
    /// </summary>
    public class DiscountAvailabilityContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DiscountAvailabilityContainerDTO> discountAvailabilityDTOList;
        private string hash;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DiscountAvailabilityContainerDTOCollection()
        {
            log.LogMethodEntry();
            discountAvailabilityDTOList = new List<DiscountAvailabilityContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public DiscountAvailabilityContainerDTOCollection(List<DiscountAvailabilityContainerDTO> discountAvailabilityDTOList)
        {
            log.LogMethodEntry(discountAvailabilityDTOList);
            this.discountAvailabilityDTOList = discountAvailabilityDTOList;
            if (this.discountAvailabilityDTOList == null)
            {
                this.discountAvailabilityDTOList = new List<DiscountAvailabilityContainerDTO>();
            }
            hash = new DtoListHash(discountAvailabilityDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set methods for discountAvailabilityDTOList field
        /// </summary>
        public List<DiscountAvailabilityContainerDTO> DiscountAvailabilityDTOList
        {
            get
            {
                return discountAvailabilityDTOList;
            }

            set
            {
                discountAvailabilityDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set methods for hash field
        /// </summary>
        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}
