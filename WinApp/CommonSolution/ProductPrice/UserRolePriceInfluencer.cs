/********************************************************************************************
 * Project Name - Product Price
 * Description  - Represents user role based price influencer
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
using Semnox.Parafait.User;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents user role based price influencer
    /// </summary>
    public class UserRolePriceInfluencer : PriceListPriceInfluencer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly UserRoleContainerDTO userRoleContainerDTO;

        /// <summary>
        /// Parameterized constructor 
        /// </summary>
        public UserRolePriceInfluencer(UserRoleContainerDTO userRoleContainerDTO, 
                                          Dictionary<int, PriceListDTO> priceListIdPriceListDTODictionary, 
                                          Dictionary<int, Dictionary<int, List<PriceListProductsDTO>>> priceListIdProductIdPriceListProductsDTODictionary)
        :base(priceListIdPriceListDTODictionary, 
              priceListIdProductIdPriceListProductsDTODictionary)
        {
            log.LogMethodEntry(userRoleContainerDTO, 
                               priceListIdPriceListDTODictionary, 
                               priceListIdProductIdPriceListProductsDTODictionary);
            this.userRoleContainerDTO = userRoleContainerDTO;
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
                log.LogMethodExit(priceContainerDTOList, "priceContainerDTOList == null");
                return priceContainerDTOList;
            }

            if (userRoleContainerDTO == null)
            {
                log.LogMethodExit(priceContainerDTOList, "userRoleContainerDTO == null");
                return priceContainerDTOList;
            }

            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                priceContainerDTO.UserRoleId = userRoleContainerDTO.RoleId;
                foreach (var priceContainerDetailDTO in priceContainerDTO.PriceContainerDetailDTOList)
                {
                    priceContainerDetailDTO.UserRolePriceListId = userRoleContainerDTO.PriceListId;
                    priceContainerDetailDTO.UserRolePrice = GetProductListPrice(userRoleContainerDTO.PriceListId, priceContainerDTO.ProductId, priceContainerDetailDTO.StartDateTime);
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
            if(userRoleContainerDTO != null &&
                userRoleContainerDTO.PriceListId != -1)
            {
                result.Add(userRoleContainerDTO.PriceListId);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
