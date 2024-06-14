/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemotePhysicalCountInventoryUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      04-Jan-2021       Abhishek                 Created 
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.PhysicalCount
{
    public class RemotePhysicalCountInventoryUseCases : RemoteUseCases, IPhysicalCountInventoryUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PHYSICAL_COUNT_REVIEWS_URL = "api/Inventory/{physicalCountId}/PhysicalCountInventory";
        private const string PHYSICAL_COUNT_INVENTORY_COUNT_URL = "api/Inventory/{physicalCountId}/PhysicalCountAdjustmentCounts";

        public RemotePhysicalCountInventoryUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<PhysicalCountReviewDTO>> GetPhysicalCountReviews(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters, string advancedSearch, string filterText, int physicalCountId, DateTime startDate, int locationId,
                                                                                bool ismodifiedDuringPhysicalCount, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(filterText);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("advancedSearch", advancedSearch.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("filterText", filterText.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("physicalCountId", physicalCountId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("startDate", startDate.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("locationId", locationId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage", currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("ismodifiedDuringPhysicalCount", ismodifiedDuringPhysicalCount.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            List<PhysicalCountReviewDTO> physicalCountReviewDTOList = await Get<List<PhysicalCountReviewDTO>>(PHYSICAL_COUNT_REVIEWS_URL.Replace("{physicalCountId}", physicalCountId.ToString()), searchParameterList);
            log.LogMethodExit(physicalCountReviewDTOList);
            return physicalCountReviewDTOList;
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> inventoryPhysicalCountsSearchParams)
        {
            log.LogMethodEntry(inventoryPhysicalCountsSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string> searchParameter in inventoryPhysicalCountsSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CATEGORYID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("categoryId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.UOM_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uomId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productCode".ToString(), searchParameter.Value));
                        }
                        break;
                    case PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.BARCODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productBarcode".ToString(), searchParameter.Value));
                        }
                        break;
                    case PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.DESCRIPTION:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("description".ToString(), searchParameter.Value));
                        }
                        break;
                    case PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.INVENTORYITEMSONLY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isPurchaseable".ToString(), searchParameter.Value));
                        }
                        break;
                    case PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.SYMBOL:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("symbol".ToString(), searchParameter.Value));
                        }
                        break;
                    case PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters.Quantity:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("quantity".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<int> GetPhysicalCountInventoryCounts(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters,
                                                              string advancedSearch, string filterText, int physicalCountId, DateTime startDate, int locationId,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(filterText);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("advancedSearch", advancedSearch.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("filterText", filterText.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("physicalCountId", physicalCountId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("startDate", startDate.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("locationId", locationId.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            int physicalCountInventoryCount = await Get<int>(PHYSICAL_COUNT_REVIEWS_URL.Replace("{physicalCountId}", physicalCountId.ToString()), searchParameterList);
            log.LogMethodExit(physicalCountInventoryCount);
            return physicalCountInventoryCount;
        }

        public async Task<string> SavePhysicalCountReviews(List<PhysicalCountReviewDTO> physicalCountReviewDTOList, int physicalCountId)
        {
            log.LogMethodEntry(physicalCountReviewDTOList);
            string responseString = await Post<string>(PHYSICAL_COUNT_REVIEWS_URL.Replace("{physicalCountId}", physicalCountId.ToString()), physicalCountReviewDTOList);
            log.LogMethodExit(responseString);
            return responseString;
        }
    }
}
