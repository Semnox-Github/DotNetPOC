/********************************************************************************************
 * Project Name - GenericOTP
 * Description  - IGenericOTPUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.130.11      19-Aug-2022      Yashodhara C H           Created
 ********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public interface IGenericOTPUseCases
    {
        Task ValidateGenericOTP(GenericOTPDTO genericOTPDTO);

        Task<GenericOTPDTO> GenerateGenericOTP(GenericOTPDTO genericOTPDTO);

        Task<GenericOTPDTO> ReSendGenericOTP(GenericOTPDTO genericOTPDTOList);
    }
}
