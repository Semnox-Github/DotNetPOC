/********************************************************************************************
 * Project Name - POS
 * Description  - Container class to hold the product menu panels
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Aug-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System.Collections.Generic;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents the key of the ProductMenuContainer cache
    /// </summary>
    public class ProductPriceContainerCacheKey : Core.GenericUtilities.ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int siteId;
        private readonly int posMachineId;
        private readonly int userRoleId;
        private readonly int languageId;
        private readonly string menuType;
        private readonly int membershipId;
        private readonly int transactionProfileId;
        private readonly DateTimeRange dateTimeRange;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductPriceContainerCacheKey(int siteId, 
                                             int posMachineId, 
                                             int userRoleId, 
                                             int languageId, 
                                             string menuType,
                                             int membershipId,
                                             int transactionProfileId,
                                             DateTimeRange dateTimeRange)
        {
            log.LogMethodEntry(siteId, posMachineId, userRoleId, languageId, menuType, membershipId, transactionProfileId, dateTimeRange);
            this.siteId = siteId;
            this.posMachineId = posMachineId;
            this.userRoleId = userRoleId;
            this.languageId = languageId;
            if (languageId <= -1)
            {
                languageId = -1;
            }
            this.menuType = menuType;
            this.membershipId = membershipId;
            this.transactionProfileId = transactionProfileId;
            this.dateTimeRange = dateTimeRange;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the atomic values
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            log.LogMethodEntry();
            yield return siteId;
            yield return posMachineId;
            yield return userRoleId;
            yield return languageId;
            yield return menuType;
            yield return membershipId;
            yield return transactionProfileId;
            yield return dateTimeRange;
            log.LogMethodExit();
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
        /// Get method of posMachineId field
        /// </summary>
        public int PosMachineId
        {
            get
            {
                return posMachineId;
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
        /// Get method of languageId field
        /// </summary>
        public int LanguageId
        {
            get
            {
                return languageId;
            }
        }

        /// <summary>
        /// Get method of menuType field
        /// </summary>
        public string MenuType
        {
            get
            {
                return menuType;
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
