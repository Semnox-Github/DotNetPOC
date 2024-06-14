/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteApprovalRuleUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    14-Oct-2020   Mushahid Faizan Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class RemoteApprovalRuleUseCases : RemoteUseCases, IApprovalRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string APPROVALRULE_URL = "api/Inventory/ApprovalRules";
        private const string INVENTORYDOCUMENTTYPE_URL = "api/Inventory/InventoryDocumentTypes";
        private const string APPROVAL_RULE_COUNT_URL = "api/Inventory/ApprovalRuleCounts";
        private const string INVENTORYDOCUMENTTYPE_CONTAINER_URL = "api/Inventory/InventoryDocumentTypeContainer";

        public RemoteApprovalRuleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ApprovalRuleDTO>> GetApprovalRules(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>>
                          parameters, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ApprovalRuleDTO> result = await Get<List<ApprovalRuleDTO>>(APPROVALRULE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetApprovalRuleCount(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(APPROVAL_RULE_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<InventoryDocumentTypeDTO>> GetInventoryDocumentTypes(List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildInventoryDocumentTypeSearchParameter(parameters));
            }
            try
            {
                List<InventoryDocumentTypeDTO> result = await Get<List<InventoryDocumentTypeDTO>>(INVENTORYDOCUMENTTYPE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildInventoryDocumentTypeSearchParameter(List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("applicability".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("code".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.DOCUMENT_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("documentTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("documentName".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }


        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ApprovalRuleDTO.SearchByApprovalRuleParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApprovalRuleDTO.SearchByApprovalRuleParameters.APPROVAL_RULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("approvalRuleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApprovalRuleDTO.SearchByApprovalRuleParameters.DOCUMENT_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("documentTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApprovalRuleDTO.SearchByApprovalRuleParameters.ROLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("roleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApprovalRuleDTO.SearchByApprovalRuleParameters.NUMBER_OF_APPROVAL_LEVELS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("numberOfApprovalLevels".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveApprovalRules(List<ApprovalRuleDTO> approvalRuleDTOList)
        {
            log.LogMethodEntry(approvalRuleDTOList);
            try
            {
                string responseString = await Post<string>(APPROVALRULE_URL, approvalRuleDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<InventoryDocumentTypeContainerDTOCollection> GetInventoryDocumentTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            InventoryDocumentTypeContainerDTOCollection result = await Get<InventoryDocumentTypeContainerDTOCollection>(INVENTORYDOCUMENTTYPE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
