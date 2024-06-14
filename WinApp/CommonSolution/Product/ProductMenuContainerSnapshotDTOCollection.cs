/********************************************************************************************
 * Project Name - Product 
 * Description  - Data object of ProductMenuContainerSnapshotDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      04-Aug-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
namespace Semnox.Parafait.Product
{
    public class ProductMenuContainerSnapshotDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductMenuContainerSnapshotDTO> productMenuContainerSnapshotDTOList;
        private string hash;

        public ProductMenuContainerSnapshotDTOCollection()
        {
            log.LogMethodEntry();
            productMenuContainerSnapshotDTOList = new List<ProductMenuContainerSnapshotDTO>();
            log.LogMethodExit();
        }

        public ProductMenuContainerSnapshotDTOCollection(List<ProductMenuContainerSnapshotDTO> productMenuContainerSnapshotDTOList)
        {
            log.LogMethodEntry(productMenuContainerSnapshotDTOList);
            this.productMenuContainerSnapshotDTOList = productMenuContainerSnapshotDTOList;
            if (this.productMenuContainerSnapshotDTOList == null)
            {
                this.productMenuContainerSnapshotDTOList = new List<ProductMenuContainerSnapshotDTO>();
            }
            hash = new DtoListHash(productMenuContainerSnapshotDTOList);
            log.LogMethodExit();
        }

        public List<ProductMenuContainerSnapshotDTO> ProductMenuContainerSnapshotDTOList
        {
            get
            {
                return productMenuContainerSnapshotDTOList;
            }

            set
            {
                productMenuContainerSnapshotDTOList = value;
            }
        }

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
