/********************************************************************************************
 * Project Name - AddressTypeController
 * Description  - Controller for returning different address type
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.80       15-Oct-2019      Nitin Pai      Initial Version 
 * 2.80       05-Apr-2020      Girish Kundar  Modified: API end point and removed token from the body 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class AddressTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object Collections of customer ui meta data.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/AddressTypes")]
        public HttpResponseMessage Get()
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                // the UI metadata is always set up at HQ level. If no HQ is set up, take the site id from execution context
                // commenting as the customer save is being done to the site and the ui metadata can be different there
                int siteId = securityTokenDTO.SiteId;
                //SiteList siteList = new SiteList();
                //SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                //if (HQSite != null && HQSite.SiteId != -1)
                //{
                //    siteId = HQSite.SiteId;
                //}

                AddressTypeListBL addressTypeListBL = new AddressTypeListBL(executionContext);
                List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchAddressTypeParams = new List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>>();
                searchAddressTypeParams.Add(new KeyValuePair<AddressTypeDTO.SearchByParameters, string>(AddressTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
                List<AddressTypeDTO> addressTypeDTOList = addressTypeListBL.GetAddressTypeDTOList(searchAddressTypeParams);
                if (addressTypeDTOList == null)
                {
                    addressTypeDTOList = new List<AddressTypeDTO>();
                }
                log.LogMethodExit(addressTypeDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = addressTypeDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}
