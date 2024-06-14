/********************************************************************************************
* Project Name - Inventory
* Description  - IEmailUseCases class 
*  
**************
**Version Log
**************
*Version     Date               Modified By               Remarks          
*********************************************************************************************
*2.110.1     01-Mar-2021      Mushahid Faizan          Created.
********************************************************************************************/
using Semnox.Parafait.Communication;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Reports
{
   public interface IEmailUseCases
    {
        Task<MessagingRequestDTO> SendPurchaseOrderEmail(int purchaseOrderId, List<MessagingRequestDTO> messagingRequestDTOList);
        Task<MessagingRequestDTO> SendRequisitionEmail(int requisitionId, string requisitionNumber, List<MessagingRequestDTO> messagingRequestDTOList);
    }
}
