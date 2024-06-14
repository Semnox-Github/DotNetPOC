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
  

    public class PointTypes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private RegularPoints regularPoint;
        private BonusPoints bonusPoint;
        private RedemptionTickets redemptionTicket;

        public PointTypes()   
        {
            log.LogMethodEntry();
            regularPoint = new RegularPoints();
            bonusPoint = new BonusPoints();
            redemptionTicket = new RedemptionTickets();
            log.LogMethodExit();
        }

        public PointTypes(RegularPoints regularPoint, BonusPoints bonusPoints, RedemptionTickets redemptionTickets)
        {
            log.LogMethodEntry(regularPoint, bonusPoints, redemptionTickets);
            this.regularPoint = regularPoint;
            this.bonusPoint = bonusPoints;
            this.redemptionTicket = redemptionTickets;
            log.LogMethodExit();
        }

        public RegularPoints regularPoints { get { return regularPoint; } set { regularPoint = value; } }
        public BonusPoints bonusPoints { get { return bonusPoint; } set { bonusPoint = value; } }
        public RedemptionTickets redemptionTickets { get { return redemptionTicket; } set { redemptionTicket = value; } }
    }

    public class RegularPoints
    {
        public bool isSupported { get; set; }
        public int maxDecimalPlaces { get; set; }

        public RegularPoints()
        {
            this.isSupported = true;
            this.maxDecimalPlaces = 0;
        }
        public RegularPoints(bool isSupported, int maxDecimalPlaces)
        {
            this.isSupported = isSupported;
            this.maxDecimalPlaces = maxDecimalPlaces;
        }
    }

    public class BonusPoints
    {
        public bool isSupported { get; set; }
        public int maxDecimalPlaces { get; set; }

        public BonusPoints()
        {
            isSupported = true;
            maxDecimalPlaces = 0;
        }
        public BonusPoints(bool isSupported, int maxDecimalPlaces)
        {
            this.isSupported = isSupported;
            this.maxDecimalPlaces = maxDecimalPlaces;
        }
    }
    public class RedemptionTickets
    {
        public bool isSupported { get; set; }
        public int maxDecimalPlaces { get; set; }

        public RedemptionTickets()
        {
            isSupported = false;
            maxDecimalPlaces = 0;
        }
        public RedemptionTickets(bool isSupported, int maxDecimalPlaces)
        {
            this.isSupported = isSupported;
            this.maxDecimalPlaces = maxDecimalPlaces;
        }
    }
}
