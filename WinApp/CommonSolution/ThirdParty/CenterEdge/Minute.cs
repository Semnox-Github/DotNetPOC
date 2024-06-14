/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - MinuteDTO class - This would return the time paramters
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
    /// This is Enum holds the enum constants for Time play starts types
    /// </summary>
    public enum StartTypes
    {
        startAtFirstUse
    }

    /// <summary>
    /// This class holds the configuration information fo Time play with minute adjustments
    /// </summary>
   public class Minute
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool supported;
        private bool addMinutes;
        private bool expire;
        private string[] minuteStartTypes;

        public Minute()
        {
            /*Capabilities incorrectly reports that adding minutes to time plays is supported  */
            log.LogMethodEntry();
            supported = false;
            addMinutes = false;
            expire = false;
            minuteStartTypes =  Enum.GetNames(typeof(StartTypes));
            log.LogMethodExit();
        }

        public Minute(bool supported , bool addMinutes, bool expire)
        {
            log.LogMethodEntry(supported, addMinutes, expire);
            this.supported = supported;
            this.addMinutes = addMinutes;
            this.expire = expire;
            minuteStartTypes = Enum.GetNames(typeof(StartTypes));
            log.LogMethodExit();
        }
        public bool isSupported { get { return supported; } set { supported = value; } }
        public bool canAddMinutes { get { return addMinutes; } set { addMinutes = value; } }
        public bool canExpire { get { return expire; } set { expire = value; } }
        public string[] startTypes { get { return minuteStartTypes; } set { minuteStartTypes = value; } }

    }
}
