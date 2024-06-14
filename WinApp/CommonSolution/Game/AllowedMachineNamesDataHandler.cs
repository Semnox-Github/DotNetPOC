/********************************************************************************************
 * Project Name - Semnox.Parafait.Game - AllowedMachineNamesDataHandler
 * Description  - AllowedMachineNamesDataHandler Data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 ********************************************************************************************* 
 *2.160.0    02-Feb-2022       Roshan Devadiga        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Game
{
    public class AllowedMachineNamesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM AllowedMachineNames AS amn ";
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Dictionary for searching Parameters for the AllowedMachineNamesDTO object.
        /// </summary>
        private static readonly Dictionary<AllowedMachineNamesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AllowedMachineNamesDTO.SearchByParameters, string>
        {
            { AllowedMachineNamesDTO.SearchByParameters.ALLOWED_MACHINE_ID,"amn.AllowedMachineId"},
            { AllowedMachineNamesDTO.SearchByParameters.GAME_ID,"amn.Game_id"},
            { AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME,"amn.MachineName"},
            { AllowedMachineNamesDTO.SearchByParameters.IS_ACTIVE,"amn.IsActive"},
             { AllowedMachineNamesDTO.SearchByParameters.MASTER_ENTITY_ID,"amn.MasterEntityId"},
            { AllowedMachineNamesDTO.SearchByParameters.SITE_ID,"amn.site_id"}
        };
        /// <summary>
        /// Parameterized Constructor for AllowedMachineNamesDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public AllowedMachineNamesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating (AllowedMachineNames Record).
        /// </summary>
        /// <param name="AllowedMachineNamesDTO">AllowedMachineNamesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AllowedMachineNamesDTO AllowedMachineNamesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(AllowedMachineNamesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AllowedMachineId", AllowedMachineNamesDTO.AllowedMachineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Game_id", AllowedMachineNamesDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineName", AllowedMachineNamesDTO.MachineName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", AllowedMachineNamesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", AllowedMachineNamesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to AllowedMachineNamesDTO class type
        /// </summary>
        /// <param name="AllowedMachineNamesDTO">AllowedMachineNamesDTO DataRow</param>
        /// <returns>Returns AllowedMachineNamesDTO</returns>
        private AllowedMachineNamesDTO GetAllowedMachineNamesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AllowedMachineNamesDTO AllowedMachineNamesDataObject = new AllowedMachineNamesDTO(Convert.ToInt32(dataRow["AllowedMachineId"]),
                                                    dataRow["Game_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Game_id"]),
                                                    dataRow["MachineName"]==DBNull.Value ? string.Empty : Convert.ToString(dataRow["MachineName"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"].ToString(),
                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                    );
            log.LogMethodExit();
            return AllowedMachineNamesDataObject;
        }

        /// <summary>
        /// Gets the AllowedMachineNames data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns AllowedMachineNamesDTO</returns>
        internal AllowedMachineNamesDTO GetAllowedMachineNames(int id)
        {
            log.LogMethodEntry(id);
            AllowedMachineNamesDTO result = null;
            string query = SELECT_QUERY + @" WHERE amn.AllowedMachineId = @AllowedMachineId";
            SqlParameter parameter = new SqlParameter("@AllowedMachineId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAllowedMachineNamesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="AllowedMachineNamesDTO">AllowedMachineNamesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshAllowedMachineNamesDTO(AllowedMachineNamesDTO AllowedMachineNamesDTO, DataTable dt)
        {
            log.LogMethodEntry(AllowedMachineNamesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                AllowedMachineNamesDTO.AllowedMachineId = Convert.ToInt32(dt.Rows[0]["AllowedMachineId"]);
                AllowedMachineNamesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                AllowedMachineNamesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                AllowedMachineNamesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                AllowedMachineNamesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                AllowedMachineNamesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                AllowedMachineNamesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the AllowedMachineNames Table. 
        /// </summary>
        /// <param name="AllowedMachineNamesDTO">AllowedMachineNames object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated AllowedMachineNamesDTO</returns>
        public AllowedMachineNamesDTO Insert(AllowedMachineNamesDTO AllowedMachineNamesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(AllowedMachineNamesDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[AllowedMachineNames]
                            (
                            Game_id,
                            MachineName ,
                            IsActive,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate
                            )
                            VALUES
                            (
                            @Game_id,
                            @MachineName,
                            @IsActive,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )
                            SELECT * FROM AllowedMachineNames WHERE AllowedMachineId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(AllowedMachineNamesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAllowedMachineNamesDTO(AllowedMachineNamesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AllowedMachineNamesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(AllowedMachineNamesDTO);
            return AllowedMachineNamesDTO;
        }
        /// <summary>
        /// Update the record in the AllowedMachineNames Table. 
        /// </summary>
        /// <param name="AllowedMachineNamesDTO">AllowedMachineNamesDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated AllowedMachineNamesDTO</returns>
        public AllowedMachineNamesDTO Update(AllowedMachineNamesDTO AllowedMachineNamesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(AllowedMachineNamesDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[AllowedMachineNames]
                             SET
                             MachineName = @MachineName,
                             IsActive=@IsActive,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE()
                             WHERE AllowedMachineId = @AllowedMachineId
                             SELECT * FROM AllowedMachineNames WHERE AllowedMachineId = @AllowedMachineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(AllowedMachineNamesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAllowedMachineNamesDTO(AllowedMachineNamesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AllowedMachineNamesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(AllowedMachineNamesDTO);
            return AllowedMachineNamesDTO;
        }
        internal List<AllowedMachineNamesDTO> GetAllowedMachineNamesDTOListOfGame(List<int> IdList, bool activeRecords)
        {
            log.LogMethodEntry(IdList);
            List<AllowedMachineNamesDTO> AllowedMachineNamesDTOList = new List<AllowedMachineNamesDTO>();
            string query = @"SELECT AllowedMachineNames.*
                            FROM AllowedMachineNames, @AllowedMachineNames List
                            WHERE game_id = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@AllowedMachineNames", IdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                AllowedMachineNamesDTOList = table.Rows.Cast<DataRow>().Select(x => GetAllowedMachineNamesDTO(x)).ToList();
            }
            log.LogMethodExit(AllowedMachineNamesDTOList);
            return AllowedMachineNamesDTOList;
        }
        /// <summary>
        /// Returns the List of AllowedMachineNamesDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of AllowedMachineNamesDTO </returns>
        public List<AllowedMachineNamesDTO> GetAllAllowedMachineNamesDTOList(List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AllowedMachineNamesDTO> AllowedMachineNamesDTOList = new List<AllowedMachineNamesDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AllowedMachineNamesDTO.SearchByParameters.ALLOWED_MACHINE_ID ||
                            searchParameter.Key == AllowedMachineNamesDTO.SearchByParameters.GAME_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AllowedMachineNamesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" || searchParameter.Value == "Y"));
                        }
                        else if (searchParameter.Key == AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME) 
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AllowedMachineNamesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AllowedMachineNamesDTO AllowedMachineNamesDTO = GetAllowedMachineNamesDTO(dataRow);
                    AllowedMachineNamesDTOList.Add(AllowedMachineNamesDTO);
                }
            }
            log.LogMethodExit(AllowedMachineNamesDTOList);
            return AllowedMachineNamesDTOList;
        }

    }
}
