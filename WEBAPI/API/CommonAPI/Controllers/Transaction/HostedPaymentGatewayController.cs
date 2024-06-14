/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Controller for posting the customer transactions
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 *2.80         05-Apr-2020   Girish Kundar        Modified: API path changes
 *2.130.6      24-Jun-2022   Nitin Pai            Issue Fix: Change done for visa net removed the customer information. Adding it back.
 *2.150.2      31-Jan-2023   Nitin Pai            Not sending the token as part of the return
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Site;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class HostedPaymentGatewayController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Transfer entitlements from parent card to child card
        /// </summary>
        [HttpGet]
        [Route("api/Transaction/HostedPaymentGateways")]
        [Authorize]
        public HttpResponseMessage Get(string hostedPaymentGateway, double amount, int transactionId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
                Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();

                log.Debug("Building transaction from DB " + transactionId);
                TransactionUtils trxUtils = new TransactionUtils(Utilities);
                Parafait.Transaction.Transaction NewTrx = trxUtils.CreateTransactionFromDB(transactionId, Utilities);
                log.Debug("Completed building transaction from DB");

                string userName = string.Empty;
                string custName = string.Empty;
                string objectGuid = string.Empty;
                string phoneNumber = string.Empty;
                string cusCity = null;
                string cusCountry = null;
                string cusAdd1 = null;
                int userId = -1;
                if (amount < 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Negative amount not allowed!" });
                }

                TransactionBL transactionBLCust = new TransactionBL(Utilities.ExecutionContext, transactionId);
                userId = transactionBLCust.TransactionDTO.CustomerId;
                if (userId > -1)
                {
                    Parafait.Customer.CustomerBL customer = new Parafait.Customer.CustomerBL(Utilities.ExecutionContext, userId);
                    if (customer.CustomerDTO != null)
                    {
                        custName = customer.CustomerDTO.FirstName;
                        phoneNumber = customer.CustomerDTO.PhoneNumber;
                        userName = customer.CustomerDTO.Email;

                        if (customer.CustomerDTO.AddressDTOList != null && customer.CustomerDTO.AddressDTOList.Count > 0)
                        {
                            AddressDTO addressDTO = customer.CustomerDTO.AddressDTOList.FirstOrDefault(address => address != null);
                            if (addressDTO != null)
                            {
                                cusCity = !string.IsNullOrEmpty(addressDTO.City) ? addressDTO.City : null;
                                cusCountry = !string.IsNullOrEmpty(addressDTO.CountryName) ? addressDTO.CountryName : null;
                                cusAdd1 = !string.IsNullOrEmpty(addressDTO.Line1) ? addressDTO.Line1 : null;
                            }
                        }
                    }
                }
                if (PaymentGateways.VisaNetsHostedPayment.ToString().Equals(hostedPaymentGateway))
                {
                    userId = transactionBLCust.TransactionDTO.CustomerId;
                    if (userId > -1)
                    {
                        Parafait.Customer.CustomerBL customerBL = new Parafait.Customer.CustomerBL(Utilities.ExecutionContext, userId);
                        userName = customerBL.CustomerDTO.UserName;
                        custName = customerBL.CustomerDTO.FirstName;
                        objectGuid = customerBL.CustomerDTO.Guid;
                        phoneNumber = customerBL.CustomerDTO.PhoneNumber;

                    }
                }
                else if (NewTrx.customerDTO != null)
                {
                    custName = string.IsNullOrEmpty(NewTrx.customerDTO.ProfileDTO.FirstName) ? "" : NewTrx.customerDTO.ProfileDTO.FirstName;
                    userName = string.IsNullOrEmpty(NewTrx.customerDTO.Email) ? "" : NewTrx.customerDTO.Email;
                    phoneNumber = string.IsNullOrEmpty(NewTrx.customerDTO.PhoneNumber) ? "" : NewTrx.customerDTO.PhoneNumber;
                }

                //Get Email and phone number - in case of Guest login 
                if (!string.IsNullOrWhiteSpace(NewTrx.customerIdentifier))
                {
                    log.Debug("Guest login. Customer Identifier " + NewTrx.customerIdentifier);
                    string decryptedCustomerIdentifier = string.Empty;
                    if (NewTrx.customerIdentifier.IndexOf("|") == -1)
                    {
                        try
                        {
                            decryptedCustomerIdentifier = Encryption.Decrypt(NewTrx.customerIdentifier);
                            log.Debug("Customer Identifier after decrypt " + decryptedCustomerIdentifier);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Failed to decrypt " + NewTrx.customerIdentifier);
                            log.Error(ex.Message);
                        }
                    }
                    string[] customerIdentifierStringArray = decryptedCustomerIdentifier.Split(new[] { '|' });

                    userName = customerIdentifierStringArray.Length > 1 && !string.IsNullOrWhiteSpace(customerIdentifierStringArray[0]) ? customerIdentifierStringArray[0] : "";
                    phoneNumber = customerIdentifierStringArray.Length > 1 && !string.IsNullOrWhiteSpace(customerIdentifierStringArray[1]) ? customerIdentifierStringArray[1] : "";
                }

                log.Debug("userName: " + userName);
                log.Debug("custName: " + custName);
                log.Debug("objectGuid: " + objectGuid);
                log.Debug("userId: " + userId);
                log.Debug("phoneNumber: " + phoneNumber);

                int siteId = -1;
                if (NewTrx.site_id != null && !string.IsNullOrEmpty(NewTrx.site_id.ToString()))
                {
                    siteId = Convert.ToInt32(NewTrx.site_id);
                }

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

                log.Debug("HostedPaymentGateway:" + Utilities.ParafaitEnv.SiteId + ":" + Utilities.ParafaitEnv.IsCorporate + ":" + Utilities.ExecutionContext.GetSiteId() + ":" + Utilities.ExecutionContext.GetIsCorporate());

                PaymentGatewayFactory.GetInstance().Initialize(Utilities, true, null);
                HostedPaymentGateway paymentGateway = (HostedPaymentGateway)PaymentGatewayFactory.GetInstance().GetPaymentGateway(hostedPaymentGateway);
                if (paymentGateway != null)
                {
                    log.Debug("Got the payment gateway");
                    int gatewayLookUpId = -1;
                    LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, hostedPaymentGateway));
                    List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
                    if (lookupValuesDTOList != null &&
                       lookupValuesDTOList.Any())
                    {
                        gatewayLookUpId = lookupValuesDTOList[0].LookupValueId;
                        log.Debug("Got the gateway lookup gatewayLookUpId:" + gatewayLookUpId);
                    }
                    else
                    {
                        log.Debug("gatewayLookUpId not found " + searchParameters.ToString());
                        log.Debug("gatewayLookUpId not found");
                    }

                    log.Debug("Trying to get the payment modes");
                    PaymentModeList paymentModesListBL = new PaymentModeList(Utilities.ExecutionContext);
                    List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(false);
                    PaymentModeDTO paymentModeDTO = paymentModesDTOList.FirstOrDefault(x => x.Gateway.Equals(gatewayLookUpId));

                    if (paymentModeDTO == null)
                    {
                        log.Debug("Did not get the payment mode for this lookup id");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Wrong input", token = securityTokenDTO.Token });
                    }
                    SubscriptionAuthorizationMode subscriptionAuthorizationModeValue = SubscriptionAuthorizationMode.N;
                    if (NewTrx.HasSubscriptionProducts())
                    {
                        subscriptionAuthorizationModeValue = SubscriptionAuthorizationMode.I;
                    }
                    TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, transactionId, paymentModeDTO.PaymentModeId, amount, "",
                                      "", "", "", "", -1, "", -1, -1, -1, "", "", false, siteId, -1, "", NewTrx.TransactionDate, "", -1,
                                      -1, -1, -1, Utilities.ParafaitEnv.POSMachine, -1, Utilities.ParafaitEnv.CURRENCY_CODE, -1);

                    log.LogVariableState("subscriptionAuthorizationModeValue", subscriptionAuthorizationModeValue);

                    transactionPaymentsDTO.SubscriptionAuthorizationMode = subscriptionAuthorizationModeValue;
                    transactionPaymentsDTO.CreditCardName = string.IsNullOrEmpty(custName) ? "" : custName;
                    transactionPaymentsDTO.NameOnCreditCard = string.IsNullOrEmpty(userName) ? "" : userName;
                    transactionPaymentsDTO.CardEntitlementType = string.IsNullOrEmpty(phoneNumber) ? "" : phoneNumber;
                    transactionPaymentsDTO.Attribute3 = string.IsNullOrEmpty(cusCountry) ? "" : cusCountry;
                    transactionPaymentsDTO.Attribute4 = string.IsNullOrEmpty(cusCity) ? "" : cusCity;
                    transactionPaymentsDTO.Attribute5 = string.IsNullOrEmpty(cusAdd1) ? "" : cusAdd1;

                    if (PaymentGateways.VisaNetsHostedPayment.ToString().Equals(hostedPaymentGateway))
                    {
                        transactionPaymentsDTO.CustomerCardProfileId = userId > -1 ? userId.ToString() : "";
                        transactionPaymentsDTO.Reference = string.IsNullOrEmpty(objectGuid) ? "" : objectGuid;
                    }

                    HostedGatewayDTO hostedGatewayDTO = paymentGateway.CreateGatewayPaymentRequest(transactionPaymentsDTO);
                    if (hostedGatewayDTO != null)
                    {
                        TransactionDTO transactionDTO = NewTrx.TransactionDTO;
                        transactionDTO.Remarks = "PaymentModeId:" + paymentModeDTO.PaymentModeId + "| offsetapplied :" + (new TimeZoneUtil().GetOffSetDuration(siteId, NewTrx.TransactionDate));
                        TransactionBL transactionBL = new TransactionBL(Utilities.ExecutionContext, transactionDTO);
                        transactionBL.Save();
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = hostedGatewayDTO });
                    }
                }
                else
                {
                    log.Debug("The payment gateway not found");
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Wrong input" });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Transfer entitlements from parent card to child card
        /// </summary>
        [HttpPost]
        [Route("api/Transaction/HostedPaymentGateways")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] HostedGatewayDTO hostedGatewayDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
                Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                Utilities.ParafaitEnv.SiteId = Convert.ToInt32(hostedGatewayDTO.TransactionPaymentsDTO.SiteId);

                bool isCorporate = false;
                SiteList siteList = new SiteList(executionContext);
                SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                if (HQSite != null && HQSite.SiteId != -1 && HQSite.SiteId != Convert.ToInt32(hostedGatewayDTO.TransactionPaymentsDTO.SiteId))
                {
                    isCorporate = true;
                }

                Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                Utilities.ParafaitEnv.IsCorporate = isCorporate;
                Utilities.ExecutionContext.SetIsCorporate(isCorporate);
                Utilities.ExecutionContext.SetSiteId(Convert.ToInt32(hostedGatewayDTO.TransactionPaymentsDTO.SiteId));
                Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                Utilities.ParafaitEnv.Initialize();

                log.Debug("HostedPaymentGateway:" + Utilities.ParafaitEnv.SiteId + ":" + Utilities.ParafaitEnv.IsCorporate + ":" + Utilities.ExecutionContext.GetSiteId() + ":" + Utilities.ExecutionContext.GetIsCorporate());

                PaymentGatewayFactory.GetInstance().Initialize(Utilities, true, null);
                HostedPaymentGateway paymentGateway = (HostedPaymentGateway)PaymentGatewayFactory.GetInstance().GetPaymentGateway(hostedGatewayDTO.GatewayLookUp);
                if (paymentGateway != null)
                {
                    hostedGatewayDTO = paymentGateway.ProcessGatewayResponse(hostedGatewayDTO.GatewayResponseString);
                    if (hostedGatewayDTO != null)
                    {
                        CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(hostedGatewayDTO.CCTransactionsPGWDTO, Utilities.ExecutionContext);
                        cCTransactionsPGWBL.Save();
                        if (hostedGatewayDTO.TransactionPaymentsDTO != null)
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.CCResponseId = cCTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = hostedGatewayDTO });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Wrong input" });
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
