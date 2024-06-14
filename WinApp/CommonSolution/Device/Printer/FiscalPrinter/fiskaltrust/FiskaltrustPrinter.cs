/********************************************************************************************
 * Project Name - Device
 * Description  - Printer implementation for Fiskaltrust
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks          
 *********************************************************************************************
 *2.90.0     14-Jul-2020      Gururaja Kanjan    Created for fiskaltrust integration.
*2.110.0     22-Dec-2020      Girish Kundar      Modified :FiscalTrust changes - Shift open/Close/PayIn/PayOut to be fiscalized
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.JobUtils;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    public class FiskaltrustPrinter : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string URL_KEY = "FISCAL_DEVICE_TCP/IP_ADDRESS";
        private const string PRINTER_TOKEN_KEY = "FISCAL_PRINTER_PASSWORD";

        private static string FISKALTRUST_URL;
        private static string FISKALTRUST_TOKEN;

        public const string FISKALTRUST = "FISKALTRUST";

        public FiskaltrustPrinter(Utilities _Utilities) : base(_Utilities)
        {
            log.LogMethodEntry(_Utilities);

            try
            {
                if (FISKALTRUST_URL == null)
                {
                    FISKALTRUST_URL =
                        ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, URL_KEY);
                    FISKALTRUST_TOKEN =
                        ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, PRINTER_TOKEN_KEY);
                    if(string.IsNullOrEmpty(FISKALTRUST_URL) || string.IsNullOrEmpty(FISKALTRUST_TOKEN)) 
                    {
                        log.Error("FISCAL_PRINTER_PASSWORD or FISCAL_DEVICE_TCP/IP_ADDRESS is not correct");
                        log.Debug("Please check the set up ");
                        return;
                    }
                    log.Debug("FISKALTRUST_URL :" + FISKALTRUST_URL);
                    log.Debug("FISKALTRUST_TOKEN :" + FISKALTRUST_TOKEN);

                }

                // first publish of fiskaltrust service.
                PublishInitialReceipt();
            }
            catch (System.Net.WebException webex)
            {
                log.Error("Error occured during initialization", webex);
            }
            catch (Exception ex)
            {
                log.Error("Error occured during initialization", ex);
            }

            log.LogMethodExit();
        }
         
        /// <summary>
        /// PrintReceipt method
        /// </summary>
        /// <param name="receiptRequest"></param>
        /// <returns></returns>
        public override bool PrintReceipt(FiscalizationRequest receiptRequest, ref string Message)
        {
            log.LogMethodEntry(receiptRequest);
            bool fiscalizationResult = false;
            if (receiptRequest != null)
            {
                try
                {
                    Message = SendInvoiceForFiscalization(receiptRequest);
                    if (Message.StartsWith("ERROR"))
                    {
                        log.Debug("Error occurred while fiscalizing the request.");
                        if (receiptRequest.transactionId > 0)
                        {
                            LogFiscalizationError(receiptRequest.transactionId, -1, Message);
                        }
                        else
                        {
                            LogFiscalizationError(-1, receiptRequest.shiftLogId, Message);
                        }
                        Message = string.Empty;
                    }
                    else
                    {
                        fiscalizationResult = true;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Exception in PrintReceipt", ex);
                }
            }
            log.LogMethodExit(fiscalizationResult);
            return fiscalizationResult;
        }

        /// <summary>
        /// publishInitialReceipt method
        /// </summary>
        /// <returns></returns>
        private void PublishInitialReceipt()
        {
            log.LogMethodEntry();
            ExecutionContext executionContext = utilities.ExecutionContext;
            FiskaltrustMapper fiskaltrustMapper = new FiskaltrustMapper(utilities.ExecutionContext);
            ReceiptRequest receipt = fiskaltrustMapper.GetInitialRequest(executionContext);
            //string receiptRequestJSON = JsonConvert.SerializeObject(receipt);
            var result = JsonConvert.SerializeObject(receipt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            log.LogVariableState("result", result);
            JObject JObject = JsonConvert.DeserializeObject<JObject>(result);
            string receiptRequestJSON = JObject.ToString();
            log.Debug("receiptRequestJSON :" + receiptRequestJSON);
            try
            {
                HTTPServiceUtil httpService = new HTTPServiceUtil(executionContext);
                Dictionary<string, string> responseDictionary = httpService.Post(receiptRequestJSON, FiskaltrustPrinter.FISKALTRUST_URL);  //

                if (responseDictionary.ContainsKey("OK"))
                {
                    log.Debug("Successful invocation of Initial Receipt.");
                }
                else
                {
                    log.Error("ERROR ... in publishInitialReceipt ...");
                    foreach (KeyValuePair<string, string> responseMessage in responseDictionary)
                    {
                        log.Error("\tKey {responseMessage.key}: Value={responseMessage.Value}");
                    }
                }
            }
            catch (System.Net.WebException webex)
            {
                log.Error("Error occured during publish to fiskaltrust", webex);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Calls the fiskaltrust REST API for fiscalization.
        /// </summary>
        /// <param name="fiscalizationRequest">FiscalizationRequest object</param>
        /// <returns>signature returned by fiscalization</returns>
        public override string SendInvoiceForFiscalization(FiscalizationRequest fiscalizationRequest)
        {
            log.LogMethodEntry(fiscalizationRequest);
            string signature = "ERROR ....";
            bool isSuccess = false;
            FiskaltrustMapper fiskaltrustMapper = new FiskaltrustMapper(utilities.ExecutionContext);

            try
            {

                ReceiptRequest receiptRequest = fiskaltrustMapper.GetFiskaltrustRequest(fiscalizationRequest);
                var result = JsonConvert.SerializeObject(receiptRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                log.LogVariableState("result" , result);
                JObject JObject = JsonConvert.DeserializeObject<JObject>(result);
                string receiptRequestJSON = JObject.ToString();
                log.Debug("String Request :" + receiptRequestJSON);
                HTTPServiceUtil httpService = new HTTPServiceUtil(utilities.ExecutionContext);
                Dictionary<string, string> responseDictionary = httpService.Post(receiptRequestJSON,
                    FiskaltrustPrinter.FISKALTRUST_URL);

                if (responseDictionary.ContainsKey("OK"))
                {
                    log.Debug("Success in publish to fiskaltrust ..");
                    string fiskaltrustResponse = responseDictionary["OK"];
                    ReceiptResponse receiptResponse = JsonConvert.DeserializeObject<ReceiptResponse>(fiskaltrustResponse);

                    string tempSignature = fiskaltrustMapper.GetSignature(receiptResponse);
                    if (!string.IsNullOrEmpty(tempSignature))
                    {
                        isSuccess = true;
                        signature = tempSignature;
                    }
                }

                if (!isSuccess)
                {
                    foreach (KeyValuePair<string, string> responseMessage in responseDictionary)
                    {
                        log.Error("\tKey {responseMessage.key}: Value={responseMessage.Value}");
                        signature = signature + responseMessage.Value;
                    }
                    log.Error("ERROR ... in publish ...");
                }
            }
            catch (System.Net.WebException webex)
            {
                log.Error("Error occured during publish to fiskaltrust", webex);
                signature = signature + webex.Message;
            }

            log.Debug("Signature from fiskaltrust :" + signature);
            log.LogMethodExit(signature);

            return signature;
        }




        /// <summary>
        /// Creates Concurrent request for failed fiscalization's
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="trxId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private void LogFiscalizationError(int trxId, int shiftlogId, string message)
        {
            log.LogMethodEntry(trxId, shiftlogId, message);
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            DateTime dt = lookupValuesList.GetServerDateTime();
            string currentDate = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            currentDate = string.Concat(currentDate, " 00:00:00");
            int fiskalTrustProgramId = -1; 
            log.Debug("The request start date : " + currentDate);

            List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchByProgramsParameters =
                new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>
                (ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>
                (ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, "fiskaltrust"));
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>
                (ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "1"));
            ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(utilities.ExecutionContext);
            List<ConcurrentProgramsDTO> concurrentProgramsDTOList =
                concurrentProgramList.GetAllConcurrentPrograms(searchByProgramsParameters);

            if (concurrentProgramsDTOList != null)
            {
                log.Debug("Concurrent program ID :" + concurrentProgramsDTOList.First().ProgramId);
                fiskalTrustProgramId = concurrentProgramsDTOList.First().ProgramId;
                string parafaitObject = string.Empty;
                int parafaitObjectId = -1;
                if (trxId > 0)
                {
                    parafaitObject = "Transaction";
                    parafaitObjectId = trxId;
                }
                else
                {
                    parafaitObject = "ShiftOperation";
                    parafaitObjectId = shiftlogId;
                }

                ConcurrentRequestDetailsListBL concurrentRequestDetailsListBL = new ConcurrentRequestDetailsListBL(utilities.ExecutionContext);
                List<KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>> rsearchParameters = new List<KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>>();
                rsearchParameters.Add(new KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>(ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_PROGRAM_ID, fiskalTrustProgramId.ToString()));
                rsearchParameters.Add(new KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>(ConcurrentRequestDetailsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                rsearchParameters.Add(new KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>(ConcurrentRequestDetailsDTO.SearchByParameters.PARAFAIT_OBJECT_ID, parafaitObjectId.ToString()));
                List<ConcurrentRequestDetailsDTO> result = concurrentRequestDetailsListBL.GetConcurrentRequestDetailsDTOList(rsearchParameters);
                if (result == null || result.Any() == false)
                {
                    ConcurrentRequestsDTO concurrentRequestsDTO = new ConcurrentRequestsDTO(-1, concurrentProgramsDTOList.First().ProgramId, -1,
                                          DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                          utilities.ExecutionContext.GetUserId(),
                                          DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                                          DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), string.Empty,
                                          "Running", "Normal", false, string.Empty, string.Empty,
                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                          string.Empty, string.Empty, -1, -1, false);
                    ConcurrentRequests concurrentRequests = new ConcurrentRequests(utilities.ExecutionContext, concurrentRequestsDTO);
                    concurrentRequests.Save();
                    concurrentRequestsDTO = concurrentRequests.GetconcurrentRequests;
                    log.Debug("Concurrent Request ID :" + concurrentRequestsDTO.RequestId);
                    ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO = new ConcurrentRequestDetailsDTO(-1, concurrentRequestsDTO.RequestId, dt,
                        concurrentRequestsDTO.ProgramId, parafaitObject, parafaitObjectId, String.Empty, false, "Failed", String.Empty, String.Empty, message, true);
                    ConcurrentRequestDetailsBL concurrentRequestDetailsBL = new ConcurrentRequestDetailsBL(utilities.ExecutionContext, concurrentRequestDetailsDTO);
                    concurrentRequestDetailsBL.Save();
                }
            }
            else
            {
                log.Error("ERROR: Concurrent Program for fiskaltrust does not exit");
            }
            log.LogMethodExit();
        }


        public override bool OpenPort()
        {
            log.LogMethodEntry();
            bool result = true;
            log.LogMethodExit(result);
            return result;
        }
        public override void ClosePort()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }

}
