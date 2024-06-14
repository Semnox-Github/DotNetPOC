/********************************************************************************************
* Project Name - CustomerContent Controller
* Description  - Customer Content for WaiverSet
* 
**************
**Version Log
**************
*Version     Date             Modified By       Remarks          
*********************************************************************************************
*0.00        06-Nov-2019      Indrajeet Kumar   Created
* 2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.Customer.Waiver
{
    public class CustomerContentWaiversController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Below Post method used to create Customer Content for Waiver, It will return the List of WaiverCustomerAndSignatureDTO.
        /// </summary>
        /// <param name="waiverCustomerAndSignatureDTOList"></param>
        /// <returns>waiverCustomerAndSignatureDTOCustContent</returns>
        [HttpPost]
        [Route("api/Customer/Waiver/CustomerContentWaivers")]
        [Authorize]
        public HttpResponseMessage Post(List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOList)
        {
            try
            {
                log.LogMethodEntry(waiverCustomerAndSignatureDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOCustomerList = new List<WaiverCustomerAndSignatureDTO>();

                foreach (WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO in waiverCustomerAndSignatureDTOList)
                {
                    WaiverCustomerAndSignatureDTO waiverCustomerAndSignature = WaiverCustomerAndSignatureBL.CreateCustomerContentForWaiver(executionContext, waiverCustomerAndSignatureDTO);
                    waiverCustomerAndSignatureDTOCustomerList.Add(waiverCustomerAndSignature);
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = waiverCustomerAndSignatureDTOCustomerList});
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
