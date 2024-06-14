/********************************************************************************************
 * Project Name -  SegmentCategorizationValueWrapperDTO
 * Description  - SegmentCategorizationValueWrapperDTO used to Map SegmentCategorizationValueUI 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        06-Mar-2019   Muhammed Mehraj         Created 
 * 2.70       29-Aug-2019   Rakesh Kumar            Added fields,constructor,searchparameter    
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class SegmentCategorizationValueWrapperDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchBySegmentCategorizationValueWrapper
        {
            /// <summary>
            /// Search by SEGMENT_CATEGORY_ID field
            /// </summary>
            SEGMENT_CATEGORY_ID,
            /// <summary>
            /// Search by SEGMENT_DEFINATION_ID field
            /// </summary>
            SEGMENT_DEFINATION_ID,
            /// <summary>
            /// Search by SEGMENT_CATEGORY_VALUE_ID field
            /// </summary>
            SEGMENT_CATEGORY_VALUE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by SEGMENT_NAME field
            /// </summary>
            SEGMENT_NAME,
            /// <summary>
            /// Search by APPLICABILITY field
            /// </summary>
            APPLICABILITY

        }

        private int segmentCategoryId;
        private string segmentName;
        private string type;
        private string applicability;
        private int segmentDefinationId;
        private int segmentCategoryValueId;
        private int valueId;
        private string dynamicValueID;
        private string segmentValueText;
        private DateTime? segmentValueDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string createdBy;
        private DateTime createdDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool isMandatory;
        private List<SegmentCategorizationValueListWrapperDTO> segmentValueList;

        public SegmentCategorizationValueWrapperDTO()
        {
            log.LogMethodEntry();
            segmentCategoryId = -1;
            segmentCategoryValueId = -1;
            segmentDefinationId = -1;
            valueId = -1;
            siteId = -1;
            masterEntityId = -1;
            isMandatory = false;
            log.LogMethodExit();
        }
        public SegmentCategorizationValueWrapperDTO(int segmentCategoryId, string segmentName, string type, string applicability, int segmentDefinationId,
                               int segmentCategoryValueId, int valueId, string dynamicValueID, string segmentValueText, DateTime? segmentValueDate, int siteId,
                               int masterEntityId, bool synchStatus, string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate)
        {
            log.LogMethodEntry(segmentCategoryId, segmentName, type, applicability, segmentDefinationId, segmentCategoryValueId, valueId, dynamicValueID,
                               segmentValueText, segmentValueDate, siteId, masterEntityId, synchStatus, createdBy, createdDate, lastUpdatedBy, lastUpdatedDate);
            this.segmentCategoryId = segmentCategoryId;
            this.segmentName = segmentName;
            this.type = type;
            this.applicability = applicability;
            this.segmentDefinationId = segmentDefinationId;
            this.segmentCategoryValueId = segmentCategoryValueId;
            this.valueId = valueId;
            this.dynamicValueID = dynamicValueID;
            this.segmentValueText = SegmentValueText;
            this.segmentValueDate = SegmentValueDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.createdDate = createdDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();

        }
        /// <summary>
        /// Get/Set method of the SegmentCategoryId field
        /// </summary>
        public int SegmentCategoryId { get { return segmentCategoryId; } set { segmentCategoryId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentName field
        /// </summary>
        public string SegmentName { get { return segmentName; } set { segmentName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public string Type { get { return type; } set { type = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Applicability field
        /// </summary>
        public string Applicability { get { return applicability; } set { applicability = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentDefinationId field
        /// </summary>
        public int SegmentDefinationId { get { return segmentDefinationId; } set { segmentDefinationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentCategoryValueId field
        /// </summary>
        public int SegmentCategoryValueId { get { return segmentCategoryValueId; } set { segmentCategoryValueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentValueList field
        /// </summary>
        public List<SegmentCategorizationValueListWrapperDTO> SegmentValueList { get { return segmentValueList; } set { segmentValueList = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ValueId field
        /// </summary>
        public int ValueId { get { return valueId; } set { valueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DynamicValueId field
        /// </summary>
        public string DynamicValueID { get { return dynamicValueID; } set { dynamicValueID = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ValueId field
        /// </summary>
        public string SegmentValueText { get { return segmentValueText; } set { segmentValueText = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentValueDate field
        /// </summary>
        public DateTime? SegmentValueDate { get { return segmentValueDate; } set { segmentValueDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedDate field
        /// </summary>
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }

        public bool IsMandatory { get { return isMandatory; } set { isMandatory = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged;
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

    public class SegmentCategorizationValueListWrapperDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public int ValueId { get; set; }
        public string Value { get; set; }



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
                    return notifyingObjectIsChanged;
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
            log.LogMethodExit(null);
        }

    }

}
