/******************************************************************************************************
 * Project Name - Device
 * Description  - Mashreq Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                 Remarks          
 ******************************************************************************************************
 *2.140.3     11-Aug-2022    Prasad, Dakshakh Raj        Mashreq Payment gateway integration
 ********************************************************************************************************/

using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{

    internal class MashreqCommandHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private dynamic result;
        int port;
        MashreqCommonRequestHandler commonRequestHandler;
        public MashreqCommandHandler(ExecutionContext executionContext, int port)
        {
            log.LogMethodEntry(port);
            this.port = port;
            MashreqCommonRequestHandler commonRequestHandler = new MashreqCommonRequestHandler(executionContext, port);
            this.commonRequestHandler = commonRequestHandler;
            log.LogMethodExit();

        }
        public async Task<object> GetTerminalInfo()
        {
            log.LogMethodEntry();
            result = await commonRequestHandler.GetTerminalInfo();
            log.LogMethodExit(result);
            return result;
        }


        public async Task<object> MakePurchase(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.transactionAmount != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreatePurchase(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<object> MakeRefund(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.transactionAmount != "" && requestDTO.mrefValue != "" && requestDTO.authCode != "")
                {
                    result = await commonRequestHandler.CreateRefund(requestDTO: requestDTO);
                }
                else
                {
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<object> MakeIndependentRefund(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.transactionAmount != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreateRefund(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<object> MakePreAuth(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.transactionAmount != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreatePreAuth(requestDTO: requestDTO);

                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }
        
        public async Task<object> MakePreAuthCompletion(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.invoiceNumber != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreatePreAuthCompletion(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<object> MakePreReceipt(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.transactionAmount != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreatePreReceipt(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<object> MakePreReceiptCompletionWithTip(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.invoiceNumber != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreatePreReceiptCompletionWithTip(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<object> MakePurchaseWithTip(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.transactionAmount != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreatePurchaseWithTip(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<object> MakeVoid(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.transactionAmount != "" && requestDTO.invoiceNumber != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreateVoid(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }
        
        public async Task<object> MakeDuplicate(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.invoiceNumber != "" && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CreateDuplicate(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<object> GetLastTransactionStatus(MashreqRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            object result;
            try
            {
                if (requestDTO != null && requestDTO.mrefValue != "")
                {
                    result = await commonRequestHandler.CheckLastTransactionStatus(requestDTO: requestDTO);
                }
                else
                {
                    log.Error("Insufficient request params");
                    throw new Exception("Insufficient request params");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }


    }
}
