/********************************************************************************************
 * Project Name - ManagementFormAccess Data Handler
 * Description  - Data handler of the ManagementFormAccess class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        15-Jul-2019      Girish Kundar       Modified : Added GetSQLParameter(),SQL Injection Fix,Missed Who columns
 *2.70.2        11-Dec-2019      Jinto Thomas        Removed siteid from update query
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class ManagementFormAccessDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<ManagementFormAccessDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ManagementFormAccessDTO.SearchByParameters, string>
            {
                {ManagementFormAccessDTO.SearchByParameters.MANAGEMENT_FORM_ACCESS_ID, "mfa.ManagementFormAccessId"},
                {ManagementFormAccessDTO.SearchByParameters.ROLE_ID, "mfa.role_id"},
                {ManagementFormAccessDTO.SearchByParameters.ROLE_ID_LIST, "mfa.role_id"},
                {ManagementFormAccessDTO.SearchByParameters.MAIN_MENU,"mfa.main_menu"},
                {ManagementFormAccessDTO.SearchByParameters.FORM_NAME,"mfa.form_name"},
                {ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED,"mfa.access_allowed"},
                {ManagementFormAccessDTO.SearchByParameters.FUNCTION_GROUP,"mfa.functiongroup"},
                {ManagementFormAccessDTO.SearchByParameters.FUNCTION_GUID,"mfa.functionGUID"}, 
                {ManagementFormAccessDTO.SearchByParameters.MASTER_ENTITY_ID,"mfa.MasterEntityId"}, 
                {ManagementFormAccessDTO.SearchByParameters.SITE_ID, "mfa.site_id"},
                {ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "mfa.IsActive"}
            };
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ManagementFormAccess AS mfa ";

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS ManagementFormAccessType
                                            MERGE INTO ManagementFormAccess tbl
                                            USING @ManagementFormAccessList AS src
                                            ON src.ManagementFormAccessId = tbl.ManagementFormAccessId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            role_id = src.role_id,
                                            main_menu = src.main_menu,
                                            form_name = src.form_name,
                                            access_allowed = src.access_allowed,
                                            FunctionId = src.FunctionId,
                                            FunctionGroup = src.FunctionGroup,
                                            Guid = src.Guid,
                                            -- site_id = src.site_id,
                                            MasterEntityId = src.MasterEntityId,
                                            FunctionGUID = src.FunctionGUID,
                                            IsActive = src.IsActive,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdateDate = GETDATE()

                                            WHEN NOT MATCHED THEN INSERT (
                                            role_id,
                                            main_menu,
                                            form_name,
                                            access_allowed,
                                            FunctionId,
                                            FunctionGroup,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            FunctionGUID,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate
                                            )VALUES (
                                            src.role_id,
                                            src.main_menu,
                                            src.form_name,
                                            src.access_allowed,
                                            src.FunctionId,
                                            src.FunctionGroup,
                                            src.Guid,
                                            src.site_id,
                                            src.MasterEntityId,
                                            src.FunctionGUID,
                                            src.IsActive,
                                            src.CreatedBy,
                                            GETDATE(),
                                            src.LastUpdatedBy,
                                            GETDATE()
                                            )
                                            OUTPUT
                                            inserted.ManagementFormAccessId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            ManagementFormAccessId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion

        /// <summary>
        /// Default constructor of ManagementFormAccessDataHandler class
        /// </summary>
        public ManagementFormAccessDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ManagementFormAccess Record.
        /// </summary>
        /// <param name="managementFormAccessDTO">ManagementFormAccessDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ManagementFormAccessDTO managementFormAccessDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(managementFormAccessDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@managementFormAccessId", managementFormAccessDTO.ManagementFormAccessId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@roleId", managementFormAccessDTO.RoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@mainMenu", managementFormAccessDTO.MainMenu));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", managementFormAccessDTO.FormName));
            string accessAllowedS = "";
            if (managementFormAccessDTO.AccessAllowed)
                accessAllowedS = "Y";
            else
                accessAllowedS = "N";
            parameters.Add(dataAccessHandler.GetSQLParameter("@accessAllowed", accessAllowedS));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionId", managementFormAccessDTO.FunctionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGroup", managementFormAccessDTO.FunctionGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGUID", managementFormAccessDTO.FunctionGUID)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", managementFormAccessDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isactive", managementFormAccessDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ManagementFormAccess record to the database
        /// </summary>
        /// <param name="managementFormAccessDTO">ManagementFormAccessDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ManagementFormAccessDTO</returns>
        public ManagementFormAccessDTO InsertManagementFormAccess(ManagementFormAccessDTO managementFormAccessDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(managementFormAccessDTO, loginId, siteId);
            string query = @"INSERT INTO ManagementFormAccess 
                                        ( 
                                            [role_id]
                                           ,[main_menu]
                                           ,[form_name]
                                           ,[access_allowed]
                                           ,[FunctionId]
                                           ,[FunctionGroup]
                                           ,[FunctionGUID]
                                           ,[Guid]
                                           ,[site_id] 
                                           ,[MasterEntityId]
                                           ,[CreatedBy]
                                           ,[CreationDate]
                                           ,[LastUpdatedBy]
                                           ,[LastUpdateDate]
                                           ,[IsActive]
                                        ) 
                                VALUES 
                                        (
                                            @roleId,
                                            @mainMenu,
                                            @formName,
                                            @accessAllowed,
                                            @functionId,
                                            @functionGroup,
                                            @functionGUID,
                                            newid(),
                                            @siteId,
                                            @masterEntityId,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            @isactive
                                        )SELECT  * from ManagementFormAccess where ManagementFormAccessId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(managementFormAccessDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshManagementFormAccessDTO(managementFormAccessDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting managementFormAccessDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(managementFormAccessDTO);
            return managementFormAccessDTO;
        }

        /// <summary>
        /// Updates the ManagementFormAccess record
        /// </summary>
        /// <param name="managementFormAccessDTO">ManagementFormAccessDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ManagementFormAccessDTO</returns>
        public ManagementFormAccessDTO UpdateManagementFormAccess(ManagementFormAccessDTO managementFormAccessDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(managementFormAccessDTO, loginId, siteId);
            string query = @"UPDATE ManagementFormAccess 
                                SET [role_id] = @roleId,
                                    [main_menu] = @mainMenu,
                                    [form_name] = @formName,
                                    [access_allowed] =  @accessAllowed,
                                    [FunctionId] =  @functionId,
                                    [FunctionGroup] = @functionGroup,
                                    -- [site_id] = @siteId,
                                    [MasterEntityId] = @masterEntityId,
                                    [FunctionGUID] = @functionGUID,
                                    [LastUpdatedBy] =@lastUpdatedBy,
                                    [LastUpdateDate] = getdate(),
                                    [IsActive] = @isactive
                              WHERE ManagementFormAccessId = @managementFormAccessId
                              SELECT  * from ManagementFormAccess where ManagementFormAccessId = @managementFormAccessId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(managementFormAccessDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshManagementFormAccessDTO(managementFormAccessDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating managementFormAccessDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(managementFormAccessDTO);
            return managementFormAccessDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="managementFormAccessDTO">ManagementFormAccessDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshManagementFormAccessDTO(ManagementFormAccessDTO managementFormAccessDTO, DataTable dt)
        {
            log.LogMethodEntry(managementFormAccessDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                managementFormAccessDTO.ManagementFormAccessId = Convert.ToInt32(dt.Rows[0]["ManagementFormAccessId"]);
                managementFormAccessDTO.LastUpdateDate = dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]);
                managementFormAccessDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                managementFormAccessDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                managementFormAccessDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to ManagementFormAccessDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ManagementFormAccessDTO</returns>
        private ManagementFormAccessDTO GetManagementFormAccessDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(Convert.ToInt32(dataRow["ManagementFormAccessId"]),
                                            dataRow["role_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["role_id"]),
                                            dataRow["main_menu"] == DBNull.Value ? string.Empty : dataRow["main_menu"].ToString(),
                                            dataRow["form_name"] == DBNull.Value ? string.Empty : dataRow["form_name"].ToString(),
                                            dataRow["access_allowed"] == DBNull.Value ? false : (dataRow["access_allowed"].ToString() == "Y" ? true : false),
                                            dataRow["FunctionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FunctionId"]),
                                            dataRow["FunctionGroup"] == DBNull.Value ? string.Empty : dataRow["FunctionGroup"].ToString(),
                                            dataRow["FunctionGUID"] == DBNull.Value ? string.Empty : dataRow["FunctionGUID"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(managementFormAccessDTO);
            return managementFormAccessDTO;
        }

        /// <summary>
        /// Gets the ManagementFormAccess data of passed ManagementFormAccess Id
        /// </summary>
        /// <param name="managementFormAccessId">integer type parameter</param>
        /// <returns>Returns ManagementFormAccessDTO</returns>
        public ManagementFormAccessDTO GetManagementFormAccessDTO(int managementFormAccessId)
        {
            log.LogMethodEntry(managementFormAccessId);
            ManagementFormAccessDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE mfa.ManagementFormAccessId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", managementFormAccessId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetManagementFormAccessDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        
  
        /// <summary>
        /// Gets the ManagementFormAccessDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ManagementFormAccessDTO matching the search criteria</returns>
        public List<ManagementFormAccessDTO> GetManagementFormAccessDTOList(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ManagementFormAccessDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if ((searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.MANAGEMENT_FORM_ACCESS_ID) ||
                            (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.ROLE_ID)  ||
                            (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.MASTER_ENTITY_ID)  
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.ROLE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N"));
                        }
                        else if ((searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.FORM_NAME)||
                                 (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.MAIN_MENU) ||
                                 (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.FUNCTION_GROUP) ||
                                 (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.FUNCTION_GUID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ManagementFormAccessDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ManagementFormAccessDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = GetManagementFormAccessDTO(dataRow);
                    list.Add(managementFormAccessDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ManagementFormAccess DTO Guid Map
        /// </summary>
        /// <param name="managementFormAccessDTOList">managementFormAccessDTOList</param>
        /// <returns>Dictionary of string, ManagementFormAccessDTO </returns>
        private Dictionary<string, ManagementFormAccessDTO> GetManagementFormAccessDTOGuidMap(List<ManagementFormAccessDTO> managementFormAccessDTOList)
        {
            Dictionary<string, ManagementFormAccessDTO> result = new Dictionary<string, ManagementFormAccessDTO>();
            for (int i = 0; i < managementFormAccessDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(managementFormAccessDTOList[i].Guid))
                {
                    managementFormAccessDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(managementFormAccessDTOList[i].Guid, managementFormAccessDTOList[i]);
            }
            return result;
        }
        /// <summary>
        /// Get Sql DataRecords
        /// </summary>
        /// <param name="managementFormAccessDTOList">managementFormAccessDTOList</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>List of SqlDataRecord</returns>
        private List<SqlDataRecord> GetSqlDataRecords(List<ManagementFormAccessDTO> managementFormAccessDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(managementFormAccessDTOList, loginId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[17];
            columnStructures[0] = new SqlMetaData("ManagementFormAccessId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("role_id", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("main_menu", SqlDbType.NVarChar, 100);
            columnStructures[3] = new SqlMetaData("form_name", SqlDbType.NVarChar, 400);
            columnStructures[4] = new SqlMetaData("access_allowed", SqlDbType.Char, 1);
            columnStructures[5] = new SqlMetaData("FunctionId", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("FunctionGroup", SqlDbType.NVarChar, 100);
            columnStructures[7] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[8] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[9] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[11] = new SqlMetaData("FunctionGUID", SqlDbType.UniqueIdentifier);
            columnStructures[12] = new SqlMetaData("IsActive", SqlDbType.Bit);
            columnStructures[13] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 100);
            columnStructures[14] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[15] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 100);
            columnStructures[16] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            for (int i = 0; i < managementFormAccessDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].ManagementFormAccessId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].RoleId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].MainMenu));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].FormName));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].AccessAllowed == true ? "Y" : "N"));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].FunctionId, true));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].FunctionGroup));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(Guid.Parse(managementFormAccessDTOList[i].Guid)));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].SynchStatus));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].MasterEntityId, true));
                if (!string.IsNullOrEmpty(managementFormAccessDTOList[i].FunctionGUID))
                {
                    dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(Guid.Parse(managementFormAccessDTOList[i].FunctionGUID)));
                }
                else
                {
                    dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(string.Empty));
                }
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].IsActive));//modified
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(loginId));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].CreationDate));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(loginId));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(managementFormAccessDTOList[i].LastUpdateDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Update ManagementFormAccessDTO List
        /// </summary>
        /// <param name="managementFormAccessDTOGuidMap">managementFormAccessDTOGuidMap</param>
        /// <param name="table">table</param>
        private void UpdateManagementFormAccessDTOGuidMapList(Dictionary<string, ManagementFormAccessDTO> managementFormAccessDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                ManagementFormAccessDTO managementFormAccessDTO = managementFormAccessDTOGuidMap[Convert.ToString(row["Guid"])];
                managementFormAccessDTO.ManagementFormAccessId = row["ManagementFormAccessId"] == DBNull.Value ? -1 : Convert.ToInt32(row["ManagementFormAccessId"]);
                managementFormAccessDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                managementFormAccessDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                managementFormAccessDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                managementFormAccessDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                managementFormAccessDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                managementFormAccessDTO.AcceptChanges();
            }
        }
        /// <summary>
        /// Inserts the ManagementFormAccessList to the database
        /// </summary>
        /// <param name="managementFormAccessDTOList">List of ManagementFormAccessDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<ManagementFormAccessDTO> managementFormAccessDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(managementFormAccessDTOList, loginId, siteId);
            Dictionary<string, ManagementFormAccessDTO> managementFormAccessDTOGuidMap = GetManagementFormAccessDTOGuidMap(managementFormAccessDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(managementFormAccessDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                        sqlTransaction,
                                        MERGE_QUERY,
                                        "ManagementFormAccessType",
                                        "@ManagementFormAccessList");
            UpdateManagementFormAccessDTOGuidMapList(managementFormAccessDTOGuidMap, dataTable);
            log.LogMethodExit();
        }
    }
}
