/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - Privileges class - This would return the Privileges info
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
    /// This class holds the privilege configuarations
    /// </summary>
    public class Privileges
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool supported;
        private bool expire;
        public Privileges()
        {
            /* Since we cannot externally control play privilege expiration using the fields on the API request 
           * to sell play privileges, the capabilities should instead report it is not supported. 
           * This will cause us to suppress UIs for configuring expiration, etc. */
            log.LogMethodEntry();
            supported = false;
            expire = false;  
            log.LogMethodExit();
        }

        public Privileges(bool supported, bool expire)
        {
            log.LogMethodEntry(supported, expire);
            this.supported = supported;
            this.expire = expire;
            log.LogMethodExit();
        }
        public bool isSupported { get { return supported; } set { supported = value; } }
        public bool canExpire { get { return expire; } set { expire = value; } }
    }
}
