/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the UserRoles Price List Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         11-Mar-2019   Jagan Mohana         Created 
 **********************************************************************************************
 *2.60         25-Mar-2019   Akshay Gulaganji      passed executionContext in Get()
 *             25-Jun-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Added HttpDelete Method.
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Product;
using Semnox.Parafait.PriceList;

namespace Semnox.CommonAPI.SiteSetup
{
    public class UserRolePriceListController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object of UserRolesPriceList Details 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/UserRolePriceList/")]
        public HttpResponseMessage Get(string userRoleId)
        {
            try
            {
                log.LogMethodEntry(userRoleId);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>> searchParameters = new List<KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>>();
                searchParameters.Add(new KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>(UserRolePriceListDTO.SearchByUserRolePriceListParameters.ROLE_ID, userRoleId.ToString()));
               
                UserRolePriceListList userRolePriceListList = new UserRolePriceListList(executionContext);
                var content = userRolePriceListList.GetAllUserRolePriceList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on userRolePriceListDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/UserRolePriceList/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<UserRolePriceListDTO> userRolePriceListDTO)
        {
            try
            {
                log.LogMethodEntry(userRolePriceListDTO);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (userRolePriceListDTO != null && userRolePriceListDTO.Count > 0)
                {
                    // if userRolePriceListDTOs.rolePriceListId is less than zero then insert or else update
                    UserRolePriceListList userRolePriceListList = new UserRolePriceListList(executionContext, userRolePriceListDTO);
                    userRolePriceListList.SaveUpdateUserRolePriceList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }       
    }
}
