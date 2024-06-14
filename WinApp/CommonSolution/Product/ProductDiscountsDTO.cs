/********************************************************************************************
 * Project Name - ProductDiscounts DTO
 * Description  - Data object of ProductDiscounts
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017   Lakshminarayana          Created 
 *2.70        18-Mar-2019   Akshay Gulaganji        Modified isActive dataType(string to bool) 
 *2.110.00    30-Nov-2020   Abhishek                Modified : Modified to 3 Tier Standard
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the ProductDiscounts data object class. This acts as data holder for the ProductDiscounts business object
    /// </summary>
    public class ProductDiscountsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ProductDiscountId field
            /// </summary>
            PRODUCT_DISCOUNT_ID,
            /// <summary>
            /// Search by ProductId field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by ExpiryDate greater field
            /// </summary>
            EXPIRY_DATE_GREATER_THAN,
            /// <summary>
            /// Search by ExpiryDate less than field
            /// </summary>
            EXPIRY_DATE_LESS_THAN,
            /// <summary>
            /// Search by DiscountId field
            /// </summary>
            DISCOUNT_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int productDiscountId;
        private int productId;
        private int discountId;
        private DateTime? expiryDate;      
        private int? internetKey;
        private int? validFor;
        private string validForDaysMonths;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedUser;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductDiscountsDTO()
        {
            log.LogMethodEntry();
            productDiscountId = -1;
            discountId = -1;
            productId = -1;
            validForDaysMonths = "D";
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ProductDiscountsDTO(int productDiscountId, int productId, int discountId, DateTime? expiryDate, int? internetKey,
                                   int? validFor, string validForDaysMonths, bool isActive)
            : this()
        {
            log.LogMethodEntry(productDiscountId, productId, discountId, expiryDate, internetKey, validFor, validForDaysMonths, isActive);
            this.productDiscountId = productDiscountId;
            this.productId = productId;
            this.discountId = discountId;
            this.expiryDate = expiryDate;
            this.internetKey = internetKey;
            this.validFor = validFor;
            this.validForDaysMonths = validForDaysMonths;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductDiscountsDTO(int productDiscountId, int productId, int discountId, DateTime? expiryDate, string createdBy, DateTime creationDate, string lastUpdatedUser,
                                   DateTime lastUpdatedDate, int? internetKey, int? validFor, string validForDaysMonths, bool isActive, int siteId,
                                   int masterEntityId, bool synchStatus, string guid)
            : this(productDiscountId, productId, discountId, expiryDate, internetKey, validFor, validForDaysMonths, isActive)
        {
            log.LogMethodEntry(productDiscountId, productId, discountId, expiryDate, createdBy, creationDate, lastUpdatedUser, lastUpdatedDate,
                               internetKey, validFor, validForDaysMonths, isActive, siteId, masterEntityId, synchStatus, guid);        
            this.lastUpdatedUser = lastUpdatedUser;
            this.lastUpdatedDate = lastUpdatedDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProductDiscountId field
        /// </summary>
        public int ProductDiscountId { get { return productDiscountId; } set { this.IsChanged = true; productDiscountId = value; } }
      
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }

        /// <summary>
        /// Get/Set method of the DiscountId field
        /// </summary>
        public int DiscountId { get { return discountId; } set { this.IsChanged = true; discountId = value; } }

        /// <summary>
        /// Get/Set method of the InternetKey field
        /// </summary>
        public int? InternetKey { get { return internetKey; } set { this.IsChanged = true; internetKey = value; } }

        /// <summary>
        /// Get/Set method of the ValidFor field
        /// </summary>
        public int? ValidFor { get { return validFor; } set { this.IsChanged = true; validFor = value; } }

        /// <summary>
        /// Get/Set method of the ValidForDaysMonths field
        /// </summary>
        public string ValidForDaysMonths { get { return validForDaysMonths; } set { this.IsChanged = true; validForDaysMonths = value; } }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { this.IsChanged = true; expiryDate = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedUser field
        /// </summary>
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || productDiscountId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current ProductDiscountsDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            //log.Debug("Starts-ToString() method.");
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------ProductDiscountsDTO-----------------------------\n");
            returnValue.Append(" ProductDiscountId : " + ProductDiscountId);
            returnValue.Append(" ProductId : " + ProductId);
            returnValue.Append(" DiscountId : " + DiscountId);
            returnValue.Append(" ExpiryDate : " + ExpiryDate);
            returnValue.Append(" LastUpdatedUser : " + LastUpdatedUser);
            returnValue.Append(" LastUpdatedDate : " + LastUpdatedDate);
            returnValue.Append(" InternetKey : " + InternetKey);
            returnValue.Append(" ValidFor : " + ValidFor);
            returnValue.Append(" ValidForDaysMonths : " + ValidForDaysMonths);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            //log.Debug("Ends-ToString() Method");
            log.LogMethodEntry();
            return returnValue.ToString();

        }
    }
}
