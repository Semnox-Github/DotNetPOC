/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalTransactionController  API -  Get and save transaction data in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 ********************************************************************************************* 
 *2.130.7    07-Apr-2022            M S Shreyas          Created
 *2.130.7    22-Jul-2022            Abhishek             Modified - External  REST API
 *2.151.3    04-Jun-2024            Abhishek             Modified - Addition of get parameter posMachine.
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.ThirdParty.External;
using Semnox.Parafait.Transaction;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using Semnox.Parafait.POS;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalTransactionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Transaction
        /// </summary>       
        /// <param name="externalTransactionDTO">externalTransactionDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Transaction/Transactions")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] ExternalTransactionDTO externalTransactionDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(externalTransactionDTO);
                if (externalTransactionDTO == null)
                {
                    string customException = "Transaction data cannot be null.Please enter the Transaction Details";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                if (externalTransactionDTO.Operators == null || string.IsNullOrEmpty(externalTransactionDTO.Operators.LoginId))
                {
                    executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                }
                else
                {
                    executionContext = GetExecutionContext(externalTransactionDTO);
                }
               
                if (externalTransactionDTO == null || String.IsNullOrEmpty(externalTransactionDTO.Type))
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Please enter the transaction type"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ExternalTransactionDTO externalTransaction = new ExternalTransactionDTO();
                ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, externalTransactionDTO);
                externalTransaction = externalTransactionBL.Save();
                log.LogMethodExit(externalTransaction);
                return Request.CreateResponse(HttpStatusCode.OK, externalTransaction);
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

        /// <summary>
        /// Get Transaction Object.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/External/Transaction/Transactions")]
        public async Task<HttpResponseMessage> Get(DateTime? fromTime = null, DateTime? toTime = null, int transactionId = -1, bool buildReceipt = false,
                                                   int customerId = -1, string transactionOTP = null, string posMachine = null, int posMachineId = -1,
                                                   int pageNumber = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(fromTime, toTime, transactionId, buildReceipt, customerId, transactionOTP, posMachine, posMachineId, pageNumber, pageSize);

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
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
                if (fromTime != null)
                {
                    //DateTime.TryParseExact(fromDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                    startDate = Convert.ToDateTime(fromTime.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (toTime != null)
                {
                    //DateTime.TryParseExact(toDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                    endDate = Convert.ToDateTime(toTime.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                else
                {
                    endDate = Utilities.getServerTime();
                }

                if (fromTime != null || toTime != null)
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
                if (!string.IsNullOrEmpty(posMachine))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_NAME, posMachine));
                }

                if (posMachineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));
                }
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                IList<TransactionDTO> TransactionDTOList = null;
                int totalNoOfTransactions = await transactionUseCases.GetTransactionCount(searchParameters);
                if (totalNoOfTransactions > 0)
                {
                    log.LogVariableState("totalNoOfCustomer", totalNoOfTransactions);
                    pageSize = pageSize > 500 || pageSize == 0 ? 500 : pageSize;
                    totalNoOfPages = (totalNoOfTransactions / pageSize) + ((totalNoOfTransactions % pageSize) > 0 ? 1 : 0);
                    pageNumber = pageNumber < -1 || pageNumber > totalNoOfPages ? 0 : pageNumber;
                }
                if (fromTime != null || toTime != null)
                {
                    if (totalNoOfTransactions > 0)
                    {
                        TransactionDTOList = await transactionUseCases.GetTransactionDTOList(searchParameters, Utilities, null, pageNumber, pageSize, true, false, buildReceipt);
                        foreach (TransactionDTO transactionDTO in TransactionDTOList)
                        {
                            if (transactionDTO.TransactionLinesDTOList != null && transactionDTO.TransactionLinesDTOList.Any())
                            {
                                transactionDTO.TransactionAmount = transactionDTO.TransactionLinesDTOList.Sum(x => x.Price);
                                if (transactionDTO.OriginalTransactionId > -1)
                                {
                                    transactionDTO.TransactionAmount = transactionDTO.TransactionAmount * -1;
                                }
                            }
                            transactionDTO.TransactionDiscountAmount = transactionDTO.TransactionAmount * (transactionDTO.TransactionDiscountPercentage / 100);
                        }
                        TransactionDTOList = new SortableBindingList<TransactionDTO>(TransactionDTOList);
                    }
                    log.LogMethodExit(TransactionDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = TransactionDTOList, currentPageNo = pageNumber, totalCount = totalNoOfTransactions });
                }
                else
                {
                    TransactionDTOList = await transactionUseCases.GetTransactionDTOList(searchParameters, Utilities, null, pageNumber, pageSize, true, false, buildReceipt);
                    if (TransactionDTOList != null && TransactionDTOList.Any())
                    {
                        foreach (TransactionDTO transactionDTO in TransactionDTOList)
                        {
                            if (transactionDTO.TransactionLinesDTOList != null && transactionDTO.TransactionLinesDTOList.Any())
                            {
                                transactionDTO.TransactionAmount = transactionDTO.TransactionLinesDTOList.Sum(x => x.Price);
                                if (transactionDTO.OriginalTransactionId > -1)
                                {
                                    transactionDTO.TransactionAmount = transactionDTO.TransactionAmount * -1;
                                }
                            }
                            transactionDTO.TransactionDiscountAmount = transactionDTO.TransactionAmount * (transactionDTO.TransactionDiscountPercentage / 100);
                        }
                        TransactionDTOList = new SortableBindingList<TransactionDTO>(TransactionDTOList);
                    }
                    log.LogMethodExit(TransactionDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, TransactionDTOList);
                }
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        /// <summary>
        /// Get Transaction Object.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/External/Transaction/{referenceId}/Transactions")]
        public async Task<HttpResponseMessage> Get([FromUri]string referenceId)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(referenceId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
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
                if (string.IsNullOrEmpty(referenceId))
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Please enter the ReferenceId"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                if (!string.IsNullOrEmpty(referenceId))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, referenceId));
                }

                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);

                IList<TransactionDTO> TransactionDTOList = null;
                TransactionDTOList = await transactionUseCases.GetTransactionDTOList(searchParameters, Utilities, null, 0, 10, true, false, false);
                if (TransactionDTOList != null && TransactionDTOList.Any())
                {
                    TransactionDTOList = new SortableBindingList<TransactionDTO>(TransactionDTOList);
                }
                log.LogMethodExit(TransactionDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, TransactionDTOList);
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        /// <summary>
        /// Set the context.
        /// </summary>
        private ExecutionContext GetExecutionContext(ExternalTransactionDTO externalTransactionDTO)
        {
            log.LogMethodEntry(externalTransactionDTO);
            int siteId = -1;
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            string loginId = externalTransactionDTO.Operators.LoginId;
            string posMachineName = externalTransactionDTO.Operators.PosMachine;
            ExecutionContext executionContext = null;
            if (externalTransactionDTO.Operators.SiteId > 0)
                siteId = externalTransactionDTO.Operators.SiteId;
            bool isCorporate = true;
            SiteList siteList = new SiteList(null);
            var content = siteList.GetAllSites(-1, -1, -1);
            if (content != null && content.Count > 1)
            {
                HttpContext.Current.Application["IsCorporate"] = "True";
                isCorporate = true;
                siteId = externalTransactionDTO.Operators.SiteId;
            }
            else
            {
                HttpContext.Current.Application["IsCorporate"] = "False";
                isCorporate = false;
            }
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId.ToString()));
            searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, loginId));

            UsersList usersList = new UsersList(null);
            List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
            if (usersDTOs == null || !usersDTOs.Any())
                throw new ValidationException("User not found");

            UsersDTO user = usersDTOs.Find(x => x.LoginId == loginId);

            if (user == null)
                throw new ValidationException("User not found");


            ExecutionContext tempContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, isCorporate, -1);
            int machineId = -1;
            if (!String.IsNullOrEmpty(posMachineName))
            {
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, posMachineName));
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                POSMachineList pOSMachineList = new POSMachineList(tempContext);
                List<POSMachineDTO> pOSMachineDTOList = pOSMachineList.GetAllPOSMachines(searchParameters, false, false);
                if (pOSMachineDTOList == null || !pOSMachineDTOList.Any())
                {
                    String messages = "POS Machine " + posMachineName + " is not set up.";
                    log.Error(messages);
                    throw new ValidationException(messages);
                }
                machineId = pOSMachineDTOList.FirstOrDefault().POSMachineId;
            }

            if (isCorporate)
            {
                securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, user.SiteId.ToString(), Convert.ToString(-1), Convert.ToString(user.RoleId), machineid: machineId.ToString(), userSessionId: Guid.NewGuid().ToString());
            }
            else
            {
                securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, "-1", Convert.ToString(-1), Convert.ToString(user.RoleId), machineid: machineId.ToString(), userSessionId: Guid.NewGuid().ToString());
            }
            SecurityTokenDTO securityTokenDTO = null;
            securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, machineId, user.UserId, isCorporate, Convert.ToInt32(securityTokenDTO.LanguageId));
            log.LogMethodExit(executionContext);
            return executionContext;
        }
    }
}



