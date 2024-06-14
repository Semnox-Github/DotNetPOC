//using Semnox.Parafait.MonitorAsset;
/********************************************************************************************
 * Project Name - Patch Asset Application
 * Description  - Bussiness logic of patch asset application 
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
using Semnox.Parafait.logger;

namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Patch asset application will creates and modifies the application    
    /// </summary>
    public class AutoPatchAssetApplication
    {
        private AutoPatchAssetApplDTO autoPatchAssetApplDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoPatchAssetApplication()
        {
            log.Debug("Starts-AutoPatchAssetApplication() default constructor");
            autoPatchAssetApplDTO = null;
            log.Debug("Ends-AutoPatchAssetApplication() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="autoPatchAssetApplDTO">Parameter of the type AutoPatchAssetApplDTO</param>
        public AutoPatchAssetApplication(AutoPatchAssetApplDTO autoPatchAssetApplDTO)
        {
            log.Debug("Starts-AutoPatchAssetApplication(autoPatchAssetApplDTO) parameterized constructor.");
            this.autoPatchAssetApplDTO = autoPatchAssetApplDTO;
            log.Debug("Ends-AutoPatchAssetApplication(autoPatchAssetApplDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the patch asset application
        /// asset application will be inserted if PatchAssetApplicationId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext assetApplicationUserContext = ExecutionContext.GetExecutionContext();
            AutoPatchAssetApplDataHandler autoPatchAssetApplDataHandler = new AutoPatchAssetApplDataHandler();
            if (autoPatchAssetApplDTO.PatchAssetApplicationId <= 0)
            {
                int autoPatchAssetApplId = autoPatchAssetApplDataHandler.InsertAutoPatchAssetAppl(autoPatchAssetApplDTO, assetApplicationUserContext.GetUserId(), assetApplicationUserContext.GetSiteId());
                autoPatchAssetApplDTO.PatchAssetApplicationId = autoPatchAssetApplId;
            }
            else
            {
                if (autoPatchAssetApplDTO.IsChanged == true)
                {
                    autoPatchAssetApplDataHandler.UpdateAutoPatchAssetAppl(autoPatchAssetApplDTO, assetApplicationUserContext.GetUserId(), assetApplicationUserContext.GetSiteId());
                    autoPatchAssetApplDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }

    }
    /// <summary>
    /// Manages the list of patch asset application
    /// </summary>
    public class AutoPatchAssetApplList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        public AutoPatchAssetApplList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the patch asset application list
        /// </summary>
        public List<AutoPatchAssetApplDTO> GetAllAutoPatchAssetApplication(List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllAutoPatchAssetApplication(searchParameters) method.");
            AutoPatchAssetApplDataHandler patchApplicationTypeDataHandler = new AutoPatchAssetApplDataHandler();
            log.Debug("Ends-GetAllAutoPatchAssetApplications(searchParameters) method by returning the result of patchApplicationTypeDataHandler.GetAutoPatchAssetApplList() call.");
            return patchApplicationTypeDataHandler.GetAutoPatchAssetApplList(searchParameters);
        }
        /// <summary>
        /// Returns the AutoPatchAssetApplDTO list where version equal to the deployment plans minimum required version and other options are operated based on like operator
        /// </summary>
        /// <param name="searchParameters">Is list of KeyValuePair AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters and string.</param>
        /// <returns>List of AutoPatchAssetApplDTO </returns>        
        public List<AutoPatchAssetApplDTO> GetLowerVersionAssetApplication(List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetLowerVersionAssetApplication(searchParameters) method.");
            AutoPatchAssetApplDataHandler patchApplicationTypeDataHandler = new AutoPatchAssetApplDataHandler();
            log.Debug("Ends-GetLowerVersionAssetApplication(searchParameters) method by returning the result of patchApplicationTypeDataHandler.GetLowerVersionAssetApplication() call.");
            return patchApplicationTypeDataHandler.GetLowerVersionAssetApplication(searchParameters);
        }

        /// <summary>
        /// Returns the AutoPatchAssetApplDTO list where version equal to the deployment version and other options are operated based on like operator
        /// </summary>
        /// <param name="searchParameters">Is list of KeyValuePair AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters and string.</param>
        /// <returns>List of AutoPatchAssetApplDTO </returns>
        public List<AutoPatchAssetApplDTO> GetCurrentOrHigherVersionAssetApplication(List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetCurrentOrHigherVersionAssetApplication(searchParameters) method.");
            AutoPatchAssetApplDataHandler patchApplicationTypeDataHandler = new AutoPatchAssetApplDataHandler();
            log.Debug("Ends-GetCurrentOrHigherVersionAssetApplication(searchParameters) method by returning the result of patchApplicationTypeDataHandler.GetCurrentOrHigherVersionAssetApplication() call.");
            return patchApplicationTypeDataHandler.GetCurrentOrHigherVersionAssetApplication(searchParameters);
        }

        /// <summary>
        /// Gets all upgrade eligible asset applications
        /// </summary>
        /// <param name="assetName"> Name of the current asset</param>
        /// <param name="hostName">Host name </param>
        /// <param name="ipAddress">IP address</param>
        /// <param name="macAddress">MAC address</param>
        /// <param name="siteid">current site siteid</param>        
        /// <returns>AutoPatchAssetApplDTO List</returns>
        public List<AutoPatchAssetApplDTO> GetCurrentSystemAssetApplications(string assetName, string hostName, string ipAddress, string macAddress, int siteid)
        {
            List<MonitorAssetDTO> monitorAssetDTOList = new List<MonitorAssetDTO>();
            List<AutoPatchAssetApplDTO> autoPatchAssetApplDTOList = new List<AutoPatchAssetApplDTO>();
            AutoPatchAssetApplList autoPatchAssetApplList = new AutoPatchAssetApplList(executionContext);

            List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> autoPatchAssetApplDTOSearchParams = new List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>>();
            MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext);
            List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> monitorAssetDTOSearchParams = new List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>>();

            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.NAME, string.IsNullOrEmpty(assetName) ? "" : assetName));
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.HOST_NAME, string.IsNullOrEmpty(hostName) ? "" : hostName));
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.IP_ADDRESS, string.IsNullOrEmpty(ipAddress) ? "" : ipAddress));
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.MAC_ADDRESS, string.IsNullOrEmpty(macAddress) ? "" : macAddress));
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.SITE_ID, siteid.ToString()));
            monitorAssetDTOList = monitorAssetList.GetAllMonitorAssets(monitorAssetDTOSearchParams);
            if (monitorAssetDTOList != null)
            {
                if (monitorAssetDTOList.Count > 1)//checking for duplicate monitor assets
                {
                    throw new Exception("There more than one asset exists with same details");
                }
                else
                {
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ASSET_ID, monitorAssetDTOList[0].AssetId.ToString()));
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID, siteid.ToString()));
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ACTIVE_FLAG, "Y"));
                    autoPatchAssetApplDTOList = autoPatchAssetApplList.GetAllAutoPatchAssetApplication(autoPatchAssetApplDTOSearchParams);
                }
            }
            return autoPatchAssetApplDTOList;
        }
        /// <summary>
        /// Gets all upgrade eligible asset applications
        /// </summary>
        /// <param name="assetName"> Name of the current asset</param>
        /// <param name="hostName">Host name </param>
        /// <param name="ipAddress">IP address</param>
        /// <param name="macAddress">MAC address</param>
        /// <param name="siteid">current site siteid</param>
        /// <param name="applicationTypeId">Current application type id</param>
        /// <param name="minimumRequiredVersion">Version of the current application</param>
        /// <param name="upgradeVesion">Version upgrade info</param>
        /// <returns>AutoPatchAssetApplDTO List</returns>
        public List<AutoPatchAssetApplDTO> GetUpgradeEligibleAssetApplication(string assetName, string hostName, string ipAddress, string macAddress, int siteid, int applicationTypeId, string minimumRequiredVersion, string upgradeVesion)
        {
            int serverAssetId = -1;
            MonitorAssetTypeList monitorAssetTypeList = new MonitorAssetTypeList(executionContext);
            List<MonitorAssetTypeDTO> monitorAssetTypeDTOList = new List<MonitorAssetTypeDTO>();
            List<KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>> monitorAssetTypeDTOSearchParams = new List<KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>>();
            MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext);
            MonitorAssetDTO monitorAssetDTO = new MonitorAssetDTO();
            List<MonitorAssetDTO> monitorAssetDTOList = new List<MonitorAssetDTO>();
            List<MonitorAssetDTO> monitorAssetServerDTOList = new List<MonitorAssetDTO>();
            List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> monitorAssetDTOSearchParams = new List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>>();
            List<AutoPatchAssetApplDTO> autoPatchAssetApplDTOList = new List<AutoPatchAssetApplDTO>();
            List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> autoPatchAssetApplDTOSearchParams = new List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>>();
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.NAME, string.IsNullOrEmpty(assetName) ? "" : assetName));
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.HOST_NAME, string.IsNullOrEmpty(hostName) ? "" : hostName));
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.IP_ADDRESS, string.IsNullOrEmpty(ipAddress) ? "" : ipAddress));
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.MAC_ADDRESS, string.IsNullOrEmpty(macAddress) ? "" : macAddress));
            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.SITE_ID, siteid.ToString()));
            monitorAssetDTOList = monitorAssetList.GetAllMonitorAssets(monitorAssetDTOSearchParams);
            if (monitorAssetDTOList != null)
            {
                if (monitorAssetDTOList.Count > 1)//checking for duplicate monitor assets
                {
                    throw new Exception("There more than one asset exists with same details");
                }
                else
                {
                    //Checking for the server type monitor asset
                    //monitorAssetTypeDTOSearchParams.Add(new KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>(MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.ASSET_TYPE_ID, monitorAssetDTOList[0].AssetTypeId.ToString()));
                    monitorAssetTypeDTOSearchParams.Add(new KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>(MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.ASSET_TYPE_NAME, "Server"));
                    monitorAssetTypeDTOSearchParams.Add(new KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>(MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters.SITE_ID, siteid.ToString()));
                    monitorAssetTypeDTOList = monitorAssetTypeList.GetAllMonitorAssetTypes(monitorAssetTypeDTOSearchParams);
                    if (monitorAssetTypeDTOList != null)
                    {
                        //Checking if the monitor asset which match with passed condition is server or not
                        if (monitorAssetTypeDTOList[0].AssetTypeId != monitorAssetDTOList[0].AssetTypeId)
                        {
                            //if not the fetching the server id and keeping for further use
                            monitorAssetDTOSearchParams = new List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>>();
                            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_TYPE_ID, monitorAssetTypeDTOList[0].AssetTypeId.ToString()));
                            monitorAssetDTOSearchParams.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.SITE_ID, siteid.ToString()));
                            monitorAssetServerDTOList = monitorAssetList.GetAllMonitorAssets(monitorAssetDTOSearchParams);
                            if (monitorAssetServerDTOList != null)
                            {
                                serverAssetId = monitorAssetServerDTOList[0].AssetId;
                            }
                        }
                        else
                        {
                            //if fetched asset is server. Then server asset id will be monitor asset id  
                            serverAssetId = monitorAssetDTOList[0].AssetId;
                        }
                    }
                    else
                    {
                        throw new Exception("Server not found in asset types.");
                    }
                    monitorAssetDTO = monitorAssetDTOList[0];
                    //Fetching the asset applications which belongs to the current monitor asset. Using asset id 
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ASSET_ID, monitorAssetDTO.AssetId.ToString()));
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_UPGRADE_STATUS, "'" + AutoPatchAssetApplDTO.AssetApplicationStatusOption.ERROR.ToString() + "' , '" + AutoPatchAssetApplDTO.AssetApplicationStatusOption.COMPLETE.ToString() + "','" + AutoPatchAssetApplDTO.AssetApplicationStatusOption.PENDING.ToString() + "'"));
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ACTIVE_FLAG, "Y"));
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID, siteid.ToString()));
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID, applicationTypeId.ToString()));
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.MINIMUM_REQUIRED_VERSION, minimumRequiredVersion.ToString()));
                    autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER, upgradeVesion.ToString()));
                    autoPatchAssetApplDTOList = GetAllAutoPatchAssetApplication(autoPatchAssetApplDTOSearchParams);
                    if (autoPatchAssetApplDTOList != null)
                    {
                        for (int i = 0; i < autoPatchAssetApplDTOList.Count; i++)
                        {
                            //If the status of the application deployment is error with counter 3, then ignoring that asset application.
                            if (autoPatchAssetApplDTOList[i].PatchUpgradeStatus.Equals(AutoPatchAssetApplDTO.AssetApplicationStatusOption.ERROR.ToString()) && autoPatchAssetApplDTOList[i].ErrorCounter >= 3)
                            {
                                autoPatchAssetApplDTOList.RemoveAt(i);
                            }
                            else if (serverAssetId != autoPatchAssetApplDTOList[i].AssetId)
                            {
                                //If the server asset id and the application asset id is not same then  fetching the same application 
                                //type in server and finding the status. If the same application deployment is not completed in server.
                                //Then we are not allowing to install in client system.
                                List<AutoPatchAssetApplDTO> serverAutoPatchAssetApplDTOList = new List<AutoPatchAssetApplDTO>();
                                autoPatchAssetApplDTOSearchParams = new List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>>();
                                autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ASSET_ID, serverAssetId.ToString()));
                                autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID, autoPatchAssetApplDTOList[i].PatchApplicationTypeId.ToString()));
                                autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID, siteid.ToString()));
                                autoPatchAssetApplDTOSearchParams.Add(new KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER, autoPatchAssetApplDTOList[i].PatchVersionNumber));
                                serverAutoPatchAssetApplDTOList = GetLowerVersionAssetApplication(autoPatchAssetApplDTOSearchParams);
                                if (serverAutoPatchAssetApplDTOList != null && serverAutoPatchAssetApplDTOList.Count > 0)
                                {
                                    //if (!serverAutoPatchAssetApplDTOList[0].PatchUpgradeStatus.Equals(AutoPatchAssetApplDTO.AssetApplicationStatusOption.COMPLETE.ToString()))
                                    autoPatchAssetApplDTOList.Remove(autoPatchAssetApplDTOList[i]);
                                }
                            }
                        }
                    }
                }
            }
            return autoPatchAssetApplDTOList;
        }
    }
}
