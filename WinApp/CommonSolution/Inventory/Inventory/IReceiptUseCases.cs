/********************************************************************************************
 * Project Name - Inventory
 * Description  - IReceiptUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0    15-Dec-2020         Abhishek                 Created 
*2.130      04-Jun-2021        Girish Kundar             Modified - POS stock changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IReceiptUseCases
    { 
        Task<List<InventoryReceiptDTO>> GetReceipts(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters,
                                                    bool loadChildRecords = false, bool activeChildRecords = true, bool loadReturnQuantity = false, int currentPage = 0, int pageSize = 0,
                                                    SqlTransaction sqlTransaction = null);
        Task<string> SaveReceipts(List<InventoryReceiptDTO> inventoryReceiptDTOList);
        Task<PurchaseOrderDTO> ReceivePurchaseOrder(int purchaseOrderId , List<InventoryReceiptDTO> inventoryReceiptDTOList);

        Task<int> GetReceiptCount(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters,
                                                    SqlTransaction sqlTransaction = null);
    }
}
