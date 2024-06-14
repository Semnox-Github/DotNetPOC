/********************************************************************************************
 * Project Name - Product Price
 * Description  - Remote proxy to product price use cases
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      18-Aug-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using System;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Remote proxy to product price use cases
    /// </summary>
    public class RemoteProductPriceUseCases : RemoteUseCases, IProductPriceUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRICE_CONTAINER_URL = "/api/Product/PriceContainer";
        private const string PRODUCT_PRICE_CONTAINER_URL = "/api/Product/ProductPriceContainer";

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public RemoteProductPriceUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get PriceContainerDTOCollection use case
        /// </summary>
        public async Task<PriceContainerDTOCollection> GetPriceContainerDTOCollection(int siteId, int membershipId, int userRoleId, int transactionProfileId, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, startDateTime, endDateTime, hash, rebuildCache);
            PriceContainerDTOCollection result = await Get<PriceContainerDTOCollection>(PRICE_CONTAINER_URL, new WebApiGetRequestParameterCollection("siteId", siteId, "membershipId", membershipId, "userRoleId", userRoleId, "transactionProfileId", transactionProfileId,  "startDateTime", startDateTime, "endDateTime", endDateTime, "hash", hash, "rebuildCache", rebuildCache));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get ProductPriceContainerSnapshotDTOCollection use case
        /// </summary>
        public async Task<ProductPriceContainerSnapshotDTOCollection> GetProductPriceContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, startDateTime, endDateTime, hash, rebuildCache);
            ProductPriceContainerSnapshotDTOCollection result = await Get<ProductPriceContainerSnapshotDTOCollection>(PRODUCT_PRICE_CONTAINER_URL, new WebApiGetRequestParameterCollection("siteId", 
                                                                                                                                                                                            siteId, 
                                                                                                                                                                                            "userRoleId", 
                                                                                                                                                                                            userRoleId, 
                                                                                                                                                                                            "posMachineId", 
                                                                                                                                                                                            posMachineId, 
                                                                                                                                                                                            "languageId", 
                                                                                                                                                                                            languageId, 
                                                                                                                                                                                            "menuType", 
                                                                                                                                                                                            menuType,
                                                                                                                                                                                            "membershipId",
                                                                                                                                                                                            membershipId,
                                                                                                                                                                                            "transactionProfileId",
                                                                                                                                                                                            transactionProfileId,
                                                                                                                                                                                            "startDateTime",
                                                                                                                                                                                            startDateTime,
                                                                                                                                                                                            "endDateTime",
                                                                                                                                                                                            endDateTime,
                                                                                                                                                                                            "hash",
                                                                                                                                                                                            hash,
                                                                                                                                                                                            "rebuildCache",
                                                                                                                                                                                            rebuildCache));
            log.LogMethodExit(result);
            return result;
        }

    }
}
