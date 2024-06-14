using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// CustomCheckInDTO
    /// </summary>
    public class CustomCheckInDTO
    {
        public CheckInDetailDTO checkInDetailDTO { get; set; }
        public Transaction.TransactionLine transactionLine { get; set; }
        public int CheckInProductId { get; set; }
    }
}
