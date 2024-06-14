/********************************************************************************************
 * Project Name - Achievements
 * Description  - LoyaltyAttributeContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
2.120.00    04-Mar-2021       Roshan Devadiga         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    public class LoyaltyAttributeContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<LoyaltyAttributesDTO> loyaltyAttributeDTOList;
        private readonly DateTime? loyaltyAttributeLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, LoyaltyAttributesDTO> loyaltyAttributeDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal LoyaltyAttributeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            loyaltyAttributeDTODictionary = new ConcurrentDictionary<int, LoyaltyAttributesDTO>();
            loyaltyAttributeDTOList = new List<LoyaltyAttributesDTO>();
            LoyaltyAttributeListBL loyaltyAttributeListBL = new LoyaltyAttributeListBL(executionContext);
            loyaltyAttributeLastUpdateTime = loyaltyAttributeListBL.GetLoyaltyAttributeLastUpdateTime(siteId);

            List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>(LoyaltyAttributesDTO.SearchByParameters.IS_ACTIVE, "1"));
            SearchParameters.Add(new KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>(LoyaltyAttributesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            loyaltyAttributeDTOList = loyaltyAttributeListBL.GetAllLoyaltyAttributesList(SearchParameters);
            if (loyaltyAttributeDTOList != null && loyaltyAttributeDTOList.Any())
            {
                foreach (LoyaltyAttributesDTO loyaltyAttributeDTO in loyaltyAttributeDTOList)
                {
                    loyaltyAttributeDTODictionary[loyaltyAttributeDTO.LoyaltyAttributeId] = loyaltyAttributeDTO;
                }
            }
            else
            {
                loyaltyAttributeDTOList = new List<LoyaltyAttributesDTO>();
                loyaltyAttributeDTODictionary = new ConcurrentDictionary<int, LoyaltyAttributesDTO>();
            }
            log.LogMethodExit();
        }
        public List<LoyaltyAttributeContainerDTO> GetLoyaltyAttributeContainerDTOList()
        {
            log.LogMethodEntry();
            List<LoyaltyAttributeContainerDTO> loyaltyAttributeViewDTOList = new List<LoyaltyAttributeContainerDTO>();
            foreach (LoyaltyAttributesDTO loyaltyAttributeDTO in loyaltyAttributeDTOList)
            {

                LoyaltyAttributeContainerDTO loyaltyAttributeViewDTO = new LoyaltyAttributeContainerDTO(loyaltyAttributeDTO.LoyaltyAttributeId,
                                                                                loyaltyAttributeDTO.Attribute,
                                                                                loyaltyAttributeDTO.PurchaseApplicable,
                                                                                loyaltyAttributeDTO.ConsumptionApplicable,
                                                                                loyaltyAttributeDTO.DBColumnName
                                                                                );

                loyaltyAttributeViewDTOList.Add(loyaltyAttributeViewDTO);
            }
            log.LogMethodExit(loyaltyAttributeViewDTOList);
            return loyaltyAttributeViewDTOList;
        }

        public LoyaltyAttributeContainer Refresh()
        {
            log.LogMethodEntry();
            LoyaltyAttributeListBL loyaltyAttributeListBL = new LoyaltyAttributeListBL(executionContext);
            DateTime? updateTime = loyaltyAttributeListBL.GetLoyaltyAttributeLastUpdateTime(siteId);
            if (loyaltyAttributeLastUpdateTime.HasValue
                && loyaltyAttributeLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in LoyaltyAttribute since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            LoyaltyAttributeContainer result = new LoyaltyAttributeContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
