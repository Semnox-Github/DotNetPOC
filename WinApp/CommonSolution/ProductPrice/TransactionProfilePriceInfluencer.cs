/********************************************************************************************
 * Project Name - Product Price
 * Description  - Represents transaction profile based price influencer
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      1-Sep-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.PriceList;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents transaction profile based price influencer
    /// </summary>
    public class TransactionProfilePriceInfluencer : PriceListPriceInfluencer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TransactionProfileContainerDTO transactionProfileContainerDTO;

        /// <summary>
        /// Parameterized constructor 
        /// </summary>
        public TransactionProfilePriceInfluencer(TransactionProfileContainerDTO transactionProfileContainerDTO, 
                                          Dictionary<int, PriceListDTO> priceListIdPriceListDTODictionary, 
                                          Dictionary<int, Dictionary<int, List<PriceListProductsDTO>>> priceListIdProductIdPriceListProductsDTODictionary)
        :base(priceListIdPriceListDTODictionary, 
              priceListIdProductIdPriceListProductsDTODictionary)
        {
            log.LogMethodEntry(transactionProfileContainerDTO, 
                               priceListIdPriceListDTODictionary, 
                               priceListIdProductIdPriceListProductsDTODictionary);
            this.transactionProfileContainerDTO = transactionProfileContainerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Influence the price container dto
        /// </summary>
        public override List<PriceContainerDTO> Influence(List<PriceContainerDTO> priceContainerDTOList)
        {
            log.LogMethodEntry(priceContainerDTOList);
            if (priceContainerDTOList == null || 
                priceContainerDTOList.Any() == false)
            {
                log.LogMethodExit(priceContainerDTOList);
                return priceContainerDTOList;
            }
            if (transactionProfileContainerDTO == null)
            {
                log.LogMethodExit(priceContainerDTOList, "transactionProfileContainerDTO == null");
                return priceContainerDTOList;
            }
            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                priceContainerDTO.TransactionProfileId = transactionProfileContainerDTO.TransactionProfileId;
                foreach (var priceContainerDetailDTO in priceContainerDTO.PriceContainerDetailDTOList)
                {
                    priceContainerDetailDTO.TransactionProfilePriceListId = transactionProfileContainerDTO.PriceListId;
                    priceContainerDetailDTO.TransactionProfilePrice = GetProductListPrice(transactionProfileContainerDTO.PriceListId, priceContainerDTO.ProductId, priceContainerDetailDTO.StartDateTime);
                }
            }
            log.LogMethodExit(priceContainerDTOList);
            return priceContainerDTOList;
        }

        /// <summary>
        /// returns the referenced price list id
        /// </summary>
        /// <returns></returns>
        protected override List<int> GetReferencedPriceListIdList()
        {
            log.LogMethodEntry();
            List<int> result = new List<int>();
            if(transactionProfileContainerDTO != null &&
                transactionProfileContainerDTO.PriceListId != -1)
            {
                result.Add(transactionProfileContainerDTO.PriceListId);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
