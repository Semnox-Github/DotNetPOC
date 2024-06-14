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
 *2.90      08-May-2020     Girish Kundar             Modified : Moved to Discount folder and REST API standard changes
 *2.90      16-Jun-2020     Mushahid Faizan          Modified : End Point changes, Removed Delete Method and Moved Sheet object logic to SheetUploadController.
 *2.120.0   12-May-2021     Mushahid Faizan          Modified : Added expiryDate search Parameter
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
namespace Semnox.CommonAPI.Controllers.Discount
{
    public class DiscountsCouponsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Discount Coupons.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Discount/DiscountCoupons")]
        public HttpResponseMessage Get(string couponType = null, string isActive = null, int discountId = -1, DateTime? expiryDate=null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(couponType, isActive, discountId, expiryDate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                Sheet sheet = new Sheet();
                List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (discountId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID, Convert.ToString(discountId)));
                }
                bool loadActiveChildRecords = false;
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive == "1" || isActive == "Y")
                    {
                        loadActiveChildRecords = true;
                        searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (expiryDate != null)
                {
                    DateTime couponExpiryDate = Convert.ToDateTime(expiryDate);
                    searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.COUPON_EXPIRY_DATE, couponExpiryDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(executionContext);
                List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList = discountCouponsHeaderListBL.GetDiscountCouponsHeaderDTOList(searchParameters, true, loadActiveChildRecords);

                log.LogMethodExit(discountCouponsHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = discountCouponsHeaderDTOList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    data = customException
                });
            }
        }



        /// <summary>
        /// Post the JSON Discount Coupons.
        /// </summary>
        /// <param name="discountCouponsList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Discount/DiscountCoupons")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(discountCouponsHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (discountCouponsHeaderDTOList != null && discountCouponsHeaderDTOList.Any())
                {
                    DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(executionContext, discountCouponsHeaderDTOList);
                    discountCouponsHeaderListBL.SaveDiscountCouponsHeaderList();
                    log.LogMethodExit(discountCouponsHeaderDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = discountCouponsHeaderDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
