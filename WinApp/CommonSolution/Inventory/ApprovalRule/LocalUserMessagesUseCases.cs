/********************************************************************************************
 * Project Name - UserMessages
 * Description  - LocalUserMessagesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     01-Jan-2021       Prajwal S                Created : POS UI Redesign with REST API
*2.110.0    18-Jan-2021       Mushahid Faizan           Web Inventory Changes with REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    class LocalUserMessagesUseCases : IUserMessagesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalUserMessagesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<UserMessagesDTO>> GetUserMessages(int roleId, int userId, string moduleType, string loginId, int siteId, string status, bool buildPendingApprovalUserMessage = false,
                                                                 SqlTransaction sqlTransaction = null)
        {
            return await Task<List<UserMessagesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(roleId, userId, moduleType, loginId, siteId, status);

                UserMessagesList userMessagesList = new UserMessagesList(executionContext);
                List<UserMessagesDTO> userMessagesDTOList = new List<UserMessagesDTO>();

                if (buildPendingApprovalUserMessage)
                {
                    userMessagesDTOList = userMessagesList.GetAllMyPendingApprovalUserMessage(roleId, moduleType, siteId, sqlTransaction);
                }
                else
                {
                    userMessagesDTOList = userMessagesList.GetHistoryUserMessage(roleId, userId, moduleType, loginId, siteId, status, sqlTransaction);
                }
                log.LogMethodExit(userMessagesDTOList);
                return userMessagesDTOList;
            });
        }
        public async Task<string> SaveUserMessages(List<UserMessagesDTO> userMessagesDTOList, SqlTransaction sqlTransaction = null)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;

                try
                {
                    UserMessages userMessages = new UserMessages(executionContext);
                    InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(executionContext);

                    RequisitionDTO requisitionDTO = new RequisitionDTO();
                    RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionDTO);

                    PurchaseOrder purchaseOrder = new PurchaseOrder(executionContext);

                    if (userMessagesDTOList != null && userMessagesDTOList.Any())
                    {
                        foreach (UserMessagesDTO userMessagesDTO in userMessagesDTOList)
                        {
                            int documentTypeId = -1;
                            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                            InventoryDocumentTypeDTO inventoryDocumentTypeDTO = new InventoryDocumentTypeDTO();

                            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
                            UserMessagesDTO userMessagesDTOForSave = userMessagesDataHandler.GetUserMessages(userMessagesDTO.MessageId);
                            if (userMessagesDTOForSave == null)
                            {
                                log.Error("userMessagesDTOForSave : " + userMessagesDTOForSave);
                                throw new Exception();
                            }
                            if (userMessagesDTO.ModuleType.Equals("Inventory"))
                            {
                                inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                                inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(userMessagesDTO.ObjectType, executionContext.GetSiteId(), null);
                                if (inventoryDocumentTypeDTO == null)
                                {
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1545));
                                }
                                else
                                {
                                    documentTypeId = inventoryDocumentTypeDTO.DocumentTypeId;
                                }
                            }
                            if (userMessagesDTO.ModuleType.Equals("Inventory") &&
                                (userMessagesDTO.ObjectType.Equals("AJIS") || userMessagesDTO.ObjectType.Equals("DIIS")
                                || userMessagesDTO.ObjectType.Equals("REIS") || userMessagesDTO.ObjectType.Equals("STIS")
                                || userMessagesDTO.ObjectType.Equals("ITIS")))
                            {
                                if (userMessagesDTO.Status.ToUpper() == "APPROVED")
                                {
                                    userMessages.UpdateStatus(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.APPROVED, documentTypeId, executionContext.GetUserPKId(), sqlTransaction);

                                    inventoryIssueHeader = new InventoryIssueHeader(userMessagesDTOForSave.ObjectGUID, executionContext);
                                    result = inventoryIssueHeader.ProcessRequests(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.APPROVED, GetUtility(), sqlTransaction);
                                }
                                else if (userMessagesDTO.Status.ToUpper() == "REJECTED")
                                {
                                    userMessages.UpdateStatus(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.REJECTED, documentTypeId, executionContext.GetUserPKId(), sqlTransaction);
                                    result = inventoryIssueHeader.ProcessRequests(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.REJECTED, GetUtility(), sqlTransaction);
                                }
                            }
                            if (userMessagesDTO.ModuleType.Equals("Inventory") &&
                                (userMessagesDTO.ObjectType.Equals("RGPO") || userMessagesDTO.ObjectType.Equals("CPPO")))
                            {
                                purchaseOrder = new PurchaseOrder(userMessagesDTOForSave.ObjectGUID, executionContext);
                                if (userMessagesDTO.Status.ToUpper() == "APPROVED")
                                {
                                    userMessages.UpdateStatus(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.APPROVED, documentTypeId, executionContext.GetUserPKId(), sqlTransaction);
                                    if(userMessagesDTOForSave.Level > 0)
                                    {
                                        purchaseOrder.ProcessRequests(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.APPROVED, GetUtility(), sqlTransaction);
                                    }
                                }
                                else if (userMessagesDTO.Status.ToUpper() == "REJECTED")
                                {
                                    userMessages.UpdateStatus(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.REJECTED, documentTypeId, executionContext.GetUserPKId(), sqlTransaction);
                                    if (userMessagesDTOForSave.Level > 0)
                                    {
                                        purchaseOrder.ProcessRequests(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.REJECTED, GetUtility(), sqlTransaction);
                                    }
                                }
                            }
                            if (userMessagesDTO.ModuleType.Equals("Inventory") &&
                                (userMessagesDTO.ObjectType.Equals("ISRQ") || userMessagesDTO.ObjectType.Equals("PURQ")
                                || userMessagesDTO.ObjectType.Equals("MLRQ") || userMessagesDTO.ObjectType.Equals("ITRQ")))
                            {
                                requisitionBL = new RequisitionBL(executionContext, userMessagesDTOForSave.ObjectGUID);
                                if (userMessagesDTO.Status.ToUpper() == "APPROVED")
                                {
                                    userMessages.UpdateStatus(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.APPROVED, documentTypeId, executionContext.GetUserPKId(), sqlTransaction);
                                    requisitionBL.ProcessRequests(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.APPROVED, GetUtility(), sqlTransaction);
                                }
                                else if (userMessagesDTO.Status.ToUpper() == "REJECTED")
                                {
                                    userMessages.UpdateStatus(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.REJECTED, documentTypeId, executionContext.GetUserPKId(), sqlTransaction);
                                    requisitionBL.ProcessRequests(userMessagesDTOForSave, UserMessagesDTO.UserMessagesStatus.REJECTED, GetUtility(), sqlTransaction);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw ex;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<int> GetUserMessagesCount(int roleId, int userId, string moduleType, string loginId, int siteId, string status,
                                                SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(roleId, userId, moduleType, loginId, siteId, status, sqlTransaction);
                UserMessagesList userMessagesListBL = new UserMessagesList(executionContext);
                int userMessagesCount = userMessagesListBL.GetUserMessagesCount(roleId, moduleType, siteId, sqlTransaction);
                log.LogMethodExit(userMessagesCount);
                return userMessagesCount;
            });
        }

        private Utilities GetUtility()
        {
            Utilities Utilities = new Utilities();
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
            Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            Utilities.ParafaitEnv.Initialize();
            return Utilities;
        }

    }
}


