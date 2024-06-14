/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Machines Game Plays
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        12-Sept-2018   Jagan          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Game;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System.Data.SqlClient;
using System.Configuration;
using Semnox.Parafait.CardCore;
using System.Linq;
using Semnox.Parafait.User;
using System.Text;
using System.IO;
using Semnox.Parafait.Languages;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class GamePlayController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object Game Plays List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/Transaction/GamePlays")]
        [Authorize]
        public HttpResponseMessage Get(string machineId = "", int gamePlayId = -1, int accountId = -1)
        {
            try
            {
                log.LogMethodEntry(machineId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                if(!String.IsNullOrEmpty(machineId))
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.MACHINE_ID, machineId));
                if(gamePlayId > -1)
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.GAME_PLAY_ID, gamePlayId.ToString()));
                if (accountId > -1)
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));

                GamePlayListBL gamePlayBL = new GamePlayListBL(executionContext);
                var content = gamePlayBL.GetGamePlayDTOListWithTagNumber(searchParameters, null);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Game Play
        /// </summary>
        /// <param name="machinesDTOList">machineDtoList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/GamePlays")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<GamePlayDTO> gamePlayDTOList)
        {
            log.LogMethodEntry(gamePlayDTOList);
            securityTokenBL.GenerateJWTToken();
            securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            if (gamePlayDTOList == null || !gamePlayDTOList.Any())
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }

            GamePlayDTO gamePlayDTO = gamePlayDTOList[0];
            int noOfGamePlays = gamePlayDTOList.Count;

            List<GamePlayDTO> createdGamePlayDTOs = new List<GamePlayDTO>();
            Semnox.Parafait.Game.Machine machine = new Semnox.Parafait.Game.Machine(gamePlayDTO.MachineId);
            if (machine.GetMachineDTO == null || machine.GetMachineDTO.MachineId < 0)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Machine is Invalid"), executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            try
            {
                Utilities utilities;
                string connstring = "";
                connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                using (utilities = new Utilities(connstring))
                {
                    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                    utilities.ParafaitEnv.Initialize();

                    Semnox.Parafait.ServerCore.GameServerEnvironment gameServerEnvironment = new Semnox.Parafait.ServerCore.GameServerEnvironment(utilities.ExecutionContext);
                    Semnox.Parafait.ServerCore.Machine servercoreMachine = new Semnox.Parafait.ServerCore.Machine(machine.GetMachineDTO.MachineId, gameServerEnvironment);
                    SqlConnection conn = utilities.getConnection();
                    using (SqlTransaction trx = conn.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < noOfGamePlays; i++)
                            {
                                createdGamePlayDTOs.Add(servercoreMachine.PlayGame(machine.GetMachineDTO.MachineId, gamePlayDTO.CardNumber, trx));
                            }
                            trx.Commit();
                        }
                        catch(Exception ex)
                        {
                            trx.Rollback();
                            string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException(ex.Message), executionContext);
                            log.Error(customException);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            log.LogMethodExit(createdGamePlayDTOs);
            return Request.CreateResponse(HttpStatusCode.OK, new { data = createdGamePlayDTOs });
        }

        private void PostGamePlayToExternalServer(string cardNumber, string machineReference, int siteId, string requestUri, int readWriteTimeout)
        {
            log.LogMethodEntry(cardNumber, machineReference, siteId);

            StringBuilder jsonBody = new StringBuilder();
            jsonBody.Append("{");
            jsonBody.Append("\"SiteID\":\"");
            jsonBody.Append(siteId.ToString());
            jsonBody.Append("\",");
            jsonBody.Append("\"ExternalReferenceID\":\"");
            jsonBody.Append(machineReference);
            jsonBody.Append("\",");
            jsonBody.Append("\"CardNumber\":\"");
            jsonBody.Append(cardNumber);
            jsonBody.Append("\"");
            jsonBody.Append("}");
            log.Debug("JSON" + jsonBody.ToString());
            string jsonResponse = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            byte[] byteArray = utf8Encoding.GetBytes(jsonBody.ToString());
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = byteArray.Length;
            httpWebRequest.ReadWriteTimeout = readWriteTimeout * 1000; // Covert readWriteTimeout FROM seconds to milliseconds
            using (Stream requestedStreamData = httpWebRequest.GetRequestStream())
            {
                requestedStreamData.Write(byteArray, 0, byteArray.Length);
            }
            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Encoding encoding = Encoding.GetEncoding("utf-8");
                    if (HttpStatusCode.OK == httpWebResponse.StatusCode)
                    {
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                            log.LogVariableState("Json Response ", jsonResponse);
                            if (jsonResponse.Contains("101"))
                                throw new Exception(MessageContainerList.GetMessage(executionContext, "GamePlay request rejected by server"));
                        }
                    }
                    else
                    {
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                            log.LogVariableState("Json Response ", jsonResponse);
                        }
                    }
                }
            }
            catch (WebException webException)
            {
                log.Error(webException.Message, webException);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
