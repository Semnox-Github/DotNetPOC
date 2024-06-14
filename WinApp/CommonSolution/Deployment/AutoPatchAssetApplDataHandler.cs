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
    public class AutoPatchAssetApplDataHandler
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string> DBSearchParameters = new Dictionary<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>
               {
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_ASSET_APPLICATION_ID, "PatchAssetApplicationId"},
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ASSET_ID, "AssetId"},
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID, "PatchApplicationTypeId"},
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.LAST_UPGRADE_DATE, "LastUpgradeDate"},
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER, "PatchVersionNumber"},
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.ACTIVE_FLAG, "IsActive"},
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_UPGRADE_STATUS, "PatchUpgradeStatus"},
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID, "site_id"},
                {AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.MINIMUM_REQUIRED_VERSION, "PatchVersionNumber"}
               };
         DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of AutoPatchAssetApplDataHandler class
        /// </summary>
        public AutoPatchAssetApplDataHandler()
        {
            log.Debug("Starts-AutoPatchAssetApplDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-AutoPatchAssetApplDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the patch application type record to the database
        /// </summary>
        /// <param name="autoPatchAssetAppl">AutoPatchAssetApplDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertAutoPatchAssetAppl(AutoPatchAssetApplDTO autoPatchAssetAppl, string userId, int siteId)
        {
            log.Debug("Starts-InsertAutoPatchAssetAppl(autoPatchAssetAppl, userId, siteId) Method.");
            string insertAutoPatchAssetApplQuery = @"insert into Patch_Asset_Application 
                                                        (
                                                        AssetId,
                                                        PatchApplicationTypeId,
                                                        PatchVersionNumber,
                                                        PatchUpgradeStatus,
                                                        ApplicationPath,
                                                        LastUpgradeDate,
                                                        ErrorCounter,
                                                        PasswordChangeStatus,
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
                                                        @assetId,
                                                        @patchApplicationTypeId,
                                                        @patchVersionNumber,
                                                        @patchUpgradeStatus,
                                                        @applicationPath,
                                                        @lastUpgradeDate,
                                                        @errorCounter,
                                                        @passwordChangeStatus,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateAutoPatchAssetApplParameters = new List<SqlParameter>();
            if (autoPatchAssetAppl.AssetId == -1)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@assetId", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@assetId", autoPatchAssetAppl.AssetId));
            }
            if (autoPatchAssetAppl.PatchApplicationTypeId == -1)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchApplicationTypeId", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchApplicationTypeId", autoPatchAssetAppl.PatchApplicationTypeId));
            }
            if (string.IsNullOrEmpty(autoPatchAssetAppl.PatchVersionNumber))
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchVersionNumber", DBNull.Value));
            }
            else
            {

                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchVersionNumber", autoPatchAssetAppl.PatchVersionNumber));
            }
            if (string.IsNullOrEmpty(autoPatchAssetAppl.PatchUpgradeStatus))
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchUpgradeStatus", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchUpgradeStatus", autoPatchAssetAppl.PatchUpgradeStatus));
            }
            if (string.IsNullOrEmpty(autoPatchAssetAppl.ApplicationPath))
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@applicationPath", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@applicationPath", autoPatchAssetAppl.ApplicationPath));
            }
            if (autoPatchAssetAppl.LastUpgradeDate.Equals(DateTime.MinValue))
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@lastUpgradeDate", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@lastUpgradeDate", autoPatchAssetAppl.LastUpgradeDate));
            }
            if (autoPatchAssetAppl.ErrorCounter == -1)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@errorCounter", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@errorCounter", autoPatchAssetAppl.ErrorCounter));
            }
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@passwordChangeStatus", autoPatchAssetAppl.PasswordChangeStatus));
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(autoPatchAssetAppl.IsActive) ? "N" : autoPatchAssetAppl.IsActive));
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@createdBy", userId));
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@lastUpdatedBy", userId));           
            if (siteId == -1)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (autoPatchAssetAppl.SynchStatus)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@synchStatus", autoPatchAssetAppl.SynchStatus));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            } 
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertAutoPatchAssetApplQuery, updateAutoPatchAssetApplParameters.ToArray());
            log.Debug("Ends-InsertAutoPatchAssetAppl(autoPatchAssetAppl, userId, siteId) Method.");
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the patch asset application record
        /// </summary>
        /// <param name="autoPatchAssetAppl">AutoPatchAssetApplDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateAutoPatchAssetAppl(AutoPatchAssetApplDTO autoPatchAssetAppl, string userId, int siteId)
        {
            log.Debug("Starts-UpdateAutoPatchAssetAppl(autoPatchAssetAppl, userId, siteId) Method.");
            string updateAutoPatchAssetApplQuery = @"update Patch_Asset_Application 
                                         set AssetId=@assetId,
                                             PatchApplicationTypeId=@patchApplicationTypeId,
                                             PatchVersionNumber=@patchVersionNumber,
                                             PatchUpgradeStatus=@patchUpgradeStatus,
                                             ApplicationPath=@applicationPath,
                                             LastUpgradeDate=@lastUpgradeDate,
                                             ErrorCounter=@errorCounter,
                                             PasswordChangeStatus=@passwordChangeStatus,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             site_id = @siteid,
                                             SynchStatus = @synchStatus                                             
                                       where PatchAssetApplicationId = @patchAssetApplicationId";
            List<SqlParameter> updateAutoPatchAssetApplParameters = new List<SqlParameter>();
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchAssetApplicationId", autoPatchAssetAppl.PatchAssetApplicationId));
            if (autoPatchAssetAppl.AssetId == -1)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@assetId", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@assetId", autoPatchAssetAppl.AssetId));
            }
            if (autoPatchAssetAppl.PatchApplicationTypeId == -1)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchApplicationTypeId", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchApplicationTypeId", autoPatchAssetAppl.PatchApplicationTypeId));
            }
            if (string.IsNullOrEmpty(autoPatchAssetAppl.PatchVersionNumber))
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchVersionNumber", DBNull.Value));
            }
            else
            {

                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchVersionNumber", autoPatchAssetAppl.PatchVersionNumber));
            }
            if (string.IsNullOrEmpty(autoPatchAssetAppl.PatchUpgradeStatus))
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchUpgradeStatus", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@patchUpgradeStatus", autoPatchAssetAppl.PatchUpgradeStatus));
            }
            if (string.IsNullOrEmpty(autoPatchAssetAppl.ApplicationPath))
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@applicationPath", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@applicationPath", autoPatchAssetAppl.ApplicationPath));
            }
            if (autoPatchAssetAppl.LastUpgradeDate.Equals(DateTime.MinValue))
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@lastUpgradeDate", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@lastUpgradeDate", autoPatchAssetAppl.LastUpgradeDate));
            }

            if (autoPatchAssetAppl.ErrorCounter == -1)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@errorCounter", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@errorCounter", autoPatchAssetAppl.ErrorCounter));
            }
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@passwordChangeStatus", autoPatchAssetAppl.PasswordChangeStatus));
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(autoPatchAssetAppl.IsActive) ? "N" : autoPatchAssetAppl.IsActive));
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@createdBy", userId));
            updateAutoPatchAssetApplParameters.Add(new SqlParameter("@lastUpdatedBy", userId));            
            if (siteId == -1)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (autoPatchAssetAppl.SynchStatus)
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@synchStatus", autoPatchAssetAppl.SynchStatus));
            }
            else
            {
                updateAutoPatchAssetApplParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }           
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateAutoPatchAssetApplQuery, updateAutoPatchAssetApplParameters.ToArray());
            log.Debug("Ends-UpdateAutoPatchAssetAppl(autoPatchAssetAppl, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to AutoPatchAssetApplDTO class type
        /// </summary>
        /// <param name="autoPatchAssetApplDataRow">AutoPatchAssetApplDTO DataRow</param>
        /// <returns>Returns AutoPatchAssetApplDTO</returns>
        private AutoPatchAssetApplDTO GetAutoPatchAssetApplDTO(DataRow autoPatchAssetApplDataRow)
        {
            log.Debug("Starts-GetAutoPatchAssetApplDTO(autoPatchAssetApplDataRow) Method.");
            AutoPatchAssetApplDTO autoPatchAssetApplDataObject = new AutoPatchAssetApplDTO(Convert.ToInt32(autoPatchAssetApplDataRow["PatchAssetApplicationId"]),
                                            autoPatchAssetApplDataRow["assetId"] == DBNull.Value ? -1 : Convert.ToInt32(autoPatchAssetApplDataRow["assetId"]),
                                            autoPatchAssetApplDataRow["PatchApplicationTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(autoPatchAssetApplDataRow["PatchApplicationTypeId"]),
                                            autoPatchAssetApplDataRow["PatchVersionNumber"].ToString(),
                                            autoPatchAssetApplDataRow["PatchUpgradeStatus"].ToString(),
                                            autoPatchAssetApplDataRow["ApplicationPath"].ToString(),
                                            autoPatchAssetApplDataRow["LastUpgradeDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(autoPatchAssetApplDataRow["LastUpgradeDate"]),
                                            autoPatchAssetApplDataRow["ErrorCounter"] == DBNull.Value ? 0 : Convert.ToInt32(autoPatchAssetApplDataRow["ErrorCounter"]),
                                            autoPatchAssetApplDataRow["PasswordChangeStatus"] == DBNull.Value ? false : Convert.ToBoolean(autoPatchAssetApplDataRow["PasswordChangeStatus"]),
                                            autoPatchAssetApplDataRow["IsActive"].ToString(),
                                            autoPatchAssetApplDataRow["CreatedBy"].ToString(),
                                            autoPatchAssetApplDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(autoPatchAssetApplDataRow["CreationDate"]),
                                            autoPatchAssetApplDataRow["LastUpdatedBy"].ToString(),
                                            autoPatchAssetApplDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(autoPatchAssetApplDataRow["LastupdatedDate"]),
                                            autoPatchAssetApplDataRow["Guid"].ToString(),
                                            autoPatchAssetApplDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(autoPatchAssetApplDataRow["site_id"]),
                                            autoPatchAssetApplDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(autoPatchAssetApplDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetAutoPatchAssetApplDTO(autoPatchAssetApplDataRow) Method.");
            return autoPatchAssetApplDataObject;
        }

        /// <summary>
        /// Gets the patch asset application data of passed patch asset application id
        /// </summary>
        /// <param name="autoPatchAssetApplId">integer type parameter</param>
        /// <returns>Returns AutoPatchAssetApplDTO</returns>
        public AutoPatchAssetApplDTO GetAutoPatchAssetAppl(int autoPatchAssetApplId)
        {
            log.Debug("Starts-GetAutoPatchAssetAppl(autoPatchAssetApplId) Method.");
            string selectAutoPatchAssetApplQuery = @"select *
                                         from Patch_Asset_Application
                                        where PatchAssetApplicationId = @autoPatchAssetApplId";
            SqlParameter[] selectAutoPatchAssetApplParameters = new SqlParameter[1];
            selectAutoPatchAssetApplParameters[0] = new SqlParameter("@autoPatchAssetApplId", autoPatchAssetApplId);
            DataTable autoPatchAssetAppl = dataAccessHandler.executeSelectQuery(selectAutoPatchAssetApplQuery, selectAutoPatchAssetApplParameters);
            if (autoPatchAssetAppl.Rows.Count > 0)
            {
                DataRow autoPatchAssetApplRow = autoPatchAssetAppl.Rows[0];
                AutoPatchAssetApplDTO autoPatchAssetApplDataObject = GetAutoPatchAssetApplDTO(autoPatchAssetApplRow);
                log.Debug("Ends-GetAutoPatchAssetAppl(autoPatchAssetApplId) Method by returnting autoPatchAssetApplDataObject.");
                return autoPatchAssetApplDataObject;
            }
            else
            {
                log.Debug("Ends-GetAutoPatchAssetAppl(autoPatchAssetApplId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the AutoPatchAssetApplDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AutoPatchAssetApplDTO matching the search criteria</returns>
        public List<AutoPatchAssetApplDTO> GetAutoPatchAssetApplList(List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAutoPatchAssetApplList(searchParameters) Method.");
            int count = 0;
            string selectAutoPatchAssetApplQuery = @"select *
                                         from Patch_Asset_Application";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_UPGRADE_STATUS))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " IN ( " + searchParameter.Value + " )");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.MINIMUM_REQUIRED_VERSION))
                            {
                                query.Append(" isNull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') >= '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER))
                            {
                                query.Append(" isNull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') < '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID))
                            {
                                query.Append("(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID) || searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_ASSET_APPLICATION_ID))
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
                            if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_UPGRADE_STATUS))
                            {
                                query.Append(" and "+DBSearchParameters[searchParameter.Key] + " IN ( " + searchParameter.Value + " )");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.MINIMUM_REQUIRED_VERSION))
                            {
                                query.Append(" and isNull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') >= '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER))
                            {
                                query.Append(" and isNull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') < '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID) || searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_ASSET_APPLICATION_ID))
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
                        log.Debug("Ends-GetAutoPatchAssetApplList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectAutoPatchAssetApplQuery = selectAutoPatchAssetApplQuery + query;
                selectAutoPatchAssetApplQuery = selectAutoPatchAssetApplQuery + " Order by assetId, PatchApplicationTypeId";
            }

            DataTable autoPatchAssetApplData = dataAccessHandler.executeSelectQuery(selectAutoPatchAssetApplQuery, null);
            if (autoPatchAssetApplData.Rows.Count > 0)
            {
                List<AutoPatchAssetApplDTO> autoPatchAssetApplList = new List<AutoPatchAssetApplDTO>();
                foreach (DataRow autoPatchAssetApplDataRow in autoPatchAssetApplData.Rows)
                {
                    AutoPatchAssetApplDTO autoPatchAssetApplDataObject = GetAutoPatchAssetApplDTO(autoPatchAssetApplDataRow);
                    autoPatchAssetApplList.Add(autoPatchAssetApplDataObject);
                }
                log.Debug("Ends-GetAutoPatchAssetApplList(searchParameters) Method by returning autoPatchAssetApplList.");
                return autoPatchAssetApplList;
            }
            else
            {
                log.Debug("Ends-GetAutoPatchAssetApplList(searchParameters) Method by returning null.");
                return null;
            }
        }
        /// <summary>
        /// Returns the AutoPatchAssetApplDTO list where version equal to the deployment plans minimum required version and other options are operated based on like operator
        /// </summary>
        /// <param name="searchParameters">Is list of KeyValuePair AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters and string.</param>
        /// <returns>List of AutoPatchAssetApplDTO </returns>
        public List<AutoPatchAssetApplDTO> GetLowerVersionAssetApplication(List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetUnupdatedAutoPatchAssetApplList(searchParameters) Method.");
            int count = 0;
            string selectAutoPatchAssetApplQuery = @"select *
                                         from Patch_Asset_Application";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER))
                            {
                                query.Append(" isNull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') = '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID))
                            {
                                query.Append("(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID) || searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_ASSET_APPLICATION_ID))
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
                            if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER))
                            {
                                query.Append(" and isNull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') = '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID) || searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_ASSET_APPLICATION_ID))
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
                        log.Debug("Ends-GetUnupdatedAutoPatchAssetApplList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectAutoPatchAssetApplQuery = selectAutoPatchAssetApplQuery + query;
            }

            DataTable autoPatchAssetApplData = dataAccessHandler.executeSelectQuery(selectAutoPatchAssetApplQuery, null);
            if (autoPatchAssetApplData.Rows.Count > 0)
            {
                List<AutoPatchAssetApplDTO> autoPatchAssetApplList = new List<AutoPatchAssetApplDTO>();
                foreach (DataRow autoPatchAssetApplDataRow in autoPatchAssetApplData.Rows)
                {
                    AutoPatchAssetApplDTO autoPatchAssetApplDataObject = GetAutoPatchAssetApplDTO(autoPatchAssetApplDataRow);
                    autoPatchAssetApplList.Add(autoPatchAssetApplDataObject);
                }
                log.Debug("Ends-GetUnupdatedAutoPatchAssetApplList(searchParameters) Method by returning autoPatchAssetApplList.");
                return autoPatchAssetApplList;
            }
            else
            {
                log.Debug("Ends-GetUnupdatedAutoPatchAssetApplList(searchParameters) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Returns the AutoPatchAssetApplDTO list where version equal to the deployment version and other options are operated based on like operator
        /// </summary>
        /// <param name="searchParameters">Is list of KeyValuePair AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters and string.</param>
        /// <returns>List of AutoPatchAssetApplDTO </returns>
        public List<AutoPatchAssetApplDTO> GetCurrentOrHigherVersionAssetApplication(List<KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetUnupdatedAutoPatchAssetApplList(searchParameters) Method.");
            int count = 0;
            string selectAutoPatchAssetApplQuery = @"select *
                                         from Patch_Asset_Application";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER))
                            {
                                query.Append(" isNull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') >= '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID))
                            {
                                query.Append("(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID) || searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_ASSET_APPLICATION_ID))
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
                            if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_VERSION_NUMBER))
                            {
                                query.Append(" and isNull(" + DBSearchParameters[searchParameter.Key] + ",'0.0') >= '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_APPLICATION_TYPE_ID) || searchParameter.Key.Equals(AutoPatchAssetApplDTO.SearchByAutoPatchAssetApplParameters.PATCH_ASSET_APPLICATION_ID))
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
                        log.Debug("Ends-GetUnupdatedAutoPatchAssetApplList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectAutoPatchAssetApplQuery = selectAutoPatchAssetApplQuery + query;
            }

            DataTable autoPatchAssetApplData = dataAccessHandler.executeSelectQuery(selectAutoPatchAssetApplQuery, null);
            if (autoPatchAssetApplData.Rows.Count > 0)
            {
                List<AutoPatchAssetApplDTO> autoPatchAssetApplList = new List<AutoPatchAssetApplDTO>();
                foreach (DataRow autoPatchAssetApplDataRow in autoPatchAssetApplData.Rows)
                {
                    AutoPatchAssetApplDTO autoPatchAssetApplDataObject = GetAutoPatchAssetApplDTO(autoPatchAssetApplDataRow);
                    autoPatchAssetApplList.Add(autoPatchAssetApplDataObject);
                }
                log.Debug("Ends-GetUnupdatedAutoPatchAssetApplList(searchParameters) Method by returning autoPatchAssetApplList.");
                return autoPatchAssetApplList;
            }
            else
            {
                log.Debug("Ends-GetUnupdatedAutoPatchAssetApplList(searchParameters) Method by returning null.");
                return null;
            }
        }

    }
}
