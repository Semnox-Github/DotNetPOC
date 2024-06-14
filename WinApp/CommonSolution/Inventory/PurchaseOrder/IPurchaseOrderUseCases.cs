/********************************************************************************************
 * Project Name - Inventory
 * Description  - IPurchaseOrderUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      09-Dec-2020       Mushahid Faizan         Created 
 ********************************************************************************************/

using Semnox.Parafait.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IPurchaseOrderUseCases
    {
        Task<List<PurchaseOrderDTO>> GetPurchaseOrders(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
           searchParameters, bool buildChildRecords, bool loadActiveChild, int currentPage = 0, int pageSize = 0, bool mostRepeated=false);
        Task<string> SavePurchaseOrders(List<PurchaseOrderDTO> purchaseOrderDTOList);
        Task<PurchaseOrderDTO> AddPurchaseOrderLines(int purchaseOrderId, List<PurchaseOrderLineDTO> purchaseOrderLineDTOList);
        Task<PurchaseOrderDTO> UpdatePurchaseOrderLines(int purchaseOrderId, List<PurchaseOrderLineDTO> purchaseOrderLineDTOList);
        Task<PurchaseOrderDTO> UpdatePurchaseOrderStatus(int purchaseOrderId, string purchaseOrderStatus);
        Task<int> GetPurchaseOrderCount(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
             searchParameters);

        Task<MemoryStream> PrintPurchaseOrders(int orderId, string reportKey, string timeStamp, DateTime? fromDate,
                            DateTime? toDate, string outputFormat, double ticketCost);

        Task<MessagingRequestDTO> SendPurchaseOrderEmail(int purchaseOrderId, List<MessagingRequestDTO> messagingRequestDTOList);
    }
}
