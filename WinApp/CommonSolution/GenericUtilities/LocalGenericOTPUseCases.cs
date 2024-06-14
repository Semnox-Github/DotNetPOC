/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - LocalGenericOTPUseCase class
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By         Remarks          
 *********************************************************************************************
 *2.130.11    20-Aug-2022    Yashodhara C H       Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class LocalGenericOTPUseCases : LocalUseCases, IGenericOTPUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LocalGenericOTPUsecase class
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalGenericOTPUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to generate OTP
        /// </summary>
        /// <param name="genericOTPDTO"></param>
        /// <returns></returns>
        public async Task<GenericOTPDTO> GenerateGenericOTP(GenericOTPDTO genericOTPDTO)
        {
            return await Task<GenericOTPDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(genericOTPDTO);
                if (genericOTPDTO == null)
                {
                    string errorMessage = "GenrericOTPDTOList is empty";
                    log.LogMethodExit("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                GenericOTPDTO result = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        GenericOTPVerificationBL genericOTPValidationBL = new GenericOTPVerificationBL(executionContext);
                        result = genericOTPValidationBL.GenerateOTP(genericOTPDTO.Phone, genericOTPDTO.EmailId, genericOTPDTO.Source, genericOTPDTO.CountryCode, parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException ex)
                    {
                        // do not rollback for validation exception
                        log.Error(ex);
                        parafaitDBTrx.EndTransaction();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        parafaitDBTrx.RollBack();
                        throw;
                    }
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        /// <summary>
        /// Method to Validate the OTP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task ValidateGenericOTP(GenericOTPDTO genericOTPDTO)
        {

            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(genericOTPDTO);
                if (genericOTPDTO.Id == -1 || string.IsNullOrWhiteSpace(genericOTPDTO.Code))
                {
                    string errorMessage = "GenrericOTPDTOList is empty";
                    log.LogMethodExit("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        GenericOTPVerificationBL genericOTPValidationBL = new GenericOTPVerificationBL(executionContext);
                        genericOTPValidationBL.ValidateOTP(genericOTPDTO.Id, genericOTPDTO.Code, parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException ex)
                    {
                        // do not rollback for validation exception
                        log.Error(ex);
                        parafaitDBTrx.EndTransaction();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        parafaitDBTrx.RollBack();
                        throw;
                    }
            }
                log.LogMethodExit();
            });   
        }

        public async Task<GenericOTPDTO> ReSendGenericOTP(GenericOTPDTO genericOTPDTO)
        {
            return await Task<GenericOTPDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(genericOTPDTO);
                if (genericOTPDTO == null)
                {
                    string errorMessage = "GenrericOTPDTOList is empty";
                    log.LogMethodExit("Throwing Exception- " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                GenericOTPDTO result = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        GenericOTPVerificationBL genericOTPValidationBL = new GenericOTPVerificationBL(executionContext);
                        result = genericOTPValidationBL.ResendOTP(genericOTPDTO.Id, genericOTPDTO.Phone, genericOTPDTO.EmailId, genericOTPDTO.Source, genericOTPDTO.CountryCode, parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException ex)
                    {
                        // do not rollback for validation exception
                        log.Error(ex);
                        parafaitDBTrx.EndTransaction();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        parafaitDBTrx.EndTransaction();
                        throw;
                    }
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }

}
