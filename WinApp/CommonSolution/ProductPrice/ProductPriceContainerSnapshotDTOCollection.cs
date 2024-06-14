/********************************************************************************************
 * Project Name - Product 
 * Description  - Data object of ProductPriceContainerSnapshotDTOCollection
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
namespace Semnox.Parafait.ProductPrice
{
    public class ProductPriceContainerSnapshotDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductPriceContainerSnapshotDTO> productPriceContainerSnapshotDTOList;
        private string hash;

        public ProductPriceContainerSnapshotDTOCollection()
        {
            log.LogMethodEntry();
            productPriceContainerSnapshotDTOList = new List<ProductPriceContainerSnapshotDTO>();
            log.LogMethodExit();
        }

        public ProductPriceContainerSnapshotDTOCollection(List<ProductPriceContainerSnapshotDTO> productPriceContainerSnapshotDTOList)
        {
            log.LogMethodEntry(productPriceContainerSnapshotDTOList);
            this.productPriceContainerSnapshotDTOList = productPriceContainerSnapshotDTOList;
            if (this.productPriceContainerSnapshotDTOList == null)
            {
                this.productPriceContainerSnapshotDTOList = new List<ProductPriceContainerSnapshotDTO>();
            }
            hash = new DtoListHash(productPriceContainerSnapshotDTOList);
            log.LogMethodExit();
        }

        public List<ProductPriceContainerSnapshotDTO> ProductPriceContainerSnapshotDTOList
        {
            get
            {
                return productPriceContainerSnapshotDTOList;
            }

            set
            {
                productPriceContainerSnapshotDTOList = value;
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
