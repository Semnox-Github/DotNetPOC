/********************************************************************************************
 * Project Name - CardTypeMigrationController
 * Description  - Created to fetch, update and insert CardTypeMigration.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.90     06-Mar-2020    Girish Kundar            Created.
 *2.90     16-Jul-2020    Mushahid Faizan          Modified : as per Rest API Standard.
 ***************************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.Customer.Membership
{
    public class CardTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object CardTypeDTO Collections.
        /// </summary>       
        [HttpGet]
        [Authorize]
        [Route("api/Customer/Membership/CardTypes")]
        public HttpResponseMessage Get(int cardTypeId = -1 , string cardType = null)
        {
            log.LogMethodEntry(cardTypeId, cardType);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                List<KeyValuePair<CardTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CardTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CardTypeDTO.SearchByParameters, string>(CardTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (cardTypeId != -1)
                {
                    searchParameters.Add(new KeyValuePair<CardTypeDTO.SearchByParameters, string>(CardTypeDTO.SearchByParameters.CARDTYPE_ID, cardTypeId.ToString()));
                }
                if (!String.IsNullOrEmpty(cardType))
                {
                    searchParameters.Add(new KeyValuePair<CardTypeDTO.SearchByParameters, string>(CardTypeDTO.SearchByParameters.CARDTYPE, cardType));
                }
                CardTypeListBL cardTypeListBL = new CardTypeListBL(executionContext);
                List<CardTypeDTO> content = cardTypeListBL.GetCardTypeDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }

        /// <summary>
        /// Post the JSON Object Memberships
        /// </summary>
        /// <param name="cardTypeDTOList">MembershipsList</param>
        [HttpPost]
        [Route("api/Customer/Membership/CardTypes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<CardTypeDTO> cardTypeDTOList)
        {
            log.LogMethodEntry(cardTypeDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (cardTypeDTOList != null && cardTypeDTOList.Any())
                {
                    CardTypeListBL cardTypeListBL = new CardTypeListBL(executionContext, cardTypeDTOList);
                    cardTypeListBL.Save();
                    log.LogMethodExit(cardTypeDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = cardTypeDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty  });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }
    }
}
