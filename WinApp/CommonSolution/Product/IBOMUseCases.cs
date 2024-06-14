/********************************************************************************************
* Project Name - Product
* Description  - Specification of the BOM use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   09-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IBOMUseCases
    {
        Task<List<BOMDTO>> GetBOMs(List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchParameters);
        Task<string> SaveBOMs(List<BOMDTO> bOMDTOList);
    }
}
