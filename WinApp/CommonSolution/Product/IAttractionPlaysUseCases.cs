/********************************************************************************************
* Project Name - Product
* Description  - Specification of the AttractionPlays use cases. 
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
    public interface IAttractionPlaysUseCases
    {
        Task<List<AttractionPlaysDTO>> GetAttractionPlays(List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>> searchParameters);
        Task<string> SaveAttractionPlays(List<AttractionPlaysDTO> attractionPlaysDTOList);
        Task<string> Delete(List<AttractionPlaysDTO> attractionPlaysDTOList);
    }
}
