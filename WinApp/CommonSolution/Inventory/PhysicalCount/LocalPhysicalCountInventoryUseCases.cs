/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalInventoryPhysicalCount class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      04-Jan-2021       Abhishek                 Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.PhysicalCount
{
    public class LocalPhysicalCountInventoryUseCases : LocalUseCases, IPhysicalCountInventoryUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalPhysicalCountInventoryUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<PhysicalCountReviewDTO>> GetPhysicalCountReviews(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters, string advancedSearch, string filterText, int physicalCountId, DateTime startDate, int locationId
                                                                                       , bool ismodifiedDuringPhysicalCount, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<PhysicalCountReviewDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(filterText);
                PhysicalCountReviewList physicalCountReviewList = new PhysicalCountReviewList(executionContext);
                int siteId = GetSiteId();
                List<PhysicalCountReviewDTO> physicalCountReviewsDTOList = physicalCountReviewList.GetAllPhysicalCountReviewsDTOList(searchParameters, advancedSearch, filterText, physicalCountId, startDate, locationId, ismodifiedDuringPhysicalCount, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(physicalCountReviewsDTOList);
                return physicalCountReviewsDTOList;
            });
        }

        public async Task<int> GetPhysicalCountInventoryCounts(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters,
                                                                string advancedSearch, string filterText, int physicalCountId, DateTime startDate, int locationId,
                                                                SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(filterText);
                PhysicalCountReviewList physicalCountReviewList = new PhysicalCountReviewList(executionContext);
                int siteId = GetSiteId();
                int physicalCountInventoryCounts = physicalCountReviewList.GetPhysicalCountReviewsDTOListCount(searchParameters, advancedSearch, filterText, physicalCountId, startDate, locationId);
                log.LogMethodExit(physicalCountInventoryCounts);
                return physicalCountInventoryCounts;
            });
        }

        public async Task<string> SavePhysicalCountReviews(List<PhysicalCountReviewDTO> physicalCountReviewDTOList, int physicalCountId)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(physicalCountReviewDTOList);
                string result = string.Empty;
                if (physicalCountReviewDTOList == null)
                {
                    throw new ValidationException("physicalCountReviewDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        PhysicalCountReviewList physicalCountReviewList = new PhysicalCountReviewList(executionContext, physicalCountReviewDTOList);//note
                        physicalCountReviewList.Save(physicalCountId, parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw new Exception(ex.Message, ex);
                    }

                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
    }
}
