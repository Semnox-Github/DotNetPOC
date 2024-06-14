/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - PrivilegesDTO class - This would return the Privileges info
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public  class PrivilegesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool supported;
        private bool expire;
        public PrivilegesDTO()
        {
            log.LogMethodEntry();
            supported = true;
            expire = true;
            log.LogMethodExit();
        }

        public bool isSupported { get { return supported; } set { supported = value; } }
        public bool canExpire { get { return expire; } set { expire = value; } }
    }
}
