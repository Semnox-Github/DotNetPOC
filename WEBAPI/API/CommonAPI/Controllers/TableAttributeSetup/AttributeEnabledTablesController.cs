/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the AttributeEnabledTables.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.140.0    06-Sep-2021     Fiona               Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.TableAttributeSetup;


namespace Semnox.CommonAPI.Controllers.TableAttributeSetup
{
    public class AttributeEnabledTablesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get Payment Gateways permitted for the respective device.
        /// </summary>
        [HttpGet]
        [Route("api/TableAttributeSetUp/AttributeEnabledTables")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int attributesEnabledTableId = -1, string isActive = null, bool loadChildRecords = false, bool loadActiveChild = false)
        {
          
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry( attributesEnabledTableId, isActive);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                
                List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>> searchAttributeEnabledTablesParameters = new List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>>();
                searchAttributeEnabledTablesParameters.Add(new KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>(AttributeEnabledTablesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchAttributeEnabledTablesParameters.Add(new KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>(AttributeEnabledTablesDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }

                if (attributesEnabledTableId > -1)
                {
                    searchAttributeEnabledTablesParameters.Add(new KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>(AttributeEnabledTablesDTO.SearchByParameters.ATTRIBUTE_ENABLED_TABLE_ID, attributesEnabledTableId.ToString()));
                }
                IAttributeEnabledTablesUseCases attributeEnabledTablesUseCases = AttributeEnabledTablesUseCaseFactory.GetAttributeEnabledTablesUseCases(executionContext);
                List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList = await attributeEnabledTablesUseCases.GetAttributeEnabledTables(searchAttributeEnabledTablesParameters, loadChildRecords, loadActiveChild);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = attributeEnabledTablesDTOList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Performs a Post operation on payment modes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/TableAttributeSetUp/AttributeEnabledTables")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList)
        {
            
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attributeEnabledTablesDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (attributeEnabledTablesDTOList != null && attributeEnabledTablesDTOList.Count > 0)
                {
                    IAttributeEnabledTablesUseCases attributeEnabledTablessUseCases = AttributeEnabledTablesUseCaseFactory.GetAttributeEnabledTablesUseCases(executionContext);
                    var content= await attributeEnabledTablessUseCases.SaveAttributeEnabledTables(attributeEnabledTablesDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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