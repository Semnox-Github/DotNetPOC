/********************************************************************************************
 * Project Name - Utilities
 * Description  - Bussiness logic of security policy
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.70        24-Mar-2016   Jagan Mohana          Created 
 *            09-Apr-2019   Mushahid Faizan       Modified SaveUpdateSecurityPolicyList() ,Added LogMethodEntry & LogMethodExit,Removed unused namespaces.
 *                                                Added SQLTransaction in Save method().
 *            30-Jul-2019   Mushahid Faizan       Added Delete in Save() method for Hard Deletion.
 *                                                Added DeleteSecurityPolicy() method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Core.Utilities
{
    public class SecurityPolicyBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityPolicyDTO securityPolicyDTO;
        private ExecutionContext executionContext;

        /// <summary>
        ///Parameterized Constructor having executionContext
        /// </summary>
        public SecurityPolicyBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.securityPolicyDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="securityPolicyDTO"></param>
        public SecurityPolicyBL(ExecutionContext executionContext, SecurityPolicyDTO securityPolicyDTO)
        {
            log.LogMethodEntry(executionContext, securityPolicyDTO);
            this.executionContext = executionContext;
            this.securityPolicyDTO = securityPolicyDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="securityPolicyDTO"></param>
        public SecurityPolicyBL(ExecutionContext executionContext, int securityPolicyId)
        {
            log.LogMethodEntry(executionContext, securityPolicyDTO);
            this.executionContext = executionContext;
            SecurityPolicyDataHandler securityPolicyDataHandler = new SecurityPolicyDataHandler();
            this.securityPolicyDTO = securityPolicyDataHandler.GetSecurityPolicyDTO(securityPolicyId);
            log.LogMethodExit(securityPolicyDTO);
        }
        /// <summary>
        /// Saves the security policy
        /// Checks if the policy id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SecurityPolicyDataHandler securityPolicyDataHandler = new SecurityPolicyDataHandler(sqlTransaction);
            if (securityPolicyDTO.IsActive)
            {
                if (securityPolicyDTO.PolicyId < 0)
                {
                    int policyId = securityPolicyDataHandler.InsertSecurityPolicy(securityPolicyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    securityPolicyDTO.PolicyId = policyId;
                }
                else
                {
                    if (securityPolicyDTO.IsChanged)
                    {
                        securityPolicyDataHandler.UpdateSecurityPolicy(securityPolicyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        securityPolicyDTO.AcceptChanges();
                    }
                }
                if (securityPolicyDTO.SecurityPolicyDTOList != null && securityPolicyDTO.SecurityPolicyDTOList.Count != 0)
                {
                    foreach (SecurityPolicyDetailsDTO securityPolicyDetailsDTO in securityPolicyDTO.SecurityPolicyDTOList)
                    {
                        securityPolicyDetailsDTO.PolicyId = securityPolicyDTO.PolicyId;
                        SecurityPolicyDetailsBL securityPolicyDetailsBL = new SecurityPolicyDetailsBL(executionContext, securityPolicyDetailsDTO);
                        securityPolicyDetailsBL.Save(sqlTransaction);
                    }
                }
            }
            else
            {
                if (securityPolicyDTO.PolicyId >= 0)
                {
                    securityPolicyDataHandler.DeleteSecurityPolicy(securityPolicyDTO.PolicyId);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SecurityPolicyDTO getSecurityPolicyDTO { get { return securityPolicyDTO; } }
    }
    /// <summary>
    /// Manages the list of Security Policy
    /// </summary>
    public class SecurityPolicyList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SecurityPolicyDTO> securityPolicyDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SecurityPolicyList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public SecurityPolicyList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.securityPolicyDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="securityPolicyDTOList"></param>
        public SecurityPolicyList(ExecutionContext executionContext, List<SecurityPolicyDTO> securityPolicyDTOList)
        {
            log.LogMethodEntry(executionContext, securityPolicyDTOList);
            this.securityPolicyDTOList = securityPolicyDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the security policy list
        /// </summary>
        public List<SecurityPolicyDTO> GetAllSecurityPolicy(List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false)
        {
            log.LogMethodEntry(searchParameters);
            SecurityPolicyDataHandler securityPolicyDataHandler = new SecurityPolicyDataHandler();
            List<SecurityPolicyDTO> securityPolicyDTOList = securityPolicyDataHandler.GetAllSecurityPolicy(searchParameters);
            if (securityPolicyDTOList != null && securityPolicyDTOList.Count != 0 && loadChildRecords)
            {
                foreach (SecurityPolicyDTO securityPolicyDTO in securityPolicyDTOList)
                {
                    List<KeyValuePair<SecurityPolicyDetailsDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<SecurityPolicyDetailsDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<SecurityPolicyDetailsDTO.SearchByParameters, string>(SecurityPolicyDetailsDTO.SearchByParameters.POLICY_ID, Convert.ToString(securityPolicyDTO.PolicyId)));

                    SecurityPolicyDetailsList securityPolicyDetailsList = new SecurityPolicyDetailsList(executionContext);
                    securityPolicyDTO.SecurityPolicyDTOList = securityPolicyDetailsList.GetAllSecurityPolicyDetails(searchByParameters);
                }
            }
            log.LogMethodExit(securityPolicyDTOList);
            return securityPolicyDTOList;
        }

        public DateTime? GetSecurityPolicyModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            SecurityPolicyDataHandler securityPolicyDataHandler = new SecurityPolicyDataHandler();
            DateTime? result = securityPolicyDataHandler.GetSecurityPolicyModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///This method should be used to Save and Update the Security Policy details for Web Management Studio.
        /// </summary>
        public void SaveUpdateSecurityPolicyList()
        {
            log.LogMethodEntry();
            if (securityPolicyDTOList != null && securityPolicyDTOList.Any())
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (SecurityPolicyDTO securityPolicyDto in securityPolicyDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (securityPolicyDto.IsActive == false)
                            {
                                if (securityPolicyDto.SecurityPolicyDTOList != null && securityPolicyDto.SecurityPolicyDTOList.Count != 0)
                                {
                                    foreach (SecurityPolicyDetailsDTO securityPolicyDetailsDTO in securityPolicyDto.SecurityPolicyDTOList)
                                    {
                                        securityPolicyDetailsDTO.IsActive = false;
                                        SecurityPolicyDetailsBL securityPolicyDetailsBL = new SecurityPolicyDetailsBL(executionContext, securityPolicyDetailsDTO);
                                        securityPolicyDetailsBL.Save(parafaitDBTrx.SQLTrx);
                                    }
                                }
                            }
                            SecurityPolicyBL securityPolicyBL = new SecurityPolicyBL(executionContext, securityPolicyDto);
                            securityPolicyBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            log.Error(ex);                            
                            if(ex.Number == 547)
                            {
                                throw new ValidationException("Unable to delete this record.Please check the reference record first.");
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}