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
    public class AutoPatchDepPlanDataHandler
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string> DBSearchParameters = new Dictionary<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>
               {
                    {AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.PATCH_DEPLOYMENT_PLAN_ID, "PatchDeploymentPlanId"},
                    {AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.DEPLOYMENT_PLAN_NAME, "DeploymentPlanName"},
                    {AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.DEPLOYMENT_PLANNED_DATE, "DeploymentPlannedDate"},
                    {AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.DEPLOYMENT_STATUS, "DeploymentStatus"},
                    {AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.IS_ACTIVE, "IsActive"},
                    {AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.SITE_ID,"site_id"},
                    {AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.MASTER_ENTITY_ID,"MasterEntityId"},
               };
         DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of AutoPatchDepPlanDataHandler class
        /// </summary>
        public AutoPatchDepPlanDataHandler()
        {
            log.Debug("Starts-AutoPatchDepPlanDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-AutoPatchDepPlanDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the patch application deployment plan record to the database
        /// </summary>
        /// <param name="patchApplicationDeploymentPlan">AutoPatchDepPlanDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertAutoPatchDepPlan(AutoPatchDepPlanDTO patchApplicationDeploymentPlan, string userId, int siteId)
        {
            log.Debug("Starts-InsertAutoPatchDepPlan(patchApplicationDeploymentPlan, userId, siteId) Method.");
            string insertAutoPatchDepPlanQuery = @"insert into Patch_Deployment_Plan 
                                                        (
                                                        DeploymentPlanName,
                                                        DeploymentPlannedDate,
                                                        PatchFileName,
                                                        DeploymentStatus,
                                                        MasterEntityId,
                                                        IsReady,
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
                                                        @deploymentPlanName,
                                                        @deploymentPlannedDate,
                                                        @patchFileName,
                                                        @deploymentStatus,
                                                        @masterEntityId,
                                                        @isReady,
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
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentPlanName))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentPlanName", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentPlanName", patchApplicationDeploymentPlan.DeploymentPlanName));
            }
            if (patchApplicationDeploymentPlan.DeploymentPlannedDate.Equals(DateTime.MinValue))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentPlannedDate", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentPlannedDate", patchApplicationDeploymentPlan.DeploymentPlannedDate));
            }            
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.PatchFileName))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchFileName", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchFileName", patchApplicationDeploymentPlan.PatchFileName));
            }
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentStatus))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentStatus", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentStatus", patchApplicationDeploymentPlan.DeploymentStatus));
            }
            if (patchApplicationDeploymentPlan.MasterEntityId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@masterEntityId", patchApplicationDeploymentPlan.MasterEntityId));
            }
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@isReady", patchApplicationDeploymentPlan.IsReady));
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(patchApplicationDeploymentPlan.IsActive) ? "N" : patchApplicationDeploymentPlan.IsActive));
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
            if (patchApplicationDeploymentPlan.SynchStatus)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@synchStatus", patchApplicationDeploymentPlan.SynchStatus));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertAutoPatchDepPlanQuery, updateAutoPatchDepPlanParameters.ToArray());
            log.Debug("Ends-InsertAutoPatchDepPlan(patchApplicationDeploymentPlan, userId, siteId) Method.");
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the patch application deployment plan record
        /// </summary>
        /// <param name="patchApplicationDeploymentPlan">AutoPatchDepPlanDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateAutoPatchDepPlan(AutoPatchDepPlanDTO patchApplicationDeploymentPlan, string userId, int siteId)
        {
            log.Debug("Starts-UpdateAutoPatchDepPlan(patchApplicationDeploymentPlan, userId, siteId) Method.");
            string updateAutoPatchDepPlanQuery = @"update Patch_Deployment_Plan 
                                         set DeploymentPlanName=@deploymentPlanName,
                                             DeploymentPlannedDate=@deploymentPlannedDate,
                                             PatchFileName=@patchFileName,
                                             DeploymentStatus=@deploymentStatus,
                                             MasterEntityId=@masterEntityId,
                                             IsReady=@isReady,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             site_id=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where PatchDeploymentPlanId = @patchDeploymentPlanId";
            List<SqlParameter> updateAutoPatchDepPlanParameters = new List<SqlParameter>();
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchDeploymentPlanId", patchApplicationDeploymentPlan.PatchDeploymentPlanId));
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentPlanName))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentPlanName", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentPlanName", patchApplicationDeploymentPlan.DeploymentPlanName));
            }
            if (patchApplicationDeploymentPlan.DeploymentPlannedDate.Equals(DateTime.MinValue))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentPlannedDate", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentPlannedDate", patchApplicationDeploymentPlan.DeploymentPlannedDate));
            }            
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.PatchFileName))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchFileName", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@patchFileName", patchApplicationDeploymentPlan.PatchFileName));
            }
            
            if (string.IsNullOrEmpty(patchApplicationDeploymentPlan.DeploymentStatus))
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentStatus", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@deploymentStatus", patchApplicationDeploymentPlan.DeploymentStatus));
            }
            if (patchApplicationDeploymentPlan.MasterEntityId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@masterEntityId", patchApplicationDeploymentPlan.MasterEntityId));
            }
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@isReady", patchApplicationDeploymentPlan.IsReady));
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(patchApplicationDeploymentPlan.IsActive) ? "N" : patchApplicationDeploymentPlan.IsActive));            
            updateAutoPatchDepPlanParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
           
            if (siteId == -1)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (patchApplicationDeploymentPlan.SynchStatus)
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@synchStatus", patchApplicationDeploymentPlan.SynchStatus));
            }
            else
            {
                updateAutoPatchDepPlanParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateAutoPatchDepPlanQuery, updateAutoPatchDepPlanParameters.ToArray());
            log.Debug("Ends-UpdateAutoPatchDepPlan(patchApplicationDeploymentPlan, userId, siteId) Method.");
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to AutoPatchDepPlanDTO class type
        /// </summary>
        /// <param name="patchApplicationDeploymentPlanDataRow">AutoPatchDepPlanDTO DataRow</param>
        /// <returns>Returns AutoPatchDepPlanDTO</returns>
        private AutoPatchDepPlanDTO GetAutoPatchDepPlanDTO(DataRow patchApplicationDeploymentPlanDataRow)
        {
            log.Debug("Starts-GetAutoPatchDepPlanDTO(patchApplicationDeploymentPlanDataRow) Method.");
            AutoPatchDepPlanDTO patchApplicationDeploymentPlanDataObject = new AutoPatchDepPlanDTO(Convert.ToInt32(patchApplicationDeploymentPlanDataRow["PatchDeploymentPlanId"]),
                                            patchApplicationDeploymentPlanDataRow["DeploymentPlanName"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["DeploymentPlannedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchApplicationDeploymentPlanDataRow["DeploymentPlannedDate"]),
                                            patchApplicationDeploymentPlanDataRow["PatchFileName"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["DeploymentStatus"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(patchApplicationDeploymentPlanDataRow["MasterEntityId"]),
                                            patchApplicationDeploymentPlanDataRow["IsReady"] == DBNull.Value ? false : Convert.ToBoolean(patchApplicationDeploymentPlanDataRow["IsReady"]),
                                            patchApplicationDeploymentPlanDataRow["IsActive"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["CreatedBy"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchApplicationDeploymentPlanDataRow["CreationDate"]),
                                            patchApplicationDeploymentPlanDataRow["LastUpdatedBy"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchApplicationDeploymentPlanDataRow["LastupdatedDate"]),
                                            patchApplicationDeploymentPlanDataRow["Guid"].ToString(),
                                            patchApplicationDeploymentPlanDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(patchApplicationDeploymentPlanDataRow["site_id"]),
                                            patchApplicationDeploymentPlanDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(patchApplicationDeploymentPlanDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetAutoPatchDepPlanDTO(patchApplicationDeploymentPlanDataRow) Method.");
            return patchApplicationDeploymentPlanDataObject;
        }

        /// <summary>
        /// Gets the patch application deployment plan data of passed patch asset application id
        /// </summary>
        /// <param name="patchApplicationDeploymentPlanId">integer type parameter</param>
        /// <returns>Returns AutoPatchDepPlanDTO</returns>
        public AutoPatchDepPlanDTO GetAutoPatchDepPlan(int patchApplicationDeploymentPlanId)
        {
            log.Debug("Starts-GetAutoPatchDepPlan(patchApplicationDeploymentPlanId) Method.");
            string selectAutoPatchDepPlanQuery = @"select *
                                         from Patch_Deployment_Plan
                                        where PatchDeploymentPlanId = @patchDeploymentPlanId";
            SqlParameter[] selectAutoPatchDepPlanParameters = new SqlParameter[1];
            selectAutoPatchDepPlanParameters[0] = new SqlParameter("@patchDeploymentPlanId", patchApplicationDeploymentPlanId);
            DataTable patchApplicationDeploymentPlan = dataAccessHandler.executeSelectQuery(selectAutoPatchDepPlanQuery, selectAutoPatchDepPlanParameters);
            if (patchApplicationDeploymentPlan.Rows.Count > 0)
            {
                DataRow patchApplicationDeploymentPlanRow = patchApplicationDeploymentPlan.Rows[0];
                AutoPatchDepPlanDTO patchApplicationDeploymentPlanDataObject = GetAutoPatchDepPlanDTO(patchApplicationDeploymentPlanRow);
                log.Debug("Ends-GetAutoPatchDepPlan(patchApplicationDeploymentPlanId) Method by returnting patchApplicationDeploymentPlanDataObject.");
                return patchApplicationDeploymentPlanDataObject;
            }
            else
            {
                log.Debug("Ends-GetAutoPatchDepPlan(patchApplicationDeploymentPlanId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the AutoPatchDepPlanDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AutoPatchDepPlanDTO matching the search criteria</returns>
        public List<AutoPatchDepPlanDTO> GetAutoPatchDepPlanList(List<KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAutoPatchDepPlanList(searchParameters) Method.");
            int count = 0;
            string selectAutoPatchDepPlanQuery = @"select *
                                         from Patch_Deployment_Plan";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.DEPLOYMENT_STATUS))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " IN(" + searchParameter.Value + ") ");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.SITE_ID))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.DEPLOYMENT_PLANNED_DATE))
                            {
                                query.Append("Convert(Datetime," +DBSearchParameters[searchParameter.Key] + ") = Convert(Datetime,'" + searchParameter.Value + "')");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.PATCH_DEPLOYMENT_PLAN_ID) || searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value );
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.DEPLOYMENT_STATUS))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " IN(" + searchParameter.Value + ") ");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.DEPLOYMENT_PLANNED_DATE))
                            {
                                query.Append(" and DATEADD(MINUTE, DATEDIFF(MINUTE, 0," + DBSearchParameters[searchParameter.Key] + "), 0) = DATEADD(MINUTE, DATEDIFF(MINUTE, 0,'" + searchParameter.Value + "'), 0)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.PATCH_DEPLOYMENT_PLAN_ID) || searchParameter.Key.Equals(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(" and "+ DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
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
                        log.Debug("Ends-GetAutoPatchDepPlanList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                query.Append(" Order by DeploymentPlannedDate, PatchDeploymentPlanId ASC");
                if (searchParameters.Count > 0)
                    selectAutoPatchDepPlanQuery = selectAutoPatchDepPlanQuery + query;
            }

            DataTable patchApplicationDeploymentPlanData = dataAccessHandler.executeSelectQuery(selectAutoPatchDepPlanQuery, null);
            if (patchApplicationDeploymentPlanData.Rows.Count > 0)
            {
                List<AutoPatchDepPlanDTO> patchApplicationDeploymentPlanList = new List<AutoPatchDepPlanDTO>();
                foreach (DataRow patchApplicationDeploymentPlanDataRow in patchApplicationDeploymentPlanData.Rows)
                {
                    AutoPatchDepPlanDTO patchApplicationDeploymentPlanDataObject = GetAutoPatchDepPlanDTO(patchApplicationDeploymentPlanDataRow);
                    patchApplicationDeploymentPlanList.Add(patchApplicationDeploymentPlanDataObject);
                }
                log.Debug("Ends-GetAutoPatchDepPlanList(searchParameters) Method by returning patchApplicationDeploymentPlanList.");
                return patchApplicationDeploymentPlanList;
            }
            else
            {
                log.Debug("Ends-GetAutoPatchDepPlanList(searchParameters) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the AutoPatchDepPlanDTO list matching the search key
        /// </summary>
        /// <param name="siteId"> Site id passed as parameter</param>
        /// <returns>Returns the list of AutoPatchDepPlanDTO matching the search criteria</returns>
        public List<AutoPatchDepPlanDTO> GetEligibleDepPlanList(int siteId)
        {
            log.Debug("Starts-GetAutoPatchDepPlanList(searchParameters) Method.");
            string selectAutoPatchDepPlanQuery = @"select * from Patch_Deployment_Plan
                                                    where (site_id=@siteId or -1 = @siteId) and  DeploymentStatus!=@Status
                                                    and DeploymentPlannedDate < @date  and IsActive=@isActive
                                                    order by DeploymentPlannedDate";


            SqlParameter[] selectAutoPatchDepPlanParameters = new SqlParameter[4];
            selectAutoPatchDepPlanParameters[0] = new SqlParameter("@siteId", siteId);
            selectAutoPatchDepPlanParameters[1] = new SqlParameter("@date", DateTime.Now);
            selectAutoPatchDepPlanParameters[2] = new SqlParameter("@Status", AutoPatchDepPlanDTO.DeploymentStatusOption.COMPLETE.ToString());
            selectAutoPatchDepPlanParameters[3] = new SqlParameter("@isActive", "Y");
            DataTable patchApplicationDeploymentPlanData = dataAccessHandler.executeSelectQuery(selectAutoPatchDepPlanQuery, selectAutoPatchDepPlanParameters);
            if (patchApplicationDeploymentPlanData.Rows.Count > 0)
            {
                List<AutoPatchDepPlanDTO> patchApplicationDeploymentPlanList = new List<AutoPatchDepPlanDTO>();
                foreach (DataRow patchApplicationDeploymentPlanDataRow in patchApplicationDeploymentPlanData.Rows)
                {
                    AutoPatchDepPlanDTO patchApplicationDeploymentPlanDataObject = GetAutoPatchDepPlanDTO(patchApplicationDeploymentPlanDataRow);
                    patchApplicationDeploymentPlanList.Add(patchApplicationDeploymentPlanDataObject);
                }
                log.Debug("Ends-GetAutoPatchDepPlanList(searchParameters) Method by returning patchApplicationDeploymentPlanList.");
                return patchApplicationDeploymentPlanList;
            }
            else
            {
                log.Debug("Ends-GetAutoPatchDepPlanList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
