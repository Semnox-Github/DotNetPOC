///********************************************************************************************
// * Project Name - MaintenanceLookUpBL
// * Description  - Created to fetch lookup values in Maintenance Module.
// *  
// **************
// **Version Log
// **************
// *Version     Date          Modified By    Remarks          
// *********************************************************************************************
// *2.70        22-May-2019   Muhammed Mehraj   Created.
// *2.80        14-May-2020   Mushahid Faizan   Modified
// ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Maintenance;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Lookups
{
    public class MaintenanceLookUpBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        string dependentDropdownName = string.Empty;
        string dependentDropdownSelectedId = string.Empty;
        string isActive = string.Empty;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private CommonLookupDTO lookupDataObject;
        public List<LookupValuesDTO> lookupValuesDTOList;
        /// <summary>
        /// Constructor for the method MaintenanceLookUpBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public MaintenanceLookUpBL(string entityName, ExecutionContext executioncontext)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }

        public enum MaintenanceEntityLookup
        {

            ASSETDETAILED,
            ASSETLIMITED,
            ASSETGROUPASSET,
            MAINTENANCETASK,
            MAINTENANCEJOBDETAILS,
            MAINTENANCEREQUESTS,
            MAINTENANCEREQUEST,
            SCHEDULECALENDER
        }
        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                string dropdownNames = "";
                string[] dropdowns = null;
                MaintenanceEntityLookup maintenanceEntityLookup = (MaintenanceEntityLookup)Enum.Parse(typeof(MaintenanceEntityLookup), entityName.ToUpper().ToString());
                switch (maintenanceEntityLookup)
                {
                    case MaintenanceEntityLookup.ASSETLIMITED:
                        dropdownNames = "ASSETTYPE,ASSETSTATUS,LOCATION";
                        break;
                    case MaintenanceEntityLookup.ASSETDETAILED:
                        dropdownNames = "ASSETTYPE,ASSETSTATUS,LOCATION,TAXTYPE";
                        break;
                    case MaintenanceEntityLookup.ASSETGROUPASSET:
                        dropdownNames = "ASSETNAME,ASSETGROUPNAME";
                        break;
                    case MaintenanceEntityLookup.MAINTENANCETASK:
                        dropdownNames = "TASKGROUPNAME";
                        break;
                    case MaintenanceEntityLookup.MAINTENANCEJOBDETAILS:
                        dropdownNames = "ASSETNAME,TASKNAME,ASSIGNEDTO,STATUS,REQUESTTYPE,PRIORITY";
                        break;
                    case MaintenanceEntityLookup.MAINTENANCEREQUESTS:
                        dropdownNames = "ASSETNAME,STATUS,REQUESTTYPE,PRIORITY,ASSIGNEDTO";
                        break;
                    case MaintenanceEntityLookup.SCHEDULECALENDER:
                        dropdownNames = "ASSIGNEDTO,TIMELOOKUP,TASKNAME,ASSETNAME,TASKGROUPNAME,ASSETTYPE,ASSETGROUPNAME,RECURFREQUENCY,RECURTYPE,DAYLOOKUP";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                foreach (string dropdownName in dropdowns)
                {
                    CommonLookupsDTO lookupDTO = new CommonLookupsDTO();
                    lookupDTO.Items = new List<CommonLookupDTO>();
                    lookupDTO.DropdownName = dropdownName;


                    if (dropdownName.ToUpper().ToString() == "ASSETTYPE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        AssetTypeList assetTypeList = new AssetTypeList(executionContext);
                        List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> assetTypeSearchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                        assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AssetTypeDTO> assetTypeListOnDisplay = assetTypeList.GetAllAssetTypes(assetTypeSearchParams);
                        if (assetTypeListOnDisplay != null && assetTypeListOnDisplay.Any())
                        {
                            foreach (AssetTypeDTO assetTypeDTO in assetTypeListOnDisplay)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(assetTypeDTO.AssetTypeId), assetTypeDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "ASSETSTATUS")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_ASSET_STATUS"));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "TAXTYPE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        TaxList tax = new TaxList(executionContext);
                        List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchByTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                        searchByTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<TaxDTO> taxListOnDisplay = tax.GetAllTaxes(searchByTaxParameters);
                        if (taxListOnDisplay != null && taxListOnDisplay.Any())
                        {
                            foreach (TaxDTO taxDTO in taxListOnDisplay)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(taxDTO.TaxId), taxDTO.TaxName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "RECURFREQUENCY")
                    {
                        List<KeyValuePair<string, string>> recurFrequecyList = new List<KeyValuePair<string, string>>();
                        recurFrequecyList.Add(new KeyValuePair<string, string>("D", "Daily"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("W", "Weekly"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("M", "Monthly"));
                        foreach (var recurFrequecy in recurFrequecyList)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(recurFrequecy.Key), recurFrequecy.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RECURTYPE")
                    {
                        List<KeyValuePair<string, string>> recurFrequecyList = new List<KeyValuePair<string, string>>();
                        recurFrequecyList.Add(new KeyValuePair<string, string>("D", "Day"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("W", "Weekday"));
                        foreach (var recurFrequecy in recurFrequecyList)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(recurFrequecy.Key), recurFrequecy.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LOCATION")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        AssetLocation assetLocation = new AssetLocation();
                        List<AssetLocationDTO> assetLocationListOnDisplay = assetLocation.GetLocation();
                        if (assetLocationListOnDisplay != null && assetLocationListOnDisplay.Any())
                        {
                            foreach (AssetLocationDTO assetLocationDTO in assetLocationListOnDisplay)
                            {
                                lookupDataObject = new CommonLookupDTO(assetLocationDTO.Location, assetLocationDTO.Location);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ASSETNAME")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        AssetList assetList = new AssetList(executionContext);
                        List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                        searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<GenericAssetDTO> assetListOnDisplay = assetList.GetAllAssets(searchByAssetParameters);
                        if (assetListOnDisplay != null && assetListOnDisplay.Any())
                        {
                            foreach (GenericAssetDTO genericAssetDTO in assetListOnDisplay)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(genericAssetDTO.AssetId), genericAssetDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ASSETGROUPNAME")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        AssetGroupList assetGroupList = new AssetGroupList(executionContext);
                        List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> assetGroupSearchParams = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
                        assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AssetGroupDTO> assetGroupListOnDisplay = assetGroupList.GetAllAssetGroups(assetGroupSearchParams);
                        if (assetGroupListOnDisplay != null && assetGroupListOnDisplay.Any())
                        {
                            foreach (AssetGroupDTO assetGroupDTO in assetGroupListOnDisplay)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(assetGroupDTO.AssetGroupId), assetGroupDTO.AssetGroupName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TASKGROUPNAME")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "None");//to load Default Value (i.e., None)
                        lookupDTO.Items.Add(lookupDataObject);
                        JobTaskGroupList maintenanceTaskGroupList = new JobTaskGroupList(executionContext);
                        List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> maintenanceTaskGroupSearchParams = new List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>>();
                        maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<JobTaskGroupDTO> maintenanceTaskGroupListOnDisplay = maintenanceTaskGroupList.GetAllJobTaskGroups(maintenanceTaskGroupSearchParams);
                        if (maintenanceTaskGroupListOnDisplay != null && maintenanceTaskGroupListOnDisplay.Any())
                        {
                            foreach (JobTaskGroupDTO maintenanceTaskGroupDTO in maintenanceTaskGroupListOnDisplay)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(maintenanceTaskGroupDTO.JobTaskGroupId), maintenanceTaskGroupDTO.TaskGroupName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TASKNAME")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        JobTaskList maintenanceTaskList = new JobTaskList(executionContext);
                        List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> searchByMaintenanceTaskParameters = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
                        searchByMaintenanceTaskParameters.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<JobTaskDTO> maintenanceTaskListOnDisplay = maintenanceTaskList.GetAllJobTasks(searchByMaintenanceTaskParameters);
                        if (maintenanceTaskListOnDisplay != null && maintenanceTaskListOnDisplay.Any())
                        {
                            foreach (JobTaskDTO maintenanceTaskDTO in maintenanceTaskListOnDisplay)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(maintenanceTaskDTO.JobTaskId), maintenanceTaskDTO.TaskName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ASSIGNEDTO")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        UsersList usersList = new UsersList(executionContext);
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                        usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UsersDTO> usersDTOList = usersList.GetAllUsers(usersSearchParams);
                        if (usersDTOList != null & usersDTOList.Any())
                        {
                            foreach (UsersDTO usersDTO in usersDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.UserName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "STATUS")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "REQUESTTYPE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_REQUEST_TYPE"));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> jobTypeDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (jobTypeDTOList != null && jobTypeDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in jobTypeDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRIORITY")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_PRIORITY"));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> jobTypeDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (jobTypeDTOList != null && jobTypeDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in jobTypeDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TIMELOOKUP")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        string time;
                        int hour;
                        int mins;
                        string ampm;
                        for (int i = 0; i < 48; i++)
                        {
                            hour = i / 2;
                            mins = (i % 2) * 30;

                            if (hour >= 12)
                                ampm = "PM";
                            else
                                ampm = "AM";

                            if (hour == 0)
                                hour = 12;
                            if (hour > 12)
                                hour = hour - 12;

                            time = hour.ToString() + ":" + mins.ToString().PadLeft(2, '0') + " " + ampm;
                            lookupDataObject = new CommonLookupDTO(time, time);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DAYLOOKUP")
                    {
                        List<KeyValuePair<string, string>> weekDays = new List<KeyValuePair<string, string>>();
                        weekDays.Add(new KeyValuePair<string, string>("-1", "<SELECT>"));
                        weekDays.Add(new KeyValuePair<string, string>("1", "Sunday"));
                        weekDays.Add(new KeyValuePair<string, string>("2", "Monday"));
                        weekDays.Add(new KeyValuePair<string, string>("3", "Tuesday"));
                        weekDays.Add(new KeyValuePair<string, string>("4", "Wednesday"));
                        weekDays.Add(new KeyValuePair<string, string>("5", "Thursday"));
                        weekDays.Add(new KeyValuePair<string, string>("6", "Friday"));
                        weekDays.Add(new KeyValuePair<string, string>("7", "Saturday"));
                        foreach (var week in weekDays)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(week.Key), week.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    lookups.Add(lookupDTO);
                }
                return lookups;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}