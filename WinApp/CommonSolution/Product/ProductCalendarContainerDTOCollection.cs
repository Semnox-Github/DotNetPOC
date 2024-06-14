/********************************************************************************************
 * Project Name - Product 
 * Description  - Data object of ProductCalendarDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.130.0      04-Aug-2021   Lakshminarayana         Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the collection of Product Calendar data object class. 
    /// </summary>
    public class ProductCalendarContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductCalendarContainerDTO> productCalendarContainerDTOList;
        private string hash;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductCalendarContainerDTOCollection()
        {
            log.LogMethodEntry();
            productCalendarContainerDTOList = new List<ProductCalendarContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public ProductCalendarContainerDTOCollection(List<ProductCalendarContainerDTO> productCalendarDTOList)
        {
            log.LogMethodEntry(productCalendarDTOList);
            this.ProductCalendarContainerDTOList = productCalendarDTOList;
            if (this.productCalendarContainerDTOList == null)
            {
                this.productCalendarContainerDTOList = new List<ProductCalendarContainerDTO>();
            }
            hash = new DtoListHash(productCalendarDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set methods for ProductCalendarDTOList field
        /// </summary>
        public List<ProductCalendarContainerDTO> ProductCalendarContainerDTOList
        {
            get
            {
                return productCalendarContainerDTOList;
            }

            set
            {
                productCalendarContainerDTOList = value;
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
