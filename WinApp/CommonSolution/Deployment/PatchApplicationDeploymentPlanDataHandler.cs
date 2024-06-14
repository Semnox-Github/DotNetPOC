/********************************************************************************************
 * Project Name - Patch Application Deployment Plan Data Handler
 * Description  - Data handler of the patch application deployment plan data handler class
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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace  Semnox.Parafait.Deployment
{
    /// <summary>
    /// Patch Application Deployment Plan Data Handler - Handles insert, update and select of patch application deployment plan data objects
    /// </summary>
    public class PatchApplicationDeploymentPlanDataHandler
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters, string> DBSearchParameters = new Dictionary<PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters, string>
               {
                    {PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters.PATCH_DEPLOYMENT_PLAN_ID, "PatchDeploymentPlanId"},
                    {PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters.DEPLOYMENT_PLAN_NAME, "DeploymentPlanName"},
                    {PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters.DEPLOYMENT_PLANNED_DATE, "DeploymentPlannedDate"},
                    {PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters.DEPLOYMENT_STATUS, "DeploymentStatus"},
                    {PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters.DEPLOYMENT_VERSION, "DeploymentVersion"},
                    {PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters.MINIMUM_VERSION_REQUIRED, "MinimumVersionRequired"},
                    {PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters.PATCH_APPLICATION_TYPE_ID, "PatchApplicationTypeId"},
                    {PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters.IS_ACTIVE, "IsActive"}
               };
         DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of PatchApplicationDeploymentPlanDataHandler class
        /// </summary>
        public PatchApplicationDeploymentPlanDataHandler()
        {
            log.Debug("Starts-PatchApplicationDeploymentPlanDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-PatchApplicationDeploymentPlanDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the patch application deployment plan record to the database
        /// </summary>
        /// <param name="patchApplicationDeploymentPlan">PatchApplicationDeploymentPlanDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertPatchApplicationDeploymentPlan(PatchApplicationDeploymentPlanDTO patchApplicationDeploymentPlan, string userId, int siteId)
        {
            log.Debug("Starts-InsertPatchApplicationDeploymentPlan(patchApplicationDeploymentPlan, userId, siteId) Method.");
            string insertPatchApplicationDeploymentPlanQuery = @"insert into Patch_Application_Deployment_Plan 
                                                        (
                                                        DeploymentPlanName,
                                                        DeploymentPlannedDate,
                                                        DeploymentVersion,
                                                        MinimumVersionRequired,
                                                        PatchApplicationTypeId,
                                                        UpgradeType,
                                                        PatchFileName,
                                                        DeploymentStatus,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        SiteId,
                                                        SynchStatus
                                                        ) 
                                                values 
                                                        (
                                                        @deploymentPlanName,
                                                        @deploymentPlannedDate,
                                                        @deploymentVersion,
                                                        @minimumVersionRequired,
                                                        @patchApplicationTypeId,
                                                        @upgradeType,
                                                        @patchFileName,
                                                        @deploymentStatus,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updatePatchApplicationDeploymentPlanParameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentPlanName))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentPlanName", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentPlanName", patchApplicationDeploymentPlan.DeploymentPlanName));
            }
            if (patchApplicationDeploymentPlan.DeploymentPlannedDate.Equals(DateTime.MinValue))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentPlannedDate", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentPlannedDate", patchApplicationDeploymentPlan.DeploymentPlannedDate));
            }            
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentVersion))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentVersion", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentVersion", patchApplicationDeploymentPlan.DeploymentVersion));
            }
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.MinimumVersionRequired))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@minimumVersionRequired", DBNull.Value));
            }
            else
            {

                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@minimumVersionRequired", patchApplicationDeploymentPlan.MinimumVersionRequired));
            }
            if (patchApplicationDeploymentPlan.PatchApplicationTypeId == -1)
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchApplicationTypeId", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchApplicationTypeId", patchApplicationDeploymentPlan.PatchApplicationTypeId));
            }
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.UpgradeType))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@upgradeType", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@upgradeType", patchApplicationDeploymentPlan.UpgradeType));
            }
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.PatchFileName))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchFileName", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchFileName", patchApplicationDeploymentPlan.PatchFileName));
            }
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentStatus))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentStatus", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentStatus", patchApplicationDeploymentPlan.DeploymentStatus));
            }
            updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(patchApplicationDeploymentPlan.IsActive) ? "N" : "Y"));
            updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@createdBy", userId));
            updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@siteid", siteId));
            }
            updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@synchStatus", patchApplicationDeploymentPlan.SynchStatus));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertPatchApplicationDeploymentPlanQuery, updatePatchApplicationDeploymentPlanParameters.ToArray());
            log.Debug("Ends-InsertPatchApplicationDeploymentPlan(patchApplicationDeploymentPlan, userId, siteId) Method.");
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the patch application deployment plan record
        /// </summary>
        /// <param name="patchApplicationDeploymentPlan">PatchApplicationDeploymentPlanDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdatePatchApplicationDeploymentPlan(PatchApplicationDeploymentPlanDTO patchApplicationDeploymentPlan, string userId, int siteId)
        {
            log.Debug("Starts-UpdatePatchApplicationDeploymentPlan(patchApplicationDeploymentPlan, userId, siteId) Method.");
            string updatePatchApplicationDeploymentPlanQuery = @"update Patch_Application_Deployment_Plan 
                                         set DeploymentPlanName=@deploymentPlanName,
                                             DeploymentPlannedDate=@deploymentPlannedDate,
                                             DeploymentVersion=@deploymentVersion,
                                             MinimumVersionRequired=@minimumVersionRequired,
                                             PatchApplicationTypeId=@patchApplicationTypeId,
                                             UpgradeType=@upgradeType,
                                             PatchFileName=@patchFileName,
                                             DeploymentStatus=@deploymentStatus,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             Siteid=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where PatchDeploymentPlanId = @patchDeploymentPlanId";
            List<SqlParameter> updatePatchApplicationDeploymentPlanParameters = new List<SqlParameter>();
            updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchDeploymentPlanId", patchApplicationDeploymentPlan.PatchDeploymentPlanId));
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentPlanName))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentPlanName", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentPlanName", patchApplicationDeploymentPlan.DeploymentPlanName));
            }
            if (patchApplicationDeploymentPlan.DeploymentPlannedDate.Equals(DateTime.MinValue))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentPlannedDate", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentPlannedDate", patchApplicationDeploymentPlan.DeploymentPlannedDate));
            }            
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentVersion))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentVersion", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentVersion", patchApplicationDeploymentPlan.DeploymentVersion));
            }
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.MinimumVersionRequired))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@minimumVersionRequired", DBNull.Value));
            }
            else
            {

                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@minimumVersionRequired", patchApplicationDeploymentPlan.MinimumVersionRequired));
            }
            if (patchApplicationDeploymentPlan.PatchApplicationTypeId == -1)
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchApplicationTypeId", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchApplicationTypeId", patchApplicationDeploymentPlan.PatchApplicationTypeId));
            }
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.UpgradeType))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@upgradeType", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@upgradeType", patchApplicationDeploymentPlan.UpgradeType));
            }
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.PatchFileName))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchFileName", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@patchFileName", patchApplicationDeploymentPlan.PatchFileName));
            }
            
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentStatus))
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentStatus", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@deploymentStatus", patchApplicationDeploymentPlan.DeploymentStatus));
            }
            updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(patchApplicationDeploymentPlan.IsActive) ? "N" : "Y"));            
            updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@siteid", siteId));
            }
            updatePatchApplicationDeploymentPlanParameters.Add(new SqlParameter("@synchStatus", patchApplicationDeploymentPlan.SynchStatus));
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updatePatchApplicationDeploymentPlanQuery, updatePatchApplicationDeploymentPlanParameters.ToArray());
            log.Debug("Ends-UpdatePatchApplicationDeploymentPlan(patchApplicationDeploymentPlan, userId, siteId) Method.");
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to PatchApplicationDeploymentPlanDTO class type
        /// </summary>
        /// <param name="patchApplicationDeploymentPlanDataRow">PatchApplicationDeploymentPlanDTO DataRow</param>
        /// <returns>Returns PatchApplicationDeploymentPlanDTO</returns>
        private PatchApplicationDeploymentPlanDTO GetPatchApplicationDeploymentPlanDTO(DataRow patchApplicationDeploymentPlanDataRow)
        {
            log.Debug("Starts-GetPatchApplicationDeploymentPlanDTO(patchApplicationDeploymentPlanDataRow) Method.");
            PatchApplicationDeploymentPlanDTO patchApplicationDeploymentPlanDataObject = new PatchApplicationDeploymentPlanDTO(Convert.ToInt32(patchApplicationDeploymentPlanDataRow["PatchDeploymentPlanId"]),
                                            patchApplicationDeploymentPlanDataRow["DeploymentPlanName"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["DeploymentPlannedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchApplicationDeploymentPlanDataRow["DeploymentPlannedDate"]),
                                            patchApplicationDeploymentPlanDataRow["DeploymentVersion"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["MinimumVersionRequired"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["PatchApplicationTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(patchApplicationDeploymentPlanDataRow["PatchApplicationTypeId"]),
                                            patchApplicationDeploymentPlanDataRow["UpgradeType"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["PatchFileName"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["DeploymentStatus"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["IsActive"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["CreatedBy"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchApplicationDeploymentPlanDataRow["CreationDate"]),
                                            patchApplicationDeploymentPlanDataRow["LastUpdatedBy"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchApplicationDeploymentPlanDataRow["LastupdatedDate"]),
                                            patchApplicationDeploymentPlanDataRow["Guid"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["Siteid"] == DBNull.Value ? -1 : Convert.ToInt32(patchApplicationDeploymentPlanDataRow["Siteid"]),
                                            patchApplicationDeploymentPlanDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(patchApplicationDeploymentPlanDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetPatchApplicationDeploymentPlanDTO(patchApplicationDeploymentPlanDataRow) Method.");
            return patchApplicationDeploymentPlanDataObject;
        }

        /// <summary>
        /// Gets the patch application deployment plan data of passed patch asset application id
        /// </summary>
        /// <param name="patchApplicationDeploymentPlanId">integer type parameter</param>
        /// <returns>Returns PatchApplicationDeploymentPlanDTO</returns>
        public PatchApplicationDeploymentPlanDTO GetPatchApplicationDeploymentPlan(int patchApplicationDeploymentPlanId)
        {
            log.Debug("Starts-GetPatchApplicationDeploymentPlan(patchApplicationDeploymentPlanId) Method.");
            string selectPatchApplicationDeploymentPlanQuery = @"select *
                                         from Patch_Application_Deployment_Plan
                                        where PatchDeploymentPlanId = @patchDeploymentPlanId";
            SqlParameter[] selectPatchApplicationDeploymentPlanParameters = new SqlParameter[1];
            selectPatchApplicationDeploymentPlanParameters[0] = new SqlParameter("@patchDeploymentPlanId", patchApplicationDeploymentPlanId);
            DataTable patchApplicationDeploymentPlan = dataAccessHandler.executeSelectQuery(selectPatchApplicationDeploymentPlanQuery, selectPatchApplicationDeploymentPlanParameters);
            if (patchApplicationDeploymentPlan.Rows.Count > 0)
            {
                DataRow patchApplicationDeploymentPlanRow = patchApplicationDeploymentPlan.Rows[0];
                PatchApplicationDeploymentPlanDTO patchApplicationDeploymentPlanDataObject = GetPatchApplicationDeploymentPlanDTO(patchApplicationDeploymentPlanRow);
                log.Debug("Ends-GetPatchApplicationDeploymentPlan(patchApplicationDeploymentPlanId) Method by returnting patchApplicationDeploymentPlanDataObject.");
                return patchApplicationDeploymentPlanDataObject;
            }
            else
            {
                log.Debug("Ends-GetPatchApplicationDeploymentPlan(patchApplicationDeploymentPlanId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the PatchApplicationDeploymentPlanDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PatchApplicationDeploymentPlanDTO matching the search criteria</returns>
        public List<PatchApplicationDeploymentPlanDTO> GetPatchApplicationDeploymentPlanList(List<KeyValuePair<PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetPatchApplicationDeploymentPlanList(searchParameters) Method.");
            int count = 0;
            string selectPatchApplicationDeploymentPlanQuery = @"select *
                                         from Patch_Application_Deployment_Plan";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PatchApplicationDeploymentPlanDTO.SearchByPatchApplicationDeploymentPlanParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                            query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        else
                            query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetPatchApplicationDeploymentPlanList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectPatchApplicationDeploymentPlanQuery = selectPatchApplicationDeploymentPlanQuery + query;
            }

            DataTable patchApplicationDeploymentPlanData = dataAccessHandler.executeSelectQuery(selectPatchApplicationDeploymentPlanQuery, null);
            if (patchApplicationDeploymentPlanData.Rows.Count > 0)
            {
                List<PatchApplicationDeploymentPlanDTO> patchApplicationDeploymentPlanList = new List<PatchApplicationDeploymentPlanDTO>();
                foreach (DataRow patchApplicationDeploymentPlanDataRow in patchApplicationDeploymentPlanData.Rows)
                {
                    PatchApplicationDeploymentPlanDTO patchApplicationDeploymentPlanDataObject = GetPatchApplicationDeploymentPlanDTO(patchApplicationDeploymentPlanDataRow);
                    patchApplicationDeploymentPlanList.Add(patchApplicationDeploymentPlanDataObject);
                }
                log.Debug("Ends-GetPatchApplicationDeploymentPlanList(searchParameters) Method by returning patchApplicationDeploymentPlanList.");
                return patchApplicationDeploymentPlanList;
            }
            else
            {
                log.Debug("Ends-GetPatchApplicationDeploymentPlanList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
