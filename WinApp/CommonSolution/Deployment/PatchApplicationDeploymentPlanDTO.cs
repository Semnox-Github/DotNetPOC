/********************************************************************************************
 * Project Name - Patch Application Deployment Plan DTO
 * Description  - Data object of patch application deployment plan 
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

namespace  Semnox.Parafait.Deployment
{
    /// <summary>
    ///  This is the patch application deployment plan data object class. This acts as data holder for the patch application deployment plan business object
    /// </summary>
    public class PatchApplicationDeploymentPlanDTO
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByPatchApplicationDeploymentPlanParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPatchApplicationDeploymentPlanParameters
        {
            /// <summary>
            /// Search by PATCH_DEPLOYMENT_PLAN_ID field
            /// </summary>
            PATCH_DEPLOYMENT_PLAN_ID = 0,
            /// <summary>
            /// Search by DEPLOYMENT_PLAN_NAME field
            /// </summary>
            DEPLOYMENT_PLAN_NAME = 1,
            /// <summary>
            /// Search by DEPLOYMENT_PLANNED_DATE field
            /// </summary>
            DEPLOYMENT_PLANNED_DATE = 2,
            /// <summary>
            /// Search by DEPLOYMENT_VERSION field
            /// </summary>
            DEPLOYMENT_VERSION = 3,
            /// <summary>
            /// Search by MINIMUM_VERSION_REQUIRED field
            /// </summary>
            MINIMUM_VERSION_REQUIRED = 4,
            /// <summary>
            /// Search by PATCH_APPLICATION_TYPE_ID field
            /// </summary>
            PATCH_APPLICATION_TYPE_ID = 5,
            /// <summary>
            /// Search by DEPLOYMENT_STATUS field
            /// </summary>
            DEPLOYMENT_STATUS = 6,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE = 7
        }
        int patchDeploymentPlanId;
        string deploymentPlanName;
        DateTime deploymentPlannedDate;
        string deploymentVersion;
        string minimumVersionRequired;
        int patchApplicationTypeId;
        string upgradeType;
        string patchFileName;
        string deploymentStatus;
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
        public PatchApplicationDeploymentPlanDTO()
        {
            log.Debug("Starts-PatchApplicationDeploymentPlanDTO() default constructor.");
            patchApplicationTypeId = -1;
            patchDeploymentPlanId = -1;
            upgradeType = "A";
            isActive = "Y";
            log.Debug("Ends-PatchApplicationDeploymentPlanDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public PatchApplicationDeploymentPlanDTO(int patchDeploymentPlanId, string deploymentPlanName, DateTime deploymentPlannedDate, string deploymentVersion,
                                                  string minimumVersionRequired, int patchApplicationTypeId, string upgradeType, string patchFileName, string deploymentStatus,
                                                  string isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                                  string guid, int siteId, bool synchStatus)
        {
            log.Debug("Starts-PatchApplicationDeploymentPlanDTO(with all the data fields) Parameterized constructor.");
            this.patchDeploymentPlanId = patchDeploymentPlanId;
            this.deploymentPlanName = deploymentPlanName;
            this.deploymentPlannedDate = deploymentPlannedDate;
            this.deploymentVersion = deploymentVersion;
            this.minimumVersionRequired = minimumVersionRequired;
            this.patchApplicationTypeId = patchApplicationTypeId;
            this.upgradeType = upgradeType;
            this.patchFileName = patchFileName;
            this.deploymentStatus = deploymentStatus;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.Debug("Ends-PatchApplicationDeploymentPlanDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get/Set method of the PatchDeploymentPlanId field
        /// </summary>
        [DisplayName("Plan Id")]
        [ReadOnly(true)]
        public int PatchDeploymentPlanId { get { return patchDeploymentPlanId; } set { patchDeploymentPlanId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DeploymentPlanName field
        /// </summary>
        [DisplayName("Plan Name")]
        public string DeploymentPlanName { get { return deploymentPlanName; } set { deploymentPlanName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DeploymentPlannedDate field
        /// </summary>
        [DisplayName("Planned Date")]
        public DateTime DeploymentPlannedDate { get { return deploymentPlannedDate; } set { deploymentPlannedDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DeploymentVersion field
        /// </summary>
        [DisplayName("Upgrade Version")]
        public string DeploymentVersion { get { return deploymentVersion; } set { deploymentVersion = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MinimumVersionRequired field
        /// </summary>
        [DisplayName("Minimum Version")]
        public string MinimumVersionRequired { get { return minimumVersionRequired; } set { minimumVersionRequired = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PatchApplicationTypeId field
        /// </summary>
        [DisplayName("Application Type")]
        public int PatchApplicationTypeId { get { return patchApplicationTypeId; } set { patchApplicationTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UpgradeType field
        /// </summary>
        [DisplayName("Upgrade Type")]
        public string UpgradeType { get { return upgradeType; } set { upgradeType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PatchFileName field
        /// </summary>
        [DisplayName("Patch File Name")]
        public string PatchFileName { get { return patchFileName; } set { patchFileName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DeploymentStatus field
        /// </summary>
        [DisplayName("Deployment Status")]
        public string DeploymentStatus { get { return deploymentStatus; } set { deploymentStatus = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || patchDeploymentPlanId < 0;
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
