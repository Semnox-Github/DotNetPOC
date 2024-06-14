/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - Common API for the AdvancedCustomerCriteria entity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.80        18-Dec-2019   Vikas Dwivedi   Created
 *2.90        27-Jul-2020   Mushahid Faizan Modified as per Rest API standards
*2.100.0     27-Aug-2020   Girish Kundar   Modified : Moved helper class files
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class CustomerSearchController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Post operation on AdvancedCustomerSearch entity
        /// Max 20 Customers per page
        /// Currentpage number displays Customers on current page ex: Page 1 displays 20 customers so on
        /// </summary>
        /// <param name="advancedSearchDTOList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Customer/CustomerSearch/")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<AdvancedSearchDTO> advancedSearchDTOList, int currentPage = 0, int pageSize = 10)
        {

            SecurityTokenDTO securityTokenDTO=null;
            ExecutionContext executionContext = null;
            SearchCriteria searchCriteria;

            try
            {
                log.LogMethodEntry(advancedSearchDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                int totalNoOfPages = 0;
                if (advancedSearchDTOList != null && advancedSearchDTOList.Any())
                {
                    CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
                    searchCriteria = new CustomerSearchCriteria();
                    CustomerSearchCriteria newCustomerSearchCriteria = new CustomerSearchCriteria();
                    ColumnProvider columnProvider = customerSearchCriteria.GetColumnProvider();
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
                            customerSearchCriteria.And(columnIdentifier, operatorColumn, obj);
                        }
                        else if (advancedSearchDTO.Criteria.ToUpper().ToString() == "OR")
                        {
                            customerSearchCriteria.Or(columnIdentifier, operatorColumn, obj);
                        }
                    }
                    CustomerListBL advancedSearchBL = new CustomerListBL(executionContext);
                    int totalNoOfCustomer = await Task<int>.Factory.StartNew(() => { return advancedSearchBL.GetCustomerCount(customerSearchCriteria, null); });
                    log.LogVariableState("totalNoOfCustomer", totalNoOfCustomer);
                    totalNoOfPages = (totalNoOfCustomer / pageSize) + ((totalNoOfCustomer % pageSize) > 0 ? 1 : 0);
                    IList<CustomerDTO> customerDTOList = null;
                    if (totalNoOfPages > 0)
                    {
                        // customerSearchCriteria.Paginate(currentPage, pageSize);
                        customerDTOList = await Task<List<CustomerDTO>>.Factory.StartNew(() => { return advancedSearchBL.GetCustomerDTOList(customerSearchCriteria, true, true); });
                        customerDTOList = new SortableBindingList<CustomerDTO>(customerDTOList);
                    }
                    log.LogMethodExit(customerDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customerDTOList, currentPageNo = currentPage, totalPages = totalNoOfCustomer });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
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
