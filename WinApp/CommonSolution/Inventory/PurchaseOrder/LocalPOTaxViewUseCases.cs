/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalPOTaxViewUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     11-Jan-2020       Abhishek         Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class LocalPOTaxViewUseCases :LocalUseCases, IPOTaxViewUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LocalPOTaxViewUseCases(ExecutionContext executionContext)
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<PurchaseOrderTaxLineDTO>> GetPurchaseOrderTaxViews(int purchaseOrderId, int poLineId, bool totalTaxView = false)
        {
            return await Task<List<PurchaseOrderTaxLineDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(purchaseOrderId);
                PurchaseOrderTaxLineListBL PurchaseOrderTaxLineListBL = new PurchaseOrderTaxLineListBL(executionContext);
                List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = PurchaseOrderTaxLineListBL.GetTotalPOTaxView(purchaseOrderId, poLineId, totalTaxView);
                log.LogMethodExit(purchaseOrderTaxLineDTOList);
                return purchaseOrderTaxLineDTOList;
            });
        } 
    }
}
