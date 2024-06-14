/********************************************************************************************
 * Project Name - Deployment Site Map DTO
 * Description  - Data object of patch application deployment site map 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Feb-2016   Raghuveera          Created 
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
    /// This is the deployment site map data object class. This acts as data holder for the patch application deployment site maping business object
    /// </summary>
    public class DeploymentSiteMapDTO
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        int patchDeploymentPlanId;
        int siteId;
        string siteName;
        bool isApply;
        /// <summary>
        /// Default constructor
        /// </summary>
        public DeploymentSiteMapDTO()
        {
            log.Debug("Starts-DeploymentSiteMapDTO() default constructor.");
            patchDeploymentPlanId = -1;
            log.Debug("Ends-DeploymentSiteMapDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DeploymentSiteMapDTO(int patchDeploymentPlanId, int siteId, string siteName, bool isApply)
        {
            log.Debug("Starts-DeploymentSiteMapDTO(with all the data fields) Parameterized constructor.");
            this.patchDeploymentPlanId = patchDeploymentPlanId;
            this.siteName = siteName;
            this.siteId = siteId;
            this.isApply = isApply;
            log.Debug("Ends-DeploymentSiteMapDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get/Set method of the PatchDeploymentPlanId field
        /// </summary>
        [DisplayName("Plan Id")]
        [Browsable(false)]
        public int PatchDeploymentPlanId { get { return patchDeploymentPlanId; } set { patchDeploymentPlanId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [ReadOnly(true)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteName field
        /// </summary>
        [DisplayName("Site Name")]
        public string SiteName { get { return siteName; } set { siteName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsApply field
        /// </summary>
        [DisplayName("Apply Patch?")]
        public bool IsApply { get { return isApply; } set { isApply = value; this.IsChanged = true; } }
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
