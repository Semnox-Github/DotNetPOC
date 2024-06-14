/********************************************************************************************
* Project Name - CommnonAPI - Transaction Module 
* Description  - API for the TransactionCount Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.140.0     24-Jun-2021     Fiona               Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Transaction;
using System.Globalization;
using System.Threading.Tasks;
using Semnox.Parafait.Customer;
using System.Linq;
using Semnox.Parafait.User;
using Semnox.Parafait.Site;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TransactionCountController: ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Hub Details List
        /// </summary>       
        /// <param name="isActive">isActive</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Transaction/GetTransactionsCount")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int transactionId = -1, string referenceId = null, string transactionOTP = null, string customerGuid = null, DateTime? fromDate = null, DateTime? toDate = null,
                                        int customerId = -1, int pageNumber = 0, int pageSize = 10, bool buildChildRecords = false, int lastTransactionId = -1, bool buildTickets = false, bool buildReceipt = false,
                                        string transactionStatus = null, bool onlineTransactionOnly = false, string originalSystemReference = null, int customerSignedWaiverId = -1, string transactionGuid = null,
                                        string customerIdentifier = null)
        {
            log.LogMethodEntry(transactionId, referenceId, transactionOTP, customerGuid, fromDate, toDate, customerId, pageNumber, pageSize, buildChildRecords, lastTransactionId, buildTickets,
                   buildReceipt, transactionStatus);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddDays(1);

                if (fromDate != null)
                {
                    //DateTime.TryParseExact(fromDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException});
                    }
                }

                if (toDate != null)
                {
                    //DateTime.TryParseExact(toDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException});
                    }
                }
                else
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    endDate = lookupValuesList.GetServerDateTime();
                }

                if (fromDate != null || toDate != null)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                if (transactionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                }

                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }

                if (!string.IsNullOrEmpty(transactionOTP))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_OTP, transactionOTP));
                }

                if (!string.IsNullOrEmpty(referenceId))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, referenceId));
                }

                if (!string.IsNullOrEmpty(transactionStatus))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, transactionStatus));
                }
                //else
                //{
                //    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, "CLOSED"));
                //}

                if (!string.IsNullOrEmpty(originalSystemReference))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE, originalSystemReference));
                }

                if (!string.IsNullOrEmpty(transactionGuid))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.GUID, transactionGuid.ToString()));
                }

                if (onlineTransactionOnly)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ONLINE_ONLY, string.Empty));
                }

                if (customerSignedWaiverId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID, customerSignedWaiverId.ToString()));
                }

                if (!string.IsNullOrEmpty(customerGuid))
                {
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    List<KeyValuePair<CustomerSearchByParameters, string>> customerSearchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                    //customerSearchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CUSTOMER_GUID, customerGuid));
                    //List<CustomerDTO> customerList = customerListBL.GetCustomerDTOList(customerSearchParameters, 0, 1, false, true);
                    CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria();
                    searchCriteria.And(CustomerSearchByParameters.CUSTOMER_GUID, Operator.EQUAL_TO, customerGuid);
                    List<CustomerDTO> customerList = customerListBL.GetCustomerDTOList(searchCriteria, 0, 1, false, true);
                    if (customerList != null && customerList.Any())
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_ID, customerList[0].Id.ToString()));
                    }
                    else
                    {
                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "No customers found with given GUID"});
                    }
                }

                if (!string.IsNullOrEmpty(customerIdentifier))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_IDENTIFIER, customerIdentifier));
                }
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                int transactionCount = await transactionUseCases.GetTransactionCount(searchParameters);
                log.LogMethodExit(transactionCount);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = transactionCount });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}