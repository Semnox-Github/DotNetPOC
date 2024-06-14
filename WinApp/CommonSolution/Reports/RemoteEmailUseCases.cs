/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteMessagingRequestUseCases class to get the data  from API by doing remote call  
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
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Reports
{
    public class RemoteEmailUseCases : RemoteUseCases, IEmailUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string PO_EMAIL_URL = "api/Inventory/PurchaseOrder/{purchaseOrderId}/Email";
        private string RQ_EMAIL_URL = "api/Inventory/Requisition/{requisitionId}/Email";

        public RemoteEmailUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
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
        public async Task<MessagingRequestDTO> SendRequisitionEmail(int requisitionId, string requisitionNumber, List<MessagingRequestDTO> messagingRequestDTOList)
        {
            log.LogMethodEntry(requisitionId, messagingRequestDTOList);
            try
            {
                RQ_EMAIL_URL = "api/Inventory/Requisition/" + requisitionId + "/" + requisitionNumber + "/Email";
                MessagingRequestDTO responseString = await Post<MessagingRequestDTO>(RQ_EMAIL_URL, messagingRequestDTOList);
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
