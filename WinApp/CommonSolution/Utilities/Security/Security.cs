/********************************************************************************************
 * Project Name - Utitlities
 * Description  - Security 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.0      19-Jun-2019   Jagan Mohan    Added the ValidateFormAccess() method to access the forms based on the roleId
 *                                         Added the new GetUserRoleId() method for user selected site roleId
 *2.80.0      15-Oct-2019   Nitin Pai      Added  AnonymousLogin() method for external inactive users like External POS                                        
 *2.100.0     15-Nov-2020   Nitin Pai      Modified Login method to send the user from master site if no site is sent, 
 *                                         else it will send user from site                                        
 *2.110.0     15-Feb-2021   Girish Kundar  Modified:  Added method to get userPKId - concurrent login changes                                        
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using Microsoft.IdentityModel.Tokens;


namespace Semnox.Core.Utilities
{
    public class Security
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal static Semnox.Core.Utilities.Utilities _utilities;
        static Logger securityLogger;
        public Security(Semnox.Core.Utilities.Utilities inUtilities)
        {
            log.LogMethodEntry(inUtilities);
            _utilities = inUtilities;
            securityLogger = new Logger(Logger.LogType.Security, _utilities.EventLog);
            log.LogMethodExit(null);
        }

        public Security(string ConnectionString)
        {
            log.LogMethodEntry(ConnectionString);
            _utilities = new Semnox.Core.Utilities.Utilities(ConnectionString);
            securityLogger = new Logger(Logger.LogType.Security, _utilities.EventLog);
            log.LogMethodExit(null);
        }

        internal class SecurityPolicy
        {
            private string policyName = "";
            private int passwordChangeFrequency = 0;
            private int passwordMinLength = 0;
            private int passwordMinAlphabets = 0;
            private int passwordMinNumbers = 0;
            private int passwordMinSpecialChars = 0;
            private int rememberPasswordsCount = 1;
            private int invalidAttemptsBeforeLockout = 0;
            private int lockoutDuration = 0;
            private int userSessionTimeout = 0;
            private int maxUserInactivityDays = 0;
            private int maxDaysToLoginAfterUserCreation = 0;

            public string PolicyName
            {
                get { return policyName; }
            }

            public int PasswordChangeFrequency
            {
                get { return passwordChangeFrequency; }
            }

            public int PasswordMinLength
            {
                get { return passwordMinLength; }
            }

            public int PasswordMinAlphabets
            {
                get { return passwordMinAlphabets; }
            }

            public int PasswordMinNumbers
            {
                get { return passwordMinNumbers; }
            }

            public int PasswordMinSpecialChars
            {
                get { return passwordMinSpecialChars; }
            }

            public int RememberPasswordsCount
            {
                get { return rememberPasswordsCount; }
            }

            public int InvalidAttemptsBeforeLockout
            {
                get { return invalidAttemptsBeforeLockout; }
            }

            public int LockoutDuration
            {
                get { return lockoutDuration; }
            }

            public int UserSessionTimeout
            {
                get { return userSessionTimeout; }
            }

            public int MaxUserInactivityDays
            {
                get { return maxUserInactivityDays; }
            }

            public int MaxDaysToLoginAfterUserCreation
            {
                get { return maxDaysToLoginAfterUserCreation; }
            }

            public SecurityPolicy(int PolicyId)
            {
                log.LogMethodEntry(PolicyId);
                DataTable dtPolicy = _utilities.executeDataTable(@"select s.*, sp.PolicyName 
                                                                    from SecurityPolicyDetails s, SecurityPolicy sp
                                                                    where sp.PolicyId = @policyId
                                                                    and sp.PolicyId = s.PolicyId",
                                                                    new SqlParameter("@policyId", PolicyId));
                log.LogVariableState("@policyId", PolicyId);
                if (dtPolicy.Rows.Count != 0)
                {
                    policyName = dtPolicy.Rows[0]["PolicyName"].ToString();
                    passwordChangeFrequency = Convert.ToInt32(dtPolicy.Rows[0]["passwordChangeFrequency"]);
                    passwordMinLength = Convert.ToInt32(dtPolicy.Rows[0]["passwordMinLength"]);
                    passwordMinAlphabets = Convert.ToInt32(dtPolicy.Rows[0]["passwordMinAlphabets"]);
                    passwordMinNumbers = Convert.ToInt32(dtPolicy.Rows[0]["passwordMinNumbers"]);
                    passwordMinSpecialChars = Convert.ToInt32(dtPolicy.Rows[0]["passwordMinSpecialChars"]);
                    rememberPasswordsCount = Convert.ToInt32(dtPolicy.Rows[0]["rememberPasswordsCount"]);
                    if (rememberPasswordsCount <= 0)
                        rememberPasswordsCount = 1;
                    invalidAttemptsBeforeLockout = Convert.ToInt32(dtPolicy.Rows[0]["invalidAttemptsBeforeLockout"]);
                    lockoutDuration = Convert.ToInt32(dtPolicy.Rows[0]["lockoutDuration"]);
                    userSessionTimeout = Convert.ToInt32(dtPolicy.Rows[0]["userSessionTimeout"]);
                    maxUserInactivityDays = Convert.ToInt32(dtPolicy.Rows[0]["maxUserInactivityDays"]);
                    maxDaysToLoginAfterUserCreation = Convert.ToInt32(dtPolicy.Rows[0]["maxDaysToLoginAfterUserCreation"]);
                }
                log.LogMethodExit(null);
            }
        }

        void LoginValidation(string LoginId, string Password, string CardNumber, ref SecurityPolicy sp, ref bool PasswordChangeRequired, ref User userDetails, int SiteId)
        {
            log.LogMethodEntry(LoginId, CardNumber, sp, PasswordChangeRequired, userDetails, SiteId);
            DataTable dtUser = _utilities.executeDataTable(@"select username, u.UserStatus, user_id, ur.SecurityPolicyId, 
                                                                    last_login_time, u.role_id, u.LoginId,
                                                                    u.CreationDate, isnull(PasswordChangeDate, u.CreationDate) PasswordChangeDate,
                                                                    getdate() today, InvalidAccessAttempts, 
                                                                    LockedOutTime, PasswordChangeOnNextLogin,
                                                                    uit.CardNumber, ur.role, ur.manager_flag,
                                                                    u.site_id, ur.manager_flag, u.EmpNumber, u.EmpLastName,
                                                                    ur.EnablePOSClockIn, ur.AllowShiftOpenClose, ur.allow_pos_access,u.Guid,
                                                                    u.PasswordSalt
                                                            from users u left outer join user_roles ur
                                                                            on ur.role_id = u.role_id
                                                                        left outer join UserIdentificationTags uit
                                                                            on uit.UserId = u.user_id
                                                                            and uit.ActiveFlag = 1
                                                                            and getdate() between isnull(uit.startDate, getdate()) and isnull(uit.EndDate + 1, getdate())
                                                                            and (@cardNumber = '' or uit.cardnumber = @cardNumber)
                                                            where (LoginId = @LoginId or (@cardNumber != '' and uit.cardnumber = @cardNumber))
                                                            and (   @siteId = -1
                                                                or ( @siteId != -1 and ISNULL(u.site_id, (select top 1 site_id from site)) = isnull((select top 1 master_site_id from company), (select top 1 site_id from site)))
                                                                or ( ISNULL(u.site_id, (select top 1 site_id from site)) = @siteId))",
                                                           new SqlParameter("@LoginId", LoginId),
                                                           new SqlParameter("@siteId", SiteId),
                                                           new SqlParameter("@cardNumber", CardNumber));
            log.LogVariableState("@LoginId", LoginId);
            log.LogVariableState("@siteId", SiteId);
            log.LogVariableState("@cardNumber", CardNumber);
            if (dtUser.Rows.Count == 0)
            {
                if (string.IsNullOrEmpty(CardNumber))
                {
                    log.LogVariableState("sp", sp);
                    log.LogVariableState("password change required", PasswordChangeRequired);
                    log.LogVariableState("user details", userDetails);
                    log.LogMethodExit(null, "Throwing Security Exception - Invalid login" +LoginId);
                    throw new SecurityException(LoginId, SecurityException.ExInvalidLogin);
                }
                   
                else
                {
                    log.LogVariableState("sp", sp);
                    log.LogVariableState("password change required", PasswordChangeRequired);
                    log.LogVariableState("user details", userDetails);
                    log.LogMethodExit(null, "Throwing Security Exception- Invalid user card" + CardNumber);
                    throw new SecurityException(CardNumber, SecurityException.ExInvalidUserCard);
                }
                    
            }

            if (dtUser.Rows[0]["UserStatus"].ToString().Equals("DISABLED"))
            {
                log.LogVariableState("sp", sp);
                log.LogVariableState("password change required", PasswordChangeRequired);
                log.LogVariableState("user details", userDetails);
                log.LogMethodExit(null, "Throwing Security Exception- user disabled" + LoginId );
                throw new SecurityException(LoginId, SecurityException.ExUserDisabled);
            }
                

            if (dtUser.Rows[0]["UserStatus"].ToString().Equals("INACTIVE"))
            {
                log.LogVariableState("sp", sp);
                log.LogVariableState("password change required", PasswordChangeRequired);
                log.LogVariableState("user details", userDetails);
                log.LogMethodExit(null, "Throwing Security Exception- user inactive" + LoginId);
                throw new SecurityException(LoginId, SecurityException.ExUserInactive);
            }
               

            if (dtUser.Rows[0]["role_id"] == DBNull.Value)
            {
                log.LogVariableState("sp", sp);
                log.LogVariableState("password change required", PasswordChangeRequired);
                log.LogVariableState("user details", userDetails);
                log.LogMethodExit(null, "Throwing Security Exception- role not defined" + LoginId );
                throw new SecurityException(LoginId, SecurityException.ExRoleNotDefined);
            }
                

            int policyId = -1;
            if (dtUser.Rows[0]["SecurityPolicyId"] == DBNull.Value)
            {
                string p = _utilities.getParafaitDefaults("DEFAULT_USER_SECURITY_POLICY");
                if (Int32.TryParse(p, out policyId) == false)
                    policyId = -1;
            }
            else
                policyId = Convert.ToInt32(dtUser.Rows[0]["SecurityPolicyId"]);

            sp = new SecurityPolicy(policyId);

            DateTime now = Convert.ToDateTime(dtUser.Rows[0]["today"]);

            if (dtUser.Rows[0]["UserStatus"].ToString().Equals("LOCKED"))
            {
                if (dtUser.Rows[0]["LockedOutTime"] == DBNull.Value || sp.LockoutDuration > (now - Convert.ToDateTime(dtUser.Rows[0]["LockedOutTime"])).TotalMinutes)
                {
                    log.LogVariableState("sp", sp);
                    log.LogVariableState("password change required", PasswordChangeRequired);
                    log.LogVariableState("user details", userDetails);
                    log.LogMethodExit(null, "Throwing Security Exception- user locked out" + LoginId);
                    throw new SecurityException(LoginId, SecurityException.ExUserLockedOut);
                }
                    
            }

            if (string.IsNullOrEmpty(CardNumber))
            {
                byte[] hashin = EncryptionHASH.CreateHash(Password + dtUser.Rows[0]["PasswordSalt"].ToString(), Semnox.Core.Utilities.StaticUtils.getKey(LoginId.ToLower()));
                log.LogVariableState("@userId", dtUser.Rows[0]["user_id"]);
                log.LogVariableState("@hashin", hashin);
                if (_utilities.executeScalar("select 1 from users where user_id = @userId and PasswordHash = @hashin",
                                            new SqlParameter("@userId", dtUser.Rows[0]["user_id"]),
                                            new SqlParameter("@hashin", hashin)) == null)
                {
                    if (dtUser.Rows[0]["UserStatus"].ToString().Equals("LOCKED")) // locked out user attempting after lock out duration. give him another x chances
                    {
                        _utilities.executeNonQuery("update users set InvalidAccessAttempts = 0, UserStatus = 'ACTIVE' where user_id = @userId", new SqlParameter("@userId", dtUser.Rows[0]["user_id"]));
                    }

                    _utilities.executeNonQuery(@"update users set InvalidAccessAttempts = isnull(InvalidAccessAttempts, 0) + 1 
                                                    where user_id = @userId;
                                                update users set UserStatus = 'LOCKED', LockedOutTime = getdate() 
                                                    where user_id = @userId
                                                    and (InvalidAccessAttempts >= @maxAttempts and @maxAttempts > 0)",
                                                 new SqlParameter("@userId", dtUser.Rows[0]["user_id"]),
                                                 new SqlParameter("@maxAttempts", sp.InvalidAttemptsBeforeLockout));
                    log.LogVariableState("@userId", dtUser.Rows[0]["user_id"]);
                    log.LogVariableState("@maxAttempts", sp.InvalidAttemptsBeforeLockout);

                    log.LogVariableState("sp", sp);
                    log.LogVariableState("password change required", PasswordChangeRequired);
                    log.LogVariableState("user details", userDetails);
                    log.LogMethodExit(null, "Throwing Security Exception- Invalid password" + LoginId);
                    throw new SecurityException(LoginId, SecurityException.ExInvalidPassword);
                }
            }
            else
            {
                if (CardNumber.Equals(dtUser.Rows[0]["CardNumber"].ToString()) == false)
                {
                    log.LogVariableState("sp", sp);
                    log.LogVariableState("password change required", PasswordChangeRequired);
                    log.LogVariableState("user details", userDetails);
                    log.LogMethodExit(null, "Throwing Security Exception- invalid user card" + LoginId);
                    throw new SecurityException(LoginId, SecurityException.ExInvalidUserCard);
                }
                   

                //Modified to accept login ID and card tap. Throw exception only if Login ID is null and card is tapped - 29-Sep-2015
                if (sp.PolicyName.Equals("PA-DSS"))
                {
                    if (string.IsNullOrEmpty(LoginId))
                    {
                        log.LogVariableState("sp", sp);
                        log.LogVariableState("password change required", PasswordChangeRequired);
                        log.LogVariableState("user details", userDetails);
                        log.LogMethodExit(null, "Throwing Security Exception- invalid login" + LoginId);
                        throw new SecurityException(LoginId, SecurityException.ExInvalidLogin);//Modified on 30-Sep-2016
                    }
                      
                    else if (LoginId.Equals(dtUser.Rows[0]["LoginId"].ToString()) == false)
                    {
                        log.LogVariableState("sp", sp);
                        log.LogVariableState("password change required", PasswordChangeRequired);
                        log.LogVariableState("user details", userDetails);
                        log.LogMethodExit(null, "Throwing Security Exception- invalid login" + LoginId);
                        throw new SecurityException(LoginId, SecurityException.ExInvalidLogin);
                    }
                        
                }
                else
                {
                    if (string.IsNullOrEmpty(LoginId) == false && LoginId.Equals(dtUser.Rows[0]["LoginId"].ToString()) == false)
                    {
                        log.LogVariableState("sp", sp);
                        log.LogVariableState("password change required", PasswordChangeRequired);
                        log.LogVariableState("user details", userDetails);
                        log.LogMethodExit(null, "Throwing Security Exception- invalid user card" + LoginId);
                        throw new SecurityException(LoginId, SecurityException.ExInvalidUserCard);
                    }
                       

                }
            }

            PasswordChangeRequired = Convert.ToBoolean(dtUser.Rows[0]["PasswordChangeOnNextLogin"]);
            if (!PasswordChangeRequired && sp.PasswordChangeFrequency != 0)
            {
                if (sp.PasswordChangeFrequency < (now - Convert.ToDateTime(dtUser.Rows[0]["PasswordChangeDate"])).TotalDays)
                    PasswordChangeRequired = true;
            }

            userDetails = new User();
            userDetails.CardNumber = dtUser.Rows[0]["CardNumber"].ToString();
            userDetails.EmpNumber = dtUser.Rows[0]["EmpNumber"].ToString();
            userDetails.LastName = dtUser.Rows[0]["EmpLastName"].ToString();
            userDetails.LoginId = dtUser.Rows[0]["LoginId"].ToString();
            userDetails.ManagerFlag = dtUser.Rows[0]["manager_flag"].ToString().Equals("Y");
            userDetails.RoleId = Convert.ToInt32(dtUser.Rows[0]["role_id"]);
            userDetails.RoleName = dtUser.Rows[0]["role"].ToString();
            userDetails.UserId = Convert.ToInt32(dtUser.Rows[0]["user_id"]);
            if (dtUser.Rows[0]["site_id"] != DBNull.Value)
                userDetails.SiteId = Convert.ToInt32(dtUser.Rows[0]["site_id"]);
            else
                userDetails.SiteId = -1;

            userDetails.UserName = dtUser.Rows[0]["username"].ToString();
            userDetails.EnablePOSClockIn = Convert.ToBoolean(dtUser.Rows[0]["EnablePOSClockIn"]);
            userDetails.AllowShiftOpenClose = Convert.ToBoolean(dtUser.Rows[0]["AllowShiftOpenClose"]);
            userDetails.UserSessionTimeOut = sp.UserSessionTimeout;
            userDetails.PasswordSalt = dtUser.Rows[0]["PasswordSalt"].ToString();
            userDetails.SecurityPolicy = sp.PolicyName;
            userDetails.AllowPOSAccess = dtUser.Rows[0]["allow_pos_access"].ToString();
            userDetails.GUID = dtUser.Rows[0]["Guid"].ToString(); //GUID is required for adding to claims in the secuirtyTokenBL. Manoj-01/Oct/2018

            log.LogVariableState("sp", sp);
            log.LogVariableState("password change required", PasswordChangeRequired);
            log.LogVariableState("user details", userDetails);
            log.LogMethodExit(null);

        }

        internal int GetUserPkId(string siteId, string loginId)
        {
            log.LogMethodEntry();
            int userId = -1;
            string selectQuery = @"select user_id from users
                                   where loginid = @loginid and (site_id = @siteId or @siteId = -1)";
            List<SqlParameter> selectQueryParameters = new List<SqlParameter>();

            selectQueryParameters.Add(new SqlParameter("@loginid", loginId));
            selectQueryParameters.Add(new SqlParameter("@siteId", Convert.ToInt32(siteId)));
            DataTable selectedUser = _utilities.executeDataTable(selectQuery, selectQueryParameters.ToArray());
            if (selectedUser.Rows.Count > 0)
            {
                DataRow formAccessRow = selectedUser.Rows[0];
                userId = Convert.ToInt32(formAccessRow["user_id"].ToString());
            }
            log.LogMethodExit(userId);
            return userId;
        }

        public User Login(string CardNumber, int SiteId = -1)
        {
            log.LogMethodEntry(CardNumber, SiteId);
            securityLogger.LogEvent("Logging in " + CardNumber, "Authentication");

            SecurityPolicy sp = null;
            bool PasswordChangeRequired = false;
            User user = null;
            LoginValidation("", "", CardNumber, ref sp, ref PasswordChangeRequired, ref user, SiteId);

            updateLoginSuccess(user.UserId);

            if (PasswordChangeRequired)
            {
                log.LogMethodExit(null, "Throwing Security Exception- Change password" + CardNumber);
                throw new SecurityException(CardNumber, SecurityException.ExChangePassword);
            }
               

            securityLogger.LogEvent("Logged in " + CardNumber + "/" + user.LoginId, "Authentication", Logger.EventType.Success);
            log.LogMethodExit(user);
            return user;            
        }

        /// <summary>
        /// Get the Form access allowed or not based on the roleId and formName.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="roleId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
		public bool ValidateFormAccess(string formName, string roleId, string siteId)
        {
            log.LogMethodEntry();
            bool access = false;
            string selectQuery = @"select access_allowed from ManagementFormAccess
                                   where role_id = @roleId and form_name = @formName and (site_id = @siteId or @siteId = -1) and isnull(IsActive,1)=1";
            List<SqlParameter> selectQueryParameters = new List<SqlParameter>();
            //selectQueryParameters.Add(new SqlParameter("@mainMenu", ""));
            selectQueryParameters.Add(new SqlParameter("@roleId", Convert.ToInt32(roleId)));
            selectQueryParameters.Add(new SqlParameter("@formName", formName));
            selectQueryParameters.Add(new SqlParameter("@siteId", Convert.ToInt32(siteId)));
            DataTable selectedFormAccess = _utilities.executeDataTable(selectQuery, selectQueryParameters.ToArray());
            if (selectedFormAccess.Rows.Count > 0)
            {
                DataRow formAccessRow = selectedFormAccess.Rows[0];
                string formAccess = formAccessRow["access_allowed"].ToString();
                if (formAccess == "Y")
                {
                    access = true;
                }
            }
            log.LogMethodExit();
            return access;
        }

        /// <summary>
        /// Get the userRoleId based on the loginId and siteId
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>RoleId</returns>
        public string GetUserRoleId(string loginId, string siteId)
        {
            log.LogMethodEntry();
            string roleId = string.Empty;
            string selectQuery = @"select role_id from users
                                   where loginid = @loginid and (site_id = @siteId or @siteId = -1)";
            List<SqlParameter> selectQueryParameters = new List<SqlParameter>();

            selectQueryParameters.Add(new SqlParameter("@loginid", loginId));
            selectQueryParameters.Add(new SqlParameter("@siteId", Convert.ToInt32(siteId)));
            DataTable selectedUserRole = _utilities.executeDataTable(selectQuery, selectQueryParameters.ToArray());
            if (selectedUserRole.Rows.Count > 0)
            {
                DataRow formAccessRow = selectedUserRole.Rows[0];
                roleId = formAccessRow["role_id"].ToString();
            }
            log.LogMethodExit(roleId);
            return roleId;
        }

        //Added override method for Login to allow POS login with LoginId and Card Number - 28-Sep-2015
        public User Login(string LoginId, int SiteId, string CardNumber)
        {
            log.LogMethodEntry(LoginId, SiteId, CardNumber);
            securityLogger.LogEvent("Logging in " + CardNumber + "/" + LoginId, "Authentication");

            SecurityPolicy sp = null;
            bool PasswordChangeRequired = false;
            User user = null;
            LoginValidation(LoginId, "", CardNumber, ref sp, ref PasswordChangeRequired, ref user, SiteId);

            updateLoginSuccess(user.UserId);

            if (PasswordChangeRequired)
            {
                log.LogMethodExit(null, "Throwing Security Exception- Change password" + CardNumber );
                throw new SecurityException(CardNumber, SecurityException.ExChangePassword);
            }
               

            securityLogger.LogEvent("Logged in " + CardNumber + "/" + user.LoginId, "Authentication", Logger.EventType.Success);
            log.LogMethodExit(user);
            return user;
        }
        //End Modification -  override method for Login to allow POS login with LoginId and Card Number - 28-Sep-2015
        
        public User Login(string LoginId, string Password, int SiteId = -1)
        {
            log.LogMethodEntry(LoginId, SiteId);
            securityLogger.LogEvent("Logging in " + LoginId, "Authentication");

            SecurityPolicy sp = null;
            bool PasswordChangeRequired = false;
            User user = null;
            LoginValidation(LoginId, Password, "", ref sp, ref PasswordChangeRequired, ref user, SiteId);

            updateLoginSuccess(user.UserId);

            if (PasswordChangeRequired)
            {
                log.LogMethodExit(null, "Throwing Security Exception- Change password" + LoginId);
                throw new SecurityException(LoginId, SecurityException.ExChangePassword);
            }

            securityLogger.LogEvent("Logged in " + LoginId, "Authentication", Logger.EventType.Success);
            log.LogMethodExit(user);
            return user;
        }

        public Dictionary<string, string> AnonymousLogin(string requestor, string loginToken)
        {
            log.LogMethodEntry(loginToken);
            securityLogger.LogEvent("Logging in " + requestor + " : " + loginToken, "Authentication");
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                Dictionary<string, string> tokenKeyValuePairs = securityTokenBL.ValidateAndParseAnonymousToken(requestor, loginToken);
                log.LogVariableState("key value pairs", tokenKeyValuePairs);
                return tokenKeyValuePairs;
            }
            catch (Exception ex)
            {
                log.LogVariableState("message", ex.Message);
                throw;
            }
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, Microsoft.IdentityModel.Tokens.SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        void updateLoginSuccess(int userId)
        {
            log.LogMethodEntry(userId);
            _utilities.executeNonQuery(@"update users set InvalidAccessAttempts = 0, last_login_time = getdate(),  
                                                UserStatus = 'ACTIVE', logout_time = null
                                            where user_Id = @userId",
                                            new SqlParameter("@userId", userId),
                                            new SqlParameter("@user", _utilities.ParafaitEnv.LoginID));
            log.LogVariableState("@userId", userId);
            log.LogVariableState("@user", _utilities.ParafaitEnv.LoginID);
            log.LogMethodExit(null);
        }

        public byte[] ValidatePassword(string LoginId, string Password, string Salt, int RoleId)
        {
            log.LogMethodEntry();
            DataTable dtUser = _utilities.executeDataTable(@"select ur.SecurityPolicyId
                                                                from user_roles ur
                                                                where ur.role_id = @roleId",
                                                                    new SqlParameter("@roleId", RoleId));

            log.LogVariableState("@roleId", RoleId);
            int policyId = -1;
            if (dtUser.Rows.Count == 0 || dtUser.Rows[0]["SecurityPolicyId"] == DBNull.Value)
            {
                string p = _utilities.getParafaitDefaults("DEFAULT_USER_SECURITY_POLICY");
                if (Int32.TryParse(p, out policyId) == false)
                    policyId = -1;
            }
            else
                policyId = Convert.ToInt32(dtUser.Rows[0]["SecurityPolicyId"]);

            SecurityPolicy sp = new SecurityPolicy(policyId);

            log.LogMethodExit("Validate password");
            return ValidatePassword(LoginId,Password, Salt, sp);
        }

        byte[] ValidatePassword(string LoginId, string Password, string Salt, SecurityPolicy securityPolicy)
        {
            log.LogMethodEntry(LoginId, securityPolicy);
            if (Password.Length < securityPolicy.PasswordMinLength)
            {
                log.LogMethodExit(null, "Throwing Security Exception Minimum length criteria not met -" + LoginId + "  PasswordMinLength: " + securityPolicy.PasswordMinLength);
                throw new SecurityException(LoginId, SecurityException.ExPasswordMinLength, securityPolicy.PasswordMinLength);
            }
                

            if (securityPolicy.PasswordMinAlphabets > 0)
            {
                if (Password.Count(c => Char.IsLetter(c)) < securityPolicy.PasswordMinAlphabets)
                {
                    log.LogMethodExit(null, "Throwing Security Exception Minimum Alphabets criteria not met " + LoginId +"Password minimum Alphabets:" + securityPolicy.PasswordMinAlphabets);
                    throw new SecurityException(LoginId, SecurityException.ExPasswordMinAlpha, securityPolicy.PasswordMinAlphabets);
                }
                    
            }

            if (securityPolicy.PasswordMinNumbers > 0)
            {
                if (Password.Count(c => Char.IsNumber(c)) < securityPolicy.PasswordMinNumbers)
                {
                    log.LogMethodExit(null, "Throwing Security Exception- Minimum Numbers criteria not met" + LoginId +"Paasword Minimum Numbers" + securityPolicy.PasswordMinNumbers);
                    throw new SecurityException(LoginId, SecurityException.ExPasswordMinNumeric, securityPolicy.PasswordMinNumbers);
                }
                    
            }

            if (securityPolicy.PasswordMinSpecialChars > 0)
            {
                if ((Password.Length - Password.Count(c => Char.IsLetter(c)) - Password.Count(c => Char.IsNumber(c))) < securityPolicy.PasswordMinSpecialChars)
                {
                    log.LogMethodExit(null, "Throwing Security Exception-Minimum Special charecters criteria not met" + LoginId +"Password Minimum special charecters:"+ securityPolicy.PasswordMinSpecialChars);
                    throw new SecurityException(LoginId, SecurityException.ExPasswordMinSpecial, securityPolicy.PasswordMinSpecialChars);
                }
                  
            }
           
            if (securityPolicy.RememberPasswordsCount > 1)
            {
                DataTable dtPasswordHistory = _utilities.executeDataTable(@"select top " + (securityPolicy.RememberPasswordsCount - 1).ToString() + " h.PasswordHash, h.PasswordSalt " +
                                                                            @"from UserPasswordHistory h, users u
                                                                            where h.UserId = u.user_id
                                                                            and u.LoginId = @loginId
                                                                           order by changeDate desc",
                                                                            new SqlParameter("@loginId", LoginId));
                log.LogVariableState("@loginId", LoginId);
                foreach (DataRow dr in dtPasswordHistory.Rows)
                {
                    byte[] passwordHash = EncryptionHASH.CreateHash(Password + dr["PasswordSalt"].ToString(), StaticUtils.getKey(LoginId.ToLower()));

                    if (EncryptionHASH.CompareHash(passwordHash, dr["PasswordHash"] as byte[]))
                    {
                        log.LogMethodExit(null, "Throwing Security Exception-Password history match occured" + LoginId +"Remember password count:" + securityPolicy.RememberPasswordsCount);
                        throw new SecurityException(LoginId, SecurityException.ExPasswordHistoryMatch, securityPolicy.RememberPasswordsCount);
                    }
                       
                }
            }

            byte[] pHash = EncryptionHASH.CreateHash(Password + Salt, StaticUtils.getKey(LoginId.ToLower()));
            log.LogMethodExit("pHash");
            return pHash;
        }

        public void ChangePassword(string LoginId, string CurrentPassword, string NewPassword, int SiteId = -1)
        {
            log.LogMethodEntry(LoginId, SiteId);
            securityLogger.LogEvent("Changing password for " + LoginId, "Authentication");
            SecurityPolicy sp = null;
            bool PasswordChangeRequired = false;
            User user = null;

            LoginValidation(LoginId, CurrentPassword, "", ref sp, ref PasswordChangeRequired, ref user, SiteId);

            if (CurrentPassword.Equals(NewPassword))
            {
                log.LogMethodExit(null, "Throwing Security Exception-Password history match Occured" + LoginId + "Remember Password Count"+ sp.RememberPasswordsCount);
                throw new SecurityException(LoginId, SecurityException.ExPasswordHistoryMatch, sp.RememberPasswordsCount);
            }
              
            string salt = _utilities.GenerateRandomCardNumber(10);
            byte[] passwordHash = ValidatePassword(LoginId, NewPassword, salt, sp);
            updateLoginSuccess(user.UserId);

            if (CurrentPassword.Equals(NewPassword))
            {
                log.LogMethodExit(null, "Throwing Security Exception-Password history match Occured" + LoginId + "Remember Password Count" + sp.RememberPasswordsCount);
                throw new SecurityException(LoginId, SecurityException.ExPasswordHistoryMatch, sp.RememberPasswordsCount);
            }
               

            using (SqlConnection cnn = _utilities.createConnection())
            {
                SqlTransaction SQLTrx = cnn.BeginTransaction();
                try
                {
                    _utilities.executeNonQuery(@"insert into UserPasswordHistory 
                                                  (UserId, PasswordHash, PasswordSalt, ChangeDate) 
                                                select user_id, PasswordHash, PasswordSalt, getdate() 
                                                from users
                                                where user_id = @userId",
                                           SQLTrx,
                                           new SqlParameter("@userId", user.UserId));
                    log.LogVariableState("@userId", user.UserId);

                    _utilities.executeNonQuery(@"update users set PasswordHash = @hash, PasswordSalt = @salt, LastUpdatedDate = getdate(), 
                                                    UserStatus = 'ACTIVE', LastUpdatedBy = @user, PasswordChangeOnNextLogin = 0,
                                                    PasswordChangeDate = getdate()
                                                where user_id = @userId",
                                         SQLTrx,
                                         new SqlParameter("@userId", user.UserId),
                                         new SqlParameter("@hash", passwordHash),
                                         new SqlParameter("@salt", salt),
                                         new SqlParameter("@user", _utilities.ParafaitEnv.LoginID));
                    log.LogVariableState("@userId", user.UserId);
                    log.LogVariableState("@hash", passwordHash);
                    log.LogVariableState("@salt", salt);
                    log.LogVariableState("@user", _utilities.ParafaitEnv.LoginID);

                    SQLTrx.Commit();
                    securityLogger.LogEvent("Password changed for " + LoginId, "Authentication", Logger.EventType.Success);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while executing insert and update query", ex);
                    SQLTrx.Rollback();
                    log.LogMethodExit(null, "Throwing exception" + ex);
                    throw ex;
                }
            }
            log.LogMethodExit(null);
        }

        [Serializable]
        public class SecurityException : Exception
        {
            public const int ExInvalidLogin = 807;
            public const int ExInvalidPassword = 808;
            public const int ExUserDisabled = 809;
            public const int ExUserInactive = 810;
            public const int ExUserLockedOut = 811;
            public const int ExChangePassword = 812;
            public const int ExRoleNotDefined = 813;
            public const int ExPasswordMinLength = 814;
            public const int ExPasswordMinAlpha = 815;
            public const int ExPasswordMinNumeric = 816;
            public const int ExPasswordMinSpecial = 817;
            public const int ExPasswordHistoryMatch = 818;
            public const int ExInvalidUserCard = 819;
            public const int ExUserCardNotAllowed = 809;
            public const int ExTimeStampExpired = 807; // need to add message for timestamp expires -  login expired .Please re-login

            public SecurityException(string loginId, int MesssageNo)
                : base(getMessage(loginId, MesssageNo)) 
            {
                log.LogMethodEntry(loginId, MesssageNo);
                base.HResult = MesssageNo;
                log.LogMethodExit(null);
            }

            public SecurityException(string loginId, int MesssageNo, params object[] Params)
                : base(getMessage(loginId, MesssageNo, Params)) 
            {
                log.LogMethodEntry(loginId, MesssageNo, Params);
                base.HResult = MesssageNo;
                log.LogMethodExit(null);
            }

            public SecurityException(string loginId, string message)
                : base(message) 
            {
                log.LogMethodEntry(loginId, message);
                base.HResult = -1;
                _utilities.EventLog.logEvent("SECURITY", Logger.EventType.Failure.ToString()[0], loginId, message, "AUTHENTICATION", 3);
                log.LogMethodExit(null);
            }

            static string getMessage(string loginId, int MessageNo, params object[] Params)
            {
                log.LogMethodEntry(loginId, MessageNo, Params);
                string message = _utilities.MessageUtils.getMessage(MessageNo, Params);
                _utilities.EventLog.logEvent("SECURITY", Logger.EventType.Failure.ToString()[0], loginId, message, "AUTHENTICATION", 3);
                log.LogMethodExit(message);
                return message;
            }
        }
    
        public class User
        {
            public int UserId, RoleId, SiteId;
            public string LoginId, UserName, LastName, RoleName, CardNumber, EmpNumber, PasswordSalt, SecurityPolicy, AllowPOSAccess;
            public string GUID; //GUID is required for adding to claims in the secuirtyTokenBL. Manoj-01/Oct/2018
            public bool ManagerFlag, EnablePOSClockIn, AllowShiftOpenClose;
            public int UserSessionTimeOut;
        }
    }

    /// <summary>
    /// This class has been created for receiving only those attributes for Login controller
    /// Manoj - 02/Oct/2018
    /// </summary>
    /// <summary>
    /// Represents the login request 
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// login Id property
        /// </summary>
        public string LoginId { get; set; }
        
        /// <summary>
        /// password property
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// new password property
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// siteId property
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// MachineName Property
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// MachineName Property
        /// </summary>
        public string LoginToken { get; set; }

        /// <summary>
        /// IPAddress Property
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// TagNumber Property
        /// </summary>
        public string TagNumber { get; set; }

    }
}
