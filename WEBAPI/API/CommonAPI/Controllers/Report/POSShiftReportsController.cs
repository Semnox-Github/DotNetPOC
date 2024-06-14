/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for POSShiftView
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.90        26-May-2020       Vikas Dwivedi             Created to Get Methods.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Reports
{
    public class POSShiftReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of ShiftDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/POSShiftReports")]
        public HttpResponseMessage Get(DateTime? fromDate = null, DateTime? toDate = null, int shiftKey = -1, string posMachine = null, string shiftLoginId = null,
            string shiftUsername = null, string shiftUsertype = null, string shiftAction = null, bool printReceipt = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(fromDate, toDate, shiftKey, posMachine, shiftLoginId, shiftUsername, shiftUsertype, shiftAction);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> shiftSearchParameter = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (fromDate != null)
                {
                    DateTime shiftFromDate = Convert.ToDateTime(fromDate);
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_FROM_TIME, shiftFromDate.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                }
                if (toDate != null)
                {
                    DateTime shiftToDate = Convert.ToDateTime(toDate);
                    shiftSearchParameter.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_TO_TIME, shiftToDate.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
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
                ShiftListBL shiftListBL = new ShiftListBL(executionContext);
                List<ShiftDTO> shiftDTOList = shiftListBL.GetShiftDTOList(shiftSearchParameter, true, printReceipt, null);
                log.LogMethodExit(shiftDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = shiftDTOList });
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
