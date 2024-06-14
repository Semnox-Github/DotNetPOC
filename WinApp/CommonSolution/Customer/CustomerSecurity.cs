/********************************************************************************************
* Project Name - Customer 
* Description  - Business logic for Customer Security
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.80.00     24-Jun-2020      Indrajeet Kumar     Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    public class CustomerSecurity
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityPolicyDetailsDTO securityPolicyDetailsDTO = null;
        private readonly DataAccessHandler dataAccessHandler;
        private readonly ExecutionContext executionContext;

        public CustomerSecurity(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        public bool Login(int Id, string userPassword, string profilePassword)
        {
            log.LogMethodEntry(Id, userPassword, profilePassword);

            string securityPolicy = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_SECURITY_POLICY");
            ProfileBL profileBL = new ProfileBL(executionContext, Id);
            ProfileDTO ProfileDTO = profileBL.ProfileDTO;
            String LoginId = ProfileDTO.UserName;
            if (!string.IsNullOrEmpty(securityPolicy))
            {
                SecurityPolicyList securityPolicyList = new SecurityPolicyList(executionContext);
                List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.POLICY_NAME, securityPolicy));
                searchParam.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.SITEID, executionContext.GetSiteId().ToString()));
                List<SecurityPolicyDTO> securityPolicyDTOList = securityPolicyList.GetAllSecurityPolicy(searchParam, true);
                if (securityPolicyDTOList != null && securityPolicyDTOList.Any())
                {
                    securityPolicyDetailsDTO = securityPolicyDTOList[0].SecurityPolicyDTOList[0];
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Security Policy Not Found"));
                }

                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime now = serverTimeObject.GetServerDateTime();

                if (ProfileDTO.UserStatus.Equals("DISABLED"))
                {
                    log.LogMethodExit(null, "Throwing Security Exception- user disabled" + Id);
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExUserDisabled));
                }

                if (ProfileDTO.UserStatus.Equals("INACTIVE"))
                {
                    log.LogMethodExit(null, "Throwing Security Exception- user inactive" + Id);
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExUserInactive));
                }

                if (ProfileDTO.UserStatus.Equals("LOCKED"))
                {
                    if (ProfileDTO.LockedOutTime == null || securityPolicyDetailsDTO.LockoutDuration > (now - Convert.ToDateTime(ProfileDTO.LockedOutTime.ToString())).TotalMinutes)
                    {
                        log.LogMethodExit(null, "Throwing Security Exception- user locked out" + LoginId);
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExUserLockedOut));
                    }
                }

                if (!profilePassword.Equals(Encryption.Encrypt(userPassword)))
                {
                    ProfileDTO.InvalidAccessAttempts = ProfileDTO.InvalidAccessAttempts + 1;
                    if(ProfileDTO.InvalidAccessAttempts >= securityPolicyDetailsDTO.InvalidAttemptsBeforeLockout)
                    {
                        ProfileDTO.UserStatus = "LOCKED";
                        ProfileDTO.LockedOutTime = now;
                    }
                    profileBL.SaveSecurityAttributes();

                    //string updateProfileQuery = @"update profile set InvalidAccessAttempts = isnull(InvalidAccessAttempts, 0) + 1 
                    //                            where Id = @Id;
                    //                        update profile set UserStatus = 'LOCKED', LockedOutTime = getdate() 
                    //                            where Id = @Id
                    //                            and (InvalidAccessAttempts >= @maxAttempts and @maxAttempts > 0)";

                    //SqlParameter[] parameters = new SqlParameter[2];
                    //parameters[0] = new SqlParameter("@Id", ProfileDTO.Id);
                    //parameters[1] = new SqlParameter("@maxAttempts", securityPolicyDetailsDTO.InvalidAttemptsBeforeLockout);

                    //dataAccessHandler.executeUpdateQuery(updateProfileQuery, parameters);                    

                    log.LogMethodExit(null, "Throwing Security Exception- Invalid password" + LoginId);
                    return false;
                    //throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExInvalidPassword));                    
                }
                else if (profilePassword.Equals(Encryption.Encrypt(userPassword)))
                {
                    log.LogVariableState("Password Matched", "Password Matched");
                    if (ProfileDTO.UserStatus.Equals("LOCKED"))
                    {
                        ProfileDTO.InvalidAccessAttempts = 0;
                        ProfileDTO.UserStatus = "ACTIVE";
                        ProfileDTO.LockedOutTime = null;
                        ProfileDTO.LastLoginTime = now;
                        profileBL.SaveSecurityAttributes();

                        //string updateProfileQuery = @"update profile set InvalidAccessAttempts = 0, LastLoginTime = getdate(),  
                        //                 UserStatus = 'ACTIVE' where Id = @Id";

                        //SqlParameter[] parameters = new SqlParameter[1];
                        //parameters[0] = new SqlParameter("@Id", ProfileDTO.Id);

                        //dataAccessHandler.executeUpdateQuery(updateProfileQuery, parameters);                      
                    }
                }

                bool PasswordChangeRequired = ProfileDTO.PasswordChangeOnNextLogin;
                if (!PasswordChangeRequired && securityPolicyDetailsDTO.PasswordChangeFrequency != 0)
                {
                    if (securityPolicyDetailsDTO.PasswordChangeFrequency < (now - Convert.ToDateTime(ProfileDTO.PasswordChangeDate == null ? ProfileDTO.CreationDate : ProfileDTO.PasswordChangeDate)).TotalDays)
                        PasswordChangeRequired = true;
                }

                if (PasswordChangeRequired)
                {
                    log.LogMethodExit(null, "Throwing Security Exception- Change password" + LoginId);
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExChangePassword));
                }
                log.LogVariableState("password change required", PasswordChangeRequired);
            }
            else
            {
                if (profilePassword.Equals(Encryption.Encrypt(userPassword)))
                {
                    log.LogVariableState("Password Matched", "Password Matched");
                }
                else
                {
                    log.LogVariableState("Invalid Paasword", "Invalid Paasword");
                    log.Error("Invalid Paasword");
                    return false;
                }
            }

            log.LogMethodExit(null);
            return true;
        }

        public void ValidatePassword(string LoginId, string Password, int profileId)
        {
            log.LogMethodEntry();

            string securityPolicy = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_SECURITY_POLICY");
            if (!string.IsNullOrEmpty(securityPolicy))
            {
                SecurityPolicyList securityPolicyList = new SecurityPolicyList(executionContext);
                List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.POLICY_NAME, securityPolicy));
                searchParam.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.SITEID, executionContext.GetSiteId().ToString()));
                List<SecurityPolicyDTO> securityPolicyDTOList = securityPolicyList.GetAllSecurityPolicy(searchParam, true);
                if (securityPolicyDTOList != null && securityPolicyDTOList.Any())
                {
                    securityPolicyDetailsDTO = securityPolicyDTOList[0].SecurityPolicyDTOList[0];
                    if (Password.Length < securityPolicyDetailsDTO.PasswordMinLength)
                    {
                        log.LogMethodExit(null, "Throwing Security Exception Minimum length criteria not met -" + LoginId + "  PasswordMinLength: " + securityPolicyDetailsDTO.PasswordMinLength);
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExPasswordMinLength, securityPolicyDetailsDTO.PasswordMinLength));
                    }

                    if (securityPolicyDetailsDTO.PasswordMinAlphabets > 0)
                    {
                        if (Password.Count(c => Char.IsLetter(c)) < securityPolicyDetailsDTO.PasswordMinAlphabets)
                        {
                            log.LogMethodExit(null, "Throwing Security Exception Minimum Alphabets criteria not met " + LoginId + "Password minimum Alphabets:" + securityPolicyDetailsDTO.PasswordMinAlphabets);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExPasswordMinAlpha, securityPolicyDetailsDTO.PasswordMinAlphabets));
                        }
                    }

                    if (securityPolicyDetailsDTO.PasswordMinNumbers > 0)
                    {
                        if (Password.Count(c => Char.IsNumber(c)) < securityPolicyDetailsDTO.PasswordMinNumbers)
                        {
                            log.LogMethodExit(null, "Throwing Security Exception- Minimum Numbers criteria not met" + LoginId + "Paasword Minimum Numbers" + securityPolicyDetailsDTO.PasswordMinNumbers);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExPasswordMinNumeric, securityPolicyDetailsDTO.PasswordMinNumbers));
                        }
                    }

                    if (securityPolicyDetailsDTO.PasswordMinSpecialChars > 0)
                    {
                        if ((Password.Length - Password.Count(c => Char.IsLetter(c)) - Password.Count(c => Char.IsNumber(c))) < securityPolicyDetailsDTO.PasswordMinSpecialChars)
                        {
                            log.LogMethodExit(null, "Throwing Security Exception-Minimum Special charecters criteria not met" + LoginId + "Password Minimum special charecters:" + securityPolicyDetailsDTO.PasswordMinSpecialChars);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExPasswordMinSpecial, securityPolicyDetailsDTO.PasswordMinSpecialChars));
                        }
                    }

                    if (securityPolicyDetailsDTO.RememberPasswordsCount > 1)
                    {
                        CustomerPasswordHistoryListBL customerPasswordHistoryListBL = new CustomerPasswordHistoryListBL(executionContext);

                        List<KeyValuePair<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string>> forgetPwsSearchParam = new List<KeyValuePair<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string>>();
                        forgetPwsSearchParam.Add(new KeyValuePair<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string>(CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.PROFILE_ID, profileId.ToString()));
                        forgetPwsSearchParam.Add(new KeyValuePair<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string>(CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<CustomerPasswordHistoryDTO> customerPasswordHistoryDTOList = customerPasswordHistoryListBL.GetCustomerPasswordHistoryDTOList(forgetPwsSearchParam);
                        if (customerPasswordHistoryDTOList != null && customerPasswordHistoryDTOList.Any())
                        {
                            customerPasswordHistoryDTOList = customerPasswordHistoryDTOList.OrderByDescending(x => x.CreationDate).ToList();
                            customerPasswordHistoryDTOList = customerPasswordHistoryDTOList.Take(securityPolicyDetailsDTO.RememberPasswordsCount - 1).ToList();
                            foreach (CustomerPasswordHistoryDTO customerPasswordHistoryDTO in customerPasswordHistoryDTOList)
                            {
                                if (customerPasswordHistoryDTO.Password.ToString().Equals(Encryption.Encrypt(Password)))
                                {
                                    log.LogMethodExit(null, "Throwing Security Exception-Password history match occured" + LoginId + "Remember password count:" + securityPolicyDetailsDTO.RememberPasswordsCount);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, Security.SecurityException.ExPasswordHistoryMatch, securityPolicyDetailsDTO.RememberPasswordsCount));
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }
        }

        public bool IsPasswordHistoryEnabled()
        {
            log.LogMethodEntry();
            bool passwordHistoryEnabled = false;
            if (securityPolicyDetailsDTO != null)
            {
                if (securityPolicyDetailsDTO.RememberPasswordsCount > 1)
                {
                    passwordHistoryEnabled = true;
                }
            }
            else
            {
                string securityPolicy = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_REGISTRATION_SECURITY_POLICY");
                if (!string.IsNullOrEmpty(securityPolicy))
                {
                    SecurityPolicyList securityPolicyList = new SecurityPolicyList(executionContext);
                    List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>>();
                    searchParam.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.POLICY_NAME, securityPolicy));
                    searchParam.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.SITEID, executionContext.GetSiteId().ToString()));
                    List<SecurityPolicyDTO> securityPolicyDTOList = securityPolicyList.GetAllSecurityPolicy(searchParam, true);
                    if (securityPolicyDTOList != null && securityPolicyDTOList.Any())
                    {
                        securityPolicyDetailsDTO = securityPolicyDTOList[0].SecurityPolicyDTOList[0];
                        if (securityPolicyDetailsDTO.RememberPasswordsCount > 1)
                        {
                            passwordHistoryEnabled = true;
                        }
                    }
                    log.LogMethodExit();
                }
            }

            return passwordHistoryEnabled;
        }
    }
}
