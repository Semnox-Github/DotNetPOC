using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleCalculator
    {
        private readonly IRedemptionCurrencyRuleProvider redemptionCurrencyRuleProvider;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RedemptionCurrencyRuleCalculator(IRedemptionCurrencyRuleProvider redemptionCurrencyRuleProvider)
        {
            log.LogMethodEntry(redemptionCurrencyRuleProvider);
            this.redemptionCurrencyRuleProvider = redemptionCurrencyRuleProvider;
            log.LogMethodExit();
        }

        public List<RedemptionCardsDTO> Calculate(List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            log.LogMethodEntry(redemptionCardsDTOList);
            List<RedemptionCurrencyRuleContainerDTO> redemptionCurrencyRuleContainerDTOList = redemptionCurrencyRuleProvider.GetRedemptionCurrencyRuleContainerDTOList();
            if (redemptionCurrencyRuleContainerDTOList == null || redemptionCurrencyRuleContainerDTOList.Any() == false)
            {
                log.LogMethodExit(redemptionCardsDTOList, "No currency rules are defined");
                return redemptionCardsDTOList;
            }
            List<RedemptionCurrencyRuleContainerDTO> sortedRedemptionCurrencyRuleContainerDTOList = redemptionCurrencyRuleContainerDTOList.OrderBy(x => x.Priority).ToList();
            List<RedemptionCardsDTO> result = new List<RedemptionCardsDTO>();
            Dictionary<int, List<RedemptionCardsDTO>> currencyIdRedemptionCardsDTODictionary = new Dictionary<int, List<RedemptionCardsDTO>>();
            foreach (var redemptionCardsDTO in redemptionCardsDTOList)
            {
                if (redemptionCardsDTO.CurrencyRuleId.HasValue && redemptionCardsDTO.CurrencyRuleId > -1)
                {
                    continue;
                }

                if (redemptionCardsDTO.CurrencyId.HasValue == false || redemptionCardsDTO.CurrencyId <= -1)
                {
                    continue;
                }
                redemptionCardsDTO.SourceCurrencyRuleId = null;
                if (currencyIdRedemptionCardsDTODictionary.ContainsKey(redemptionCardsDTO.CurrencyId.Value) == false)
                {
                    currencyIdRedemptionCardsDTODictionary.Add(redemptionCardsDTO.CurrencyId.Value, new List<RedemptionCardsDTO>());
                }
                result.Add(redemptionCardsDTO);
                currencyIdRedemptionCardsDTODictionary[redemptionCardsDTO.CurrencyId.Value].Add(redemptionCardsDTO);
                if (redemptionCardsDTO.CurrencyQuantity > 1)
                {
                    for (int i = 1; i < redemptionCardsDTO.CurrencyQuantity; i++)
                    {
                        RedemptionCardsDTO singleQuantityRedemptionCardsDTO = CreateSingleQuantityRedemptionCardsDTO(redemptionCardsDTO);
                        result.Add(singleQuantityRedemptionCardsDTO);
                        currencyIdRedemptionCardsDTODictionary[redemptionCardsDTO.CurrencyId.Value].Add(singleQuantityRedemptionCardsDTO);
                    }
                    redemptionCardsDTO.CurrencyQuantity = 1;
                }
            }

            foreach (var redemptionCurrencyRuleContainerDTO in sortedRedemptionCurrencyRuleContainerDTOList)
            {
                List<RedemptionCardsDTO> rewardRedemptionCardsDTOList = Apply(currencyIdRedemptionCardsDTODictionary, redemptionCurrencyRuleContainerDTO);
                result.AddRange(rewardRedemptionCardsDTOList);
            }

            log.LogMethodExit(result);
            return result;
        }

        private List<RedemptionCardsDTO> Apply(Dictionary<int, List<RedemptionCardsDTO>> currencyIdRedemptionCardsDTODictionary, RedemptionCurrencyRuleContainerDTO redemptionCurrencyRuleContainerDTO)
        {
            log.LogMethodEntry(currencyIdRedemptionCardsDTODictionary, redemptionCurrencyRuleContainerDTO);
            List<RedemptionCardsDTO> rewardRedemptionCardsDTOList = new List<RedemptionCardsDTO>();
            do
            {
                if (CanApply(currencyIdRedemptionCardsDTODictionary, redemptionCurrencyRuleContainerDTO) == false)
                {
                    break;
                }
                foreach (var redemptionCurrencyRuleDetailContainerDTO in redemptionCurrencyRuleContainerDTO.RedemptionCurrencyRuleDetailContainerDTOList)
                {
                    if (redemptionCurrencyRuleDetailContainerDTO.Quantity.HasValue == false ||
                       redemptionCurrencyRuleDetailContainerDTO.Quantity.Value <= 0)
                    {
                        continue;
                    }
                    List<RedemptionCardsDTO> redemptionCardsDTOList = currencyIdRedemptionCardsDTODictionary[redemptionCurrencyRuleDetailContainerDTO.CurrencyId].GetRange(0, redemptionCurrencyRuleDetailContainerDTO.Quantity.Value);
                    currencyIdRedemptionCardsDTODictionary[redemptionCurrencyRuleDetailContainerDTO.CurrencyId].RemoveRange(0, redemptionCurrencyRuleDetailContainerDTO.Quantity.Value);
                    foreach (var redemptionCardsDTO in redemptionCardsDTOList)
                    {
                        redemptionCardsDTO.SourceCurrencyRuleId = redemptionCurrencyRuleContainerDTO.RedemptionCurrencyRuleId;
                    }
                }
                rewardRedemptionCardsDTOList.Add(CreateRewardRedemptionCardsDTO(redemptionCurrencyRuleContainerDTO));
            } while (redemptionCurrencyRuleContainerDTO.Cumulative);
            log.LogMethodExit(rewardRedemptionCardsDTOList);
            return rewardRedemptionCardsDTOList;
        }

        private RedemptionCardsDTO CreateRewardRedemptionCardsDTO(RedemptionCurrencyRuleContainerDTO redemptionCurrencyRuleContainerDTO)
        {
            log.LogMethodEntry(redemptionCurrencyRuleContainerDTO);
            int rewardTicket = GetRewardTickets(redemptionCurrencyRuleContainerDTO);
            RedemptionCardsDTO result = new RedemptionCardsDTO(-1, -1, string.Empty, -1, rewardTicket, -1, 1, rewardTicket, string.Empty, null,null);
            result.CurrencyRuleId = redemptionCurrencyRuleContainerDTO.RedemptionCurrencyRuleId;
            log.LogMethodExit(result);
            return result;
        }

        private int GetRewardTickets(RedemptionCurrencyRuleContainerDTO redemptionCurrencyRuleContainerDTO)
        {
            int result = 0;
            double totalValueIntikets = 0;
            foreach (var redemptionCurrencyRuleDetailContainerDTO in redemptionCurrencyRuleContainerDTO.RedemptionCurrencyRuleDetailContainerDTOList)
            {
                if (redemptionCurrencyRuleDetailContainerDTO.Quantity.HasValue == false ||
                   redemptionCurrencyRuleDetailContainerDTO.Quantity.Value <= 0)
                {
                    continue;
                }
                totalValueIntikets += redemptionCurrencyRuleProvider.GetValueInTickets(redemptionCurrencyRuleDetailContainerDTO.CurrencyId) * redemptionCurrencyRuleDetailContainerDTO.Quantity.Value;
            }
            if (redemptionCurrencyRuleContainerDTO.Percentage > 0)
            {
                result = (int)((decimal)totalValueIntikets * redemptionCurrencyRuleContainerDTO.Percentage / 100m);
            }
            else if (redemptionCurrencyRuleContainerDTO.Amount > 0)
            {
                result = (int)(redemptionCurrencyRuleContainerDTO.Amount - (decimal)totalValueIntikets);
                if (result < 0)
                {
                    result = 0;
                }
            }
            else
            {
                result = 0;
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool CanApply(Dictionary<int, List<RedemptionCardsDTO>> currencyIdRedemptionCardsDTODictionary, RedemptionCurrencyRuleContainerDTO redemptionCurrencyRuleContainerDTO)
        {
            log.LogMethodEntry(currencyIdRedemptionCardsDTODictionary, redemptionCurrencyRuleContainerDTO);
            bool result = false;
            foreach (RedemptionCurrencyRuleDetailContainerDTO redemptionCurrencyRuleDetailContainerDTO in redemptionCurrencyRuleContainerDTO.RedemptionCurrencyRuleDetailContainerDTOList)
            {
                if (redemptionCurrencyRuleDetailContainerDTO.Quantity.HasValue == false ||
                   redemptionCurrencyRuleDetailContainerDTO.Quantity <= 0)
                {
                    continue;
                }
                if (currencyIdRedemptionCardsDTODictionary.ContainsKey(redemptionCurrencyRuleDetailContainerDTO.CurrencyId) == false)
                {
                    log.LogMethodExit(result, "Unable to find currency Id: " + redemptionCurrencyRuleDetailContainerDTO.CurrencyId + " rule detail id: " + redemptionCurrencyRuleDetailContainerDTO.RedemptionCurrencyRuleDetailId);
                    return result;
                }
                int actualCount = currencyIdRedemptionCardsDTODictionary[redemptionCurrencyRuleDetailContainerDTO.CurrencyId].Count;
                if (actualCount < redemptionCurrencyRuleDetailContainerDTO.Quantity)
                {
                    log.LogMethodExit(result, "No of currency with currency Id: " + redemptionCurrencyRuleDetailContainerDTO.CurrencyId + " is : " + actualCount + " but expected count: " + redemptionCurrencyRuleDetailContainerDTO.Quantity + " rule detail id: " + redemptionCurrencyRuleDetailContainerDTO.RedemptionCurrencyRuleDetailId);
                    return result;
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        private RedemptionCardsDTO CreateSingleQuantityRedemptionCardsDTO(RedemptionCardsDTO redemptionCardsDTO)
        {
            log.LogMethodEntry(redemptionCardsDTO);
            RedemptionCardsDTO result = new RedemptionCardsDTO(-1, -1, string.Empty, -1, (int)redemptionCurrencyRuleProvider.GetValueInTickets(redemptionCardsDTO.CurrencyId.Value), redemptionCardsDTO.CurrencyId, 1, redemptionCardsDTO.CurrencyValueInTickets, redemptionCardsDTO.CurrencyName, null, redemptionCardsDTO.ViewGroupingNumber);
            log.LogMethodExit(result);
            return result;
        }
    }
}
