using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// PerformTransactionDTO - Envelope DTO for Transaction.POST
    /// <Parameter>TransactionParams transactionParams</Parameter>
    /// <Parameter>List<LinkedPurchaseProductsStruct> orderedProducts</Parameter>
    /// </summary>
    public class PerformTransactionDTO
    {
        public TransactionParams transactionParams;
        public List<LinkedPurchaseProductsStruct> orderedProducts;
    }
}
