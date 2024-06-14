using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class TimePlayMinutesAdjustment : AdjustmentBase
    {

        public TimePlayMinutesAdjustment()
        {
            groupId = -1;
            minutes = 0;
        }
        public string type { get; set; }
        public int groupId { get; set; }
        public int minutes { get; set; }
    }
}
