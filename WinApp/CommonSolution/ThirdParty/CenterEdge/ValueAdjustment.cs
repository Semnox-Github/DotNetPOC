using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
     
    public class ValueAdjustment //: AdjustmentBase
    {
        public ValueAdjustment()  
        {
            type = "";
            amount = new Points();
        }
        public string type { get; set; }
        public Points amount { get; set; }
    }
}
