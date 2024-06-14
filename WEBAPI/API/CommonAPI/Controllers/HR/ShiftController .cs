/**************************************************************************************************
 * Project Name - HR 
 * Description  - Controller for POS Shift
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *1.0         27-Jul-2020       Girish Kundar              Created
 *2.120.0     01-Apr-2021      Prajwal S                  Modified.
 *2.140.0     14-Sep-2021      Deeksha                    Modified- Provisional shift changes.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.HR
{
    public class ShiftController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of ShiftDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/Shifts")]
        public async Task<HttpResponseMessage> Get(DateTime? fromDate = null, DateTime? toDate = null, int shiftKey = -1, string posMachine = null, string shiftLoginId = null,
            string shiftUsername = null, string shiftUsertype = null, string shiftAction = null, bool printReceipt = false, bool loadChildRecords = false, bool buildReceipt = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(fromDate, toDate, shiftKey, posMachine, shiftLoginId, shiftUsername, shiftUsertype, shiftAction);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> shiftSearchParameter = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (fromDate != null)
                {
                    DateTime shiftFromDate = Convert.ToDateTime(fromDate);
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_FROM_TIME, shiftFromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (toDate != null)
                {
                    DateTime shiftToDate = Convert.ToDateTime(toDate);
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_TO_TIME, shiftToDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (fromDate == null || toDate == null)
                {
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_FROM_TIME, DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_TO_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                }
                if (shiftKey > -1)
                {
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_KEY, shiftKey.ToString()));
                }
                if (!string.IsNullOrEmpty(posMachine))
                {
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, posMachine.ToString()));
                }
                if (!string.IsNullOrEmpty(shiftLoginId))
                {
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_LOGIN_ID, shiftLoginId.ToString()));
                }
                if (!string.IsNullOrEmpty(shiftUsername))
                {
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_USERNAME, shiftUsername.ToString()));
                }
                if (!string.IsNullOrEmpty(shiftUsertype))
                {
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_USERTYPE, shiftUsertype.ToString()));
                }
                if (!string.IsNullOrEmpty(shiftAction))
                {
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION, shiftAction.ToString()));
                }
                IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(executionContext);
                List<ShiftDTO> shiftDTOList = await shiftUseCases.GetShift(shiftSearchParameter, loadChildRecords, buildReceipt);
                if (shiftDTOList != null && buildReceipt)
                {
                    Utilities utilities = new Utilities();
                    foreach (ShiftDTO shiftDTO in shiftDTOList)
                    {
                        PrinterBL printerBL = new PrinterBL(executionContext);
                        shiftDTO.Receipt = printerBL.PrintShiftReceipt(shiftDTO.ShiftKey, shiftDTO.POSMachine, shiftDTO.ShiftUserName, utilities, shiftDTO.ShiftTime, null);
                    }
                }
                log.LogMethodExit(shiftDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = shiftDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object of ShiftDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/Shifts")]
        public async Task<HttpResponseMessage> Post([FromBody] List<ShiftDTO> shiftDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(shiftDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (shiftDTOList == null || shiftDTOList.Any(a => a.ShiftKey > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (shiftDTOList != null && shiftDTOList.Any())
                {
                    IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(executionContext);
                    List<ShiftDTO> shiftDTOLists = await shiftUseCases.SaveShift(shiftDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = shiftDTOLists });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }


        /// <summary>
        /// Post the ShiftList collection
        /// <param name="shiftDTOList">ShiftList</param>
        [HttpPut]
        [Route("api/HR/Shifts")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ShiftDTO> shiftDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(shiftDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (shiftDTOList == null || shiftDTOList.Any(a => a.ShiftKey < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(executionContext);
                shiftDTOList = await shiftUseCases.SaveShift(shiftDTOList);
                log.LogMethodExit(shiftDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = shiftDTOList });
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
