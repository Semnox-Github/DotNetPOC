/********************************************************************************************
* Project Name - Loyalty
* Description  - Aloha - Class Punchh Loyalty programs
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{

    /// <summary>
    /// Punchh
    /// </summary>
    public class PunchhBL : LoyaltyPrograms
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities Utilities;
        private string serviceUri;
        private string authorizationToken;
        private LoyaltyMemberDetails loyaltyMemberDetails;
        private bool continueOnError;

        /// <summary>
        /// Punchh
        /// </summary>
        /// <param name="_utilities"></param>
        public PunchhBL(Utilities _utilities) : base(_utilities)
        {
            log.LogMethodEntry(_utilities);
            Utilities = _utilities;
            continueOnError = false;
            log.LogMethodExit(null);
        }
        /// <summary>
        /// GetCustomers 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public override async Task<LoyaltyMemberDetails> GetCustomers(string phoneNumber)
        {
            log.LogMethodEntry(phoneNumber);
            string searchUrl = string.Empty;
            if (!serviceUri.ToString().EndsWith("/"))
            {
                searchUrl = "/api/pos/users/search?phone=";
            }
            else
            {
                searchUrl = "api/pos/users/search?phone=";
            }

            searchUrl = searchUrl + phoneNumber;
            searchUrl = serviceUri + searchUrl;
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Add("Authorization", authorizationToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
                client.DefaultRequestHeaders.Host = "pos.punchh.com";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                using (HttpResponseMessage response = await client.GetAsync(searchUrl))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        PunchhMemberDetailDTO memberDetails = serializer.Deserialize<PunchhMemberDetailDTO>(responseContent);
                        if (memberDetails != null)
                        {
                            loyaltyMemberDetails = new LoyaltyMemberDetails(memberDetails.phone, memberDetails.first_name, memberDetails.last_name, memberDetails.phone);
                        }
                    }
                    else
                    {
                        log.Debug("GetCustomers():We’re sorry, rewards are not available at this time. Please see front counter.");
                        string message = Utilities.MessageUtils.getMessage("We are unable to locate your account. Please check in at the front register to verify your account");// Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
                        //string message = Utilities.MessageUtils.getMessage(1458);// Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
                        log.LogMethodExit();
                        throw new ValidationException(message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("GetCustomers():We’re sorry, rewards are not available at this time. Please see front counter.");
                string message = Utilities.MessageUtils.getMessage(1458);// Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
                log.LogMethodExit();
                throw new ValidationException(message);
            }

            log.LogMethodExit(loyaltyMemberDetails);
            return loyaltyMemberDetails;
        }

        /// <summary>
        /// LoadLoyaltyConfigs
        /// </summary>
        public override void LoadLoyaltyConfigs()
        {
            log.LogMethodEntry();
            try
            {
                serviceUri = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOYALTY_INTERFACE_API_URL");
                if (string.IsNullOrWhiteSpace(serviceUri))
                {
                    log.Debug("Punchh API URL is not set . Please check the configurations");
                    throw new Exception("Punchh API URL is not set . Please check the configurations");
                }
                authorizationToken = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOYALTY_INTERFACE_AUTHORIZATION_KEY");
                if (string.IsNullOrWhiteSpace(authorizationToken))
                {
                    log.Debug("Punchh authorizationToken is not set . Please check the configurations");
                    throw new Exception("Punchh API authorizationToken is not set . Please check the configurations");
                }
                continueOnError = Utilities.getParafaitDefaults("IGNORE_THIRD_PARTY_SYNCH_ERROR").Equals("Y");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private PunchhTransactionDTO BuildPunchhTransactionDTO(DataRow dataRow)
        {
            try
            {
                log.LogMethodEntry(dataRow);
                //"The minimum checkin amount is $3.00" as per punchh so trx amount should be greater than 3
                PunchhTransactionDTO transactionDTO = new PunchhTransactionDTO();
                List<MenuItem> menuItems = new List<MenuItem>();
                int transactionNo = Convert.ToInt32(dataRow["TrxId"].ToString());
                string externalUid = dataRow["Guid"].ToString();
                string cardNumber = dataRow["LoyaltyCardNumber"].ToString();
                string posMachineId = dataRow["POSMachineId"].ToString();
                decimal receiptAmount = 0;
                string ccLast4 = string.Empty;
                TransactionUtils transactionUtils = new TransactionUtils(Utilities);
                Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionNo, utilities);
                int i = 0;
                if (transaction.TrxLines != null && transaction.TrxLines.Any())
                {

                    foreach (Transaction.Transaction.TransactionLine trxLine in transaction.TrxLines)
                    {
                        // Add products as items
                        if (Convert.ToDecimal(trxLine.LineAmount) >= 0)
                        {
                            MenuItem menuItem = new MenuItem(trxLine.ProductName, Convert.ToInt32(trxLine.quantity),
                                                         Convert.ToDecimal(trxLine.LineAmount), "M", trxLine.ProductID,
                                                         string.Empty, string.Empty, ++i);
                            menuItems.Add(menuItem);
                        }
                        else
                        {  // line reversed needs to check , skip 
                            MenuItem menuItem = new MenuItem(trxLine.ProductName, Convert.ToInt32(trxLine.quantity),
                                                         Convert.ToDecimal(trxLine.LineAmount), "-M", trxLine.ProductID,
                                                         string.Empty, string.Empty, ++i);
                            menuItems.Add(menuItem);
                        }
                        // Add tax as items
                        if (trxLine.tax_id > -1)
                        {
                            MenuItem menuItemForTax = new MenuItem(trxLine.taxName, 1,
                                                         Convert.ToDecimal(trxLine.tax_amount), "T", trxLine.tax_id,
                                                         string.Empty, string.Empty, ++i);
                            menuItems.Add(menuItemForTax);
                        }
                    }
                }
                // Add payments as item
                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                {
                    foreach (TransactionPaymentsDTO transactionPaymentsDTO in transaction.TransactionPaymentsDTOList)
                    {
                        MenuItem menuItem = new MenuItem(transactionPaymentsDTO.paymentModeDTO.PaymentMode,
                                                         1, Convert.ToDecimal(transactionPaymentsDTO.Amount), "P",
                                                          transactionPaymentsDTO.PaymentModeId, string.Empty,
                                                          string.Empty, ++i);
                        menuItems.Add(menuItem);

                        if (transactionPaymentsDTO.paymentModeDTO.IsCreditCard)
                        {
                            ccLast4 = transactionPaymentsDTO.CreditCardNumber.ToString().Substring(12, 4);
                        }
                    }
                }

                // Add Discounts as item
                if (transaction.DiscountsSummaryDTOList != null && transaction.DiscountsSummaryDTOList.Any())
                {
                    foreach (DiscountsSummaryDTO discountsSummaryDTO in transaction.DiscountsSummaryDTOList)
                    {
                        MenuItem menuItemForDiscount = new MenuItem(discountsSummaryDTO.DiscountName,
                                                         1, Convert.ToDecimal(discountsSummaryDTO.DiscountAmount), "D",
                                                          discountsSummaryDTO.DiscountId, string.Empty,
                                                          string.Empty, ++i);
                        menuItems.Add(menuItemForDiscount);

                    }
                }

                DateTime localDateTime = DateTime.Parse(transaction.TransactionDate.ToString());
                DateTime receiptDatetime = localDateTime.ToUniversalTime();
                int sequenceNo = transaction.Trx_id;
                string punchhKey = GetNextPunchhKey();
                decimal subTotalAmount = Convert.ToDecimal(transaction.Net_Transaction_Amount - transaction.Discount_Amount);
                string channel = "Parafait_Kiosk";
                string revenueId = string.Empty;
                string revenueCode = string.Empty;
                string employeeId = utilities.ExecutionContext.GetUserPKId().ToString();
                string employeeName = utilities.ExecutionContext.GetUserId();
                double payable = transaction.TotalPaidAmount;
                receiptAmount = Convert.ToDecimal(transaction.Transaction_Amount) < 0 ? 0 : Convert.ToDecimal(transaction.Transaction_Amount);
                transactionDTO = new PunchhTransactionDTO(cardNumber, receiptAmount, receiptDatetime, sequenceNo, punchhKey, transactionNo.ToString(),
                                                          subTotalAmount, channel, revenueId, revenueCode, employeeId, employeeName, payable,
                                                          menuItems, "parafait_kiosk", transaction.TrxGuid, string.Empty,
                                                          true, utilities.ParafaitEnv.SiteName, ccLast4);
                log.LogMethodExit(transactionDTO);
                return transactionDTO;
            }
            catch (Exception ex)
            {
                log.Debug("Error in building Punchh transactionDTO from parfait ");
                log.Error(ex);
                throw;
            }
        }

        private string GetNextPunchhKey()
        {
            log.LogMethodEntry();
            string punchhKey = string.Empty;
            try
            {
                //var bytes = new byte[sizeof(Int64)];
                //RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
                //Gen.GetBytes(bytes);
                //long random = BitConverter.ToInt64(bytes, 0);
                //string punchhKey = random.ToString().Replace("-", "").Substring(0, 12);
                //log.LogMethodExit();
                punchhKey = (ServerDateTime.Now.Ticks).ToString().Substring(0, 12);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Error while creating the punchh key.");
            }
            log.LogMethodExit(punchhKey);
            return punchhKey;
        }

        /// <summary>
        /// PostTransaction 
        /// </summary>
        /// <returns></returns>
        public override string PostTransaction()
        {
            log.LogMethodEntry();
            try
            {
                LoadLoyaltyConfigs();
                string message = string.Empty;
                int failedCount = 0;
                int concurrentRequestId = -1;
                DateTime endUpdateTime = ServerDateTime.Now;
                PunchhDataHandler punchhDataHandler = new PunchhDataHandler(null);
                DateTime lastUpdateTime = punchhDataHandler.GetLastUpdatedTime();
                LoyaltyProgramBL loyaltyProgramBL = new LoyaltyProgramBL(utilities.ExecutionContext);
                DataTable dtLoyaltyTrxDetails = punchhDataHandler.GetTransactionDetails(utilities.ParafaitEnv.IsCorporate ? utilities.ParafaitEnv.SiteId : -1);
                log.Debug("Last Synch Time: " + lastUpdateTime.ToString("dd-MMM-yyyy hh:mm:ss"));
                log.LogVariableState("dtLoyaltyTrxDetails", dtLoyaltyTrxDetails);
                if (dtLoyaltyTrxDetails != null && dtLoyaltyTrxDetails.Rows.Count == 0)
                {
                    log.Debug("No Loyalty Transactions to synchronize with Punchh. Returning");
                    return "Success";
                }
                else
                {
                    // Create request only if transaction exists for Post
                    log.Debug(" Creation Time: " + ServerDateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"));
                    concurrentRequestId = loyaltyProgramBL.CreateConcurrentRequest();
                    log.Debug(dtLoyaltyTrxDetails.Rows.Count.ToString() + " Loyalty transactions to synchronize with Punchh");
                }
                foreach (DataRow drLoyaltyTrx in dtLoyaltyTrxDetails.Rows)
                {
                    try
                    {
                        log.Debug("Transaction to Process : drLoyaltyTrx: " + drLoyaltyTrx);
                        PunchhTransactionDTO punchhTransactionDTO = BuildPunchhTransactionDTO(drLoyaltyTrx);
                        if (punchhTransactionDTO.receipt_amount <= 0)
                        {
                            continue; // skip reversed transactions 
                        }
                        //ExecuteAction(async () =>
                        //{
                        Core.GenericUtilities.WebApiResponse webApiResponse = null;
                        using (NoSynchronizationContextScope.Enter())
                        {
                            Task<Core.GenericUtilities.WebApiResponse> result =  PostLoyaltyTransactionAsync(punchhTransactionDTO);
                            result.Wait();
                            webApiResponse = result.Result;
                            log.LogVariableState("webApiResponse", webApiResponse);
                        }

                           // WebApiResponse webApiResponse = await PostLoyaltyTransactionAsync(punchhTransactionDTO);
                            log.LogVariableState("webApiResponse", webApiResponse);
                            if (webApiResponse != null && webApiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                PunchhDataHandler punchhDataHandlerForUpdate = new PunchhDataHandler(null);
                                int concurrentRequestDetailId = punchhDataHandlerForUpdate.UpdateConcurrentRequestDetails(drLoyaltyTrx["TrxId"], drLoyaltyTrx["LoyaltyProgramId"], drLoyaltyTrx["GUID"], true, "SUCCESS", drLoyaltyTrx["LoyaltyCardNumber"].ToString(), "Complete Assignment for " + drLoyaltyTrx["LoyaltyCardNumber"].ToString() + " Successful ", utilities.ParafaitEnv.Username, concurrentRequestId);
                                log.Debug("concurrentRequestDetailId : " + concurrentRequestDetailId);
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                PunchhMemberDetailDTO memberDetails = serializer.Deserialize<PunchhMemberDetailDTO>(webApiResponse.Response);
                                if (memberDetails != null)
                                {
                                    log.LogVariableState("memberDetails :", memberDetails);
                                    log.Debug("memberDetails : Name" + memberDetails.first_name + ": Phone    " + memberDetails.phone + " : Updated At  " + memberDetails.updated_at);
                                }
                            }
                            else
                            {
                                log.Debug("webApiResponse.StatusCode != OK");
                                log.Debug("Error from API : " + webApiResponse != null ? webApiResponse.ErrorMessage : "Unknown");
                                failedCount++;
                                PunchhDataHandler punchhDataHandlerForError = new PunchhDataHandler(null);
                                int concurrentRequestDetailId = punchhDataHandlerForError.UpdateConcurrentRequestDetails(drLoyaltyTrx["TrxId"], drLoyaltyTrx["LoyaltyProgramId"], drLoyaltyTrx["GUID"], false, "FAILED", drLoyaltyTrx["LoyaltyCardNumber"].ToString(), "Complete Assignment for " + drLoyaltyTrx["LoyaltyCardNumber"].ToString() + " Failed", utilities.ParafaitEnv.Username, concurrentRequestId);
                                log.Debug("concurrentRequestDetailId : " + concurrentRequestDetailId);
                            }
                     //   });

                    }
                    catch (Exception ex)
                    {
                        if (continueOnError)
                        {
                            log.Fatal(ex);
                            failedCount++;
                            PunchhDataHandler punchhDataHandlerForError = new PunchhDataHandler(null);
                            int concurrentRequestDetailId = punchhDataHandlerForError.UpdateConcurrentRequestDetails(drLoyaltyTrx["TrxId"], drLoyaltyTrx["LoyaltyProgramId"], drLoyaltyTrx["GUID"], false, "FAILED", drLoyaltyTrx["LoyaltyCardNumber"], DBNull.Value, utilities.ParafaitEnv.Username, concurrentRequestId);
                            log.Debug("concurrentRequestDetailId : " + concurrentRequestDetailId);
                        }
                        else
                        {
                            log.Fatal(ex);
                            failedCount++;
                            loyaltyProgramBL.UpdateConcurrentRequests(concurrentRequestId, "Error", endUpdateTime);
                            return "Failed";
                        }
                    }
                }
                loyaltyProgramBL.UpdateConcurrentRequests(concurrentRequestId, "Normal", endUpdateTime);
                log.Debug("Total number of failed records : " + failedCount);
                log.LogMethodExit("Success");
                return "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "Failed";
            }
        }

     
        private async Task<Core.GenericUtilities.WebApiResponse> PostLoyaltyTransactionAsync(PunchhTransactionDTO punchhTransactionDTO)
        {
            log.LogMethodEntry(punchhTransactionDTO);
            Core.GenericUtilities.WebApiResponse result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    string postAPIURL = string.Empty;
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.DefaultRequestHeaders.Add("Authorization", authorizationToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
                    client.DefaultRequestHeaders.Host = "pos.punchh.com";
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    if (!serviceUri.ToString().EndsWith("/"))
                    {
                        postAPIURL = serviceUri + "/api/pos/checkins";
                    }
                    else
                    {
                        postAPIURL = serviceUri + "api/pos/checkins";
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string assignmentRequestJSONString = serializer.Serialize(punchhTransactionDTO);
                    HttpResponseMessage assignmentRequestResponse = await client.PostAsync(postAPIURL.ToString(), new StringContent(assignmentRequestJSONString, Encoding.UTF8, "application/json"));
                    string response = await assignmentRequestResponse.Content.ReadAsStringAsync();
                    log.Debug("Response : " + response);
                    result = new Core.GenericUtilities.WebApiResponse(assignmentRequestResponse.StatusCode, response);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Failed to post the data : PostLoyaltyTransactionAsync()");
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}
