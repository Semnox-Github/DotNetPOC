/**************************************************************************************************
 * Project Name - HR 
 * Description  - Controller for AssignCashdrawer for the shift
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.140.0     27-Aug-2021       Girish Kundar              Created
 **************************************************************************************************/


using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.HR
{
    public class AssignCashdrawerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object of ShiftDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/Shift/{shiftId}/UnAssign")]
        public async Task<HttpResponseMessage> Post([FromUri] int shiftId, [FromBody] CashdrawerActivityDTO cashdrawerActivityDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(cashdrawerActivityDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (cashdrawerActivityDTO == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(executionContext);
                ShiftDTO shiftDTO = await shiftUseCases.AssignCashdrawer(shiftId, cashdrawerActivityDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = shiftDTO });

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
