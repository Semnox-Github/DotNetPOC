/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - LoginDTO class represnts the login credentials for the users 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      24-Sep-2020      Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public  class LoginDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string username { get; set; }
        public string passwordHash { get; set; }
        public string requestTimestamp { get; set; }
    }
}
