/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for ViewTransactions
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.90        27-May-2020       Vikas Dwivedi             Created to Get Methods.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Reports
{
    public class TransactionReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Object of TransactionDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/TransactionReports")]
        public HttpResponseMessage Get(int userId = -1, DateTime? fromDate = null, DateTime? toDate = null, int transactionId = -1, int orderId = -1, string status = null,
                                        int pOSMachineId = -1, string posMachine = null, string transactionOTP = null, int customerId = -1,
                                        string transactionNumber = null, string remarks = null, bool loadDetailsOnly = false,
                                        bool loadRefreshLine = false, bool downloadExcel = false, int trxIdForLineDetail = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(fromDate, toDate, transactionId, orderId, status, pOSMachineId, posMachine, transactionOTP, customerId,
                                   userId, transactionNumber, remarks, loadDetailsOnly, loadRefreshLine);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                Utilities utilities = new Utilities();
                Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
                UsersDTO usersDTO = user.UserDTO;
                DateTime trxFromDate = DateTime.MinValue;
                DateTime trxToDate = DateTime.MinValue;
                utilities.ParafaitEnv.User_Id = usersDTO.UserId;
                utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                utilities.ParafaitEnv.RoleId = securityTokenDTO.RoleId;
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                utilities.ParafaitEnv.Initialize();
                bool enableOrderShareAcrossUsers = false;
                bool enableOrderShareAcrossPOS = false;
                int userloggedIn = utilities.ParafaitEnv.User_Id;
                enableOrderShareAcrossUsers = (utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_USERS") == "Y");
                enableOrderShareAcrossPOS = (utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_POS") == "Y");
                bool showAmountFieldsTransaction = (utilities.getParafaitDefaults("SHOW_AMOUNT_FIELDS_MYTRANSACTIONS") == "Y");
                int bussinessStartHour = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")) ? 6 :
                Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"));
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> trxSearchParameter = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                if (transactionId > -1)
                {
                    trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                }
                else
                {

                    if (string.IsNullOrEmpty(posMachine) == false && posMachine.Trim() != " - All -".Trim())
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_NAME, posMachine));
                    }

                    if (userId > -1)
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.USER_ID, userId.ToString()));
                    }

                    if (fromDate != null && toDate != null && DateTime.Compare(Convert.ToDateTime(fromDate).Date, Convert.ToDateTime(toDate).Date) == 0)
                    {
                        trxFromDate = Convert.ToDateTime(fromDate);
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, trxFromDate.Date.AddHours(bussinessStartHour).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, trxFromDate.Date.AddDays(1).AddHours(bussinessStartHour).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        if (fromDate != null)
                        {
                            trxFromDate = Convert.ToDateTime(fromDate);
                            trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, trxFromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        if (toDate != null)
                        {
                            trxToDate = Convert.ToDateTime(toDate);
                            trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, trxToDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                    }
                    if (transactionId == -1 && (fromDate == null || toDate == null))
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }

                    if (orderId > -1)
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ORDER_ID, orderId.ToString()));
                    }
                    if (!string.IsNullOrEmpty(status) && status.Trim() != "-ALL-".Trim())
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, status.ToString()));
                    }
                    if (pOSMachineId > -1)
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_MACHINE_ID, pOSMachineId.ToString()));
                    }
                    if (!string.IsNullOrEmpty(posMachine))
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_NAME, posMachine.ToString()));
                    }
                    if (!string.IsNullOrEmpty(transactionOTP))
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_OTP, transactionOTP.ToString()));
                    }
                    if (customerId > -1)
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                    }
                    if (!string.IsNullOrEmpty(transactionNumber))
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_NUMBER, transactionNumber.ToString()));
                    }
                    if (!string.IsNullOrEmpty(remarks))
                    {
                        trxSearchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.REMARKS, remarks.ToString()));
                    }
                }
                DataTable headerDetails = new DataTable();
                DataTable linedetails = new DataTable();
                List<Sheet> sheets = new List<Sheet>();
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                log.LogMethodExit(headerDetails);
                headerDetails = transactionListBL.GetRefreshHeaderRecords(trxSearchParameter, utilities.ParafaitEnv.TRXNO_USER_COLUMN_HEADING, showAmountFieldsTransaction, userloggedIn, loadDetailsOnly, null);
                if (loadRefreshLine && transactionId > -1)
                {
                    log.LogMethodExit(linedetails);
                    linedetails = transactionListBL.GetRefreshedTransactionLines(transactionId, showAmountFieldsTransaction);
                }
                if (downloadExcel)
                {
                    try
                    {
                        if (loadRefreshLine && trxIdForLineDetail > -1)
                        {
                            linedetails = transactionListBL.GetRefreshedTransactionLines(trxIdForLineDetail, showAmountFieldsTransaction);
                        }
                        else
                        {
                            trxIdForLineDetail = headerDetails.Rows[0]["ID"] == DBNull.Value ? -1 : Convert.ToInt32(headerDetails.Rows[0]["ID"]);
                            linedetails = transactionListBL.GetRefreshedTransactionLines(trxIdForLineDetail, showAmountFieldsTransaction);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    string siteName = string.Empty;
                    List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                    searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                    SiteList siteList = new SiteList(executionContext);
                    var content = siteList.GetAllSites(searchParameters);
                    if (content != null)
                    {
                        siteName = content[0].SiteName;
                    }
                    sheets = transactionListBL.BuildTemplete(loadDetailsOnly, headerDetails, linedetails, fromDate, toDate, siteName, trxIdForLineDetail);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = headerDetails, lines = linedetails, Sheets = sheets });
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
