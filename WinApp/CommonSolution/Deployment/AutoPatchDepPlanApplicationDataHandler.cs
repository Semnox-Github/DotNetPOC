/********************************************************************************************
 * Project Name - Auto Patch Deployment Plan Application Data Handler
 * Description  - Data handler of the auto patch deployment plan application data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        03-Mar-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Deployment
{

    /// <summary>
    /// Patch Application Deployment Plan Data Handler - Handles insert, update and select of patch application deployment plan data objects
    /// </summary>
    public class AutoPatchDepPlanApplicationDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string> DBSearchParameters = new Dictionary<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>
               {
                    {AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_APPLICATION_ID, "PatchDeploymentPlanApplicationId"},
                    {AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_ID, "PatchDeploymentPlanId"},
                    {AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.DEPLOYMENT_VERSION, "DeploymentVersion"},
                    {AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.MINIMUM_VERSION_REQUIRED, "MinimumVersionRequired"},
                    {AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_APPLICATION_TYPE_ID, "PatchApplicationTypeId"},
                    {AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.IS_ACTIVE, "IsActive"},
                    {AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.SITE_ID,"site_id"},
                    {AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.MASTER_ENTITY_ID,"MasterEntityId"}
               };
        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of AutoPatchDepPlanApplicationDataHandler class
        /// </summary>
        public AutoPatchDepPlanApplicationDataHandler()
        {
            log.Debug("Starts-AutoPatchDepPlanApplicationDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-AutoPatchDepPlanApplicationDataHandler() default constructor.");
        }
        /// <summary>
        /// Inserts the patch application deployment plan record to the database
        /// </summary>
        /// <param name="autoPatchDepPlanApplication">AutoPatchDepPlanApplicationDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertAutoPatchDepPlanApplication(AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplication, string userId, int siteId)
        {
            log.Debug("Starts-InsertAutoPatchDepPlan(autoPatchDepPlanApplication, userId, siteId) Method.");
            string insertAutoPatchDepPlanQuery = @"insert into Patch_Deployment_Plan_Application 
                                                        (
                                                        PatchDeploymentPlanId,                                                        
                                                        DeploymentVersion,
                                                        MinimumVersionRequired,
                                                        PatchApplicationTypeId,
                                                        UpgradeType,
                                                        MasterEntityId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id,
                                                        SynchStatus
                                                        ) 
                                                values 
                                                        (
                                                        @patchDeploymentPlanId,
                                                        @deploymentVersion,
                                                        @minimumVersionRequired,
                                                        @patchApplicationTypeId,
                                                        @upgradeType,
                                                        @masterEntityId,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateAutoPatchDepPlanParameters = new List<SqlParameter>();
            if (autoPatchDepPlanApplication.PatchDeploymentPlanId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchDeploymentPlanId", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchDeploymentPlanId", autoPatchDepPlanApplication.PatchDeploymentPlanId));
            }
            if (string.IsNullOrEmpty(autoPatchDepPlanApplication.DeploymentVersion))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentVersion", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentVersion", autoPatchDepPlanApplication.DeploymentVersion));
            }
            if (string.IsNullOrEmpty(autoPatchDepPlanApplication.MinimumVersionRequired))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@minimumVersionRequired", DBNull.Value));
            }
            else
            {

                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@minimumVersionRequired", autoPatchDepPlanApplication.MinimumVersionRequired));
            }
            if (autoPatchDepPlanApplication.PatchApplicationTypeId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchApplicationTypeId", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchApplicationTypeId", autoPatchDepPlanApplication.PatchApplicationTypeId));
            }
            if (string.IsNullOrEmpty(autoPatchDepPlanApplication.UpgradeType))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@upgradeType", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@upgradeType", autoPatchDepPlanApplication.UpgradeType));
            }
            if (autoPatchDepPlanApplication.MasterEntityId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@masterEntityId", autoPatchDepPlanApplication.MasterEntityId));
            }
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(autoPatchDepPlanApplication.IsActive) ? "N" : autoPatchDepPlanApplication.IsActive));
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@createdBy", userId));
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (autoPatchDepPlanApplication.SynchStatus)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@synchStatus", autoPatchDepPlanApplication.SynchStatus));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertAutoPatchDepPlanQuery, updateAutoPatchDepPlanParameters.ToArray());
            log.Debug("Ends-InsertAutoPatchDepPlan(autoPatchDepPlanApplication, userId, siteId) Method.");
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the patch application deployment plan record
        /// </summary>
        /// <param name="autoPatchDepPlanApplication">AutoPatchDepPlanApplicationDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateAutoPatchDepPlanApplication(AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplication, string userId, int siteId)
        {
            log.Debug("Starts-UpdateAutoPatchDepPlan(autoPatchDepPlanApplication, userId, siteId) Method.");
            string updateAutoPatchDepPlanQuery = @"update Patch_Deployment_Plan_Application 
                                         set PatchDeploymentPlanId=@patchDeploymentPlanId,
                                             DeploymentVersion=@deploymentVersion,
                                             MinimumVersionRequired=@minimumVersionRequired,
                                             PatchApplicationTypeId=@patchApplicationTypeId,
                                             UpgradeType=@upgradeType,
                                             MasterEntityId=@masterEntityId,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             site_id=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where PatchDeploymentPlanApplicationId = @patchDeploymentPlanApplicationId";
            List<SqlParameter> updateAutoPatchDepPlanParameters = new List<SqlParameter>();
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchDeploymentPlanApplicationId", autoPatchDepPlanApplication.PatchDeploymentPlanApplicationId));
            if (autoPatchDepPlanApplication.PatchDeploymentPlanId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchDeploymentPlanId", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchDeploymentPlanId", autoPatchDepPlanApplication.PatchDeploymentPlanId));
            }
            if (string.IsNullOrEmpty(autoPatchDepPlanApplication.DeploymentVersion))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentVersion", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentVersion", autoPatchDepPlanApplication.DeploymentVersion));
            }
            if (string.IsNullOrEmpty(autoPatchDepPlanApplication.MinimumVersionRequired))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@minimumVersionRequired", DBNull.Value));
            }
            else
            {

                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@minimumVersionRequired", autoPatchDepPlanApplication.MinimumVersionRequired));
            }
            if (autoPatchDepPlanApplication.PatchApplicationTypeId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchApplicationTypeId", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchApplicationTypeId", autoPatchDepPlanApplication.PatchApplicationTypeId));
            }
            if (string.IsNullOrEmpty(autoPatchDepPlanApplication.UpgradeType))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@upgradeType", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@upgradeType", autoPatchDepPlanApplication.UpgradeType));
            }
            if (autoPatchDepPlanApplication.MasterEntityId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@masterEntityId", autoPatchDepPlanApplication.MasterEntityId));
            }
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(autoPatchDepPlanApplication.IsActive) ? "N" : autoPatchDepPlanApplication.IsActive));
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@lastUpdatedBy", userId));

            if (siteId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (autoPatchDepPlanApplication.SynchStatus)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@synchStatus", autoPatchDepPlanApplication.SynchStatus));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateAutoPatchDepPlanQuery, updateAutoPatchDepPlanParameters.ToArray());
            log.Debug("Ends-UpdateAutoPatchDepPlan(autoPatchDepPlanApplication, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to AutoPatchDepPlanApplicationDTO class type
        /// </summary>
        /// <param name="autoPatchDepPlanApplicationDataRow">AutoPatchDepPlanApplicationDTO DataRow</param>
        /// <returns>Returns AutoPatchDepPlanApplicationDTO</returns>
        private AutoPatchDepPlanApplicationDTO GetAutoPatchDepPlanApplicationDTO(DataRow autoPatchDepPlanApplicationDataRow)
        {
            log.Debug("Starts-GetAutoPatchDepPlanApplicationDTO(autoPatchDepPlanApplicationDataRow) Method.");
            AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplicationDataObject = new AutoPatchDepPlanApplicationDTO(Convert.ToInt32(autoPatchDepPlanApplicationDataRow["PatchDeploymentPlanApplicationId"]),
                                            autoPatchDepPlanApplicationDataRow["PatchDeploymentPlanId"] == DBNull.Value ? -1 : Convert.ToInt32(autoPatchDepPlanApplicationDataRow["PatchDeploymentPlanId"]),
                                            autoPatchDepPlanApplicationDataRow["DeploymentVersion"].ToString(),
                                            autoPatchDepPlanApplicationDataRow["MinimumVersionRequired"].ToString(),
                                            autoPatchDepPlanApplicationDataRow["PatchApplicationTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(autoPatchDepPlanApplicationDataRow["PatchApplicationTypeId"]),
                                            autoPatchDepPlanApplicationDataRow["UpgradeType"].ToString(),
                                            autoPatchDepPlanApplicationDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(autoPatchDepPlanApplicationDataRow["MasterEntityId"]),
                                            autoPatchDepPlanApplicationDataRow["IsActive"].ToString(),
                                            autoPatchDepPlanApplicationDataRow["CreatedBy"].ToString(),
                                            autoPatchDepPlanApplicationDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(autoPatchDepPlanApplicationDataRow["CreationDate"]),
                                            autoPatchDepPlanApplicationDataRow["LastUpdatedBy"].ToString(),
                                            autoPatchDepPlanApplicationDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(autoPatchDepPlanApplicationDataRow["LastupdatedDate"]),
                                            autoPatchDepPlanApplicationDataRow["Guid"].ToString(),
                                            autoPatchDepPlanApplicationDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(autoPatchDepPlanApplicationDataRow["site_id"]),
                                            autoPatchDepPlanApplicationDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(autoPatchDepPlanApplicationDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetAutoPatchDepPlanApplicationDTO(autoPatchDepPlanApplicationDataRow) Method.");
            return autoPatchDepPlanApplicationDataObject;
        }
        /// <summary>
        /// Gets the patch application deployment plan data of passed patch asset application id
        /// </summary>
        /// <param name="autoPatchDepPlanApplicationId">integer type parameter</param>
        /// <returns>Returns AutoPatchDepPlanApplicationDTO</returns>
        public AutoPatchDepPlanApplicationDTO GetAutoPatchDepPlan(int autoPatchDepPlanApplicationId)
        {
            log.Debug("Starts-GetAutoPatchDepPlan(autoPatchDepPlanApplicationId) Method.");
            string selectAutoPatchDepPlanQuery = @"select *
                                         from Patch_Deployment_Plan_Application
                                        where PatchDeploymentPlanId = @patchDeploymentPlanId";
            SqlParameter[] selectAutoPatchDepPlanParameters = new SqlParameter[1];
            selectAutoPatchDepPlanParameters[0] = new SqlParameter("@patchDeploymentPlanId", autoPatchDepPlanApplicationId);
            DataTable autoPatchDepPlanApplication = dataAccessHandler.executeSelectQuery(selectAutoPatchDepPlanQuery, selectAutoPatchDepPlanParameters);
            if (autoPatchDepPlanApplication.Rows.Count > 0)
            {
                DataRow autoPatchDepPlanApplicationRow = autoPatchDepPlanApplication.Rows[0];
                AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplicationDataObject = GetAutoPatchDepPlanApplicationDTO(autoPatchDepPlanApplicationRow);
                log.Debug("Ends-GetAutoPatchDepPlan(autoPatchDepPlanApplicationId) Method by returnting autoPatchDepPlanApplicationDataObject.");
                return autoPatchDepPlanApplicationDataObject;
            }
            else
            {
                log.Debug("Ends-GetAutoPatchDepPlan(autoPatchDepPlanApplicationId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the AutoPatchDepPlanApplicationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AutoPatchDepPlanApplicationDTO matching the search criteria</returns>
        public List<AutoPatchDepPlanApplicationDTO> AutoPatchDepPlanApplicationList(List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>> searchParameters)
        {
            log.Debug("Starts-AutoPatchDepPlanApplicationList(searchParameters) Method.");
            int count = 0;
            string selectAutoPatchDepPlanQuery = @"select *
                                         from Patch_Deployment_Plan_Application";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.MINIMUM_VERSION_REQUIRED))
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') " + "<='" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.DEPLOYMENT_VERSION))
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') " + ">'" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.SITE_ID))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_APPLICATION_TYPE_ID) || searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_APPLICATION_ID) || searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_ID) || searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(" " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.MINIMUM_VERSION_REQUIRED))
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') " + "<='" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.DEPLOYMENT_VERSION))
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') " + ">'" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_APPLICATION_TYPE_ID) || searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_APPLICATION_ID) || searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_ID) || searchParameter.Key.Equals(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-AutoPatchDepPlanApplicationList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                query.Append(" Order by PatchApplicationTypeId,DeploymentVersion ASC");
                if (searchParameters.Count > 0)
                    selectAutoPatchDepPlanQuery = selectAutoPatchDepPlanQuery + query;
            }

            DataTable autoPatchDepPlanApplicationData = dataAccessHandler.executeSelectQuery(selectAutoPatchDepPlanQuery, null);
            if (autoPatchDepPlanApplicationData.Rows.Count > 0)
            {
                List<AutoPatchDepPlanApplicationDTO> autoPatchDepPlanApplicationList = new List<AutoPatchDepPlanApplicationDTO>();
                foreach (DataRow autoPatchDepPlanApplicationDataRow in autoPatchDepPlanApplicationData.Rows)
                {
                    AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplicationDataObject = GetAutoPatchDepPlanApplicationDTO(autoPatchDepPlanApplicationDataRow);
                    autoPatchDepPlanApplicationList.Add(autoPatchDepPlanApplicationDataObject);
                }
                log.Debug("Ends-AutoPatchDepPlanApplicationList(searchParameters) Method by returning autoPatchDepPlanApplicationList.");
                return autoPatchDepPlanApplicationList;
            }
            else
            {
                log.Debug("Ends-AutoPatchDepPlanApplicationList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
