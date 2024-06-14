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
//using Semnox.Parafait.AutomatedPatchAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Patch application deployment plan will creates and modifies the deployment plan   
    /// </summary>
    public class AutoPatchDeploymentPlan
    {
        private AutoPatchDepPlanDTO autoPatchDepPlanDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoPatchDeploymentPlan()
        {
            log.Debug("Starts-AutoPatchDepPlan() default constructor");
            autoPatchDepPlanDTO = null;
            log.Debug("Ends-AutoPatchDepPlan() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="autoPatchDepPlanDTO">Parameter of the type AutoPatchDepPlanDTO</param>
        public AutoPatchDeploymentPlan(AutoPatchDepPlanDTO autoPatchDepPlanDTO)
        {
            log.Debug("Starts-AutoPatchDepPlan(AutoPatchDepPlanDTO) parameterized constructor.");
            this.autoPatchDepPlanDTO = autoPatchDepPlanDTO;
            log.Debug("Ends-AutoPatchDepPlan(AutoPatchDepPlanDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the patch application deployment plan
        /// deployment plan will be inserted if PatchDeploymentPlanId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            AutoPatchDepPlanDataHandler autoPatchDepPlanDataHandler = new AutoPatchDepPlanDataHandler();
            if (autoPatchDepPlanDTO.PatchDeploymentPlanId <= 0)
            {
                int AutoPatchDepPlanId = autoPatchDepPlanDataHandler.InsertAutoPatchDepPlan(autoPatchDepPlanDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                autoPatchDepPlanDTO.PatchDeploymentPlanId = AutoPatchDepPlanId;
            }
            else
            {
                if (autoPatchDepPlanDTO.IsChanged == true)
                {
                    autoPatchDepPlanDataHandler.UpdateAutoPatchDepPlan(autoPatchDepPlanDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                    autoPatchDepPlanDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }
        /// <summary>
        /// Check the status and updates the status to the autoPatchDepPlanDTO passed in AutoPatchDeploymentPlan class constructor
        /// </summary>
        public void CheckDeploymentStatus()
        {
            ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            AutoPatchDepPlanApplicationList autoPatchDepPlanApplicationList = new AutoPatchDepPlanApplicationList();
            autoPatchDepPlanDTO.DeploymentStatus = autoPatchDepPlanApplicationList.GetDeploymentStatus(autoPatchDepPlanDTO.PatchDeploymentPlanId, deploymentPlanUserContext.GetSiteId());
        }
        /// <summary>
        /// Updates the password change status
        /// </summary>
        public void UpdateAssetApplication()
        {
            if (autoPatchDepPlanDTO.DeploymentStatus == AutoPatchDepPlanDTO.DeploymentStatusOption.COMPLETE.ToString())
            {
                ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
                AutoPatchAssetApplList autoPatchAssetApplList = new AutoPatchAssetApplList(deploymentPlanUserContext);
                AutoPatchAssetApplication autoPatchAssetApplication;
                List<AutoPatchAssetApplDTO> autoPatchAssetApplDTOList = new List<AutoPatchAssetApplDTO>();
                List<AutoPatchAssetApplDTO> autoPatchAssetApplDTOFilterList = new List<AutoPatchAssetApplDTO>();
                List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> autoPatchAssetApplDTOSearchParams;
                autoPatchAssetApplDTOSearchParams = new List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>>();
                autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID, autoPatchDepPlanDTO.Siteid.ToString()));
                autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ACTIVE_FLAG, "Y"));
                autoPatchAssetApplDTOList = autoPatchAssetApplList.GetLowerVersionAssetApplication(autoPatchAssetApplDTOSearchParams);
                autoPatchAssetApplDTOFilterList = autoPatchAssetApplDTOList.Where(x => (bool)x.PasswordChangeStatus).ToList<AutoPatchAssetApplDTO>();
                if (autoPatchAssetApplDTOFilterList != null && autoPatchAssetApplDTOList != null)
                {
                    if (autoPatchAssetApplDTOFilterList.Count == autoPatchAssetApplDTOList.Count)
                    {
                        foreach (AutoPatchAssetApplDTO autoPatchAssetApplDTO in autoPatchAssetApplDTOList)
                        {
                            autoPatchAssetApplDTO.PasswordChangeStatus = false;
                            autoPatchAssetApplication = new AutoPatchAssetApplication(autoPatchAssetApplDTO);
                            autoPatchAssetApplication.Save();
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Manages the list of patch application deployment plan
    /// </summary>
    public class AutoPatchDepPlanList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the patch application deployment plan list
        /// </summary>
        public List<AutoPatchDepPlanDTO> GetAllAutoPatchDepPlans(List<KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllAutoPatchDepPlans(searchParameters) method.");
            AutoPatchDepPlanDataHandler autoPatchDepPlanDataHandler = new AutoPatchDepPlanDataHandler();
            log.Debug("Ends-GetAllAutoPatchDepPlans(searchParameters) method by returning the result of AutoPatchDepPlanDataHandler.GetAutoPatchDepPlanList() call.");
            return autoPatchDepPlanDataHandler.GetAutoPatchDepPlanList(searchParameters);
        }
        /// <summary>
        /// Returns the eligible patch application deployment plan list
        /// </summary>
        public List<AutoPatchDepPlanDTO> GetEligibleDepPlanList(int siteId)
        {
            log.Debug("Starts-GetEligibleDepPlanList(siteId) method.");
            AutoPatchDepPlanDataHandler autoPatchDepPlanDataHandler = new AutoPatchDepPlanDataHandler();
            log.Debug("Ends-GetEligibleDepPlanList(siteId) method by returning the result of AutoPatchDepPlanDataHandler.GetAutoPatchDepPlanList() call.");
            return autoPatchDepPlanDataHandler.GetEligibleDepPlanList(siteId);
        }
    }
}
