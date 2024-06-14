/********************************************************************************************
 * Project Name - Discounts DTO
 * Description  - Data object of Discounts
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        12-Jul-2017   Lakshminarayana          Created  
 *2.70.2        30-Jul-2019   Girish Kundar           Modified : Added constructor with required Parameter, Missing Who columns
 *                                                              and IsRecurssive() method for child lists.
 *2.150.0      29-Apr-2021      Abhishek             Modified : added schedulecalendarDTO for scheduleId                                                         
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the Discounts data object class. This acts as data holder for the Discounts business object
    /// </summary>
    public class DiscountsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by DiscountId field
            /// </summary>
            DISCOUNT_ID,
            /// <summary>
            /// Search by DiscountId LIST field
            /// </summary>
            DISCOUNT_ID_LIST,
            /// <summary>
            /// Search by DiscountType field
            /// </summary>
            DISCOUNT_TYPE,
            /// <summary>
            /// Search by DiscountName field
            /// </summary>
            DISCOUNT_NAME,
            /// <summary>
            /// Search by DisplayInPos field
            /// </summary>
            DISPLAY_IN_POS,
            /// <summary>
            /// Search by Minimum Credits Greater Than field
            /// </summary>
            MINIMUM_CREDITS_GREATER_THAN,
            /// <summary>
            /// Search by Minimum Sale Amount Greater Than field
            /// </summary>
            MINIMUM_SALE_AMOUNT_GREATER_THAN,
            /// <summary>
            /// Search by ActiveFlag field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by AutomaticApply field
            /// </summary>
            AUTOMATIC_APPLY,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by DiscountedGameId field
            /// </summary>
            DISCOUNTED_GAME_ID,
            /// <summary>
            /// Search by DiscountedProductId field
            /// </summary>
            DISCOUNTED_PRODUCT_ID,
            /// <summary>
            /// Search by DiscountedCategoryId field
            /// </summary>
            DISCOUNTED_CATEGORY_ID,
            /// <summary>
            /// Search by CouponMandatory field
            /// </summary>
            COUPON_MANDATORY


        }

        private int discountId;
        private string discountName;
        private double? discountPercentage;
        private string automaticApply;
        private double? minimumSaleAmount;
        private double? minimumCredits;
        private string displayInPOS;
        private int sortOrder;
        private string managerApprovalRequired;
        private int? internetKey;
        private string discountType;
        private string couponMandatory;
        private double? discountAmount;
        private string remarksMandatory;
        private string variableDiscounts;
        private int scheduleId;
        private int transactionProfileId;
        private bool isActive;
        private bool discountCriteriaLines;
        private bool allowMultipleApplication;
        private int? applicationLimit;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private List<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOList;
        private List<DiscountedProductsDTO> discountedProductsDTOList;
        private List<DiscountedGamesDTO> discountedGamesDTOList;
        private ScheduleCalendarDTO scheduleCalendarDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountsDTO()
        {
            log.LogMethodEntry();
            discountId = -1;
            discountName = string.Empty;
            couponMandatory = "N";
            variableDiscounts = "N";
            remarksMandatory = "N";
            displayInPOS = "N";
            automaticApply = "N";
            managerApprovalRequired = "N";
            scheduleId = -1;
            transactionProfileId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            discountPurchaseCriteriaDTOList = new List<DiscountPurchaseCriteriaDTO>();
            discountedProductsDTOList = new List<DiscountedProductsDTO>();
            discountedGamesDTOList = new List<DiscountedGamesDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public DiscountsDTO(int discountId, string discountName, double? discountPercentage, string automaticApply,
                            double? minimumSaleAmount, double? minimumCredits, string displayInPOS, int sortOrder,
                            string managerApprovalRequired, int? internetKey, string discountType, string couponMandatory,
                            double? discountAmount, string remarksMandatory, string variableDiscounts, int scheduleId,
                            int transactionProfileId, bool isActive, bool discountCriteriaLines, bool allowMultipleApplication, int? applicationLimit)
            : this()
        {
            log.LogMethodEntry(discountId, discountName, discountPercentage, automaticApply,
                             minimumSaleAmount, minimumCredits, displayInPOS, sortOrder,
                             managerApprovalRequired, internetKey, discountType, couponMandatory,
                             discountAmount, remarksMandatory, variableDiscounts, scheduleId,
                             transactionProfileId, isActive, discountCriteriaLines, allowMultipleApplication, applicationLimit);
            this.discountId = discountId;
            this.discountName = discountName;
            this.discountPercentage = discountPercentage;
            this.automaticApply = automaticApply;
            this.minimumSaleAmount = minimumSaleAmount;
            this.minimumCredits = minimumCredits;
            this.displayInPOS = displayInPOS;
            this.sortOrder = sortOrder;
            this.managerApprovalRequired = managerApprovalRequired;
            this.internetKey = internetKey;
            this.discountType = discountType;
            this.couponMandatory = couponMandatory;
            this.discountAmount = discountAmount;
            this.remarksMandatory = remarksMandatory;
            this.variableDiscounts = variableDiscounts;
            this.scheduleId = scheduleId;
            this.transactionProfileId = transactionProfileId;
            this.isActive = isActive;
            this.discountCriteriaLines = discountCriteriaLines;
            this.allowMultipleApplication = allowMultipleApplication;
            this.applicationLimit = applicationLimit;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountsDTO(int discountId, string discountName, double? discountPercentage, string automaticApply,
                            double? minimumSaleAmount, double? minimumCredits, string displayInPOS, int sortOrder,
                            string managerApprovalRequired, int? internetKey, string discountType, string couponMandatory,
                            double? discountAmount, string remarksMandatory, string variableDiscounts, int scheduleId,
                            int transactionProfileId, bool isActive, bool discountCriteriaLines, bool allowMultipleApplication, 
                            int? applicationLimit, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                            int masterEntityId, bool synchStatus, string guid, string createdBy, DateTime creationDate)
            : this(discountId, discountName, discountPercentage, automaticApply,
                             minimumSaleAmount, minimumCredits, displayInPOS, sortOrder,
                             managerApprovalRequired, internetKey, discountType, couponMandatory,
                             discountAmount, remarksMandatory, variableDiscounts, scheduleId,
                             transactionProfileId, isActive, discountCriteriaLines, allowMultipleApplication, applicationLimit)
        {
            log.LogMethodEntry(discountId, discountName, discountPercentage, automaticApply,
                             minimumSaleAmount, minimumCredits, displayInPOS, sortOrder,
                             managerApprovalRequired, internetKey, discountType, couponMandatory,
                             discountAmount, remarksMandatory, variableDiscounts, scheduleId,
                             transactionProfileId, isActive, discountCriteriaLines, allowMultipleApplication,
                             applicationLimit, lastUpdatedBy, lastUpdatedDate, siteId,
                             masterEntityId, synchStatus, guid, createdBy, creationDate);

            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountsDTO(DiscountsDTO discountsDTO)
            : this(discountsDTO.discountId, discountsDTO.discountName, 
                   discountsDTO.discountPercentage, discountsDTO.automaticApply,
                   discountsDTO.minimumSaleAmount, discountsDTO.minimumCredits,
                   discountsDTO.displayInPOS, discountsDTO.sortOrder,
                   discountsDTO.managerApprovalRequired, discountsDTO.internetKey,
                   discountsDTO.discountType, discountsDTO.couponMandatory,
                   discountsDTO.discountAmount, discountsDTO.remarksMandatory,
                   discountsDTO.variableDiscounts, discountsDTO.scheduleId,
                   discountsDTO.transactionProfileId, discountsDTO.isActive,
                   discountsDTO.discountCriteriaLines, discountsDTO.allowMultipleApplication,
                   discountsDTO.applicationLimit,
                   discountsDTO.lastUpdatedBy, discountsDTO.lastUpdatedDate,
                   discountsDTO.siteId, discountsDTO.masterEntityId,
                   discountsDTO.synchStatus, discountsDTO.guid,
                   discountsDTO.createdBy, discountsDTO.creationDate)
        {
            log.LogMethodEntry(discountsDTO);

            if(discountsDTO.discountPurchaseCriteriaDTOList != null)
            {
                foreach (var discountPurchaseCriteriaDTO in discountsDTO.discountPurchaseCriteriaDTOList)
                {
                    DiscountPurchaseCriteriaDTO copy = new DiscountPurchaseCriteriaDTO(discountPurchaseCriteriaDTO);
                    discountPurchaseCriteriaDTOList.Add(copy);
                }
            }
            if (discountsDTO.discountedProductsDTOList != null)
            {
                foreach (var discountedProductsDTO in discountsDTO.discountedProductsDTOList)
                {
                    DiscountedProductsDTO copy = new DiscountedProductsDTO(discountedProductsDTO);
                    discountedProductsDTOList.Add(copy);
                }
            }
            if (discountsDTO.discountedGamesDTOList != null)
            {
                foreach (var discountedGamesDTO in discountsDTO.discountedGamesDTOList)
                {
                    DiscountedGamesDTO copy = new DiscountedGamesDTO(discountedGamesDTO);
                    discountedGamesDTOList.Add(copy);
                }
            }
            if(discountsDTO.scheduleCalendarDTO != null)
            {
                scheduleCalendarDTO = new ScheduleCalendarDTO(discountsDTO.scheduleCalendarDTO);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the discountId field
        /// </summary>
        [DisplayName("Discount Id")]
        [ReadOnly(true)]
        public int DiscountId
        {
            get
            {
                return discountId;
            }

            set
            {
                this.IsChanged = true;
                discountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountName field
        /// </summary>
        [DisplayName("Discount Name")]
        public string DiscountName
        {
            get
            {
                return discountName;
            }

            set
            {
                this.IsChanged = true;
                discountName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the automaticApply field
        /// </summary>
        [DisplayName("Automatic Apply")]
        public string AutomaticApply
        {
            get
            {
                return automaticApply;
            }

            set
            {
                this.IsChanged = true;
                automaticApply = value;
            }
        }

        /// <summary>
        /// Get/Set method of the discountPercentage field
        /// </summary>
        [DisplayName("Discount Percentage")]
        public double? DiscountPercentage
        {
            get
            {
                return discountPercentage;
            }

            set
            {
                this.IsChanged = true;
                discountPercentage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountAmount field
        /// </summary>
        [DisplayName("Discount Amount")]
        public double? DiscountAmount
        {
            get
            {
                return discountAmount;
            }

            set
            {
                this.IsChanged = true;
                discountAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MinimumSaleAmount field
        /// </summary>
        [DisplayName("Minimum Sale Amount")]
        public double? MinimumSaleAmount
        {
            get
            {
                return minimumSaleAmount;
            }

            set
            {
                this.IsChanged = true;
                minimumSaleAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MinimumCredits field
        /// </summary>
        [DisplayName("Minimum Used Credits")]
        public double? MinimumCredits
        {
            get
            {
                return minimumCredits;
            }

            set
            {
                this.IsChanged = true;
                minimumCredits = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Schedule field
        /// </summary>
        [DisplayName("Schedule")]
        [ReadOnly(true)]
        public int ScheduleId
        {
            get
            {
                return scheduleId;
            }

            set
            {
                this.IsChanged = true;
                scheduleId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionProfileId field
        /// </summary>
        [DisplayName("Transaction Profile")]
        public int TransactionProfileId
        {
            get
            {
                return transactionProfileId;
            }

            set
            {
                this.IsChanged = true;
                transactionProfileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DisplayInPOS field
        /// </summary>
        [DisplayName("Display In POS")]
        public string DisplayInPOS
        {
            get
            {
                return displayInPOS;
            }

            set
            {
                this.IsChanged = true;
                displayInPOS = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active Flag")]
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
        /// Get/Set method of the SortOrder field
        /// </summary>
        [DisplayName("Display Order")]
        public int SortOrder
        {
            get
            {
                return sortOrder;
            }

            set
            {
                this.IsChanged = true;
                sortOrder = value;
            }
        }

        /// <summary>
        /// Get/Set method of the managerApprovalRequired field
        /// </summary>
        [DisplayName("Manager Approval Required")]
        public string ManagerApprovalRequired
        {
            get
            {
                return managerApprovalRequired;
            }

            set
            {
                this.IsChanged = true;
                managerApprovalRequired = value;
            }
        }

        /// <summary>
        /// Get/Set method of the VariableDiscounts field
        /// </summary>
        [DisplayName("Variable Discounts")]
        public string VariableDiscounts
        {
            get
            {
                return variableDiscounts;
            }

            set
            {
                this.IsChanged = true;
                variableDiscounts = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CouponMandatory field
        /// </summary>
        [DisplayName("Coupon Mandatory?")]
        public string CouponMandatory
        {
            get
            {
                return couponMandatory;
            }

            set
            {
                this.IsChanged = true;
                couponMandatory = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RemarksMandatory field
        /// </summary>
        [DisplayName("Remarks Mandatory")]
        public string RemarksMandatory
        {
            get
            {
                return remarksMandatory;
            }

            set
            {
                this.IsChanged = true;
                remarksMandatory = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountCriteriaLines field
        /// </summary>
        [DisplayName("Discount criteria lines")]
        public bool DiscountCriteriaLines
        {
            get
            {
                return discountCriteriaLines;
            }
            set
            {
                this.IsChanged = true;
                discountCriteriaLines = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AllowMultipleApplication field
        /// </summary>
        [DisplayName("Allow Multiple Application")]
        public bool AllowMultipleApplication
        {
            get
            {
                return allowMultipleApplication;
            }
            set
            {   this.IsChanged = true;
                allowMultipleApplication = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ApplicationLimit field
        /// </summary>
        [DisplayName("Application Limit")]
        public int? ApplicationLimit
        {
            get
            {
                return applicationLimit;
            }
            set
            {   this.IsChanged = true;
                applicationLimit = value;
            }
        }

        /// <summary>
        /// Get/Set method of the InternetKey field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the discountType field
        /// </summary>
        [Browsable(false)]
        public string DiscountType
        {
            get
            {
                return discountType;
            }

            set
            {
                this.IsChanged = true;
                discountType = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
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
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated User")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
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
        /// Get method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [Browsable(false)]
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
        /// Get method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
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
        /// Get method of the CreationDate field
        /// </summary>
        [Browsable(false)]
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
                    return notifyingObjectIsChanged || discountId < 0;
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
        /// Get/Set methods for discountedGamesDTOs
        /// </summary>
        public List<DiscountedGamesDTO> DiscountedGamesDTOList
        {
            get
            {
                return discountedGamesDTOList;
            }
            set
            {
                discountedGamesDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set methods for discountPurchaseCriteriaDTOs
        /// </summary>
        public List<DiscountPurchaseCriteriaDTO> DiscountPurchaseCriteriaDTOList
        {
            get
            {
                return discountPurchaseCriteriaDTOList;
            }
            set
            {
                discountPurchaseCriteriaDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set methods for discountedProductsDTOs
        /// </summary>
        public List<DiscountedProductsDTO> DiscountedProductsDTOList
        {
            get
            {
                return discountedProductsDTOList;
            }
            set
            {
                discountedProductsDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set methods for scheduleCalendarDTO
        /// </summary>
        public ScheduleCalendarDTO ScheduleCalendarDTO        
        {
            get
            {
                return scheduleCalendarDTO;
            }
            set
            {
                scheduleCalendarDTO = value;
            }
        }

        /// <summary>
        /// Returns true /false whether the DiscountDTO changed or any of its Children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if(scheduleCalendarDTO != null &&
                   scheduleCalendarDTO.IsChangedRecursive)
                {
                    return true;
                }

                if (discountedGamesDTOList != null &&
                   discountedGamesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (discountPurchaseCriteriaDTOList != null &&
                  discountPurchaseCriteriaDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (discountedProductsDTOList != null &&
                  discountedProductsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        


        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            IsChanged = false;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns a string that represents the current DiscountsDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DiscountsDTO-----------------------------\n");
            returnValue.Append(" DiscountId : " + DiscountId);
            returnValue.Append(" DiscountName : " + DiscountName);
            returnValue.Append(" DiscountPercentage : " + DiscountPercentage);
            returnValue.Append(" MinimumSaleAmount : " + MinimumSaleAmount);
            returnValue.Append(" MinimumCredits : " + MinimumCredits);
            returnValue.Append(" DisplayInPOS : " + DisplayInPOS);
            returnValue.Append(" SortOrder : " + SortOrder);
            returnValue.Append(" ManagerApprovalRequired : " + ManagerApprovalRequired);
            returnValue.Append(" InternetKey : " + InternetKey);
            returnValue.Append(" DiscountType : " + DiscountType);
            returnValue.Append(" CouponMandatory : " + CouponMandatory);
            returnValue.Append(" DiscountAmount : " + DiscountAmount);
            returnValue.Append(" RemarksMandatory : " + RemarksMandatory);
            returnValue.Append(" VariableDiscounts : " + VariableDiscounts);
            returnValue.Append(" ScheduleId : " + ScheduleId);
            returnValue.Append(" TransactionProfileId : " + TransactionProfileId);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue);
            return returnValue.ToString();

        }
    }
}
