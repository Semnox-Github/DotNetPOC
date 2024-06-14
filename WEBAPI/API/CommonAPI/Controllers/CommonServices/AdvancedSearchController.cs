/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - Common API for the AdvancedSearch entity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.60.2      27-May-2019   Jagan Mohana    Created
 *2.100.0     27-Aug-2020   Girish Kundar   Modified : Moved helper class files
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.CommonAPI.CommonServices
{
    public class AdvancedSearchController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;
        private SecurityTokenBL securityTokenBL;
        private SearchCriteria searchCriteria;

        /// <summary>
        /// Performs a Post operation on AdvancedSearch entity
        /// Max 20 Customers per page
        /// Currentpage number displays Customers on current page ex: Page 1 displays 20 customers so on
        /// </summary>
        /// <param name="scheduleDTO"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/CommonServices/AdvancedSearch/")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<AdvancedSearchDTO> advancedSearchDTOList, int currentPage = 0, int pageSize = 0)
        {
            try
            {
                log.LogMethodEntry(advancedSearchDTOList);
                securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                int totalNoOfPages = 0;                
                if (advancedSearchDTOList != null)
                {
                    AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria();
                    this.searchCriteria = new AccountSearchCriteria();
                    AccountSearchCriteria newAccountSearchCriteria = new AccountSearchCriteria();
                    ColumnProvider columnProvider = accountSearchCriteria.GetColumnProvider();
                    Operator operatorColumn = new Operator();
                    Enum columnIdentifier = null;
                    Dictionary<Enum, Column> columnDictionary = new Dictionary<Enum, Column>();
                    columnDictionary = columnProvider.GetAllColumns();

                    Operator @operator = new Operator();
                    foreach (AdvancedSearchDTO advancedSearchDTO in advancedSearchDTOList)
                    {
                        object obj = null;

                        if (!string.IsNullOrEmpty(advancedSearchDTO.Column))
                        {
                            if (!string.IsNullOrEmpty(advancedSearchDTO.Column))
                            {
                                foreach (var findColumn in columnDictionary)
                                {
                                    if (findColumn.Value.Name == advancedSearchDTO.Column)
                                    {
                                        columnIdentifier = findColumn.Key;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(advancedSearchDTO.Operator))
                        {
                            operatorColumn = (Operator)Enum.Parse(typeof(Operator), advancedSearchDTO.Operator);
                        }
                        if (!string.IsNullOrEmpty(advancedSearchDTO.Parameter))
                        {
                            obj = GetValue(columnDictionary, columnIdentifier, operatorColumn, advancedSearchDTO.Parameter);
                        }
                        if (advancedSearchDTO.Criteria.ToUpper().ToString() == "AND")
                        {
                            accountSearchCriteria.And(columnIdentifier, operatorColumn, obj);
                        }
                        else if (advancedSearchDTO.Criteria.ToUpper().ToString() == "OR")
                        {
                            accountSearchCriteria.Or(columnIdentifier, operatorColumn, obj);
                        }
                    }
                    AccountListBL advancedSearchBL = new AccountListBL(executionContext);
                    int totalNoOfCustomer = await Task<int>.Factory.StartNew(() => { return advancedSearchBL.GetAccountCount(accountSearchCriteria, null); });
                    log.LogVariableState("totalNoOfCustomer", totalNoOfCustomer);
                    totalNoOfPages = (totalNoOfCustomer / pageSize) + ((totalNoOfCustomer % pageSize) > 0 ? 1 : 0);
                    IList<AccountDTO> accountDTOList = null;
                    if (totalNoOfPages > 0)
                    {
                        accountSearchCriteria.OrderBy(AccountDTO.SearchByParameters.ISSUE_DATE, OrderByType.DESC);
                        accountSearchCriteria.OrderBy(AccountDTO.SearchByParameters.ACCOUNT_ID, OrderByType.DESC);
                        accountSearchCriteria.Paginate(currentPage, pageSize);
                        accountDTOList = await Task<List<AccountDTO>>.Factory.StartNew(() => { return advancedSearchBL.GetAccountDTOList(accountSearchCriteria, true, true); });
                        //accountDTOList = accountDTOList.OrderByDescending(m => m.IssueDate).ThenByDescending(m => m.AccountId).ToList();
                        accountDTOList = new SortableBindingList<AccountDTO>(accountDTOList);
                    }
                    log.LogMethodExit(accountDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = accountDTOList, currentPageNo = currentPage, totalCount = totalNoOfCustomer, token = securityTokenDTO.Token });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        private object GetValue(Dictionary<Enum, Column> columnDictionary, Enum columnIdentifier, Operator operatorColumn, string parameter)
        {
            log.LogMethodEntry();
            object returnValue = null;
            Column column = columnDictionary[(Enum)columnIdentifier];
            ColumnType columnType = column.GetColumnType();
            Operator @operator = operatorColumn;
            if (@operator != Operator.IS_NOT_NULL &&
                @operator != Operator.IS_NULL)
            {
                if (columnType == ColumnType.DATE_TIME)
                {
                    returnValue = Convert.ToDateTime(parameter);
                }
                else if (columnType == ColumnType.NUMBER)
                {
                    decimal value;
                    decimal.TryParse(parameter, out value);
                    returnValue = value;
                }
                else if (columnType == ColumnType.TEXT)
                {
                    returnValue = parameter;
                }
                else if (columnType == ColumnType.BIT)
                {
                    returnValue = true ? parameter : "Y";
                }
                else if (columnType == ColumnType.UNIQUE_IDENTIFIER)
                {
                    Guid value;
                    Guid.TryParse(parameter, out value);
                    returnValue = value;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}