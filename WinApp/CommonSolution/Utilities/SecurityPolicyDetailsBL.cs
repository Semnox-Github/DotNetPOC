/********************************************************************************************
 * Project Name - Utilities
 * Description  - Bussiness logic of Security Policy Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.70        24-Mar-2016   Jagan Mohana          Created 
              09-Apr-2019   Mushahid Faizan       Added LogMethodEntry & LogMethodExit,Removed unused namespaces.
                                                  Added SQLTransaction & DeleteSecurityPolicyDetails() method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.Utilities
{
    public class SecurityPolicyDetailsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityPolicyDetailsDTO securityPolicyDetailsDTO;
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="securityPolicyDTO"></param>
        public SecurityPolicyDetailsBL(ExecutionContext executionContext, SecurityPolicyDetailsDTO securityPolicyDTO)
        {
            log.LogMethodEntry(executionContext, securityPolicyDTO);
            this.executionContext = executionContext;
            this.securityPolicyDetailsDTO = securityPolicyDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the security policy details
        /// Checks if the policy detail id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SecurityPolicyDetailsDataHandler securityPolicyDetailsDataHandler = new SecurityPolicyDetailsDataHandler(sqlTransaction);
            if (securityPolicyDetailsDTO.IsActive)
            {
                if (securityPolicyDetailsDTO.PolicyDetailId < 0)
                {
                    int policyId = securityPolicyDetailsDataHandler.InsertSecurityPolicyDetails(securityPolicyDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    securityPolicyDetailsDTO.PolicyDetailId = policyId;
                }
                else
                {
                    if (securityPolicyDetailsDTO.IsChanged)
                    {
                        securityPolicyDetailsDataHandler.UpdateSecurityPolicyDetails(securityPolicyDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        securityPolicyDetailsDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if(securityPolicyDetailsDTO.PolicyDetailId >= 0)
                {
                    securityPolicyDetailsDataHandler.DeleteSecurityPolicyDetails(securityPolicyDetailsDTO.PolicyDetailId);
                }
            }
            log.LogMethodExit();
        }       
    }
    /// <summary>
    /// Manages the list of SecurityPolicyDetails
    /// </summary>
    public class SecurityPolicyDetailsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SecurityPolicyDetailsDTO> securityPolicyDetailsDTOList;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public SecurityPolicyDetailsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.securityPolicyDetailsDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the security policy list
        /// </summary>
        public List<SecurityPolicyDetailsDTO> GetAllSecurityPolicyDetails(List<KeyValuePair<SecurityPolicyDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            SecurityPolicyDetailsDataHandler securityPolicyDetailsDataHandler = new SecurityPolicyDetailsDataHandler();
            log.LogMethodExit();
            return securityPolicyDetailsDataHandler.GetAllSecurityPolicyDetails(searchParameters);
        }
    }
}