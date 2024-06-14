/********************************************************************************************
 * Project Name - Promotions
 * Description  - LoyaltyRedemptionRuleContainerDTOCollection class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    05-Mar-2021      Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    public class LoyaltyRedemptionRuleContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleContainerDTOList;
        private string hash;

        public LoyaltyRedemptionRuleContainerDTOCollection()
        {
            log.LogMethodEntry();
            loyaltyRedemptionRuleContainerDTOList = new List<LoyaltyRedemptionRuleContainerDTO>();
            log.LogMethodExit();
        }
        public LoyaltyRedemptionRuleContainerDTOCollection(List<LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleContainerDTOList)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleContainerDTOList);
            this.loyaltyRedemptionRuleContainerDTOList = loyaltyRedemptionRuleContainerDTOList;
            if (loyaltyRedemptionRuleContainerDTOList == null)
            {
                loyaltyRedemptionRuleContainerDTOList = new List<LoyaltyRedemptionRuleContainerDTO>();
            }
            hash = new DtoListHash(loyaltyRedemptionRuleContainerDTOList);
            log.LogMethodExit();
        }

        public List<LoyaltyRedemptionRuleContainerDTO> LoyaltyRedemptionRuleContainerDTOList
        {
            get
            {
                return loyaltyRedemptionRuleContainerDTOList;
            }

            set
            {
                loyaltyRedemptionRuleContainerDTOList = value;
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
