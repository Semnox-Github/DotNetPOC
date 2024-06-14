/********************************************************************************************
 * Project Name - Product Price
 * Description  - Abstract base class of price influencers
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      1-Sep-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Abstract base class of price influencer
    /// </summary>
    public abstract class PriceInfluencer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Splits the ContinuousDateTimeRanges date time range 
        /// </summary>
        /// <param name="continuousDateTimeRanges"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        public abstract ContinuousDateTimeRanges SplitDateTimeRange(ContinuousDateTimeRanges continuousDateTimeRanges, TimeSpan margin);

        /// <summary>
        /// Influence the priceContainerDTOList
        /// </summary>
        /// <param name="priceContainerDTOList"></param>
        /// <returns></returns>
        public abstract List<PriceContainerDTO> Influence(List<PriceContainerDTO> priceContainerDTOList);

        /// <summary>
        /// returns the copy of the price container DTO
        /// </summary>
        /// <param name="priceContainerDTOList"></param>
        /// <returns></returns>
        protected List<PriceContainerDTO> GetCopy(List<PriceContainerDTO> priceContainerDTOList)
        {
            log.LogMethodEntry(priceContainerDTOList);
            List<PriceContainerDTO> result = new List<PriceContainerDTO>();
            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                PriceContainerDTO copy = new PriceContainerDTO(priceContainerDTO);
                result.Add(copy);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
