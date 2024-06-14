/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - TimePlayGroupDTO class-  This would hold the time play products
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
    public class TimePlayGroupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId;
        private string productName;
        public TimePlayGroupDTO()
        {
            log.LogMethodEntry();
            productId = -1;
            productName = string.Empty;
            log.LogMethodExit();
        }

        public TimePlayGroupDTO(int id, string name)
        {
            log.LogMethodEntry();
            this.id = id;
            this.name = name;
            log.LogMethodExit();
        }
        public int id { get { return productId; } set { productId = value; } }
        public string name { get { return productName; } set { productName = value; } }
    }
}
