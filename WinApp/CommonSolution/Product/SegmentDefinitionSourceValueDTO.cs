/********************************************************************************************
 * Project Name - Segment Definition Source Value DTO
 * Description  - Data object of segment definition source value DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        07-Apr-2016   Raghuveera          Created 
 *2.70        25-Mar-2019   Nagesh Badiger      modified IsActive DataType (from string to bool)
 *2.110.0     16-oct-2019   Mushahid Faizan     Modified as per standards.
 ********************************************************************************************/


using System;
using System.ComponentModel;


namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the segment definition source value data object class. This acts as data holder for the segment definition source value business object
    /// </summary>
    public class SegmentDefinitionSourceValueDTO
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchBySegmentDefinitionSourceValueParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySegmentDefinitionSourceValueParameters
        {
            /// <summary>
            /// Search by SEGMENT_DEFINITION_SOURCE_ID field
            /// </summary>
            SEGMENT_DEFINITION_SOURCE_ID,
            /// <summary>
            /// Search by SEGMENT_DEFINITION_SOURCE_VALUE_ID field
            /// </summary>
            SEGMENT_DEFINITION_SOURCE_VALUE_ID,
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
        private int segmentDefinitionSourceValueId;
        private int segmentDefinitionSourceId;
        private string listValue;
        private string dBQuery;
        private string description;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SegmentDefinitionSourceValueDTO()
        {
            log.LogMethodEntry();
            segmentDefinitionSourceValueId = -1;
            segmentDefinitionSourceId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SegmentDefinitionSourceValueDTO(int segmentDefinitionSourceValueId, int segmentDefinitionSourceId, string listValue,
                                    string dBQuery, string description, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                    DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
        {
            log.Debug("Starts-SegmentDefinitionSourceValueDTO(with all the data fields) Parameterized constructor.");
            this.segmentDefinitionSourceValueId = segmentDefinitionSourceValueId;
            this.segmentDefinitionSourceId = segmentDefinitionSourceId;
            this.listValue = listValue;
            this.dBQuery = dBQuery;
            this.description = description;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.Debug("Ends-SegmentDefinitionSourceValueDTO(with all the data fields) Parameterized constructor.");
        }

        /// <summary>
        /// Get method of the SegmentDefinitionSourceValueId field
        /// </summary>
        [DisplayName("Value Id")]
        [ReadOnly(true)]
        public int SegmentDefinitionSourceValueId { get { return segmentDefinitionSourceValueId; } set { segmentDefinitionSourceValueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentDefinitionSourceId field
        /// </summary>
        [DisplayName("SegmentDefinitionSourceId")]
        public int SegmentDefinitionSourceId { get { return segmentDefinitionSourceId; } set { segmentDefinitionSourceId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ListValue field
        /// </summary>
        [DisplayName("List Value")]
        public string ListValue { get { return listValue; } set { listValue = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DBQuery field
        /// </summary>
        [DisplayName("DBQuery")]
        public string DBQuery { get { return dBQuery; } set { dBQuery = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || segmentDefinitionSourceValueId < 0;
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
