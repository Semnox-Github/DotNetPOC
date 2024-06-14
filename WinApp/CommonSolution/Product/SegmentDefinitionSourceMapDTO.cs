/********************************************************************************************
 * Project Name - Segment Definition Source Map DTO
 * Description  - Data object of segment definition source map DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Apr-2016   Raghuveera          Created 
 *2.70        25-Mar-2019   Nagesh Badiger      IsActive Datatype is changed string to bool
 *2.70        05-Apr-2019   Akshay Gulaganji    added segmentDefinitionSourceValueDTOList as child
 *2.110.0     15-Oct-2020   Mushahid Faizan     added IsChangedRecursive() and Modified IsChanged().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the segment definition source map object class. This acts as data holder for the segment definition source map business object
    /// </summary>
    public class SegmentDefinitionSourceMapDTO
    {
        private Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
       
        /// <summary>
        /// SearchBySegmentDefinitionSourceMapParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySegmentDefinitionSourceMapParameters
        {
            /// <summary>
            /// Search by SEGMENT_DEFINITION_SOURCE_ID field
            /// </summary>
            SEGMENT_DEFINITION_SOURCE_ID,
            /// <summary>
            /// Search by SEGMENT_DEFINITION_ID field
            /// </summary>
            SEGMENT_DEFINITION_ID,
            /// <summary>
            /// Search by DATA_SOURCE_TYPE field
            /// </summary>
            DATA_SOURCE_TYPE,
            /// <summary>
            /// Search by DATA_SOURCE_ENTITY field
            /// </summary>
            DATA_SOURCE_ENTITY,
            /// <summary>
            /// Search by DATA_SOURCE_COLUMN field
            /// </summary>
            DATA_SOURCE_COLUMN,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by SEGMENT_DEFINITION_APPLICABILITY field
            /// </summary>
            SEGMENT_DEFINITION_APPLICABILITY,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE
        }
        private int segmentDefinitionSourceId;
        private int segmentDefinitionId;
        private string dataSourceType;
        private string dataSourceEntity;
        private string dataSourceColumn;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SegmentDefinitionSourceMapDTO()
        {
            log.LogMethodEntry();
            segmentDefinitionId = -1;
            segmentDefinitionSourceId = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            segmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SegmentDefinitionSourceMapDTO(int segmentDefinitionSourceId, int segmentDefinitionId, string dataSourceType,
                                    string dataSourceEntity, string dataSourceColumn, bool isActive, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                    int siteId, bool synchStatus)
        {
            log.LogMethodEntry(segmentDefinitionSourceId, segmentDefinitionId, dataSourceType, dataSourceEntity,
                               dataSourceColumn, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                               guid, siteId, synchStatus);
            this.segmentDefinitionSourceId = segmentDefinitionSourceId;
            this.segmentDefinitionId = segmentDefinitionId;
            this.dataSourceType = dataSourceType;
            this.dataSourceEntity = dataSourceEntity;
            this.dataSourceColumn = dataSourceColumn;
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
        /// Get/Set method of the SegmentDefinitionSourceId field
        /// </summary>
        [DisplayName("Definition Source Id")]
        [ReadOnly(true)]
        public int SegmentDefinitionSourceId { get { return segmentDefinitionSourceId; } set { segmentDefinitionSourceId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentDefinitionId field
        /// </summary>
        [DisplayName("Definition")]
        public int SegmentDefinitionId { get { return segmentDefinitionId; } set { segmentDefinitionId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataSourceType field
        /// </summary>
        [DisplayName("Data Source Type")]
        public string DataSourceType { get { return dataSourceType; } set { dataSourceType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataSourceEntity field
        /// </summary>
        [DisplayName("Data Source Entity")]
        public string DataSourceEntity { get { return dataSourceEntity; } set { dataSourceEntity = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataSourceColumn field
        /// </summary>
        [DisplayName("Data Source Column")]
        public string DataSourceColumn { get { return dataSourceColumn; } set { dataSourceColumn = value; this.IsChanged = true; } }
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
        /// Get/Set method of the MasterEntityId field
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
        /// Get/Set method of the SchedulesDTOListList field
        /// </summary>
        [Browsable(false)]
        public List<SegmentDefinitionSourceValueDTO> SegmentDefinitionSourceValueDTOList
        {
            get
            {
                return segmentDefinitionSourceValueDTOList;
            }

            set
            {
                segmentDefinitionSourceValueDTOList = value;
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
                    return notifyingObjectIsChanged || segmentDefinitionSourceId < 0 ;
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
        /// Returns whether the segmentDefinitionSourceMapDTO changed or any of its segmentDefinitionSourceValueDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (segmentDefinitionSourceValueDTOList != null &&
                   segmentDefinitionSourceValueDTOList.Any(x => x.IsChanged))
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
