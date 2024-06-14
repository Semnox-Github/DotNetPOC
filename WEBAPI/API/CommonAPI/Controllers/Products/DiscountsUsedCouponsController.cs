/******************************************************************************************************
 * Project Name - Discounts Setup Used Coupons Controller
 * Description  - Created to fetch, update and inserts for Discounts Setup Used Coupons Entity.
 *  
 **************
 **Version Log
 **************
 *Version   Date            Modified By               Remarks          
 ******************************************************************************************************
 *2.60      9-Feb-2019     Indrajeet kumar          Created to Get Method. 
 ******************************************************************************************************
 *2.60      18-Mar-2019     Akshay Gulaganji        Added Author details, added ExecutionContext and modified isActive (string to bool)
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
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.CommonAPI.Products
{
    public class DiscountsUsedCouponsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Used Coupons.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/DiscountsUsedCoupons/")]
        public HttpResponseMessage Get(string activityType = "DISCOUNTSUSEDCOUPONS", string isActive = "1", int couponsId = -1)
        {
            try
            {
                log.LogMethodEntry(isActive, couponsId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (activityType.ToUpper().ToString() == "DISCOUNTSUSEDCOUPONS")
                {
                    List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                    if (couponsId > 0)
                    {
                        searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.COUPON_SET_ID, couponsId.ToString()));
                    }
                    if (isActive == "1")
                    {
                        searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                    DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(executionContext);
                    List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchParameters);
                    log.LogMethodExit(discountCouponsUsedDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = discountCouponsUsedDTOList, token = securityTokenDTO.Token });
                }
                else if (activityType.ToUpper().ToString() == "IMPORTDISCOUNTSUSEDCOUPONS")
                {
                    DiscountServices discountServices = new DiscountServices(executionContext);
                    Sheet sheet = discountServices.DiscountUsedCouponBuildTemplete();
                    log.LogMethodExit(sheet);
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
        /// Post the sheet object as Json 
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Products/DiscountsUsedCoupons/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]Sheet sheet)
        {
            try
            {
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (sheet.RowList != null || sheet.RowList.Count > 0)
                {
                    DiscountServices discountServices = new DiscountServices(executionContext);
                    var content = discountServices.BulkUploadUsedCoupons(sheet);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
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
