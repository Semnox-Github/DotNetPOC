/********************************************************************************************
 * Project Name - Achievements
 * Description  - LoyaltyAttributeContainerDTOCollection class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    04-Mar-2021      Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    public class LoyaltyAttributeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LoyaltyAttributeContainerDTO> loyaltyAttributeContainerDTOList;
        private string hash;

        public LoyaltyAttributeContainerDTOCollection()
        {
            log.LogMethodEntry();
            loyaltyAttributeContainerDTOList = new List<LoyaltyAttributeContainerDTO>();
            log.LogMethodExit();
        }
        public LoyaltyAttributeContainerDTOCollection(List<LoyaltyAttributeContainerDTO> loyaltyAttributeContainerDTOList)
        {
            log.LogMethodEntry(loyaltyAttributeContainerDTOList);
            this.loyaltyAttributeContainerDTOList = loyaltyAttributeContainerDTOList;
            if (loyaltyAttributeContainerDTOList == null)
            {
                loyaltyAttributeContainerDTOList = new List<LoyaltyAttributeContainerDTO>();
            }
            hash = new DtoListHash(loyaltyAttributeContainerDTOList);
            log.LogMethodExit();
        }

        public List<LoyaltyAttributeContainerDTO> LoyaltyAttributeContainerDTOList
        {
            get
            {
                return loyaltyAttributeContainerDTOList;
            }

            set
            {
                loyaltyAttributeContainerDTOList = value;
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
