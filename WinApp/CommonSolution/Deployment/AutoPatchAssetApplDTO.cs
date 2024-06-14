/********************************************************************************************
 * Project Name - Patch Asset Application DTO
 * Description  - Data object of patch asset application
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
    /// This is the patch asset application data object class. This acts as data holder for the patch asset application business object
    /// </summary>
    public class AutoPatchAssetApplDTO
    {
       Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Different options for asset application status
        /// </summary>
        public enum AssetApplicationStatusOption
        {
            /// <summary>
            /// This indicates the application upgrade is in progress status.
            /// </summary>
            IN_PROGRESS,
            /// <summary>
            /// This indicates the application upgrade is in pending status.
            /// </summary>
            PENDING,
            /// <summary>
            /// This indicates the application upgrade is in error status
            /// </summary>
            ERROR,
            /// <summary>
            /// This indicates the application upgrade is completed
            /// </summary>
            COMPLETE
        }
        /// <summary>
        /// SearchByPatchAssetApplicationParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByAutoPatchAssetApplParameters
        {
            /// <summary>
            /// Search by PATCH_ASSET_APPLICATION_ID field
            /// </summary>
            PATCH_ASSET_APPLICATION_ID = 0,
            /// <summary>
            /// Search by ASSET_ID field
            /// </summary>
            ASSET_ID = 1,
            /// <summary>
            /// Search by PATCH_APPLICATION_TYPE_ID field
            /// </summary>
            PATCH_APPLICATION_TYPE_ID = 2,
            /// <summary>
            /// Search by PATCH_VERSION_NUMBER field
            /// </summary>
            PATCH_VERSION_NUMBER = 3,
            /// <summary>
            /// Search by LAST_UPGRADE_DATE field
            /// </summary>
            LAST_UPGRADE_DATE = 4,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG = 5,
            /// <summary>
            /// Search by PATCH_UPGRADE_STATUS field
            /// </summary>
            PATCH_UPGRADE_STATUS=6,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 7,
            /// <summary>
            /// Search by MINIMUM_REQUIRED_VERSION field
            /// </summary>
            MINIMUM_REQUIRED_VERSION = 8
        }
        int patchAssetApplicationId;
        int assetId;
        int patchApplicationTypeId;
        string patchVersionNumber;
        string patchUpgradeStatus;
        string applicationPath;
        DateTime lastUpgradeDate;
        int errorCounter;
        bool passwordChangeStatus;
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
        public AutoPatchAssetApplDTO()
        {
            log.Debug("Starts-PatchAssetApplicationDTO() default constructor.");
            PatchAssetApplicationId = -1;
            patchApplicationTypeId = -1;
            assetId = -1;
            siteId = -1;
            isActive = "Y";
            patchUpgradeStatus = AssetApplicationStatusOption.COMPLETE.ToString();
            log.Debug("Ends-PatchAssetApplicationDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AutoPatchAssetApplDTO(int patchAssetApplicationId, int assetId, int patchApplicationTypeId, string patchVersionNumber,
                                        string patchUpgradeStatus, string applicationPath, DateTime lastUpgradeDate, int errorCounter, bool passwordChangeStatus, string isActive,
                                        string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                        int siteId, bool synchStatus)
        {
            log.Debug("Starts-PatchAssetApplicationDTO(with all the data fields) Parameterized constructor.");
            this.patchAssetApplicationId = patchAssetApplicationId;
            this.assetId = assetId;
            this.patchApplicationTypeId = patchApplicationTypeId;
            this.patchVersionNumber = patchVersionNumber;
            this.patchUpgradeStatus = patchUpgradeStatus;
            this.applicationPath = applicationPath;
            this.lastUpgradeDate = lastUpgradeDate;
            this.errorCounter = errorCounter;
            this.passwordChangeStatus = passwordChangeStatus;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.Debug("Ends-PatchAssetApplicationDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get/Set method of the PatchAssetApplicationId field
        /// </summary>
        [DisplayName("Patch Asset Application Id")]
        [ReadOnly(true)]
        public int PatchAssetApplicationId { get { return patchAssetApplicationId; } set { patchAssetApplicationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetId field
        /// </summary>
        [DisplayName("Asset")]
        public int AssetId { get { return assetId; } set { assetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PatchApplicationTypeId field
        /// </summary>
        [DisplayName("Application Type")]
        public int PatchApplicationTypeId { get { return patchApplicationTypeId; } set { patchApplicationTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PatchVersionNumber field
        /// </summary>
        [DisplayName("Version")]
        public string PatchVersionNumber { get { return patchVersionNumber; } set { patchVersionNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PatchUpgradeStatus field
        /// </summary>
        [DisplayName("Upgrade Status")]
        [ReadOnly(true)]
        public string PatchUpgradeStatus { get { return patchUpgradeStatus; } set { patchUpgradeStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ApplicationPath field
        /// </summary>
        [DisplayName("Application Path")]
        public string ApplicationPath { get { return applicationPath; } set { applicationPath = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpgradeDate field
        /// </summary>
        [DisplayName("Last Upgraded On")]
        [ReadOnly(true)]
        public DateTime LastUpgradeDate { get { return lastUpgradeDate; } set { lastUpgradeDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ErrorCounter field
        /// </summary>
        [DisplayName("Error Counter")]
        //[ReadOnly(true)]
        public int ErrorCounter { get { return errorCounter; } set { errorCounter = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordChangeStatus field
        /// </summary>
        [DisplayName("Password Changed?")]
        [Browsable(false)]
        public bool PasswordChangeStatus { get { return passwordChangeStatus; } set { passwordChangeStatus = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || patchAssetApplicationId < 0;
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
