/**************************************************************************************************
 * Project Name - KDS 
 * Description  - Controller for Kitchen delivery System
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *1.0        27-Jul-2020       Girish Kundar              Created
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Transaction
{
    public class KDSController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of WOrkWorkShiftDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/KDSOrders")]
        public HttpResponseMessage Get( int displayBatchId = -1, int posMachineId =-1, int terminalId =-1,int transactionId  = -1, DateTime? orderedTime = null,
                                        int displayTemplateId = -1,string tableNumber =null, bool loadChildRecords = false,
                                         DateTime? deliveredTime = null, DateTime? preparedTime = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(displayBatchId, posMachineId, terminalId, transactionId, displayTemplateId, 
                    tableNumber, preparedTime);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<KDSOrderDTO.SearchByParameters, string>> shiftSearchParameter = new List<KeyValuePair<KDSOrderDTO.SearchByParameters, string>>();
                shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (deliveredTime != null)
                {
                    DateTime deliveredTimeTemp = Convert.ToDateTime(deliveredTime);
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.DELIVERED_TIME_NOT_NULL, deliveredTimeTemp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (orderedTime != null)
                {
                    DateTime orderedTimeTemp = Convert.ToDateTime(orderedTime);
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.ORDERED_TIME_EQUAL_TO, orderedTimeTemp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (preparedTime != null)
                {
                    DateTime preparedTimeTemp = Convert.ToDateTime(preparedTime);
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.PREPARED_TIME_GREATER_THAN, preparedTimeTemp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
               
                if (displayBatchId > -1)
                {
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.DISPLAY_BATCH_ID, displayBatchId.ToString()));
                }
                if (posMachineId > -1)
                {
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));
                }
                if (transactionId > -1)
                {
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                }
                if (terminalId > -1)
                {
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.TERMINAL_ID, terminalId.ToString()));
                }
                if (displayTemplateId > -1)
                {
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.DISPLAY_TEMPLATE_ID, displayTemplateId.ToString()));
                }
                if (!string.IsNullOrEmpty(tableNumber))
                {
                    shiftSearchParameter.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.TABLE_NUMBER, tableNumber.ToString()));
                }
                KDSOrderListBL kdsOrderListBL = new KDSOrderListBL(executionContext);
                List<KDSOrderDTO> kdsOrderDTOList = kdsOrderListBL.GetKDSOrderDTOList(shiftSearchParameter, loadChildRecords, null);
                log.LogMethodExit(kdsOrderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = kdsOrderDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of KDSOrderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Transaction/KDSOrders")]
        public HttpResponseMessage Post([FromBody] List<KDSOrderDTO> kdsOrderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(kdsOrderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (kdsOrderDTOList != null && kdsOrderDTOList.Any())
                {
                    KDSOrderListBL kdsOrderListBL = new KDSOrderListBL(executionContext, kdsOrderDTOList);
                    kdsOrderListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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
