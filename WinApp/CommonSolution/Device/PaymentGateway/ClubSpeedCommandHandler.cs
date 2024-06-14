using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Newtonsoft.Json.Linq;
using Semnox.Parafait.Languages;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class ClubSpeedCommandHandler
    {
        #region members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.ExecutionContext ExecutionContext;
        List<LookupValuesContainerDTO> lookupValuesContainerDTOList = null;
        private string API_KEY = string.Empty;
        private string token = string.Empty;
        private string sessionId = string.Empty;
        private string GET_CUSTOMER_BALANCE = string.Empty;
        private string API_URL_AUTHORIZATION = string.Empty;
        private string GET_API_SETGIFT_CARD_URL = string.Empty;
        private string CLUBSPEED_USER_NAME = string.Empty;
        private string CLUBSPEED_PASSWORD = string.Empty;
        private string GET_API_REVERT_GIFT_CARD_URL = string.Empty;
        // Set the maximum allowed time in milliseconds
        const int MaxAllowedTime = 10000; // 10 seconds
        #endregion
        #region constructors
        /// <summary>
        /// Paramterized constructor with lookupvalues
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="lookupValuesContainerDTOList"></param>
        public ClubSpeedCommandHandler(Semnox.Core.Utilities.ExecutionContext executionContext, List<LookupValuesContainerDTO> lookupValuesContainerDTOList)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            this.lookupValuesContainerDTOList = lookupValuesContainerDTOList;
            log.LogMethodExit();
        }
        #endregion

        #region methods
        public Customer MakePayment(string cardId, double purchaseAmount, ExecutionContext executionContext)
        {
            log.LogMethodEntry(cardId, purchaseAmount, executionContext);
            log.Debug("Starting making payment for CardId:" + cardId);
            Customer customerDetailsAfterPayment = null;
            try
            {
                // get the balance for the customer
                log.Debug("Starting to get the card balance based on card Id:" + cardId);
                List<Customer> responseDTO = getCustomerBalance(cardId, executionContext);//added siteId
                log.Debug("Reponse generated for card Id:" + responseDTO);
                if (responseDTO != null)
                {
                    Customer customerDetails = null;
                    foreach (var item in responseDTO)
                    {
                        customerDetails = item;
                    }

                    if (customerDetails.balance < purchaseAmount)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 4715, purchaseAmount, customerDetails.cardId, customerDetails.balance);//The amount requested to pay is &1 but the balance of the card &2 is only &3.
                        throw new Exception(message);
                    }
                    // if balance > purchaseAmt then show balance and proceed to payment(deduction)

                    ClubSpeedRequestDTO requestDTO = new ClubSpeedRequestDTO
                    {
                        cardId = customerDetails.cardId,
                        payAmount = purchaseAmount * 1,
                        balance = customerDetails.balance,
                        sessionId = customerDetails.sessionId,
                    };
                    log.Debug("Starting to deduct card balance based on card Id:" + cardId);
                    Customer deductionResponse = updateBalance(requestDTO, executionContext);
                    log.Debug("After the decuction of Card balance the reposne:" + deductionResponse);
                    if (deductionResponse != null)
                    {
                        List<Customer> CustomerDataAfterPaymentResponseDTO = getCustomerBalance(cardId, executionContext);
                        if (CustomerDataAfterPaymentResponseDTO != null)
                        {

                            foreach (var item in CustomerDataAfterPaymentResponseDTO)
                            {
                                customerDetailsAfterPayment = item;
                            }
                            if (customerDetailsAfterPayment.balance != customerDetails.balance - purchaseAmount)
                            {
                                double diffAmount = customerDetails.balance - purchaseAmount;
                                // could not update points
                                throw new Exception(MessageContainerList.GetMessage(executionContext, 5182, diffAmount));//Something is wrong, balance difference after SettingBalance = &1
                            }
                            else
                            {
                                customerDetailsAfterPayment.message = "Payment Succeeded!";
                                customerDetailsAfterPayment.referenceNo = deductionResponse.cardId.ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            // success
            // update Parafait DB
            log.LogMethodExit(customerDetailsAfterPayment);
            return customerDetailsAfterPayment;
        }
        private Customer RevertBalance(ClubSpeedRequestDTO requestDTO, ExecutionContext executionContext)
        {
            log.LogMethodEntry(requestDTO, executionContext);
            dynamic responseDTO = null;
            Customer customerDTO = new Customer();
            try
            {
                string responseFromServer;
                string urlParams_RevertGiftCard = $"?CardId={requestDTO.cardId}&PayAmount={requestDTO.payAmount}&SessionId={requestDTO.sessionId}";
                string REVERT_GIFT_API_URL = GET_API_REVERT_GIFT_CARD_URL + urlParams_RevertGiftCard;
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(REVERT_GIFT_API_URL);
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + token);//sending token for Authorization
                string json = JsonConvert.SerializeObject(requestDTO, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                byte[] data = Encoding.UTF8.GetBytes(json);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                DateTime startTime = DateTime.Now; // Start the timer
                log.Debug("Starting to get response for Revert Gift Card");
                HttpWebResponse myHttpWebResponse;
                try
                {
                    myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    if (myHttpWebResponse == null)
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                }
                DateTime endTime = DateTime.Now; // Stop the timer

                TimeSpan elapsed = endTime - startTime; // Calculate the elapsed time

                if (elapsed.TotalMilliseconds > MaxAllowedTime)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4361));//Error occured while communicating with the server.
                }
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                responseFromServer = myStreamReader.ReadToEnd();
                log.Debug("Reponse from the server on Revert balance: " + responseFromServer);
                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();
                log.LogMethodEntry(responseFromServer);
                if (!string.IsNullOrWhiteSpace(responseFromServer))
                {
                    log.Debug("Response from the server after Revert Gift Card." + responseFromServer);
                    responseDTO = JsonConvert.DeserializeObject<dynamic>(responseFromServer);
                    customerDTO.balance = responseDTO.balance;
                    customerDTO.cardId = requestDTO.cardId;
                    customerDTO.sessionId = requestDTO.sessionId;
                }
                else
                {
                    log.Error("Response from the server is empty");
                    customerDTO = null;
                }
                return customerDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private Customer updateBalance(ClubSpeedRequestDTO requestDTO, ExecutionContext executionContext)
        {
            log.LogMethodEntry(requestDTO, executionContext);
            dynamic responseDTO = null;
            Customer customerDTO = new Customer();
            try
            {
                string responseFromServer;
                string urlParams_SetGiftCard = $"?CardId={requestDTO.cardId}&PayAmount={requestDTO.payAmount}&SessionId={requestDTO.sessionId}";
                string SET_GIFT_API_URL = GET_API_SETGIFT_CARD_URL + urlParams_SetGiftCard;
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(SET_GIFT_API_URL);
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + token);//sending token for Authorization

                string json = JsonConvert.SerializeObject(requestDTO, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                byte[] data = Encoding.UTF8.GetBytes(json);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                DateTime startTime = DateTime.Now; // Start the timer
                HttpWebResponse myHttpWebResponse;
                try
                {
                    myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    if (myHttpWebResponse == null)
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                }
                DateTime endTime = DateTime.Now;
                TimeSpan elapsed = endTime - startTime; // Calculate the elapsed time

                if (elapsed.TotalMilliseconds > MaxAllowedTime)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4361));
                }
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                responseFromServer = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();
                log.LogMethodEntry(responseFromServer);
                if (!string.IsNullOrWhiteSpace(responseFromServer))
                {
                    responseDTO = JsonConvert.DeserializeObject<dynamic>(responseFromServer);
                    customerDTO.balance = responseDTO.balance;
                    customerDTO.cardId = requestDTO.cardId;
                    customerDTO.sessionId = requestDTO.sessionId;
                }
                return customerDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        private string TrimEndURL(string url)
        {
            string trimmedURL = string.Empty;
            log.LogMethodEntry(url);
            if (url != null && url.EndsWith("/"))
            {
                trimmedURL = url.TrimEnd('/');
            }
            log.LogMethodExit();
            return trimmedURL;

        }
        private void TrimEndpoints()
        {
            log.LogMethodEntry();
            TrimEndURL(API_URL_AUTHORIZATION);
            TrimEndURL(GET_CUSTOMER_BALANCE);
            TrimEndURL(GET_API_SETGIFT_CARD_URL);
            TrimEndURL(GET_API_REVERT_GIFT_CARD_URL);
            log.LogMethodExit();
        }
        public List<Customer> getCustomerBalance(string cardId, ExecutionContext executionContext)
        {
            log.LogMethodEntry(cardId, executionContext);
            string errorMessage = string.Empty;
            List<Customer> customerReponseDTO = new List<Customer>();
            bool hasSpecialChars = false;
            GET_CUSTOMER_BALANCE = lookupValuesContainerDTOList.Where(x => x.LookupValue == "CLUBSPEED_GET_CUSTOMER_BALANCE_URL").FirstOrDefault().Description;
            API_URL_AUTHORIZATION = lookupValuesContainerDTOList.Where(x => x.LookupValue == "CLUBSPEED_AUTHORIZE_URL").FirstOrDefault().Description;
            CLUBSPEED_USER_NAME = lookupValuesContainerDTOList.Where(x => x.LookupValue == "CLUBSPEED_USER_NAME").FirstOrDefault().Description;
            CLUBSPEED_PASSWORD = lookupValuesContainerDTOList.Where(x => x.LookupValue == "CLUBSPEED_USER_PASSWORD").FirstOrDefault().Description;
            GET_API_SETGIFT_CARD_URL = lookupValuesContainerDTOList.Where(x => x.LookupValue == "CLUBSPEED_SET_GIFT_CARD_URL").FirstOrDefault().Description;
            GET_API_REVERT_GIFT_CARD_URL = lookupValuesContainerDTOList.Where(x => x.LookupValue == "CLUBSPEED_REVERT_GIFT_CARD_URL").FirstOrDefault().Description;
            if (string.IsNullOrWhiteSpace(CLUBSPEED_PASSWORD) || string.IsNullOrWhiteSpace(API_URL_AUTHORIZATION) || string.IsNullOrWhiteSpace(CLUBSPEED_USER_NAME)
                || string.IsNullOrWhiteSpace(CLUBSPEED_PASSWORD) || string.IsNullOrWhiteSpace(GET_CUSTOMER_BALANCE))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5183));//There is a problem with club speed setup..please check!!
            }
            TrimEndpoints();//method to trim end point if it has any '/' in the end under lookupvalues description

            hasSpecialChars = !Regex.IsMatch(cardId, @"^[a-zA-Z0-9]+$");//to validate if any special chracters entered for card number
            if (hasSpecialChars)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5184));//Gift cards only accept numbers.
            }
            string responseFromServer;
            Dictionary<string, string> credentials = new Dictionary<string, string>();
            //if (string.IsNullOrWhiteSpace(token))
            //{

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
            }

            credentials.Add("username", CLUBSPEED_USER_NAME);
            credentials.Add("password", CLUBSPEED_PASSWORD);
            credentials.Add("sessionid", sessionId);

            string json = JsonConvert.SerializeObject(credentials);
            log.Debug("Sending Authorization request");
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(API_URL_AUTHORIZATION);
                myHttpWebRequest.Method = "POST";
                byte[] data = Encoding.UTF8.GetBytes(json);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                DateTime startTime = DateTime.Now; // Start the timer
                HttpWebResponse myHttpWebResponse = null;
                try
                {
                    myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    if (myHttpWebResponse == null)
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                    }
                }

                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            string errorMessages = reader.ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(errorMessages))
                            {
                                JObject errorObject = JObject.Parse(errorMessages);
                                if (errorObject.ContainsKey("message"))
                                {
                                    // Get the value of the "message" key
                                    string message = (string)errorObject["message"];
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, message));
                                }
                            }
                            else
                            {
                                throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                }
                DateTime endTime = DateTime.Now; // Stop the timer

                TimeSpan elapsed = endTime - startTime; // Calculate the elapsed time

                if (elapsed.TotalMilliseconds > MaxAllowedTime)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4361));//Error occured while communicating with the server.
                }

                Stream responseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                responseFromServer = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();


                if (!string.IsNullOrWhiteSpace(responseFromServer))
                {
                    JToken jToken = JsonConvert.DeserializeObject<JToken>(responseFromServer);
                    token = jToken["token"].ToString();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            //}

            Customer customer = new Customer();
            dynamic responseDTO = null;
            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    ClubSpeedRequestDTO requestDTO = new ClubSpeedRequestDTO
                    {
                        cardId = cardId,
                        sessionId = sessionId
                    };
                    string urlParams = $"?cardId={requestDTO.cardId}&SessionId={requestDTO.sessionId}";
                    string API_URL = GET_CUSTOMER_BALANCE + urlParams;
                    string statusFromServer;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL);
                    request.Method = "GET";
                    request.Headers.Add("Authorization", "Bearer " + token);//sending token for Authorization
                    DateTime startTime = DateTime.Now; // Start the timer
                    HttpWebResponse response;
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                        if (response == null)
                        {
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 5178));//System cannot authorize giftcard payment at this time.Please check balance and try again."
                    }


                    DateTime endTime = DateTime.Now; // Stop the timer

                    TimeSpan elapsed = endTime - startTime; // Calculate the elapsed time

                    if (elapsed.TotalMilliseconds > MaxAllowedTime)
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 4361));
                    }
                    statusFromServer = ((HttpWebResponse)response).StatusDescription;
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    responseFromServer = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();

                    log.Debug("Reponse from the server -Get CardId" + responseFromServer);
                    if (!string.IsNullOrEmpty(responseFromServer))
                    {
                        responseDTO = JsonConvert.DeserializeObject<dynamic>(responseFromServer);
                        if (responseDTO.errorMessage == null)
                        {
                            customer.balance = responseDTO.balance;
                            customer.cardId = cardId;
                            customer.sessionId = sessionId;
                            customerReponseDTO.Add(customer);
                            log.Debug("responseDTO after Get CardId:" + responseDTO);
                        }
                        else
                        {
                            customer = null;
                            customerReponseDTO = null;

                        }
                    }
                    else
                    {
                        customer = null;
                        customerReponseDTO = null;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw;
                }
                log.LogMethodExit(responseDTO);

            }
            return customerReponseDTO;
        }

        public Customer RefundAmount(string cardId, double refundAmount, ExecutionContext executionContext)
        {
            log.LogMethodEntry(cardId, refundAmount, executionContext);
            Customer customerDetailsAfterProcess = null;
            try
            {
                // get the balance for the customer
                List<Customer> responseDTO = getCustomerBalance(cardId, executionContext);

                if (responseDTO == null)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4361));//Error occured while communicating with the server.
                }
                Customer customer = null;
                foreach (var item in responseDTO)
                {
                    customer = item;
                }
                ClubSpeedRequestDTO requestDTO = new ClubSpeedRequestDTO
                {
                    cardId = customer.cardId,
                    payAmount = refundAmount,
                    sessionId = customer.sessionId,
                };

                Customer response = RevertBalance(requestDTO, executionContext);
                if (response == null)
                {
                    //changes made
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4361));//Error occured while communicating with the server.
                }
                List<Customer> CustomerDataAfterRefundResponseDTO = getCustomerBalance(cardId, executionContext);
                if (CustomerDataAfterRefundResponseDTO == null)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4717));//Error fetching customer info after Refund.
                }
                foreach (var item in CustomerDataAfterRefundResponseDTO)
                {
                    customerDetailsAfterProcess = item;
                }
                if (customerDetailsAfterProcess.balance != customer.balance + refundAmount)
                {
                    // could not credit points
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 5179));//System cannot refund giftcard payment at this time.Make sure that payAmount is matching payment you wish to return, verify that the payment has not already been refunded.
                }
                customerDetailsAfterProcess.message = "Refund Succeeded!";
                customerDetailsAfterProcess.referenceNo = response.cardId.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(customerDetailsAfterProcess);
            return customerDetailsAfterProcess;
        }
    }
    #endregion
}