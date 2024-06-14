/**************************************************************************************************
 * Project Name - Transaction 
 * Description  - TransactionPaymentStatusController gets the staus of the gateway payments
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.110.0     27-Nov-2020       Girish Kundar              Created
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Site;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Transaction
{
    public class TransactionPaymentStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of TransactionPaymentStatus
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/TransactionPaymentStatus")]
        public HttpResponseMessage Get(int transactionId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                KeyValuePair<PaymentGateway.TRX_STATUS_CHECK_RESPONSE, List<ValidationError>> trx_STATUS_CHECK_RESPONSE;
                trx_STATUS_CHECK_RESPONSE = new KeyValuePair<PaymentGateway.TRX_STATUS_CHECK_RESPONSE, List<ValidationError>>();
                List<ValidationError> validationErrorsList = new List<ValidationError>();
                if (transactionId > -1)
                {
                    CCRequestPGWDTO cCRequestPGWDTO;
                    CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                    CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> ccRequestSearchParams = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                    ccRequestSearchParams.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, transactionId.ToString()));
                    ccRequestSearchParams.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    cCRequestPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(ccRequestSearchParams);
                    if (cCRequestPGWDTO == null)
                    {
                        trx_STATUS_CHECK_RESPONSE = new KeyValuePair<PaymentGateway.TRX_STATUS_CHECK_RESPONSE, List<ValidationError>>(PaymentGateway.TRX_STATUS_CHECK_RESPONSE.NO_ACTION, validationErrorsList);
                        log.Info("There is no credit card transaction done for this transaction");
                    }
                    else
                    {
                        Utilities Utilities = new Utilities();
                        Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                        Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                        TransactionUtils trxUtils = new TransactionUtils(Utilities);
                        Parafait.Transaction.Transaction NewTrx = trxUtils.CreateTransactionFromDB(transactionId, Utilities);
                        int siteId = -1;
                        if (NewTrx.site_id != null && !string.IsNullOrEmpty(NewTrx.site_id.ToString()))
                            siteId = Convert.ToInt32(NewTrx.site_id);
                        Utilities.ParafaitEnv.SiteId = siteId;
                        bool isCorporate = false;
                        SiteList siteList = new SiteList(executionContext);
                        SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                        if (HQSite != null && HQSite.SiteId != -1 && HQSite.SiteId != siteId)
                        {
                            isCorporate = true;
                        }
                        Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                        Utilities.ParafaitEnv.IsCorporate = isCorporate;
                        Utilities.ExecutionContext.SetIsCorporate(isCorporate);
                        Utilities.ExecutionContext.SetSiteId(Convert.ToInt32(siteId));
                        Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                        Utilities.ParafaitEnv.Initialize();
                        // Check the transaction is reversed
                        bool isReversed = NewTrx.IsReversedTransaction(null);
                        if (isReversed)
                        {
                            log.Error("Transaction is reversed : " + transactionId);
                            throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Transaction is reversed"));
                        }

                        // Get the payment mode Id from transaction
                        PaymentModeDTO paymentModeDTO = null;
                        if (NewTrx.TransactionPaymentsDTOList != null && NewTrx.TransactionPaymentsDTOList.Any())
                        {
                            TransactionPaymentsDTO transactionPaymentsDTO = NewTrx.TransactionPaymentsDTOList[0];
                            if (transactionPaymentsDTO.PaymentModeId > -1)
                            {
                                PaymentMode paymentModesBL = new PaymentMode(executionContext, transactionPaymentsDTO.PaymentModeId);
                                paymentModeDTO = paymentModesBL.GetPaymentModeDTO;
                                PaymentGatewayFactory.GetInstance().Initialize(Utilities, true, null);
                                HostedPaymentGateway paymentGateway = (HostedPaymentGateway)PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModeDTO.GatewayLookUp);
                                if (paymentGateway != null)
                                {
                                    KeyValuePair<PaymentGateway.TRX_STATUS_CHECK_RESPONSE, List<ValidationError>> ValidationResponse = paymentGateway.ValidateLastTransactionStatus(transactionId);

                                    if (ValidationResponse.Value.Count > 0)
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ValidationResponse.Key + ":" + ValidationResponse.Value[0].Message });

                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
                log.LogMethodExit();

                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

    }
}


