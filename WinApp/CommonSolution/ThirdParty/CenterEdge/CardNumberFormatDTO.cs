/********************************************************************************************
 * Project Name - CenterEdge
 * Description  - CardNumberFormatDTO class This would hold the card format info that are configured in the system.
 *
 **************
 ** Version Log
 **************
 * Version     Date Modified By Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge REST API
 ********************************************************************************************/

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class CardNumberFormatDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int minmumLength;
        private int maxmumLength;
        private string numberPrefix;
        public CardNumberFormatDTO()
        {
            log.LogMethodEntry();
            minmumLength = 8;
            maxmumLength = 8;
            numberPrefix = string.Empty;
            log.LogMethodExit();
        }

        public CardNumberFormatDTO(int minLength, int maxLength, string numberPrefix)
        {
            log.LogMethodEntry(minLength, maxLength, prefix);
            this.minmumLength = minLength;
            this.maxmumLength = maxLength;
            this.numberPrefix = prefix;
            log.LogMethodExit();
        }
        public int minLength { get { return minmumLength; } set { minmumLength = value; } }
        public int maxLength { get { return maxmumLength; } set { maxmumLength = value; } }
        public string prefix { get { return numberPrefix; } set { numberPrefix = value; } }
    }
}
