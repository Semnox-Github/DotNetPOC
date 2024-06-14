/********************************************************************************************
 * Project Name - Communications
 * Description  - Controller for Push Notification Devices .
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.100.0     15-Sep-2020     Nitin Pai         Push Notification: Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class PushNotificationDeviceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Post operation on PushNotificationDevice details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Communication/PushNotificationDevices")]
        public HttpResponseMessage Post([FromBody]List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(pushNotificationDeviceDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (pushNotificationDeviceDTOList != null && pushNotificationDeviceDTOList.Any())
                {
                    PushNotificationDeviceListBL pushNotificationDeviceListBL = new PushNotificationDeviceListBL(executionContext, pushNotificationDeviceDTOList);
                    pushNotificationDeviceListBL.SavePushNotificationDeviceList();
                    log.LogMethodExit(pushNotificationDeviceDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = pushNotificationDeviceDTOList });
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

        /// <summary>
        /// Performs a Get operation on PushNotificationDevice details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Communication/PushNotificationDevices")]
        public HttpResponseMessage Get(int pushNotificationDeviceId = -1, int customerId = -1, string pushNotificationToken = null, string deviceType = null, bool? isActive = null,
                                        bool? customerSignedIn = null, string guid = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(pushNotificationDeviceId, customerId, pushNotificationToken, deviceType, customerSignedIn, guid);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>>();
                if (pushNotificationDeviceId != -1)
                {
                    searchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.ID, pushNotificationDeviceId.ToString()));
                }

                if (customerId != -1)
                {
                    searchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }

                if (!String.IsNullOrEmpty(pushNotificationToken))
                {
                    searchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.PUSH_NOTIFICATION_TOKEN, pushNotificationToken.ToString()));
                }

                if (!String.IsNullOrEmpty(deviceType))
                {
                    searchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.DEVICE_TYPE, deviceType.ToString()));
                }

                if (!String.IsNullOrEmpty(guid))
                {
                    searchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.GUID, guid));
                }

                if (isActive != null)
                {
                    searchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                }

                if (customerSignedIn != null)
                {
                    searchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_SIGNED_IN, customerSignedIn.ToString()));
                }

                PushNotificationDeviceListBL pushNotificationDeviceListBL = new PushNotificationDeviceListBL(executionContext);
                List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList = pushNotificationDeviceListBL.GetPushNotificationDeviceDTOList(searchParameters);
                log.LogMethodExit(pushNotificationDeviceDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = pushNotificationDeviceDTOList });
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
