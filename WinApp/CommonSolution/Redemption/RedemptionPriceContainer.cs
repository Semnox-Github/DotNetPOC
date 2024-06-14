/********************************************************************************************
 * Project Name - Redemption
 * Description  - RedemptionPriceContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Class holds the price in tickets for the redemption products.
    /// </summary>
    public class RedemptionPriceContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<int, Dictionary<int, decimal>> productIdMembershipIdPriceInTicketsDictionary = new Dictionary<int, Dictionary<int, decimal>>();
        private readonly RedemptionPriceContainerDTOCollection redemptionPriceContainerDTOCollection;
        private readonly DateTime? maxLastUpdateTime;
        private readonly int siteId;
        private readonly List<ProductsContainerDTO> productsContainerDTOList;
        private readonly List<MembershipContainerDTO> membershipContainerDTOList;

        public RedemptionPriceContainer(int siteId)
            : this(siteId, GetProductsContainerDTOList(siteId), GetMembershipContainerDTOList(siteId), GetMaxUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit(siteId);
        }

        public RedemptionPriceContainer(int siteId, List<ProductsContainerDTO> productsContainerDTOList, List<MembershipContainerDTO> membershipContainerDTOList, DateTime? maxLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            List<RedemptionPriceContainerDTO> redemptionPriceContainerDTOList = new List<RedemptionPriceContainerDTO>();

            this.maxLastUpdateTime = maxLastUpdateTime;
            this.productsContainerDTOList = productsContainerDTOList;
            this.membershipContainerDTOList = membershipContainerDTOList;
            foreach (var productsContainerDTO in productsContainerDTOList)
            {
                if (productIdMembershipIdPriceInTicketsDictionary.ContainsKey(productsContainerDTO.ProductId) == false)
                {
                    productIdMembershipIdPriceInTicketsDictionary.Add(productsContainerDTO.ProductId, new Dictionary<int, decimal>());
                }
                if (productIdMembershipIdPriceInTicketsDictionary[productsContainerDTO.ProductId].ContainsKey(-1) == false)
                {
                    productIdMembershipIdPriceInTicketsDictionary[productsContainerDTO.ProductId].Add(-1, productsContainerDTO.InventoryItemContainerDTO.PriceInTickets);
                }
                RedemptionPriceContainerDTO redemptionPriceContainerDTO = new RedemptionPriceContainerDTO(productsContainerDTO.ProductId, -1, productsContainerDTO.InventoryItemContainerDTO.PriceInTickets);
                redemptionPriceContainerDTOList.Add(redemptionPriceContainerDTO);
                foreach (var membershipContainerDTO in membershipContainerDTOList)
                {
                    decimal priceIntickets = productsContainerDTO.InventoryItemContainerDTO.PriceInTickets - (productsContainerDTO.InventoryItemContainerDTO.PriceInTickets * membershipContainerDTO.RedemptionDiscount / 100);
                    if (productIdMembershipIdPriceInTicketsDictionary[productsContainerDTO.ProductId].ContainsKey(membershipContainerDTO.MembershipId) == false)
                    {
                        productIdMembershipIdPriceInTicketsDictionary[productsContainerDTO.ProductId].Add(membershipContainerDTO.MembershipId, priceIntickets);
                    }
                    redemptionPriceContainerDTO = new RedemptionPriceContainerDTO(productsContainerDTO.ProductId, membershipContainerDTO.MembershipId, priceIntickets);
                    redemptionPriceContainerDTOList.Add(redemptionPriceContainerDTO);
                }
            }
            redemptionPriceContainerDTOCollection = new RedemptionPriceContainerDTOCollection(redemptionPriceContainerDTOList);
            log.LogMethodExit();
        }

        private static DateTime? GetMaxUpdateTime(int siteId)
        {
            log.LogMethodEntry();
            DateTime? result;
            try
            {
                ProductsList productsList = new ProductsList();
                MembershipsList membershipsList = new MembershipsList();
                DateTime? productModuleMaxUpdateTime = productsList.GetProductsLastUpdateTime(siteId);
                DateTime? membershipModuleMaxUpdateTime = membershipsList.GetMembershipLastUpdateTime(siteId);
                result = productModuleMaxUpdateTime > membershipModuleMaxUpdateTime ? productModuleMaxUpdateTime : membershipModuleMaxUpdateTime;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the max update date.", ex);
                result = null;
            }

            log.LogMethodExit(result);
            return result;
        }

        private static List<ProductsContainerDTO> GetProductsContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ProductsContainerDTO> result = null;
            try
            {
                result = ProductsContainerList.GetActiveProductsContainerDTOList(siteId, ManualProductType.REDEEMABLE.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the products.", ex);
            }
            if (result != null && result.Any(x => x.InventoryItemContainerDTO != null && x.InventoryItemContainerDTO.IsRedeemable == "Y"))
            {
                result = result.Where(x => x.InventoryItemContainerDTO != null && x.InventoryItemContainerDTO.IsRedeemable == "Y").ToList();
            }
            else
            {
                result = new List<ProductsContainerDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<MembershipContainerDTO> GetMembershipContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<MembershipContainerDTO> result = null;
            try
            {
                result = Semnox.Parafait.Customer.Membership.Sample.MembershipContainerList.GetMembershipContainerDTOList(siteId);
            }
            catch (Exception ex)
            {

                log.Error("Error occurred while retrieving the products.", ex);
            }
            if (result == null)
            {
                result = new List<MembershipContainerDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }
        internal decimal GetPriceInTickets(int productId, int membershipId)
        {
            log.LogMethodEntry(productId, membershipId);

            if (productIdMembershipIdPriceInTicketsDictionary.ContainsKey(productId) == false)
            {
                string errorMessage = "Redeemable product with productId :" + productId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            if (productIdMembershipIdPriceInTicketsDictionary[productId].ContainsKey(membershipId) == false)
            {
                string errorMessage = "Membership with membershipId :" + membershipId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            decimal result = productIdMembershipIdPriceInTicketsDictionary[productId][membershipId];
            log.LogMethodExit(result);
            return result;
        }

        public decimal GetLeastPriceInTickets(int productId, List<int> membershipIdList)
        {
            log.LogMethodEntry(productId, membershipIdList);
            decimal? priceInTickets = null;
            if (membershipIdList == null || membershipIdList.Any() == false)
            {
                var value = GetPriceInTickets(productId, -1);
                priceInTickets = value;
            }
            foreach (var membershipId in membershipIdList)
            {
                var value = GetPriceInTickets(productId, membershipId);
                if (priceInTickets == null || value < priceInTickets.Value)
                {
                    priceInTickets = value;
                }
            }
            decimal result = priceInTickets.Value;
            log.LogMethodExit(result);
            return result;
        }

        public RedemptionPriceContainerDTOCollection GetRedemptionPriceContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(redemptionPriceContainerDTOCollection);
            return redemptionPriceContainerDTOCollection;
        }

        public RedemptionPriceContainer Refresh()
        {
            log.LogMethodEntry();
            DateTime? updateTime = GetMaxUpdateTime(siteId);
            if (maxLastUpdateTime.HasValue
                && maxLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in either products or membership since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            Semnox.Parafait.Customer.Membership.Sample.MembershipContainerList.Rebuild(siteId);
            ProductsContainerList.Rebuild(siteId);
            RedemptionPriceContainer result = new RedemptionPriceContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
