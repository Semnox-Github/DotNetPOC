/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemotePurchaseOrderUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.120.0     09-Dec-2020       Mushahid Faizan         Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Inventory
{
    public class RemotePurchaseOrderUseCases : RemoteUseCases, IPurchaseOrderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PO_URL = "api/Inventory/PurchaseOrders";
        private const string POC_URL = "api/Inventory/PurchaseOrderCounts";
        private string PURCHASEORDER_LINE_URL = "api/Inventory/PurchaseOrder/{purchaseOrderId}/PurchaseOrderLines";
        private string PURCHASEORDER_STATUS_URL = "api/Inventory/PurchaseOrder/{purchaseOrderId}/Status";
        private string PRINT_PURCHASE_ORDER_URL = "api/Inventory/PurchaseOrder/{orderId}/Print";
        private string PO_EMAIL_URL = "api/Inventory/PurchaseOrder/{purchaseOrderId}/Email";

        public RemotePurchaseOrderUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<PurchaseOrderDTO>> GetPurchaseOrders(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
                          parameters, bool buildChildRecords, bool loadActiveChild, int currentPage = 0, int pageSize = 0, bool mostRepeated = false)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("mostRepeated".ToString(), mostRepeated.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            List<PurchaseOrderDTO> result = await Get<List<PurchaseOrderDTO>>(PO_URL, searchParameterList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<int> GetPurchaseOrderCount(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
                          parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(POC_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> lookupSearchParams)
        {

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("documentTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseOrderId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.VENDORID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("documentStatus".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERSTATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderStatus".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.FROM_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.TO_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDER_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseOrderIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.GUID_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("guidIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case PurchaseOrderDTO.SearchByPurchaseOrderParameters.IS_AUTO_PO:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isAutoPO".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SavePurchaseOrders(List<PurchaseOrderDTO> purchaseOrderDTOList)
        {
            log.LogMethodEntry(purchaseOrderDTOList);
            try
            {
                string responseString = await Post<string>(PO_URL, purchaseOrderDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<PurchaseOrderDTO> AddPurchaseOrderLines(int purchaseOrderId, List<PurchaseOrderLineDTO> purchaseOrderLineDTOList)
        {
            log.LogMethodEntry(purchaseOrderId, purchaseOrderLineDTOList);
            try
            {
                PURCHASEORDER_LINE_URL = "api/Inventory/PurchaseOrder/" + purchaseOrderId + "/PurchaseOrderLines";
                PurchaseOrderDTO responseString = await Post<PurchaseOrderDTO>(PURCHASEORDER_LINE_URL, purchaseOrderLineDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<PurchaseOrderDTO> UpdatePurchaseOrderLines(int purchaseOrderId, List<PurchaseOrderLineDTO> purchaseOrderLineDTOList)
        {
            log.LogMethodEntry(purchaseOrderId, purchaseOrderLineDTOList);
            try
            {
                PURCHASEORDER_LINE_URL = "api/Inventory/PurchaseOrder/" + purchaseOrderId + "/PurchaseOrderLines";
                PurchaseOrderDTO responseString = await Post<PurchaseOrderDTO>(PURCHASEORDER_LINE_URL, purchaseOrderLineDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<PurchaseOrderDTO> UpdatePurchaseOrderStatus(int purchaseOrderId, string purchaseOrderStatus)
        {
            log.LogMethodEntry(purchaseOrderId, purchaseOrderStatus);
            try
            {
                PURCHASEORDER_STATUS_URL = "api/Inventory/PurchaseOrder/" + purchaseOrderId + "/Status";
                PurchaseOrderDTO responseString = await Post<PurchaseOrderDTO>(PURCHASEORDER_STATUS_URL, purchaseOrderStatus);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<MessagingRequestDTO> SendPurchaseOrderEmail(int purchaseOrderId, List<MessagingRequestDTO> messagingRequestDTOList)
        {
            log.LogMethodEntry(purchaseOrderId, messagingRequestDTOList);
            try
            {
                PO_EMAIL_URL = "api/Inventory/PurchaseOrder/" + purchaseOrderId + "/Email";
                MessagingRequestDTO responseString = await Post<MessagingRequestDTO>(PO_EMAIL_URL, messagingRequestDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<MemoryStream> PrintPurchaseOrders(int orderId, string reportKey, string timeStamp, DateTime? fromDate,
                             DateTime? toDate, string outputFormat, double ticketCost)
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate, orderId, outputFormat);
            {
                try
                {
                    List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                    searchParameterList.Add(new KeyValuePair<string, string>("reportKey".ToString(), reportKey.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("timeStamp".ToString(), timeStamp.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), fromDate.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), toDate.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("outputFormat".ToString(), outputFormat.ToString()));

                    List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                    backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("PurchaseOrderId", orderId.ToString()));
                    backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("TicketCost", ticketCost.ToString()));


                    if (backgroundParameters != null)
                    {
                        foreach (clsReportParameters.SelectedParameterValue selectedParameterValue in backgroundParameters)
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("selectedParameterValue".ToString(), selectedParameterValue.ToString()));
                        }
                    }
                    MemoryStream responseString = await Get<MemoryStream>(PRINT_PURCHASE_ORDER_URL, searchParameterList);
                    log.LogMethodExit(responseString);
                    return responseString;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }

            }

        }
    }
}
