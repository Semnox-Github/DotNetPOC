/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalPurchaseOrderUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     09-Dec-2020       Mushahid Faizan         Created : Web Inventory UI Redesign with REST API
 *2.130.0     04-Jun-2021      Girish Kundar            Modified - POS stock changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Inventory
{
    public class LocalPurchaseOrderUseCases : IPurchaseOrderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalPurchaseOrderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<PurchaseOrderDTO>> GetPurchaseOrders(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
                          searchParameters, bool buildChildRecords, bool loadActiveChild,  int currentPage = 0, int pageSize = 0, bool mostRepeated=false)
        {

            return await Task<List<PurchaseOrderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                PurchaseOrderList purchaseOrderListBL = new PurchaseOrderList(executionContext);
                List<PurchaseOrderDTO> purchaseOrderDTOList = purchaseOrderListBL.GetAllPurchaseOrder(searchParameters, buildChildRecords,null, currentPage, pageSize,mostRepeated);
                log.LogMethodExit(purchaseOrderDTOList);
                return purchaseOrderDTOList;
            });
        }

        public async Task<int> GetPurchaseOrderCount(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
                          searchParameters)
        {

            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                PurchaseOrderList purchaseOrderListBL = new PurchaseOrderList(executionContext);
                int purchaseOrderCount = purchaseOrderListBL.GetPurchaseOrderCount(searchParameters);
                log.LogMethodExit(purchaseOrderCount);
                return purchaseOrderCount;
            });
        }

        public async Task<PurchaseOrderDTO> UpdatePurchaseOrderLines(int purchaseOrderId, List<PurchaseOrderLineDTO> purchaseOrderLineDTOList)
        {
            return await Task<PurchaseOrderDTO>.Factory.StartNew(() =>
            {
                PurchaseOrder purchaseOrder = null;
                PurchaseOrderDTO purchaseOrderDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(purchaseOrderLineDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (purchaseOrderId == -1)
                        {
                            throw new Exception("Purchase Order Id is not found");
                        }
                        purchaseOrder = new PurchaseOrder(purchaseOrderId, executionContext, parafaitDBTrx.SQLTrx);
                        purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
                        if (purchaseOrderDTO.PurchaseOrderLineListDTO == null || purchaseOrderDTO.PurchaseOrderLineListDTO.Count == 0)
                        {
                            purchaseOrderDTO.PurchaseOrderLineListDTO = new List<PurchaseOrderLineDTO>();
                        }
                        purchaseOrderDTO.PurchaseOrderLineListDTO.AddRange(purchaseOrderLineDTOList);
                        purchaseOrder.SavePurchaseOrderForInbox(parafaitDBTrx.SQLTrx);
                        purchaseOrder = new PurchaseOrder(purchaseOrderId, executionContext, parafaitDBTrx.SQLTrx, true);
                        purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
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
                log.LogMethodExit(purchaseOrderDTO);
                return purchaseOrderDTO;
            });
        }

        public async Task<PurchaseOrderDTO> UpdatePurchaseOrderStatus(int purchaseOrderId, string purchaseOrderStatus)
        {
            return await Task<PurchaseOrderDTO>.Factory.StartNew(() =>
            {
                PurchaseOrder purchaseOrder = null;
                PurchaseOrderDTO purchaseOrderDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(purchaseOrderId, purchaseOrderStatus);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (purchaseOrderId == -1)
                        {
                            throw new Exception(" Purchase Order Id is not found");
                        }
                        purchaseOrder = new PurchaseOrder(purchaseOrderId, executionContext, parafaitDBTrx.SQLTrx, true);
                        purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;

                        if (!string.IsNullOrWhiteSpace(purchaseOrderStatus))
                        {
                            purchaseOrderDTO.OrderStatus = purchaseOrderStatus;
                        }
                        purchaseOrder.SavePurchaseOrderForInbox(parafaitDBTrx.SQLTrx);
                        purchaseOrder = new PurchaseOrder(purchaseOrderId, executionContext, parafaitDBTrx.SQLTrx, true);
                        purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
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
                log.LogMethodExit(purchaseOrderDTO);
                return purchaseOrderDTO;
            });
        }

        public async Task<PurchaseOrderDTO> AddPurchaseOrderLines(int purchaseOrderId, List<PurchaseOrderLineDTO> purchaseOrderLineDTOList)
        {
            return await Task<PurchaseOrderDTO>.Factory.StartNew(() =>
            {
                PurchaseOrder purchaseOrder = null;
                PurchaseOrderDTO purchaseOrderDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(purchaseOrderLineDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (purchaseOrderId == -1)    // Create new Purchase Order with status OPEN
                        {
                            purchaseOrderDTO = new PurchaseOrderDTO();
                            purchaseOrderDTO.OrderDate = ServerDateTime.Now;
                            purchaseOrderDTO.OrderStatus = "Open";
                            purchaseOrder = new PurchaseOrder(executionContext, purchaseOrderDTO);
                            purchaseOrder.SavePurchaseOrderForInbox(parafaitDBTrx.SQLTrx);
                            purchaseOrderId = purchaseOrder.getPurchaseOrderDTO.PurchaseOrderId;
                        }
                        purchaseOrder = new PurchaseOrder(purchaseOrderId, executionContext, parafaitDBTrx.SQLTrx);
                        purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
                        if (purchaseOrderDTO.PurchaseOrderLineListDTO == null)
                        {
                            purchaseOrderDTO.PurchaseOrderLineListDTO = new List<PurchaseOrderLineDTO>();
                        }
                        purchaseOrderDTO.PurchaseOrderLineListDTO.AddRange(purchaseOrderLineDTOList);
                        purchaseOrder.SavePurchaseOrderForInbox(parafaitDBTrx.SQLTrx);
                        purchaseOrder = new PurchaseOrder(purchaseOrderId, executionContext, parafaitDBTrx.SQLTrx, true);
                        purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
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
                log.LogMethodExit(purchaseOrderDTO);
                return purchaseOrderDTO;
            });
        }

        public async Task<string> SavePurchaseOrders(List<PurchaseOrderDTO> purchaseOrderDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(purchaseOrderDTOList);
                if (purchaseOrderDTOList == null)
                {
                    throw new ValidationException("purchaseOrderDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (PurchaseOrderDTO purchaseOrderDTO in purchaseOrderDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            PurchaseOrder purchaseOrderBL = new PurchaseOrder(purchaseOrderDTO, executionContext);
                            //purchaseOrderBL.SavePurchaseOrderForInbox(parafaitDBTrx.SQLTrx);
                            purchaseOrderBL.Save(parafaitDBTrx.SQLTrx);
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
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }


        public async Task<MessagingRequestDTO> SendPurchaseOrderEmail(int purchaseOrderId, List<MessagingRequestDTO> messagingRequestDTOList)
        {
            return await Task<MessagingRequestDTO>.Factory.StartNew(() =>
            {
                MessagingRequestDTO result = null;
                log.LogMethodEntry(purchaseOrderId);

                if (purchaseOrderId < -1)
                {
                    throw new ValidationException("PurchaseOrder not found");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (MessagingRequestDTO messagingRequestDTO in messagingRequestDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                            backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("PurchaseOrderId", purchaseOrderId.ToString()));
                            backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("TicketCost", 0));

                            ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryPurchaseOrderReceipt", "", DateTime.MinValue,
                                                                   DateTime.MinValue, backgroundParameters, "P");


                            MemoryStream ms = receiptReports.GenerateReport();
                            string filePath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(), "PDF_OUTPUT_DIR");
                            FileStream fileStream = new FileStream(filePath + "\\" + "InventoryPurchaseOrderReceipt" + " - " + purchaseOrderId + ".pdf", FileMode.Create);
                            ms.WriteTo(fileStream);

                            // File path with file name can be store in the attach file field, server will take the path and send .
                            messagingRequestDTO.AttachFile = filePath + "\\" + "InventoryPurchaseOrderReceipt" + " - " + purchaseOrderId + ".pdf";

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

        public async Task<MemoryStream> PrintPurchaseOrders(int orderId, string reportKey, string timeStamp, DateTime? fromDate,
                            DateTime? toDate, string outputFormat, double ticketCost)
        {
            log.LogMethodEntry(orderId, reportKey, timeStamp, fromDate, toDate, ticketCost, outputFormat);

            return await Task<MemoryStream>.Factory.StartNew(() =>
            {
                // Validation - restrict user from printing if it is not approved by the manager.

                PurchaseOrder purchaseOrder = new PurchaseOrder(orderId, executionContext);
                PurchaseOrderDTO purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;  // Get the PurchaseOrderDTO

                UserMessages userMessages = new UserMessages(executionContext);
                UserMessagesList userMessagesList = new UserMessagesList();
                List<UserMessagesDTO> userMessagesDTOList = new List<UserMessagesDTO>();

                List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, purchaseOrderDTO.Guid.ToString()));
                userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);

                if (userMessagesDTOList != null && userMessagesDTOList.Count > 0)
                {
                    UserMessagesDTO approvedUserMessagesDTO = userMessages.GetHighestApprovedUserMessage(userMessagesDTOList[0].ApprovalRuleID, -1, -1, "Inventory", userMessagesDTOList[0].ObjectType, purchaseOrderDTO.Guid, executionContext.GetSiteId(), null);
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

                // for Generating Reports
                List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("PurchaseOrderId", orderId.ToString()));
                backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("TicketCost", ticketCost.ToString()));

                ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryPurchaseOrderReceipt", timeStamp, fromDate,
                                                                    toDate, backgroundParameters, outputFormat);

                var content = receiptReports.GenerateReport();

                log.LogMethodExit(content);
                return content;
            });

        }
    }
}
