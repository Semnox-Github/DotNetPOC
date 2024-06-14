/********************************************************************************************
 * Project Name - Product Price
 * Description  - Abstract base class of price list based price influencer
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      1-Sep-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.PriceList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Abstract base class of price list based price influencer
    /// </summary>
    public abstract class PriceListPriceInfluencer: PriceInfluencer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// price list id priceListDTO dictionary
        /// </summary>
        protected readonly Dictionary<int, PriceListDTO> priceListIdPriceListDTODictionary;
        /// <summary>
        /// priceListId ProductId PriceListProductsDTO Dictionary
        /// </summary>
        protected readonly Dictionary<int, Dictionary<int, List<PriceListProductsDTO>>> priceListIdProductIdPriceListProductsDTODictionary;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        protected PriceListPriceInfluencer(Dictionary<int, PriceListDTO> priceListIdPriceListDTODictionary, Dictionary<int, Dictionary<int, List<PriceListProductsDTO>>> priceListIdProductIdPriceListProductsDTODictionary)
        {
            log.LogMethodEntry(priceListIdPriceListDTODictionary, priceListIdProductIdPriceListProductsDTODictionary);
            this.priceListIdPriceListDTODictionary = priceListIdPriceListDTODictionary;
            this.priceListIdProductIdPriceListProductsDTODictionary = priceListIdProductIdPriceListProductsDTODictionary;
            log.LogMethodExit();
        }

        /// <summary>
        /// returns the price list price for a given product
        /// </summary>
        protected decimal? GetProductListPrice(int priceListId, int productId, DateTime startDateTime)
        {
            log.LogMethodEntry(priceListId, productId, startDateTime);
            decimal? result = null;
            if (priceListIdProductIdPriceListProductsDTODictionary == null ||
                priceListIdProductIdPriceListProductsDTODictionary.ContainsKey(priceListId) == false ||
               priceListIdProductIdPriceListProductsDTODictionary[priceListId].ContainsKey(productId) == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            var priceListProductsDTOList = priceListIdProductIdPriceListProductsDTODictionary[priceListId][productId]
                                          .Where(x => x.EffectiveDate.HasValue == false || x.EffectiveDate.Value <= startDateTime)
                                          .OrderByDescending(x => x.EffectiveDate.HasValue? x.EffectiveDate : DateTime.MinValue);
            PriceListProductsDTO priceListProductsDTO = priceListProductsDTOList.FirstOrDefault();
            if (priceListProductsDTO != null)
            {
                result = priceListProductsDTO.Price;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the referenced price list Id list
        /// </summary>
        /// <returns></returns>
        protected abstract List<int> GetReferencedPriceListIdList();


        /// <summary>
        /// Splits the Continuous DateTimeRanges
        /// </summary>
        public override ContinuousDateTimeRanges SplitDateTimeRange(ContinuousDateTimeRanges continuousDateTimeRanges, TimeSpan margin)
        {
            log.LogMethodEntry(continuousDateTimeRanges, margin);
            ContinuousDateTimeRanges result = continuousDateTimeRanges;
            List<int> priceListIdList = GetReferencedPriceListIdList();
            if (priceListIdList.Any() == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            foreach (var priceListId in priceListIdList)
            {
                result = SplitDateTimeRangeBasedOnPriceList(priceListId, result, margin);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// splits the continuous date time range based on price list effective date
        /// </summary>
        protected ContinuousDateTimeRanges SplitDateTimeRangeBasedOnPriceList(int priceListId, ContinuousDateTimeRanges continuousDateTimeRanges, TimeSpan margin)
        {
            log.LogMethodEntry(priceListId, continuousDateTimeRanges, margin);
            ContinuousDateTimeRanges result = continuousDateTimeRanges;
            if (priceListIdPriceListDTODictionary == null ||
                priceListIdPriceListDTODictionary.ContainsKey(priceListId) == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            PriceListDTO priceListDTO = priceListIdPriceListDTODictionary[priceListId];
            if (priceListDTO.PriceListProductsList == null || priceListDTO.PriceListProductsList.Any() == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            foreach (var priceListProductsDTO in priceListDTO.PriceListProductsList)
            {
                if (priceListProductsDTO.EffectiveDate.HasValue == false)
                {
                    continue;
                }
                result = result.Split(priceListProductsDTO.EffectiveDate.Value, margin);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
