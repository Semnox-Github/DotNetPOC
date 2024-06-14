/********************************************************************************************
 * Project Name - Product
 * Description  - ProductContainerDTOCollection class
 *
 **************
 ** Version Log
  **************
  * Version     Date             Modified By             Remarks
 *********************************************************************************************
 2.110.0        01-Dec-2020       Deeksha                 Created : Web Inventory/POS design with REST API
 ********************************************************************************************/
using System.Linq;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class ProductsContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductsContainerDTO> productContainerDTOList;
        private string hash;

        public ProductsContainerDTOCollection()
        {
            log.LogMethodEntry();
            productContainerDTOList = new List<ProductsContainerDTO>();
            log.LogMethodExit();
        }

        public ProductsContainerDTOCollection(List<ProductsContainerDTO> productContainerDTOList)
        {
            log.LogMethodEntry(productContainerDTOList);
            this.productContainerDTOList = productContainerDTOList;
            if (productContainerDTOList == null)
            {
                productContainerDTOList = new List<ProductsContainerDTO>();
            }
            hash = new DtoListHash(productContainerDTOList);
            log.LogMethodExit();
        }
        
        public List<ProductsContainerDTO> ProductContainerDTOList
        {
            get
            {
                return productContainerDTOList;
            }

            set
            {
                productContainerDTOList = value;
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
