/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyContainerDTOCollection Data object of RedemptionCurrencyContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *0.0         05-Nov-2020   Vikas Dwivedi           Created
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RedemptionCurrencyContainerDTO> redemptioCurrencyContainerDTOList;
        private string hash;

        public RedemptionCurrencyContainerDTOCollection()
        {
            log.LogMethodEntry();
            redemptioCurrencyContainerDTOList = new List<RedemptionCurrencyContainerDTO>();
            log.LogMethodExit();
        }

        public RedemptionCurrencyContainerDTOCollection(List<RedemptionCurrencyContainerDTO> redemptioCurrencyContainerDTOList)
        {
            log.LogMethodEntry(redemptioCurrencyContainerDTOList);
            this.redemptioCurrencyContainerDTOList = redemptioCurrencyContainerDTOList;
            if (redemptioCurrencyContainerDTOList == null)
            {
                redemptioCurrencyContainerDTOList = new List<RedemptionCurrencyContainerDTO>();
            }
            hash = new DtoListHash(redemptioCurrencyContainerDTOList);
            log.LogMethodExit();
        }

        public List<RedemptionCurrencyContainerDTO> RedemptionCurrencyContainerDTOList
        {
            get
            {
                return redemptioCurrencyContainerDTOList;
            }

            set
            {
                redemptioCurrencyContainerDTOList = value;
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
