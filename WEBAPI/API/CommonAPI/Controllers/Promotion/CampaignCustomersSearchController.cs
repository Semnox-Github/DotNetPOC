/********************************************************************************************
 * Project Name - Promotions
 * Description  - Created to Get Campaign Customers in Promotions
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.80      18-Nov-2019       Rakesh             Created 
 *2.80      20-Dec-2019       Jagan Mohana       Modified post method for both Cards and Customer search as JObject
 *2.80      22-Apr-2020       Mushahid Faizan    Removed token from response body.
 *2.100.0     27-Aug-2020   Girish Kundar   Modified : Moved helper class files
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Promotions;

namespace Semnox.CommonAPI.Promotion
{
    public class CampaignCustomersSearchController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SearchCriteria searchCriteria;
        /// <summary>
        /// Post the JSON Object of Campaign Customers Details Search criteria List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Promotion/CampaignCustomersSearch")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] JObject jObject, string membershipId = null, string totalAddPoint = null, string totalSpend = null, string totalAddPointOperator = null, string totalSpendOperator = null, string birthDayInDays = null)
        {
            try
            {
                log.LogMethodEntry(jObject);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (jObject != null)
                {
                    List<AdvancedSearchDTO> cardSearchDTOList = new List<AdvancedSearchDTO>();
                    List<AdvancedSearchDTO> customerSearchDTOList = new List<AdvancedSearchDTO>();

                    string cardSearchDTOString = jObject.SelectToken("CardSearchDTOList").ToString();
                    string customerSearchDTOString = jObject.SelectToken("CustomerSearchDTOList").ToString();

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    // Deserialize the string to the List<AdvancedSearchDTO>
                    if (!string.IsNullOrEmpty(cardSearchDTOString))
                    {
                        cardSearchDTOList = JsonConvert.DeserializeObject<List<AdvancedSearchDTO>>(cardSearchDTOString, settings);
                    }

                    // Deserialize the string to the List<AdvancedSearchDTO>
                    if (!string.IsNullOrEmpty(customerSearchDTOString) && customerSearchDTOString != "RELOAD")
                    {
                        customerSearchDTOList = JsonConvert.DeserializeObject<List<AdvancedSearchDTO>>(customerSearchDTOString, settings);
                    }
                    CampaignCustomerSearchCriteria customerSearchCriteria = new CampaignCustomerSearchCriteria();
                    AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria();

                    string advCardCriteria = string.Empty;
                    if (customerSearchDTOList != null && customerSearchDTOList.Count != 0)
                    {
                        this.searchCriteria = new CampaignCustomerSearchCriteria();
                        CampaignCustomerSearchCriteria newCustomerSearchCriteria = new CampaignCustomerSearchCriteria();
                        ColumnProvider columnProvider = customerSearchCriteria.GetColumnProvider();
                        Operator operatorColumn = new Operator();
                        Enum columnIdentifier = null;
                        Dictionary<Enum, Column> columnDictionary = new Dictionary<Enum, Column>();
                        columnDictionary = columnProvider.GetAllColumns();
                        List<SqlParameter> sqlParameters = new List<SqlParameter>();
                        Operator @operator = new Operator();
                        foreach (AdvancedSearchDTO advancedSearchDTO in customerSearchDTOList)
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

                     }
                    if (cardSearchDTOList != null && cardSearchDTOList.Count != 0)
                    {
                        this.searchCriteria = new AccountSearchCriteria();
                        AccountSearchCriteria newAccountSearchCriteria = new AccountSearchCriteria();
                        ColumnProvider columnProvider = accountSearchCriteria.GetColumnProvider();
                        Operator operatorColumn = new Operator();
                        Enum columnIdentifier = null;
                        Dictionary<Enum, Column> columnDictionary = new Dictionary<Enum, Column>();
                        columnDictionary = columnProvider.GetAllColumns();

                        Operator @operator = new Operator();
                        foreach (AdvancedSearchDTO advancedSearchDTO in cardSearchDTOList)
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
                    }
                    CampaignCustomerListBL campaignCustomerListBL = new CampaignCustomerListBL(executionContext);
                    var content = campaignCustomerListBL.GetCampaignCustomerDTOList(membershipId, totalAddPoint, totalSpend, totalAddPointOperator,
                                                          totalSpendOperator, birthDayInDays, customerSearchCriteria, accountSearchCriteria);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
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
