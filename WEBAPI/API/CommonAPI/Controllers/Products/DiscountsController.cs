/******************************************************************************************************
 * Project Name - Products
 * Description  - Created to fetch, update and inserts for Discounts Setup Entity.
 *  
 **************
 **Version Log
 **************
 *Version   Date            Modified By               Remarks          
 ******************************************************************************************************
 *2.60      25-Jan-2019     Akshay Gulaganji          Created to Get, POST and Delete Methods.
 ******************************************************************************************************
 *2.60      08-Mar-2019     Akshay Gulaganji          Added loadChildRecords and activeChildRecords
 *******************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Parafait.Discounts;
using Semnox.Core.GenericUtilities;
using System.Linq;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Products
{
    public class DiscountsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string discount_Type = DiscountsBL.DISCOUNT_TYPE_TRANSACTION; //By Default Discount Type will be Transaction

        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Discounts Setup.
        /// </summary>
        /// <param name="discountType">discountType i.e., T for Transaction G for Game Play and L for Loyalty</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/Discounts/")]
        public async Task<HttpResponseMessage> Get(string isActive, string discountType = null, string discountId = null, string discountName = null, int currentPage = 0, int pageSize = 5)
        {
            try
            {
                log.LogMethodEntry(isActive, discountType);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                int totalNoOfPages = 0;
                bool activeChildRecords = false;
                List<KeyValuePair<DiscountsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountsDTO.SearchByParameters, string>>();
                if (!string.IsNullOrEmpty(discountType) && discountType.ToUpper().ToString() == "T")
                {
                    discount_Type = DiscountsBL.DISCOUNT_TYPE_TRANSACTION;
                }
                else if (!string.IsNullOrEmpty(discountType) && discountType.ToUpper().ToString() == "G")
                {
                    discount_Type = DiscountsBL.DISCOUNT_TYPE_GAMEPLAY;
                }
                else if(!string.IsNullOrEmpty(discountType) && discountType.ToUpper().ToString() == "L")
                {
                    discount_Type = DiscountsBL.DISCOUNT_TYPE_LOYALTY;
                }
                if (isActive == "1")
                {
                    activeChildRecords = true;
                }
                if(!string.IsNullOrEmpty(discountName))
                {
                    searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_NAME, discountName));
                }
                
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, discount_Type));
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                DiscountsListBL discountsListBL = new DiscountsListBL(executionContext);
                List<DiscountsDTO> discountsDTOList = await Task<List<DiscountsDTO>>.Factory.StartNew(() => { return discountsListBL.GetDiscountedDTOsList(searchParameters, discount_Type, discountId, true, activeChildRecords); });
                if (discountsDTOList != null && discountsDTOList.Count != 0 && string.IsNullOrEmpty(discountId))
                {
                    totalNoOfPages = discountsDTOList.Count();
                    discountsDTOList = discountsDTOList.Skip(pageSize * currentPage).Take(pageSize).ToList();
                }
                log.LogMethodExit(discountsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = discountsDTOList, currentPageNo = currentPage, totalCount = totalNoOfPages, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Object Discounts Setup
        /// </summary>
        /// <param name="discountDTOList">discountDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Products/Discounts/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<DiscountsDTO> discountDTOList)
        {
            try
            {
                log.LogMethodEntry(discountDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (discountDTOList != null || discountDTOList.Count != 0)
                {
                    DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, discountDTOList);
                    discountsListBL.SaveUpdateDiscountsList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Deletes the JSON Object Discounts Setup
        /// </summary>
        /// <param name="discountDTOList">discountDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Products/Discounts/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<DiscountsDTO> discountDTOList)
        {
            try
            {
                log.LogMethodEntry(discountDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (discountDTOList != null || discountDTOList.Count != 0)
                {
                    DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, discountDTOList);
                    discountsListBL.SaveUpdateDiscountsList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
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