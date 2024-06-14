/********************************************************************************************
 * Project Name - Product 
 * Description  - Data object of ProductTypeContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.120.1    24-Jun-2021    Abhishek           Created : POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductTypeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductTypeContainerDTO> productTypeContainerDTOList;
        private string hash;

        public ProductTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            productTypeContainerDTOList = new List<ProductTypeContainerDTO>();
            log.LogMethodExit();
        }

        public ProductTypeContainerDTOCollection(List<ProductTypeContainerDTO> productTypeContainerDTOList)
        {
            log.LogMethodEntry(productTypeContainerDTOList);
            this.productTypeContainerDTOList = productTypeContainerDTOList;
            if (this.productTypeContainerDTOList == null)
            {
                this.productTypeContainerDTOList = new List<ProductTypeContainerDTO>();
            }
            hash = new DtoListHash(productTypeContainerDTOList);
            log.LogMethodExit();
        }

        public List<ProductTypeContainerDTO> ProductTypeContainerDTOList
        {
            get
            {
                return productTypeContainerDTOList;
            }

            set
            {
                productTypeContainerDTOList = value;
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
