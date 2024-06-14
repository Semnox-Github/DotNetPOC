/********************************************************************************************
* Project Name - Utilities
* Description  - Implementation of user use-cases 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0     12-Nov-2019   Lakshminarayana         Created 
********************************************************************************************/
using System;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Authentication
{
    /// <summary>
    /// Implementation of Authentication use-cases
    /// </summary>
    public class LocalAuthenticationUseCases : LocalUseCases, IAuthenticationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public LocalAuthenticationUseCases()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public LocalAuthenticationUseCases(ExecutionContext executionContext)
        : base(executionContext)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private ExecutionContext CreateExecutionContext(LoginRequest loginRequest)
        {
            log.LogMethodEntry("loginRequest");
            UserContainerDTO userContainerDTO = UserContainerList.GetUserContainerDTOOrDefault(loginRequest.LoginId, loginRequest.TagNumber, SiteContainerList.IsCorporate() && string.IsNullOrWhiteSpace(loginRequest.SiteId) == false ? Convert.ToInt32(loginRequest.SiteId) : -1);
            if (userContainerDTO == null)
            {
                if (string.IsNullOrEmpty(loginRequest.TagNumber))
                {
                    string errorMessage = MessageContainerList.GetMessage(SiteContainerList.IsCorporate() ? SiteContainerList.GetMasterSiteId() : -1, (int)UserAuthenticationErrorType.INVALID_LOGIN);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.INVALID_LOGIN);
                }

                else
                {
                    string errorMessage = MessageContainerList.GetMessage(SiteContainerList.IsCorporate() ? SiteContainerList.GetMasterSiteId() : -1, (int)UserAuthenticationErrorType.INVALID_USER_TAG);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.INVALID_USER_TAG);
                }
            }
            POSMachineContainerDTO posMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(SiteContainerList.IsCorporate() ? userContainerDTO.SiteId : -1, loginRequest.MachineName, loginRequest.IPAddress, userContainerDTO.POSTypeId);
            int userPKId = userContainerDTO.UserId;
            string loginId = userContainerDTO.LoginId;
            bool isCorporate = SiteContainerList.IsCorporate();
            int siteId = isCorporate ? userContainerDTO.SiteId : -1;
            int sitePKId = SiteContainerList.GetCurrentSiteContainerDTO(siteId).SiteId;
            int machineId = -1;
            string posMachineGuid = string.Empty;
            string posMachineName = string.IsNullOrWhiteSpace(loginRequest.MachineName) ? string.Empty : loginRequest.MachineName;
            if (posMachineContainerDTO != null)
            {
                machineId = posMachineContainerDTO.POSMachineId;
                posMachineGuid = posMachineContainerDTO.Guid;
                posMachineName = posMachineContainerDTO.POSName;
            }
            ExecutionContext result = new ExecutionContext(loginId, siteId, sitePKId, machineId, userPKId, isCorporate, -1, string.Empty, posMachineGuid, posMachineName, "en-US");
            int languageId = -1;
            try
            {
                languageId = ParafaitDefaultContainerList.GetParafaitDefault<int>(result, "DEFAULT_LANGUAGE", -1);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving default language", ex);
            }
            
            string languageCode = "en-US";
            if (languageId != -1)
            {
                try
                {
                    LanguageContainerDTO languageContainerDTO = LanguageContainerList.GetLanguageContainerDTO(result.SiteId, languageId);
                    languageCode = languageContainerDTO.LanguageCode;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while retrieving language container DTO", ex);
                }
            }
            result.LanguageId = languageId;
            result.LanguageCode = languageCode;

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Implementation of system user login use case
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<ExecutionContext> LoginSystemUser(LoginRequest loginRequest)
        {
            return await Task<ExecutionContext>.Factory.StartNew(() =>
            {
                if (string.IsNullOrEmpty(loginRequest.LoginId) || string.IsNullOrEmpty(loginRequest.TagNumber) == false)
                {
                    string errorMessage = MessageContainerList.GetMessage(SiteContainerList.IsCorporate() ? SiteContainerList.GetMasterSiteId() : -1, (int)UserAuthenticationErrorType.INVALID_LOGIN);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.INVALID_LOGIN);
                }
                log.LogMethodEntry(loginRequest);
                ExecutionContext result = CreateExecutionContext(loginRequest);
                Users user = new Users(result, result.UserPKId, true, true);
                user.AuthenticateSystemUser(loginRequest.LoginId, loginRequest.Password);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                UsersDTO usersDTO = user.UserDTO;
                int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(result, "JWT_TOKEN_LIFE_TIME", 0);
                securityTokenBL.GenerateNewJWTToken(result.UserId, usersDTO.Guid, result.SiteId.ToString(), result.LanguageId.ToString(), usersDTO.RoleId.ToString(), "User", result.MachineId.ToString(), Guid.NewGuid().ToString(), tokenLifeTime);
                var securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                result.WebApiToken = securityTokenDTO.Token;
                log.LogMethodExit(result);
                return result;
            });
        }


        /// <summary>
        /// Implementation of user login use case
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<ExecutionContext> LoginUser(LoginRequest loginRequest)
        {
            return await Task<ExecutionContext>.Factory.StartNew(() =>
            {
                ExecutionContext result = CreateExecutionContext(loginRequest);
                Users user = new Users(result, result.UserPKId, true, true);
                user.Authenticate(loginRequest.LoginId, loginRequest.TagNumber, loginRequest.Password, loginRequest.NewPassword);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                UsersDTO usersDTO = user.UserDTO;
                int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(result, "JWT_TOKEN_LIFE_TIME", 0);
                securityTokenBL.GenerateNewJWTToken(result.UserId, usersDTO.Guid, result.SiteId.ToString(), result.LanguageId.ToString(), usersDTO.RoleId.ToString(), "User", result.MachineId.ToString(), Guid.NewGuid().ToString(), tokenLifeTime);
                var securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                result.WebApiToken = securityTokenDTO.Token;
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
