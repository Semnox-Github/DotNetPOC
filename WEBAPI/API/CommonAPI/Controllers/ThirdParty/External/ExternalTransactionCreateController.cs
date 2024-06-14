/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalTransactionCreateController  API -  Create transaction in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *2.130.7     22-Jul-2022           Abhishek              Created - External  REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.ThirdParty.External;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Site;
using Semnox.Parafait.POS;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalTransactionCreateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Transaction
        /// </summary>       
        /// <param name="externalTransactionDTO">externalTransactionDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Transaction/Create")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] ExternalTransactionDTO externalTransactionDTO)
        {
            log.LogMethodEntry(externalTransactionDTO);
            ExecutionContext executionContext = null;
            try
            {
                if (externalTransactionDTO.Operators == null || string.IsNullOrEmpty(externalTransactionDTO.Operators.LoginId))
                {
                    executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                }
                else
                {
                    executionContext = GetExecutionContext(externalTransactionDTO);
                }
                if (externalTransactionDTO == null)
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Transaction data cannot be null.Please enter the Transaction Details"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                if (externalTransactionDTO.Id > 0)
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("TransactionId cannot be greater than zero on transaction creation"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                if (String.IsNullOrEmpty(externalTransactionDTO.Type))
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Please enter transaction type"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                if (externalTransactionDTO.Type!="Create")
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Please enter valid entitlement type"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ExternalTransactionDTO externalTransaction = new ExternalTransactionDTO();
                ExternalTransactionBL externalTransactionBL = new ExternalTransactionBL(executionContext, externalTransactionDTO);
                externalTransaction = externalTransactionBL.Save();
                log.LogMethodExit(externalTransaction);
                return Request.CreateResponse(HttpStatusCode.OK, externalTransaction);
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Set the context.
        /// </summary>
        private ExecutionContext GetExecutionContext(ExternalTransactionDTO externalTransactionDTO)
        {
            log.LogMethodEntry(externalTransactionDTO);
            int siteId = -1;
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            string loginId = externalTransactionDTO.Operators.LoginId;
            string posMachineName = externalTransactionDTO.Operators.PosMachine;
            ExecutionContext executionContext = null;
            if (externalTransactionDTO.Operators.SiteId > 0)
                siteId = externalTransactionDTO.Operators.SiteId;
            bool isCorporate = true;
            SiteList siteList = new SiteList(null);
            var content = siteList.GetAllSites(-1, -1, -1);
            if (content != null && content.Count > 1)
            {
                HttpContext.Current.Application["IsCorporate"] = "True";
                isCorporate = true;
                siteId = externalTransactionDTO.Operators.SiteId;
            }
            else
            {
                HttpContext.Current.Application["IsCorporate"] = "False";
                isCorporate = false;
            }


            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId.ToString()));
            searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, loginId));

            UsersList usersList = new UsersList(null);
            List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
            if (usersDTOs == null || !usersDTOs.Any())
                throw new ValidationException("User not found");

            UsersDTO user = usersDTOs.Find(x => x.LoginId == loginId);

            if (user == null)
                throw new ValidationException("User not found");


            ExecutionContext tempContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, isCorporate, -1);
            int machineId = -1;
            if (!String.IsNullOrEmpty(posMachineName))
            {
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, posMachineName));
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                POSMachineList pOSMachineList = new POSMachineList(tempContext);
                List<POSMachineDTO> pOSMachineDTOList = pOSMachineList.GetAllPOSMachines(searchParameters, false, false);
                if (pOSMachineDTOList == null || !pOSMachineDTOList.Any())
                {
                    String messages = "POS Machine " + posMachineName + " is not set up.";
                    log.Error(messages);
                    throw new ValidationException(messages);
                }
                machineId = pOSMachineDTOList.FirstOrDefault().POSMachineId;
            }

            if (isCorporate)
            {
                securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, user.SiteId.ToString(), Convert.ToString(-1), Convert.ToString(user.RoleId), machineid: machineId.ToString(), userSessionId: Guid.NewGuid().ToString());
            }
            else
            {
                securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, "-1", Convert.ToString(-1), Convert.ToString(user.RoleId), machineid: machineId.ToString(), userSessionId: Guid.NewGuid().ToString());
            }
            SecurityTokenDTO securityTokenDTO = null;
            securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, machineId, user.UserId, isCorporate, Convert.ToInt32(securityTokenDTO.LanguageId));
            log.LogMethodEntry(executionContext);
            return executionContext;
        }
    }
}



