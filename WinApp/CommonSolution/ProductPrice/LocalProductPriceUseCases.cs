/********************************************************************************************
 * Project Name - Product Price
 * Description  - product price use cases implementation
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
    /// product price use cases implementation
    /// </summary>
    public class LocalProductPriceUseCases : LocalUseCases, IProductPriceUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public LocalProductPriceUseCases(ExecutionContext executionContext)
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
            return await Task<PriceContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, startDateTime, endDateTime, hash, rebuildCache);
                if (rebuildCache)
                {
                    PriceContainerList.Rebuild(siteId);
                }
                PriceContainerDTOCollection result = PriceContainerList.GetPriceContainerDTOCollection(siteId, membershipId, userRoleId, transactionProfileId, startDateTime, endDateTime, hash);
                log.LogMethodExit(result);
                return result;
            });
        }

        /// <summary>
        /// Get ProductPriceContainerSnapshotDTOCollection use case
        /// </summary>
        public async Task<ProductPriceContainerSnapshotDTOCollection> GetProductPriceContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache)
        {
            return await Task<ProductPriceContainerSnapshotDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, membershipId, transactionProfileId, startDateTime, endDateTime, hash, rebuildCache);
                if (rebuildCache)
                {
                    ProductPriceContainerList.Rebuild(siteId);
                }
                ProductPriceContainerSnapshotDTOCollection result = ProductPriceContainerList.GetProductPriceContainerSnapshotDTOCollection(siteId, posMachineId, userRoleId, languageId, menuType, membershipId, transactionProfileId, startDateTime, endDateTime, hash);
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}
