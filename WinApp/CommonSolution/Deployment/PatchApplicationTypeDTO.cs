/********************************************************************************************
 * Project Name - Patch Application Type DTO
 * Description  - Data object of patch application type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Feb-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// This is the patch application type data object class. This acts as data holder for the patch application type business object
    /// </summary>
    public class PatchApplicationTypeDTO
    {
       Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByPatchApplicationTypeParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPatchApplicationTypeParameters
        {
            /// <summary>
            /// Search by PATCH_APPLICATION_TYPE_ID field
            /// </summary>
            PATCH_APPLICATION_TYPE_ID = 0,
            /// <summary>
            /// Search by APPLICATION_TYPE field
            /// </summary>
            APPLICATION_TYPE = 1,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG = 2
        }
        int patchApplicationTypeId;
        string applicationType;
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
        public PatchApplicationTypeDTO()
        {
            log.Debug("Starts-PatchApplicationTypeDTO() default constructor.");
            patchApplicationTypeId = -1;
            isActive = "Y";
            log.Debug("Ends-PatchApplicationTypeDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public PatchApplicationTypeDTO(int patchApplicationTypeId, string applicationType, string isActive, string createdBy,
                                       DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                       int siteId, bool synchStatus)
        {
            log.Debug("Starts-PatchApplicationTypeDTO(with all the data fields) Parameterized constructor.");
            this.patchApplicationTypeId = patchApplicationTypeId;
            this.applicationType = applicationType;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.Debug("Ends-PatchApplicationTypeDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get/Set method of the PatchApplicationTypeId field
        /// </summary>
        [DisplayName("Application Type Id")]
        [ReadOnly(true)]
        public int PatchApplicationTypeId { get { return patchApplicationTypeId; } set { patchApplicationTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ApplicationType field
        /// </summary>
        [DisplayName("Application Type")]
        public string ApplicationType { get { return applicationType; } set { applicationType = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || patchApplicationTypeId < 0;
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
