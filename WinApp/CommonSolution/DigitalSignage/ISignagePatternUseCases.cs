/********************************************************************************************
* Project Name - DigitalSignage
* Description  - Specification of the SignagePattern use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   23-Apr-2021   Roshan Devadiga          Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
   public interface ISignagePatternUseCases
    {
        Task <List<SignagePatternDTO>> GetSignagePatterns(List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveSignagePatterns(List<SignagePatternDTO> signagePatternDTOList);
    }
}
