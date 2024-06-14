/********************************************************************************************
 * Project Name - ProductCreditPlus DTO  
 * Description  - Data object of ProductCreditPlusDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *2.70        31-Jan-2019   Indrajeet Kumar             Created
 *2.80.0      31-Jan-2019   Girish Kundar               Modified : Added EffectiveAfterMinutes field , modified as per 3 Tier standard
 *2.80.0      04-May-2020   Akshay Gulaganji            Added search parameter - PRODUCTCREDITPLUS_ID_LIST
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductCreditPlusDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByHubParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// PRODUCTCREDITPLUS_ID search field
            /// </summary>
            PRODUCTCREDITPLUS_ID,
            /// <summary>
            /// PRODUCT_ID search field
            /// </summary>
            PRODUCT_ID,
            // <summary>
            /// SITE_ID search field
            /// </summary>
            SITE_ID,
            // <summary>
            ///  MASTERENTITY_ID search field
            /// </summary>
            MASTERENTITY_ID,
            // <summary>
            ///  ISACTIVE search field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// PRODUCT_ID PRODUCT_ID_LIST field
            /// </summary>
            PRODUCT_ID_LIST,
            // <summary>
            ///  PRODUCTCREDITPLUS_ID_LIST search field
            /// </summary>
            PRODUCTCREDITPLUS_ID_LIST
        }

        private int productCreditPlusId;
        private decimal creditPlus;
        private string refundable;
        private string remarks;
        private int product_id;
        private string creditPlusType;
        private string guid;
        private int site_id;
        private bool synchStatus;
        private DateTime periodFrom;
        private DateTime periodTo;
        private int? validForDays;
        private string extendOnReload;
        private decimal timeFrom;
        private decimal timeTo;
        private int? minutes;
        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
        private bool ticketAllowed;
        private int masterEntityId;
        private string frequency;
        private bool pauseAllowed;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool isActive;
        private List<CreditPlusConsumptionRulesDTO> creditPlusConsumptionRulesList;
        private int effectiveAfterMinutes;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductCreditPlusDTO()
        {
            log.LogMethodEntry();
            this.productCreditPlusId = -1;
            this.refundable = "Y";
            this.monday = true;
            this.tuesday = true;
            this.wednesday = true;
            this.thursday = true;
            this.friday = true;
            this.saturday = true;
            this.sunday = true;
            this.ticketAllowed = true;
            this.pauseAllowed = true;
            this.masterEntityId = -1;
            this.site_id = -1;
            this.isActive = true;
            this.timeFrom = -1;
            this.timeTo = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor required data fields
        /// </summary>
        public ProductCreditPlusDTO(int productCreditPlusId, decimal creditPlus, string refundable, string remarks, int product_id,
                                    string creditPlusType, DateTime periodFrom,
                                    DateTime periodTo, int? validForDays, string extendOnReload, decimal timeFrom, decimal timeTo, int? minutes,
                                    bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday,
                                    bool ticketAllowed,string frequency, bool pauseAllowed,
                                    bool isActive, int effectiveAfterMinutes = 0)
            :this()
        {
            log.LogMethodEntry(productCreditPlusId, creditPlus, refundable, remarks, product_id, creditPlusType, periodFrom,
                               periodTo, validForDays, extendOnReload, timeFrom, timeTo, minutes, monday, tuesday, wednesday, thursday, friday, saturday, sunday,
                               ticketAllowed,  frequency, pauseAllowed,isActive, effectiveAfterMinutes);

            this.productCreditPlusId = productCreditPlusId;
            this.creditPlus = creditPlus;
            this.refundable = refundable;
            this.remarks = remarks;
            this.product_id = product_id;
            this.creditPlusType = creditPlusType;
            this.periodFrom = periodFrom;
            this.periodTo = periodTo;
            this.validForDays = validForDays;
            this.extendOnReload = extendOnReload;
            this.timeFrom = timeFrom;
            this.timeTo = timeTo;
            this.minutes = minutes;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.ticketAllowed = ticketAllowed;
            this.frequency = frequency;
            this.pauseAllowed = pauseAllowed;
            this.isActive = isActive;
            creditPlusConsumptionRulesList = new List<CreditPlusConsumptionRulesDTO>();
            this.effectiveAfterMinutes = effectiveAfterMinutes;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public ProductCreditPlusDTO(int productCreditPlusId, decimal creditPlus, string refundable, string remarks, int product_id,
                                    string creditPlusType, string guid, int site_id, bool synchStatus, DateTime periodFrom,
                                    DateTime periodTo, int? validForDays, string extendOnReload, decimal timeFrom, decimal timeTo, int? minutes,
                                    bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday,
                                    bool ticketAllowed, int masterEntityId, string frequency, bool pauseAllowed, string createdBy, DateTime creationDate,
                                    string lastUpdatedBy, DateTime lastUpdateDate,bool isActive, int effectiveAfterMinutes =0)
        {
            log.LogMethodEntry(productCreditPlusId, creditPlus, refundable, remarks, product_id, creditPlusType, guid, site_id, synchStatus, periodFrom,
                               periodTo, validForDays, extendOnReload, timeFrom, timeTo, minutes, monday, tuesday, wednesday, thursday, friday, saturday, sunday,
                               ticketAllowed, masterEntityId, frequency, pauseAllowed, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, isActive, effectiveAfterMinutes);

            this.productCreditPlusId = productCreditPlusId;
            this.creditPlus = creditPlus;
            this.refundable = refundable;
            this.remarks = remarks;
            this.product_id = product_id;
            this.creditPlusType = creditPlusType;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.periodFrom = periodFrom;
            this.periodTo = periodTo;
            this.validForDays = validForDays;
            this.extendOnReload = extendOnReload;
            this.timeFrom = timeFrom;
            this.timeTo = timeTo;
            this.minutes = minutes;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.ticketAllowed = ticketAllowed;
            this.masterEntityId = masterEntityId;
            this.frequency = frequency;
            this.pauseAllowed = pauseAllowed;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = isActive;
            creditPlusConsumptionRulesList = new List<CreditPlusConsumptionRulesDTO>();
            this.effectiveAfterMinutes = effectiveAfterMinutes;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set for ProductCreditPlusId
        /// </summary>
        public int ProductCreditPlusId { get { return productCreditPlusId; } set { productCreditPlusId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CreditPlus
        /// </summary>
        public decimal CreditPlus { get { return creditPlus; } set { creditPlus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Refundable
        /// </summary>
        public string Refundable { get { return refundable; } set { refundable = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Remarks
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set for ProductId
        /// </summary>
        public int Product_id { get { return product_id; } set { product_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CreditPlusType
        /// </summary>
        public string CreditPlusType { get { return creditPlusType; } set { creditPlusType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for SiteId
        /// </summary>
        public int Site_id { get { return site_id; } set { site_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for SynchStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set for PeriodFrom
        /// </summary>
        public DateTime PeriodFrom { get { return periodFrom; } set { periodFrom = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for PeriodTo
        /// </summary>
        public DateTime PeriodTo { get { return periodTo; } set { periodTo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for ValidForDays
        /// </summary>
        public int? ValidForDays { get { return validForDays; } set { validForDays = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for ExtendOnReload
        /// </summary>
        public string ExtendOnReload { get { return extendOnReload; } set { extendOnReload = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for TimeFrom
        /// </summary>
        public decimal TimeFrom { get { return timeFrom; } set { timeFrom = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for TimeToss
        /// </summary>
        public decimal TimeTo { get { return timeTo; } set { timeTo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Minutes
        /// </summary>
        public int? Minutes { get { return minutes; } set { minutes = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Monday
        /// </summary>
        public bool Monday { get { return monday; } set { monday = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Tuesday
        /// </summary>
        public bool Tuesday { get { return tuesday; } set { tuesday = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Wednesday
        /// </summary>
        public bool Wednesday { get { return wednesday; } set { wednesday = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Thursday
        /// </summary>
        public bool Thursday { get { return thursday; } set { thursday = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Friday
        /// </summary>
        public bool Friday { get { return friday; } set { friday = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Saturday
        /// </summary>
        public bool Saturday { get { return saturday; } set { saturday = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Sunday
        /// </summary>
        public bool Sunday { get { return sunday; } set { sunday = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for TicketAllowed
        /// </summary>
        public bool TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Frequency
        /// </summary>
        public string Frequency { get { return frequency; } set { frequency = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for PauseAllowed
        /// </summary>
        public bool PauseAllowed { get { return pauseAllowed; } set { pauseAllowed = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CreatedBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set for CreationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set for LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set for LastUpdateDate
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CreditPlusConsumptionRulesList Field
        /// </summary>
        public List<CreditPlusConsumptionRulesDTO> CreditPlusConsumptionRulesList { get { return creditPlusConsumptionRulesList; } set { creditPlusConsumptionRulesList = value; this.IsChanged = true; } }

        /// <summary> 
        /// Get/Set for ProductCreditPlusId
        /// </summary>
        public int EffectiveAfterMinutes { get { return effectiveAfterMinutes; } set { effectiveAfterMinutes = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || productCreditPlusId < 0;
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
            log.LogMethodExit();
        }
    }
}
