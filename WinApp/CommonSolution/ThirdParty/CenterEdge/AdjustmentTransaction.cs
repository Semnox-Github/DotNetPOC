using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
   public class AdjustmentTransaction : TransactionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public OperatorDTO operators { get; set; }
        public List<AdjustmentBase> adjustments { get; set; }
    }
}
