/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert product details in the product entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        10-Jan-2019   Jagan Mohana Rao          Created to get, insert, update and Delete Methods.
 *2.60        24-Mar-2019   Nagesh Badiger           Added Custom Generic Exception and log method entry and method exit
 **********************************************************************************************
 *2.60        01-Apr-2019    Akshay Gulaganji        modified isActive search Parameter check 
 **********************************************************************************************
 *2.70        29-Jun-2019  Indrajeet Kumar          Modified Delete() - Implemented Hard Deletion
 *2.70        05-Aug-2019   Akshay Gulaganji        modified Get()
 *2.80        12-May-2020   Nitin Pai               Cobra Changes - New website show transaction products
 * *2.110.0     14-Dec-2020   Deeksha                  Added Put method logic.
 *2.120.00     28-Apr-2021  Mushahid Faizan       Modified - Return DTOList in Put/Post response for WMS UI.
 *2.130.04    17-Feb-2022   Nitin Pai               Added Product offset value to calculate against Product Calendar for website and app
 *2.130.05    17-Mar-2022   Nitin Pai               Add the offset time to purchase date only if it is sent by client. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Product;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using Semnox.Parafait.User;
using Semnox.Parafait.Customer;
using Semnox.CommonAPI.Helpers;
using Ganss.XSS;

