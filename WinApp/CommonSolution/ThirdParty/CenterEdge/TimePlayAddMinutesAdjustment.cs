using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class TimePlayAddMinutesAdjustment : TimePlayMinutesAdjustment
    {
        public TimePlayAddMinutesAdjustment()
        {
            type = "addMinute";
            expirationDateTime = null;
            startTimePlay = true;
        }
        public string expirationDateTime { get; set; }
        public bool startTimePlay { get; set; }
    }
}
