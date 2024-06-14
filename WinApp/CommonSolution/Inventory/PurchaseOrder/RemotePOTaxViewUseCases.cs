/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemotePOTaxViewUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     11-Jan-2021      Abhishek         Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class RemotePOTaxViewUseCases : RemoteUseCases, IPOTaxViewUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PO_Tax_view_URL = "api/Inventory/POTaxes";

        public RemotePOTaxViewUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<PurchaseOrderTaxLineDTO>> GetPurchaseOrderTaxViews(int poId, int poLineId, bool totalTax)     
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("poId", poId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("poLineId", poLineId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("totalTax", totalTax.ToString()));         
            try
            {
                List<PurchaseOrderTaxLineDTO> result = await Get<List<PurchaseOrderTaxLineDTO>>(PO_Tax_view_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
