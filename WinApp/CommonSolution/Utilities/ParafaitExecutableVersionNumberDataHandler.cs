/********************************************************************************************
 * Project Name - Utilities
 * Description  - ParafaitExecutableVersionNumber Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2.0      23-Sep-2019   Mithesh                 Created 
 *2.70.2        11-Dec-2019   Jinto Thomas        Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// ParafaitExecutableVersionNumberDataHandler Data Handler - Handles insert, update and select of  ParafaitExecutableVersionNumber objects
    /// </summary>
    public class ParafaitExecutableVersionNumberDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ParafaitExecutableVersionNumber AS evn";
        /// <summary>
        /// Dictionary for searching Parameters for the CheckIns object.
        /// </summary>
        private static readonly Dictionary<ParafaitExecutableVersionNumberDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>
        {
            { ParafaitExecutableVersionNumberDTO.SearchByParameters.ID,"evn.Id"},
            { ParafaitExecutableVersionNumberDTO.SearchByParameters.PARAFAIT_EXECUTABLE_NAME,"evn.ParafaitExecutableName"},
            { ParafaitExecutableVersionNumberDTO.SearchByParameters.MAJOR_VERSION,"evn.MajorVersion"},
            { ParafaitExecutableVersionNumberDTO.SearchByParameters.MINOR_VERSION,"evn.MinorVersion"},
            { ParafaitExecutableVersionNumberDTO.SearchByParameters.PATCH_VERSION,"evn.PatchVersion"},
            { ParafaitExecutableVersionNumberDTO.SearchByParameters.EXECUTABLE_GENERATED_AT,"evn.ExecutableGeneratedAt"},
            { ParafaitExecutableVersionNumberDTO.SearchByParameters.SITE_ID,"evn.SiteId"},
            { ParafaitExecutableVersionNumberDTO.SearchByParameters.MASTER_ENTITY_ID,"evn.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for ParafaitExecutableVersionNumberDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public ParafaitExecutableVersionNumberDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ParafaitExecutableVersionNumber Record.
        /// </summary>
        /// <param name=""></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(ParafaitExecutableVersionNumberDTO parafaitExecutableVersionNumberDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitExecutableVersionNumberDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", parafaitExecutableVersionNumberDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitExecutableName", parafaitExecutableVersionNumberDTO.ParafaitExecutableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MajorVersion", parafaitExecutableVersionNumberDTO.MajorVersion));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinorVersion", parafaitExecutableVersionNumberDTO.MinorVersion));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PatchVersion", parafaitExecutableVersionNumberDTO.PatchVersion));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExecutableGeneratedAt", parafaitExecutableVersionNumberDTO.ExecutableGeneratedAt));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", parafaitExecutableVersionNumberDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", parafaitExecutableVersionNumberDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        private ParafaitExecutableVersionNumberDTO GetParafaitExecutableVersionNumberDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ParafaitExecutableVersionNumberDTO parafaitExecutableVersionNumberDTO = new ParafaitExecutableVersionNumberDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                       dataRow["ParafaitExecutableName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParafaitExecutableName"]),
                                       dataRow["MajorVersion"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MajorVersion"]),
                                       dataRow["MinorVersion"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MinorVersion"].ToString()),
                                       dataRow["PatchVersion"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PatchVersion"]),
                                       dataRow["ExecutableGeneratedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ExecutableGeneratedAt"]),
                                       dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                       dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                       dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                       dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                       dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                       dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                       dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                       dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                       dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                      );
            log.LogMethodExit(parafaitExecutableVersionNumberDTO);
            return parafaitExecutableVersionNumberDTO;
        }

        /// <summary>
        /// Gets the ParafaitExecutableVersionNumber data of passed ExecutableName 
        /// </summary>
        /// <param name="ParafaitExecutableName">string type parameter</param>
        /// <returns>Returns ParafaitExecutableVersionNumberDTO</returns>
        public ParafaitExecutableVersionNumberDTO GetParafaitExecutableVersionNumber(string ParafaitExecutableName)
        {
            log.LogMethodEntry(ParafaitExecutableName);
            ParafaitExecutableVersionNumberDTO result = null;
            string query = SELECT_QUERY + @" WHERE evn.ParafaitExecutableName = @ParafaitExecutableName";
            SqlParameter parameter = new SqlParameter("@ParafaitExecutableName", ParafaitExecutableName);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetParafaitExecutableVersionNumberDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Inserts the record to ParafaitExecutableVersionNumber Table
        /// </summary>
        /// <param name="parafaitExecutableVersionNumberDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ParafaitExecutableVersionNumberDTO Insert(ParafaitExecutableVersionNumberDTO parafaitExecutableVersionNumberDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitExecutableVersionNumberDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ParafaitExecutableVersionNumber]
                           (ParafaitExecutableName
                           ,MajorVersion
                           ,MinorVersion
                           ,PatchVersion
                           ,ExecutableGeneratedAt
                           ,IsActive
                           ,CreatedBy
                           ,CreationDate
                           ,LastUpdatedBy
                           ,LastupdatedDate
                           ,Guid
                           ,site_id
                           ,SynchStatus
                           ,MasterEntityId)
                     VALUES
                           (@ParafaitExecutableName
                           ,@MajorVersion
                           ,@MinorVersion
                           ,@PatchVersion
                           ,@ExecutableGeneratedAt
                           ,@IsActive
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE()
                           ,NEWID()
                           ,@SiteId
                           ,@SynchStatus
                           ,@MasterEntityId)
                                SELECT * FROM ParafaitExecutableVersionNumber WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parafaitExecutableVersionNumberDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshParafaitExecutableVersionNumberDTO(parafaitExecutableVersionNumberDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting CheckInDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(parafaitExecutableVersionNumberDTO);
            return parafaitExecutableVersionNumberDTO;
        }

        /// <summary>
        /// Update the record to ParafaitExecutableVersionNumber Table
        /// </summary>
        /// <param name="parafaitExecutableVersionNumberDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ParafaitExecutableVersionNumberDTO Update(ParafaitExecutableVersionNumberDTO parafaitExecutableVersionNumberDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitExecutableVersionNumberDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ParafaitExecutableVersionNumber]
                            SET 
                            ParafaitExecutableName = @ParafaitExecutableName
                           ,MajorVersion = @MajorVersion
                           ,MinorVersion = @MinorVersion
                           ,PatchVersion = @PatchVersion
                           ,ExecutableGeneratedAt = @ExecutableGeneratedAt
                           ,IsActive = @IsActive
                           ,CreatedBy = @CreatedBy
                           ,CreationDate = GETDATE()
                           ,LastUpdatedBy = @LastUpdatedBy
                           ,LastupdatedDate = GETDATE()
                           ,Guid = NEWID()
                           -- ,site_id = @SiteId
                           ,SynchStatus = @SynchStatus
                           ,MasterEntityId = @MasterEntityId
                            Where Id = @Id
                            SELECT * FROM ParafaitExecutableVersionNumber WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parafaitExecutableVersionNumberDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshParafaitExecutableVersionNumberDTO(parafaitExecutableVersionNumberDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting ParafaitExecutableVersionNumberDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(parafaitExecutableVersionNumberDTO);
            return parafaitExecutableVersionNumberDTO;
        }



        /// <summary>
        /// Returns the List of ParafaitExecutableVersionNumberDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ParafaitExecutableVersionNumberDTO> GetAllParafaitExecutableVersionNumberDTOList(List<KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ParafaitExecutableVersionNumberDTO> parafaitExecutableVersionNumberDTOList = new List<ParafaitExecutableVersionNumberDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ParafaitExecutableVersionNumberDTO.SearchByParameters.ID                            
                            || searchParameter.Key == ParafaitExecutableVersionNumberDTO.SearchByParameters.MAJOR_VERSION
                            || searchParameter.Key == ParafaitExecutableVersionNumberDTO.SearchByParameters.MINOR_VERSION
                            || searchParameter.Key == ParafaitExecutableVersionNumberDTO.SearchByParameters.PATCH_VERSION
                            || searchParameter.Key == ParafaitExecutableVersionNumberDTO.SearchByParameters.EXECUTABLE_GENERATED_AT
                            || searchParameter.Key == ParafaitExecutableVersionNumberDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitExecutableVersionNumberDTO.SearchByParameters.PARAFAIT_EXECUTABLE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ParafaitExecutableVersionNumberDTO.SearchByParameters.SITE_ID)
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
                    ParafaitExecutableVersionNumberDTO parafaitExecutableVersionNumberDTO = GetParafaitExecutableVersionNumberDTO(dataRow);
                    parafaitExecutableVersionNumberDTOList.Add(parafaitExecutableVersionNumberDTO);
                }
            }
            log.LogMethodExit(parafaitExecutableVersionNumberDTOList);
            return parafaitExecutableVersionNumberDTOList;
        }

        /// <summary>
        /// Refresh ParafaitExecutableVersionNumberDTO with DB values
        /// </summary>
        /// <param name="parafaitExecutableVersionNumberDTO">ParafaitExecutableVersionNumber DTO</param>
        /// <param name="dt">Data table</param>
        /// <param name="loginId">Login ID</param>
        /// <param name="siteId">site </param>
        private void RefreshParafaitExecutableVersionNumberDTO(ParafaitExecutableVersionNumberDTO parafaitExecutableVersionNumberDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitExecutableVersionNumberDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                parafaitExecutableVersionNumberDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                parafaitExecutableVersionNumberDTO.LastupdatedDate = Convert.ToDateTime(dt.Rows[0]["LastupdatedDate"]);
                parafaitExecutableVersionNumberDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                parafaitExecutableVersionNumberDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                parafaitExecutableVersionNumberDTO.LastUpdatedBy = Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                parafaitExecutableVersionNumberDTO.CreatedBy = Convert.ToString(dt.Rows[0]["CreatedBy"]);
                parafaitExecutableVersionNumberDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }
    }
}
