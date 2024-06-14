/********************************************************************************************
 * Project Name - Discounts
 * Description  - Data structure of DiscountContainer
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      12-Apr-2021      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Data structure of DiscountContainerDTO
    /// </summary>
    public class DiscountContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int discountId;
        private string discountName;
        private double? discountAmount;
        private double? discountPercentage;
        private string couponMandatory;
        private bool discountCriteriaLines;
        private string discountType;
        public string automaticApply;
        private bool isActive;
        private string variableDiscounts;
        private double? minimumCredits;
        private double? minimumSaleAmount;
        private string displayInPOS;
        private string managerApprovalRequired;
        private string remarksMandatory;
        public int? applicationLimit;
        private int scheduleId;
        private int transactionProfileId;
        private bool allowMultipleApplication;
        private int? discountPurchaseCriteriaCount;
        private int? discountPurchaseCriteriaQuantityCount;
        private int? discountPurchaseCriteriaValidityQuantityCount;
        private bool? overridingDiscountAmountExists;
        private bool? overridingDiscountedPriceExists;
        private bool? overridingDiscountPercentageExists;
        private bool? allProductsAreDiscounted;
        private string transactionDiscountType;
        private int sortOrder;
        private List<DiscountPurchaseCriteriaContainerDTO> discountPurchaseCriteriaContainerDTOList = new List<DiscountPurchaseCriteriaContainerDTO>();
        private List<DiscountedProductsContainerDTO> discountedProductsContainerDTOList = new List<DiscountedProductsContainerDTO>();
        private List<DiscountedGamesContainerDTO> discountedGamesContainerDTOList = new List<DiscountedGamesContainerDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountContainerDTO()
        {
            log.LogMethodEntry();
            discountId = -1;
            discountPurchaseCriteriaCount = 0;
            discountPurchaseCriteriaQuantityCount = 0;
            discountPurchaseCriteriaValidityQuantityCount = 0;
            overridingDiscountAmountExists = false;
            overridingDiscountedPriceExists = false;
            overridingDiscountPercentageExists = false;
            allProductsAreDiscounted = false;
            discountedProductsContainerDTOList = new List<DiscountedProductsContainerDTO>();
            discountPurchaseCriteriaContainerDTOList = new List<DiscountPurchaseCriteriaContainerDTO>();
            discountedGamesContainerDTOList = new List<DiscountedGamesContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public DiscountContainerDTO(int discountId, string discountName, double? discountAmount, double? discountPercentage,
                                    string discountType, string managerApprovalRequired, string automaticApply, double? minimumCredits, double? minimumSaleAmount, bool discountCriteriaLines,string couponMandatory, 
                                    string variableDiscounts,string remarksMandatory, string displayInPOS, bool allowMultipleApplication, int transactionProfileId, int scheduleId , int? applicationLimit, bool isActive, int? discountPurchaseCriteriaCount,
                                    int? discountPurchaseCriteriaQuantityCount, int? discountPurchaseCriteriaValidityQuantityCount,bool? overridingDiscountAmountExists,
                                    bool? overridingDiscountedPriceExists,bool? allProductsAreDiscounted, string transactionDiscountType, int sortOrder)
            : this()
        {
            log.LogMethodEntry(discountId,discountPercentage, discountType, managerApprovalRequired, automaticApply, minimumCredits, minimumSaleAmount, discountCriteriaLines, couponMandatory,
                               variableDiscounts, remarksMandatory, displayInPOS, allowMultipleApplication, transactionProfileId, scheduleId, applicationLimit, isActive, discountPurchaseCriteriaCount,
                               discountPurchaseCriteriaQuantityCount, discountPurchaseCriteriaValidityQuantityCount, overridingDiscountAmountExists,
                               overridingDiscountedPriceExists, allProductsAreDiscounted, transactionDiscountType, sortOrder);
            this.discountId = discountId;
            this.discountName = discountName;
            this.managerApprovalRequired = managerApprovalRequired;
            this.discountPercentage = discountPercentage;
            this.discountAmount = discountAmount;
            this.discountType = discountType;
            this.minimumCredits = minimumCredits;
            this.minimumSaleAmount = minimumSaleAmount;
            this.automaticApply = automaticApply;
            this.discountCriteriaLines = discountCriteriaLines;
            this.couponMandatory = couponMandatory;
            this.variableDiscounts = variableDiscounts;
            this.displayInPOS = displayInPOS;
            this.allowMultipleApplication = allowMultipleApplication;
            this.remarksMandatory = remarksMandatory;
            this.transactionProfileId = transactionProfileId;
            this.scheduleId = scheduleId;
            this.applicationLimit = applicationLimit;
            this.isActive = isActive;
            this.discountPurchaseCriteriaCount = discountPurchaseCriteriaCount;
            this.discountPurchaseCriteriaQuantityCount = discountPurchaseCriteriaQuantityCount;
            this.discountPurchaseCriteriaValidityQuantityCount = discountPurchaseCriteriaValidityQuantityCount;
            this.overridingDiscountAmountExists = overridingDiscountAmountExists;
            this.overridingDiscountedPriceExists = overridingDiscountedPriceExists;
            this.allProductsAreDiscounted = allProductsAreDiscounted;
            this.transactionDiscountType = transactionDiscountType;
            this.sortOrder = sortOrder;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the discountId field
        /// </summary>
        public int DiscountId
        {
            get { return discountId; }
            set { discountId = value; }
        }

        /// <summary>
        /// Get/Set method of the discountName field
        /// </summary>
        public string DiscountName
        {
            get { return discountName; }
            set { discountName = value; }
        }

        /// <summary>
        /// Get/Set method of the discountPercentage field
        /// </summary>
        public double? DiscountPercentage
        {
            get { return discountPercentage; }
            set { discountPercentage = value; }
        }

        /// <summary>
        /// Get/Set method of the discountAmount field
        /// </summary>
        public double? DiscountAmount
        {
            get { return discountAmount; }
            set { discountAmount = value; }
        }

        /// <summary>
        /// Get/Set method of the managerApprovalRequired field
        /// </summary>
        public string ManagerApprovalRequired
        {
            get { return managerApprovalRequired; }
            set { managerApprovalRequired = value; }
        }

        /// <summary>
        /// Get/Set method of the remarksMandatory field
        /// </summary>
        public string RemarksMandatory
        {
            get { return remarksMandatory; }
            set { remarksMandatory = value; }
        }

        /// <summary>
        /// Get/Set method of the minimumCredits field
        /// </summary>
        public double? MinimumCredits
        {
            get { return minimumCredits; }
            set { minimumCredits = value; }
        }

        /// <summary>
        /// Get/Set method of the minimumSaleAmount field
        /// </summary>
        public double? MinimumSaleAmount
        {
            get { return minimumSaleAmount; }
            set { minimumSaleAmount = value; }
        }

        /// <summary>
        /// Get/Set method of the couponMandatory field
        /// </summary>
        public string CouponMandatory
        {
            get { return couponMandatory; }
            set { couponMandatory = value; }
        }

        /// <summary>
        /// Get/Set method of the discountType field
        /// </summary>
        public string DiscountType
        {
            get { return discountType; }
            set { discountType = value; }
        }

        /// <summary>
        /// Get/Set method of the applicationLimit field
        /// </summary>
        public int? ApplicationLimit
        {
            get { return applicationLimit; }
            set { applicationLimit = value; }
        }

        /// <summary>
        /// Get/Set method of the automaticApply field
        /// </summary>
        public string AutomaticApply
        {
            get { return automaticApply; }
            set{ automaticApply = value; }
        }

        /// <summary>
        /// Get/Set method of the displayInPOS field
        /// </summary>
        public string DisplayInPOS
        {
            get{ return displayInPOS; }
            set {displayInPOS = value; }
        }

        /// <summary>
        /// Get/Set method of the allowMultipleApplication field
        /// </summary>
        public bool AllowMultipleApplication
        {
            get { return allowMultipleApplication; }
            set {  allowMultipleApplication = value; }
        }

        /// <summary>
        /// Get/Set method of the transactionProfileId field
        /// </summary>
        public int TransactionProfileId
        {
            get { return transactionProfileId; }
            set { transactionProfileId = value; }
        }

        /// <summary>
        /// Get/Set method of the variableDiscounts field
        /// </summary>
        public string VariableDiscounts
        {
            get { return variableDiscounts; }
            set { variableDiscounts = value; }
        }

        /// <summary>
        /// Get/Set method of the discountCriteriaLines field
        /// </summary>
        public bool DiscountCriteriaLines
        {
            get { return discountCriteriaLines; }
            set { discountCriteriaLines = value; }
        }

        /// <summary>
        /// Get/Set method of the scheduleId field
        /// </summary>
        public int ScheduleId
        {
            get { return scheduleId; }
            set { scheduleId = value; }
        }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive
        {
            get { return isActive;}
            set{ isActive = value; }
        }

        /// <summary>
        /// Get/Set method of the discountPurchaseCriteriaCount field
        /// </summary>
        public int? DiscountPurchaseCriteriaCount
        {
            get
            {
                return discountPurchaseCriteriaCount.Value;
            }
            set
            {
                discountPurchaseCriteriaCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the discountPurchaseCriteriaQuantityCount field
        /// </summary>
        public int? DiscountPurchaseCriteriaQuantityCount
        {
            get
            {
                return discountPurchaseCriteriaQuantityCount.Value;
            }
            set
            {
                discountPurchaseCriteriaQuantityCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the discountPurchaseCriteriaValidityQuantityCount field
        /// </summary>
        public int? DiscountPurchaseCriteriaValidityQuantityCount
        {
            get
            {
                return discountPurchaseCriteriaValidityQuantityCount.Value;
            }
            set
            {
                discountPurchaseCriteriaValidityQuantityCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the overridingDiscountAmountExists field
        /// </summary>
        public bool OverridingDiscountAmountExists
        {
            get
            {
                return overridingDiscountAmountExists.Value;
            }
            set
            {
                overridingDiscountAmountExists = value;
            }
        }

        /// <summary>
        /// Get/Set method of the overridingDiscountedPriceExists field
        /// </summary>
        public bool OverridingDiscountedPriceExists
        {
            get
            {
                return overridingDiscountedPriceExists.Value;
            }
            set
            {
                overridingDiscountedPriceExists = value;
            }
        }

        /// <summary>
        /// Get/Set method of the overridingDiscountPercentageExists field
        /// </summary>
        public bool OverridingDiscountPercentageExists
        {
            get
            {
                return overridingDiscountPercentageExists.Value;
            }
            set
            {
                overridingDiscountPercentageExists = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        public int SortOrder
        {
            get
            {
                return sortOrder;
            }
            set
            {
                sortOrder = value;
            }
        }

        /// <summary>
        /// Get/Set method of the transactionDiscountType field
        /// </summary>
        public string TransactionDiscountType
        {
            get
            {
                return transactionDiscountType;
            }
            set
            {
                transactionDiscountType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the allProductsAreDiscounted field
        /// </summary>
        public bool AllProductsAreDiscounted
        {
            get
            {
                return allProductsAreDiscounted.Value;
            }
            set
            {
                allProductsAreDiscounted = value;
            }
        }

        /// <summary>
        /// Get method of discountPurchaseCriteriaContainerDTOList field 
        /// </summary>
        public List<DiscountPurchaseCriteriaContainerDTO> DiscountPurchaseCriteriaContainerDTOList
        {
            get
            {
                return discountPurchaseCriteriaContainerDTOList;
            }
            set
            {
                discountPurchaseCriteriaContainerDTOList = value;
            }
        }

       

        /// <summary>
        /// Get/Set method of discountedProductsContainerDTOList field 
        /// </summary>
        public List<DiscountedProductsContainerDTO> DiscountedProductsContainerDTOList
        {
            get { return discountedProductsContainerDTOList; }
            set { discountedProductsContainerDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of discountedGamesContainerDTOList field 
        /// </summary>
        public List<DiscountedGamesContainerDTO> DiscountedGamesContainerDTOList
        {
            get { return discountedGamesContainerDTOList; }
            set { discountedGamesContainerDTOList = value; }
        }
    }
}
