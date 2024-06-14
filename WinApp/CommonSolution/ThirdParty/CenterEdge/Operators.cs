/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - Operators class represents the stafs details to create card
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
    public class Operators
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Operators()
        {
            operators = new OperatorDTO();
        }
        public OperatorDTO operators { get; set; }

    }
}
