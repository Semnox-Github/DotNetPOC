/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - RemoteGenericOTPUseCase class
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By       Remarks          
 *********************************************************************************************
 *2.130.11   20-Aug-2022    Yashodhara C H     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class RemoteGenericOTPUseCases : RemoteUseCases, IGenericOTPUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GENERIC_OTP_CREATE_URL = "api/CommonServices/GenericOTP/Create";
        private const string GENERIC_OTP_VALIDATE_URL = "api/CommonServices/GenericOTP/{id}/Validate";
        private const string GENERIC_OTP_RESEND_URL = "api/CommonServices/GenericOTP/Resend";

        /// <summary>
        ///  Parameterized constructor 
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteGenericOTPUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to generate and save OTP
        /// </summary>
        /// <param name="genericOTPDTO"></param>
        /// <returns></returns>
        public async Task<GenericOTPDTO> GenerateGenericOTP(GenericOTPDTO genericOTPDTO)
        {
            try
            {
                log.LogMethodEntry(genericOTPDTO);
                GenericOTPDTO result = await Post<GenericOTPDTO>(GENERIC_OTP_CREATE_URL, genericOTPDTO);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        ///  Method to get OTP 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <param name="siteId"></param>
        public async Task ValidateGenericOTP(GenericOTPDTO genericOTPDTO)
        {
            try
            {
                log.LogMethodEntry(genericOTPDTO);
                await Post<Task>(GENERIC_OTP_VALIDATE_URL.Replace("{id}", genericOTPDTO.Id.ToString()), genericOTPDTO);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Method to to ReSend OTP
        /// </summary>
        /// <param name="genericOTPDTO"></param>
        /// <returns></returns>
        public async Task<GenericOTPDTO> ReSendGenericOTP(GenericOTPDTO genericOTPDTO)
        {
            try
            {
                log.LogMethodEntry(genericOTPDTO);
                GenericOTPDTO result = await Post<GenericOTPDTO>(GENERIC_OTP_RESEND_URL, genericOTPDTO);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

    }
}
