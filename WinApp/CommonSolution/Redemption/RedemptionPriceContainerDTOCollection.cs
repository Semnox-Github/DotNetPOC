/********************************************************************************************
 * Project Name - Redemption 
 * Description  - Data object of RedemptionPriceContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionPriceContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RedemptionPriceContainerDTO> redemptionPriceContainerDTOList;
        private string hash;

        public RedemptionPriceContainerDTOCollection()
        {
            log.LogMethodEntry();
            redemptionPriceContainerDTOList = new List<RedemptionPriceContainerDTO>();
            log.LogMethodExit();
        }

        public RedemptionPriceContainerDTOCollection(List<RedemptionPriceContainerDTO> redemptionPriceContainerDTOList)
        {
            log.LogMethodEntry(redemptionPriceContainerDTOList);
            this.redemptionPriceContainerDTOList = redemptionPriceContainerDTOList;
            if (this.redemptionPriceContainerDTOList == null)
            {
                this.redemptionPriceContainerDTOList = new List<RedemptionPriceContainerDTO>();
            }
            hash = new DtoListHash(redemptionPriceContainerDTOList);
            log.LogMethodExit();
        }

        public List<RedemptionPriceContainerDTO> RedemptionPriceContainerDTOList
        {
            get
            {
                return redemptionPriceContainerDTOList;
            }

            set
            {
                redemptionPriceContainerDTOList = value;
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
