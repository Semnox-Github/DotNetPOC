/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Reader configuration
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        07-Aug-2018   Jagan          Created 
 *2.80        07-Jul-2020   Girish Kundar  Modified : REST API changes/ Performance Fix 
*2.100       27-Oct-2020    Girish Kundar        Modified : Implemented factory class to get/save the data
 ********************************************************************************************/


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
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.CommonAPI.Games
{


    public class ReaderConfigurationsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Reader Configaration Details
        /// <paramref name="moduleName"/>
        /// <paramref name="moduleRowId"/>
        /// </summary>   
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Game/ReaderConfigs")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string moduleName, string moduleRowId)
        {
            log.LogMethodEntry(moduleName, moduleRowId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<string, string>> searchParameter = new List<KeyValuePair<string, string>>();
                searchParameter.Add(new KeyValuePair<string, string>("moduleName", moduleName));
                searchParameter.Add(new KeyValuePair<string, string>("moduleRowId", moduleRowId));
                IReaderConfigurationUseCases readerConfigurationDataService = GameUseCaseFactory.GetReaderConfigurationUseCases(executionContext);
                var result = await readerConfigurationDataService.GetMachineAttributes(searchParameter);
                List<MachineAttributeDTO> machineAttributes = new List<MachineAttributeDTO>();
                Utilities utilities = new Utilities();
                string configHardWareVersion = utilities.getParafaitDefaults("READER_HARDWARE_VERSION");
                log.LogMethodExit(result, configHardWareVersion);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result, hardWareVersion = configHardWareVersion });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Reader Configaration Details
        /// </summary>
        /// <param name="List<MachineAttributeDTO>">readerConfigValues</param>
        /// <param name="moduleName">moduleName</param>
        /// <param name="moduleId">moduleId</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Game/ReaderConfigs")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<MachineAttributeDTO> readerConfigValues, string moduleName, string moduleId)
        {
            log.LogMethodEntry(readerConfigValues, moduleName, moduleId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (readerConfigValues != null && readerConfigValues.Any())
                {
                    IReaderConfigurationUseCases readerConfigurationDataService = GameUseCaseFactory.GetReaderConfigurationUseCases(executionContext);
                    var result = await readerConfigurationDataService.SaveMachineAttributes(readerConfigValues, moduleName, moduleId);
                    log.LogMethodExit(result);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "NotFound" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Delete the Reader Configuration record 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Game/ReaderConfigs")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete(MachineAttributeDTO machineAttributeDTO, string entityName, string entityId)
        {
            log.LogMethodEntry(machineAttributeDTO, entityName, entityId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (machineAttributeDTO != null && entityName != null)
                {
                    IReaderConfigurationUseCases readerConfigurationDataService = GameUseCaseFactory.GetReaderConfigurationUseCases(executionContext);
                    var result = await readerConfigurationDataService.DeleteMachineAttributes(machineAttributeDTO, entityName, entityId);
                    log.LogMethodExit(result);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "NotFound" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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