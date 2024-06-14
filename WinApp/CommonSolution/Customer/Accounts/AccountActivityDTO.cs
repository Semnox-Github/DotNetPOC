/********************************************************************************************
 * Project Name - Contact Data Handler
 * Description  - Data handler of the Contact class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.60        08-May-2019   Nitin Pai           Modified for Guest App
 *2.70.2      15-Oct-2019   Nitin Pai           Gateway Cleanup
 *2.70.3      30-Mar-2020   Jeevan              Added Search parameter for excluded list
*2.130.0     19-July-2021     Girish Kundar       Modified : Virtual point column added part of Arcade changes
*2.130.2      13-Dec-2021   Deeksha             Modified : CounterItems,PlayCredits fields are added as part of TransferBalance Enhancements.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountActivityView data object class.
    /// </summary>
    public class AccountActivityDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            ACCOUNT_ID,
            FROM_DATE,
            TO_DATE,
            ACCOUNT_ID_LIST,
            TRANSACTION_STATUS,
            EXCLUDED_PRODUCT_LIST,
            SITE
        }
        private int accountId;
        private DateTime? date;
        private string product;
        private decimal? amount;
        private decimal? credits;
        private decimal? courtesy;
        private decimal? bonus;
        private decimal? time;
        private decimal? tokens;
        private int? tickets;
        private decimal? loyaltyPoints;
        private string site;
        private string pOS;
        private string userName;
        private decimal? quantity;
        private decimal? price;
        private int? refId;
        private string activityType;
        private int? rowNumber;
        private decimal? virtualPoints;
        private decimal? counterItems;
        private decimal? playCredits;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountActivityDTO()
        {
            log.LogMethodEntry();
            accountId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountActivityDTO(int accountId, DateTime? date, string product, decimal? amount, decimal? credits,
                         decimal? courtesy, decimal? bonus, decimal? time, decimal? tokens,
                         int? tickets, decimal? loyaltyPoints, string site, string pOS, string userName,
                         decimal? quantity, decimal? price, int? refId, string activityType, int? rowNumber,decimal? virtualPoints,
                         decimal? counterItems, decimal? playCredits)
        {
            log.LogMethodEntry(accountId, date, product, amount, credits, courtesy, bonus,
                               time, tokens, tickets, loyaltyPoints, site, pOS, userName,
                               quantity, price, refId, activityType, rowNumber, virtualPoints);
            this.accountId = accountId;
            this.date = date;
            this.product = product;
            this.amount = amount;
            this.credits = credits;
            this.courtesy = courtesy;
            this.bonus = bonus;
            this.time = time;
            this.tokens = tokens;
            this.tickets = tickets;
            this.loyaltyPoints = loyaltyPoints;
            this.site = site;
            this.pOS = pOS;
            this.userName = userName;
            this.time = time;
            this.quantity = quantity;
            this.price = price;
            this.refId = refId;
            this.activityType = activityType;
            this.rowNumber = rowNumber;
            this.virtualPoints = virtualPoints;
            this.counterItems = counterItems;
            this.playCredits = playCredits;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the accountId field
        /// </summary>
        [Browsable(false)]
        public int AccountId
        {
            get
            {
                return accountId;
            }

            set
            {
                accountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the date field
        /// </summary>
        [DisplayName("Date")]
        public DateTime? Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
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
        [DisplayName("Amount")]
        public decimal? Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the credits field
        /// </summary>
        [DisplayName("Credits")]
        public decimal? Credits
        {
            get
            {
                return credits;
            }

            set
            {
                credits = value;
            }
        }

        /// <summary>
        /// Get/Set method of the courtesy field
        /// </summary>
        [DisplayName("Courtesy")]
        public decimal? Courtesy
        {
            get
            {
                return courtesy;
            }

            set
            {
                courtesy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the bonus field
        /// </summary>
        [DisplayName("Bonus")]
        public decimal? Bonus
        {
            get
            {
                return bonus;
            }

            set
            {
                bonus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the time field
        /// </summary>
        [DisplayName("Time")]
        public decimal? Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        /// <summary>
        /// Get/Set method of the tockens field
        /// </summary>
        [DisplayName("Tockens")]
        public decimal? Tokens
        {
            get
            {
                return tokens;
            }

            set
            {
                tokens = value;
            }
        }

        /// <summary>
        /// Get/Set method of the tickets field
        /// </summary>
        [DisplayName("Tickets")]
        public int? Tickets
        {
            get
            {
                return tickets;
            }

            set
            {
                tickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the loyaltyPoints field
        /// </summary>
        [DisplayName("Loyalty Points")]
        public decimal? LoyaltyPoints
        {
            get
            {
                return loyaltyPoints;
            }

            set
            {
                loyaltyPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the virtualPoints field
        /// </summary>
        [DisplayName("Virtual Points")]
        public decimal? VirtualPoints
        {
            get
            {
                return virtualPoints;
            }

            set
            {
                virtualPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the site field
        /// </summary>
        [DisplayName("Site")]
        public string Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
            }
        }

        /// <summary>
        /// Get/Set method of the pOS field
        /// </summary>
        [DisplayName("POS")]
        public string POS
        {
            get
            {
                return pOS;
            }

            set
            {
                pOS = value;
            }
        }

        /// <summary>
        /// Get/Set method of the userName field
        /// </summary>
        [DisplayName("UserName")]
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public decimal? Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        [DisplayName("Price")]
        public decimal? Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        /// <summary>
        /// Get/Set method of the refId field
        /// </summary>
        [DisplayName("RefId")]
        public int? RefId
        {
            get
            {
                return refId;
            }
            set
            {
                refId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the rowNumber field
        /// </summary>
        [DisplayName("RowNumber")]
        public int? RowNumber
        {
            get
            {
                return rowNumber;
            }
            set
            {
                rowNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the activityType field
        /// </summary>
        [DisplayName("Activity Type")]
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

        /// <summary>
        /// Get/Set method of the PlayCredits field
        /// </summary>
        [DisplayName("PlayCredits")]
        public decimal? PlayCredits
        {
            get
            {
                return playCredits;
            }

            set
            {
                playCredits = value;
            }
        }


        /// <summary>
        /// Get/Set method of the CounterItems field
        /// </summary>
        [DisplayName("CounterItems")]
        public decimal? CounterItems
        {
            get
            {
                return counterItems;
            }

            set
            {
                counterItems = value;
            }
        }
    }
}
