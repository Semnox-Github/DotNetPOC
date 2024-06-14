/********************************************************************************************
* Project Name - Product
* Description  - Data structure of ProductCalendar Container
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0      04-Aug-2021      Lakshminarayana           Created 
********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Product Calendar Container data object class
    /// </summary>
    public class ProductCalendarContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId;
        private string productGuid;
        private List<ProductCalendarDetailContainerDTO> productCalendarDetailContainerDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductCalendarContainerDTO()
        {
            log.LogMethodEntry();
            productId = -1;
            productGuid = string.Empty;
            productCalendarDetailContainerDTOList = new List<ProductCalendarDetailContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductCalendarContainerDTO(int productId, string productGuid, List<ProductCalendarDetailContainerDTO> productCalendarDetailDTOList)
        {
            log.LogMethodEntry(productId, productCalendarDetailDTOList);
            this.productId = productId;
            this.productGuid = productGuid;
            this.productCalendarDetailContainerDTOList = productCalendarDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the productId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        /// <summary>
        /// Get/Set method of the productGuid field
        /// </summary>
        public string ProductGuid
        {
            get { return productGuid; }
            set { productGuid = value; }
        }

        /// <summary>
        /// Get/Set method of the productCalendarDetailDTOList field
        /// </summary>
        public List<ProductCalendarDetailContainerDTO> ProductCalendarDetailContainerDTOList
        {
            get { return productCalendarDetailContainerDTOList; }
            set { productCalendarDetailContainerDTOList = value; }
        }
    }
}