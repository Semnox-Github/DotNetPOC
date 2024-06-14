/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Machines Input Devices
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        10-Sept-2018   Jagan          Created 
 *2.110.0     21-Dec-2020   Prajwal S            Modified as per New standards for REST API.  
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Game;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;
using System.Linq;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Games
{
    [Route("api/[controller]")]
    public class InputDevicesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Object Input Devices List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Game/InputDevices")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, int deviceId = -1, int deviceModelId = -1, int deviceTypeId = -1, string deviceName = null,
                                        string macAddress = null, string ipAddress = null, int machineId = -1, int currentPage = 0, int pageSize = 0)
        {

            log.LogMethodEntry(isActive, deviceId, deviceModelId, deviceTypeId, deviceName, macAddress, ipAddress, machineId);

            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>>();

                if (deviceId > 0)
                    searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.DEVICE_ID, deviceId.ToString()));

                if (deviceTypeId > 0)
                    searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.DEVICE_TYPE_ID, deviceTypeId.ToString()));

                if (deviceModelId > 0)
                    searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.DEVICE_MODEL_ID, deviceModelId.ToString()));

                if (machineId > 0)
                    searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.MACHINE_ID, machineId.ToString()));

                if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.IS_ACTIVE, "1"));

                if (!string.IsNullOrEmpty(deviceName))
                    searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.DEVICE_NAME, deviceName));

                if (!string.IsNullOrEmpty(ipAddress))
                    searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.IP_ADDRESS, ipAddress));

                if (!string.IsNullOrEmpty(macAddress))
                    searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.MAC_ADDRESS, macAddress));


                List<MachineInputDevicesDTO> machineInputDevicesDTOList = new List<MachineInputDevicesDTO>();
                IMachineInputDevicesUseCases machineInputDevicesUseCases = GameUseCaseFactory.GetMachineInputDevicesUseCases(executionContext);
                machineInputDevicesDTOList = await machineInputDevicesUseCases.GetMachineInputDevices(searchParameters, currentPage, pageSize, null);
                log.LogMethodExit(machineInputDevicesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = machineInputDevicesDTOList });

            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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


        /// <summary>
        /// Post the JSON Object Input Devices
        /// </summary>
        /// <param name="inputDevicesList">inputValues</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Game/InputDevices")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MachineInputDevicesDTO> machineInputDevicesDTOList)
        {

            log.LogMethodEntry(machineInputDevicesDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (machineInputDevicesDTOList != null && machineInputDevicesDTOList.Any(a => a.DeviceId > 0))
                {
                    log.LogMethodExit(machineInputDevicesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IMachineInputDevicesUseCases machineInputDevicesUseCases = GameUseCaseFactory.GetMachineInputDevicesUseCases(executionContext);
                machineInputDevicesUseCases.SaveMachineInputDevices(machineInputDevicesDTOList);
                log.LogMethodExit(machineInputDevicesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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