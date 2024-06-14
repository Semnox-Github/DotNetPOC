/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Account Game Plays Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.130.11   14-Oct-2022     Yashodhara C H       Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Game;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.Customer.Accounts
{
    public class AccountGamePlaysViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>C:\TFS WORKSPACE\Parafait\Sources\Testing\Web\WEBAPI\API\CommonAPI\Controllers\Tag\
        /// Get the JSON Object Game Plays List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/Customer/Account/{accountId}/AccountGamePlays")]
        [Authorize]
        [HttpGet]
        public async Task<HttpResponseMessage> Get(int accountId, String startDate = "", String endDate = "", int numberOfDays = 180, int pageNumber = 0, bool addSummaryRow = false, bool addDetails = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(accountId, startDate, endDate, numberOfDays, pageNumber, addSummaryRow, addDetails);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                List<GamePlayDTO> gamePlayDTOs = await accountUseCases.GetAccountGamePlaysDTOList(accountId, startDate, endDate, numberOfDays, pageNumber, addSummaryRow, addDetails);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = gamePlayDTOs });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}
