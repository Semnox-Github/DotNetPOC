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
    public class PriceContainerKey : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int siteId;
        private readonly int membershipId;
        private readonly int userRoleId;
        private readonly int transactionProfileId;
        private readonly DateTimeRange dateTimeRange;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public PriceContainerKey(int siteId, int membershipId, int userRoleId, int transactionProfileId, DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId, membershipId, userRoleId, transactionProfileId, dateTimeRange);
            this.siteId = siteId;
            this.membershipId = membershipId;
            this.transactionProfileId = transactionProfileId;
            this.userRoleId = userRoleId;
            this.dateTimeRange = dateTimeRange;
            log.LogMethodExit();
        }
        /// <summary>
        /// returns the atomic values
        /// </summary>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return siteId;
            yield return membershipId;
            yield return userRoleId;
            yield return transactionProfileId;
            yield return dateTimeRange;
        }

        /// <summary>
        /// Get method of siteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
            
        }

        /// <summary>
        /// Get method of membershipId field
        /// </summary>
        public int MembershipId
        {
            get
            {
                return membershipId;
            }

        }

        /// <summary>
        /// Get method of userRoleId field
        /// </summary>
        public int UserRoleId
        {
            get
            {
                return userRoleId;
            }

        }

        /// <summary>
        /// Get method of transactionProfileId field
        /// </summary>
        public int TransactionProfileId
        {
            get
            {
                return transactionProfileId;
            }

        }

        /// <summary>
        /// Get method of dateTimeRange field
        /// </summary>
        public DateTimeRange DateTimeRange
        {
            get
            {
                return dateTimeRange;
            }

        }
    }
}
