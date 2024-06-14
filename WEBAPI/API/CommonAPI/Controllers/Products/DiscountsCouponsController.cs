/********************************************************************************************
 * Project Name - Discount Coupons Controller
 * Description  - Created to fetch, update and insert Display Coupons in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60        25-Jan-2019   Indrajeet Kumar          Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.60        05-Mar-2019   Akshay Gulaganji         Modified Get, Post and Delete methods
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Parafait.Discounts;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.Products
{
    public class DiscountsCouponsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Discount Coupons.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/DiscountsCoupons/")]
        public HttpResponseMessage Get(string activityType = "DISCOUNTCOUPONS", string isActive = "1", int discountId = -1)
        {  
            try
            {
                log.LogMethodEntry(isActive, discountId);                
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (activityType.ToUpper().ToString() == "DISCOUNTCOUPONS")
                {
                    List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                    searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID, Convert.ToString(discountId)));
                    bool loadActiveChildRecords = false;
                    if (isActive == "1")
                    {
                        loadActiveChildRecords = true;
                        searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                    DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(executionContext);
                    List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList = discountCouponsHeaderListBL.GetDiscountCouponsHeaderDTOList(searchParameters, true, loadActiveChildRecords);
                    log.LogMethodExit(discountCouponsHeaderDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = discountCouponsHeaderDTOList, token = securityTokenDTO.Token });
                }
                else if(activityType.ToUpper().ToString() == "PAYMENTMODECOUPONS" || activityType.ToUpper().ToString() == "PRODUCTDISCOUNTCOUPONS")
                {
                    DiscountServices discountServices = new DiscountServices(executionContext);
                    Sheet sheet = discountServices.BuildTemplete(activityType);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = sheet, token = securityTokenDTO.Token });
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Discount Coupons.
        /// </summary>
        /// <param name="discountCouponsList"></param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Products/DiscountsCoupons/")]
        [Authorize]
        public HttpResponseMessage Post(string activityType, [FromBody]JObject jObject, int discountId = -1, int discountHeaderId = -1, int paymentModeId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(jObject, discountId, discountHeaderId, paymentModeId);               
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                string discountCouponsHeaderDTOString = jObject.SelectToken("DiscountCouponsHeaderDTOList").ToString();
                string sheetObjString = jObject.SelectToken("SheetObj").ToString();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                if ((activityType.ToUpper().ToString() == "PRODUCTDISCOUNTCOUPONS" || activityType.ToUpper().ToString() == "PAYMENTMODECOUPONS") && !string.IsNullOrEmpty(sheetObjString))
                {
                    Sheet sheet = JsonConvert.DeserializeObject<Sheet>(sheetObjString, settings);
                    /// In sitesetup module => Paymentmode entity, the paymentModeId > 0
                    /// In Products module => Discounts Entity, the paymentModeId = -1
                    //If paymentModeId > 0 then DTODefination is for Payment coupon else DTODefination is for Discount Coupon
                    DiscountServices discountServices = new DiscountServices(executionContext);
                    var content = discountServices.BulkUpload(sheet, discountId, discountHeaderId, paymentModeId);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
                }
                else if (activityType.ToUpper().ToString() == "DISCOUNTCOUPONS" && !string.IsNullOrEmpty(discountCouponsHeaderDTOString))
                {
                    List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList = JsonConvert.DeserializeObject<List<DiscountCouponsHeaderDTO>>(discountCouponsHeaderDTOString, settings);
                    DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(executionContext, discountCouponsHeaderDTOList);
                    discountCouponsHeaderListBL.SaveDiscountCouponsHeaderList();                   
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Delete the JSON Discount Coupons.
        /// </summary>
        /// <param name="discountCouponsList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Products/DiscountsCoupons/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList)
        {            
            try
            {
                log.LogMethodEntry(discountCouponsHeaderDTOList);               
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (discountCouponsHeaderDTOList != null || discountCouponsHeaderDTOList.Count != 0)
                {
                    DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(executionContext, discountCouponsHeaderDTOList);
                    discountCouponsHeaderListBL.SaveDiscountCouponsHeaderList();
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
