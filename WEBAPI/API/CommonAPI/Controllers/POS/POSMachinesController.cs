/********************************************************************************************
 * Project Name - POSMachines Controller
 * Description  - Created to fetch, update and insert product exclusion pos management machines in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.60        23-Jan-2019   Mushahid Faizan     Created to get, insert, update and Delete Methods.
 *2.60        20-Mar-2019   Akshay Gulaganji    Added executionContext, customGenericException 
 *2.60        05-May-2019   Nitin Pai           Customer App related updates
 *2.70        16-Jul-2019   Akshay Gulaganji   modified Delete() method
 * 2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Parafait.POS;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.POS
{
    public class POSMachinesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON POS Management Machines.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/POS/POSMachines")]
        public HttpResponseMessage Get(string isActive = null, int siteId = -1, string machineName = null, bool buildChildRecords = false ,
                                       int posMachineId =-1 , int posTypeId = -1,bool loadActiveChildRecords =true)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, isActive, machineName, buildChildRecords, posMachineId, posTypeId, loadActiveChildRecords);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                int selectedSite = siteId == -1 ? securityTokenDTO.SiteId : siteId;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, selectedSite, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, Convert.ToString(selectedSite)));
                if (!String.IsNullOrEmpty(machineName))
                {
                        searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME, machineName));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, isActive));
                    }
                }
                if (posMachineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID, posMachineId.ToString()));
                }
                if (posTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_TYPE_ID, posTypeId.ToString()));
                }

                POSMachineList pOSMachineList = new POSMachineList(executionContext);
                var content = pOSMachineList.GetAllPOSMachines(searchParameters, buildChildRecords, loadActiveChildRecords);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content});
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException});
            }
        }
        /// <summary>
        /// Post the JSON Object POS Management Machines.
        /// </summary>
        /// <param name="posMachinesList">POSMachineList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/POS/POSMachines")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<POSMachineDTO> posMachinesList, bool isLicensedPOSMachines = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(posMachinesList, isLicensedPOSMachines);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (posMachinesList != null && posMachinesList.Count != 0)
                {
                    POSMachineList pOSMachineList = new POSMachineList(executionContext, posMachinesList);
                    pOSMachineList.Save(isLicensedPOSMachines);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""});
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""});
                }
            }

            catch (POSMachinesLicenseException posex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(posex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Accepted, new { data = customException });

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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException});
            }
        }

    }
}