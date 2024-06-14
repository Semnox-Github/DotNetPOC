/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of Promotions
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        24-May-2019   Girish Kundar           Created 
 *            18-Jul-2019   Mushahid Faizan         Changed ActiveFlag datatype from char to bool
 *2.80        04-Dec-2019   Rakesh                  Add promotionExclusionDateDTOList property   
 *2.80        31-Mar-2020   Mushahid Faizan         Modified as per the Rest API Phase 1 changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the PromotionDTO data object class. This acts as data holder for the Promotions business object
    /// </summary>
    public class PromotionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    PROMOTION ID field
            /// </summary>
            PROMOTION_ID,
            /// <summary>
            /// Search by    PROMOTION NAME field
            /// </summary>
            PROMOTION_NAME,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by  PROMOTION TYPE field
            /// </summary>
            PROMOTION_TYPE,
            /// <summary>
            /// Search by RECURRENT TYPE field
            /// </summary>
            RECUR_TYPE,
            /// <summary>
            /// Search by  PROMOTION TIME FROM field
            /// </summary>
            TIME_FROM,
            /// <summary>
            /// Search by  PROMOTION TIME TO  field
            /// </summary>
            TIME_TO,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int promotionId;
        private string promotionName;
        private DateTime timeFrom;
        private DateTime timeTo;
        private bool activeFlag;
        private char? recurFlag;
        private char? recurFrequency;
        private DateTime? recurEndDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int? internetKey;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private char? promotionType;
        private char? recurType;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private List<PromotionDetailDTO> promotionDetailDTOList;
        private List<PromotionRuleDTO> promotionRuleDTOList;
        private List<PromotionExclusionDateDTO> promotionExclusionDateDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PromotionDTO()
        {
            log.LogMethodEntry();
            promotionId = -1;
            activeFlag = true;
            siteId = -1;
            masterEntityId = -1;
            promotionDetailDTOList = new List<PromotionDetailDTO>();
            promotionRuleDTOList = new List<PromotionRuleDTO>();
            promotionExclusionDateDTOList = new List<PromotionExclusionDateDTO>();
            recurFlag = null;
            recurFrequency = null;
            recurEndDate =null;
            internetKey = null;
            promotionType = null;
            recurType = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required the fields
        /// </summary>
        public PromotionDTO(int promotionId,string promotionName,DateTime timeFrom, DateTime timeTo, bool activeFlag,char? recurFlag,
                            char? recurFrequency, DateTime? recurEndDate,int? internetKey, char? promotionType, char? recurType) 
            : this()
        {
            log.LogMethodEntry(promotionId, promotionName, timeFrom,timeTo, activeFlag, recurFlag,recurFrequency, recurEndDate,internetKey,
                           promotionType, recurType);
            this.promotionId = promotionId;
            this.promotionName = promotionName;
            this.timeFrom = timeFrom;
            this.timeTo = timeTo;
            this.internetKey = internetKey;
            this.activeFlag = activeFlag;
            this.recurFlag = recurFlag;
            this.promotionType = promotionType;
            this.recurType = recurType;
            this.recurFrequency = recurFrequency;
            this.recurEndDate = recurEndDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public PromotionDTO(int promotionId,string promotionName,DateTime timeFrom, DateTime timeTo, bool activeFlag,char? recurFlag,
                            char? recurFrequency, DateTime? recurEndDate, DateTime lastUpdatedDate,string lastUpdatedBy,int? internetKey,
                            string guid,int siteId,bool synchStatus,char? promotionType, char? recurType,int masterEntityId,
                            string createdBy, DateTime creationDate)
            : this(promotionId, promotionName, timeFrom, timeTo, activeFlag, recurFlag, recurFrequency, recurEndDate, internetKey,
                          promotionType, recurType)
        {
            log.LogMethodEntry(promotionId, promotionName, timeFrom,timeTo, activeFlag, recurFlag,recurFrequency, recurEndDate,
                              lastUpdatedDate, lastUpdatedBy, internetKey,guid, siteId, synchStatus, promotionType, recurType, 
                              masterEntityId,createdBy, creationDate);            
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PromotionId Id field
        /// </summary>
        public int PromotionId
        {
            get { return promotionId; }
            set { promotionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PromotionName field
        /// </summary>
        public string PromotionName
        {
            get { return promotionName; }
            set { promotionName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TimeFrom field
        /// </summary>
        public DateTime TimeFrom
        {
            get { return timeFrom; }
            set { timeFrom = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TimeTo Id field
        /// </summary>
        public DateTime TimeTo
        {
            get { return timeTo; }
            set { timeTo = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the RecurFlag field
        /// </summary>
        public char? RecurFlag
        {
            get { return recurFlag; }
            set { recurFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the RecurFrequency field
        /// </summary>
        public char? RecurFrequency
        {
            get { return recurFrequency; }
            set { recurFrequency = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the RecurEndDate Id field
        /// </summary>
        public DateTime? RecurEndDate
        {
            get { return recurEndDate; }
            set { recurEndDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PromotionType field
        /// </summary>
        public char? PromotionType
        {
            get { return promotionType; }
            set { promotionType = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the RecurType field
        /// </summary>
        public char? RecurType
        {
            get { return recurType; }
            set { recurType = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the InternetKey  field
        /// </summary>
        public int? InternetKey
        {
            get { return internetKey; }
            set { internetKey = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method of the PromotionDetailDTOList field
        /// </summary>
        public List<PromotionDetailDTO> PromotionDetailDTOList
        {
            get { return promotionDetailDTOList; }
            set { promotionDetailDTOList = value; }
        }
        /// <summary>
        /// Get/Set method of the PromotionRuleDTOList field
        /// </summary>
        public List<PromotionRuleDTO> PromotionRuleDTOList
        {
            get { return promotionRuleDTOList; }
            set { promotionRuleDTOList = value; }
        }
        /// <summary>
        /// Get/Set method of the PromotionExclusionDateDTOLists field
        /// </summary>
        public List<PromotionExclusionDateDTO> PromotionExclusionDateDTOLists
        {
            get { return promotionExclusionDateDTOList; }
            set { promotionExclusionDateDTOList = value; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || promotionId < 0;
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
        /// Returns whether the PromotionDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (promotionDetailDTOList != null &&
                  promotionDetailDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (promotionRuleDTOList != null &&
                  promotionRuleDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (promotionExclusionDateDTOList != null &&
                  promotionExclusionDateDTOList.Any(x => x.IsChanged))
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
    }
}
