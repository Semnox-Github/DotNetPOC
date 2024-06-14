/********************************************************************************************
 * Project Name - CommnonAPI - Transaction Module                                                                     
 * Description  - Controller for posting the customer transactions
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 *2.70         26-Jul-2019   Nitin Pai            Moving from Transaction Core to Transaction
 *2.70.2       15-Oct-2019   Nitin Pai            Implementing transaction.get for guest app phase 2
 *2.80         26-Nov-2019   Lakshminarayana      Virtual store enhancement
 *2.80.0         04-Jun-2020   Nitin                Removed TrxHeaderDTO and TrxLineDTO. Using TransactionDTO and TransactionLineDTO impact in Execute(), CreateTransactionLine() and SaveTransaction()
 *2.80.0         04-Jun-2020   Nitin                Website Enhancement - Continue as Guest - Saving CustomerIdentifier field in trx_header table, 
 *2.80.0         04-Jun-2020   Nitin                Pending code review comments
 *2.130.0        07-Sep-2021   Nitin              Added isCancelled filter criteria.
 *2.140.0        09-Jun-2021   Fiona              Modified: As per new format
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
using System.Configuration;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TransactionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Transfer entitlements from parent card to child card
        /// </summary>
        [HttpPost]
        [Route("api/Transaction/Transactions")]
        [Authorize]
        public HttpResponseMessage PostTransaction([FromBody]Semnox.Parafait.Transaction.TransactionDTO transactionDTO)
        {
            
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(transactionDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (transactionDTO != null)
                {
                    String checkSite = ConfigurationManager.AppSettings["ENABLE_TOKEN_SITE_TRX_ENFORCEMENT"];
                    if (securityTokenDTO.MachineId != -1 && !string.IsNullOrWhiteSpace(checkSite) &&
                            (checkSite.Equals("Y", StringComparison.InvariantCultureIgnoreCase)
                            || checkSite.Equals("TRUE", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        log.Debug("Check if trx is being created for the same machine " + securityTokenDTO.MachineId);
                        POSMachines pOSMachines = new POSMachines(executionContext, securityTokenDTO.MachineId, false, false, null);
                        POSMachineDTO pOSMachineDTO = pOSMachines.POSMachineDTO;
                        if (pOSMachineDTO == null || !pOSMachineDTO.POSName.Equals(transactionDTO.PosMachine, StringComparison.InvariantCultureIgnoreCase))
                        {
                            log.Debug("Trx is being created for a different machine " + transactionDTO.PosMachine + ":" + pOSMachineDTO.POSName);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "POS Machine does not exist." });
                        }
                    }

                    String virtualSiteTransaction = ConfigurationManager.AppSettings["ENABLE_VIRTUAL_SITE_TRX_ENFORCEMENT"];
                    if (!string.IsNullOrWhiteSpace(virtualSiteTransaction) &&
                            (virtualSiteTransaction.Equals("Y", StringComparison.InvariantCultureIgnoreCase)
                            || virtualSiteTransaction.Equals("TRUE", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        String virtualStore = ParafaitDefaultContainerList.GetParafaitDefault(securityTokenDTO.SiteId, "VIRTUAL_STORE_SITE_ID");
                        int virtualSiteId = 0;
                        int.TryParse(virtualStore, out virtualSiteId);
                        if (virtualSiteId > 0 && virtualSiteId != transactionDTO.SiteId)
                        {
                            string allowLocationOverride = ParafaitDefaultContainerList.GetParafaitDefault(virtualSiteId, "ALLOW_ONLINE_RECHARGE_LOCATION_OVERIDE");
                            if (string.IsNullOrWhiteSpace(allowLocationOverride) || allowLocationOverride.ToUpper().Equals("N"))
                            {
                                log.Debug("Trx is being created for a non virtual site " + virtualSiteId + ":" + transactionDTO.SiteId);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Trx is being created for a non virtual site." });
                            }
                        }
                    }

                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionDTO);
                    TransactionDTO TransactionDTO = transactionBL.Execute();
                    log.LogMethodExit(TransactionDTO);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = TransactionDTO});
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Wrong input"});
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = customException});
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException});
            }
        }

        /// <summary>
        /// Get Transaction Object.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/Transactions")]
        public async Task<HttpResponseMessage> Get(int transactionId = -1, string referenceId = null, string transactionOTP = null, string customerGuid = null, DateTime? fromDate = null, DateTime? toDate = null,
                                        int customerId = -1, int pageNumber = 0, int pageSize = 10, bool buildChildRecords = false, int lastTransactionId = -1, bool buildTickets = false, bool buildReceipt = false,
                                        string transactionStatus = null, bool onlineTransactionOnly = false, string originalSystemReference = null, int customerSignedWaiverId = -1, string transactionGuid = null,
                                        string customerIdentifier = null, Boolean? includeCancelled = false, bool needsOrderDispensingEntry = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(transactionId, referenceId, transactionOTP, customerGuid, fromDate, toDate, customerId, pageNumber, pageSize, buildChildRecords, lastTransactionId, buildTickets,
                    buildReceipt, transactionStatus);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
                Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
                Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                Utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
                Utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
                Utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
                Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                Utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
                //Utilities.ParafaitEnv.Initialize();                

                int totalNoOfPages = 0;
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
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
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
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                }
                else
                {
                    endDate = Utilities.getServerTime();
                }

                if (fromDate != null || toDate!= null )
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
                else if (includeCancelled == null || includeCancelled == false)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS_NOT_IN, "CANCELLED"));
                }
                //else
                //{
                //    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, "CLOSED"));
                //}

                if(!string.IsNullOrEmpty(originalSystemReference))
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
                if(needsOrderDispensingEntry)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.NEEDS_ORDER_DISPENSING, "1"));
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
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "No customers found with given GUID", token = securityTokenDTO.Token });
                    }
                }

                if (!string.IsNullOrEmpty(customerIdentifier))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_IDENTIFIER, customerIdentifier));
                }
                
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                int totalNoOfTransactions =await transactionUseCases.GetTransactionCount(searchParameters);
                IList<TransactionDTO> TransactionDTOList = null;
                if (totalNoOfTransactions > 0)
                {
                    log.LogVariableState("totalNoOfCustomer", totalNoOfTransactions);
                    pageSize = pageSize > 500 || pageSize == 0 ? 500 : pageSize;
                    totalNoOfPages = (totalNoOfTransactions / pageSize) + ((totalNoOfTransactions % pageSize) > 0 ? 1 : 0);
                    pageNumber = pageNumber < -1 || pageNumber > totalNoOfPages ? 0 : pageNumber;

                   
                    TransactionDTOList = await transactionUseCases.GetTransactionDTOList(searchParameters, Utilities, null, pageNumber, pageSize, buildChildRecords, buildTickets, buildReceipt);
                    TransactionDTOList = new SortableBindingList<TransactionDTO>(TransactionDTOList);
                }
                log.LogMethodExit(TransactionDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = TransactionDTOList, currentPageNo = pageNumber, totalCount = totalNoOfTransactions, token = securityTokenDTO.Token });

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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });

            }
        }
    }
}
