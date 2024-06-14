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
    public class AutoPatchDepPlanDTO
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Different options for deployment status
        /// </summary>
        public enum DeploymentStatusOption
        {
            /// <summary>
            /// This indicates the deployment is in progress status.
            /// </summary>
            IN_PROGRESS,
            /// <summary>
            /// This indicates the deployment is in pending status.
            /// </summary>
            PENDING,
            /// <summary>
            /// This indicates the deployment is in error status
            /// </summary>
            ERROR,
            /// <summary>
            /// This indicates the deployment is completed
            /// </summary>
            COMPLETE
        }

        /// <summary>
        /// SearchByAutoPatchDepPlanParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByAutoPatchDepPlanParameters
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
            /// Search by DEPLOYMENT_STATUS field
            /// </summary>
            DEPLOYMENT_STATUS = 3,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE = 4,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 5,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID=6
        }
        int patchDeploymentPlanId;
        string deploymentPlanName;
        DateTime deploymentPlannedDate;
        string patchFileName;
        string deploymentStatus;
        int masterEntityId;
        bool isReady;
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
        public AutoPatchDepPlanDTO()
        {
            log.Debug("Starts-AutoPatchDepPlanDTO() default constructor.");
            patchDeploymentPlanId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = "Y";
            isReady = false;
            deploymentStatus = DeploymentStatusOption.PENDING.ToString();
            log.Debug("Ends-AutoPatchDepPlanDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AutoPatchDepPlanDTO(int patchDeploymentPlanId, string deploymentPlanName, DateTime deploymentPlannedDate, string patchFileName, string deploymentStatus,
                                                  int masterEntityId, bool isReady, string isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                                  string guid, int siteId, bool synchStatus)
        {
            log.Debug("Starts-AutoPatchDepPlanDTO(with all the data fields) Parameterized constructor.");
            this.patchDeploymentPlanId = patchDeploymentPlanId;
            this.deploymentPlanName = deploymentPlanName;
            this.deploymentPlannedDate = deploymentPlannedDate;
            this.patchFileName = patchFileName;
            this.deploymentStatus = deploymentStatus;
            this.masterEntityId = masterEntityId;
            this.isReady = isReady;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.Debug("Ends-AutoPatchDepPlanDTO(with all the data fields) Parameterized constructor.");
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
        /// Get/Set method of the PatchFileName field
        /// </summary>
        [DisplayName("Patch File Name")]
        public string PatchFileName { get { return patchFileName; } set { patchFileName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DeploymentStatus field
        /// </summary>
        [DisplayName("Deployment Status")]
        [ReadOnly(true)]
        public string DeploymentStatus { get { return deploymentStatus; } set { deploymentStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsReady field
        /// </summary>
        [DisplayName("IsReady?")]
        [Browsable(false)]
        public bool IsReady { get { return isReady; } set { isReady = value; } }
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
