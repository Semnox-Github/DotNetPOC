/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - OperatorDTO class represnts the stafs details to create card
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
    public class OperatorDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public OperatorDTO()
        {
            log.LogMethodEntry();
            employeeName = string.Empty;
            employeeNumber = 0;
            stationName = string.Empty;
            stationNumber = 0;
            log.LogMethodExit();
        }

        public string employeeName { get; set; }
        public int employeeNumber { get; set; }
        public string stationName { get; set; }
        public int stationNumber { get; set; }


    }
}
