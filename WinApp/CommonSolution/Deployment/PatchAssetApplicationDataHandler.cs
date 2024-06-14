/********************************************************************************************
 * Project Name - Patch asset application data handler
 * Description  - Data handler of the patch asset application data handler class
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
namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Patch Asset Application Data Handler - Handles insert, update and select of patch asset application data objects
    /// </summary>
    public class PatchAssetApplicationDataHandler
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters, string> DBSearchParameters = new Dictionary<PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters, string>
               {
                {PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters.PATCH_ASSET_APPLICATION_ID, "PatchAssetApplicationId"},
                {PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters.ASSET_ID, "AssetId"},
                {PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters.PATCH_APPLICATION_TYPE_ID, "PatchAssetApplicationId"},
                {PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters.LAST_UPGRADE_DATE, "LastUpgradeDate"},
                {PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters.PATCH_VERSION_NUMBER, "PatchVersionNumber"},
                {PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters.ACTIVE_FLAG, "IsActive"}
               };
         DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of PatchAssetApplicationDataHandler class
        /// </summary>
        public PatchAssetApplicationDataHandler()
        {
            log.Debug("Starts-PatchAssetApplicationDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-PatchAssetApplicationDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the patch application type record to the database
        /// </summary>
        /// <param name="patchAssetApplication">PatchAssetApplicationDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertPatchAssetApplication(PatchAssetApplicationDTO patchAssetApplication, string userId, int siteId)
        {
            log.Debug("Starts-InsertPatchAssetApplication(patchAssetApplication, userId, siteId) Method.");
            string insertPatchAssetApplicationQuery = @"insert into Patch_Asset_Application 
                                                        (
                                                        AssetId,
                                                        PatchApplicationTypeId,
                                                        PatchVersionNumber,
                                                        PatchUpgradeStatus,
                                                        ApplicationPath,
                                                        LastUpgradeDate,
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
                                                        @assetId,
                                                        @patchApplicationTypeId,
                                                        @patchVersionNumber,
                                                        @patchUpgradeStatus,
                                                        @applicationPath,
                                                        @lastUpgradeDate,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updatePatchAssetApplicationParameters = new List<SqlParameter>();
            if (patchAssetApplication.AssetId == -1)
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@assetId", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@assetId", patchAssetApplication.AssetId));
            }
            if (patchAssetApplication.PatchApplicationTypeId == -1)
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchApplicationTypeId", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchApplicationTypeId", patchAssetApplication.PatchApplicationTypeId));
            }
            if (string.IsNullOrEmpty(patchAssetApplication.PatchVersionNumber))
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchVersionNumber", DBNull.Value));
            }
            else
            {

                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchVersionNumber", patchAssetApplication.PatchVersionNumber));
            }
            if (string.IsNullOrEmpty(patchAssetApplication.PatchUpgradeStatus))
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchUpgradeStatus", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchUpgradeStatus", patchAssetApplication.PatchUpgradeStatus));
            }
            if (string.IsNullOrEmpty(patchAssetApplication.ApplicationPath))
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@applicationPath", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@applicationPath", patchAssetApplication.ApplicationPath));
            }
            if (patchAssetApplication.LastUpgradeDate.Equals(DateTime.MinValue))
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@lastUpgradeDate", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@lastUpgradeDate", patchAssetApplication.LastUpgradeDate));
            }
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(patchAssetApplication.IsActive) ? "N" : "Y"));
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@createdBy", userId));
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@siteid", siteId));
            }
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@synchStatus", patchAssetApplication.SynchStatus));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertPatchAssetApplicationQuery, updatePatchAssetApplicationParameters.ToArray());
            log.Debug("Ends-InsertPatchAssetApplication(patchAssetApplication, userId, siteId) Method.");
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the patch asset application record
        /// </summary>
        /// <param name="patchAssetApplication">PatchAssetApplicationDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdatePatchAssetApplication(PatchAssetApplicationDTO patchAssetApplication, string userId, int siteId)
        {
            log.Debug("Starts-UpdatePatchAssetApplication(patchAssetApplication, userId, siteId) Method.");
            string updatePatchAssetApplicationQuery = @"update Patch_Asset_Application 
                                         set AssetId=@assetId,
                                             PatchApplicationTypeId=@patchApplicationTypeId,
                                             PatchVersionNumber=@patchVersionNumber,
                                             PatchUpgradeStatus=@patchUpgradeStatus,
                                             ApplicationPath=@applicationPath,
                                             LastUpgradeDate=@lastUpgradeDate,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             Siteid=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where PatchAssetApplicationId = @patchAssetApplicationId";
            List<SqlParameter> updatePatchAssetApplicationParameters = new List<SqlParameter>();
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchAssetApplicationId", patchAssetApplication.PatchAssetApplicationId));
            if (patchAssetApplication.AssetId == -1)
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@assetId", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@assetId", patchAssetApplication.AssetId));
            }
            if (patchAssetApplication.PatchApplicationTypeId == -1)
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchApplicationTypeId", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchApplicationTypeId", patchAssetApplication.PatchApplicationTypeId));
            }
            if (string.IsNullOrEmpty(patchAssetApplication.PatchVersionNumber))
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchVersionNumber", DBNull.Value));
            }
            else
            {

                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchVersionNumber", patchAssetApplication.PatchVersionNumber));
            }
            if (string.IsNullOrEmpty(patchAssetApplication.PatchUpgradeStatus))
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchUpgradeStatus", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@patchUpgradeStatus", patchAssetApplication.PatchUpgradeStatus));
            }
            if (string.IsNullOrEmpty(patchAssetApplication.ApplicationPath))
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@applicationPath", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@applicationPath", patchAssetApplication.ApplicationPath));
            }
            if (patchAssetApplication.LastUpgradeDate.Equals(DateTime.MinValue))
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@lastUpgradeDate", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@lastUpgradeDate", patchAssetApplication.LastUpgradeDate));
            }
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(patchAssetApplication.IsActive) ? "N" : "Y"));
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@createdBy", userId));
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updatePatchAssetApplicationParameters.Add(new SqlParameter("@siteid", siteId));
            }
            updatePatchAssetApplicationParameters.Add(new SqlParameter("@synchStatus", patchAssetApplication.SynchStatus));
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updatePatchAssetApplicationQuery, updatePatchAssetApplicationParameters.ToArray());
            log.Debug("Ends-UpdatePatchAssetApplication(patchAssetApplication, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to PatchAssetApplicationDTO class type
        /// </summary>
        /// <param name="patchAssetApplicationDataRow">PatchAssetApplicationDTO DataRow</param>
        /// <returns>Returns PatchAssetApplicationDTO</returns>
        private PatchAssetApplicationDTO GetPatchAssetApplicationDTO(DataRow patchAssetApplicationDataRow)
        {
            log.Debug("Starts-GetPatchAssetApplicationDTO(patchAssetApplicationDataRow) Method.");
            PatchAssetApplicationDTO patchAssetApplicationDataObject = new PatchAssetApplicationDTO(Convert.ToInt32(patchAssetApplicationDataRow["PatchAssetApplicationId"]),
                                            patchAssetApplicationDataRow["assetId"] == DBNull.Value ? -1 : Convert.ToInt32(patchAssetApplicationDataRow["assetId"]),
                                            patchAssetApplicationDataRow["PatchApplicationTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(patchAssetApplicationDataRow["PatchApplicationTypeId"]),
                                            patchAssetApplicationDataRow["PatchVersionNumber"].ToString(),
                                            patchAssetApplicationDataRow["PatchUpgradeStatus"].ToString(),
                                            patchAssetApplicationDataRow["ApplicationPath"].ToString(),
                                            patchAssetApplicationDataRow["LastUpgradeDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchAssetApplicationDataRow["LastUpgradeDate"]),
                                            patchAssetApplicationDataRow["IsActive"].ToString(),
                                            patchAssetApplicationDataRow["CreatedBy"].ToString(),
                                            patchAssetApplicationDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchAssetApplicationDataRow["CreationDate"]),
                                            patchAssetApplicationDataRow["LastUpdatedBy"].ToString(),
                                            patchAssetApplicationDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchAssetApplicationDataRow["LastupdatedDate"]),
                                            patchAssetApplicationDataRow["Guid"].ToString(),
                                            patchAssetApplicationDataRow["Siteid"] == DBNull.Value ? -1 : Convert.ToInt32(patchAssetApplicationDataRow["Siteid"]),
                                            patchAssetApplicationDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(patchAssetApplicationDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetPatchAssetApplicationDTO(patchAssetApplicationDataRow) Method.");
            return patchAssetApplicationDataObject;
        }

        /// <summary>
        /// Gets the patch asset application data of passed patch asset application id
        /// </summary>
        /// <param name="patchAssetApplicationId">integer type parameter</param>
        /// <returns>Returns PatchAssetApplicationDTO</returns>
        public PatchAssetApplicationDTO GetPatchAssetApplication(int patchAssetApplicationId)
        {
            log.Debug("Starts-GetPatchAssetApplication(patchAssetApplicationId) Method.");
            string selectPatchAssetApplicationQuery = @"select *
                                         from Patch_Asset_Application
                                        where PatchAssetApplicationId = @patchAssetApplicationId";
            SqlParameter[] selectPatchAssetApplicationParameters = new SqlParameter[1];
            selectPatchAssetApplicationParameters[0] = new SqlParameter("@patchAssetApplicationId", patchAssetApplicationId);
            DataTable patchAssetApplication = dataAccessHandler.executeSelectQuery(selectPatchAssetApplicationQuery, selectPatchAssetApplicationParameters);
            if (patchAssetApplication.Rows.Count > 0)
            {
                DataRow patchAssetApplicationRow = patchAssetApplication.Rows[0];
                PatchAssetApplicationDTO patchAssetApplicationDataObject = GetPatchAssetApplicationDTO(patchAssetApplicationRow);
                log.Debug("Ends-GetPatchAssetApplication(patchAssetApplicationId) Method by returnting patchAssetApplicationDataObject.");
                return patchAssetApplicationDataObject;
            }
            else
            {
                log.Debug("Ends-GetPatchAssetApplication(patchAssetApplicationId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the PatchAssetApplicationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PatchAssetApplicationDTO matching the search criteria</returns>
        public List<PatchAssetApplicationDTO> GetPatchAssetApplicationList(List<KeyValuePair<PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetPatchAssetApplicationList(searchParameters) Method.");
            int count = 0;
            string selectPatchAssetApplicationQuery = @"select *
                                         from Patch_Asset_Application";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters, string> searchParameter in searchParameters)
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
                        log.Debug("Ends-GetPatchAssetApplicationList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectPatchAssetApplicationQuery = selectPatchAssetApplicationQuery + query;
            }

            DataTable patchAssetApplicationData = dataAccessHandler.executeSelectQuery(selectPatchAssetApplicationQuery, null);
            if (patchAssetApplicationData.Rows.Count > 0)
            {
                List<PatchAssetApplicationDTO> patchAssetApplicationList = new List<PatchAssetApplicationDTO>();
                foreach (DataRow patchAssetApplicationDataRow in patchAssetApplicationData.Rows)
                {
                    PatchAssetApplicationDTO patchAssetApplicationDataObject = GetPatchAssetApplicationDTO(patchAssetApplicationDataRow);
                    patchAssetApplicationList.Add(patchAssetApplicationDataObject);
                }
                log.Debug("Ends-GetPatchAssetApplicationList(searchParameters) Method by returning patchAssetApplicationList.");
                return patchAssetApplicationList;
            }
            else
            {
                log.Debug("Ends-GetPatchAssetApplicationList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
