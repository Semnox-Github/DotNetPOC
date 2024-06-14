/********************************************************************************************
 * Project Name - Segment Categorization Value DTO
 * Description  - Data object of segment categorization value DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Apr-2016   Raghuveera          Created 
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
    /// This is the segment categorization value data object class. This acts as data holder for the segment categorization value business object
    /// </summary>
    public class SegmentCategorizationValueDTO
    {
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchBySegmentCategorizationValueParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySegmentCategorizationValueParameters
        {
            /// <summary>
            /// Search by SEGMENT_CATEGORY_VALUE_ID field
            /// </summary>
            SEGMENT_CATEGORY_VALUE_ID = 0,
            /// <summary>
            /// Search by SEGMENT_CATEGORY_ID field
            /// </summary>
            SEGMENT_CATEGORY_ID = 1,
            /// <summary>
            /// Search by SEGMENT_DEFINITION_ID field
            /// </summary>
            SEGMENT_DEFINITION_ID = 2,
            /// <summary>
            /// Search by SEGMENT_VALUE_TEXT field
            /// </summary>
            SEGMENT_VALUE_TEXT = 3,
            /// <summary>
            /// Search by SEGMENT_VALUE_DATE field
            /// </summary>
            SEGMENT_VALUE_DATE = 4,
            /// <summary>
            /// Search by SEGMENT_STATIC_VALUE_ID field
            /// </summary>
            SEGMENT_STATIC_VALUE_ID = 5,
            /// <summary>
            /// Search by SEGMENT_DYNAMIC_VALUE_ID field
            /// </summary>
            SEGMENT_DYNAMIC_VALUE_ID = 6,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE = 7,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 8
        }
        int segmentCategoryValueId;
        int segmentCategoryId;
        int segmentDefinitionId;
        string segmentValueText;
        DateTime segmentValueDate;
        int segmentStaticValueId;
        string segmentDynamicValueId;
        string isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        string guid;
        int siteId;
        bool synchStatus;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SegmentCategorizationValueDTO()
        {
            log.Debug("Starts-SegmentCategorizationValueDTO() default constructor.");
            segmentDefinitionId = -1;
            segmentCategoryValueId = -1;
            segmentCategoryId = -1;
            segmentStaticValueId = -1;
            siteId = -1;
            isActive = "Y";
            log.Debug("Ends-SegmentCategorizationValueDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SegmentCategorizationValueDTO(int segmentCategoryValueId, int segmentCategoryId, int segmentDefinitionId,
                                             string segmentValueText, DateTime segmentValueDate, int segmentStaticValueId,
                                             string segmentDynamicValueId, string isActive, string createdBy,
                                             DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                             int siteId, bool synchStatus)
        {
            log.Debug("Starts-SegmentCategorizationValueDTO(with all the data fields) Parameterized constructor.");
            this.segmentCategoryValueId = segmentCategoryValueId;
            this.segmentCategoryId = segmentCategoryId;
            this.segmentDefinitionId = segmentDefinitionId;
            this.segmentValueText = segmentValueText;
            this.segmentValueDate = segmentValueDate;
            this.segmentStaticValueId = segmentStaticValueId;
            this.segmentDynamicValueId = segmentDynamicValueId;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.Debug("Ends-SegmentCategorizationValueDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get method of the segmentCategoryValueId field
        /// </summary>
        [DisplayName("Value Id")]
        [ReadOnly(true)]
        public int SegmentCategoryValueId { get { return segmentCategoryValueId; } set { segmentCategoryValueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the segmentCategoryId field
        /// </summary>
        [DisplayName("Segment Category Id")]
        public int SegmentCategoryId { get { return segmentCategoryId; } set { segmentCategoryId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the segmentDefinitionId field
        /// </summary>
        [DisplayName("Definition")]
        public int SegmentDefinitionId { get { return segmentDefinitionId; } set { segmentDefinitionId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentValueText field
        /// </summary>
        [DisplayName("Value Text")]
        public string SegmentValueText { get { return segmentValueText; } set { segmentValueText = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the segmentValueDate field
        /// </summary>
        [DisplayName("Value Date")]
        public DateTime SegmentValueDate { get { return segmentValueDate; } set { segmentValueDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentStaticValueId field
        /// </summary>
        [DisplayName("Static Value Id")]
        public int SegmentStaticValueId { get { return segmentStaticValueId; } set { segmentStaticValueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentDynamicValueId field
        /// </summary>
        [DisplayName("Dynamic Value Id")]
        public string SegmentDynamicValueId { get { return segmentDynamicValueId; } set { segmentDynamicValueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public string IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

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
                    return notifyingObjectIsChanged || segmentCategoryValueId < 0;
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}
