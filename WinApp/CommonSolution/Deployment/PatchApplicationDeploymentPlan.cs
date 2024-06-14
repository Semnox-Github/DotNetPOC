/********************************************************************************************
 * Project Name - Patch Application Deployment Plan
 * Description  - Bussiness logic of patch application deployment plan
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace  Semnox.Parafait.Deployment
{
    /// <summary>
    /// Patch application deployment plan will creates and modifies the deployment plan   
    /// </summary>
    public class PatchApplicationDeploymentPlan
    {
        private PatchApplicationDeploymentPlanDTO patchApplicationDeploymentPlanDTO;
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public PatchApplicationDeploymentPlan()
        {
            log.Debug("Starts-PatchApplicationDeploymentPlan() default constructor");
            patchApplicationDeploymentPlanDTO = null;
            log.Debug("Ends-PatchApplicationDeploymentPlan() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="patchApplicationDeploymentPlanDTO">Parameter of the type PatchApplicationDeploymentPlanDTO</param>
        public PatchApplicationDeploymentPlan(PatchApplicationDeploymentPlanDTO patchApplicationDeploymentPlanDTO)
        {
            log.Debug("Starts-PatchApplicationDeploymentPlan(patchApplicationDeploymentPlanDTO) parameterized constructor.");
            this.patchApplicationDeploymentPlanDTO = patchApplicationDeploymentPlanDTO;
            log.Debug("Ends-PatchApplicationDeploymentPlan(patchApplicationDeploymentPlanDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the patch application deployment plan
        /// deployment plan will be inserted if PatchDeploymentPlanId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
             ExecutionContext deploymentPlanUserContext =  ExecutionContext.GetExecutionContext();
            PatchApplicationDeploymentPlanDataHandler patchApplicationDeploymentPlanDataHandler = new PatchApplicationDeploymentPlanDataHandler();
            if (patchApplicationDeploymentPlanDTO.PatchDeploymentPlanId <= 0)
            {
                int patchApplicationDeploymentPlanId = patchApplicationDeploymentPlanDataHandler.InsertPatchApplicationDeploymentPlan(patchApplicationDeploymentPlanDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                patchApplicationDeploymentPlanDTO.PatchDeploymentPlanId = patchApplicationDeploymentPlanId;
            }
            else
            {
                if (patchApplicationDeploymentPlanDTO.IsChanged == true)
                {
                    patchApplicationDeploymentPlanDataHandler.UpdatePatchApplicationDeploymentPlan(patchApplicationDeploymentPlanDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                    patchApplicationDeploymentPlanDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }
    }
    /// <summary>
    /// Manages the list of patch application deployment plan
    /// </summary>
    public class PatchApplicationDeploymentPlanList
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the patch application deployment plan list
        /// </summary>
        public List<PatchApplicationDeploymentPlanDTO> GetAllPatchApplicationDeploymentPlans(List<KeyValuePair<PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllPatchApplicationDeploymentPlans(searchParameters) method.");
            PatchApplicationDeploymentPlanDataHandler patchApplicationDeploymentPlanDataHandler = new PatchApplicationDeploymentPlanDataHandler();
            log.Debug("Ends-GetAllPatchApplicationDeploymentPlans(searchParameters) method by returning the result of patchApplicationDeploymentPlanDataHandler.GetPatchApplicationDeploymentPlanList() call.");
            return patchApplicationDeploymentPlanDataHandler.GetPatchApplicationDeploymentPlanList(searchParameters);
        }
    }
}
