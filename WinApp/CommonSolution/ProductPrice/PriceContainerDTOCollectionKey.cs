/********************************************************************************************
 * Project Name - Product Price
 * Description  - Represents key to the price container dto 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      18-Aug-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System.Collections.Generic;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents key to the price container dto 
    /// </summary>
    public class PriceContainerDTOCollectionKey : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int productId;
        private readonly int membershipId;
        private readonly int userRoleId;
        private readonly int transactionProfileId;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public PriceContainerDTOCollectionKey(int productId, int membershipId, int userRoleId, int transactionProfileId)
        {
            log.LogMethodEntry(productId, membershipId, userRoleId, transactionProfileId);
            this.productId = productId;
            this.membershipId = membershipId;
            this.transactionProfileId = transactionProfileId;
            this.userRoleId = userRoleId;
            log.LogMethodExit();
        }
        /// <summary>
        /// returns the atomic values
        /// </summary>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return productId;
            yield return membershipId;
            yield return userRoleId;
            yield return transactionProfileId;
        }
    }
}
