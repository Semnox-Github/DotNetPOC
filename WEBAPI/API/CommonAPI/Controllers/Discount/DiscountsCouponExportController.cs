/******************************************************************************************************
 * Project Name - Discounts
 * Description  - Created to fetch for DiscountsCouponsHeader Setup Entity.
 *  
 **************
 **Version Log
 **************
 *Version   Date            Modified By               Remarks          
 ******************************************************************************************************
 *2.150.1  14-Mar-2023    Roshan Devadiga              Created
 *******************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;

namespace Semnox.CommonAPI.Controllers.Discount
{
    public class DiscountsCouponExportController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// Get-DiscountsCouponExportController
        /// </summary>
        /// <param name="discountIdList"></param>
        /// <param name="transactionId"></param>
        /// <param name="couponNumber"></param>
        /// <param name="isActive"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Discount/DiscountCouponsExport")]
        public async Task<HttpResponseMessage> Get(string discountIdList=null, int transactionId = -1, string couponNumber = null, string isActive = null)                                        
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(discountIdList, transactionId, couponNumber, isActive);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                Sheet sheet = new Sheet();
                DiscountCouponsExcelDTODefinition discountCouponsExcelDTODefinition = null;
                Row headerRow = new Row();
                discountCouponsExcelDTODefinition = new DiscountCouponsExcelDTODefinition(executionContext, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"), true);
                discountCouponsExcelDTODefinition.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);
               
                DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(executionContext);

                List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrWhiteSpace(discountIdList))
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.DISCOUNT_ID_LIST, discountIdList.ToString()));
                }
                if (transactionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(couponNumber))
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.COUPON_NUMBER, couponNumber.ToString()));
                }
                if (!String.IsNullOrEmpty(isActive) && (isActive.Equals("1", StringComparison.InvariantCultureIgnoreCase)
                                                        || isActive.Equals("Y", StringComparison.InvariantCultureIgnoreCase)
                                                        || isActive.Equals("True", StringComparison.InvariantCultureIgnoreCase)))
                {
                    isActive = "Y";
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                }
                DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler();
                List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsDataHandler.GetDiscountCouponsDTOList(searchParameters);
                if (discountCouponsDTOList != null && discountCouponsDTOList.Any())
                {
                    foreach (DiscountCouponsDTO discountCouponsDTO in discountCouponsDTOList)
                    {
                        discountCouponsExcelDTODefinition.Configure(discountCouponsDTO);

                        Row row = new Row();
                        discountCouponsExcelDTODefinition.Serialize(row, discountCouponsDTO);
                        sheet.AddRow(row);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = sheet });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
