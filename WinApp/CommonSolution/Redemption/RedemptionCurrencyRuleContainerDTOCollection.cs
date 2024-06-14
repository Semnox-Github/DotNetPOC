/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyRuleContainerDTOCollection Data object of RedemptionCurrencyRuleContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *0.0         05-Nov-2020   Vikas Dwivedi          Created
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RedemptionCurrencyRuleContainerDTO> redemptionCurrencyRuleContainerDTOList;
        private string hash;

        public RedemptionCurrencyRuleContainerDTOCollection()
        {
            log.LogMethodEntry();
            redemptionCurrencyRuleContainerDTOList = new List<RedemptionCurrencyRuleContainerDTO>();
            log.LogMethodExit();
        }

        public RedemptionCurrencyRuleContainerDTOCollection(List<RedemptionCurrencyRuleContainerDTO> redemptionCurrencyRuleContainerDTOList)
        {
            log.LogMethodEntry(redemptionCurrencyRuleContainerDTOList);
            this.redemptionCurrencyRuleContainerDTOList = redemptionCurrencyRuleContainerDTOList;
            if (redemptionCurrencyRuleContainerDTOList == null)
            {
                redemptionCurrencyRuleContainerDTOList = new List<RedemptionCurrencyRuleContainerDTO>();
            }
            hash = new DtoListHash(redemptionCurrencyRuleContainerDTOList);
            log.LogMethodExit();
        }
      
        public List<RedemptionCurrencyRuleContainerDTO> RedemptionCurrencyRuleContainerDTOList
        {
            get
            {
                return redemptionCurrencyRuleContainerDTOList;
            }

            set
            {
                redemptionCurrencyRuleContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}
