/********************************************************************************************
 * Project Name - UserAccount
 * Description  - UserAccount object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        30-Jun-2016   Rakshith          Created 
 *2.70.2      20-Sept-2019  Girish Kundar     Modified: Passed SQLTransaction object for methods. 
 *2.70.2      01-Jan-2020   Jeevan            Modification: ForgotPassword token method GenerateJWT Token used and saved 
 *2.150.6     01-Dec-2023   Nitin             Modification: Customer security token changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Site;
using System.Data.SqlClient;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// UserAccount Class
    /// </summary>
    public class UserAccount
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private ExecutionContext executionContext;
        private ParafaitDBTransaction parafaitDBTrx;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserAccount()
        {
            log.LogMethodEntry();
            string connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            utilities = new Utilities(connstring);
            log.LogMethodExit();
        }

        public UserAccount(ExecutionContext executionContext) : this()
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// ValidateLogin used to validate Agent
        /// </summary>
        /// <param name="userAuthParams">UserAuthParams</param>
        /// <returns>returns List of KeyValueStruct </returns>
        public List<CoreKeyValueStruct> ValidateLogin(UserAuthParams userAuthParams , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<CoreKeyValueStruct> vaildateKeyStruct = new List<CoreKeyValueStruct>();
            try
            {
                if (userAuthParams.LoginId == null || userAuthParams.PasswordPassed == null ||
                    userAuthParams.LoginId == "" || userAuthParams.PasswordPassed == "")
                {
                    vaildateKeyStruct.Add(new CoreKeyValueStruct("FAILED", "Invalid Login Id / Password. Please retry."));
                }
                else
                {
                    List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameterAgent = new List<KeyValuePair<AgentsDTO.SearchByParameters, string>>();
                    AgentsList agentsList = new AgentsList(executionContext);

                    UsersDataHandler usersDatahandler = new UsersDataHandler(Users.GetUsersPassPhrase(), sqlTransaction);
                    List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, userAuthParams.LoginId));
                    List<UsersDTO> userDTOList = usersDatahandler.GetUsersList(searchParameter);

                    if (userDTOList.Count == 1)
                    {
                        if (userDTOList[0].IsActive == false)
                        {
                            throw new Exception("Login Account is Not Active.");
                        }

                        searchParameterAgent.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.USER_ID, userDTOList[0].UserId.ToString()));
                        List<AgentsDTO> agentDTOList = agentsList.GetAllAgentsList(searchParameterAgent);
                        if (agentDTOList.Count == 1)
                        {
                            if (agentDTOList[0].Active == false)
                            {
                                throw new Exception(" Login Account is Not Active.");
                            }
                        }
                        else
                        {
                            throw new Exception("Login is Invalid");
                        }
                    }
                    else
                    {
                        throw new Exception("Login is Invalid.");
                    }

                    Security security = new Security(utilities);
                    Security.User user = security.Login(userAuthParams.LoginId, userAuthParams.PasswordPassed);

                    if (user.UserId > 0)
                    {
                        searchParameterAgent = new List<KeyValuePair<AgentsDTO.SearchByParameters, string>>();
                        searchParameterAgent.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.USER_ID, user.UserId.ToString()));

                        List<AgentsDTO> agentDTOList = agentsList.GetAllAgentsList(searchParameterAgent);

                        if (agentDTOList.Count > 0)
                        {
                            if (agentDTOList[0].Active == false)
                            {
                                throw new Exception(" Login Account is Not Active.");
                            }
                            else
                            {
                                vaildateKeyStruct.Add(new CoreKeyValueStruct("SUCCESS", userAuthParams.LoginId));
                                vaildateKeyStruct.Add(new CoreKeyValueStruct("UserId", agentDTOList[0].User_Id.ToString()));
                                vaildateKeyStruct.Add(new CoreKeyValueStruct("LoginUserName", user.UserName));
                                vaildateKeyStruct.Add(new CoreKeyValueStruct("Phone", agentDTOList[0].MobileNo.ToString()));
                                vaildateKeyStruct.Add(new CoreKeyValueStruct("SiteId", user.SiteId.ToString()));
                                vaildateKeyStruct.Add(new CoreKeyValueStruct("AgentGuid", agentDTOList[0].Guid.ToString()));
                                vaildateKeyStruct.Add(new CoreKeyValueStruct("AgentRole", user.RoleId.ToString()));
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Login.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                vaildateKeyStruct.Add(new CoreKeyValueStruct("ERROR", ex.Message.ToString()));
                throw new Exception(ex.Message.ToString());
            }
            log.LogMethodExit();
            return vaildateKeyStruct;
        }


        /// <summary>
        /// ChangePassword() calling Security class of ParafitUtil class 
        /// </summary>
        /// <param name="userAuthParams">UserAuthParams</param>
        /// <param name="message">message</param>
        /// <returns>returns bool and string message as reference</returns>
        public bool ChangePassword(UserAuthParams userAuthParams, ref string message)
        {
            log.LogMethodEntry();
            try
            {
                if (userAuthParams.LoginId == null || userAuthParams.PasswordPassed == null || userAuthParams.NewPassword == null ||
                    userAuthParams.LoginId == "" || userAuthParams.PasswordPassed == "" || userAuthParams.NewPassword == "")
                {
                    message = "Invalid Login Id / Password(s). Please retry.";
                }
                else
                {
                    Security security = new Security(utilities);
                    security.ChangePassword(userAuthParams.LoginId, userAuthParams.PasswordPassed, userAuthParams.NewPassword, -1);
                    message = "Password changed successfully";
                    log.Debug("Ends- ChangePassword((UserAuthParams userAuthParams, ref  string message) method.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Forgot Password Method
        /// </summary>
        /// <param name="userAuthParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public void ForgotPassword(UserAuthParams userAuthParams, SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(userAuthParams);

            try
            {
                if (userAuthParams != null)
                {
                    if (string.IsNullOrWhiteSpace(userAuthParams.LoginId))
                    {
                        throw new Exception("Please provide LoginId");
                    }
                    else if (string.IsNullOrWhiteSpace(userAuthParams.Email))
                    {
                        throw new Exception("Please provide Email");
                    }
                }
                else
                {
                    throw new Exception("Invalid Empty Parameters");
                }

                string newTempPassword = Guid.NewGuid().ToString().Substring(0, 6);
                Security sec = new Security(utilities);

                string salt = utilities.GenerateRandomCardNumber(10);

                UsersDataHandler usersDataHandler = new UsersDataHandler(Users.GetUsersPassPhrase(), sqlTransaction);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, userAuthParams.LoginId));
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.EMAIL, userAuthParams.Email));

                List<UsersDTO> userDTOList = usersDataHandler.GetUsersList(searchParameter);
                if (userDTOList == null || userDTOList.Count == 0)
                {
                    throw new Exception("LoginId / Email does not exist");
                }
                else if (userDTOList != null && userDTOList.Count == 1)
                {
                    foreach (UsersDTO userDTO in userDTOList)
                    {
                        int roleId = userDTO.RoleId;
                        Security security = new Security(utilities);
                        byte[] binaryHash = security.ValidatePassword(userAuthParams.LoginId, newTempPassword, salt, roleId);

                        // Update New Password
                        //int idOfRowUpdated = new UserAccountDatahandler().UpdateUserPassword(userDTO.UserId, binaryHash, salt);

                        userDTO.PasswordHash = binaryHash;
                        userDTO.PasswordSalt = salt;

                        // BEGIN Added for Site DB Web compatability changes to avoid updating the site Id when context is not set
                        if (userDTO.SiteId != -1)
                        {
                            int siteId = userDTO.SiteId;
                            executionContext.SetSiteId(siteId);
                            if (new SiteList(executionContext).GetAllSites(null).Count > 1)
                            {
                                executionContext.SetIsCorporate(true);
                            }
                        }
                        // EOF Added for Site DB Web compatability changes

                        new Users(executionContext, userDTO).Save();

                        //Get Email template with reset password link
                        EmailTemplateDTO emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONLINE_PASSWORD_RESET_EMAIL_TEMPLATE"), userDTO.SiteId);

                        string emailContent = "";
                        string emailSubject = "Your Online account";
                        if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                        {
                            emailContent = emailTemplateDTO.EmailTemplate;
                            emailSubject = emailTemplateDTO.Description;
                            SecurityTokenBL securityTokenBL = new SecurityTokenBL(executionContext);
                            string token = userDTO.Guid.ToString().Replace("-", "").Substring(0, 10) + System.Guid.NewGuid().ToString().Replace("-", "") + DateTime.Now.Ticks.ToString();
                            securityTokenBL.GenerateToken(userDTO.Guid, "Users", token.ToLower());

                            string securityTokenLink = "agents/passwordreset/" + userDTO.UserId + "/" + securityTokenBL.GetSecurityTokenDTO.Token;
                            emailContent = emailContent.Replace("@passwordResetTokenLink", securityTokenLink);
                        }
                        else
                        {
                            emailContent = " Hi " + userDTO.UserName + Environment.NewLine + Environment.NewLine +
                            "<p>Password Reset Token could not be sent -  Email Template Missing </p>" + Environment.NewLine + Environment.NewLine +
                            "Thank you";
                        }
                        parafaitDBTrx = new ParafaitDBTransaction();
                        using (Utilities parafaitUtility = new Utilities(parafaitDBTrx.GetConnection()))
                        {
                            parafaitUtility.ParafaitEnv.SiteId = executionContext.GetSiteId();
                            parafaitUtility.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                            new Core.GenericUtilities.SendEmailUI(userDTO.Email, "", userDTO.Email, emailSubject, emailContent, "", "", true, parafaitUtility);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw ex;
            }
            finally
            {
                parafaitDBTrx.Dispose();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="userAuthParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public void ResetPassword(UserAuthParams userAuthParams , SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(userAuthParams);
                if (userAuthParams != null)
                {
                    if (!string.IsNullOrWhiteSpace(userAuthParams.Token))
                    {
                        SecurityTokenBL securityTokenBL = new SecurityTokenBL(executionContext);
                        int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "JWT_TOKEN_LIFE_TIME", 0);
                        if (!securityTokenBL.ValidateAndUpdateToken(userAuthParams.Token, "", false, null, null, tokenLifeTime))
                        {
                            throw new Exception("Invalid Token");
                        }
                    }
                    if (string.IsNullOrWhiteSpace(userAuthParams.NewPassword))
                    {
                        throw new Exception("Please provide New Password");
                    }
                }
                else
                {
                    throw new Exception("Invalid Empty Parameters");
                }

                // Do passsword secuity validation here 
                UsersDataHandler usersDataHandler = new UsersDataHandler(Users.GetUsersPassPhrase(), sqlTransaction);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, userAuthParams.UserID.ToString()));

                List<UsersDTO> userDTOList = usersDataHandler.GetUsersList(searchParameter);
                if (userDTOList == null || userDTOList.Count == 0)
                {
                    throw new Exception("Invalid User");
                }
                else if (userDTOList != null && userDTOList.Count == 1)
                {
                    string salt = utilities.GenerateRandomCardNumber(10);

                    UsersDTO userDTO = new UsersDTO();
                    userDTO = userDTOList[0];
                    int roleId = userDTO.RoleId;
                    Security security = new Security(utilities);
                    byte[] binaryHash = security.ValidatePassword(userDTO.LoginId, userAuthParams.NewPassword, salt, roleId);

                    // Update New Password
                    //int idOfRowUpdated = new UserAccountDatahandler().UpdateUserPassword(userDTO.UserId, binaryHash, salt);

                    userDTO.PasswordHash = binaryHash;
                    userDTO.PasswordSalt = salt;

                    // BEGIN Added for Site DB Web compatability changes to avoid updating the site Id when context is not set
                    if (userDTO.SiteId != -1)
                    {
                        int siteId = userDTO.SiteId;
                        executionContext.SetSiteId(siteId);
                        if (new SiteList(executionContext).GetAllSites(null).Count > 1)
                        {
                            executionContext.SetIsCorporate(true);
                        }
                    }


                    // EOF Added for Site DB Web compatability changes

                    Users users = new Users(executionContext, userDTO);
                    users.Save();
                    SecurityTokenBL securityTokenBL = new SecurityTokenBL(executionContext);
                    securityTokenBL.Cleartoken(userAuthParams.Token);
                }
                log.LogMethodExit(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex);
                throw ex;
            }
        }

        /// <summary>
        /// GetCardBalance
        /// </summary>
        /// <param name="agentId">agentId</param>
        /// <returns>returns double value</returns>
        public double GetCardBalance(int agentId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(agentId);

            int userid = -1;
            double credits = 0;
            AgentsList agentsList = new AgentsList(executionContext);
            List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchAgentParameter = new List<KeyValuePair<AgentsDTO.SearchByParameters, string>>();
            searchAgentParameter.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.AGENT_ID, agentId.ToString()));
            List<AgentsDTO> agentDTOList = agentsList.GetAllAgentsList(searchAgentParameter);

            if (agentDTOList.Count == 1)
            {
                foreach (AgentsDTO agentsDTO in agentDTOList)
                {
                    userid = agentsDTO.User_Id;
                }

                UsersDataHandler usersDatahandler = new UsersDataHandler(Users.GetUsersPassPhrase(), sqlTransaction);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, userid.ToString()));
                List<UsersDTO> userDTOList = usersDatahandler.GetUsersList(searchParameter);

                if (userDTOList.Count == 1)
                {
                    return new UserAccountDatahandler().GetCardBalance(userDTOList[0].CardNumber);
                }
                log.LogMethodExit(credits);
                return credits;
            }
            else
            {
                log.Error("Invalid Agent Id");
                throw new Exception("Invalid Agent Id");
            }
        }
    }
}
