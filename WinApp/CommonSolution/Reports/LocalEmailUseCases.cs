/********************************************************************************************
 * Project Name - MessagingRequest
 * Description  - LocalMessagingRequestUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.110.1     01-Mar-2021      Mushahid Faizan          Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Reports
{
    public class LocalEmailUseCases : IEmailUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalEmailUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
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
                            string filePath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(),"PDF_OUTPUT_DIR");
                            FileStream fileStream = new FileStream(filePath + "\\"+ "InventoryPurchaseOrderReceipt" + " - " + purchaseOrderId + ".pdf", FileMode.Create);
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
        public async Task<MessagingRequestDTO> SendRequisitionEmail(int requisitionId, string requisitionNumber, List<MessagingRequestDTO> messagingRequestDTOList)
        {
            return await Task<MessagingRequestDTO>.Factory.StartNew(() =>
            {
                MessagingRequestDTO result = null;
                log.LogMethodEntry(requisitionId);

                if (requisitionId < -1 || string.IsNullOrEmpty(requisitionNumber))
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
                            backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("RequisitionNo", requisitionNumber.ToString()));

                            ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryRequisitionReceipt", "", DateTime.MinValue,
                                                                   DateTime.MinValue, backgroundParameters, "P");


                            MemoryStream ms = receiptReports.GenerateReport();
                            //string filePath = (backgroundParameters != null ? Path.GetTempPath() : ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PDF_OUTPUT_DIR"));
                            string filePath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext.GetSiteId(),"PDF_OUTPUT_DIR");
                            FileStream fileStream = new FileStream(filePath + "\\"+ "InventoryRequisitionReceipt" + " - " + requisitionId + ".pdf", FileMode.Create);
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
    }
}
