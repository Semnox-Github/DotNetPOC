//using Semnox.Parafait.AutomatedPatchAsset;
/********************************************************************************************
 * Project Name - Auto Patch Dep Plan Application
 * Description  - Bussiness logic of auto patch dep plan application
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Feb-2016   Raghuveera          Created 
 //********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace  Semnox.Parafait.Deployment
{
    /// <summary>
    /// Auto patch dep plan application will creates and modifies the auto patch dep plan application
    /// </summary>
    public class AutoPatchDepPlanApplication
    {
        private AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplicationDTO;
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoPatchDepPlanApplication()
        {
            log.Debug("Starts-AutoPatchDepPlanApplication() default constructor");
            autoPatchDepPlanApplicationDTO = null;
            log.Debug("Ends-AutoPatchDepPlanApplication() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="autoPatchDepPlanApplicationDTO">Parameter of the type AutoPatchDepPlanApplicationDTO</param>
        public AutoPatchDepPlanApplication(AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplicationDTO)
        {
            log.Debug("Starts-AutoPatchDepPlanApplication(AutoPatchDepPlanApplicationDTO) parameterized constructor.");
            this.autoPatchDepPlanApplicationDTO = autoPatchDepPlanApplicationDTO;
            log.Debug("Ends-AutoPatchDepPlanApplication(AutoPatchDepPlanApplicationDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the auto patch dep plan application
        /// deployment plan will be inserted if PatchDeploymentPlanId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
             ExecutionContext deploymentPlanUserContext =  ExecutionContext.GetExecutionContext();
            AutoPatchDepPlanApplicationDataHandler AutoPatchDepPlanApplicationDataHandler = new AutoPatchDepPlanApplicationDataHandler();
            if (autoPatchDepPlanApplicationDTO.PatchDeploymentPlanApplicationId <= 0)
            {
                int AutoPatchDepPlanApplicationId = AutoPatchDepPlanApplicationDataHandler.InsertAutoPatchDepPlanApplication(autoPatchDepPlanApplicationDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                autoPatchDepPlanApplicationDTO.PatchDeploymentPlanApplicationId = AutoPatchDepPlanApplicationId;
            }
            else
            {
                if (autoPatchDepPlanApplicationDTO.IsChanged == true)
                {
                    AutoPatchDepPlanApplicationDataHandler.UpdateAutoPatchDepPlanApplication(autoPatchDepPlanApplicationDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                    autoPatchDepPlanApplicationDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }        
    }
    /// <summary>
    /// Manages the list of auto patch dep plan application
    /// </summary>
    public class AutoPatchDepPlanApplicationList
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the auto patch dep plan application list
        /// </summary>
        public List<AutoPatchDepPlanApplicationDTO> GetAllAutoPatchDepPlanApplications(List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllAutoPatchDepPlans(searchParameters) method.");
            AutoPatchDepPlanApplicationDataHandler autoPatchDepPlanApplicationDataHandler = new AutoPatchDepPlanApplicationDataHandler();
            log.Debug("Ends-GetAllAutoPatchDepPlans(searchParameters) method by returning the result of AutoPatchDepPlanApplicationDataHandler.GetAutoPatchDepPlanList() call.");
            return autoPatchDepPlanApplicationDataHandler.AutoPatchDepPlanApplicationList(searchParameters);
        }

        /// <summary>
        /// Getting the deployment status of passed deployment plan and site id
        /// </summary>
        /// <param name="deploymentPlanId">Deployment plan id</param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public string GetDeploymentStatus(int deploymentPlanId, int siteId)
        {
            int status;
            string deploymentStatus;
            status = 3;
            int changePasswordApplTypeId = -1;
            ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.PENDING.ToString();
            List<AutoPatchDepPlanApplicationDTO> autoPatchDepPlanApplicationDTOList = new List<AutoPatchDepPlanApplicationDTO>();
            List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>> autoPatchDepPlanApplicationSearchParams = new List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>>();
            AutoPatchAssetApplList autoPatchAssetApplList = new AutoPatchAssetApplList(deploymentPlanUserContext);
            List<AutoPatchAssetApplDTO> autoPatchAssetApplDTOList = new List<AutoPatchAssetApplDTO>();
            List<AutoPatchAssetApplDTO> autoPatchAssetApplDTOFilterdList = new List<AutoPatchAssetApplDTO>();
            List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> autoPatchAssetApplDTOSearchParams;
            List<AutoPatchApplTypeDTO> autoPatchApplTypeListOnDisplay;
            AutoPatchApplTypeList autoPatchApplTypeList = new AutoPatchApplTypeList();
            List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>> autoPatchApplTypeDTOSearchParams = new List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>>();
            autoPatchApplTypeDTOSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID, siteId.ToString()));
            autoPatchApplTypeDTOSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.APPLICATION_TYPE, "Change Password"));
            autoPatchApplTypeListOnDisplay = autoPatchApplTypeList.GetAllAutoPatchApplTypes(autoPatchApplTypeDTOSearchParams);
            if (autoPatchApplTypeListOnDisplay != null && autoPatchApplTypeListOnDisplay.Count > 0)
            {
                changePasswordApplTypeId = autoPatchApplTypeListOnDisplay[0].PatchApplicationTypeId;//Retrives the change password application type id
            }
            //retriving all the deployment applications
            autoPatchDepPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_ID, deploymentPlanId.ToString()));
            autoPatchDepPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.SITE_ID, siteId.ToString()));
            autoPatchDepPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.IS_ACTIVE, "Y"));
            autoPatchDepPlanApplicationDTOList = GetAllAutoPatchDepPlanApplications(autoPatchDepPlanApplicationSearchParams);
            if (autoPatchDepPlanApplicationDTOList != null)
            {
                foreach (AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplicationDTO in autoPatchDepPlanApplicationDTOList)
                {
                    autoPatchAssetApplDTOSearchParams = new List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>>();
                    if (changePasswordApplTypeId != autoPatchDepPlanApplicationDTO.PatchApplicationTypeId)
                    {
                        autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID, autoPatchDepPlanApplicationDTO.PatchApplicationTypeId.ToString()));
                        autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER, autoPatchDepPlanApplicationDTO.MinimumVersionRequired.ToString()));
                    }
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID, siteId.ToString()));                    
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ACTIVE_FLAG, "Y"));
                    //fetches the record PATCH_VERSION_NUMBER=MinimumVersionRequired version if records found means few applications are not upgraded
                    autoPatchAssetApplDTOList = autoPatchAssetApplList.GetLowerVersionAssetApplication(autoPatchAssetApplDTOSearchParams);
                    if (changePasswordApplTypeId != autoPatchDepPlanApplicationDTO.PatchApplicationTypeId)
                    {
                        if (autoPatchAssetApplDTOList != null)
                        {
                            //Checking for Error status in asset application 
                            autoPatchAssetApplDTOFilterdList = autoPatchAssetApplDTOList.Where(x => (bool)(x.PatchUpgradeStatus.Equals(AutoPatchAssetApplDTO.AssetApplicationStatusOption.ERROR.ToString()) && x.ErrorCounter >= 3)).ToList<AutoPatchAssetApplDTO>();
                            if (autoPatchAssetApplDTOFilterdList != null && autoPatchAssetApplDTOFilterdList.Count > 0)
                            {
                                //Updating the status to DTO
                                status = 0;
                                deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.ERROR.ToString();
                                break;
                            }
                            else
                            {
                                //Checking for In progress status in asset application
                                autoPatchAssetApplDTOFilterdList = new List<AutoPatchAssetApplDTO>();
                                autoPatchAssetApplDTOFilterdList = autoPatchAssetApplDTOList.Where(x => (bool)(x.PatchUpgradeStatus.Equals(AutoPatchAssetApplDTO.AssetApplicationStatusOption.IN_PROGRESS.ToString())||(x.PatchUpgradeStatus.Equals(AutoPatchAssetApplDTO.AssetApplicationStatusOption.ERROR.ToString())&&x.ErrorCounter<3))).ToList<AutoPatchAssetApplDTO>();
                                if (autoPatchAssetApplDTOFilterdList != null && autoPatchAssetApplDTOFilterdList.Count > 0)
                                {
                                    //Updating the status to DTO
                                    if (status > 1)
                                    {
                                        status = 1;
                                        deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.IN_PROGRESS.ToString();
                                    }
                                }
                                else
                                {
                                    //Checking for In pending status in asset application
                                    autoPatchAssetApplDTOFilterdList = new List<AutoPatchAssetApplDTO>();
                                    autoPatchAssetApplDTOFilterdList = autoPatchAssetApplDTOList.Where(x => (bool)x.PatchUpgradeStatus.Equals(AutoPatchAssetApplDTO.AssetApplicationStatusOption.PENDING.ToString())).ToList<AutoPatchAssetApplDTO>();
                                    if (autoPatchAssetApplDTOFilterdList != null && autoPatchAssetApplDTOFilterdList.Count > 0)
                                    {
                                        //Updating the status to DTO
                                        if (status > 2)
                                        {
                                            status = 2;
                                            deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.PENDING.ToString();
                                        }
                                    }
                                }
                            }
                        }
                        else//If lower vesion records are not found then
                        {
                            autoPatchAssetApplDTOSearchParams = new List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>>();
                            autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID, autoPatchDepPlanApplicationDTO.PatchApplicationTypeId.ToString()));
                            autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID, siteId.ToString()));
                            autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER, autoPatchDepPlanApplicationDTO.DeploymentVersion.ToString()));
                            autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ACTIVE_FLAG, "Y"));
                            autoPatchAssetApplDTOFilterdList = autoPatchAssetApplList.GetCurrentOrHigherVersionAssetApplication(autoPatchAssetApplDTOSearchParams);

                            autoPatchAssetApplDTOSearchParams = new List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>>();
                            autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID, autoPatchDepPlanApplicationDTO.PatchApplicationTypeId.ToString()));
                            autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID, siteId.ToString()));
                            autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ACTIVE_FLAG, "Y"));
                            autoPatchAssetApplDTOList = autoPatchAssetApplList.GetCurrentOrHigherVersionAssetApplication(autoPatchAssetApplDTOSearchParams);
                            //Checking for the deployment version & above version and all the asset types belongs to the same site id 
                            //If both are equal then upgrade status is complete else it is in Pending
                            if ((autoPatchAssetApplDTOFilterdList == null) || (autoPatchAssetApplDTOFilterdList != null && autoPatchAssetApplDTOFilterdList.Count==0))//Starts:added later
                            {
                                if (status > 2)
                                {
                                    status = 2;
                                    deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.PENDING.ToString();
                                }
                            }
                            else if (autoPatchAssetApplDTOFilterdList != null && autoPatchAssetApplDTOList != null && autoPatchAssetApplDTOFilterdList.Count !=0 && autoPatchAssetApplDTOFilterdList.Count < autoPatchAssetApplDTOList.Count)
                            {
                                if (status >= 1)
                                {
                                    status = 1;
                                    deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.IN_PROGRESS.ToString();
                                }
                            }
                            else//Ends:added later 
                                if (autoPatchAssetApplDTOFilterdList != null && autoPatchAssetApplDTOList != null && autoPatchAssetApplDTOFilterdList.Count == autoPatchAssetApplDTOList.Count)
                            {
                                if (status >= 3)
                                {
                                    status = 3;
                                    deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.COMPLETE.ToString();
                                }
                            }
                            else
                            {
                                if (status > 2)
                                {
                                    status = 2;
                                    deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.PENDING.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        autoPatchAssetApplDTOFilterdList = autoPatchAssetApplDTOList.Where(x => (bool)x.PasswordChangeStatus).ToList<AutoPatchAssetApplDTO>();
                        if (autoPatchAssetApplDTOFilterdList != null && autoPatchAssetApplDTOList!=null && autoPatchAssetApplDTOFilterdList.Count == autoPatchAssetApplDTOList.Count)
                        {
                            deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.COMPLETE.ToString();
                        }
                        else
                        {
                            deploymentStatus = AutoPatchDepPlanDTO.DeploymentStatusOption.PENDING.ToString();
                            break;
                        }
                    }
                }
            }
            return deploymentStatus;
        }        
    }
}
