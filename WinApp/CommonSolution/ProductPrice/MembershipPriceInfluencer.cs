/********************************************************************************************
 * Project Name - Product Price
 * Description  - Represents membership based price influencer
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
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.PriceList;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents membership based price influencer
    /// </summary>
    public class MembershipPriceInfluencer : PriceListPriceInfluencer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MembershipContainerDTO membershipContainerDTO;

        /// <summary>
        /// Parameterized constructor 
        /// </summary>
        public MembershipPriceInfluencer(MembershipContainerDTO membershipContainerDTO,
                                          Dictionary<int, PriceListDTO> priceListIdPriceListDTODictionary,
                                          Dictionary<int, Dictionary<int, List<PriceListProductsDTO>>> priceListIdProductIdPriceListProductsDTODictionary)
        : base(priceListIdPriceListDTODictionary,
              priceListIdProductIdPriceListProductsDTODictionary)
        {
            log.LogMethodEntry(membershipContainerDTO,
                               priceListIdPriceListDTODictionary,
                               priceListIdProductIdPriceListProductsDTODictionary);
            this.membershipContainerDTO = membershipContainerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Influence the price container dto
        /// </summary>
        public override List<PriceContainerDTO> Influence(List<PriceContainerDTO> priceContainerDTOList)
        {
            log.LogMethodEntry(priceContainerDTOList);
            if(priceContainerDTOList == null || 
               priceContainerDTOList.Any() == false)
            {
                log.LogMethodExit(priceContainerDTOList);
                return priceContainerDTOList;
            }
            if (membershipContainerDTO == null)
            {
                log.LogMethodExit(priceContainerDTOList, "membershipContainerDTO == null");
                return priceContainerDTOList;
            }
            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                priceContainerDTO.MembershipId = membershipContainerDTO.MembershipId;
                foreach (var priceContainerDetailDTO in priceContainerDTO.PriceContainerDetailDTOList)
                {
                    priceContainerDetailDTO.MembershipPriceListId = membershipContainerDTO.PriceListId;
                    priceContainerDetailDTO.MembershipPrice = GetProductListPrice(membershipContainerDTO.PriceListId, priceContainerDTO.ProductId, priceContainerDetailDTO.StartDateTime);
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
            if(membershipContainerDTO != null &&
                membershipContainerDTO.PriceListId != -1)
            {
                result.Add(membershipContainerDTO.PriceListId);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
