/********************************************************************************************
* Project Name - Waiver
* Description  - Specification of the WaiverSetSigningOptions use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   26-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Waiver
{
   public interface IWaiverSetSigningOptionsUseCases
    {
        Task<List<WaiverSetSigningOptionsDTO>> GetWaiverSetSigningOptions(List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveWaiverSetSigningOptions(List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOs); 
    }
}
