/********************************************************************************************
 * Project Name - Segment Categorization DTO
 * Description  - Data object of segment categorization DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Apr-2016   Raghuveera          Created 
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
    /// 
    /// </summary>
    public class SegmentCategorizationDTO
    {
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchBySegmentCategorizationParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySegmentCategorizationParameters
        {
            /// <summary>
            /// Search by SEGMENT_CATEGORY_ID field
            /// </summary>
            SEGMENT_CATEGORY_ID = 0,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 1
        }
        int segmentCategoryId;
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
        public SegmentCategorizationDTO()
        {
            log.Debug("Starts-SegmentCategorizationDTO() default constructor.");
            segmentCategoryId = -1;
            siteId = -1;
            log.Debug("Ends-SegmentCategorizationDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SegmentCategorizationDTO(int segmentCategoryId, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                        DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
        {
            log.Debug("Starts-SegmentCategorizationDTO(with all the data fields) Parameterized constructor.");
            this.segmentCategoryId = segmentCategoryId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.Debug("Ends-SegmentCategorizationDTO(with all the data fields) Parameterized constructor.");
        }

        /// <summary>
        /// Get method of the SegmentCategoryId field
        /// </summary>
        [DisplayName("CategoryId")]
        [ReadOnly(true)]
        public int SegmentCategoryId { get { return segmentCategoryId; } set { segmentCategoryId = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || segmentCategoryId < 0;
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
