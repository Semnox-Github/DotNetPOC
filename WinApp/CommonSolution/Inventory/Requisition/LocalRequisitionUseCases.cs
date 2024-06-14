/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalRequisitionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      10-Dec-2020       Mushahid Faizan          Created for Inventory UI Redesign changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Inventory.Requisition
{
    public class LocalRequisitionUseCases : LocalUseCases, IRequisitionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalRequisitionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<RequisitionDTO>> GetRequisitions(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> parameters,
                                                                bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0,
                                                                int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<RequisitionDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RequisitionList requisitionListBL = new RequisitionList(executionContext);
                List<RequisitionDTO> requisitionDTOList = requisitionListBL.GetAllRequisition(parameters, loadChildRecords, activeChildRecords, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(requisitionDTOList);
                return requisitionDTOList;
            });
        }

        public async Task<int> GetRequisitionCount(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> parameters,
                                                                SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RequisitionList requisitionListBL = new RequisitionList(executionContext);
                int requisitionCount = requisitionListBL.GetRequisitionsCount(parameters);
                log.LogMethodExit(requisitionCount);
                return requisitionCount;
            });
        }

        public async Task<RequisitionDTO> UpdateRequisitionLines(int requisitionId, List<RequisitionLinesDTO> requisitionLineDTOList)
        {
            return await Task<RequisitionDTO>.Factory.StartNew(() =>
            {
                RequisitionBL requisitionBL = null;
                RequisitionDTO requisitionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(requisitionLineDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (requisitionId == -1)
                        {
                            throw new Exception("Requisition Id is not found");
                        }
                        requisitionBL = new RequisitionBL(executionContext, requisitionId, parafaitDBTrx.SQLTrx);
                        requisitionDTO = requisitionBL.GetRequisitionsDTO;
                        if (requisitionDTO.RequisitionLinesListDTO == null || requisitionDTO.RequisitionLinesListDTO.Count == 0)
                        {
                            requisitionDTO.RequisitionLinesListDTO = new List<RequisitionLinesDTO>();
                        }
                        //foreach (RequisitionLinesListDTO requisitionLinesListDTO in requisitionLineDTOList)
                        //{
                        //    //purchaseOrderDTO.PurchaseOrderLineListDTO.Add(purchaseOrderLineDTOList);
                        //    PurchaseOrderLineDTO result = purchaseOrderDTO.PurchaseOrderLineListDTO.Where(x => x.PurchaseOrderLineId == purchaseOrderLineDTO.PurchaseOrderLineId).FirstOrDefault();
                        //    if (result != null)
                        //    {
                        //        int index = purchaseOrderDTO.PurchaseOrderLineListDTO.FindIndex(x => x.PurchaseOrderLineId == purchaseOrderLineDTO.PurchaseOrderLineId);
                        //        purchaseOrderDTO.PurchaseOrderLineListDTO[index] = result;
                        //        purchaseOrderDTO.PurchaseOrderLineListDTO[index].IsChanged=true;
                        //    }
                        //}
                        requisitionDTO.RequisitionLinesListDTO.AddRange(requisitionLineDTOList);
                        requisitionBL.SaveRequisitionForInbox(parafaitDBTrx.SQLTrx);
                        requisitionBL = new RequisitionBL(executionContext, requisitionId, parafaitDBTrx.SQLTrx, true);
                        requisitionDTO = requisitionBL.GetRequisitionsDTO;
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
                log.LogMethodExit(requisitionDTO);
                return requisitionDTO;
            });
        }

        public async Task<RequisitionDTO> UpdateRequisitionStatus(int requisitionId, string requisitionStatus)
        {
            return await Task<RequisitionDTO>.Factory.StartNew(() =>
            {
                RequisitionBL requisitionBL = null;
                RequisitionDTO requisitionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(requisitionId, requisitionStatus);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (requisitionId == -1)
                        {
                            throw new Exception(" Requisition Id is not found");
                        }
                        requisitionBL = new RequisitionBL(executionContext, requisitionId, parafaitDBTrx.SQLTrx, true);
                        requisitionDTO = requisitionBL.GetRequisitionsDTO;

                        if (!string.IsNullOrWhiteSpace(requisitionStatus))
                        {
                            requisitionDTO.Status = requisitionStatus;
                        }
                        requisitionBL.SaveRequisitionForInbox(parafaitDBTrx.SQLTrx);
                        requisitionBL = new RequisitionBL(executionContext, requisitionId, parafaitDBTrx.SQLTrx, true);
                        requisitionDTO = requisitionBL.GetRequisitionsDTO;
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
                log.LogMethodExit(requisitionDTO);
                return requisitionDTO;
            });
        }

        public async Task<RequisitionDTO> AddRequisitionLines(int requisitionId, List<RequisitionLinesDTO> requisitionLineDTOList)
        {
            return await Task<RequisitionDTO>.Factory.StartNew(() =>
            {
                RequisitionBL requisitionBL = null;
                RequisitionDTO requisitionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(requisitionLineDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (requisitionId == -1)    // Create new Requisition with status OPEN
                        {
                            requisitionDTO = new RequisitionDTO();
                            requisitionDTO.Status = "Open";
                            requisitionBL = new RequisitionBL(executionContext, requisitionDTO);
                            requisitionBL.SaveRequisitionForInbox(parafaitDBTrx.SQLTrx);
                            requisitionId = requisitionBL.GetRequisitionsDTO.RequisitionId;
                        }
                        requisitionBL = new RequisitionBL(executionContext, requisitionId, parafaitDBTrx.SQLTrx);
                        requisitionDTO = requisitionBL.GetRequisitionsDTO;
                        if (requisitionDTO.RequisitionLinesListDTO == null)
                        {
                            requisitionDTO.RequisitionLinesListDTO = new List<RequisitionLinesDTO>();
                        }
                        requisitionDTO.RequisitionLinesListDTO.AddRange(requisitionLineDTOList);
                        requisitionBL.SaveRequisitionForInbox(parafaitDBTrx.SQLTrx);
                        requisitionBL = new RequisitionBL(executionContext, requisitionId, parafaitDBTrx.SQLTrx, true);
                        requisitionDTO = requisitionBL.GetRequisitionsDTO;
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
                log.LogMethodExit(requisitionDTO);
                return requisitionDTO;
            });
        }

        public async Task<List<RequisitionDTO>> SaveRequisitions(List<RequisitionDTO> requisitionDTOList)
        {
            return await Task<List<RequisitionDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(requisitionDTOList);
                List<RequisitionDTO> savedRequisitionDTOList = new List<RequisitionDTO>();
                if (requisitionDTOList == null)
                {
                    throw new ValidationException("requisitionDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RequisitionDTO requisitionDTO in requisitionDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionDTO);
                            requisitionBL.SaveRequisitionForInbox(parafaitDBTrx.SQLTrx);
                            requisitionBL = new RequisitionBL(executionContext, requisitionBL.GetRequisitionsDTO.RequisitionLinesListDTO[0].RequisitionId, parafaitDBTrx.SQLTrx, true);
                            savedRequisitionDTOList.Add(requisitionBL.GetRequisitionsDTO);
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
                }
                log.LogMethodExit(savedRequisitionDTOList);
                return savedRequisitionDTOList;
            });
        }
        public async Task<MessagingRequestDTO> SendRequisitionEmail(int requisitionId, List<MessagingRequestDTO> messagingRequestDTOList)
        {
            return await Task<MessagingRequestDTO>.Factory.StartNew(() =>
            {
                MessagingRequestDTO result = null;
                log.LogMethodEntry(requisitionId);

                if (requisitionId < -1)
                {
                    throw new ValidationException("Requisition not found");
                }
                RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionId);
                RequisitionDTO requisitionDTO = requisitionBL.GetRequisitionsDTO;
                if (requisitionDTO == null)
                {
                    throw new ValidationException("Requisition not found");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (MessagingRequestDTO messagingRequestDTO in messagingRequestDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                            backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("RequisitionId", requisitionId.ToString()));
                            backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("RequisitionNo", requisitionDTO.RequisitionNo.ToString()));

                            ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryRequisitionReceipt", "", DateTime.MinValue,
                                                                   DateTime.MinValue, backgroundParameters, "P");


                            MemoryStream ms = receiptReports.GenerateReport();
                            string filePath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(), "PDF_OUTPUT_DIR");
                            FileStream fileStream = new FileStream(filePath + "\\" + "InventoryRequisitionReceipt" + " - " + requisitionId + ".pdf", FileMode.Create);
                            ms.WriteTo(fileStream);

                            // File path with file name can be store in the attach file field, server will take the path and send .
                            messagingRequestDTO.AttachFile = filePath + "\\" + "InventoryRequisitionReceipt" + " - " + requisitionId + ".pdf";

                            MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                            messagingRequestBL.Save(parafaitDBTrx.SQLTrx);
                            result = messagingRequestDTO;
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

                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<MemoryStream> PrintRequisitions(int requisitionId, string reportKey, string timeStamp, DateTime? fromDate,
                              DateTime? toDate, string outputFormat)
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate, outputFormat);

            return await Task<MemoryStream>.Factory.StartNew(() =>
            {
                // Validation - restrict user from printing if it is not approved by the manager.

                RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionId);
                RequisitionDTO requisitionDTO = requisitionBL.GetRequisitionsDTO;

                UserMessages userMessages = new UserMessages(executionContext);
                UserMessagesList userMessagesList = new UserMessagesList(executionContext);
                List<UserMessagesDTO> userMessagesDTOList = new List<UserMessagesDTO>();

                List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, requisitionDTO.GUID.ToString()));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);

                if (userMessagesDTOList != null && userMessagesDTOList.Count > 0)
                {
                    UserMessagesDTO approvedUserMessagesDTO = userMessages.GetHighestApprovedUserMessage(userMessagesDTOList[0].ApprovalRuleID, -1, -1, "Inventory", userMessagesDTOList[0].ObjectType, requisitionDTO.GUID, executionContext.GetSiteId(), null);
                    foreach (UserMessagesDTO higherUserMessagesDTO in userMessagesDTOList)
                    {
                        if (approvedUserMessagesDTO != null)
                        {
                            if (approvedUserMessagesDTO.Level < higherUserMessagesDTO.Level)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required for Print"));// Hardcoded Message need to change with messageNo
                            }
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required for Print"));
                        }
                    }
                }
                // For Generating report
                List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("RequisitionId", requisitionId.ToString()));
                backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("RequisitionNo", requisitionDTO.RequisitionNo.ToString()));

                ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryRequisitionReceipt", timeStamp, fromDate,
                                                                    toDate, backgroundParameters, outputFormat);

                var content = receiptReports.GenerateReport();

                log.LogMethodExit(content);
                return content;
            });

        }
    }
}
