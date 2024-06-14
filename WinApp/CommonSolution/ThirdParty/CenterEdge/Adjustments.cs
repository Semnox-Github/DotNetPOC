/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - Adjustments class - This would return adjustment details
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

    public class Adjustments
    {
        public Adjustments()
        {
            type = string.Empty;
           // points = new Points();
            amount = new Points();
            groupId = null;
            minutes = null;
            startTimePlay = null;
            count = null;
            expirationDateTime = null;
        }
        public string type { get; set; }
        public Points amount { get; set; }
        public int? groupId { get; set; }
        public int? minutes { get; set; }
        public bool? startTimePlay { get; set; }
        public int? count { get; set; }
        public string expirationDateTime { get; set; }
    }
}
