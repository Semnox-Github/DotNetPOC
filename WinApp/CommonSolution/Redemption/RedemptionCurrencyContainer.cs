/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyServerContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020       Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<RedemptionCurrencyDTO> redemptionCurrencyDTOList;
        private readonly DateTime? redemptionCurrencyLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, RedemptionCurrencyDTO> redemptionCurrencyDTODictionary;

        internal RedemptionCurrencyContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            redemptionCurrencyDTODictionary = new ConcurrentDictionary<int, RedemptionCurrencyDTO>();
            redemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();
            RedemptionCurrencyList redemptionCurrencyListBL = new RedemptionCurrencyList();
            redemptionCurrencyLastUpdateTime = redemptionCurrencyListBL.GetRedemptionCurrencyLastUpdateTime(siteId);

            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, siteId.ToString()));
            redemptionCurrencyDTOList = redemptionCurrencyListBL.GetAllRedemptionCurrency(searchParameters);
            if (redemptionCurrencyDTOList != null && redemptionCurrencyDTOList.Any())
            {
                foreach (RedemptionCurrencyDTO redemptionCurrencyDTO in redemptionCurrencyDTOList)
                {
                    redemptionCurrencyDTODictionary[redemptionCurrencyDTO.CurrencyId] = redemptionCurrencyDTO;
                }
            }
            else
            {
                redemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();
                redemptionCurrencyDTODictionary = new ConcurrentDictionary<int, RedemptionCurrencyDTO>();
            }
            log.LogMethodExit();
        }

        internal double GetValueInTickets(int currencyId)
        {
            if(redemptionCurrencyDTODictionary.ContainsKey(currencyId) == false)
            {
                string errorMessage = "Currency with currencyId :" + currencyId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            double result = redemptionCurrencyDTODictionary[currencyId].ValueInTickets;
            log.LogMethodExit(result);
            return result;
        }

        public List<RedemptionCurrencyContainerDTO> GetRedemptionCurrencyContainerDTOList()
        {
            log.LogMethodEntry();
            List<RedemptionCurrencyContainerDTO> redemptionCurrencyViewDTOList = new List<RedemptionCurrencyContainerDTO>();
            foreach (RedemptionCurrencyDTO redemptionCurrencyDTO in redemptionCurrencyDTOList)
            {
                RedemptionCurrencyContainerDTO redemptionCurrencyViewDTO = new RedemptionCurrencyContainerDTO(redemptionCurrencyDTO.CurrencyId,
                                                                                                              redemptionCurrencyDTO.ProductId,
                                                                                                              redemptionCurrencyDTO.CurrencyName,
                                                                                                              redemptionCurrencyDTO.ValueInTickets,
                                                                                                              redemptionCurrencyDTO.BarCode,
                                                                                                              redemptionCurrencyDTO.ShowQtyPrompt,
                                                                                                              redemptionCurrencyDTO.ManagerApproval,
                                                                                                              redemptionCurrencyDTO.ShortCutKeys);
                redemptionCurrencyViewDTOList.Add(redemptionCurrencyViewDTO);
            }
            log.LogMethodExit(redemptionCurrencyViewDTOList);
            return redemptionCurrencyViewDTOList;
        }

        public RedemptionCurrencyContainer Refresh()
        {
            log.LogMethodEntry();
            RedemptionCurrencyList redemptionCurrencyListBL = new RedemptionCurrencyList();
            DateTime? updateTime = redemptionCurrencyListBL.GetRedemptionCurrencyLastUpdateTime(siteId);
            if (redemptionCurrencyLastUpdateTime.HasValue
                && redemptionCurrencyLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in RedemptionCurrency since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            RedemptionCurrencyContainer result = new RedemptionCurrencyContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
