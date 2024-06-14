/********************************************************************************************
 * Project Name - ProductType DTO
 * Description  - Data object of Product Type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        23-Jan-2019   Deeksha                 Created 
 *2.70        21-Feb-2019   Guru S A                Booking phase 2 changes
 ********************************************************************************************/
using System;
using System.ComponentModel;
namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Product Type List
    /// </summary>
    public static class ProductTypeValues
    {
        public const string NEW = "NEW";
        public const string RECHARGE = "RECHARGE";
        public const string MANUAL = "MANUAL";
        public const string REFUND = "REFUND";
        public const string GAMETIME = "GAMETIME";
        public const string LOYALTY = "LOYALTY";
        public const string EXTERNALPOS = "EXTERNAL POS";
        public const string ATTRACTION = "ATTRACTION";
        public const string CHECKIN = "CHECK-IN";
        public const string CHECKOUT = "CHECK-OUT";
        public const string CARDDEPOSIT = "CARDDEPOSIT";
        public const string GAMEPLAYCREDIT = "GAMEPLAYCREDIT";
        public const string CARDSALE = "CARDSALE";
        public const string CREDITCARDSURCHARGE = "CREDITCARDSURCHARGE";
        public const string COMBO = "COMBO";
        public const string REFUNDCARDDEPOSIT = "REFUNDCARDDEPOSIT";
        public const string GENERICSALE = "GENERICSALE";
        public const string LOCKER = "LOCKER";
        public const string LOCKERRETURN = "LOCKER_RETURN";
        public const string LOCKERDEPOSIT = "LOCKERDEPOSIT";
        public const string BOOKINGS = "BOOKINGS";
        public const string DEPOSIT = "DEPOSIT";
        public const string RENTALRETURN = "RENTAL_RETURN";
        public const string RENTAL = "RENTAL";
        public const string CASHREFUND = "CASHREFUND";
        public const string VOUCHER = "VOUCHER";
        public const string GAMEPLAYTRXPRODUCT = "GAMEPLAYTRXPRODUCT";
        public const string ACHIVEMENTS = "ACHIEVEMENTS";
        public const string LOADTICKETS = "LOADTICKETS";
        public const string EXCESSVOUCHERVALUE = "EXCESSVOUCHERVALUE";
        public const string INVENTORYINTERSTORE = "INVENTORYINTERSTORE";
        public const string SERVICECHARGE = "SERVICECHARGE";
        public const string PACKINGCHARGE = "PACKINGCHARGE";
        public const string GRATUITY = "GRATUITY";
        public const string VARIABLECARD = "VARIABLECARD";
    }
    public class ProductTypeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  productTypeId field
            /// </summary>
            PRODUCT_TYPE_ID,
            /// <summary>
            /// Search by  productType field
            /// </summary>
            PRODUCT_TYPE,
            /// <summary>
            /// Search by  activeFlag field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by  siteId field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by  masterEntityId field
            /// </summary>
            MASTERENTITYID,
            /// <summary>
            /// Search by  orderTypeId field
            /// </summary>
            ORDERTYPEID,
        }

        
        int productTypeId;
        string productType;
        string description;
        bool isActive;
        DateTime lastUpdatedDate;
        string lastUpdatedUser;
        int? internetKey;
        string guid;
        int siteId;
        bool synchStatus;
        bool cardSale;
        string reportGroup;
        int masterEntityId;
        int orderTypeId;
        string createdBy;
        DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductTypeDTO()
        {
            log.LogMethodEntry();
            productTypeId = -1;
            orderTypeId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductTypeDTO(int productTypeId, string productType, string description, bool isActive,
                                DateTime lastUpdatedDate, string lastUpdatedUser, int? internetKey,
                                string guid, int siteId, bool synchStatus, bool cardSale, string reportGroup,
                                int masterEntityId, int orderTypeId, string createdBy, DateTime creationDate)
        {
            log.LogMethodEntry(productTypeId, productType, description, isActive, lastUpdatedDate, lastUpdatedUser, internetKey,
                                guid, siteId, synchStatus, cardSale, reportGroup, masterEntityId, orderTypeId, createdBy, creationDate);
            this.productTypeId = productTypeId;
            this.productType = productType;
            this.description = description;
            this.isActive = isActive;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.internetKey = internetKey;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.cardSale = cardSale;
            this.reportGroup = reportGroup;
            this.masterEntityId = masterEntityId;
            this.orderTypeId = orderTypeId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the productTypeId field
        /// </summary>
        [DisplayName("productTypeId")]
        public int ProductTypeId
        {
            get
            {
                return productTypeId;
            }

            set
            {
                this.IsChanged = true;
                productTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the productType field
        /// </summary>
        [DisplayName("productType")]
        public string ProductType
        {
            get
            {
                return productType;
            }

            set
            {
                this.IsChanged = true;
                productType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the description field
        /// </summary>
        [DisplayName("description")]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.IsChanged = true;
                description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [DisplayName("isActive")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lastUpdatedDate field
        /// </summary>
        [DisplayName("lastUpdatedDate")]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
                lastUpdatedDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lastUpdatedUser field
        /// </summary>
        [DisplayName("lastUpdatedUser")]
        public string LastUpdatedUser
        {
            get
            {
                return lastUpdatedUser;
            }
            set
            {
                lastUpdatedUser = value;
            }
        }

        /// <summary>
        /// Get/Set method of the internetKey field
        /// </summary>
        [DisplayName("internetKey")]
        public int? InternetKey
        {
            get
            {
                return internetKey;
            }

            set
            {
                this.IsChanged = true;
                internetKey = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [DisplayName("guid")]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        [DisplayName("siteId")]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                this.IsChanged = true;
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        [DisplayName("synchStatus")]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the cardSale field
        /// </summary>
        [DisplayName("cardSale")]
        public bool CardSale
        {
            get
            {
                return cardSale;
            }
            set
            {
                this.IsChanged = true;
                cardSale = value;
            }
        }

        /// <summary>
        /// Get/Set method of the reportGroup field
        /// </summary>
        [DisplayName("reportGroup")]
        public string ReportGroup
        {
            get
            {
                return reportGroup;
            }

            set
            {
                this.IsChanged = true;
                reportGroup = value;
            }
        }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("masterEntityId")]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the orderTypeId field
        /// </summary>
        [DisplayName("orderTypeId")]
        public int OrderTypeId
        {
            get
            {
                return orderTypeId;
            }
            set
            {
                this.IsChanged = true;
                orderTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the orderTypeId field
        /// </summary>
        [DisplayName("createdBy")]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        [DisplayName("creationDate")]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || productTypeId < 0;
                }
            }
            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit(null);
        }
    }

}




