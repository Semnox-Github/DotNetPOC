using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
   public  class PrivilegeRemoveAdjustment : PrivilegeAdjustment
    {
        public PrivilegeAddAdjustment()
        {
            expirationDateTime = null;
        }
        public string expirationDateTime { get; set; }
    }
}
