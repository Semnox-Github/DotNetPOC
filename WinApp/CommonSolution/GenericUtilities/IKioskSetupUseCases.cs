/********************************************************************************************
* Project Name - GenericUtilities
* Description  - Specification of the KioskSetup . 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   27-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
   public interface IKioskSetupUseCases
    {
        Task<List<KioskSetupDTO>> GetKioskSetups(List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveKioskSetups(List<KioskSetupDTO> kioskSetupDTOList);
        Task<string> Delete(List<KioskSetupDTO> kioskSetupDTOList);

    }
}
