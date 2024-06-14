/********************************************************************************************
 * Project Name - Product Price
 * Description  - Method object to consolidate the prices
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      1-Sep-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Method object to consolidate the prices
    /// </summary>
    public class PriceContainerDTOConsolidator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// consolidates the prices
        /// </summary>
        public List<PriceContainerDTO> Consolidate(List<PriceContainerDTO> priceContainerDTOList)
        {
            log.LogMethodEntry(priceContainerDTOList);
            if(priceContainerDTOList == null ||
                priceContainerDTOList.Any() == false)
            {
                log.LogMethodExit(priceContainerDTOList);
                return priceContainerDTOList;
            }
            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                priceContainerDTO.PriceContainerDetailDTOList = Consolidate(0, priceContainerDTO.PriceContainerDetailDTOList);
            }
            log.LogMethodExit(priceContainerDTOList);
            return priceContainerDTOList;
        }

        private List<PriceContainerDetailDTO> Consolidate(int position, List<PriceContainerDetailDTO> priceContainerDetailDTOList)
        {
            log.LogMethodEntry(position, priceContainerDetailDTOList);
            if (priceContainerDetailDTOList == null ||
                position >= priceContainerDetailDTOList.Count - 1)
            {
                log.LogMethodExit(priceContainerDetailDTOList);
                return priceContainerDetailDTOList;
            }
            if (CanConsolidate(priceContainerDetailDTOList[position], priceContainerDetailDTOList[position + 1]))
            {
                priceContainerDetailDTOList[position].EndDateTime = priceContainerDetailDTOList[position + 1].EndDateTime;
                priceContainerDetailDTOList.Remove(priceContainerDetailDTOList[position + 1]);
                Consolidate(position, priceContainerDetailDTOList);
            }
            else
            {
                Consolidate(position + 1, priceContainerDetailDTOList);
            }
            log.LogMethodExit(priceContainerDetailDTOList);
            return priceContainerDetailDTOList;
        }

        private bool CanConsolidate(PriceContainerDetailDTO firstPriceContainerDetailDTO, PriceContainerDetailDTO secondPriceContainerDetailDTO)
        {
            log.LogMethodEntry(firstPriceContainerDetailDTO, secondPriceContainerDetailDTO);
            if (firstPriceContainerDetailDTO.EndDateTime != secondPriceContainerDetailDTO.StartDateTime)
            {
                log.LogMethodExit(false, "firstPriceContainerDetailDTO.EndDateTime != secondPriceContainerDetailDTO.StartDateTime");
                return false;
            }
            if (firstPriceContainerDetailDTO.BasePrice != secondPriceContainerDetailDTO.BasePrice)
            {
                log.LogMethodExit(false, "base price doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.MembershipPrice != secondPriceContainerDetailDTO.MembershipPrice)
            {
                log.LogMethodExit(false, "Membership Price doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.MembershipPriceListId != secondPriceContainerDetailDTO.MembershipPriceListId)
            {
                log.LogMethodExit(false, "membershipPriceListId doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.UserRolePrice != secondPriceContainerDetailDTO.UserRolePrice)
            {
                log.LogMethodExit(false, "userRolePrice doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.UserRolePriceListId != secondPriceContainerDetailDTO.UserRolePriceListId)
            {
                log.LogMethodExit(false, "userRolePriceListId doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.TransactionProfilePrice != secondPriceContainerDetailDTO.TransactionProfilePrice)
            {
                log.LogMethodExit(false, "transactionProfilePrice doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.TransactionProfilePriceListId != secondPriceContainerDetailDTO.TransactionProfilePriceListId)
            {
                log.LogMethodExit(false, "transactionProfilePriceListId doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.PromotionPrice != secondPriceContainerDetailDTO.PromotionPrice)
            {
                log.LogMethodExit(false, "promotionPrice doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.PriceType != secondPriceContainerDetailDTO.PriceType)
            {
                log.LogMethodExit(false, "priceType doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.FinalPrice != secondPriceContainerDetailDTO.FinalPrice)
            {
                log.LogMethodExit(false, "finalPrice doesn't match");
                return false;
            }
            if (firstPriceContainerDetailDTO.PromotionId != secondPriceContainerDetailDTO.PromotionId)
            {
                log.LogMethodExit(false, "promotionId doesn't match");
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }
    }
}