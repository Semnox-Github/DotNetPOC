/********************************************************************************************
 * Project Name - Inventory
 * Description  - IPOTaxView class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      11-Jan-2020      Abhishek                  Created 
 ********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IPOTaxViewUseCases
    {     
        Task<List<PurchaseOrderTaxLineDTO>> GetPurchaseOrderTaxViews(int poId, int poLineId, bool totalTax);
    }
}
