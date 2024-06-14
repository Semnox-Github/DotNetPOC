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
    public class PatchApplicationTypeDataHandler
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<PatchApplicationTypeDTO.SearchByPatchApplicationTypeParameters, string> DBSearchParameters = new Dictionary<PatchApplicationTypeDTO.SearchByPatchApplicationTypeParameters, string>
               {
                {PatchApplicationTypeDTO.SearchByPatchApplicationTypeParameters.PATCH_APPLICATION_TYPE_ID, "PatchApplicationTypeId"},
                {PatchApplicationTypeDTO.SearchByPatchApplicationTypeParameters.APPLICATION_TYPE, "ApplicationType"},
                {PatchApplicationTypeDTO.SearchByPatchApplicationTypeParameters.ACTIVE_FLAG, "IsActive"}
               };
         DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of PatchApplicationTypeDataHandler class
        /// </summary>
        public PatchApplicationTypeDataHandler()
        {
            log.Debug("Starts-PatchApplicationTypeDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-PatchApplicationTypeDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the patch application type record to the database
        /// </summary>
        /// <param name="patchApplicationType">PatchApplicationTypeDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertPatchApplicationType(PatchApplicationTypeDTO patchApplicationType, string userId, int siteId)
        {
            log.Debug("Starts-InsertPatchApplicationType(patchApplicationType, userId, siteId) Method.");
            string insertPatchApplicationTypeQuery = @"insert into Patch_Application_Types 
                                                        (
                                                        ApplicationType,
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
            List<SqlParameter> updatePatchApplicationTypeParameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(patchApplicationType.ApplicationType))
            {
                updatePatchApplicationTypeParameters.Add(new SqlParameter("@applicationType", DBNull.Value));
            }
            else
            {
                updatePatchApplicationTypeParameters.Add(new SqlParameter("@applicationType", patchApplicationType.ApplicationType));
            }
            updatePatchApplicationTypeParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(patchApplicationType.IsActive) ? "N" : "Y"));
            updatePatchApplicationTypeParameters.Add(new SqlParameter("@createdBy", userId));
            updatePatchApplicationTypeParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
            {
                updatePatchApplicationTypeParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updatePatchApplicationTypeParameters.Add(new SqlParameter("@siteid", siteId));
            }
            updatePatchApplicationTypeParameters.Add(new SqlParameter("@synchStatus", patchApplicationType.SynchStatus));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertPatchApplicationTypeQuery, updatePatchApplicationTypeParameters.ToArray());
            log.Debug("Ends-InsertPatchApplicationType(patchApplicationType, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the patch application type record
        /// </summary>
        /// <param name="patchApplicationType">PatchApplicationTypeDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdatePatchApplicationType(PatchApplicationTypeDTO patchApplicationType, string userId, int siteId)
        {
            log.Debug("Starts-UpdatePatchApplicationType(patchApplicationType, userId, siteId) Method.");
            string updatePatchApplicationTypeQuery = @"update Patch_Application_Types 
                                         set ApplicationType = @applicationType,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             Siteid=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where PatchApplicationTypeId = @patchApplicationTypeId";
            List<SqlParameter> updatePatchApplicationTypeParameters = new List<SqlParameter>();
            updatePatchApplicationTypeParameters.Add(new SqlParameter("@patchApplicationTypeId", patchApplicationType.PatchApplicationTypeId));
            if (string.IsNullOrEmpty(patchApplicationType.ApplicationType))
            {
                updatePatchApplicationTypeParameters.Add(new SqlParameter("@applicationType", DBNull.Value));
            }
            else
            {
                updatePatchApplicationTypeParameters.Add(new SqlParameter("@applicationType", patchApplicationType.ApplicationType));
            }
            updatePatchApplicationTypeParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(patchApplicationType.IsActive) ? "N" : "Y")); 
            updatePatchApplicationTypeParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updatePatchApplicationTypeParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updatePatchApplicationTypeParameters.Add(new SqlParameter("@siteId", siteId));
            updatePatchApplicationTypeParameters.Add(new SqlParameter("@synchStatus", patchApplicationType.SynchStatus));
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updatePatchApplicationTypeQuery, updatePatchApplicationTypeParameters.ToArray());
            log.Debug("Ends-UpdatePatchApplicationType(patchApplicationType, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to PatchApplicationTypeDTO class type
        /// </summary>
        /// <param name="patchApplicationTypeDataRow">PatchApplicationTypeDTO DataRow</param>
        /// <returns>Returns PatchApplicationTypeDTO</returns>
        private PatchApplicationTypeDTO GetPatchApplicationTypeDTO(DataRow patchApplicationTypeDataRow)
        {
            log.Debug("Starts-GetPatchApplicationTypeDTO(patchApplicationTypeDataRow) Method.");
            PatchApplicationTypeDTO patchApplicationTypeDataObject = new PatchApplicationTypeDTO(Convert.ToInt32(patchApplicationTypeDataRow["PatchApplicationTypeId"]),
                                            patchApplicationTypeDataRow["ApplicationType"].ToString(),
                                            patchApplicationTypeDataRow["IsActive"].ToString(),
                                            patchApplicationTypeDataRow["CreatedBy"].ToString(),
                                            patchApplicationTypeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchApplicationTypeDataRow["CreationDate"]),
                                            patchApplicationTypeDataRow["LastUpdatedBy"].ToString(),
                                            patchApplicationTypeDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(patchApplicationTypeDataRow["LastupdatedDate"]),
                                            patchApplicationTypeDataRow["Guid"].ToString(),
                                            patchApplicationTypeDataRow["Siteid"] == DBNull.Value ? -1 : Convert.ToInt32(patchApplicationTypeDataRow["Siteid"]),
                                            patchApplicationTypeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(patchApplicationTypeDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetPatchApplicationTypeDTO(patchApplicationTypeDataRow) Method.");
            return patchApplicationTypeDataObject;
        }

        /// <summary>
        /// Gets the patch application type data of passed patch application type id
        /// </summary>
        /// <param name="patchApplicationTypeId">integer type parameter</param>
        /// <returns>Returns PatchApplicationTypeDTO</returns>
        public PatchApplicationTypeDTO GetPatchApplicationType(int patchApplicationTypeId)
        {
            log.Debug("Starts-GetPatchApplicationType(patchApplicationTypeId) Method.");
            string selectPatchApplicationTypeQuery = @"select *
                                         from Patch_Application_Types
                                        where PatchApplicationTypeId = @patchApplicationTypeId";
            SqlParameter[] selectPatchApplicationTypeParameters = new SqlParameter[1];
            selectPatchApplicationTypeParameters[0] = new SqlParameter("@patchApplicationTypeId", patchApplicationTypeId);
            DataTable patchApplicationType = dataAccessHandler.executeSelectQuery(selectPatchApplicationTypeQuery, selectPatchApplicationTypeParameters);
            if (patchApplicationType.Rows.Count > 0)
            {
                DataRow patchApplicationTypeRow = patchApplicationType.Rows[0];
                PatchApplicationTypeDTO patchApplicationTypeDataObject = GetPatchApplicationTypeDTO(patchApplicationTypeRow);
                log.Debug("Ends-GetPatchApplicationType(patchApplicationTypeId) Method by returnting patchApplicationTypeDataObject.");
                return patchApplicationTypeDataObject;
            }
            else
            {
                log.Debug("Ends-GetPatchApplicationType(patchApplicationTypeId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the PatchApplicationTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PatchApplicationTypeDTO matching the search criteria</returns>
        public List<PatchApplicationTypeDTO> GetPatchApplicationTypeList(List<KeyValuePair<PatchApplicationTypeDTO.SearchByPatchApplicationTypeParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetPatchApplicationTypeList(searchParameters) Method.");
            int count = 0;
            string selectPatchApplicationTypeQuery = @"select *
                                         from Patch_Application_Types";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PatchApplicationTypeDTO.SearchByPatchApplicationTypeParameters, string> searchParameter in searchParameters)
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
                        log.Debug("Ends-GetPatchApplicationTypeList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectPatchApplicationTypeQuery = selectPatchApplicationTypeQuery + query;
            }

            DataTable patchApplicationTypeData = dataAccessHandler.executeSelectQuery(selectPatchApplicationTypeQuery, null);
            if (patchApplicationTypeData.Rows.Count > 0)
            {
                List<PatchApplicationTypeDTO> patchApplicationTypeList = new List<PatchApplicationTypeDTO>();
                foreach (DataRow patchApplicationTypeDataRow in patchApplicationTypeData.Rows)
                {
                    PatchApplicationTypeDTO patchApplicationTypeDataObject = GetPatchApplicationTypeDTO(patchApplicationTypeDataRow);
                    patchApplicationTypeList.Add(patchApplicationTypeDataObject);
                }
                log.Debug("Ends-GetPatchApplicationTypeList(searchParameters) Method by returning patchApplicationTypeList.");
                return patchApplicationTypeList;
            }
            else
            {
                log.Debug("Ends-GetPatchApplicationTypeList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
