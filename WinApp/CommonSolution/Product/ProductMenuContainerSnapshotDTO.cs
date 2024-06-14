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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// product menu container Snapshot data transfer object
    /// </summary>
    public class ProductMenuContainerSnapshotDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime startDateTime;
        private DateTime endDateTime;
        private List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductMenuContainerSnapshotDTO()
        {
            log.LogMethodEntry();
            productMenuPanelContainerDTOList = new List<ProductMenuPanelContainerDTO>();
            log.LogMethodExit();
        }

        public ProductMenuContainerSnapshotDTO(DateTime startDateTime, DateTime endDateTime)
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
        /// Get/Set method of the endDateTime field
        /// </summary>
        public List<ProductMenuPanelContainerDTO> ProductMenuPanelContainerDTOList
        {
            get
            {
                return productMenuPanelContainerDTOList;
            }

            set
            {
                productMenuPanelContainerDTOList = value;
            }
        }
    }
}
