/********************************************************************************************
 * Project Name - Product
 * Description  - product menu container snapshot data transfer object
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      04-Aug-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// product menu container Snapshot data transfer object
    /// </summary>
    public class ProductPriceContainerSnapshotDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime startDateTime;
        private DateTime endDateTime;
        private List<ProductsPriceContainerDTO> productsPriceContainerDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductPriceContainerSnapshotDTO()
        {
            log.LogMethodEntry();
            productsPriceContainerDTOList = new List<ProductsPriceContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        public ProductPriceContainerSnapshotDTO(DateTime startDateTime, DateTime endDateTime)
        {
            log.LogMethodEntry(startDateTime, endDateTime);
            this.startDateTime = startDateTime;
            this.endDateTime = endDateTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the startDateTime field
        /// </summary>
        public DateTime StartDateTime
        {
            get
            {
                return startDateTime;
            }

            set
            {
                startDateTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the endDateTime field
        /// </summary>
        public DateTime EndDateTime
        {
            get
            {
                return endDateTime;
            }

            set
            {
                endDateTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the productsPriceContainerDTOList field
        /// </summary>
        public List<ProductsPriceContainerDTO> ProductsPriceContainerDTOList
        {
            get
            {
                return productsPriceContainerDTOList;
            }

            set
            {
                productsPriceContainerDTOList = value;
            }
        }
    }
}
