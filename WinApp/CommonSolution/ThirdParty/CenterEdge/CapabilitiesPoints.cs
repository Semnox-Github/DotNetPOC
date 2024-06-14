/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CapabilitiesPoints class - This would return the point types
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
    public class CapabilitiesPoints
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool supported;
        private int maximumDecimalPlaces;
        public CapabilitiesPoints()
        {
            log.LogMethodEntry();
            maxDecimalPlaces = 0;
            isSupported = false;
            log.LogMethodExit();
        }

        public CapabilitiesPoints(bool isSupported, int maxDecimalPlaces)
        {
            log.LogMethodEntry(isSupported, maxDecimalPlaces);
            this.isSupported = isSupported;
            this.maxDecimalPlaces = maxDecimalPlaces;
            log.LogMethodExit();
        }
        public int maxDecimalPlaces { get { return maximumDecimalPlaces; } set { maximumDecimalPlaces = value; } }
        public bool isSupported { get { return supported; } set { supported = value; } }
    }
}
