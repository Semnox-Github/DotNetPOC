/********************************************************************************************
 * Project Name - Product Price
 * Description  - Represents product price use case specification
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

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents product price use case specification
    /// </summary>
    public interface IProductPriceUseCases
    {
        /// <summary>
        /// Get PriceContainerDTOCollection use case specification
        /// </summary>
        Task<PriceContainerDTOCollection> GetPriceContainerDTOCollection(int siteId, int membershipId, int userRoleId, int transactionProfileId,  DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache);
        /// <summary>
        /// Get PriceContainerDTOCollection use case specification
        /// </summary>
        Task<ProductPriceContainerSnapshotDTOCollection> GetProductPriceContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, int membershipId, int transactionProfileId, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache);
    }
}
