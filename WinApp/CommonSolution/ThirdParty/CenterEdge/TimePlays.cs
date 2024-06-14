/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - TimePlayDTO class - This would return the point types
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public  class TimePlays : TimePlayBase
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool timePlayStarted;
        private int timePlayMinutesRemaining;
        public TimePlays()
        {
            log.LogMethodEntry();
            timePlayStarted = true;
            timePlayMinutesRemaining = 0;
            log.LogMethodExit();
        }
        public bool started { get { return timePlayStarted; } set { timePlayStarted = value; } }
        public int minutesRemaining { get { return timePlayMinutesRemaining; } set { timePlayMinutesRemaining = value; } }

    }
}
