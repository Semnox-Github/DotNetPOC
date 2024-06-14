/********************************************************************************************
 * Project Name - POS                                                                        
 * Description  -AssignCashdrawerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class CashdrawerActivityDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int cashdrawerId;
        private string managerId;
        public CashdrawerActivityDTO()
        {
            log.LogMethodEntry();
            cashdrawerId = -1;
            managerId = string.Empty;
            log.LogMethodExit();
        }

        public CashdrawerActivityDTO(int cashdrawerId, string managerToken)
        {
            log.LogMethodEntry(cashdrawerId, managerToken);
            this.cashdrawerId = cashdrawerId;
            this.managerId = managerToken;
            log.LogMethodExit();
        }
        public int CashdrawerId
        {
            get
            {
                return cashdrawerId;
            }
            set
            {
                cashdrawerId = value;
            }
        }
        public string ManagerId
        {
            get
            {
                return managerId;
            }
            set
            {
                managerId = value;
            }
        }
    }
}
