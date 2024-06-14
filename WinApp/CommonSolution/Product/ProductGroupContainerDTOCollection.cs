/********************************************************************************************
 * Project Name - Product 
 * Description  - Data object of ProductGroupContainerDTOCollection
 * 
 **************
 **Version Log
 **************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.170.0      07-Jul-2023      Lakshminarayana           Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductGroupContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductGroupContainerDTO> productGroupContainerDTOList;
        private string hash;

        public ProductGroupContainerDTOCollection()
        {
            log.LogMethodEntry();
            productGroupContainerDTOList = new List<ProductGroupContainerDTO>();
            log.LogMethodExit();
        }

        public ProductGroupContainerDTOCollection(List<ProductGroupContainerDTO> productGroupContainerDTOList)
        {
            log.LogMethodEntry(productGroupContainerDTOList);
            this.productGroupContainerDTOList = productGroupContainerDTOList;
            if (this.productGroupContainerDTOList == null)
            {
                this.productGroupContainerDTOList = new List<ProductGroupContainerDTO>();
            }
            hash = new DtoListHash(productGroupContainerDTOList);
            log.LogMethodExit();
        }

        public List<ProductGroupContainerDTO> ProductGroupContainerDTOList
        {
            get
            {
                return productGroupContainerDTOList;
            }

            set
            {
                productGroupContainerDTOList = value;
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
