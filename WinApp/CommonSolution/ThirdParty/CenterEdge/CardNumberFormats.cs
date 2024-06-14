/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CardNumberFormatDTO class This would hold the card number formats that are allowed in the system. 
 *                        In case of Parafait, it would be fetched from the Parafait defaults – Card number length field. 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/

using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class CardNumberFormats
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int skippedRecords;
        private int totalRecordCount;
        private List<CardNumberFormatDTO> cardNumberFormatDTOList;

        public CardNumberFormats()
        {
            log.LogMethodEntry();
            cardNumberFormatDTOList = new List<CardNumberFormatDTO>();
            skippedRecords = 0;
            totalRecordCount = 0;
            log.LogMethodExit();
        }

        public CardNumberFormats(int skipped, int totalCount)
        {
            log.LogMethodEntry(totalCount, skipped);
            this.skipped = skipped;
            this.totalRecordCount = totalCount;
            log.LogMethodExit();
        }
        public int skipped { get { return skippedRecords; } set { skippedRecords = value; } }
        public int totalCount { get { return totalRecordCount; } set { totalRecordCount = value; } }
        public List<CardNumberFormatDTO> formats { get { return cardNumberFormatDTOList; } set { cardNumberFormatDTOList = value; } }
    }
}
