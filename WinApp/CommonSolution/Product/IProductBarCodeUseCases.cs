/********************************************************************************************
* Project Name - Product
* Description  - Specification of the ProductBarcode use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   14-Sep-2021    Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IProductBarcodeUseCases
    {
        Task<List<ProductBarcodeDTO>> GetProductBarcodes(List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveProductBarcodes(List<ProductBarcodeDTO> productBarcodeDTOList);
    }
}
