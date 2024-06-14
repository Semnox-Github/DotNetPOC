/********************************************************************************************
 * Project Name - Publish
 * Description  - Bussiness logic of publish to site function
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00          14-Jul-2016   Raghuveera     Created 
 *2.70.2        17-Oct-2019   Guru S A       Waiver-Phase-2 Enhancements
 *2.90.0        27-Jul-2020   Dakshakh       AchievementScoreLog entity as roaming
 *2.90.0        28-Jul-2020   Deeksha        Issue Fix : Inv Barcode publish for the entity "products"
 *2.130.0       12-Jul-2021   Lakshminarayana    Modified : Static menu enhancement
 *2.140.0       04-Oct-2021   Girish Kundar      Modified : Issue Fix - Exception handling
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Publish
{
    /// <summary>
    /// Classs of EntityDetails
    /// </summary>
    public class EntityDetails
    {
        /// <summary>
        /// variable to store the entity name
        /// </summary>
        public string entityName;
        /// <summary>
        /// variable to store active flag column name
        /// </summary>
        public string activeFlagColumnName;
        /// <summary>
        /// variable to store site primary key id
        /// </summary>
        public int sitePkId;
        /// <summary>
        /// variable to store master entity id
        /// </summary>
        public int publishEntityPKId;
        /// <summary>
        /// variable to selectedSiteId
        /// </summary>
        public int selectedSiteId;
    }

    /// <summary>
    /// The Business logic for the publish to site
    /// </summary>
    public class Publish
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string entity = string.Empty;
        /// <summary>
        /// Default constructor
        /// </summary>
        public Publish()
        {

        }

        /// <summary>
        /// Argument constructor
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="_Utilities"></param>
        public Publish(string entity, Semnox.Core.Utilities.Utilities _Utilities)
        {
            this.entity = entity;
            Utilities = _Utilities;
        }

        /* #region AssetType
         AssetTypeList assetTypeList;
         AssetType assetType;
         List<AssetTypeDTO> assetTypeDTOList;
         List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchByAssetTypeParameters;
         List<AssetTypeDTO> siteAssetTypeDTOList;
         #endregion
         #region AssetGroup
         AssetGroupList assetGroupList;
         AssetGroup assetGroup;
         List<AssetGroupDTO> assetGroupDTOList;
         List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> searchByAssetGroupParameters;
         List<AssetGroupDTO> siteAssetGroupDTOList;
         #endregion
         #region Assets
         AssetList assetList;
         GenericAsset genericAsset;
         List<GenericAssetDTO> genericAssetDTOList;
         List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchByAssetParameters;
         List<GenericAssetDTO> siteGenericAssetDTOList;
         #endregion
         #region AssetGroupAsset
         AssetGroupAssetMapperList assetGroupAssetMapperList;
         AssetGroupAssetMapper assetGroupAssetMapper;
         List<AssetGroupAssetDTO> assetGroupAssetDTOList;
         List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> searchByAssetGroupAssetParameters;
         List<AssetGroupAssetDTO> siteAssetGroupAseetDTOList;
         #endregion
         #region MaintenanceTaskGroup
         Semnox.Parafait.MaintenanceTasks.MaintenanceTaskGroupList maintenanceTaskGroupList;
         Semnox.Parafait.MaintenanceTasks.MaintenanceTaskGroup maintenanceTaskGroup;
         List<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO> maintenanceTaskGroupDTOList;
         List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>> searchByMaintenanceTaskGroupParameters;
         List<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO> siteMaintenanceTaskGroupDTOList;
         #endregion
         #region MaintenanceTask
         Semnox.Parafait.MaintenanceTasks.MaintenanceTaskList maintenanceTaskList;
         Semnox.Parafait.MaintenanceTasks.MaintenanceTask maintenanceTask;
         List<Parafait.MaintenanceTasks.MaintenanceTaskDTO> maintenanceTaskDTOList;
         List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>> searchByMaintenanceTaskParameters;
         List<Parafait.MaintenanceTasks.MaintenanceTaskDTO> siteMaintenanceTaskDTOList;
         #endregion
         #region Tax
         Semnox.Parafait.Product.TaxList taxList;
         List<Semnox.Parafait.Product.TaxDTO> taxDTOList;
         List<Semnox.Parafait.Product.TaxDTO> siteTaxDTOList;
         List<KeyValuePair<Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>> searchByTaxParameters;
         #endregion
         #region Lookups
         LookupValuesList lookupValuesList;
         List<LookupValuesDTO> lookupValuesDTOList;
         List<LookupValuesDTO> siteLookupValuesDTOList;
         List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchByLookupValuesParameters;
         #endregion
         #region Schedule
         ScheduleList scheduleList;
         Schedule schedule;
         List<ScheduleDTO> scheduleDTOList;
         List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>> searchByScheduleParameters;
         List<ScheduleDTO> siteScheduleDTOList;
         #endregion
         #region MaintSchedule
         MaintenanceScheduleList maintenanceScheduleList;
         MaintenanceSchedule maintenanceSchedule;
         List<MaintenanceScheduleDTO> maintenanceScheduleDTOList;
         List<KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>> searchByMaintenanceScheduleParameters;
         List<MaintenanceScheduleDTO> siteMaintenanceScheduleDTOList;
         #endregion
         #region MaintSchAssetTasks
         ScheduleAssetTaskList scheduleAssetTaskList;
         ScheduleAssetTask scheduleAssetTask;
         List<ScheduleAssetTaskDTO> scheduleAssetTaskDTOList;
         List<KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>> searchByScheduleAssetTaskParameters;
         List<ScheduleAssetTaskDTO> siteScheduleAssetTaskDTOList;
         #endregion
         #region MaintCheckList(Job)
         Parafait.MaintenanceTasks.MaintenanceJobList maintenanceJobList;
         Parafait.MaintenanceTasks.MaintenanceJob maintenanceJob;
         Parafait.MaintenanceTasks.MaintenanceJobDTO maintenanceJobDTO;
         List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>> searchByMaintenanceJobParameters;
         List<Parafait.MaintenanceTasks.MaintenanceJobDTO> siteMaintenanceJobDTOList;
         #endregion
         #region MaintExclusionDays
         ScheduleExclusionList scheduleExclusionList;
         ScheduleExclusion scheduleExclusion;
         List<ScheduleExclusionDTO> scheduleExclusionDTOList;
         List<KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>> searchByScheduleExclusionParameters;
         List<ScheduleExclusionDTO> siteScheduleExclusionDTOList;
         #endregion
         #region Users
         UsersList usersList;
         List<UsersDTO> usersDTOList;
         List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByUserParameters;
         List<UsersDTO> siteUsersDTOList;

         #endregion*/
        Semnox.Core.Utilities.Utilities Utilities;
        /// <summary>
        /// Function to publish the asset type
        /// </summary>
        /// <param name="AssetTypeId"> assetTypeId</param>
        /// <param name="masterSiteId">Master site Id</param>
        /// <param name="siteId">The site id to which you want to publish.</param>
        /* public void PublishAssetType(int AssetTypeId, int masterSiteId, int siteId)
         {
             log.Debug("Starts-PublishAssetType(" + AssetTypeId + ", " + masterSiteId + ", " + siteId + ") event.");
             assetTypeList = new AssetTypeList();
             assetTypeDTOList = new List<AssetTypeDTO>();
             searchByAssetTypeParameters = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
             searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, masterSiteId.ToString()));
             searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_ID, AssetTypeId.ToString()));
             assetTypeDTOList = assetTypeList.GetAllAssetTypes(searchByAssetTypeParameters);
             if (assetTypeDTOList != null)
             {
                 searchByAssetTypeParameters = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                 searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, siteId.ToString()));
                 //searchByAssetTypeParameters.Add(new KeyValuePair< AssetTypeDTO.SearchByAssetTypeParameters, string>( AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME, assetTypeDTOList[0].Name));
                 searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.MASTER_ENTITY_ID, assetTypeDTOList[0].AssetTypeId.ToString()));
                 siteAssetTypeDTOList = assetTypeList.GetAllAssetTypes(searchByAssetTypeParameters);
                 if (siteAssetTypeDTOList != null)
                 {
                     siteAssetTypeDTOList[0].Name = assetTypeDTOList[0].Name;
                     siteAssetTypeDTOList[0].IsActive = assetTypeDTOList[0].IsActive;
                     assetType = new AssetType(siteAssetTypeDTOList[0]);
                 }
                 else
                 {
                     assetTypeDTOList[0].MasterEntityId = assetTypeDTOList[0].AssetTypeId;
                     assetTypeDTOList[0].AssetTypeId = -1;
                     assetType = new AssetType(assetTypeDTOList[0]);
                 }
                 assetType.Save();
             }
             log.Debug("Ends-PublishAssetType(" + AssetTypeId + ", " + masterSiteId + ", " + siteId + ") event.");
         }*/

        /// <summary>
        /// Function to publish the asset Group
        /// </summary>
        /// <param name="AssetGroupId"> assetGroupId</param>
        /// <param name="masterSiteId">Master site Id</param>
        /// <param name="siteId">The site id to which you want to publish.</param>
        /* public void PublishAssetGroup(int AssetGroupId, int masterSiteId, int siteId)
         {
             log.Debug("Starts-PublishAssetGroup(" + AssetGroupId + ", " + masterSiteId + ", " + siteId + ") event.");
             assetGroupList = new AssetGroupList();
             assetGroupDTOList = new List<AssetGroupDTO>();
             searchByAssetGroupParameters = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
             searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, masterSiteId.ToString()));
             searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_ID, AssetGroupId.ToString()));
             assetGroupDTOList = assetGroupList.GetAllAssetGroups(searchByAssetGroupParameters);
             if (assetGroupDTOList != null)
             {
                 searchByAssetGroupParameters = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
                 searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, siteId.ToString()));
                 //searchByAssetGroupParameters.Add(new KeyValuePair< AssetGroupDTO.SearchByAssetGroupParameters, string>( AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_NAME, assetGroupDTOList[0].AssetGroupName));
                 searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.MASTER_ENTITY_ID, assetGroupDTOList[0].AssetGroupId.ToString()));
                 siteAssetGroupDTOList = assetGroupList.GetAllAssetGroups(searchByAssetGroupParameters);
                 if (siteAssetGroupDTOList != null)
                 {
                     siteAssetGroupDTOList[0].AssetGroupName = assetGroupDTOList[0].AssetGroupName;
                     siteAssetGroupDTOList[0].IsActive = assetGroupDTOList[0].IsActive;
                     assetGroup = new AssetGroup(siteAssetGroupDTOList[0]);
                 }
                 else
                 {
                     assetGroupDTOList[0].MasterEntityId = assetGroupDTOList[0].AssetGroupId;
                     assetGroupDTOList[0].AssetGroupId = -1;
                     assetGroup = new AssetGroup(assetGroupDTOList[0]);
                 }
                 assetGroup.Save();
             }
             log.Debug("Ends-PublishAssetGroup(" + AssetGroupId + ", " + masterSiteId + ", " + siteId + ") event.");
         }*/

        /*/// <summary>
        /// Function to publish the Maintenance Task Group
        /// </summary>
        /// <param name="MaintenanceTaskGroupId"> maintenanceTaskGroupId</param>
        /// <param name="masterSiteId">Master site Id</param>
        /// <param name="siteId">The site id to which you want to publish.</param>
        public void PublishMaintenanceTaskGroup(int MaintenanceTaskGroupId, int masterSiteId, int siteId)
        {
            log.Debug("Starts-PublishMaintenanceTaskGroup(" + MaintenanceTaskGroupId + ", " + masterSiteId + ", " + siteId + ") event.");
            maintenanceTaskGroupList = new Parafait.MaintenanceTasks.MaintenanceTaskGroupList();
            maintenanceTaskGroupDTOList = new List<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO>();
            searchByMaintenanceTaskGroupParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>>();
            searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.SITE_ID, masterSiteId.ToString()));
            searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MAINT_TASK_GROUP_ID, MaintenanceTaskGroupId.ToString()));
            maintenanceTaskGroupDTOList = maintenanceTaskGroupList.GetAllMaintenanceTaskGroups(searchByMaintenanceTaskGroupParameters);
            if (maintenanceTaskGroupDTOList != null)
            {
                searchByMaintenanceTaskGroupParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>>();
                searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.SITE_ID, siteId.ToString()));
                //searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.TASK_GROUP_NAME, maintenanceTaskGroupDTOList[0].TaskGroupName));
                searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MASTER_ENTITY_ID, maintenanceTaskGroupDTOList[0].MaintTaskGroupId.ToString()));
                siteMaintenanceTaskGroupDTOList = maintenanceTaskGroupList.GetAllMaintenanceTaskGroups(searchByMaintenanceTaskGroupParameters);
                if (siteMaintenanceTaskGroupDTOList != null)
                {
                    siteMaintenanceTaskGroupDTOList[0].TaskGroupName = maintenanceTaskGroupDTOList[0].TaskGroupName;
                    siteMaintenanceTaskGroupDTOList[0].IsActive = maintenanceTaskGroupDTOList[0].IsActive;
                    maintenanceTaskGroup = new Parafait.MaintenanceTasks.MaintenanceTaskGroup(siteMaintenanceTaskGroupDTOList[0]);
                }
                else
                {
                    maintenanceTaskGroupDTOList[0].MasterEntityId = maintenanceTaskGroupDTOList[0].MaintTaskGroupId;
                    maintenanceTaskGroupDTOList[0].MaintTaskGroupId = -1;
                    maintenanceTaskGroup = new Parafait.MaintenanceTasks.MaintenanceTaskGroup(maintenanceTaskGroupDTOList[0]);
                }
                maintenanceTaskGroup.Save();
            }
            log.Debug("Ends-PublishMaintenanceTaskGroup(" + MaintenanceTaskGroupId + ", " + masterSiteId + ", " + siteId + ") event.");
        }*/

        /// <summary>
        /// Function to publish the Asset
        /// </summary>
        /// <param name="assetId"> Asset Id</param>
        /// <param name="masterSiteId">Master site Id</param>
        /// <param name="siteId">The site id to which you want to publish.</param>
        /* public void PublishAsset(int assetId, int masterSiteId, int siteId)
         {
             log.Debug("Starts-PublishAsset(" + assetId + ", " + masterSiteId + ", " + siteId + ") event.");
             assetList = new AssetList();
             genericAssetDTOList = new List<GenericAssetDTO>();
             searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
             searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, masterSiteId.ToString()));
             searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_ID, assetId.ToString()));
             genericAssetDTOList = assetList.GetAllAssets(searchByAssetParameters);
             if (genericAssetDTOList != null)
             {
                 searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                 searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, siteId.ToString()));
                 //searchByAssetParameters.Add(new KeyValuePair< GenericAssetDTO.SearchByAssetParameters, string>( GenericAssetDTO.SearchByAssetParameters.ASSET_NAME, genericAssetDTOList[0].Name));
                 searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.MASTER_ENTITY_ID, genericAssetDTOList[0].AssetId.ToString()));
                 siteGenericAssetDTOList = assetList.GetAllAssets(searchByAssetParameters);
                 if (siteGenericAssetDTOList != null)//already exists
                 {
                     genericAssetDTOList[0].AssetId = siteGenericAssetDTOList[0].AssetId;
                     genericAssetDTOList[0].MasterEntityId = siteGenericAssetDTOList[0].MasterEntityId;
                 }
                 else//Not exists
                 {
                     genericAssetDTOList[0].MasterEntityId = genericAssetDTOList[0].AssetId;
                     genericAssetDTOList[0].AssetId = -1;
                 }
                 if (genericAssetDTOList[0].Machineid > 0)
                 {
                     log.Error("Ends-PublishAsset(" + assetId + ", " + masterSiteId + ", " + siteId + ") event with error: Asset with machine id cannot be published.");
                     throw (new Exception("Asset with machine id cannot be published."));
                 }
                 //site level asset type finding
                 assetTypeList = new AssetTypeList();
                 assetTypeDTOList = new List<AssetTypeDTO>();
                 searchByAssetTypeParameters = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                 searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, masterSiteId.ToString()));
                 searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_ID, genericAssetDTOList[0].AssetTypeId.ToString()));
                 assetTypeDTOList = assetTypeList.GetAllAssetTypes(searchByAssetTypeParameters);
                 if (assetTypeDTOList != null)
                 {
                     searchByAssetTypeParameters = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                     searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, siteId.ToString()));
                     //searchByAssetTypeParameters.Add(new KeyValuePair< AssetTypeDTO.SearchByAssetTypeParameters, string>( AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME, assetTypeDTOList[0].Name));
                     searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.MASTER_ENTITY_ID, assetTypeDTOList[0].AssetTypeId.ToString()));
                     siteAssetTypeDTOList = assetTypeList.GetAllAssetTypes(searchByAssetTypeParameters);
                     if (siteAssetTypeDTOList != null)
                     {
                         genericAssetDTOList[0].AssetTypeId = siteAssetTypeDTOList[0].AssetTypeId;
                     }
                     else
                     {
                         log.Error("Ends-PublishAsset(" + assetId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + assetTypeDTOList[0].Name + " asset type first.");
                         throw (new Exception("Publish asset type entity first."));
                     }
                 }
                 //site level Tax finding.
                 taxList = new Semnox.Parafait.Product.TaxList();
                 taxDTOList = new List<Semnox.Parafait.Product.TaxDTO>();
                 searchByTaxParameters = new List<KeyValuePair<Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>>();
                 searchByTaxParameters.Add(new KeyValuePair<Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>(Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters.SITE_ID, masterSiteId.ToString()));
                 searchByTaxParameters.Add(new KeyValuePair<Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>(Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters.TAX_ID, genericAssetDTOList[0].AssetTaxTypeId.ToString()));
                 taxDTOList = taxList.GetAllTaxes(searchByTaxParameters);
                 if (taxDTOList != null)
                 {
                     searchByTaxParameters = new List<KeyValuePair<Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>>();
                     searchByTaxParameters.Add(new KeyValuePair<Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>(Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters.SITE_ID, siteId.ToString()));
                     //searchByTaxParameters.Add(new KeyValuePair<  Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>(  Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters.TAX_NAME, taxDTOList[0].TaxName));
                     searchByTaxParameters.Add(new KeyValuePair<Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>(Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters.TAX_PERCENTAGE, taxDTOList[0].TaxPercentage.ToString()));
                     searchByTaxParameters.Add(new KeyValuePair<Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters, string>(Semnox.Parafait.Product.TaxDTO.SearchByTaxParameters.MASTER_ENTITY_ID, taxDTOList[0].TaxId.ToString()));
                     siteTaxDTOList = taxList.GetAllTaxes(searchByTaxParameters);
                     if (siteTaxDTOList != null)
                     {
                         genericAssetDTOList[0].AssetTaxTypeId = siteTaxDTOList[0].TaxId;
                     }
                     else
                     {
                         log.Error("Ends-PublishAsset(" + assetId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + taxDTOList[0].TaxName + " tax first.");
                         throw (new Exception("Publish tax entity first."));
                     }
                 }
                 //site level Asset Status finding.
                 lookupValuesList = new LookupValuesList();
                 lookupValuesDTOList = new List<LookupValuesDTO>();
                 searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                 searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                 searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, genericAssetDTOList[0].AssetStatus.ToString()));
                 searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_ASSET_STATUS"));
                 lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                 if (lookupValuesDTOList == null)
                 {
                     log.Error("Ends-PublishAsset(" + assetId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + genericAssetDTOList[0].AssetStatus + " lookup value for Lookup MAINT_ASSET_STATUS first.");
                     throw (new Exception("Publish lookup entity first."));
                 }
                 genericAsset = new GenericAsset(genericAssetDTOList[0]);
                 genericAsset.Save();
             }
             log.Debug("Ends-PublishAsset(" + assetId + ", " + masterSiteId + ", " + siteId + ") event.");
         }*/

        /// <summary>
        /// Function to publish the Task
        /// </summary>
        /// <param name="taskId"> Task Id</param>
        /// <param name="masterSiteId">Master site Id</param>
        /// <param name="siteId">The site id to which you want to publish.</param>
        /*public void PublishMaintenanceTask(int taskId, int masterSiteId, int siteId)
        {
            log.Debug("Starts-PublishMaintenanceTask(" + taskId + ", " + masterSiteId + ", " + siteId + ") event.");
            maintenanceTaskList = new Parafait.MaintenanceTasks.MaintenanceTaskList();
            maintenanceTaskDTOList = new List<Parafait.MaintenanceTasks.MaintenanceTaskDTO>();
            searchByMaintenanceTaskParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>>();
            searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.SITE_ID, masterSiteId.ToString()));
            searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MAINT_TASK_ID, taskId.ToString()));
            maintenanceTaskDTOList = maintenanceTaskList.GetAllMaintenanceTasks(searchByMaintenanceTaskParameters);
            if (maintenanceTaskDTOList != null)
            {
                searchByMaintenanceTaskParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>>();
                searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.SITE_ID, siteId.ToString()));
                //searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.TASK_NAME, maintenanceTaskDTOList[0].TaskName));
                searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MASTER_ENTITY_ID, maintenanceTaskDTOList[0].MaintTaskId.ToString()));
                siteMaintenanceTaskDTOList = maintenanceTaskList.GetAllMaintenanceTasks(searchByMaintenanceTaskParameters);
                if (siteMaintenanceTaskDTOList != null)//already exists
                {
                    maintenanceTaskDTOList[0].MaintTaskId = siteMaintenanceTaskDTOList[0].MaintTaskId;
                    maintenanceTaskDTOList[0].MasterEntityId = siteMaintenanceTaskDTOList[0].MasterEntityId;
                }
                else//Not exists
                {
                    maintenanceTaskDTOList[0].MasterEntityId = maintenanceTaskDTOList[0].MaintTaskId;
                    maintenanceTaskDTOList[0].MaintTaskId = -1;
                }
                //site level Task group finding
                maintenanceTaskGroupList = new Parafait.MaintenanceTasks.MaintenanceTaskGroupList();
                maintenanceTaskGroupDTOList = new List<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO>();
                searchByMaintenanceTaskGroupParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>>();
                searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.SITE_ID, masterSiteId.ToString()));
                searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MAINT_TASK_GROUP_ID, maintenanceTaskDTOList[0].MaintTaskGroupId.ToString()));
                maintenanceTaskGroupDTOList = maintenanceTaskGroupList.GetAllMaintenanceTaskGroups(searchByMaintenanceTaskGroupParameters);
                if (maintenanceTaskGroupDTOList != null)
                {
                    searchByMaintenanceTaskGroupParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>>();
                    searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.SITE_ID, siteId.ToString()));
                    //searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.TASK_GROUP_NAME, maintenanceTaskGroupDTOList[0].TaskGroupName));
                    searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MASTER_ENTITY_ID, maintenanceTaskGroupDTOList[0].MaintTaskGroupId.ToString()));
                    siteMaintenanceTaskGroupDTOList = maintenanceTaskGroupList.GetAllMaintenanceTaskGroups(searchByMaintenanceTaskGroupParameters);
                    if (siteMaintenanceTaskGroupDTOList != null)
                    {
                        maintenanceTaskDTOList[0].MaintTaskGroupId = siteMaintenanceTaskGroupDTOList[0].MaintTaskGroupId;
                    }
                    else
                    {
                        log.Error("Ends-PublishMaintenanceTask(" + taskId + ", " + masterSiteId + ", " + siteId + ") event. With error publish " + maintenanceTaskGroupDTOList[0].TaskGroupName + " maintenance task group first.");
                        throw (new Exception("Publish maintenance task group entity first."));
                    }
                }

                maintenanceTask = new Parafait.MaintenanceTasks.MaintenanceTask(maintenanceTaskDTOList[0]);
                maintenanceTask.Save();
            }
            log.Debug("Ends-PublishMaintenanceTask(" + taskId + ", " + masterSiteId + ", " + siteId + ") event.");
        }*/

        /// <summary>
        /// Function to publish the asset group asset
        /// </summary>
        /// <param name="assetGroupAssetId"> assetGroupAssetId</param>
        /// <param name="masterSiteId">Master site Id</param>
        /// <param name="siteId">The site id to which you want to publish.</param>
        /*public void PublishAssetGroupAsset(int assetGroupAssetId, int masterSiteId, int siteId)
        {
            log.Debug("Starts-PublishAssetGroupAsset(" + assetGroupAssetId + ", " + masterSiteId + ", " + siteId + ") event.");
            assetGroupAssetMapperList = new AssetGroupAssetMapperList();
            assetGroupAssetDTOList = new List<AssetGroupAssetDTO>();
            searchByAssetGroupAssetParameters = new List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>>();
            searchByAssetGroupAssetParameters.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.SITE_ID, masterSiteId.ToString()));
            searchByAssetGroupAssetParameters.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ASSET_ID, assetGroupAssetId.ToString()));
            assetGroupAssetDTOList = assetGroupAssetMapperList.GetAllAssetGroupAsset(searchByAssetGroupAssetParameters);
            if (assetGroupAssetDTOList != null)
            {
                //site level Asset finding.
                assetList = new AssetList();
                genericAssetDTOList = new List<GenericAssetDTO>();
                searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, masterSiteId.ToString()));
                searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_ID, assetGroupAssetDTOList[0].AssetId.ToString()));
                genericAssetDTOList = assetList.GetAllAssets(searchByAssetParameters);
                if (genericAssetDTOList != null)
                {
                    searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                    searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, siteId.ToString()));
                    //searchByAssetParameters.Add(new KeyValuePair< GenericAssetDTO.SearchByAssetParameters, string>( GenericAssetDTO.SearchByAssetParameters.ASSET_NAME, genericAssetDTOList[0].Name));
                    searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.MASTER_ENTITY_ID, genericAssetDTOList[0].AssetId.ToString()));
                    siteGenericAssetDTOList = assetList.GetAllAssets(searchByAssetParameters);
                    if (siteGenericAssetDTOList != null)//already exists
                    {
                        assetGroupAssetDTOList[0].AssetId = siteGenericAssetDTOList[0].AssetId;
                    }
                    else
                    {
                        log.Error("Ends-PublishAssetGroupAsset(" + assetGroupAssetId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + genericAssetDTOList[0].Name + " asset first.");
                        throw (new Exception("Publish asset entity first."));
                    }
                }
                //Finding asset group in site level
                assetGroupList = new AssetGroupList();
                assetGroupDTOList = new List<AssetGroupDTO>();
                searchByAssetGroupParameters = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
                searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, masterSiteId.ToString()));
                searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_ID, assetGroupAssetDTOList[0].AssetGroupId.ToString()));
                assetGroupDTOList = assetGroupList.GetAllAssetGroups(searchByAssetGroupParameters);
                if (assetGroupDTOList != null)
                {
                    searchByAssetGroupParameters = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
                    searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, siteId.ToString()));
                    //searchByAssetGroupParameters.Add(new KeyValuePair< AssetGroupDTO.SearchByAssetGroupParameters, string>( AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_NAME, assetGroupDTOList[0].AssetGroupName));
                    searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.MASTER_ENTITY_ID, assetGroupDTOList[0].AssetGroupId.ToString()));
                    siteAssetGroupDTOList = assetGroupList.GetAllAssetGroups(searchByAssetGroupParameters);
                    if (siteAssetGroupDTOList != null)
                    {
                        assetGroupAssetDTOList[0].AssetGroupId = siteAssetGroupDTOList[0].AssetGroupId;
                    }
                    else
                    {
                        log.Error("Ends-PublishAssetGroupAsset(" + assetGroupAssetId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + assetGroupDTOList[0].AssetGroupName + " asset group first.");
                        throw (new Exception("Publish asset group entity first."));
                    }
                }
                searchByAssetGroupAssetParameters = new List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>>();
                searchByAssetGroupAssetParameters.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.SITE_ID, siteId.ToString()));
                searchByAssetGroupAssetParameters.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ID, assetGroupAssetDTOList[0].AssetGroupId.ToString()));
                searchByAssetGroupAssetParameters.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_ID, assetGroupAssetDTOList[0].AssetId.ToString()));
                searchByAssetGroupAssetParameters.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.MASTER_ENTITY_ID, assetGroupAssetDTOList[0].AssetGroupAssetId.ToString()));
                siteAssetGroupAseetDTOList = assetGroupAssetMapperList.GetAllAssetGroupAsset(searchByAssetGroupAssetParameters);

                if (siteAssetGroupAseetDTOList != null)//already exists
                {
                    assetGroupAssetDTOList[0].AssetGroupAssetId = siteAssetGroupAseetDTOList[0].AssetGroupAssetId;
                    assetGroupAssetDTOList[0].MasterEntityId = siteAssetGroupAseetDTOList[0].MasterEntityId;
                }
                else//Not exists
                {
                    assetGroupAssetDTOList[0].MasterEntityId = assetGroupAssetDTOList[0].AssetGroupAssetId;
                    assetGroupAssetDTOList[0].AssetGroupAssetId = -1;
                }

                assetGroupAssetMapper = new AssetGroupAssetMapper(assetGroupAssetDTOList[0]);
                assetGroupAssetMapper.Save();
            }
            log.Debug("Ends-PublishAssetGroupAsset(" + assetGroupAssetId + ", " + masterSiteId + ", " + siteId + ") event.");
        }*/


        /// <summary>
        /// Function to publish the schedule
        /// </summary>
        /// <param name="scheduleId"> Schedule Id</param>
        /// <param name="masterSiteId">Master site Id</param>
        /// <param name="siteId">The site id to which you want to publish.</param>
        /*public void PublishSchedule(int scheduleId, int masterSiteId, int siteId)
        {
            log.Debug("Starts-PublishSchedule(" + scheduleId + ", " + masterSiteId + ", " + siteId + ") event.");
            int siteScheduleId = -1;
            scheduleList = new ScheduleList();
            scheduleDTOList = new List<ScheduleDTO>();
            searchByScheduleParameters = new List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>>();
            searchByScheduleParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.SITE_ID, masterSiteId.ToString()));
            searchByScheduleParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.SCHEDULE_ID, scheduleId.ToString()));
            scheduleDTOList = scheduleList.GetAllSchedule(searchByScheduleParameters);
            if (scheduleDTOList != null)
            {
                searchByScheduleParameters = new List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>>();
                searchByScheduleParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.SITE_ID, siteId.ToString()));
                searchByScheduleParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.MASTER_ENTITY_ID, scheduleDTOList[0].ScheduleId.ToString()));
                siteScheduleDTOList = scheduleList.GetAllSchedule(searchByScheduleParameters);
                if (siteScheduleDTOList != null)
                {
                    scheduleDTOList[0].ScheduleId = siteScheduleDTOList[0].ScheduleId;
                    scheduleDTOList[0].MasterEntityId = siteScheduleDTOList[0].MasterEntityId;
                }
                else//Not exists
                {
                    scheduleDTOList[0].MasterEntityId = scheduleDTOList[0].ScheduleId;
                    scheduleDTOList[0].ScheduleId = -1;
                }
                schedule = new Schedule(scheduleDTOList[0]);
                schedule.Save();
                siteScheduleId = scheduleDTOList[0].ScheduleId;
                //Site level maintenace schedule
                maintenanceScheduleList = new MaintenanceScheduleList();
                maintenanceScheduleDTOList = new List<MaintenanceScheduleDTO>();
                searchByMaintenanceScheduleParameters = new List<KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>>();
                searchByMaintenanceScheduleParameters.Add(new KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>(MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.SITE_ID, masterSiteId.ToString()));
                searchByMaintenanceScheduleParameters.Add(new KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>(MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.SCHEDULE_ID, scheduleId.ToString()));
                maintenanceScheduleDTOList = maintenanceScheduleList.GetAllMaintenanceSchedule(searchByMaintenanceScheduleParameters);
                if (maintenanceScheduleDTOList != null)
                {
                    searchByMaintenanceScheduleParameters = new List<KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>>();
                    searchByMaintenanceScheduleParameters.Add(new KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>(MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.SITE_ID, siteId.ToString()));
                    searchByMaintenanceScheduleParameters.Add(new KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>(MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.MASTER_ENTITY_ID, maintenanceScheduleDTOList[0].MaintScheduleId.ToString()));
                    siteMaintenanceScheduleDTOList = maintenanceScheduleList.GetAllMaintenanceSchedule(searchByMaintenanceScheduleParameters);
                    if (siteMaintenanceScheduleDTOList != null)
                    {
                        maintenanceScheduleDTOList[0].MaintScheduleId = siteMaintenanceScheduleDTOList[0].MaintScheduleId;
                        maintenanceScheduleDTOList[0].MasterEntityId = siteMaintenanceScheduleDTOList[0].MasterEntityId;
                        //maintenanceScheduleDTOList[0].MaxValueJobCreated = siteMaintenanceScheduleDTOList[0].MaxValueJobCreated;
                    }
                    else//Not exists
                    {

                        maintenanceScheduleDTOList[0].MasterEntityId = maintenanceScheduleDTOList[0].MaintScheduleId;
                        maintenanceScheduleDTOList[0].MaintScheduleId = -1;
                    }
                    maintenanceScheduleDTOList[0].ScheduleId = siteScheduleId;
                    usersList = new UsersList();
                    usersDTOList = new List<UsersDTO>();
                    searchByUserParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                    searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, masterSiteId.ToString()));
                    searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, maintenanceScheduleDTOList[0].UserId.ToString()));
                    usersDTOList = usersList.GetAllUsers(searchByUserParameters);
                    if (usersDTOList != null)
                    {
                        searchByUserParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId.ToString()));
                        searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.MASTER_ENTITY_ID, usersDTOList[0].UserId.ToString()));
                        siteUsersDTOList = usersList.GetAllUsers(searchByUserParameters);
                        if (siteUsersDTOList != null)
                        {
                            maintenanceScheduleDTOList[0].UserId = siteUsersDTOList[0].UserId;
                        }
                        else
                        {
                            log.Error("Ends-PublishSchedule(" + scheduleId + ", " + masterSiteId + ", " + siteId + ") event with error publish user entity first.");
                            throw (new Exception("Publish user entity first."));
                        }
                    }
                    maintenanceSchedule = new MaintenanceSchedule(maintenanceScheduleDTOList[0]);
                    maintenanceSchedule.SaveShedule();

                    //Site level maintenace schedule
                    scheduleAssetTaskList = new ScheduleAssetTaskList();
                    scheduleAssetTaskDTOList = new List<ScheduleAssetTaskDTO>();
                    searchByScheduleAssetTaskParameters = new List<KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>>();
                    searchByScheduleAssetTaskParameters.Add(new KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>(ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.SITE_ID, masterSiteId.ToString()));
                    searchByScheduleAssetTaskParameters.Add(new KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>(ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MAINT_SCHEDULE_ID, maintenanceScheduleDTOList[0].MasterEntityId.ToString()));
                    scheduleAssetTaskDTOList = scheduleAssetTaskList.GetAllScheduleAssetTasks(searchByScheduleAssetTaskParameters);
                    if (scheduleAssetTaskDTOList != null)
                    {
                        foreach (ScheduleAssetTaskDTO scheduleAssetTaskDTO in scheduleAssetTaskDTOList)
                        {
                            searchByScheduleAssetTaskParameters = new List<KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>>();
                            searchByScheduleAssetTaskParameters.Add(new KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>(ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.SITE_ID, siteId.ToString()));
                            searchByScheduleAssetTaskParameters.Add(new KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>(ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters.MASTER_ENTITY_ID, scheduleAssetTaskDTO.MaintSchAssetTaskId.ToString()));
                            siteScheduleAssetTaskDTOList = scheduleAssetTaskList.GetAllScheduleAssetTasks(searchByScheduleAssetTaskParameters);
                            if (siteScheduleAssetTaskDTOList != null)
                            {
                                scheduleAssetTaskDTO.MaintSchAssetTaskId = siteScheduleAssetTaskDTOList[0].MaintSchAssetTaskId;
                                scheduleAssetTaskDTO.MasterEntityId = siteScheduleAssetTaskDTOList[0].MasterEntityId;
                                //maintenanceScheduleDTOList[0].MaxValueJobCreated = siteMaintenanceScheduleDTOList[0].MaxValueJobCreated;
                            }
                            else//Not exists
                            {
                                scheduleAssetTaskDTO.MasterEntityId = scheduleAssetTaskDTO.MaintSchAssetTaskId;
                                scheduleAssetTaskDTO.MaintSchAssetTaskId = -1;
                            }
                            scheduleAssetTaskDTO.MaintScheduleId = maintenanceScheduleDTOList[0].MaintScheduleId;
                            if (scheduleAssetTaskDTO.AssetID != -1)
                            {
                                assetList = new AssetList();
                                searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                                searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, siteId.ToString()));
                                searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.MASTER_ENTITY_ID, scheduleAssetTaskDTO.AssetID.ToString()));
                                siteGenericAssetDTOList = assetList.GetAllAssets(searchByAssetParameters);
                                if (siteGenericAssetDTOList != null)//already exists
                                {
                                    scheduleAssetTaskDTO.AssetID = siteGenericAssetDTOList[0].AssetId;
                                }
                                else
                                {
                                    log.Error("Ends-PublishSchedule(" + scheduleId + ", " + masterSiteId + ", " + siteId + ") event with error publish asset first.");
                                    throw (new Exception("Publish asset entity first."));
                                }
                            }
                            if (scheduleAssetTaskDTO.AssetGroupId != -1)
                            {
                                assetGroupList = new AssetGroupList();
                                searchByAssetGroupParameters = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
                                searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, siteId.ToString()));
                                searchByAssetGroupParameters.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.MASTER_ENTITY_ID, scheduleAssetTaskDTO.AssetGroupId.ToString()));
                                siteAssetGroupDTOList = assetGroupList.GetAllAssetGroups(searchByAssetGroupParameters);
                                if (siteAssetGroupDTOList != null)
                                {
                                    scheduleAssetTaskDTO.AssetGroupId = siteAssetGroupDTOList[0].AssetGroupId;
                                }
                                else
                                {
                                    log.Error("Ends-PublishSchedule(" + scheduleId + ", " + masterSiteId + ", " + siteId + ") event with error publish asset group first.");
                                    throw (new Exception("Publish asset group entity first."));
                                }
                            }
                            if (scheduleAssetTaskDTO.AssetTypeId != -1)
                            {
                                assetTypeList = new AssetTypeList();
                                searchByAssetTypeParameters = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                                searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, siteId.ToString()));
                                searchByAssetTypeParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.MASTER_ENTITY_ID, scheduleAssetTaskDTO.AssetTypeId.ToString()));
                                siteAssetTypeDTOList = assetTypeList.GetAllAssetTypes(searchByAssetTypeParameters);
                                if (siteAssetTypeDTOList != null)
                                {
                                    scheduleAssetTaskDTO.AssetTypeId = siteAssetTypeDTOList[0].AssetTypeId;
                                }
                                else
                                {
                                    log.Error("Ends-PublishSchedule(" + scheduleId + ", " + masterSiteId + ", " + siteId + ") event with error publish asset group first.");
                                    throw (new Exception("Publish asset type entity first."));
                                }
                            }
                            if (scheduleAssetTaskDTO.MaintTaskId != -1)
                            {
                                maintenanceTaskList = new Parafait.MaintenanceTasks.MaintenanceTaskList();
                                searchByMaintenanceTaskParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>>();
                                searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.SITE_ID, siteId.ToString()));
                                //searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.TASK_NAME, maintenanceTaskDTOList[0].TaskName));
                                searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MASTER_ENTITY_ID, scheduleAssetTaskDTO.MaintTaskId.ToString()));
                                siteMaintenanceTaskDTOList = maintenanceTaskList.GetAllMaintenanceTasks(searchByMaintenanceTaskParameters);
                                if (siteMaintenanceTaskDTOList != null)//already exists
                                {
                                    scheduleAssetTaskDTO.MaintTaskId = siteMaintenanceTaskDTOList[0].MaintTaskId;
                                }
                                else//Not exists
                                {
                                    log.Error("Ends-PublishSchedule(" + scheduleId + ", " + masterSiteId + ", " + siteId + ") event with error publish maintenance task first.");
                                    throw (new Exception("Publish maintenance task entity first."));
                                }
                            }
                            if (scheduleAssetTaskDTO.MaintTaskGroupId != -1)
                            {
                                maintenanceTaskGroupList = new Parafait.MaintenanceTasks.MaintenanceTaskGroupList();
                                searchByMaintenanceTaskGroupParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>>();
                                searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.SITE_ID, siteId.ToString()));
                                //searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.TASK_GROUP_NAME, maintenanceTaskGroupDTOList[0].TaskGroupName));
                                searchByMaintenanceTaskGroupParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters.MASTER_ENTITY_ID, scheduleAssetTaskDTO.MaintTaskGroupId.ToString()));
                                siteMaintenanceTaskGroupDTOList = maintenanceTaskGroupList.GetAllMaintenanceTaskGroups(searchByMaintenanceTaskGroupParameters);
                                if (siteMaintenanceTaskGroupDTOList != null)
                                {
                                    scheduleAssetTaskDTO.MaintTaskGroupId = siteMaintenanceTaskGroupDTOList[0].MaintTaskGroupId;
                                }
                                else
                                {
                                    log.Error("Ends-PublishSchedule(" + scheduleId + ", " + masterSiteId + ", " + siteId + ") event with error publish maintenance task group first.");
                                    throw (new Exception("Publish maintenance task group entity first."));
                                }
                            }
                            scheduleAssetTask = new ScheduleAssetTask(scheduleAssetTaskDTO);
                            scheduleAssetTask.Save();
                        }
                    }
                }
                //Exclusion days
                if (siteScheduleId > 0)
                {
                    scheduleExclusionList = new ScheduleExclusionList();
                    scheduleExclusionDTOList = new List<ScheduleExclusionDTO>();
                    searchByScheduleExclusionParameters = new List<KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>>();
                    searchByScheduleExclusionParameters.Add(new KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>(ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SITE_ID, masterSiteId.ToString()));
                    searchByScheduleExclusionParameters.Add(new KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>(ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SCHEDULE_ID, scheduleId.ToString()));
                    scheduleExclusionDTOList = scheduleExclusionList.GetAllScheduleExclusions(searchByScheduleExclusionParameters);
                    if (scheduleExclusionDTOList != null)
                    {
                        foreach (ScheduleExclusionDTO scheduleExclusionDTO in scheduleExclusionDTOList)
                        {
                            searchByScheduleExclusionParameters = new List<KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>>();
                            searchByScheduleExclusionParameters.Add(new KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>(ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SITE_ID, siteId.ToString()));
                            searchByScheduleExclusionParameters.Add(new KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>(ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SCHEDULE_ID, siteScheduleId.ToString()));
                            searchByScheduleExclusionParameters.Add(new KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>(ScheduleExclusionDTO.SearchByScheduleExclusionParameters.MASTER_ENTITY_ID, scheduleExclusionDTO.ScheduleExclusionId.ToString()));
                            siteScheduleExclusionDTOList = scheduleExclusionList.GetAllScheduleExclusions(searchByScheduleExclusionParameters);
                            if (siteScheduleExclusionDTOList != null)
                            {
                                scheduleExclusionDTO.MasterEntityId = scheduleExclusionDTO.ScheduleExclusionId;
                                scheduleExclusionDTO.ScheduleExclusionId = siteScheduleExclusionDTOList[0].ScheduleExclusionId;
                                scheduleExclusionDTO.ScheduleId = siteScheduleId;
                            }
                            else
                            {
                                scheduleExclusionDTO.MasterEntityId = scheduleExclusionDTO.ScheduleExclusionId;
                                scheduleExclusionDTO.ScheduleExclusionId = -1;
                                scheduleExclusionDTO.ScheduleId = siteScheduleId;
                            }

                            scheduleExclusion = new ScheduleExclusion(scheduleExclusionDTO);
                            scheduleExclusion.Save();
                        }
                    }
                }
            }
            log.Debug("Ends-PublishSchedule(" + scheduleId + ", " + masterSiteId + ", " + siteId + ") event.");
        }*/

        /// <summary>
        /// Function to publish the job/Service request
        /// </summary>
        /// <param name="jobId"> job Id</param>
        /// <param name="masterSiteId">Master site Id</param>
        /// <param name="siteId">The site id to which you want to publish.</param>
        /*public void PublishMaintenanceJob(int jobId, int masterSiteId, int siteId)
        {
            log.Debug("Starts-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event.");
            maintenanceJobDTO = null;
            maintenanceJobList = new Parafait.MaintenanceTasks.MaintenanceJobList();
            maintenanceJobDTO = maintenanceJobList.GetMaintenanceJob(jobId);
            if (maintenanceJobDTO != null)
            {
                maintenanceJobDTO.MasterEntityId = maintenanceJobDTO.MaintChklstdetId;
                siteMaintenanceJobDTOList = new List<Parafait.MaintenanceTasks.MaintenanceJobDTO>();
                searchByMaintenanceJobParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>>();
                searchByMaintenanceJobParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>(Parafait.MaintenanceTasks.MaintenanceJobDTO.SearchByMaintenanceJobParameters.SITE_ID, siteId.ToString()));
                searchByMaintenanceJobParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>(Parafait.MaintenanceTasks.MaintenanceJobDTO.SearchByMaintenanceJobParameters.MASTER_ENTITY_ID, jobId.ToString()));
                siteMaintenanceJobDTOList = maintenanceJobList.GetAllMaintenanceJobs(searchByMaintenanceJobParameters, -1);
                if (siteMaintenanceJobDTOList != null)//In site if exists
                {
                    maintenanceJobDTO.MaintChklstdetId = siteMaintenanceJobDTOList[0].MaintChklstdetId;
                }
                else//In site if not exists
                {
                    maintenanceJobDTO.MaintChklstdetId = -1;
                }

                if (maintenanceJobDTO.AssetId != -1)
                {
                    assetList = new AssetList();
                    searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                    searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, siteId.ToString()));
                    searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.MASTER_ENTITY_ID, maintenanceJobDTO.AssetId.ToString()));
                    siteGenericAssetDTOList = assetList.GetAllAssets(searchByAssetParameters);
                    if (siteGenericAssetDTOList != null)//already exists
                    {
                        maintenanceJobDTO.AssetId = siteGenericAssetDTOList[0].AssetId;
                    }
                    else
                    {
                        log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish asset first.");
                        throw (new Exception("Publish asset entity first."));
                    }
                }
                if (maintenanceJobDTO.AssignedUserId != -1)
                {
                    usersList = new UsersList();
                    searchByUserParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                    searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId.ToString()));
                    searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.MASTER_ENTITY_ID, maintenanceJobDTO.AssignedUserId.ToString()));
                    siteUsersDTOList = usersList.GetAllUsers(searchByUserParameters);
                    if (siteUsersDTOList != null)
                    {
                        maintenanceJobDTO.AssignedUserId = siteUsersDTOList[0].UserId;
                    }
                    else
                    {
                        log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish user entity first.");
                        throw (new Exception("Publish user entity first."));
                    }
                }
                if (maintenanceJobDTO.CardId != -1)
                {
                    log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish card entity first.");
                    throw (new Exception("Publish card entity first."));
                }
                if (maintenanceJobDTO.MaintJobType != -1)
                {
                    //site level job type finding.
                    lookupValuesList = new LookupValuesList();
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                    searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, masterSiteId.ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, maintenanceJobDTO.MaintJobType.ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                    if (lookupValuesDTOList != null)
                    {
                        searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, lookupValuesDTOList[0].LookupValue.ToString()));
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                        siteLookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                        if (siteLookupValuesDTOList == null)
                        {
                            log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + lookupValuesDTOList[0].LookupValue + " lookup value for Lookup MAINT_JOB_TYPE first.");
                            throw (new Exception("Publish lookup entity first."));
                        }
                        else
                        {
                            maintenanceJobDTO.MaintJobType = siteLookupValuesDTOList[0].LookupValueId;
                        }
                    }
                }
                if (maintenanceJobDTO.MaintScheduleId != -1)
                {
                    maintenanceScheduleList = new MaintenanceScheduleList();
                    searchByMaintenanceScheduleParameters = new List<KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>>();
                    searchByMaintenanceScheduleParameters.Add(new KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>(MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.SITE_ID, siteId.ToString()));
                    searchByMaintenanceScheduleParameters.Add(new KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>(MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters.MASTER_ENTITY_ID, maintenanceJobDTO.MaintScheduleId.ToString()));
                    siteMaintenanceScheduleDTOList = maintenanceScheduleList.GetAllMaintenanceSchedule(searchByMaintenanceScheduleParameters);
                    if (siteMaintenanceScheduleDTOList != null)
                    {
                        maintenanceJobDTO.MaintScheduleId = siteMaintenanceScheduleDTOList[0].MaintScheduleId;
                    }
                    else
                    {
                        log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish maintenance schedule entity first.");
                        throw (new Exception("Publish maintenance schedule entity first."));
                    }
                }
                if (maintenanceJobDTO.MaintTaskId != -1)
                {
                    maintenanceTaskList = new Parafait.MaintenanceTasks.MaintenanceTaskList();
                    searchByMaintenanceTaskParameters = new List<KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>>();
                    searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.SITE_ID, siteId.ToString()));
                    searchByMaintenanceTaskParameters.Add(new KeyValuePair<Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>(Parafait.MaintenanceTasks.MaintenanceTaskDTO.SearchByMaintenanceTaskParameters.MASTER_ENTITY_ID, maintenanceJobDTO.MaintTaskId.ToString()));
                    siteMaintenanceTaskDTOList = maintenanceTaskList.GetAllMaintenanceTasks(searchByMaintenanceTaskParameters);
                    if (siteMaintenanceTaskDTOList != null)//already exists
                    {
                        maintenanceJobDTO.MaintTaskId = siteMaintenanceTaskDTOList[0].MaintTaskId;
                    }
                    else//Not exists
                    {
                        log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish maintenance task first.");
                        throw (new Exception("Publish maintenance task entity first."));
                    }
                }
                if (maintenanceJobDTO.Priority != -1)
                {
                    lookupValuesList = new LookupValuesList();
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                    searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, masterSiteId.ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, maintenanceJobDTO.Priority.ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_PRIORITY"));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                    if (lookupValuesDTOList != null)
                    {
                        searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, lookupValuesDTOList[0].LookupValue.ToString()));
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_PRIORITY"));
                        siteLookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                        if (siteLookupValuesDTOList == null)
                        {
                            log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + lookupValuesDTOList[0].LookupValue + " lookup value for Lookup MAINT_JOB_PRIORITY first.");
                            throw (new Exception("Publish lookup entity first."));
                        }
                        else
                        {
                            maintenanceJobDTO.Priority = siteLookupValuesDTOList[0].LookupValueId;
                        }
                    }
                }
                if (maintenanceJobDTO.RequestType != -1)
                {
                    lookupValuesList = new LookupValuesList();
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                    searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, masterSiteId.ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, maintenanceJobDTO.RequestType.ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_REQUEST_TYPE"));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                    if (lookupValuesDTOList != null)
                    {
                        searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, lookupValuesDTOList[0].LookupValue.ToString()));
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_REQUEST_TYPE"));
                        siteLookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                        if (siteLookupValuesDTOList == null)
                        {
                            log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + lookupValuesDTOList[0].LookupValue + " lookup value for Lookup MAINT_REQUEST_TYPE first.");
                            throw (new Exception("Publish lookup entity first."));
                        }
                        else
                        {
                            maintenanceJobDTO.RequestType = siteLookupValuesDTOList[0].LookupValueId;
                        }
                    }
                }
                if (maintenanceJobDTO.Status != -1)
                {
                    lookupValuesList = new LookupValuesList();
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                    searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, masterSiteId.ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, maintenanceJobDTO.Status.ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                    if (lookupValuesDTOList != null)
                    {
                        searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, lookupValuesDTOList[0].LookupValue.ToString()));
                        searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                        siteLookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);
                        if (siteLookupValuesDTOList == null)
                        {
                            log.Error("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event with error publish " + lookupValuesDTOList[0].LookupValue + " lookup value for Lookup MAINT_JOB_STATUS first.");
                            throw (new Exception("Publish lookup entity first."));
                        }
                        else
                        {
                            maintenanceJobDTO.Status = siteLookupValuesDTOList[0].LookupValueId;
                        }
                    }
                }
                maintenanceJob = new Parafait.MaintenanceTasks.MaintenanceJob(maintenanceJobDTO);
                maintenanceJob.Save();
            }
            log.Debug("Ends-PublishMaintenanceJob(" + jobId + ", " + masterSiteId + ", " + siteId + ") event.");
        }*/

        /// <summary>
        /// Added to publish entity list
        /// </summary>
        /// <param name="lstEntityDetails"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public bool PublishAllEntity(List<EntityDetails> lstEntityDetails, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-PublishAllEntity() Handler method");
            bool published = false;
            foreach (EntityDetails d in lstEntityDetails)
            {
                try
                {
                    this.entity = d.entityName;
                    if (!string.IsNullOrEmpty(d.activeFlagColumnName))
                    {
                        published = PublishToSite(d.publishEntityPKId, d.selectedSiteId, SQLTrx); //Publish the record
                        InActivateSiteEntity(d.entityName, d.sitePkId, d.publishEntityPKId, SQLTrx, d.activeFlagColumnName); //InAcive the site record
                    }
                    else
                    {
                        if (d.entityName.Equals("games", StringComparison.CurrentCultureIgnoreCase))
                        {
                            UpdateGameProfileAttributes("game_id", d.publishEntityPKId, d.sitePkId, d.selectedSiteId, SQLTrx);
                        }
                        if (d.entityName.Equals("game_profile", StringComparison.CurrentCultureIgnoreCase))
                        {
                            UpdateGameProfileAttributes("game_profile_id", d.publishEntityPKId, d.sitePkId, d.selectedSiteId, SQLTrx);
                        }

                        UpdateSiteEntity(d.entityName, d.sitePkId, d.publishEntityPKId, SQLTrx); //Update masterEntityId in site  
                        published = PublishToSite(d.publishEntityPKId, d.selectedSiteId, SQLTrx); // publish the record
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("ends-PublishAllEntity() Handler method");
                    throw new Exception(ex.Message);
                }

                if (!published)
                {
                    break;
                }
            }
            log.Debug("ends-PublishAllEntity() Handler method");
            return published;
        }

        /// <summary>
        /// Added to publish entity
        /// </summary>
        /// <param name="publishEntityPKId"></param>
        /// <param name="masterSiteId"></param>
        /// <param name="siteId"></param>
        public void PublishEntity(int publishEntityPKId, int masterSiteId, int siteId)
        {

            SqlConnection TrxCnn = Utilities.createConnection();
            SqlTransaction SQLTrx = TrxCnn.BeginTransaction();
            try
            {
                PublishToSite(publishEntityPKId, siteId, SQLTrx);
            }
            catch (Exception)
            {
                SQLTrx.Rollback();
                TrxCnn.Close();
                throw;
            }
            SQLTrx.Commit();
            TrxCnn.Close();
        }

        /// <summary>
        /// Added Publish one record to site 
        /// </summary>
        /// <param name="_publishEntityPKId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        public bool PublishToSite(int _publishEntityPKId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-PublishToSite() Handler method");
            bool published = false;
            try
            {
                switch (entity.ToLower())
                {
                    case "products": publishProduct(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "discounts": publishDiscount(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "games": publishgames(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "game_profile": publishgameProfile(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "promotions": publishPromotion(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "loyaltyrule": publishLoyaltyPromotion(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "user_roles": publishUserRoles(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "users": publishUsers(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    // case "cardtype": publishMembership(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    //case "MembershipRule": publishMembershipRule(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "membership": PublishMembership(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "modifierset": publishModiferSet(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "richcontent": publishRichContent(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "applicationcontent": publishAppContent(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "customattributes": publishCustomAttribute(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "messages": publishMessages(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "kioskmoneyacceptorinfo": publishKioskSetup(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "monitor": publishMonitor(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "monitorpriority": publishMonitorPriority(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "vendor": publishVendor(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "location": publishLocation(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "uom": publishUOM(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "purchasetax": publishPurchaseTax(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "redemptioncurrency": publishRedemptionCurrency(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "product": publishInventoryProduct(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "segment_definition": publishSegmentDefinition(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "segment_definition_source_mapping": publishSegmentDefinitionSourceMapping(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "category": publishCategory(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "inventorydocumenttype": publishInventoryDocumentType(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "tax": PublishTax(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "posmachines": PublishPOSMachines(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "poscounter": PublishPOSTypes(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "lookups": PublishLookups(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "paymentmode": PublishPaymentMode(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "productdisplaygroupformat": PublishProductDisplayGroupFormat(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "themes": PublishThemes(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "printer": PublishPrinter(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "printtemplate": PublishPrintTempate(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "securitypolicy": PublishSecurityPolicy(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "checkinfacility": PublishCheckInFacility(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "facilityposassignment": PublishFacilityPOSAssignment(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "attractionplays": PublishAttractionPlay(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "attractionmasterschedule": PublishAttractionMasterSchedule(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "trxprofiles": PublishTrxProfiles(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "emailtemplate": PublishEmailTemplate(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "media": PublishMedia(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "dslookup": PublishDSLookup(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "ticker": PublishTicker(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "signagepattern": PublishSignagePattern(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "event": PublishEvent(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "displaypanel": PublishDisplayPanel(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    //case "screensetup": PublishScreenSetup(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "schedule": PublishSignageSchedule(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "facilitymap": PublishFacilityMap(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "waiverset": PublishWaiverSet(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "asset": PublishAsset(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "assettype": PublishAssetType(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "assetgroup": PublishAssetGroup(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "assetgroupasset": PublishAssetGroupAsset(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "maintenancetask": PublishMaintanaceTask(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "maintenancetaskgroup": PublishMaintanaceTaskGroup(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "maintanaceschedule": PublishMaintanaceSchedule(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "maintenancejob/service": PublishMaintenanceJob(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "achievement": PublishAchievement(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "dataaccessrule": PublishDataAccessRule(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "notificationtags": PublishNotificationTags(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "notificationtagprofile": PublishNotificationTagProfile(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "notificationtagpattern": PublishNotificationTagPattern(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    case "sequence": published = false; throw new Exception(entity + " publish is not allowed.");//PublishSequence(_publishEntityPKId, siteId, SQLTrx); published = true; break;
                    default: published = false; throw new Exception(entity + " is not part of publish entity");
                }

                log.Debug("Ends-PublishToSite() Handler method");
                return published;
            }
            catch (Exception ex)
            {
                published = false;
                log.Debug("Ends-PublishToSite() Handler method");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Update the site entity details
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="pkId"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="SQLTrx"></param>
        public void UpdateSiteEntity(string entityName, int pkId, int masterEntityId, SqlTransaction SQLTrx)
        {
            log.Debug("Ends-UpdateSiteEntity(string entityName, int pkId, int masterEntityId, SqlTransaction SQLTrx) Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            try
            {
                pubEnt.UpdateSiteEntity(entityName, pkId, masterEntityId, SQLTrx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            log.Debug("Ends-UpdateSiteEntity(string entityName, int pkId, int masterEntityId, SqlTransaction SQLTrx) Handler method");
        }

        /// <summary>
        /// InActive the site entity details
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="pkId"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="activeFlagColumnName"></param>
        public void InActivateSiteEntity(string entityName, int pkId, int masterEntityId, SqlTransaction SQLTrx, string activeFlagColumnName)
        {
            log.Debug("Ends-InActivateSiteEntity(string entityName, int pkId,  int masterEntityId, SqlTransaction SQLTrx, string activeFlagColumnName) Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            try
            {
                pubEnt.InActivateSiteEntity(entityName, pkId, masterEntityId, SQLTrx, activeFlagColumnName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            log.Debug("Ends-InActivateSiteEntity(string entityName, int pkId,  int masterEntityId, SqlTransaction SQLTrx, string activeFlagColumnName) Handler method");
        }

        /// <summary>
        /// Update Gameprofile attributes for passed site
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="pkId"></param>
        /// <param name="selectedSiteId"></param>
        /// <param name="SQLTrx"></param>
        public void UpdateGameProfileAttributes(string columnName, int masterEntityId, int pkId, int selectedSiteId, SqlTransaction SQLTrx)
        {
            log.Debug("Ends-UpdateGameProfileAttributes(string columnName, int masterEntityId, int pkId, int selectedSiteId, SqlTransaction SQLTrx) Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            try
            {
                pubEnt.UpdateGameProfileAttributes(columnName, masterEntityId, pkId, selectedSiteId, SQLTrx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            log.Debug("Ends-UpdateGameProfileAttributes(string columnName, int masterEntityId, int pkId, int selectedSiteId, SqlTransaction SQLTrx) Handler method");
        }

        #region Publish Methods for all entity

        void PublishMaintanaceTaskGroup(int taskId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(taskId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity maintTaskGroup = new PublishDataHandler.Entity("Maint_TaskGroups");

            try
            {
                pubEnt.Publish(taskId, siteId, maintTaskGroup, SQLTrx);
                log.Debug("ends-PublishAssetGroup() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishMaintanaceTask(int taskId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(taskId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity maintTasks = new PublishDataHandler.Entity("Maint_Tasks");
            PublishDataHandler.Entity maintTaskGroup = new PublishDataHandler.Entity("Maint_TaskGroups", "MaintTaskGroupId");
            maintTasks.ChildList.Add(maintTaskGroup);
            try
            {
                pubEnt.Publish(taskId, siteId, maintTasks, SQLTrx);
                log.Debug("ends-PublishAssetGroup() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishAssetGroupAsset(int assetGroupAssetId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(assetGroupAssetId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity assetGroupAsset = new PublishDataHandler.Entity("Maint_AssetGroup_Assets");

            PublishDataHandler.Entity assetGroup = new PublishDataHandler.Entity("Maint_AssetGroups", "AssetGroupId");
            PublishDataHandler.Entity asset = new PublishDataHandler.Entity("Maint_Assets", "AssetId");
            PublishDataHandler.Entity assetType = new PublishDataHandler.Entity("Maint_Asset_Types", "AssetTypeId");
            asset.ChildList.Add(assetType);
            assetGroupAsset.ChildList.Add(assetGroup);
            assetGroupAsset.ChildList.Add(asset);

            try
            {
                pubEnt.Publish(assetGroupAssetId, siteId, assetGroupAsset, SQLTrx);
                log.Debug("ends-PublishAssetGroupAsset() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishAssetGroup(int groupId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(groupId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity assetGroup = new PublishDataHandler.Entity("Maint_AssetGroups");

            try
            {
                pubEnt.Publish(groupId, siteId, assetGroup, SQLTrx);
                log.Debug("ends-PublishAssetGroup() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishAssetType(int typeId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(typeId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity assetType = new PublishDataHandler.Entity("Maint_Asset_Types");

            try
            {
                pubEnt.Publish(typeId, siteId, assetType, SQLTrx);
                log.Debug("ends-PublishAssetType() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishAsset(int assetId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(assetId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity asset = new PublishDataHandler.Entity("Maint_Assets");
            PublishDataHandler.Entity assetType = new PublishDataHandler.Entity("Maint_Asset_Types", "AssetTypeId");
            asset.ChildList.Add(assetType);
            try
            {
                pubEnt.Publish(assetId, siteId, asset, SQLTrx);
                log.Debug("ends-PublishAsset() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void publishVendor(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishVendor() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity vendor = new PublishDataHandler.Entity("Vendor");

            try
            {
                pubEnt.Publish(contentId, siteId, vendor, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishVendor() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishVendor() Handler method");
        }
        void publishLocation(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishLocation() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity location = new PublishDataHandler.Entity("Location");

            try
            {
                pubEnt.Publish(contentId, siteId, location, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishLocation() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishLocation() Handler method");
        }

        void publishCategory(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishCategory() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity category = new PublishDataHandler.Entity("Category");

            try
            {
                pubEnt.Publish(contentId, siteId, category, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishCategory() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishCategory() Handler method");
        }
        void publishInventoryDocumentType(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishInventoryDocumentType() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity inventoryDocumentType = new PublishDataHandler.Entity("InventoryDocumentType");

            try
            {
                pubEnt.Publish(contentId, siteId, inventoryDocumentType, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishInventoryDocumentType() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishInventoryDocumentType() Handler method");
        }

        void publishUOM(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishUOM() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity uom = new PublishDataHandler.Entity("UOM");

            try
            {
                pubEnt.Publish(contentId, siteId, uom, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishUOM() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishUOM() Handler method");
        }

        void publishPurchaseTax(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishPurchaseTax() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity purchaseTax = new PublishDataHandler.Entity("PurchaseTax");

            try
            {
                pubEnt.Publish(contentId, siteId, purchaseTax, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishPurchaseTax() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishPurchaseTax() Handler method");
        }

        void PublishTax(int taxId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(taxId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity tax = new PublishDataHandler.Entity("Tax");
            PublishDataHandler.Entity taxStructure = new PublishDataHandler.Entity("TaxStructure", "Taxid");
            try
            {
                tax.ChildList.Add(taxStructure);
                pubEnt.Publish(taxId, siteId, tax, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishSecurityPolicy(int policyId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(policyId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity securityPolicy = new PublishDataHandler.Entity("SecurityPolicy");
            PublishDataHandler.Entity securityPolicyDetails = new PublishDataHandler.Entity("SecurityPolicyDetails", "PolicyId");
            securityPolicy.ChildList.Add(securityPolicyDetails);
            try
            {
                pubEnt.Publish(policyId, siteId, securityPolicy, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishPrintTempate(int templateId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(templateId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity receiptPrintTemplateHeader = new PublishDataHandler.Entity("ReceiptPrintTemplateHeader");
            PublishDataHandler.Entity receiptPrintTemplate = new PublishDataHandler.Entity("ReceiptPrintTemplate", "TemplateId");
            receiptPrintTemplateHeader.ChildList.Add(receiptPrintTemplate);
            try
            {
                pubEnt.Publish(templateId, siteId, receiptPrintTemplateHeader, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishPrinter(int printerId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(printerId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity printers = new PublishDataHandler.Entity("Printers");
            PublishDataHandler.Entity printerDisplayGroup = new PublishDataHandler.Entity("PrinterDisplayGroup", "PrinterId");
            PublishDataHandler.Entity printerProducts = new PublishDataHandler.Entity("PrinterProducts", "PrinterId");
            printers.ChildList.Add(printerProducts);
            printers.ChildList.Add(printerDisplayGroup);
            try
            {
                pubEnt.Publish(printerId, siteId, printers, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishThemes(int themeId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(themeId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity theme = new PublishDataHandler.Entity("Theme");
            PublishDataHandler.Entity screenTransitions = new PublishDataHandler.Entity("ScreenTransitions","ThemeId");
            theme.ChildList.Add(screenTransitions);
            try
            {
                pubEnt.Publish(themeId, siteId, theme, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishProductDisplayGroupFormat(int formatId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(formatId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity productDisplayGroupFormat = new PublishDataHandler.Entity("ProductDisplayGroupFormat");
            try
            {
                pubEnt.Publish(formatId, siteId, productDisplayGroupFormat, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishPaymentMode(int paymentModeId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(paymentModeId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity paymentModes = new PublishDataHandler.Entity("PaymentModes");

            try
            {

                pubEnt.Publish(paymentModeId, siteId, paymentModes, SQLTrx, Utilities.ExecutionContext.GetUserId());
                PaymentMode paymentMode = new PaymentMode(Utilities.ExecutionContext, paymentModeId, SQLTrx);
                if (paymentMode != null)
                {
                    string publishedsite = DBSynch.getRoamingSitesForEntity("PaymentModes", Utilities.ExecutionContext.GetSiteId(), Guid.Parse(paymentMode.GetPaymentModeDTO.Guid), SQLTrx.Connection, SQLTrx);
                    DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeId.ToString()));

                    DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchParameterscouponUsed;
                    List<DiscountCouponsDTO> discountCouponsDTOs = discountCouponsListBL.GetDiscountCouponsDTOList(searchParameters, SQLTrx);
                    List<DiscountCouponsUsedDTO> discountCouponsUsedDTOs;
                    if (discountCouponsDTOs != null)
                    {
                        foreach (DiscountCouponsDTO discountCouponsDTO in discountCouponsDTOs)
                        {
                            DBSynch.CreateRoamingData("DiscountCoupons", Guid.Parse(discountCouponsDTO.Guid), Utilities.ExecutionContext.GetSiteId(), publishedsite, Utilities.getServerTime(), SQLTrx.Connection, SQLTrx);
                            searchParameterscouponUsed = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                            searchParameterscouponUsed.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                            searchParameterscouponUsed.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.COUPON_SET_ID, discountCouponsDTO.CouponSetId.ToString()));
                            discountCouponsUsedDTOs = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchParameterscouponUsed, SQLTrx);
                            if (discountCouponsUsedDTOs != null)
                            {
                                foreach (DiscountCouponsUsedDTO discountCouponsUsedDTO in discountCouponsUsedDTOs)
                                {
                                    DBSynch.CreateRoamingData("DiscountCouponsused", Guid.Parse(discountCouponsUsedDTO.Guid), Utilities.ExecutionContext.GetSiteId(), publishedsite, Utilities.getServerTime(), SQLTrx.Connection, SQLTrx);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishLookups(int lookupId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(lookupId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity lookups = new PublishDataHandler.Entity("Lookups");
            PublishDataHandler.Entity lookupValues = new PublishDataHandler.Entity("LookupValues","LookupId");
            try
            {
                lookups.ChildList.Add(lookupValues);
                pubEnt.Publish(lookupId, siteId, lookups, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        void PublishPOSTypes(int posMachineId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(posMachineId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity posTypes = new PublishDataHandler.Entity("POSTypes");
            try
            {
                pubEnt.Publish(posMachineId, siteId, posTypes, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishPOSMachines(int posMachineId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(posMachineId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity posMachines = new PublishDataHandler.Entity("POSMachines");
            PublishDataHandler.Entity facilityPOSAssignment = new PublishDataHandler.Entity("FacilityPOSAssignment", "POSMachineId");
            PublishDataHandler.Entity custFeedbackSurveyPOSMapping = new PublishDataHandler.Entity("CustFeedbackSurveyPOSMapping", "POSMachineId");
            PublishDataHandler.Entity parafaitOptionValues = new PublishDataHandler.Entity("ParafaitOptionValues", "POSMachineId");
            PublishDataHandler.Entity posprinters = new PublishDataHandler.Entity("Posprinters", "POSMachineId");
            PublishDataHandler.Entity printers = new PublishDataHandler.Entity("Printers", "PrinterId");
            PublishDataHandler.Entity printerDisplayGroup = new PublishDataHandler.Entity("PrinterDisplayGroup", "PrinterId");
            PublishDataHandler.Entity printproducts = new PublishDataHandler.Entity("PrinterProducts", "PrinterId");
            PublishDataHandler.Entity productMenuPanelExclusion = new PublishDataHandler.Entity("ProductMenuPanelExclusion", "POSMachineId");
            //PublishDataHandler.Entity sequences = new PublishDataHandler.Entity("Sequences", "POSMachineId");


            try
            {
                posprinters.ChildList.Add(printers);
                posprinters.ChildList.Add(printerDisplayGroup);
                posprinters.ChildList.Add(printproducts);
                posMachines.ChildList.Add(facilityPOSAssignment);
                posMachines.ChildList.Add(custFeedbackSurveyPOSMapping);
                posMachines.ChildList.Add(parafaitOptionValues);
                posMachines.ChildList.Add(posprinters);
                posMachines.ChildList.Add(printproducts);
                posMachines.ChildList.Add(productMenuPanelExclusion);
                //posMachines.ChildList.Add(sequences);
                pubEnt.Publish(posMachineId, siteId, posMachines, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void publishRedemptionCurrency(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishRedemptionCurrency() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity redemptionCurrency = new PublishDataHandler.Entity("RedemptionCurrency");

            try
            {
                pubEnt.Publish(contentId, siteId, redemptionCurrency, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishRedemptionCurrency() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishRedemptionCurrency() Handler method");
        }
        void publishSegmentDefinition(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishSegmentDefinition() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity segmentDefinition = new PublishDataHandler.Entity("Segment_Definition");

            try
            {
                pubEnt.Publish(contentId, siteId, segmentDefinition, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishSegmentDefinition() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishSegmentDefinition() Handler method");
        }
        void publishSegmentDefinitionSourceMapping(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishSegmentDefinitionSourceMapping() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity segmentDefinitionSourceMapping = new PublishDataHandler.Entity("Segment_Definition_Source_Mapping");
            PublishDataHandler.Entity segmentDefinitionSourceValues = new PublishDataHandler.Entity("Segment_Definition_Source_Values");
            segmentDefinitionSourceMapping.ChildList.Add(segmentDefinitionSourceValues);

            try
            {
                pubEnt.Publish(contentId, siteId, segmentDefinitionSourceMapping, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishSegmentDefinitionSourceMapping() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishSegmentDefinitionSourceMapping() Handler method");
        }
        void publishInventoryProduct(int _publishEntityPKId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-publishInventoryProduct() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            Semnox.Parafait.Product.ProductBL productBl = new Semnox.Parafait.Product.ProductBL(_publishEntityPKId);
            if (productBl.getProductDTO == null)
            {
                throw new Exception("Unable to fetch the product.");
            }
            else if (productBl.getProductDTO.SegmentCategoryId != -1)
            {
                PublishDataHandler.Entity segmentCategorization = new PublishDataHandler.Entity("Segment_Categorization");
                PublishDataHandler.Entity segmentCategorizationVales = new PublishDataHandler.Entity("Segment_Categorization_Values");
                segmentCategorization.ChildList.Add(segmentCategorizationVales);
                pubEnt.Publish(productBl.getProductDTO.SegmentCategoryId, siteId, segmentCategorization, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            if (productBl.getProductDTO.CustomDataSetId != -1)
            {
                PublishDataHandler.Entity customDataSet = new PublishDataHandler.Entity("customDataSet");
                PublishDataHandler.Entity customData = new PublishDataHandler.Entity("customData");
                customDataSet.ChildList.Add(customData);
                pubEnt.Publish(productBl.getProductDTO.CustomDataSetId, siteId, customDataSet, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }

            PublishDataHandler.Entity product = new PublishDataHandler.Entity("Product");

            PublishDataHandler.Entity productBarCode = new PublishDataHandler.Entity("ProductBarcode");
            product.ChildList.Add(productBarCode);

            PublishDataHandler.Entity objectTranslations = new PublishDataHandler.Entity("ObjectTranslations");
            product.ChildList.Add(objectTranslations);
            try
            {
                pubEnt.Publish(_publishEntityPKId, siteId, product, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("Ends-publishInventoryProduct() Handler method");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-publishInventoryProduct() Handler method");
                throw new Exception(ex.Message);
            }
        }
        void PublishSchedule(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-PublishSchedule() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity schedule = new PublishDataHandler.Entity("Schedule");
            PublishDataHandler.Entity maintSchAssetTasks = new PublishDataHandler.Entity("Maint_SchAssetTasks");
            PublishDataHandler.Entity maintSchedule = new PublishDataHandler.Entity("Maint_Schedule");
            PublishDataHandler.Entity maintAssetGroups = new PublishDataHandler.Entity("Maint_AssetGroups");
            PublishDataHandler.Entity maintAssetType = new PublishDataHandler.Entity("Maint_Asset_Types");
            PublishDataHandler.Entity maintAssets = new PublishDataHandler.Entity("Maint_Assets");
            PublishDataHandler.Entity monitorAsset = new PublishDataHandler.Entity("MonitorAsset");
            maintSchAssetTasks.ChildList.Add(maintAssetGroups);
            maintSchAssetTasks.ChildList.Add(maintSchedule);
            maintSchAssetTasks.ChildList.Add(maintAssetType);
            maintSchAssetTasks.ChildList.Add(maintAssets);
            maintSchAssetTasks.ChildList.Add(monitorAsset);
            maintSchedule.ChildList.Add(schedule);
            PublishDataHandler.Entity users = new PublishDataHandler.Entity("Users");

            maintSchedule.ChildList.Add(users);
            schedule.ChildList.Add(maintSchedule);


            PublishDataHandler.Entity maintTasks = new PublishDataHandler.Entity("Maint_Tasks");


            PublishDataHandler.Entity scheduleExclusionDays = new PublishDataHandler.Entity("Schedule_ExclusionDays");


            try
            {
                pubEnt.Publish(contentId, siteId, schedule, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-PublishSchedule() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-PublishSchedule() Handler method");
        }

        void PublishMaintenanceJob(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(contentId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity maintjobs = new PublishDataHandler.Entity("Maint_ChecklistDetails");
            PublishDataHandler.Entity maintAssets = new PublishDataHandler.Entity("Maint_Assets", "AssetId");
            PublishDataHandler.Entity assetType = new PublishDataHandler.Entity("Maint_Asset_Types", "AssetTypeId");
            PublishDataHandler.Entity cards = new PublishDataHandler.Entity("Cards", "card_id");

            PublishDataHandler.Entity cardType = new PublishDataHandler.Entity("CardType", "CardTypeId");
            cards.ChildList.Add(cardType);
            PublishDataHandler.Entity maintSchedule = new PublishDataHandler.Entity("Maint_Schedule", "MaintScheduleId");
            PublishDataHandler.Entity department = new PublishDataHandler.Entity("Department", "DepartmentId");
            PublishDataHandler.Entity schedule = new PublishDataHandler.Entity("Schedule", "ScheduleId");
            PublishDataHandler.Entity user = new PublishDataHandler.Entity("users", "user_id");
            maintSchedule.ChildList.Add(department);
            maintSchedule.ChildList.Add(schedule);
            maintSchedule.ChildList.Add(user);
            PublishDataHandler.Entity maintTask = new PublishDataHandler.Entity("Maint_Tasks", "MaintTaskId");
            PublishDataHandler.Entity maintTaskGroup = new PublishDataHandler.Entity("Maint_TaskGroups", "MaintTaskGroupId");
            maintTask.ChildList.Add(maintTaskGroup);
            maintTask.ChildList.Add(cards);
            PublishDataHandler.Entity lookupValue = new PublishDataHandler.Entity("LookupValues", "LookupValueId");
            PublishDataHandler.Entity maintScheduleAssetTask = new PublishDataHandler.Entity("Maint_SchAssetTasks", "MaintSchAssetTaskId");
            PublishDataHandler.Entity maintAssetGroup = new PublishDataHandler.Entity("Maint_AssetGroups", "AssetGroupId");
            PublishDataHandler.Entity bookings = new PublishDataHandler.Entity("Bookings", "BookingId");
            PublishDataHandler.Entity bookingCheckList = new PublishDataHandler.Entity("BookingCheckList", "BookingCheckListId");
            maintScheduleAssetTask.ChildList.Add(maintAssetGroup);
            maintScheduleAssetTask.ChildList.Add(bookings);
            maintScheduleAssetTask.ChildList.Add(bookingCheckList);
            maintAssets.ChildList.Add(assetType);
            maintjobs.ChildList.Add(maintAssets);
            maintjobs.ChildList.Add(cards);
            maintjobs.ChildList.Add(maintSchedule);
            maintjobs.ChildList.Add(maintTask);
            maintjobs.ChildList.Add(lookupValue);
            maintjobs.ChildList.Add(maintScheduleAssetTask);


            try
            {
                pubEnt.Publish(contentId, siteId, maintAssets, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-PublishMaintenanceJob() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-PublishMaintenanceJob() Handler method");
        }
        void publishProduct(int _publishEntityPKId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(_publishEntityPKId, siteId, SQLTrx);
            //PublishDataHandler pubEnt = new PublishDataHandler();

            //PublishDataHandler.Entity products = new PublishDataHandler.Entity("Products");

            //PublishDataHandler.Entity productGames = new PublishDataHandler.Entity("ProductGames");
            //PublishDataHandler.Entity productGameExtended = new PublishDataHandler.Entity("ProductGameExtended");
            //productGames.ChildList.Add(productGameExtended);
            //products.ChildList.Add(productGames);

            //PublishDataHandler.Entity productCreditPlus = new PublishDataHandler.Entity("ProductCreditPlus");
            //PublishDataHandler.Entity productCreditPlusConsumption = new PublishDataHandler.Entity("ProductCreditPlusConsumption");
            //productCreditPlus.ChildList.Add(productCreditPlusConsumption);
            //products.ChildList.Add(productCreditPlus);

            //PublishDataHandler.Entity productCalendar = new PublitshDataHandler.Entity("ProductCalendar");
            //products.ChildList.Add(productCalendar);

            //PublishDataHandler.Entity productDiscounts = new PublishDataHandler.Entity("ProductDiscounts");
            //products.ChildList.Add(productDiscounts);

            //PublishDataHandler.Entity productModifiers = new PublishDataHandler.Entity("ProductModifiers");
            //products.ChildList.Add(productModifiers);

            //PublishDataHandler.Entity productSpecialPricing = new PublishDataHandler.Entity("ProductSpecialPricing");
            //products.ChildList.Add(productSpecialPricing);

            //PublishDataHandler.Entity upsell = new PublishDataHandler.Entity("UpsellOffers", "ProductId");
            //products.ChildList.Add(upsell);

            //PublishDataHandler.Entity comboProduct = new PublishDataHandler.Entity("ComboProduct", "Product_Id");
            //products.ChildList.Add(comboProduct);

            //PublishDataHandler.Entity cardTypeRule = new PublishDataHandler.Entity("CardTypeRule");
            //products.ChildList.Add(cardTypeRule);

            //PublishDataHandler.Entity objectTranslations = new PublishDataHandler.Entity("ObjectTranslations");
            //products.ChildList.Add(objectTranslations);

            //PublishDataHandler.Entity productsDisplayGroup = new PublishDataHandler.Entity("ProductsDisplayGroup");
            //products.ChildList.Add(productsDisplayGroup);

            //PublishDataHandler.Entity inventoryProduct = new PublishDataHandler.Entity("Product");
            //products.ChildList.Add(inventoryProduct);

            //PublishDataHandler.Entity inventoryProductBarcode = new PublishDataHandler.Entity("ProductBarcode");
            //inventoryProduct.ChildList.Add(inventoryProductBarcode);

            //PublishDataHandler.Entity segmentCategorization = new PublishDataHandler.Entity("Segment_Categorization");
            //PublishDataHandler.Entity segmentCategorizationValues = new PublishDataHandler.Entity("Segment_Categorization_Values", "SegmentCategoryId");
            //segmentCategorization.ChildList.Add(segmentCategorizationValues);
            //try
            //{
            //    Products productsObj = new Products(_publishEntityPKId);
            //    if (productsObj.GetProductsDTO != null)
            //    {
            //        if (productsObj.GetProductsDTO.SegmentCategoryId != -1)
            //        {
            //            pubEnt.Publish(productsObj.GetProductsDTO.SegmentCategoryId, siteId, segmentCategorization, SQLTrx, Utilities.ExecutionContext.GetUserId());
            //        }
            //    }
            //    pubEnt.Publish(_publishEntityPKId, siteId, products, SQLTrx, Utilities.ExecutionContext.GetUserId());
            //    log.Debug("Ends-publishProduct() Handler method");
            //}
            //catch (Exception ex)
            //{
            //    log.Debug("Ends-publishProduct() Handler method");
            //    throw new Exception(ex.Message);
            //}
            try
            {
                BatchPublish batchPublish = new BatchPublish(Utilities.ExecutionContext);
                HashSet<int> primaryKeyIdList = new HashSet<int>();
                primaryKeyIdList.Add(_publishEntityPKId);
                HashSet<int> selectedSiteIdList = new HashSet<int>();
                selectedSiteIdList.Add(siteId);
                batchPublish.Publish("Products", primaryKeyIdList, selectedSiteIdList, true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        void publishDiscount(int discountId, int siteId, SqlTransaction SQLTrx = null)
        {
            //log.Debug("Starts-publishDiscount() Handler method");
            //try
            //{
            //    PublishDataHandler pubEnt = new PublishDataHandler();

            //    DiscountsBL discountsBL = new DiscountsBL(Utilities.ExecutionContext, discountId);
            //    if (discountsBL.DiscountsDTO.ScheduleId != -1)
            //    {
            //        PublishDataHandler.Entity schedule = new PublishDataHandler.Entity("Schedule");
            //        PublishDataHandler.Entity schedule_ExclusionDays = new PublishDataHandler.Entity("Schedule_ExclusionDays");
            //        schedule.ChildList.Add(schedule_ExclusionDays);
            //        pubEnt.Publish(discountsBL.DiscountsDTO.ScheduleId, siteId, schedule, SQLTrx, Utilities.ExecutionContext.GetUserId());
            //    }

            //    PublishDataHandler.Entity discounts = new PublishDataHandler.Entity("Discounts");

            //    PublishDataHandler.Entity discountCouponsHeader = new PublishDataHandler.Entity("DiscountCouponsHeader");
            //    discounts.ChildList.Add(discountCouponsHeader);

            //    //PublishDataHandler.Entity DiscountCoupons = new PublishDataHandler.Entity("DiscountCoupons");
            //    //discounts.ChildList.Add(DiscountCoupons);

            //    PublishDataHandler.Entity DiscountedGames = new PublishDataHandler.Entity("DiscountedGames");
            //    discounts.ChildList.Add(DiscountedGames);

            //    PublishDataHandler.Entity DiscountedProducts = new PublishDataHandler.Entity("DiscountedProducts");
            //    discounts.ChildList.Add(DiscountedProducts);

            //    PublishDataHandler.Entity DiscountPurchaseCriteria = new PublishDataHandler.Entity("DiscountPurchaseCriteria");
            //    discounts.ChildList.Add(DiscountPurchaseCriteria);

            //    pubEnt.Publish(discountId, siteId, discounts, SQLTrx, Utilities.ExecutionContext.GetUserId());
            //    string publishedsite = DBSynch.getRoamingSitesForEntity("Discounts", Utilities.ExecutionContext.GetSiteId(), Guid.Parse(discountsBL.DiscountsDTO.Guid), SQLTrx.Connection, SQLTrx);
            //    DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(Utilities.ExecutionContext);
            //    List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
            //    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            //    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.DISCOUNT_ID, discountId.ToString()));

            //    DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(Utilities.ExecutionContext);
            //    List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchParameterscouponUsed;
            //    List<DiscountCouponsDTO> discountCouponsDTOs = discountCouponsListBL.GetDiscountCouponsDTOList(searchParameters, SQLTrx);
            //    List<DiscountCouponsUsedDTO> discountCouponsUsedDTOs;
            //    if (discountCouponsDTOs != null)
            //    {
            //        foreach (DiscountCouponsDTO discountCouponsDTO in discountCouponsDTOs)
            //        {
            //            DBSynch.CreateRoamingData("DiscountCoupons", Guid.Parse(discountCouponsDTO.Guid), Utilities.ExecutionContext.GetSiteId(), publishedsite, Utilities.getServerTime(), SQLTrx.Connection, SQLTrx);
            //            searchParameterscouponUsed = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
            //            searchParameterscouponUsed.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            //            searchParameterscouponUsed.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.COUPON_SET_ID, discountCouponsDTO.CouponSetId.ToString()));
            //            discountCouponsUsedDTOs = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchParameterscouponUsed, SQLTrx);
            //            if (discountCouponsUsedDTOs != null)
            //            {
            //                foreach (DiscountCouponsUsedDTO discountCouponsUsedDTO in discountCouponsUsedDTOs)
            //                {
            //                    DBSynch.CreateRoamingData("DiscountCouponsused", Guid.Parse(discountCouponsUsedDTO.Guid), Utilities.ExecutionContext.GetSiteId(), publishedsite, Utilities.getServerTime(), SQLTrx.Connection, SQLTrx);
            //                }
            //            }
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    log.Debug("ends-publishDiscount() Handler method");
            //    throw new Exception(ex.Message);
            //}
            //log.Debug("ends-publishDiscount() Handler method");
        }

        void publishModiferSet(int setId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishModiferSet() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity set = new PublishDataHandler.Entity("ModifierSet");

            PublishDataHandler.Entity setDetails = new PublishDataHandler.Entity("ModifierSetDetails");
            set.ChildList.Add(setDetails);

            try
            {
                pubEnt.Publish(setId, siteId, set, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishModiferSet() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishModiferSet() Handler method");
        }

        void publishgameProfile(int gameprofileId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishgameProfile() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity gameProfile = new PublishDataHandler.Entity("game_profile");

            PublishDataHandler.Entity gpAttributes = new PublishDataHandler.Entity("GameProfileAttributeValues");
            gameProfile.ChildList.Add(gpAttributes);

            PublishDataHandler.Entity GenericCalendar = new PublishDataHandler.Entity("GenericCalendar");
            gameProfile.ChildList.Add(GenericCalendar);

            try
            {
                pubEnt.Publish(gameprofileId, siteId, gameProfile, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishgameProfile() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishgameProfile() Handler method");
        }

        void publishgames(int gameId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishgames() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity games = new PublishDataHandler.Entity("games");

            PublishDataHandler.Entity gameAttributes = new PublishDataHandler.Entity("GameProfileAttributeValues");
            games.ChildList.Add(gameAttributes);

            PublishDataHandler.Entity gamePriceTier = new PublishDataHandler.Entity("GamePriceTier");
            games.ChildList.Add(gamePriceTier);

            try
            {
                pubEnt.Publish(gameId, siteId, games, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishgames() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishgames() Handler method");
        }

        void publishUserRoles(int roleId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishUserRoles() Handler method");

            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity userRoles = new PublishDataHandler.Entity("user_roles");

            PublishDataHandler.Entity mgmt = new PublishDataHandler.Entity("ManagementFormAccess");
            userRoles.ChildList.Add(mgmt);

            PublishDataHandler.Entity productMenuPanelExclusion = new PublishDataHandler.Entity("ProductMenuPanelExclusion", "UserRoleId");
            userRoles.ChildList.Add(productMenuPanelExclusion);

            try
            {
                pubEnt.Publish(roleId, siteId, userRoles, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishUserRoles() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishUserRoles() Handler method");
        }

        void publishUsers(int userId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishUsers() Handler method");

            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity users = new PublishDataHandler.Entity("users");

            PublishDataHandler.Entity userpwd = new PublishDataHandler.Entity("UserPasswordHistory");
            users.ChildList.Add(userpwd);

            PublishDataHandler.Entity UserIdentificationTags = new PublishDataHandler.Entity("UserIdentificationTags");
            users.ChildList.Add(UserIdentificationTags);

            try
            {
                pubEnt.Publish(userId, siteId, users, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishUsers() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishUsers() Handler method");
        }

        void publishRichContent(int contentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishRichContent() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity RichContent = new PublishDataHandler.Entity("RichContent");

            try
            {
                pubEnt.Publish(contentId, siteId, RichContent, SQLTrx, Utilities.ExecutionContext.GetUserId());
                log.Debug("ends-publishRichContent() Handler method");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishRichContent() Handler method");
        }

        void publishKioskSetup(int kioskInfoId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishKioskSetup() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity KioskMoneyAcceptorInfo = new PublishDataHandler.Entity("KioskMoneyAcceptorInfo");

            try
            {
                pubEnt.Publish(kioskInfoId, siteId, KioskMoneyAcceptorInfo, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishKioskSetup() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishKioskSetup() Handler method");
        }

        void publishCustomAttribute(int customAttributeId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishCustomAttribute() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity CustomAttributes = new PublishDataHandler.Entity("CustomAttributes");
            PublishDataHandler.Entity CustomAttributeValueList = new PublishDataHandler.Entity("CustomAttributeValueList");
            CustomAttributes.ChildList.Add(CustomAttributeValueList);

            try
            {
                pubEnt.Publish(customAttributeId, siteId, CustomAttributes, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishCustomAttribute() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishCustomAttribute() Handler method");
        }

        void publishMessages(int messageId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishMessages() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity Messages = new PublishDataHandler.Entity("Messages");
            PublishDataHandler.Entity MessagesTranslated = new PublishDataHandler.Entity("MessagesTranslated");
            Messages.ChildList.Add(MessagesTranslated);

            try
            {
                pubEnt.Publish(messageId, siteId, Messages, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishMessages() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishMessages() Handler method");
        }

        void publishMonitor(int monitorId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishMonitor() Handler method");

            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity Monitor = new PublishDataHandler.Entity("Monitor");

            try
            {
                pubEnt.Publish(monitorId, siteId, Monitor, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishMonitor() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishMonitor() Handler method");
        }

        void publishMonitorPriority(int priorityId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishMonitorPriority() Handler method");

            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity MonitorPriority = new PublishDataHandler.Entity("MonitorPriority");

            try
            {
                pubEnt.Publish(priorityId, siteId, MonitorPriority, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishMonitorPriority() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishMonitorPriority() Handler method");
        }

        void publishAppContent(int appContentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishAppContent() Handler method");

            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity ApplicationContent = new PublishDataHandler.Entity("ApplicationContent");
            PublishDataHandler.Entity ApplicationContentTranslated = new PublishDataHandler.Entity("ApplicationContentTranslated");
            ApplicationContent.ChildList.Add(ApplicationContentTranslated);

            try
            {
                pubEnt.Publish(appContentId, siteId, ApplicationContent, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishAppContent() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishAppContent() Handler method");
        }

        void publishPromotion(int promotionId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("starts-publishPromotion() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();

            PublishDataHandler.Entity promotions = new PublishDataHandler.Entity("promotions");

            PublishDataHandler.Entity promotionDetail = new PublishDataHandler.Entity("promotion_detail");
            promotions.ChildList.Add(promotionDetail);

            PublishDataHandler.Entity promotionExclusionDates = new PublishDataHandler.Entity("PromotionExclusionDates");
            promotions.ChildList.Add(promotionExclusionDates);

            PublishDataHandler.Entity promotionRule = new PublishDataHandler.Entity("PromotionRule");
            promotions.ChildList.Add(promotionRule);

            try
            {
                pubEnt.Publish(promotionId, siteId, promotions, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishPromotion() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishPromotion() Handler method");
        }

        void publishLoyaltyPromotion(int loyaltyRuleId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.Debug("Starts-publishLoyaltyPromotion() Handler method");
            PublishDataHandler pubEnt = new PublishDataHandler();

            PublishDataHandler.Entity LoyaltyRule = new PublishDataHandler.Entity("LoyaltyRule");

            PublishDataHandler.Entity LoyaltyRuleTriggers = new PublishDataHandler.Entity("LoyaltyRuleTriggers");
            LoyaltyRule.ChildList.Add(LoyaltyRuleTriggers);

            PublishDataHandler.Entity LoyaltyBonusAttributes = new PublishDataHandler.Entity("LoyaltyBonusAttributes");
            LoyaltyRule.ChildList.Add(LoyaltyBonusAttributes);

            PublishDataHandler.Entity LoyaltyBonusRewardCriteria = new PublishDataHandler.Entity("LoyaltyBonusRewardCriteria");
            LoyaltyBonusAttributes.ChildList.Add(LoyaltyBonusRewardCriteria);

            PublishDataHandler.Entity LoyaltyBonusPurchaseCriteria = new PublishDataHandler.Entity("LoyaltyBonusPurchaseCriteria");
            LoyaltyBonusAttributes.ChildList.Add(LoyaltyBonusPurchaseCriteria);

            try
            {
                pubEnt.Publish(loyaltyRuleId, siteId, LoyaltyRule, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Debug("ends-publishLoyaltyPromotion() Handler method");
                throw new Exception(ex.Message);
            }
            log.Debug("ends-publishLoyaltyPromotion() Handler method");
        }

        //void publishMembership(int CardTypeId, int siteId, SqlTransaction SQLTrx = null)
        void PublishMembership(int membershipId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(membershipId, siteId, SQLTrx);

            PublishDataHandler pubEnt = new PublishDataHandler();
            // PublishDataHandler.Entity Membership = new PublishDataHandler.Entity("Membership");
            PublishDataHandler.Entity MembershipRewards = new PublishDataHandler.Entity("MembershipRewards");
            PublishDataHandler.Entity CardTypeRule = new PublishDataHandler.Entity("CardTypeRule");
            //PublishDataHandler.Entity productGames = new PublishDataHandler.Entity("ProductGames");
            //PublishDataHandler.Entity productGameExtended = new PublishDataHandler.Entity("ProductGameExtended");
            //productGames.ChildList.Add(productGameExtended); 
            DataTable dt = pubEnt.GetCardTypeRule(membershipId, siteId, Utilities.ParafaitEnv.SiteId, SQLTrx);

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    pubEnt.Publish(dr["ID"], siteId, CardTypeRule, SQLTrx, Utilities.ExecutionContext.GetUserId());
                }
                catch (Exception ex)
                {
                    log.Debug("ends-publishMembership() Handler method");
                    throw new Exception(ex.Message);
                }
            }

            DataTable dtMembershipRewards = pubEnt.GetMembershipRewards(membershipId, siteId, Utilities.ParafaitEnv.SiteId, SQLTrx);

            foreach (DataRow dr in dtMembershipRewards.Rows)
            {
                try
                {
                    pubEnt.Publish(dr["MembershipRewardsId"], siteId, MembershipRewards, SQLTrx, Utilities.ExecutionContext.GetUserId());
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(ex.Message);
                }
            }

            //dt = pubEnt.GetProductGame(cardTypeId, siteId, Utilities.ParafaitEnv.SiteId, SQLTrx);

            //foreach (DataRow dr in dt.Rows)
            //{
            //    try
            //    {
            //        pubEnt.Publish(dr["product_game_id"], siteId, productGames, SQLTrx);
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Debug("ends-publishMembership() Handler method");
            //        throw new Exception(ex.Message);
            //    }
            //}

            log.LogMethodExit();
        }
        //void publishMembershipRule(int membershipRuleId, int siteId, SqlTransaction SQLTrx = null)
        //{
        //    PublishDataHandler pubEnt = new PublishDataHandler();
        //    PublishDataHandler.Entity MembershipRule = new PublishDataHandler.Entity("MembershipRule");
        //    pubEnt.Publish(membershipRuleId, siteId, MembershipRule, SQLTrx);
        //}

        void PublishCheckInFacility(int facilityId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(facilityId, siteId, SQLTrx);
            try
            {
                PublishDataHandler pubEnt = new PublishDataHandler();

                PublishDataHandler.Entity checkInFacility = new PublishDataHandler.Entity("CheckInFacility");
                PublishDataHandler.Entity facilityPOSAssignment = new PublishDataHandler.Entity("FacilityPOSAssignment");
                checkInFacility.ChildList.Add(facilityPOSAssignment);
                pubEnt.Publish(facilityId, siteId, checkInFacility, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.LogMethodExit("ends-PublishFacility() Handler method");
                throw new Exception(ex.Message);
            }
            log.LogMethodExit("ends-PublishFacility() Handler method");
        }

        void PublishFacilityPOSAssignment(int facilityPOSAssignmentId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(facilityPOSAssignmentId, siteId, SQLTrx);
            try
            {
                PublishDataHandler pubEnt = new PublishDataHandler();

                PublishDataHandler.Entity facilityPOSAssignment = new PublishDataHandler.Entity("FacilityPOSAssignment");
                pubEnt.Publish(facilityPOSAssignmentId, siteId, facilityPOSAssignment, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.LogMethodExit("ends-PublishFacilityPOSAssignment() Handler method");
                throw new Exception(ex.Message);
            }
            log.LogMethodExit("ends-PublishFacilityPOSAssignment() Handler method");
        }

        void PublishAttractionPlay(int attractionPlayId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(attractionPlayId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity attractionPlay = new PublishDataHandler.Entity("AttractionPlays");
            try
            {
                pubEnt.Publish(attractionPlayId, siteId, attractionPlay, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishAttractionMasterSchedule(int attractionMasterScheduleId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(attractionMasterScheduleId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity attractionMasterSchedule = new PublishDataHandler.Entity("AttractionMasterSchedule");
            PublishDataHandler.Entity attractionSchedule = new PublishDataHandler.Entity("AttractionSchedules");
            attractionMasterSchedule.ChildList.Add(attractionSchedule);
            try
            {
                pubEnt.Publish(attractionMasterScheduleId, siteId, attractionMasterSchedule, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishTrxProfiles(int trxProfileId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(trxProfileId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity trxProfile = new PublishDataHandler.Entity("TrxProfiles");
            PublishDataHandler.Entity trxProfileTaxRules = new PublishDataHandler.Entity("TrxProfileTaxRules");
            trxProfile.ChildList.Add(trxProfileTaxRules);
            try
            {
                pubEnt.Publish(trxProfileId, siteId, trxProfile, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishEmailTemplate(int emailTemplateId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(emailTemplateId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity emailTemplate = new PublishDataHandler.Entity("EmailTemplate");
            try
            {
                pubEnt.Publish(emailTemplateId, siteId, emailTemplate, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishMedia(int mediaId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(mediaId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity media = new PublishDataHandler.Entity("Media");
            try
            {
                pubEnt.Publish(mediaId, siteId, media, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishDSLookup(int dsLookupId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(dsLookupId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity dsLookUp = new PublishDataHandler.Entity("DSLookup");
            PublishDataHandler.Entity dsLookupValues = new PublishDataHandler.Entity("DSignageLookupValues");
            dsLookUp.ChildList.Add(dsLookupValues);
            try
            {
                pubEnt.Publish(dsLookupId, siteId, dsLookUp, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishTicker(int tickerId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(tickerId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity ticker = new PublishDataHandler.Entity("Ticker");
            try
            {
                pubEnt.Publish(tickerId, siteId, ticker, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishSignagePattern(int signagePatternId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(signagePatternId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity signagePattern = new PublishDataHandler.Entity("SignagePattern");
            try
            {
                pubEnt.Publish(signagePatternId, siteId, signagePattern, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishEvent(int eventId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(eventId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity dsEvent = new PublishDataHandler.Entity("Event");
            try
            {
                pubEnt.Publish(eventId, siteId, dsEvent, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishDisplayPanel(int displayPanelId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(displayPanelId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity displayPanel = new PublishDataHandler.Entity("DisplayPanel");
            try
            {
                pubEnt.Publish(displayPanelId, siteId, displayPanel, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        //void PublishScreenSetup(int screenSetupId, int siteId, SqlTransaction SQLTrx = null)
        //{
        //    log.LogMethodEntry(screenSetupId, siteId, SQLTrx);
        //    PublishDataHandler pubEnt = new PublishDataHandler();
        //    PublishDataHandler.Entity screenSetup = new PublishDataHandler.Entity("ScreenSetup");
        //    try
        //    {
        //        pubEnt.Publish(screenSetupId, siteId, screenSetup, SQLTrx);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw new Exception(ex.Message);
        //    }
        //    log.LogMethodExit();
        //}

        void PublishSignageSchedule(int scheduleId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(scheduleId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity signageSchedule = new PublishDataHandler.Entity("Schedule");
            PublishDataHandler.Entity displayPanelThemeMap = new PublishDataHandler.Entity("DisplayPanelThemeMap");
            PublishDataHandler.Entity scheduleExclusionDays = new PublishDataHandler.Entity("Schedule_ExclusionDays");
            signageSchedule.ChildList.Add(displayPanelThemeMap);
            signageSchedule.ChildList.Add(scheduleExclusionDays);
            try
            {
                pubEnt.Publish(scheduleId, siteId, signageSchedule, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishMaintanaceSchedule(int scheduleId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(scheduleId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity schedule = new PublishDataHandler.Entity("Schedule");
            PublishDataHandler.Entity maintSchedule = new PublishDataHandler.Entity("Maint_Schedule", "ScheduleId");
            PublishDataHandler.Entity department = new PublishDataHandler.Entity("Department", "DepartmentId");
            PublishDataHandler.Entity user = new PublishDataHandler.Entity("users", "user_id");
            maintSchedule.ChildList.Add(department);
            maintSchedule.ChildList.Add(user);
            PublishDataHandler.Entity schAssetTasks = new PublishDataHandler.Entity("Maint_SchAssetTasks", "MaintScheduleId");
            maintSchedule.ChildList.Add(schAssetTasks);
            PublishDataHandler.Entity assetGroup = new PublishDataHandler.Entity("Maint_AssetGroups", "AssetGroupId");
            PublishDataHandler.Entity asset = new PublishDataHandler.Entity("Maint_Assets", "AssetId");
            PublishDataHandler.Entity assetType = new PublishDataHandler.Entity("Maint_Asset_Types", "AssetTypeId");
            PublishDataHandler.Entity maintTasks = new PublishDataHandler.Entity("Maint_Tasks", "MaintTaskId");
            PublishDataHandler.Entity maintTaskGroup = new PublishDataHandler.Entity("Maint_TaskGroups", "MaintTaskGroupId");
            maintTasks.ChildList.Add(maintTaskGroup);
            schAssetTasks.ChildList.Add(maintTasks);
            schAssetTasks.ChildList.Add(maintTaskGroup);
            asset.ChildList.Add(assetType);
            schAssetTasks.ChildList.Add(assetGroup);
            schAssetTasks.ChildList.Add(asset);
            schAssetTasks.ChildList.Add(assetType);
            PublishDataHandler.Entity scheduleExclusionDays = new PublishDataHandler.Entity("Schedule_ExclusionDays", "ScheduleId");
            schedule.ChildList.Add(maintSchedule);
            schedule.ChildList.Add(scheduleExclusionDays);
            try
            {
                pubEnt.Publish(scheduleId, siteId, schedule, SQLTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        void PublishFacilityMap(int facilityMapId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(facilityMapId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity facilityMap = new PublishDataHandler.Entity("FacilityMap");
            PublishDataHandler.Entity facilityMapDetailsMap = new PublishDataHandler.Entity("FacilityMapDetails");
            facilityMap.ChildList.Add(facilityMapDetailsMap);
            try
            {
                pubEnt.Publish(facilityMapId, siteId, facilityMap, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        private void PublishWaiverSet(int waiverSetId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(waiverSetId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity waiverSet = new PublishDataHandler.Entity("WaiverSet");
            PublishDataHandler.Entity waiverSetSigningOptions = new PublishDataHandler.Entity("WaiverSetSigningOptions");
            PublishDataHandler.Entity waiverSetDetails = new PublishDataHandler.Entity("WaiverSetDetails");
            PublishDataHandler.Entity objectTranslations = new PublishDataHandler.Entity("ObjectTranslations");
            waiverSetDetails.ChildList.Add(objectTranslations);
            waiverSet.ChildList.Add(waiverSetSigningOptions);
            waiverSet.ChildList.Add(waiverSetDetails);
            try
            {
                pubEnt.Publish(waiverSetId, siteId, waiverSet, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        private void PublishAchievement(int achievementProjectId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(achievementProjectId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity achievementProject = new PublishDataHandler.Entity("AchievementProject");
            PublishDataHandler.Entity achievementClass = new PublishDataHandler.Entity("AchievementClass");
            PublishDataHandler.Entity achievementClassLevel = new PublishDataHandler.Entity("AchievementClassLevel");
            PublishDataHandler.Entity achievementScoreConversion = new PublishDataHandler.Entity("AchievementScoreConversion");
            achievementClassLevel.ChildList.Add(achievementScoreConversion);
            achievementClass.ChildList.Add(achievementClassLevel);
            achievementProject.ChildList.Add(achievementClass);
            try
            {
                pubEnt.Publish(achievementProjectId, siteId, achievementProject, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        private void PublishDataAccessRule(int dataAccessRuleId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(dataAccessRuleId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity dataAccessRule = new PublishDataHandler.Entity("DataAccessRule");
            PublishDataHandler.Entity dataAccessDetail = new PublishDataHandler.Entity("DataAccessDetail");
            PublishDataHandler.Entity entityExclusionDetail = new PublishDataHandler.Entity("EntityExclusionDetail");
            dataAccessDetail.ChildList.Add(entityExclusionDetail);
            dataAccessRule.ChildList.Add(dataAccessDetail);
            try
            {
                pubEnt.Publish(dataAccessRuleId, siteId, dataAccessRule, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        private void PublishNotificationTags(int notificationTagId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(notificationTagId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity notificationTags = new PublishDataHandler.Entity("NotificationTags");
            PublishDataHandler.Entity notificationTagStatus = new PublishDataHandler.Entity("NotificationTagStatus");
            notificationTags.ChildList.Add(notificationTagStatus);
            try
            {
                pubEnt.Publish(notificationTagId, siteId, notificationTags, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        private void PublishNotificationTagProfile(int notificationTagProfileId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(notificationTagProfileId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity notificationTagProfile = new PublishDataHandler.Entity("NotificationTagProfile");
            try
            {
                pubEnt.Publish(notificationTagProfileId, siteId, notificationTagProfile, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        private void PublishNotificationTagPattern(int notificationTagPatternId, int siteId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(notificationTagPatternId, siteId, SQLTrx);
            PublishDataHandler pubEnt = new PublishDataHandler();
            PublishDataHandler.Entity notificationTagPattern = new PublishDataHandler.Entity("NotificationTagPattern");
            try
            {
                pubEnt.Publish(notificationTagPatternId, siteId, notificationTagPattern, SQLTrx, Utilities.ExecutionContext.GetUserId());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method publishes the product list in batch publish mode for seleted sites
        /// </summary>
        /// <param name="selectedSiteIdList"></param>
        /// <param name="primaryKeyIdList"></param>
        internal void BulkEntityPublish(HashSet<int> selectedSiteIdList, HashSet<int> primaryKeyIdList, bool enableAuditLog, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(selectedSiteIdList, primaryKeyIdList, enableAuditLog, sqlTransaction);

            SqlConnection sqlConnection = null;
            SqlTransaction parafaitDBTrx;
            if (sqlTransaction == null)
            {
                sqlConnection = Utilities.createConnection();
                parafaitDBTrx = sqlConnection.BeginTransaction();
            }
            else
            {
                parafaitDBTrx = sqlTransaction;
            }
            if (primaryKeyIdList.Any() == false || selectedSiteIdList.Any() == false)
            {
                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2734); // Please choose the site
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            try
            {
                BatchPublish batchPublish = new BatchPublish(Utilities.ExecutionContext);
                batchPublish.Publish(entity, primaryKeyIdList, selectedSiteIdList, true, 20, enableAuditLog, parafaitDBTrx);
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                if (sqlTransaction == null)  //SQLTransaction handled locally
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                log.Error(ex.Message);
                throw ex;
            }
            log.LogMethodExit();
        }

        #endregion
    }
}
