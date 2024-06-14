/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of RecipePlanHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0        21-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RecipePlanHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Recipe Plan Header Id field
            /// </summary>
            RECIPE_PLAN_HEADER__ID,

            /// <summary>
            /// Search by  Recipe Estimation Header Id field
            /// </summary>
            RECIPE_ESTIMATION_HEADER_ID,

            /// <summary>
            /// Search by  Recipe plan FROM Date Time field
            /// </summary>
            PLAN_FROM_DATE,

            /// <summary>
            /// Search by  Recipe plan To Date Time field
            /// </summary>

            PLAN_TO_DATE,

            /// <summary>
            /// Search by  Recipe plan FROM Date Time field
            /// </summary>
            FROM_DATE,

            /// <summary>
            /// Search by  Recipe plan To Date Time field
            /// </summary>
            TO_DATE,

            /// <summary>
            /// Search by  recur End Date Id field
            /// </summary>
            RECUR_END_DATE,

            /// </summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by Recipe Plan Header Id List field
            /// </summary>
            RECIPE_PLAN_HEADER_ID_LIST
        }

        public enum RecurFrequencyEnum
        {
            /// <summary>
            /// Recur Type Daily field
            /// </summary>
            DAILY,

            /// <summary>
            /// Recur Type Weekly field
            /// </summary>
            WEEKLY,

            /// <summary>
            /// Recur Type Monthly field
            /// </summary>
            MONTHLY
        }

        public enum RecurTypeEnum
        {
            /// <summary>
            /// Recur Type Date field
            /// </summary>
            DATE,

            /// <summary>
            /// Recur Type Week Day field
            /// </summary>
            WEEKDAY
        }


        private int recipePlanHeaderId;
        private DateTime planDateTime;
        private bool? recurFlag;
        private DateTime? recurEndDate;
        private char? recurFrequency;
        private char? recurType;
        private int recipeEstimationHeaderId;
        private bool? sunday;
        private bool? monday;
        private bool? tuesday;
        private bool? wednesday;
        private bool? thursday;
        private bool? friday;
        private bool? saturday;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string recipeCount;
        private List<RecipePlanDetailsDTO> recipePlanDetailsDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecipePlanHeaderDTO()
        {
            log.LogMethodEntry();
            recipePlanHeaderId = -1;
            recipeEstimationHeaderId = -1;
            siteId = -1;
            masterEntityId = -1;
            recurFlag = null;
            recurEndDate = null;
            recurType = null;
            sunday = null;
            monday = null;
            tuesday = null;
            wednesday = null;
            thursday = null;
            friday = null;
            saturday = null;
            isActive = true;
            recipePlanDetailsDTO = new List<RecipePlanDetailsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public RecipePlanHeaderDTO(int recipePlanHeaderId, DateTime planDateTime, bool? recurFlag, DateTime? recurEndDate, char? recurFrequency,
                                   char? recurType, int recipeEstimationHeaderId, bool? sunday, bool? monday, bool? tuesday,
                                   bool? wednesday, bool? thursday, bool? friday, bool? saturday, bool isActive)
            : this()
        {
            log.LogMethodEntry(recipePlanHeaderId, planDateTime, recurFlag, recurEndDate, recurFrequency, recurType, recipeEstimationHeaderId,
                                sunday, monday, tuesday, wednesday, thursday, friday, saturday, isActive);
            this.recipePlanHeaderId = recipePlanHeaderId;
            this.planDateTime = planDateTime;
            this.recurFlag = recurFlag;
            this.recurFrequency = recurFrequency;
            this.recurEndDate = recurEndDate;
            this.recurType = recurType;
            this.recipeEstimationHeaderId = recipeEstimationHeaderId;
            this.sunday = sunday;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with All the data fields.
        /// </summary>
        public RecipePlanHeaderDTO(int recipePlanHeaderId, DateTime planDateTime, bool? recurFlag, DateTime? recurEndDate, char ? recurFrequency,
                                   char? recurType, int recipeEstimationHeaderId, bool? sunday, bool? monday, bool? tuesday,
                                   bool? wednesday, bool? thursday, bool? friday, bool? saturday, bool isActive, string createdBy,
                                   DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId,
                                   bool synchStatus, int masterEntityId)
            : this(recipePlanHeaderId, planDateTime, recurFlag, recurEndDate, recurFrequency, recurType, recipeEstimationHeaderId, sunday,
                    monday, tuesday, wednesday, thursday, friday, saturday, isActive)
        {
            log.LogMethodEntry(recipePlanHeaderId, planDateTime, recurFlag, recurEndDate, recurFrequency, recurType, recipeEstimationHeaderId,
                                sunday, monday, tuesday, wednesday, thursday, friday, saturday, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        public  static char? RecurFrequencyEnumToString(RecurFrequencyEnum recurFrequency)
        {
            char? returnString;
            switch (recurFrequency)
            {
                case RecurFrequencyEnum.DAILY:
                    returnString = 'D';
                    break;
                case RecurFrequencyEnum.WEEKLY:
                    returnString = 'W';
                    break;
                case RecurFrequencyEnum.MONTHLY:
                    returnString = 'M';
                    break;
                default:
                    returnString = null;
                    break;
            }
            return returnString;
        }

        public static char? RecurTypeEnumToString(RecurTypeEnum recurType)
        {
            char? returnString;
            switch (recurType)
            {
                case RecurTypeEnum.DATE:
                    returnString = 'D';
                    break;
                case RecurTypeEnum.WEEKDAY:
                    returnString = 'W';
                    break;
                default:
                    returnString = null;
                    break;
            }
            return returnString;
        }

        /// <summary>
        /// Get/Set method of the RecipePlanHeaderId field
        /// </summary>
        public int RecipePlanHeaderId { get { return recipePlanHeaderId; } set { this.IsChanged = true; recipePlanHeaderId = value; } }

        /// <summary>
        /// Get/Set method of the PlanDateTime field
        /// </summary>
        public DateTime PlanDateTime { get { return planDateTime; } set { this.IsChanged = true; planDateTime = value; } }

        /// <summary>
        /// Get/Set method of the RecurFlag field
        /// </summary>
        public bool? RecurFlag { get { return recurFlag; } set { this.IsChanged = true; recurFlag = value; } }

        /// <summary>
        /// Get/Set method of the RecurEndDate field
        /// </summary>
        public DateTime? RecurEndDate { get { return recurEndDate; } set { this.IsChanged = true; recurEndDate = value; } }

        /// <summary>
        /// Get/Set method of the RecurType field
        /// </summary>
        public char? RecurType { get { return recurType; } set { this.IsChanged = true; recurType = value; } }

        /// <summary>
        /// Get/Set method of the RecurType field
        /// </summary>
        public char? RecurFrequency { get { return recurFrequency; } set { this.IsChanged = true; recurFrequency = value; } }


        /// <summary>
        /// Get/Set method of the RecipeEstimationHeaderId field
        /// </summary>
        public int RecipeEstimationHeaderId { get { return recipeEstimationHeaderId; } set { this.IsChanged = true; recipeEstimationHeaderId = value; } }

        /// <summary>
        /// Get/Set method of the Sunday field
        /// </summary>
        public bool? Sunday { get { return sunday; } set { this.IsChanged = true; sunday = value; } }

        /// <summary>
        /// Get/Set method of the Monday field
        /// </summary>
        public bool? Monday { get { return monday; } set { this.IsChanged = true; monday = value; } }

        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        public bool? Tuesday { get { return tuesday; } set { this.IsChanged = true; tuesday = value; } }

        /// <summary>
        /// Get/Set method of the Wednesday field
        /// </summary>
        public bool? Wednesday { get { return wednesday; } set { this.IsChanged = true; wednesday = value; } }

        /// <summary>
        /// Get/Set method of the Thursday field
        /// </summary>
        public bool? Thursday { get { return thursday; } set { this.IsChanged = true; thursday = value; } }

        /// <summary>
        /// Get/Set method of the Friday field
        /// </summary>
        public bool? Friday { get { return friday; } set { this.IsChanged = true; friday = value; } }

        /// <summary>
        /// Get/Set method of the Saturday field
        /// </summary>
        public bool? Saturday { get { return saturday; } set { this.IsChanged = true; saturday = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the RecipeCount field
        /// </summary>
        public string RecipeCount { get { return recipeCount; } set { this.IsChanged = true; recipeCount = value; } }

        /// <summary>
        /// Get/Set method of the RecipePlanDetailsDTOList field
        /// </summary>
        public List<RecipePlanDetailsDTO> RecipePlanDetailsDTOList
        {
            get { return recipePlanDetailsDTO; }
            set { recipePlanDetailsDTO = value; }
        }

        /// <summary>
        /// Returns true or false whether the RecipeEstimationHeaderDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (recipePlanDetailsDTO != null &&
                   recipePlanDetailsDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
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
                    return notifyingObjectIsChanged || recipePlanHeaderId < 0;
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
