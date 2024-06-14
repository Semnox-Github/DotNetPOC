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
 *2.90      19-Jun-2020     Mushahid Faizan          Modified : Moved to Discount Folder, End Point changes, Removed Post Method and Moved Sheet object logic to SheetUploadController.
 *******************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;

namespace Semnox.CommonAPI.Controllers.Discount
{
    public class DiscountsUsedCouponsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Used Coupons.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Discount/DiscountUsedCoupons")]
        public HttpResponseMessage Get(string isActive = null, int couponsId = -1,string couponNumber = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, couponsId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (couponsId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.COUPON_SET_ID, couponsId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(couponNumber) == false)
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.COUPON_NUMBER, couponNumber));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(executionContext);
                List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchParameters);
                log.LogMethodExit(discountCouponsUsedDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = discountCouponsUsedDTOList });

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
