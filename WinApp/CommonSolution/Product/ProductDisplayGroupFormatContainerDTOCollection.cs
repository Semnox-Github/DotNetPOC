/********************************************************************************************
 * Project Name - Product 
 * Description  - Data object of ProductDisplayGroupFormatContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.140.0      25-Jun-2021   Abhishek                 Created : POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductDisplayGroupFormatContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductDisplayGroupFormatContainerDTO> productDisplayGroupFormatContainerDTOList;
        private string hash;

        public ProductDisplayGroupFormatContainerDTOCollection()
        {
            log.LogMethodEntry();
            productDisplayGroupFormatContainerDTOList = new List<ProductDisplayGroupFormatContainerDTO>();
            log.LogMethodExit();
        }

        public ProductDisplayGroupFormatContainerDTOCollection(List<ProductDisplayGroupFormatContainerDTO> productDisplayGroupFormatContainerDTOList)
        {
            log.LogMethodEntry(productDisplayGroupFormatContainerDTOList);
            this.productDisplayGroupFormatContainerDTOList = productDisplayGroupFormatContainerDTOList;
            if (this.productDisplayGroupFormatContainerDTOList == null)
            {
                this.productDisplayGroupFormatContainerDTOList = new List<ProductDisplayGroupFormatContainerDTO>();
            }
            hash = new DtoListHash(productDisplayGroupFormatContainerDTOList);
            log.LogMethodExit();
        }
        
        public List<ProductDisplayGroupFormatContainerDTO> ProductDisplayGroupFormatContainerDTOList
        {
            get
            {
                return productDisplayGroupFormatContainerDTOList;
            }

            set
            {
                productDisplayGroupFormatContainerDTOList = value;
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
