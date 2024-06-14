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

namespace Semnox.CommonAPI.Controllers.TableAttributeSetup
{
    public class TableAttributeDetailsSQLDataController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/TableAttributeSetup/GetSQLDataList")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string sqlSource, string sqlDisplayMember, string sqlValueMember)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(sqlSource, sqlValueMember);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                
                ITableAttributeDetailsUseCases tableAttributeDetailsUseCases = TableAttributeDetailsUseCaseFactory.GetTableAttributeDetailsUseCases(executionContext);
                List<KeyValuePair<string, string>> result = await tableAttributeDetailsUseCases.GetSQLDataList(sqlSource, sqlDisplayMember, sqlValueMember);
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