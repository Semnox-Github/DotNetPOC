/********************************************************************************************
 * Project Name - Utitlities
 * Description  - Validate token adn authorization 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        23-Sep-2018   Manoj          Created
 *********************************************************************************************
 *2.60        14-Mar-2019   Jagan Mohan    Implemented Roles Authorization for Form Access: ValidateAuthorization()
 *2.60        25-Mar-2019   Nagesh Badiger Added log method entry and method exit                                         
 *2.70.0      27-Jul-2019   Nitin Pai      Converted Guest App Loging to Anonymous Login 
 *2.70.2      01-Jan-2020   Jeevan         GenerateJWT Token , update and local server date time using genericLookupValuesList methid
 *2.80        02-Apr-2020   Nitin Pai      Changed token handler for Customer Registration Changes, sending authorization token in header,      
 *                                         refresh token only if it is close to expiry and not on every call
 *2.110       09-Feb-2021   Girish Kundar  Modified: Added SessionId in SecurityTokenDTO
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.Security.Claims;
using System.Threading;
using System.Web.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;
using System.Globalization;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Semnox.Core.Utilities
{

    public class ParafitToken : SoapHeader
    {
        public string Token;
    }

    public class SecurityTokenBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext; // Required in adding a new token. Added by Manoj on 10/Oct/2018
        private bool newJWTRequired = true;
        private string existingJWTToken;

        private static readonly int JWTTokenLifeTimeInMinutes;
        private static readonly string jwtKey;
        private static readonly string JWTIssuer;
        private static readonly string JWTAudience;
        static SecurityTokenBL()
        {
            try
            {
                JWTTokenLifeTimeInMinutes = Convert.ToInt32(Encryption.GetParafaitKeys("JWTTokenLifeTimeInMinutes"));
                jwtKey = Encryption.GetParafaitKeys("JWTKey");
                JWTIssuer = Encryption.GetParafaitKeys("JWTIssuer");
                JWTAudience = Encryption.GetParafaitKeys("JWTAudience");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                JWTTokenLifeTimeInMinutes = 30;
            }
            
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public SecurityTokenBL()
        {
            log.LogMethodEntry();
            securityTokenDTO = null;
            log.LogMethodExit();
        }

        public SecurityTokenBL(string existingJWTToken, bool newJWTRequired = false)
        {
            log.LogMethodEntry();
            securityTokenDTO = null;
            this.newJWTRequired = newJWTRequired;
            this.existingJWTToken = existingJWTToken;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SecurityTokenBL(ExecutionContext executionContext) : this()
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        public bool GenerateToken(string objGuid, string objectName, string token = "", DateTime? expiry = null,
                                   string userSessionId = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();

            List<SecurityTokenDTO> securityTokenDTOList = CheckActiveTokens(objGuid, token, userSessionId, sqlTransaction);// Check if there are active tokens with Invalid attempts execceding Limit and show lock our message
            log.Debug("CheckActiveTokens () - securityTokenDTOList : " + securityTokenDTOList == null ? 0 : securityTokenDTOList.Count);

            if (securityTokenDTOList != null && securityTokenDTOList.Any())
            {
                log.Debug("Active token exists");
                securityTokenDTOList = securityTokenDTOList.OrderByDescending(x => x.LastActivityTime).ToList();
                securityTokenDTO = securityTokenDTOList[0];
                if (securityTokenDTO.ExpiryTime != DateTime.MinValue && securityTokenDTO.ExpiryTime < ServerDateTime.Now)
                {
                    log.Debug("securityTokenDTO.ExpiryTime < ServerDateTime.Now. Need to create new token  ");
                    securityTokenDTO = null;
                }
            }

            if (securityTokenDTO != null)
            {
                log.Debug("securityTokenDTO != null");
                securityTokenDTO.InvalidAttempts = 0;
                if (string.IsNullOrEmpty(token))
                {
                    log.Debug("token empty");
                    token = System.Guid.NewGuid().ToString().Replace("-", "") + DateTime.Now.Ticks.ToString();
                }
                securityTokenDTO.Token = token;
                if (expiry != null)
                {
                    log.Debug("expiry time : " + expiry);
                    securityTokenDTO.ExpiryTime = Convert.ToDateTime(expiry.ToString());
                }
                log.Debug("saving token" + securityTokenDTO.ExpiryTime);
                this.Save();
            }
            else
            {
                log.Debug("creating token");
                securityTokenDTO = new SecurityTokenDTO();
                securityTokenDTO.TableObject = objectName;
                securityTokenDTO.ObjectGuid = objGuid;
                if (string.IsNullOrEmpty(token))
                {
                    log.Debug("token empty");
                    token = System.Guid.NewGuid().ToString().Replace("-", "") + DateTime.Now.Ticks.ToString();
                }
                securityTokenDTO.Token = token;
                securityTokenDTO.IsActive = "Y";
                if (expiry != null)
                {
                    log.Debug("expiry time : " + expiry);
                    securityTokenDTO.ExpiryTime = Convert.ToDateTime(expiry.ToString());
                }
                securityTokenDTO.UserSessionId = userSessionId;

                log.Debug("saving new token" + securityTokenDTO.ExpiryTime);
                this.Save(sqlTransaction);

            }

            log.LogMethodExit();
            return true;
        }

        /// <summary>
        /// Added by Manoj- 02/Oct
        /// The below  function generates a new token. based on the below parameters
        /// </summary>
        /// <param name="loginid"></param>
        /// <param name="guid"></param>
        /// <param name="siteid"></param>
        /// <param name="languageid"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public bool GenerateNewJWTToken(string loginid = "", string guid = "", string siteid = "",
                                        string languageid = "", string roleid = "", string objectName = "User",
                                        string machineid = "-1", string userSessionId = null, int lifeTime = -1)
        {

            // After login GUID will be received, this will beinserted in to the securty token table onece.
            // For further requests, GUID is not required.
            log.LogMethodEntry();
            string tokenString = "";
            try
            {
                if (String.IsNullOrEmpty(userSessionId))
                    userSessionId = Guid.NewGuid().ToString();
                // userid will be passed only from the login controlleC:\SourceCode\Parafait\Sources\Development\Web\WEBAPI\API\CommonAPI\Controllers\Games\ReaderThemesController.csr.
                // once the user is authenticated, userid and user name will be  stored in JWT via claims

                if (guid == "")
                {
                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    guid = identity.FindFirst(ClaimTypes.UserData).Value;
                    if (string.IsNullOrWhiteSpace(siteid))
                    {
                        siteid = identity.FindFirst(ClaimTypes.Sid).Value;
                    }
                    loginid = identity.FindFirst(ClaimTypes.Name).Value;
                    languageid = identity.FindFirst(ClaimTypes.Locality).Value;
                    roleid = identity.FindFirst(ClaimTypes.Role).Value;
                    machineid = identity.FindFirst(ClaimTypes.System).Value;
                    userSessionId = identity.FindFirst(ClaimTypes.PrimarySid).Value;
                }


                //Set issued at date
                DateTime issuedAt = DateTime.UtcNow;
                

                //set the time when it expires
                if(lifeTime <= 0)
                {
                    lifeTime = JWTTokenLifeTimeInMinutes;
                }
                DateTime expires = DateTime.UtcNow.AddMinutes(lifeTime);
                DateTime expiryTime = ServerDateTime.Now.AddMinutes(lifeTime);
                //JWTTokenLifeTimeInMinutes
                //create a identity and add claims to the user which we want to log in
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                    // There is no direct attribute for GUID, siteid hence used UserData and UserData. 
                    // This attribute required to  valdiate the JWT in the request message of the toekn validator.
                    new Claim(ClaimTypes.UserData, guid), // GUID
                    new Claim(ClaimTypes.Sid, siteid), // SiteID
                    new Claim(ClaimTypes.Name, loginid), // Login Id
                    new Claim(ClaimTypes.Locality, languageid), // Language Id 
                    new Claim(ClaimTypes.Role, roleid), // Role Id 
                    new Claim(ClaimTypes.System, machineid), // Machine Id 
                    new Claim(ClaimTypes.PrimarySid, userSessionId) // user Session Id 
                });

                if (newJWTRequired == true)
                {
                    var now = DateTime.UtcNow;
                    var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(jwtKey));
                    var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

                    //create the jwt
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateJwtSecurityToken(
                                 issuer: JWTIssuer,
                                    audience: JWTAudience,
                                subject: claimsIdentity,
                                notBefore: issuedAt,
                                expires: expires,
                                signingCredentials: signingCredentials);

                    tokenString = tokenHandler.WriteToken(token);
                }
                else if (newJWTRequired == false)
                {
                    tokenString = this.existingJWTToken;
                }
                ExecutionContext executionContext = new ExecutionContext(loginid, Convert.ToInt32(siteid), Convert.ToInt32(machineid), -1, false, Convert.ToInt32(languageid));
                this.executionContext = executionContext;
                GenerateToken(guid, objectName, tokenString, expiryTime, userSessionId);

                //add token string to claimns
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Authentication, tokenString));

                this.securityTokenDTO.LoginId = loginid; // this is required for creating context from individual controller
                this.securityTokenDTO.LanguageId = Convert.ToInt32(languageid);
                this.securityTokenDTO.RoleId = Convert.ToInt32(roleid);
                this.securityTokenDTO.SiteId = Convert.ToInt32(siteid);
                this.securityTokenDTO.MachineId = Convert.ToInt32(machineid);
                this.securityTokenDTO.UserSessionId = userSessionId;
            }
            catch (Exception ex)
            {
                log.Debug("Unable to generate JWT token " + ex.Message);
                throw;
            }
            log.LogMethodExit();
            return true;
        }

        /// <summary>
        /// Added by Manoj- 02/Oct
        /// The below  function generates a new token. based on the below parameters
        /// </summary>
        /// <param name="loginid"></param>
        /// <param name="guid"></param>
        /// <param name="siteid"></param>
        /// <param name="languageid"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public bool GenerateJWTToken(string loginid = "", string guid = "", string siteid = "", string languageid = "",
                                     string roleid = "", string objectName = "User", string machineid = "-1",
                                     string userSessionId = null)
        {

            // After login GUID will be received, this will beinserted in to the securty token table onece.
            // For further requests, GUID is not required.
            log.LogMethodEntry();
            try
            {
                // userid will be passed only from the login controlleC:\SourceCode\Parafait\Sources\Development\Web\WEBAPI\API\CommonAPI\Controllers\Games\ReaderThemesController.csr.
                // once the user is authenticated, userid and user name will be  stored in JWT via claims
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                guid = identity.FindFirst(ClaimTypes.UserData).Value;
                if (siteid == "")
                    siteid = identity.FindFirst(ClaimTypes.Sid).Value;
                loginid = identity.FindFirst(ClaimTypes.Name).Value;
                languageid = identity.FindFirst(ClaimTypes.Locality).Value;
                roleid = identity.FindFirst(ClaimTypes.Role).Value;
                machineid = identity.FindFirst(ClaimTypes.System).Value;
                userSessionId = identity.FindFirst(ClaimTypes.PrimarySid).Value; // userSessionId

                String token = "";// identity.FindFirst(ClaimTypes.Authentication).Value;

                if (securityTokenDTO == null || securityTokenDTO.TokenId == -1)
                {
                    securityTokenDTO = new SecurityTokenDTO();
                    securityTokenDTO.TableObject = objectName;
                    securityTokenDTO.ObjectGuid = guid;
                }

                securityTokenDTO.LoginId = loginid; // this is required for creating context from individual controller
                securityTokenDTO.LanguageId = Convert.ToInt32(languageid);
                securityTokenDTO.RoleId = Convert.ToInt32(roleid);
                securityTokenDTO.SiteId = Convert.ToInt32(siteid);
                securityTokenDTO.MachineId = Convert.ToInt32(machineid);
                securityTokenDTO.Token = token;
                securityTokenDTO.UserSessionId = userSessionId;
            }
            catch
            {
                throw;
            }
            log.LogMethodExit();
            return true;
        }

        internal string DecryptSignature(string signature)
        {
            log.LogMethodEntry();
            string result = string.Empty;
            string privateKey = string.Empty;
            var encoding = new System.Text.ASCIIEncoding();
            SystemOptionsList systemOptionsList = new SystemOptionsList();
            List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>> searchSystemOptionsParameter = new List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>>();
            searchSystemOptionsParameter.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.OPTION_NAME, "VirtualArcadePrivateKey"));
            List<SystemOptionsDTO> systemOptionsDTOList = systemOptionsList.GetSystemOptionsDTOList(searchSystemOptionsParameter);
            if (systemOptionsDTOList != null && systemOptionsDTOList.Any())
            {
                privateKey = systemOptionsDTOList[0].OptionValue;
            }
            else
            {
                log.Error("Unable to find the Virtual Arcade PrivateKey");
                throw new Exception("Unable to find the Virtual Arcade PrivateKey");
            }
            // string privateKey = "5u7x!A%D*G-KaPdSgVkYp3s6v9y$B?E(";
            byte[] privateKeyByte = encoding.GetBytes(privateKey);
            byte[] newBytes = Convert.FromBase64String(signature);
            result = Encoding.UTF8.GetString(Encryption.Decrypt(newBytes, privateKeyByte));
            log.LogMethodExit();
            return result;
        }

        internal string DecryptPayLoad(string key, string payload)
        {
            log.LogMethodEntry();
            string result = string.Empty;
            var encoding = new System.Text.ASCIIEncoding();
            byte[] sessionKeyByte = encoding.GetBytes(key);
            byte[] payLoadByte = Convert.FromBase64String(payload);
            result = encoding.GetString(Encryption.Decrypt(payLoadByte, sessionKeyByte));
            log.LogMethodExit();
            return result;
        }


        /// <summary>
        /// Manoj -02/Oct/2018
        /// This function verifies the incoming with the existing token
        /// returns boolean value
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        //public bool ValidateToken(string token, string userSessionId, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry();
        //    try
        //    {
        //        log.LogMethodEntry();
        //        SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(sqlTransaction);
        //        this.securityTokenDTO = securityTokenHandler.GetSecurityTokenDTO(token, userSessionId);
        //        if (this.securityTokenDTO != null && this.securityTokenDTO.TokenId != 0)
        //        {
        //            log.Debug("The token is exists in DB");
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //        log.Debug("The token Error");
        //        throw;
        //    }
        //    return false;
        //}

        public bool ValidateToken(string objGuid, string token, DateTime? expiry = null, int? tokenLife = null, string userSessionId = null)
        {
            log.LogMethodEntry();
            bool validated = false;
            int tokenLifeInMinutes = tokenLife != null ? Convert.ToInt32(tokenLife) : 30;

            DateTime expiryTime = expiry == null ? ServerDateTime.Now : Convert.ToDateTime(expiry);

            List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> securityTokenDTOSearchParams = new List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>>();
            securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT_GUID, objGuid));
            securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.TOKEN, token));
            securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
            if(!String.IsNullOrEmpty(userSessionId))
                securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.USER_SESSION_ID, userSessionId));

            List<SecurityTokenDTO> securityTokenDTOList = new SecurityTokenHandler(null).GetSecurityTokenDTOList(securityTokenDTOSearchParams);

            if (securityTokenDTOList != null && securityTokenDTOList.Any())
            {
                log.Error("Tokens found");
                securityTokenDTOList = securityTokenDTOList.Where(x => x.Token == token).ToList();
                if (securityTokenDTOList != null && securityTokenDTOList.Any())
                {
                    SecurityTokenDTO securityTokenDTO = securityTokenDTOList.OrderByDescending(x => x.LastActivityTime).ToList()[0];
                    log.Error(securityTokenDTO.ExpiryTime + ":" + expiry);
                    if (securityTokenDTO != null && (securityTokenDTO.ExpiryTime == DateTime.MinValue || securityTokenDTO.ExpiryTime > expiryTime))
                    {
                        DateTime localServerTime = ServerDateTime.Now;
                        if (((localServerTime - securityTokenDTO.LastActivityTime).TotalMinutes <= tokenLifeInMinutes && securityTokenDTO.InvalidAttempts > 0))
                        {
                            log.Debug("ValidateToken(false)  securityTokenDTO.InvalidAttempts > 0,TotalMinutes <= tokenLifeInMinutes");
                            validated = false;
                        }
                        else
                        {
                            log.Debug("ValidateToken(true)");
                            validated = true;
                        }
                    }
                }
                else
                {
                    log.Error("no matching Tokens found");
                }
            }
            else
            {
                log.Error("no tokens found");
            }

            log.Error("validation status " + validated);
            log.LogMethodExit();
            return validated;
        }

        public bool ExpiryToken(string objGuid, string token, string userSessionId = null)
        {
            log.LogMethodEntry(objGuid, token, userSessionId);
            bool expiryStatus = false;
            SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(null);

            List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> securityTokenDTOSearchParams = new List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>>();
            securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT_GUID, objGuid));
            securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.TOKEN, token));
            securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
            if (!String.IsNullOrEmpty(userSessionId))
                securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.USER_SESSION_ID, userSessionId));

            List<SecurityTokenDTO> securityTokenDTOList = new SecurityTokenHandler(null).GetSecurityTokenDTOList(securityTokenDTOSearchParams);

            if (securityTokenDTOList != null && securityTokenDTOList.Any())
            {
                log.Debug("Tokens found");
                securityTokenDTOList = securityTokenDTOList.Where(x => x.Token == token).ToList();
                if (securityTokenDTOList != null && securityTokenDTOList.Any())
                {
                    SecurityTokenDTO securityTokenDTO = securityTokenDTOList.OrderByDescending(x => x.LastActivityTime).ToList()[0];
                    log.Error(securityTokenDTO.Token);
                    securityTokenDTO.IsActive = "N";
                    securityTokenDTO.ExpiryTime = ServerDateTime.Now;
                    securityTokenDTO = securityTokenHandler.UpdateSecurityTokenDTO(securityTokenDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    expiryStatus = true;
                }
            }
            else
            {
                log.Error("No Tokens found");
                expiryStatus = false;
            }
            log.LogMethodExit();
            return expiryStatus;
        }

        public bool ValidateAndUpdateToken(string token, string objGuid = "", bool refreshToken = false, string userSessionId = null, SqlTransaction sqlTransaction = null, int lifeTime = -1)
        {
            log.LogMethodEntry();
            log.Debug("begin ValidateAndUpdateToken()");
            int lockOutDuration = 30;
            int inValidAttemptsBeforeLockOut = 5;
            int pOSInActiveDuration = 30;
            int inValidAttempts = 0;

            SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(sqlTransaction);
            this.securityTokenDTO = securityTokenHandler.GetSecurityTokenDTO(token, userSessionId);

            if (securityTokenDTO == null)
            {
                log.Debug("securityTokenDTO == null");
                this.securityTokenDTO = new SecurityTokenDTO();
                securityTokenDTO.TokenValidationMessage = "Invalid Token";
                log.Debug("Invalid Token");
                securityTokenDTO.TokenValidated = false;
                log.LogMethodExit(null);
                log.Debug("return false");
                return false;
            }
            else
            {

                bool isTokenValid = false;

                try
                {
                    inValidAttempts = securityTokenDTO.InvalidAttempts <= 0 ? 1 : securityTokenDTO.InvalidAttempts;
                    log.Debug("inValidAttempts :" + inValidAttempts);
                    DateTime localServerTime = ServerDateTime.Now;
                    if (localServerTime > securityTokenDTO.ExpiryTime)
                    {
                        securityTokenDTO.TokenValidationMessage = "Invalid Token Expired";
                        log.LogVariableState("Invalid Token Expired : ", securityTokenDTO);
                        inValidAttempts++;
                        securityTokenDTO.InvalidAttempts = inValidAttempts;
                    }
                    if (((localServerTime.Subtract(securityTokenDTO.LastActivityTime)).TotalMinutes <= lockOutDuration && securityTokenDTO.InvalidAttempts > inValidAttemptsBeforeLockOut))
                    {
                        securityTokenDTO.TokenValidationMessage = "Your account has been locked! Please try after 30 mins or contact your admin to unlock.";
                        log.Debug(" return false : Your account has been locked! Please try after 30 mins or contact your admin to unlock");
                        return false;
                    }
                    else if (securityTokenDTO.IsActive == "N" || ((securityTokenDTO.TableObject == "Customer" || securityTokenDTO.TableObject == "Users") && String.IsNullOrEmpty(objGuid) == false && securityTokenDTO.ObjectGuid.ToString() != objGuid))
                    {
                        log.Debug(" Invalid Token");
                        securityTokenDTO.TokenValidationMessage = "Invalid Token";
                        inValidAttempts++;
                        securityTokenDTO.InvalidAttempts = inValidAttempts;
                    }
                    else if (securityTokenDTO.ExpiryTime == DateTime.MinValue && (localServerTime.Subtract(securityTokenDTO.LastActivityTime)).TotalMinutes >= pOSInActiveDuration)
                    {
                        securityTokenDTO.TokenValidationMessage = "Invalid Token Expired";
                        log.LogVariableState("Invalid Token Expired : ", securityTokenDTO);
                        log.Debug(" Invalid Token Expired :");
                        inValidAttempts++;
                        securityTokenDTO.InvalidAttempts = inValidAttempts;
                    }
                    else
                    {
                        if (refreshToken)
                        {
                            // should the token be regenerated in refresh or extend the token expiry?
                            securityTokenDTO.InvalidAttempts = 0;
                            GenerateNewJWTToken("External POS", securityTokenDTO.ObjectGuid, executionContext.GetSiteId().ToString(), "-1", "-1", securityTokenDTO.TableObject, "-1", Guid.NewGuid().ToString(), lifeTime);
                        }
                        isTokenValid = true;
                    }

                    log.Debug(" Calling save() ");
                    this.Save(sqlTransaction);
                    securityTokenDTO.TokenValidated = isTokenValid;
                }
                catch (Exception ex)
                {
                    log.Debug(" Exception ValidateAndUpdateToken() ");
                    log.Debug(ex);
                }
                log.LogMethodExit(securityTokenDTO.TokenValidationMessage);
                return isTokenValid;
            }

        }

        public void UpdateTokenInvalidAttempt(string token, string userSessionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(sqlTransaction);
            this.securityTokenDTO = securityTokenHandler.GetSecurityTokenDTO(token, userSessionId);
            if (securityTokenDTO != null)
            {
                securityTokenDTO.TokenValidationMessage = "Invalid Token";
                int inValidAttempts = securityTokenDTO.InvalidAttempts <= 0 ? 1 : securityTokenDTO.InvalidAttempts;
                log.Debug("Invalid Attempts" + securityTokenDTO.InvalidAttempts);
                inValidAttempts++;
                securityTokenDTO.InvalidAttempts = inValidAttempts;
                log.Debug("Ends - Invalid Attempts" + securityTokenDTO.InvalidAttempts);
                this.Save(sqlTransaction);
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// CheckActiveTokens - Method to check if there are active tokens 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private List<SecurityTokenDTO> CheckActiveTokens(string guid, string token = "", string userSessionId = "", SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            log.Debug(" begin CheckActiveTokens()");
            int inValidAttemptsThreshold = 5;
            int lockOutDuration = 30;
            int lockedTokens = 0;

            DateTime localServerTime = ServerDateTime.Now;

            List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> securityTokenDTOSearchParams = new List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>>();
            securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT_GUID, guid));
            securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
            if (string.IsNullOrEmpty(userSessionId) == false)
            {
                securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.USER_SESSION_ID, userSessionId));
            }
            List<SecurityTokenDTO> securityTokenDTOList = new SecurityTokenHandler(sqlTransaction).GetSecurityTokenDTOList(securityTokenDTOSearchParams);
            if (securityTokenDTOList != null && securityTokenDTOList.Any())
            {
                log.Debug(" Token exists");
                foreach (SecurityTokenDTO securityTokenDTO in securityTokenDTOList)
                {
                    if (((localServerTime - securityTokenDTO.LastActivityTime).TotalMinutes <= lockOutDuration && securityTokenDTO.InvalidAttempts > inValidAttemptsThreshold))
                    {
                        log.Debug(" Token : LastActivityTime).TotalMinutes <= lockOutDuration && securityTokenDTO.InvalidAttempts > inValidAttemptsThreshold)");
                        lockedTokens++;
                        break;
                    }
                    if (localServerTime > securityTokenDTO.ExpiryTime)
                    {
                        log.Debug("Token expired time : " + securityTokenDTO.ExpiryTime);
                        break;
                    }
                }

                if (lockedTokens > 0)
                {
                    log.Debug("lockedTokens > 0");
                    throw new Exception("Your account has been locked! Please try after 30 mins or contact your admin to unlock. ");
                }
            }

            log.LogMethodExit();
            return securityTokenDTOList;
        }

        /// <summary>
        ///  Clear Token
        /// </summary>
        /// <param name="token"></param>
        public void Cleartoken(string token, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(sqlTransaction);
            this.securityTokenDTO = securityTokenHandler.GetSecurityTokenDTO(token);

            if (securityTokenDTO != null)
            {
                log.Debug("securityTokenDTO != null");
                DateTime localServerTime = DateTime.Now;  // update as utc time 
                securityTokenDTO.ExpiryTime = localServerTime;
                securityTokenDTO.IsActive = "N";
                log.Debug("calling save() securityTokenDTO");
                this.Save(sqlTransaction);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// get SecurityTokenDTO object
        /// </summary>
        public SecurityTokenDTO GetSecurityTokenDTO
        {
            get { return securityTokenDTO; }
        }
        /// <summary>
        /// Get the Form access allowed or not based on the roleId and formName.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="roleId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public bool ValidateFormAccess(string formName, string roleId, string siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(formName, roleId, siteId);
            try
            {
                log.Debug("calling ValidateFormAccess()");
                SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(sqlTransaction);
                log.LogMethodExit();
                return securityTokenHandler.ValidateFormAccess(formName, roleId, siteId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///    Saves the SecurityTokenDTO
        /// Checks if the pk_SecurityTokens is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(sqlTransaction);
            if (securityTokenDTO.TokenId == -1)
            {
                // Manoj - executionContext has been defined in the constructor. When  the  BL class was instantiated, executionContext will be set
                securityTokenDTO = securityTokenHandler.InsertSecurityTokenDTO(securityTokenDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                securityTokenDTO.AcceptChanges();
            }
            else
            {
                if (securityTokenDTO.IsChanged == true)
                {
                    // Manoj - executionContext has been defined in the constructor. When the BL class was instantiated, executionContext will be set
                    securityTokenDTO = securityTokenHandler.UpdateSecurityTokenDTO(securityTokenDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    securityTokenDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///    Decrypts the login token string sent by the guest app
        ///    Converts the payload sent into Disctionary entries and sends back to calling function
        /// </summary>
        public Dictionary<string, string> ValidateAndParseAnonymousToken(string requestor, string loginToken)
        {
            log.LogMethodEntry(loginToken);
            Dictionary<string, string> tokenKeyValuePairs = new Dictionary<string, string>();
            try
            {
                string anonymousLoginKey = Encryption.GetParafaitKeys("AnonymousLoginKey" + requestor);
                log.LogVariableState("AnonymousLoginKey" + requestor, anonymousLoginKey);
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(anonymousLoginKey));
                Microsoft.IdentityModel.Tokens.SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                };

                log.LogVariableState("anonymousLoginKey", securityKey);

                // validate the key sent by the client
                ClaimsPrincipal claims = handler.ValidateToken(loginToken, validationParameters, out securityToken);
                log.LogVariableState("claims", claims);

                for (int i = 0; i < claims.Claims.Count(); i++)
                {
                    string temp = (claims.Claims.ToArray())[i].ToString();
                    string[] kvpair = new string[2];
                    kvpair[0] = temp.Substring(0, temp.IndexOf(':'));
                    kvpair[1] = temp.Substring(temp.IndexOf(':') + 1);
                    if (kvpair[0].ToString().ToUpper().Equals("NAME"))
                    {
                        tokenKeyValuePairs.Add("origin", kvpair[1].ToUpper());
                    }
                    else if (kvpair[0].ToString().ToUpper().Equals("USERDATA"))
                    {
                        tokenKeyValuePairs.Add("identifier", kvpair[1].ToUpper());
                    }
                    else if (kvpair[0].ToString().ToUpper().Equals("ISSUEDAT"))
                    {
                        tokenKeyValuePairs.Add("issuedAt", kvpair[1]);
                    }
                    else if (kvpair[0].ToString().ToUpper().Equals("EXPIRESAT"))
                    {
                        tokenKeyValuePairs.Add("expiresAt", kvpair[1]);
                    }
                }
                //tokenKeyValuePairs.Add("origin", "COM.SEMNOX.CONSUMERAPP");
                //tokenKeyValuePairs.Add("identifier", (new Guid("89127BF7-4AF6-4D60-A22F-88D0807CFC9B")).ToString());
                //tokenKeyValuePairs.Add("issuedAt", DateTime.Now.ToString());
                //tokenKeyValuePairs.Add("expiresAt", DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                log.Error("login failed", ex);
                log.LogVariableState("message", ex.Message);
                throw;
            }

            return tokenKeyValuePairs;
        }
        /// <summary>
        /// This method is used to generate HashValue based on the passed API Key
        /// </summary>
        /// <param name="requestingId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateChecksum(string requestingId, string request)
        {
            log.LogMethodEntry(requestingId, request);

            string anonymousLoginKey = Encryption.GetParafaitKeys("AnonymousLoginKey" + requestingId);
            log.Info("anonymousLoginKey" + anonymousLoginKey);
            var encoding = new System.Text.ASCIIEncoding();
            byte[] securityKeyByte = encoding.GetBytes(anonymousLoginKey);

            byte[] rawData = encoding.GetBytes(request);
            using (var hmacsha256 = new HMACSHA256(securityKeyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(rawData);
                var hashValue = Convert.ToBase64String(hashmessage);
                log.LogMethodExit(hashValue);
                return hashValue;
            }
        }
    }



    /// <summary>
    /// Manages the list of SecurityTokenDTO
    /// </summary>
    public class SecurityTokenListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the GetSecurityTokenDTOList list
        /// </summary>       
        public List<SecurityTokenDTO> GetSecurityTokenDTOList(List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>>
                                                             searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(sqlTransaction);
            List<SecurityTokenDTO> securityTokenDTOList = securityTokenHandler.GetSecurityTokenDTOList(searchParameters);
            log.LogMethodExit(securityTokenDTOList);
            return securityTokenDTOList;
        }


        /// <summary>
        /// Takes LookupParams as parameter
        /// </summary>
        /// <returns>Returns KeyValuePair List of SecurityTokenDTO.SearchByParameters by converting SecurityTokenDTO</returns>
        public List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> BuildSecurityTokenDTOSearchParametersList(SecurityTokenDTO securityTokenDTO)
        {
            log.LogMethodEntry();
            List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> securityTokenDTOSearchParams = new List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>>();
            if (securityTokenDTO != null)
            {
                if (securityTokenDTO.TokenId >= 0)
                    securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.TOKENID, securityTokenDTO.TokenId.ToString()));

                if (!(string.IsNullOrEmpty(securityTokenDTO.Token)))
                    securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.TOKEN, securityTokenDTO.Token.ToString()));

                if (!(string.IsNullOrEmpty(securityTokenDTO.TableObject)))
                    securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT, securityTokenDTO.TableObject.ToString()));

                if (!(string.IsNullOrEmpty(securityTokenDTO.ObjectGuid)))
                    securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT_GUID, securityTokenDTO.ObjectGuid.ToString()));

                if (securityTokenDTO.SiteId >= 0)
                    securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));

                securityTokenDTOSearchParams.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.ACTIVE_FLAG, securityTokenDTO.IsActive.ToString()));

            }

            log.LogMethodExit();
            return securityTokenDTOSearchParams;
        }


        /// <summary>
        /// GetSecurityTokenDTOsList(SecurityTokenDTO SecurityTokenDTO) method search based on SecurityTokenDTO
        /// </summary>
        /// <param name="SecurityTokenDTO"></param>
        /// <returns>List of SecurityTokenDTO object</returns>
        public List<SecurityTokenDTO> GetSecurityTokenDTOList(SecurityTokenDTO SecurityTokenDTO, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> searchParameters = BuildSecurityTokenDTOSearchParametersList(SecurityTokenDTO);
                SecurityTokenHandler securityTokenHandler = new SecurityTokenHandler(sqlTransaction);
                log.LogMethodExit();
                return securityTokenHandler.GetSecurityTokenDTOList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }
    }
}
