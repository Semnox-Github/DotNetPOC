/********************************************************************************************
 * Project Name - ModifierSetDetails Data Handler
 * Description  - Data handler of the ModifierSetDetails class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40        17-Sep-2018      Indhu               Created 
 *2.60        21-Mar-2019      Akshay Gulaganji    handeled isActive after modifing in DTO(i.e., from string to bool) 
 *2.70        28-Jun-2019      Akshay Gulaganji    added DeleteModifierSetDetails()
 *2.70.2      10-Dec-2019      Jinto Thomas        Removed siteid from update query
 *2.110.00    27-Nov-2020      Abhishek            Modified : Modified to 3 Tier Standard
 *2.140.0     06-Dec-2021      Fiona               modified :Added MODIFIER_SET_ID_LIST search Parameter.
  ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    public class ModifierSetDetailsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ModifierSetDetails AS msd ";

        private static readonly Dictionary<ModifierSetDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ModifierSetDetailsDTO.SearchByParameters, string>
        {
                {ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_DETAIL_ID,"msd.Id"},
                {ModifierSetDetailsDTO.SearchByParameters.ISACTIVE, "msd.IsActive"},
                {ModifierSetDetailsDTO.SearchByParameters.SITE_ID, "msd.site_id"},
                {ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_ID, "msd.ModifierSetId"},
                {ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_ID_LIST, "msd.ModifierSetId"},
                {ModifierSetDetailsDTO.SearchByParameters.MASTER_ENTITY_ID, "msd.MasterEntityId"},
                {ModifierSetDetailsDTO.SearchByParameters.MODIFIER_PRODUCT_ID, "msd.ModifierProductId"},
                {ModifierSetDetailsDTO.SearchByParameters.GUID, "msd.GUID"},
                {ModifierSetDetailsDTO.SearchByParameters.LAST_UPDATED_DATE, "msd.LastUpdateDate"}
        };

        /// <summary>
        /// Default constructor of ModifierSetDetailsDataHandler class
        /// </summary>
        public ModifierSetDetailsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ModifierSetDetails Record.
        /// </summary>
        /// <param name="modifierSetDetailsDTO">ModifierSetDetailsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ModifierSetDetailsDTO modifierSetDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(modifierSetDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", modifierSetDetailsDTO.ModifierSetDetailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@modifierSetId", modifierSetDetailsDTO.ModifierSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@modifierProductId", modifierSetDetailsDTO.ModifierProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parentId", modifierSetDetailsDTO.ParentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@price", modifierSetDetailsDTO.Price < 0 ? DBNull.Value : (object)modifierSetDetailsDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sortOrder", modifierSetDetailsDTO.SortOrder < 0 ? DBNull.Value : (object)modifierSetDetailsDTO.SortOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", modifierSetDetailsDTO.IsActive == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdateUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", modifierSetDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ModifierSetDetail record to the database
        /// </summary>
        public ModifierSetDetailsDTO Insert(ModifierSetDetailsDTO modifierSetDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(modifierSetDetailsDTO, loginId, siteId);
            string insertmodifierSetDetailsQuery = @"INSERT INTO ModifierSetDetails
                                                       (  
                                                          [LastUpdateUser],
                                                          [ModifierSetId],
                                                          [ModifierProductId],
                                                          [Price],
                                                          [ParentId],
                                                          [SortOrder],
                                                          [IsActive],
                                                          [CreatedBy],
                                                          [CreationDate],
                                                          [LastUpdateDate],
                                                          [GUID],
                                                          [site_id], 
                                                          [MasterEntityId]
                                                       )
                                                     values
                                                       (  @lastUpdateUser,
                                                          @modifierSetId,
                                                          @modifierProductId,
                                                          @price,
                                                          @parentId,
                                                          @sortOrder,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          Getdate(),
                                                          NewId(),
                                                          @siteId,
                                                          @masterEntityId
                                                       ) SELECT * FROM ModifierSetDetails WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertmodifierSetDetailsQuery, GetSQLParameters(modifierSetDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshModifierSetDetailsDTO(modifierSetDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(modifierSetDetailsDTO);
            return modifierSetDetailsDTO;

        }

        /// <summary>
        /// Updates the Modifier set details DTO
        /// </summary>
        /// <param name="modifierSetDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ModifierSetDetailsDTO Update(ModifierSetDetailsDTO modifierSetDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(modifierSetDetailsDTO, loginId, siteId);
            string updateModifierSetDetailsQuery = @"UPDATE ModifierSetDetails  
                                                        SET  [LastUpdateUser] = @lastUpdateUser,
                                                             [ModifierSetId] = @modifierSetId,
                                                             [ModifierProductId] = @modifierProductId,
                                                             [Price] = @price,
                                                             [ParentId] = @parentId,
                                                             [SortOrder] = @sortOrder,
                                                             [IsActive] = @isActive,
                                                             [LastUpdateDate] = Getdate(),
                                                             -- [site_id] = @siteId,
                                                             [MasterEntityId] =  @masterEntityId
                                                            WHERE  Id = @id
                                                            SELECT * FROM ModifierSetDetails WHERE Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateModifierSetDetailsQuery, GetSQLParameters(modifierSetDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshModifierSetDetailsDTO(modifierSetDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(modifierSetDetailsDTO);
            return modifierSetDetailsDTO;
        }

        /// <summary>
        /// Converts the Data row object to ModifiersSetDetailsDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ModifierSetDetailsDTO GetModifierSetDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry();
            ModifierSetDetailsDTO modifierSetDetailDTO = new ModifierSetDetailsDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ModifierSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ModifierSetId"]),
                                            dataRow["ModifierProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ModifierProductId"]),
                                            dataRow["Price"] == DBNull.Value ? -1 : Convert.ToDouble(dataRow["Price"]),
                                            dataRow["ParentId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentId"]),
                                            dataRow["SortOrder"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SortOrder"]),
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
            log.LogMethodExit(modifierSetDetailDTO);
            return modifierSetDetailDTO;
        }

        private void RefreshModifierSetDetailsDTO(ModifierSetDetailsDTO modifierSetDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(modifierSetDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                modifierSetDetailsDTO.ModifierSetDetailId = Convert.ToInt32(dt.Rows[0]["Id"]);
                modifierSetDetailsDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                modifierSetDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                modifierSetDetailsDTO.LastUpdatedUser = dataRow["LastUpdateUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdateUser"]);
                modifierSetDetailsDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                modifierSetDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                modifierSetDetailsDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ModifierSetDetail data of passed modifierSetDetail Id
        /// </summary>
        /// <param name="modifierSetDetailId">integer type parameter</param>
        /// <returns>Returns ModifierSetDetailDTO</returns>
        public ModifierSetDetailsDTO GetModifierSetDetailsDTO(int id)
        {
            log.LogMethodEntry(id);
            string selectModifierSetDetailsQuery = SELECT_QUERY + @" WHERE msd.Id = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            ModifierSetDetailsDTO modifierSetDetailsDataObject = null;
            DataTable modifierSetDetails = dataAccessHandler.executeSelectQuery(selectModifierSetDetailsQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (modifierSetDetails.Rows.Count > 0)
            {
                DataRow ModifierSetDetailsRow = modifierSetDetails.Rows[0];
                modifierSetDetailsDataObject = GetModifierSetDetailsDTO(ModifierSetDetailsRow);
            }
            log.LogMethodExit(modifierSetDetailsDataObject);
            return modifierSetDetailsDataObject;
        }


        internal List<ModifierSetDetailsDTO> GetModifierSetDetailsDTOList(List<int> modifierSetIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(modifierSetIdList);
            List<ModifierSetDetailsDTO> modifierSetDetailsDTOList = new List<ModifierSetDetailsDTO>();
            string query = @"SELECT *
                            FROM ModifierSetDetails, @modifierSetIdList List
                            WHERE ModifierSetId = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 'Y' or IsActive is null)";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@modifierSetIdList", modifierSetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                modifierSetDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetModifierSetDetailsDTO(x)).ToList();
            }
            log.LogMethodExit(modifierSetDetailsDTOList);
            return modifierSetDetailsDTOList;
        }


        /// <summary>
        /// Gets the ModifierSetDetailsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of modifierSetDetailsDTO matching the search criteria</returns>
        public List<ModifierSetDetailsDTO> GetModifierSetDetails(List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            List<ModifierSetDetailsDTO> modifierSetDetailsList = null;
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_DETAIL_ID) ||
                                searchParameter.Key.Equals(ModifierSetDetailsDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                searchParameter.Key.Equals(ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_ID) ||
                                searchParameter.Key.Equals(ModifierSetDetailsDTO.SearchByParameters.MODIFIER_PRODUCT_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(ModifierSetDetailsDTO.SearchByParameters.GUID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                            }
                            else if (searchParameter.Key == ModifierSetDetailsDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == ModifierSetDetailsDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                            }
                            else if (searchParameter.Key.Equals(ModifierSetDetailsDTO.SearchByParameters.LAST_UPDATED_DATE))
                            {
                                query.Append(joiner + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key.Equals(ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_ID_LIST))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN  " + "(" + searchParameter.Value + " )");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                    selectQuery = selectQuery + query + " order by SortOrder";
            }

            DataTable modifierSetDetailsData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (modifierSetDetailsData.Rows.Count > 0)
            {
                modifierSetDetailsList = new List<ModifierSetDetailsDTO>();
                foreach (DataRow modifierSetDetailsDataRow in modifierSetDetailsData.Rows)
                {
                    ModifierSetDetailsDTO modifierSetDetailsDataObject = GetModifierSetDetailsDTO(modifierSetDetailsDataRow);
                    modifierSetDetailsList.Add(modifierSetDetailsDataObject);
                }
            }
            log.LogMethodExit(modifierSetDetailsList);
            return modifierSetDetailsList;

        }
        /// <summary>
        /// Based on the id, appropriate ModifierSetDetails record will be deleted
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>return the int</returns>
        public void Delete(int id)
        {
            log.LogMethodEntry(id);
            try
            {
                string deleteQuery = @"delete from ModifierSetDetails where Id = @id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@id", id);
                dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit();
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
