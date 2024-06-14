/********************************************************************************************
 * Project Name - Waiver 
 * Description  - IWaiverSetUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************
 *2.130.0     20-Jul-2021   Mushahid Faizan    Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Waiver
{
    public interface IWaiverSetUseCases
    {
        Task<List<WaiverSetDTO>> GetWaiverSets(List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> waiversSearchList,
             bool loadActiveChild = false, bool removeIncompleteRecords = false, bool getLanguageSpecificContent = false, string waiverSetSigningOptions = null);
        Task<string> SaveWaiverSets(List<WaiverSetDTO> waiverSetDTOList);

        Task<WaiverSetContainerDTOCollection> GetWaiverSetContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache);
    }
}
  
