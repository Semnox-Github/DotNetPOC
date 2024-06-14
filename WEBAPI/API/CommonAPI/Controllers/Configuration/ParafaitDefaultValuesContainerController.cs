/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the Parafait Configuration Values.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.100         11-May-2020   Girish Kundar        Created : POS UI redesign
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Configuration
{
    public class ParafaitDefaultValuesContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/ParafaitDefaultContainer/Values")]
        public HttpResponseMessage Get(int siteId = -1, 
                                       int userPkId = -1, 
                                       int machineId = -1, 
                                       bool rebuildCache = false,
                                       string defaultValueNames = "")
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, userPkId, machineId, rebuildCache, defaultValueNames);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if(rebuildCache)
                {
                    ParafaitDefaultViewContainerList.Rebuild(siteId, userPkId, machineId);
                }
                List<ParafaitDefaultContainerDTO> parafaitDefaultContainerDTOList = new List<ParafaitDefaultContainerDTO>();
                foreach (var defaultValueName in defaultValueNames.Split(new char[]{ ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    parafaitDefaultContainerDTOList.Add(new ParafaitDefaultContainerDTO(defaultValueName, ParafaitDefaultViewContainerList.GetParafaitDefault(siteId, userPkId, machineId, defaultValueName)));
                }
                ParafaitDefaultContainerDTOCollection result = new ParafaitDefaultContainerDTOCollection(parafaitDefaultContainerDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