namespace Semnox.CommonAPI.Products
{
    public class ProductsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;
        string productsType;
        bool activeChildRecords = false;
        /// <summary>
        /// Get the JSON products list
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/Products")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string isActive = null, string productType = null, bool isSellable = false, string productId = null,
              string posCounter = null, string displayGroup = null, string productName = null, int currentPage = 0, int pageSize = 20, DateTime? purchaseDate = null, bool displayInPOS = false,
            int displayGroupId = -1, int categoryId = -1, string externalSystemReference = null, bool buildChildRecords = false, bool buildForTransaction = true, string POSMachine = null, int customerId = -1,
            bool isSubscriptionProduct = false, string isSubscriptionORHasActiveSubscriptionChild = null)
        {

            try
            {
                log.LogMethodEntry(isActive, productType, isSellable, posCounter, displayGroup, productName, currentPage, pageSize, isSubscriptionProduct, isSubscriptionORHasActiveSubscriptionChild);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                int totalNoOfPages = 0;
                int totalNoOfProducts = 0;
                List<ProductsDTO> productsDTOList = new List<ProductsDTO>();
                ProductsList productList = new ProductsList(executionContext);
                siteId = siteId == -1 ? executionContext.GetSiteId() : siteId;

                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, siteId.ToString()));
                if (!string.IsNullOrEmpty(productId))
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, productId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        activeChildRecords = true;
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(posCounter) && posCounter != "-1")
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.POS_TYPE_ID, posCounter));
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_NAME, productName));
                }
                if (!string.IsNullOrEmpty(displayGroup))
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.DISPLAY_GROUP_NAME, displayGroup));
                }
                if (isSellable)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.IS_SELLABLE, "Y"));
                }
                if (displayInPOS)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.DISPLAY_IN_POS, "Y"));
                }
                if (displayGroupId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID, displayGroupId.ToString()));
                }
                if (categoryId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.CATEGORY_ID, categoryId.ToString()));
                }
                if (!String.IsNullOrEmpty(externalSystemReference))
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE, externalSystemReference));
                }
               
                if (!string.IsNullOrEmpty(productType))
                {
                    string[] typesArray = productType.Split(',');

                    for (int i = 0; i < typesArray.Count(); i++)
                    {
                        if (!String.IsNullOrEmpty(productsType))
                            productsType += ",";

                        String temp = typesArray[i];
                        switch (temp.ToUpper().ToString())
                        {
                            case "CARDS":
                                productsType += "'LOCKER','LOCKER_RETURN','NEW','RECHARGE','VARIABLECARD','CARDSALE','GAMETIME'";
                                break;
                            case "NEWCARD":
                                productsType += "'NEW','VARIABLECARD','CARDSALE','GAMETIME'";
                                break;
                            case "RECHARGE":
                                productsType += "'RECHARGE','VARIABLECARD','CARDSALE','GAMETIME'";
                                break;
                            case "NON-CARD":
                                productsType += "'MANUAL'";
                                break;
                            case "COMBO":
                                productsType += "'COMBO'";
                                break;
                            case "ATTRACTIONS":
                                productsType += "'ATTRACTION'";
                                break;
                            case "CHECKINOUT":
                                productsType += "'CHECK-IN','CHECK-OUT'";
                                break;
                            case "RENTAL":
                                productsType += "'RENTAL','RENTAL_RETURN'";
                                break;
                            case "VOUCHERS":
                                productsType += "'VOUCHER'";
                                break;
                            case "BOOKINGS":
                                productsType += "'BOOKINGS'";
                                break;
                            case "MANUAL":
                                productsType += "'MANUAL'";
                                break;
                            default:
                                productsType += ("'" + temp.ToUpper() + "'");
                                //Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid product Type" });
                                break;
                        }
                    }

                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST, productsType));
                }

                if(isSubscriptionProduct)
                {
                    searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.IS_SUBSCRIPTION_PRODUCT, "1"));
                }

                if (string.IsNullOrEmpty(isSubscriptionORHasActiveSubscriptionChild) == false)
                {
                    if (isSubscriptionORHasActiveSubscriptionChild == "1" || isSubscriptionORHasActiveSubscriptionChild == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.IS_A_SUBSCRIPTION_PRODUCT_OR_HAS_ACTIVE_SUBSCRIPTION_CHILD, "1"));
                    }
                    else
                    {
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.IS_A_SUBSCRIPTION_PRODUCT_OR_HAS_ACTIVE_SUBSCRIPTION_CHILD, "0"));
                    }
                }

                totalNoOfProducts = await Task<int>.Factory.StartNew(() => { return productList.GetProductsDTOCount(searchParameters); });

                if (totalNoOfProducts > 0)
                {
                    List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> transactionSearchParameters = null;
                    if (buildForTransaction)
                    {
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId.ToString()));
                        searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, executionContext.GetUserId()));

                        UsersList usersList = new UsersList(null);
                        List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
                        if (usersDTOs == null || !usersDTOs.Any())
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "User not found" });

                        transactionSearchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        transactionSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, siteId.ToString()));
                        transactionSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TRX_ONLY_POS_MACHINE, string.IsNullOrEmpty(POSMachine) ? Environment.MachineName : POSMachine));
                        transactionSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TRX_ONLY_USER_ROLE, usersDTOs[0].RoleId.ToString()));

                        LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                        DateTime productSellDate = serverTimeObject.GetServerDateTime();
                        TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                        int offsetTimeSecs = timeZoneUtil.GetOffSetDuration(siteId, productSellDate);
                        transactionSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.OFFSET, offsetTimeSecs.ToString()));

                        if (purchaseDate != null)
                        {
                            productSellDate = Convert.ToDateTime(purchaseDate.ToString());
                            if (productSellDate == DateTime.MinValue)
                            {
                                string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                                log.Error(customException);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                            }
                            productSellDate = productSellDate.AddSeconds(offsetTimeSecs);
                        }

                        transactionSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TRX_ONLY_PRODUCT_PURCHASE_DATE, productSellDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

                        int membershipId = -1;
                        if(customerId != -1)
                        {
                            CustomerBL customer = new CustomerBL(executionContext, customerId);
                            if(customer != null && customer.CustomerDTO != null)
                            {
                                membershipId = customer.CustomerDTO.MembershipId;
                            }
                        }
                        transactionSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TRX_ONLY_MEMBERSHIP, membershipId.ToString()));
                        pageSize = 500;
                    }

                    log.LogVariableState("totalNoOfCustomer", totalNoOfProducts);
                    pageSize = pageSize > 500 || pageSize == 0 ? 500 : pageSize;
                    totalNoOfPages = (totalNoOfProducts / pageSize) + ((totalNoOfProducts % pageSize) > 0 ? 1 : 0);
                    currentPage = currentPage < -1 || currentPage > totalNoOfPages ? 0 : currentPage;

                    productsDTOList = productList.GetProductsList(searchParameters, currentPage, pageSize, buildChildRecords, activeChildRecords, buildForTransaction: buildForTransaction, transactionSearchParameters: transactionSearchParameters);
                }
                log.LogMethodExit(productsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsDTOList, currentPageNo = currentPage, totalCount = totalNoOfProducts });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON products List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Product/Products")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ProductsDTO> productDtoList)
        {
            try
            {
                log.LogMethodEntry(productDtoList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productDtoList != null && productDtoList.Count > 0)
                {
                    foreach (ProductsDTO productsDTO in productDtoList)
                    {
                        var sanitizer = new HtmlSanitizer();
                        var sanitizedHTML = sanitizer.Sanitize(productsDTO.WebDescription);
                        productsDTO.WebDescription = sanitizedHTML;
                    }
                    ProductsList products = new ProductsList(executionContext, productDtoList);
                    products.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = productDtoList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Delete Product Details Record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Product/Products")]
        [Authorize]
        public HttpResponseMessage Delete(List<ProductsDTO> productDtoList)
        {
            try
            {
                log.LogMethodEntry(productDtoList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productDtoList != null || productDtoList.Count > 0)
                {
                    ProductsList products = new ProductsList(executionContext, productDtoList);
                    products.DeleteProductsList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Products
        /// </summary>
        /// <param name="productsDTOList">productsDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Product/Products")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ProductsDTO> productsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                // Checks if the id is greater than to 0, If it is greater than to 0, then update 
                if (productsDTOList == null || productsDTOList.Any(a => a.ProductId < 0))
                {
                    log.LogMethodExit(productsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductsUseCases productsUseCases = ProductsUseCaseFactory.GetProductUseCases(executionContext);
                await productsUseCases.SaveProducts(productsDTOList);
                log.LogMethodExit(productsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsDTOList });
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
    }
}