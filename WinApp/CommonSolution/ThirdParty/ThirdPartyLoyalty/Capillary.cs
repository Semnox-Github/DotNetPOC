/********************************************************************************************
* Project Name - Loyalty
* Description  - Capillary - Class Capillary Loyalty programs
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/

using Semnox.Core.Utilities;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    public class Capillary : LoyaltyPrograms
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;

        public Capillary(Utilities _utilities) : base(_utilities)
        {
            log.LogMethodEntry(_utilities);
            Utilities = _utilities;
            log.LogMethodExit(null);
        }
    }
}
