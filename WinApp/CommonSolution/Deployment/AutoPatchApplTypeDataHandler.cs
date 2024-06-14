/********************************************************************************************
 * Project Name - Patch application type data handler
 * Description  - Data handler of the patch application type data handler class
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
    /// Patch Application Type Data Handler - Handles insert, update and select of patch application type data objects
    /// </summary>
    public class AutoPatchApplTypeDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string> DBSearchParameters = new Dictionary<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>
               {
                {AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.PATCH_APPLICATION_TYPE_ID, "PatchApplicationTypeId"},
                {AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.APPLICATION_TYPE, "ApplicationType"},
                {AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.ACTIVE_FLAG, "IsActive"},
                {AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID, "site_Id"},
               };
        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of AutoPatchApplTypeDataHandler class
        /// </summary>
        public AutoPatchApplTypeDataHandler()
        {
            log.Debug("Starts-AutoPatchApplTypeDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-AutoPatchApplTypeDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the patch application type record to the database
        /// </summary>
        /// <param name="autoPatchApplType">AutoPatchApplTypeDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertAutoPatchApplType(AutoPatchApplTypeDTO autoPatchApplType, string userId, int siteId)
        {
            log.Debug("Starts-InsertAutoPatchApplType(autoPatchApplType, userId, siteId) Method.");
            string insertAutoPatchApplTypeQuery = @"insert into Patch_Application_Types 
                                                        (
                                                        ApplicationType,
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
                                                        @applicationType,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateAutoPatchApplTypeParameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(autoPatchApplType.ApplicationType))
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@applicationType", DBNull.Value));
            }
            else
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@applicationType", autoPatchApplType.ApplicationType));
            }
            updateAutoPatchApplTypeParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(autoPatchApplType.IsActive) ? "N" : autoPatchApplType.IsActive));
            updateAutoPatchApplTypeParameters.Add(new SqlParameter("@createdBy", userId));
            updateAutoPatchApplTypeParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (autoPatchApplType.SynchStatus)
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@synchStatus", autoPatchApplType.SynchStatus));
            }
            else
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertAutoPatchApplTypeQuery, updateAutoPatchApplTypeParameters.ToArray());
            log.Debug("Ends-InsertAutoPatchApplType(autoPatchApplType, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the patch application type record
        /// </summary>
        /// <param name="autoPatchApplType">AutoPatchApplTypeDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateAutoPatchApplType(AutoPatchApplTypeDTO autoPatchApplType, string userId, int siteId)
        {
            log.Debug("Starts-UpdateAutoPatchApplType(autoPatchApplType, userId, siteId) Method.");
            string updateAutoPatchApplTypeQuery = @"update Patch_Application_Types 
                                         set ApplicationType = @applicationType,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             site_id=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where PatchApplicationTypeId = @patchApplicationTypeId";
            List<SqlParameter> updateAutoPatchApplTypeParameters = new List<SqlParameter>();
            updateAutoPatchApplTypeParameters.Add(new SqlParameter("@patchApplicationTypeId", autoPatchApplType.PatchApplicationTypeId));
            if (string.IsNullOrEmpty(autoPatchApplType.ApplicationType))
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@applicationType", DBNull.Value));
            }
            else
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@applicationType", autoPatchApplType.ApplicationType));
            }
            updateAutoPatchApplTypeParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(autoPatchApplType.IsActive) ? "N" : autoPatchApplType.IsActive));

            updateAutoPatchApplTypeParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@siteId", siteId));
            if (autoPatchApplType.SynchStatus)
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@synchStatus", autoPatchApplType.SynchStatus));
            }
            else
            {
                updateAutoPatchApplTypeParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateAutoPatchApplTypeQuery, updateAutoPatchApplTypeParameters.ToArray());
            log.Debug("Ends-UpdateAutoPatchApplType(autoPatchApplType, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to AutoPatchApplTypeDTO class type
        /// </summary>
        /// <param name="autoPatchApplTypeDataRow">AutoPatchApplTypeDTO DataRow</param>
        /// <returns>Returns AutoPatchApplTypeDTO</returns>
        private AutoPatchApplTypeDTO GetAutoPatchApplTypeDTO(DataRow autoPatchApplTypeDataRow)
        {
            log.Debug("Starts-GetAutoPatchApplTypeDTO(autoPatchApplTypeDataRow) Method.");
            AutoPatchApplTypeDTO autoPatchApplTypeDataObject = new AutoPatchApplTypeDTO(Convert.ToInt32(autoPatchApplTypeDataRow["PatchApplicationTypeId"]),
                                            autoPatchApplTypeDataRow["ApplicationType"].ToString(),
                                            autoPatchApplTypeDataRow["IsActive"].ToString(),
                                            autoPatchApplTypeDataRow["CreatedBy"].ToString(),
                                            autoPatchApplTypeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(autoPatchApplTypeDataRow["CreationDate"]),
                                            autoPatchApplTypeDataRow["LastUpdatedBy"].ToString(),
                                            autoPatchApplTypeDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(autoPatchApplTypeDataRow["LastupdatedDate"]),
                                            autoPatchApplTypeDataRow["Guid"].ToString(),
                                            autoPatchApplTypeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(autoPatchApplTypeDataRow["site_id"]),
                                            autoPatchApplTypeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(autoPatchApplTypeDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetAutoPatchApplTypeDTO(autoPatchApplTypeDataRow) Method.");
            return autoPatchApplTypeDataObject;
        }

        /// <summary>
        /// Gets the patch application type data of passed patch application type id
        /// </summary>
        /// <param name="autoPatchApplTypeId">integer type parameter</param>
        /// <returns>Returns AutoPatchApplTypeDTO</returns>
        public AutoPatchApplTypeDTO GetAutoPatchApplType(int autoPatchApplTypeId)
        {
            log.Debug("Starts-GetAutoPatchApplType(autoPatchApplTypeId) Method.");
            string selectAutoPatchApplTypeQuery = @"select *
                                         from Patch_Application_Types
                                        where PatchApplicationTypeId = @autoPatchApplTypeId";
            SqlParameter[] selectAutoPatchApplTypeParameters = new SqlParameter[1];
            selectAutoPatchApplTypeParameters[0] = new SqlParameter("@autoPatchApplTypeId", autoPatchApplTypeId);
            DataTable autoPatchApplType = dataAccessHandler.executeSelectQuery(selectAutoPatchApplTypeQuery, selectAutoPatchApplTypeParameters);
            if (autoPatchApplType.Rows.Count > 0)
            {
                DataRow autoPatchApplTypeRow = autoPatchApplType.Rows[0];
                AutoPatchApplTypeDTO autoPatchApplTypeDataObject = GetAutoPatchApplTypeDTO(autoPatchApplTypeRow);
                log.Debug("Ends-GetAutoPatchApplType(autoPatchApplTypeId) Method by returnting autoPatchApplTypeDataObject.");
                return autoPatchApplTypeDataObject;
            }
            else
            {
                log.Debug("Ends-GetAutoPatchApplType(autoPatchApplTypeId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the AutoPatchApplTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AutoPatchApplTypeDTO matching the search criteria</returns>
        public List<AutoPatchApplTypeDTO> GetAutoPatchApplTypeList(List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAutoPatchApplTypeList(searchParameters) Method.");
            int count = 0;
            string selectAutoPatchApplTypeQuery = @"select *
                                         from Patch_Application_Types";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID))
                            {
                                query.Append("(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
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
                        log.Debug("Ends-GetAutoPatchApplTypeList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectAutoPatchApplTypeQuery = selectAutoPatchApplTypeQuery + query;
            }

            DataTable autoPatchApplTypeData = dataAccessHandler.executeSelectQuery(selectAutoPatchApplTypeQuery, null);
            if (autoPatchApplTypeData.Rows.Count > 0)
            {
                List<AutoPatchApplTypeDTO> autoPatchApplTypeList = new List<AutoPatchApplTypeDTO>();
                foreach (DataRow autoPatchApplTypeDataRow in autoPatchApplTypeData.Rows)
                {
                    AutoPatchApplTypeDTO autoPatchApplTypeDataObject = GetAutoPatchApplTypeDTO(autoPatchApplTypeDataRow);
                    autoPatchApplTypeList.Add(autoPatchApplTypeDataObject);
                }
                log.Debug("Ends-GetAutoPatchApplTypeList(searchParameters) Method by returning autoPatchApplTypeList.");
                return autoPatchApplTypeList;
            }
            else
            {
                log.Debug("Ends-GetAutoPatchApplTypeList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
