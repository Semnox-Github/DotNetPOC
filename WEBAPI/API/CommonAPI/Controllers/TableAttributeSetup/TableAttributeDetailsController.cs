/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the TableAttributeDetailsSQLDataController.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.140.0    14-Sep-2021     Fiona               Created 
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
using Semnox.Parafait.TableAttributeDetailsUtils;
using Semnox.Parafait.TableAttributeSetup;

namespace Semnox.CommonAPI.Controllers.TableAttributeSetup
{
    public class TableAttributeDetailsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/TableAttributeSetup/TableAtrributeDetails")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(EnabledAttributesDTO.TableWithEnabledAttributes enabledTableName, string recordGuid)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(enabledTableName, recordGuid);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                ITableAttributeDetailsUseCases tableAttributeDetailsUseCases = TableAttributeDetailsUseCaseFactory.GetTableAttributeDetailsUseCases(executionContext);
                List<TableAttributeDetailsDTO> result = await tableAttributeDetailsUseCases.GetTableAttributeDetailsDTOList(enabledTableName, recordGuid);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });

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