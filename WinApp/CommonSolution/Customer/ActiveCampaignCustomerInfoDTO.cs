/********************************************************************************************
 * Project Name - ActiveCampaignCustomerInfoDTO
 * Description  - Data handler of the Active Campaign Info DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.3      01-Feb-2020   Nitin Pai           Created, new DTO of active campaign object 
 ********************************************************************************************/

 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public class ActiveCampaignCustomerInfoDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            ACCOUNT_ID,
            FROM_DATE,
            TO_DATE,
            ACCOUNT_ID_LIST,
            CUSTOMER_ID,
        }

        private int customerId;
        private DateTime lastPurchasedDate;
        private string product;
        private int countOfPurchases;
        private string activityType;
        private int siteId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ActiveCampaignCustomerInfoDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ActiveCampaignCustomerInfoDTO(int customerId, DateTime lastPurchasedDate, string product, int countOfPurchases, string activityType, int siteId)
        {
            log.LogMethodEntry(customerId, lastPurchasedDate, product, countOfPurchases, activityType);
            this.customerId = customerId;
            this.lastPurchasedDate = lastPurchasedDate;
            this.product = product;
            this.countOfPurchases = countOfPurchases;
            this.activityType = activityType;
            this.siteId = siteId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the accountId field
        /// </summary>
        [Browsable(false)]
        public int CustomerId
        {
            get
            {
                return customerId;
            }

            set
            {
                customerId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the date field
        /// </summary>
        [DisplayName("Date")]
        public DateTime LastPurchasedDate
        {
            get
            {
                return lastPurchasedDate;
            }

            set
            {
                lastPurchasedDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the product field
        /// </summary>
        [DisplayName("Product")]
        public string Product
        {
            get
            {
                return product;
            }

            set
            {
                product = value;
            }
        }

        /// <summary>
        /// Get/Set method of the amount field
        /// </summary>
        [DisplayName("Quantity")]
        public int CountOfPurchases
        {
            get
            {
                return countOfPurchases;
            }

            set
            {
                countOfPurchases = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the site field
        /// </summary>
        [DisplayName("ActivityType")]
        public string ActivityType
        {
            get
            {
                return activityType;
            }

            set
            {
                activityType = value;
            }
        }
    }
}
