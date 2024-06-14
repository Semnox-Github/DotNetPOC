/********************************************************************************************
 * Project Name - Discounts
 * Description  - IDiscountUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0    15-Apr-2021         Abhishek               Created : POS UI Redesign with REST API 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    public interface IDiscountUseCases
    {
        Task<DiscountContainerDTOCollection> GetDiscountContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<DiscountAvailabilityContainerDTOCollection> GetDiscountAvailabilityDTOCollection(int siteId, DateTime startDate, DateTime endDate, string hash, bool rebuildCache);

        Task<List<DiscountsDTO>> GetDiscountsDTOList(string isActive = null,
                                                     int discountId = -1,
                                                     string discountName = null,
                                                     string discountType = null,
                                                     string displayInPOS = null,
                                                     decimal? minimumCreditsGreaterThan = null,
                                                     decimal? minimumSaleAmountGreaterThan = null,
                                                     string automaticApply = null,
                                                     string couponMandatory = null,
                                                     int discountedGameId = -1,
                                                     int discountedProductId = -1,
                                                     int discountedCategoryId = -1,
                                                     int siteId = -1,
                                                     int pageNumber = 0,
                                                     int pageSize = 0,
                                                     bool loadChildRecords = false,
                                                     bool loadActiveChildRecords = true,
                                                     bool onlyDiscountedChildRecords = false);

        Task<int> GetDiscountsDTOListCount(string isActive = null,
                                           int discountId = -1,
                                           string discountName = null,
                                           string discountType = null,
                                           string displayInPOS = null,
                                           decimal? minimumCreditsGreaterThan = null,
                                           decimal? minimumSaleAmountGreaterThan = null,
                                           string automaticApply = null,
                                           string couponMandatory = null,
                                           int discountedGameId = -1,
                                           int discountedProductId = -1,
                                           int discountedCategoryId = -1,
                                           int siteId = -1);

        Task<List<DiscountsDTO>> SaveDiscountsDTOList(List<DiscountsDTO> discountsDTOList);

        Task ClearDiscountedProducts(int discountId);
        
        Task DeselectDiscountedProducts(int discountId);

        Task ClearDiscountedGames(int discountId);
    }
}
