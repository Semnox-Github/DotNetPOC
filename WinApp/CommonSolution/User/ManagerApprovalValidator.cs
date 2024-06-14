/********************************************************************************************
 * Project Name - User                                                                        
 * Description  -ManagerApprovalValidator
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.User
{
    /// <summary>
    /// THis class is created to validate the manager token for the REMOTE implementation use cases
    /// </summary>
    public class ManagerApprovalValidator
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public ManagerApprovalValidator(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public  bool ManagerApprovalReceived(string managerToken)
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                if (managerToken.Contains("Bearer"))
                {
                    managerToken = managerToken.Replace("Bearer ", "");
                }
                string jwtKey = Encryption.GetParafaitKeys("JWTKey");
                var now = DateTime.UtcNow;
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(jwtKey));
                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Encryption.GetParafaitKeys("JWTIssuer"),
                    ValidAudience = Encryption.GetParafaitKeys("JWTAudience"),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey,
                };
                System.Threading.Thread.CurrentPrincipal = handler.ValidateToken(managerToken, validationParameters, out securityToken);
                ClaimsPrincipal claims = handler.ValidateToken(managerToken, validationParameters, out securityToken);
                string roleId = claims.FindFirst(ClaimTypes.Role).Value;
                string siteId = claims.FindFirst(ClaimTypes.Sid).Value;
                string loginId = claims.FindFirst(ClaimTypes.Name).Value;
                UserContainerDTO userContainerDTO = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", Convert.ToInt32(siteId));
                UserContainerDTO manageruserContainerDTO = UserContainerList.GetUserContainerDTOOrDefault(loginId, "", Convert.ToInt32(siteId));
                if (userContainerDTO != null)
                {
                    if (userContainerDTO.SelfApprovalAllowed)
                    {
                        if (loginId == userContainerDTO.LoginId)
                        {
                            result = true;
                            return result;
                        }
                    }
                    else
                    {
                        if (manageruserContainerDTO != null)
                        {
                            if (UserRoleContainerList.CanApproveFor(executionContext.SiteId, userContainerDTO.RoleId, manageruserContainerDTO.RoleId))
                            {
                                result = true;
                                return result;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }
        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, Microsoft.IdentityModel.Tokens.SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}
