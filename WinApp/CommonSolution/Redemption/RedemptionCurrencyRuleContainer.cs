/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - Holds the currency rules for a given site
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi            Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList;
        private readonly DateTime? redemptionCurrencyRuleLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTODictionary;
        private readonly RedemptionCurrencyRuleContainerDTOCollection redemptionCurrencyRuleContainerDTOCollection;

        internal RedemptionCurrencyRuleContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            redemptionCurrencyRuleDTODictionary = new ConcurrentDictionary<int, RedemptionCurrencyRuleDTO>();
            List<RedemptionCurrencyRuleContainerDTO> redemptionCurrencyRuleContainerDTOList = new List<RedemptionCurrencyRuleContainerDTO>();
            try
            {
                RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL();
                redemptionCurrencyRuleLastUpdateTime = redemptionCurrencyRuleListBL.GetRedemptionCurrencyRuleLastUpdateTime(siteId);

                List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>>();
                searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, siteId.ToString()));
                redemptionCurrencyRuleDTOList = redemptionCurrencyRuleListBL.GetAllRedemptionCurrencyRuleList(searchParameters,true);
                if (redemptionCurrencyRuleDTOList != null && redemptionCurrencyRuleDTOList.Any())
                {
                    foreach (RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO in redemptionCurrencyRuleDTOList)
                    {
                        redemptionCurrencyRuleDTODictionary[redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId] = redemptionCurrencyRuleDTO;
                        if (IsValid(redemptionCurrencyRuleDTO) == false)
                        {
                            continue;
                        }
                        RedemptionCurrencyRuleContainerDTO redemptionCurrencyRuleContainerDTO = CreateRedemptionCurrencyRuleContainerDTO(redemptionCurrencyRuleDTO);
                        redemptionCurrencyRuleContainerDTOList.Add(redemptionCurrencyRuleContainerDTO);
                    }

                }
                else
                {
                    redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the system option container.", ex);
                redemptionCurrencyRuleLastUpdateTime = null;
                redemptionCurrencyRuleDTODictionary.Clear();
                redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
                redemptionCurrencyRuleContainerDTOList.Clear();
            }
            redemptionCurrencyRuleContainerDTOCollection = new RedemptionCurrencyRuleContainerDTOCollection(redemptionCurrencyRuleContainerDTOList);
            log.LogMethodExit();
        }

        private RedemptionCurrencyRuleContainerDTO CreateRedemptionCurrencyRuleContainerDTO(RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO);
            RedemptionCurrencyRuleContainerDTO result = new RedemptionCurrencyRuleContainerDTO(redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId,
                                                                                                redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName, 
                                                                                                redemptionCurrencyRuleDTO.Description, 
                                                                                                redemptionCurrencyRuleDTO.Percentage, 
                                                                                                redemptionCurrencyRuleDTO.Amount, 
                                                                                                redemptionCurrencyRuleDTO.Priority, 
                                                                                                redemptionCurrencyRuleDTO.Cumulative);
            foreach (var redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)
            {
                result.RedemptionCurrencyRuleDetailContainerDTOList.Add(new RedemptionCurrencyRuleDetailContainerDTO(redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleDetailId, 
                                                                                                                     redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleId, 
                                                                                                                     redemptionCurrencyRuleDetailDTO.CurrencyId, 
                                                                                                                     redemptionCurrencyRuleDetailDTO.Quantity));
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool IsValid(RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO);
            bool result = false;
            try
            {
                if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId == -1)
                {
                    log.LogMethodExit(result, "RedemptionCurrencyRuleId == -1");
                    return result;
                }
                if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList == null
                    || redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Any(x => x.IsActive) == false)
                {
                    log.LogMethodExit(result, "RedemptionCurrencyRuleDetailDTOList is empty");
                    return result;
                }
                result = true;
            }
            catch (Exception ex)
            {
                log.Error("RedemptionCurrencyRuleDTO is not valid", ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<RedemptionCurrencyRuleContainerDTO> GetRedemptionCurrencyRuleContainerDTOList()
        {
            log.LogMethodEntry();
            List<RedemptionCurrencyRuleContainerDTO> result = redemptionCurrencyRuleContainerDTOCollection.RedemptionCurrencyRuleContainerDTOList;
            log.LogMethodExit(result);
            return result;
        }

        internal RedemptionCurrencyRuleContainerDTOCollection GetRedemptionCurrencyRuleContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(redemptionCurrencyRuleContainerDTOCollection);
            return redemptionCurrencyRuleContainerDTOCollection;
        }

        internal RedemptionCurrencyRuleContainer Refresh()
        {
            log.LogMethodEntry();
            RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL();
            DateTime? updateTime = redemptionCurrencyRuleListBL.GetRedemptionCurrencyRuleLastUpdateTime(siteId);
            if (redemptionCurrencyRuleLastUpdateTime.HasValue
                && redemptionCurrencyRuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in RedemptionCurrencyRule since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            RedemptionCurrencyRuleContainer result = new RedemptionCurrencyRuleContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
