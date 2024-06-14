/********************************************************************************************
 * Project Name - ModifierSet Data Handler
 * Description  - Data handler of the ModifierSet class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40        17-Sep-2018      Indhu               Created
 *2.60        26-Apr-2019      Akshay G            modified isActive dataType(from string to bool) and handled in this handler
 *2.70        28-Jun-2019      Nagesh Badiger      Added DeleteModifierSet() method.
  *2.70.2      10-Dec-2019    Jinto Thomas      Removed siteid from update query
 *2.110.00    26-Nov-2020      Abhishek            Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    public class ModifierSetDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ModifierSet AS ms ";

        private static readonly Dictionary<ModifierSetDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ModifierSetDTO.SearchByParameters, string>
        {
                {ModifierSetDTO.SearchByParameters.MODIFIER_SET_ID,"ms.ModifierSetId"},
                {ModifierSetDTO.SearchByParameters.MODIFIER_SET_NAME,"ms.SetName"},
                {ModifierSetDTO.SearchByParameters.ISACTIVE, "ms.IsActive"},
                {ModifierSetDTO.SearchByParameters.SITE_ID, "ms.site_id"},
                {ModifierSetDTO.SearchByParameters.MASTER_ENTITY_ID, "ms.MasterEntityId"},
                {ModifierSetDTO.SearchByParameters.MODIFIER_SET_ID_LIST,"ms.ModifierSetId"},
                {ModifierSetDTO.SearchByParameters.GUID,"ms.GUID"},
                {ModifierSetDTO.SearchByParameters.PARENT_MODIFIER_ID,"ms.ParentModifierSetId"},
                {ModifierSetDTO.SearchByParameters.LAST_UPDATED_DATE,"ms.LastUpdateDate"}
        };

        /// <summary>
        /// Default constructor of ModifierSetDataHandler class
        /// </summary>
        public ModifierSetDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ModifierSet Record.
        /// </summary>
        /// <param name="modifierSetDTO">ModifierSetDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ModifierSetDTO modifierSetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(modifierSetDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@modifierSetId", modifierSetDTO.ModifierSetId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@setName", modifierSetDTO.SetName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@minQuantity", modifierSetDTO.MinQuantity == -1 ? DBNull.Value : (object)modifierSetDTO.MinQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maxQuantity", modifierSetDTO.MaxQuantity == -1 ? DBNull.Value : (object)modifierSetDTO.MaxQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@freeQuantity", modifierSetDTO.FreeQuantity == -1 ? DBNull.Value : (object)modifierSetDTO.FreeQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parentModifierSetId", modifierSetDTO.ParentModifierSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", modifierSetDTO.IsActive == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdateUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", modifierSetDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ModifierSet record to the database
        /// </summary>
        public ModifierSetDTO Insert(ModifierSetDTO modifierSetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(modifierSetDTO, loginId, siteId);
            string InsertModifierSetQuery = @"INSERT INTO ModifierSet
                                                       (  [SetName],
                                                          [LastUpdateUser],
                                                          [MinQuantity],
                                                          [MaxQuantity],
                                                          [FreeQuantity],
                                                          [ParentModifierSetId],
                                                          [IsActive],
                                                          [CreatedBy],
                                                          [CreationDate],
                                                          [LastUpdateDate],
                                                          [GUID],
                                                          [site_id], 
                                                          [MasterEntityId]
                                                       )
                                                 VALUES
                                                       (
                                                          @setName,
                                                          @lastUpdateUser,
                                                          @minQuantity,
                                                          @maxQuantity,
                                                          @freeQuantity,
                                                          @parentModifierSetId,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          Getdate(),
                                                          NewId(),
                                                          @siteId,
                                                          @masterEntityId
                                                        )SELECT * FROM ModifierSet WHERE ModifierSetId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertModifierSetQuery, GetSQLParameters(modifierSetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshModifierSetDTO(modifierSetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(modifierSetDTO);
            return modifierSetDTO;
        }

        /// <summary>
        /// Updates the Modifier set DTO
        /// </summary>
        /// <param name="modifierSetDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ModifierSetDTO Update(ModifierSetDTO modifierSetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(modifierSetDTO, loginId, siteId);
            string updateModifierSetQuery = @"UPDATE ModifierSet 
                                                        SET [SetName]  = @setName,
                                                            [LastUpdateUser] = @lastUpdateUser,
                                                            [MinQuantity] = @minQuantity,
                                                            [MaxQuantity] = @maxQuantity,
                                                            [FreeQuantity] = @freeQuantity,
                                                            [ParentModifierSetId] = @parentModifierSetId,
                                                            [IsActive] = @isActive,
                                                            [LastUpdateDate] = Getdate(),
                                                            -- [site_id] = @siteId,
                                                            [MasterEntityId] =  @masterEntityId
                                                        WHERE  ModifierSetId = @modifierSetId
                                                        SELECT* FROM ModifierSet WHERE ModifierSetId = @modifierSetId";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateModifierSetQuery, GetSQLParameters(modifierSetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshModifierSetDTO(modifierSetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(modifierSetDTO);
            return modifierSetDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="modifierSetDTO">ModifierSetDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshModifierSetDTO(ModifierSetDTO modifierSetDTO, DataTable dt)
        {
            log.LogMethodEntry(modifierSetDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                modifierSetDTO.ModifierSetId = Convert.ToInt32(dt.Rows[0]["ModifierSetId"]);
                modifierSetDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                modifierSetDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                modifierSetDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                modifierSetDTO.LastUpdatedUser = dataRow["LastUpdateUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdateUser"]);
                modifierSetDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                modifierSetDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to ModifierSetDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ModifierSetDTO GetModifierSetDTO(DataRow dataRow)
        {
            log.LogMethodEntry();
            ModifierSetDTO ModifierSetDTO = new ModifierSetDTO(Convert.ToInt32(dataRow["ModifierSetId"]),
                                            dataRow["SetName"] == DBNull.Value ? string.Empty : dataRow["SetName"].ToString(),
                                            dataRow["MinQuantity"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MinQuantity"]),
                                            dataRow["MaxQuantity"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MaxQuantity"]),
                                            dataRow["FreeQuantity"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FreeQuantity"]),
                                            dataRow["ParentModifierSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentModifierSetId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y" ? true : false,
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdateUser"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(ModifierSetDTO);
            return ModifierSetDTO;
        }

        /// <summary>
        /// Gets the ModifierSet data of passed ModifierSetId
        /// </summary>
        /// <param name="ModifierSetId">integer type parameter</param>
        /// <returns>Returns ModifierSetDTO</returns>
        public ModifierSetDTO GetModifierSetDTO(int modifierSetId)
        {
            log.LogMethodEntry(modifierSetId);
            ModifierSetDTO modifierSetDTO = null;
            string selectModifierSetQuery = SELECT_QUERY + @" WHERE ms.ModifierSetId = @modifierSetId";
            SqlParameter[] selectModifierSetParameters = new SqlParameter[1];
            selectModifierSetParameters[0] = new SqlParameter("@modifierSetId", modifierSetId);
            DataTable modifierSet = dataAccessHandler.executeSelectQuery(selectModifierSetQuery, selectModifierSetParameters, sqlTransaction);
            if (modifierSet.Rows.Count > 0)
            {
                modifierSetDTO = GetModifierSetDTO(modifierSet.Rows[0]);
            }
            log.LogMethodExit(modifierSetDTO);
            return modifierSetDTO;

        }

        internal DateTime? GetModifierSetModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from ModifierSet WHERE (site_id = @siteId or @siteId = -1)
                             ) modefierSetlastupdate";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ModifierSetDTO> GetModifierSetDTOList(List<int> modifierSetIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(modifierSetIdList);
            List<ModifierSetDTO> productIdListDetailsDTOList = new List<ModifierSetDTO>();
            string query = @"SELECT *
                            FROM ModifierSet, @modifierSetIdList List
                            WHERE ModifierSetId = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 'Y' or IsActive is null)";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@modifierSetIdList", modifierSetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productIdListDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetModifierSetDTO(x)).ToList();
            }
            log.LogMethodExit(productIdListDetailsDTOList);
            return productIdListDetailsDTOList;
        }


        /// <summary>
        /// Gets the ModifierSetDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ModifierSetDTO matching the search criteria</returns>
        public List<ModifierSetDTO> GetModifierSetList(List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ModifierSetDTO> modifierSetDTOList = null;
            string selectQuery = SELECT_QUERY;
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ModifierSetDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(ModifierSetDTO.SearchByParameters.MODIFIER_SET_ID) ||
                                searchParameter.Key.Equals(ModifierSetDTO.SearchByParameters.PARENT_MODIFIER_ID) ||
                                searchParameter.Key.Equals(ModifierSetDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(ModifierSetDTO.SearchByParameters.GUID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == ModifierSetDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == ModifierSetDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                            }
                            else if (searchParameter.Key.Equals(ModifierSetDTO.SearchByParameters.MODIFIER_SET_ID_LIST))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(ModifierSetDTO.SearchByParameters.LAST_UPDATED_DATE))
                            {
                                query.Append(joiner + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable modifierSetData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (modifierSetData.Rows.Count > 0)
            {
                modifierSetDTOList = new List<ModifierSetDTO>();
                foreach (DataRow modifierSetDataRow in modifierSetData.Rows)
                {
                    ModifierSetDTO modifierSetDTO = GetModifierSetDTO(modifierSetDataRow);
                    modifierSetDTOList.Add(modifierSetDTO);
                }
            }
            log.LogMethodExit(modifierSetDTOList);
            return modifierSetDTOList;
        }
        /// <summary>
        /// Based on the modifierSerDetailId, appropriate ModifierSetDetails record will be deleted
        /// </summary>
        /// <param name="modifierSetId">modifierSetId</param>
        /// <returns>return the int </returns>
        public int Delete(int modifierSetId)
        {
            log.LogMethodEntry(modifierSetId);
            try
            {
                string deleteQuery = @"delete from ModifierSet where ModifierSetId = @modifierSetId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@modifierSetId", modifierSetId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }
}
