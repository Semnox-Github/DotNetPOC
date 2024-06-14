/********************************************************************************************
 * Project Name - Segment Definition DTO
 * Description  - Data object of segment definition DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Apr-2016   Raghuveera          Created 
 *2.70        29-Mar-2019   Akshay Gulaganji    modified isActive DataType from string to bool 
 *2.110.0     15-Oct-2020   Mushahid Faizan     Web Inventory UI changes..
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the segment definition data object class. This acts as data holder for the segment definition business object
    /// </summary>
    public class SegmentDefinitionDTO
    {
        private Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchBySegmentDefinitionParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySegmentDefinitionParameters
        {
            /// <summary>
            /// Search by SEGMENT_DEFINITION_ID field
            /// </summary>
            SEGMENT_DEFINITION_ID,
            /// <summary>
            /// Search by SEGMENT_NAME field
            /// </summary>
            SEGMENT_NAME,
            /// <summary>
            /// Search by APPLICABLE_ENTITY field
            /// </summary>
            APPLICABLE_ENTITY,
            /// <summary>
            /// Search by SEQUENCE_ORDER field
            /// </summary>
            SEQUENCE_ORDER,
            /// <summary>
            /// Search by IS_MANDATORY field
            /// </summary>
            IS_MANDATORY,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE
        }
        private int segmentDefinitionId;
        private string segmentName;
        private string applicableEntity;
        private string sequenceOrder;
        private string isMandatory;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SegmentDefinitionDTO()
        {
            log.LogMethodEntry();
            segmentDefinitionId = -1;
            isMandatory = "N";
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            segmentDefinitionSourceMapDTOList = new List<SegmentDefinitionSourceMapDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SegmentDefinitionDTO(int segmentDefinitionId, string segmentName, string applicableEntity,
                                    string sequenceOrder, string isMandatory, bool isActive, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                    int siteId, bool synchStatus)
        {
            log.LogMethodEntry(segmentDefinitionId, segmentName, applicableEntity,
                                    sequenceOrder, isMandatory, isActive, createdBy,
                                    creationDate, lastUpdatedBy, lastUpdatedDate, guid,
                                    siteId, synchStatus);
            this.segmentDefinitionId = segmentDefinitionId;
            this.segmentName = segmentName;
            this.applicableEntity = applicableEntity;
            this.sequenceOrder = sequenceOrder;
            this.isMandatory = isMandatory;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the SegmentDefinitionId field
        /// </summary>
        [DisplayName("Definition Id")]
        [ReadOnly(true)]
        public int SegmentDefinitionId { get { return segmentDefinitionId; } set { segmentDefinitionId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentName field
        /// </summary>
        [DisplayName("Name")]
        public string SegmentName { get { return segmentName; } set { segmentName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ApplicableEntity field
        /// </summary>
        [DisplayName("Applicable Entity")]
        public string ApplicableEntity { get { return applicableEntity; } set { applicableEntity = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SequenceOrder field
        /// </summary>
        [DisplayName("Sequence Order")]
        public string SequenceOrder { get { return sequenceOrder; } set { sequenceOrder = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsMandatory field
        /// </summary>
        [DisplayName("Mandatory?")]
        public string IsMandatory { get { return isMandatory; } set { isMandatory = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
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
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the SegmentDefinitionSourceMapDTOList field
        /// </summary>
        [Browsable(false)]
        public List<SegmentDefinitionSourceMapDTO> SegmentDefinitionSourceMapDTOList
        {
            get
            {
                return segmentDefinitionSourceMapDTOList;
            }

            set
            {
                segmentDefinitionSourceMapDTOList = value;
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
                    return notifyingObjectIsChanged || segmentDefinitionId < 0;
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
        /// IsChangedRecursive
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (segmentDefinitionSourceMapDTOList != null &&
                   segmentDefinitionSourceMapDTOList.Any(x => x.IsChanged))
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
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
