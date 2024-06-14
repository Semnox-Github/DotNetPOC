/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - PointTypeDTO class - This would return the point types
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
    public class Points
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Points()
        {
            regularPoints = 0;
            bonusPoints = 0;
            redemptionTickets = 0;
        }
        public decimal regularPoints { get; set; }
        public decimal bonusPoints { get; set; }
        public decimal redemptionTickets { get; set; }
    }
}
