using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
   public class Transactions  // CE response object
    {
            public List<TransactionDTO> transactions { get; set; }
            public int sinceId { get; set; }
    }
}
