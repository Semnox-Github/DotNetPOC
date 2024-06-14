/********************************************************************************************
 * Project Name - Device  
 * Description  - IEnabledAttributesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      19-Aug-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public interface IEnabledAttributesUseCases
    {
        Task<EnabledAttributesContainerDTOCollection> GetEnabledAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> SaveEnabledAttributes(List<EnabledAttributesDTO> enabledAttributesDTOList);
        Task<List<EnabledAttributesDTO>> GetEnabledAttributes(List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> parameters);
    }
}
