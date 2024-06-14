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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class Privilege
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int privilegeGroupId;
        private int privilegeCount;
        private string privilegeExpirationDateTime;



        public Privilege()
        {
            log.LogMethodEntry();
            privilegeGroupId = 0;
            privilegeCount = 0;
            privilegeExpirationDateTime ="";
            log.LogMethodExit();
        }

        public Privilege(int groupId, int count, string expirationDateTime)
        {
            log.LogMethodEntry();
            privilegeGroupId = groupId;
            privilegeCount = count;
            privilegeExpirationDateTime = expirationDateTime;
            log.LogMethodExit();
        }

        public int groupId { get { return privilegeGroupId; } set { privilegeGroupId = value; } }
        public int count { get { return privilegeCount; } set { privilegeCount = value; } }
        public string expirationDateTime { get { return privilegeExpirationDateTime; } set { privilegeExpirationDateTime = value; } }

    }
}
