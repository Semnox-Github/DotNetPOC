﻿/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the Products controller.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 2.110.0       14-Dec-2020   Deeksha              Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.ProductPrice;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Products
{
    public class ProductPriceContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductPriceContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, int userRoleId = -1, int posMachineId = -1, int languageId = -1, string menuType= null, int membershipId = -1, int transactionProfileId = -1, DateTime? startDateTime = null, DateTime? endDateTime = null, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, startDateTime, endDateTime, hash, rebuildCache);
                if (ProductMenuType.IsValid(menuType) == false ||
                    userRoleId == -1 ||
                    posMachineId == -1 ||
                    startDateTime.HasValue == false ||
                    endDateTime.HasValue == false ||
                   startDateTime.Value >= endDateTime.Value)
                {
                    log.LogMethodExit(null, "Bad Request");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                ProductPriceContainerSnapshotDTOCollection productPriceContainerSnapshotDTOCollection = await
                           Task<ProductPriceContainerSnapshotDTOCollection>.Factory.StartNew(() =>
                           {
                               if (rebuildCache)
                               {
                                   ProductPriceViewContainerList.Rebuild(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, new DateTimeRange(startDateTime.Value, endDateTime.Value));
                               }
                               return ProductPriceViewContainerList.GetProductPriceContainerSnapshotDTOCollection(siteId, userRoleId, posMachineId, languageId,menuType, membershipId, transactionProfileId, startDateTime.Value, endDateTime.Value, hash);
                           });
                log.LogMethodExit(productPriceContainerSnapshotDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productPriceContainerSnapshotDTOCollection });
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
