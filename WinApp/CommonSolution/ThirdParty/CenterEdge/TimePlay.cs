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
    /// <summary>
    /// This class holds the configuration information for the timeplays
    /// </summary>
    public class TimePlay
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int maxTimePlaysPerCard;
        private Minute minute;
        public TimePlay()
        {
            log.LogMethodEntry();
            maxTimePlaysPerCard = 0;
            minute = new Minute();
            log.LogMethodExit();
        }

        public TimePlay(int maxTimePlaysPerCard, Minute minute)
        {
            log.LogMethodEntry(maxTimePlaysPerCard, minute);
            this.maxTimePlaysPerCard = maxTimePlaysPerCard;
            this.minute = minute;
            log.LogMethodExit();
        }
        public int maximumTimePlaysPerCard { get { return maxTimePlaysPerCard; } set { maxTimePlaysPerCard = value; } }
        public Minute minutes { get { return minute; } set { minute = value; } }
    }
}
