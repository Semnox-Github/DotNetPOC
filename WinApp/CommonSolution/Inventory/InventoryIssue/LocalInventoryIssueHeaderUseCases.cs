/********************************************************************************************
 * Project Name - InventoryIssueHeader
 * Description  - LocalInventoryIssueHeaderUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     28-Dec-2020       Prajwal S                Created : POS UI Redesign with REST API
*2.110.1     01-Mar-2021      Mushahid Faizan          Modified : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Inventory
{
    class LocalInventoryIssueHeaderUseCases : IInventoryIssueHeaderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalInventoryIssueHeaderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<InventoryIssueHeaderDTO>> GetInventoryIssueHeaders(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0, bool buildChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<InventoryIssueHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                InventoryIssueHeaderList inventoryIssueHeaderList = new InventoryIssueHeaderList(executionContext);
                List<InventoryIssueHeaderDTO> InventoryIssueHeaderDTOList = inventoryIssueHeaderList.GetAllInventoryIssueHeader(searchParameters, currentPage, pageSize, buildChildRecords, loadActiveChild);

                log.LogMethodExit(InventoryIssueHeaderDTOList);
                return InventoryIssueHeaderDTOList;
            });
        }

        public async Task<int> GetInventoryIssueCount(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                InventoryIssueHeaderList inventoryIssueHeaderList = new InventoryIssueHeaderList(executionContext);
                int InventoryIssueCount = inventoryIssueHeaderList.GetInventoryIssueHeaderCount(searchParameters, sqlTransaction);

                log.LogMethodExit(InventoryIssueCount);
                return InventoryIssueCount;
            });
        }

        public async Task<InventoryIssueHeaderDTO> UpdateInventoryIssueLines(int issueId, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList)
        {
            return await Task<InventoryIssueHeaderDTO>.Factory.StartNew(() =>
            {
                InventoryIssueHeader inventoryIssueHeader = null;
                InventoryIssueHeaderDTO inventoryIssueHeaderDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(inventoryIssueLineDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (issueId == -1)
                        {
                            throw new Exception("issue Id is not found");
                        }
                        inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, parafaitDBTrx.SQLTrx);
                        inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;
                        if (inventoryIssueHeaderDTO.InventoryIssueLinesListDTO == null || inventoryIssueHeaderDTO.InventoryIssueLinesListDTO.Count == 0)
                        {
                            inventoryIssueHeaderDTO.InventoryIssueLinesListDTO = new List<InventoryIssueLinesDTO>();
                        }
                        inventoryIssueHeaderDTO.InventoryIssueLinesListDTO.AddRange(inventoryIssueLineDTOList);
                        inventoryIssueHeader.SaveIssueForInbox(parafaitDBTrx.SQLTrx);
                        inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, parafaitDBTrx.SQLTrx);
                        inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }

                }
                log.LogMethodExit(inventoryIssueHeaderDTO);
                return inventoryIssueHeaderDTO;
            });
        }

        public async Task<InventoryIssueHeaderDTO> UpdateIssueStatus(int issueId, string issueStatus)
        {
            return await Task<InventoryIssueHeaderDTO>.Factory.StartNew(() =>
            {
                InventoryIssueHeader inventoryIssueHeader = null;
                InventoryIssueHeaderDTO inventoryIssueHeaderDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(issueId, issueStatus);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (issueId == -1)
                        {
                            throw new Exception(" issue Id is not found");
                        }
                        inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, parafaitDBTrx.SQLTrx, true);
                        inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;

                        if (!string.IsNullOrWhiteSpace(issueStatus))
                        {
                            inventoryIssueHeaderDTO.Status = issueStatus;
                        }

                        inventoryIssueHeader.SaveIssueForInbox(parafaitDBTrx.SQLTrx);

                        inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, parafaitDBTrx.SQLTrx, true);
                        inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }

                }
                log.LogMethodExit(inventoryIssueHeaderDTO);
                return inventoryIssueHeaderDTO;
            });
        }
        //public async Task<InventoryIssueHeaderDTO> RemoveInventoryIssueLines(int issueId, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList)
        //{
        //    return await Task<InventoryIssueHeaderDTO>.Factory.StartNew(() =>
        //    {
        //        InventoryIssueHeader inventoryIssueHeader = null;
        //        InventoryIssueHeaderDTO inventoryIssueHeaderDTO = null;
        //        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //        {
        //            log.LogMethodEntry(inventoryIssueLineDTOList);
        //            try
        //            {
        //                parafaitDBTrx.BeginTransaction();
        //                if (issueId == -1)
        //                {
        //                    throw new Exception("issue Id is not found");
        //                }
        //                inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, parafaitDBTrx.SQLTrx);
        //                inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;
        //                if (inventoryIssueHeaderDTO.InventoryIssueLinesListDTO == null || inventoryIssueHeaderDTO.InventoryIssueLinesListDTO.Count == 0)
        //                {
        //                    inventoryIssueHeaderDTO.InventoryIssueLinesListDTO = new List<InventoryIssueLinesDTO>();
        //                }
        //                inventoryIssueHeaderDTO.InventoryIssueLinesListDTO.AddRange(inventoryIssueLineDTOList); // Need to pass IsActive = false
        //                inventoryIssueHeader.UpdateIssueForInbox(parafaitDBTrx.SQLTrx);
        //                inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, parafaitDBTrx.SQLTrx);
        //                inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;
        //                parafaitDBTrx.EndTransaction();
        //            }
        //            catch (ValidationException valEx)
        //            {
        //                parafaitDBTrx.RollBack();
        //                log.Error(valEx);
        //                throw valEx;
        //            }
        //            catch (Exception ex)
        //            {
        //                parafaitDBTrx.RollBack();
        //                log.Error(ex);
        //                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
        //                throw ex;
        //            }

        //        }
        //        log.LogMethodExit(inventoryIssueHeaderDTO);
        //        return inventoryIssueHeaderDTO;
        //    });
        //}

        public async Task<InventoryIssueHeaderDTO> AddInventoryIssueLines(int issueId, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList)
        {
            return await Task<InventoryIssueHeaderDTO>.Factory.StartNew(() =>
            {
                InventoryIssueHeader inventoryIssueHeader = null;
                InventoryIssueHeaderDTO inventoryIssueHeaderDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(inventoryIssueLineDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();

                        inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, parafaitDBTrx.SQLTrx);
                        inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;
                        if (inventoryIssueHeaderDTO.InventoryIssueLinesListDTO == null)
                        {
                            inventoryIssueHeaderDTO.InventoryIssueLinesListDTO = new List<InventoryIssueLinesDTO>();
                        }
                        inventoryIssueHeaderDTO.InventoryIssueLinesListDTO.AddRange(inventoryIssueLineDTOList);
                        inventoryIssueHeader.SaveIssueForInbox(parafaitDBTrx.SQLTrx);
                        inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, parafaitDBTrx.SQLTrx);
                        inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit(inventoryIssueHeaderDTO);
                return inventoryIssueHeaderDTO;
            });
        }

        public async Task<List<InventoryIssueHeaderDTO>> SaveInventoryIssueHeaders(List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList)
        {
            return await Task<List<InventoryIssueHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(inventoryIssueHeaderDTOList);

                if (inventoryIssueHeaderDTOList == null)
                {
                    throw new ValidationException("InventoryIssueHeaderDTOList is Empty");
                }
                List<InventoryIssueHeaderDTO> savedInventoryIssueHeaderDTOList = new List<InventoryIssueHeaderDTO>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (InventoryIssueHeaderDTO inventoryIssueHeaderDTO in inventoryIssueHeaderDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTO, executionContext);
                            if (inventoryIssueHeaderDTO.IsActive)
                            {
                                inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTO, executionContext);
                                inventoryIssueHeader.SaveIssueForInbox(null);
                            }
                            else
                            {
                                // To Delete - Update status as Cancelled: {Status value should be passed as SUBMITTED/OPEN}
                                inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTO, executionContext);
                                inventoryIssueHeader.UpdateIssueForInbox(null);

                                // END
                            }
                            InventoryIssueHeader inventoryIssueHeaders = new InventoryIssueHeader(inventoryIssueHeader.getInventoryIssueHeaderDTO.InventoryIssueLinesListDTO[0].IssueId,
                                                                         executionContext, null, true);
                            savedInventoryIssueHeaderDTOList.Add(inventoryIssueHeaders.getInventoryIssueHeaderDTO);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }

                log.LogMethodExit(savedInventoryIssueHeaderDTOList);
                return savedInventoryIssueHeaderDTOList;
            });
        }

        public async Task<MemoryStream> PrintIssues(int issueId,string reportKey, string timeStamp, DateTime? fromDate,
                              DateTime? toDate, string outputFormat)
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate, outputFormat);

            return await Task<MemoryStream>.Factory.StartNew(() =>
            {

                // Validation - restrict user from printing if it is not approved by the manager.

                InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(issueId, executionContext, null);
                InventoryIssueHeaderDTO inventoryIssueHeaderDTO = inventoryIssueHeader.getInventoryIssueHeaderDTO;

                UserMessages userMessages = new UserMessages(executionContext);
                UserMessagesList userMessagesList = new UserMessagesList();

                List<UserMessagesDTO> userMessagesDTOList = new List<UserMessagesDTO>();
                List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, inventoryIssueHeaderDTO.Guid.ToString()));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);  

                if (userMessagesDTOList != null && userMessagesDTOList.Count > 0)
                {
                    UserMessagesDTO approvedUserMessagesDTO = userMessages.GetHighestApprovedUserMessage(userMessagesDTOList[0].ApprovalRuleID, -1, -1, "Inventory", userMessagesDTOList[0].ObjectType, inventoryIssueHeaderDTO.Guid, executionContext.GetSiteId(), null);
                    foreach (UserMessagesDTO higherUserMessagesDTO in userMessagesDTOList)
                    {
                        if (approvedUserMessagesDTO != null)
                        {
                            if (approvedUserMessagesDTO.Level < higherUserMessagesDTO.Level) 
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required for Print")); // Hardcoded Message need to change with messageNo
                            }
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required for Print"));
                        }
                    }
                }

                List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("IssueId", issueId.ToString()));


                ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryIssueReceipt", timeStamp, fromDate,
                                                                    toDate, backgroundParameters, outputFormat);

                var content = receiptReports.GenerateReport();

                log.LogMethodExit(content);
                return content;
            });
        }
    }
}
