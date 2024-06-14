using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class PrivilegeAdjustment  : AdjustmentBase
    {
        public PrivilegeAdjustment()
        {
            type = "addPrivilage";
            groupId = -1;
            count = 0;
        }
        // public string type { get; set; }
        public int groupId { get; set; }
        public int count { get; set; }
    }
}
