/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - GamePlayTransaction class - This would return adjustment types
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
    public class GamePlayTransaction : TransactionDTO
    {
        public int gameId { get; set; }
        public string gameDescription { get; set; }
        public Points amount { get; set; }
        public bool usedTimePlay { get; set; }
        public bool usedPlayPrivilege { get; set; }
    }
}
