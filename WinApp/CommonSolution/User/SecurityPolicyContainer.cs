/********************************************************************************************
 * Project Name - User
 * Description  - SecurityPolicyContainer class to caches the security policies for faster business processing
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Class holds the security policies.
    /// </summary>
    public class SecurityPolicyContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<int, SecurityPolicyDTO> securityPolicyIdSecurityPolicyDTODictionary = new Dictionary<int, SecurityPolicyDTO>();
        private readonly Dictionary<int, SecurityPolicyDetailsDTO> securityPolicyIdSecurityPolicyDetailsDTODictionary = new Dictionary<int, SecurityPolicyDetailsDTO>();
        private readonly DateTime? securityPolicyModuleLastUpdateTime;
        private readonly int siteId;
        
        internal SecurityPolicyContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            List<SecurityPolicyDTO> securityPolicysDTOList = new List<SecurityPolicyDTO>();
            try
            {
                SecurityPolicyList securityPolicysList = new SecurityPolicyList();
                securityPolicyModuleLastUpdateTime = securityPolicysList.GetSecurityPolicyModuleLastUpdateTime(siteId);

                List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.SITEID, siteId.ToString()));
                securityPolicysDTOList = securityPolicysList.GetAllSecurityPolicy(searchParameters, true);
                if (securityPolicysDTOList != null && securityPolicysDTOList.Any())
                {
                    foreach (SecurityPolicyDTO securityPolicysDTO in securityPolicysDTOList)
                    {
                        if(securityPolicyIdSecurityPolicyDTODictionary.ContainsKey(securityPolicysDTO.PolicyId))
                        {
                            continue;
                        }
                        securityPolicyIdSecurityPolicyDTODictionary.Add(securityPolicysDTO.PolicyId, securityPolicysDTO);
                        if(securityPolicysDTO.SecurityPolicyDTOList != null && 
                            securityPolicysDTO.SecurityPolicyDTOList.Any())
                        {
                            securityPolicyIdSecurityPolicyDetailsDTODictionary.Add(securityPolicysDTO.PolicyId, securityPolicysDTO.SecurityPolicyDTOList.Last());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the security policy container.", ex);
                securityPolicyModuleLastUpdateTime = null;
                securityPolicyIdSecurityPolicyDTODictionary.Clear();
                securityPolicyIdSecurityPolicyDetailsDTODictionary.Clear();
            }
            log.LogMethodExit();
        }

        internal int GetPasswordChangeFrequency(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].PasswordChangeFrequency;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetPasswordMinLength(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].PasswordMinLength;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetPasswordMinAlphabets(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].PasswordMinAlphabets;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetPasswordMinNumbers(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].PasswordMinNumbers;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetPasswordMinSpecialChars(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].PasswordMinSpecialChars;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetRememberPasswordsCount(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 1;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].RememberPasswordsCount;
            }
            if(result <= 0)
            {
                result = 1;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetInvalidAttemptsBeforeLockout(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].InvalidAttemptsBeforeLockout;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetLockoutDuration(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].LockoutDuration;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetUserSessionTimeout(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].UserSessionTimeout;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetMaxUserInactivityDays(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].MaxUserInactivityDays;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetMaxDaysToLoginAfterUserCreation(int policyId)
        {
            log.LogMethodEntry(policyId);
            int result = 0;
            if(securityPolicyIdSecurityPolicyDetailsDTODictionary.ContainsKey(policyId))
            {
                result = securityPolicyIdSecurityPolicyDetailsDTODictionary[policyId].MaxDaysToLoginAfterUserCreation;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal bool GetAuthenticationRequiresTagAndLoginId(int policyId)
        {
            log.LogMethodEntry(policyId);
            bool result = securityPolicyIdSecurityPolicyDTODictionary.ContainsKey(policyId) &&
                securityPolicyIdSecurityPolicyDTODictionary[policyId].PolicyName == "PA-DSS";
            log.LogMethodExit(result);
            return result;
        }

        public SecurityPolicyContainer Refresh()
        {
            log.LogMethodEntry();
            SecurityPolicyList securityPolicysList = new SecurityPolicyList();
            DateTime? updateTime = securityPolicysList.GetSecurityPolicyModuleLastUpdateTime(siteId);
            if (securityPolicyModuleLastUpdateTime.HasValue
                && securityPolicyModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in system option since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            SecurityPolicyContainer result = new SecurityPolicyContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
