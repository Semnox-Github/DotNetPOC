/********************************************************************************************
 * Project Name - Utilities
 * Description  - SecurityPolicyMasterList class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public static class SecurityPolicyMasterList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, SecurityPolicyContainer> securityPolicyContainerDictionary = new ConcurrentDictionary<int, SecurityPolicyContainer>();
        private static Timer refreshTimer;

        static SecurityPolicyMasterList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e) 
        {
            log.LogMethodEntry();
            List<int> uniqueKeyList = securityPolicyContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                SecurityPolicyContainer securityPolicyContainer;
                if (securityPolicyContainerDictionary.TryGetValue(uniqueKey, out securityPolicyContainer))
                {
                    securityPolicyContainerDictionary[uniqueKey] = securityPolicyContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static SecurityPolicyContainer GetSecurityPolicyContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (securityPolicyContainerDictionary.ContainsKey(siteId) == false)
            {
                securityPolicyContainerDictionary[siteId] = CreateSecurityPolicyContainer(siteId);
            }
            SecurityPolicyContainer result = securityPolicyContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        private static SecurityPolicyContainer CreateSecurityPolicyContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            SecurityPolicyContainer result = new SecurityPolicyContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(siteId);
            securityPolicyContainerDictionary[siteId] = securityPolicyContainer.Refresh();
            log.LogMethodExit();
        }

        public static int GetPasswordChangeFrequency(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetPasswordChangeFrequency(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetPasswordMinLength(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetPasswordMinLength(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetPasswordMinAlphabets(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetPasswordMinAlphabets(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetPasswordMinNumbers(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetPasswordMinNumbers(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetPasswordMinSpecialChars(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetPasswordMinSpecialChars(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetRememberPasswordsCount(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetRememberPasswordsCount(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetInvalidAttemptsBeforeLockout(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetInvalidAttemptsBeforeLockout(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetLockoutDuration(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetLockoutDuration(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetUserSessionTimeout(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetUserSessionTimeout(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetMaxUserInactivityDays(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetMaxUserInactivityDays(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static int GetMaxDaysToLoginAfterUserCreation(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            int result = securityPolicyContainer.GetMaxDaysToLoginAfterUserCreation(policyId);
            log.LogMethodExit(result);
            return result;
        }

        public static bool GetAuthenticationRequiresTagAndLoginId(ExecutionContext executionContext, int policyId)
        {
            log.LogMethodEntry(executionContext, policyId);
            SecurityPolicyContainer securityPolicyContainer = GetSecurityPolicyContainer(executionContext.SiteId);
            bool result = securityPolicyContainer.GetAuthenticationRequiresTagAndLoginId(policyId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
