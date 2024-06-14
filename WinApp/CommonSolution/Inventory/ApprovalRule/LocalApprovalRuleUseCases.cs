/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalApprovalRuleUseCases
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
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    public class LocalApprovalRuleUseCases : IApprovalRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalApprovalRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ApprovalRuleDTO>> GetApprovalRules(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0)
        {
            return await Task<List<ApprovalRuleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ApprovalRulesList approvalRuleListBL = new ApprovalRulesList(executionContext);
                List<ApprovalRuleDTO> approvalRuleDTOList = approvalRuleListBL.GetAllApprovalRule(searchParameters, currentPage, pageSize);
                log.LogMethodExit(approvalRuleDTOList);
                return approvalRuleDTOList;
            });
        }

        public async Task<int> GetApprovalRuleCount(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ApprovalRulesList approvalRuleListBL = new ApprovalRulesList(executionContext);
                int approvalRuleCount = approvalRuleListBL.GetApprovalRuleCount(searchParameters);
                log.LogMethodExit(approvalRuleCount);
                return approvalRuleCount;
            });
        }

        public async Task<List<InventoryDocumentTypeDTO>> GetInventoryDocumentTypes(List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> searchParameters)
        {
            return await Task<List<InventoryDocumentTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(searchParameters);
                log.LogMethodExit(inventoryDocumentTypeDTOList);
                return inventoryDocumentTypeDTOList;
            });
        }

        public async Task<string> SaveApprovalRules(List<ApprovalRuleDTO> approvalRuleDTOList)
        {
            log.LogMethodEntry("approvalRuleDTOList");
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (approvalRuleDTOList == null)
                {
                    throw new ValidationException("approvalRuleDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ApprovalRuleDTO approvalRuleDTO in approvalRuleDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ApprovalRule approvalRuleBL = new ApprovalRule(executionContext, approvalRuleDTO);
                            approvalRuleBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }

                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw ;
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
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
                                throw ;
                            }
                        }
                    }
                    result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<InventoryDocumentTypeContainerDTOCollection> GetInventoryDocumentTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<InventoryDocumentTypeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    InventoryDocumentTypeContainerList.Rebuild(siteId);
                }
                InventoryDocumentTypeContainerDTOCollection result = InventoryDocumentTypeContainerList.GetInventoryDocumentTypeContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
